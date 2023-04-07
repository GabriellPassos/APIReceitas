using Microsoft.EntityFrameworkCore;
using ReitasAPI.Data;

namespace ReitasAPI.Models
{
    public class UnitMesure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        List<Ingredient_UnitMesure> ingrediente_UnitMesure { get; set; }
        public UnitMesure(string name)
        {
            Name = name;
        }
    }
}
