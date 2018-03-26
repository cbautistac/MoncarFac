using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;
using System.Data;

/// <summary>
/// Descripción breve de DatosProgSemanal
/// </summary>
public class DatosProgSemanal
{
    Fechas fechas = new Fechas();
    Ejecuciones ejecuta = new Ejecuciones();
    string sql;

    public DatosProgSemanal()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string obtieneTerminados(int noOrden, int idEmpresa, int idTaller, string fechaIni, string fechaFin)
    {
        sql = "select cast(count(*)as varchar)as autos from seguimiento_orden " +
              "where no_orden="+noOrden.ToString()+" and id_empresa="+idEmpresa.ToString()+" and id_taller="+idTaller.ToString()+" and f_terminado between '" + fechaIni + "' and '" + fechaFin + "' and f_entrega in ('1900-01-01',null)";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneEnProceso(int noOrden, int idEmpresa, int idTaller, string fechaIni, string fechaFin)
    {
        sql = "select cast(count(*)as varchar)as autos from seguimiento_orden " +
              "where no_orden="+noOrden.ToString()+" and id_empresa="+idEmpresa.ToString()+" and id_taller="+idTaller.ToString()+" and f_tocado> '1900-01-01' and f_tocado is not null and f_entrega in ('1900-01-01',null) and f_entrega_estimada between '" + fechaIni + "' and '" + fechaFin + "'";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneEnregados(int noOrden, int idEmpresa, int idTaller, string fechaIni, string fechaFin)
    {
        sql = "select cast(count(*)as varchar)as autos from seguimiento_orden " +
              "where no_orden="+noOrden.ToString()+" and id_empresa="+idEmpresa.ToString()+" and id_taller="+idTaller.ToString()+" and f_terminado not in ('1900-01-01',null) and f_entrega between '" + fechaIni + "' and '" + fechaFin + "'";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneIncumplimiento(int noOrden, int idEmpresa, int idTaller, string fechaIni, string fechaFin)
    {
        sql = "select cast(count(*)as varchar)as autos from seguimiento_orden "+
              "where no_orden="+noOrden.ToString()+" and id_empresa="+idEmpresa.ToString()+" and id_taller="+idTaller.ToString()+" and f_entrega_estimada < '"+fechaIni+"' and f_entrega in ('1900-01-01',null)";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneProxVencer(int noOrden, int idEmpresa, int idTaller)
    {
        DateTime fechaLimite = Convert.ToDateTime(fechas.obtieneFechaLocal());
        fechaLimite = fechaLimite.AddDays(3);
        sql = "select cast(count(*)as varchar)as autos from seguimiento_orden " +
              "where no_orden="+noOrden.ToString()+" and id_empresa="+idEmpresa.ToString()+" and id_taller="+idTaller.ToString()+" and f_entrega_estimada = '" + fechaLimite.ToString("yyyy-MM-dd") + "' and f_entrega in ('1900-01-01',null)";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneTotales(int idEmpresa, int idTaller, string fechaIni, string fechaFin, string cuenta)
    {
        string where = "";
        sql = "select cast(count(*)as varchar)as cuenta from " +
              "(select case when orp.id_localizacion = 3 then datepart(dw, '" + fechaIni + "') " +
              " when orp.id_localizacion = 4 then datepart(dw, so.f_entrega_estimada) " +
              " when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) and so.f_entrega_estimada < '" + fechaIni + "' " +
              " then datepart(dw, '" + fechaIni + "') else datepart(dw, '" + fechaIni + "') end as dia, orp.no_orden, " +
              " upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, " +
              " upper(v.color_ext) as color, " +
              " upper(c.razon_social) as cliente,case  when orp.id_localizacion = 4 then so.f_entrega_estimada " +
              " when orp.id_localizacion = 3 then so.f_entrega_estimada " +
              " when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha, " +
              " orp.id_localizacion, l.descripcion as localizacion, orp.avance_orden, orp.fase_orden, " +
              " so.f_entrega_estimada, so.f_terminado, so.f_tocado, so.f_entrega " +
              " from ordenes_reparacion orp " +
              " left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden " +
              " left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
              " left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
              " left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
              " left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
              " left join localizaciones l on l.id_localizacion = orp.id_localizacion " +
              " where orp.id_empresa = "+idEmpresa.ToString()+" and orp.id_taller = "+idTaller.ToString()+" and orp.status_orden in ('A', 'T') and " +
              " ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4 " +
              " and so.f_entrega_estimada between '" + fechaIni + "' and '" + fechaFin + "') " +
              " or so.f_entrega_estimada between '" + fechaIni + "' and '" + fechaFin + "' or so.f_entrega_estimada < '" + fechaIni + "')) as tabla " +
              " where tabla.fecha <> '1900-01-01' ";
        switch (cuenta)
            {
            case "terminados":
                where = " and tabla.id_localizacion =3 ";
                break;
            case "productividad":
                where = " and tabla.id_localizacion =4 ";
                break;
            case "procesos":
                where = " and tabla.id_localizacion not in (3,4) and (tabla.f_tocado <> '1900-01-01' and tabla.f_tocado is not null) ";
                break;
            case "otros":
                where = " --and tabla.id_localizacion not in (3,4) ";
                break;
            case "incumplimiento":
                where = " and tabla.id_localizacion !=4 and tabla.f_entrega_estimada<'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd")+"'";
                break;
            case "proxVencer":
                where = " and tabla.f_entrega_estimada='" + fechas.obtieneFechaLocal().AddDays(3).ToString("yyyy-MM-dd") + "'";
                break;
        }
        sql = sql + " " + where;
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] optieneAseguradoras(int idEmpresa, int idTaller, string fechaIni, string fechaFin)
    {
        sql = "select tabla.id_cliprov,count(tabla.cliente)as total,tabla.cliente,tabla.rgb_r,rgb_g,rgb_b from " +
              "(select case when orp.id_localizacion = 3 then datepart(dw, '" + fechaIni + "') " +
              " when orp.id_localizacion = 4 then datepart(dw, so.f_entrega_estimada) " +
              " when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) and so.f_entrega_estimada < '" + fechaIni + "' " +
              " then datepart(dw, '" + fechaIni + "') else datepart(dw, '" + fechaIni + "') end as dia, orp.no_orden, " +
              " upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, " +
              " upper(v.color_ext) as color, " +
              " upper(c.razon_social) as cliente,case  when orp.id_localizacion = 4 then so.f_entrega_estimada " +
              " when orp.id_localizacion = 3 then so.f_entrega_estimada " +
              " when(orp.id_localizacion <> 4 or orp.id_localizacion <> 3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha, " +
              " orp.id_localizacion, l.descripcion as localizacion, orp.avance_orden, orp.fase_orden, " +
              " so.f_entrega_estimada, so.f_terminado, so.f_tocado, so.f_entrega,c.rgb_r,c.rgb_g,c.rgb_b,c.id_cliprov " +
              " from ordenes_reparacion orp " +
              " left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden " +
              " left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
              " left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
              " left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
              " left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
              " left join localizaciones l on l.id_localizacion = orp.id_localizacion " +
              " where orp.id_empresa = " + idEmpresa.ToString() + " and orp.id_taller = " + idTaller.ToString() + " and orp.status_orden in ('A', 'T') and " +
              " ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4 " +
              " and so.f_entrega_estimada between '" + fechaIni + "' and '" + fechaFin + "') " +
              " or so.f_entrega_estimada between '" + fechaIni + "' and '" + fechaFin + "' or so.f_entrega_estimada < '" + fechaIni + "')) as tabla " +
              " where tabla.fecha <> '1900-01-01' and tabla.id_localizacion not in(3,4) and tabla.f_entrega_estimada<'"+fechas.obtieneFechaLocal().ToString("yyyy-MM-dd")+"' group by tabla.id_cliprov,tabla.cliente,tabla.rgb_r,tabla.rgb_g,tabla.rgb_b";
        return ejecuta.dataSet(sql);
    }
}