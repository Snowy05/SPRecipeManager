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
        public string Username { get; set; }
        public string Password { get; set; } // This will store the password thats been hashed
        public bool IsAdmin { get; set; }
        public RecipeManager UserRecipes { get; set; } = new RecipeManager();

        //Constructor for new users with the already hashed password
        public User(string username, string password, bool isAdmin)
        {
            Username = username;
            Password = PasswordHashMethod(password);
            IsAdmin = isAdmin;
        }

        //constructor loading users from file
        public User(string username, string hashedPassword, bool isAdmin, bool isHashedPassword)
        {
            Username = username;
            Password = isHashedPassword ? hashedPassword : PasswordHashMethod(hashedPassword);
            IsAdmin = isAdmin;
        }

        // Method for hashing the password as i store it in Plain text file
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

        //Method to do given password == hashed password
        public bool PasswordVerification(string password)
        {
            string passwordVerify = PasswordHashMethod(password);
            return Password == passwordVerify;
        }

        //users submission method to Globalrecipies
        public void SubmitRecipeRequest(GlobalRecipeManager globalRecipeManager)
        {
            var curseWordChecker = new CurseWordChecker();
            Console.Clear();

            Console.WriteLine("Enter the recipe name:");
            string recipeName = Console.ReadLine();

            //check if the recipe name is empty
            if (string.IsNullOrWhiteSpace(recipeName))
            {
                Console.WriteLine("Recipe name cannot be empty.");
                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
                return;
            }

            //Check if the recipe name contains curse words
            if (curseWordChecker.ContainsCurseWords(recipeName))
            {
                Console.WriteLine("Recipe name is inappropriate.");
                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
                return;
            }

            //get the ingredients
            Console.WriteLine("Enter the ingredients (comma-separated):");
            List<string> ingredients = Console.ReadLine().Split(',').Select(ingredient => ingredient.Trim()).ToList();

            //check if the ingredients list is empty
            if (ingredients.Count == 0 || ingredients.All(string.IsNullOrWhiteSpace))
            {
                Console.WriteLine("Ingredients cannot be empty.");
                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
                return;
            }

            //instructions
            Console.WriteLine("Enter the instructions:");
            string instructions = Console.ReadLine();

            //check if the instructions are empty
            if (string.IsNullOrWhiteSpace(instructions))
            {
                Console.WriteLine("Recipe instructions cannot be empty.");
                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
                return;
            }

            //check if the instructions contain curse words
            if (curseWordChecker.ContainsCurseWords(instructions))
            {
                Console.WriteLine("Recipe instructions might contain inappropriate language.");
                Console.WriteLine("Press enter to continue.");
                Console.ReadLine();
                return;
            }

            var newRecipeRequest = new Recipe(globalRecipeManager.GetNextRecipeNumber(), recipeName, ingredients, instructions);
            globalRecipeManager.AddRecipeRequest(newRecipeRequest);

            Console.WriteLine("Recipe request submitted for admin approval.");
        }


        //Saving and Loading Recipes 
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
    }

    public class Admin : User
    {
        public List<User> Users { get; private set; } = new List<User>();

        public Admin(string username, string password) : base(username, password, true) { }

        //General Admin Functions
        public void AddNewUser(User user)
        {
            Users.Add(user);
            SaveUsersToFile();
        }

        public void AdminFunctionRemoveUser(User user)
        {
            Users.Remove(user);
            SaveUsersToFile();
        }

        public User GetUser(string username)
        {
            return Users.Find(un => un.Username == username);
        }
        // Admin Request  Functions
            public void FunctionReviewRecipeRequests(GlobalRecipeManager globalRecipeManager)
            {
            Console.Clear();
            var requests = globalRecipeManager.GetAllRecipeRequests();
                if (requests.Count == 0)
                {
                    Console.WriteLine("No pending recipe requests.");
                    return;
                }

                foreach (var request in requests)
                {
                    Console.WriteLine($"Recipe {request.RecipeNumber}: {request.RecipeName}");
                    Console.WriteLine($"Ingredients: {string.Join(", ", request.Ingredients)}");
                    Console.WriteLine($"Instructions: {request.Instructions}");
                    Console.WriteLine("Do you want to approve this recipe? (yes/no)");

                    string response = Console.ReadLine();
                    if (response.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    {
                        globalRecipeManager.ApproveRecipeRequest(request.RecipeNumber);
                        Console.WriteLine("Recipe approved!");
                    }
                    else
                    {
                        globalRecipeManager.RejectRecipeRequest(request.RecipeNumber);
                        Console.WriteLine("Recipe rejected.");
                    }
                }
            }


        // Saving Users and loading usres
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

                        // Add check to ensure we have the correct number of parts
                        if (parts.Length == 3)
                        {
                            string username = parts[0];
                            string hashedPassword = parts[1];
                            bool isAdmin = bool.Parse(parts[2]);

                            Users.Add(new User(username, hashedPassword, isAdmin, true));
                        }
                        else
                        {
                            Console.WriteLine("Invalid line format: " + line);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("User file not found.");
            }
        }

        //lisitng all users for admin
        public void AdminFunctionListAllUsers()
        {
            Console.Clear();
            Console.WriteLine("=============================");
            Console.WriteLine(" All users:");
            foreach (var user in Users)
            {
                Console.WriteLine($"Username: {user.Username}, Admin: {user.IsAdmin}");
            }
            Console.WriteLine("=============================");
        }

    }
}
