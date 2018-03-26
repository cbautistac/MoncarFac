using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;

public partial class FacturasOrden : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            cargaDatosPie();
            if (Request.QueryString["sta"] == "A")
            {
                lnkNuevo.Visible = true;
                lnkCerrarOrden.Visible = true;
            }
            else if (Request.QueryString["sta"] == "T")
            {
                lnkNuevo.Visible = true;
                lnkCerrarOrden.Visible = false;
            }
            else
            {
                lnkNuevo.Visible = true;
                lnkCerrarOrden.Visible = false;
            }
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                    lnkCerrarOrden.Visible = false;
                if (estatus == "R" || estatus == "S" || estatus == "C")
                    lnkNuevo.Visible = false;
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

    //Metodos Generales
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
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
    protected void lnkNuevo_Click(object sender, EventArgs e)
    {
        Response.Redirect("Facturaacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&fact=0" + "&refct=0");
    }
    protected void lnkSeleccionaDocumento_Click(object sender, EventArgs e)
    {
        LinkButton bnt = (LinkButton)sender;
        int factura = Convert.ToInt32(bnt.CommandArgument.ToString());
        Response.Redirect("Facturacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&fact=" + factura + "&refct=2");
    }

    protected void lnkCancelar_Click(object sender, EventArgs e)
    {
        LinkButton bnt = (LinkButton)sender;
        int factura = Convert.ToInt32(bnt.CommandArgument.ToString());
        try
        {
            FacturacionElectronica.GeneracionDocumentos genara = new FacturacionElectronica.GeneracionDocumentos();
            genara.idCfd = factura;
            genara.cancelaDocumento();
            if (Convert.ToBoolean(genara.info[0]))
            {
                Refacciones refacciones = new Refacciones();
                refacciones.actualizaFacturados(factura.ToString(), Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"], "C");
                lblError.Text = "Se ha cancelado el documento correctamente";
            }
            else
                lblError.Text = "Error: " + Convert.ToString(genara.info[1]);
            GridView1.DataBind();            
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    protected void lnkImprimir_Click(object sender, EventArgs e)
    {
        LinkButton bnt = (LinkButton)sender;
        int idFactura = Convert.ToInt32(bnt.CommandArgument.ToString());
        lblError.Text = "";
        try
        {
            FacturacionElectronica.Facturas factura = new FacturacionElectronica.Facturas();
            ImprimeFacturaPrueba imprime = new ImprimeFacturaPrueba();            
            if (idFactura == 0)
            { }
            else
            {
                object[] encabezado = null, timbre = null;
                DataTable detalle = null;
                //Encabezado
                factura.idCfd = idFactura;
                factura.obtieneEncabezado();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iEnc = (DataSet)factura.info[1];
                    foreach (DataRow fEnc in iEnc.Tables[0].Rows)
                    {
                        encabezado = fEnc.ItemArray;
                    }
                }

                //Detalle
                factura.obtieneDetalle();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iDet = (DataSet)factura.info[1];
                    detalle = iDet.Tables[0];
                }

                //Timbrado
                factura.obtieneTimbrado();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iTim = (DataSet)factura.info[1];
                    foreach (DataRow fTim in iTim.Tables[0].Rows)
                    {
                        timbre = fTim.ItemArray;
                    }
                }

                string archivo = imprime.GenFactura(idFactura, encabezado, detalle, timbre);
                try
                {
                    if (archivo != "")
                    {
                        FileInfo filename = new FileInfo(archivo);
                        if (filename.Exists)
                        {
                            string ruta = HttpContext.Current.Server.MapPath("~/files");
                            FileInfo tmp = new FileInfo(ruta+"\\"+filename.Name);
                            if (tmp.Exists)
                                tmp.Delete();
                            filename.CopyTo(ruta + "\\" + filename.Name);
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
            // Imprimir factura
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al imprimir la factura: " + ex.Message;
        }
    }
    protected void lnkCerrarOrden_Click(object sender, EventArgs e)
    {
        try
        {
            string status_ini = obtieneEstatus();
            CatUsuarios usuarios = new CatUsuarios();
            string usuarioAutoriza = usuarios.obtieneNickName(Request.QueryString["u"]);
            object[] cerrado = recepciones.actualizaEstatusOrdenIndicado(Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(Request.QueryString["t"]), Convert.ToInt32(Request.QueryString["o"]), "T", Request.QueryString["u"], usuarioAutoriza, status_ini);
            if (Convert.ToBoolean(cerrado[0]))
            {
                lnkNuevo.Visible = true;
                lnkCerrarOrden.Visible = false;
            }
            else
            {
                lnkNuevo.Visible = false;
                lnkCerrarOrden.Visible = true;
            }
        }
        catch (Exception ex) { lnkNuevo.Visible = false; lblError.Text = "Error: " + ex.Message; }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int factura = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "idCfd").ToString());
            string estatus = DataBinder.Eval(e.Row.DataItem, "EncEstatus").ToString();
            var btnCancelar = e.Row.Cells[9].Controls[1].FindControl("lnkCancelar") as LinkButton;
            var btnEnviar = e.Row.Cells[11].Controls[1].FindControl("lnkEnviar") as LinkButton;
            var btnAddenda = e.Row.Cells[12].Controls[1].FindControl("lnkAddenda") as LinkButton;
            var lnkRefacturarDocumento = e.Row.Cells[11].Controls[1].FindControl("lnkRefacturarDocumento") as LinkButton;
            if (estatus == "T" || estatus == "C")
                lnkRefacturarDocumento.Visible = true;
            else
                lnkRefacturarDocumento.Visible = false;
            if (estatus == "T" || estatus == "P")
            {
                btnCancelar.Visible = true;
            }
            else
                btnCancelar.Visible = false;
            if (estatus == "T")
            {
                btnEnviar.Visible = true;
                btnAddenda.Visible = true;
            }
            else
            {
                btnEnviar.Visible = true;
                btnAddenda.Visible = false;
            }


        }
    }

    protected void lnkEnviarCorreo_Click(object sender, EventArgs e)
    {
        LinkButton bnt = (LinkButton)sender;
        string[] argumentos = bnt.CommandArgument.ToString().Split(new char[] { ';' });
        int documento = Convert.ToInt32(argumentos[0]);
        lblDocumnetoPopup.Text = documento.ToString();
        txtPara.Text = argumentos[1];
        lblError.Text = "";
        string script1 = "abreWinEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreCorreo", script1, true);
    }

    private string generaPDF()
    {
        string ruta = "";
        try
        {
            FacturacionElectronica.Facturas factura = new FacturacionElectronica.Facturas();
            ImprimeFacturaPrueba imprime = new ImprimeFacturaPrueba();
            int documento = Convert.ToInt32(Request.QueryString["fact"]);
            if (documento == 0)
            { }
            else
            {
                object[] encabezado = null, timbre = null;
                DataTable detalle = null;
                //Encabezado
                factura.idCfd = documento;
                factura.obtieneEncabezado();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iEnc = (DataSet)factura.info[1];
                    foreach (DataRow fEnc in iEnc.Tables[0].Rows)
                    {
                        encabezado = fEnc.ItemArray;
                    }
                }

                //Detalle
                factura.obtieneDetalle();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iDet = (DataSet)factura.info[1];
                    detalle = iDet.Tables[0];
                }

                //Timbrado
                factura.obtieneTimbrado();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iTim = (DataSet)factura.info[1];
                    foreach (DataRow fTim in iTim.Tables[0].Rows)
                    {
                        timbre = fTim.ItemArray;
                    }
                }

                string archivo = imprime.GenFactura(documento, encabezado, detalle, timbre);
                try
                {
                    if (archivo != "")
                    {
                        FileInfo filename = new FileInfo(archivo);
                        if (filename.Exists)
                        {
                            ruta = HttpContext.Current.Server.MapPath("~/files");
                            /*filename.CopyTo(ruta + "\\" + filename.Name);
                            string url = "Descargas.aspx?filename=" + filename.Name;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, fullscreen=yes,resizable=yes');", true);*/
                        }
                    }
                }
                catch (Exception ex)
                {
                    ruta= "";
                    lblError.Text = "Error al descargar el archivo: " + ex.Message;
                }

            }
            // Imprimir factura
        }
        catch (Exception ex)
        {
            ruta= "";
            lblError.Text = "Error al imprimir la factura: " + ex.Message;
        }
        return ruta;
    }
    private string generaPDFTimbrado()
    {
        string ruta = "";
        try
        {
            FacturacionElectronica.Facturas factura = new FacturacionElectronica.Facturas();
            ImprimeFacturaPrueba imprime = new ImprimeFacturaPrueba();
            int documento = Convert.ToInt32(Request.QueryString["fact"]);
            if (documento == 0)
            { }
            else
            {
                object[] encabezado = null, timbre = null;
                DataTable detalle = null;
                //Encabezado
                factura.idCfd = documento;
                factura.obtieneEncabezado();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iEnc = (DataSet)factura.info[1];
                    foreach (DataRow fEnc in iEnc.Tables[0].Rows)
                    {
                        encabezado = fEnc.ItemArray;
                    }
                }

                //Detalle
                factura.obtieneDetalle();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iDet = (DataSet)factura.info[1];
                    detalle = iDet.Tables[0];
                }

                //Timbrado
                factura.obtieneTimbrado();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iTim = (DataSet)factura.info[1];
                    foreach (DataRow fTim in iTim.Tables[0].Rows)
                    {
                        timbre = fTim.ItemArray;
                    }
                }

                string archivo = imprime.GenFactura(documento, encabezado, detalle, timbre);
                try
                {
                    if (archivo != "")
                    {
                        FileInfo filename = new FileInfo(archivo);
                        if (filename.Exists)
                        {
                            ruta = HttpContext.Current.Server.MapPath("~/files");
                            /*filename.CopyTo(ruta + "\\" + filename.Name);
                            string url = "Descargas.aspx?filename=" + filename.Name;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, fullscreen=yes,resizable=yes');", true);*/
                        }
                    }
                }
                catch (Exception ex)
                {
                    ruta = "";
                    lblError.Text = "Error al descargar el archivo: " + ex.Message;
                }

            }
            // Imprimir factura
        }
        catch (Exception ex)
        {
            ruta = "";
            lblError.Text = "Error al imprimir la factura: " + ex.Message;
        }
        return ruta;
    }

    protected void lnkAddenda_Click(object sender, EventArgs e)
    {
        LinkButton bnt = (LinkButton)sender;
        try
        {
            int documento = Convert.ToInt32(bnt.CommandArgument.ToString());
            Response.Redirect("AdendaQualitas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&add=" + documento.ToString());
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    protected void lnkEnviaCorreoPop_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        int documento = 0;
        try { documento = Convert.ToInt32(lblDocumnetoPopup.Text); } catch (Exception) { documento = 0; }
        if (documento != 0)
        {
            object[] encabezado = null, timbre = null, infoReceptor = null, infoEmisor = null;
            try
            {
                FacturacionElectronica.Facturas factura = new FacturacionElectronica.Facturas();
                FacturacionElectronica.Receptores receptor = new FacturacionElectronica.Receptores();
                FacturacionElectronica.Emisores emisor = new FacturacionElectronica.Emisores();

                //Encabezado
                factura.idCfd = documento;
                factura.obtieneEncabezado();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iEnc = (DataSet)factura.info[1];
                    foreach (DataRow fEnc in iEnc.Tables[0].Rows)
                    {
                        encabezado = fEnc.ItemArray;
                    }
                }

                //Timbrado
                factura.obtieneTimbrado();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iTim = (DataSet)factura.info[1];
                    foreach (DataRow fTim in iTim.Tables[0].Rows)
                    {
                        timbre = fTim.ItemArray;
                    }
                }

                //Receptor
                receptor.idReceptor = Convert.ToInt32(Convert.ToString(encabezado[3]));
                receptor.obtieneInfoReceptor();
                if (Convert.ToBoolean(receptor.info[0]))
                {
                    DataSet iRec = (DataSet)receptor.info[1];
                    foreach (DataRow itRec in iRec.Tables[0].Rows)
                    {
                        infoReceptor = itRec.ItemArray;
                    }
                }

                //Emisor
                emisor.idEmisor = Convert.ToInt32(Convert.ToString(encabezado[2]));
                emisor.obtieneInfoEmisor();
                if (Convert.ToBoolean(emisor.info[0]))
                {
                    DataSet iEm = (DataSet)emisor.info[1];
                    foreach (DataRow itEm in iEm.Tables[0].Rows)
                    {
                        infoEmisor = itEm.ItemArray;
                    }
                }

                //Detalle
                DataTable detalle = null;
                factura.obtieneDetalle();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iDet = (DataSet)factura.info[1];
                    detalle = iDet.Tables[0];
                }

                string nombreArchivo = "";
                if (timbre != null)
                {
                    if (Convert.ToString(timbre[4]) != "")
                    {
                        FileInfo archivoFacturado = new FileInfo(Convert.ToString(timbre[4]));
                        string[] facturasEnviar = archivoFacturado.Name.Trim().Split(new char[] { '.' });
                        nombreArchivo = facturasEnviar[0];
                    }
                    else
                        nombreArchivo = encabezado[19].ToString();
                }
                else
                    nombreArchivo = encabezado[19].ToString();

                string ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + Convert.ToString(encabezado[20]).Trim() + "/" + Convert.ToString(encabezado[21]).Trim());

                if (!(Directory.Exists(ruta)))
                    Directory.CreateDirectory(ruta);
                string archivoXml = ruta + "\\" + nombreArchivo.Trim() + ".xml";
                string archivoPdf = ruta + "\\" + nombreArchivo.Trim() + ".pdf";

                ListBox archivosEnviar = new ListBox();
                FileInfo filenameXML = new FileInfo(archivoXml);
                FileInfo filenamePDF = new FileInfo(archivoPdf);

                if (filenameXML.Exists)
                {
                    ListItem adjuntos = new ListItem();
                    adjuntos.Value = adjuntos.Text = ruta + "\\" + filenameXML.Name;
                    archivosEnviar.Items.Add(adjuntos);
                    //archivosEnviar.Items.Add(adjuntos);
                }
                else
                {
                    if (Convert.ToString(timbre[4]) != "")
                    {
                        FacturacionElectronica.GeneracionDocumentos genera = new FacturacionElectronica.GeneracionDocumentos();
                        genera.idCfd = documento;                       
                        genera.generaDoctoTimbrado();
                        
                        ListItem adjuntos = new ListItem();
                        adjuntos.Value = adjuntos.Text = ruta + "\\" + Convert.ToString(timbre[4]) + ".xml";
                        archivosEnviar.Items.Add(adjuntos);
                    }
                }

                if (filenamePDF.Exists)
                {
                    ListItem adjuntos = new ListItem();
                    adjuntos.Value = adjuntos.Text = ruta + "\\" + filenamePDF.Name;
                    archivosEnviar.Items.Add(adjuntos);
                }
                else
                {
                    if (Convert.ToString(timbre[4]) == "")
                        nombreArchivo = "Factura_" + encabezado[0].ToString().Trim();
                    else
                        nombreArchivo = "Factura_" + timbre[4].ToString().Trim();
                    archivoPdf = ruta + "\\" + nombreArchivo.Trim() + ".pdf";
                    filenamePDF = new FileInfo(archivoPdf);
                    if (filenamePDF.Exists)
                    {
                        ListItem adjuntos = new ListItem();
                        adjuntos.Value = adjuntos.Text = ruta + "\\" + filenamePDF.Name;
                        archivosEnviar.Items.Add(adjuntos);
                    }else
                    {
                        ImprimeFacturaPrueba imprime = new ImprimeFacturaPrueba();
                        string archivo = imprime.GenFactura(documento, encabezado, detalle, timbre);
                        ListItem adjuntos = new ListItem();
                        adjuntos.Value = adjuntos.Text = archivo;
                        archivosEnviar.Items.Add(adjuntos);
                    }
                }
                
                Envio_Mail enviar = new Envio_Mail();
                string mensaje = string.Format("<table width='553' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                    "<tr><td><p>&nbsp;</p><p>&nbsp;</p></td></tr>" +
                    "<tr><td><img src='http://www.formulasistemas.com/empresas/logoMoncar.png' widht='200' height='100'></td></tr><tr><td><p>&nbsp;</p></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>" +
                    "<tr><td><p align='justify'>"+ txtContenido.Text + "</p></td></tr>" +
                    "<tr><td><p align='justify'>Moncar Aztahuacan le agradece su preferencia.</p></td></tr>" +
                    "<tr><td>&nbsp;</td></tr>" +
                    "<tr><td><p align='justify'>" + Convert.ToString(infoEmisor[47]).Trim() + "</p></td></tr></table>");
                object[] enviado = enviar.obtieneDatosServidor("", txtPara.Text, mensaje, "", txtAsunto.Text, archivosEnviar, sesiones[2], txtCC.Text, txtCCO.Text);
                if (Convert.ToBoolean(enviado[0]))
                    lblError.Text = "Se ha enviado la factura vía correo electrónico";
                else
                    lblError.Text = "No pudo enviar el correo electrónico, intente de nuevo. Detalle: "+Convert.ToString(enviado[1]);
                limpiarPopup();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error de envio: " + ex.Message;
            }
        }
        else
            lblError.Text = "No se logro generar el PDF, verifique sus datos e intentelo nuevamente.";
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        limpiarPopup();
    }

    private void limpiarPopup()
    {
        txtAsunto.Text = "";
        txtCC.Text = "";
        txtCCO.Text = "";
        txtContenido.Text = "";
        txtPara.Text = "";
        lblDocumnetoPopup.Text = "";
    }

    protected void lnkRefacturarDocumento_Click(object sender, EventArgs e)
    {
        LinkButton bnt = (LinkButton)sender;
        int factura = Convert.ToInt32(bnt.CommandArgument.ToString());
        Response.Redirect("Facturaacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&fact=" + factura+"&refct=1");
    }
}