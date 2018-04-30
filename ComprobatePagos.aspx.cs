using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Xml;

public partial class ComprobatePagos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkNuevo.Visible = true;
        }
    }

    //Metodos Generales

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
        Response.Redirect("FComprobantePagos.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=0");
    }
    protected void lnkSeleccionaDocumento_Click(object sender, EventArgs e)
    {
        LinkButton bnt = (LinkButton)sender;
        int factura = Convert.ToInt32(bnt.CommandArgument.ToString());
        Response.Redirect("FComprobantePagos.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=" + factura);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        /*LinkButton aut = (LinkButton)sender;
        string[] arg = aut.CommandArgument.ToString().Split(new char[] { ';' });
        int fac = Convert.ToInt16(arg[0]);

        Ejecuciones eje = new Ejecuciones();
        object[] emisor = eje.scalarToString("select EncEmRFC from enccfd_f where idcfd=" + fac);
        object[] receptor = eje.scalarToString("select EncReRFC from enccfd_f where idcfd=" + fac);

        string Directorio = HttpContext.Current.Server.MapPath("~/Comprobantes");
        Directorio = Directorio + "\\" + emisor[1].ToString() + "\\" + receptor[1].ToString();
        string rutaXML = Directorio;

        string nombre = receptor[1].ToString() + "-" + fac + ".xml";
        string rutaDestino = HttpContext.Current.Server.MapPath("~/TMP");
        XmlDocument xDoc = new XmlDocument();

        xDoc.Load(rutaXML + "\\" + nombre);

        string CadenaXML = xDoc.InnerXml;

        // Crea el objeto
        RVCFDI33.RVCancelacion.Cancelacion objCan = new RVCFDI33.RVCancelacion.Cancelacion();
        // Timbra el archivo
        //string UUID = LeerValorXML(CadenaXML, "UUID", "TimbreFiscalDigital");
        object[] UUID = eje.scalarToString("select EncFolioUUID from encCfd_f where idcfd=" + fac);
        string emi = emisor[1].ToString();
        string rec = receptor[1].ToString();
        string claveUuID = UUID[1].ToString();
        int timbrado = 0;
        string ruta = HttpContext.Current.Server.MapPath("~\\Comprobantes\\MCA9505036Z2\\" + rec.Trim().ToUpper());
        if (!Directory.Exists(ruta + "\\Cancelacion"))
            Directory.CreateDirectory(ruta + "\\Cancelados");
        string rutaCancelacion = HttpContext.Current.Server.MapPath("~/Comprobantes/MCA9505036Z2/" + rec.Trim().ToUpper() + "/Cancelados/Cancela" + claveUuID + ".xml");
        //string rutaCancelacion = HttpContext.Current.Server.MapPath("~/Comprobantes/"+emisor+"/"+receptor+"/Cancelados/Cancelacion:"+UUID[1].ToString()+".xml");
        // string rutaCer = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/CSD_Pruebas_CFDI_LAN7008173R5.cer");
        //string rutaKey = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/CSD_Pruebas_CFDI_LAN7008173R5.key");
        string rutaCer = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/00001000000406147836.cer");
        string rutaKey = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/CSD_DEL_ORIENTE_MCA9505036Z2_20170511_143948.key");
        string usuario = "MCA9505036Z2";
        string contrasena = "K2C694v6";
        string clave = "MONCAR2017";
        if (UUID[1].ToString() == "")
        {
            lblError.Text = "Esta factura no ha sido timbrada";
            return;
        }
        objCan.crearXMLCancelacionArchivo(rutaCer, rutaKey, clave, UUID[1].ToString(), rutaCancelacion);
        if (objCan.MensajeDeError != "")
        {
            lblError.Text = "Ocurrió un error al creal la cancelación: " + objCan.MensajeDeError;
            return;
        }
        objCan.enviarCancelacionArchivo(rutaCancelacion, usuario, contrasena, "http://generacfdi.com.mx/rvltimbrado/service1.asmx?WSDL", true);
        // Verifica el resultado
        if (objCan.MensajeDeError == "")
        {
            lblError.Text = "Se canceló el XML con éxito";
            ProdServ cesta = new ProdServ();
            cesta.fac = fac;
            cesta.cambiaEstatus();
            /* com.formulasistemas.www.ManejadordeTimbres foliosWSFormula = new com.formulasistemas.www.ManejadordeTimbres();
             object[] info = new object[2] { false, "" };
             string rfc = "MCA9505036Z2";
             try
             {
                 int empresaActiva = foliosWSFormula.ObtieneEstatus(rfc);
                 if (empresaActiva != 6)
                 {
                     info[0] = false;
                     info[1] = "Error al timbrar documento: La empresa con R.F.C. " + rfc.Trim().ToUpper() + " esta dada de baja o no existe registrada en el catálogo de empresas de su proveedor de servicios; por favor contáctelo para resolver este error";
                 }
                 else
                 {
                     int foliosDisponibles = 0;
                     foliosDisponibles = foliosWSFormula.ObtieneFoliosDisponibles(rfc);
                     if (foliosDisponibles == 0)
                     {
                         info[0] = false;
                         info[1] = "Error al timbrar documento: La empresa con R.F.C. " + rfc.Trim().ToUpper() + " no cuenta con folios disponibles; por favor a su proveedor de servicio de timbrado para solicitar más folios";
                     }
                     else
                     {
                         int cancelados = 0;

                             cancelados = foliosWSFormula.Timbrar(rfc);

                         foliosDisponibles = foliosWSFormula.ObtieneFoliosDisponibles(rfc);
                     }
                 }
             }
             catch (Exception ex)
             {

             }*/
        /*}
        else
            lblError.Text = "Ocurrió un error al cancelar el XML: " + objCan.MensajeDeError;
        */
    }

    public string LeerValorXML(string xml, string atributo, string nodo)
    {
        //Variables
        string valor;
        int inicio = 0;
        int fin = 0;
        int indexNodo = 0;
        atributo = " " + atributo + "=\"";
        if (!string.IsNullOrEmpty(nodo))
        {
            indexNodo = xml.IndexOf(nodo);
        }
        if (indexNodo < 0)
            return "";
        //Buscamos y leemos el nombre del atributo
        inicio = xml.IndexOf(atributo, indexNodo) + atributo.Length;
        if (inicio < atributo.Length)
        {
            return "";
        }
        fin = xml.IndexOf("\"", inicio);
        /*xml = xml.Remove(inicio, fin - inicio);
        xml = xml.Insert(inicio, nuevoValor);*/
        valor = xml.Substring(inicio, fin - inicio);
        //Regreso de valores si encontro
        return valor;
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
            RadGrid1.DataBind();
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
            FacturacionPago.Facturas factura = new FacturacionPago.Facturas();
            ImprimeFacturaPago imprime = new ImprimeFacturaPago();
            if (idFactura == 0)
            { }
            else
            {
                object[] encabezado = null, timbre = null;
                DataTable detalle = null;
                //Encabezado
                factura.idCfd = idFactura;
                factura.obtieneEncabezadoPAGO();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iEnc = (DataSet)factura.info[1];
                    foreach (DataRow fEnc in iEnc.Tables[0].Rows)
                    {
                        encabezado = fEnc.ItemArray;
                    }
                }

                //Detalle
                factura.obtieneDetallePAGO();
                if (Convert.ToBoolean(factura.info[0]))
                {
                    DataSet iDet = (DataSet)factura.info[1];
                    detalle = iDet.Tables[0];
                }

                //Timbrado
                factura.obtieneTimbradoPAGO();
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
                            FileInfo tmp = new FileInfo(ruta + "\\" + filename.Name);
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


    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
        {
            int factura = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "idCfd").ToString());
            string estatus = DataBinder.Eval(e.Item.DataItem, "EncEstatus").ToString();
            string timbrado = DataBinder.Eval(e.Item.DataItem, "EncFolioUUID").ToString();
            var btnCancelar = e.Item.FindControl("lnkCancelar") as LinkButton;
            //var btnEnviar = e.Item.FindControl("lnkEnviar") as LinkButton;
            //var btnaddenda = e.Item.FindControl("lnkADD") as LinkButton;
            if (estatus == "T" || estatus == "P")
                btnCancelar.Visible = true;
            else
                btnCancelar.Visible = false;
            if (estatus == "T")
            {
                //btnaddenda.Visible = true;
                //btnEnviar.Visible = true;
            }
            else
            {
                //btnEnviar.Visible = false;
                //btnaddenda.Visible = false;
            }

        }
    }

    protected void lnkEnviaCorreoPop_Click(object sender, EventArgs e)
    {
        int documento = Convert.ToInt32(lblDocumnetoPopup.Text);
        int[] sesiones = obtieneSesiones();
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

            FileInfo archivoFacturado = new FileInfo(Convert.ToString(timbre[7]));
            string[] facturasEnviar = archivoFacturado.Name.Trim().Split(new char[] { '.' });
            string nombreArchivo = facturasEnviar[0];


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
                }
                else
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
                                "<tr><td><p align='justify'>Moncar Aztahuacan le agradece su preferencia.</p></td></tr>" +
                                "<tr><td>&nbsp;</td></tr>" +
                                "<tr><td><p align='justify'>" + Convert.ToString(infoEmisor[47]).Trim() + "</p></td></tr></table>");
            object[] enviado = enviar.obtieneDatosServidor("", Convert.ToString(infoReceptor[22]).ToLower().Trim(), mensaje, "", "Envio de Factura", archivosEnviar, sesiones[2], "", "");
            if (Convert.ToBoolean(enviado[0]))
                lblError.Text = "Se ha enviado la factura vía correo electrónico";
            else
                lblError.Text = "No pudo enviar el correo electrónico, intente de nuevo. Detalle: " + Convert.ToString(enviado[1]);
        }
        catch (Exception ex)
        {
            lblError.Text = "Error de envio: " + ex.Message;
        }
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
    protected void lnkCancelar_Click1(object sender, EventArgs e)
    {
        Cancelacion edt = new Cancelacion();
        LinkButton aut = (LinkButton)sender;
        string[] arg = aut.CommandArgument.ToString().Split(new char[] { ';' });
        int factura = Convert.ToInt16(arg[0]);
        edt.factura = factura;
        edt.editaEstatus();
        RadGrid1.DataBind();
    }

    protected void lnkADD_Click(object sender, EventArgs e)
    {
        LinkButton bnt = (LinkButton)sender;
        try
        {
            LinkButton aut = (LinkButton)sender;
            string[] arg = aut.CommandArgument.ToString().Split(new char[] { ';' });
            int adenda = Convert.ToInt16(arg[0]);
            Response.Redirect("AdendaQualitas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&add=" + adenda.ToString());
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }
    protected void impCatProd_Click(object sender, EventArgs e)
    {
        string usuario = Request.QueryString["u"].ToString();
        ImprimeFacturaPrueba imprimirExcel = new ImprimeFacturaPrueba();
        imprimirExcel.usuario = usuario;
        imprimirExcel.estatusFact = ddlEstatus.SelectedValue;
        imprimirExcel.Ini = txtFechaIni.Text;
        imprimirExcel.Fin = txtFechaFin.Text;
        imprimirExcel.imprimeExcel();
        string Archivo = imprimirExcel.archivo;
        if (Archivo != "")
        {
            try
            {
                FileInfo docto = new FileInfo(Archivo);
                if (docto.Exists)
                {
                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + docto.Name);
                    Response.WriteFile(Archivo);
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

}