using System;
using Twitter.Models;

namespace Twitter.Interfaces
{
    public interface IUserService
    {
        User? GetUserById(int id); //fetch user by id from database
        IEnumerable<User> GetAllUsers(); //listing all users..//here Ienumarable will just read, not write //ICollection will read and write
        public void RegisterUser(User user);
        void RemoveUser(int id); //delete user from db
        void UpdateUser(User user); //update user in db
        bool IsEmailTaken(string email);
    }
}