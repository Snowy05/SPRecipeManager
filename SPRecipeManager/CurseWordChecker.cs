using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    public class CurseWordChecker
    {
        //hash set to keep them uniqe,look up if a word exists in the set very quickly, regardless of the number of items in our set.
        private HashSet<string> curseWords = new HashSet<string> 
        {
            "piss", "idiot", "badwordone",
        }; 
        public bool ContainsCurseWords(string text) 
        { 
            var words = text.Split(' ').Select(word => word.Trim().ToLower()); //Checking against curse words in submissions
            foreach (var word in words) { if (curseWords.Contains(word)) 
                { 
                    return true; 
                } 
        } 
            return false;
        }
    }
}
