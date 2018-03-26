using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ComparativoFacturasCC : System.Web.UI.Page
{
    string id;
    E_Utilities.Fechas fechas = new E_Utilities.Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lbFecha.Text = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
            RadGridCC.Rebind();
        }
    }

    private DataTable generaInfo()
    {
        string fechaActual = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
        string qry = "";
        DataTable dt = new DataTable();
        Facturas datosFacturas = new Facturas();

        qry = "declare @fechaActual date set @fechaActual = '" + fechaActual + "' "+
"Select t.idrecep as id_cliprov,t.rerfc,t.renombre as razon_social,(select count(*) from facturas f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and e.encestatus = 'T' AND f.estatus = 'PEN' AND not f.fechaprogpago is null) as fPen,(select isnull(SUM(e.enctotal), 0) from facturas f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and e.encestatus = 'T' AND f.estatus = 'PEN' AND not f.fechaprogpago is null) as mPen,(select count(*) from facturas_f f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and((e.encestatus = 'T' and f.estatus = 'PAG') OR(e.encestatus = 'T' and f.estatus = 'PEN' and not f.fechapago is null))) as fPag,(select isnull(SUM(e.enctotal), 0) from facturas_f f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and((e.encestatus = 'T' and f.estatus = 'PAG') OR(e.encestatus = 'T' and f.estatus = 'PEN' and not f.fechapago is null))) as mPag,(select count(*) from facturas_f f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and e.encestatus = 'T' AND f.estatus = 'PEN' AND not f.fechaprogpago is null) as fPro,(select isnull(SUM(e.enctotal), 0) from facturas_f f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and e.encestatus = 'T' AND f.estatus = 'PEN' AND not f.fechaprogpago is null) as mPro,(select count(*) from facturas_f f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and((e.encestatus = 'C' and f.estatus = 'CAN') OR(e.encestatus = 'C' or f.estatus = 'CAN'))) as fCan,(select isnull(SUM(e.enctotal), 0) from facturas_f f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and((e.encestatus = 'C' and f.estatus = 'CAN') OR(e.encestatus = 'C' or f.estatus = 'CAN'))) as mCan,(select count(*) from facturas_f f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and e.encestatus = 'T' AND f.estatus = 'PEN' AND not f.fechaprogpago is null AND DATEDIFF(d, f.fechaprogpago, @fechaActual)< 0) as fVen,(select isnull(SUM(e.enctotal), 0) from facturas_f f left join enccfd_f e on e.idcfd = f.idcfd and e.idrecep = f.id_cliprov where f.id_cliprov = t.idrecep and f.tipocuenta = 'CC' and e.encestatus = 'T' AND f.estatus = 'PEN' AND not f.fechaprogpago is null AND DATEDIFF(d, f.fechaprogpago, @fechaActual)< 0) as mVen "+
"from(select e.idrecep, r.rerfc, r.renombre from enccfd_f e left join receptores_f r on r.idrecep = e.idrecep group by e.idrecep, r.rerfc, r.renombre) as t order by t.idrecep";
        object[] info = datosFacturas.dataSet(qry);
        if (Convert.ToBoolean(info[0]))
        {
            DataSet ds = (DataSet)info[1];
            dt = ds.Tables[0];
        }

        return dt;
    }
    
    protected void RadGridCC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        (sender as RadGrid).DataSource = generaInfo();
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
        lblOrdenF.Text = lblTaller.Text = lblValuacion.Text = lblFrecepcion.Text = lblFentregaEst.Text = lblCliente.Text = lblAseguradora.Text = lblAsegurado.Text = lblVehiculo.Text = lblModelo.Text = lblSiniestro.Text = lblPlacas.Text = lblPoliza.Text = "";
        pnlAnticipos.Visible = false;
        RadGrid4.DataBind();
        try
        {
            Recepciones recepcion = new Recepciones();
            object[] datosOrden = recepcion.obtieneInfoOrdenCC(argumentos[1], argumentos[2], argumentos[3]);
            if (Convert.ToBoolean(datosOrden[0]))
            {
                DataSet infoOrdne = (DataSet)datosOrden[1];
                foreach (DataRow dato in infoOrdne.Tables[0].Rows)
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
                if (Convert.ToBoolean(facturas.retorno[0]))
                {
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
            if (status == "PAG" || status == "CAN")
                ocultarValores();
            string script = "abreWinCtrl()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
        }
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
            txtPPP.Enabled = radDcto.Enabled = radImpDcto.Enabled = txtNota.Enabled = true;
        else
        {
            txtPPP.Enabled = radDcto.Enabled = radImpDcto.Enabled = txtNota.Enabled = false;
            txtPPP.Text = "P.P.P";
            txtNota.Text = "";
            radImpDcto.Value = radDcto.Value = 0;
        }
    }

    protected void RadGridPenCc_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;
            try
            {
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
            catch (Exception ex) { }
        }
    }

    protected void ddlPoliticaPagoPop_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaFechaProg();
    }

    protected void ddlFormaPagopop_SelectedIndexChanged(object sender, EventArgs e)
    {
        activaCampos();
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
        catch (Exception) { }
    }

    protected void radDcto_TextChanged(object sender, EventArgs e)
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

    protected void radImpDcto_TextChanged(object sender, EventArgs e)
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
                }
                else
                    lblErrorPopup.Text = "Ocurrio un error inesperado en la actualización: " + actualizado[1].ToString();
            }
        }
    }

    protected void lnkSalirPop_Click(object sender, EventArgs e)
    {
        RadGridCC.Rebind();
    }

    protected void chkNota_CheckedChanged(object sender, EventArgs e)
    {
        activaCamposNota();
    }
}