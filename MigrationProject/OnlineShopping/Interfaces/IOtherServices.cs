
using Online.Models;

namespace Online.Interfaces
{
    public interface IOtherServices
    {
        Task<Color> GetColorByName(string colorname);
        Task<Category> GetCategoryByName(string categoryname);
        Task<Model> GetModelByName(string modelname);
    }
}