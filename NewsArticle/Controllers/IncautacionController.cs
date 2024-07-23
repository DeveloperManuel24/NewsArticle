using Microsoft.AspNetCore.Mvc;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;

namespace NewsArticle.Controllers
{
    public class IncautacionController : Controller
    {
        private readonly IRepositorioIncautacion repositorioIncautacion;
        private readonly IServicioUsuarios servicioUsuarios;

        public IncautacionController(IRepositorioIncautacion repositorioIncautacion, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioIncautacion = repositorioIncautacion;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var incautaciones = await repositorioIncautacion.Obtener(usuarioId);

            return View(incautaciones);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Incautacion incautacion)
        {
            if (!ModelState.IsValid)
            {
                return View(incautacion);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            incautacion.idUsuario = usuarioId;
            await repositorioIncautacion.Crear(incautacion);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var incautacion = await repositorioIncautacion.ObtenerPorId(id, usuarioId);

            if (incautacion == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(incautacion);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Incautacion incautacion)
        {
            if (!ModelState.IsValid)
            {
                return View(incautacion);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            incautacion.idUsuario = usuarioId;

            await repositorioIncautacion.Actualizar(incautacion);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var incautacion = await repositorioIncautacion.ObtenerPorId(id, usuarioId);

            if (incautacion == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(incautacion);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(Incautacion incautacion)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var incautacionExistente = await repositorioIncautacion.ObtenerPorId(incautacion.Id, usuarioId);

            if (incautacionExistente == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioIncautacion.Borrar(incautacion.Id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar la incautación porque está referenciada en una nota periodística.");
                return View("Borrar", incautacionExistente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar la incautación.");
                return View("Borrar", incautacionExistente);
            }

            return RedirectToAction("Index");
        }


    }
}
