using ReitasAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReitasAPI.Modelos
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StepByStep { get; set; }
        public string Timer { get; set; }
        public int Portions { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Ingredient_UnitMesure> Ingredients { get; set; }
        public List<Recipe_FilterTag> Recipe_FilterTags { get; set; }
        public List<Picture> Pictures { get; set; }
        
        public Recipe()
        {
        }
        public Recipe(string name, string timer, int portions, string stepByStep,List<FilterTag> tags, User user)
        {
            Name = name;
            Timer = timer;
            Portions = portions;
            StepByStep = stepByStep;
            User = user;
        }


    }
}
