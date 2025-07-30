using Online.Contexts;
using Online.Models;
using Online.Interfaces;

public class ContactService : IContactService
{
    private readonly MigrationContext _context;

    public ContactService(MigrationContext context)
    {
        _context = context;
    }

    public async Task<ContactUs> AddContactAsync(ContactUs contact)
    {
        contact.CreatedAt = DateTime.UtcNow;
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }
}
