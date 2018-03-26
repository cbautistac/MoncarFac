using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Vehiculos
/// </summary>
public class Vehiculos
{
    Ejecuciones ejecuta = new Ejecuciones();
	public Vehiculos()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] obtieneInfoVehiculo(int orden, int empresa, int taller)
    {
        string sql = "select * from Vehiculos where placas in (select placas from Ordenes_Reparacion where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + ")";
        return ejecuta.dataSet(sql);
    }

    public object[] actualizaVehiculo(string placas, string[] datos)
    {       
        string sql = "update Vehiculos set id_marca=" + datos[0] + " ,id_tipo_vehiculo=" + datos[1] + " ,id_tipo_unidad=" + datos[2] + " ,modelo=" + datos[4] + " ,placas='" + datos[5].ToUpper() + "' ,serie_vin='" + datos[6] + "' ,motor='" + datos[7] + "' ,color_int='" + datos[8] + "' ,color_ext='" + datos[9] + "' ,id_tipo_transmision=" + datos[10] + " ,id_tipo_traccion=" + datos[11] + " ,cilindros=" + datos[12] + " ,no_economico='" + datos[15] + "' ,puertas=" + datos[14] + " ,version='" + datos[13] + "' ,id_tipo_rin=" + datos[16] + " ,medida_llanta='" + datos[17] + "' ,quemacocos=" + datos[18] + " ,bolsas_aire=" + datos[19] + " ,aire_acondicionado=" + datos[20] + " ,direccion_hidraulica=" + datos[21] + " ,elevadores_manuales=" + datos[22] + " ,espejos_lat_man=" + datos[23] + " ,color_espejos_negro=" + datos[24] + " ,molduras=" + datos[25] + " ,cantoneras_negras=" + datos[27] + " ,vertiduras='" + datos[26] + "' ,faros_niebla=" + datos[28] + " ,facia_defensa_corrugada=" + datos[29] + " ,cabina='" + datos[30] + "' ,defensa_cromada=" + datos[31] + " where placas='" + placas.ToUpper() + "'";
        return ejecuta.insertUpdateDelete(sql);
    }

    internal object[] obtieneInfoVehiculoImpr(int orden, int empresa, int taller)
    {
        string sql =
"select m.descripcion as Marca, (select upper(tu.descripcion) from Tipo_Unidad tu where tu.id_marca=v.id_marca and tu.id_tipo_vehiculo=v.id_tipo_vehiculo and tu.id_tipo_unidad = v.id_tipo_unidad)+' '+cast(v.modelo as char(4)) as Modelo, v.serie_vin as 'Serie Vin', v.placas as Placas, v.version as Version,v.motor as Motor," +
"v.puertas as Puertas, v.medida_llanta as 'Medidas Llantas', v.color_ext as 'Color Exterior'," +
"v.color_int as 'Color Interior'," +
"isnull((select tr.descripcion from Tipo_Rin tr where tr.id_tipo_rin=v.id_tipo_rin ),'') as 'Tipo Rin'," +
"case v.quemacocos when 1 then 'Si' when 0 then 'No' else 'No Aplica' end as Quemacocos," +
"case v.bolsas_aire when 1 then 'Si' when 0 then 'No' else 'No Aplica' end as 'Bolse de Aire'," +
"isnull((select tt.descripcion from Tipo_Transmision tt where v.id_tipo_transmision=tt.id_tipo_transmision ),'') as 'Tipo Transmición'," +
"case v.aire_acondicionado when 1 then 'Si' when 0 then 'No' else 'No Aplica' end as 'Aire Acondicionado'," +
"case v.direccion_hidraulica when 1 then 'Si' when 0 then 'No' else 'No Aplica' end as 'Direccion Hidraulica'," +
"case v.elevadores_manuales when 1 then 'Manuales' when 0 then 'Eléctricos' else 'No Aplica' end as 'Elevadores Puertas'," +
"case v.espejos_lat_man when 1 then 'Manuales' when 0 then 'Eléctricos' else 'No Aplica' end as 'Espejos Laterales'," +
"case v.color_espejos_negro when 1 then 'Negros' when 0 then 'Color Auto' else 'No Aplica' end as 'Color Espejos'," +
"case v.molduras when 1 then 'Si' when 0 then 'No' else 'No Aplcia' end as 'Molduras'," +
"case v.cantoneras_negras when 1 then 'Negro' when 0 then 'Color Auto' else 'No Aplica' end as 'Cantoneras'," +
"case v.vertiduras when 'T' then 'Tela' when 'P' then 'Piel' else 'No Aplica' end as 'Vertiduras'," +
"case v.faros_niebla when 1 then 'Si' when 0 then 'No' else 'No Aplica' end as 'Faros de Niebla'," +
"case v.facia_defensa_corrugada when 1 then 'Corrugada' when 0 then 'Color Auto' else 'No Aplica' end as 'Facia o Defensa'," +
"case v.cabina when 'S' then 'Sencilla' when 'D' then 'Doble' else 'No Aplica' end as 'Cabina'," +
"case v.defensa_cromada when 1 then 'Si' when 0 then 'No' else 'No Aplica' end as 'Defensa Cromada'" +
"from Vehiculos v " +
"left join marcas m on m.id_marca = v.id_marca " +
"left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = v.id_tipo_vehiculo " +
"where v.placas in (select placas from Ordenes_Reparacion where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + ")";
        return ejecuta.dataSet(sql);
    }

    internal object[] DatosAseguradora(int orden, int empresa, int taller)
    {
        string sql = "select orp.no_torre,c.razon_social ,(c.calle+' '+ c.num_ext) As calle, c.colonia, (c.municipio + ' '+ c.estado+ ' '+ c.cp) as Delegacion, " +
"(m.descripcion+ ' '+(select tu.descripcion from tipo_unidad tu where tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad)+' '+ltrim(rtrim(v.version))+' '+v.color_ext) as Linea," +
"v.modelo,orp.placas," +
"(isnull(orp.nombre_propietario,'') +' '+ isnull(orp.ap_paterno_propietario,'')+' '+ isnull(orp.ap_materno_propietario,'')) as Nombre," +
"orp.no_poliza, orp.no_siniestro,(SELECT (CASE convert(char(10),f_entrega_estimada,126) when '1900-01-01' then '' else convert(char(10),f_entrega_estimada,126) end) FROM SEGUIMIENTO_ORDEN WHERE id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " ) as entregaEstimada,orp.total_mo,orp.total_ref,cast((orp.total_mo+orp.total_ref) as varchar) as total_presupuesto,"+
"v.version,v.color_ext,(SELECT (CASE convert(char(10),f_recepcion,126) when '1900-01-01' then '' else convert(char(10),f_recepcion,126) end) FROM SEGUIMIENTO_ORDEN WHERE id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " ) as f_recepcion, (SELECT (CASE convert(char(10),f_retorno_transito,126) when '1900-01-01' then '' else convert(char(10),f_retorno_transito,126) end) FROM SEGUIMIENTO_ORDEN WHERE id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " ) as f_retorno_transito " +
",case when v.no_economico is null then '' when v.no_economico='N/A' then '' when v.no_economico='' then '' else v.no_economico end as economico "+
"from Ordenes_Reparacion orp " +
"left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo='C'" +
"left join marcas m on m.id_marca=orp.id_marca " +
"left join vehiculos v on orp.id_marca=v.id_marca and orp.id_tipo_vehiculo=v.id_tipo_vehiculo and orp.id_tipo_unidad=v.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo " +
"where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " and v.id_vehiculo=orp.id_vehiculo";
        return ejecuta.dataSet(sql);
    }

    internal object[] DatosManoObra(int orden, int empresa, int taller)
    {
        string sql = "select gp.descripcion_go as Operacion, (op.descripcion_op+' '+ r.descripcion_ref) as 'Descripcion Operacion',mo.monto_mo from Mano_Obra mo left join Grupo_Operacion gp on gp.id_grupo_op =mo.id_grupo_op left join Operaciones op on op.id_operacion = mo.id_operacion left join Refacciones r on r.id_refaccion = mo.id_refaccion " + "where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " order by gp.descripcion_go asc ";
        return ejecuta.dataSet(sql);
    }

    internal object[] datosEncabezadoRemisionSS(int idremision, char tipo, int empresa, int taller, int orden)
    {
        string sql = "select r.no_remision_ss,convert(char(10),r.fecha,126) as fecha,r.hora, u.nombre_usuario,r.total_mo,r.total_refacciones,r.total from Remision_SalidasSinCargo r inner join Usuarios u on u.id_usuario = r.id_usuario where r.id_remision_ss = " + idremision + " and r.tipo = '" + tipo + "' and r.id_empresa = " + empresa + " and r.id_taller =" + taller + " and r.no_orden = " + orden;
        return ejecuta.dataSet(sql);
    }

    internal object[] DatosManoObraGrupo(int orden, int empresa, int taller)
    {
        string sql = "SELECT m.id_grupo_op, G.DESCRIPCION_GO AS GRUPO,sum(m.monto_mo) as monto FROM MANO_OBRA M INNER JOIN GRUPO_OPERACION G ON G.ID_GRUPO_OP=M.ID_GRUPO_OP WHERE M.ID_EMPRESA=" + empresa.ToString() + " AND M.ID_TALLER=" + taller.ToString() + " AND M.NO_ORDEN=" + orden.ToString() + " GROUP BY M.ID_GRUPO_OP,G.DESCRIPCION_GO";
        return ejecuta.dataSet(sql);
    }

    internal int obtieneTamñoManoObra(int orden, int empresa, int taller)
    {
        int retorno = 0;
        string sql = "select isnull(count(*),0) from Mano_obra" +
                     " where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString();
        object[] ejecutado = ejecuta.scalarToInt(sql);
        if ((bool)ejecutado[0])
            retorno = Convert.ToInt32(ejecutado[1].ToString());
        else
            retorno = 0;
        return retorno;
    }
    internal int obtieneTamñoManoObraGrupo(int orden, int empresa, int taller)
    {
        int retorno = 0;
        string sql = "select isnull(count(*),0) from Mano_obra" +
                     " where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString();
        object[] ejecutado = ejecuta.scalarToInt(sql);
        if ((bool)ejecutado[0])
            retorno = Convert.ToInt32(ejecutado[1].ToString());
        else
            retorno = 0;
        return retorno;
    }

    internal object[] DatosManoObraGrupoDetalle(int orden, int empresa, int taller, string grupo)
    {
        string sql = "select case when lower(o.descripcion_op)=lower(m.id_refaccion) then o.descripcion_op else (o.descripcion_op +' '+m.id_refaccion) end as mo, m.id_operacion,o.descripcion_op, m.id_refaccion,isnull(m.monto_mo,0) as monto from mano_obra m inner join operaciones o on o.id_operacion=m.id_operacion where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " and id_grupo_op=" + grupo;
        return ejecuta.dataSet(sql);
    }

    internal object[] DatosRefacciones(int orden, int empresa, int taller)
    {
        string sql = "select refOrd_id,refCantidad,refDescripcion,(isnull(refPrecioVenta,0) * refCantidad),refEstatus from refacciones_orden where ref_id_empresa=" + empresa.ToString() + " and ref_id_taller=" + taller.ToString() + "  and proceso is null and ref_no_orden=" + orden.ToString() + "and refEstatusSolicitud <> 11";
        return ejecuta.dataSet(sql);
    }

    internal object[] DatosOperativos(int orden, int empresa, int taller)
    {
        string sql = "select o.idEmp,CASE WHEN e.LLave_nombre_empleado IS NULL THEN isnull(e.nombres,'')+' '+isnull(e.apellido_paterno,'')+' '+isnull(e.apellido_materno,'') else e.llave_nombre_empleado end as Llave_nombre_empleado from operativos_orden o left join empleados e on e.idEmp=o.idEmp where o.no_orden=" + orden.ToString() + " and o.id_empresa=" + empresa.ToString() + " and o.id_taller=" + taller.ToString() + " group by o.idEmp,e.llave_nombre_empleado,o.idEmp,e.llave_nombre_empleado,e.nombres,e.apellido_paterno,e.apellido_materno";
        return ejecuta.dataSet(sql);
    }

    internal object[] DatosManoObraGrupoRemSS(int idremision, char tipo)
    {
        string sql = "select m.id_grupo_op,g.descripcion_go as GRUPO,sum(m.monto_mo) as monto from det_rem_ss d inner join Remision_SalidasSinCargo r on r.id_remision_ss = d.id_remision_ss and r.tipo = d.tipo inner join mano_obra m on m.id_empresa = r.id_empresa and m.id_taller = r.id_taller and m.no_orden = r.no_orden and m.consecutivo = d.id_concepto inner join Grupo_Operacion g on g.id_grupo_op = m.id_grupo_op where d.id_remision_ss = " + idremision.ToString() + " and d.tipo = '" + tipo + "' and d.clasificacion = 'M' and r.id_remision_ss = d.id_remision_ss and r.tipo = d.tipo group by m.id_grupo_op,g.descripcion_go order by m.id_grupo_op";
        return ejecuta.dataSet(sql);
    }

    internal object[] DatosManoObraGrupoDetalleRemSS(int idRemision, char tipo, string grupo)
    {
        string sql = "select o.descripcion_op+' '+lower(m.id_refaccion) as mo,m.monto_mo from det_rem_ss d inner join Remision_SalidasSinCargo r on r.id_remision_ss = d.id_remision_ss and r.tipo = d.tipo inner join mano_obra m on m.id_empresa = r.id_empresa and m.id_taller = r.id_taller and m.no_orden = r.no_orden and m.consecutivo = d.id_concepto inner join Grupo_Operacion g on g.id_grupo_op = m.id_grupo_op inner join Operaciones o on o.id_operacion = m.id_operacion where d.id_remision_ss = " + idRemision + " and d.tipo = '" + tipo + "' and d.clasificacion = 'M' and r.id_remision_ss = d.id_remision_ss and r.tipo = d.tipo and m.id_grupo_op = " + grupo + " order by m.id_grupo_op,m.id_operacion";
        return ejecuta.dataSet(sql);
    }

    internal object[] DatosRefaccionesRemSS(int idremision, char tipo)
    {
        string sql = "select d.cantidad,d.descripcion,d.importe from det_rem_ss d inner join remision_salidassincargo r on r.id_remision_ss = d.id_remision_ss and r.tipo = d.tipo inner join Refacciones_Orden ro on ro.ref_id_empresa = r.id_empresa and ro.ref_id_taller = r.id_taller and ro.ref_no_orden = r.no_orden and ro.refOrd_Id = d.id_concepto where d.id_remision_ss = " + idremision + " and d.tipo = '" + tipo + "' and d.clasificacion = 'R' and r.id_remision_ss = d.id_remision_ss and r.tipo = d.tipo";
        return ejecuta.dataSet(sql);
    }
}