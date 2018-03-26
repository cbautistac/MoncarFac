using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de ManoObraOrden
/// </summary>
public class ManoObraOrden
{
    string sql;
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    object[] ejecutado = new object[2];
    public ManoObraOrden()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string obtieneNombreEmpleadoRH(int idEmpleado)
    {
        sql = "select (rtrim(ltrim(Nombres))+' '+rtrim(ltrim(Apellido_Paterno))+' '+isnull(rtrim(ltrim(Apellido_Materno)),'')) as nombre from Empleados where IdEmp=" + idEmpleado.ToString();
        return ejecuta.scalarToStringSimple(sql);
    }

    public int obtieneAsignaciones(int idEmp)
    {
        sql = "select count(*) from Operativos_Orden_MO where IdEmp=" + idEmp.ToString() + " and estatus='I' or estatus='A' and IdEmp=" + idEmp.ToString();
        ejecutado = ejecuta.scalarToInt(sql);
        if ((bool)ejecutado[0])
            return Convert.ToInt32(ejecutado[1]);
        else
            return 0;
    }
    
    public bool asignaOperadorMO(int noOrden, int idEmpresa, int idTaller, int idEmp, int idConsecutivoMO)
    {
        sql = "INSERT INTO Operativos_Orden" +
                "(no_orden,id_empresa,id_taller,IdEmp,id_asignacion" +
                " ,id_consecutivo_mo,estatus) " +
                " VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + idEmp.ToString() + ", " +
                " (select isnull((select top 1 oo.id_asignacion from Operativos_Orden_MO oo" +
                " where oo.no_orden = " + noOrden.ToString() + " and oo.id_empresa = " + idEmpresa.ToString() + " and oo.id_taller = " + idTaller.ToString() + " and oo.IdEmp = " + idEmp.ToString() +
                " order by oo.id_asignacion desc),0)+1), " +
                idConsecutivoMO.ToString() + ",'A')";
        ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }

