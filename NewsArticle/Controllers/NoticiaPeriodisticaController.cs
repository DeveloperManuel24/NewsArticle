using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsArticle.Models;
using NewsArticle.Servicios;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace NewsArticle.Controllers
{
    [Authorize]
    public class NoticiaPeriodisticaController : Controller
    {
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioNotaPeriodistica repositorioNotaPeriodistica;
        private readonly IRepositorioPalabrasClave repositorioPalabrasClave;
        private readonly IRepositorioPais repositorioPais;
        private readonly IRepositorioSectorEconomico repositorioSectorEconomico;
        private readonly IRepositorioEconomiaIlicita repositorioEconomiaIlicita;
        private readonly IRepositorioIncautacion repositorioIncautacion;
        private readonly IRepositorioDroga repositorioDroga;
        private readonly IRepositorioMatasArbustos repositorioMatasArbustos;
        private readonly IRepositorioVehiculo repositorioVehiculo;
        private readonly IRepositorioTipoViolencia repositorioTipoViolencia;
        private readonly IRepositorioTransporte repositorioTransporte;
        private readonly GeoLocationService geoLocationService;

        public NoticiaPeriodisticaController(
            IServicioUsuarios servicioUsuarios,
            IRepositorioNotaPeriodistica repositorioNotaPeriodistica,
            IRepositorioPalabrasClave repositorioPalabrasClave,
            IRepositorioPais repositorioPais,
            IRepositorioSectorEconomico repositorioSectorEconomico,
            IRepositorioEconomiaIlicita repositorioEconomiaIlicita,
            IRepositorioIncautacion repositorioIncautacion,
            IRepositorioDroga repositorioDroga,
            IRepositorioMatasArbustos repositorioMatasArbustos,
            IRepositorioVehiculo repositorioVehiculo,
            IRepositorioTipoViolencia repositorioTipoViolencia,
            IRepositorioTransporte repositorioTransporte,
            GeoLocationService geoLocationService
        )
        {
            this.servicioUsuarios = servicioUsuarios;
            this.repositorioNotaPeriodistica = repositorioNotaPeriodistica;
            this.repositorioPalabrasClave = repositorioPalabrasClave;
            this.repositorioPais = repositorioPais;
            this.repositorioSectorEconomico = repositorioSectorEconomico;
            this.repositorioEconomiaIlicita = repositorioEconomiaIlicita;
            this.repositorioIncautacion = repositorioIncautacion;
            this.repositorioDroga = repositorioDroga;
            this.repositorioMatasArbustos = repositorioMatasArbustos;
            this.repositorioVehiculo = repositorioVehiculo;
            this.repositorioTipoViolencia = repositorioTipoViolencia;
            this.repositorioTransporte = repositorioTransporte;
            this.geoLocationService = geoLocationService;
        }

        public async Task<IActionResult> Index(string titulo, int? palabraClaveId, DateTime? fechaHecho, int? AnoHecho, int? paisId)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var notasPeriodisticas = await repositorioNotaPeriodistica.ObtenerConFiltros(usuarioId, titulo, palabraClaveId, fechaHecho, AnoHecho, paisId);

            // Cargar las listas de filtros
            await CargarListasFiltros();

            // Pasar los valores de filtro a la vista para mantenerlos en los campos
            ViewBag.TituloFiltro = titulo;
            ViewBag.PalabraClaveIdFiltro = palabraClaveId;
            ViewBag.FechaHechoFiltro = fechaHecho?.ToString("yyyy-MM-dd");
            ViewBag.AnoHechoFiltro = AnoHecho;
            ViewBag.PaisIdFiltro = paisId;

            return View(notasPeriodisticas);
        }

        private async Task CargarListasFiltros()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            ViewBag.PalabrasClaveList = new SelectList(await repositorioPalabrasClave.Obtener(usuarioId), "Id", "NombrePalabraClave");
            ViewBag.PaisesList = new SelectList(await repositorioPais.Obtener(), "Id", "NombrePais");
        }

        // GET: NoticiaPeriodisticaController/Detalles
        public async Task<IActionResult> ListadoCompleto(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var notaPeriodistica = await repositorioNotaPeriodistica.ObtenerPorId(id, usuarioId);
            if (notaPeriodistica == null)
            {
                return NotFound();
            }

            return View(notaPeriodistica);
        }

        // GET: NoticiaPeriodisticaController/Create
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            await CargarListas();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(NotaPeriodistica notaPeriodistica)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                await CargarListas();
                return View(notaPeriodistica);
            }

            notaPeriodistica.IdUsuario = servicioUsuarios.ObtenerUsuarioId();
            await repositorioNotaPeriodistica.Crear(notaPeriodistica);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Mapa(int idPalabraClave, string returnAction)
        {
            var notas = await repositorioNotaPeriodistica.MostrarEnMapa(idPalabraClave);
            ViewBag.ReturnAction = returnAction;
            ViewBag.Notas = notas;
            return View("Mapa");
        }

        public async Task<IActionResult> MapaEditar(int id, int idPalabraClave)
        {
            var notas = await repositorioNotaPeriodistica.MostrarEnMapa(idPalabraClave);
            var model = new NotaPeriodistica { Id = id };
            ViewBag.Notas = notas;
            return View("MapaEditar", model);
        }

        [HttpPost]
        public IActionResult SetLocation(double latitud, double longitud)
        {
            geoLocationService.SetLocation(latitud, longitud);
            return RedirectToAction("Crear");
        }

        public async Task<IActionResult> MapaGeneral()
        {
            var notasPeriodisticas = await repositorioNotaPeriodistica.ObtenerTodasLasNotas();
            return View(notasPeriodisticas);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var notaPeriodistica = await repositorioNotaPeriodistica.ObtenerPorId(id, usuarioId);
            if (notaPeriodistica == null)
            {
                return NotFound();
            }

            await CargarListas(notaPeriodistica);
            return View(notaPeriodistica);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(NotaPeriodistica notaPeriodistica)
        {
            if (!ModelState.IsValid)
            {
                await CargarListas(notaPeriodistica);
                return View(notaPeriodistica);
            }

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            notaPeriodistica.IdUsuario = usuarioId;

            await repositorioNotaPeriodistica.Actualizar(notaPeriodistica);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var notaPeriodistica = await repositorioNotaPeriodistica.ObtenerPorId(id, usuarioId);
            if (notaPeriodistica == null)
            {
                return NotFound();
            }

            return View(notaPeriodistica);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarConfirmado(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var notaPeriodisticaExiste = await repositorioNotaPeriodistica.ObtenerPorId(id, usuarioId);

            if (notaPeriodisticaExiste == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioNotaPeriodistica.Borrar(id);
            return RedirectToAction("Index");
        }

        //Funciones de funcionalidad

        [HttpGet]
        public async Task<JsonResult> ObtenerProvincias(int paisId)
        {
            var provincias = await repositorioPais.ObtenerProvincias(paisId);
            return Json(provincias);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerComarcas(int provinciaId)
        {
            var comarcas = await repositorioPais.ObtenerComarcas(provinciaId);
            return Json(comarcas);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerDistritos(int provinciaId)
        {
            var distritos = await repositorioPais.ObtenerDistritos(provinciaId);
            return Json(distritos);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerCorregimientos(int distritoId)
        {
            var corregimientos = await repositorioPais.ObtenerCorregimientos(distritoId);
            return Json(corregimientos);
        }

        private async Task CargarListas()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            ViewBag.PalabrasClaveList = new SelectList(await repositorioPalabrasClave.Obtener(usuarioId), "Id", "NombrePalabraClave");
            ViewBag.PaisesList = new SelectList(await repositorioPais.Obtener(), "Id", "NombrePais");
            ViewBag.SectorEconomicoList = new SelectList(await repositorioSectorEconomico.Obtener(usuarioId), "Id", "NombreSector");
            ViewBag.EconomiasIlicitas = new SelectList(await repositorioEconomiaIlicita.Obtener(usuarioId), "Id", "NombreEconomiaIlicita");
            ViewBag.IncautacionesList = new SelectList(await repositorioIncautacion.Obtener(usuarioId), "Id", "TipoIncautacion");
            ViewBag.DrogasList = new SelectList(await repositorioDroga.Obtener(usuarioId), "Id", "TipoDroga");
            ViewBag.MatasArbustosList = new SelectList(await repositorioMatasArbustos.Obtener(usuarioId), "Id", "TipoMatasArbustos");
            ViewBag.VehiculosList = new SelectList(await repositorioVehiculo.Obtener(usuarioId), "Id", "TipoVehiculo");
            ViewBag.ViolenciaList = new SelectList(await repositorioTipoViolencia.Obtener(usuarioId), "Id", "TipoViolenciaNombre");
            ViewBag.TransporteList = new SelectList(await repositorioTransporte.Obtener(usuarioId), "Id", "TipoTransporte");
        }

        private async Task CargarListas(NotaPeriodistica notaPeriodistica)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var PalabrasClave = await repositorioPalabrasClave.Obtener(usuarioId);
            var Paises = await repositorioPais.Obtener();
            var SectorEconomico = await repositorioSectorEconomico.Obtener(usuarioId);
            var EconomiaIlicita = await repositorioEconomiaIlicita.Obtener(usuarioId);
            var Incautaciones = await repositorioIncautacion.Obtener(usuarioId);
            var Drogas = await repositorioDroga.Obtener(usuarioId);
            var MatasArbustos = await repositorioMatasArbustos.Obtener(usuarioId);
            var Vehiculos = await repositorioVehiculo.Obtener(usuarioId);
            var Violencia = await repositorioTipoViolencia.Obtener(usuarioId);
            var Transporte = await repositorioTransporte.Obtener(usuarioId);

            var idPais = notaPeriodistica.IdPais;
            var idDepartamento = notaPeriodistica.IdDepartamento ?? 0;
            var idMunicipio = notaPeriodistica.IdMunicipio ?? 0;
            var idComarca = notaPeriodistica.IdComarca ?? 0;

            var Provincias = await repositorioPais.ObtenerProvincias(idPais);
            var Comarcas = await repositorioPais.ObtenerComarcas(idDepartamento);
            var Distritos = await repositorioPais.ObtenerDistritos(idDepartamento);
            var Corregimientos = await repositorioPais.ObtenerCorregimientos(idMunicipio);

            ViewBag.PalabrasClaveList = new SelectList(PalabrasClave, "Id", "NombrePalabraClave", notaPeriodistica.IdPalabraClave);
            ViewBag.PaisesList = new SelectList(Paises, "Id", "NombrePais", notaPeriodistica.IdPais);
            ViewBag.SectorEconomicoList = new SelectList(SectorEconomico, "Id", "NombreSector", notaPeriodistica.IdSectorEconomico);
            ViewBag.EconomiasIlicitas = new SelectList(EconomiaIlicita, "Id", "NombreEconomiaIlicita", notaPeriodistica.IdEconomiaIlicita);
            ViewBag.IncautacionesList = new SelectList(Incautaciones, "Id", "TipoIncautacion", notaPeriodistica.IdIncautacion);
            ViewBag.DrogasList = new SelectList(Drogas, "Id", "TipoDroga", notaPeriodistica.IdDroga);
            ViewBag.MatasArbustosList = new SelectList(MatasArbustos, "Id", "TipoMatasArbustos", notaPeriodistica.IdMatasArbustos);
            ViewBag.VehiculosList = new SelectList(Vehiculos, "Id", "TipoVehiculo", notaPeriodistica.IdVehiculo);
            ViewBag.ViolenciaList = new SelectList(Violencia, "Id", "TipoViolenciaNombre", notaPeriodistica.huboViolencia);
            ViewBag.TransporteList = new SelectList(Transporte, "Id", "TipoTransporte", notaPeriodistica.tipoTransporte);
            ViewBag.ProvinciasList = new SelectList(Provincias, "Id", "NombreProvincia", notaPeriodistica.IdDepartamento);
            ViewBag.ComarcasList = new SelectList(Comarcas, "Id", "NombreComarca", notaPeriodistica.IdComarca);
            ViewBag.DistritosList = new SelectList(Distritos, "Id", "NombreDistrito", notaPeriodistica.IdMunicipio);
            ViewBag.CorregimientosList = new SelectList(Corregimientos, "Id", "NombreCorregimiento", notaPeriodistica.IdAldea);
        }

        //REPORTE DE EXCEL

        [HttpGet]
        public async Task<FileResult> ExportarExcel(string titulo, int? palabraClaveId, DateTime? fechaHecho, int? AnoHecho, int? paisId)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var notasPeriodisticas = await repositorioNotaPeriodistica.ObtenerParaReporte(usuarioId, titulo, palabraClaveId, fechaHecho, AnoHecho, paisId);

            var nombreArchivo = $"Reporte_Noticias_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            return GenerarExcel(nombreArchivo, notasPeriodisticas);
        }

        public FileResult GenerarExcel(string nombreArchivo, IEnumerable<NotaPeriodistica> notasPeriodisticas)
        {
            DataTable dataTable = new DataTable("NotaPeriodistica");
            dataTable.Columns.AddRange(new DataColumn[] {
        new DataColumn("ID"),
        new DataColumn("Palabra Clave"),
        new DataColumn("Título"),
        new DataColumn("URL Fuente"),
        new DataColumn("Nombre Fuente"),
        new DataColumn("Fecha de Publicación"),
        new DataColumn("Año de Publicación"),
        new DataColumn("Fecha del Hecho"),
        new DataColumn("Año del Hecho"),
        new DataColumn("Nota Grande"),
        new DataColumn("País"),
        new DataColumn("Departamento"),
        new DataColumn("Comarca"),
        new DataColumn("Municipio"),
        new DataColumn("Aldea"),
        new DataColumn("Ruta Carretera"),
        new DataColumn("Latitud"),
        new DataColumn("Longitud"),
        new DataColumn("Nombre Cartel"),
        new DataColumn("Nombre Operación Policial"),
        new DataColumn("Hubo Violencia"),
        new DataColumn("Tipo Transporte"),
        new DataColumn("Cantidad en Kilos"),
        new DataColumn("Monto en Dólares Transporte"),
        new DataColumn("Número de Pistas Destruidas"),
        new DataColumn("Monto en Dólares Intento Soborno"),
        new DataColumn("Breve Nota"),
        new DataColumn("Institución Responsable"),
        new DataColumn("Instituciones de Apoyo"),
        new DataColumn("Sector Económico"),
        new DataColumn("Economía Ilícita"),
        new DataColumn("Nombre Fauna"),
        new DataColumn("Cantidad Fauna"),
        new DataColumn("Nombre Flora"),
        new DataColumn("Cantidad Flora"),
        new DataColumn("Cantidad Personas Trata"),
        new DataColumn("Nacionalidad Trata"),
        new DataColumn("Número de Detenidos"),
        new DataColumn("Nacionalidad Detenidos"),
        new DataColumn("Nombre Área Protegida"),
        new DataColumn("Número de Hectáreas Área Protegida"),
        new DataColumn("Incautación"),
        new DataColumn("Droga"),
        new DataColumn("Matas Arbustos"),
        new DataColumn("Número Matas Arbustos"),
        new DataColumn("Número Hectáreas"),
        new DataColumn("Monto en Dólares Matas Arbustos"),
        new DataColumn("Vehículo"),
        new DataColumn("Monto en Dólares Dinero"),
        new DataColumn("Monto en Dólares Joyas"),
        new DataColumn("Número de Inmuebles"),
        new DataColumn("Número de Fincas"),
        new DataColumn("Número de Hectáreas Terrenos"),
        new DataColumn("Número de Armas"),
        new DataColumn("Número de Municiones"),
        new DataColumn("Número de Vehículos"),
        new DataColumn("ID Usuario")
    });

            foreach (var nota in notasPeriodisticas)
            {
                dataTable.Rows.Add(
                    nota.Id,
                    nota.MostrarNombrePalabraClave,
                    nota.Titulo,
                    nota.UrlFuente,
                    nota.NombreFuente,
                    nota.FechaPublicacion.ToString("dd/MM/yyyy"),
                    nota.AnoPublicacion,
                    nota.FechaHecho.ToString("dd/MM/yyyy"),
                    nota.AnoHecho,
                    nota.NotaGrande,
                    nota.MostrarNombrePaís,
                    nota.MostrarNombreDepartamento,
                    nota.MostrarNombreComarca,
                    nota.MostrarNombreMunicipio,
                    nota.MostrarNombreAldea,
                    nota.rutaCarretera,
                    nota.Latitud,
                    nota.Longitud,
                    nota.NombreCartel,
                    nota.NombreOperacionPolicial,
                    nota.MostrarNombreHuboViolencia,
                    nota.MostrarNombreTipoTransporte,
                    nota.CantidadEnKilos,
                    nota.MontoEnDolaresTransporte,
                    nota.NumeroPistasDestruidas,
                    nota.MontoEnDolaresIntentoSoborno,
                    nota.BreveNota,
                    nota.InstitucionResponsable,
                    nota.InstitucionesDeApoyo,
                    nota.MostrarNombreSectorEconómico,
                    nota.MostrarNombreEconomíaIlícita,
                    nota.NombreFauna,
                    nota.CantidadFauna,
                    nota.NombreFlora,
                    nota.CantidadFlora,
                    nota.CantidadPersonasTrata,
                    nota.MostrarNombreNacionalidadTrata,
                    nota.NumeroDetenidos,
                    nota.NacionalidadDetenidos,
                    nota.NombreAreaProtegida,
                    nota.NumeroHectareasAreaProtegida,
                    nota.MostrarNombreIncautación,
                    nota.MostrarNombreDroga,
                    nota.MostrarNombreMatasArbustos,
                    nota.NumeroMatasArbustos,
                    nota.NumeroHectareas,
                    nota.MontoEnDolaresMatasArbustos,
                    nota.MostrarNombreVehículo,
                    nota.MontoEnDolaresDinero,
                    nota.MontoEnDolaresJoyas,
                    nota.NumeroInmuebles,
                    nota.NumeroFincas,
                    nota.NumeroHectareasTerrenos,
                    nota.numeroArmas,
                    nota.numeroMuniciones,
                    nota.numeroVehiculos,
                    nota.IdUsuario
                );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = nombreArchivo
                    };
                }
            }
        }
    }
}
