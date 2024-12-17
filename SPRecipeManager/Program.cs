using System;
using System.Collections.Generic;
using static SPRecipeManager.User;


namespace SPRecipeManager
{
    class Program
    {
        static Admin admin = new Admin("admin", "adminpass");
        static User currentUser;
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

        }
    }
}
