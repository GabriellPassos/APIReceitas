using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using ReitasAPI.Models;
using ReitasAPI.Services;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace ReitasAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TokenService _tokenService;
        public IWebHostEnvironment _webHostEnvironment { get; set; }

        public AuthenticationController(UserService userService, TokenService tokenService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _tokenService = tokenService;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize]
        [HttpGet]
        public ActionResult<string> UserCheck()
        {
            return Ok("User:" + User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        [Authorize]
        [HttpGet]
        public ActionResult<string> Logout()
        {
            HttpContext.Response.Cookies.Append("token", "logout", new CookieOptions
            {
                //Expires = DateTime.Now.AddDays(2),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });
            return Ok("Logout Success");

        }
        [HttpGet]
        public ActionResult<string> Register()
        {
            //O email nao foi incluido no "Required" em sua classe, pois o teste do email precisa ser mais acertivo
            if (Request.Headers.ContainsKey("Authorization"))
            {
                /*Recebido do client um string em base64, convertido para string novamente
                e deserializado contra a classe "userRequest"*/
                string authorizationHeader = Request.Headers["Authorization"];
                authorizationHeader = authorizationHeader.Replace("Basic ", "");
                var authorizationBytes = Convert.FromBase64String(authorizationHeader);
                var userLoginData = Encoding.UTF8.GetString(authorizationBytes);

                //Validacao dos campos contra um jsonSchema
                JObject userJObject = JObject.Parse(userLoginData); 
                JSchema registerSchema = JSchema.Parse(System.IO.File.ReadAllText("./Models/ValidationSchemes/registerSchema.json"));

                IList<string> validationError = new List<string>();
                if (userJObject.IsValid(registerSchema, out validationError))
                {
                    UserRequest userObject = JsonConvert.DeserializeObject<UserRequest>(userLoginData);
                    var newUser = _userService.Create(userObject.Name, userObject.Email, userObject.Password);
                    if (newUser != null)
                    {
                        var newToken = _tokenService.Generate(newUser);
                        HttpContext.Response.Cookies.Append("token", newToken,
                            new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(2),
                                HttpOnly = true,
                                Secure = true,
                                IsEssential = true,
                                SameSite = SameSiteMode.None
                            });

                        return Ok("Usuario criado com sucesso");
                    }
                }
                return BadRequest("Error ao criar novo usuario");
            }
            return BadRequest("Error ao criar novo usuario");
        }
        [HttpGet]
        public ActionResult<string> Login()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string authorizationHeader = Request.Headers["Authorization"];
                authorizationHeader = authorizationHeader.Replace("Basic ", "");
                var authorizationBytes = Convert.FromBase64String(authorizationHeader);
                var userLoginData = Encoding.UTF8.GetString(authorizationBytes);
                string[] userNamePassword = userLoginData.Split(":");
                var user = _userService.SearchByEmail(userNamePassword[0]);
                if (user != null)
                {
                    if (_userService.ValidPassword(user, userNamePassword[1]))
                    {
                        var newToken = _tokenService.Generate(user);
                        HttpContext.Response.Cookies.Append("token", newToken,
                            new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(2),
                                HttpOnly = true,
                                Secure = true,
                                IsEssential = true,
                                SameSite = SameSiteMode.None
                            });
                        return Ok("Login efetuado com sucesso");
                    }
                }
            }
            return BadRequest("nome/email ou senha incorretos");
        }
    }
}
