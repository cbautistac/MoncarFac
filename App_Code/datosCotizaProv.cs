using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de datosCotizaProv
/// </summary>
public class datosCotizaProv
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    string sql;
    object[] resultado = new object[2];
    DataSet data = new DataSet();
    public datosCotizaProv()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool agregarProvARef(int noOrde, int idEmpresa, int idTaller, int idProv, int folio)
    {
        sql = "insert into Proveedor_Cotizacion_Tmp values (" + noOrde.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + "," + folio.ToString() + "," + idProv.ToString() + ")";
        resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public bool quitaProveedorCotizacion(int noOrde, int idEmpresa, int idTaller, int idProv, int folio)
    {
        sql = "delete from Proveedor_Cotizacion_Tmp where no_orden=" + noOrde.ToString() + " and id_taller=" + idTaller.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and folio=" + folio.ToString();
        resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public bool insertaEncabezadoCotizacion(int noOrde, int idEmpresa, int idTaller, string folioCot, int refTotales)
    {
        sql = "insert into cotizacion_encabezado values(noo,idta,idem,(select isnull((select top 1 c.id_cotizacion from cotizacion_encabezado c " +
              " where c.no_orden = " + noOrde.ToString() + " and c.id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " order by c.id_cotizacion desc),0)+1)," +
              " '" + folioCot + "',0,0,0," + refTotales.ToString() + ",'AC')";
        resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public int obtieneRefTotales(int noOrde, int idEmpresa, int idTaller)
    {
        sql = "select count(*) from refacciones_orden where ref_no_orden=" + noOrde.ToString() + " and ref_id_empresa=" + idEmpresa.ToString() + " and ref_id_taller=" + idTaller.ToString() + " and refCotizacionEstatus=0 and refEstatus in('NA','CO')";
        resultado = ejecuta.scalarToInt(sql);
        if ((bool)resultado[0])
            return Convert.ToInt32(resultado[1]);
        else
            return 0;
    }

    public object[] obtieneFolio(int empresa, int taller, int orden)
    {
        sql = "select isnull((select top 1 folio from Proveedor_Cotizacion_Tmp where no_orden=" + orden.ToString() + " and id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " order by folio desc),0)+1";
        return ejecuta.scalarToInt(sql);
    }

    public object[] obtieneProveedores(int empresa, int taller, int orden)
    {
        sql = "select p.id_cliprov,c.razon_social,p.folio,c.correo from Proveedor_Cotizacion_Tmp p inner join Cliprov c on c.id_cliprov=p.id_cliprov and c.tipo='P' where p.no_orden=" + orden.ToString() + " and p.id_taller =" + taller.ToString() + " and p.id_empresa=" + empresa.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] generaCot(int empresa, int taller, int orden, int cotizacion, int proveedor, string folio)
    {
        sql = "cotizacion";
        return ejecuta.exeStoredCotizacion(sql, empresa, taller, orden, cotizacion, proveedor, folio, fechas.obtieneFechaLocal());
    }

    public object[] obtieneDetalleCotizacion(int empresa, int taller, int orden, int cotizacion)
    {
        sql = "select id_cotizacion_detalle,cantidad,descripcion,costo_unitario,porc_desc,importe_desc,importe,existencia,dias_entrega,id_cliprov from Cotizacion_detalle where no_orden=" + orden.ToString() + " and id_taller =" + taller.ToString() + " and id_empresa=" + empresa.ToString() + " and id_cotizacion=" + cotizacion.ToString() + " and estatus='PEN'";
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneDetalleCotizacionCotiza(int empresa, int taller, int orden, int cotizacion, int proveedor)
    {
        sql = "SELECT cd.id_cotizacion_detalle,cantidad,descripcion,cp.proc_Descrip,costo_unitario,porc_desc,importe_desc,importe,existencia,dias_entrega,id_cliprov FROM Cotizacion_detalle AS cd " +
                "LEFT JOIN Refacciones_Orden ro ON refOrd_id = cd.id_cotizacion_detalle LEFT JOIN cat_Procedencia cp ON cp.id_Proc = ro.id_Procedencia " +
                "WHERE no_orden=" + orden + " and id_taller=" + taller + " and id_empresa=" + empresa + " and cd.id_cotizacion=" + cotizacion + " and estatus= 'PEN' and id_cliprov=" + proveedor;
        return ejecuta.dataSet(sql);
    }

    public object[] actualizaRefaccionCotiza(int[] sesiones, Refacciones refacc)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        int val = 0;
        if (Convert.ToBoolean(refacc._existencia))
            val = 1;
        else
            val = 0;
        string estatus = "P";
        if (refacc._costo != 0)
            estatus = "R";
        else
            estatus = "D";

        sql = "update cotizacion_detalle set costo_unitario=" + refacc._costo.ToString() + ",porc_desc=" + refacc._porcentajeDescuento.ToString() + ",importe_desc=" + refacc._importeDescuento.ToString() + ",importe=" + refacc._importe.ToString() + ",estatus='COT',fecha_captura='" + fechaRetorno + "', existencia=" + val + ", dias_entrega=" + refacc._dias + ", estatus_proveedor='" + estatus + "' where no_orden=" + sesiones[0].ToString() + " and id_empresa=" + sesiones[1].ToString() + " and id_taller=" + sesiones[2].ToString() + " and id_cotizacion=" + sesiones[3].ToString() + " and id_cotizacion_detalle=" + refacc._refaccion.ToString() + " and id_cliprov=" + sesiones[4].ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaRefaccionCotizaRef(int[] sesiones, Refacciones refacc, string fechaLimite)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        int val = 0;
        if (Convert.ToBoolean(refacc._existencia))
            val = 1;
        else
            val = 0;
        string estatus = "P";
        if (refacc._costo != 0)
            estatus = "R";
        else
            estatus = "D";

        sql = "update cotizacion_detalle set costo_unitario=" + refacc._costo.ToString() + ",porc_desc=" + refacc._porcentajeDescuento.ToString() + ",importe_desc=" + refacc._importeDescuento.ToString() + ",importe=" + refacc._importe.ToString() + ",estatus='COT',fecha_captura='" + fechaRetorno + "', existencia=" + val + ", dias_entrega=" + refacc._dias + ", estatus_proveedor='" + estatus + "', id_Procedencia=" + refacc._procedencia + " where no_orden=" + sesiones[0].ToString() + " and id_empresa=" + sesiones[1].ToString() + " and id_taller=" + sesiones[2].ToString() + " and id_cotizacion=" + sesiones[3].ToString() + " and id_cotizacion_detalle=" + refacc._refaccion.ToString() + " and id_cliprov=" + sesiones[4].ToString() +
              " update cotizacion_detalle set fecha_limite='" + fechaLimite + "' where no_orden=" + sesiones[0] + " and id_taller=" + sesiones[2] + " and id_empresa=" + sesiones[1] + " and id_cotizacion=" + sesiones[3] + " and id_cliprov=" + sesiones[4] + " and id_cotizacion_detalle=" + refacc._refaccion.ToString(); 
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaRefaccion(int[] sesiones, Refacciones refacc)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        int val = 0;
        if (Convert.ToBoolean(refacc._existencia))
            val = 1;
        else
            val = 0;
        string estatus = "P";
        if (refacc._costo != 0)
            estatus = "R";
        else
            estatus = "D";

        sql = "update cotizacion_detalle set costo_unitario=" + refacc._costo.ToString() + ",porc_desc=" + refacc._porcentajeDescuento.ToString() + ",importe_desc=" + refacc._importeDescuento.ToString() + ",importe=" + refacc._importe.ToString() + ",estatus='COT',fecha_captura='" + fechaRetorno + "', existencia=" + val + ", dias_entrega=" + refacc._dias + ", estatus_proveedor='" + estatus + "' where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refacc._refaccion.ToString() + " and id_cliprov=" + refacc._proveedor.ToString();
        return ejecuta.insertUpdateDelete(sql);
    }



    public object[] verificaCotizacionCotiza(int[] sesiones, int horasTaller)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 13;
        string fechaRetorno = fechas.obtieneFechaConFormato();

        sql = "select (select case (select case when datediff(HH,CONVERT(DATETIME,convert(char(10),fecha,126)+' '+convert(char(8),hora,108),120),'" + fechaRetorno + "')<" + horasTaller.ToString() + " then 1 else 0 end as valido from Cotizacion_Encabezado where no_orden=" + sesiones[0].ToString() + " and id_empresa=" + sesiones[1].ToString() + " and id_taller=" + sesiones[2].ToString() + " and id_cotizacion=" + sesiones[3].ToString() + ") when 0 then 0 else (SELECT CASE when (select count(*) from Cotizacion_Detalle where no_orden=" + sesiones[0].ToString() + " and id_empresa=" + sesiones[1].ToString() + " and id_taller=" + sesiones[2].ToString() + " and id_cotizacion=" + sesiones[3].ToString() + " and id_cliprov="+sesiones[4].ToString()+" and estatus='COT')>0 THEN 0 ELSE 1 END ) end as valido)as valido";
        return ejecuta.scalarToBool(sql);
    }

    public object[] verificaCotizacion(int[] sesiones, int horasTaller)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 13;
        string fechaRetorno = fechas.obtieneFechaConFormato();

        sql = "select (select case when datediff(HH,CONVERT(DATETIME,convert(char(10),fecha,126)+' '+convert(char(8),hora,108),120),'" + fechaRetorno + "')<" + horasTaller.ToString() + " then 1 else 0 end as valido from Cotizacion_Encabezado where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + ") as valido";
        return ejecuta.scalarToBool(sql);
    }

    public object[] obtieneRefacciones(int[] sesiones)
    {
        sql = "select r.refOrd_Id,r.refCantidad,r.refDescripcion,r.refPrecioVenta,refProveedor,c.razon_social,r.refCosto,(SELECT porc_desc from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=ref_id_taller and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=r.refOrd_id and id_cliprov=r.refProveedor ) as porc_desc from Refacciones_Orden r left join Cliprov c on c.id_cliprov=r.refProveedor where r.ref_no_orden=" + sesiones[4].ToString() + " and r.ref_id_empresa=" + sesiones[2].ToString() + " and r.ref_id_taller=" + sesiones[3].ToString() + " and r.refCosto=0  and r.refOrd_Id in (select distinct id_cotizacion_detalle from Cotizacion_Detalle where no_orden=r.ref_no_orden and id_empresa=r.ref_id_empresa and id_taller=r.ref_id_taller and id_cotizacion=" + sesiones[6].ToString() + ")";
        return ejecuta.dataSet(sql);
    }

    public object[] actualizaRefacciones(int refaccion, int[] sesiones)
    {
        object[] actualizado = new object[2] { false, "" };
        //sql = "select (select min(costo_unitario) from Cotizacion_Detalle where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + " and (costo_unitario<>0 and not costo_unitario is null) and estatus='COT') as costo,(select top 1 id_cliprov from Cotizacion_Detalle where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + " and costo_unitario in(select min(costo_unitario) from Cotizacion_Detalle where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + " and (costo_unitario<>0 and not costo_unitario is null) and estatus='COT') order by id_cliprov) as proveedor";
        //sql = "declare @costo decimal(15,2)"
        //+ " declare @minDia int"
        //+ " declare @precio decimal(15,2)"
        //+ " set @costo = (select min(costo_unitario) from Cotizacion_Detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + " and costo_unitario>0)"
        //+ " set @minDia = (select min(isnull(dias_entrega,0)) from Cotizacion_Detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + ")"
        //+ " set @precio = (select refPrecioVenta from Refacciones_Orden where ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " and ref_no_orden=" + sesiones[4].ToString() + " and refOrd_Id=" + refaccion.ToString() + ")"
        //+ " select top 1 tabla.id_cotizacion_detalle,tabla.id_cliprov, tabla.costo_unitario,tabla.porc_desc,tabla.importe_desc,tabla.importe,tabla.existencia ,tabla.dias,tabla.precio,"
        //+ "tabla.calif_costo,tabla.calif_existe,tabla.calif_dias,tabla.calif_precio,tabla.total from("
        //+ " select id_cotizacion_detalle,id_cliprov, costo_unitario,porc_desc,importe_desc,importe,isnull(existencia,0) as existencia ,isnull(dias_entrega ,0) as dias,@precio as precio,"
        //+ "case when costo_unitario=@costo then 3 else 0 end as calif_costo,"
        //+ "case when existencia=1 then 1 else 0 end as calif_existe,"
        //+ "case when dias_entrega=@minDia then 2 else 0 end as calif_dias,"
        //+ "case when @precio>0 then (case when costo_unitario<=@precio then 1 else 0 end) else 0 end as calif_precio,"
        //+ "(case when costo_unitario=@costo then 3 else 0 end + case when existencia=1 then 1 else 0 end + case when dias_entrega=@minDia then 2 else 0 end + case when @precio>0 then (case when costo_unitario<=@precio then 1 else 0 end) else 0 end) as total"
        //+ " from Cotizacion_Detalle "
        //+ " where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + " and estatus='COT' and estatus_proveedor='R') as tabla"
        //+ " order by tabla.total desc";

        sql = "declare @costo decimal(15,2)"
        + " declare @minDia int"
        + " declare @precio decimal(15,2)"
        + " set @costo = (select min(costo_unitario) from Cotizacion_Detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + " and costo_unitario>0)"
        + " set @minDia = (select min(isnull(dias_entrega,0)) from Cotizacion_Detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + ")"
        + " set @precio = (select refPrecioVenta from Refacciones_Orden where ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " and ref_no_orden=" + sesiones[4].ToString() + " and refOrd_Id=" + refaccion.ToString() + ")"
        + " select top 1 tabla.id_cotizacion_detalle,tabla.id_cliprov, tabla.costo_unitario,tabla.porc_desc,tabla.importe_desc,tabla.importe,tabla.existencia ,tabla.dias,tabla.precio,"
        + "tabla.calif_costo,tabla.calif_existe,tabla.calif_dias,tabla.calif_precio,tabla.total from("
        + " select id_cotizacion_detalle,id_cliprov, costo_unitario,porc_desc,importe_desc,importe,isnull(existencia,0) as existencia ,isnull(dias_entrega ,0) as dias,@precio as precio,"
        + "case when costo_unitario=@costo then 4 else 0 end as calif_costo,"
        + "case when existencia=1 then 0 else 0 end as calif_existe,"
        + "case when dias_entrega=@minDia then 0 else 0 end as calif_dias,"
        + "case when @precio>0 then (case when costo_unitario<=@precio then 1 else 0 end) else 0 end as calif_precio,"
        + "(case when costo_unitario=@costo then 3 else 0 end + case when existencia=1 then 1 else 0 end + case when dias_entrega=@minDia then 2 else 0 end + case when @precio>0 then (case when costo_unitario<=@precio then 1 else 0 end) else 0 end) as total"
        + " from Cotizacion_Detalle "
        + " where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + refaccion.ToString() + " and estatus='COT' and estatus_proveedor='R') as tabla"
        + " order by tabla.total desc";
        object[] datos = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            DataSet info = (DataSet)datos[1];
            foreach (DataRow fila in info.Tables[0].Rows)
            {
                sql = "update refacciones_orden set refCosto=" + fila[2].ToString() + ", refProveedor=" + fila[1].ToString() + ", id_cotizacion=" + sesiones[6].ToString() + ", id_cotizacion_detalle=" + fila[0].ToString() + ", id_cliprov_cotizacion=" + fila[1].ToString() + ", id_cotizacion_autorizada=" + sesiones[6].ToString() + " where ref_no_orden=" + sesiones[4].ToString() + " and ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " and refOrd_id=" + refaccion.ToString();
                actualizado = ejecuta.insertUpdateDelete(sql);
            }
        }
        return actualizado;
    }

    public string ultimaEntregaActualizada(string noOrden, string idEmpresa, string idTaller)
    {
        sql = "select top 1 reffechentregareal from refacciones_orden where ref_no_orden=" + noOrden + " and ref_id_empresa=" + idEmpresa + " and ref_id_taller=" + idTaller + " order by reffechentregareal desc";
        string ejecutado = ejecuta.scalarToStringSimple(sql);
        return ejecutado;
    }

    public bool actualizaCronosEntregas(string noOrden, string idEmpresa, string idTaller, string ultimaFecha)
    {
        sql = "update seguimiento_orden set f_ult_entrega_ref='" + ultimaFecha + "' where no_orden=" + noOrden + " and id_empresa=" + idEmpresa + " and id_taller=" + idTaller;
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return Convert.ToBoolean(ejecutado[0]);
        else
            return false;
    }

    public int obtieneRefaccionesPendientes(string noOrden, string idEmpresa, string idTaller)
    {
        sql = "select (select count(*) from Refacciones_Orden " +
            "where ref_no_orden = " + noOrden + " and ref_id_taller = " + idTaller + " and ref_id_empresa = " + idEmpresa + " and refEstatusSolicitud = 2 )- " +
            "(select count(*) from Refacciones_Orden " +
            "where ref_no_orden = " + noOrden + " and ref_id_taller = " + idTaller + " and ref_id_empresa = " + idEmpresa + " and refEstatusSolicitud = 2 and reffechentregareal != null or " +
            "ref_no_orden = " + noOrden + " and ref_id_taller = " + idTaller + " and ref_id_empresa = " + idEmpresa + " and refEstatusSolicitud = 2 and reffechentregareal != '1900-01-01')";
        object[] retorno = ejecuta.scalarToInt(sql);
        if ((bool)retorno[0])
            return Convert.ToInt32(retorno[1]);
        else
            return 1;
    }

    public object[] obtieneCotizacionesProveedores(int[] sesiones)
    {
        sql = "select c.id_cotizacion_detalle,c.descripcion, c.id_cliprov,P.razon_social,c.costo_unitario,c.porc_desc,c.importe_desc,c.importe,c.existencia,c.dias_entrega from Cotizacion_Detalle c left join Cliprov p on p.id_cliprov=c.id_cliprov and p.tipo='P' where c.id_empresa=" + sesiones[2].ToString() + " and c.id_taller=" + sesiones[3].ToString() + " and c.no_orden=" + sesiones[4].ToString() + " and C.estatus='COT'";
        return ejecuta.dataSet(sql);
    }

    public object[] actualizaRefaccionCotizada(int[] sesiones, string[] argumentos)
    {
        sql = "DECLARE @precio decimal(15,2) declare @porcentaje decimal(5,2) " +
              "set @porcentaje = (select isnull(refPorcentSobreCosto,0) from refacciones_orden where ref_no_orden=" + sesiones[4].ToString() + " and ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " and refOrd_id=" + argumentos[1].ToString() + ") " +
              "set @precio = " + argumentos[2].ToString() + " + (" + argumentos[2].ToString() + " * (@porcentaje/100)) " +
              "update refacciones_orden set refCosto=" + argumentos[2].ToString() + ", refProveedor=" + argumentos[0].ToString() + ", id_cotizacion_autorizada=" + sesiones[6].ToString() + ", refPrecioVenta=@precio where ref_no_orden=" + sesiones[4].ToString() + " and ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " and refOrd_id=" + argumentos[1].ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaCancelacion(string argumentos, int[] sesiones)
    {
        sql = "update cotizacion_encabezado set estatus='CA' where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + argumentos + " " +
            "UPDATE cotizacion_detalle set estatus='CAN' where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + argumentos;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneIdProveedor(string proveedor)
    {
        sql = "select top 1 id_cliprov from cliprov where tipo='P' and ltrim(rtrim(upper(razon_social)))='" + proveedor + "'";
        return ejecuta.scalarToInt(sql);
    }

    public object[] agregaProveedorCotizacion(int[] sesiones, int id, string refaccion, int cantidad, int idCliprov, decimal costo, decimal descuento, decimal importeDescuento, decimal importe, int existe, int dias)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();

        sql = "insert into cotizacion_detalle values(" + sesiones[4].ToString() + "," + sesiones[3].ToString() + "," + sesiones[2].ToString() + "," + sesiones[6].ToString() + "," + id.ToString() + "," + idCliprov.ToString() + "," + cantidad.ToString() + ",'" + refaccion + "'," + costo.ToString() + "," + descuento.ToString() + "," + importeDescuento.ToString() + "," + importe.ToString() + ",'COT','" + fechaRetorno + "'," + id.ToString() + "," + existe.ToString() + "," + dias.ToString() + ",'M','R')";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaProveedorCotizacion(int[] sesiones, int id, string refaccion, int cantidad, int idCliprov, decimal costo, decimal descuento, decimal importeDescuento, decimal importe, int existe, int dias)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        sql = "update cotizacion_detalle set costo_unitario=" + costo.ToString() + ",porc_desc=" + descuento.ToString() + ", importe_desc=" + importeDescuento.ToString() + ",importe=" + importe.ToString() + ",estatus='COT',fecha_captura='" + fechaRetorno + "',id_refaccion=" + id.ToString() + ",existencia=" + existe.ToString() + ",dias_entrega=" + dias.ToString() + ",accion='M',estatus_proveedor='R'  where no_orden=" + sesiones[4].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + id.ToString() + " and id_cliprov=" + idCliprov.ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] existeProveedorCotizacion(int[] sesiones, int idCliprov, int id)
    {
        sql = "select count(*) from cotizacion_detalle where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and id_cotizacion_detalle=" + id.ToString() + " and id_cliprov=" + idCliprov.ToString();
        return ejecuta.scalarToBool(sql);
    }

    public object[] existeProveedorCotizacionGral(int[] sesiones, int idCliprov, int id)
    {
        sql = "select count(*) from cotizacion_detalle where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion_detalle=" + id.ToString() + " and id_cliprov=" + idCliprov.ToString();
        return ejecuta.scalarToBool(sql);
    }

    public object[] obtieneProveedoresPendientes(int[] sesiones)
    {
        object[] retorno = new object[2] { false, "" };
        string sql = "select id_cliprov,count(*) from cotizacion_detalle where NO_ORDEN=" + sesiones[4].ToString() + " AND ID_EMPRESA=" + sesiones[2].ToString() + " AND ID_TALLER=" + sesiones[3].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " and estatus_proveedor='P' group by id_cliprov";
        object[] info = ejecuta.dataSet(sql);
        try
        {
            if (Convert.ToBoolean(info[0]))
            {
                int pendientes = 0;
                DataSet datos = (DataSet)info[1];
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    if (Convert.ToInt32(fila[1].ToString()) > 0)
                        pendientes++;
                }
                retorno[0] = true;
                retorno[1] = pendientes;
            }
        }
        catch (Exception ex)
        {
            retorno[0] = false;
            retorno[1] = ex.Message;
        }
        return retorno;
    }

    public object[] existeAutorizada(int idCotizacion, int[] sesiones)
    {
        string sql = "SELECT COUNT(*) FROM REFACCIONES_ORDEN WHERE REF_NO_ORDEN=" + sesiones[4].ToString() + " AND REF_ID_EMPRESA=" + sesiones[2].ToString() + " AND REF_ID_TALLER=" + sesiones[3].ToString() + " and id_cotizacion_autorizada=" + idCotizacion + " and refEstatus='AU'";
        return ejecuta.scalarToBool(sql);
    }

    public string obtieneFechaCotEnc(int noOrden, int idEmpresa, int idTaller, int idCotizacion)
    {
        string sql = "select convert(char(10),fecha,126)+' '+convert(char(8),hora,108) from Cotizacion_Encabezado where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and id_cotizacion=" + idCotizacion.ToString();
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] obtieneHrsMaxTaller(int idEmpresa, int idTaller)
    {
        string sql = "select tiempo_max_cot from Empresas_Taller where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString();
        return ejecuta.scalarToInt(sql);
    }

    public object[] generaComparativo(int[] sesiones)
    {
        #region tmp
        /*string sql = "delete from tmp_comparativo where id_usuario=" + sesiones[0].ToString() + "  insert into tmp_comparativo(id_usuario,id_cotizacion_detalle,cantidad,descripcion) select " + sesiones[0].ToString() + ",refOrd_Id,refCantidad,refDescripcion from Refacciones_Orden where ref_no_orden=" + sesiones[4].ToString() + " and ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " AND RefEstatus<>'CA'";
        object[] agregados = ejecuta.insertUpdateDelete(sql);
        int proveedoreRegistrados = 0;
        if (Convert.ToBoolean(agregados[0]))
        {
            sql = "select id_cotizacion_detalle from tmp_comparativo where id_usuario=" + sesiones[0].ToString();
            object[] refacc = ejecuta.dataSet(sql);
            if (Convert.ToBoolean(refacc[0]))
            {
                DataSet refacciones = (DataSet)refacc[1];
                int contProvee = 1;
                sql = "select rank() over(order by id_cliprov),id_cliprov from Cotizacion_Detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " group by id_cliprov";
                DataSet proveedoresCotizacion = new DataSet();
                object[] proveedoresCotizantes = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(proveedoresCotizantes[0]))
                {
                    proveedoresCotizacion = (DataSet)proveedoresCotizantes[1];
                }

                proveedoreRegistrados = proveedoresCotizacion.Tables[0].Rows.Count;

                foreach (DataRow refaccion in refacciones.Tables[0].Rows)
                {
                    sql = "select id_cliprov from Cotizacion_Detalle where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion_detalle=" + refaccion[0].ToString() + " and id_cotizacion=" + sesiones[6].ToString();
                    object[] proveedoresRef = ejecuta.dataSet(sql);
                    if (Convert.ToBoolean(proveedoresRef[0]))
                    {
                        DataSet proveedores = (DataSet)proveedoresRef[1];
                        foreach (DataRow proveedor in proveedores.Tables[0].Rows)
                        {

                            foreach (DataRow id in proveedoresCotizacion.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(id[1].ToString()) == Convert.ToInt32(proveedor[0].ToString()))
                                    contProvee = Convert.ToInt32(id[0].ToString());
                            }
                            sql = "select c.id_cliprov,(select RAZON_SOCIAL from cliprov where id_cliprOv=c.id_cliprov and tipo='P'), c.costo_unitario,c.porc_desc,c.importe_desc,c.importe,case c.existencia when 1 then 1 else 0 end,c.dias_entrega,c.estatus_proveedor from Cotizacion_Detalle c where c.id_empresa=" + sesiones[2].ToString() + " and c.id_taller=" + sesiones[3].ToString() + " and c.no_orden=" + sesiones[4].ToString() + " and c.id_cotizacion=" + sesiones[6].ToString() + " and c.id_cotizacion_detalle=" + refaccion[0].ToString() + " and c.id_cliprov=" + proveedor[0].ToString();
                            object[] datos = ejecuta.dataSet(sql);
                            if (Convert.ToBoolean(datos[0]))
                            {
                                DataSet info = (DataSet)datos[1];
                                foreach (DataRow fila in info.Tables[0].Rows)
                                {
                                    sql = "update tmp_comparativo set id_prov" + contProvee.ToString() + "=" + fila[0].ToString() + ", razon_social" + contProvee.ToString() + "='" + fila[1].ToString() + "', costo_unitario" + contProvee.ToString() + "=" + Convert.ToDecimal(fila[2].ToString()).ToString() + ", porc_desc" + contProvee.ToString() + "=" + Convert.ToDecimal(fila[3].ToString()).ToString() + ",importe_desc" + contProvee.ToString() + "=" + Convert.ToDecimal(fila[4].ToString()).ToString() + ",importe" + contProvee.ToString() + "=" + Convert.ToDecimal(fila[5].ToString()).ToString() + ",existencia" + contProvee.ToString() + "=" + Convert.ToInt32(fila[6].ToString()).ToString() + ",dias" + contProvee.ToString() + "=" + Convert.ToInt32(fila[7].ToString()).ToString() + ",estatus" + contProvee.ToString() + "='" + fila[8].ToString() + "' where id_usuario=" + sesiones[0].ToString() + " and id_cotizacion_detalle=" + refaccion[0].ToString();
                                    object[] actualizado = ejecuta.insertUpdateDelete(sql);
                                    if (Convert.ToBoolean(actualizado[0])) { }

                                }
                            }
                        }
                    }
                }
            }
        }

        sql = "select t.id_usuario,t.id_cotizacion_detalle,t.cantidad,t.descripcion," +
"t.id_prov1,t.razon_social1,t.costo_unitario1,t.porc_desc1,t.importe_desc1,t.importe1,t.existencia1,t.dias1,t.estatus1," +
"t.id_prov2,t.razon_social2,t.costo_unitario2,t.porc_desc2,t.importe_desc2,t.importe2,t.existencia2,t.dias2,t.estatus2," +
"t.id_prov3,t.razon_social3,t.costo_unitario3,t.porc_desc3,t.importe_desc3,t.importe3,t.existencia3,t.dias3,t.estatus3," +
"t.id_prov4,t.razon_social4,t.costo_unitario4,t.porc_desc4,t.importe_desc4,t.importe4,t.existencia4,t.dias4,t.estatus4," +
"t.id_prov5,t.razon_social5,t.costo_unitario5,t.porc_desc5,t.importe_desc5,t.importe5,t.existencia5,t.dias5,t.estatus5," +
"t.id_prov6,t.razon_social6,t.costo_unitario6,t.porc_desc6,t.importe_desc6,t.importe6,t.existencia6,t.dias6,t.estatus6," +
"t.id_prov7,t.razon_social7,t.costo_unitario7,t.porc_desc7,t.importe_desc7,t.importe7,t.existencia7,t.dias7,t.estatus7," +
"t.id_prov8,t.razon_social8,t.costo_unitario8,t.porc_desc8,t.importe_desc8,t.importe8,t.existencia8,t.dias8,t.estatus8," +
"t.id_prov9,t.razon_social9,t.costo_unitario9,t.porc_desc9,t.importe_desc9,t.importe9,t.existencia9,t.dias9,t.estatus9," +
"t.id_prov10,t.razon_social10,t.costo_unitario10,t.porc_desc10,t.importe_desc10,t.importe10,t.existencia10,t.dias10,t.estatus10," +
"t.id_prov11,t.razon_social11,t.costo_unitario11,t.porc_desc11,t.importe_desc11,t.importe11,t.existencia11,t.dias11,t.estatus11," +
"t.id_prov12,t.razon_social12,t.costo_unitario12,t.porc_desc12,t.importe_desc12,t.importe12,t.existencia12,t.dias12,t.estatus12," +
"t.id_prov13,t.razon_social13,t.costo_unitario13,t.porc_desc13,t.importe_desc13,t.importe13,t.existencia13,t.dias13,t.estatus13," +
"t.id_prov14,t.razon_social14,t.costo_unitario14,t.porc_desc14,t.importe_desc14,t.importe14,t.existencia14,t.dias14,t.estatus14," +
"t.id_prov15,t.razon_social15,t.costo_unitario15,t.porc_desc15,t.importe_desc15,t.importe15,t.existencia15,t.dias15,t.estatus15," +
"t.id_prov16,t.razon_social16,t.costo_unitario16,t.porc_desc16,t.importe_desc16,t.importe16,t.existencia16,t.dias16,t.estatus16," +
"t.id_prov17,t.razon_social17,t.costo_unitario17,t.porc_desc17,t.importe_desc17,t.importe17,t.existencia17,t.dias17,t.estatus17," +
"t.id_prov18,t.razon_social18,t.costo_unitario18,t.porc_desc18,t.importe_desc18,t.importe18,t.existencia18,t.dias18,t.estatus18," +
"t.id_prov19,t.razon_social19,t.costo_unitario19,t.porc_desc19,t.importe_desc19,t.importe19,t.existencia19,t.dias19,t.estatus19," +
"t.id_prov20,t.razon_social20,t.costo_unitario20,t.porc_desc20,t.importe_desc20,t.importe20,t.existencia20,t.dias20,t.estatus20," +
"r.refEstatus from tmp_comparativo t inner join Refacciones_Orden r on r.ref_id_taller=" + sesiones[3].ToString() + " and r.ref_id_empresa=" + sesiones[2].ToString() + " and r.ref_no_orden=" + sesiones[4].ToString() +
" and r.refOrd_Id=t.id_cotizacion_detalle where t.id_usuario=" + sesiones[0].ToString() + " and t.id_cotizacion_detalle in (select distinct cd.id_cotizacion_detalle from Cotizacion_Detalle cd where cd.id_empresa=r.ref_id_empresa and cd.id_taller=r.ref_id_taller and cd.no_orden=r.ref_no_orden and id_cotizacion="+sesiones[6].ToString()+")";*/
        #endregion

        int proveedoreRegistrados = 0;
        string sql = "select rank() over(order by id_cliprov),id_cliprov from Cotizacion_Detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_cotizacion=" + sesiones[6].ToString() + " group by id_cliprov";
        DataSet proveedoresCotizacion = new DataSet();
        object[] proveedoresCotizantes = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(proveedoresCotizantes[0]))
            proveedoresCotizacion = (DataSet)proveedoresCotizantes[1];
        proveedoreRegistrados = proveedoresCotizacion.Tables[0].Rows.Count;
        sql = "declare @orden int declare @empresa int declare @taller int declare @usuario int declare @cotizacion int " +
