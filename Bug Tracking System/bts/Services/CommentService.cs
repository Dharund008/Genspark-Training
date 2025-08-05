using Bts.Contexts;
using Bts.Models;
using Bts.Interfaces;
using Bts.Services;
using Bts.Hubs;
using Bts.Repositories;
using Bts.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
namespace Bts.Services
{
    public class CommentService : ICommentService
    {
        private readonly BugContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<string, User> _userRepository;
        private readonly ILogger<CommentService> _logger;

        public CommentService(BugContext context, ICurrentUserService currentUserService, IRepository<string, User> userRepository,
            ILogger<CommentService> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Comment> AddCommentAsync(CommentRequestDTO dto, string userId)
        {
            try
            {
                var bug = await _context.Bugs.FindAsync(dto.BugId);
                if (bug == null)  
                {
                    _logger.LogWarning("Bug not found with ID {BugId} in AddCommentAsync", dto.BugId);
                    throw new Exception("Bug not found");
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found with ID {UserId} in AddCommentAsync", userId);
                    throw new Exception("User not found");
                }

                var comment = new Comment
                {
                    BugId = dto.BugId,
                    UserId = userId,
                    Message = dto.Message,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Added comment for bug {BugId} by user {UserId}", dto.BugId, userId);
                return comment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment for bug {BugId} by user {UserId}", dto.BugId, userId);
                throw;
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBugIdAsync(int bugId)
        {
            var comments = await _context.Comments
                .Where(c => c.BugId == bugId)
                .Include(c => c.User)
                .ToListAsync();
            _logger.LogInformation("Retrieved comments for bug {BugId}", bugId);
            return comments;
        }

        public async Task<List<Comment>> GetAllComments()
        {
            var comments = await _context.Comments.ToListAsync();
            _logger.LogInformation("Retrieved all comments");
            return comments;
        }

        public async Task<List<Comment>> GetCommentsByUserRole(string userRole)
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.User.Role == userRole)
                .ToListAsync();
            _logger.LogInformation("Retrieved comments by user role {UserRole}", userRole);
            return comments;
        }
    }
}
