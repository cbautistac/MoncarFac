using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;

public partial class RegistroPinturas : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();
    decimal importeTotal = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargaDatosOrden();
            cargaDatosPie();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    pnlDatosIni.Visible = false;
                    grdRegistro.Columns[13].Visible = false;//axl grdDetalle.Columns[5].Visible = false;
                    lnkActualiza.Visible = false;
                    lnkFigualacion.Visible = lnkFterminado.Visible = lnkFentregado.Visible = false;
                    txtRecibePintura.ReadOnly = txtEntregaReal.ReadOnly = true;
                    txtTicket.ReadOnly = false;
                    txtTicket.Text = "0";
                    txtHterminado.Enabled = txtHentregado.Enabled = false;
                }
            }
            else
                Response.Redirect("BienvenidaOrdenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
        }
    }

    private string obtieneEstatus()
    {
        string estatus = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            object[] infoEstatus = recepciones.obtieneEstatusOrden(sesiones[2].ToString(), sesiones[3].ToString(), sesiones[4].ToString());
            if (Convert.ToBoolean(infoEstatus[0]))
                estatus = Convert.ToString(infoEstatus[1]);
            else
                estatus = "";
        }
        catch (Exception) { estatus = ""; }
        return estatus;
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[6] { 0, 0, 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);
            sesiones[4] = Convert.ToInt32(Request.QueryString["o"]);
            sesiones[5] = Convert.ToInt32(Request.QueryString["f"]);
        }
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    private void cargaDatosPie()
    {
        int[] sesiones = obtieneSesiones();
        object[] datosOrden = recepciones.obtieneInfoOrdenPie(sesiones[4], sesiones[2], sesiones[3]);
        if (Convert.ToBoolean(datosOrden[0]))
        {
            DataSet ordenDatos = (DataSet)datosOrden[1];
            foreach (DataRow filaOrd in ordenDatos.Tables[0].Rows)
            {
                ddlToOrden.Text = filaOrd[0].ToString();
                ddlClienteOrden.Text = filaOrd[1].ToString();
                ddlTsOrden.Text = filaOrd[2].ToString();
                ddlValOrden.Text = filaOrd[3].ToString();
                ddlTaOrden.Text = filaOrd[4].ToString();
                ddlLocOrden.Text = filaOrd[5].ToString();
                ddlPerfil.Text = filaOrd[13].ToString();
                lblSiniestro.Text = filaOrd[9].ToString();
                lblDeducible.Text = Convert.ToDecimal(filaOrd[10].ToString()).ToString("C2");
                lblTotOrden.Text = Convert.ToDecimal(filaOrd[11].ToString()).ToString("C2");
                try
                {
                    DateTime fechaEntrega = Convert.ToDateTime(filaOrd[14].ToString());
                    if (fechaEntrega.ToString("yyyy-MM-dd") == "1900-01-01")
                        lblEntregaEstimada.Text = "No establecida";
                    else
                        lblEntregaEstimada.Text = filaOrd[14].ToString();
                }
                catch (Exception)
                {
                    lblEntregaEstimada.Text = "No establecida";
                }
                lblPorcDedu.Text = filaOrd[16].ToString() + "%";
            }
        }
    }
    protected void txtFsolicitud_TextChanged(object sender, EventArgs e)
    {
        txtFrecepcion.Text = txtFsolicitud.Text;
    }
    protected void txtHsolicitud_TextChanged(object sender, EventArgs e)
    {
        txtFrecepcion.Text = txtFsolicitud.Text;
        txtHrecepcion.SelectedTime = txtHsolicitud.SelectedTime;
    }
    protected void ddlOperativo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        DatosEmpleados emp = new DatosEmpleados();
        object[] obtieneMonto = emp.obtieneMontoAutorizado(sesiones, ddlOperativo.SelectedValue);
        if (Convert.ToBoolean(obtieneMonto[0]))
        {
            DataSet datos = (DataSet)obtieneMonto[1];
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                string tipo = fila[3].ToString();
                decimal monto = 0;
                if (tipo == "EX")
                    Convert.ToDecimal(fila[2].ToString());
                else
                    monto = 0;
                lblAutorizado.Text = monto.ToString();
                lblMontoAutorizado.Text = monto.ToString("C2");
                lblMonto.Text = monto.ToString();
                break;
            }
        }
    }
    protected void lnkAgregar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string f_solicitud = txtFsolicitud.Text;
        string h_solicitud = txtHsolicitud.SelectedTime.Value.ToString();
        string f_recepcion = txtFrecepcion.Text;
        string h_rececpion = txtHrecepcion.SelectedTime.Value.ToString();
        string operario = ddlOperativo.SelectedValue;
        string recibe = txtRecibe.Text;
        string entrega = txtEntregaMuestra.Text;
        string observacion = txtObservaciones.Text;

        try
        {
            DateTime solicitud, recepcion;
            solicitud = Convert.ToDateTime(f_solicitud + " " + h_solicitud);
            recepcion = Convert.ToDateTime(f_recepcion + " " + h_rececpion);
            if (recepcion < solicitud)
                lblError.Text = "La fecha de recepción no puede ser menor a la fecha de solicitud";
            else
            {
                int[] sesiones = obtieneSesiones();
                DatosOrdenes regPint = new DatosOrdenes();
                if (operario == "")
                {
                    operario = "0";
                    if (lblTipoOrden.Text == "1")
                    {
                        operario = "-1";
                    }
                }
                if (lblTipoOrden.Text == "1")
                {
                    operario = "-1";
                }
                object[] agregado = regPint.agregaRegistroPintura(sesiones, solicitud, recepcion, operario, recibe.ToUpper(), entrega.ToUpper(), observacion, txtDetalle.Text);
                if (Convert.ToBoolean(agregado[0]))
                {
                    txtFsolicitud.Text = txtFrecepcion.Text = txtRecibe.Text = txtEntregaMuestra.Text = txtObservaciones.Text = txtDetalle.Text = "";
                    txtHsolicitud.Clear();
                    txtHrecepcion.Clear();
                    try { ddlOperativo.SelectedIndex = 0; }
                    catch (Exception) { ddlOperativo.SelectedIndex = -1; }
                    grdRegistro.SelectedIndex = -1;
                    grdRegistro.DataBind();
                }
                else
                    lblError.Text = "Error: " + Convert.ToString(agregado[1]);
            }
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }
    protected void lnkSeleccionar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] argumentos = btn.CommandArgument.ToString().Split(new char[] { ';' });
        lblOrdenPintura.Text = argumentos[2];
        lblEstatusOrden.Text = argumentos[3];
        lblIdOrden.Text = argumentos[1];
        lblanoOrdne.Text = argumentos[0];
        lblFecharece.Text = argumentos[4];
        SqlDsEncDet.SelectParameters["ticket"].DefaultValue = argumentos[5];
        pnlRegistros.Visible = pnlDatosIni.Visible = false;
        pnlDatosSeguimiento.Visible = pnlDetalle.Visible = true;
        string estatus = obtieneEstatus();
        if (argumentos[3] == "T" || argumentos[3] == "C" || estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S")
        {
            lnkActualiza.Visible = false;
            lnkFigualacion.Visible = lnkFterminado.Visible = lnkFentregado.Visible = false;
            txtRecibePintura.ReadOnly = txtEntregaReal.ReadOnly = true;
            txtTicket.ReadOnly = true;
            txtHterminado.Enabled = txtHentregado.Enabled = false;
        }
        else
        {
            lnkActualiza.Visible = true;
            lnkFigualacion.Visible = lnkFterminado.Visible = lnkFentregado.Visible = true;
            txtRecibePintura.ReadOnly = txtEntregaReal.ReadOnly = false;
            txtTicket.ReadOnly = false;
            txtHterminado.Enabled = txtHentregado.Enabled = true;
        }

        DatosOrdenes regPint = new DatosOrdenes();
        object[] datos = regPint.obtieneInfoSolicitudPintura(argumentos[0], argumentos[1]);
        if (Convert.ToBoolean(datos[0]))
        {
            DataSet info = (DataSet)datos[1];
            foreach (DataRow fila in info.Tables[0].Rows)
            {
                txtFigualacion.Text = fila[0].ToString();
                txtFterminado.Text = fila[1].ToString();
                try { txtHterminado.SelectedDate = Convert.ToDateTime(fila[2]); } catch (Exception) { txtHterminado.Clear(); }
                txtFentregado.Text = fila[3].ToString();
                try { txtHentregado.SelectedDate = Convert.ToDateTime(fila[4]); } catch (Exception) { txtHentregado.Clear(); }
                txtRecibePintura.Text = fila[5].ToString();
                txtEntregaReal.Text = fila[6].ToString();
                ddlOperativoMod.SelectedValue = fila[7].ToString();
                txtTicket.Text = fila[8].ToString();
            }
        }

        if (ddlOperativoMod.SelectedValue == "" || ddlOperativoMod.SelectedValue == "0")
            ddlOperativoMod.Enabled = true;
        else
            ddlOperativoMod.Enabled = false;


    }
    protected void lnkCancelar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] argumentos = btn.CommandArgument.ToString().Split(new char[] { ';' });
        DatosOrdenes regPint = new DatosOrdenes();
        object[] cancelado = regPint.actualizaEstatusOrdenPintura(argumentos, "C");
        if (Convert.ToBoolean(cancelado[0]))
        {
            grdRegistro.SelectedIndex = -1;
            grdRegistro.DataBind();
        }
        else
            lblError.Text = "Error: " + Convert.ToString(cancelado[1]);
    }
    protected void lnkSalir_Click(object sender, EventArgs e)
    {
        lblOrdenPintura.Text = lblEstatusOrden.Text = "";
        pnlRegistros.Visible = true;
        pnlDatosSeguimiento.Visible = pnlDetalle.Visible = false;
        grdRegistro.SelectedIndex = -1;
        grdRegistro.DataBind();
        lblError.Text = "";
        string estatus = obtieneEstatus();
        if (estatus != "")
        {
            if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                pnlDatosIni.Visible = false;
            else
                pnlDatosIni.Visible = true;
        }


    }
    protected void lnkActualiza_Click(object sender, EventArgs e)
    {
        try
        {
            fechas.fecha = fechas.obtieneFechaLocal();
            fechas.tipoFormato = 4;
            string fechaRetorno = fechas.obtieneFechaConFormato();

            DateTime recepcion = Convert.ToDateTime(lblFecharece.Text + " 00:00:00");
            DateTime igualacion = Convert.ToDateTime(txtFigualacion.Text + " 00:00:00");
            DateTime termiando = Convert.ToDateTime(txtFterminado.Text + " " + txtHterminado.SelectedTime.Value.ToString());
            DateTime entregado = Convert.ToDateTime(txtFentregado.Text + " " + txtHentregado.SelectedTime.Value.ToString());
            DateTime actual = Convert.ToDateTime(fechaRetorno + " 00:00:00");

            if (igualacion < recepcion || termiando < recepcion || entregado < recepcion)
                lblError.Text = "La fecha igualación, terminado y entregado no pueden ser menor a la fecha de recepcion " + recepcion.ToString("dd/MM/yyyy");
            else
            {
                if (termiando < igualacion)
                    lblError.Text = "La fecha de terminado no puede ser menor a la de igualación";
                else if (entregado < termiando)
                    lblError.Text = "La fecha de entregado no puede ser menor a la de terminado";
                else if (entregado < igualacion)
                    lblError.Text = "La fecha de entregado no puede ser menor a la de igualación";
                else
                {
                    TimeSpan ts = actual - igualacion;
                    int diasIgualacion = ts.Days;
                    DatosOrdenes regPint = new DatosOrdenes();
                    string operario = "";
                    if (ddlOperativoMod.SelectedValue == "")
                        operario = "0";
                    else
                        operario = ddlOperativoMod.SelectedValue;

                    if (operario != "0")
                    {
                        if (txtTicket.Text == "" || txtTicket.Text == "0")
                            lblError.Text = "Debe indicar un ticket";
                        else
                        {
                            object[] actualiza = regPint.actualizaOrdenPintura(lblanoOrdne.Text, lblIdOrden.Text, igualacion, termiando, entregado, diasIgualacion, txtRecibePintura.Text, txtEntregaReal.Text, operario, txtTicket.Text);
                            if (Convert.ToBoolean(actualiza[0]))
                            {
                                lnkActualiza.Visible = false;
                                lnkFigualacion.Visible = lnkFterminado.Visible = lnkFentregado.Visible = false;
                                txtRecibePintura.ReadOnly = txtEntregaReal.ReadOnly = true;
                                txtTicket.ReadOnly = true;
                                txtHterminado.Enabled = txtHentregado.Enabled = false;
                            }
                            else
                                lblError.Text = "Error: " + Convert.ToString(actualiza[1]);
                        }
                    }
                    else
                        lblError.Text = "Debe indicar el operario";
                }
            }
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    protected void grdRegistro_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string estatus = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
            LinkButton btn = e.Row.FindControl("lnkCancelar") as LinkButton;
            if (estatus == "T" || estatus == "C")
                btn.Visible = false;
            else
                btn.Visible = true;
        }
    }

    protected void grdDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            importeTotal = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal importes = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "importe").ToString());
            importeTotal = importeTotal + importes;
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            decimal autorizado = 0;

            if (lblAutorizado.Text == "")
                autorizado = 0;
            else
                autorizado = Convert.ToDecimal(lblAutorizado.Text);
            Label lblAuto = e.Row.FindControl("lblAuto") as Label;
            Label lblTotal = e.Row.FindControl("lblTotal") as Label;

            lblTotal.Text = importeTotal.ToString("C2");
            if (autorizado <= 0)
            {
                lblAuto.Visible = false;
                e.Row.Cells[4].CssClass = "";
            }
            else
            {
                lblAuto.Visible = true;
                lblAuto.Text = "Monto Autorizado: " + autorizado.ToString("C2");
                if (importeTotal < autorizado)
                    e.Row.Cells[4].CssClass = "alert-success";
                else if (importeTotal > autorizado)
                    e.Row.Cells[4].CssClass = "alert-danger";
                else if (importeTotal == autorizado)
                    e.Row.Cells[4].CssClass = "alert-warning";
                else
                    e.Row.Cells[4].CssClass = "";
            }
        }
    }

    protected void txtHsolicitud_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        txtHrecepcion.SelectedTime = txtHsolicitud.SelectedTime;
    }

    protected void ddlOperativoMod_SelectedIndexChanged(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        DatosEmpleados emp = new DatosEmpleados();
        object[] obtieneMonto = emp.obtieneMontoAutorizado(sesiones, ddlOperativoMod.SelectedValue);
        if (Convert.ToBoolean(obtieneMonto[0]))
        {
            DataSet datos = (DataSet)obtieneMonto[1];
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                string tipo = fila[3].ToString();
                decimal monto = 0;
                if (tipo == "EX")
                    Convert.ToDecimal(fila[2].ToString());
                else
                    monto = 0;
                lblAutorizado.Text = monto.ToString();
                lblMontoAutorizado.Text = monto.ToString("C2");
                lblMonto.Text = monto.ToString();
                break;
            }
        }
    }
    private void cargaDatosOrden()
    {
        int[] sesiones = obtieneSesiones();
        object[] ordenTransito = recepciones.esOrdenTransito(sesiones[2], sesiones[3], sesiones[4]);
        if (Convert.ToInt32(ordenTransito[1]) == 1)
        {
            ddlOperativo.DataSourceID = "SqlDataSourceTransito";
            ddlOperativo.DataBind();
            lblTipoOrden.Text = "1";
        }
    }
}