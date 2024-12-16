using System;
using System.Collections.Generic;


namespace SPRecipeManager
{
    internal class Program
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
                        Console.WriteLine(" 1) Log-In ");
                        Console.WriteLine(" 2) Register ");
                        Console.WriteLine(" 3) Exit" );
                        Console.WriteLine("Please select an option:");
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
            Console.Clear ();
            Console.WriteLine("=========================");
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            Console.WriteLine("=========================");

            string password = Console.ReadLine();

        }
        static void CallRegister()
        {

        }
        static void CallAdminMenu()
        {

        }

        static void CallUserMenu()
        {

        }
    }
}
