using Microsoft.AspNetCore.Mvc;
using ReitasAPI.Modelos;
using ReitasAPI.Models;
using ReitasAPI.Services;


namespace ReitasAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FilterTagController : ControllerBase
    {
        private readonly FilterTagService _filterTagService;

        public FilterTagController(FilterTagService filterTagService)
        {
            _filterTagService = filterTagService;
        }

        [HttpPost]
        public ActionResult<string> Create(string name)
        {
            try
            {
                if (_filterTagService.Create(name) != null) { return Ok("success"); }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }
        [HttpDelete]
        public ActionResult<string> Delete(string name)
        {
            try
            {
                if (_filterTagService.Remove(name) != null) { return Ok("success"); }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        public ActionResult<List<FilterTag>> GetAll()
        {
            try
            {
                List<FilterTag> filterTags = _filterTagService.SearchAll();
                if (filterTags != null) { return Ok(filterTags); }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        public ActionResult<FilterTag> GetById(int id)
        {
            try
            {
                FilterTag filterTag = _filterTagService.SearchById(id);
                if (filterTag != null) { return Ok(filterTag); }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }

    }

}
