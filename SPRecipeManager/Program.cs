using System;
using System.Collections.Generic;
using static SPRecipeManager.User;


namespace SPRecipeManager
{
    class Program
    {
        static Admin admin = new Admin("admin", "adminpass");
        static User currentUser;
        //static RecipeManager globalRecipes = new RecipeManager();


        static void Main(string[] args)
        {
            admin.LoadUsersFromFile();


            //try
            //{
            //    globalRecipes.LoadFromFile("global_recipes.txt");
            //    Console.WriteLine("Global recipes loaded successfully.");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error loading global recipes: {ex.Message}");
            //}
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
                currentUser.LoadRecipes();
                Console.WriteLine($"Welcome, {username}");
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

        static void CallUserMenu()
        {
            Console.Clear();
            Console.WriteLine("=========================");
            Console.WriteLine($" Welcome, {currentUser.Username}!");
            Console.WriteLine(" 1) View all recipes");
            Console.WriteLine(" 2) View your recipes");
            Console.WriteLine(" 3) View shopping list");
            Console.WriteLine(" 4) Logout");
            Console.WriteLine("=========================");


            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine();
                    break;
                case "2":
                    AllUserRecipes(currentUser.UserRecipes);
                    break;
                case "3":
                    //CallShoppingList(shoppingList);
                    break;
                case "4":
                    CallLogout();
                    break;
                default: 
                    Console.WriteLine("Please select a valid option!");
                    break;
            }

            static void AllUserRecipes(RecipeManager recipeManager)
            {
                Console.Clear();
                Console.WriteLine("=========================");
       
                Console.WriteLine("What Would you like to do?");
                Console.WriteLine(" 1) Add a new recipe");
                Console.WriteLine(" 2) Delete a recipe");
                Console.WriteLine(" 3) View a recipe");
                Console.WriteLine(" 5) Search Recipe");
                Console.WriteLine(" 6) Go back");

                Console.WriteLine("All Recipes:");
                foreach (var recipe in recipeManager.GetAllRecipes())
                {
                    Console.WriteLine($"#{recipe.RecipeNumber}: {recipe.RecipeName}");
                }


                switch (Console.ReadLine())
                {
                    case "1":

                        AddRecipe(currentUser.UserRecipes);
                        break;
                    case "2":
                        RemoveUserRecipe(currentUser.UserRecipes);
                        break;
                    case "3":
                        Console.WriteLine();
                        break;
                    case "4":
                        Console.WriteLine();
                        break;
                    case "5":
                        SearchRecipes(currentUser.UserRecipes);
                        break;
                    case "6":
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine("Select a valid option!");
                        break;
                }

            }
            static void AddRecipe(RecipeManager recipeManager)
            {
                Console.Clear();
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
            static void RemoveUserRecipe(RecipeManager recipeManager)
            {
                foreach (var recipe in recipeManager.GetAllRecipes())
                {
                    Console.WriteLine($"#{recipe.RecipeNumber}: {recipe.RecipeName}");
                }
                Console.Write("Enter recipe number to remove: ");
                if (int.TryParse(Console.ReadLine(), out int recipeNumber))
                {
                    recipeManager.RemoveUserRecipe(recipeNumber);
                    Console.WriteLine("Recipe removed successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid recipe number. Please try again.");
                }
                Console.ReadKey();
            }
            static void SearchRecipes(RecipeManager recipeManager)
            {
                Console.Write("Enter a keyword to search for recipes: ");
                string getAllRecipes = Console.ReadLine();

                var results = recipeManager.SearchRecipes(getAllRecipes);

                if (results.Any())
                {
                    Console.Clear();
                    Console.WriteLine("=========================");
                    Console.WriteLine("Search Results:");
                    foreach (var recipe in results)
                    {
                        Console.WriteLine($"#{recipe.RecipeNumber}: {recipe.RecipeName}");
                    }
                    Console.WriteLine("=========================");
                    ViewRecipeDetails(currentUser.UserRecipes);
                }
                else
                {
                    Console.WriteLine("No recipes found matching your search.");
                }
                Console.ReadKey();

                static void ViewRecipeDetails(RecipeManager recipeManager)
                {
                    Console.Write("Enter the recipe number to view details");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int recipeNumber))
                    {
                        recipeManager.DisplayRecipe(recipeNumber);
                    }
                    Console.ReadKey();
                }
            }
            static void CallLogout()
            {
                Console.Clear();
                currentUser.SaveRecipes();
                currentUser = null;
                Console.WriteLine("Logged out successfully.");
            }
        }
        static void CallAdminMenu()
        {
            bool adminRun = true;
            while (adminRun)
            {
                Console.Clear();
                Console.WriteLine("=========================");
                Console.WriteLine(" Admin Menu:");
                Console.WriteLine(" 1) Add User ");
                Console.WriteLine(" 2) List All Users ");
                Console.WriteLine(" 3) Logout");
                Console.WriteLine("=========================");

                switch (Console.ReadLine())
                {
                    case "1":
                        AdminFunctionAddUser();
                        break;
                    case "2":
                        admin.AdminFunctionListAllUsers();
                        Console.WriteLine("Press any key to return to the admin menu...");
                        Console.ReadKey();
                        break;
                    case "3":
                        currentUser = null;
                        adminRun = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Please Select a valid option!");
                        break;
                }
            }
            static void AdminFunctionAddUser()
            {
                Console.Clear();
                Console.WriteLine("Enter username:");
                string username = Console.ReadLine();

                if (!string.IsNullOrEmpty(username))
                {
                    if (admin.GetUser(username) == null)
                    {
                        Console.WriteLine("Enter password:");
                        string password = Console.ReadLine();

                        if (!string.IsNullOrEmpty(password))
                        {
                            Console.WriteLine("Is this user an admin? (yes/no):");
                            string isUserAdmin = Console.ReadLine();

                            bool isAdmin = isUserAdmin.Equals("yes");

                            var newUser = new User(username, password, isAdmin);
                            admin.AddNewUser(newUser);

                            Console.Clear();
                            Console.WriteLine("=========================");
                            Console.WriteLine($"User '{username}' registered successfully.");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("=========================");
                            Console.WriteLine("Password cannot be empty.");
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
                static void CallAdminLogout()
                {
                    Console.Clear();
                    currentUser.SaveRecipes();
                    currentUser = null;
                    Console.WriteLine("Logged out successfully.");
                }
            }
            static void CallAdminLogout()
            {
                Console.Clear();
                currentUser = null;
                Console.WriteLine("Logged out successfully.");
            }
        }        
    }
}