"set @orden = " + sesiones[4] + " set @empresa = " + sesiones[2] + " set @taller = " + sesiones[3] + " set @usuario = " + sesiones[0] + " set @cotizacion = " + sesiones[6] + " " +
"delete from tmp_comparativo where id_usuario = @usuario  insert into tmp_comparativo(id_usuario, id_cotizacion_detalle, cantidad, descripcion) select @usuario, refOrd_Id, refCantidad, refDescripcion from Refacciones_Orden where ref_no_orden = @orden and ref_id_empresa = @empresa and ref_id_taller = @taller AND RefEstatus<> 'CA' and(proceso IS NULL OR PROCESO != 'C') " +
"declare @idCotizacionDetalle int declare @proveedoresCotizantes int declare @proveedor int declare @pConsecutivo int declare @id int declare @razon varchar(200) declare @costo decimal(15, 2) declare @porcdesc decimal(5, 2) declare @importedesc decimal(15, 2) declare @importe decimal(15, 2)  declare @existencia bit declare @dias int declare @estatus char(1) declare @fecha_limite date " +
"set @proveedoresCotizantes = 0 " +
    "declare refacciones cursor for " +
    "select id_cotizacion_detalle from tmp_comparativo where id_usuario = @usuario " +
    "open refacciones " +
    "fetch next from refacciones " +
    "into @idCotizacionDetalle " +
    "while @@fetch_status = 0 " +
    "begin " +
        "set @proveedoresCotizantes = (select count(*) from(select rank() over(order by id_cliprov) as consecutivo, id_cliprov from Cotizacion_Detalle where id_empresa = @empresa and id_taller = @taller and no_orden = @orden and id_cotizacion = @cotizacion group by id_cliprov) as t) " +
            "declare proveedores cursor for " +
            "select rank() over(order by id_cliprov) as consecutivo, id_cliprov from Cotizacion_Detalle where no_orden = @orden and id_empresa = @empresa and id_taller = @taller and id_cotizacion_detalle = @idCotizacionDetalle and id_cotizacion = @cotizacion " +
            "open proveedores " +
            "fetch next from proveedores " +
            "into @pConsecutivo, @proveedor " +
            "while @@fetch_status = 0 " +
            "begin " +
                "declare cotiza cursor for " +
                "select c.id_cliprov, (select RAZON_SOCIAL from cliprov where id_cliprOv = c.id_cliprov and tipo = 'P') as razon, c.costo_unitario,c.porc_desc,c.importe_desc,c.importe,case c.existencia when 1 then 1 else 0 end as existencia,c.dias_entrega,c.estatus_proveedor,c.fecha_limite from Cotizacion_Detalle c where c.id_empresa = @empresa and c.id_taller = @taller and c.no_orden = @orden and c.id_cotizacion = @cotizacion and c.id_cotizacion_detalle = @idCotizacionDetalle and c.id_cliprov = @proveedor " +
                "open cotiza " +
                "fetch next from cotiza " +
                "into @id,@razon,@costo,@porcdesc,@importedesc,@importe,@existencia,@dias,@estatus,@fecha_limite " +
                "while @@fetch_status = 0 " +
                "begin " +
                    "if (@pConsecutivo = 1) begin update tmp_comparativo set id_prov1 = @id, razon_social1 = @razon, costo_unitario1 = @costo, porc_desc1 = @porcdesc,importe_desc1 = @importedesc,importe1 = @importe,existencia1 = @existencia,dias1 = @dias,estatus1 = @estatus,fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 2) begin update tmp_comparativo set id_prov2 = @id, razon_social2 = @razon, costo_unitario2 = @costo, porc_desc2 = @porcdesc,importe_desc2 = @importedesc,importe2 = @importe,existencia2 = @existencia,dias2 = @dias,estatus2 = @estatus,fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 3) begin update tmp_comparativo set id_prov3 = @id, razon_social3 = @razon, costo_unitario3 = @costo, porc_desc3 = @porcdesc,importe_desc3 = @importedesc,importe3 = @importe,existencia3 = @existencia,dias3 = @dias,estatus3 = @estatus,fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 4) begin update tmp_comparativo set id_prov4 = @id, razon_social4 = @razon, costo_unitario4 = @costo, porc_desc4 = @porcdesc,importe_desc4 = @importedesc,importe4 = @importe,existencia4 = @existencia,dias4 = @dias,estatus4 = @estatus,fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 5) begin update tmp_comparativo set id_prov5 = @id, razon_social5 = @razon, costo_unitario5 = @costo, porc_desc5 = @porcdesc,importe_desc5 = @importedesc,importe5 = @importe,existencia5 = @existencia,dias5 = @dias,estatus5 = @estatus,fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 6) begin update tmp_comparativo set id_prov6 = @id, razon_social6 = @razon, costo_unitario6 = @costo, porc_desc6 = @porcdesc,importe_desc6 = @importedesc,importe6 = @importe,existencia6 = @existencia,dias6 = @dias,estatus6 = @estatus,fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 7) begin update tmp_comparativo set id_prov7 = @id, razon_social7 = @razon, costo_unitario7 = @costo, porc_desc7 = @porcdesc,importe_desc7 = @importedesc,importe7 = @importe,existencia7 = @existencia,dias7 = @dias,estatus7 = @estatus ,fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 8) begin update tmp_comparativo set id_prov8 = @id, razon_social8 = @razon, costo_unitario8 = @costo, porc_desc8 = @porcdesc,importe_desc8 = @importedesc,importe8 = @importe,existencia8 = @existencia,dias8 = @dias,estatus8 = @estatus ,fecha_limite1= @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 9) begin update tmp_comparativo set id_prov9 = @id, razon_social9 = @razon, costo_unitario9 = @costo, porc_desc9 = @porcdesc,importe_desc9 = @importedesc,importe9 = @importe,existencia9 = @existencia,dias9 = @dias,estatus9 = @estatus ,fecha_limite1= @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle end " +
                    "if (@pConsecutivo = 10) begin update tmp_comparativo set id_prov10 = @id, razon_social10 = @razon, costo_unitario10 = @costo, porc_desc10 = @porcdesc,importe_desc10 = @importedesc,importe10 = @importe,existencia10 = @existencia,dias10 = @dias,estatus10 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 11) begin update tmp_comparativo set id_prov11 = @id, razon_social11 = @razon, costo_unitario11 = @costo, porc_desc11 = @porcdesc,importe_desc11 = @importedesc,importe11 = @importe,existencia11 = @existencia,dias11 = @dias,estatus11 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 12) begin update tmp_comparativo set id_prov12 = @id, razon_social12 = @razon, costo_unitario12 = @costo, porc_desc12 = @porcdesc,importe_desc12 = @importedesc,importe12 = @importe,existencia12 = @existencia,dias12 = @dias,estatus12 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 13) begin update tmp_comparativo set id_prov13 = @id, razon_social13 = @razon, costo_unitario13 = @costo, porc_desc13 = @porcdesc,importe_desc13 = @importedesc,importe13 = @importe,existencia13 = @existencia,dias13 = @dias,estatus13 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 14) begin update tmp_comparativo set id_prov14 = @id, razon_social14 = @razon, costo_unitario14 = @costo, porc_desc14 = @porcdesc,importe_desc14 = @importedesc,importe14 = @importe,existencia14 = @existencia,dias14 = @dias,estatus14 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 15) begin update tmp_comparativo set id_prov15 = @id, razon_social15 = @razon, costo_unitario15 = @costo, porc_desc15 = @porcdesc,importe_desc15 = @importedesc,importe15 = @importe,existencia15 = @existencia,dias15 = @dias,estatus15 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 16) begin update tmp_comparativo set id_prov16 = @id, razon_social16 = @razon, costo_unitario16 = @costo, porc_desc16 = @porcdesc,importe_desc16 = @importedesc,importe16 = @importe,existencia16 = @existencia,dias16 = @dias,estatus16 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 17) begin update tmp_comparativo set id_prov17 = @id, razon_social17 = @razon, costo_unitario17 = @costo, porc_desc17 = @porcdesc,importe_desc17 = @importedesc,importe17 = @importe,existencia17 = @existencia,dias17 = @dias,estatus17 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 18) begin update tmp_comparativo set id_prov18 = @id, razon_social18 = @razon, costo_unitario18 = @costo, porc_desc18 = @porcdesc,importe_desc18 = @importedesc,importe18 = @importe,existencia18 = @existencia,dias18 = @dias,estatus18 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 19) begin update tmp_comparativo set id_prov19 = @id, razon_social19 = @razon, costo_unitario19 = @costo, porc_desc19 = @porcdesc,importe_desc19 = @importedesc,importe19 = @importe,existencia19 = @existencia,dias19 = @dias,estatus19 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "if (@pConsecutivo = 20) begin update tmp_comparativo set id_prov20 = @id, razon_social20 = @razon, costo_unitario20 = @costo, porc_desc20 = @porcdesc,importe_desc20 = @importedesc,importe20 = @importe,existencia20 = @existencia,dias20 = @dias,estatus20 = @estatus, fecha_limite1 = @fecha_limite where id_usuario = @usuario and id_cotizacion_detalle = @idCotizacionDetalle    end " +
                    "fetch next from cotiza " +
                    "into @id,@razon,@costo,@porcdesc,@importedesc,@importe,@existencia,@dias,@estatus,@fecha_limite " +
                "end " +
                "close cotiza  " +
                "deallocate cotiza  " +
                "fetch next from proveedores " +
                "into @pConsecutivo,@proveedor " +
            "end " +
            "close proveedores  " +
            "deallocate proveedores " +
        "fetch next from refacciones " +
        "into @idCotizacionDetalle " +
    "end " +
    "close refacciones " +
    "deallocate refacciones " +
    "select t.id_usuario,t.id_cotizacion_detalle,t.cantidad,t.descripcion, " +
