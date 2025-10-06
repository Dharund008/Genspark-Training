using Twitter.Interfaces;
using Twitter.Models;

namespace Twitter.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userrepo;//implements IUserRepo interface.

        public UserService(IUserRepository userrepo)
        {
            _userrepo = userrepo;
        }

        public User? GetUserById(int id)
        {
            var getuser = _userrepo.GetUserById(id);
            return getuser;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = _userrepo.GetAllUsers(); // Fetch users once
            if (!users.Any()) // Check if list is empty
            {
                throw new Exception("No Users Found");
            }
            return users; // If users exist, return the list
        }

        public void RegisterUser(User user)
        {
            if (string.IsNullOrEmpty(user.Username))
            {
                throw new Exception("Username cannot be empty.");
            }
            if (user.Username.Length < 6)
            {
                throw new Exception("Username must be at least 6 characters long.");
            }
            if (string.IsNullOrEmpty(user.Email) || !user.Email.Contains("@"))
            {
                throw new Exception("Invalid email format.");
            }
            if (IsEmailTaken(user.Email))
            {
                throw new Exception("Email is already taken.");
            }
            if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 8)
            {
                throw new Exception("Password must be at least 8 characters long.");
            }
            Console.WriteLine($"New User Registered : {user.Username} ");

            _userrepo.AddUser(user);
        }

        public void RemoveUser(int id)
        {
            var users = _userrepo.GetAllUsers();

            if (!users.Any())
            {
                throw new Exception("No users available. Please add a user first.");
            }

            var userToRemove = users.FirstOrDefault(p => p.Id == id);
            
            if (userToRemove == null)
            {
                throw new Exception("Invalid User Id");
            }
            _userrepo.RemoveUser(id);//void return type so, no need to return anything
        }

        public void UpdateUser(User user)
        {
            var rem = _userrepo.GetUserById(user.Id);
            if (rem == null)
            {
                throw new Exception("Invalid User Id");
            }
            _userrepo.UpdateUser(user);
        }

        public bool IsEmailTaken(string email)
        {
            return _userrepo.GetAllUsers().Any(p => p.Email == email);
        }

    }
}