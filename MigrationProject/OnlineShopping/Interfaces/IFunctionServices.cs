
using Online.Models;

namespace Online.Interfaces
{
    public interface IFunctionServices
    {
        Task<Color> GetColorByName(string colorname);
        Task<Category> GetCategoryByName(string categoryname);
        Task<Model> GetModelByName(string modelname);
    }
}