"t.id_prov1,t.razon_social1,t.costo_unitario1,t.porc_desc1,t.importe_desc1,t.importe1,t.existencia1,t.dias1,t.estatus1,case t.fecha_limite1 when '1900-01-01' then '' else convert(char(10),t.fecha_limite1,120) end as fecha_limite1, " +
"t.id_prov2,t.razon_social2,t.costo_unitario2,t.porc_desc2,t.importe_desc2,t.importe2,t.existencia2,t.dias2,t.estatus2,case t.fecha_limite2 when '1900-01-01' then '' else convert(char(10),t.fecha_limite2,120) end as fecha_limite2, " +
"t.id_prov3,t.razon_social3,t.costo_unitario3,t.porc_desc3,t.importe_desc3,t.importe3,t.existencia3,t.dias3,t.estatus3,case t.fecha_limite3 when '1900-01-01' then '' else convert(char(10),t.fecha_limite3,120) end as fecha_limite3, " +
"t.id_prov4,t.razon_social4,t.costo_unitario4,t.porc_desc4,t.importe_desc4,t.importe4,t.existencia4,t.dias4,t.estatus4,case t.fecha_limite4 when '1900-01-01' then '' else convert(char(10),t.fecha_limite4,120) end as fecha_limite4, " +
"t.id_prov5,t.razon_social5,t.costo_unitario5,t.porc_desc5,t.importe_desc5,t.importe5,t.existencia5,t.dias5,t.estatus5,case t.fecha_limite5 when '1900-01-01' then '' else convert(char(10),t.fecha_limite5,120) end as fecha_limite5, " +
"t.id_prov6,t.razon_social6,t.costo_unitario6,t.porc_desc6,t.importe_desc6,t.importe6,t.existencia6,t.dias6,t.estatus6,case t.fecha_limite6 when '1900-01-01' then '' else convert(char(10),t.fecha_limite6,120) end as fecha_limite6, " +
"t.id_prov7,t.razon_social7,t.costo_unitario7,t.porc_desc7,t.importe_desc7,t.importe7,t.existencia7,t.dias7,t.estatus7,case t.fecha_limite7 when '1900-01-01' then '' else convert(char(10),t.fecha_limite7,120) end as fecha_limite7, " +
"t.id_prov8,t.razon_social8,t.costo_unitario8,t.porc_desc8,t.importe_desc8,t.importe8,t.existencia8,t.dias8,t.estatus8,case t.fecha_limite8 when '1900-01-01' then '' else convert(char(10),t.fecha_limite8,120) end as fecha_limite8, " +
"t.id_prov9,t.razon_social9,t.costo_unitario9,t.porc_desc9,t.importe_desc9,t.importe9,t.existencia9,t.dias9,t.estatus9,case t.fecha_limite9 when '1900-01-01' then '' else convert(char(10),t.fecha_limite9,120) end as fecha_limite9, " +
"t.id_prov10,t.razon_social10,t.costo_unitario10,t.porc_desc10,t.importe_desc10,t.importe10,t.existencia10,t.dias10,t.estatus10,case t.fecha_limite10 when '1900-01-01' then '' else convert(char(10),t.fecha_limite10,120) end as fecha_limite10, " +
"t.id_prov11,t.razon_social11,t.costo_unitario11,t.porc_desc11,t.importe_desc11,t.importe11,t.existencia11,t.dias11,t.estatus11,case t.fecha_limite11 when '1900-01-01' then '' else convert(char(10),t.fecha_limite11,120) end as fecha_limite11, " +
"t.id_prov12,t.razon_social12,t.costo_unitario12,t.porc_desc12,t.importe_desc12,t.importe12,t.existencia12,t.dias12,t.estatus12,case t.fecha_limite12 when '1900-01-01' then '' else convert(char(10),t.fecha_limite12,120) end as fecha_limite12, " +
"t.id_prov13,t.razon_social13,t.costo_unitario13,t.porc_desc13,t.importe_desc13,t.importe13,t.existencia13,t.dias13,t.estatus13,case t.fecha_limite13 when '1900-01-01' then '' else convert(char(10),t.fecha_limite13,120) end as fecha_limite13, " +
"t.id_prov14,t.razon_social14,t.costo_unitario14,t.porc_desc14,t.importe_desc14,t.importe14,t.existencia14,t.dias14,t.estatus14,case t.fecha_limite14 when '1900-01-01' then '' else convert(char(10),t.fecha_limite14,120) end as fecha_limite14, " +
"t.id_prov15,t.razon_social15,t.costo_unitario15,t.porc_desc15,t.importe_desc15,t.importe15,t.existencia15,t.dias15,t.estatus15,case t.fecha_limite15 when '1900-01-01' then '' else convert(char(10),t.fecha_limite15,120) end as fecha_limite15, " +
"t.id_prov16,t.razon_social16,t.costo_unitario16,t.porc_desc16,t.importe_desc16,t.importe16,t.existencia16,t.dias16,t.estatus16,case t.fecha_limite16 when '1900-01-01' then '' else convert(char(10),t.fecha_limite16,120) end as fecha_limite16, " +
"t.id_prov17,t.razon_social17,t.costo_unitario17,t.porc_desc17,t.importe_desc17,t.importe17,t.existencia17,t.dias17,t.estatus17,case t.fecha_limite17 when '1900-01-01' then '' else convert(char(10),t.fecha_limite17,120) end as fecha_limite17, " +
"t.id_prov18,t.razon_social18,t.costo_unitario18,t.porc_desc18,t.importe_desc18,t.importe18,t.existencia18,t.dias18,t.estatus18,case t.fecha_limite18 when '1900-01-01' then '' else convert(char(10),t.fecha_limite18,120) end as fecha_limite18, " +
"t.id_prov19,t.razon_social19,t.costo_unitario19,t.porc_desc19,t.importe_desc19,t.importe19,t.existencia19,t.dias19,t.estatus19,case t.fecha_limite19 when '1900-01-01' then '' else convert(char(10),t.fecha_limite19,120) end as fecha_limite19, " +
"t.id_prov20,t.razon_social20,t.costo_unitario20,t.porc_desc20,t.importe_desc20,t.importe20,t.existencia20,t.dias20,t.estatus20,case t.fecha_limite20 when '1900-01-01' then '' else convert(char(10),t.fecha_limite20,120) end as fecha_limite20, " +
"r.refEstatus from tmp_comparativo t inner join Refacciones_Orden r on r.ref_id_taller = @taller and r.ref_id_empresa = @empresa and r.ref_no_orden = @orden " +
" and r.refOrd_Id = t.id_cotizacion_detalle where t.id_usuario = @usuario and t.id_cotizacion_detalle in (select distinct cd.id_cotizacion_detalle from Cotizacion_Detalle cd where cd.id_empresa = r.ref_id_empresa and cd.id_taller = r.ref_id_taller and cd.no_orden = r.ref_no_orden and id_cotizacion = @cotizacion) ";
        object[] valores = ejecuta.dataSet(sql);
        object[] retorno = new object[2] { false, "" };
        if (Convert.ToBoolean(valores[0]))
        {
            retorno[0] = proveedoreRegistrados;
            retorno[1] = valores[1];
        }
        else {
            retorno = valores;
        }
        return retorno;
    }

    public object[] autorizaRefaccion(string empresa, string taller, string orden, string cotizacion, string refaccion)
    {
        string sql = "UPDATE Refacciones_Orden SET refEstatus = 'AU' WHERE refOrd_Id = " + refaccion + " AND ref_no_orden = " + orden + " and ref_id_empresa=" + empresa + " and ref_id_taller=" + taller;
        return ejecuta.insertUpdateDelete(sql);
    }

    public string obtieneIdsCotGral(int[] sesiones, string refaccion)
    {
        sql = "declare @id_cotizacion varchar(200) " +
              "select @id_cotizacion = coalesce(cast(@id_cotizacion as varchar) + ',', '') + cast(id_cotizacion as varchar) from Cotizacion_Detalle " +
              "where no_orden = " + sesiones[4] + " and id_empresa =" + sesiones[2] + " and id_taller = " + sesiones[3] + " and descripcion = '" + refaccion + "' group by id_cotizacion " +
              "select @id_cotizacion";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneEstatusCotizacion(int[] sesiones)
    {
        sql = "select estatus from cotizacion_encabezado " +
            "where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion=" + sesiones[6].ToString();
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] existeMoncarCotizacion(int idEmpresa, int idTaller, int noOrden)
    {
        sql = "select count(*) from Proveedor_Cotizacion_Tmp where no_orden = " + noOrden.ToString() + " and id_empresa =" + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() + " and id_cliprov = 0";
        return ejecuta.scalarToBoolLog(sql);
    }

    public object[] insertaMoncarCotizacion(int idEmpresa, int idTaller, int noOrden)
    {
        sql = "insert into Proveedor_Cotizacion_Tmp values(" + noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + ",(select(select top 1 folio as ultimo from Proveedor_Cotizacion_Tmp " +
            "where no_orden = " + noOrden.ToString() + " and id_empresa =" + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() + ") + 1),0)";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] generaComparativoGral(int[] sesiones)
    {
        string sql = "delete from tmp_comparativo where id_usuario=" + sesiones[0].ToString() + "  insert into tmp_comparativo(id_usuario,id_cotizacion_detalle,cantidad,descripcion) select " + sesiones[0].ToString() + ",refOrd_Id,refCantidad,refDescripcion from Refacciones_Orden where ref_no_orden=" + sesiones[4].ToString() + " and ref_id_empresa=" + sesiones[2].ToString() + " and ref_id_taller=" + sesiones[3].ToString() + " AND RefEstatus<>'CA'";
        object[] agregados = ejecuta.insertUpdateDelete(sql);
        int proveedoreRegistrados = 0;
        if (Convert.ToBoolean(agregados[0]))
        {
            sql = "select id_cotizacion_detalle from tmp_comparativo where id_usuario=" + sesiones[0].ToString();
            object[] refacc = ejecuta.dataSet(sql);
            if (Convert.ToBoolean(refacc[0]))
            {
                DataSet refacciones = (DataSet)refacc[1];
                int contProvee = 1;
                sql = "select rank() over(order by id_cliprov),id_cliprov from Cotizacion_Detalle where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " group by id_cliprov";
                DataSet proveedoresCotizacion = new DataSet();
                object[] proveedoresCotizantes = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(proveedoresCotizantes[0]))
                {
                    proveedoresCotizacion = (DataSet)proveedoresCotizantes[1];
                }

                proveedoreRegistrados = proveedoresCotizacion.Tables[0].Rows.Count;

                foreach (DataRow refaccion in refacciones.Tables[0].Rows)
                {
                    sql = "select id_cliprov from Cotizacion_Detalle where no_orden=" + sesiones[4].ToString() + " and id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and id_cotizacion_detalle=" + refaccion[0].ToString();
                    object[] proveedoresRef = ejecuta.dataSet(sql);
                    if (Convert.ToBoolean(proveedoresRef[0]))
                    {
                        DataSet proveedores = (DataSet)proveedoresRef[1];
                        foreach (DataRow proveedor in proveedores.Tables[0].Rows)
                        {

                            foreach (DataRow id in proveedoresCotizacion.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(id[1].ToString()) == Convert.ToInt32(proveedor[0].ToString()))
                                    contProvee = Convert.ToInt32(id[0].ToString());
                            }
                            sql = "select c.id_cliprov,(select RAZON_SOCIAL from cliprov where id_cliprOv=c.id_cliprov and tipo='P'), c.costo_unitario,c.porc_desc,c.importe_desc,c.importe,case c.existencia when 1 then 1 else 0 end,c.dias_entrega,c.estatus_proveedor from Cotizacion_Detalle c where c.id_empresa=" + sesiones[2].ToString() + " and c.id_taller=" + sesiones[3].ToString() + " and c.no_orden=" + sesiones[4].ToString() + " and c.id_cotizacion_detalle=" + refaccion[0].ToString() + " and c.id_cliprov=" + proveedor[0].ToString();
                            object[] datos = ejecuta.dataSet(sql);
                            if (Convert.ToBoolean(datos[0]))
                            {
                                DataSet info = (DataSet)datos[1];
                                foreach (DataRow fila in info.Tables[0].Rows)
                                {
                                    sql = "update tmp_comparativo set id_prov" + contProvee.ToString() + "=" + fila[0].ToString() + ", razon_social" + contProvee.ToString() + "='" + fila[1].ToString() + "', costo_unitario" + contProvee.ToString() + "=" + Convert.ToDecimal(fila[2].ToString()).ToString() + ", porc_desc" + contProvee.ToString() + "=" + Convert.ToDecimal(fila[3].ToString()).ToString() + ",importe_desc" + contProvee.ToString() + "=" + Convert.ToDecimal(fila[4].ToString()).ToString() + ",importe" + contProvee.ToString() + "=" + Convert.ToDecimal(fila[5].ToString()).ToString() + ",existencia" + contProvee.ToString() + "=" + Convert.ToInt32(fila[6].ToString()).ToString() + ",dias" + contProvee.ToString() + "=" + Convert.ToInt32(fila[7].ToString()).ToString() + ",estatus" + contProvee.ToString() + "='" + fila[8].ToString() + "' where id_usuario=" + sesiones[0].ToString() + " and id_cotizacion_detalle=" + refaccion[0].ToString();
                                    object[] actualizado = ejecuta.insertUpdateDelete(sql);
                                    if (Convert.ToBoolean(actualizado[0])) { }

                                }
                            }
                        }
                    }
                }
            }
        }

        sql = "select t.id_usuario,t.id_cotizacion_detalle,t.cantidad,t.descripcion," +
