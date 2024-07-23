using Microsoft.AspNetCore.Mvc;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;

namespace NewsArticle.Controllers
{
    public class TransporteController : Controller
    {
        private readonly IRepositorioTransporte repositorioTransporte;
        private readonly IServicioUsuarios servicioUsuarios;

        public TransporteController(IRepositorioTransporte repositorioTransporte, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTransporte = repositorioTransporte;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var transportes = await repositorioTransporte.Obtener(usuarioId);

            return View(transportes);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Transporte transporte)
        {
            if (!ModelState.IsValid)
            {
                return View(transporte);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            transporte.idUsuario = usuarioId;
            await repositorioTransporte.Crear(transporte);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var transporte = await repositorioTransporte.ObtenerPorId(id, usuarioId);

            if (transporte == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(transporte);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Transporte transporte)
        {
            if (!ModelState.IsValid)
            {
                return View(transporte);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            transporte.idUsuario = usuarioId;

            await repositorioTransporte.Actualizar(transporte);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var transporte = await repositorioTransporte.ObtenerPorId(id, usuarioId);

            if (transporte == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(transporte);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(Transporte transporte)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var transporteExistente = await repositorioTransporte.ObtenerPorId(transporte.Id, usuarioId);

            if (transporteExistente == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioTransporte.Borrar(transporte.Id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar el transporte porque está referenciado en una nota periodística.");
                return View("Borrar", transporteExistente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar el transporte.");
                return View("Borrar", transporteExistente);
            }

            return RedirectToAction("Index");
        }


    }
}
