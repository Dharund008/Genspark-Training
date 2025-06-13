using Bts.Models;
using Bts.Interfaces;
using Bts.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Bts.Repositories
{
    public class BlacklistedTokenRepository : IBlacklistedTokenRepository
    {
        private readonly BugContext _context;

        public BlacklistedTokenRepository(BugContext context)
        {
            _context = context;
        }

        public async Task AddTokenAsync(BlacklistedToken token)
        {
            await _context.BlacklistedTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _context.BlacklistedTokens
                .AnyAsync(t => t.Token == token && t.ExpiryDate > DateTime.UtcNow);
        }
    }

}