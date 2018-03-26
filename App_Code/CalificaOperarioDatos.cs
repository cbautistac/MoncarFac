using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de CalificaOperarioDatos
/// </summary>
public class CalificaOperarioDatos
{
    Ejecuciones ejecutar = new Ejecuciones();
    Fechas fechas = new Fechas();
    string sql;
    object[] resultados = new object[2];
    bool respuesta;
    public CalificaOperarioDatos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool actualizaCalificacion(int noOrden, int idEmpresa, int idTaller, int idGops, int calificacion)
    {
        sql = "update Operativos_Orden set id_calificacion=" + calificacion.ToString() +
              " where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and idgops like '%"+idGops+"%'";
        resultados = ejecutar.insertUpdateDelete(sql);
        if ((bool)resultados[0])
            return Convert.ToBoolean(resultados[1]);
        else
            return false;
    }

    public object[] gruposOpExistentes(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "select distinct mo.id_grupo_op,upper(g.descripcion_go) as descripcion,"
        + "isnull((select s.p25 from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as p25,"
        + "isnull((select s.p50 from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as p50,"
        + "isnull((select s.p75 from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as p75,"
        + "isnull((select s.p100 from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as p100,"
        + "isnull((select s.terminado from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as VoBo "
        + "from Mano_Obra mo "
        + "inner join Grupo_Operacion g on g.id_grupo_op=mo.id_grupo_op "
        + "where mo.id_empresa=" + idEmpresa.ToString() + " and mo.id_taller=" + idTaller.ToString() + "and mo.id_grupo_op in (1,2,3) and mo.no_orden=" + noOrden.ToString();
        return ejecutar.dataSet(sql);
    }

    public object[] existeSeguimientoOperacion(int noOrden, int idEmpresa, int idTaller, int gop)
    {
        sql = "select isnull(p25,0)as p25,isnull(p50,0)as p50,isnull(p75,0)as p75,isnull(p100,0)as p100,isnull(terminado,0)as terminado " +
              "from seguimiento_operacion where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and id_grupo_op in (1,2.3)"; //+ gop.ToString();
        return ejecutar.dataSet(sql);
    }

    public object[] existeSegOp(int noOrden, int idEmpresa, int idTaller, int gop)
    {
        sql = "select count(*) from seguimiento_operacion " +
              "where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and id_grupo_op =" + gop.ToString();
        return ejecutar.scalarToBoolLog(sql);
    }

    public object[] insertaSegOp(int noOrden, int idEmpresa, int idTaller, int gop, bool p25, bool p50, bool p75, bool p100, bool pTer, int idUsuario, string observaciones)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        sql = "insert into seguimiento_operacion values(" + noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + "," + gop.ToString() + "," + pasaInt(p25).ToString() + "," + pasaInt(p50).ToString() + "," + pasaInt(p75).ToString() + "," + pasaInt(p100).ToString() + "," + pasaInt(pTer).ToString() + ") " +
              "insert into bitacora_operacion values(" +
              noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + ",(select isnull((select top 1 bo.id_consecutivo_bit_op from bitacora_operacion bo where bo.no_orden = " + noOrden.ToString() + " and bo.id_empresa = " + idEmpresa.ToString() + " and bo.id_taller = " + idTaller.ToString() + " order by bo.id_consecutivo_bit_op desc),0)+1)," +
              idUsuario.ToString() + ",'" + fechaRetorno + "','" + horaRetorno + "','" + observaciones + "') " +
              "update seguimiento_orden set f_tocado='" + fechaRetorno + "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() +
              " if(" + gop + "=1 or " + gop + "=3) begin if(" + pasaInt(p75).ToString() + "=1) begin " +
              "update operativos_orden set fecha_fin='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',hora_fin='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',estatus='T', fecha_ult_modifica='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',hora_ult_modifica='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',id_usuario_ult_modifi=" + idUsuario + " where id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and no_orden = " + noOrden + " and estatus = 'A' and(select tabla.grupo from(select distinct case when CHARINDEX('-', oo.idgops) = 0 then oo.idgops else substring(oo.idgops, 1, CHARINDEX('-', oo.idgops) - 1) end as grupo from operativos_orden oo where oo.no_orden = " + noOrden + " and oo.id_empresa = " + idEmpresa + " and oo.id_taller = " + idTaller + ") as tabla where tabla.grupo = " + gop + ") = idgops and idgops = " + gop + " end end " +
              "else begin if(" + pasaInt(p100).ToString() + "=1) begin update operativos_orden set fecha_fin='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',hora_fin='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',estatus='T', fecha_ult_modifica='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',hora_ult_modifica='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',id_usuario_ult_modifi=" + idUsuario + " where id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and no_orden = " + noOrden + " and estatus = 'A' and(select tabla.grupo from(select distinct case when CHARINDEX('-', oo.idgops) = 0 then oo.idgops else substring(oo.idgops, 1, CHARINDEX('-', oo.idgops) - 1) end as grupo from operativos_orden oo where oo.no_orden = " + noOrden + " and oo.id_empresa = " + idEmpresa + " and oo.id_taller = " + idTaller + ") as tabla where tabla.grupo = " + gop + ") = idgops and idgops = " + gop + " end end " +
              "update empleados set clv_pichonera=clv_pichonera+1 where idemp in (select idemp from operativos_orden where id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and no_orden = " + noOrden + " and estatus = 'A' and (select tabla.grupo from(select distinct case when CHARINDEX('-', oo.idgops) = 0 then oo.idgops else substring(oo.idgops, 1, CHARINDEX('-', oo.idgops) - 1) end as grupo from operativos_orden oo where oo.no_orden = " + noOrden + " and oo.id_empresa = " + idEmpresa + " and oo.id_taller = " + idTaller + ") as tabla where tabla.grupo = " + gop + ") = idgops and idgops = " + gop + ")";


