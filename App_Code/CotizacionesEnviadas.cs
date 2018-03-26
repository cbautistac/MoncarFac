using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CotizacionesEnviadas
/// </summary>
public class CotizacionesEnviadas
{
    E_Utilities.Fechas fechas = new E_Utilities.Fechas();
    Ejecuciones ejecuta = new Ejecuciones();
    public int orden { get; set; }
    public int empresa { get; set; }
    public int taller { get; set; }
    public int cotizacion { get; set; }
    public int proveedor { get; set; }
    public bool enviado { get; set; }
    public DateTime fecha { get; set; }
    public string correo { get; set; }
    public int usuario { get; set; }
    public string motivo { get; set; }
    public object[] retorno { get; set; }

    private string sql { get; set; }

    public CotizacionesEnviadas()
    {
        orden = empresa = taller = cotizacion = proveedor = usuario = 0;
        enviado = false;
        fecha = DateTime.Now;
        correo = motivo = sql = string.Empty;
        retorno = new object[2] { false, "" };
    }


    public void insertaEnvio() {
        int envio = 0;
        if (enviado)
            envio = 1;
        sql = string.Format("insert into cotizaciones_enviadas values({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}',{9},'{10}')", orden, empresa, taller, cotizacion, proveedor, envio, fecha.ToString("yyyy-MM-dd"), fecha.ToString("HH:mm:ss"), correo, usuario, motivo.Replace("'", " "));
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void actualizaEnvio()
    {
        int envio = 0;
        if (enviado)
            envio = 1;
        sql = string.Format("update cotizaciones_enviadas set enviado={5},fecha='{6}',hora='{7}',correo='{8}',usuario={9},motivo_fallo='{10}' where no_orden={0} and id_empresa={1} and id_taller={2} and id_cotizacion={3} and id_cliprov={4}", orden, empresa, taller, cotizacion, proveedor, envio, fecha.ToString("yyyy-MM-dd"), fecha.ToString("HH:mm:ss"), correo, usuario, motivo.Replace("'", " "));
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void insertaEnvioCompra()
    {
        int envio = 0;
        if (enviado)
            envio = 1;
        sql = string.Format("insert into compras_enviadas values({0},{1},{2},{3},{4},{5},'{6}','{7}','{8}',{9},'{10}')", orden, empresa, taller, cotizacion, proveedor, envio, fecha.ToString("yyyy-MM-dd"), fecha.ToString("HH:mm:ss"), correo, usuario, motivo.Replace("'", " "));
        retorno = ejecuta.insertUpdateDelete(sql);
    }


}