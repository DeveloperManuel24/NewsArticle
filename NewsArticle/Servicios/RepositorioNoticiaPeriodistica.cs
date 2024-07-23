using Dapper;
using NewsArticle.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsArticle.Servicios
{
    public class RepositorioNotaPeriodistica : IRepositorioNotaPeriodistica
    {
        private readonly string connectionString;

        public RepositorioNotaPeriodistica(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task Crear(NotaPeriodistica notaPeriodistica)
        {
            Console.WriteLine("IdSectorEconomico: " + notaPeriodistica.IdSectorEconomico);

            using var connection = new NpgsqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO notaperiodistica (
        url_fuente, nombre_fuente, titulo, fecha_publicacion, ano_publicacion, 
        fecha_hecho, ano_hecho, id_palabra_clave, id_sector_economico, nombre_area_protegida,numero_detenidos, nacionalidad_detenidos, 
        institucion_responsable, instituciones_de_apoyo, id_economia_ilicita, 
        id_usuario,latitud, longitud, id_pais,id_provincia_departamento,id_comarca,id_distrito_municipio,
        id_corregimiento_aldea,nombre_fauna, cantidad_fauna, nombre_flora, 
        cantidad_flora, cantidad_personas_trata, nacionalidad_trata, 
        nombre_cartel, nombre_operacion_policial, idtipoviolencia, cantidad_en_kilos, 
        monto_en_dolares_transporte, numero_pistas_destruidas, numero_hectareas_area_protegida, 
        monto_en_dolares_intento_soborno, id_incautacion, monto_en_dolares_dinero, 
        monto_en_dolares_joyas, numero_inmuebles, numero_fincas, numero_hectareas, 
        numero_matas_arbustos, id_droga, 
        monto_en_dolares_matas_arbustos, breve_nota, nota_grande, id_vehiculo, ruta_carretera, id_transporte, numero_hectareas_terrenos,id_matas_arbustos
    ) VALUES (
        @UrlFuente, @NombreFuente, @Titulo, @FechaPublicacion, @AnoPublicacion, 
        @FechaHecho, @AnoHecho, @IdPalabraClave, @IdSectorEconomico,
        @NombreAreaProtegida,@NumeroDetenidos, @NacionalidadDetenidos, 
        @InstitucionResponsable, @InstitucionesDeApoyo, @IdEconomiaIlicita, 
        @IdUsuario,
        @Latitud, @Longitud, @IdPais,@IdDepartamento,@IdComarca,@IdMunicipio,@IdAldea,@NombreFauna, @CantidadFauna, @NombreFlora, 
        @CantidadFlora, @CantidadPersonasTrata, @NacionalidadTrata,
        @NombreCartel, @NombreOperacionPolicial, @huboViolencia, @CantidadEnKilos, 
        @MontoEnDolaresTransporte, @NumeroPistasDestruidas, @NumeroHectareasAreaProtegida, 
        @MontoEnDolaresIntentoSoborno, @IdIncautacion, @MontoEnDolaresDinero, 
        @MontoEnDolaresJoyas, @NumeroInmuebles, @NumeroFincas, @NumeroHectareas,@NumeroMatasArbustos, @IdDroga, 
        @MontoEnDolaresMatasArbustos, @BreveNota, @NotaGrande, @IdVehiculo, @rutaCarretera, @tipoTransporte, @NumeroHectareasTerrenos,@IdMatasArbustos
    ) RETURNING id_nota;", notaPeriodistica);
            notaPeriodistica.Id = id;
        }

        public async Task<IEnumerable<NotaPeriodistica>> Obtener(int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<NotaPeriodistica>(@"
            SELECT 
                np.id_nota AS Id,
                url_fuente AS UrlFuente,
                titulo AS Titulo, fecha_publicacion AS FechaPublicacion, 
                fecha_hecho AS FechaHecho, 
                p.nombre_pais AS MostrarNombrePaís,pc.palabra_clave AS MostrarNombrePalabraClave
            FROM notaperiodistica as np
            INNER JOIN pais p ON np.id_pais = p.id_pais
            INNER JOIN palabraclave pc ON np.id_palabra_clave = pc.id_palabra_clave 
            WHERE id_usuario = @idUsuario", new { idUsuario });
        }
        public async Task<NotaPeriodistica?> ObtenerPorId(int id, int idUsuario)
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<NotaPeriodistica>(@"
             SELECT 
                 np.id_nota AS Id, np.url_fuente AS UrlFuente, np.nombre_fuente AS NombreFuente, 
                 np.titulo AS Titulo, np.fecha_publicacion AS FechaPublicacion, np.ano_publicacion AS AnoPublicacion, 
                 np.fecha_hecho AS FechaHecho, np.ano_hecho AS AnoHecho, np.id_palabra_clave AS IdPalabraClave, 
                 np.id_sector_economico AS IdSectorEconomico, np.nombre_area_protegida AS NombreAreaProtegida, 
                 np.numero_detenidos AS NumeroDetenidos, np.nacionalidad_detenidos AS NacionalidadDetenidos, 
                 np.institucion_responsable AS InstitucionResponsable, np.instituciones_de_apoyo AS InstitucionesDeApoyo, 
                 np.id_economia_ilicita AS IdEconomiaIlicita, np.id_usuario AS IdUsuario, 
                 np.latitud AS Latitud, np.longitud AS Longitud, np.id_pais AS IdPais, 
                 np.id_provincia_departamento AS IdProvinciaDepartamento, np.id_comarca AS IdComarca, 
                 np.id_distrito_municipio AS IdDistritoMunicipio, np.id_corregimiento_aldea AS IdCorregimientoAldea, 
                 np.nombre_fauna AS NombreFauna, np.cantidad_fauna AS CantidadFauna, np.nombre_flora AS NombreFlora, 
                 np.cantidad_flora AS CantidadFlora, np.cantidad_personas_trata AS CantidadPersonasTrata, 
                 np.nacionalidad_trata AS NacionalidadTrata, np.nombre_cartel AS NombreCartel, 
                 np.nombre_operacion_policial AS NombreOperacionPolicial, np.idtipoviolencia AS HuboViolencia, 
                 np.cantidad_en_kilos AS CantidadEnKilos, np.monto_en_dolares_transporte AS MontoEnDolaresTransporte, 
                 np.numero_pistas_destruidas AS NumeroPistasDestruidas, 
                 np.numero_hectareas_area_protegida AS NumeroHectareasAreaProtegida, 
                 np.monto_en_dolares_intento_soborno AS MontoEnDolaresIntentoSoborno, np.id_incautacion AS IdIncautacion, 
                 np.monto_en_dolares_dinero AS MontoEnDolaresDinero, np.monto_en_dolares_joyas AS MontoEnDolaresJoyas, 
                 np.numero_inmuebles AS NumeroInmuebles, np.numero_fincas AS NumeroFincas, 
                 np.numero_hectareas AS NumeroHectareas, np.numero_matas_arbustos AS NumeroMatasArbustos, 
                 np.id_droga AS IdDroga, np.monto_en_dolares_matas_arbustos AS MontoEnDolaresMatasArbustos, 
                 np.breve_nota AS BreveNota, np.nota_grande AS NotaGrande, np.id_vehiculo AS IdVehiculo, 
                 np.ruta_carretera AS RutaCarretera, np.id_transporte AS TipoTransporte, 
                 np.numero_hectareas_terrenos AS NumeroHectareasTerrenos, np.id_matas_arbustos AS IdMatasArbustos,
                 pc.palabra_clave AS MostrarNombrePalabraClave,
                 p.nombre_pais AS MostrarNombrePaís,
                 pd.nombre_provincia AS MostrarNombreDepartamento,
                 c.nombre_comarca AS MostrarNombreComarca,
                 dm.nombre_distrito AS MostrarNombreMunicipio,
                 ca.nombre_corregimiento AS MostrarNombreAldea,
                 t.tipo_violencia  AS MostrarNombreHuboViolencia,
                 tt.tipo_transporte AS MostrarNombreTipoTransporte,
                 s.nombre_sector AS MostrarNombreSectorEconómico,
                 e.nombre_economia AS MostrarNombreEconomíaIlícita,
                 nt.nombre_pais AS MostrarNombreNacionalidadTrata,
                 i.tipo_incautacion AS MostrarNombreIncautación,
                 d.tipo_droga AS MostrarNombreDroga,
                 ma.tipo_matas_arbustos AS MostrarNombreMatasArbustos,
                 v.tipo_vehiculo AS MostrarNombreVehículo,
                 np.id_provincia_departamento AS IdDepartamento,
                 np.id_comarca AS IdComarca,
                 np.id_distrito_municipio AS IdMunicipio,
                 np.id_corregimiento_aldea AS IdAldea
             FROM notaperiodistica np
             LEFT JOIN palabraclave pc ON np.id_palabra_clave = pc.id_palabra_clave 
             LEFT JOIN pais p ON np.id_pais = p.id_pais
             LEFT JOIN provincia_departamento pd ON np.id_provincia_departamento = pd.id_provincia 
             LEFT JOIN comarca c ON np.id_comarca = c.id_comarca 
             LEFT JOIN distrito_municipio dm ON np.id_distrito_municipio = dm.id_distrito 
             LEFT JOIN corregimiento_aldea ca ON np.id_corregimiento_aldea = ca.id_corregimiento 
             LEFT JOIN tipoviolencia t ON np.idtipoviolencia = t.id_tipoviolencia 
             LEFT JOIN transporte tt ON np.id_transporte = tt.id_transporte 
             LEFT JOIN sectoreconomico s ON np.id_sector_economico = s.id_sector_economico 
             LEFT JOIN economiailicita e ON np.id_economia_ilicita = e.id_economia_ilicita 
             LEFT JOIN pais nt ON np.nacionalidad_trata = nt.id_pais
             LEFT JOIN incautaciones i ON np.id_incautacion = i.id_incautacion 
             LEFT JOIN drogas d ON np.id_droga = d.id_drogas 
             LEFT JOIN matas_arbustos ma ON np.id_matas_arbustos = ma.id_matas_arbustos 
             LEFT JOIN vehiculos v ON np.id_vehiculo = v.id_vehiculo 
             WHERE np.id_nota =  @Id AND np.id_usuario = @idUsuario;",
                                         new { Id = id, idUsuario });
        }


        public async Task Actualizar(NotaPeriodistica notaPeriodistica)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(
        @"UPDATE notaperiodistica
        SET url_fuente = @UrlFuente, 
            nombre_fuente = @NombreFuente, 
            titulo = @Titulo, 
            fecha_publicacion = @FechaPublicacion, 
            ano_publicacion = @AnoPublicacion, 
            fecha_hecho = @FechaHecho, 
            ano_hecho = @AnoHecho, 
            id_palabra_clave = @IdPalabraClave, 
            id_sector_economico = @IdSectorEconomico, 
            nombre_area_protegida = @NombreAreaProtegida, 
            numero_detenidos = @NumeroDetenidos, 
            nacionalidad_detenidos = @NacionalidadDetenidos, 
            institucion_responsable = @InstitucionResponsable, 
            instituciones_de_apoyo = @InstitucionesDeApoyo, 
            id_economia_ilicita = @IdEconomiaIlicita, 
            id_usuario = @IdUsuario, 
            latitud = @Latitud, 
            longitud = @Longitud, 
            id_pais = @IdPais, 
            id_provincia_departamento = @IdDepartamento, 
            id_comarca = @IdComarca, 
            id_distrito_municipio = @IdMunicipio, 
            id_corregimiento_aldea = @IdAldea, 
            nombre_fauna = @NombreFauna, 
            cantidad_fauna = @CantidadFauna, 
            nombre_flora = @NombreFlora, 
            cantidad_flora = @CantidadFlora, 
            cantidad_personas_trata = @CantidadPersonasTrata, 
            nacionalidad_trata = @NacionalidadTrata, 
            nombre_cartel = @NombreCartel, 
            nombre_operacion_policial = @NombreOperacionPolicial, 
            idtipoviolencia = @HuboViolencia, 
            cantidad_en_kilos = @CantidadEnKilos, 
            monto_en_dolares_transporte = @MontoEnDolaresTransporte, 
            numero_pistas_destruidas = @NumeroPistasDestruidas, 
            numero_hectareas_area_protegida = @NumeroHectareasAreaProtegida, 
            monto_en_dolares_intento_soborno = @MontoEnDolaresIntentoSoborno, 
            id_incautacion = @IdIncautacion, 
            monto_en_dolares_dinero = @MontoEnDolaresDinero, 
            monto_en_dolares_joyas = @MontoEnDolaresJoyas, 
            numero_inmuebles = @NumeroInmuebles, 
            numero_fincas = @NumeroFincas, 
            numero_hectareas = @NumeroHectareas, 
            numero_matas_arbustos = @NumeroMatasArbustos, 
            id_droga = @IdDroga, 
            monto_en_dolares_matas_arbustos = @MontoEnDolaresMatasArbustos, 
            breve_nota = @BreveNota, 
            nota_grande = @NotaGrande, 
            id_vehiculo = @IdVehiculo, 
            ruta_carretera = @RutaCarretera, 
            id_transporte = @TipoTransporte, 
            numero_hectareas_terrenos = @NumeroHectareasTerrenos, 
            id_matas_arbustos = @IdMatasArbustos
        WHERE id_nota = @Id AND id_usuario = @IdUsuario;",
        notaPeriodistica);
        }


        public async Task Borrar(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE FROM notaperiodistica WHERE id_nota = @Id", new { Id = id });
        }

        public async Task<IEnumerable<NotaPeriodistica>> ObtenerTodasLasNotas()
        {
            using var connection = new NpgsqlConnection(connectionString);
            return await connection.QueryAsync<NotaPeriodistica>(@"
        SELECT 
            n.titulo, 
            n.latitud, 
            n.longitud, 
            p.palabra_clave AS nombrePalabraClave 
        FROM notaperiodistica n
        INNER JOIN palabraclave p ON p.id_palabra_clave = n.id_palabra_clave");
        }

        public async Task<IEnumerable<string>> MostrarEnMapa(int id)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var query = @"
        select palabra_clave 
            from palabraclave p 
            where id_palabra_clave = @Id";

            return await connection.QueryAsync<string>(query, new { Id = id });
        }


        //Filtros:
        public async Task<IEnumerable<NotaPeriodistica>> ObtenerConFiltros(int idUsuario, string titulo, int? palabraClaveId, DateTime? fechaHecho, int? AnoHecho, int? paisId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var query = @"
        SELECT 
            np.id_nota AS Id,
            url_fuente AS UrlFuente,
            titulo AS Titulo, 
            fecha_publicacion AS FechaPublicacion, 
            fecha_hecho AS FechaHecho, 
            ano_hecho AS AnoHecho,
            p.nombre_pais AS MostrarNombrePaís,
            pc.palabra_clave AS MostrarNombrePalabraClave
        FROM notaperiodistica AS np
        INNER JOIN pais p ON np.id_pais = p.id_pais
        INNER JOIN palabraclave pc ON np.id_palabra_clave = pc.id_palabra_clave 
        WHERE id_usuario = @idUsuario";

            var parameters = new DynamicParameters();
            parameters.Add("idUsuario", idUsuario);

            if (!string.IsNullOrEmpty(titulo))
            {
                query += " AND titulo ILIKE @titulo";
                parameters.Add("titulo", $"%{titulo}%");
            }

            if (palabraClaveId.HasValue)
            {
                query += " AND np.id_palabra_clave = @palabraClaveId";
                parameters.Add("palabraClaveId", palabraClaveId);
            }

            if (fechaHecho.HasValue)
            {
                query += " AND fecha_hecho = @fechaHecho";
                parameters.Add("fechaHecho", fechaHecho);
            }

            if (AnoHecho.HasValue)
            {
                query += " AND ano_hecho = @AnoHecho";
                parameters.Add("AnoHecho", AnoHecho);
            }

            if (paisId.HasValue)
            {
                query += " AND np.id_pais = @paisId";
                parameters.Add("paisId", paisId);
            }

            return await connection.QueryAsync<NotaPeriodistica>(query, parameters);
        }

        //Reporte Excel
        public async Task<IEnumerable<NotaPeriodistica>> ObtenerParaReporte(int idUsuario, string titulo, int? palabraClaveId, DateTime? fechaHecho, int? AnoHecho, int? paisId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var query = @"
        SELECT 
            np.id_nota AS Id,
            np.url_fuente AS UrlFuente,
            np.nombre_fuente AS NombreFuente, 
            np.titulo AS Titulo, 
            np.fecha_publicacion AS FechaPublicacion, 
            np.ano_publicacion AS AnoPublicacion, 
            np.fecha_hecho AS FechaHecho, 
            np.ano_hecho AS AnoHecho, 
            np.nota_grande AS NotaGrande, 
            np.id_pais AS IdPais, 
            np.id_provincia_departamento AS IdDepartamento, 
            np.id_comarca AS IdComarca, 
            np.id_distrito_municipio AS IdMunicipio, 
            np.id_corregimiento_aldea AS IdAldea, 
            np.ruta_carretera AS rutaCarretera, 
            np.latitud AS Latitud, 
            np.longitud AS Longitud, 
            np.nombre_cartel AS NombreCartel, 
            np.nombre_operacion_policial AS NombreOperacionPolicial, 
            np.idtipoviolencia AS huboViolencia, 
            np.id_transporte AS tipoTransporte, 
            np.cantidad_en_kilos AS CantidadEnKilos, 
            np.monto_en_dolares_transporte AS MontoEnDolaresTransporte, 
            np.numero_pistas_destruidas AS NumeroPistasDestruidas, 
            np.monto_en_dolares_intento_soborno AS MontoEnDolaresIntentoSoborno, 
            np.breve_nota AS BreveNota, 
            np.institucion_responsable AS InstitucionResponsable, 
            np.instituciones_de_apoyo AS InstitucionesDeApoyo, 
            np.id_sector_economico AS IdSectorEconomico, 
            np.id_economia_ilicita AS IdEconomiaIlicita, 
            np.nombre_fauna AS NombreFauna, 
            np.cantidad_fauna AS CantidadFauna, 
            np.nombre_flora AS NombreFlora, 
            np.cantidad_flora AS CantidadFlora, 
            np.cantidad_personas_trata AS CantidadPersonasTrata, 
            np.nacionalidad_trata AS NacionalidadTrata, 
            np.numero_detenidos AS NumeroDetenidos, 
            np.nacionalidad_detenidos AS NacionalidadDetenidos, 
            np.nombre_area_protegida AS NombreAreaProtegida, 
            np.numero_hectareas_area_protegida AS NumeroHectareasAreaProtegida, 
            np.id_incautacion AS IdIncautacion, 
            np.id_droga AS IdDroga, 
            np.id_matas_arbustos AS IdMatasArbustos, 
            np.numero_matas_arbustos AS NumeroMatasArbustos, 
            np.numero_hectareas AS NumeroHectareas, 
            np.monto_en_dolares_matas_arbustos AS MontoEnDolaresMatasArbustos, 
            np.id_vehiculo AS IdVehiculo, 
            np.monto_en_dolares_dinero AS MontoEnDolaresDinero, 
            np.monto_en_dolares_joyas AS MontoEnDolaresJoyas, 
            np.numero_inmuebles AS NumeroInmuebles, 
            np.numero_fincas AS NumeroFincas, 
            np.numero_hectareas_terrenos AS NumeroHectareasTerrenos, 
            np.id_usuario AS IdUsuario,
            pc.palabra_clave AS MostrarNombrePalabraClave,
            p.nombre_pais AS MostrarNombrePaís,
            pd.nombre_provincia AS MostrarNombreDepartamento,
            c.nombre_comarca AS MostrarNombreComarca,
            dm.nombre_distrito AS MostrarNombreMunicipio,
            ca.nombre_corregimiento AS MostrarNombreAldea,
            t.tipo_violencia  AS MostrarNombreHuboViolencia,
            tt.tipo_transporte AS MostrarNombreTipoTransporte,
            s.nombre_sector AS MostrarNombreSectorEconómico,
            e.nombre_economia AS MostrarNombreEconomíaIlícita,
            nt.nombre_pais AS MostrarNombreNacionalidadTrata,
            i.tipo_incautacion AS MostrarNombreIncautación,
            d.tipo_droga AS MostrarNombreDroga,
            ma.tipo_matas_arbustos AS MostrarNombreMatasArbustos,
            v.tipo_vehiculo AS MostrarNombreVehículo
        FROM notaperiodistica AS np
        LEFT JOIN palabraclave pc ON np.id_palabra_clave = pc.id_palabra_clave 
        LEFT JOIN pais p ON np.id_pais = p.id_pais
        LEFT JOIN provincia_departamento pd ON np.id_provincia_departamento = pd.id_provincia 
        LEFT JOIN comarca c ON np.id_comarca = c.id_comarca 
        LEFT JOIN distrito_municipio dm ON np.id_distrito_municipio = dm.id_distrito 
        LEFT JOIN corregimiento_aldea ca ON np.id_corregimiento_aldea = ca.id_corregimiento 
        LEFT JOIN tipoviolencia t ON np.idtipoviolencia = t.id_tipoviolencia 
        LEFT JOIN transporte tt ON np.id_transporte = tt.id_transporte 
        LEFT JOIN sectoreconomico s ON np.id_sector_economico = s.id_sector_economico 
        LEFT JOIN economiailicita e ON np.id_economia_ilicita = e.id_economia_ilicita 
        LEFT JOIN pais nt ON np.nacionalidad_trata = nt.id_pais
        LEFT JOIN incautaciones i ON np.id_incautacion = i.id_incautacion 
        LEFT JOIN drogas d ON np.id_droga = d.id_drogas 
        LEFT JOIN matas_arbustos ma ON np.id_matas_arbustos = ma.id_matas_arbustos 
        LEFT JOIN vehiculos v ON np.id_vehiculo = v.id_vehiculo 
        WHERE np.id_usuario = @idUsuario";

            var parameters = new DynamicParameters();
            parameters.Add("idUsuario", idUsuario);

            if (!string.IsNullOrEmpty(titulo))
            {
                query += " AND titulo ILIKE @titulo";
                parameters.Add("titulo", $"%{titulo}%");
            }

            if (palabraClaveId.HasValue)
            {
                query += " AND np.id_palabra_clave = @palabraClaveId";
                parameters.Add("palabraClaveId", palabraClaveId);
            }

            if (fechaHecho.HasValue)
            {
                query += " AND fecha_hecho = @fechaHecho";
                parameters.Add("fechaHecho", fechaHecho);
            }

            if (AnoHecho.HasValue)
            {
                query += " AND ano_hecho = @AnoHecho";
                parameters.Add("AnoHecho", AnoHecho);
            }

            if (paisId.HasValue)
            {
                query += " AND np.id_pais = @paisId";
                parameters.Add("paisId", paisId);
            }

            return await connection.QueryAsync<NotaPeriodistica>(query, parameters);
        }


    }
}
