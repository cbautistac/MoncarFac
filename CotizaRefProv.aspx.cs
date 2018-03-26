using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class CotizaRefProv : System.Web.UI.Page
{
    datosCotizaProv datos = new datosCotizaProv();
    Recepciones recepciones = new Recepciones();
    int noOrde, idEmpresa, idTaller;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            checaSesiones();
            cargaDatosPie();
            lblCotizacion.Text = "0";          
        }
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

    private void llenaFolio()
    {        
        lblFolio.Text = (noOrde + idEmpresa + idTaller).ToString();
    }

    private int[] checaSesiones()
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

    protected void lnkAsignaProvCotiza_Click(object sender, EventArgs e)
    {
        int[] sesiones = checaSesiones();
        LinkButton lnkAsignaProvCotiza = (LinkButton)sender;
        int idProv = Convert.ToInt32(lnkAsignaProvCotiza.CommandArgument);
        int folio = 0;
        object[] folios = datos.obtieneFolio(sesiones[2], sesiones[3], sesiones[4]);
        if (Convert.ToBoolean(folios[0]))
            folio = Convert.ToInt32(folios[1]);
        else
        {
            folio = 0;
            lblError.Text = "Error: " + Convert.ToString(folios[1]);
        }
        if (folio > 0)
        {
            bool agregado = datos.agregarProvARef(sesiones[4], sesiones[2], sesiones[3], idProv, folio);
            if (!agregado)
                lblError.Text = "No se logro agregar el proveedor de la lista de cotización.";
            RadGrid1.DataBind();
            RadGrid2.DataBind();
        }
    }

    protected void lnkDelProv_Click(object sender, EventArgs e)
    {
        int[] sesiones = checaSesiones();
        LinkButton lnkDelProv = (LinkButton)sender;
        string[] argumentos = lnkDelProv.CommandArgument.ToString().Split(new char[] { ';' });
        int idProv = Convert.ToInt32(argumentos[0]);
        int folio = Convert.ToInt32(argumentos[1]);
        bool quitaProv = datos.quitaProveedorCotizacion(sesiones[4], sesiones[2], sesiones[3], idProv, folio);
        if (!quitaProv)
            lblError.Text = "No se logro quitar el proveedor de la lista de cotización.";
        RadGrid1.DataBind();
        RadGrid2.DataBind();
    }

    protected void lnkEnviarCotizacion_Click(object sender, EventArgs e)
    {
        int[] sesiones = checaSesiones();
        //int folio = Convert.ToInt32(lblFolio.Text);
        string folioCot = txtFolio.Text.ToUpper();
        int cotizacion = 0;
       bool existeMoncar = false;
        object[] buscaMoncar = datos.existeMoncarCotizacion(sesiones[2], sesiones[3], sesiones[4]);
        if (Convert.ToBoolean(buscaMoncar[0]))
            existeMoncar = Convert.ToBoolean(buscaMoncar[1]);
        if (!existeMoncar)
        {
            try { datos.insertaMoncarCotizacion(sesiones[2], sesiones[3], sesiones[4]); }
            catch (Exception) { }
        }

        object[] proveedores = datos.obtieneProveedores(sesiones[2], sesiones[3], sesiones[4]);
        if (Convert.ToBoolean(proveedores[0]))
        {
            DataSet datosProv = (DataSet)proveedores[1];
            int cont = 0;
            if (datosProv.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow fila in datosProv.Tables[0].Rows)
                {
                    object[] generaCot = datos.generaCot(sesiones[2], sesiones[3], sesiones[4], cotizacion, Convert.ToInt32(fila[0].ToString()), txtFolio.Text.ToUpper());
                    if (Convert.ToBoolean(generaCot[0]))
                    {
                        cotizacion = Convert.ToInt32(generaCot[1]);
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
                            if (Convert.ToInt32(fila[0].ToString()) != 0)
                            {
                                Envio_Mail correo = new Envio_Mail();
                                string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Cotiza.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + cotizacion + "&p=" + fila[0].ToString();
                                string mensaje = "<h3>Solicitud de Cotizaci&oacute;n</h3><p>Estimado proveedor haga clic en el siguiente link para realizar la cotización de la o las refacciones que se solicitan. Tome en cuenta que en un lapso de " + info + " horas la cotizaci&oacute;n dejar&aacute; de estar disponible.</p><br/>" +
                                    "<a href='" + url + "' target='_blank'>Realice la cotizaci&oacute;n aqu&iacute;</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...<br/><br/><b>LOS MONTOS REFLEJADOS EN LA PRESENTE SUBASTA DEBERAN SER RESPETADOS AL MOMENTO DEL SURTIMIENTO</b></p>";
                                string asunto = "Solicitud de Cotización";
                                object[] correoEnviado = correo.obtieneDatosServidor("", fila[3].ToString(), mensaje, "", asunto, null, sesiones[2], "", "");

                                CotizacionesEnviadas cotizacionEnviada = new CotizacionesEnviadas();
                                
                                if (Convert.ToBoolean(correoEnviado[0]))
                                {
                                    bool enviado = Convert.ToBoolean(correoEnviado[0]);
                                    cotizacionEnviada.orden = sesiones[4];
                                    cotizacionEnviada.empresa = sesiones[2];
                                    cotizacionEnviada.taller = sesiones[3];
                                    cotizacionEnviada.cotizacion = cotizacion;
                                    cotizacionEnviada.proveedor = Convert.ToInt32(fila[0].ToString());
                                    cotizacionEnviada.enviado = enviado;
                                    cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                                    cotizacionEnviada.correo = fila[3].ToString();
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
                                    cotizacionEnviada.proveedor = Convert.ToInt32(fila[0].ToString());
                                    cotizacionEnviada.enviado = false;
                                    cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                                    cotizacionEnviada.usuario = sesiones[0];
                                    cotizacionEnviada.correo = fila[3].ToString();
                                    cotizacionEnviada.motivo = Convert.ToString(correoEnviado[1]);
                                    cotizacionEnviada.insertaEnvio();
                                }
                                cont++;
                            }
                        }
                    }
                }
                if (cont == datosProv.Tables[0].Rows.Count - 1)                
                    lblError.Text = "Se ha generado la petición a los proveedores indicados para la cotización con folio " + txtFolio.Text.ToUpper();
                else
                    lblError.Text = "Solo se ha generado la cotización para algunos de los proveedores y/o ya se ha generado una cotización para dichos proveedores, vuelva a intentar";
            }
            else
                lblError.Text = "No se han indicado proveedores para cotizar; seleccione al menos un proveedor";
        }
        else
            lblError.Text = "Error: " + Convert.ToString(proveedores[1]);

        lblCotizacion.Text = cotizacion.ToString();
        RadGrid3.DataBind();
    }

    protected void lnkReenviar_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            int[] sesiones = checaSesiones();
            string[] parametros = btn.CommandArgument.ToString().Split(new char[] { ';' });
            int cotizacion = Convert.ToInt32(parametros[0]);
            int proveedor = Convert.ToInt32(parametros[1]);
            string correos = parametros[2];
            int horasT = 0;
            object[] horas = datos.obtieneHrsMaxTaller(sesiones[2], sesiones[3]);
            if (Convert.ToBoolean(horas[0]))
                horasT = Convert.ToInt32(horas[1]);
            string info = "";
            if (horasT == 0)
                info = " corto ";
            else
                info = horasT.ToString();

            Envio_Mail correo = new Envio_Mail();
            string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Cotiza.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + cotizacion + "&p=" + proveedor;
            string mensaje = "<h3>Solicitud de Cotizaci&oacute;n</h3><p>Estimado proveedor haga clic en el siguiente link para realizar la cotización de la o las refacciones que se solicitan. Tome en cuenta que en un lapso de " + info + " horas la cotizaci&oacute;n dejar&aacute; de estar disponible.</p><br/>" +
                "<a href='" + url + "' target='_blank'>Realice la cotizaci&oacute;n aqu&iacute;</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...<br/><br/><b>LOS MONTOS REFLEJADOS EN LA PRESENTE SUBASTA DEBERAN SER RESPETADOS AL MOMENTO DEL SURTIMIENTO</b></p>";
            string asunto = "Solicitud de Cotización";
            object[] correoEnviado = correo.obtieneDatosServidor("", correos, mensaje, "", asunto, null, sesiones[2], "", "");

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
            lblCotizacion.Text = cotizacion.ToString();
            RadGrid3.DataBind();
        }
        catch (Exception ex) {
            lblError.Text = "Error de reenvio. Detalle: " + ex.Message;
        }



    }
}