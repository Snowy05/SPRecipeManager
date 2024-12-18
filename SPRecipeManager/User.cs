using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    public class User
    {
        public string Username {  get; set; }
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public RecipeManager UserRecipes { get; set; } = new RecipeManager();

        public User (string username, string password, bool isAdmin)
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
        //Saving and Loading Recipes(PerUser)
        public void LoadRecipes()
        {
            string filename = $"{Username}_recipes.txt";
            UserRecipes.LoadFromFile(filename);
        }

        public void SaveRecipes()
        {
            string filename = $"{Username}_recipes.txt";
            UserRecipes.SaveToFile(filename);
        }
        //Admin inherits from the "General User"
    }

    public class Admin : User
    {
        public List<User> Users { get; private set; } = new List<User>();
        public Admin(string username, string password) : base(username, password, true) { }
        public void AddNewUser(User user)
        {
            Users.Add(user);
            SaveUsersToFile();
        }

        public void RemoveUser(User user) { }

        public User GetUser(string username)
        {
            return Users.Find(un => un.Username == username);
        }

        //Saving Users
        public void SaveUsersToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("users.txt"))
                {
                    foreach (var user in Users)
                    {
                        writer.WriteLine($"{user.Username}|{user.Password}|{user.IsAdmin}");
                    }
                }
                Console.WriteLine("Users saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }
        public void LoadUsersFromFile()
        {
            if (File.Exists("users.txt"))
            {
                using (StreamReader reader = new StreamReader("users.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        string username = parts[0];
                        string hashedPassword = parts[1];
                        bool isAdmin = bool.Parse(parts[2]);

                        Users.Add(new User(username, hashedPassword, isAdmin));
                    }
                }
            }
        }

    }
}
