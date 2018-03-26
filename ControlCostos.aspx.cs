using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data; 
using Telerik.Web.UI;
using E_Utilities;
using System.Globalization;
using System.IO; 

public partial class ControlCostos : System.Web.UI.Page
{
    CostoUnidad producto = new CostoUnidad();
    Fechas fechas = new Fechas();
    Recepciones recepciones = new Recepciones();
    decimal montoMo, montoPi, montoCc, montoRef, montoExt;
    protected void Page_Load(object sender, EventArgs e)
    {
        CultureInfo newCulture = CultureInfo.CreateSpecificCulture("es-Mx");
        RadGrid1.Culture = newCulture;
        cargaInfo();
        if (!IsPostBack) {
            lblAno.Text = fechas.obtieneFechaLocal().Year.ToString();
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

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;
            var lblFechaIni = r[11].ToString();
            var btnOrden = r[4].ToString();
            var status = r[15].ToString();
            try
            {
                string fechaIni = Convert.ToString(lblFechaIni);
                int noOrden = Convert.ToInt32(btnOrden);
                var btn = e.Item.Controls[6].FindControl("btnOrden") as LinkButton;
                if (Convert.ToString(status) == "A")
                    btn.CssClass = "btn btn-primary textoBold colorBlanco";
                else if (Convert.ToString(status) == "T")
                    btn.CssClass = "btn btn-info textoBold colorBlanco";
                else if (Convert.ToString(status) == "C")
                    btn.CssClass = "btn btn-default textoBold colorBlanco";
                else if (Convert.ToString(status) == "R")
                    btn.CssClass = "btn btn-success textoBold colorBlanco";
                else if (Convert.ToString(status) == "F")
                    btn.CssClass = "btn btn-warning textoBold colorBlanco";
                else if (Convert.ToString(status) == "S")
                    btn.CssClass = "btn btn-danger textoBold colorBlanco";

            }
            catch (Exception ex)
            {

            }
        }
    }

