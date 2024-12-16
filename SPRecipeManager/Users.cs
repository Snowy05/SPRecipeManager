using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    internal class Users
    {
        public string Username {  get; set; }
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public Users (string username, string password, bool isAdmin)
        {
            Username = username;
            Password = PasswordHashMethod(password);
            IsAdmin = isAdmin;
        }

        //Hashing the given password to make safer in the text file
        private string PasswordHashMethod(string password)
        {

          using (SHA256 sha256Pass = SHA256.Create())
            {
                byte[] bytes = sha256Pass.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool PasswordVerification (string password)
        {
            string passwordVerify = PasswordHashMethod(password);
            return Password == passwordVerify;
        }

        //Admin inherits from the "General User"
        public class Admin : Users
        {
            public List<Users> Users { get; private set; } = new List<Users>();
            public Admin(string username, string password) : base(username, password, true) { }

            public void AddNewUser(Users user)
            {
                Users.Add(user);
            }
        }
    }
}
