using System;
using Twitter.Models;

namespace Twitter.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int id); //fetch user by id from database
        IEnumerable<User> GetAllUsers(); //listing all users..//here Ienumarable will just read, not write //ICollection will read and write
        void AddUser(User user);//add user to db
        void RemoveUser(int id); //delete user from db
        void UpdateUser(User user); //update user in db

        void UpdateUserById(int id, User user);
    }
}