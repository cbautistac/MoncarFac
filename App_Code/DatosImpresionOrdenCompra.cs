using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de DatosImpresionOrdenCompra
/// </summary>
public class DatosImpresionOrdenCompra
{
    Ejecuciones ejecuta = new Ejecuciones();
    string sql;
    object[] resultado = new object[2];

    public DatosImpresionOrdenCompra()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    internal object[] encabezadoOrdenCompra(int noOrden, int idEmpresa, int idTaller, int idCotizacion)
    { 
        sql = "select o.id_orden,o.folio_orden,o.estatus," +
              "case o.estatus when 'PEN' then 'Pendiente' when 'CAN' then 'Cancelada' when 'CO' then 'Concluida' else '' end as valorEstatus," +
              "convert(char(10), o.fecha, 120) + ' ' + convert(char(8), o.hora, 120) as fecha,o.total_orden,o.id_cliprov,C.razon_social, convert(char(10),o.fecha_entrega,120)+' '+convert(char(8),o.hora_entrega,120) as fEntrega,o.nombre_entrega,o.id_usuario_autoriza,o.factura,op.id_cliprov,c1.razon_social, " +
              "(select sum(z.importe) from Orden_Compra_Detalle z where z.id_empresa=o.id_empresa and z.id_taller=o.id_taller and z.no_orden=o.no_orden and z.id_orden=o.id_orden and estatus_imp='I')" +
              "from Orden_Compra_Encabezado o " +
              "inner join Cliprov c on c.id_cliprov = o.id_cliprov and c.tipo = 'P' inner join ordenes_reparacion op on op.no_orden=o.no_orden and op.id_empresa=o.id_empresa and op.id_taller=o.id_taller left join Cliprov c1 on c1.id_cliprov = op.id_cliprov and c1.tipo = 'C' " +
              "where o.id_empresa = " + idEmpresa.ToString() + " and o.id_taller = " + idTaller.ToString() + " and o.no_orden = " + noOrden.ToString() + " and id_orden=" + idCotizacion.ToString() + "  order by o.id_orden desc";
        return ejecuta.dataSet(sql);
    }

    internal object[] detalleOrdenCompra(int noOrden, int idEmpresa, int idTaller, int idCotizacion)
    {
        sql = "select tabla.id_detalle,tabla.id_refaccion,tabla.descripcion,tabla.cantidad,tabla.costo_unitario,tabla.porc_desc,tabla.importe_desc,tabla.importe,tabla.estatus,tabla.estatusPre,tabla.estatusRef," +
              "(select staDescripcion from Rafacciones_Estatus where staRefID = tabla.estatusRef) as descripEstatus,tabla.observacion,tabla.fSolicitud,tabla.fEntregaEstimada,tabla.fEntrega,tabla.procedencia " +
              "from(select o.id_detalle, o.id_refaccion, o.descripcion, o.cantidad, o.costo_unitario, o.porc_desc, o.importe_desc, o.importe," +
              "(select r.refEstatus from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as estatus," +
              "case (select r.refEstatus from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) when 'NA' then 'No Aplica' when 'EV' then 'Evaluación' when 'RP' THEN 'Reparación' when 'CO' then 'Compra' when 'CA' THEN 'Cancelada' when 'AP' then 'Aplicada' when 'AU' then 'Autorizada' else '' end as estatusPre," +
              "(select r.refEstatusSolicitud from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as estatusRef," +
              "(select r.observacion from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as observacion," +
              "(select convert(char(10), r.refFechSolicitud, 126) from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as fSolicitud," +
              "(select convert(char(10), r.refFechEntregaEst, 126) from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as fEntregaEstimada," +
              "(select convert(char(10), r.refFechEntregaReal, 126) from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as fEntrega,p.proc_Descrip as procedencia " +
              "from Orden_Compra_Detalle o " +
              "left join cat_Procedencia p on p.id_Proc=o.id_procedencia "+
              "where o.no_orden = " + noOrden.ToString() + " and o.id_empresa = " + idEmpresa.ToString() + " and o.id_taller = " + idTaller.ToString() + " and o.id_orden = " + idCotizacion.ToString() +") as tabla";
        return ejecuta.dataSet(sql);
    }
    internal object[] detalleOrdenCompraImpresion(int noOrden, int idEmpresa, int idTaller, int idCotizacion)
    {
        sql = "select tabla.id_detalle,tabla.id_refaccion,tabla.descripcion,tabla.cantidad,tabla.costo_unitario,tabla.porc_desc,tabla.importe_desc,tabla.importe,tabla.estatus,tabla.estatusPre,tabla.estatusRef," +
              "(select staDescripcion from Rafacciones_Estatus where staRefID = tabla.estatusRef) as descripEstatus,tabla.observacion,tabla.fSolicitud,tabla.fEntregaEstimada,tabla.fEntrega,tabla.procedencia " +
              "from(select o.id_detalle, o.id_refaccion, o.descripcion, o.cantidad, o.costo_unitario, o.porc_desc, o.importe_desc, o.importe," +
              "(select r.refEstatus from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as estatus," +
              "case (select r.refEstatus from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) when 'NA' then 'No Aplica' when 'EV' then 'Evaluación' when 'RP' THEN 'Reparación' when 'CO' then 'Compra' when 'CA' THEN 'Cancelada' when 'AP' then 'Aplicada' when 'AU' then 'Autorizada' else '' end as estatusPre," +
              "(select r.refEstatusSolicitud from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as estatusRef," +
              "(select r.observacion from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as observacion," +
              "(select convert(char(10), r.refFechSolicitud, 126) from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as fSolicitud," +
              "(select convert(char(10), r.refFechEntregaEst, 126) from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as fEntregaEstimada," +
              "(select convert(char(10), r.refFechEntregaReal, 126) from Refacciones_Orden r where r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.refOrd_Id = o.id_refaccion) as fEntrega,p.proc_Descrip as procedencia " +
              "from Orden_Compra_Detalle o " +
              "left join cat_Procedencia p on p.id_Proc=o.id_procedencia " +
              "where o.no_orden = " + noOrden.ToString() + " and o.id_empresa = " + idEmpresa.ToString() + " and o.id_taller = " + idTaller.ToString() + " and o.id_orden = " + idCotizacion.ToString() + " and o.estatus_imp='I') as tabla";
        return ejecuta.dataSet(sql);
    }
}