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

public partial class CuentasCobrar : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    Ejecuciones ejecuta = new Ejecuciones();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblEstatus.Text = "";
            cargaInfo();
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

        string qryEstatusTot = "SELECT can.totCanceladas, rev.totRevisadas, pen.totPend, pag.totPagadas FROM " +
            "(SELECT COUNT(Factura) AS totCanceladas FROM Facturas WHERE TipoCuenta = 'CC' AND Estatus = 'CAN') AS can, " +
            "(SELECT COUNT(Factura) AS totRevisadas FROM Facturas WHERE TipoCuenta = 'CC' AND Estatus = 'REV') AS rev, " +
            "(SELECT COUNT(Factura) AS totPend FROM Facturas WHERE TipoCuenta = 'CC' AND Estatus = 'PEN') AS pen, " +
            "(SELECT COUNT(Factura) AS totPagadas FROM Facturas WHERE TipoCuenta = 'CC' AND Estatus = 'PAG') AS pag";

        FacturacionElectronica.Ejecucion eje = new FacturacionElectronica.Ejecucion();
        eje.baseDatos = "PVW";
        object[] obj = eje.dataSet(qryEstatusTot);
        
        if (Convert.ToBoolean(obj[0]))
        {
            DataTable dt = ((DataSet)obj[1]).Tables[0];
            lnkEstatusCan.Text = "Canceladas: " + dt.Rows[0]["totCanceladas"].ToString();
            lnkEstatusRev.Text = "Revisadas: " + dt.Rows[0]["totRevisadas"].ToString();
            lnkEstatusPend.Text = "Pendientes: " + dt.Rows[0]["totPend"].ToString();
            lnkEstatusPag.Text = "Pagadas: " + dt.Rows[0]["totPagadas"].ToString();
        }
            
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
        lblImporte.Text = Convert.ToDecimal(argumentos[4]).ToString("F2");
        string status = argumentos[5].ToString();
        lblError.Text = lblErrorPopup.Text = "";
        lnkGuardarPopup.Visible = true;
        lblRenglonFactura.Text = argumentos[6].ToString();

        lblOrdenF.Text = lblTaller.Text = lblValuacion.Text = lblFrecepcion.Text = lblFentregaEst.Text = lblCliente.Text = lblAseguradora.Text = 
            lblAsegurado.Text = lblVehiculo.Text = lblModelo.Text = lblSiniestro.Text = lblPlacas.Text = lblPoliza.Text = "";

        pnlAnticipos.Visible = false;
        RadGrid4.DataBind();
        try
        {
            Recepciones recepcion = new Recepciones();
            object[] datosOrden = recepcion.obtieneInfoOrdenCC(argumentos[1], argumentos[2], argumentos[3]);
            if (Convert.ToBoolean(datosOrden[0]))
            {
                DataSet infoOrdne = (DataSet)datosOrden[1];
                foreach(DataRow dato in infoOrdne.Tables[0].Rows)
                {
                    lblOrdenF.Text = Convert.ToString(dato[0]);
                    lblTaller.Text = Convert.ToString(dato[3]);
                    lblValuacion.Text = Convert.ToString(dato[4]);
                    try { lblFrecepcion.Text = Convert.ToDateTime(dato[5]).ToString("dd/MM/yyyy"); } catch (Exception) { lblFrecepcion.Text = ""; }
                    try { lblFentregaEst.Text = Convert.ToDateTime(dato[6]).ToString("dd/MM/yyyy"); } catch (Exception) { lblFentregaEst.Text = ""; }
                    lblCliente.Text = Convert.ToString(dato[8]);
                    lblAseguradora.Text = Convert.ToString(dato[10]);
                    lblAsegurado.Text = Convert.ToString(dato[11]);
                    lblVehiculo.Text = Convert.ToString(dato[12]);
                    lblModelo.Text = Convert.ToString(dato[13]);
                    lblPlacas.Text = Convert.ToString(dato[14]);
                    lblSiniestro.Text = Convert.ToString(dato[15]);
                    lblPoliza.Text = Convert.ToString(dato[16]);
                    break;
                }
            }
            else
            {
                lblErrorPopup.Text = "Error al cargar la información de la factura. " + Convert.ToString(datosOrden[1]);
                lnkGuardarPopup.Visible = false;
            }


            Facturas facturas = new Facturas();            
            facturas.tipoCuenta = "CC";
            facturas.factura = factura;
            facturas.id_cliprov = Convert.ToInt32(argumentos[0]);
            facturas.empresa = Convert.ToInt32(argumentos[1]);
            facturas.taller = Convert.ToInt32(argumentos[2]);
            facturas.orden = Convert.ToInt32(argumentos[3]);
            facturas.renglon = Convert.ToInt32(argumentos[6]);
            facturas.obtieneFolioFactura();
            try { facturas.folio = Convert.ToInt32(facturas.retorno[1]); } catch (Exception ex) { facturas.folio = Convert.ToInt32(argumentos[3]); }
            facturas.obtieneInfoFactura();
            if (Convert.ToBoolean(facturas.retorno[0]))
            {
                DataSet dataOtrosCostos = new DataSet();
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

                    try { if (r[9].ToString() != "") txtPPP.Text = r[9].ToString(); else txtPPP.Text = "P.P.P"; } catch (Exception) { txtPPP.Text = "P.P.P"; }
                    try { radDcto.Value = Convert.ToDouble(r[10]); } catch (Exception) { radDcto.Value = 0; }
                    try { radImpDcto.Value = Convert.ToDouble(r[12]); } catch (Exception) { radImpDcto.Value = 0; }
                    try { lblMontoPagar.Text = Convert.ToDecimal(r[13]).ToString("F2"); } catch (Exception) { lblMontoPagar.Text = lblImporte.Text; }
                    try { txtNota.Text = r[14].ToString(); } catch (Exception) { txtNota.Text = ""; }
                    if (radImpDcto.Value != 0)
                        chkNota.Checked = true;
                    else
                        chkNota.Checked = false;
                }

                facturas.obtieneUltimoResto();
                if (Convert.ToBoolean(facturas.retorno[0])) {
                    decimal valor = Convert.ToDecimal(facturas.retorno[1]);
                    if (valor == 0)
                        radMontoAnticipo.MaxValue = Convert.ToDouble(lblMontoPagar.Text);
                    else
                        radMontoAnticipo.MaxValue = Convert.ToDouble(valor);
                }

            }
        }
        catch (Exception ex) { lblErrorPopup.Text = "Error al cargar la información de la factura. " + ex.Message; lnkGuardarPopup.Visible = false; }
        finally
        {
            
            activaCamposNota();
            activaCampos();
            activaValores();
            if (status == "PAG" || status=="CAN")
                ocultarValores();
            string script = "abreWinCtrl()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
        }
    }

    protected void radDcto_TextChanged(object sender, System.EventArgs e)
    {
        if (radDcto.Value != 0)
        {
            decimal importe = Convert.ToDecimal(lblImporte.Text);
            decimal valorDescuento = importe * (Convert.ToDecimal(radDcto.Value) / 100);
            radImpDcto.Value = Convert.ToDouble(valorDescuento);
            lblMontoPagar.Text = (importe - valorDescuento).ToString("F2");
        }
        else
            lblMontoPagar.Text = lblImporte.Text;
    }

    protected void radImpDcto_TextChanged(object sender, System.EventArgs e)
    {
        if (radImpDcto.Value != 0)
        {
            decimal importe = Convert.ToDecimal(lblImporte.Text);
            decimal valorDescuento = (Convert.ToDecimal(radImpDcto.Value) * 100) / importe;
            radDcto.Value = Convert.ToDouble(valorDescuento);
            lblMontoPagar.Text = (importe - Convert.ToDecimal(radImpDcto.Value)).ToString("F2");
        }
        else
            lblMontoPagar.Text = lblImporte.Text;
    }

    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;
            
            var lnk = e.Item.FindControl("btnfactura") as LinkButton;            
            string estatus = r[7].ToString();

            if (estatus == "PEN")
                lnk.CssClass = "btn btn-info textoBold";
            else if (estatus == "PAG")
                lnk.CssClass = "btn btn-success textoBold";
            else if (estatus == "REV")
                lnk.CssClass = "btn btn-warning textoBold";
            else if (estatus == "CAN")
                lnk.CssClass = "btn btn-danger textoBold";
            
        }
    }

    private string obtieneNombreTaller(string idTaller)
    {
        string sql = "select nombre_taller from talleres " +
            "where id_taller =" + idTaller;
        object[] ejecutado = ejecuta.scalarToString(sql);
        if ((bool)ejecutado[0])
            return ejecutado[1].ToString();
        else
            return "";
    }

    private string[] obtieneSieniestroPoliza(string noOrden, string idEmpresa, string idTaller)
    {
        string[] sinisPlza;
        string sql = "select isnull(ltrim(rtrim(no_siniestro)),'')+';'+isnull(ltrim(rtrim(no_poliza)),'') as sinisPlza from Ordenes_Reparacion " +
            "where no_orden =" + noOrden + " and id_empresa =" + idEmpresa + " and id_taller =" + idTaller;
        object[] ejecutado = ejecuta.scalarToString(sql);
        if ((bool)ejecutado[0])
            sinisPlza = ejecutado[1].ToString().Split(';');
        else
            sinisPlza = null;
        return sinisPlza;
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
        DateTime fechaRev = Convert.ToDateTime(txtFecharevisionPop.Text);
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
        if (fechasValidas)
        {
            string noOrden = lblOrden.Text;
            string idEmpresa = lblIdEmpresa.Text;
            string idTaller = lblIdTaller.Text;
            string factura = lblFactura.Text;
            string idCliprov = lblIdCliprov.Text;
            string renglon = lblRenglonFactura.Text;

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

            string sql = "update facturas set FechaRevision=" + obtieneNulo(txtFecharevisionPop.Text, 1) + ",FechaProgPago=" + obtieneNulo(txtFechaProgPagopop.Text, 1) + ",FechaPago=" + obtieneNulo(txtFechaPagoPop.Text, 1) + ", " +
                "FormaPago=" + obtieneNulo(ddlFormaPagopop.SelectedValue, 1) + ",ReferenciaPago=" + obtieneNulo(txtReferenciaPagPop.Text, 1) + ",clvBanco=" + obtieneNulo(ddlBanco.SelectedValue, 1) + ",Observaciones=" + obtieneNulo(txtObsevacionesPop.Text, 1) + ",clv_politica=" + obtieneNulo(clavePolitica, 1) + "," +
                "Estatus='" + estatus + "',Importe=" + obtieneNulo(lblImporte.Text, 0) + ",concepto=" + obtieneNulo(txtPPP.Text, 1) + ",porcentaje_pp=" + obtieneNulo(radDcto.Text, 0) + ",importe_pp=" + obtieneNulo(radImpDcto.Text, 0) + ",monto_pagar=" + obtieneNulo(lblMontoPagar.Text, 0) + ",id_nota_credito='" + obtieneNulo(txtNota.Text, 0) + "'" +
                " where Factura='" + factura + "' and id_cliprov=" + idCliprov + " and id_taller=" + idTaller + " and id_empresa=" + idEmpresa + " and no_orden=" + noOrden + " and renglon=" + renglon;
            datosFacturas.actualizaFacturas(sql, 0);
            object[] actualizado = datosFacturas.retorno;
            if ((bool)actualizado[0])
            {
                if ((bool)actualizado[1])
                {
                    lblErrorPopup.Text = "Datos actualizados exitosamente.";
                    if (txtFechaPagoPop.Text != "")
                        ocultarValores();
                    RadGrid1.Rebind();
                }
                else
                    lblErrorPopup.Text = "Ocurrio un error inesperado en la actualización: " + actualizado[1].ToString();
            }
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
        catch (Exception) { }
    }

    protected void ddlPoliticaPagoPop_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaFechaProg();
    }

    protected void ddlFormaPagopop_SelectedIndexChanged(object sender, EventArgs e)
    {
        activaCampos();
    }

    private void activaCampos()
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

    private void activaCamposNota()
    {
        if (chkNota.Checked)
            txtPPP.Enabled = radDcto.Enabled = radImpDcto.Enabled =txtNota.Enabled = true;
        else
        {
            txtPPP.Enabled = radDcto.Enabled = radImpDcto.Enabled = txtNota.Enabled = false;
            txtPPP.Text = "P.P.P";
            txtNota.Text = "";
            radImpDcto.Value = radDcto.Value = 0;
        }
    }

    protected void chkNota_CheckedChanged(object sender, EventArgs e)
    {
        activaCamposNota();
    }

    private void ocultarValores()
    {
        lnkFechaPagoPop.Visible = lnkFechaProgPagopop.Visible = lnkFecharevisionPop.Visible = lnkGuardarPopup.Visible = false;
        ddlPoliticaPagoPop.Enabled = ddlFormaPagopop.Enabled = ddlBanco.Enabled = txtReferenciaPagPop.Enabled = txtObsevacionesPop.Enabled = txtPPP.Enabled = radImpDcto.Enabled = chkNota.Enabled = radDcto.Enabled = txtNota.Enabled = false;
        lnkAgregarAnticipo.Visible = pnlAnticipos.Visible = false;
    }
    private void activaValores()
    {
        lnkFechaPagoPop.Visible = lnkFechaProgPagopop.Visible = lnkFecharevisionPop.Visible = lnkGuardarPopup.Visible = true;
        ddlPoliticaPagoPop.Enabled = ddlFormaPagopop.Enabled = ddlBanco.Enabled = txtReferenciaPagPop.Enabled = txtObsevacionesPop.Enabled = txtPPP.Enabled = radImpDcto.Enabled = chkNota.Enabled = radDcto.Enabled = txtNota.Enabled = true;
        lnkAgregarAnticipo.Visible = true;
    }

    protected void lnkAgregarAnticipo_Click(object sender, EventArgs e)
    {
        pnlAnticipos.Visible = true;
        txtFechaPagoAnticipo.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
        ddlFormaPagoAnticipo.SelectedValue = "E";
        ddlBancoAnticipo.Enabled = txtReferenciaAnticipo.Enabled = false;
        ddlBancoAnticipo.SelectedValue = "";
        txtReferenciaAnticipo.Text = "";
        radMontoAnticipo.Text = "0";
        lblErrorPopup.Text = "";
    }

    protected void ddlFormaPagoAnticipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFormaPagoAnticipo.SelectedValue != "E")
            ddlBancoAnticipo.Enabled = txtReferenciaAnticipo.Enabled = true;
        else
        {
            ddlBancoAnticipo.Enabled = txtReferenciaAnticipo.Enabled = false;
            ddlBancoAnticipo.SelectedValue = "";
            txtReferenciaAnticipo.Text = "";
        }
    }

    protected void lnkAgregaPagoAnticipo_Click(object sender, EventArgs e)
    {
        lblErrorPopup.Text = "";
        string fechaPago = txtFechaPagoAnticipo.Text;
        string formaPago = ddlFormaPagoAnticipo.SelectedValue;
        string referencia = txtReferenciaAnticipo.Text;
        string banco = ddlBancoAnticipo.SelectedValue;
        string monto = radMontoAnticipo.Value.ToString();
        if (Convert.ToDecimal(monto) != 0)
        {
            Facturas facturas = new Facturas();
            facturas.renglon = Convert.ToInt32(lblRenglonFactura.Text);
            facturas.folio = Convert.ToInt32(lblOrden.Text);
            facturas.factura = lblFactura.Text;
            facturas.tipoCuenta = "CC";
            facturas.obtieneUltimoResto();
            decimal ultimoRetante = 0;
            decimal restante = 0;
            decimal montoPagar = Convert.ToDecimal(lblMontoPagar.Text);
            if (Convert.ToBoolean(facturas.retorno[0]))
            {
                ultimoRetante = Convert.ToDecimal(facturas.retorno[1]);
                if (ultimoRetante != 0)
                    restante = ultimoRetante - Convert.ToDecimal(monto);
                else
                    restante = montoPagar - Convert.ToDecimal(monto);
            }

            //decimal ultimoResto
            
           
            string sql = "insert into detallefacturas values(" + lblRenglonFactura.Text + ",'" + lblFactura.Text + "',(select isnull((select top 1 pago from detallefacturas where renglon=" + lblRenglonFactura.Text + " and factura='" + lblFactura.Text + "' and tipocuenta='CC' and folio =" + lblOrden.Text + " order by pago desc),0))+1,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'," + obtieneNulo(ddlFormaPagoAnticipo.SelectedValue, 1) + "," + obtieneNulo(txtReferenciaAnticipo.Text, 1) + "," + obtieneNulo(ddlBancoAnticipo.SelectedValue, 1) + ",'','CC'," + lblOrden.Text + "," + montoPagar + "," + monto + "," + restante + ")";
            facturas.agregaPagoAnticipado(sql);
            object[] retorno = facturas.retorno;
            if (Convert.ToBoolean(retorno[0]))
            {
                if (restante == 0)
                {
                    string noOrden = lblOrden.Text;
                    string idEmpresa = lblIdEmpresa.Text;
                    string idTaller = lblIdTaller.Text;
                    string factura = lblFactura.Text;
                    string idCliprov = lblIdCliprov.Text;
                    string renglon = lblRenglonFactura.Text;
                    txtFechaPagoPop.Text = txtFechaPagoAnticipo.Text;
                    ddlFormaPagopop.SelectedValue = ddlFormaPagoAnticipo.SelectedValue;
                    txtReferenciaPagPop.Text = txtReferenciaAnticipo.Text;
                    ddlBanco.SelectedValue = ddlBancoAnticipo.SelectedValue;
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

                    sql = "update facturas set FechaRevision=" + obtieneNulo(txtFecharevisionPop.Text, 1) + ",FechaProgPago=" + obtieneNulo(txtFechaProgPagopop.Text, 1) + ",FechaPago=" + obtieneNulo(txtFechaPagoPop.Text, 1) + ", " +
                        "FormaPago=" + obtieneNulo(ddlFormaPagopop.SelectedValue, 1) + ",ReferenciaPago=" + obtieneNulo(txtReferenciaPagPop.Text, 1) + ",clvBanco=" + obtieneNulo(ddlBanco.SelectedValue, 1) + ",Observaciones=" + obtieneNulo(txtObsevacionesPop.Text, 1) + ",clv_politica=" + obtieneNulo(clavePolitica, 1) + "," +
                        "Estatus='" + estatus + "',Importe=" + obtieneNulo(lblImporte.Text, 0) + ",concepto=" + obtieneNulo(txtPPP.Text, 1) + ",porcentaje_pp=" + obtieneNulo(radDcto.Text, 0) + ",importe_pp=" + obtieneNulo(radImpDcto.Text, 0) + ",monto_pagar=" + obtieneNulo(lblMontoPagar.Text, 0) + ",id_nota_credito='" + obtieneNulo(txtNota.Text, 0) + "'" +
                        " where Factura='" + factura + "' and id_cliprov=" + idCliprov + " and id_taller=" + idTaller + " and id_empresa=" + idEmpresa + " and no_orden=" + noOrden + " and renglon=" + renglon;
                    datosFacturas.actualizaFacturas(sql, 0);
                    object[] actualizado = datosFacturas.retorno;
                    if ((bool)actualizado[0])
                    {
                        if ((bool)actualizado[1])
                        {
                            lblErrorPopup.Text = "Datos actualizados exitosamente.";
                            if (txtFechaPagoPop.Text != "")
                                ocultarValores();
                            RadGrid1.Rebind();
                        }
                        else
                            lblErrorPopup.Text = "Ocurrio un error inesperado en la actualización: " + actualizado[1].ToString();
                    }
                }
                txtFechaPagoAnticipo.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                ddlFormaPagoAnticipo.SelectedValue = "E";
                ddlBancoAnticipo.Enabled = txtReferenciaAnticipo.Enabled = false;
                ddlBancoAnticipo.SelectedValue = "";
                txtReferenciaAnticipo.Text = "";
                radMontoAnticipo.Text = "0";
                radMontoAnticipo.MaxValue = Convert.ToDouble(restante);
                RadGrid4.DataBind();
                lnkAgregarAnticipo.Visible = pnlAnticipos.Visible = false;
               
            }
            else
                lblErrorPopup.Text = "Error: " + retorno[1].ToString();
        }
        else
            lblErrorPopup.Text = "Debe indicar un monto para el anticipo a realizar";
    }

    protected void lnkEstatus_Click(object sender, EventArgs e)
    {
        LinkButton lnkEstat = (LinkButton)sender;
        lblEstatus.Text = lnkEstat.CommandArgument;        
        RadGrid1.Rebind();
    }

    private DataTable generaInfo()
    {
        string estatus = lblEstatus.Text;
        string condicion = "";
        string qry = "select * from (select e.encreferencia as Factura,r.renombre,f.no_orden,f.id_empresa,f.id_taller,e.enctotal as monto_pagar,e.encestatus,f.estatus,case e.encestatus when 'P' then 'En Captura' when 'T' then 'Timbrada' when 'C' then 'Cancelada' else 'No Indicado' end as estatusFacturacion,f.folio,f.renglon,r.idrecep,f.FechaRevision,f.FechaProgPago,f.FechaPago from enccfd e left join facturas f on f.idcfd=e.idcfd left join receptores r on r.idrecep=e.idrecep where f.id_cliprov=r.idrecep and f.tipocuenta='CC' and e.encestatus='T' AND f.estatus='PEN' AND not f.fechaprogpago is null union all select e.encreferencia as Factura,r.renombre,f.no_orden,f.id_empresa,f.id_taller,e.enctotal as monto_pagar,e.encestatus,f.estatus,case e.encestatus when 'P' then 'En Captura' when 'T' then 'Timbrada' when 'C' then 'Cancelada' else 'No Indicado' end as estatusFacturacion,f.folio,f.renglon,r.idrecep,f.FechaRevision,f.FechaProgPago,f.FechaPago from enccfd e left join facturas f on f.idcfd=e.idcfd left join receptores r on r.idrecep=e.idrecep where f.id_cliprov=r.idrecep and f.tipocuenta='CC' and ((e.encestatus='T' and f.estatus='PAG') OR (e.encestatus='T' and f.estatus='PEN' and not f.fechapago is null)) union all select e.encreferencia as Factura,r.renombre,f.no_orden,f.id_empresa,f.id_taller,e.enctotal as monto_pagar,e.encestatus,f.estatus,case e.encestatus when 'P' then 'En Captura' when 'T' then 'Timbrada' when 'C' then 'Cancelada' else 'No Indicado' end as estatusFacturacion,f.folio,f.renglon,r.idrecep,f.FechaRevision,f.FechaProgPago,f.FechaPago from enccfd e left join facturas f on f.idcfd=e.idcfd left join receptores r on r.idrecep=e.idrecep where f.id_cliprov=r.idrecep and f.tipocuenta='CC' and e.encestatus='T' AND f.estatus='PEN' AND not f.fechaprogpago is null union all select e.encreferencia as Factura,r.renombre,f.no_orden,f.id_empresa,f.id_taller,e.enctotal as monto_pagar,e.encestatus,f.estatus,case e.encestatus when 'P' then 'En Captura' when 'T' then 'Timbrada' when 'C' then 'Cancelada' else 'No Indicado' end as estatusFacturacion,f.folio,f.renglon,r.idrecep,f.FechaRevision,f.FechaProgPago,f.FechaPago from enccfd e left join facturas f on f.idcfd=e.idcfd left join receptores r on r.idrecep=e.idrecep where f.id_cliprov=r.idrecep and f.tipocuenta='CC' and e.encestatus='T' AND f.estatus='PEN' AND not f.fechaprogpago is null AND DATEDIFF(d,f.fechaprogpago,'2017-05-11')<0 union all select e.encreferencia as Factura,r.renombre,f.no_orden,f.id_empresa,f.id_taller,e.enctotal as monto,e.encestatus,f.estatus,case e.encestatus when 'P' then 'En Captura' when 'T' then 'Timbrada' when 'C' then 'Cancelada' else 'No Indicado' end as estatusFacturacion,f.folio,f.renglon,r.idrecep,f.FechaRevision,f.FechaProgPago,f.FechaPago from enccfd e left join facturas f on f.idcfd=e.idcfd left join receptores r on r.idrecep=e.idrecep where f.id_cliprov=r.idrecep and f.tipocuenta='CC' and ((e.encestatus='C' and f.estatus='CAN') OR (e.encestatus='C' or f.estatus='CAN'))) as t ";
        switch (estatus)
        {
            case "CAN":
                condicion = " where t.Estatus = 'CAN' order by t.idrecep desc";
                break;
            case "REV":
                condicion = " where t.Estatus = 'REV' order by t.idrecep desc";
                break;
            case "PEN":
                condicion = " where t.Estatus = 'PEN' order by t.idrecep desc";
                break;
            case "PAG":
                condicion = " where t.Estatus = 'PAG' order by t.idrecep desc";
                break;
            default:
                condicion = " order by t.idrecep desc";
                break;
        }
        qry = qry + condicion;
        Facturas datosFacturas = new Facturas();
        DataTable dt = new DataTable();
        try
        {
            /*
            dt.Columns.Add("factura");
            dt.Columns.Add("FechaRevision");
            dt.Columns.Add("FechaProgPago");
            dt.Columns.Add("FechaPago");
            dt.Columns.Add("id_cliprov");
            dt.Columns.Add("folio");
            dt.Columns.Add("estatus");
            dt.Columns.Add("id_empresa");
            dt.Columns.Add("id_taller");
            dt.Columns.Add("no_orden");
            dt.Columns.Add("clv_politica");            
            dt.Columns.Add("monto_pagar");
            dt.Columns.Add("estatus_factura");
            dt.Columns.Add("idcfd");
            dt.Columns.Add("estatusFacturacion");
            dt.Columns.Add("renglon");
            dt.Columns.Add("encestatus");
            dt.Columns.Add("esTicket");
            dt.Columns.Add("razon_social");
            dt.Columns.Add("taller");
            dt.Columns.Add("siniestro");
            dt.Columns.Add("poliza");
            */

            object[] info = datosFacturas.dataSet(qry);
            if (Convert.ToBoolean(info[0]))
            {
                DataSet datos = (DataSet)info[1];
                dt = datos.Tables[0];
                /*
                int REGISTROS = datos.Tables[0].Rows.Count;
                for (int i = 0; i < REGISTROS; i++)
                {
                    DataRow f = datos.Tables[0].Rows[i];
                    string nomTaller = obtieneNombreTaller(f[8].ToString());                    
                    string poliza = "";
                    string siniestro = "";

                    DataRow row = dt.NewRow();
                    row["factura"] = f[0].ToString();
                    try { row["FechaRevision"] = Convert.ToDateTime(f[1].ToString()).ToString("yyyy-MM-dd"); } catch (Exception) { row["FechaRevision"] = ""; }
                    try { row["FechaProgPago"] = Convert.ToDateTime(f[2].ToString()).ToString("yyyy-MM-dd"); } catch (Exception) { row["FechaProgPago"] = ""; }
                    try { row["FechaPago"] = Convert.ToDateTime(f[3].ToString()).ToString("yyyy-MM-dd"); } catch (Exception) { row["FechaPago"] = ""; }
                    row["id_cliprov"] = f[4].ToString();
                    row["folio"] = f[5].ToString();
                    row["estatus"] = f[6].ToString();
                    row["id_empresa"] = f[7].ToString();
                    row["id_taller"] = f[8].ToString();
                    row["no_orden"] = f[9].ToString();
                    row["clv_politica"] = f[10].ToString();
                    try { row["monto_pagar"] = Convert.ToDecimal(f[11].ToString()).ToString("N2"); } catch (Exception) { row["monto_pagar"] = "0.00"; }
                    row["estatus_factura"] = f[12].ToString();
                    row["idcfd"] = f[13].ToString();
                    row["estatusFacturacion"] = f[14].ToString();
                    row["renglon"] = f[15].ToString();
                    row["encestatus"] = f[16].ToString();
                    bool esTicket = Convert.ToBoolean(f[17]);
                    if (esTicket)
                    {
                        nomTaller = obtieneNombrePunto(f[8].ToString());
                    }
                    else {
                        string[] valores = obtieneSieniestroPoliza(f[9].ToString(), f[7].ToString(), f[8].ToString());
                        poliza = valores[1];
                        siniestro = valores[0];
                    }
                    row["esTicket"] = f[17].ToString();
                    row["razon_social"] = f[18].ToString();
                    row["taller"] = nomTaller;
                    row["siniestro"] = siniestro;
                    row["poliza"] = poliza;
                    dt.Rows.Add(row);
                }
                */                
            }
        }
        catch (Exception ex) { }
        return dt;

    }

    private string obtieneNombrePunto(string punto)
    {
        string sql = "select nombre_pv from punto_venta where id_punto =" + punto;
        object[] ejecutado = ejecuta.scalarToString(sql);
        if ((bool)ejecutado[0])
            return ejecutado[1].ToString();
        else
            return "";
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        (sender as RadGrid).DataSource = generaInfo();
        cargaInfo();
    }
}