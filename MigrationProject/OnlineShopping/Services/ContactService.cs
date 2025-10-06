using Online.Contexts;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;

public class ContactService : IContactService
{
    private readonly MigrationContext _context;
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUser;

    public ContactService(MigrationContext context, IUserService userService, ICurrentUserService currentUser)
    {
        _context = context;
        _userService = userService;
        _currentUser = currentUser;
    }

    public async Task<ContactUs> AddContactAsync(SupportDTO contact)
    {
        var user = await _userService.GetByIdAsync(_currentUser.Id);

        var con = new ContactUs
        {
            Name = contact.Name,
            Email = user.CustomerEmail,
            Phone = user.CustomerPhone,
            Message = contact.Message,
            CreatedAt = DateTime.UtcNow
        };

        _context.Contacts.Add(con);
        await _context.SaveChangesAsync();
        return con;
    }
}
