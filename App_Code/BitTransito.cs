using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de BitacoraTransito
/// </summary>
public class BitTransito
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    public int empresa { get; set; }
    public int taller { get; set; }
    public string fechaIni { get; set; }
    public string fechaFin { get; set; }
    public int retorno { get; set; }
    public DataTable contadores { get; set; }

    private string sql;
    private object[] resultado;
    public BitTransito()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public void obtieneTransito()
    {
        sql = "select count(orp.no_orden) as transito "+
                " from Ordenes_Reparacion orp"+
                " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller" +
                " where orp.status_orden='A' and orp.id_perfilOrden = 2 and orp.id_empresa = " + empresa.ToString()+ " and orp.id_taller = "+taller.ToString()
                /*" select count(orp.no_orden) as transito" +
                " from Ordenes_Reparacion orp" +
                " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller" +
                " where orp.id_perfilOrden = 2 and orp.id_empresa = " + empresa.ToString() + " and orp.id_taller = " + taller.ToString() + " and" +
                " so.f_retorno_transito < '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd")+"' )-("+
                " select count(orp.no_orden) as transito" +
                " from Ordenes_Reparacion orp" +
                " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller" +
                " where orp.id_perfilOrden = 2 and orp.id_empresa = " + empresa.ToString() + " and orp.id_taller = " + taller.ToString() + " and" +
                " so.f_retorno_transito in ('1900-01-01', null))"*/;
        resultado = ejecuta.scalarToInt(sql);
        retorno = retornaValor();
    }

    public void obtieneRetornoProgVenc()
    {
        sql = "select count(orp.no_orden) as transito" +
                " from Ordenes_Reparacion orp" +
                " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller" +
                " where orp.status_orden='A' and orp.id_perfilOrden = 2 and orp.id_empresa = " + empresa.ToString() + " and orp.id_taller = " + taller.ToString() + " and" +
                " so.f_prog_retorno_tran < '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'";
        resultado = ejecuta.scalarToInt(sql);
        retorno = retornaValor();
    }

    public void obtieneRetornoProgHoy()
    {
        sql = "select count(orp.no_orden) as transito" +
                " from Ordenes_Reparacion orp" +
                " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller" +
                " where orp.status_orden='A' and orp.id_perfilOrden = 2 and orp.id_empresa = " + empresa.ToString() + " and orp.id_taller = " + taller.ToString() + " and" +
                " so.f_prog_retorno_tran = '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'";
        resultado = ejecuta.scalarToInt(sql);
        retorno = retornaValor();
    }

    public void obtieneTipoServicioTransito()
    {
        sql = string.Format("select ts.descripcion as Servicio,count(orp.id_tipo_servicio) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller left join tipo_servicios ts on ts.id_tipo_servicio=orp.id_tipo_servicio where orp.id_perfilOrden=2 and orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}' group by ts.descripcion,orp.id_tipo_servicio order by orp.id_tipo_servicio desc", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    private DataTable retornaTabla()
    {
        DataTable dt = new DataTable();
        try
        {
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

    public void obtieneClientesTransito()
    {
        sql = string.Format("select c.razon_social as Cliente,count(orp.id_cliprov) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C' where orp.id_perfilOrden=2 and orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}' group by c.razon_social,orp.id_cliprov order by orp.id_cliprov desc", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    public void obtieneValuacionTransito()
    {
        sql = string.Format("select ts.descripcion as Valuacion,count(orp.id_tipo_valuacion) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller left join tipo_valuacion ts on ts.id_tipo_valuacion=orp.id_tipo_valuacion where orp.id_perfilOrden=2 and orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}' group by ts.descripcion,orp.id_tipo_valuacion order by orp.id_tipo_valuacion desc", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    public void obtieneEtapasTransito()
    {
        sql = string.Format("select tabla.Etapa,count(tabla.Ordenes) as Ordenes from(select case (select (case cs.datos_orden when 1 then 1 else 0 end)+(case cs.inventario when 1 then 1 else 0 end)+(case cs.caracteristicas_vehiculo when 1 then 1 else 0 end)) when 1 then 'Datos Orden' when 2 then 'Inventario' when 3 then 'Características del Vehículo' else 'Pendiente' end as Etapa,(case cs.datos_orden when 1 then 1 else 0 end)+(case cs.inventario when 1 then 1 else 0 end)+(case cs.caracteristicas_vehiculo when 1 then 1 else 0 end) as Ordenes from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller inner join control_procesos cs on cs.id_empresa= orp.id_empresa and cs.id_taller=orp.id_taller and cs.no_orden=orp.no_orden where orp.id_perfilOrden = 2 and orp.id_empresa= {0}  and orp.id_taller= {1} and so.f_recepcion between '{2}' AND '{3}') as tabla group by tabla.Etapa, tabla.Ordenes", empresa, taller, fechaIni, fechaFin);
        resultado = ejecuta.dataSet(sql);
        contadores = retornaTabla();
    }

    public object[] optieneAseguradorasTransitoVencido()
    {
        sql = "select orp.id_cliprov,c.razon_social as cliente,count(orp.id_cliprov) as total,c.rgb_r,c.rgb_g,c.rgb_b" +
              " from Ordenes_Reparacion orp" +
              " inner join cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = 'C'" +
              " inner join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden" +
              " where orp.status_orden='A' and orp.id_perfilOrden = 2 and orp.id_empresa = " + empresa.ToString() + " and orp.id_taller = " + taller.ToString() + " and f_prog_retorno_tran < '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'" +
              " group by orp.id_cliprov,c.razon_social,c.rgb_r,c.rgb_g,c.rgb_b";
        return ejecuta.dataSet(sql);
    }
}