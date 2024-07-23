using Microsoft.AspNetCore.Mvc;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;

namespace NewsArticle.Controllers
{
    public class DrogaController : Controller
    {
        private readonly IRepositorioDroga repositorioDroga;
        private readonly IServicioUsuarios servicioUsuarios;

        public DrogaController(IRepositorioDroga repositorioDroga, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioDroga = repositorioDroga;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var drogas = await repositorioDroga.Obtener(usuarioId);

            return View(drogas);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Droga droga)
        {
            if (!ModelState.IsValid)
            {
                return View(droga);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            droga.idUsuario = usuarioId;
            await repositorioDroga.Crear(droga);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var droga = await repositorioDroga.ObtenerPorId(id, usuarioId);

            if (droga == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(droga);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Droga droga)
        {
            if (!ModelState.IsValid)
            {
                return View(droga);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            droga.idUsuario = usuarioId;

            await repositorioDroga.Actualizar(droga);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var droga = await repositorioDroga.ObtenerPorId(id, usuarioId);

            if (droga == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(droga);
        }
        [HttpPost]
        public async Task<IActionResult> Borrar(Droga droga)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var drogaExistente = await repositorioDroga.ObtenerPorId(droga.Id, usuarioId);

            if (drogaExistente == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioDroga.Borrar(droga.Id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar la droga porque está referenciada en una nota periodística.");
                return View("Borrar", drogaExistente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar la droga.");
                return View("Borrar", drogaExistente);
            }

            return RedirectToAction("Index");
        }



    }
}
