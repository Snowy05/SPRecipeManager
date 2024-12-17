using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    public class PasswordChecker
    {
        public static bool IsUppercase(string password)
        {
            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPasswordStrong(string password)
        {
            if (IsUppercase(password) && password.Length > 8)
            {
                return true;
            }
            return false;
        }
    }
}
