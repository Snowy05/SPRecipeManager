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
        public List<string> Ingredients { get; set; }
        public string Instructions { get; set; }

        public Recipe(int recipeNumber, string recipeName, List<string> ingredients, string instructions)
        {
            RecipeNumber = recipeNumber;
            RecipeName = recipeName;
            Ingredients = ingredients;
            Instructions = instructions;
        }
    }

}
