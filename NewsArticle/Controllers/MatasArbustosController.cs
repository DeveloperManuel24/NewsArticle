using Microsoft.AspNetCore.Mvc;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;

namespace NewsArticle.Controllers
{
    public class MatasArbustosController : Controller
    {
        private readonly IRepositorioMatasArbustos repositorioMatasArbustos;
        private readonly IServicioUsuarios servicioUsuarios;

        public MatasArbustosController(IRepositorioMatasArbustos repositorioMatasArbustos, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioMatasArbustos = repositorioMatasArbustos;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var matasArbustos = await repositorioMatasArbustos.Obtener(usuarioId);

            return View(matasArbustos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(MatasArbustos matasArbustos)
        {
            if (!ModelState.IsValid)
            {
                return View(matasArbustos);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            matasArbustos.idUsuario = usuarioId;
            await repositorioMatasArbustos.Crear(matasArbustos);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var matasArbustos = await repositorioMatasArbustos.ObtenerPorId(id, usuarioId);

            if (matasArbustos == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(matasArbustos);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(MatasArbustos matasArbustos)
        {
            if (!ModelState.IsValid)
            {
                return View(matasArbustos);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            matasArbustos.idUsuario = usuarioId;

            await repositorioMatasArbustos.Actualizar(matasArbustos);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var matasArbustos = await repositorioMatasArbustos.ObtenerPorId(id, usuarioId);

            if (matasArbustos == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(matasArbustos);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(MatasArbustos matasArbustos)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var matasArbustosExistente = await repositorioMatasArbustos.ObtenerPorId(matasArbustos.Id, usuarioId);

            if (matasArbustosExistente == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioMatasArbustos.Borrar(matasArbustos.Id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar las matas o arbustos porque están referenciadas en una nota periodística.");
                return View("Borrar", matasArbustosExistente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar las matas o arbustos.");
                return View("Borrar", matasArbustosExistente);
            }

            return RedirectToAction("Index");
        }


    }
}
