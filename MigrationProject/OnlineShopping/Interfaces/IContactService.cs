using Online.Models;

public interface IContactService
{
    Task<ContactUs> AddContactAsync(ContactUs contact);
}