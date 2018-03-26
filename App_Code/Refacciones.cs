using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Descripción breve de Refacciones
/// </summary>
/// 


public class Refacciones
{

    public int _orden { get; set; }
    public int _empresa { get; set; }
    public int _taller { get; set; }
    public int _cotizacion { get; set; }
    public int _proveedor { get; set; }
    public int _refaccion { get; set; }
    public int _cantidad { get; set; }
    public string _descripcion { get; set; }
    public decimal _costo { get; set; }
    public decimal _porcentajeDescuento { get; set; }
    public decimal _importeDescuento { get; set; }
    public decimal _importe { get; set; }
    public bool _existencia { get; set; }
    public int _dias { get; set; }
    public int _procedencia { get; set; }
    public object[] retorno { get; set; }

   

	public Refacciones()
	{
        retorno = new object[] { false, "" };
	}

    public List<Refacciones> obtieneRefacciones() {
        List<Refacciones> refacciones = new List<Refacciones>();
        datosCotizaProv cotizacion = new datosCotizaProv();
        object[] refaccionesCotizar = cotizacion.obtieneDetalleCotizacion(_empresa, _taller, _orden, _cotizacion);
        if (Convert.ToBoolean(refaccionesCotizar[0])) {
            DataSet refCot = (DataSet)refaccionesCotizar[1];
            foreach (DataRow fila in refCot.Tables[0].Rows) {
                refacciones.Add(new Refacciones() { _refaccion = Convert.ToInt32(fila[0].ToString()), _cantidad = Convert.ToInt32(fila[1].ToString()), _descripcion = fila[2].ToString(), _costo = Convert.ToDecimal(fila[3].ToString()), _porcentajeDescuento = Convert.ToDecimal(fila[4].ToString()), _importeDescuento = Convert.ToDecimal(fila[5].ToString()), _importe = Convert.ToDecimal(fila[6].ToString()), _existencia = Convert.ToBoolean(fila[7].ToString()), _dias = Convert.ToInt32(fila[8].ToString()), _proveedor = Convert.ToInt32(fila[9].ToString()) });
            }
        }
        else
            refacciones = null;

        return refacciones;
    }

    public List<Refacciones> obtieneRefaccionesCotiza()
    {
        List<Refacciones> refacciones = new List<Refacciones>();
        datosCotizaProv cotizacion = new datosCotizaProv();
        object[] refaccionesCotizar = cotizacion.obtieneDetalleCotizacionCotiza(_empresa, _taller, _orden, _cotizacion,_proveedor);
        if (Convert.ToBoolean(refaccionesCotizar[0]))
        {
            DataSet refCot = (DataSet)refaccionesCotizar[1];
            foreach (DataRow fila in refCot.Tables[0].Rows)
            {
                refacciones.Add(new Refacciones() { _refaccion = Convert.ToInt32(fila[0].ToString()), _cantidad = Convert.ToInt32(fila[1].ToString()), _descripcion = fila[2].ToString(), _costo = Convert.ToDecimal(fila[4].ToString()), _porcentajeDescuento = Convert.ToDecimal(fila[5].ToString()), _importeDescuento = Convert.ToDecimal(fila[6].ToString()), _importe = Convert.ToDecimal(fila[7].ToString()), _existencia = Convert.ToBoolean(fila[8].ToString()), _dias = Convert.ToInt32(fila[9].ToString()), _proveedor = Convert.ToInt32(fila[10].ToString()) });
            }
        }
        else
            refacciones = null;

        return refacciones;
    }

