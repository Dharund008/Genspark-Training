
using Online.Models;

namespace Online.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendContactAutoReplyAsync(ContactUs contact);
    }
}
