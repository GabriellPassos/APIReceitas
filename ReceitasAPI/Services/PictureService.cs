using Microsoft.AspNetCore.Hosting;
using ReitasAPI.Data;
using ReitasAPI.Modelos;
using ReitasAPI.Models;
using System.IO;

namespace ReitasAPI.Services
{
    public class PictureService
    {
        RecipeContext _context;
        public IWebHostEnvironment _webHostEnvironment { get; set; }
        public PictureService(RecipeContext receitasContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = receitasContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public Picture Create(Recipe recipe, string imageBase64)
        {
           Picture newImage = new Picture(recipe, imageBase64);
            _context.Pictures.Add(newImage);
            _context.SaveChanges();
            return newImage;
        }
        public List<Picture> Search(int recipeId)
        {
            return _context.Pictures.Where(x => x.RecipeId == recipeId).ToList();
            /*string rootPath = _webHostEnvironment.ContentRootPath;

            var imagesPath = _context.Pictures.Where(x => x.RecipeId == recipeId).Select(x => new {
                Id = x.Id,
                base64 = Path.Combine(rootPath, x.Path)
            }).ToList();
            List<string> imageArray =  new List<string>();
            foreach (var path in imagesPath)
            {
                imageArray.Add($"id {path.Id}: {Convert.ToBase64String(File.ReadAllBytes(path.base64))}");
            }
             return imageArray;*/
        }
        public Picture Remove(int id)
        {
            Picture image = _context.Pictures.FirstOrDefault(x => x.Id == id);
            _context.Pictures.Remove(image);
            _context.SaveChanges();
            return image;
        }
        public List<string> Load(List<string> arquivos)
        {
            //Os arquivos recebem seus diretórios e são salvos no disco local (caminhoLocal) 
            //e retorna uma lista com os diretórios salvos.
            string caminhoRoot = _webHostEnvironment.ContentRootPath;
            string caminhoLocal = "Data/UploadedFiles/Images/";
            string caminhoDiretorio = Path.Combine(caminhoRoot, caminhoLocal);
            List<string> caminhosLocal = new List<string>();
            foreach (var arquivo in arquivos)
            {

                var nomeAleatorio = Guid.NewGuid().ToString() + ".jpg";
                string caminhoArquivo = Path.Combine(caminhoDiretorio, nomeAleatorio);
                File.WriteAllBytes(caminhoArquivo, Convert.FromBase64String(arquivo));
                caminhosLocal.Add(Path.Combine(caminhoLocal, nomeAleatorio));
               /* using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    arquivo.CopyTo(stream);
                    caminhosLocal.Add(Path.Combine(caminhoLocal, nomeAleatorio));
                }*/
            }
            return (caminhosLocal);
        }
    }
}
