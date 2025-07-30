using Online.Interfaces;
using Online.Models;
using Online.Contexts;
using Microsoft.EntityFrameworkCore;
using Online.Models.DTO;

namespace Online.Services
{
    public class NewsService : INewsService
    {
        private readonly MigrationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsService(MigrationContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _httpContextAccessor = accessor;
        }

        public async Task<IEnumerable<News>> GetAllNewsAsync()
        {
            return await _context.News
                .Where(n => n.Status == "Active")
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<News> GetNewsByIdAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null || news.Status != "Active")
                throw new Exception("News item not found.");
            return news;
        }

        public async Task<News> AddNewsAsync(AddNewsDTO news)
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User.FindFirst("MyApp_Id")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
                throw new UnauthorizedAccessException("User not authenticated.");

            var res = new News
            {
                UserId = int.Parse(userIdStr),
                Title = news.Title,
                Content = news.Content,
                ShortDescription = news.ShortDescription,
                Image = news.Image,
                CreatedDate = DateTime.UtcNow
            };
            _context.News.Add(res);
            await _context.SaveChangesAsync();
            return res;
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return false;

            news.Status = "Removed";

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
