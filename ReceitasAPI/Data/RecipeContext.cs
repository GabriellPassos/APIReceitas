using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Attributes;
using ReitasAPI.Modelos;
using ReitasAPI.Models;

namespace ReitasAPI.Data
{
    public class RecipeContext : DbContext
    {
        public RecipeContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<UnitMesure> UnitsMesure { get; set; }
        public DbSet<Ingredient_UnitMesure> Ingredients_UnitsMesure { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FilterTag> FilterTags { get; set; }
        public DbSet<Recipe_FilterTag> Recipe_FilterTags { get; set; }
    }
}
