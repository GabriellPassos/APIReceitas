using ReitasAPI.Modelos;

namespace ReitasAPI.Models
{
    public class Recipe_FilterTag
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int FilterTagId { get; set; }
        public FilterTag FilterTag { get; set; }

        public Recipe_FilterTag() { }
        public Recipe_FilterTag(FilterTag filterTag, Recipe recipe)
        {
            FilterTag = filterTag;
            Recipe = recipe;
        }
    }
}
