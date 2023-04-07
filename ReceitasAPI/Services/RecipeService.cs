using ReitasAPI.Data;
using ReitasAPI.Modelos;
using ReitasAPI.Models;

namespace ReitasAPI.Services
{
    public class RecipeService
    {
        public readonly RecipeContext _context;
        public RecipeService(RecipeContext recipeContext)
        {
            _context = recipeContext;
        }

        public Recipe Create(string name, string timer, int portions, string stepByStep, List<FilterTag> tags, User user)
        {
            if (user != null)
            {
                Recipe newRecipe = new Recipe(name, timer, portions, stepByStep, tags, user);
                _context.Recipes.Add(newRecipe);
                _context.SaveChanges();
                return newRecipe;
            }
            return null;
        }
        public List<Recipe> Search(int userId)
        {
            return _context.Recipes.Where(x => x.User.Id == userId).ToList(); ;
        }
        public Recipe SearchByRecipe(int recipeId)
        {
            return _context.Recipes.FirstOrDefault(x => x.Id == recipeId);

        }
        public Recipe Remove(int id)
        {
            Recipe recipe = _context.Recipes.FirstOrDefault(x => x.Id == id);
            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            return recipe;
        }
    }
}
