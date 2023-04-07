using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using ReitasAPI.Models;
using ReitasAPI.Services;
using System.Drawing;
using System.IO;

namespace ReitasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        readonly PictureService _pictureService;
        public IWebHostEnvironment _webHostEnvironment { get; set; }
        public PictureController(PictureService pictureService, IWebHostEnvironment webHostEnvironment)
        {
            _pictureService = pictureService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpDelete]
        public ActionResult<string> Delete(int id)
        {
            try
            {
                if(_pictureService.Remove(id) != null){ return Ok("success"); }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest();
        }
        [HttpGet]
        public List<Picture> Search(int recipeId)
        {return _pictureService.Search(recipeId);}
    }
}
