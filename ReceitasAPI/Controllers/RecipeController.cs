using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using ReitasAPI.Modelos;
using ReitasAPI.Models;
using ReitasAPI.Services;
using System.Security.Claims;


namespace ReitasAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RecipeController : Controller
    {
        private readonly BuidRecipeService _buidRecipeService;
        private readonly RecipeService _recipeService;
        public RecipeController(BuidRecipeService buidRecipeService, RecipeService recipeService)
        {
            _buidRecipeService = buidRecipeService;
            _recipeService = recipeService;
        }
        [Authorize]
        [HttpPost]
        public ActionResult<List<string>> New(Object recipe)
        {
            try
            {
                if (User.FindFirstValue("Id") != null)
                {
                    string recipeString = recipe.ToString();
                    int userId = int.Parse(User.FindFirstValue("Id"));
                    JSchema recipeSchema = JSchema.Parse(System.IO.File.ReadAllText("./ReitasAPI/Models/ValidationSchemes/recipeSchema.json"));
                    JObject recipeJObject = JObject.Parse(recipeString);
                    IList<string> validationErrors = new List<string>();
                    if (recipeJObject.IsValid(recipeSchema, out validationErrors))
                    {
                        RecipeObject receitaObject = JsonConvert.DeserializeObject<RecipeObject>(recipeString);
                        _buidRecipeService.Build(receitaObject, userId);
                        return Ok("Success");
                    }
                    if(validationErrors.Count > 0)
                    {
                        throw new Exception("Recipe data format incorrect");
                    }
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }
        [HttpDelete]
        public ActionResult<string> Delete(int id)
        {
            try
            {
                if (_recipeService.Remove(id) != null) { return Ok("success"); }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }
        //Faz uma busca de receitas usando ou Id da receita ou do usuario.
        //Se nenhum valor for especificado será usado o Id do usuario da sessão requisitante
        [Authorize]
        [HttpGet]
        public ActionResult<List<RecipeObject>> SearchUserRecipe()
        {
            try
            {
                var userId = User.FindFirstValue("Id");
                List<Recipe> recipeList = _recipeService.Search(int.Parse(userId));

                if (recipeList != null)
                {
                    return _buidRecipeService.BuildRecipeObject(recipeList);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult<List<RecipeObject>> Search([FromQuery] int userId)
        {
            try
            {
                List<Recipe> recipeList = _recipeService.Search(userId);
                if (recipeList != null)
                {
                    return _buidRecipeService.BuildRecipeObject(recipeList);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public ActionResult<List<RecipeObject>> SearchByRecipe([FromQuery] int recipeId)
        {
            try
            {
                List<Recipe> recipe = new List<Recipe>();
                recipe.Add(_recipeService.SearchByRecipe(recipeId));
                if (recipe[0] != null)
                {
                    return _buidRecipeService.BuildRecipeObject(recipe);
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

