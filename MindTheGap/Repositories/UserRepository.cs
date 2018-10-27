using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MindTheGap.Models;

namespace MindTheGap.Repositories
{
    public class UserRepository
    {
        private List<User> fakeUsers = new List<User>()
        {
            new User() {UserName = "Trev", Password = "trev"}
        };

        public User GetUser(string username)
        {
            return fakeUsers.FirstOrDefault(u => u.UserName == username);
        }

        public void AddUser(User user)
        {
            if (fakeUsers.Any(u => u.UserName == user.UserName))
                return;
            fakeUsers.Add(user);
        }
    }
}