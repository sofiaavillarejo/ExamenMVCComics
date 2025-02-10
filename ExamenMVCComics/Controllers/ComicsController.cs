using ExamenMVCComics.Models;
using ExamenMVCComics.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenMVCComics.Controllers
{
    public class ComicsController : Controller
    {
        RepositoryComic repo;

        public ComicsController()
        {
            this.repo = new RepositoryComic();
        }   
        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(int IdComic, string Nombre, string Imagen, string Descripcion)
        {
            await this.repo.CreateComicAsync(IdComic, Nombre, Imagen, Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult BuscadorComics()
        {
            ViewData["NOMBRE"] = this.repo.GetIdComic();
            return View();
        }

        [HttpPost]
        public IActionResult BuscadorComics(string Nombre)
        {
            ViewData["NOMBRE"] = this.repo.GetIdComic();
            Comic comic = this.repo.DetalleComic(Nombre);
            return View(comic);
        }

    }
}