"t.id_prov1,t.razon_social1,t.costo_unitario1,t.porc_desc1,t.importe_desc1,t.importe1,t.existencia1,t.dias1,t.estatus1," +
"t.id_prov2,t.razon_social2,t.costo_unitario2,t.porc_desc2,t.importe_desc2,t.importe2,t.existencia2,t.dias2,t.estatus2," +
"t.id_prov3,t.razon_social3,t.costo_unitario3,t.porc_desc3,t.importe_desc3,t.importe3,t.existencia3,t.dias3,t.estatus3," +
"t.id_prov4,t.razon_social4,t.costo_unitario4,t.porc_desc4,t.importe_desc4,t.importe4,t.existencia4,t.dias4,t.estatus4," +
"t.id_prov5,t.razon_social5,t.costo_unitario5,t.porc_desc5,t.importe_desc5,t.importe5,t.existencia5,t.dias5,t.estatus5," +
"t.id_prov6,t.razon_social6,t.costo_unitario6,t.porc_desc6,t.importe_desc6,t.importe6,t.existencia6,t.dias6,t.estatus6," +
"t.id_prov7,t.razon_social7,t.costo_unitario7,t.porc_desc7,t.importe_desc7,t.importe7,t.existencia7,t.dias7,t.estatus7," +
"t.id_prov8,t.razon_social8,t.costo_unitario8,t.porc_desc8,t.importe_desc8,t.importe8,t.existencia8,t.dias8,t.estatus8," +
"t.id_prov9,t.razon_social9,t.costo_unitario9,t.porc_desc9,t.importe_desc9,t.importe9,t.existencia9,t.dias9,t.estatus9," +
"t.id_prov10,t.razon_social10,t.costo_unitario10,t.porc_desc10,t.importe_desc10,t.importe10,t.existencia10,t.dias10,t.estatus10," +
"t.id_prov11,t.razon_social11,t.costo_unitario11,t.porc_desc11,t.importe_desc11,t.importe11,t.existencia11,t.dias11,t.estatus11," +
"t.id_prov12,t.razon_social12,t.costo_unitario12,t.porc_desc12,t.importe_desc12,t.importe12,t.existencia12,t.dias12,t.estatus12," +
"t.id_prov13,t.razon_social13,t.costo_unitario13,t.porc_desc13,t.importe_desc13,t.importe13,t.existencia13,t.dias13,t.estatus13," +
"t.id_prov14,t.razon_social14,t.costo_unitario14,t.porc_desc14,t.importe_desc14,t.importe14,t.existencia14,t.dias14,t.estatus14," +
"t.id_prov15,t.razon_social15,t.costo_unitario15,t.porc_desc15,t.importe_desc15,t.importe15,t.existencia15,t.dias15,t.estatus15," +
"t.id_prov16,t.razon_social16,t.costo_unitario16,t.porc_desc16,t.importe_desc16,t.importe16,t.existencia16,t.dias16,t.estatus16," +
"t.id_prov17,t.razon_social17,t.costo_unitario17,t.porc_desc17,t.importe_desc17,t.importe17,t.existencia17,t.dias17,t.estatus17," +
"t.id_prov18,t.razon_social18,t.costo_unitario18,t.porc_desc18,t.importe_desc18,t.importe18,t.existencia18,t.dias18,t.estatus18," +
"t.id_prov19,t.razon_social19,t.costo_unitario19,t.porc_desc19,t.importe_desc19,t.importe19,t.existencia19,t.dias19,t.estatus19," +
"t.id_prov20,t.razon_social20,t.costo_unitario20,t.porc_desc20,t.importe_desc20,t.importe20,t.existencia20,t.dias20,t.estatus20," +
"r.refEstatus from tmp_comparativo t inner join Refacciones_Orden r on r.ref_id_taller=" + sesiones[3].ToString() + " and r.ref_id_empresa=" + sesiones[2].ToString() + " and r.ref_no_orden=" + sesiones[4].ToString() +
" and r.refOrd_Id=t.id_cotizacion_detalle where t.id_usuario=" + sesiones[0].ToString() + " and t.id_cotizacion_detalle in (select distinct cd.id_cotizacion_detalle from Cotizacion_Detalle cd where cd.id_empresa=r.ref_id_empresa and cd.id_taller=r.ref_id_taller and cd.no_orden=r.ref_no_orden)";
        object[] valores = ejecuta.dataSet(sql);
        object[] retorno = new object[2] { false, "" };
        if (Convert.ToBoolean(valores[0]))
        {
            retorno[0] = proveedoreRegistrados;
            retorno[1] = valores[1];
        }
        else {
            retorno = valores;
        }
        return retorno;
    }

    public object[] agregaProveedorCotizacionGral(int[] sesiones, int id, string refaccion, int cantidad, int idCliprov, decimal costo, decimal descuento, decimal importeDescuento, decimal importe, int existe, int dias, string idCot)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();

        sql = "insert into cotizacion_detalle values(" + sesiones[4].ToString() + "," + sesiones[3].ToString() + "," + sesiones[2].ToString() + "," + idCot + "," + id.ToString() + "," + idCliprov.ToString() + "," + cantidad.ToString() + ",'" + refaccion + "'," + costo.ToString() + "," + descuento.ToString() + "," + importeDescuento.ToString() + "," + importe.ToString() + ",'COT','" + fechaRetorno + "'," + id.ToString() + "," + existe.ToString() + "," + dias.ToString() + ",'M','R')";
        return ejecuta.insertUpdateDelete(sql);
    }

    public string obtieneCorreoProveedor(int proveedor, string tipo)
    {
        sql = "select correo from Cliprov where id_cliprov=" + proveedor + " and tipo='" + tipo + "'";
        return ejecuta.scalarToStringSimple(sql);
    }

    public string obtieneFolioCot(int empresa, int taller, int orden, string cotizacionSelect)
    {
        sql = "select folio from cotizacion_encabezado where no_orden=" + orden + " and id_empresa=" + empresa + " and id_taller=" + taller + " and id_cotizacion=" + cotizacionSelect;
        return ejecuta.scalarToStringSimple(sql);
    }
}