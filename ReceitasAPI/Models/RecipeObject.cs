namespace ReitasAPI.Models
{
    public class RecipeObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Portions { get; set; }
        public string Time { get; set; }
        public string StepByStep { get; set; }
        public List<JsonIngredientModel> Ingredients { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Pictures { get; set; }
        public string UserName { get; set; }
    }
    public class JsonIngredientModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string UnitMesure { get; set; }
    }
}
