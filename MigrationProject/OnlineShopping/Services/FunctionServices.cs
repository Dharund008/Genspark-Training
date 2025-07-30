using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;
using Online.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Online.Services
{
    public class FunctionServices : IFunctionServices
    {
        private readonly IRepository<int, Model> modelrepo;
        private readonly IRepository<int, Color> colorrepo;

        private readonly IRepository<int, Category> categrepo;

        public FunctionServices(IRepository<int, Model> m, IRepository<int, Color> c, IRepository<int, Category> ca)
        {
            modelrepo = m;
            colorrepo = c;
            categrepo = ca;
        }
        public async Task<Color> GetColorByName(string colorname)
        {
            colorname = colorname.ToLower();
            var allcolors = await colorrepo.GetAllAsync();

            var color = allcolors.FirstOrDefault(c => c.ColorName.ToLower() == colorname);
            return color;
        }
        public async Task<Category> GetCategoryByName(string categoryname)
        {
            categoryname = categoryname.ToLower();
            var allcategs = await categrepo.GetAllAsync();

            var cat = allcategs.FirstOrDefault(c => c.Name.ToLower() == categoryname);


            return cat;
        }
        public async Task<Model> GetModelByName(string modelname)
        {
            modelname = modelname.ToLower();
            var allmodels = await modelrepo.GetAllAsync();

            var model = allmodels.FirstOrDefault(c => c.ModelName.ToLower() == modelname);
            return model;
        }
    }
}