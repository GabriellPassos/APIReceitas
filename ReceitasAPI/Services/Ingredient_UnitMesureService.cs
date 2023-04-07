using ReitasAPI.Data;
using ReitasAPI.Modelos;
using ReitasAPI.Models;

namespace ReitasAPI.Services
{
    public class Ingredient_UnitMesureService
    {
        public RecipeContext _context;

        public Ingredient_UnitMesureService(RecipeContext context)
        {
            _context = context;
        }
        public Ingredient_UnitMesure Create(int qnt, Ingredient ingrediente, UnitMesure unidadeMedida,Recipe receita)
        {
            Ingredient_UnitMesure ingredienteUnidade = new Ingredient_UnitMesure(qnt, ingrediente, unidadeMedida, receita);
            _context.Ingredients_UnitsMesure.Add(ingredienteUnidade);
            _context.SaveChanges();
            return ingredienteUnidade;
        }

        public List<Ingredient_UnitMesure> Search(int? recipeId)
        {
            return _context.Ingredients_UnitsMesure.Where(x => x.RecipeId == recipeId).ToList();
        }
    }
}
