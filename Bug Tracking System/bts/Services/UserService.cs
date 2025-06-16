using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Bts.Interfaces;
using Bts.Models;
using Bts.Models.DTO;
using Bts.Repositories;
using Bts.Contexts;
using Bts.MISC;
using Microsoft.AspNetCore.Authorization;

namespace Bts.Services
{
    public class UserService : IUserService
    {
        private readonly BugContext _context;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<UserService> _logger;

        public UserService(BugContext context, IEncryptionService encryptionService, ILogger<UserService> logger)
        {
            _context = context;
            _encryptionService = encryptionService;
            _logger = logger;
        }

        public async Task<string?> GeneratePasswordResetTokenAsync(string email)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == email);
                if (user == null) return null;

                var token = Guid.NewGuid().ToString();

                var resetEntry = new PasswordReset
                {
                    UserId = user.Id,
                    Token = token,
                    Expiry = DateTime.UtcNow.AddMinutes(5)
                };

                _context.PasswordResets.Add(resetEntry);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Password reset token saved for user {UserId} with token {Token}", user.Id, token);

                return token; // email this to the user
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating password reset token for email: {Email}", email);
                return null;
            }
            
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var resetEntry = await _context.PasswordResets
                .FirstOrDefaultAsync(r => r.Token == dto.Token && r.Expiry > DateTime.UtcNow);

            if (resetEntry == null) return false;

            var user = await _context.Users.FindAsync(resetEntry.UserId);
            if (user == null) return false;

            var data = new EncryptModel { Data = dto.NewPassword };
            var encrypted = await _encryptionService.EncryptData(data);

            user.Password = encrypted.EncryptedString;
            _context.Users.Update(user);

        // Update password in role table (Admin, Tester, Developer)
            if (user.Role == "ADMIN")
            {
                var admin = await _context.Admins.FindAsync(user.Id);
                if (admin != null)
                {
                    admin.Password = encrypted.EncryptedString;
                    _context.Admins.Update(admin);
                }
            }
            else if (user.Role == "TESTER")
            {
                var tester = await _context.Testers.FindAsync(user.Id);
                if (tester != null)
                {
                    tester.Password = encrypted.EncryptedString;
                    _context.Testers.Update(tester);
                }
            }
            else if (user.Role == "DEVELOPER")
            {
                var dev = await _context.Developers.FindAsync(user.Id);
                if (dev != null)
                {
                    dev.Password = encrypted.EncryptedString;
                    _context.Developers.Update(dev);
                }
            }

            //_context.PasswordResets.Remove(resetEntry); // remove used token
            await _context.SaveChangesAsync();

            return true;
        }



    }
}