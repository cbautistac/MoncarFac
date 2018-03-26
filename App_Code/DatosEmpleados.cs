using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Descripción breve de DatosEmpleados
/// </summary>
public class DatosEmpleados
{
    Ejecuciones ejecuta = new Ejecuciones();
    string sql;
    public DatosEmpleados()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool agregaNuevoEmpleado(string nombre, string paterno, string materno, string puesto, string pichonera, string tipo)
    {
        sql = "insert into empleados(idEmp,nombres,Apellido_Paterno,Apellido_Materno,clv_pichonera,puesto,status_empleado,tipo_empleado) " +
              " values((select isnull((select top 1 emp.idEmp from empleados emp order by emp.IdEmp desc), 0) + 1),'" + nombre + "','" + paterno + "','" + materno + "'," + pichonera + "," + puesto + ",'N','" + tipo + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }

    public bool actualizaEmpleado(int idEmp, string nombre, string paterno, string materno, string puesto, string pichonera, string tipo)
    {
        sql = "update empleados set nombres='" + nombre + "', Apellido_Paterno='" + paterno + "', Apellido_Materno='" + materno + "', clv_pichonera=" + pichonera + ", puesto=" + puesto + ",tipo_empleado='" + tipo + "' where IdEmp=" + idEmp.ToString();
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }

    public object[] obtieneMontoAutorizado(int[] sesiones, string empleado)
    {
        sql = "select O.IdEmp,UPPER(LTRIM(RTRIM(E.Nombres))+' '+LTRIM(RTRIM(ISNULL(E.Apellido_Paterno,'')))+' '+LTRIM(RTRIM(ISNULL(E.Apellido_Materno,'')))) AS nombre,o.monto,e.Tipo_Empleado "
+ "from Operativos_Orden O "
+ "INNER JOIN Empleados E ON E.IdEmp=O.IdEmp "
+ "where O.no_orden=" + sesiones[4].ToString() + " and O.id_empresa=" + sesiones[2].ToString() + " and O.id_taller=" + sesiones[3].ToString() + " and O.estatus<>'T' and Tipo_Empleado<>'AD' and o.IdEmp=1";
        return ejecuta.dataSet(sql);
    }

    public bool tieneRelacionEmpleado(string empleado)
    {
        sql = "select count(*) from operativos_orden where idemp=" + empleado;
        object[] ejecutado = ejecuta.scalarToBool(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return true;
    }

    public bool eliminaEmpleado(int idEmp)
    {
        sql = "delete from empleados where idemp=" + idEmp.ToString();
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }
}