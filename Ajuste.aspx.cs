using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Ajuste : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            lblFecha.Text = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
            lblHora.Text = new E_Utilities.Fechas().obtieneFechaLocal().ToString("HH:mm:ss");
            cargaDatosPie();
            actualizaTotales();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "S" || estatus == "T" || estatus == "F" || estatus == "C")
                {
                    RadGrid1.MasterTableView.CommandItemSettings.ShowSaveChangesButton = false;
                    RadGrid1.MasterTableView.CommandItemSettings.ShowCancelChangesButton = false;
                    //RadGrid2.MasterTableView.CommandItemSettings.ShowSaveChangesButton = false;
                    //RadGrid2.MasterTableView.CommandItemSettings.ShowCancelChangesButton = false;
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
    private void cargaDatosPie()
    {
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
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
            Session["paginaOrigen"] = "Ajuste.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    private int[] obtieneSesionesCotiza()
    {
        int[] sesiones = new int[7] { 0, 0, 0, 0, 0, 0 ,0};
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);
            sesiones[4] = Convert.ToInt32(Request.QueryString["o"]);
            sesiones[5] = Convert.ToInt32(Request.QueryString["f"]);
            sesiones[6] = 0;
        }
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ajuste.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    protected void RadGrid1_ItemUpdated(object sender, Telerik.Web.UI.GridUpdatedEventArgs e)
    {
        string sql = SqlDataSource1.UpdateCommand;
        if (e.Exception != null)
            lblError.Text = e.Exception.Message;
        else
            actualizaTotales();
        
    }

    private void actualizaTotales()
    {
        decimal totMo = 0, totRef = 0, totalOrden = 0;
        TotalesOrden totales = new TotalesOrden();
        // Total Mano Obra
        totales.Empresa = Convert.ToInt32(Request.QueryString["e"]);
        totales.Taller = Convert.ToInt32(Request.QueryString["t"]);
        totales.Orden = Convert.ToInt32(Request.QueryString["o"]);
        totales.obtieneTotalManoObra();
        totMo = totales.ManoObra;
        lblTotalMo.Text = "Total Mano Obra: " + totMo.ToString("C2");
        // Total Refacciones
        totales.obtieneTotalManoRefacciones();
        totRef = totales.Refacciones;
        lblTotalRef.Text = "Total Refacciones: " + totRef.ToString("C2");
        totalOrden = totMo + totRef;
        totales.Importe = totalOrden;
        totales.actualizaTotales();       
    }

    
    protected void RadGrid2_ItemUpdated(object sender, GridUpdatedEventArgs e)
    {
        actualizaTotales();        
    }



    protected void lnkGenerarOrdenCompra_Click(object sender, EventArgs e)
    {
       int[] sesiones = obtieneSesiones();
        OrdenCompra ordenCompra = new OrdenCompra();
        ordenCompra.sesiones = sesiones;
        //ordenCompra.acceso = Convert.ToInt32(Request.QueryString["a"]);
        object[] proveedores = ordenCompra.obtieneProveedoresCompra();
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
                sesiones = obtieneSesionesCotiza();
                sesiones[6] = Convert.ToInt32(fila[3]);
                ordenCompra.sesiones = sesiones;
                ordenCompra.autoriza = sesiones[0];
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
                        refacciones._taller = Convert.ToInt32(Request.QueryString["t"]);
                        refacciones._proveedor = Convert.ToInt32(fila[0].ToString());
                        string fechaPromesa = refacciones.obtieneFechaMinEntEstimada();                        
                        string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Orden.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + ordenGenerada.ToString() + "&p=" + fila[0].ToString() + "&s=SOL";

                        DateTime fechaEntrega = new E_Utilities.Fechas().obtieneFechaLocal();
                        string valorFecha = "";
                        try { fechaEntrega = Convert.ToDateTime(fechaPromesa); valorFecha = " Recuerde que debe entregar las refacciones y/o piezas a mas tardar el " + fechaEntrega.ToString("d MMM yyyy") + ", fecha especificada en su cotizaci&oacute;n; "; } catch (Exception) { valorFecha = " Recuerde que debe entregar las refacciones y/o piezas en las fechas definidas en su cotizaci&oacute;n; "; }

                        string mensaje = "<h3>Orden de Compra</h3><p>Estimado proveedor haga clic en el siguiente link para consultar la orden de compra de la o las refacciones que se solicitan. <br/><br/><strong>" + valorFecha + " tambi&eacute;n recuerde que al entregar debe acompa&ntilde;ar con la factura correspondiente.</strong></p><br/>" +
                            "<a href='" + url + "' target='_blank'>Consultar Orden de Compra</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...</p>";
                        string asunto = "Orden de Compra";
                        object[] correoEnviado = correo.obtieneDatosServidor("", fila[2].ToString(), mensaje, "", asunto, null, Convert.ToInt32(Request.QueryString["e"]), "", "");
                        ordenesGeneradas++;

                        CotizacionesEnviadas cotizacionEnviada = new CotizacionesEnviadas();

                        if (Convert.ToBoolean(correoEnviado[0]))
                        {
                            bool enviado = Convert.ToBoolean(correoEnviado[0]);
                            cotizacionEnviada.orden = sesiones[4];
                            cotizacionEnviada.empresa = sesiones[2];
                            cotizacionEnviada.taller = sesiones[3];
                            cotizacionEnviada.cotizacion = ordenGenerada;
                            cotizacionEnviada.proveedor = Convert.ToInt32(fila[0].ToString());
                            cotizacionEnviada.enviado = enviado;
                            cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                            cotizacionEnviada.correo = fila[2].ToString();
                            cotizacionEnviada.usuario = sesiones[0];
                            string motivo = "";
                            if (!enviado)
                                motivo = Convert.ToString(correoEnviado[1]);
                            cotizacionEnviada.motivo = motivo;
                            cotizacionEnviada.insertaEnvioCompra();
                        }
                        else
                        {
                            cotizacionEnviada.orden = sesiones[4];
                            cotizacionEnviada.empresa = sesiones[2];
                            cotizacionEnviada.taller = sesiones[3];
                            cotizacionEnviada.cotizacion = ordenGenerada;
                            cotizacionEnviada.proveedor = Convert.ToInt32(fila[0].ToString());
                            cotizacionEnviada.enviado = false;
                            cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                            cotizacionEnviada.usuario = sesiones[0];
                            cotizacionEnviada.correo = fila[2].ToString();
                            cotizacionEnviada.motivo = Convert.ToString(correoEnviado[1]);
                            cotizacionEnviada.insertaEnvioCompra();
                        }


                    }
                }
            }
            lblError.Text = "Se ha generado la orden de compra correspondiente para el o los proveedores indicados en esta cotización, asi se han enviado " + ordenesGeneradas.ToString() + " correos electrónicos de " + proveedoresCompras.ToString() + " proveedores a informar";
            //RadGrid3.DataBind();
            //PanelMascara.Visible = PanelPopUpPermiso.Visible = false;
            //string script = "cierraWinAuto()";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
        }
        else
            lblError.Text = "Error: " + Convert.ToString(proveedores[1]);
            
    }

    protected void lnkReenviar_Click(object sender, EventArgs e)
    {
        try
        {
            Envio_Mail correo = new Envio_Mail();
            Refacciones refacciones = new Refacciones();
            LinkButton btn = (LinkButton)sender;
            int[] sesiones = obtieneSesiones();
            string[] parametros = btn.CommandArgument.ToString().Split(new char[] { ';' });
            int cotizacion = Convert.ToInt32(parametros[0]);
            int proveedor = Convert.ToInt32(parametros[1]);
            string correos = parametros[2];

            refacciones._orden = Convert.ToInt32(Request.QueryString["o"]);
            refacciones._empresa = Convert.ToInt32(Request.QueryString["e"]);
            refacciones._taller = Convert.ToInt32(Request.QueryString["t"]);
            refacciones._proveedor = proveedor;
            string fechaPromesa = refacciones.obtieneFechaMinEntEstimada();
            string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Orden.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + cotizacion.ToString() + "&p=" +proveedor.ToString() + "&s=SOL";

            DateTime fechaEntrega = new E_Utilities.Fechas().obtieneFechaLocal();
            string valorFecha = "";
            try { fechaEntrega = Convert.ToDateTime(fechaPromesa); valorFecha = " Recuerde que debe entregar las refacciones y/o piezas a mas tardar el " + fechaEntrega.ToString("d MMM yyyy") + ", fecha especificada en su cotizaci&oacute;n; "; } catch (Exception) { valorFecha = " Recuerde que debe entregar las refacciones y/o piezas en las fechas definidas en su cotizaci&oacute;n; "; }

            string mensaje = "<h3>Orden de Compra</h3><p>Estimado proveedor haga clic en el siguiente link para consultar la orden de compra de la o las refacciones que se solicitan. <br/><br/><strong>" + valorFecha + " tambi&eacute;n recuerde que al entregar debe acompa&ntilde;ar con la factura correspondiente.</strong></p><br/>" +
                "<a href='" + url + "' target='_blank'>Consultar Orden de Compra</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...</p>";
            string asunto = "Orden de Compra";
            object[] correoEnviado = correo.obtieneDatosServidor("", correos, mensaje, "", asunto, null, Convert.ToInt32(Request.QueryString["e"]), "", "");
                       

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
                cotizacionEnviada.correo = correos;
                cotizacionEnviada.usuario = sesiones[0];
                string motivo = "";
                if (!enviado)
                    motivo = Convert.ToString(correoEnviado[1]);
                cotizacionEnviada.motivo = motivo;
                cotizacionEnviada.actualizaEnvio();
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
                cotizacionEnviada.correo = correos;
                cotizacionEnviada.motivo = Convert.ToString(correoEnviado[1]);
                cotizacionEnviada.actualizaEnvio();
            }
            //lblCotizacion.Text = cotizacion.ToString();
            //RadGrid3.DataBind();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error de reenvio. Detalle: " + ex.Message;
        }



    }
        
}