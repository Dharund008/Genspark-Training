using Online.Models;
using Online.Models.DTO;

public interface IContactService
{
    Task<ContactUs> AddContactAsync(SupportDTO contact);
}