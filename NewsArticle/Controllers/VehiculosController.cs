using Microsoft.AspNetCore.Mvc;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Threading.Tasks;

namespace NewsArticle.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly IRepositorioVehiculo repositorioVehiculo;
        private readonly IServicioUsuarios servicioUsuarios;

        public VehiculosController(IRepositorioVehiculo repositorioVehiculo, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioVehiculo = repositorioVehiculo;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var vehiculos = await repositorioVehiculo.Obtener(usuarioId);

            return View(vehiculos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                return View(vehiculo);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            vehiculo.idUsuario = usuarioId;
            await repositorioVehiculo.Crear(vehiculo);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var vehiculo = await repositorioVehiculo.ObtenerPorId(id, usuarioId);

            if (vehiculo == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(vehiculo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                return View(vehiculo);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            vehiculo.idUsuario = usuarioId;

            await repositorioVehiculo.Actualizar(vehiculo);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var vehiculo = await repositorioVehiculo.ObtenerPorId(id, usuarioId);

            if (vehiculo == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(vehiculo);
        }
        [HttpPost]
        public async Task<IActionResult> Borrar(Vehiculo vehiculo)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var vehiculoExistente = await repositorioVehiculo.ObtenerPorId(vehiculo.Id, usuarioId);

            if (vehiculoExistente == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            try
            {
                await repositorioVehiculo.Borrar(vehiculo.Id);
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "No se puede borrar el vehículo porque está referenciado en una nota periodística.");
                return View("Borrar", vehiculoExistente);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al intentar borrar el vehículo.");
                return View("Borrar", vehiculoExistente);
            }

            return RedirectToAction("Index");
        }


    }
}
