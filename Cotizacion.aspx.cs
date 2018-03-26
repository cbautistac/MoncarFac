using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using ClosedXML.Excel;
using System.Drawing;

public partial class Cotizacion : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["a"] == "0")
                lnkImprimir.Visible = lnkComparativo.Visible = false;
            else
                lnkImprimir.Visible = lnkComparativo.Visible = true;
            cargaDatosPie();
            checaCambioMontos();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    lnkAgregarProveedor.Visible= pnlNuevoProv.Visible = false;
                    RadGridCot.MasterTableView.Columns[6].Visible = false;
                    pnlNuevoProveedorCot.Visible = false;
                    radEnviados.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;                    
                    //GridView1.Columns[7].Visible = false;
                    Label1.Visible = ddlProveedores.Visible = chkCambiaMontos.Visible = Label3.Visible = txtCosto.Visible = lnkActualizarCosto.Visible = false;
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

    private void checaCambioMontos()
    {

        txtCosto.Enabled = chkCambiaMontos.Checked;
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
    protected void lnkSeleccionar_Click(object sender, EventArgs e)
    {
        try
        {
            datosCotizaProv datos = new datosCotizaProv();
            //LinkButton btn = (LinkButton)sender;
            //string[] argumentos = btn.CommandArgument.ToString().Split(new char[]{';'});
            int[] sesiones = obtieneSesiones();
            string cotizacionSelect = RadGridCot.SelectedValue.ToString();
            if (cotizacionSelect != "")
            {
                string folio = datos.obtieneFolioCot(sesiones[2], sesiones[3], sesiones[4], cotizacionSelect);

                Response.Redirect("ComprativoCot.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&c=" + cotizacionSelect + "&fc=" + folio + "&a=" + Request.QueryString["a"]);
            }
            else
                lblError.Text = "Debe seleccionar una cotización";
        }
        catch (Exception ex) { lblError.Text = "Error al accesar a la cotización. Debe seleccionar una cotización. Detalle: " + ex.Message; }
    }

    protected void lnkCancelar_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        LinkButton btn = (LinkButton)sender;
        string argumentos = btn.CommandArgument.ToString();
        datosCotizaProv cotizacion = new datosCotizaProv();
        object[] actualiza = cotizacion.actualizaCancelacion(argumentos, sesiones);
        if (Convert.ToBoolean(actualiza[0])) {
            RadGridCot.DataBind();
            actualizaFase();
        }
        else
            lblError.Text = "Error: " + Convert.ToString(actualiza[1]);
    }
   

    private void actualizaFase()
    {
        int faseSActual = 1;
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
        try
        {
            object[] datos = recepciones.obtieneInfoOrden(orden, empresa, taller);
            if (Convert.ToBoolean(datos[0]))
            {
                DataSet valores = (DataSet)datos[1];
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    faseSActual = Convert.ToInt32(fila[53].ToString());
                }

                if (faseSActual < 4)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 4);
                }
            }
        }
        catch (Exception) { }

    }
    protected void lnkActualizarCosto_Click(object sender, EventArgs e)
    {
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
        int idProv = Convert.ToInt32(ddlProveedores.SelectedValue);
        bool cambiaCostos = chkCambiaMontos.Checked;
        decimal costo = 0;
        if (cambiaCostos)
            costo = Convert.ToDecimal(txtCosto.Text);
        try
        {
            object[] datos = recepciones.actualizaUnicoProveedor(orden, empresa, taller, idProv, costo, cambiaCostos);
            if (Convert.ToBoolean(datos[0]))
            {
                if (cambiaCostos)
                    lblError.Text = "Se actualizo el proveedor correctamente y los montos se establecieron en: " + costo.ToString("C2");
                else
                    lblError.Text = "Se actualizo el proveedor correctamente";
            }
            else
                lblError.Text = "Ocurrio un error inseperado al actualizar la información";
        }
        catch (Exception) { }
    }

    protected void chkCambiaMontos_CheckedChanged(object sender, EventArgs e)
    {
        bool cambiarMontos = chkCambiaMontos.Checked;
        if (cambiarMontos)
            txtCosto.Enabled = true;
        else
            txtCosto.Enabled = false;
    }

    protected void lnkComparativo_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        lblError.Text = "";
        txtProveedorAdd.Text = "";
        //txtPorcDesc.Text = txtContoUnitario.Text = Convert.ToDecimal("0").ToString();
        //txtDias.Text = "";
        radDias.Value = radPorcDesc.Value = radCostoUnitario.Value = 0;
        chkExistencia.Checked = false;
        cargaComparativo(sesiones);

        string script = "abreWinComp()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "comparativo", script, true);
    }

    private void cargaComparativo(int[] sesiones)
    {
        datosCotizaProv cotizacion = new datosCotizaProv();
        object[] info = cotizacion.generaComparativoGral(sesiones);
        if (Convert.ToBoolean(info[0]))
        {
            DataSet datos = (DataSet)info[1];
            int proveedores = Convert.ToInt32(info[0]);
            int cont = 0;
            for (int i = 2; i < RadGrid1.Columns.Count - 1; i++)
            {
                if (cont < proveedores)
                    RadGrid1.Columns[i].Visible = true;
                else
                    RadGrid1.Columns[i].Visible = false;
                cont++;
            }
            RadGrid1.Columns[0].ItemStyle.CssClass = RadGrid1.Columns[1].ItemStyle.CssClass = "ancho20";
            RadGrid1.DataSource = datos.Tables[0];

            //grdComparativo.DataSource = datos;
        }
        else
        {
            RadGrid1.DataSource = null;
            //grdComparativo.DataSource = null;
        }
        RadGrid1.DataBind();
        //inactivaControlesGrid();
        //grdComparativo.DataBind();        
    }

    [WebMethod]
    [ScriptMethod]
    public static List<string> obtieneProveedores(string prefixText)
    {
        List<string> lista = new List<string>();

        SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["Taller"].ToString());
        try
        {
            conexionBD.Open();
            SqlCommand cmd = new SqlCommand("select rtrim(ltrim(upper(razon_social))) from cliprov where tipo='P' and razon_social like '%" + prefixText + "%'", conexionBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            IDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(lector.GetString(0));
            }
            lector.Close();
        }
        catch (Exception) { }
        finally { conexionBD.Close(); }
        return lista;
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //DataRowView filas = (DataRowView)e.Item.DataItem;
            //DataRow r = filas.Row;
            var estatus = DataBinder.Eval(e.Item.DataItem, "refEstatus").ToString();

            LinkButton btnSelecciona1 = e.Item.FindControl("lnkProveedor1") as LinkButton;
            LinkButton btnSelecciona2 = e.Item.FindControl("lnkProveedor2") as LinkButton;
            LinkButton btnSelecciona3 = e.Item.FindControl("lnkProveedor3") as LinkButton;
            LinkButton btnSelecciona4 = e.Item.FindControl("lnkProveedor4") as LinkButton;
            LinkButton btnSelecciona5 = e.Item.FindControl("lnkProveedor5") as LinkButton;
            LinkButton btnSelecciona6 = e.Item.FindControl("lnkProveedor6") as LinkButton;
            LinkButton btnSelecciona7 = e.Item.FindControl("lnkProveedor7") as LinkButton;
            LinkButton btnSelecciona8 = e.Item.FindControl("lnkProveedor8") as LinkButton;
            LinkButton btnSelecciona9 = e.Item.FindControl("lnkProveedor9") as LinkButton;
            LinkButton btnSelecciona10 = e.Item.FindControl("lnkProveedor10") as LinkButton;
            LinkButton btnSelecciona11 = e.Item.FindControl("lnkProveedor11") as LinkButton;
            LinkButton btnSelecciona12 = e.Item.FindControl("lnkProveedor12") as LinkButton;
            LinkButton btnSelecciona13 = e.Item.FindControl("lnkProveedor13") as LinkButton;
            LinkButton btnSelecciona14 = e.Item.FindControl("lnkProveedor14") as LinkButton;
            LinkButton btnSelecciona15 = e.Item.FindControl("lnkProveedor15") as LinkButton;
            LinkButton btnSelecciona16 = e.Item.FindControl("lnkProveedor16") as LinkButton;
            LinkButton btnSelecciona17 = e.Item.FindControl("lnkProveedor17") as LinkButton;
            LinkButton btnSelecciona18 = e.Item.FindControl("lnkProveedor18") as LinkButton;
            LinkButton btnSelecciona19 = e.Item.FindControl("lnkProveedor19") as LinkButton;
            LinkButton btnSelecciona20 = e.Item.FindControl("lnkProveedor20") as LinkButton;

            Panel pp1 = e.Item.FindControl("pp1") as Panel;
            Panel pp2 = e.Item.FindControl("pp2") as Panel;
            Panel pp3 = e.Item.FindControl("pp3") as Panel;
            Panel pp4 = e.Item.FindControl("pp4") as Panel;
            Panel pp5 = e.Item.FindControl("pp5") as Panel;
            Panel pp6 = e.Item.FindControl("pp6") as Panel;
            Panel pp7 = e.Item.FindControl("pp7") as Panel;
            Panel pp8 = e.Item.FindControl("pp8") as Panel;
            Panel pp9 = e.Item.FindControl("pp9") as Panel;
            Panel pp10 = e.Item.FindControl("pp10") as Panel;
            Panel pp11 = e.Item.FindControl("pp11") as Panel;
            Panel pp12 = e.Item.FindControl("pp12") as Panel;
            Panel pp13 = e.Item.FindControl("pp13") as Panel;
            Panel pp14 = e.Item.FindControl("pp14") as Panel;
            Panel pp15 = e.Item.FindControl("pp15") as Panel;
            Panel pp16 = e.Item.FindControl("pp16") as Panel;
            Panel pp17 = e.Item.FindControl("pp17") as Panel;
            Panel pp18 = e.Item.FindControl("pp18") as Panel;
            Panel pp19 = e.Item.FindControl("pp19") as Panel;
            Panel pp20 = e.Item.FindControl("pp20") as Panel;

            string costo1 = DataBinder.Eval(e.Item.DataItem, "costo_unitario1").ToString();
            string costo2 = DataBinder.Eval(e.Item.DataItem, "costo_unitario2").ToString();
            string costo3 = DataBinder.Eval(e.Item.DataItem, "costo_unitario3").ToString();
            string costo4 = DataBinder.Eval(e.Item.DataItem, "costo_unitario4").ToString();
            string costo5 = DataBinder.Eval(e.Item.DataItem, "costo_unitario5").ToString();
            string costo6 = DataBinder.Eval(e.Item.DataItem, "costo_unitario6").ToString();
            string costo7 = DataBinder.Eval(e.Item.DataItem, "costo_unitario7").ToString();
            string costo8 = DataBinder.Eval(e.Item.DataItem, "costo_unitario8").ToString();
            string costo9 = DataBinder.Eval(e.Item.DataItem, "costo_unitario9").ToString();
            string costo10 = DataBinder.Eval(e.Item.DataItem, "costo_unitario10").ToString();
            string costo11 = DataBinder.Eval(e.Item.DataItem, "costo_unitario11").ToString();
            string costo12 = DataBinder.Eval(e.Item.DataItem, "costo_unitario12").ToString();
            string costo13 = DataBinder.Eval(e.Item.DataItem, "costo_unitario13").ToString();
            string costo14 = DataBinder.Eval(e.Item.DataItem, "costo_unitario14").ToString();
            string costo15 = DataBinder.Eval(e.Item.DataItem, "costo_unitario15").ToString();
            string costo16 = DataBinder.Eval(e.Item.DataItem, "costo_unitario16").ToString();
            string costo17 = DataBinder.Eval(e.Item.DataItem, "costo_unitario17").ToString();
            string costo18 = DataBinder.Eval(e.Item.DataItem, "costo_unitario18").ToString();
            string costo19 = DataBinder.Eval(e.Item.DataItem, "costo_unitario19").ToString();
            string costo20 = DataBinder.Eval(e.Item.DataItem, "costo_unitario20").ToString();

            decimal cost1 = Convert.ToDecimal(costo1);
            decimal cost2 = Convert.ToDecimal(costo2);
            decimal cost3 = Convert.ToDecimal(costo3);
            decimal cost4 = Convert.ToDecimal(costo4);
            decimal cost5 = Convert.ToDecimal(costo5);
            decimal cost6 = Convert.ToDecimal(costo6);
            decimal cost7 = Convert.ToDecimal(costo7);
            decimal cost8 = Convert.ToDecimal(costo8);
            decimal cost9 = Convert.ToDecimal(costo9);
            decimal cost10 = Convert.ToDecimal(costo10);
            decimal cost11 = Convert.ToDecimal(costo11);
            decimal cost12 = Convert.ToDecimal(costo12);
            decimal cost13 = Convert.ToDecimal(costo13);
            decimal cost14 = Convert.ToDecimal(costo14);
            decimal cost15 = Convert.ToDecimal(costo15);
            decimal cost16 = Convert.ToDecimal(costo16);
            decimal cost17 = Convert.ToDecimal(costo17);
            decimal cost18 = Convert.ToDecimal(costo18);
            decimal cost19 = Convert.ToDecimal(costo19);
            decimal cost20 = Convert.ToDecimal(costo20);

            if (cost1 <= 0) btnSelecciona1.Enabled = pp1.Visible = false; else btnSelecciona1.Enabled = pp1.Visible = true;
            if (cost2 <= 0) btnSelecciona2.Enabled = pp2.Visible = false; else btnSelecciona2.Enabled = pp2.Visible = true;
            if (cost3 <= 0) btnSelecciona3.Enabled = pp3.Visible = false; else btnSelecciona3.Enabled = pp3.Visible = true;
            if (cost4 <= 0) btnSelecciona4.Enabled = pp4.Visible = false; else btnSelecciona4.Enabled = pp4.Visible = true;
            if (cost5 <= 0) btnSelecciona5.Enabled = pp5.Visible = false; else btnSelecciona5.Enabled = pp5.Visible = true;
            if (cost6 <= 0) btnSelecciona6.Enabled = pp6.Visible = false; else btnSelecciona6.Enabled = pp6.Visible = true;
            if (cost7 <= 0) btnSelecciona7.Enabled = pp7.Visible = false; else btnSelecciona7.Enabled = pp7.Visible = true;
            if (cost8 <= 0) btnSelecciona8.Enabled = pp8.Visible = false; else btnSelecciona8.Enabled = pp8.Visible = true;
            if (cost9 <= 0) btnSelecciona9.Enabled = pp9.Visible = false; else btnSelecciona9.Enabled = pp9.Visible = true;
            if (cost10 <= 0) btnSelecciona10.Enabled = pp10.Visible = false; else btnSelecciona10.Enabled = pp10.Visible = true;
            if (cost11 <= 0) btnSelecciona11.Enabled = pp11.Visible = false; else btnSelecciona11.Enabled = pp11.Visible = true;
            if (cost12 <= 0) btnSelecciona12.Enabled = pp12.Visible = false; else btnSelecciona12.Enabled = pp12.Visible = true;
            if (cost13 <= 0) btnSelecciona13.Enabled = pp13.Visible = false; else btnSelecciona13.Enabled = pp13.Visible = true;
            if (cost14 <= 0) btnSelecciona14.Enabled = pp14.Visible = false; else btnSelecciona14.Enabled = pp14.Visible = true;
            if (cost15 <= 0) btnSelecciona15.Enabled = pp15.Visible = false; else btnSelecciona15.Enabled = pp15.Visible = true;
            if (cost16 <= 0) btnSelecciona16.Enabled = pp16.Visible = false; else btnSelecciona16.Enabled = pp16.Visible = true;
            if (cost17 <= 0) btnSelecciona17.Enabled = pp17.Visible = false; else btnSelecciona17.Enabled = pp17.Visible = true;
            if (cost18 <= 0) btnSelecciona18.Enabled = pp18.Visible = false; else btnSelecciona18.Enabled = pp18.Visible = true;
            if (cost19 <= 0) btnSelecciona19.Enabled = pp19.Visible = false; else btnSelecciona19.Enabled = pp19.Visible = true;
            if (cost20 <= 0) btnSelecciona20.Enabled = pp20.Visible = false; else btnSelecciona20.Enabled = pp20.Visible = true;

            if (estatus == "AU")
                e.Item.Enabled = false;
            //nuevo Por Alfredo Gonzalez
            //esto quita la seleccion de botones 
            //dentro del comparativo 21jul2017-19:30pm
            btnSelecciona1.Enabled = false;
            btnSelecciona2.Enabled = false;
            btnSelecciona3.Enabled = false;
            btnSelecciona4.Enabled = false;
            btnSelecciona5.Enabled = false;
            btnSelecciona6.Enabled = false;
            btnSelecciona7.Enabled = false;
            btnSelecciona8.Enabled = false;
            btnSelecciona9.Enabled = false;
            btnSelecciona10.Enabled = false;
            btnSelecciona11.Enabled = false;
            btnSelecciona12.Enabled = false;
            btnSelecciona13.Enabled = false;
            btnSelecciona14.Enabled = false;
            btnSelecciona15.Enabled = false;
            btnSelecciona16.Enabled = false;
            btnSelecciona17.Enabled = false;
            btnSelecciona18.Enabled = false;
            btnSelecciona19.Enabled = false;
            btnSelecciona20.Enabled = false;
        }
    }

    protected void lnkAgregarProveedor_Click(object sender, EventArgs e)
    {
        Session["P"] = 2;
        txtContraseñaLog.Text = txtUsuarioLog.Text = lblErrorLog.Text = "";
        //PanelPopUpPermiso.Visible = true;
        string script1 = "cierraWinComp()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);
        string script = "abreWinAuto()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        Autorizaciones autoriza = new Autorizaciones();
        datosCotizaProv cotizacion = new datosCotizaProv();
        autoriza.nick = txtUsuarioLog.Text;
        autoriza.contrasena = txtContraseñaLog.Text;
        autoriza.permiso = 1;
        autoriza.validaUsuario();
        if (autoriza.Valido)
        {
            /* Generacion de orden de compra  */
            if (Session["P"].ToString() == "1")
            {
                int[] sesiones = obtieneSesiones();
                OrdenCompra ordenCompra = new OrdenCompra();
                ordenCompra.sesiones = sesiones;
                ordenCompra.acceso = Convert.ToInt32(Request.QueryString["a"]);
                object[] proveedores = ordenCompra.obtieneProveedores();
                Envio_Mail correo = new Envio_Mail();
                Refacciones refacciones = new Refacciones();
                if (Convert.ToBoolean(proveedores[0]))
                {
                    DataSet datos = (DataSet)proveedores[1];
                    int ordenesGeneradas = 0;
                    int proveedoresCompras = 0;
                    foreach (DataRow fila in datos.Tables[0].Rows)
                    {
                        proveedoresCompras++;
                        ordenCompra.proveedor = Convert.ToInt32(fila[0].ToString());
                        ordenCompra.autoriza = autoriza.IdUsuario;
                        object[] orden = ordenCompra.generaOrden();

                        if (Convert.ToBoolean(orden[0]))
                        {
                            int ordenGenerada = Convert.ToInt32(orden[1]);
                            if (ordenGenerada == -2)
                                lblError.Text = "La orden a generar no cuenta con proveedores para su envío, por lo cual no es posible generarla, verfique su información e intente de nuevo";
                            else if (ordenGenerada == -1)
                                lblError.Text = "La orden a generar no cuenta con refacciones con los argumentos válidos, por lo cual no es posible generarla, verfique su información e intente de nuevo";
                            else if (ordenGenerada == -3)
                                lblError.Text = "Ya se ha generado la orden correspondiente a uno o más proveedores";
                            else
                            {
                                ordenCompra.actualizaRefacciones(Convert.ToInt32(fila[0].ToString()), ordenGenerada);
                                refacciones._orden = Convert.ToInt32(Request.QueryString["o"]);
                                refacciones._empresa = Convert.ToInt32(Request.QueryString["e"]);
                                refacciones._taller = Convert.ToInt32(Request.QueryString["r"]);
                                refacciones._proveedor = Convert.ToInt32(fila[0].ToString());
                                string fechaPromesa = refacciones.obtieneFechaMinEntEstimada();
                                string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Orden.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + ordenGenerada.ToString() + "&p=" + fila[0].ToString() + "&s=SOL";
                                string mensaje = "<h3>Orden de Compra</h3><p>Estimado proveedor haga clic en el siguiente link para consultar la orden de compra de la o las refacciones que se solicitan. <br/><br/><strong>Recuerde que debe entregar a mas tardar el " + Convert.ToDateTime(fechaPromesa).ToShortDateString() + ", fecha especificada en su cotizaci&oacute;n; tambi&eacute;n recuerde que al entregar debe acompa&ntilde;ar con la factura correspondiente.</strong></p><br/>" +
                                    "<a href='" + url + "' target='_blank'>Consultar Orden de Compra</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...<br/><br/><b>LOS MONTOS REFLEJADOS EN LA PRESENTE SUBASTA DEBERAN SER RESPETADOS AL MOMENTO DEL SURTIMIENTO</b></p>";
                                string asunto = "Orden de Compra";
                                correo.obtieneDatosServidor("", fila[2].ToString(), mensaje, "", asunto, null, Convert.ToInt32(Request.QueryString["e"]), "", "");
                                ordenesGeneradas++;
                            }
                        }
                    }
                    lblError.Text = "Se ha generado la orden de compra correspondiente para el o los proveedores indicados en esta cotización, asi se han enviado " + ordenesGeneradas.ToString() + " correos electrónicos de " + proveedoresCompras.ToString() + " proveedores a informar";
                    //PanelMascara.Visible = PanelPopUpPermiso.Visible = false;
                    string script = "cierraWinAuto()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
                }
                else
                    lblError.Text = "Error: " + Convert.ToString(proveedores[1]);
            }
            else if (Session["P"].ToString() == "2")
            {
                /*  Comprarativo de Proveedores  */
                //PanelPopUpPermiso.Visible = false;

                string script = "cierraWinAuto()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
                lblErrorNuevo.Text = "";
                int idCliprov = 0;
                int[] sesiones = obtieneSesiones();
                object[] datoProv = cotizacion.obtieneIdProveedor(txtProveedorAdd.Text);
                if (Convert.ToBoolean(datoProv[0]))
                {
                    try { idCliprov = Convert.ToInt32(datoProv[1]); }
                    catch (Exception) { idCliprov = 0; }
                }
                decimal costo, descuento;
                int dias, cantidad, id, existe = 0;
                if (idCliprov != 0)
                {

                    try
                    {
                        costo = Convert.ToDecimal(radCostoUnitario.Value);
                        string[] infoRefacc = ddlRefaccionesNueva.SelectedItem.Text.Split(new char[] { ' ' });
                        string cantidadRef = infoRefacc[0].Substring(1, infoRefacc[0].LastIndexOf(')') - 1);
                        lblCantidad.Text = cantidadRef;
                        lblIdRef.Text = ddlRefaccionesNueva.SelectedValue;
                        cantidad = Convert.ToInt32(lblCantidad.Text);
                        id = Convert.ToInt32(lblIdRef.Text);
                        lblRefaccion.Text = "";
                        int cont = 0;
                        foreach (string dato in infoRefacc)
                        {
                            if (cont > 0)
                                lblRefaccion.Text = lblRefaccion.Text + dato.ToString() + " ";
                            cont++;
                        }
                        lblRefaccion.Text = lblRefaccion.Text.Trim();

                        if (radPorcDesc.Value.ToString() == "")
                            descuento = 0;
                        else
                            descuento = Convert.ToDecimal(radPorcDesc.Value);
                        if (chkExistencia.Checked)
                            existe = 1;
                        else
                            existe = 0;
                        if (radDias.Value.ToString() == "")
                            dias = -1;
                        else
                            dias = Convert.ToInt32(radDias.Value);

                        if (costo == 0)
                            lblErrorNuevo.Text = "Debe indicar un costo unitario";
                        else
                        {
                            if (dias == -1)
                                lblErrorNuevo.Text = "Debe indicar los dias de entrega";
                            else
                            {
                                object[] existeProv = cotizacion.existeProveedorCotizacionGral(sesiones, idCliprov, id);
                                bool exisProv = true;
                                if (Convert.ToBoolean(existeProv[0]))
                                    exisProv = Convert.ToBoolean(existeProv[1]);
                                else
                                    exisProv = true;
                                if (!exisProv)
                                {
                                    decimal importe = cantidad * (costo - (costo * (descuento / 100)));
                                    decimal importeDescuento = (costo * (descuento / 100)) * cantidad;
                                    string idsCotizacionArr = cotizacion.obtieneIdsCotGral(sesiones, lblRefaccion.Text);
                                    for (int intCot=0;intCot< idsCotizacionArr.Length; intCot++)
                                    {
                                        object[] agregaCosto = cotizacion.agregaProveedorCotizacionGral(sesiones, id, lblRefaccion.Text, cantidad, idCliprov, costo, descuento, importeDescuento, importe, existe, dias, idsCotizacionArr[intCot].ToString());
                                        if (Convert.ToBoolean(agregaCosto[0]))
                                        {
                                            //cargaGridProveedores(sesiones);
                                            cargaComparativo(sesiones);
                                            txtProveedorAdd.Text = "";//txtDias.Text = "";
                                            radDias.Value = radPorcDesc.Value = radCostoUnitario.Value = 0;
                                            //txtPorcDesc.Text = txtContoUnitario.Text = Convert.ToDecimal("0.00").ToString();
                                            chkExistencia.Checked = false;
                                        }
                                        else
                                            lblErrorNuevo.Text += "Error: " + Convert.ToString(agregaCosto[1])+"<br/>";
                                    }
                                }
                                else
                                    lblErrorNuevo.Text = "El proveedor " + txtProveedorAdd.Text + " ya existe en la cotización de la refacción";
                            }
                        }
                    }
                    catch (Exception ex) { lblErrorNuevo.Text = "Error: " + ex.Message; }
                    finally
                    {
                        string script1 = "abreWinComp()";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);
                    }
                }
                else {
                    try
                    {
                        costo = Convert.ToDecimal(radCostoUnitario.Value);
                        string[] infoRefacc = ddlRefaccionesNueva.SelectedItem.Text.Split(new char[] { ' ' });
                        string cantidadRef = infoRefacc[0].Substring(1, infoRefacc[0].LastIndexOf(')') - 1);
                        lblCantidad.Text = cantidadRef;
                        lblIdRef.Text = ddlRefaccionesNueva.SelectedValue;
                        cantidad = Convert.ToInt32(lblCantidad.Text);
                        id = Convert.ToInt32(lblIdRef.Text);
                        lblRefaccion.Text = "";
                        int cont = 0;
                        foreach (string dato in infoRefacc)
                        {
                            if (cont > 0)
                                lblRefaccion.Text = lblRefaccion.Text + dato.ToString() + " ";
                            cont++;
                        }
                        lblRefaccion.Text = lblRefaccion.Text.Trim();

                        if (radPorcDesc.Value.ToString() == "")
                            descuento = 0;
                        else
                            descuento = Convert.ToDecimal(radPorcDesc.Value);
                        if (chkExistencia.Checked)
                            existe = 1;
                        else
                            existe = 0;
                        if (radDias.Value.ToString() == "")
                            dias = -1;
                        else
                            dias = Convert.ToInt32(radDias.Value);

                        if (costo == 0)
                            lblErrorNuevo.Text = "Debe indicar un costo unitario";
                        else
                        {
                            if (dias == -1)
                                lblErrorNuevo.Text = "Debe indicar los dias de entrega";
                            else
                            {
                                object[] existeProv = cotizacion.existeProveedorCotizacion(sesiones, idCliprov, id);
                                bool exisProv = true;
                                if (Convert.ToBoolean(existeProv[0]))
                                    exisProv = Convert.ToBoolean(existeProv[1]);
                                else
                                    exisProv = true;

                                decimal importe = cantidad * (costo - (costo * (descuento / 100)));
                                decimal importeDescuento = (costo * (descuento / 100)) * cantidad;

                                if (!exisProv)
                                {
                                    object[] agregaCosto = cotizacion.agregaProveedorCotizacion(sesiones, id, lblRefaccion.Text, cantidad, idCliprov, costo, descuento, importeDescuento, importe, existe, dias);
                                    if (Convert.ToBoolean(agregaCosto[0]))
                                    {
                                        //cargaGridProveedores(sesiones);
                                        cargaComparativo(sesiones);
                                        txtProveedorAdd.Text = /*txtDias.Text =*/ "";
                                        //txtPorcDesc.Text = txtContoUnitario.Text = Convert.ToDecimal("0.00").ToString();
                                        radDias.Value = radPorcDesc.Value = radCostoUnitario.Value = 0;
                                        chkExistencia.Checked = false;
                                    }
                                    else
                                        lblErrorNuevo.Text = "Error: " + Convert.ToString(agregaCosto[1]);
                                }
                                else
                                {
                                    object[] agregaCosto = cotizacion.actualizaProveedorCotizacion(sesiones, id, lblRefaccion.Text, cantidad, idCliprov, costo, descuento, importeDescuento, importe, existe, dias);
                                    if (Convert.ToBoolean(agregaCosto[0]))
                                    {
                                        //cargaGridProveedores(sesiones);
                                        cargaComparativo(sesiones);
                                        txtProveedorAdd.Text = /*txtDias.Text = */"";
                                        //txtPorcDesc.Text = txtContoUnitario.Text = Convert.ToDecimal("0.00").ToString();
                                        radDias.Value = radPorcDesc.Value = radCostoUnitario.Value = 0;
                                        chkExistencia.Checked = false;
                                    }
                                    else
                                        lblErrorNuevo.Text = "Error: " + Convert.ToString(agregaCosto[1]);
                                }
                            }
                        }
                    }
                    catch (Exception ex) { lblErrorNuevo.Text = "Error: " + ex.Message; }
                    finally
                    {
                        string script1 = "abreWinComp()";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);
                    }
                }
            }
        }
        else
            lblErrorLog.Text = "Error: " + autoriza.Error;
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["P"].ToString() == "1")
            {
                string script = "cierraWinAuto()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
                //PanelMascara.Visible = PanelPopUpPermiso.Visible = false;
            }
            else if (Session["P"].ToString() == "2")
            {
                string script1 = "abreWinComp()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);
                string script = "cierraWinAuto()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
                //PanelPopUpPermiso.Visible = false;
            }
        }
        catch (Exception)
        {
            string script = "cierraWinAuto()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
        }
    }


    protected void lnkImprimeComparativo_Click(object sender, EventArgs e)
    {
        ImpresionComparativo imprime = new ImpresionComparativo();
        Recepciones recepciones = new Recepciones();

        string archivo = imprime.GenComparativo(obtieneSesiones(),1);
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

    protected void lnkImprimir_Click(object sender, EventArgs e)
    {
        ImprimirValuacionGral imprime = new ImprimirValuacionGral();
        Recepciones recepciones = new Recepciones();
        string archivo = imprime.generaComparativoGeneral(obtieneSesiones());
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

    protected void lnkImprimeComparativoMapfre_Click(object sender, EventArgs e)
    {
        generaArchivo(1, "");
    }

    protected void lnkPresupuestoRefaccionesExel_Click(object sender, EventArgs e)
    {
        generaArchivo(2, "M");
    }

    protected void lnkCompleMO_Click(object sender, EventArgs e)
    {
        generaArchivo(3, "");
    }

    protected void lnkCompleREF_Click(object sender, EventArgs e)
    {
        generaArchivo(4, "");
    }

    protected void btnComparativoAfirme_Click(object sender, EventArgs e)
    {
        generaArchivo(5,"");
    }

    protected void btnComparativoAtlas_Click(object sender, EventArgs e)
    {
        generaArchivo(2,"A");
    }

    protected void btnComparativoHDI_Click(object sender, EventArgs e)
    {
        generaArchivo(2,"H");
    }

    private void generaArchivo(int opcion, string formato)
    {
        lblErrorNuevo.Text = lblError.Text = lblErrorLog.Text = "";
        string empresa = Request.QueryString["e"];
        string taller = Request.QueryString["t"];
        string orden = Request.QueryString["o"];

        ComparativoExcel exceles = new ComparativoExcel();
        exceles.empresa = empresa;
        exceles.taller = taller;
        exceles.orden = orden;
        exceles.opcion = opcion;
        exceles.formatoColor = formato;
        exceles.generaArchivos();
        string archivo = exceles.archivo;
        try
        {
            FileInfo archExc = new FileInfo(archivo);
            if (archExc.Exists)
            {
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=" + archExc.Name);
                Response.WriteFile(archivo);
                Response.End();
            }
            else
                lblErrorNuevo.Text = "Error al generar el archivo, por favor intente de nuevo, si este problema persiste por favor contacte al administrador del sistema. Detalle:" + archivo;
        }
        catch (Exception ex)
        {
            lblErrorNuevo.Text = "Error al generar el archivo. Detalle:" + ex.Message;
        }
    }

    protected void RadGridCot_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType==GridItemType.AlternatingItem)
        {
            LinkButton btnSelecciona = e.Item.FindControl("lnkCancelar") as LinkButton;
            Label lblRefacciones = e.Item.FindControl("lblRefacciones") as Label;
            int idCotizacion = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "id_cotizacion").ToString());
            int noOrden, idEmpresa, idTaller;
            noOrden = Convert.ToInt32(Request.QueryString["o"]);
            idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            idTaller = Convert.ToInt32(Request.QueryString["t"]);
            string[] refacSplit = recepciones.obtieneRefCot(noOrden, idEmpresa, idTaller, idCotizacion).Split(',');
            string refacConcat = "";
            for (int cont = 0; cont < refacSplit.Length; cont++)
            {
                if (cont == 3)
                    break;
                if (cont == refacSplit.Length - 1)
                    refacConcat += refacSplit[cont] + "...";
                else
                    refacConcat += refacSplit[cont] + ",";
            }
            lblRefacciones.Text = refacConcat;
            if (Request.QueryString["a"].ToString() == "1")
            {
                string estatus = DataBinder.Eval(e.Item.DataItem, "estatus").ToString();
                if (estatus == "CA")
                {
                    btnSelecciona.Visible = false;
                    pnlNuevoProveedorCot.Visible = false;
                    radEnviados.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                }
                if (estatus == "AU")
                {
                    btnSelecciona.Visible = false;
                    pnlNuevoProveedorCot.Visible = false;
                    radEnviados.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                }
                datosCotizaProv cotizacion = new datosCotizaProv();
                int[] sesiones = obtieneSesiones();
                object[] cancelar = cotizacion.existeAutorizada(idCotizacion, sesiones);
                if (Convert.ToBoolean(cancelar[0]))
                {
                    if (Convert.ToBoolean(cancelar[1]))
                    {
                        btnSelecciona.Visible = false;
                        pnlNuevoProveedorCot.Visible = false;
                        radEnviados.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    }
                }
                else
                {
                    btnSelecciona.Visible = false;
                    pnlNuevoProveedorCot.Visible = false;
                    radEnviados.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                }
            }
            else
            {
                btnSelecciona.Visible = false;
                pnlNuevoProveedorCot.Visible = false;
                radEnviados.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
            }
        }
    }

    protected void lnkReenviar_Click(object sender, EventArgs e)
    {
        try
        {
            datosCotizaProv datos = new datosCotizaProv();
            LinkButton btn = (LinkButton)sender;
            int[] sesione = obtieneSesiones();
            string[] parametros = btn.CommandArgument.ToString().Split(new char[] { ';' });
            int cotizacion = Convert.ToInt32(parametros[0]);
            int proveedor = Convert.ToInt32(parametros[1]);
            string correos = parametros[2];
            int horasT = 0;
            object[] horas = datos.obtieneHrsMaxTaller(sesione[2], sesione[3]);
            if (Convert.ToBoolean(horas[0]))
                horasT = Convert.ToInt32(horas[1]);
            string info = "";
            if (horasT == 0)
                info = " corto ";
            else
                info = horasT.ToString();

            Envio_Mail correo = new Envio_Mail();
            string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Cotiza.aspx?o=" + sesione[4] + "&e=" + sesione[2] + "&t=" + sesione[3] + "&c=" + cotizacion + "&p=" + proveedor;
            string mensaje = "<h3>Solicitud de Cotizaci&oacute;n</h3><p>Estimado proveedor haga clic en el siguiente link para realizar la cotización de la o las refacciones que se solicitan. Tome en cuenta que en un lapso de " + info + " horas la cotizaci&oacute;n dejar&aacute; de estar disponible.</p><br/>" +
                "<a href='" + url + "' target='_blank'>Realice la cotizaci&oacute;n aqu&iacute;</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...<br/><br/><b>LOS MONTOS REFLEJADOS EN LA PRESENTE SUBASTA DEBERAN SER RESPETADOS AL MOMENTO DEL SURTIMIENTO</b></p>";
            string asunto = "Solicitud de Cotización";
            object[] correoEnviado = correo.obtieneDatosServidor("", correos, mensaje, "", asunto, null, sesione[2], "", "");

            CotizacionesEnviadas cotizacionEnviada = new CotizacionesEnviadas();

            if (Convert.ToBoolean(correoEnviado[0]))
            {
                bool enviado = Convert.ToBoolean(correoEnviado[0]);
                cotizacionEnviada.orden = sesione[4];
                cotizacionEnviada.empresa = sesione[2];
                cotizacionEnviada.taller = sesione[3];
                cotizacionEnviada.cotizacion = cotizacion;
                cotizacionEnviada.proveedor = proveedor;
                cotizacionEnviada.enviado = enviado;
                cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                cotizacionEnviada.correo = correos;
                cotizacionEnviada.usuario = sesione[0];
                string motivo = "";
                if (!enviado)
                    motivo = Convert.ToString(correoEnviado[1]);
                cotizacionEnviada.motivo = motivo;
                cotizacionEnviada.actualizaEnvio();
            }
            else
            {
                cotizacionEnviada.orden = sesione[4];
                cotizacionEnviada.empresa = sesione[2];
                cotizacionEnviada.taller = sesione[3];
                cotizacionEnviada.cotizacion = cotizacion;
                cotizacionEnviada.proveedor = proveedor;
                cotizacionEnviada.enviado = false;
                cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                cotizacionEnviada.usuario = sesione[0];
                cotizacionEnviada.correo = correos;
                cotizacionEnviada.motivo = Convert.ToString(correoEnviado[1]);
                cotizacionEnviada.actualizaEnvio();
            }

            radEnviados.DataBind();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error de reenvio. Detalle: " + ex.Message;
        }



    }

    protected void lnkAgregarCotización_Click(object sender, EventArgs e)
    {
        lblErrorNuevoPorveedor.Text = "";
        datosCotizaProv datos = new datosCotizaProv();
        int[] sesiones = obtieneSesiones();
        if (ddlPrveedor.SelectedValue == "" || ddlPrveedor.SelectedIndex == -1)
            lblErrorNuevoPorveedor.Text = "Debe indicar un proveedor, pero debe de seleccionarlo";
        else {
            int proveedor = Convert.ToInt32(ddlPrveedor.SelectedValue);
            string cotizacionSelect = RadGridCot.SelectedValue.ToString();
            string folio = datos.obtieneFolioCot(sesiones[2], sesiones[3], sesiones[4], cotizacionSelect);
            string correoP = datos.obtieneCorreoProveedor(proveedor, "P");
            if (correoP != "")
            {
                object[] generaCot = datos.generaCot(sesiones[2], sesiones[3], sesiones[4], Convert.ToInt32(cotizacionSelect), proveedor, folio.ToUpper());
                if (Convert.ToBoolean(generaCot[0]))
                {
                    int cotizacion = Convert.ToInt32(generaCot[1]);
                    if (cotizacion > 0)
                    {
                        int horasT = 0;
                        object[] horas = datos.obtieneHrsMaxTaller(sesiones[2], sesiones[3]);
                        if (Convert.ToBoolean(horas[0]))
                        {
                            horasT = Convert.ToInt32(horas[1]);
                        }
                        string info = "";
                        if (horasT == 0)
                            info = " corto ";
                        else
                            info = horasT.ToString();
                        if (proveedor != 0)
                        {
                            Envio_Mail correo = new Envio_Mail();
                            string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Cotiza.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + cotizacion + "&p=" + proveedor;
                            string mensaje = "<h3>Solicitud de Cotizaci&oacute;n</h3><p>Estimado proveedor haga clic en el siguiente link para realizar la cotización de la o las refacciones que se solicitan. Tome en cuenta que en un lapso de " + info + " horas la cotizaci&oacute;n dejar&aacute; de estar disponible.</p><br/>" +
                                "<a href='" + url + "' target='_blank'>Realice la cotizaci&oacute;n aqu&iacute;</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...<br/><br/><b>LOS MONTOS REFLEJADOS EN LA PRESENTE SUBASTA DEBERAN SER RESPETADOS AL MOMENTO DEL SURTIMIENTO</b></p>";
                            string asunto = "Solicitud de Cotización";
                            
                            object[] correoEnviado = correo.obtieneDatosServidor("", correoP.ToLower().ToString(), mensaje, "", asunto, null, sesiones[2], "", "");

                            CotizacionesEnviadas cotizacionEnviada = new CotizacionesEnviadas();

                            if (Convert.ToBoolean(correoEnviado[0]))
                            {
                                bool enviado = Convert.ToBoolean(correoEnviado[0]);
                                cotizacionEnviada.orden = sesiones[4];
                                cotizacionEnviada.empresa = sesiones[2];
                                cotizacionEnviada.taller = sesiones[3];
                                cotizacionEnviada.cotizacion = cotizacion;
                                cotizacionEnviada.proveedor = proveedor;
                                cotizacionEnviada.enviado = enviado;
                                cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                                cotizacionEnviada.correo = correoP.ToLower();
                                cotizacionEnviada.usuario = sesiones[0];
                                string motivo = "";
                                if (!enviado)
                                    motivo = Convert.ToString(correoEnviado[1]);
                                cotizacionEnviada.motivo = motivo;
                                cotizacionEnviada.insertaEnvio();
                            }
                            else
                            {
                                cotizacionEnviada.orden = sesiones[4];
                                cotizacionEnviada.empresa = sesiones[2];
                                cotizacionEnviada.taller = sesiones[3];
                                cotizacionEnviada.cotizacion = cotizacion;
                                cotizacionEnviada.proveedor = proveedor;
                                cotizacionEnviada.enviado = false;
                                cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                                cotizacionEnviada.usuario = sesiones[0];
                                cotizacionEnviada.correo = correoP;
                                cotizacionEnviada.motivo = Convert.ToString(correoEnviado[1]);
                                cotizacionEnviada.insertaEnvio();
                            }

                            ddlPrveedor.Items.Clear();
                            ddlPrveedor.Text = "";
                            ddlPrveedor.DataBind();
                            radEnviados.Rebind();                            
                        }
                    }
                }
            }
            else
                lblErrorNuevoPorveedor.Text = "No se puede agregar el proveedor a la cotizacion ya que no cuenta con un correo";
        }
    }
}

