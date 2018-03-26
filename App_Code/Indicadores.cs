using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de Indicadores
/// </summary>
public class Indicadores
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    public int empresa { get; set; }
    public int taller { get; set; }
    public string fechaIni { get; set; }
    public string fechaFin { get; set; }
    public int indicador { get; set; }
    public DataTable contadores { get; set; }

    private string sql;
    private object[] resultado;

	public Indicadores()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public void obtieneIngresos() {
        sql = string.Format("select count(orp.no_orden) as ingresos from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller where orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}'", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.scalarToInt(sql);
        indicador =  retornaValor();
    }

    
    public void obtieneTipoServicioIngresos() {
        sql = string.Format("select ts.descripcion as Servicio,count(orp.id_tipo_servicio) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller left join tipo_servicios ts on ts.id_tipo_servicio=orp.id_tipo_servicio where orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}' group by ts.descripcion,orp.id_tipo_servicio order by orp.id_tipo_servicio desc", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    public void obtieneClientesIngresos()
    {
        sql = string.Format("select c.razon_social as Cliente,count(orp.id_cliprov) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C' where orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}' group by c.razon_social,orp.id_cliprov order by orp.id_cliprov desc", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    private int retornaValor()
    {
        int valor = 0;
        try
        {
            if (Convert.ToBoolean(resultado[0]))
                valor = Convert.ToInt32(resultado[1]);
            else
                valor = 0;
        }
        catch (Exception)
        {
            valor = 0;
        }
        return valor;
    }

    private DataTable retornaTabla()
    {
        DataTable dt = new DataTable();
        try {
            if (Convert.ToBoolean(resultado[0]))
            {
                DataSet info = (DataSet)resultado[1];
                dt = info.Tables[0];
            }
            else
                dt = null;
        }
        catch (Exception) { dt = null; }
        return dt;
    }

    public void obtieneLocalizacionesIngresos()
    {
        sql = string.Format("select l.descripcion as Localizacion,count(orp.id_localizacion) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller left join Localizaciones l on l.id_localizacion=orp.id_localizacion where orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}' group by l.descripcion,orp.id_localizacion order by orp.id_localizacion desc", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    public void obtienePerfilesIngresos()
    {
        sql = string.Format("select po.descripcion as Perfil,count(orp.id_perfilOrden) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller left join PerfilesOrdenes po on po.id_perfilOrden=orp.id_perfilOrden where orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}' group by po.descripcion,orp.id_perfilOrden order by orp.id_perfilOrden desc", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    public void obtieneValuacionIngresos()
    {
        sql = string.Format("select ts.descripcion as Valuacion,count(orp.id_tipo_valuacion) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller left join tipo_valuacion ts on ts.id_tipo_valuacion=orp.id_tipo_valuacion where orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}' group by ts.descripcion,orp.id_tipo_valuacion order by orp.id_tipo_valuacion desc", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    public void obtieneEtapasIngresos()
    {
        sql = string.Format("select tabla.Etapa,count(tabla.Ordenes) as Ordenes from(select case (select (case cs.datos_orden when 1 then 1 else 0 end)+(case cs.inventario when 1 then 1 else 0 end)+(case cs.caracteristicas_vehiculo when 1 then 1 else 0 end)) when 1 then 'Datos Orden' when 2 then 'Inventario' when 3 then 'Características del Vehículo' else 'Pendiente' end as Etapa,(case cs.datos_orden when 1 then 1 else 0 end)+(case cs.inventario when 1 then 1 else 0 end)+(case cs.caracteristicas_vehiculo when 1 then 1 else 0 end) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller inner join control_procesos cs on cs.id_empresa= orp.id_empresa and cs.id_taller=orp.id_taller and cs.no_orden=orp.no_orden where orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}') as tabla group by tabla.Etapa, tabla.Ordenes", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    public void obtienePendientesValuar()
    {
        sql = string.Format("SELECT COUNT(orp.no_orden) FROM Ordenes_Reparacion AS orp " +
        "INNER JOIN Seguimiento_Orden AS so ON orp.no_orden = so.no_orden AND orp.id_empresa = so.id_empresa AND orp.id_taller = so.id_taller " +
        "WHERE orp.tipo_cliprov = 'C' AND orp.id_empresa = {0} AND orp.id_taller = {1} AND(orp.status_orden = 'A') " +
        "AND (((so.f_alta IS NULL OR so.f_alta='1900-01-01') OR (f_entrega IS NULL OR f_entrega='1900-01-01') OR (f_alta_portal IS NULL OR f_alta_portal='1900-01-01')) " +
        "OR ((so.f_alta IS NOT NULL OR so.f_alta<>'1900-01-01') AND (f_entrega IS NOT NULL OR f_entrega<>'1900-01-01') AND (f_alta_portal IS NOT NULL OR f_alta_portal<>'1900-01-01') AND (f_valuacion IS NULL OR f_valuacion='1900-01-01'))) " +
        "AND so.f_recepcion BETWEEN '{2}' AND '{3}'", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.scalarToInt(sql);
        indicador = retornaValor();
    }

    public void obtieneValuados()
    {
        sql = string.Format("SELECT COUNT(orp.no_orden) FROM Ordenes_Reparacion AS orp " +
        "INNER JOIN Seguimiento_Orden AS so ON orp.no_orden = so.no_orden AND orp.id_empresa = so.id_empresa AND orp.id_taller = so.id_taller " +
        "WHERE orp.id_empresa = {0} AND orp.id_taller = {1} AND orp.tipo_cliprov = 'C' AND orp.status_orden = 'A' " +
        "AND (so.f_alta IS NOT NULL OR so.f_alta <> '1900-01-01') AND (f_entrega IS NOT NULL OR f_entrega <> '1900-01-01') AND (f_alta_portal IS NOT NULL OR f_alta_portal <> '1900-01-01') " +
        "AND (f_valuacion IS NOT NULL OR f_valuacion <> '1900-01-01') AND (f_autorizacion IS NULL OR f_autorizacion = '1900-01-01') AND (so.f_recepcion BETWEEN '{2}' AND '{3}')", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.scalarToInt(sql);
        indicador = retornaValor();
    }

    public void obtieneValuacionesAutorizadas()
    {
        sql = string.Format("SELECT COUNT(orp.no_orden) FROM Ordenes_Reparacion AS orp " +
        "INNER JOIN Seguimiento_Orden AS so ON orp.no_orden = so.no_orden AND orp.id_empresa = so.id_empresa AND orp.id_taller = so.id_taller " +
        "WHERE orp.id_empresa = {0} AND orp.id_taller = {1} AND orp.tipo_cliprov = 'C' AND orp.status_orden = 'A' " +
        "AND (so.f_alta IS NOT NULL OR so.f_alta <> '1900-01-01') AND (f_entrega IS NOT NULL OR f_entrega <> '1900-01-01') AND (f_alta_portal IS NOT NULL OR f_alta_portal <> '1900-01-01') " +
        "AND (f_valuacion IS NOT NULL OR f_valuacion <> '1900-01-01') AND (f_autorizacion IS NOT NULL OR f_autorizacion <> '1900-01-01') AND (so.f_recepcion BETWEEN '{2}' AND '{3}')", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.scalarToInt(sql);
        indicador = retornaValor();
    }

    public object[] obtieneValores(int idEmpresa, int idTaller, string fechaIni, string fechaFin)
    {
        sql = string.Format("declare @total int declare @terminados int declare @proceso int declare @entregados int declare @vencidos int declare @porvencer int " +
"set @total=0 set @terminados=0 set @proceso=0 set @entregados=0 set @vencidos=0 set @porvencer=0 " +
"set @total = (select count(*) from (select case when orp.id_localizacion = 3 then datepart(dw, '{2}') when orp.id_localizacion = 4 then datepart(dw, so.f_entrega_estimada) when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) and so.f_entrega_estimada < '{2}' then datepart(dw, '{2}') else datepart(dw, '{2}') end as dia, " +
"orp.no_orden, upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, upper(v.color_ext) as color,  upper(c.razon_social) as cliente, case  when orp.id_localizacion = 4 then so.f_entrega_estimada  when orp.id_localizacion = 3 then so.f_entrega_estimada when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha, " +
"orp.id_localizacion, l.descripcion as localizacion, orp.avance_orden, orp.fase_orden, so.f_entrega_estimada, so.f_terminado, so.f_tocado, so.f_entrega from ordenes_reparacion orp left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' left join localizaciones l on l.id_localizacion = orp.id_localizacion where orp.id_empresa = {0} and orp.id_taller = {1} and orp.status_orden in ('A', 'T') " +
"and  ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4  and so.f_entrega_estimada between '{2}' and '{3}') or so.f_entrega_estimada between '{2}' and '{3}' or so.f_entrega_estimada < '{2}')) as tabla where tabla.fecha <> '1900-01-01' ) " +
"set @terminados = (select count(*) from (select case when orp.id_localizacion = 3 then datepart(dw, '{2}') when orp.id_localizacion = 4 then datepart(dw, so.f_entrega_estimada) when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) and so.f_entrega_estimada < '{2}' then datepart(dw, '{2}') else datepart(dw, '{2}') end as dia, " +
"orp.no_orden, upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, upper(v.color_ext) as color,  upper(c.razon_social) as cliente, case  when orp.id_localizacion = 4 then so.f_entrega_estimada  when orp.id_localizacion = 3 then so.f_entrega_estimada when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha, " +
"orp.id_localizacion, l.descripcion as localizacion, orp.avance_orden, orp.fase_orden, so.f_entrega_estimada, so.f_terminado, so.f_tocado, so.f_entrega from ordenes_reparacion orp left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' left join localizaciones l on l.id_localizacion = orp.id_localizacion where orp.id_empresa = {0} and orp.id_taller = {1} and orp.status_orden in ('A', 'T') " +
"and  ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4  and so.f_entrega_estimada between '{2}' and '{3}') or so.f_entrega_estimada between '{2}' and '{3}' or so.f_entrega_estimada < '{2}')) as tabla where tabla.fecha <> '1900-01-01' and tabla.id_localizacion =3 ) " +
"set @proceso = (select count(*) from (select case when orp.id_localizacion = 3 then datepart(dw, '{2}') when orp.id_localizacion = 4 then datepart(dw, so.f_entrega_estimada) when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) and so.f_entrega_estimada < '{2}' then datepart(dw, '{2}') else datepart(dw, '{2}') end as dia, " +
"orp.no_orden, upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, upper(v.color_ext) as color,  upper(c.razon_social) as cliente, case  when orp.id_localizacion = 4 then so.f_entrega_estimada  when orp.id_localizacion = 3 then so.f_entrega_estimada when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha, " +
"orp.id_localizacion, l.descripcion as localizacion, orp.avance_orden, orp.fase_orden, so.f_entrega_estimada, so.f_terminado, so.f_tocado, so.f_entrega from ordenes_reparacion orp left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' left join localizaciones l on l.id_localizacion = orp.id_localizacion where orp.id_empresa = {0} and orp.id_taller = {1} and orp.status_orden in ('A', 'T') " +
"and  ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4  and so.f_entrega_estimada between '{2}' and '{3}') or so.f_entrega_estimada between '{2}' and '{3}' or so.f_entrega_estimada < '{2}')) as tabla where tabla.fecha <> '1900-01-01' and tabla.id_localizacion not in (3,4) and (tabla.f_tocado <> '1900-01-01' and tabla.f_tocado is not null)  and tabla.f_entrega_estimada between '{2}' and '{3}') " +
"set @entregados = (select count(*) from (select case when orp.id_localizacion = 3 then datepart(dw, '{2}') when orp.id_localizacion = 4 then datepart(dw, so.f_entrega_estimada) when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) and so.f_entrega_estimada < '{2}' then datepart(dw, '{2}') else datepart(dw, '{2}') end as dia, " +
"orp.no_orden, upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, upper(v.color_ext) as color,  upper(c.razon_social) as cliente, case  when orp.id_localizacion = 4 then so.f_entrega_estimada  when orp.id_localizacion = 3 then so.f_entrega_estimada when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha, " +
"orp.id_localizacion, l.descripcion as localizacion, orp.avance_orden, orp.fase_orden, so.f_entrega_estimada, so.f_terminado, so.f_tocado, so.f_entrega from ordenes_reparacion orp left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' left join localizaciones l on l.id_localizacion = orp.id_localizacion where orp.id_empresa = {0} and orp.id_taller = {1} and orp.status_orden in ('A', 'T') " +
"and  ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4  and so.f_entrega_estimada between '{2}' and '{3}') or so.f_entrega_estimada between '{2}' and '{3}' or so.f_entrega_estimada < '{2}')) as tabla where tabla.fecha <> '1900-01-01' and tabla.id_localizacion =4) " +
"set @vencidos = (select count(*) from (select case when orp.id_localizacion = 3 then datepart(dw, '{2}') when orp.id_localizacion = 4 then datepart(dw, so.f_entrega_estimada) when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) and so.f_entrega_estimada < '{2}' then datepart(dw, '{2}') else datepart(dw, '{2}') end as dia, " +
"orp.no_orden, upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, upper(v.color_ext) as color,  upper(c.razon_social) as cliente, case  when orp.id_localizacion = 4 then so.f_entrega_estimada  when orp.id_localizacion = 3 then so.f_entrega_estimada when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha, " +
"orp.id_localizacion, l.descripcion as localizacion, orp.avance_orden, orp.fase_orden, so.f_entrega_estimada, so.f_terminado, so.f_tocado, so.f_entrega from ordenes_reparacion orp left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' left join localizaciones l on l.id_localizacion = orp.id_localizacion where orp.id_empresa = {0} and orp.id_taller = {1} and orp.status_orden in ('A', 'T') " +
"and  ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4  and so.f_entrega_estimada between '{2}' and '{3}') or so.f_entrega_estimada between '{2}' and '{3}' or so.f_entrega_estimada < '{2}')) as tabla where tabla.fecha <> '1900-01-01' and tabla.id_localizacion not in(3,4) and tabla.f_entrega_estimada<'{4}') " +
"set @porvencer = (select count(*) from (select case when orp.id_localizacion = 3 then datepart(dw, '{2}') when orp.id_localizacion = 4 then datepart(dw, so.f_entrega_estimada) when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) and so.f_entrega_estimada < '{2}' then datepart(dw, '{2}') else datepart(dw, '{2}') end as dia, " +
"orp.no_orden, upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, upper(v.color_ext) as color,  upper(c.razon_social) as cliente, case  when orp.id_localizacion = 4 then so.f_entrega_estimada  when orp.id_localizacion = 3 then so.f_entrega_estimada when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha, " +
"orp.id_localizacion, l.descripcion as localizacion, orp.avance_orden, orp.fase_orden, so.f_entrega_estimada, so.f_terminado, so.f_tocado, so.f_entrega from ordenes_reparacion orp left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' left join localizaciones l on l.id_localizacion = orp.id_localizacion where orp.id_empresa = {0} and orp.id_taller = {1} and orp.status_orden in ('A', 'T') " +
"and  ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4  and so.f_entrega_estimada between '{2}' and '{3}') or so.f_entrega_estimada between '{2}' and '{3}' or so.f_entrega_estimada < '{2}')) as tabla where tabla.fecha <> '1900-01-01' and tabla.f_entrega_estimada = dateadd(d,3,'{4}')) " +
"select @total as totales,@terminados as terminados,@proceso as proceso,@entregados as entregados,@vencidos as vencidos,@porvencer as porvencer", idEmpresa.ToString(), idTaller.ToString(), fechaIni, fechaFin, fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"));
        return ejecuta.dataSet(sql);
    }

    public object[] optieneAseguradorasValuacion(int idEmpresa, int idTaller, string fechaIni, string fechaFin)
    {
        sql = string.Format("SELECT COUNT(orp.no_orden) AS total, orp.id_cliprov, c.razon_social AS cliente, isnull(c.rgb_r,0) AS rgb_r, isnull(c.rgb_g,0) AS rgb_g, isnull(c.rgb_b,0) AS rgb_b FROM Ordenes_Reparacion AS orp INNER JOIN Seguimiento_Orden AS so ON orp.no_orden = so.no_orden AND orp.id_empresa = so.id_empresa AND orp.id_taller = so.id_taller " +
            "INNER JOIN Cliprov c ON orp.id_cliprov = c.id_cliprov and c.tipo = 'C' " +
            "WHERE orp.id_empresa ={0} AND orp.id_taller = {1} AND  orp.tipo_cliprov = 'C' AND orp.status_orden = 'A' AND(so.f_alta IS NULL OR so.f_alta = '1900-01-01' OR f_valuacion IS NULL OR f_valuacion = '1900-01-01' OR f_autorizacion IS NULL OR f_autorizacion = '1900-01-01' " +
            "OR f_entrega IS NULL OR f_entrega = '1900-01-01' OR f_alta_portal IS NULL OR f_alta_portal = '1900-01-01') AND(so.f_recepcion BETWEEN '{2}' AND '{3}') " +
            "GROUP BY orp.id_cliprov, c.razon_social, c.rgb_r, c.rgb_g, c.rgb_b ORDER BY id_cliprov", idEmpresa, idTaller, fechaIni, fechaFin);

        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Taller"].ToString());
        object[] obj = new object[2] { false, false };
        using (conn)
        {
            using (System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand("genIndicadores", conn))
            {
                try {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@id_empresa", idEmpresa).SqlDbType = SqlDbType.Int;
                    comm.Parameters.AddWithValue("@id_taller", idTaller).SqlDbType = SqlDbType.Int;
                    comm.Parameters.AddWithValue("@fechaIni", fechaIni).SqlDbType = SqlDbType.Date;
                    comm.Parameters.AddWithValue("@fechaFin", fechaFin).SqlDbType = SqlDbType.Date;
                
                    System.Data.SqlClient.SqlDataReader dr = comm.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("id_cliprov");
                    dt.Columns.Add("cliente");
                    dt.Columns.Add("rgb_r");
                    dt.Columns.Add("rgb_g");
                    dt.Columns.Add("rgb_b");
                    dt.Columns.Add("totPorValuar");
                    dt.Columns.Add("totPorAut");
                    dt.Columns.Add("totAutorizados");
                    while (dr.Read())
                    {
                        dt.Rows.Add(dr.GetValue(1), dr.GetString(2), dr.GetValue(3), dr.GetValue(4), dr.GetValue(5), dr.GetValue(6), dr.GetValue(7), dr.GetValue(8));
                    }
                    obj[0] = true;
                    obj[1] = dt;
                    dr.Close();
                }
                catch(Exception ex)
                {
                    obj[1] = ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        return obj; //ejecuta.dataSet(sql);
    }
}