using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Recepciones
/// </summary>
public class Recepciones
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
	public Recepciones()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] existePlaca(string placa)
    {
        string sql = "select count(*) from Vehiculos where upper(placas)='" + placa.ToUpper() + "'";
        return ejecuta.scalarToBool(sql);
    }

    public object[] existeOrdenesPrevias(string placa, int empresa, int taller)
    {
        string sql = "select count(*) from Ordenes_Reparacion ord where ord.placas='" + placa.ToUpper() + "' and ord.id_empresa=" + empresa.ToString() + " and ord.id_taller=" + taller.ToString();
        return ejecuta.scalarToBool(sql);        
    }

    public object[] obtieneInfoVehiculo(string placa)
    {
        string sql = "select id_marca,id_tipo_vehiculo,id_tipo_unidad,id_vehiculo,modelo,color_ext from Vehiculos where placas='" + placa.ToUpper() + "'";
        return ejecuta.dataSet(sql); 
    }

    public object[] agregaVehiculo(string marca, string tVehiculo, string tUnidad, string modelo, string placa, string color)
    {
        object[] retorno = new object[2];
        string sql = "select top 1 id_vehiculo from Vehiculos where id_marca=" + marca + " and id_tipo_vehiculo=" + tVehiculo + " and id_tipo_unidad=" + tUnidad+" order by id_vehiculo desc";
        object[] ultimoVehiculo = ejecuta.scalarToInt(sql);
        if (Convert.ToBoolean(ultimoVehiculo[0]))
        {
            int vehiculoNuevo = Convert.ToInt32(ultimoVehiculo[1].ToString())+1;
            sql = "insert into Vehiculos(id_marca,id_tipo_vehiculo,id_tipo_unidad,id_vehiculo,modelo,placas,color_ext) values(" + marca + "," + tVehiculo + "," + tUnidad + "," + vehiculoNuevo.ToString() + "," + modelo + ",'" + placa.ToUpper() + "','" + color + "')";
            object[] insertado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(insertado[0])) {
                retorno[0] = true;
                object[] datosVehi = new object[4];
                datosVehi[0] = marca;
                datosVehi[1] = tVehiculo;
                datosVehi[2] = tUnidad;
                datosVehi[3] = vehiculoNuevo;
                retorno[1] = datosVehi;
            }
            else
                retorno = insertado;
        }
        else {
            retorno[0] = false;
            retorno[1] = ultimoVehiculo[1];
        }
        return retorno; 
    }

    public object[] actualizaDatosBasicosVehiculo(int[] datosVehiculo, string color)
    {
        string sql = "update Vehiculos set color_ext='" + color + "' where id_marca=" + Convert.ToInt32(datosVehiculo[0].ToString()).ToString() + " and id_tipo_vehiculo=" + Convert.ToInt32(datosVehiculo[1].ToString()).ToString() + " and id_tipo_unidad=" + Convert.ToInt32(datosVehiculo[2].ToString()).ToString() + " and id_vehiculo= " + Convert.ToInt32(datosVehiculo[3].ToString()).ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneInfoOrdenCC(string empresa, string taller, string orden)
    {
        string sql = "select orp.no_orden,orp.id_empresa,orp.id_taller,t.nombre_taller,tv.descripcion as valuacion,so.f_recepcion as fechaRecepcion,so.f_entrega_estimada,orp.id_cliprov as idCliente,c.razon_social as cliente," +
"orp.id_cliprov_aseguradora as idAseguradora,ca.razon_social as aseguradora," +
"ltrim(rtrim(ltrim(rtrim(isnull(orp.nombre_propietario, ''))) + ' ' + ltrim(rtrim(isnull(orp.ap_paterno_propietario, ''))) + ' ' + ltrim(rtrim(isnull(orp.ap_materno_propietario, ''))))) as asegurado," +
"upper(ltrim(rtrim(m.descripcion)) + ' ' + ltrim(rtrim(tu.descripcion))) as vehiculo,v.modelo,orp.placas,orp.no_siniestro,orp.no_poliza from ordenes_reparacion orp " +
"left join talleres t on t.id_taller = orp.id_taller " +
"left join tipo_valuacion tv on tv.id_tipo_valuacion = orp.id_tipo_valuacion " +
"left join Seguimiento_Orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden " +
"left join cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = 'C' " +
"left join cliprov ca on ca.id_cliprov = orp.id_cliprov_aseguradora and ca.tipo = 'C' and ca.aseguradora = 1 " +
"left join marcas m on m.id_marca = orp.id_marca " +
"left join tipo_vehiculo tvv on tvv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
"left join tipo_unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad and m.id_marca = orp.id_marca and tvv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
"left join vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"where orp.no_orden = " + orden + " and orp.id_taller = " + taller + " and orp.id_empresa = " + empresa;
        return ejecuta.dataSet(sql);
    }

   
    public object[] obtieneTodoAplicadoSalida(int[] sesiones)
    {
        string sql = string.Format("select (select count(*) from mano_obra mo where mo.no_orden={2} and mo.id_empresa={0} and mo.id_taller={1} and mo.aplica_ss=1 and id_salidasincargo=0)+(select count(*) from Refacciones_Orden where ref_no_orden={2} and ref_id_empresa={0} and ref_id_taller={1} and refestatus<>'CA' and aplica_ss=1 and refestatusSolicitud<>11  and id_salidasincargo=0) as aplicados", sesiones[2].ToString(), sesiones[3].ToString(), sesiones[4].ToString());
        return ejecuta.scalarToInt(sql);
    }

    public object[] actualizaEstatusOrdenIndicado(int idEmpresa, int idTaller, int noOrden, string estatus, string usuario, string usuario_autoriza, string estatus_ini)
    {
        string condicion = " update refacciones_orden set aplica_rem=0,aplica_ss=0,id_remision=0,id_salidasincargo=0 where ref_id_empresa=" + idEmpresa + " and ref_id_taller=" + idTaller + " and ref_no_orden=" + noOrden + " update mano_obra set aplica_rem = 0, aplica_ss = 0, id_remision = 0, id_salidasincargo = 0 where id_empresa = " + idEmpresa + " and id_taller = " + idTaller + " and no_orden = " + noOrden + " ";
        string sql = "update ordenes_reparacion set status_orden='" + estatus + "' where id_empresa=" + idEmpresa + " and id_taller=" + idTaller + " and no_orden=" + noOrden + 
            " insert into Bitacora_estatus_orden values(" + idEmpresa + "," + idTaller + "," + noOrden + ",(select isnull((select top 1 consecutivo from bitacora_estatus_orden where id_empresa=" + idEmpresa + " and id_taller=" + idTaller + " and orden=" + noOrden + " order by consecutivo desc),0))+1,'" + estatus_ini + "','" + estatus + "'," + usuario + ",'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "','" + usuario_autoriza + "') ";
        if (estatus_ini == "S" || estatus_ini == "R")
            sql = sql + condicion;
        return ejecuta.insertUpdateDelete(sql);
    }

    internal object[] obtieneReporteOrdenes2(string idEmpresa, string idTaller, string condicion, string fechaIni, string fechaFin, string fechaACTUAL)
    {
        string sql = string.Format("select orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion as descripcion,upper(v.color_ext) as color_ext,orp.placas,l.descripcion as localizacion,C.razon_social,orp.fase_orden, " +
"convert(char(10),so.f_recepcion,120) as f_recepcion, orp.no_siniestro, v.modelo, po.descripcion as perfil, orp.status_orden, orp.avance_orden, case when so.f_entrega_estimada is null then '' else convert(char(10),so.f_entrega_estimada,120) end as f_entrega_estimada, case when so.f_asignacion is null then '' else convert(char(10),so.f_asignacion,120) end as f_asignacion, case when so.f_terminacion is null then '' else convert(char(10),so.f_terminacion,120) end as f_entrega," +
"isnull(case orp.id_localizacion when 4 then(select datediff(dd,so.f_entrega_estimada, so.f_terminacion )) else (select datediff(dd, so.f_entrega_estimada, '{4}')) end, 0) as días_atraso," +
"case orp.status_orden when 'A' then 'ABIERTA' when 'T' then 'COMPLETADA' when 'F' then 'FACTURADA' WHEN 'C' THEN 'CERRADA' WHEN 'R' THEN 'REMISIONADA' WHEN 'S' THEN 'SALIDA SIN CARGO' ELSE '' END AS estatus " +
"from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller " +
"left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca " +
"left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
"left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
"left join Localizaciones l on l.id_localizacion = orp.id_localizacion " +
"left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
"left join PerfilesOrdenes po on po.id_perfilOrden = orp.id_perfilOrden " +
"where orp.id_empresa = {0}  and orp.id_taller = {1} and so.f_recepcion between '{2}' and '{3}' {5} " +
"order by orp.no_orden, orp.status_orden, orp.id_localizacion, orp.id_perfilorden ", idEmpresa, idTaller, fechaIni, fechaFin,fechaACTUAL,condicion);
        return ejecuta.dataSet(sql);
    }

    public object[] ordenPreviaMismoDia(string placa, int empresa, int taller)
    {
        string sql = "select count(*) from ordenes_reparacion o inner join Seguimiento_Orden s on s.id_empresa = o.id_empresa and s.id_taller = o.id_taller and s.no_orden = o.no_orden where o.id_empresa = " + empresa + " and o.id_taller = " + taller + " and o.placas = '" + placa + "' and s.f_recepcion = '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'";
        return ejecuta.scalarToBool(sql);
    }

    public bool agregaProducto(int usuario, string argumentos)
    {
        string sql = "insert into documentocfditmp_fact select * from documentocfditmp where idusuario=" + usuario + " and idfila=" + argumentos + " delete from documentocfditmp where idusuario=" + usuario + " and idfila=" + argumentos;
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    internal object[] obtieneLocalizacionesOrdenes(string idEmpresa, string idTaller, string condicion, string fechaIni, string fechaFin, string valorEstatus)
    {        
        string sql = string.Format("select distinct orp.id_localizacion,l.descripcion from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Localizaciones l on l.id_localizacion = orp.id_localizacion left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
"left join PerfilesOrdenes po on po.id_perfilOrden = orp.id_perfilOrden where orp.id_empresa = {0}  and orp.id_taller = {1} and so.f_recepcion between '{2}' and '{3}' and orp.status_orden = '{4}' {5} order by orp.id_localizacion ", idEmpresa, idTaller, fechaIni, fechaFin, valorEstatus, condicion);
        return ejecuta.dataSet(sql);
    }

    internal object[] obtieneReporteOrdenes(string idEmpresa, string idTaller, string condicion, string fechaIni, string fechaFin, string valorEstatus, string valorLoc, string valorPefil, string fechaActual, string cliente)
    {
        string valor = "";
        if (cliente != "")
            valor = " and orp.id_cliprov=" + cliente + " ";
        string sql = string.Format("select orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion as descripcion,upper(v.color_ext) as color_ext,orp.placas,l.descripcion as localizacion,C.razon_social,orp.fase_orden, " +
"convert(char(10),so.f_recepcion,120) as f_recepcion, orp.no_siniestro, v.modelo, po.descripcion as perfil, orp.status_orden, orp.avance_orden, case when so.f_entrega_estimada is null then '' else convert(char(10),so.f_entrega_estimada,120) end as f_entrega_estimada, case when so.f_asignacion is null then '' else convert(char(10),so.f_asignacion,120) end as f_asignacion, case when so.f_entrega is null then '' else convert(char(10),so.f_entrega,120) end as f_entrega," +
"isnull(case orp.id_localizacion when 4 then(select datediff(dd, so.f_entrega, '{7}')) else '' end, 0) as días_atraso," +
"case orp.status_orden when 'A' then 'ABIERTA' when 'T' then 'COMPLETADA' when 'F' then 'FACTURADA' WHEN 'C' THEN 'CERRADA' WHEN 'R' THEN 'REMISIONADA' WHEN 'S' THEN 'SALIDA SIN CARGO' ELSE '' END AS estatus " +
"from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller " +
"left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca " +
"left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
"left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
"left join Localizaciones l on l.id_localizacion = orp.id_localizacion " +
"left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
"left join PerfilesOrdenes po on po.id_perfilOrden = orp.id_perfilOrden " +
"where orp.id_empresa = {0}  and orp.id_taller = {1} and so.f_recepcion between '{2}' and '{3}' and orp.status_orden = '{4}' and orp.id_localizacion = {5} and orp.id_perfilorden = {6} {8} " +
"order by orp.status_orden, orp.id_localizacion, orp.id_perfilorden, orp.no_orden ", idEmpresa, idTaller, fechaIni, fechaFin, valorEstatus, valorLoc, valorPefil, fechaActual, valor);
        return ejecuta.dataSet(sql);
    }

    

    internal object[] obtienePefilesOrdenes(string idEmpresa, string idTaller, string condicion, string fechaIni, string fechaFin, string valorEstatus, string valorLoc)
    {
        string sql = string.Format("select distinct orp.id_perfilOrden,po.descripcion from Ordenes_Reparacion orp left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Localizaciones l on l.id_localizacion = orp.id_localizacion left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
"left join PerfilesOrdenes po on po.id_perfilOrden = orp.id_perfilOrden where orp.id_empresa = {0}  and orp.id_taller = {1} and so.f_recepcion between '{2}' and '{3}' and orp.status_orden = '{4}' and orp.id_localizacion = {5} {6} order by orp.id_perfilOrden ", idEmpresa, idTaller, fechaIni, fechaFin, valorEstatus, valorLoc, condicion);
        return ejecuta.dataSet(sql);
    }

    internal object[] obtieneEstatusOrdenes(string idEmpresa, string idTaller, string condicion, string fechaIni, string fechaFin)
    {       
        string sql = string.Format("select distinct orp.status_orden,case orp.status_orden when 'A' then 'ABIERTA' when 'T' then 'COMPLETADA' when 'F' then 'FACTURADA' WHEN 'C' THEN 'CERRADA' WHEN 'R' THEN 'REMISIONADA' WHEN 'S' THEN 'SALIDA SIN CARGO' ELSE '' END AS estatus from Ordenes_Reparacion orp " +
"left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad left join Localizaciones l on l.id_localizacion = orp.id_localizacion " +
"left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' left join PerfilesOrdenes po on po.id_perfilOrden = orp.id_perfilOrden where orp.id_empresa = {0}  and orp.id_taller = {1} and so.f_recepcion between '{2}' and '{3}' {4} order by orp.status_orden ", idEmpresa, idTaller, fechaIni, fechaFin, condicion);
        return ejecuta.dataSet(sql);
    }

    public object[] generaOrdenNueva(object[] datosOrden)
    {
        string sql = "generaOrden";
        return ejecuta.exeStoredOrden(sql, datosOrden);
    }

    public string obtieneNombreEmpresa(string empresa)
    {
        string sql = "select razon_social from empresas where id_empresa=" + empresa;
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneNombreUsuario(string usuario)
    {
        string sql = "select nombre_usuario from usuarios where id_usuario=" + usuario;
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneNombreTaller(string taller)
    {
        string sql = "select nombre_taller from talleres where id_taller=" + taller;
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] obtieneTodoAplicado(int[] sesiones)
    {
        string sql = string.Format("select (select count(*) from mano_obra mo where mo.no_orden={2} and mo.id_empresa={0} and mo.id_taller={1} and mo.aplica_rem=1 and id_remision=0)+(select count(*) from Refacciones_Orden where ref_no_orden={2} and ref_id_empresa={0} and ref_id_taller={1} and refestatus<>'CA' and aplica_rem=1 and refestatusSolicitud<>11  and id_remision=0) as aplicados", sesiones[2].ToString(), sesiones[3].ToString(), sesiones[4].ToString());
        return ejecuta.scalarToInt(sql);
    }

    public object[] obtieneInfoOrden(int orden, int empresa, int taller)
    {
        string sql = "select * from Ordenes_Reparacion where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] actualizaOrden(int empresa, int taller, string noOrden, object[] orden)
    {
        int clienteProblema, flotilla = 0;
        clienteProblema = convierteBooltoInt(orden[10].ToString());
        flotilla = convierteBooltoInt(orden[29].ToString());
        if (orden[34].ToString() == "")
            orden[34] = 0;

        decimal montodeducible = 0, porcDeducible = 0, montoDemerito=0;
        try { montodeducible = Convert.ToDecimal(orden[34].ToString()); } catch (Exception) { montodeducible = 0; }
        try { porcDeducible = Convert.ToDecimal(orden[43].ToString()); } catch (Exception) { porcDeducible = 0; }
        try { montoDemerito = Convert.ToDecimal(orden[44].ToString()); } catch (Exception) { montoDemerito = 0; }

        string sql = "update Ordenes_Reparacion set id_tipo_orden=" + orden[0].ToString() + ", id_cliprov=" + orden[1].ToString() + ", id_tipo_servicio=" + orden[2].ToString() + ", id_tipo_valuacion=" + orden[3].ToString() + ", id_tipo_asegurado=" + orden[4].ToString() + ", no_torre='" + orden[6].ToString() + "',"
                   + "id_med_gas=" + orden[7].ToString() + ",km_actual=" + orden[8].ToString() + ",observaciones='" + orden[9].ToString() + "',cliente_problema=" + clienteProblema + ",id_cat_cliente=" + orden[11].ToString() + ",nombre_propietario='" + orden[12].ToString() + "', ap_paterno_propietario='" + orden[13].ToString() + "',"
                   + "ap_materno_propietario='" + orden[14].ToString() + "',calle_propietario='" + orden[15].ToString() + "', num_ext_propietario='" + orden[16].ToString() + "',num_int_propietario='" + orden[17].ToString() + "',colonia_propietario='" + orden[18].ToString() + "',municipio_propietario='" + orden[19].ToString() + "',"
                   + "estado_propietario='" + orden[20].ToString() + "', cp_propietario='" + orden[21].ToString() + "', tel_part_propietario='" + orden[22].ToString() + "',tel_ofi_propietario='" + orden[23].ToString() + "',tel_cel_propietario='" + orden[24].ToString() + "',correo_electronico='" + orden[25].ToString() + "',"
                   + "dirigirse_contacto='" + orden[26].ToString() + "',no_siniestro='" + orden[27].ToString() + "',no_poliza='" + orden[28].ToString() + "',flotilla=" + flotilla + ",no_reporte='" + orden[30].ToString() + "',folio_electronico='" + orden[31].ToString() + "', fecha_siniestro='" + orden[32].ToString() + "',ajustador_aseguradora='" + orden[33].ToString() + "',"
                   + "monto_deducible=" + montodeducible.ToString() + ",num_grua_recepcion='" + orden[35].ToString() + "', num_grua_salida='" + orden[36].ToString() + "',id_localizacion=" + orden[5].ToString() + ",id_cliprov_aseguradora=" + orden[37].ToString() + ",tipo_cliprov_aseguradora='C', "
                   + "grua_emp_en='" + orden[40].ToString() + "', grua_emp_sal='" + orden[41].ToString() + "', grua_oper_en='" + orden[38].ToString() + "', grua_oper_sal='" + orden[39].ToString() + "', id_perfilOrden=" + orden[42].ToString() + ", porc_deducible=" + porcDeducible.ToString() + ",monto_demerito=" + montoDemerito.ToString() + " "
                   + "where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + noOrden;
        return ejecuta.insertUpdateDelete(sql);
    }

    public decimal obtieneMonto(int empresa, int taller, int orden, string factura, string proveedor)
    {
        decimal valorMonto = 0;
        string sql = "select isnull(sum(importe+(importe*0.16)),0) from otros_costos " +
            "where no_orden = " + orden + " and id_empresa =" + empresa + " and id_taller =" + taller + " and factura = '" + factura + "' and proveedor =" + proveedor;
        object[] retornos = ejecuta.scalarToDecimal(sql);
        try
        {
            if (Convert.ToBoolean(retornos[0]))
                valorMonto = Convert.ToDecimal(retornos[1]);
            else
                valorMonto = 0;
        }
        catch (Exception) { valorMonto = 0; }
        return valorMonto;
    }

    public object[] generarRemisionSS(int[] sesiones, DataTable dt, decimal totalManoObra, decimal totalRefacciones, decimal totalTotal, string tipo, string comentarios)
    {
        string sql = "generaRemisionSS";
        return ejecuta.exeStoredRemisionesSS(sql, sesiones, dt, totalManoObra, totalRefacciones, totalTotal, tipo, fechas.obtieneFechaLocal(), comentarios);
    }

    public bool llenaBitacoraRef(int noOrden, int idEmpresa, int idTaller, int idUsuario, string observaciones, string proceso)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        string sql = "insert into Bitacora_Refacciones values(" + noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + ", " +
                      idUsuario.ToString() + ",(select isnull((select top 1 b.consecutivo_bitacora from Bitacora_Refacciones b where b.no_orden=" + noOrden.ToString() + " and b.id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " order by b.consecutivo_bitacora desc),0)+1),'" + fechaRetorno + "','" + horaRetorno + "','" + proceso + "','" + observaciones + "')";
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    private int convierteBooltoInt(string valor)
    {
        if (valor == "True")
            return 1;
        else
            return 0;
    }

    public object[] agregaBitacoraLocalizaciones(int[] sesiones, string localizacion, string orden)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        string sql = "insert into Bitacora_Orden_Localizacion values(" + orden + "," + sesiones[2].ToString() + "," + sesiones[3].ToString() + ",(select top 1 id_registro+1 from Bitacora_Orden_Localizacion where no_orden=" + orden + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " order by id_registro desc)," + localizacion + ",'" + fechaRetorno + "','" + horaRetorno + "'," + sesiones[0].ToString() + ")";
        return ejecuta.insertUpdateDelete(sql);
    }
        
    public object[] agregaBitacoraPerfiles(int[] sesiones, string perfil, string orden)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        string sql = "insert into Bitacora_Orden_Perfiles values(" + orden + "," + sesiones[2].ToString() + "," + sesiones[3].ToString() + ",(select isnull((select top 1 id_registro from Bitacora_Orden_Perfiles where no_orden=" + orden + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " order by id_registro desc),0)+1)," + perfil + ",'" + fechaRetorno + "','" + horaRetorno + "'," + sesiones[0].ToString() + ")";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneInfoOrdenPie(int orden, int empresa, int taller)
    {
        string sql = "select t.descripcion,c.razon_social, s.descripcion, v.descripcion,a.descripcion, l.descripcion,o.fase_orden,o.id_localizacion,o.avance_orden,o.no_siniestro,isnull(o.monto_deducible,0) as monto_deducible,isnull(o.total_orden,0) as total_orden,o.id_perfilOrden,po.descripcion as perfil,isnull(convert(char(10),so.f_entrega_estimada,126),'')+' '+isnull(convert(char(18),so.h_estrega_estimada,108),'') as fecha_entrega_estimada,o.correo_electronico,isnull(o.porc_deducible,0) as porc_deducible,o.no_poliza,isnull(o.costo_fijo,0) as costo_fijo from Ordenes_Reparacion o left join Tipo_Orden t on t.id_tipo_orden=o.id_tipo_orden left join cliprov c on c.id_cliprov=o.id_cliprov and c.tipo='C' left join tipo_servicios s on s.id_tipo_servicio=o.id_tipo_servicio left join tipo_valuacion v on v.id_tipo_valuacion = o.id_tipo_valuacion left join Tipo_Asegurados a on a.id_tipo_asegurado=o.id_tipo_asegurado left join localizaciones l on l.id_localizacion=o.id_localizacion left join perfilesordenes po on po.id_perfilOrden=o.id_perfilOrden left join seguimiento_orden so on so.id_empresa=o.id_empresa and so.id_taller=o.id_taller and so.no_orden=o.no_orden where o.id_empresa=" + empresa.ToString() + " and o.id_taller=" + taller.ToString() + " and o.no_orden=" + orden.ToString();
        return ejecuta.dataSet(sql);
    }

    public void actualizaFaseOrden(int orden, int taller, int empresa, int fase)
    {
        string sql = "update ordenes_reparacion set fase_orden=" + fase.ToString() + " where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString();
        object[] data = ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaOrdenDatos(int empresa, int taller, int orden, string localizacion, string avance, string perfil)
    {
        string sql = "update Ordenes_Reparacion set id_localizacion=" + localizacion + ",avance_orden=" + avance + ", id_perfilOrden=" + perfil + " where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaProcesosOrden(int empresa, int taller, int orden, int compleato, int seccion)
    {
        string sql = "";
        string dato = "";
        switch(seccion){
            case 1:
                dato = "datos_orden";
                break;
            case 2:
                dato = "inventario";
                break;
            case 3:
                dato = "caracteristicas_vehiculo";
                break;
            default:
                break;
        }

        if (existeControl(empresa, taller, orden))
            sql = "Update Control_Procesos set " + dato + "=" + compleato.ToString() + " where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString();
        else
            sql = "insert into Control_Procesos(no_orden,id_empresa,id_taller," + dato + ") values(" + orden.ToString() + "," + empresa.ToString() + "," + taller.ToString() + "," + compleato.ToString() + ")";
        return ejecuta.insertUpdateDelete(sql);
    }

    private bool existeControl(int empresa, int taller, int orden) {
        string sql = "select count(*) from Control_Procesos where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString();
        object[] resultado = ejecuta.scalarToBool(sql);
        if (Convert.ToBoolean(resultado[0]))
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public void actualizaCorreoOrden(int empresa, int taller, int orden, string correo)
    {
        string sql = "update Ordenes_Reparacion set correo_electronico='" + correo + "' where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden;
        object[] acutalizado = ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneInfoOrdenCompra(int orden, int empresa, int taller, int cotizacion)
    {
        string sql = "select convert(char(10),fecha_entrega,120),convert(char(8),hora_entrega,120),nombre_entrega,factura from orden_compra_encabezado where no_orden=" + orden.ToString() + " and id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and id_orden=" + cotizacion.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneTotalesPresupuesto(int empresa, int taller, int orden)
    {
        string sql = "select count(*) as refacciones, isnull(sum( (refPrecioVenta*refCantidad)),0) from Refacciones_Orden where ref_id_empresa=" + empresa.ToString() + " and ref_id_taller=" + taller.ToString() + " and ref_no_orden=" + orden.ToString() + " and refEstatusSolicitud not in(4,5) and proceso is null and refEstatus not in('CA')";
        return ejecuta.dataSet(sql);
    }

    public object[] existeInventario(int noOrden, int idEmpresa, int idTaller)
    {
        string sql = "select count(*)from inventario_vehiculo where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
        return ejecuta.scalarToInt(sql);
    }

    public object[] actualizaInvGas(int noOrden, int idEmpresa, int idTaller, int gaso)
    {
        string sql = "update inventario_vehiculo set gasolina=" + gaso.ToString() + " where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] cuentaRecepcionRealizada(int noOrden, int idEmpresa, int idTaller)
    {
        string sql = "select (case datos_orden when 1 then 1 else 0 end)+(case inventario when 1 then 1 else 0 end)+(case caracteristicas_vehiculo when 1 then 1 else 0 end) from Control_Procesos where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
        return ejecuta.scalarToInt(sql);
    }

    public object[] obtieneRegBitacoraLoc(int noOrden, int idEmpresa, int idTaller)
    {
        string sql = "select count(*) from Bitacora_Orden_Localizacion where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
        return ejecuta.scalarToInt(sql);
    }

    public object[] actualizaLocRec(int noOrden, int idEmpresa, int idTaller, int idUsuario)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();
        string sql = "update ordenes_reparacion set id_localizacion=2 where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() +
            " insert into Bitacora_Orden_Localizacion values(" + noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + "," +
            "(select isnull((select top 1 id_registro from Bitacora_Orden_Localizacion b where b.no_orden = " + noOrden.ToString() + " and b.id_empresa = " + idEmpresa.ToString() + " and b.id_taller = " + idTaller.ToString() + " order by id_registro desc),0)+1)," +
            "2,'" + fechaRetorno + "', '" + horaRetorno + "', " + idUsuario.ToString() + ") " +
            "update ordenes_reparacion set id_localizacion=2 where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaPresupuestos(int[] sesiones, string estatus)
    {
        string sql = "";
        if (estatus == "T")
            sql = "update refacciones_orden set estatus_presupuesto='" + estatus + "' where ref_no_orden=" + sesiones[4].ToString() + " and ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " and (estatus_presupuesto='P' or estatus_presupuesto is null) " +
                "update mano_obra set estatus_presupuesto ='" + estatus + "' where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and (estatus_presupuesto='P' or estatus_presupuesto is null)";
        else
            sql = "update refacciones_orden set estatus_presupuesto='" + estatus + "' where ref_no_orden=" + sesiones[4].ToString() + " and ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() +
                " update mano_obra set estatus_presupuesto ='" + estatus + "' where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] existeRelacionRefaccion(string empresa, string taller, string orden, string id)
    {
        string sql = string.Format("select sum(registros) as relaciones from(select count(*) as registros from orden_compra_detalle where id_empresa = {0} and id_taller = {1} and no_orden = {2} and id_refaccion = {3} union all select count(*) as registros from cotizacion_detalle where id_empresa = {0} and id_taller = {1} and no_orden = {2} and id_refaccion = {3}) as tabla", empresa, taller, orden, id);
        return ejecuta.scalarToBool(sql);

    }

    public object[] actualizaOrdenDeducible(int[] sesiones, decimal monto, decimal montoDemerito, string nombre, string ap_paterno, string ap_materno)
    {
        string sql = "update ordenes_reparacion set nombre_propietario='" + nombre.ToUpper().Trim() + "', ap_paterno_propietario='" + ap_paterno.ToUpper().Trim() + "', ap_materno_propietario='" + ap_materno.ToUpper().Trim() + "',monto_deducible=" + monto.ToString() + ", monto_demerito=" + montoDemerito.ToString() + " where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString();
        return ejecuta.insertUpdateDelete(sql);
    }




    public object[] obtieneAvance(int idEmpresa, int idTaller, int noOrden)
    {
        string sql = "declare @registros int declare @avances int " +
"set @registros = (select count(*) from seguimiento_operacion mo where mo.no_orden = " + noOrden + " and mo.id_empresa = " + idEmpresa + " and mo.id_taller = " + idTaller + ")" +
//"and mo.id_grupo_op in (1,2,3) ) " +
"set @avances = (select isnull(sum(tabla.valor), 0) as avance from( " +
"select case mo.p25 when 1 then 1 * 25 else 0 end + case mo.p50 when 1 then 1 * 25 else 0 end + case mo.p75 when 1 then 1 * 25 else 0 end + case mo.p100 when 1 then 1 * 25 else 0 end as valor from seguimiento_operacion mo where mo.no_orden = " + noOrden + " and mo.id_empresa = " + idEmpresa + " and mo.id_taller = " + idTaller + /*" and mo.id_grupo_op in (1,2,3)*/") as tabla) " +
"select isnull(@avances / @registros, 0) as avance";
        return ejecuta.scalarToDecimal(sql);
    }

    public string obtieneRefCot(int noOrden, int idEmpresa, int idTaller, int idCotizacion)
    {
        string sql = "declare @refacciones varchar(500) select @refacciones = coalesce(@refacciones+', ','')+descripcion from cotizacion_detalle where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and id_cotizacion=" + idCotizacion.ToString() + " group by descripcion order by descripcion asc select @refacciones";
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] obtienePrefijoTaller(string taller)
    {
        string sql = "select identificador from talleres where id_taller=" + taller;
        return ejecuta.scalarToString(sql);
    }

    public object[] obtieneInfoFacturar(int[] sesiones, string opcion)
    {
        string sql = "Declare @tipo int Declare @empresa int Declare @taller int Declare @orden int set @tipo = " + opcion + " set @empresa=" + sesiones[2].ToString() + " set @taller=" + sesiones[3].ToString() + " set @orden=" + sesiones[4].ToString() + " " +
"select distinct tabla.id,tabla.concepto,tabla.cantidad,tabla.unidad,tabla.valorUnitario,tabla.importe,tabla.Iva,tabla.montoIva,tabla.subConcepto,tabla.opcion,tabla.dato from( " +
"select 'MO-'+cast(m.consecutivo as char(10))  as id,g.descripcion_op+' '+m.id_refaccion as concepto,1 as cantidad,'SERVICIO' AS unidad, " +
"isnull(m.monto_mo,0) as valorUnitario,isnull(m.monto_mo,0) as importe, '16' as Iva,(m.monto_mo*0.16) as montoIva,m.monto_mo+(m.monto_mo*0.16) as subConcepto,'1' as opcion,m.consecutivo as dato " +
"from Mano_Obra m  " +
"inner join operaciones g on g.id_operacion=m.id_operacion " +
"where m.id_empresa=@empresa and m.id_taller=@taller and m.no_orden=@orden and m.idCfd=0 " +
"union all  " +
"select 'REF-'+cast(refOrd_id as char(10)) as id,refdescripcion as concepto,refcantidad as cantidad,'REFACCIÓN' as unidad, " +
"isnull(refPrecioVenta,0) as valorUnitario,(isnull(refprecioventa,0)*refcantidad) as importe,'16' as Iva,(isnull(refprecioventa,0)*refcantidad)*0.16 as montoIva,(isnull(refprecioventa,0)*refcantidad)+((isnull(refprecioventa,0)*refcantidad)*0.16) as subConcepto,'1' as opcion,refOrd_id as dato " +
"from refacciones_orden where ref_id_empresa=@empresa and ref_id_taller=@taller and ref_no_orden=@orden and refEstatus<>'CA' and (idCfd=0 or idcfd is null) and proceso is null  and refEstatusSolicitud <> 11 " +
"union all " +
"select 'MO-'+cast(m.consecutivo as char(10))  as id,g.descripcion_op+' '+m.id_refaccion as concepto,1 as cantidad,'SERVICIO' AS unidad, " +
"isnull(m.monto_mo,0) as valorUnitario,isnull(m.monto_mo,0) as importe, '16' as Iva,(m.monto_mo*0.16) as montoIva,m.monto_mo+(m.monto_mo*0.16) as subConcepto,'2' as opcion,m.consecutivo as dato " +
"from Mano_Obra m  " +
"inner join operaciones g on g.id_operacion=m.id_operacion " +
"where m.id_empresa=@empresa and m.id_taller=@taller and m.no_orden=@orden  and m.idCfd=0 " +
"union all  " +
"select 'REF-'+cast(refOrd_id as char(10)) as id,refdescripcion as concepto,refcantidad as cantidad,'REFACCIÓN' as unidad, " +
"isnull(refPrecioVenta,0) as valorUnitario,(isnull(refprecioventa,0)*refcantidad) as importe,'16' as Iva,(isnull(refprecioventa,0)*refcantidad)*0.16 as montoIva,(isnull(refprecioventa,0)*refcantidad)+((isnull(refprecioventa,0)*refcantidad)*0.16) as subConcepto,'3' as opcion,refOrd_id as dato " +
"from refacciones_orden where ref_id_empresa=@empresa and ref_id_taller=@taller and ref_no_orden=@orden and refEstatus<>'CA' and (idCfd=0 or idcfd is null) and proceso is null and refEstatusSolicitud <> 11) as tabla " +
"where tabla.opcion = @tipo order by tabla.opcion";
        return ejecuta.dataSet(sql);
    }


    public object[] actualizaEstatusOrden(string empresa, string taller, string orden)
    {
        string sql = "update ordenes_reparacion set status_orden='T' where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneEstatusOrden(string empresa, string taller, string orden)
    {
        string sql = "select status_orden from ordenes_reparacion where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden;
        return ejecuta.scalarToString(sql);
    }

    public object[] obtieneUltimaFacturaTaller(string empresa, string taller)
    {
        object[] retorno = new object[]{false,0};
        //string sql = "select factura from empresas_taller where id_empresa=" + empresa + " and id_taller=1";
        string sql = "select factura from empresas_taller where id_empresa=" + empresa + " and id_taller=" + taller;
        int ultimaFactura = 0;
        object[] facturas = ejecuta.scalarToInt(sql);
        if (Convert.ToBoolean(facturas[0]))
        {
            ultimaFactura = Convert.ToInt32(facturas[1]) + 1;
            sql = "update empresas_taller set factura =" + ultimaFactura + " where id_empresa=" + empresa + " and id_taller=" + taller;
            object[] actualizado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(actualizado[0]))
            {
                retorno[0] = true;
                retorno[1] = ultimaFactura;
            }
            else
                retorno = facturas;
        }
        else
            retorno = facturas;
        return retorno;
    }

    public object[] actualizaUnicoProveedor(int noOrden, int idEmpresa, int idTaller, object idProv, decimal costo, bool cambiaCosto)
    {
        string sql = "";
        if (cambiaCosto)
            sql = "update refacciones_orden set refProveedor=" + idProv.ToString() + ", refcosto=" + costo.ToString() + ",refporcentsobrecosto=" + costo.ToString() + ", refPrecioVenta=" + costo.ToString() + " where ref_no_orden =" + noOrden.ToString() + " and ref_id_empresa =" + idEmpresa.ToString() + " and ref_id_taller =" + idTaller.ToString();
        else
            sql = "update refacciones_orden set refProveedor=" + idProv.ToString() + " where ref_no_orden =" + noOrden.ToString() + " and ref_id_empresa =" + idEmpresa.ToString() + " and ref_id_taller =" + idTaller.ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneInfoOrdenAddendaQualitas(int empresa, int taller, int orden)
    {
        string sql = string.Format("select t.no_poliza,t.no_reporte,t.fecha_siniestro,t.no_siniestro,t.marca,t.model,"+
"case when t.anio is null then(select TOP 1 MODELO from vehiculos where placas = t.placasOrden) else t.anio end as anio,"+
"case when t.placas is null then t.placasOrden else t.placas end as placas,"+
"case when t.color is null then(select TOP 1 color_ext from vehiculos where placas = t.placasOrden) else t.color end as color,"+
"case when t.serie is null then(select TOP 1 serie_vin from vehiculos where placas = t.placasOrden) else t.serie end as serie,"+
"t.folio_electronico, t.monto_deducible from("+
"select o.no_poliza, o.no_reporte, o.fecha_siniestro, o.no_siniestro,"+
"(select descripcion from marcas where id_marca = o.id_marca) as marca," +
"(select descripcion from tipo_unidad where id_marca = o.id_marca and id_tipo_vehiculo = o.id_tipo_vehiculo and id_tipo_unidad = o.id_tipo_unidad) as model," +
"(select v.modelo from vehiculos v where v.id_marca = o.id_marca and v.id_tipo_vehiculo = o.id_tipo_vehiculo and v.id_tipo_unidad = o.id_tipo_unidad and v.placas = o.placas) as anio," +
"(select v.placas from vehiculos v where v.id_marca = o.id_marca and v.id_tipo_vehiculo = o.id_tipo_vehiculo and v.id_tipo_unidad = o.id_tipo_unidad and v.placas = o.placas) as placas," +
"(select v.color_ext from vehiculos v where v.id_marca = o.id_marca and v.id_tipo_vehiculo = o.id_tipo_vehiculo and v.id_tipo_unidad = o.id_tipo_unidad and v.placas = o.placas) as color," +
"(select v.serie_vin from vehiculos v where v.id_marca = o.id_marca and v.id_tipo_vehiculo = o.id_tipo_vehiculo and v.id_tipo_unidad = o.id_tipo_unidad and v.placas = o.placas) as serie,o.folio_electronico, " +
"o.monto_deducible,o.placas as placasOrden from ordenes_reparacion o where o.id_empresa = {0} and o.id_taller = {1} and o.no_orden = {2}) as t", empresa, taller, orden);
        return ejecuta.dataSet(sql);
    }

    public object[] actualizaCostoFijo(int empresa, int taller, int orden, decimal costo)
    {
        string sql = "update ordenes_reparacion set costo_fijo=" + costo + " where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneDatosVehiculoExel(string noOrden, string idEmpresa, string idTaller)
    {
        string sql = "select m.descripcion as marca,tu.descripcion as tipo_unidad,v.serie_vin,orp.no_siniestro,orp.no_poliza,orp.no_reporte, " +
                     "cast(v.modelo as char(4)) as modelo,rtrim(ltrim(v.color_ext)) as color, rtrim(ltrim(orp.placas)) as placas,v.puertas,tt.descripcion as transmision, " +
                     "ta.descripcion as asegurado,v.cilindros,case v.aire_acondicionado when 1 then 'Si' else 'No' end as aireAcondicionado,s.f_valuacion,f_recepcion,t.identificador as taller  " +
                     "from Ordenes_Reparacion orp " +
                     "left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
                     "left join Marcas m on m.id_marca = orp.id_marca " +
                     "left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
                     "left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
                     "left join Cliprov c on c.id_cliprov = orp.id_cliprov_aseguradora and c.tipo = 'C' " +
                     "inner join Seguimiento_Orden s on s.id_empresa = orp.id_empresa and s.id_taller = orp.id_taller and s.no_orden = orp.no_orden " +
                     "left join Tipo_Asegurados ta on ta.id_tipo_asegurado = orp.id_tipo_asegurado " +
                     "left join Tipo_Transmision tt on tt.id_tipo_transmision = v.id_tipo_transmision " +
                     "inner join talleres t on t.id_taller = orp.id_taller " +
                     "where orp.id_empresa =" + idEmpresa + " and orp.id_taller =" + idTaller + " and orp.no_orden =" + noOrden;
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneCotizacionGral(string noOrden, string idEmpresa, string idTaller)
    {
        string sql = "declare @orden int " +
                     "declare @empresa int " +
                     "declare @taller int " +
                     "declare @acceso int " +
                     "set @orden = " + noOrden + " " +
                     "set @empresa = " + idEmpresa + " " +
                     "set @taller =" + idTaller + " " +
                     "set @acceso = 1 " +
                     "select r.refOrd_Id,r.refCantidad,r.refDescripcion,r.refPrecioVenta, " +
                     "case @acceso when 0 then " +
                     "case r.refEstatus when 'AU' THEN r.refProveedor else " +
                     "(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
                     "else r.refProveedor end  as refProveedor, " +
                     "(select razon_social from Cliprov where tipo = 'P' and id_cliprov in (select case @acceso when 0 then " +
                     "case r.refEstatus when 'AU' THEN r.refProveedor else " +
                     "(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
                     "else r.refProveedor end)) as razon_social, " +
                     "case @acceso when 0 then " +
                     "case r.refEstatus when 'AU' THEN r.refCosto else " +
                     "(select isnull((SELECT costo_unitario from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
                     "else r.refCosto end  as refCosto, " +
                     "case @acceso when 0 then " +
                     "case r.refEstatus when 'AU' THEN " +
                     "(SELECT isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor), 0)) " +
                     "else " +
                     "(select isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
                     "else " +
                     "(SELECT isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),0)) end as porc_desc, " +
                     "case @acceso when 0 then " +
                     "case r.refEstatus when 'AU' THEN " +
                     "(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor), 0)) " +
                     "else " +
                     "(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
                     "else " +
                     "(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),0)) end as importeDesc, " +
                     "case @acceso when 0 then " +
                     "case r.refEstatus when 'AU' THEN " +
                     "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor), 0)) " +
                     "else " +
                     "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
                     "else " +
                     "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),0)) end as importeCosto, " +
                     "isnull((r.refCantidad * r.refPrecioVenta), 0) as importeVenta,r.refEstatus, " +
                     "isnull((SELECT estatus from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),'CAN') as estatusRef,r.refEstatusSolicitud, " +
                     "(select staDescripcion from rafacciones_estatus where staRefId = r.refEstatusSolicitud) as descripEstatus, " +
                     "(isnull((r.refCantidad * r.refPrecioVenta), 0) - " +
                     "(select case @acceso when 0 then " +
                     "case r.refEstatus when 'AU' THEN " +
                     "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor), 0)) " +
                     "else " +
                     "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
                     "else " +
                     "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),0)) end)) as utilidad, " +
                     "r.refPorcentSobreCosto as porcSobre, " +
                     "(select staDescripcion from rafacciones_estatus where starefid = r.refEstatusSolicitud) as estatusSoli, " +
                     "r.refEstatusSolicitud, " +
                     "case r.refEstatus when 'NA' then 'No Aplica' when 'EV' then 'Evaluación' when 'RP' THEN 'Reparación' when 'CO' then 'Compra' when 'CA' THEN 'Cancelada' when 'AP' then 'Aplicada' when 'AU' then 'Autorizada' else '' end as estatus " +
                     "from Refacciones_Orden r " +
                     "left join Cliprov c on c.id_cliprov = r.refProveedor and c.tipo = 'P' " +
                     "where r.ref_no_orden = @orden and r.ref_id_empresa = @empresa and r.ref_id_taller = @taller " +
                     "and r.refEstatusSolicitud <> 11 and r.proceso is null and refOrd_Id in (select distinct id_cotizacion_detalle from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = r.ref_id_taller)";
        return ejecuta.dataSet(sql);
    }

    internal object[] obtieneManoObra(string orden, string empresa, string taller)
    {
        string sql = " select mo.id_empresa,mo.id_taller,mo.no_orden,mo.consecutivo,mo.id_grupo_op,mo.id_operacion, " +
            "mo.id_refaccion,isnull(mo.monto_mo, 0) as monto_mo,gop.descripcion_go,op.descripcion_op, " +
            "isnull(mo.monto_ini, 0) as monto_ini from mano_obra mo " +
            "inner join Grupo_Operacion gop on gop.id_grupo_op = mo.id_grupo_op " +
            "inner join Operaciones op on op.id_operacion = mo.id_operacion " +
            "where id_empresa =" + empresa + " and id_taller = " + taller + " and no_orden = " + orden;
        return ejecuta.dataSet(sql);
    }

    internal object[] obtieneRefacciones(string orden, string empresa, string taller)
    {
        string sql = "select refCantidad,refDescripcion,refprecioventa from refacciones_orden " +
            "where ref_id_empresa =" + empresa + " and ref_id_taller = " + taller + " and ref_no_orden = " + orden;
        return ejecuta.dataSet(sql);
    }

    public bool quitaProductoFactura(int usuario, string argumentos)
    {
        string sql = "delete from documentocfditmp_fact where idusuario=" + usuario + " and idfila=" + argumentos + " insert into documentocfditmp select * from documentocfditmp where idusuario=" + usuario + " and idfila=" + argumentos;
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public bool quitarTodo(int usuario)
    {
        string sql = "delete from documentocfditmp where idusuario=" + usuario + " insert into documentocfditmp select * from documentocfditmp_fact where idusuario=" + usuario + " delete from documentocfditmp_fact where idusuario=" + usuario;
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public bool agregarTodo(int usuario)
    {
        string sql = "delete from documentocfditmp_fact where idusuario=" + usuario + " insert into documentocfditmp_fact select * from documentocfditmp where idusuario=" + usuario + " delete from documentocfditmp where idusuario=" + usuario;
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public object[] obtieneInfoFacturarPrev(int usuario, int emisor, int receptor)
    {
        string sql = "insert into documentocfdi select idfila," + emisor + "," + receptor + ",txtIdent,txtConcepto,radnumCantidad,ddlUnidad,txtValUnit,lblImporte,txtPtjeDscto,txtDscto,lblSubTotal,ddlIvaTras,ddlIeps,lblIvaTras,lblIeps,ddlIvaRet,ddlIsrRet,lblIvaRet,lblIsrRet,lblTotal,EncFechaGenera from documentocfditmp_fact where idusuario= " + usuario;
        return ejecuta.insertUpdateDelete(sql);
    }
    public object[] cambiaProveedor(int empresa, int taller, int orden,int refordid,int proveedor)
    {
        string sql = "update refacciones_orden set refProveedor=" + proveedor + " where ref_no_orden="+orden+" and ref_id_empresa="+empresa+" and ref_id_taller="+taller+" and refOrd_id=" +refordid;
        return ejecuta.insertUpdateDelete(sql);
    }
    public object[] traeRefOrdId(int empresa, int taller, int orden, int refordid)
    {
        string sql = "select refOrd_Id from refacciones_orden where ref_no_orden=" + orden + " and ref_id_empresa=" + empresa + " and ref_id_taller=" + taller + " and refOrd_id=" + refordid;
        return ejecuta.scalarToInt(sql);
    }
    public object[] esOrdenTransito(int empresa, int taller, int orden)
    {
        string sql = "select count(*) from Ordenes_Reparacion where no_orden=" + orden + " and id_empresa=" + empresa + " and id_taller=" + taller + " and id_perfilOrden=2";
        return ejecuta.scalarToInt(sql);
    }
}