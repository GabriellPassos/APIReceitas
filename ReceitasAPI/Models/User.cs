using ReitasAPI.Modelos;
using System.ComponentModel.DataAnnotations;

namespace ReitasAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public List<Recipe> Recipes { get; set; }

        public User(string name, string email, byte[] passwordSalt, byte[] passwordHash)
        {
            Name = name;
            Email = email;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
        }
    }

}
