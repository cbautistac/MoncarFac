using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de DatosUtilerias
/// </summary>
public class DatosUtilerias
{
    Ejecuciones ejecuta = new Ejecuciones();
    public DatosUtilerias()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool actualizaPlaca(int noOrden, int idEmpresa, int idTaller, string placa)
    {
        bool actualizado = false;
        string sql = "update Ordenes_Reparacion set placas='" + placa + "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa + " and id_taller=" + idTaller.ToString();
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        bool existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            actualizado = Convert.ToBoolean(ejecutado[1]);
        else
            actualizado = false;
        return actualizado;
    }

    public bool existePlaca(int noOrden, int idEmpresa, int idTaller)
    {
        bool existe = false;
        string sql = "select count(*) from Ordenes_Reparacion where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa + " and id_taller=" + idTaller.ToString();
        object[] ejecutado = ejecuta.scalarToBool(sql);
        existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            existe = Convert.ToBoolean(ejecutado[1]);
        else
            existe = false;
        return existe;
    }

    public string obtienePlaca(int noOrden, int idEmpresa, int idTaller)
    {
        string regreso = "";
        string sql = "select placas from Ordenes_Reparacion where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa + " and id_taller=" + idTaller.ToString();
        regreso = ejecuta.scalarToStringSimple(sql);
        return regreso;
    }

    public bool existePlacaVehiculo(string placa)
    {
        bool existe = false;
        string sql = "select count(*) from Vehiculos where placas='" + placa + "'";
        object[] ejecutado = ejecuta.scalarToBool(sql);
        existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            existe = Convert.ToBoolean(ejecutado[1]);
        else
            existe = false;
        return existe;
    }

    public int obtieneOrdenesRelacionas(int idEmpresa, int idTaller, string placas)
    {
        int relaciones = 0;
        string sql = "select count(*) from Ordenes_Reparacion where placas='" + placas + "' and id_empresa=" + idEmpresa + " and id_taller=" + idTaller.ToString();
        object[] ejecutado = ejecuta.scalarToInt(sql);        
        if (Convert.ToBoolean(ejecutado[0]))
            relaciones = Convert.ToInt32(ejecutado[1]);
        else
            relaciones = 2;
        return relaciones;
    }

    public bool actualizaPlacaVehiculo(int marca, int vehiculo, int unidad, int id, string placa)
    {
        bool actualizado = false;
        string sql = "update vehiculos set placas='" + placa + "' where id_marca=" + marca.ToString() + " and id_tipo_vehiculo=" + vehiculo.ToString() + " and id_tipo_unidad=" + unidad.ToString() + " and id_vehiculo=" + id.ToString();
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        bool existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            actualizado = Convert.ToBoolean(ejecutado[1]);
        else
            actualizado = false;
        return actualizado;
    }
}