using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Data;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;

public partial class FacturasPorPagar : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    Ejecuciones ejecuta = new Ejecuciones();
    decimal sumas;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargaInfo();
            lblEstatus.Text = "";
        }
    }

    private void cargaInfo()
    {
        try
        {
            int[] sesiones = obtieneSesiones();
            if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0)
                Response.Redirect("Default.aspx");
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }

        string qryEstatusTot = /*"select " +
"(select count(*) from(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social, " +
"case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto " +
"from facturas f where f.TipoCuenta = 'PA' ) as t) as total, " +
"(select count(*) from(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social, " +
"case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto " +
"from facturas f where f.TipoCuenta = 'PA' ) as t where t.FechaPago IS NULL AND t.FechaProgPago IS NULL AND t.FechaRevision IS NULL) as srv, " +
"(select count(*) from(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social, " +
"case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto " +
"from facturas f where f.TipoCuenta = 'PA' ) as t where t.FechaPago IS NULL AND t.FechaProgPago IS NULL AND t.FechaRevision IS not NULL) as rev, " +
"(select count(*) from(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social, " +
"case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto " +
"from facturas f where f.TipoCuenta = 'PA' ) as t where t.FechaPago IS NULL AND t.FechaProgPago IS not NULL) as pro, " +
"(select count(*) from(select distinct f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, isnull(f.importe, 0) as importe, isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social, " +
"case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto " +
"from facturas f where f.TipoCuenta = 'PA' ) as t where t.FechaPago IS not NULL) as pag"*/
" select (select count(*) from(select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov and no_orden=t.no_orden AND estatus = 'PEN' and factura = t.factura  order by 1 desc) as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.no_orden=no_orden and f.id_cliprov = id_cliprov AND f.estatus = 'PEN') as t group by factura, FechaRevision, FechaProgPago, FechaPago, id_cliprov, folio, estatus, id_empresa, id_taller, no_orden, clv_politica, razon_social) as t1) as srev, " +
" (select count(*) from(select *,  (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND(FechaPago IS  NULL AND FechaProgPago IS NULL AND FechaRevision IS NOT NULL AND estatus = 'REV') or(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS not NULL AND estatus = 'PRO') and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) /1.16) as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where tipocuenta = 'PA' AND(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NOT NULL AND estatus = 'REV') or(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS not NULL AND estatus = 'PRO') and f.no_orden = no_orden and id_cliprov = id_cliprov) as t group by factura, FechaRevision, FechaProgPago, FechaPago, id_cliprov, folio, estatus, id_empresa, id_taller, no_orden, clv_politica, razon_social) as t1) as rev, " +
" (select count(*) from(select *,  (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) / 1.16)as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social) as t1) as pro, " +
" (select count(*) from(select *,  (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc) / 1.16)as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social) as t1) as pag";

        FacturacionElectronica.Ejecucion eje = new FacturacionElectronica.Ejecucion();
        eje.baseDatos = "PVW";
        object[] obj = eje.dataSet(qryEstatusTot);

        if (Convert.ToBoolean(obj[0]))
        {
            DataTable dt = ((DataSet)obj[1]).Tables[0];
            foreach (DataRow r in dt.Rows) {
                lnkEstatusSinRev.Text = "Sin Revisión: " + r[0].ToString();
                lnkEstatusRev.Text = "Revisadas: " + r[1].ToString();
                lnkEstatusProg.Text = "Programadas: " + r[2].ToString();
                lnkEstatusPag.Text = "Pagadas: " + r[3].ToString();
            }            
        }
        //RadGrid1.Rebind();
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
        catch (Exception x)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
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
        lnkGuardarPopup.Visible = true;
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
                    if (Convert.ToBoolean(ordenesCompra[0])) {
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
        finally {
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

                    try {
                        if (string.IsNullOrEmpty(r[8].ToString().Trim()))
                            ddlPoliticaPagoPop.SelectedValue = datosClientes.obtieneIdPoliticaCliprov(int.Parse(r[7].ToString()));
                        else
                            ddlPoliticaPagoPop.SelectedValue = datosClientes.obtieneIdPoliticaPago(r[8].ToString());
                    } catch (Exception) { ddlPoliticaPagoPop.SelectedValue = "0"; }

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
            if (estatus == "PAG" || estatus=="CAN")
                ocultarValores();

            string script = "abreWinCtrl()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
        }            
    }

    private void ocultarValores()
    {
        lnkFechaPagoPop.Visible = lnkFechaProgPagopop.Visible = lnkFecharevisionPop.Visible = lnkGuardarPopup.Visible = false;
        ddlPoliticaPagoPop.Enabled = ddlFormaPagopop.Enabled = ddlBanco.Enabled = txtReferenciaPagPop.Enabled = txtObsevacionesPop.Enabled = txtPorcPPP.Enabled = txtPPP.Enabled = txtFirmante.Enabled= lblFacturaPop.Enabled= false;
    }

    private void activaValores()
    {
        lnkFechaPagoPop.Visible = lnkFechaProgPagopop.Visible = lnkFecharevisionPop.Visible = lnkGuardarPopup.Visible = true;
        ddlPoliticaPagoPop.Enabled = ddlFormaPagopop.Enabled = ddlBanco.Enabled = txtReferenciaPagPop.Enabled = txtObsevacionesPop.Enabled = txtPorcPPP.Enabled = txtPPP.Enabled = txtFirmante.Enabled = lblFacturaPop.Enabled = true;
    }

    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;
            
            string FechaRevision = r[1].ToString();
            string FechaProgPago = r[2].ToString();
            string FechaPago = r[3].ToString();
            string estatus = r[6].ToString();
            string idcliprov = r[4].ToString();
            
            var btn = e.Item.Cells[0].Controls[0].FindControl("btnfactura") as LinkButton;
            if ( (estatus == "PAG" && FechaPago != "") || (FechaPago == "" && estatus == "PAG"))
                btn.CssClass = "btn btn-success textoBold";
            else
            {
                if (FechaPago == "" && FechaProgPago != "")
                    btn.CssClass = "btn btn-info textoBold";
                else
                {
                   if ((FechaProgPago == "" && FechaPago == "" && FechaRevision != "" && estatus == "REV") || (FechaProgPago == "" && FechaPago == "" && FechaRevision != "" && estatus == "PRO"))
                      btn.CssClass = "btn btn-primary textoBold";
                    else
                        if (FechaPago == "" && FechaProgPago == "" && FechaRevision == "" && estatus == "PEN")
                        btn.CssClass = "btn btn-danger textoBold";
                }
            }
        }
    }

    private decimal obtieneMonto(string noOrden, string idEmpresa, string idTaller, string factura, int idCliprov)
    {
        decimal monto = 0;
        //double unMonto = 0;
        //double montosiniva = 0;
        string sql = "select isnull(sum(importe+(importe*0.16)),0) from otros_costos " +
            "where no_orden = " + noOrden + " and id_empresa =" + idEmpresa + " and id_taller =" + idTaller + " and factura = '" + factura + "' and proveedor =" + idCliprov;
        object[] ejecutado = ejecuta.scalarToDecimal(sql);
        if ((bool)ejecutado[0])
            monto = Convert.ToDecimal(ejecutado[1].ToString());
        else
            monto = 0;

        return monto;
    }

    private string obtieneRazonSocial(int idCliprov)
    {
        string razonsocial = "";
        string sql = "select razon_social from cliprov where id_cliprov=" + idCliprov.ToString() + " and tipo='P'";
        object[] ejecutado = ejecuta.scalarToString(sql);
        if ((bool)ejecutado[0])
            razonsocial = ejecutado[1].ToString();
        else
            razonsocial = "";
        return razonsocial;
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
                if (fechaRev.Date > fechas.obtieneFechaLocal().Date)
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
                    "Estatus='" + estatus + "'" + condicionante + ",concepto=" + obtieneNulo(txtPPP.Text, 1) + ",porcentaje_pp=" + obtieneNulo(txtPorcPPP.Text, 0) + ",importe_pp=" + obtieneNulo(txtPorcPPPAplicado.Text, 0) + ",monto_pagar=" + obtieneNulo(lblMontoAPagarSuma.Text, 0) + ",Imp_descuento=" + obtieneNulo(lblDescuentoAplicado.Text, 0) + ", " +
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
                        {
                            ocultarValores();
                        }
                        string script = "cierraWinCtrl()";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
                        RadGrid1.DataBind();
                    }
                    else
                        lblErrorPopup.Text = "Ocurrio un error inesperado en la actualización: " + actualizado[1].ToString();
                }
            }
            else
                lblErrorPopup.Text = "Una o más fechas inidicadas no es válida, por favor verifique";
        }
        /*
        *Agregado por Alfredo Gonzalez el 04Ago2017
        **********************************************************************
        * si es el caso en que el textBox "lblFacturaPop" sea diferente a vacio ...
        * se actualiza la tabla de facturas con este dato.
        * tambien se debe de actualizar la tabla de encabezado de ordenes de compra con el
        * nuevo valor (el que proviene de "lblFacturaPop") sera reemplazado en el cam,po factura 
        * del encabezado de orden de compra.
        *
        * Nuestra referencia es el numero de orden, ya que este no debe de cambiar
        * campo [Orden_Compra_Encabezado]. [no_orden]
        */
        if (lblFacturaPop.Text != "") 
        {
            string noOrden = lblOrden.Text;
            string idEmpresa = lblIdEmpresa.Text;
            string idTaller = lblIdTaller.Text;
            string factura = lblFactura.Text;
            string idCliprov = lblIdCliprov.Text;
            string facturaRealProveedor = lblFacturaPop.Text;

            // variable donde traemos el folio real de la factura del proveedor.
            // contenido en el textbox con id= lblFacturaPop

            //creacion de instancia para actualizacion de dato en Tabla:facturas
            Facturas datoXActualizar = new Facturas();
            //se vacian datos requeridos para la actualizacion en tabla facturas
            //en las propiedades de clase adecuadas.
            datoXActualizar.orden = Convert.ToInt32(noOrden); //numero de orden
            datoXActualizar.empresa = Convert.ToInt32(idEmpresa);
            datoXActualizar.taller = Convert.ToInt32(idTaller);
            datoXActualizar.factura = factura;
            datoXActualizar.folioFacturaRealDelProveedor = facturaRealProveedor;
            //ejecucion del metodo que actualiza el dato en facturas
            datoXActualizar.actualizaFolioRealProveedorEnFactura();
            /*
             * Hasta aqui la actualizacion esta concluida en la tabla de 
             * facturas(base de datos e-factura), falta la misma actualizacion en
             * la tabla  de encabezado de facturas.
             */

            //preparacion para la actualizacion del fdato real del folio 
            //de proveedor en tabla de "Orden_Compra_Encabezado".

            cuentasXPagar datoXactualizarEnOrdenesComprasEncabezado = new cuentasXPagar();
            datoXactualizarEnOrdenesComprasEncabezado.no_orden = Convert.ToInt32( noOrden );
            datoXactualizarEnOrdenesComprasEncabezado.id_empresa = Convert.ToInt32(idEmpresa);
            datoXactualizarEnOrdenesComprasEncabezado.id_taller = Convert.ToInt32(idTaller);
            datoXactualizarEnOrdenesComprasEncabezado.id_cliprov = Convert.ToInt32(idCliprov);
            datoXactualizarEnOrdenesComprasEncabezado.factura = factura;
            datoXactualizarEnOrdenesComprasEncabezado.folioFacturaRealDelProveedor = facturaRealProveedor;

            //ejecutamos actualizacion en Encabezado de Ordenes de Compra ...
            datoXactualizarEnOrdenesComprasEncabezado.actualizaFolioRealFacturaEnOrdenCompraEncabezado();

            //Actualizacion completa!!!



        }

    }

    private string obtieneNulo(string text, int opcion)
    {
        if (opcion==1)
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

    private void llenaFechaProg()
    {
        try
        {
            string idPolitica = ddlPoliticaPagoPop.SelectedValue;
            string sql = "select dias_plazo from politica_pago where id_politica=" + idPolitica;
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

    protected void ddlPoliticaPagoPop_SelectedIndexChanged(object sender, EventArgs e)
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
            descuentoPPPAplicado = total * (descuentoPPP/100);
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

    protected void txtPorcPPP_TextChanged(object sender, EventArgs e)
    {
        calculaTotales();
    }

    protected void ddlFormaPagopop_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFormaPagopop.SelectedValue != "E")
        {
            ddlBanco.Enabled = txtReferenciaPagPop.Enabled = true;
        }
        else
        {
            ddlBanco.Enabled = txtReferenciaPagPop.Enabled = false;
            ddlBanco.SelectedValue = "";
            txtReferenciaPagPop.Text = "";
        }
    }

    protected void txtFecharevisionPop_TextChanged(object sender, EventArgs e)
    {
        llenaFechaProg();
    }

    protected void lnkEstatus_Click(object sender, EventArgs e)
    {
        LinkButton lnkEstat = (LinkButton)sender;
        if (lnkEstat.CommandArgument.ToString() != "TOD")
            lblEstatus.Text = lnkEstat.CommandArgument;
        else
            lblEstatus.Text = "";
        RadGrid1.Rebind();
    }

    private DataTable generaInfo() {
        string estatus = lblEstatus.Text;

        //string qry = "select distinct factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,isnull(importe,0) as importe,isnull(monto_pagar,0) as monto_pagar, renglon,razon_social,case isnull(importe,0) when 0 then isnull(monto,0) else isnull(importe,0) end as monto from facturas where TipoCuenta='PA' ";
        string qry ="" /*"select distinct f.factura,f.FechaRevision,f.FechaProgPago,f.FechaPago,f.id_cliprov,f.folio,estatus,f.id_empresa,f.id_taller,f.no_orden,f.clv_politica,isnull(f.importe, 0) as importe,isnull(f.monto_pagar, 0) as monto_pagar, f.razon_social," +
"case isnull(f.importe, 0) when 0 then(select TOP 1 a.monto from facturas a where a.TipoCuenta = f.tipocuenta AND a.FACTURA = f.factura AND a.ID_EMPRESA = f.id_empresa AND a.ID_TALLER = f.id_taller AND a.NO_ORDEN = f.no_orden AND a.ID_CLIPROV = f.id_cliprov ORDER BY a.RENGLON DESC) else isnull(f.importe, 0) end as monto " +
"from facturas f where f.TipoCuenta = 'PA' "*/;
        /*
         Modificacion 05ago2017
         Alfredo Gonzalez Davila
         El cliente pide que en la tabla general de CXP se despliegen los datos del monto sin iva
         se agrega antes de " as monto" en los select la operacion " / 1.16"
         para no alterar el flujo del programa actual.
         */
        switch (estatus)
        {
            case "PEN":
                qry = "select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov  and no_orden = t.no_orden AND estatus = 'PEN' and factura = t.factura  order by 1 desc ) /1.16 as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = id_cliprov and f.no_orden = no_orden AND f.estatus = 'PEN') as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social";
                break;
            case "REV":
                qry = "select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND(FechaPago IS  NULL AND FechaProgPago IS NULL AND FechaRevision IS NOT NULL AND estatus = 'REV') or(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS not NULL AND estatus = 'PRO') and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc ) / 1.16 as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where tipocuenta = 'PA' AND(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NOT NULL AND estatus = 'REV') or(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS not NULL AND estatus = 'PRO') and id_cliprov = id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social";
                break;
            case "PRO":
                qry = "select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc ) /1.16 as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social";
                break;
            case "PAG":
                qry = "select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc ) /1.16 as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = id_cliprov and f.no_orden = no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social";
                break;
            default:
                qry =
            " select * from(select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov and no_orden = t.no_orden AND estatus = 'PEN' and factura = t.factura  order by 1 desc ) /1.16 as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = id_cliprov and f.no_orden = no_orden AND f.estatus = 'PEN') as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social " + 
                    " UNION ALL " +
" select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND(FechaPago IS  NULL AND FechaProgPago IS NULL AND FechaRevision IS NOT NULL AND estatus = 'REV') or(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS not NULL AND estatus = 'PRO') and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc ) as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where tipocuenta = 'PA' AND(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NOT NULL AND estatus = 'REV') or(FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS not NULL AND estatus = 'PRO') and id_cliprov = id_cliprov and f.no_orden=no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social" + 
" union all " +
"select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc ) as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = id_cliprov and f.no_orden=no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social " + 
" union all " +
"select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov = t.id_cliprov and no_orden = t.no_orden and factura = t.factura order by 1 desc ) as monto from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = id_cliprov and f.no_orden=no_orden) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social) as r ";               
                break;
        }
        Facturas datosFacturas = new Facturas();
        DataTable dt = new DataTable();
        try
        {
            object[] info = datosFacturas.dataSet(qry);
            if (Convert.ToBoolean(info[0]))
            {
                DataSet datos = (DataSet)info[1];
                dt = datos.Tables[0];
            }
            else
                dt = new DataTable();
        }
        catch (Exception ex) { dt = new DataTable(); }
        return dt;

    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = generaInfo();
        if (dt.Rows.Count == 0)
        {
            lblEstatus.Text = "";
            dt = generaInfo();
        }

        (sender as RadGrid).DataSource = dt;
        cargaInfo();
    }
}