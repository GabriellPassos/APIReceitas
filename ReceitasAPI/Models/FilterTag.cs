using ReitasAPI.Modelos;

namespace ReitasAPI.Models
{
    public class FilterTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Recipe_FilterTag> Recipe_FilterTags { get; set; }

        public FilterTag(string name)
        {
            Name = name;
        }
    }
}
