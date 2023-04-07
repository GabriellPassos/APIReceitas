using Microsoft.EntityFrameworkCore;
using ReitasAPI.Data;
using ReitasAPI.Modelos;
using ReitasAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace ReitasAPI.Services
{
    public class UserService
    {
        public readonly RecipeContext _context;
        public UserService(RecipeContext receitasContext)
        {
            _context = receitasContext;
        }

        public User Create(string name, string email, string password)
        {
            if(SearchByEmail(email) == null)
            {
                CreatePasswordHash(password, out byte[] passwordSalt, out byte[] passwordHash);
                User newUser = new User(name, email, passwordSalt, passwordHash);
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return newUser;
            }
            return null;

        }
        public User Search(int? id)
        {
            return _context.Users.FirstOrDefault<User>(x => x.Id == id);
        }
        public List<User> SearchByRecipe(int? recipeId)
        {

            return _context.Recipes.Where(x => x.Id == recipeId).Select(x => x.User).ToList();
        }
        public User SearchByName(string? name)
        {
            return _context.Users.FirstOrDefault<User>(x => x.Name.ToLower() == name.ToLower());
        }
        public User SearchByEmail(string? email)
        {
            return _context.Users.FirstOrDefault<User>(x => x.Email == email);
        }

        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public bool ValidPassword(User user, string passwordReceived)
        {
            using(var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var passwordRecebidaManipulada = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordReceived));
                return passwordRecebidaManipulada.SequenceEqual(user.PasswordHash);
            }
        }
    }

}
