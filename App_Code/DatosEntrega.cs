using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de DatosEntrega
/// </summary>
public class DatosEntrega
{
    string sql;
    object[] ejecutado = new object[2];
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fecha = new Fechas();

    public DatosEntrega()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public object[] obtieneVistoBueno(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "select isnull(" +
              " (select sum(cast(terminado as int))from seguimiento_operacion" +
              " where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() +
              " group by no_orden, id_empresa, id_taller) - " +
              " (select sum(cast(terminado as int))from seguimiento_operacion" +
              " where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() + " and terminado = 1" +
              " group by terminado, no_orden,id_empresa,id_taller),1)as termiandos";
        return ejecuta.scalarToInt(sql);
    }

    public object[] existeRegClienteEntrega(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "select count(*) from cliente_entrega where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString();
        return ejecuta.scalarToBoolLog(sql);
    }

    public string obtieneInfoClienteEntrega(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "select isnull(nom_cliente,'')+';'+" +
              "isnull(convert(varchar, fecha_entrada, 126), '') + ';' +" +
              "isnull(convert(varchar, hora_entrada, 108), '') + ';' +" +
              "isnull(convert(varchar, fecha_salida, 126), '') + ';' + isnull(convert(varchar, hora_salida, 108), '') + ';' +" +
              "isnull(convert(varchar(10), f_terminacion, 126), '')+ ';' + isnull(convert(varchar, h_terminacion, 108), '') " +
              "from cliente_entrega AS ce INNER JOIN seguimiento_orden AS so ON so.no_orden = ce.no_orden and so.id_empresa = ce.id_empresa and so.id_taller = ce.id_taller " +
              "where ce.no_orden = " + noOrden.ToString() + " and ce.id_empresa = " + idEmpresa.ToString() + " and ce.id_taller =" + idTaller.ToString() +
              " and ce.id_registro=(select top 1 id_registro from cliente_entrega where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + "  ORDER BY id_registro DESC)";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneFechaBajaPortal(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "select case convert(varchar,isnull(f_baja_portal,''),120) when '1900-01-01' then'' else  convert(varchar,isnull(f_baja_portal,''),120) end from seguimiento_orden where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() +
              " and id_registro=(select top 1 so.id_registro from seguimiento_orden so where so.no_orden = " + noOrden.ToString() + " and so.id_empresa = " + idEmpresa.ToString() + " and so.id_taller = " + idTaller.ToString() + ")";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneNombreUsuarioLog(int idUsuario)
    {
        sql = "select nombre_usuario from usuarios where id_usuario=" + idUsuario.ToString();
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] guardaIngresoCliente(int noOrden, int idEmpresa, int idTaller, string nom_cliente, string fecha_entrada, string hora_entrada, string fecha_salida, string hora_salida, string fecha_baja, string fecha_entrega, string hr_entrega, string localizacion, string perfil)
    {
        sql = "insert into cliente_entrega values(" +
              noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + "," +
              "(select isnull((select top 1 c.id_registro from cliente_entrega c where c.no_orden = " + noOrden.ToString() + " and c.id_empresa =" + idEmpresa.ToString() + " and c.id_taller =" + idTaller.ToString() + " order by c.id_registro desc),0)+1)," +
              "'" + nom_cliente + "','" + fecha_entrada + "','" + hora_entrada + "','" + fecha_salida + "','" + hora_salida + "')";
        sql += " " +
               " update seguimiento_orden set f_terminacion='" + fecha_entrega + "',h_terminacion='" + hr_entrega + "', f_baja_portal='" + fecha_baja + "' where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() +
               " and id_registro=(select top 1 so.id_registro from seguimiento_orden so where so.no_orden = " + noOrden.ToString() + " and so.id_empresa = " + idEmpresa.ToString() + " and so.id_taller = " + idTaller.ToString() + ") " +
               " update ordenes_reparacion set id_localizacion=" + localizacion + ", id_perfilorden=" + perfil + " where no_orden=" + noOrden + " and id_empresa=" + idEmpresa + " and id_taller=" + idTaller;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaIngresoCliente(int noOrden, int idEmpresa, int idTaller, string nom_cliente, string fecha_entrada, string hora_entrada, string fecha_salida, string hora_salida, string fecha_baja, string fecha_entrega, string hr_entrega, string localizacion, string perfil)
    {
        sql = "update cliente_entrega set nom_cliente='" + nom_cliente + "',fecha_entrada='" + fecha_entrada + "',hora_entrada='" + hora_entrada + "',fecha_salida='" + fecha_salida + "',hora_salida='" + hora_salida + "'" +
              " where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() +
              " and id_registro=(select top 1 so.id_registro from seguimiento_orden so where so.no_orden = " + noOrden.ToString() + " and so.id_empresa = " + idEmpresa.ToString() + " and so.id_taller = " + idTaller.ToString() + ")";
        sql += " " +
               " update seguimiento_orden set f_terminacion='"+fecha_entrega+"',h_terminacion='"+hr_entrega+"', f_baja_portal='" + fecha_baja + "' where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() +
               " and id_registro=(select top 1 so.id_registro from seguimiento_orden so where so.no_orden = " + noOrden.ToString() + " and so.id_empresa = " + idEmpresa.ToString() + " and so.id_taller = " + idTaller.ToString() + ") "+
               " update ordenes_reparacion set id_localizacion=" + localizacion + ", id_perfilorden=" + perfil + " where no_orden=" + noOrden + " and id_empresa=" + idEmpresa + " and id_taller=" + idTaller;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] terminaOrden(int noOrden, int idEmpresa, int idTaller)
    {
        //falta terminar 
        sql = "";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] registraOrdenesEntrega(int noOrden, int idEmpresa, int idTaller, int idUsuario, int idUsuarioAut)
    {
        sql = "insert into ordenes_entregadas values(" + noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + "," +
              "(select isnull((select top 1 so.id_orden_entrega from ordenes_entregadas so where " +
              "so.no_orden =" + noOrden.ToString() + " and so.id_empresa =" + idEmpresa.ToString() + " and so.id_taller =" + idTaller.ToString() + " order by so.id_orden_entrega desc), 0) + 1)" +
              ",1," + idUsuario + "," + idUsuarioAut + ",'" + fecha.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fecha.obtieneFechaLocal().ToString("HH:mm:ss") + "')";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object obtieneIdUsuarioAut(string usuarioLog)
    {
        sql = "select id_usuario from usuarios where nick='"+ usuarioLog + "' and estatus='A'";
        object[] proceso = ejecuta.scalarToInt(sql);
        if ((bool)proceso[0])
            return Convert.ToInt32(proceso[1]);
        else
            return 0;
    }








    public string obtieneInconformidad(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "select descripcion from inconformidades where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and id_inconformidad=(" +
              "select top 1 i.id_inconformidad from inconformidades i where i.no_orden =" + noOrden.ToString() + " and i.id_empresa = " + idEmpresa.ToString() + " and i.id_taller = " + idTaller.ToString() + " order by i.id_inconformidad desc)";
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] existeParaCronologico(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "select count(*) from ordenes_entregadas where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString();
        return ejecuta.scalarToInt(sql);
    }
}