using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    public class CurseWordChecker
    {
        private HashSet<string> curseWords = new HashSet<string> 
        {
            "piss", "idiot", "badword1",
        }; 
        public bool ContainsCurseWords(string text) 
        { 
            var words = text.Split(' ').Select(word => word.Trim().ToLower()); 
            foreach (var word in words) { if (curseWords.Contains(word)) 
                { 
                    return true; 
                } 
        } 
            return false;
        }
    }
}