    private void cargaInfo()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0)
            Response.Redirect("Default.aspx");
        try
        {
            SqlDataSource1.SelectCommand = "select orp.id_empresa,em.razon_social as empresa, orp.id_taller,t.nombre_taller as taller, orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion as descripcion,upper(v.color_ext) as color_ext,orp.placas,l.descripcion as localizacion,C.razon_social,orp.fase_orden, so.f_recepcion, orp.no_siniestro,v.modelo,po.descripcion as perfil,orp.status_orden"
                                                    + " from Ordenes_Reparacion orp"
                                                    + " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller"
                                                    + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                                                    + " left join Marcas m on m.id_marca=orp.id_marca"
                                                    + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
                                                    + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                                                    + " left join Localizaciones l on l.id_localizacion=orp.id_localizacion"
                                                    + " left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C'"
                                                    + " left join PerfilesOrdenes po on po.id_perfilOrden=orp.id_perfilOrden "
                                                    + " left join Empresas em on em.id_empresa=orp.id_empresa "
                                                    + " left join Talleres t on t.id_taller=orp.id_taller "
                                                    + " order by orp.id_empresa,orp.id_taller,orp.no_orden desc";
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    protected void btnOrden_Click(object sender, EventArgs e)
    {
         lblError.Text = lblErrorCostos.Text = "";
        LinkButton lknReferencia = (LinkButton)sender;
        string[] argumentos = lknReferencia.CommandArgument.ToString().Split(new char[] { ';' });        
        int orden = Convert.ToInt32(argumentos[0]);
        int taller = Convert.ToInt32(argumentos[1]);
        int empresa = Convert.ToInt32(argumentos[2]);
        lblTall.Text = taller.ToString();
        lblEmp.Text = empresa.ToString();
        lblOrd.Text = orden.ToString();
        lblOrden.Text = orden.ToString();
        lblTaller.Text = taller.ToString();
        lblEmpresa.Text = empresa.ToString();
        lblRenglon.Text = "0";
        lblAcceso.Text = "C";
        lblError.Text = lblErrorCostos.Text = lblErrorPi.Text = lblErrorCc.Text = "";
        //lblTotTotal.Text = (Convert.ToDecimal(lblTotMo.Text) + Convert.ToDecimal(lblTotPint.Text) + Convert.ToDecimal(lblTotCc.Text) + Convert.ToDecimal(lblTotCostoIndirecto.Text)).ToString("F2");
        cargaInfoOrden();
        cargaDatosPie();
        RadTabStrip1.SelectedIndex = 0;
        RadMultiPage1.SelectedIndex = 0;
        string script = "abreWinCtrl()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
        grdPint_NeedDataSource(sender, null);
        grdCajaChica_Carga();
        grdAlmacen_Carga();
    }

    private void cargaInfoOrden()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0)
            Response.Redirect("Default.aspx");
        try
        {
            string argumentos = "Orden: " + lblOrden.Text;
            DatosVehiculos vehiculos = new DatosVehiculos();
            object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculo(Convert.ToInt32(lblOrden.Text), Convert.ToInt32(lblEmpresa.Text), Convert.ToInt32(lblTaller.Text));
            if (Convert.ToBoolean(vehiculo[0]))
            {
                DataSet valores = (DataSet)vehiculo[1];
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    argumentos = argumentos.Trim() + " / " + fila[1].ToString().ToUpper();
                    break;
                }
            }
            lblOrdenSelect.Text = argumentos;
            lblOrdenSele.Text = argumentos;
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    private void cargaDatosPie()
    {
        int empresa = Convert.ToInt32(lblEmpresa.Text);
        int taller = Convert.ToInt32(lblTaller.Text);
        int orden = Convert.ToInt32(lblOrden.Text);
        object[] datosOrden = recepciones.obtieneInfoOrdenPie(orden, empresa, taller);
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
                lblSiniestro.Text = filaOrd[9].ToString();
                lblDeducible.Text = Convert.ToDecimal(filaOrd[10].ToString()).ToString("C2");
                lblTotOrden.Text = Convert.ToDecimal(filaOrd[11].ToString()).ToString("C2");
                ddlPerfil.Text = filaOrd[13].ToString();
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
                lblTotCostoIndirecto.Text = Convert.ToDecimal(filaOrd[18]).ToString("F2");
                radCostoFijo.Value = Convert.ToDouble(filaOrd[18]);
            }
        }
    }

    protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
                montoMo = 0;
            else if (e.Item is GridDataItem)
            {
                DataRowView filas = (DataRowView)e.Item.DataItem;
                DataRow r = filas.Row;

                var monto = r[9].ToString();
                try
                {
                    decimal montos = Convert.ToDecimal(monto.ToString());
                    montoMo = montoMo + montos;
                }
                catch (Exception ex)
                {
                    montoMo = montoMo + 0;
                }
            }
            else if (e.Item is GridFooterItem)
            {
                lblMoTotal.Text = lblTotMo.Text = montoMo.ToString("F2");
                lblTotTotal.Text = (Convert.ToDecimal(lblTotMo.Text) + Convert.ToDecimal(lblTotPint.Text) + Convert.ToDecimal(lblTotCc.Text) + Convert.ToDecimal(lblTotCostoIndirecto.Text) + Convert.ToDecimal(lblTotRef.Text) + Convert.ToDecimal(lblTotExt.Text)).ToString("F2");
            }
        }
        catch (Exception ex)
        {

        }
        finally {

        }
    }

    protected void grdPint_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
                montoPi = 0;
            else if (e.Item is GridDataItem)
            {
                DataRowView filas = (DataRowView)e.Item.DataItem;
                DataRow r = filas.Row;

                var monto = r[8].ToString();
                try
                {
                    decimal montos = Convert.ToDecimal(monto.ToString());
                    montoPi = montoPi + montos;
                }
                catch (Exception ex)
                {
                    montoPi = montoPi + 0;
                }
                string fact = DataBinder.Eval(e.Item.DataItem, "Factura").ToString();
                if (fact.StartsWith("T-"))
                {
                    LinkButton btnBorraRegPint = (LinkButton)e.Item.FindControl("btnBorraRegPint");
                    LinkButton btnRegistroPintura = (LinkButton)e.Item.FindControl("btnRegistroPintura");
                    btnBorraRegPint.Visible = btnRegistroPintura.Visible = false;
                }
            }
            else if (e.Item is GridFooterItem)
            {
                lblPinturaTot.Text = lblTotPint.Text = montoPi.ToString("F2");
                lblTotTotal.Text = (Convert.ToDecimal(lblTotMo.Text) + Convert.ToDecimal(lblTotPint.Text) + Convert.ToDecimal(lblTotCc.Text) + Convert.ToDecimal(lblTotCostoIndirecto.Text) + Convert.ToDecimal(lblTotRef.Text) + Convert.ToDecimal(lblTotExt.Text)).ToString("F2");
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void grdCajaChica_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
                montoCc = 0;
            else if (e.Item is GridDataItem)
            {
                DataRowView filas = (DataRowView)e.Item.DataItem;
                DataRow r = filas.Row;

                var monto = r[8].ToString();
                try
                {
                    decimal montos = Convert.ToDecimal(monto.ToString());
                    montoCc = montoCc + montos;
                }
                catch (Exception ex)
                {
                    montoCc = montoCc + 0;
                }
                string fact = DataBinder.Eval(e.Item.DataItem, "Factura").ToString();
                if (fact.StartsWith("T-"))
                {
                    LinkButton btnBorraCajChi = (LinkButton)e.Item.FindControl("btnBorraCajChi");
                    LinkButton btnRegistroCC = (LinkButton)e.Item.FindControl("btnRegistroCC");
                    btnBorraCajChi.Visible = btnRegistroCC.Visible = false;
                }
            }
            else if (e.Item is GridFooterItem)
            {
                lblRefaccionesTot.Text = lblTotCc.Text = montoCc.ToString("F2");
                lblTotTotal.Text = (Convert.ToDecimal(lblTotMo.Text) + Convert.ToDecimal(lblTotPint.Text) + Convert.ToDecimal(lblTotCc.Text) + Convert.ToDecimal(lblTotCostoIndirecto.Text) + Convert.ToDecimal(lblTotRef.Text) + Convert.ToDecimal(lblTotExt.Text)).ToString("F2");
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void RadGrid5_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
                 montoRef = 0;
            else if (e.Item is GridDataItem)
            {
                DataRowView filas = (DataRowView)e.Item.DataItem;
                DataRow r = filas.Row;
                //correcion Alfredo Gonzalez Davila
                //09 Ago 2017| instruccion orinal|var monto = r[5].ToString();
                //           | conversion cantidad     |  * | conversion costo       || el resultado se convierte a string
                var monto = (( Convert.ToDouble(  r[2] )) * (Convert.ToDouble( r[6]) ) ).ToString();

                try
                {
                    decimal montos = Convert.ToDecimal(monto.ToString());
                    montoRef = montoRef + montos;
                }
                catch (Exception ex)
                {
                    montoRef = montoRef + 0;
                }
            }
            else if (e.Item is GridFooterItem)
            {
                lblTotRefacciones.Text = lblTotRef.Text = montoRef.ToString("F2");
                lblTotTotal.Text = (Convert.ToDecimal(lblTotMo.Text) + Convert.ToDecimal(lblTotPint.Text) + Convert.ToDecimal(lblTotCc.Text) + Convert.ToDecimal(lblTotCostoIndirecto.Text) + Convert.ToDecimal(lblTotRef.Text) + Convert.ToDecimal(lblTotExt.Text)).ToString("F2");

            }
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void RadGrid6_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
                montoExt = 0;
            else if (e.Item is GridDataItem)
            {
                DataRowView filas = (DataRowView)e.Item.DataItem;
                DataRow r = filas.Row;

                var monto = r[8].ToString();
                try
                {
                    decimal montos = Convert.ToDecimal(monto.ToString());
                    montoExt = montoExt + montos;
                }
                catch (Exception ex)
                {
                    montoExt = montoExt + 0;
                }
            }
            else if (e.Item is GridFooterItem)
            {
                lblTotExtras.Text = lblTotExt.Text = montoExt.ToString("F2");
                lblTotTotal.Text = (Convert.ToDecimal(lblTotMo.Text) + Convert.ToDecimal(lblTotPint.Text) + Convert.ToDecimal(lblTotCc.Text) + Convert.ToDecimal(lblTotCostoIndirecto.Text) + Convert.ToDecimal(lblTotRef.Text) + Convert.ToDecimal(lblTotExt.Text)).ToString("F2");

            }
        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void radCatidadPi_TextChanged(object sender, System.EventArgs e)
    {
        if (radCatidadPi.Value != 0)
        {
            if (radCuPi.Value != 0)
            {
                if (radDectoPi.Value != 0)
                    lblImportePi.Text = Convert.ToDecimal((radCatidadPi.Value * radCuPi.Value) - ((radCatidadPi.Value * radCuPi.Value) * (radDectoPi.Value / 100))).ToString("F2");
                else
                    lblImportePi.Text = Convert.ToDecimal((radCatidadPi.Value * radCuPi.Value)).ToString("F2");
            }
            else
                lblImportePi.Text = "0.00";
        }
        else
            lblImportePi.Text = "0.00";
    }

    protected void radCuPi_TextChanged(object sender, System.EventArgs e)
    {
        if (radCatidadPi.Value != 0)
        {
            if (radCuPi.Value != 0)
            {
                if (radDectoPi.Value != 0)
                    lblImportePi.Text = Convert.ToDecimal((radCatidadPi.Value * radCuPi.Value) - ((radCatidadPi.Value * radCuPi.Value) * (radDectoPi.Value / 100))).ToString("F2");
                else
                    lblImportePi.Text = Convert.ToDecimal((radCatidadPi.Value * radCuPi.Value)).ToString("F2");
            }
            else
                lblImportePi.Text = "0.00";
        }
        else
            lblImportePi.Text = "0.00";
    }

    protected void radDesctoPi_TextChanged(object sender, System.EventArgs e)
    {
        if (radCatidadPi.Value != 0)
        {
            if (radCuPi.Value != 0)
            {
                if (radDectoPi.Value != 0)
                    lblImportePi.Text = Convert.ToDecimal((radCatidadPi.Value * radCuPi.Value) - ((radCatidadPi.Value * radCuPi.Value) * (radDectoPi.Value / 100))).ToString("F2");
                else
                    lblImportePi.Text = Convert.ToDecimal((radCatidadPi.Value * radCuPi.Value)).ToString("F2");
            }
            else
                lblImportePi.Text = "0.00";
        }
        else
            lblImportePi.Text = "0.00";
    }

    protected void radCatidadCc_TextChanged(object sender, System.EventArgs e)
    {
        if (radCatidadCc.Value != 0)
        {
            if (radCuCc.Value != 0)
            {
                if (radDectoCc.Value != 0)
                    lblImporteCc.Text = Convert.ToDecimal((radCatidadCc.Value * radCuCc.Value) - ((radCatidadCc.Value * radCuCc.Value) * (radDectoCc.Value / 100))).ToString("F2");
                else
                    lblImporteCc.Text = Convert.ToDecimal((radCatidadCc.Value * radCuCc.Value)).ToString("F2");
            }
            else
                lblImporteCc.Text = "0.00";
        }
        else
            lblImporteCc.Text = "0.00";
    }

    protected void radCuCc_TextChanged(object sender, System.EventArgs e)
    {
        if (radCatidadCc.Value != 0)
        {
            if (radCuCc.Value != 0)
            {
                if (radDectoCc.Value != 0)
                    lblImporteCc.Text = Convert.ToDecimal((radCatidadCc.Value * radCuCc.Value) - ((radCatidadCc.Value * radCuCc.Value) * (radDectoCc.Value / 100))).ToString("F2");
                else
                    lblImporteCc.Text = Convert.ToDecimal((radCatidadCc.Value * radCuCc.Value)).ToString("F2");
            }
            else
                lblImporteCc.Text = "0.00";
        }
        else
            lblImporteCc.Text = "0.00";
    }

    protected void radDesctoCc_TextChanged(object sender, System.EventArgs e)
    {
        if (radCatidadCc.Value != 0)
        {
            if (radCuCc.Value != 0)
            {
                if (radDectoCc.Value != 0)
                    lblImporteCc.Text = Convert.ToDecimal((radCatidadCc.Value * radCuCc.Value) - ((radCatidadCc.Value * radCuCc.Value) * (radDectoCc.Value / 100))).ToString("F2");
                else
                    lblImporteCc.Text = Convert.ToDecimal((radCatidadCc.Value * radCuCc.Value)).ToString("F2");
            }
            else
                lblImporteCc.Text = "0.00";
        }
        else
            lblImporteCc.Text = "0.00";
    }

    protected void lnkAceptarPi_Click(object sender, EventArgs e)
    {
        lblError.Text = lblErrorCostos.Text = lblErrorPi.Text = lblErrorCc.Text = "";
        try {
            if (ddlProv.SelectedValue == "" || ddlProv.Items == null)
                lblErrorPi.Text = "Debe indicar un proveedor";
            else {
                if (txtFactura.Text == "")
                    lblErrorPi.Text = "Debe indicar la factura";
                else {
                    if (txtFechaPi.Text == "")
                        lblErrorPi.Text = "Debe indicar la fecha de la factura";
                    else {
                        DateTime fecha = Convert.ToDateTime(txtFechaPi.Text);
                        DateTime fechaAcual = fechas.obtieneFechaLocal();
                        if (fecha.Date > fechaAcual.Date)
                            lblErrorPi.Text = "La fecha no puede ser mayor a la fecha actual";
                        else {
                            if (txtDescripcionPi.Text == "")
                                lblErrorPi.Text = "Debe indicar la descripcion";
                            else {
                                int[] sesiones = obtieneSesiones();
                                OtrosCostos oc = new OtrosCostos();
                                oc.orden = Convert.ToInt32(lblOrden.Text);
                                oc.taller = Convert.ToInt32(lblTaller.Text);
                                oc.empresa = Convert.ToInt32(lblEmpresa.Text);
                                oc.fecha = Convert.ToDateTime(txtFechaPi.Text);
                                oc.descripcion = txtDescripcionPi.Text;
                                oc.proveedor = Convert.ToDecimal(ddlProv.SelectedValue);
                                oc.cantidad = Convert.ToDouble(radCatidadPi.Value);
                                oc.costo = Convert.ToDouble(radCuPi.Value);
                                oc.importe = Convert.ToDouble(lblImportePi.Text);
                                oc.area = "PI";
                                oc.factura = txtFactura.Text;
                                oc.descuento = Convert.ToDouble(radDectoPi.Value);
                                oc.pago = -1;
                                oc.notaCredito = "";
                                if (lblAcceso.Text == "C")
                                    oc.generaOtroCosto();
                                else {
                                    oc.renglon = Convert.ToInt32(lblRenglon.Text);
                                    oc.actualizaOtroCosto();
                                }
                                object[] generado = oc.retorno;
                                if (Convert.ToBoolean(generado[0]))
                                {
                                    lblRenglon.Text = "0";
                                    lblAcceso.Text = "C";
                                    ddlProv.SelectedIndex = 0;
                                    txtFactura.Text = txtFechaPi.Text = txtDescripcionPi.Text = "";
                                    radCatidadPi.Value = radCuPi.Value = radDectoPi.Value = 0;
                                    lblErrorPi.Text = "Cargo generado";
                                    ddlProv.Enabled = txtFactura.Enabled = true;
                                    lnkFechaPi.Visible = true;
                                    grdPint_NeedDataSource(sender, null);
                                    lblImportePi.Text = "0.00";
                                }
                                else
                                    lblErrorPi.Text = generado[1].ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { lblErrorPi.Text = "Error: " + ex.Message; }

    }

    protected void lnkAceptarCc_Click(object sender, EventArgs e)
    {
        lblError.Text = lblErrorCostos.Text = lblErrorPi.Text = lblErrorCc.Text = "";
        try
        {
            if (radcboProv.SelectedValue == "" || radcboProv.Items == null)
                lblErrorCc.Text = "Debe indicar un proveedor";
            else
            {
                if (txtFacturaCc.Text == "")
                    lblErrorCc.Text = "Debe indicar la factura";
                else
                {
                    if (txtFechaCc.Text == "")
                        lblErrorCc.Text = "Debe indicar la fecha de la factura";
                    else
                    {
                        DateTime fecha = Convert.ToDateTime(txtFechaCc.Text);
                        DateTime fechaAcual = fechas.obtieneFechaLocal();
                        if (fecha.Date > fechaAcual.Date)
                            lblErrorCc.Text = "La fecha no puede ser mayor a la fecha actual";
                        else
                        {
                            if (txtDescripcionCc.Text == "")
                                lblErrorCc.Text = "Debe indicar la descripcion";
                            else
                            {
                                int[] sesiones = obtieneSesiones();
                                OtrosCostos oc = new OtrosCostos();
                                oc.orden = Convert.ToInt32(lblOrden.Text);
                                oc.taller = Convert.ToInt32(lblTaller.Text);
                                oc.empresa = Convert.ToInt32(lblEmpresa.Text);
                                oc.fecha = Convert.ToDateTime(txtFechaCc.Text);
                                oc.descripcion = txtDescripcionCc.Text;
                                oc.proveedor = Convert.ToDecimal(radcboProv.SelectedValue);
                                oc.cantidad = Convert.ToDouble(radCatidadCc.Value);
                                oc.costo = Convert.ToDouble(radCuCc.Value);
                                oc.importe = Convert.ToDouble(lblImporteCc.Text);
                                oc.pago = Convert.ToInt32(rblPago.SelectedValue);
                                oc.area = "CA";
                                oc.factura = txtFacturaCc.Text;
                                oc.descuento = Convert.ToDouble(radDectoCc.Value);
                                oc.notaCredito = "";
                                if (lblAcceso.Text == "C")
                                    oc.generaOtroCosto();
                                else
                                {
                                    oc.renglon = Convert.ToInt32(lblRenglon.Text);
                                    oc.actualizaOtroCosto();
                                }
                                object[] generado = oc.retorno;
                                if (Convert.ToBoolean(generado[0]))
                                {
                                    lblRenglon.Text = "0";
                                    lblAcceso.Text = "C";
                                    radcboProv.SelectedIndex = 0;
                                    txtFacturaCc.Text = txtFechaCc.Text = txtDescripcionCc.Text = "";
                                    radCatidadCc.Value = radCuCc.Value = radDectoCc.Value = 0;
                                    lblErrorCc.Text = "Cargo generado";
                                    radcboProv.Enabled = txtFacturaCc.Enabled = true;
                                    lnkFechaCc.Visible = true;
                                    grdCajaChica_Carga();
                                    lblImporteCc.Text = "0.00";
                                    rblPago.SelectedValue = "-1";

                                }
                                else
                                    lblErrorCc.Text = generado[1].ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { lblErrorCc.Text = "Error: " + ex.Message; }

    }

    protected void lnkAceptarCF_Click(object sender, EventArgs e)
    {
        lblError.Text = lblErrorCostos.Text = lblErrorPi.Text = lblErrorCc.Text = "";
        try
        {
            decimal costo = 0;
            try { costo = Convert.ToDecimal(radCostoFijo.Value); }
            catch (Exception) { costo = 0; }
            if (costo != 0)
            {
                int[] sesiones = obtieneSesiones();
                int orden = Convert.ToInt32(lblOrden.Text);
                object[] actualizado = recepciones.actualizaCostoFijo(sesiones[2], sesiones[3], orden, costo);
                if (Convert.ToBoolean(actualizado[0]))
                {
                    lblTotCostoIndirecto.Text = Convert.ToDecimal(radCostoFijo.Value).ToString("F2");
                    lblTotTotal.Text = (Convert.ToDecimal(lblTotMo.Text) + Convert.ToDecimal(lblTotPint.Text) + Convert.ToDecimal(lblTotCc.Text) + Convert.ToDecimal(lblTotCostoIndirecto.Text) + Convert.ToDecimal(lblTotRef.Text) + Convert.ToDecimal(lblTotExt.Text)).ToString("F2");
                    lblErrorCostos.Text = "Costo Fijo Actualizado";
                    radCostoFijo.Value = 0;
                }
                else
                    lblErrorCostos.Text = "Error: " + actualizado[1].ToString();
            }
            else
                lblErrorCostos.Text = "Debe indicar un costo fijo y/o debe ser mayor a cero";
        }
        catch (Exception ex) { lblErrorCostos.Text = "Error: " + ex.Message; }
    }

    protected void btnRegistroPintura_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        lblError.Text = lblErrorCostos.Text = lblErrorPi.Text = lblErrorCc.Text = "";
        try
        {
            int renglon = Convert.ToInt32(btn.CommandArgument.ToString());
            lblRenglon.Text = renglon.ToString();
            int[] sesiones = obtieneSesiones();
            int orden = Convert.ToInt32(lblOrden.Text);
            OtrosCostos oc = new OtrosCostos();
            oc.orden = Convert.ToInt32(lblOrden.Text);
            oc.taller = Convert.ToInt32(lblTaller.Text);
            oc.empresa = Convert.ToInt32(lblEmpresa.Text);
            oc.renglon = renglon;
            oc.area = "PI";
            oc.obtieneInfoOtroCosto();
            object[] datos = oc.retorno;
            if (Convert.ToBoolean(datos[0])) {
                DataSet info = (DataSet)datos[1];
                if (info.Tables[0].Rows.Count != 0)
                {
                    lblAcceso.Text = "M";
                    foreach (DataRow r in info.Tables[0].Rows)
                    {
                        ddlProv.SelectedValue = r[6].ToString();
                        txtFactura.Text = r[11].ToString();
                        txtFechaPi.Text = r[4].ToString();
                        CalendarExtender1.SelectedDate = Convert.ToDateTime(r[4].ToString());
                        txtDescripcionPi.Text = r[5].ToString();
                        radCatidadPi.Value = Convert.ToDouble(r[7].ToString());
                        radCuPi.Value = Convert.ToDouble(r[8].ToString());
                        radDectoPi.Value = Convert.ToDouble(r[12].ToString());
                        lblImportePi.Text = Convert.ToDecimal(r[9].ToString()).ToString("F2");
                    }
                    ddlProv.Enabled = txtFactura.Enabled = false;
                    lnkFechaPi.Visible = false;
                }
                else
                {
                    lblRenglon.Text = "0";
                    lblAcceso.Text = "C";
                    ddlProv.Enabled = txtFactura.Enabled = true;
                    lnkFechaPi.Visible = true;
                    ddlProv.SelectedIndex = 0;
                    txtFactura.Text = txtFechaPi.Text = txtDescripcionPi.Text = "";
                    radCatidadPi.Value = radCuPi.Value = radDectoPi.Value = 0;
                }
            }
        }
        catch (Exception ex) { lblErrorPi.Text = "Error: " + ex.Message; }
    }

    protected void btnRegistroCC_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        lblError.Text = lblErrorCostos.Text = lblErrorPi.Text = lblErrorCc.Text = "";
        try
        {
            int renglon = Convert.ToInt32(btn.CommandArgument.ToString());
            lblRenglon.Text = renglon.ToString();
            int[] sesiones = obtieneSesiones();
            int orden = Convert.ToInt32(lblOrden.Text);
            OtrosCostos oc = new OtrosCostos();
            oc.orden = Convert.ToInt32(lblOrden.Text);
            oc.taller = Convert.ToInt32(lblTaller.Text);
            oc.empresa = Convert.ToInt32(lblEmpresa.Text);
            oc.renglon = renglon;
            oc.area = "CA";
            oc.obtieneInfoOtroCosto();
            object[] datos = oc.retorno;
            if (Convert.ToBoolean(datos[0]))
            {
                DataSet info = (DataSet)datos[1];
                if (info.Tables[0].Rows.Count != 0)
                {
                    lblAcceso.Text = "M";
                    foreach (DataRow r in info.Tables[0].Rows)
                    {
                        radcboProv.SelectedValue = r[6].ToString();
                        txtFacturaCc.Text = r[11].ToString();
                        txtFechaCc.Text = r[4].ToString();
                        CalendarExtender2.SelectedDate = Convert.ToDateTime(r[4].ToString());
                        txtDescripcionCc.Text = r[5].ToString();
                        radCatidadCc.Value = Convert.ToDouble(r[7].ToString());
                        radCuCc.Value = Convert.ToDouble(r[8].ToString());
                        radDectoCc.Value = Convert.ToDouble(r[12].ToString());
                        lblImporteCc.Text = Convert.ToDecimal(r[9].ToString()).ToString("F2");
                        try { rblPago.SelectedValue = Convert.ToInt32(r[14]).ToString(); } catch (Exception) { rblPago.SelectedValue = "-1"; }

                    }
                    radcboProv.Enabled = txtFacturaCc.Enabled = false;
                    lnkFechaCc.Visible = false;
                }
                else
                {
                    lblRenglon.Text = "0";
                    lblAcceso.Text = "C";
                    radcboProv.Enabled = txtFacturaCc.Enabled = true;
                    lnkFechaCc.Visible = true;
                    radcboProv.SelectedIndex = 0;
                    txtFacturaCc.Text = txtFechaCc.Text = txtDescripcionCc.Text = "";
                    radCatidadCc.Value = radCuCc.Value = radDectoCc.Value = 0;
                    rblPago.SelectedValue = "-1";
                }
            }
        }
        catch (Exception ex) { lblErrorCc.Text = "Error: " + ex.Message; }
    }

    protected void grdPint_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string sqlOtrosCost = "SELECT OC.renglon,oc.Factura,OC.fecha,ltrim(rtrim(C.razon_social)) as razon_social,oc.cantidad,OC.descripcion,oc.Costo_Unitario,oc.Descuento,oc.Importe,c.id_cliprov,oc.id_nota_credito " +
            "FROM otros_costos OC LEFT JOIN CLIPROV C ON c.id_cliprov = Cast(oc.proveedor AS INT) AND c.tipo = 'P' " +
            "WHERE OC.id_empresa=" + lblEmpresa.Text + " and OC.id_taller=" + lblTaller.Text + " and OC.no_orden=" + lblOrden.Text + " and OC.area_de_aplicacion = 'PI' ORDER BY OC.fecha";

        string sqlVentaEnc = "SELECT venta_det.renglon, venta_enc.ticket, venta_enc.fecha_venta, LTRIM(Cliprov.razon_social), venta_det.cantidad, venta_det.descripcion, venta_det.venta_unitaria, venta_det.valor_descuento, venta_det.importe, Cliprov.id_cliprov " +
            "FROM venta_det INNER JOIN Registro_Pinturas AS rp ON venta_det.ticket = rp.ticket " +
            "INNER JOIN venta_enc ON venta_det.ticket = venta_enc.ticket " +
            "INNER JOIN Cliprov ON venta_enc.id_prov = Cliprov.id_cliprov AND Cliprov.tipo = 'P' " +
            "WHERE(rp.id_empresa=" + lblEmpresa.Text + ") AND(rp.id_taller=" + lblTaller.Text + ") AND(rp.no_orden=" + lblOrden.Text + ") AND(venta_enc.Area_Aplicacion = 'Pn') ORDER BY venta_enc.fecha_venta";

        DataTable dt = inicializaTb(sqlOtrosCost, sqlVentaEnc);
        grdPint.DataSource = dt;
        grdPint.DataBind();
    }

    private void grdCajaChica_Carga()
    {
        string sqlOtrosCost = "SELECT OC.renglon,oc.Factura,OC.fecha,ltrim(rtrim(C.razon_social)) as razon_social,oc.cantidad,OC.descripcion,oc.Costo_Unitario,oc.Descuento,oc.Importe,c.id_cliprov,oc.id_nota_credito,case oc.pago when -1 then 'No Especificado' when 0 then 'Contado' when 1 then 'Crédito' else '' end as pago " +
            "FROM otros_costos OC LEFT JOIN CLIPROV C ON c.id_cliprov = Cast(oc.proveedor AS INT) anD c.tipo = 'P' " +
            "WHERE OC.id_empresa=" + lblEmpresa.Text + " and OC.id_taller=" + lblTaller.Text + " and OC.no_orden=" + lblOrden.Text + " and OC.area_de_aplicacion = 'CA' ORDER BY OC.fecha";

        string sqlVentaEnc = "SELECT venta_det.renglon, venta_enc.ticket, venta_enc.fecha_venta, LTRIM(Cliprov.razon_social), venta_det.cantidad, venta_det.descripcion, venta_det.venta_unitaria, venta_det.valor_descuento, venta_det.importe, Cliprov.id_cliprov " +
            "FROM venta_det INNER JOIN Registro_Pinturas AS rp ON venta_det.ticket = rp.ticket " +
            "INNER JOIN venta_enc ON venta_det.ticket = venta_enc.ticket " +
            "INNER JOIN Cliprov ON venta_enc.id_prov = Cliprov.id_cliprov AND Cliprov.tipo = 'P' " +
            "WHERE(rp.id_empresa=" + lblEmpresa.Text + ") AND(rp.id_taller=" + lblTaller.Text + ") AND(rp.no_orden=" + lblOrden.Text + ") AND(venta_enc.Area_Aplicacion = 'Cc') ORDER BY venta_enc.fecha_venta";

        DataTable dt = inicializaTb(sqlOtrosCost, sqlVentaEnc);
        grdCajaChica.DataSource = dt;
        grdCajaChica.DataBind();
    }

    private void grdAlmacen_Carga()
    {
        string sqlOtrosCost = "";

        string sqlVentaEnc = "SELECT venta_det.renglon, venta_enc.ticket, venta_enc.fecha_venta, LTRIM(Cliprov.razon_social), venta_det.cantidad, venta_det.descripcion, venta_det.venta_unitaria, venta_det.valor_descuento, venta_det.importe, Cliprov.id_cliprov " +
            "FROM venta_det INNER JOIN Registro_Pinturas AS rp ON venta_det.ticket = rp.ticket " +
            "INNER JOIN venta_enc ON venta_det.ticket = venta_enc.ticket " +
            "INNER JOIN Cliprov ON venta_enc.id_prov = Cliprov.id_cliprov AND Cliprov.tipo = 'P' " +
            "WHERE(rp.id_empresa=" + lblEmpresa.Text + ") AND(rp.id_taller=" + lblTaller.Text + ") AND(rp.no_orden=" + lblOrden.Text + ") AND(venta_enc.Area_Aplicacion = 'Al') ORDER BY venta_enc.fecha_venta";

        DataTable dt = inicializaTb(sqlOtrosCost, sqlVentaEnc);
        RadGrid6.DataSource = dt;
        RadGrid6.DataBind();
    }

    private DataTable inicializaTb(string qry1, string qry2)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("renglon", typeof(string));
        dt.Columns.Add("Factura", typeof(string));
        dt.Columns.Add("fecha", typeof(string));
        dt.Columns.Add("razon_social", typeof(string));
        dt.Columns.Add("cantidad", typeof(string));
        dt.Columns.Add("descripcion", typeof(string));
        dt.Columns.Add("Costo_Unitario", typeof(string));
        dt.Columns.Add("Descuento");
        dt.Columns.Add("Importe");
        dt.Columns.Add("Provedor");

        Ejecuciones ejec = new Ejecuciones();
        DataSet ds = new DataSet();
        if (qry1 != "")
        {
            ds = (DataSet)ejec.dataSet(qry1)[1];
            foreach (DataRow row in ds.Tables[0].Rows)
                dt.Rows.Add(row[0], row[1], Convert.ToDateTime(row[2]).ToString("yyyy-MM-dd"), row[3], row[4], row[5], row[6], row[7], row[8], row[9]);
        }
        ds = (DataSet)ejec.dataSet(qry2)[1];
        foreach (DataRow row in ds.Tables[0].Rows)
            dt.Rows.Add('0' + row[0].ToString(), "T-" + row[1].ToString(), Convert.ToDateTime(row[2]).ToString("yyyy-MM-dd"), row[3], row[4], row[5], row[6], row[7], row[8], row[9]);

        return dt;
    }

    protected void grdPint_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        SqlDsPintura.DeleteParameters["reng"].DefaultValue = ((GridDataItem)e.Item).GetDataKeyValue("renglon").ToString();
        SqlDsPintura.Delete();
        string noFact = ((GridDataItem)e.Item)["Factura"].Text;
        string prov = ((GridDataItem)e.Item).GetDataKeyValue("Provedor").ToString();
        string sqlBorraFact = "DELETE FROM Facturas WHERE Factura='" + noFact + "' AND id_cliprov=" + prov + " AND id_taller=" + lblTaller.Text + " AND id_empresa=" + lblEmpresa.Text + " AND no_orden=" + lblOrden.Text + " AND TipoCuenta='PA'";
        Facturas fact = new Facturas();
        fact.actualizaFacturas(sqlBorraFact, 0);
        grdPint_NeedDataSource(sender, null);
    }

    protected void grdCajaChica_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        SqlDsCajChica.DeleteParameters["reng"].DefaultValue = ((GridDataItem)e.Item).GetDataKeyValue("renglon").ToString();
        SqlDsCajChica.Delete();
        string noFact = ((GridDataItem)e.Item)["Factura"].Text;
        string prov = ((GridDataItem)e.Item).GetDataKeyValue("Provedor").ToString();
        string sqlBorraFact = "DELETE FROM Facturas WHERE Factura='" + noFact + "' AND id_cliprov=" + prov + " AND id_taller=" + lblTaller.Text + " AND id_empresa=" + lblEmpresa.Text + " AND no_orden=" + lblOrden.Text + " AND TipoCuenta='PA'";
        Facturas fact = new Facturas();
        fact.actualizaFacturas(sqlBorraFact, 0);
        grdCajaChica_Carga();
    }

    protected void lnkAgregaProv_Click(object sender, EventArgs e)
    {
        string razSoc = ddlProv.Text;
        string sqlInsProv = "DECLARE @provID int SET @provID = (SELECT ISNULL((SELECT TOP 1 id_cliprov FROM Cliprov WHERE tipo='P' ORDER BY id_cliprov DESC),0) + 1) " +
                            "INSERT INTO Cliprov (id_cliprov, tipo, persona, rfc, fecha_nacimiento, razon_social) VALUES(@provID, 'P', 'M', '.', '1900-01-01','" + razSoc + "') " +
                            "SELECT @provID";
        Ejecuciones ejec = new Ejecuciones();
        string provID = ejec.scalarToString(sqlInsProv)[1].ToString();
        ddlProv.DataBind();
        ddlProv.SelectedValue = provID;
        lblErrorCc.Text = "Proveedor " + razSoc + " agregado";
    }

    protected void lnkAgregaProvCc_Click(object sender, EventArgs e)
    {
        string razSoc = radcboProv.Text;
        string sqlInsProv = "DECLARE @provID int SET @provID = (SELECT ISNULL((SELECT TOP 1 id_cliprov FROM Cliprov WHERE tipo='P' ORDER BY id_cliprov DESC),0) + 1) " +
                            "INSERT INTO Cliprov (id_cliprov, tipo, persona, rfc, fecha_nacimiento, razon_social) VALUES(@provID, 'P', 'M', '.', '1900-01-01','" + razSoc + "') " +
                            "SELECT @provID";
        Ejecuciones ejec = new Ejecuciones();
        string provID = ejec.scalarToString(sqlInsProv)[1].ToString();
        radcboProv.DataBind();
        radcboProv.SelectedValue = provID;
        lblErrorCc.Text = "Proveedor " + razSoc + " agregado";
    }

    protected void lnkImprimirtodo_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        ImprimeInfoCostoUnidad imprimir = new ImprimeInfoCostoUnidad();
        int[] sesiones = obtieneSesiones();

        string archivo = imprimir.imprimeCostoUnidad(Convert.ToInt32(lblEmp.Text), Convert.ToInt32(lblTall.Text), Convert.ToInt32(lblOrd.Text));
        try
        {
            if (archivo != "")
            {
                FileInfo filename = new FileInfo(archivo);
                if (filename.Exists)
                {
                    string url = "Descargas.aspx?filename=" + filename.Name;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, fullscreen=yes,resizable=yes');", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al descargar el archivo: " + ex.Message;
        }
    }
    protected void lnkimprimir_Click(object sender, EventArgs e)
    {
            
            string script = "abreWin()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
        RadGrid3.Rebind();
        
    }

    private DataTable tablaTodo(string qry)
    {
        char estatus ='A';
        CostoUnidad inserta = new CostoUnidad();
        DataTable dt = new DataTable();
        dt.Columns.Add("id_empresa");
        dt.Columns.Add("id_taller");
        dt.Columns.Add("no_orden");
        dt.Columns.Add("fecha");
        dt.Columns.Add("identificador");
        dt.Columns.Add("cantidad", typeof(string));
        dt.Columns.Add("nombre");
        dt.Columns.Add("montoAutorizado");
        dt.Columns.Add("razon_social");
        dt.Columns.Add("monto1");
        dt.Columns.Add("monto2");
        dt.Columns.Add("estatus");
        

        Ejecuciones ejec = new Ejecuciones();        
        DataSet ds = new DataSet();
        
        if (qry != "")
        {
            
            ds = (DataSet)ejec.dataSet(qry)[1];
            foreach (DataRow row in ds.Tables[0].Rows)
                if (Convert.ToString(row[3]) == "")
                {
                    dt.Rows.Add(row[0], row[1], row[2],row[3], row[4], row[5], row[6], row[7], row[8], row[9], row[10], estatus);
                }
            else
                dt.Rows.Add(row[0], row[1], row[2], Convert.ToDateTime(row[3]).ToString("yyyy-MM-dd"), row[4], row[5], row[6], row[7], row[8], row[9], row[10], estatus);            
        }

        return dt;
    }


    protected void lnkAgregar_Click(object sender, EventArgs e)
    {
        LinkButton lnkAgrega = (LinkButton)sender;
        string fecha="";
        string[] argumentos = lnkAgrega.CommandArgument.ToString().Split(';');        
        string identificador = Convert.ToString(argumentos[3]);
        if (argumentos[2] == fecha)
        {
            fecha = null;
        }
        else
        {
            DateTime fecha1 = Convert.ToDateTime(argumentos[2]);
            fecha = fecha1.ToString("yyyy-MM-dd");
        }
        decimal cantidad = Convert.ToDecimal(argumentos[4]);
        int id_material = Convert.ToInt32(argumentos[1]);
        string nombre = Convert.ToString(argumentos[5]);
        decimal montoAutorizado = Convert.ToDecimal(argumentos[6]);
        string razonSocial = argumentos[7];
        decimal monto1 = Convert.ToDecimal(argumentos[8]);
        decimal monto2 = Convert.ToDecimal(argumentos[9]);
        int no_orden = Convert.ToInt32(argumentos[0]);
        bool argegaProd = producto.agregaProductoImpresion(Convert.ToInt32(lblEmp.Text), Convert.ToInt32(lblTall.Text), identificador, fecha, cantidad, nombre, montoAutorizado, razonSocial, monto1, monto2, no_orden,id_material);
        if (!argegaProd)
            lblErrorCosto.Text = "No se logro agregar el concepto a imprimir";
        RadGrid3.Rebind();
        RadGrid4.Rebind();
        
    
    }
    protected void lnkQuitar_Click(object sender, EventArgs e)
    {
        
        LinkButton lnkQuita = (LinkButton)sender;
        string[] argumentos = lnkQuita.CommandArgument.ToString().Split(';');
        string identificador = argumentos[0];
        int no_orden = Convert.ToInt32(argumentos[1]);
        int idmaterial = Convert.ToInt32(argumentos[2]);
        bool quitaProd = producto.quitaProductoImpresion(Convert.ToInt32(lblEmp.Text), Convert.ToInt32(lblTall.Text), no_orden, idmaterial, identificador);
        if (!quitaProd)
            lblErrorCosto.Text = "No se logro quitar el concepto a imprimir.";
        RadGrid3.Rebind();
        RadGrid4.Rebind();
    }
    protected void lnkAgregarTodo_Click(object sender, EventArgs e)
    {
        CostoUnidad AG = new CostoUnidad();
        int empresa = Convert.ToInt32(lblEmp.Text);
        int taller = Convert.ToInt32(lblTall.Text);
        int orden = Convert.ToInt32(lblOrd.Text);
        int id_material = 0;
        string fecha;

        AG.borrarTemporal2(empresa,taller,orden);

        string sql = "select tabla.ref_id_empresa, tabla.ref_id_taller, tabla.ref_no_orden,'' as Fecha, 'Refacciones' as Identificador, cast(tabla.refCantidad as varchar(100)) as refCantidad, tabla.refDescripcion,case when tabla.proceso = 'C' then 0 else tabla.refprecioVenta * tabla.refcantidad end as montoAutorizado, c.razon_social, cast(tabla.refCosto as varchar (100)) as Monto1, cast(tabla.refprecioVenta as varchar(100)) as Monto2 from( select ro.ref_id_empresa, ro.ref_id_taller, ro.ref_no_orden, ro.refOrd_id, ro.refDescripcion, ro.refCantidad,case ro.refProveedor when 0 then ro.id_cliprov_cotizacion else ro.refproveedor end as refProveedor, ro.refCosto, ro.refprecioVenta, (ro.refcantidad * ro.refCosto) as importe,ro.refEstatus, ro.refestatussolicitud, re.staDescripcion, ro.reffechSolicitud, ro.reffechEntregaEst, ro.refFechEntregaReal, isnull((select sum(o.importe) from orden_compra_detalle o inner join refacciones_orden r on r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.reford_id = o.id_refaccion where o.no_orden = ro.ref_no_orden and o.id_empresa = ro.ref_id_empresa and o.id_taller = ro.ref_id_taller and r.reford_id = ro.reford_id), 0) as Compra, ro.proceso from refacciones_orden ro left join Rafacciones_Estatus re on re.staRefID = ro.refEstatusSolicitud where ro.ref_no_orden =" + orden + " and ro.ref_id_empresa =" + empresa + " and ro.ref_id_taller =" + taller + "and refEstatusSolicitud =3) as tabla left join cliprov c on c.id_cliprov = tabla.refproveedor and c.tipo = 'P'";
        DataTable dt = tablaTodo(sql);

        foreach (DataRow row in dt.Rows)
        {

            int id_empresa = Convert.ToInt32(row[0]);
            int id_taller = Convert.ToInt32(row[1]);
            int no_orden = Convert.ToInt32(row[2]);
            id_material = id_material + 1;
            if (Convert.ToString(row[3]) == "")
            {
                fecha = null;
            }
            else
            {
                DateTime fechas = Convert.ToDateTime(row[3]);
                fecha = fechas.ToString("yyyy-MM-dd");
            }
            string identificador = Convert.ToString(row[4]);
            decimal cantidad = Convert.ToDecimal(row[5]);
            string nombre = Convert.ToString(row[6]);
            decimal montoA = Convert.ToDecimal(row[7]);
            string razon_social = Convert.ToString(row[8]);
            decimal monto1 = Convert.ToDecimal(row[9]);
            decimal monto2 = Convert.ToDecimal(row[10]);


            AG.tb1atb2(id_empresa, id_taller, no_orden, id_material, fecha, identificador, cantidad, nombre, montoA, razon_social, monto1, monto2);
        }

        RadGrid3.DataBind();
        RadGrid4.DataBind();
    }

    protected void lnkQuitarTodo_Click(object sender, EventArgs e)
    {

        int empresa = Convert.ToInt32(lblEmp.Text);
        int taller = Convert.ToInt32(lblTall.Text);
        int orden = Convert.ToInt32(lblOrd.Text);
        bool quitaTodo = producto.quitarTodo(empresa,taller,orden);
        if (!quitaTodo)
            lblError.Text = "No se logro quitar el o los conceptos de la impresion.";
        RadGrid3.Rebind();
        RadGrid4.Rebind();
    }
    protected void lnkImprimirs_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        ImprimeCostoUnidad imprimir = new ImprimeCostoUnidad();
        int[] sesiones = obtieneSesiones();

        string archivo = imprimir.imprimeCostoUnidad(Convert.ToInt32(lblEmp.Text), Convert.ToInt32(lblTall.Text), Convert.ToInt32(lblOrd.Text));
        try
        {
            if (archivo != "")
            {
                FileInfo filename = new FileInfo(archivo);
                if (filename.Exists)
                {
                    string url = "Descargas.aspx?filename=" + filename.Name;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, fullscreen=yes,resizable=yes');", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al descargar el archivo: " + ex.Message;
        }
    }
    protected void lnkRefrescar_Click(object sender, EventArgs e)
    {
        
        CostoUnidad AG = new CostoUnidad();
        int empresa = Convert.ToInt32(lblEmp.Text);
        int taller = Convert.ToInt32(lblTall.Text);
        int orden = Convert.ToInt32(lblOrd.Text);
        int id_material = 0;
        string fecha;

        AG.borrarTemporal2(empresa,taller,orden);
        AG.borrarTemporal1(empresa,taller,orden);

        string sql = "select tabla.ref_id_empresa, tabla.ref_id_taller, tabla.ref_no_orden,'' as Fecha, 'Refacciones' as Identificador, cast(tabla.refCantidad as varchar(100)) as refCantidad, tabla.refDescripcion,case when tabla.proceso = 'C' then 0 else tabla.refprecioVenta * tabla.refcantidad end as montoAutorizado, c.razon_social, cast(tabla.refCosto as varchar (100)) as Monto1, cast(tabla.refprecioVenta as varchar(100)) as Monto2 from( select ro.ref_id_empresa, ro.ref_id_taller, ro.ref_no_orden, ro.refOrd_id, ro.refDescripcion, ro.refCantidad,case ro.refProveedor when 0 then ro.id_cliprov_cotizacion else ro.refproveedor end as refProveedor, ro.refCosto, ro.refprecioVenta, (ro.refcantidad * ro.refprecioVenta) as importe,ro.refEstatus, ro.refestatussolicitud, re.staDescripcion, ro.reffechSolicitud, ro.reffechEntregaEst, ro.refFechEntregaReal, isnull((select sum(o.importe) from orden_compra_detalle o inner join refacciones_orden r on r.ref_no_orden = o.no_orden and r.ref_id_empresa = o.id_empresa and r.ref_id_taller = o.id_taller and r.reford_id = o.id_refaccion where o.no_orden = ro.ref_no_orden and o.id_empresa = ro.ref_id_empresa and o.id_taller = ro.ref_id_taller and r.reford_id = ro.reford_id), 0) as Compra, ro.proceso from refacciones_orden ro left join Rafacciones_Estatus re on re.staRefID = ro.refEstatusSolicitud where ro.ref_no_orden =" + orden + " and ro.ref_id_empresa =" + empresa + " and ro.ref_id_taller =" + taller + " and refEstatusSolicitud =3 ) as tabla left join cliprov c on c.id_cliprov = tabla.refproveedor and c.tipo = 'P'";
        DataTable dt = tablaTodo(sql);

        foreach (DataRow row in dt.Rows)
        {

            int id_empresa = Convert.ToInt32(row[0]);
            int id_taller = Convert.ToInt32(row[1]);
            int no_orden = Convert.ToInt32(row[2]);
            id_material = id_material + 1;
            if (Convert.ToString(row[3]) == "")
            {
                fecha = null;
            }
            else
            {
                DateTime fechas = Convert.ToDateTime(row[3]);
                fecha = fechas.ToString("yyyy-MM-dd");
            }
            string identificador = Convert.ToString(row[4]);
            decimal cantidad = Convert.ToDecimal(row[5]);
            string nombre = Convert.ToString(row[6]);
            decimal montoA = Convert.ToDecimal(row[7]);
            string razon_social = Convert.ToString(row[8]);
            decimal monto1 = Convert.ToDecimal(row[9]);
            decimal monto2 = Convert.ToDecimal(row[10]);


            AG.AgregaTodoASeleccionar(id_empresa, id_taller, no_orden, id_material, fecha, identificador, cantidad, nombre, montoA, razon_social, monto1, monto2);

        }
        
        RadGrid3.Rebind();
        RadGrid4.Rebind();
    }
    protected void lnkTerminar_Click(object sender, EventArgs e)
    {
        int taller= Convert.ToInt32(lblTall.Text);
        int empresa= Convert.ToInt32(lblEmp.Text);
        int orden= Convert.ToInt32(lblOrd.Text);
        CostoUnidad borra = new CostoUnidad();
        borra.TerminarImpresion1(taller,empresa,orden);
        borra.TerminarImpresion2(taller,empresa,orden);
        RadGrid3.Rebind();
        RadGrid4.Rebind();
        string script = "cierraWin()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
    }
}
