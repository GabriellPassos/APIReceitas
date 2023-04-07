using ReitasAPI.Models;

namespace ReitasAPI.Modelos
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ingredient_UnitMesure> Ingredient_UnitMesure { get; set; }
        public Ingredient()
        {
        }
        public Ingredient(string name)
        {
            Name = name;
        }
    }
}
