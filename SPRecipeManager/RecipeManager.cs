using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    public class RecipeManager
    {

        private Dictionary<string, Recipe> recipes = new Dictionary<string, Recipe>();
        private int nextRecipeNumber = 1;
    
        //public void DisplayRecipe(int recipeNumber)
        //{   
        //    if (recipes.TryGetValue(recipeNumber, out var recipe))
        //    {
        //        Console.Clear();
        //        Console.WriteLine($"Recipe {recipe.RecipeNumber}): {recipe.RecipeName}");
        //        Console.WriteLine($"Ingredients: {string.Join(", ", recipe.Ingridients)}");
        //        Console.WriteLine($"Instructions: {recipe.Instructions}");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Recipe not found.");
        //    }
        //}
    }
}