    public void actualizaFacturados(string idcfd, string empresa, string taller, string orden, string status)
    {
        Ejecuciones ejecuta = new Ejecuciones();
        DataSet ds = new DataSet();
        try
        {
            string qrySelect = "";
            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString))
            {
                try
                {
                    conLoc.Open();
                    qrySelect = "select * from DetCFD where IdCfd=" + idcfd;

                    using (conLoc)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand(qrySelect, conLoc);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(ds);
                        }
                        catch (Exception ex)
                        {
                            ds = null;
                        }
                        finally
                        {
                            conLoc.Close();
                        }
                    }
                }
                catch (Exception ex) { ds = null; }
            }

            int clave = 0;
            string sql = "";
            foreach (DataRow fila in ds.Tables[0].Rows) {
                string[] concepto = fila[3].ToString().Split(new char[] { '-' });
                try { 
                    clave = Convert.ToInt32(concepto[1]);
                    
                    string actual = "";
                    if (status != "C")
                        actual = "idCfd=" + idcfd + ", statusCfd='" + status + "'";
                    else
                        actual = "idCfd=0 , statusCfd='P'";

                    if (concepto[0] == "MO")
                        sql = "update mano_obra set " + actual + " where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden + " and consecutivo=" + clave.ToString();
                    else if (concepto[0] == "REF")
                        sql = "update refacciones_orden set " + actual + " where ref_id_empresa=" + empresa + " and ref_id_taller=" + taller + " and ref_no_orden=" + orden + " and refOrd_id=" + clave.ToString();
                    if (sql != "") {
                        ejecuta.insertUpdateDelete(sql);
                    }
                }
                catch (Exception ex) { }
            }

            sql = "update ordenes_reparacion set status_orden='F' WHERE id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden;
            ejecuta.insertUpdateDelete(sql);

            /*sql = "declare @empresa int declare @taller int declare @orden int set @empresa = " + empresa + " set @taller = " + taller + " set @orden = " + orden + " select(tabla.Mo - tabla.FactMo) + (tabla.Ref - tabla.FactRef) as restan from(select (select count(*) as facturadosMo from mano_obra where id_empresa = @empresa and id_taller = @taller and no_orden = @orden and idcfd > 0) as FactMo, (select count(*) as factruadosRef from refacciones_orden where ref_id_empresa = @empresa and ref_id_taller = @taller and ref_no_orden = @orden and idcfd > 0) as FactRef, (select count(*) as facturadosMo from mano_obra where id_empresa = @empresa and id_taller = @taller and no_orden = @orden) as Mo, (select count(*) as factruadosRef from refacciones_orden where ref_id_empresa = @empresa and ref_id_taller = @taller and ref_no_orden = @orden) as Ref )as tabla";
            object[] pendientes = ejecuta.scalarToInt(sql);
            if (Convert.ToBoolean(pendientes[0])) {
                int restantes = Convert.ToInt32(pendientes[1]);
                if (restantes == 0)
                {
                    sql = "update ordenes_reparacion set status_orden='F' WHERE id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden;
                    ejecuta.insertUpdateDelete(sql);
                }
            }*/
        }
        catch (Exception ex)
        {
            
        }
    }

    public string obtieneFechaMinEntEstimada()
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = "SELECT ISNULL((select top 1 CONVERT(CHAR(10),refFechEntregaEst,120) from refacciones_orden where ref_no_orden=" + _orden + " and ref_id_empresa=" + _empresa + " and ref_id_taller=" + _taller + " and refProveedor=" + _proveedor + " order by refFechEntregaEst asc),'')";
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] obtieneCantidad()
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = "select refCantidad from Refacciones_Orden where ref_no_orden=" + _orden + " and ref_id_taller=" + _taller + " and ref_id_empresa=" + _empresa + " and refOrd_Id=" + _refaccion;
        return ejecuta.scalarToInt(sql);
    }

    internal object[] obtienerefaccionesc(int empresa, int taller, int ordenT)
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = string.Format("select tabla.refOrd_id,tabla.refDescripcion,tabla.refCantidad,tabla.refProveedor,c.razon_social,tabla.refCosto,tabla.refprecioVenta,tabla.refprecioVenta*tabla.refcantidad as importe,tabla.refEstatus,tabla.refestatussolicitud,tabla.staDescripcion," +
"tabla.reffechSolicitud, tabla.reffechEntregaEst, tabla.refFechEntregaReal, tabla.Compra from("+
"select ro.refOrd_id, ro.refDescripcion, ro.refCantidad,case ro.refProveedor when 0 then ro.id_cliprov_cotizacion else ro.refproveedor end as refProveedor, ro.refCosto, ro.refprecioVenta, (ro.refcantidad * ro.refprecioVenta) as importe,"+
"ro.refEstatus, ro.refestatussolicitud, re.staDescripcion, ro.reffechSolicitud, ro.reffechEntregaEst, ro.refFechEntregaReal,isnull((select sum(o.importe) from orden_compra_detalle o "+
"inner join refacciones_orden r on r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.reford_id = o.id_refaccion "+
"where o.no_orden = ro.ref_no_orden and o.id_empresa = ro.ref_id_empresa and o.id_taller = ro.ref_id_taller and r.reford_id = ro.reford_id), 0) as Compra , proceso "+
"from refacciones_orden ro left join Rafacciones_Estatus re on re.staRefID = ro.refEstatusSolicitud where ro.ref_no_orden = {2} and ro.ref_id_empresa = {0} and ro.ref_id_taller = {1} and ro.refEstatusSolicitud=3) as tabla left join cliprov c on c.id_cliprov = tabla.refproveedor and c.tipo = 'P'", empresa, taller, ordenT);