        return ejecutar.insertUpdateDelete(sql);
    }

    public object[] actualizaSegOp(int noOrden, int idEmpresa, int idTaller, int gop, bool p25, bool p50, bool p75, bool p100, bool pTer, int idUsuario, string observaciones)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        sql = "update seguimiento_operacion set p25=" + pasaInt(p25).ToString() + ",p50=" + pasaInt(p50).ToString() + ",p75=" + pasaInt(p75).ToString() + ",p100=" + pasaInt(p100).ToString() + ",terminado=" + pasaInt(pTer).ToString() +
              " where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and id_grupo_op =" + gop.ToString() +
              " insert into bitacora_operacion values(" +
              noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + ",(select isnull((select top 1 bo.id_consecutivo_bit_op from bitacora_operacion bo where bo.no_orden = " + noOrden.ToString() + " and bo.id_empresa = " + idEmpresa.ToString() + " and bo.id_taller = " + idTaller.ToString() + " order by bo.id_consecutivo_bit_op desc),0)+1)," +
              idUsuario.ToString() + ",'" + fechaRetorno + "','" + horaRetorno + "','" + observaciones + "') " +
              "update seguimiento_orden set f_tocado='" + fechaRetorno + "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() +
              " if(" + gop + "=1 or " + gop + "=3) begin if(" + pasaInt(p75).ToString() + "=1) begin " +
              "update operativos_orden set fecha_fin='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',hora_fin='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',estatus='T', fecha_ult_modifica='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',hora_ult_modifica='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',id_usuario_ult_modifi=" + idUsuario + " where id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and no_orden = " + noOrden + " and estatus = 'A' and(select tabla.grupo from(select distinct case when CHARINDEX('-', oo.idgops) = 0 then oo.idgops else substring(oo.idgops, 1, CHARINDEX('-', oo.idgops) - 1) end as grupo from operativos_orden oo where oo.no_orden = " + noOrden + " and oo.id_empresa = " + idEmpresa + " and oo.id_taller = " + idTaller + ") as tabla where tabla.grupo = " + gop + ") = case when CHARINDEX('-', idgops) = 0 then idgops else substring(idgops, 1, CHARINDEX('-', idgops) - 1) end and case when CHARINDEX('-', idgops) = 0 then idgops else substring(idgops, 1, CHARINDEX('-', idgops) - 1) end = " + gop +
              " update empleados set clv_pichonera=clv_pichonera+1 where idemp in (select idemp from operativos_orden where id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and no_orden = " + noOrden + " and estatus = 'A' and (select tabla.grupo from(select distinct case when CHARINDEX('-', oo.idgops) = 0 then oo.idgops else substring(oo.idgops, 1, CHARINDEX('-', oo.idgops) - 1) end as grupo from operativos_orden oo where oo.no_orden = " + noOrden + " and oo.id_empresa = " + idEmpresa + " and oo.id_taller = " + idTaller + ") as tabla where tabla.grupo = " + gop + ") = case when CHARINDEX('-', idgops) = 0 then idgops else substring(idgops, 1, CHARINDEX('-', idgops) - 1) end and case when CHARINDEX('-', idgops) = 0 then idgops else substring(idgops, 1, CHARINDEX('-', idgops) - 1) end = " + gop + ") " +
              " end end " +
              "else begin if(" + pasaInt(p100).ToString() + "=1) begin update operativos_orden set fecha_fin='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',hora_fin='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',estatus='T', fecha_ult_modifica='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',hora_ult_modifica='" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',id_usuario_ult_modifi=" + idUsuario + " where id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and no_orden = " + noOrden + " and estatus = 'A' and(select tabla.grupo from(select distinct case when CHARINDEX('-', oo.idgops) = 0 then oo.idgops else substring(oo.idgops, 1, CHARINDEX('-', oo.idgops) - 1) end as grupo from operativos_orden oo where oo.no_orden = " + noOrden + " and oo.id_empresa = " + idEmpresa + " and oo.id_taller = " + idTaller + ") as tabla where tabla.grupo = " + gop + ") = case when CHARINDEX('-', idgops) = 0 then idgops else substring(idgops, 1, CHARINDEX('-', idgops) - 1) end and case when CHARINDEX('-', idgops) = 0 then idgops else substring(idgops, 1, CHARINDEX('-', idgops) - 1) end = " + gop +
              " update empleados set clv_pichonera=clv_pichonera+1 where idemp in (select idemp from operativos_orden where id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and no_orden = " + noOrden + " and estatus = 'A' and (select tabla.grupo from(select distinct case when CHARINDEX('-', oo.idgops) = 0 then oo.idgops else substring(oo.idgops, 1, CHARINDEX('-', oo.idgops) - 1) end as grupo from operativos_orden oo where oo.no_orden = " + noOrden + " and oo.id_empresa = " + idEmpresa + " and oo.id_taller = " + idTaller + ") as tabla where tabla.grupo = " + gop + ") = case when CHARINDEX('-', idgops) = 0 then idgops else substring(idgops, 1, CHARINDEX('-', idgops) - 1) end and case when CHARINDEX('-', idgops) = 0 then idgops else substring(idgops, 1, CHARINDEX('-', idgops) - 1) end = " + gop + ") " +
              " end end ";
        return ejecutar.insertUpdateDelete(sql);
    }

    private int pasaInt(bool boleano)
    {
        if (boleano)
            return 1;
        else
            return 0;
    }

    public string obtieneNombreUsuario(int idUsuario)
    {
        sql = "select ltrim(rtrim(nick))as nick from usuarios where id_usuario=" + idUsuario.ToString();
        return ejecutar.scalarToStringSimple(sql);
    }

    public object[] actualizaCronos(int noOrden, int idEmpresa, int idTaller, string fechaTocado, string fechaTerminado)
    {
        sql = "update seguimiento_orden set f_tocado='" + fechaTocado + "', f_terminado='" + fechaTerminado + "' where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
        return ejecutar.insertUpdateDelete(sql);
    }

    public object[] insertaBitVistaPatio(int noOrden, int idEmpresa, int idTaller, int idGop, int idUsuario, string fecha, string hora)
    {
        sql = "insert into bitacora_vista_patio values(" + noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + ",(select isnull((select top 1 b.consecutivo_bit_patio from bitacora_vista_patio b where b.no_orden=" + noOrden.ToString() + " and b.id_empresa=" + idEmpresa.ToString() + " and b.id_taller=" + idTaller.ToString() + " order by b.consecutivo_bit_patio desc),0)+1)," + idGop.ToString() + "," + idUsuario.ToString() + ",'" + fecha + "','" + hora + "')";
        return ejecutar.insertUpdateDelete(sql);
    }

    public string obtieneCalificacionGO(string idGop, string noOrden, string idEmpresa, string idTaller)
    {
        sql = "select id_calificacion from operativos_orden where idgops like '%"+idGop+"%' and id_empresa="+idEmpresa+" and id_taller="+idTaller+" and no_orden="+noOrden;
        return ejecutar.scalarToStringSimple(sql);
    }

    public object[] existeOperarioasignado(string empresa, string taller, string orden, string grupo)
    {
        sql = "select  count(*) from operativos_orden where  id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden + " and charindex('" + grupo + "',idgops)>0";
        return ejecutar.scalarToBool(sql);
    }

    
}