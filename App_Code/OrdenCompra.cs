using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;
using System.Data;

/// <summary>
/// Descripción breve de OrdenCompra
/// </summary>
public class OrdenCompra
{

    Ejecuciones ejecuta = new Ejecuciones();
    cuentasXPagar borra = new cuentasXPagar();

    Fechas fechas = new Fechas();
    public int[] sesiones { get; set; }
    public int orden { get; set; }
    public int idCfd { get; set; }
    public int id_orden { get; set; }
    public string factura { get; set; }
    public string imp { get; set; }
    public int empresa { get; set; }
    public int taller { get; set; }
    public int proveedor { get; set; }
    public int autoriza { get; set; }
    public int acceso { get; set; }
    public int refaccion { get; set; }
    public int numero_orden { get; set; }

    public object[] retorno { get; set; }
    private string sql;
    public OrdenCompra()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] generaOrden() {
        string sql = "generaOrdenCompra";
        return ejecuta.exeStoredOrdenCompra(sesiones, sql, proveedor, autoriza, acceso, fechas.obtieneFechaLocal());
    }

    public object[] obtieneProveedores()
    {
        string sql = "declare @orden int " +
"declare @empresa int " +
"declare @taller int " +
"declare @cotizacion int " +
"declare @acceso int " +
"set @orden=" + sesiones[4].ToString() +
" set @empresa=" + sesiones[2].ToString() +
" set @taller=" + sesiones[3].ToString() +
" set @cotizacion=" + sesiones[6].ToString() +
" set @acceso=" + acceso.ToString() +
" select tabla.refProveedor,tabla.razon_social,c.correo from(" +
" select distinct " +
"case @acceso when 0 then  " +
"case r.refEstatus when 'AU' THEN (case when r.refProveedor=0 then r.id_cliprov_cotizacion else r.refProveedor end ) else " +
"(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end " +
"else r.refProveedor end  as refProveedor, " +
"(select razon_social from Cliprov where tipo='P' and id_cliprov in (select case @acceso when 0 then  " +
"case r.refEstatus when 'AU' THEN (case when r.refProveedor=0 then r.id_cliprov_cotizacion else r.refProveedor end ) else " +
"(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end " +
"else r.refProveedor end)) as razon_social " +
"from Refacciones_Orden r " +
"where r.ref_no_orden=@orden and r.ref_id_empresa=@empresa and r.ref_id_taller=@taller and  " +
"refOrd_Id in (select distinct id_cotizacion_detalle from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=r.ref_id_taller and id_cotizacion=@cotizacion) " +
") as tabla inner join Cliprov c on c.id_cliprov = tabla.refProveedor and c.tipo = 'P'";
        return ejecuta.dataSet(sql);
    }

    public object[] actualizaCancelacion(string argumentos, int[] sesiones)
    {
        string sql = "update orden_compra_encabezado set estatus='CAN' where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_orden=" + argumentos;            
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneRefacciones(int orden)
    {
        string sql = "select id_refaccion from Orden_Compra_Detalle where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[4].ToString() + " and id_orden=" + orden.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneRefaccionesCot(int[] sesiones)
    {
        string sql = string.Format("select r.refOrd_Id,r.refCantidad,r.refDescripcion,r.refPrecioVenta,refProveedor,c.razon_social,r.refCosto,"
+ "isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion={3} and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0) as porc_desc,"
+ "isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion={3} and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0) as importeDesc,"
+ "isnull((SELECT importe from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion={3} and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0) as importeCosto,"
+ "isnull((r.refCantidad*r.refPrecioVenta),0) as importeVenta,r.refEstatus,"
+ "isnull((SELECT estatus from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion={3} and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),'CAN') as estatusRef,r.refEstatusSolicitud,"
+ "(select staDescripcion from rafacciones_estatus where staRefId=r.refEstatusSolicitud) as descripEstatus,"
+ "(isnull((r.refCantidad*r.refPrecioVenta),0)-isnull((SELECT importe from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion={3} and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ),0)) as utilidad,"
+ "r.refPorcentSobreCosto as porcSobre,"
+ "(select staDescripcion from rafacciones_estatus where starefid=r.refEstatusSolicitud) as estatusSoli,"
+ "r.refEstatusSolicitud,"
+ "case r.refEstatus when 'NA' then 'No Aplica' when 'EV' then 'Evaluación' when 'RP' THEN 'Reparación' when 'CO' then 'Compra' when 'CA' THEN 'Cancelada' when 'AP' then 'Aplicada' when 'AU' then 'Autorizada' else '' end as estatus "
+ "from Refacciones_Orden r "
+ "left join Cliprov c on c.id_cliprov=r.refProveedor and c.tipo='P' "
+ "where r.ref_no_orden={0} and r.ref_id_empresa={1} and r.ref_id_taller={2} and "
+ "refOrd_Id in (select distinct id_cotizacion_detalle from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=r.ref_id_taller and id_cotizacion={3})", sesiones[4], sesiones[2], sesiones[3], sesiones[6]);
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneInfoOrdenCompra(int empresa, int taller, int orden, string factura, decimal proveedor)
    {
        string sql = "select oe.no_orden, oe.id_taller,oe.id_empresa,od.id_refaccion as Renglon,oe.Fecha,od.Descripcion,oe.id_cliprov as Proveedor,od.Cantidad,od.Costo_Unitario,od.Importe,'Re' as Area_de_Aplicacion,oe.Factura,od.importe_desc as Descuento,'' as id_nota_credito " +
                    "from Orden_Compra_Encabezado oe, orden_compra_detalle od where oe.no_orden = " + orden + " and oe.id_taller = " + taller + " and oe.id_empresa = " + empresa + " and oe.factura = '" + factura + "' and oe.id_cliprov = " + proveedor + " and oe.no_orden = od.no_orden and oe.id_empresa = od.id_empresa and oe.id_taller = od.id_taller and oe.id_orden = od.id_orden";
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneProveedoresCompra()
    {
        string sql = "declare @orden int " +
"declare @empresa int " +
"declare @taller int " +
"declare @cotizacion int " +
"declare @acceso int " +
"set @orden=" + sesiones[4].ToString() +
" set @empresa=" + sesiones[2].ToString() +
" set @taller=" + sesiones[3].ToString() +
" set @acceso=" + acceso.ToString() +
" select tabla.refProveedor,tabla.razon_social,c.correo,tabla.id_cotizacion_autorizada from(" +
" select distinct " +
"case @acceso when 0 then  " +
"case r.refEstatus when 'AU' THEN (case when r.refProveedor=0 then r.id_cliprov_cotizacion else r.refProveedor end ) else " +
"(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end " +
"else r.refProveedor end  as refProveedor, " +
"(select razon_social from Cliprov where tipo='P' and id_cliprov in (select case @acceso when 0 then  " +
"case r.refEstatus when 'AU' THEN (case when r.refProveedor=0 then r.id_cliprov_cotizacion else r.refProveedor end ) else " +
"(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=r.id_cotizacion and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.id_cliprov_cotizacion ),0)) end " +
"else r.refProveedor end)) as razon_social,isnull(r.id_cotizacion_autorizada,0) as id_cotizacion_autorizada " +
"from Refacciones_Orden r " +
"where r.ref_no_orden=@orden and r.ref_id_empresa=@empresa and r.ref_id_taller=@taller and  " +
"refOrd_Id in (select distinct id_cotizacion_detalle from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=r.ref_id_taller and id_cotizacion=r.id_cotizacion_autorizada) " +
") as tabla inner join Cliprov c on c.id_cliprov = tabla.refProveedor and c.tipo = 'P' where tabla.refProveedor>0 and tabla.id_cotizacion_autorizada>0";
        return ejecuta.dataSet(sql);
    }

    public object[] agregaOrdenIndependiente(int[] sesiones, string folio, string factura, string fecha, string hora, string nombre, string proveedor)
    {
        string sql = string.Format("declare @ordenSiguiente as int set @ordenSiguiente=(SELECT ISNULL((SELECT TOP 1 id_orden FROM ORDEN_COMPRA_ENCABEZADO WHERE NO_ORDEN={0} AND id_empresa={1} AND ID_TALLER={2} ORDER BY ID_ORDEN DESC),0)+1) insert into orden_compra_encabezado values({0},{1},{2},@ordenSiguiente,0,'S/C','{3}','REC','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',{4},0,{5},'{6}','{7}','{8}',{5},'{9}') select @ordenSiguiente", sesiones[4], sesiones[2], sesiones[3], folio, proveedor, sesiones[0], fecha, hora, nombre, factura);
        return ejecuta.scalarToInt(sql);
    }

    public object[] agregaDetalleOrdenIndependiente(int[] sesiones, string ordenCompra, string detalle, string refaccion, string cantidad, string costo, string porcDesc, string importeDesc, string importe, string procedencia, string descripRefaccion)
    {
        string sql = "";
        decimal porcentaje = 0;
        try { porcentaje = Convert.ToDecimal(porcDesc); } catch (Exception) { porcentaje = 0; }
        if (detalle == "0" || detalle == "")
            sql = "insert into Orden_Compra_Detalle values(" + sesiones[4] + "," + sesiones[2] + "," + sesiones[3] + "," + ordenCompra + ",(select isnull((select top 1 id_detalle from orden_compra_detalle where no_orden=" + sesiones[4] + " and id_taller=" + sesiones[3] + " and id_empresa=" + sesiones[2] + " and id_orden=" + ordenCompra + " order by id_detalle desc),0)+1)," + refaccion + ",'" + descripRefaccion + "'," + cantidad + "," + costo + "," + porcentaje + "," + importeDesc + "," + importe + "," + procedencia + ",'N') " +
                " update refacciones_orden set refestatussolicitud=3, reffechsolicitud='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "', reffechentregaest='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',reffechentregareal='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' where ref_no_orden=" + sesiones[4] + " and ref_id_empresa=" + sesiones[2] + " and ref_id_taller=" + sesiones[3] + "and proceso is null and refDescripcion='"+descripRefaccion + "'";
        else
            sql = "update Orden_Compra_Detalle set cantidad=" + cantidad + ", costo_unitario=" + costo + ", porc_desc=" + porcentaje + ",importe_desc=" + importeDesc + ",importe=" + importe + ",id_procedencia=" + procedencia + " where no_orden=" + sesiones[4] + " and id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and id_orden=" + ordenCompra + " and id_detalle=" + detalle +
                " update refacciones_orden set refestatussolicitud=3, reffechsolicitud='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "', reffechentregaest='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',reffechentregareal='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' where ref_no_orden=" + sesiones[4] + " and ref_id_empresa=" + sesiones[2] + " and ref_id_taller=" + sesiones[3] + "and proceso is null and refDescripcion='" + descripRefaccion + "'";

        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneInfoOrdenDetalle(int[] sesiones, string ordenCompra, string detalle)
    {
        string sql = "select * from orden_compra_detalle where no_orden=" + sesiones[4] + " and id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and id_orden=" + ordenCompra + " and id_detalle=" + detalle;
        return ejecuta.dataSet(sql);
    }

    public object[] existeRefaccionOrden(int[] sesiones, string refaccion)
    {
        if (refaccion == "")
        {
            refaccion = "0";
        }
        string sql = "select count(*) from orden_compra_detalle where no_orden=" + sesiones[4] + " and id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and id_refaccion=" + refaccion;
        return ejecuta.scalarToBool(sql);
    }

    public void actualizaTotalEncabezado(int[] sesiones, string ordenCompra) {
        string sql = "declare @total decimal(15,2) set @total=(select isnull((select sum(importe) from orden_compra_detalle where no_orden=" + sesiones[4] + " and id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and id_orden=" + ordenCompra + "),0)) update Orden_Compra_Encabezado set total_orden=@total where no_orden=" + sesiones[4] + " and id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and id_orden=" + ordenCompra;
        ejecuta.insertUpdateDelete(sql);
    }

    public object[] agregaRefaccion(int[] sesiones, string refaccion, string canitidad, string costo, string desceunto, string importe, string proveedor)
    {
        decimal precio = Convert.ToDecimal(canitidad) * Convert.ToDecimal(costo);
        string sql = "INSERT INTO[Refacciones_Orden] ([refDescripcion], [refCantidad], [refObservaciones], refProveedor, refCosto, [ref_no_orden], ref_id_empresa, ref_id_taller, refPorcentSobreCosto, refEstatus, refEstatusSolicitud, refPrecioVenta,precio_venta_ini,precio_venta_proove,estatus_presupuesto,proceso)" +
            " VALUES('" + refaccion + "', " + canitidad + ", '', " + proveedor + ", " + costo + ", " + sesiones[4] + ", " + sesiones[2] + ", " + sesiones[3] + ", 0, 'CO',3," + precio.ToString() + "," + precio.ToString() + "," + precio.ToString() + ",'P','C') SELECT @@IDENTITY";
        return ejecuta.scalarToInt(sql);
    }

    public object[] actualizaPrecioVenta(int[] sesiones, int id, decimal porcentaje, decimal importeVentaUnitario)
    {
        string sql = string.Format("update refacciones_orden set refPrecioVenta={3}, refPorcentSobreCosto={4} where ref_no_orden={0} and ref_id_empresa={1} and ref_id_taller={2} and refOrd_Id={5}", sesiones[4], sesiones[2], sesiones[3], importeVentaUnitario, porcentaje, id);
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaOrden(int[] sesiones, int usuarioAutoriza)
    {         
        string sql = "update orden_compra_encabezado set estatus='REC', id_usuario_autoriza = "+usuarioAutoriza.ToString()+" where no_orden="+sesiones[4].ToString()+" and id_empresa="+sesiones[2].ToString()+" and id_taller="+sesiones[3].ToString()+" and id_orden="+sesiones[6].ToString();
        return ejecuta.insertUpdateDelete(sql);
    }
     

    public object[] obtieneCorreoProveedor(int[] sesiones)
    {
        string sql = "select c.id_cliprov,c.razon_social,c.correo from Cliprov c where c.tipo='P' and c.id_cliprov in (select id_cliprov from Orden_Compra_Encabezado where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_orden=" + sesiones[6].ToString() + ")";
        return ejecuta.dataSet(sql);
    }

    public object[] ordenAutorizada()
    {
        string sql = "select isnull(id_usuario_autoriza,0) from orden_compra_encabezado where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_orden=" + sesiones[6].ToString();
        return ejecuta.scalarToBoolLog(sql);
    }

    public void actualizaRefacciones(int proveedor, int ordenGenerada)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();

        string sql = "select id_cotizacion_detalle,dias_entrega from cotizacion_detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cliprov=" + proveedor.ToString() + " and id_cotizacion_detalle in (select id_refaccion from orden_compra_detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_orden=" + ordenGenerada.ToString() + ")";
        object[] info = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(info[0])) {
            DataSet datos = (DataSet)info[1];
            foreach (DataRow fila in datos.Tables[0].Rows) {
                DateTime fecha = fechas.obtieneFechaLocal();
                double dias = Convert.ToDouble(fila[1].ToString());
                DateTime fEntrega = fecha.AddDays(dias);

                sql = "update Refacciones_Orden set refEstatusSolicitud=2, refFechSolicitud = '" + fecha.ToString("yyyy-MM-dd") + "', refFechEntregaEst = '"+fEntrega.ToString("yyyy-MM-dd")+"' where ref_no_orden=" + sesiones[4].ToString() + " and ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " and refProveedor=" + proveedor.ToString() + " and refOrd_Id = " + fila[0].ToString();                    
                object[] actualizado = ejecuta.insertUpdateDelete(sql);                
            }
            sql = " update Cotizacion_Detalle set estatus='SOL' where id_empresa=" + sesiones[2].ToString() + " AND id_taller=" + sesiones[3].ToString() + " AND no_orden=" + sesiones[4].ToString() + " AND id_cliprov=" + proveedor.ToString() + " AND id_cotizacion=" + sesiones[6].ToString();
            object[] fin = ejecuta.insertUpdateDelete(sql);
        }
    }

    public object[] agregaManoObra(int[] sesiones, string manoObra, string costo)
    {
        string sql = "insert into mano_obra values (" + sesiones[2] + "," + sesiones[3] + "," + sesiones[4] + ",(select isnull(max(mo.consecutivo),0) from mano_obra mo where id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and no_orden=" + sesiones[4] + ")+1,0,0,lower('" + manoObra + "')," + costo + "," + costo + ",0,'P','P',0,0,0,0,0,0,null,null)";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneFotosRefaccion(int noOrden, int idEmpresa, int idTaller, int idRefaccion)
    {
        string sql = "select nombre_imagen,imagen from fotos_refacciones " +
            " where no_orden =" + noOrden.ToString() + "  and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and id_refaccion = " + idRefaccion.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneInfoOrden(int empresa, int taller, int orden, int compra)
    {
        string sql = "select id_cliprov,total_orden from orden_compra_encabezado where no_orden=" + orden + " and id_taller=" + taller + " and id_empresa=" + empresa + " and id_orden=" + compra;
        return ejecuta.dataSet(sql);
    }

    public decimal obtieneTotalOrden(int[] sesiones, string ordenCompra)
    {
        decimal valor = 0;
        string sql = "declare @total decimal(15,2) set @total=(select isnull((select sum(importe) from orden_compra_detalle where no_orden=" + sesiones[4] + " and id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and id_orden=" + ordenCompra + "),0)) select @total";
        object[] ret = ejecuta.scalarToDecimal(sql);
        try
        {
            if (Convert.ToBoolean(ret[0]))
                valor = Convert.ToDecimal(ret[1]);
            else
                valor = 0;
        }
        catch (Exception) { valor = 0; }
        return valor;
    }

    public object[] eliminaDetalleOrdenIndependiente(int[] sesiones, string ordenCompra, int idDetalle, int idRefaccion)
    {
        string sql = "delete from orden_compra_detalle where id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and no_orden=" + sesiones[4] + " and id_orden=" + ordenCompra + " and id_detalle=" + idDetalle + " and id_refaccion=" + idRefaccion +
            " delete from refacciones_orden where ref_id_empresa=" + sesiones[2] + " and ref_id_taller=" + sesiones[3] + " and ref_no_orden=" + sesiones[4] + " and reford_id=" + idRefaccion;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaDetalleOrdenIndependiente(int[] sesiones, string ordenCompra, string detalle, string refaccion, string cantidad, string costo, string porcDesc, string importeDesc, string importe, string procedencia, string descripRefaccion)
    {
        string sql = "";
        decimal porcentaje = 0;
        try { porcentaje = Convert.ToDecimal(porcDesc); } catch (Exception) { porcentaje = 0; }        
        sql = "update Orden_Compra_Detalle set cantidad=" + cantidad + ", costo_unitario=" + costo + ", porc_desc=" + porcentaje + ",importe_desc=" + importeDesc + ",importe=" + importe + ",id_procedencia=" + procedencia + " where no_orden=" + sesiones[4] + " and id_empresa=" + sesiones[2] + " and id_taller=" + sesiones[3] + " and id_orden=" + ordenCompra + " and id_detalle=" + detalle+
            " update refacciones_orden set refdescripcion = '"+descripRefaccion+"', refcantidad = " +cantidad + ", refcosto = " + costo + ",refprecioventa = " + costo  +", precio_venta_ini = " + costo + ",precio_venta_proove = " + costo + " where ref_no_orden = " + sesiones[4] + " and ref_id_empresa = " + sesiones[2] + " and ref_id_taller = " + sesiones[3] + " and reford_id = " + refaccion;
        return ejecuta.insertUpdateDelete(sql);
    }

    public void recuperafolio()
    {
        sql = "(select isnull((select top 1 id_orden from Orden_Compra_Encabezado where id_empresa="+empresa+" and id_taller="+taller+" and no_orden ="+orden+" order by id_orden desc),0)+1)";
        retorno = ejecuta.scalarToInt(sql);
    }

    public void recuperaEstatusmos()
    {
        sql = "select folioCotizacion from orden_compra_encabezado where id_empresa= " + empresa + " and id_taller = " + taller + " and no_orden="+orden;
        retorno = ejecuta.dataSet(sql);
    }

    public void eliminaDetalle()
    {
        sql = "delete from orden_compra_detalle where id_empresa= " + empresa + " and id_taller = " + taller + " and no_orden=" + orden+ " and id_orden="+numero_orden;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void eliminaEnacabezado()
    {
        sql = "delete from Orden_Compra_Encabezado where id_empresa= " + empresa + " and id_taller = " + taller + " and no_orden=" + orden + " and id_orden=" + numero_orden;
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void RecuperarFactura()
    {
        sql = "select folio_orden from Orden_Compra_Encabezado where id_empresa= " + empresa + " and id_taller = " + taller + " and no_orden=" + orden + " and id_orden=" + numero_orden;
        retorno = ejecuta.dataSet(sql);
    }
    
    public void borraEncDetaFactura()
    {
        sql = "delete from facturas where id_empresa= " + empresa + " and id_taller = " + taller + " and factura='" + factura+"' and idCfd="+idCfd;
        retorno = borra.insertUpdateDelete(sql);
    }

    public void CambiarEstatusI()
    {
        sql = "Update  orden_compra_detalle set estatus_imp='I' where id_empresa= " + empresa + " and id_taller = " + taller + " and no_orden=" + orden + " and id_refaccion=" + refaccion;
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void CambiarEstatusN()
    {
        sql = "Update  orden_compra_detalle set estatus_imp='N' where id_empresa= " + empresa + " and id_taller = " + taller + " and no_orden=" + orden + " and id_refaccion=" + refaccion;
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void eliminaRefaccionOrden(string descripcion)
    {
        sql = "delete from   refacciones_orden where ref_no_orden=" + orden + " and ref_id_empresa=" + empresa + " and ref_id_taller=" + taller + " and proceso='C' and refDescripcion='" + descripcion + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void traeDescripciones()
    {
        sql = "select descripcion from orden_compra_detalle where id_empresa= " + empresa + " and id_taller = " + taller + " and no_orden = " + orden + " and id_orden = " + numero_orden;
        retorno = ejecuta.dataSet(sql);
    }
    public void cambiaEstatusRefaccion(string descripcion)
    {
        sql = "update refacciones_orden set refEstatusSolicitud=1 where ref_no_orden=" + orden + " and ref_id_empresa=" + empresa + " and ref_id_taller=" + taller + " and proceso is null and refDescripcion='" + descripcion + "'";
        retorno = ejecuta.dataSet(sql);
    }
    public void obtieneEncabezadoOrdenCompra()
    {
        sql = "select * from orden_compra_encabezado where no_orden=" + orden + " and id_empresa=" + empresa + " and id_taller=" + taller + " and id_orden=" + numero_orden;
        retorno = ejecuta.dataSet(sql);
    }
    public void ordenesPendientesDeEntrega()
    {
        sql = "select folio_orden,id_orden from orden_compra_encabezado  where no_orden=" + orden + " and id_empresa=" + empresa + " and id_taller=" + taller + " and fecha_entrega is null";
        retorno = ejecuta.dataSet(sql);
    }
    public void obtieneRefOrdId()
    {
        sql = "select id_refaccion from orden_compra_detalle where no_orden=" + orden + " and id_empresa=" + empresa +" and id_taller=" + taller + " and id_orden=" + id_orden;
        retorno = ejecuta.dataSet(sql);
    }
    public void fechaPendiente()
    {
        sql = "select refFechEntregaEst,refdescripcion from  refacciones_orden where ref_no_orden=" + orden + " and ref_id_empresa=" + empresa + " and ref_id_taller=" + taller + " and refOrd_Id=" + refaccion;
        retorno = ejecuta.dataSet(sql);
    }
}