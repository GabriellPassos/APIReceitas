using ReitasAPI.Data;
using ReitasAPI.Modelos;
using ReitasAPI.Models;

namespace ReitasAPI.Services
{
    public class UnitMesureService
    {
        RecipeContext _context;

        public UnitMesureService(RecipeContext context)
        {
            _context = context;
        }
        public UnitMesure Create(string name)
        {
            UnitMesure unidadeMedida = _context.UnitsMesure.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (unidadeMedida == null)
            {
                unidadeMedida = new UnitMesure(name);
                _context.UnitsMesure.Add(unidadeMedida);
                _context.SaveChanges();
                return unidadeMedida;
            }
            return unidadeMedida;
        }
        public UnitMesure Search(int unitMesureId) { return _context.UnitsMesure.FirstOrDefault(x => x.Id == unitMesureId); }
    }
}
