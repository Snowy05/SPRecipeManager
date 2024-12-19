using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPRecipeManager
{
    public class GlobalRecipeManager
    {
        //Dictionary time complexity for lookups, can retrieve a recipe by its recipe number almost instantly
        private Dictionary<int, Recipe> recipes = new Dictionary<int, Recipe>(); //caching
        private List<Recipe> recipeRequests = new List<Recipe>();

        //loading recipes to cache
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
                            Console.WriteLine("Invalid line format at: " + line);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("recipe file not found");
            }

        }

        //getting cahced recipes
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

        public int GetNextRecipeNumber()
        {
            if (recipes.Count == 0) return 1;
            return recipes.Keys.Max() + 1;
        }
        //Recipe request section
        public void AddRecipeRequest(Recipe recipe)
        {
            recipeRequests.Add(recipe);
            SaveRecipeRequestsToFile();
        }
        public List<Recipe> GetAllRecipeRequests()
        {
            return new List<Recipe>(recipeRequests);
        }
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
        public void RejectRecipeRequest(int recipeNumber)
        {
            var recipe = recipeRequests.FirstOrDefault(r => r.RecipeNumber == recipeNumber);
            if (recipe != null)
            {
                recipeRequests.Remove(recipe);
                SaveRecipeRequestsToFile();
            }
        }

        //saving methods 
        public void SaveRecipesToFile()
        {

            using (StreamWriter writer = new StreamWriter("global_recipes.txt"))
            {
                foreach (var recipe in recipes.Values)
                {
                    string ingredients = string.Join(",", recipe.Ingredients);
                    writer.WriteLine($"{recipe.RecipeNumber}|{recipe.RecipeName}|{ingredients}|{recipe.Instructions}");
                }
            }
            Console.WriteLine("Recipes saved successfully!");
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

        //loading up recipe(s)
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

        //loading up request(s)
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
