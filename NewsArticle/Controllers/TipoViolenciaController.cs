using Microsoft.AspNetCore.Mvc;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;

namespace NewsArticle.Controllers
{
    public class TipoViolenciaController : Controller
    {
        private readonly IRepositorioTipoViolencia repositorioTipoViolencia;
        private readonly IServicioUsuarios servicioUsuarios;

        public TipoViolenciaController(IRepositorioTipoViolencia repositorioTipoViolencia, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTipoViolencia = repositorioTipoViolencia;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposViolencia = await repositorioTipoViolencia.Obtener(usuarioId);

            return View(tiposViolencia);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoViolencia tipoViolencia)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoViolencia);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            tipoViolencia.idUsuario = usuarioId;
            await repositorioTipoViolencia.Crear(tipoViolencia);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoViolencia = await repositorioTipoViolencia.ObtenerPorId(id, usuarioId);

            if (tipoViolencia == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoViolencia);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoViolencia tipoViolencia)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoViolencia);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            tipoViolencia.idUsuario = usuarioId;

            await repositorioTipoViolencia.Actualizar(tipoViolencia);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoViolencia = await repositorioTipoViolencia.ObtenerPorId(id, usuarioId);

            if (tipoViolencia == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tipoViolencia);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(TipoViolencia tipoViolencia)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoViolenciaExistente = await repositorioTipoViolencia.ObtenerPorId(tipoViolencia.Id, usuarioId);

            if (tipoViolenciaExistente == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioTipoViolencia.Borrar(tipoViolencia.Id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar el tipo de violencia porque está referenciado en una nota periodística.");
                return View("Borrar", tipoViolenciaExistente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar el tipo de violencia.");
                return View("Borrar", tipoViolenciaExistente);
            }

            return RedirectToAction("Index");
        }


    }
}
