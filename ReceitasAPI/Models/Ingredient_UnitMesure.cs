using ReitasAPI.Modelos;
using ReitasAPI.Services;

namespace ReitasAPI.Models
{
    public class Ingredient_UnitMesure
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int UnitMesureId { get; set; }
        public UnitMesure UnitMesure { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public Ingredient_UnitMesure()
        {
        }
        public Ingredient_UnitMesure(int quantity, Ingredient ingredient, UnitMesure unitMesure, Recipe recipe)
        {
            Quantity = quantity;
            Ingredient = ingredient;
            UnitMesure = unitMesure;
            Recipe = recipe;
        }

    }
}