    public bool existeNoOrden(int noOrden, int idTaller, int idEmpresa)
    {
        sql = "select count(*) from Ordenes_Reparacion where no_orden =" + noOrden.ToString() + " and id_empresa =" + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString();
        ejecutado = ejecuta.scalarToInt(sql);
        if ((bool)ejecutado[0])
        {
            if (Convert.ToInt32(ejecutado[1]) == 1)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public bool operadorMOAsignado(int noOrden, int idEmpresa, int idTaller, int idConsecutivoMO)
    {
        sql = "select count(*) from Operativos_Orden where no_orden =" + noOrden.ToString() + " and id_empresa =" + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() + " and id_consecutivo_mo =" + idConsecutivoMO.ToString();
        ejecutado = ejecuta.scalarToInt(sql);
        if ((bool)ejecutado[0])
        {
            if (Convert.ToInt32(ejecutado[1]) > 0)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public bool actualizaOperadorMO(int noOrden, int idEmpresa, int idTaller, int idEmp, int idConsecutivoMO, int idAsignacion)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        sql = "update Operativos_Orden_MO set fecha_fin='" + fechaRetorno + "', hora_fin='" + horaRetorno + "',estatus='T'" +
                " where no_orden=" + noOrden.ToString() + " and IdEmp=" + idEmp.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and id_asignacion=" + idAsignacion.ToString() + " and id_consecutivo_mo=" + idConsecutivoMO.ToString();                
        ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
        {
            sql=" INSERT INTO Operativos_Orden_MO" +
                " (no_orden,id_empresa,id_taller,IdEmp,id_asignacion" +
                " ,id_consecutivo_mo,estatus,fecha_ini,hora_ini) " +
                " VALUES(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + ", " + idEmp.ToString() + ", " +
                " (select isnull((select top 1 oo.id_asignacion from Operativos_Orden_MO oo" +
                " where oo.no_orden = " + noOrden.ToString() + " and oo.id_empresa = " + idEmpresa.ToString() + " and oo.id_taller = " + idTaller.ToString() + " and oo.IdEmp = " + idEmp.ToString() +
                " order by oo.id_asignacion desc),0)+1), " +
                idConsecutivoMO.ToString() + ",'I','" + fechaRetorno + "','" + horaRetorno + "')";
            ejecutado = ejecuta.insertUpdateDelete(sql);
            if ((bool)ejecutado[0])
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public int obtieneIdEmpActual(int noOrden, int idEmpresa, int idTaller, int idConsecutivoMO)
    {
        sql = "select IdEmp from Operativos_Orden_MO where no_orden =" + noOrden.ToString() + " and id_empresa =" + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() + " and id_consecutivo_mo =" + idConsecutivoMO.ToString() + " and estatus = 'A'";
        ejecutado = ejecuta.scalarToInt(sql);
        if ((bool)ejecutado[0])
            return Convert.ToInt32(ejecutado[1]);
        else
            return 0;
    }

    public bool actualizaTiemposOperadorMO(int noOrden, int idTaller, int idEmpresa, int idEmp, int idSignacionMO, string tiempo, object fechaE, char estatus, string usuario, string observaciones, decimal monto)
    {
        string setTiempo = "";
        if (fechaE != null)
        {
            fechas.fecha = Convert.ToDateTime(fechaE);
            fechas.tipoFormato = 4;
            string fechaRetorno = fechas.obtieneFechaConFormato();
            fechas.tipoFormato = 6;
            string horaRetorno = fechas.obtieneFechaConFormato();
            
            if (tiempo == "Ini")
                setTiempo = "fecha_ini='" + fechaRetorno + "', hora_ini='" + horaRetorno + "', estatus='" + estatus + "',";
            else if (tiempo == "Fin")
                setTiempo = "fecha_fin='" + fechaRetorno + "', hora_fin='" + horaRetorno + "', estatus='" + estatus + "',";
            
        }
        else
        {
            if (tiempo == "Ini")
                setTiempo = "fecha_ini=null, hora_ini=null, estatus='A', ";
            else if (tiempo == "Fin")
                setTiempo = "fecha_fin=null, hora_fin=null, estatus='I', ";
        }

        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetornoD = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetornoD = fechas.obtieneFechaConFormato();

        sql = "update Operativos_Orden set " + setTiempo + " fecha_ult_modifica='" + fechaRetornoD + "',hora_ult_modifica='" + horaRetornoD + "',id_usuario_ult_modifi=" + usuario + ",oservaciones='" + observaciones + "', monto=" + monto.ToString() +
              "  where no_orden =" + noOrden.ToString() + " and id_empresa =" + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() + " and id_asignacion =" + idSignacionMO.ToString() + " and idEmp=" + idEmp.ToString();
        ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }

    internal object[] obtieneManoObraCc(int empresa, int taller, int ordenT)
    {
        string sql = string.Format("select e.IdEmp,oo.id_asignacion,ltrim(rtrim(ltrim(rtrim(e.Nombres))+' '+ltrim(rtrim(isnull(e.Apellido_Paterno,'')))+' '+ltrim(rtrim(isnull(e.Apellido_Materno,''))))) as nombre,p.descripcion as puesto,oo.estatus,oo.fecha_asignacion,oo.fecha_ini_prog,oo.fecha_ini,oo.fecha_fin,oo.monto as montoAutorizado," +
                    "(select isnull(sum(total), 0) from Registro_Pinturas r where r.id_empresa = oo.id_empresa and r.id_taller = oo.id_taller and r.no_orden = oo.no_orden and r.idEmp = oo.IdEmp and r.ano = {3}) as montoUsuado," +
                    "oo.monto - ((select isnull(sum(total), 0) from Registro_Pinturas r where r.id_empresa = oo.id_empresa and r.id_taller = oo.id_taller and r.no_orden = oo.no_orden and r.idEmp = oo.IdEmp and r.ano = {3})) as pagar,oo.pagado,oo.fechaPago from Operativos_Orden oo inner join empleados e on e.IdEmp = oo.IdEmp " +
                    "left join Puestos p on p.id_puesto = e.Puesto where oo.id_empresa = {0} and oo.id_taller = {1} and oo.no_orden = {2}", empresa, taller, ordenT, new E_Utilities.Fechas().obtieneFechaLocal().Year);
        return ejecuta.dataSet(sql);
    }

    public int pichoneraEmpleado(int idEmp)
    {
        sql = "select clv_pichonera from empleados where IdEmp="+idEmp;
        ejecutado = ejecuta.scalarToInt(sql);
        if ((bool)ejecutado[0])
            return Convert.ToInt32(ejecutado[1]);
        else
            return 0;
    }

    public int obtieneOperariosTerminado(int noOrden, int idTaller, int idEmpresa, int idMO)
    {
        try
        {
            sql = "select (select count(*) from Operativos_Orden_MO oom" +
                  " where oom.no_orden = " + noOrden.ToString() + " and oom.id_empresa = " + idEmpresa.ToString() + " and oom.id_taller = " + idTaller.ToString() + " and oom.id_consecutivo_mo =" + idMO.ToString() +
                  " )-(select count(*) from Operativos_Orden_MO oo" +
                  " where oo.no_orden = " + noOrden.ToString() + " and oo.id_empresa = " + idEmpresa.ToString() + " and oo.id_taller = " + idTaller.ToString() + " and oo.id_consecutivo_mo =" + idMO.ToString() +
                  " and oo.estatus = 'T')";
            ejecutado = ejecuta.scalarToInt(sql);
            if ((bool)ejecutado[0])
                return Convert.ToInt32(ejecutado[1]);
            else
                return -1;
        }
        catch (Exception) { return -1; }
    }

    public int obtieneOperariosMo(int noOrden, int idTaller, int idEmpresa, int idMO)
    {
        try
        {
            sql = "select count(*) from Operativos_Orden_MO oom where oom.no_orden = " + noOrden.ToString() + " and oom.id_empresa = " + idEmpresa.ToString() + " and oom.id_taller = " + idTaller.ToString() + " and oom.id_consecutivo_mo =" + idMO.ToString();
            ejecutado = ejecuta.scalarToInt(sql);
            if ((bool)ejecutado[0])
                return Convert.ToInt32(ejecutado[1]);
            else
                return -1;
        }
        catch (Exception) { return -1; }
    }

    public object[] asignaOperario(int usuario, int empresa, int taller, int orden, string idEmp, DateTime fecha, decimal monto, string idGops)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        sql = "INSERT INTO Operativos_Orden(no_orden,id_empresa,id_taller,IdEmp,id_asignacion,fecha_asignacion,hora_asignacion,fecha_ini_prog,hora_ini_prog,fecha_ult_modifica,hora_ult_modifica,id_usuario_asignacion,id_usuario_ult_modifi,estatus, monto,idGops) " +
                " VALUES(" + orden.ToString() + ", " + empresa.ToString() + ", " + taller.ToString() + ", " + idEmp.ToString() + ", " +
                " (select isnull((select top 1 oo.id_asignacion from Operativos_Orden oo" +
                " where oo.no_orden = " + orden.ToString() + " and oo.id_empresa = " + empresa.ToString() + " and oo.id_taller = " + taller.ToString() + " and oo.IdEmp = " + idEmp.ToString() +
                " order by oo.id_asignacion desc),0)+1),'" + fechaRetorno + "','" + horaRetorno + "','" + fecha.ToString("yyyy-MM-dd") + "','" + fecha.ToString("HH:mm:ss") + "','" + fechaRetorno + "','" + horaRetorno + "'," + usuario.ToString() + "," + usuario.ToString() + ",'A'," + monto.ToString() + ",'" + idGops + "')";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] existeAsignadoAorden(int empresa, int taller, int orden, string idEmp)
    {
        sql = "select count(*) from operativos_orden where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " and idemp=" + idEmp + " and (fecha_fin is null or Convert(char(10),fecha_fin,126)='' or Convert(char(10),fecha_fin,126)='1900-01-01' or estatus<>'T')";
        return ejecuta.scalarToBool(sql);
    }

    public object[] actualizaOperario(int usuario, int empresa, int taller, int orden, string id, DateTime fechaIni)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        sql = "update operativos_orden set fecha_ini_prog='" + fechaIni.ToString("yyyy-MM-dd") + "',hora_ini_prog='" + fechaIni.ToString("HH:mm:ss") + "',fecha_ult_modifica='" + fechaRetorno + "',hora_ult_modifica='" + horaRetorno + "',id_usuario_ult_modifi=" + usuario.ToString() + " where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " and id_asignacion=" + id;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizarTiempoEstimadoEntrega(int noOrden, int idEmpresa, int idTaller, string fechaPromesa, string horaPromesa)
    {
        sql = "select convert(char(10),f_asignacion,126) as fecha from seguimiento_orden where id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and no_orden = " + noOrden.ToString();
        object[] fechaAsig = ejecuta.dataSet(sql);
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        
        string condicion = " ";
        try
        {
            if (Convert.ToBoolean(fechaAsig[0]))
            {
                DataSet info = (DataSet)fechaAsig[1];
                foreach (DataRow row in info.Tables[0].Rows)
                {
                    if (row[0].ToString() == "" || row[0] == null || row[0].ToString() == "1900-01-01")
                        condicion = ", f_asignacion='" + fechaRetorno + "' ";
                    else
                        condicion = " ";
                }
            }
        }
        catch (Exception) { }


        sql = "update seguimiento_orden set f_entrega_estimada='" + fechaPromesa + "', h_estrega_estimada='" + horaPromesa +"' "+ condicion +
              " where id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and no_orden = " + noOrden.ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public string obtieneFechaPromesa(int noOrden, int idTaller, int idEmpresa)
    {
        sql = "select ltrim(rtrim((isnull((Convert(char(10),f_entrega_estimada,126)),''))))+';'+ltrim(rtrim((isnull((Convert(char(8),h_estrega_estimada,108)),'')))) from seguimiento_orden" +
              " where id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and no_orden = " + noOrden.ToString();
        return ejecuta.scalarToStringSimple(sql);
    }
}