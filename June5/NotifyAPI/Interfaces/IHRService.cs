using NotifyAPI.Models.DTO;
using NotifyAPI.Models;

namespace NotifyAPI.Interfaces
{
    public interface IHRService
    {
        public Task<HRAdmin> AddHRadmin(HRRequestDTO hr);

        public Task<HRAdmin> GetHRAdmin(string name);

    }
}