using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{

    public class RecipeManager
    {
        //Dictionary time complexity for lookups, can retrieve a recipe by its recipe number almost instantly
        private Dictionary<int, Recipe> recipes = new Dictionary<int, Recipe>();
        private int nextRecipeNumber = 1;



        public List<Recipe> GetAllRecipes()
        {
            return recipes.Values.ToList();
        }

        //Displaying the recipe
        public void DisplayAllRecipeNames()
        {
            Console.Clear();
            foreach (var recipe in recipes.Values)
            {
                Console.WriteLine($"Recipe #{recipe.RecipeNumber}: {recipe.RecipeName}");
            }
        }
        public Recipe GetRecipe(int recipeNumber)
        {
            recipes.TryGetValue(recipeNumber, out var recipe);
            return recipe;
        }
        public void DisplayRecipe(int recipeNumber)
        {
            if (recipes.TryGetValue(recipeNumber, out var recipe))
            {
                Console.Clear();
                Console.WriteLine($"Recipe :{recipe.RecipeNumber}: {recipe.RecipeName}");
                Console.WriteLine($"Ingredients: {string.Join(", ", recipe.Ingredients)}");
                Console.WriteLine($"Instructions: {recipe.Instructions}");
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
        }
        public void AddRecipe(string name, List<string> ingredients, string instructions)
        {
            Recipe recipe = new Recipe(nextRecipeNumber++, name, ingredients, instructions);
            recipes.Add(recipe.RecipeNumber, recipe);
        }
        public void RemoveUserRecipe(int recipeNumber)
        {
            recipes.Remove(recipeNumber);
        }

        public List<Recipe> SearchRecipes(string getrecipes)
        {
            var results = recipes.Values
                .Where(r => r.RecipeName.Contains(getrecipes, StringComparison.OrdinalIgnoreCase) ||
                            r.Ingredients.Any(i => i.Contains(getrecipes, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return results;
        }

        public void SaveToFile(string fileName = "recipes.txt")
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var recipe in recipes.Values)
                {
                    writer.WriteLine($"{recipe.RecipeNumber}|{recipe.RecipeName}|{string.Join(",", recipe.Ingredients)}|{recipe.Instructions}");
                }
            }
            Console.WriteLine("Recipes saved successfully.");
        }



        public void LoadFromFile(string fileName = "recipes.txt")
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 4)
                        {
                            int recipeNumber = int.Parse(parts[0]);
                            string recipeName = parts[1];
                            List<string> ingredients = new List<string>(parts[2].Split(','));
                            string instructions = parts[3];

                            recipes.Add(recipeNumber, new Recipe(recipeNumber, recipeName, ingredients, instructions));
                        }
                        else
                        {
                            Console.WriteLine($"Invalid recipe format in line: {line}");
                        }
                    }

                    if (recipes.Count > 0)
                    {
                        nextRecipeNumber = recipes.Keys.Max() + 1;
                    }
                }
                Console.WriteLine("Recipes loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while loading recipes: {ex.Message}");
            }
        }


    }
}
