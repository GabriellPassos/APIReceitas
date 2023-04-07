using ReitasAPI.Modelos;

namespace ReitasAPI.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Base64Image { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public Picture()
        {
        }
        public Picture(Recipe recipe, string base64Image)
        {
            Base64Image = base64Image;
            Recipe = recipe;
        }
    }
}
