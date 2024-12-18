using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    public class GlobalRecipeManager
    {
        private Dictionary<int, Recipe> recipes = new Dictionary<int, Recipe>(); // This is the cache
        private List<Recipe> recipeRequests = new List<Recipe>();

        // Load recipes from a file into the cache
        public void LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Assuming a file format: RecipeNumber|RecipeName|Ingredients|Instructions
                        var parts = line.Split('|');
                        if (parts.Length == 4)
                        {
                            int recipeNumber = int.Parse(parts[0]);
                            string recipeName = parts[1];
                            List<string> ingredients = parts[2].Split(',').ToList();
                            string instructions = parts[3];
                            recipes[recipeNumber] = new Recipe(recipeNumber, recipeName, ingredients, instructions);
                        }
                        else
                        {
                            Console.WriteLine("Invalid line format: " + line);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Recipe file not found.");
            }

        }

        // Retrieve all cached recipes
        public List<Recipe> GetAllRecipes()
        {
            return recipes.Values.ToList();
        }
        public List<Recipe> SearchRecipes(string keyword) 
        { 
            return recipes.Values.Where(r =>
            r.RecipeName.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || r.Ingredients.Any(i => i.Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToList(); 
        }
            // Get the next recipe number
            public int GetNextRecipeNumber()
            {
                if (recipes.Count == 0) return 1;
                return recipes.Keys.Max() + 1;
            }

            // Add a recipe request

            public void AddRecipeRequest(Recipe recipe)
            {
                recipeRequests.Add(recipe);
                SaveRecipeRequestsToFile();
            }

            // Get all recipe requests
            public List<Recipe> GetAllRecipeRequests()
            {
                return new List<Recipe>(recipeRequests);
            }

            // Approve a recipe request
            public void ApproveRecipeRequest(int recipeNumber)
            {
                var recipe = recipeRequests.FirstOrDefault(r => r.RecipeNumber == recipeNumber);
                if (recipe != null)
                {
                    recipes[recipeNumber] = recipe;
                    recipeRequests.Remove(recipe);
                    SaveRecipesToFile();
                    SaveRecipeRequestsToFile();
                }
            }

            // Reject a recipe request
            public void RejectRecipeRequest(int recipeNumber)
            {
                var recipe = recipeRequests.FirstOrDefault(r => r.RecipeNumber == recipeNumber);
                if (recipe != null)
                {
                    recipeRequests.Remove(recipe);
                    SaveRecipeRequestsToFile();
                }
            }

        // Save methods
        public void SaveRecipesToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("global_recipes.txt"))
                {
                    foreach (var recipe in recipes.Values)
                    {
                        string ingredients = string.Join(",", recipe.Ingredients);
                        writer.WriteLine($"{recipe.RecipeNumber}|{recipe.RecipeName}|{ingredients}|{recipe.Instructions}");
                    }
                }
                Console.WriteLine("Recipes saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving recipes: {ex.Message}");
            }
        }

        public void SaveRecipeRequestsToFile()
        {
            using (StreamWriter writer = new StreamWriter("recipe_requests.txt"))
            {
                foreach (var recipe in recipeRequests)
                {
                    string ingredients = string.Join(",", recipe.Ingredients);
                    writer.WriteLine($"{recipe.RecipeNumber}|{recipe.RecipeName}|{ingredients}|{recipe.Instructions}");
                }
            }
            Console.WriteLine("Recipe requests saved successfully.");
        }

        public void LoadRecipesFromFile()
        {
                using (StreamReader reader = new StreamReader("global_recipes.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 4)
                        {
                            int recipeNumber = int.Parse(parts[0]);
                            string recipeName = parts[1];
                            List<string> ingredients = parts[2].Split(',').ToList();
                            string instructions = parts[3];
                            recipes[recipeNumber] = new Recipe(recipeNumber, recipeName, ingredients, instructions);
                        }
                        else
                        {
                            Console.WriteLine("Invalid line format: " + line);
                        }
                    }
                }
        }

        public void LoadRecipeRequestsFromFile()
        {
                using (StreamReader reader = new StreamReader("recipe_requests.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 4)
                        {
                            int recipeNumber = int.Parse(parts[0]);
                            string recipeName = parts[1];
                            List<string> ingredients = parts[2].Split(',').ToList();
                            string instructions = parts[3];
                            recipeRequests.Add(new Recipe(recipeNumber, recipeName, ingredients, instructions));
                        }
                        else
                        {
                            Console.WriteLine("Invalid line format: " + line);
                        }
                    }
                }
        }


    }

}
