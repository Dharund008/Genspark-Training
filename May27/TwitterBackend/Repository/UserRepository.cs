using Twitter.Models;
using Twitter.Interfaces;
using Twitter.Contexts; //access to interact with database.

namespace Twitter.Repositories
{
    public class UserRepository : IUserRepository //concrete class implements the IuserRepository interface.
    //connects interface with logic.
    {
        private readonly TwitterContext _context;// holds the database
                                                 //this how repo connects to database and gets data.

        public UserRepository(TwitterContext context)//constructor
        {
            _context = context; //DI to inject database context
        }
        //if this not used, context might be null and repo doesnt know that and also wont know how to get data.

        public User? GetUserById(int id)
        {
            var getuser = _context.Users.FirstOrDefault(u => u.Id == id);
            //Users is the table in database. [public DbSet<User> Users].. in context.cs
            return getuser;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var getall = _context.Users.ToList();
            return getall;
        }

        public void AddUser(User user)
        {
            var add = _context.Users.Add(user); //add user to database
            _context.SaveChanges(); //save changes to database [commiting]
        }

        public void UpdateUser(User user)
        {
            var upd = _context.Users.Update(user); //says that is modified and EF will generate update query to database.
            _context.SaveChanges(); //save changes to database [commiting]
        }

        public void UpdateUserById(int id, User user)
        {
            var ExisitingUser = GetUserById(id);
            if (ExisitingUser != null)
            {
                ExisitingUser.Username = user.Username;
                ExisitingUser.Email = user.Email;
                ExisitingUser.Password = user.Password;
                //UpdateUser(ExisitingUser);
            }
        }

        public void RemoveUser(int id)
        {
            var rem = _context.Users.Find(id);
            //var rem = GetUserById(id);
            if (rem != null)
            {
                _context.Users.Remove(rem);
                _context.SaveChanges();
            }
        }

    }
}