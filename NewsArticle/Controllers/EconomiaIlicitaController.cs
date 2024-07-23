using Microsoft.AspNetCore.Mvc;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;

namespace NewsArticle.Controllers
{
    public class EconomiaIlicitaController : Controller
    {
        private readonly IRepositorioEconomiaIlicita repositorioEconomiaIlicita;
        private readonly IServicioUsuarios servicioUsuarios;

        public EconomiaIlicitaController(IRepositorioEconomiaIlicita repositorioEconomiaIlicita, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioEconomiaIlicita = repositorioEconomiaIlicita;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var economiaIlicita = await repositorioEconomiaIlicita.Obtener(usuarioId);

            return View(economiaIlicita);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(EconomiaIlicita economiaIlicita)
        {
            if (!ModelState.IsValid)
            {
                return View(economiaIlicita);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            economiaIlicita.idUsuario = usuarioId;
            await repositorioEconomiaIlicita.Crear(economiaIlicita);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var economiaIlicita = await repositorioEconomiaIlicita.ObtenerPorId(id, usuarioId);

            if (economiaIlicita == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(economiaIlicita);
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var economia = await repositorioEconomiaIlicita.ObtenerPorId(id, usuarioId);

            if (economia == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(economia);
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(EconomiaIlicita economiaIlicita)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var economiaIlicitaExistente = await repositorioEconomiaIlicita.ObtenerPorId(economiaIlicita.Id, usuarioId);

            if (economiaIlicitaExistente == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioEconomiaIlicita.Borrar(economiaIlicita.Id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar la economía ilícita porque está referenciada en una nota periodística.");
                return View("Borrar", economiaIlicitaExistente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar la economía ilícita.");
                return View("Borrar", economiaIlicitaExistente);
            }

            return RedirectToAction("Index");
        }


    }
}