/*        string sql = string.Format("select tabla.refOrd_id,tabla.refDescripcion,tabla.refCantidad,tabla.refProveedor,c.razon_social,tabla.refCosto,tabla.refprecioVenta,case when tabla.proceso='C' then 0 else tabla.refprecioVenta*tabla.refcantidad end as importe,tabla.refEstatus,tabla.refestatussolicitud,tabla.staDescripcion," +
"tabla.reffechSolicitud, tabla.reffechEntregaEst, tabla.refFechEntregaReal, tabla.Compra from(" +
"select ro.refOrd_id, ro.refDescripcion, ro.refCantidad,case ro.refProveedor when 0 then ro.id_cliprov_cotizacion else ro.refproveedor end as refProveedor, ro.refCosto, ro.refprecioVenta, (ro.refcantidad * ro.refprecioVenta) as importe," +
"ro.refEstatus, ro.refestatussolicitud, re.staDescripcion, ro.reffechSolicitud, ro.reffechEntregaEst, ro.refFechEntregaReal,isnull((select sum(o.importe) from orden_compra_detalle o " +
"inner join refacciones_orden r on r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.reford_id = o.id_refaccion " +
"where o.no_orden = ro.ref_no_orden and o.id_empresa = ro.ref_id_empresa and o.id_taller = ro.ref_id_taller and r.reford_id = ro.reford_id), 0) as Compra , proceso " +
"from refacciones_orden ro left join Rafacciones_Estatus re on re.staRefID = ro.refEstatusSolicitud where ro.ref_no_orden = {2} and ro.ref_id_empresa = {0} and ro.ref_id_taller = {1} and ro.proceso is null) as tabla left join cliprov c on c.id_cliprov = tabla.refproveedor and c.tipo = 'P'", empresa, taller, ordenT);*/

        /*select ro.refOrd_id,ro.refDescripcion,ro.refCantidad,ro.refProveedor,c.razon_social,ro.refCosto,ro.refprecioVenta,(ro.refcantidad * ro.refprecioVenta) as importe, 
                                                                ro.refEstatus,ro.refestatussolicitud,re.staDescripcion,ro.reffechSolicitud,ro.reffechEntregaEst,ro.refFechEntregaReal
                                                                from refacciones_orden ro
                                                                left join cliprov c on c.id_cliprov = ro.refproveedor and c.tipo = 'P'
                                                                left join Rafacciones_Estatus re on re.staRefID = ro.refEstatusSolicitud
                                                                where ro.ref_no_orden = @orden and ro.ref_id_empresa = @empresa and ro.ref_id_taller = @taller and ro.refEstatusSolicitud = 3*/
        return ejecuta.dataSet(sql);
    }
    public void obtieneInfoObra()
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = "select tabla.refOrd_id,tabla.refDescripcion,tabla.refCantidad,tabla.refProveedor,c.razon_social,tabla.refCosto,tabla.refprecioVenta,tabla.refprecioVenta*tabla.refcantidad as importe,tabla.refEstatus,tabla.refestatussolicitud,tabla.staDescripcion," +
"tabla.reffechSolicitud, tabla.reffechEntregaEst, tabla.refFechEntregaReal, tabla.Compra from("+
"select ro.refOrd_id, ro.refDescripcion, ro.refCantidad,case ro.refProveedor when 0 then ro.id_cliprov_cotizacion else ro.refproveedor end as refProveedor, ro.refCosto, ro.refprecioVenta, (ro.refcantidad * ro.refprecioVenta) as importe,"+
"ro.refEstatus, ro.refestatussolicitud, re.staDescripcion, ro.reffechSolicitud, ro.reffechEntregaEst, ro.refFechEntregaReal,isnull((select sum(o.importe) from orden_compra_detalle o "+
"inner join refacciones_orden r on r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.reford_id = o.id_refaccion "+
"where o.no_orden = ro.ref_no_orden and o.id_empresa = ro.ref_id_empresa and o.id_taller = ro.ref_id_taller and r.reford_id = ro.reford_id), 0) as Compra , proceso "+
"from refacciones_orden ro left join Rafacciones_Estatus re on re.staRefID = ro.refEstatusSolicitud where ro.ref_no_orden = {2} and ro.ref_id_empresa = {0} and ro.ref_id_taller = {1} and ro.refEstatusSolicitud=3) as tabla left join cliprov c on c.id_cliprov = tabla.refproveedor and c.tipo = 'P'";
        retorno = ejecuta.dataSet(sql);
    }
}

