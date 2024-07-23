using Microsoft.AspNetCore.Mvc;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;

namespace NewsArticle.Controllers
{
    public class SectorEconomicoController : Controller
    {
        private readonly IRepositorioSectorEconomico repositorioSectorEconomico;
        private readonly IServicioUsuarios servicioUsuarios;

        public SectorEconomicoController(IRepositorioSectorEconomico repositorioSectorEconomico, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioSectorEconomico = repositorioSectorEconomico;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var sectoresEconomicos = await repositorioSectorEconomico.Obtener(usuarioId);

            return View(sectoresEconomicos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SectorEconomico sectorEconomico)
        {
            if (!ModelState.IsValid)
            {
                return View(sectorEconomico);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            sectorEconomico.idUsuario = usuarioId;
            await repositorioSectorEconomico.Crear(sectorEconomico);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var sectorEconomico = await repositorioSectorEconomico.ObtenerPorId(id, usuarioId);

            if (sectorEconomico == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(sectorEconomico);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SectorEconomico sectorEconomico)
        {
            if (!ModelState.IsValid)
            {
                return View(sectorEconomico);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            sectorEconomico.idUsuario = usuarioId;

            await repositorioSectorEconomico.Actualizar(sectorEconomico);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var sectorEconomico = await repositorioSectorEconomico.ObtenerPorId(id, usuarioId);

            if (sectorEconomico == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(sectorEconomico);
        }
        [HttpPost]
        public async Task<IActionResult> Borrar(SectorEconomico sectorEconomico)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var sectorEconomicoExistente = await repositorioSectorEconomico.ObtenerPorId(sectorEconomico.Id, usuarioId);

            if (sectorEconomicoExistente == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioSectorEconomico.Borrar(sectorEconomico.Id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar el sector económico porque está referenciado en una nota periodística.");
                return View("Borrar", sectorEconomicoExistente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar el sector económico.");
                return View("Borrar", sectorEconomicoExistente);
            }

            return RedirectToAction("Index");
        }


    }
}
