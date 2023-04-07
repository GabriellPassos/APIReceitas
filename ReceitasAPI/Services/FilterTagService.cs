using ReitasAPI.Data;
using ReitasAPI.Modelos;
using ReitasAPI.Models;

namespace ReitasAPI.Services
{
    public class FilterTagService
    {
        public RecipeContext _context { get; set; }
        public FilterTagService(RecipeContext recipeContext)
        {
            _context = recipeContext;
        }
        public FilterTag Create(string name)
        {
            FilterTag filterTag = _context.FilterTags.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (filterTag == null)
            {
                filterTag = new FilterTag(name);
                _context.FilterTags.Add(filterTag);
                _context.SaveChanges();
            }
            return filterTag;
        }
        public FilterTag Search(int filterTagId)
        { return _context.FilterTags.FirstOrDefault(x => x.Id == filterTagId); }
        public FilterTag SearchByName(string name)
        { return _context.FilterTags.FirstOrDefault(x => x.Name == name); }
        public FilterTag Remove(string name)
        {
            var filterTag = _context.FilterTags.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            _context.FilterTags.Remove(filterTag);
            _context.SaveChanges();
            return filterTag;
        }
        public List<FilterTag> SearchAll() { return _context.FilterTags.ToList(); }

        public FilterTag SearchById(int id) { return _context.FilterTags.FirstOrDefault(x => x.Id == id); }
    }
}