public class RefProvCotiza
{
    public int _orden { get; set; }
    public int _empresa { get; set; }
    public int _taller { get; set; }
    public int _cotizacion { get; set; }
    public int _proveedor { get; set; }
    public int _refaccion { get; set; }
    public string _refProced { get; set; }
    public int _cantidad { get; set; }
    public string _descripcion { get; set; }
    public decimal _costo { get; set; }
    public decimal _porcentajeDescuento { get; set; }
    public decimal _importeDescuento { get; set; }
    public decimal _importe { get; set; }
    public bool _existencia { get; set; }
    public int _dias { get; set; }

    public RefProvCotiza()
    { }

    public List<RefProvCotiza> obtieneRefaccionesCotiza()
    {
        List<RefProvCotiza> refacciones = new List<RefProvCotiza>();
        datosCotizaProv cotizacion = new datosCotizaProv();
        object[] refaccionesCotizar = cotizacion.obtieneDetalleCotizacionCotiza(_empresa, _taller, _orden, _cotizacion, _proveedor);
        if (Convert.ToBoolean(refaccionesCotizar[0]))
        {
            DataSet refCot = (DataSet)refaccionesCotizar[1];
            foreach (DataRow fila in refCot.Tables[0].Rows)
            {
                refacciones.Add(new RefProvCotiza() { _refaccion = Convert.ToInt32(fila[0].ToString()), _cantidad = Convert.ToInt32(fila[1].ToString()), _descripcion = fila[2].ToString(), _refProced = string.IsNullOrEmpty(fila[3].ToString()) ? "s/p" : fila[3].ToString(), _costo = Convert.ToDecimal(fila[4].ToString()), _porcentajeDescuento = Convert.ToDecimal(fila[5].ToString()), _importeDescuento = Convert.ToDecimal(fila[6].ToString()), _importe = Convert.ToDecimal(fila[7].ToString()), _existencia = Convert.ToBoolean(fila[8].ToString()), _dias = Convert.ToInt32(fila[9].ToString()), _proveedor = Convert.ToInt32(fila[10].ToString()) });
            }
        }
        else
            refacciones = null;

        return refacciones;
    }
}