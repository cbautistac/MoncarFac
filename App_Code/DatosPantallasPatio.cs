using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de DatosPantallasPatio
/// </summary>
public class DatosPantallasPatio
{
    string sql;
    Ejecuciones ejecuta = new Ejecuciones();    
    public DatosPantallasPatio()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string obtieneDescGOP(int gop)
    {
        sql = "select ltrim(rtrim(descripcion_go))as descripcion_go from grupo_operacion where id_grupo_op=" + gop.ToString();
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneNombresEmpleados(string noOrden, string idEmpresa, string idTaller, int idGops)
    {
        sql = "declare @nomEmpleados varchar(500)" +
              " select @nomEmpleados = coalesce(@nomEmpleados + '/', '') + ltrim(rtrim(e.Nombres)) + ' ' + ltrim(rtrim(e.Apellido_Paterno)) + ' ' + ltrim(rtrim(e.Apellido_Materno))" +
              " from operativos_orden oo" +
              " inner join empleados e on e.idEmp = oo.idEmp" +
              " where oo.estatus!='T' and oo.no_orden = " + noOrden + " and oo.id_empresa = " + idEmpresa + " and oo.id_taller = " + idTaller + " and oo.idGops like '%" + idGops + "%'" +
              " select @nomEmpleados";
        return ejecuta.scalarToStringSimple(sql);
    }

    public object obtieneCalificaciones()
    {
        sql = "select count(*) from calificacion where id_calificacion!=0";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneIdEmps(string noOrden, string idEmpresa, string idTaller, int idGops)
    {
        sql = "declare @idEmps varchar(500)" +
              " select @idEmps = (coalesce(@idEmps + ';', '') + cast(idemp as varchar))" +
              " from operativos_orden" +
              " where no_orden = " + noOrden + " and id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and idGops like '%" + idGops + "%'" +
              " select @idEmps";
        return ejecuta.scalarToStringSimple(sql);
    }

    public DataSet obtieneTotalCalEmp(string noOrden, string idEmpresa, string idTaller, int idEmp)
    {
        DataSet data = null;
        try
        {
            sql = "select id_calificacion as calificacion,count(*)as calificaciones from operativos_orden " +
                  " where no_orden = " + noOrden + " and id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and idemp =" + idEmp + " and id_calificacion !=0" +
                  " group by id_calificacion order by id_calificacion asc";
            object[] ejecutado = ejecuta.dataSet(sql);
            if ((bool)ejecutado[0])
                data = (DataSet)ejecutado[1];
        }
        catch (Exception)
        {
            data = null;
        }
        return data;
    }
}