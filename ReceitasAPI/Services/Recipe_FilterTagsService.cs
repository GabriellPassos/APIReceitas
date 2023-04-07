using Microsoft.EntityFrameworkCore;
using ReitasAPI.Data;
using ReitasAPI.Modelos;
using ReitasAPI.Models;

namespace ReitasAPI.Services
{
    public class Recipe_FilterTagsService
    {
        public RecipeContext _context { get; set; }
        public Recipe_FilterTagsService(RecipeContext recipeContext)
        {
            _context = recipeContext;
        }
        public void Create(string tagName, int recipeId)
        {
            var tag = _context.FilterTags.FirstOrDefault(x => x.Name.ToLower() == tagName.ToLower());
            if (tag != null)
            {
                Recipe recipe = _context.Recipes.FirstOrDefault(x => x.Id == recipeId);
                var newRecipeTag = new Recipe_FilterTag(tag, recipe);
                _context.Recipe_FilterTags.Add(newRecipeTag);
                _context.SaveChanges();

            }
        }
        public void Remove(string tagName, int recipeId)
        {
            _context.Recipe_FilterTags.Remove(_context.Recipe_FilterTags.Where(x => x.RecipeId == recipeId)
                .FirstOrDefault(tag => tag.FilterTag.Name.ToLower() == tagName.ToLower()));
            _context.SaveChanges();
        }
        public List<Recipe> SearchRecipeByTagName(string tagName)
        {
            return _context.Recipe_FilterTags.Include( x => x.Recipe)
                .Where(x => x.FilterTag.Name.ToLower() == tagName.ToLower()).Select(x => x.Recipe).ToList();
        }
        public List<string> SearchTagNameByRecipe(int recipeId)
        {
            return _context.Recipe_FilterTags.
                Where(x => x.RecipeId == recipeId).Select(x => x.FilterTag.Name).ToList();
        }
    }
}
