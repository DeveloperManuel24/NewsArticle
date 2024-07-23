using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;
using System.IO;

namespace NewsArticle.Controllers
{
    public class PalabraClaveController : Controller
    {
        private readonly IRepositorioPalabrasClave repositorioPalabrasClave;
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PalabraClaveController(IRepositorioPalabrasClave repositorioPalabrasClave, IServicioUsuarios servicioUsuarios, IWebHostEnvironment webHostEnvironment)
        {
            this.repositorioPalabrasClave = repositorioPalabrasClave;
            this.servicioUsuarios = servicioUsuarios;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var palabrasClave = await repositorioPalabrasClave.Obtener(usuarioId);

            var viewModel = new PalabraClaveViewModel
            {
                PalabrasClave = palabrasClave
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PalabraClave palabraClave, IFormFile icono)
        {
            if (!ModelState.IsValid)
            {
                return View(palabraClave);
            }

            if (icono == null || icono.Length == 0 || !icono.FileName.EndsWith(".png"))
            {
                ModelState.AddModelError("Icono", "Debe subir un archivo PNG válido.");
                return View(palabraClave);
            }

            var iconoFileName = Path.GetFileNameWithoutExtension(icono.FileName).Trim();

            if (palabraClave.NombrePalabraClave.Trim() != iconoFileName)
            {
                ModelState.AddModelError("Icono", "El nombre del archivo PNG debe coincidir con la palabra clave.");
                return View(palabraClave);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            palabraClave.idUsuario = usuarioId;

            // Guardar la imagen en wwwroot/images/icons
            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/icons");
            var filePath = Path.Combine(uploadsFolder, palabraClave.NombrePalabraClave + ".png");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await icono.CopyToAsync(fileStream);
            }

            await repositorioPalabrasClave.Crear(palabraClave);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var palabraClave = await repositorioPalabrasClave.ObtenerPorId(id, usuarioId);

            if (palabraClave is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(palabraClave);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(PalabraClave palabraEditar, IFormFile icono)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var palabraClave = await repositorioPalabrasClave.ObtenerPorId(palabraEditar.Id, usuarioId);

            if (palabraClave is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (icono == null || icono.Length == 0)
            {
                ModelState.AddModelError("Icono", "Debe subir un archivo PNG.");
                return View(palabraEditar);
            }

            if (!icono.FileName.EndsWith(".png"))
            {
                ModelState.AddModelError("Icono", "Debe subir un archivo PNG válido.");
                return View(palabraEditar);
            }

            var iconoFileName = Path.GetFileNameWithoutExtension(icono.FileName).Trim();

            if (palabraEditar.NombrePalabraClave.Trim() != iconoFileName)
            {
                ModelState.AddModelError("Icono", "El nombre del archivo PNG debe coincidir con la palabra clave.");
                return View(palabraEditar);
            }

            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/icons");
            var filePath = Path.Combine(uploadsFolder, palabraEditar.NombrePalabraClave + ".png");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await icono.CopyToAsync(fileStream);
            }

            await repositorioPalabrasClave.Actualizar(palabraEditar);
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var palabraClave = await repositorioPalabrasClave.ObtenerPorId(id, usuarioId);

            if (palabraClave is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(palabraClave);
        }
        [HttpPost]
        public async Task<IActionResult> BorrarPalabraClave(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var palabraClave = await repositorioPalabrasClave.ObtenerPorId(id, usuarioId);

            if (palabraClave is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

           
            try
            {
                await repositorioPalabrasClave.Borrar(id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503") // 23503 is the SQL state code for foreign key violation
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar la palabra clave porque está referenciada en una nota periodística.");
                return View("Borrar", palabraClave);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar la palabra clave.");
                return View("Borrar", palabraClave);
            }
            // Eliminar la imagen asociada
            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images/icons");
            var filePath = Path.Combine(uploadsFolder, palabraClave.NombrePalabraClave + ".png");

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return RedirectToAction("Index");
        }
    }
}
