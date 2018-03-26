using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de BitacoraValuacionDatos
/// </summary>
public class BitacoraValuacionDatos
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechasLoc = new Fechas();
    object[] resultado = new object[2];
    string sql;
    public BitacoraValuacionDatos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool actualizaBitacoraValuacion(int noOrden, int idEmpresa, int idTaller, string[] fechas, string observacion, int idUsuario)
    {
        
        fechasLoc.fecha = fechasLoc.obtieneFechaLocal();
        fechasLoc.tipoFormato = 4;
        string fechaRetorno = fechasLoc.obtieneFechaConFormato();
        fechasLoc.tipoFormato = 6;
        string horaRetorno = fechasLoc.obtieneFechaConFormato();
        
        sql = "update seguimiento_orden set f_alta='" + fechas[0] + "',f_alta_portal='" + fechas[1] + "',f_entrega='" + fechas[2] + "', f_valuacion='" + fechas[3] + "', f_autorizacion='" + fechas[4] + "' " +
              "where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and id_registro=1 " +
              "insert into bitacora_orden_comentarios values(" + noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + ",(select isnull((select top 1 boc.id_observacion from bitacora_orden_comentarios boc " +
              "where boc.no_orden = " + noOrden.ToString() + " and boc.id_empresa = " + idEmpresa.ToString() + " and boc.id_taller = " + idTaller.ToString() + " order by boc.id_observacion desc),0)+1),'" + observacion + "','" + fechaRetorno + "','" + horaRetorno + "'," + idUsuario.ToString() + ")";
        resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
}