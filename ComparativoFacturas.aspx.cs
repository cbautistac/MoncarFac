using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class ComparativoFacturas : System.Web.UI.Page
{
    int contador;
    string id;
    decimal sumas;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {   
            lbFecha.Text = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
            RadGrid1.Rebind();
        }
    }

    private DataTable generaInfo()
    {
        string fechaActual = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
        string qry = "";
        DataTable dt = new DataTable();
        Facturas datosFacturas = new Facturas();

        qry = /*"DECLARE @fecha date set @fecha = '" + fechaActual + "' select tf.id_cliprov,tf.razon_social," +
"(select count(*) as registros from(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = tf.id_cliprov AND f.estatus = 'PEN') as tpen) as fPen," +
"(select isnull(sum(isnull(monto, 0)), 0) as registros from(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f  where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = tf.id_cliprov  AND f.estatus = 'PEN') as tpen) as mPen," +
"(SELECT COUNT(*) AS REGISTROS FROM(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = tf.id_cliprov) as tpag) as fPag," +
"(SELECT isnull(sum(isnull(monto, 0)), 0) AS REGISTROS FROM(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = tf.id_cliprov) as tpag) as mPag," +
"(SELECT COUNT(*) AS REGISTROS FROM(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = tf.id_cliprov) as tpro) as fPro," +
"(SELECT isnull(sum(isnull(monto, 0)), 0) AS REGISTROS FROM(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = tf.id_cliprov) as tpro) as mPro," +
"(SELECT COUNT(*) AS REGISTROS FROM(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NOT NULL and f.id_cliprov = tf.id_cliprov) as tpro) as fRev," +
"(SELECT isnull(sum(isnull(monto, 0)), 0) AS REGISTROS FROM(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NOT NULL and f.id_cliprov = tf.id_cliprov) as tpro) as mRev," +
"(SELECT COUNT(*) AS REGISTROS FROM(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and datediff(day, f.fechaprogpago, @fecha) > 0 and f.id_cliprov = tf.id_cliprov) as tpro) as fVen," +
"(SELECT isnull(sum(isnull(monto, 0)), 0) AS REGISTROS FROM(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social,case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and datediff(day, f.fechaprogpago, @fecha) > 0 and f.id_cliprov = tf.id_cliprov) as tpro) as mVen " +
"from(select distinct fa.id_cliprov, fa.razon_social from facturas fa where fa.tipocuenta = 'PA' ) as tf"*/
"declare @fechaActual date set @fechaActual = '" + fechaActual + "' select t.id_cliprov,t.razon_social, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov and no_orden = t.no_orden AND estatus = 'PEN' and factura = t.factura  order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden AND f.estatus = 'PEN') as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r ) as fPen, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov and no_orden = t.no_orden AND estatus = 'PEN' and factura = t.factura  order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden AND f.estatus = 'PEN') as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r ) as mPen, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as fPag, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as mPag, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as fPro, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as mPro, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' and estatus = 'CAN' and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' and f.estatus = 'CAN' and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as fCan, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' and estatus = 'CAN' and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' and f.estatus = 'CAN' and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as mCan, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and datediff(day, fechaprogpago, @fechaActual) > 0 and id_cliprov = t.id_cliprov and no_orden = no_orden and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and datediff(day, f.fechaprogpago, @fechaActual) > 0 and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as fVen, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and datediff(day, fechaprogpago, @fechaActual) > 0 and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and datediff(day, f.fechaprogpago, @fechaActual) > 0 and f.id_cliprov = t.id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as mVen " +
" from(select fa.id_cliprov, fa.razon_social from facturas fa where fa.tipocuenta = 'PA' group by fa.id_cliprov, fa.razon_social) as t " +
" order by t.id_cliprov";
        object[] info = datosFacturas.dataSet(qry);
        if (Convert.ToBoolean(info[0]))
        {
            DataSet ds = (DataSet)info[1];
            dt = ds.Tables[0];
        }

        return dt;

    }

     
    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
            (sender as RadGrid).DataSource = generaInfo();   
    }

    private string obtieneRazonSocial(int idCliprov)
    {
        string razonsocial = "";
        string sql = "select razon_social from cliprov where id_cliprov=" + idCliprov.ToString() + " and tipo='P'";
        Ejecuciones ejecuta = new Ejecuciones();
        object[] ejecutado = ejecuta.scalarToString(sql);
        if ((bool)ejecutado[0])
            razonsocial = ejecutado[1].ToString();
        else
            razonsocial = "";
        return razonsocial;
    }

    protected void btnfactura_Click(object sender, EventArgs e)
    {
        lblError.Text = lblErrorPopup.Text = "";
        LinkButton btnfactura = (LinkButton)sender;

        string factura = btnfactura.Text;
        string[] argumentos = btnfactura.CommandArgument.Split(';');
        lblOrden.Text = argumentos[3];
        lblIdEmpresa.Text = argumentos[1];
        lblIdTaller.Text = argumentos[2];
        lblFactura.Text = lblFacturaPop.Text = factura.ToString();
        lblIdCliprov.Text = argumentos[0];
        string estatus = argumentos[4];
        lblError.Text = lblErrorPopup.Text = "";
        lblProveedorPop.Text = obtieneRazonSocial(Convert.ToInt32(argumentos[0]));
        //lblRenglonFactura.Text = argumentos[5].ToString();
        lblSuma.Text = "0";
        lnkGuardarPopup.Visible = lnkSalirPop.Visible= true;
        lnkFechaPagoPop.Visible = lnkFechaProgPagopop.Visible = lnkFecharevisionPop.Visible = lnkGuardarPopup.Visible = true;
        ddlPoliticaPagoPop.Enabled = ddlFormaPagopop.Enabled = ddlBanco.Enabled = txtReferenciaPagPop.Enabled = txtObsevacionesPop.Enabled = txtPorcPPP.Enabled = txtPPP.Enabled = true;
        txtFirmante.Enabled = true;

        txtFechaPagoPop.Text = "";
        txtFechaProgPagopop.Text = "";
        txtFecharevisionPop.Text = "";
        txtPorcPPP.Text = "0.00";
        DataSet dataOtrosCostos = new DataSet();

        try
        {
            OtrosCostos otrosCostos = new OtrosCostos();
            otrosCostos.empresa = Convert.ToInt32(argumentos[1]);
            otrosCostos.taller = Convert.ToInt32(argumentos[2]);
            otrosCostos.orden = Convert.ToInt32(argumentos[3]);
            otrosCostos.factura = factura.ToString();
            otrosCostos.proveedor = Convert.ToDecimal(argumentos[0]);
            otrosCostos.obtieneInfoOtrosCostosCCyCP();

            if (Convert.ToBoolean(otrosCostos.retorno[0]))
            {                
                dataOtrosCostos = (DataSet)otrosCostos.retorno[1];
                if (dataOtrosCostos.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dataOtrosCostos.Tables[0].Rows)
                    {
                        lblDescuento.Text = r[12].ToString();
                        break;
                    }
                }
                else
                {
                    OrdenCompra ordenCompra = new OrdenCompra();
                    object[] ordenesCompra = ordenCompra.obtieneInfoOrdenCompra(Convert.ToInt32(argumentos[1]), Convert.ToInt32(argumentos[2]), Convert.ToInt32(argumentos[3]), factura.ToString(), Convert.ToDecimal(argumentos[0]));
                    dataOtrosCostos = new DataSet();
                    if (Convert.ToBoolean(ordenesCompra[0]))
                    {
                        dataOtrosCostos = (DataSet)ordenesCompra[1];
                        if (dataOtrosCostos.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow r in dataOtrosCostos.Tables[0].Rows)
                            {
                                lblDescuento.Text = r[12].ToString();
                                break;
                            }
                        }
                    }
                }

            }
        }
        catch (Exception ex) { dataOtrosCostos = null; }
        finally
        {
            GridDetalleFac.DataSource = dataOtrosCostos;
            GridDetalleFac.DataBind();
        }

        try
        {
            Facturas facturas = new Facturas();
            facturas.folio = Convert.ToInt32(argumentos[3]);
            facturas.tipoCuenta = "PA";
            facturas.factura = factura;
            facturas.id_cliprov = Convert.ToInt32(argumentos[0]);
            facturas.empresa = Convert.ToInt32(argumentos[1]);
            facturas.taller = Convert.ToInt32(argumentos[2]);
            facturas.orden = Convert.ToInt32(argumentos[3]);
            //facturas.renglon = Convert.ToInt32(argumentos[5]);
            facturas.obtieneInfoFactura();
            if (Convert.ToBoolean(facturas.retorno[0]))
            {
                dataOtrosCostos = new DataSet();
                dataOtrosCostos = (DataSet)facturas.retorno[1];

                CatClientes datosClientes = new CatClientes();

                foreach (DataRow r in dataOtrosCostos.Tables[0].Rows)
                {
                    try { txtFechaPagoPop.Text = Convert.ToDateTime(r[2]).ToString("yyyy-MM-dd"); } catch (Exception) { txtFechaPagoPop.Text = ""; }
                    try { txtFechaProgPagopop.Text = Convert.ToDateTime(r[1]).ToString("yyyy-MM-dd"); } catch (Exception) { txtFechaProgPagopop.Text = ""; }
                    try { txtFecharevisionPop.Text = Convert.ToDateTime(r[0]).ToString("yyyy-MM-dd"); } catch (Exception) { txtFecharevisionPop.Text = ""; }
                    try { ddlFormaPagopop.SelectedValue = r[3].ToString(); } catch (Exception) { ddlFormaPagopop.SelectedValue = "E"; }
                    txtReferenciaPagPop.Text = r[4].ToString();
                    try { ddlBanco.SelectedValue = r[5].ToString(); } catch (Exception) { ddlBanco.SelectedValue = ""; }
                    txtObsevacionesPop.Text = r[6].ToString();

                    try
                    {
                        if (string.IsNullOrEmpty(r[8].ToString().Trim()))
                            ddlPoliticaPagoPop.SelectedValue = datosClientes.obtieneIdPoliticaCliprov(int.Parse(r[7].ToString()));
                        else
                            ddlPoliticaPagoPop.SelectedValue = datosClientes.obtieneIdPoliticaPago(r[8].ToString());
                    }
                    catch (Exception) { ddlPoliticaPagoPop.SelectedValue = "0"; }

                    txtPPP.Text = r[9].ToString();
                    try { txtPorcPPP.Text = Convert.ToDecimal(r[10]).ToString("F2"); } catch (Exception) { txtPorcPPP.Text = "0.00"; }
                    try { lblDescuento.Text = Convert.ToDecimal(r[11]).ToString("F2"); } catch (Exception) { lblDescuento.Text = "0.00"; }
                    txtFirmante.Text = r[15].ToString();
                }
            }
        }
        catch (Exception ex) { }
        finally
        {
            calculaTotales();

            if (ddlFormaPagopop.SelectedValue != "E")
                ddlBanco.Enabled = txtReferenciaPagPop.Enabled = true;
            else
            {
                ddlBanco.Enabled = txtReferenciaPagPop.Enabled = false;
                ddlBanco.SelectedValue = "";
                txtReferenciaPagPop.Text = "";
            }
            activaValores();
            if (estatus == "PAG" || estatus == "CAN")
                ocultarValores();

            string script = "abreWinCtrl()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
        }
    }

    private void calculaTotales()
    {
        decimal suma = 0;
        decimal subtotal = 0;
        decimal total = 0;
        decimal descuento = 0;
        decimal descuentoAplicado = 0;
        decimal iva = Convert.ToDecimal(0.16);
        decimal ivaAplicado = 0;
        decimal descuentoPPP = 0;
        decimal descuentoPPPAplicado = 0;
        decimal montoAPagar = 0;
        try { suma = Convert.ToDecimal(lblSuma.Text); } catch (Exception) { suma = 0; }
        try { descuento = Convert.ToDecimal(lblDescuento.Text); } catch (Exception) { descuento = 0; }
        try { descuentoPPP = Convert.ToDecimal(txtPorcPPP.Text); } catch (Exception) { descuentoPPP = 0; }
        if (suma != 0)
        {
            lblSuma.Text = suma.ToString("F2");
            descuentoAplicado = suma * descuento;
            lblDescuentoAplicado.Text = descuentoAplicado.ToString("F2");
            subtotal = suma + descuentoAplicado;
            lblSubtotal.Text = subtotal.ToString("F2");
            ivaAplicado = subtotal * iva;
            total = subtotal + ivaAplicado;
            lblIVAAplicado.Text = ivaAplicado.ToString("F2");
            lblTotal.Text = total.ToString("F2");
            descuentoPPPAplicado = total * (descuentoPPP / 100);
            txtPorcPPPAplicado.Text = descuentoPPPAplicado.ToString("F2");
            montoAPagar = total - descuentoPPPAplicado;
            lblMontoAPagarSuma.Text = montoAPagar.ToString("F2");
        }
        else
        {
            lblErrorPopup.Text = "Se perdio la información generada, ingrese nuevamente a la factura para reestablecer la información.";
            lblSuma.Text = "0.00";
            lblDescuentoAplicado.Text = "0.00";
            lblSubtotal.Text = "0.00";
            lblIVAAplicado.Text = "0.00";
            lblTotal.Text = "0.00";
            txtPorcPPPAplicado.Text = "0.00";
            lblMontoAPagarSuma.Text = "0.00";
        }
    }

    private void ocultarValores()
    {
        lnkFechaPagoPop.Visible = lnkFechaProgPagopop.Visible = lnkFecharevisionPop.Visible = lnkGuardarPopup.Visible = false;
        ddlPoliticaPagoPop.Enabled = ddlFormaPagopop.Enabled = ddlBanco.Enabled = txtReferenciaPagPop.Enabled = txtObsevacionesPop.Enabled = txtPorcPPP.Enabled = txtPPP.Enabled = txtFirmante.Enabled = lblFacturaPop.Enabled = false;
    }

    private void activaValores()
    {
        lnkFechaPagoPop.Visible = lnkFechaProgPagopop.Visible = lnkFecharevisionPop.Visible = lnkGuardarPopup.Visible = true;
        ddlPoliticaPagoPop.Enabled = ddlFormaPagopop.Enabled = ddlBanco.Enabled = txtReferenciaPagPop.Enabled = txtObsevacionesPop.Enabled = txtPorcPPP.Enabled = txtPPP.Enabled = txtFirmante.Enabled = lblFacturaPop.Enabled = true;
    }

    protected void lnkGuardarPopup_Click(object sender, EventArgs e)
    {
        lblErrorPopup.Text = "";
        bool fechasValidas = false;
        string FECHArEVISION = "";
        DateTime fechaRev = new E_Utilities.Fechas().obtieneFechaLocal();
        try
        {
            fechaRev = Convert.ToDateTime(txtFecharevisionPop.Text);
            FECHArEVISION = fechaRev.ToString("yyyy-MM-dd");
            fechasValidas = true;
        }
        catch (Exception) { FECHArEVISION = ""; }
        if (lblFacturaPop.Text == "")
            lblErrorPopup.Text = "Debe indicar le numero de factura";
        else
        {
            if (FECHArEVISION != "")
            {
                if (fechaRev.Date > new E_Utilities.Fechas().obtieneFechaLocal().Date)
                    lblErrorPopup.Text = "La fecha de revisión no puede ser mayor a la fecha actual";
                else
                {
                    if (txtFechaProgPagopop.Text != "")
                    {
                        DateTime fechapp = Convert.ToDateTime(txtFechaProgPagopop.Text);
                        if (txtFecharevisionPop.Text != "" && (fechapp.Date < fechaRev.Date))
                            lblErrorPopup.Text = "La fecha programada de pago no puede ser menor a la fecha de revisión.";
                        else
                        {
                            if (txtFechaPagoPop.Text != "")
                            {
                                DateTime fechaPag = Convert.ToDateTime(txtFechaPagoPop.Text);
                                if (txtFecharevisionPop.Text != "" && txtFechaProgPagopop.Text != "" && (fechaPag.Date < fechaRev.Date))
                                    lblErrorPopup.Text = "La fecha de pago no puede ser menor a la fecha de revisión.";
                                else
                                    fechasValidas = true;
                            }
                            else
                                fechasValidas = true;
                        }
                    }
                    else
                        fechasValidas = true;
                }
            }
            if (fechasValidas)
            {
                string noOrden = lblOrden.Text;
                string idEmpresa = lblIdEmpresa.Text;
                string idTaller = lblIdTaller.Text;
                string factura = lblFactura.Text;
                string idCliprov = lblIdCliprov.Text;
                //string renglon = lblRenglonFactura.Text;

                string estatus = "PEN";
                if (txtFecharevisionPop.Text != "" && txtFechaPagoPop.Text == "")
                    estatus = "REV";
                else if (txtFecharevisionPop.Text != "" && txtFechaPagoPop.Text != "")
                    estatus = "PAG";
                else
                    estatus = "PEN";

                CatClientes datosClientes = new CatClientes();
                string clavePolitica = datosClientes.obtieneClavePoliticaPago(ddlPoliticaPagoPop.SelectedValue.ToString());
                Facturas datosFacturas = new Facturas();


                decimal importe = 0;
                string condicionante = "";
                if (lblTotal.Text != "")
                {
                    importe = Convert.ToDecimal(lblTotal.Text);
                    if (importe != 0)
                        condicionante = ",Importe=" + obtieneNulo(lblTotal.Text, 0);


                }
                string sql = "update facturas set FechaRevision=" + obtieneNulo(txtFecharevisionPop.Text, 1) + ",FechaProgPago=" + obtieneNulo(txtFechaProgPagopop.Text, 1) + ",FechaPago=" + obtieneNulo(txtFechaPagoPop.Text, 1) + ", " +
                    "FormaPago=" + obtieneNulo(ddlFormaPagopop.SelectedValue, 1) + ",ReferenciaPago=" + obtieneNulo(txtReferenciaPagPop.Text, 1) + ",clvBanco=" + obtieneNulo(ddlBanco.SelectedValue, 1) + ",Observaciones=" + obtieneNulo(txtObsevacionesPop.Text, 1) + ",clv_politica=" + obtieneNulo(clavePolitica, 1) + "," +
                    "Estatus='" + estatus + "'"+condicionante + ",concepto=" + obtieneNulo(txtPPP.Text, 1) + ",porcentaje_pp=" + obtieneNulo(txtPorcPPP.Text, 0) + ",importe_pp=" + obtieneNulo(txtPorcPPPAplicado.Text, 0) + ",monto_pagar=" + obtieneNulo(lblMontoAPagarSuma.Text, 0) + ",Imp_descuento=" + obtieneNulo(lblDescuentoAplicado.Text, 0) + ", " +
                    "Porcentaje_desc=" + obtieneNulo(lblDescuento.Text, 0) + ", firmante ='" + txtFirmante.Text + "', factura='" + lblFacturaPop.Text + "' " +
                    " where Factura='" + factura + "' and id_cliprov=" + idCliprov + " and id_taller=" + idTaller + " and id_empresa=" + idEmpresa + " and no_orden=" + noOrden;//+ " and renglon=" + renglon;
                datosFacturas.actualizaFacturas(sql, 0);
                object[] actualizado = datosFacturas.retorno;
                if ((bool)actualizado[0])
                {
                    if ((bool)actualizado[1])
                    {
                        lblErrorPopup.Text = "Datos actualizados exitosamente.";
                        if (txtFechaPagoPop.Text != "")
                            ocultarValores();
                        string script = "cierraWinCtrl()";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
                    }
                    else
                        lblErrorPopup.Text = "Ocurrio un error inesperado en la actualización: " + actualizado[1].ToString();
                }
            }
            else
                lblErrorPopup.Text = "Una o más fechas inidicadas no es válida, por favor verifique";
        }
    }

    private string obtieneNulo(string text, int opcion)
    {
        if (opcion == 1)
        {
            if (text == "")
                return " null ";
            else
                return " '" + text + "' ";
        }
        else
        {
            if (text == "")
                return " null ";
            else
                return " " + text + " ";
        }
    }

    protected void ddlPoliticaPagoPop_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaFechaProg();
    }

    private void llenaFechaProg()
    {
        try
        {
            string idPolitica = ddlPoliticaPagoPop.SelectedValue;
            string sql = "select dias_plazo from politica_pago where id_politica=" + idPolitica;
            Ejecuciones ejecuta = new Ejecuciones();
            object[] ejecutado = ejecuta.scalarToString(sql);
            int diasPlazo = 0;
            if ((bool)ejecutado[0])
                diasPlazo = Convert.ToInt32(ejecutado[1].ToString());
            if (txtFecharevisionPop.Text != "")
            {
                DateTime fechaRevison = Convert.ToDateTime(txtFecharevisionPop.Text);
                DateTime fechaProgPago = fechaRevison.AddDays(diasPlazo);
                txtFechaProgPagopop.Text = fechaProgPago.ToString("yyyy-MM-dd");
            }
        }
        catch (Exception) { txtFechaProgPagopop.Text = ""; }
    }

    protected void ddlFormaPagopop_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFormaPagopop.SelectedValue != "E")
            ddlBanco.Enabled = txtReferenciaPagPop.Enabled = true;
        else
        {
            ddlBanco.Enabled = txtReferenciaPagPop.Enabled = false;
            ddlBanco.SelectedValue = "";
            txtReferenciaPagPop.Text = "";
        }
    }

    protected void RadGridPendientes_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;

            string FechaRevision = r[1].ToString();
            string FechaProgPago = r[2].ToString();
            string FechaPago = r[3].ToString();

            var btn = e.Item.Cells[0].Controls[0].FindControl("btnfactura") as LinkButton;
            if (FechaPago != "")
                btn.CssClass = "btn btn-success textoBold";
            else
            {
                if (FechaProgPago != "")
                    btn.CssClass = "btn btn-info textoBold";
                else
                {
                    if (FechaRevision != "")
                        btn.CssClass = "btn btn-primary textoBold";
                    else
                        btn.CssClass = "btn btn-danger textoBold";
                }
            }
        }
    }

    protected void txtFecharevisionPop_TextChanged(object sender, EventArgs e)
    {
        llenaFechaProg();
    }

    protected void GridDetalleFac_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            sumas = 0;
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblMonto = e.Row.FindControl("lblMonto") as Label;
            decimal monto = Convert.ToDecimal(lblMonto.Text);
            sumas = sumas + monto;
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
            lblSuma.Text = sumas.ToString("F2");
    }

    protected void txtPorcPPP_TextChanged(object sender, EventArgs e)
    {
        calculaTotales();
    }

    protected void lnkSalirPop_Click(object sender, EventArgs e)
    {
        RadGrid1.Rebind();
        string script = "cierraWinCtrl()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
    }

    protected void lnkDescarga_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        ImprimeReporteFactuPA facturacion = new ImprimeReporteFactuPA();
        facturacion.usuario = sesiones[0];
        facturacion.empresa = sesiones[2];
        facturacion.taller = sesiones[3];
        facturacion.generaReporte(0);
        string archivo = facturacion.archivo;
        if (archivo != "")
        {
            try
            {
                FileInfo docto = new FileInfo(archivo);
                if (docto.Exists)
                {
                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + docto.Name);
                    Response.WriteFile(archivo);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al accesar al archivo en el servidor. Detalle: " + ex.Message;
            }
        }
        else
            lblError.Text = "No se puedo generar el documento por favor vuelva a intentar";
    }

    protected void lnkDescargaProv_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        LinkButton btn = (LinkButton)sender;
        int id = Convert.ToInt32(btn.CommandArgument.ToString());
        ImprimeReporteFactuPA facturacion = new ImprimeReporteFactuPA();
        facturacion.usuario = sesiones[0];
        facturacion.empresa = sesiones[2];
        facturacion.taller = sesiones[3];
        facturacion.generaReporte(id);
        string archivo = facturacion.archivo;
        if (archivo != "")
        {
            try
            {
                FileInfo docto = new FileInfo(archivo);
                if (docto.Exists)
                {
                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + docto.Name);
                    Response.WriteFile(archivo);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al accesar al archivo en el servidor. Detalle: " + ex.Message;
            }
        }
        else
            lblError.Text = "No se puedo generar el documento por favor vuelva a intentar";
    }
    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[4] { 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);

        }
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "ReporteFacturacion.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(item.FindControl("lnkDescargaPorv"));

        }
    }
}

