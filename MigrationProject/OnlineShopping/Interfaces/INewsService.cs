using Online.Models;
using Online.Models.DTO;

namespace Online.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetAllNewsAsync();
        Task<News> GetNewsByIdAsync(int id);
        Task<News> AddNewsAsync(AddNewsDTO news);
        Task<bool> DeleteNewsAsync(int id);
    }
}
