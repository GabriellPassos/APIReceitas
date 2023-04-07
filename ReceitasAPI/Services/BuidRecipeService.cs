using Microsoft.AspNetCore.Hosting;
using ReitasAPI.Data;
using ReitasAPI.Modelos;
using ReitasAPI.Models;
using System.Diagnostics;
using System.IO;

namespace ReitasAPI.Services
{
    public class BuidRecipeService
    {
        public RecipeContext _context;
        public IWebHostEnvironment _webHostEnvironment { get; set; }
        private readonly UserService _userService;
        private readonly Recipe_FilterTagsService _recipe_FiltersTagsService;
        private readonly PictureService _pictureService;
        public BuidRecipeService(RecipeContext context, IWebHostEnvironment webHostEnvironment,
            UserService userService, Recipe_FilterTagsService recipe_FilterTagsService, PictureService pictureService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userService = userService;
            _recipe_FiltersTagsService = recipe_FilterTagsService;
            _pictureService = pictureService;
        }
        public void Build(RecipeObject receitaObject, int userId)
        {
            //Os services chamam seus devidos metodos para criação, em ordem para ao final
            //terem seus Id anexados a recipe.
            RecipeService receitaService = new RecipeService(_context);
            IngredientService ingredienteService = new IngredientService(_context);
            UnitMesureService unitMesureService = new UnitMesureService(_context);
            Ingredient_UnitMesureService ingredient_UnitMesureService = new Ingredient_UnitMesureService(_context);
            FilterTagService filterTagService = new FilterTagService(_context);
            List<FilterTag> tags = new List<FilterTag>();
            var user = _userService.Search(userId);
            Recipe recipe = receitaService.Create(receitaObject.Name, receitaObject.Time, receitaObject.Portions, receitaObject.StepByStep, tags, user);

            foreach (var item in receitaObject.Tags)
            {
                FilterTag filterTag = filterTagService.SearchByName(item);
                if (filterTag != null)
                {
                    _recipe_FiltersTagsService.Create(filterTag.Name, recipe.Id);
                }
            }
            foreach (var item in receitaObject.Ingredients)
            {
                int quantity = item.Quantity;
                Ingredient ingredient = ingredienteService.Create(item.Name);
                UnitMesure unitMesure = unitMesureService.Create(item.UnitMesure);
                ingredient_UnitMesureService.Create(quantity, ingredient, unitMesure, recipe);
            }
            foreach (var picture in receitaObject.Pictures)
            {
                _pictureService.Create(recipe, picture);
            }
            /*//Povoando objetos "Foto" com os caminhos das imagens recebidas enquanto as
            //imagens são armazenadas em outro diretório fora do banco de dados.
            PictureService pictureService = new PictureService(_context, _webHostEnvironment);
            List<string> imagesPath = pictureService.Load(images);
            foreach (var path in imagesPath)
            {
                pictureService.Create(path, recipe);
            }*/

        }
        public List<JsonIngredientModel> BuildIngredientLine(int recipeId)
        {
            Ingredient_UnitMesureService ingredient_UnitMesureService = new Ingredient_UnitMesureService(_context);
            UnitMesureService unitMesureService = new UnitMesureService(_context);
            IngredientService ingredientService = new IngredientService(_context);
            List<Ingredient_UnitMesure> ingredients = ingredient_UnitMesureService.Search(recipeId);
            List<JsonIngredientModel> jsonIngredientList = new List<JsonIngredientModel>();
            
            foreach (var item in ingredients)
            {
                JsonIngredientModel jsonIngredientModel = new JsonIngredientModel();
                jsonIngredientModel.Name = ingredientService.Search(item.IngredientId).Name;
                jsonIngredientModel.UnitMesure = unitMesureService.Search(item.UnitMesureId).Name;
                jsonIngredientModel.Quantity = item.Quantity;
                jsonIngredientList.Add(jsonIngredientModel);
            }
            return jsonIngredientList;
        }
        public List<RecipeObject> BuildRecipeObject(List<Recipe> recipesList)
        {
            List<RecipeObject> recipeObjectList = new List<RecipeObject>();
            foreach (var recipe in recipesList)
            {
                RecipeObject recipeObject = new RecipeObject();
                recipeObject.Id = recipe.Id;
                recipeObject.Name = recipe.Name;
                recipeObject.Portions = recipe.Portions;
                recipeObject.StepByStep = recipe.StepByStep;
                recipeObject.Time = recipe.Timer;
                recipeObject.Ingredients = BuildIngredientLine(recipe.Id);
                recipeObject.UserName = _userService.Search(recipe.UserId).Name;
                recipeObject.Tags = _recipe_FiltersTagsService.SearchTagNameByRecipe(recipe.Id);
                recipeObject.Pictures = _pictureService.Search(recipe.Id).Select(x => x.Base64Image).ToList();
                recipeObjectList.Add(recipeObject);
            }
            return recipeObjectList;
        }
    }
}
