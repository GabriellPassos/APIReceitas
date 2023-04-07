
using Microsoft.AspNetCore.Mvc;
using ReitasAPI.Modelos;
using ReitasAPI.Models;
using ReitasAPI.Services;


namespace ReitasAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class Recipe_FilterTagController : ControllerBase
    {
        public Recipe_FilterTagsService _recipeFilterTagService { get; set; }
        public BuidRecipeService _buidRecipeService { get; set; }
        public Recipe_FilterTagController(Recipe_FilterTagsService recipe_FilterTagService, BuidRecipeService buildRecipeService)
        {
            _recipeFilterTagService = recipe_FilterTagService;
            _buidRecipeService = buildRecipeService;
        }
        [HttpPost]
        public ActionResult<string> Insert(string tagName, int recipeId)
        {
            try
            {
                _recipeFilterTagService.Create(tagName, recipeId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("success");
        }
        [HttpDelete]
        public ActionResult<string> Remove(string tagName, int recipeId)
        {
            try
            {
                _recipeFilterTagService.Remove(tagName, recipeId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("success");
        }
        [HttpGet]
        public ActionResult<List<string>> TagByRecipeId(int recipeId)
        {
            List<string> tagList = new List<string>();
            try
            {
                tagList = _recipeFilterTagService.SearchTagNameByRecipe(recipeId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(tagList);
        }

        [HttpGet]
        public ActionResult<List<Recipe>> RecipeByTagName(string name)
        {
            try
            {
                List<Recipe> recipe = _recipeFilterTagService.SearchRecipeByTagName(name);
                if (recipe.Count > 0)
                {
                    return Ok(_buidRecipeService.BuildRecipeObject(recipe));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
