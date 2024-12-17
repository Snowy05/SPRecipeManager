using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    public class Recipe
    {
        public int RecipeNumber { get; set; }
        public string RecipeName { get; set; }
        public List<Recipe> Ingridients { get; set; }

        public string Instructions { get; set; }

        public Recipe (int recipeNumber, string recipeName, List<Recipe> ingridients, string instructions)
        {
            RecipeNumber = recipeNumber;
            RecipeName = recipeName;
            Ingridients = ingridients;
            Instructions = instructions;
            //Create global recipes for guests they can look for recipes for that exact name, lets say 1000 recipes and use CACHE in order to save memory
        }
    }
}
