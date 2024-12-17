using System;
using System.Collections.Generic;
using static SPRecipeManager.User;


namespace SPRecipeManager
{
    class Program
    {
        static Admin admin = new Admin("admin", "adminpass");
        static User currentUser;
        static RecipeManager globalRecipes = new RecipeManager();

        static void Main(string[] args)
        {
            bool loginRun = true;

            while (loginRun)
            {
                if (currentUser == null)
                {
                        Console.WriteLine("=========================");
                        Console.WriteLine(" Please select an option:");
                        Console.WriteLine(" 1) Log-In ");
                        Console.WriteLine(" 2) Register ");
                        Console.WriteLine(" 3) Exit" );
                        Console.WriteLine("=========================");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            CallLogin();
                            break;
                        case "2":
                            CallRegister();
                            break;
                        case "3":   
                            loginRun = false;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Please Select a valid option!");
                            break;
                    }

                }
                else if (currentUser.IsAdmin)
                {
                    CallAdminMenu();
                }
                else
                {
                    CallUserMenu();
                }
            }
        }
        static void CallLogin()
        {
            Console.Clear();
            Console.WriteLine("=========================");
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();
            Console.WriteLine("=========================");
            currentUser = admin.GetUser(username);

            if (currentUser != null && currentUser.PasswordVerification(password))
            {
                Console.WriteLine($"Welcome,{username}");
            }
            else
            {
                Console.WriteLine("Incorrect username or password!"); 
                currentUser = null;
            }
        }


        static void CallRegister()
        {
            Console.Clear();
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();

            if (!string.IsNullOrEmpty(username))
            {
                if (admin.GetUser(username) == null)
                {
                    Console.WriteLine("Enter your password:");
                    string password = Console.ReadLine();

                    if (PasswordChecker.IsPasswordStrong(password))
                    {
                        var newUser = new User(username, password, false); 
                        admin.AddNewUser(newUser);
                        Console.WriteLine($"User: {username} registered successfully.");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("=========================");
                        Console.WriteLine("Too weak password, try again!");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("=========================");
                    Console.WriteLine("Username is already taken.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("=========================");
                Console.WriteLine("Username cannot be empty.");
            }


        }
        static void CallAdminMenu()
        {


        }

        static void CallUserMenu()
        {
            Console.Clear();
            Console.WriteLine($" Welcome, {currentUser.Username}!");
            Console.WriteLine(" 1)Add a new recipe");
            Console.WriteLine(" 2) View all recipes");
            Console.WriteLine(" 3) View your recipes");
            Console.WriteLine(" 4) Remove a recipe");
            Console.WriteLine(" 6) View shopping list");
            Console.WriteLine(" 7) Search recipes");
            Console.WriteLine(" 8) Manage users (Admin)");
            Console.WriteLine(" 9) Logout");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine();
                    break;
                case "2": 
                    Console.WriteLine();
                    break;
                case "3":
                    Console.WriteLine();
                    break;
                case "4":
                    Console.WriteLine();
                    break;
                case "5":
                    Console.WriteLine();
                    break;
                case "6":
                    Console.WriteLine();
                    break;
                case "7":
                    Console.WriteLine();
                    break;
                case "8":
                    Console.WriteLine();
                    break;
                case "9":
                    Console.WriteLine();
                    break;
                default: 
                    Console.WriteLine("Please select a valid option!");
                    break;
            }
            static void AddRecipe(RecipeManager recipeManager)
            {
                Console.Write("Enter recipe name: ");
                string name = Console.ReadLine();

                if (!string.IsNullOrEmpty(name))
                {
                    Console.Write("Enter recipe ingredients (comma-separated): ");
                    List<string> ingredients = new List<string>(Console.ReadLine().Split(','));

                    Console.Write("Enter recipe instructions: ");
                    string instructions = Console.ReadLine();
                    recipeManager.AddRecipe(name, ingredients, instructions);

                    Console.WriteLine("Recipe added successfully!");
                }
                else Console.WriteLine("Please fill out the the name of the recipe!");

                Console.ReadKey();
            }
        }
    }
}
