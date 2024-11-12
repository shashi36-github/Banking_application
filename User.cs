using System;
using System.Collections.Generic;

namespace BankingApplication
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Account> Accounts { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Accounts = new List<Account>();
        }
    }
}
