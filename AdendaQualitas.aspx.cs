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

public partial class AdendaQualitas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            cargaInfo();
            Session["folios"] = null;
        }
    }

    private void cargaInfo()
    {
        lblError.Text = "";
        try
        {
            txtPeriodo.Text = new E_Utilities.Fechas().obtieneFechaLocal().Year.ToString();
            FacturacionElectronica.AddendaQualitas parametros = new FacturacionElectronica.AddendaQualitas();
            parametros.id = 1;
            parametros.obtieneParametros();
            object[] addenda = null;
            if (Convert.ToBoolean(parametros.info[0]))
            {
                DataSet datos = (DataSet)parametros.info[1];
                foreach (DataRow r in datos.Tables[0].Rows) {
                    addenda = r.ItemArray;
                    break;
                }

                if (addenda != null) {
                    txtNoInterno.Text = Convert.ToString(addenda[1]).Trim();
                    txtIdArea.Text = Convert.ToString(addenda[2]).Trim();
                    txtIdRevision.Text = Convert.ToString(addenda[3]).Trim();
                    txtCdgIntEmisor.Text = Convert.ToString(addenda[4]).Trim();
                    txtCdgIntRec.Text = Convert.ToString(addenda[14]).Trim();
                    txtOficina.Text = Convert.ToString(addenda[13]).Trim();
                    txtNombreEmisor.Text = Convert.ToString(addenda[6]).Trim();
                    txtEmailEmisor.Text = Convert.ToString(addenda[7]).Trim();
                    txtTelEmisor.Text = Convert.ToString(addenda[8]).Trim();
                    txtNombreReceptor.Text = Convert.ToString(addenda[10]).Trim();
                    txtEmailReceptor.Text = Convert.ToString(addenda[11]).Trim();
                    txtTelReceptor.Text = Convert.ToString(addenda[12]).Trim();
                    ddlTipoEmisor.SelectedValue = Convert.ToString(addenda[5]).Trim();
                    ddlTipoReceptor.SelectedValue = Convert.ToString(addenda[9]).Trim();

                    txtDeducible.Text = Convert.ToDecimal(addenda[15]).ToString("F2");
                    txtBancoDeduc.Text = Convert.ToString(addenda[16]);
                    txtFechaDeduc.Text = Convert.ToString(addenda[17]);
                    txtFolioDeduc.Text = Convert.ToString(addenda[18]);

                    txtDemerito.Text = Convert.ToDecimal(addenda[19]).ToString("F2");
                    txtBancoDeme.Text = Convert.ToString(addenda[20]);
                    txtFechaDeme.Text = Convert.ToString(addenda[21]);
                    txtFolioDeme.Text = Convert.ToString(addenda[22]);

                    try { ddlTipoCliente.SelectedValue = Convert.ToString(addenda[23]); } catch (Exception) { ddlTipoCliente.SelectedValue = "0"; }

                }
                else
                    txtNoInterno.Text = txtIdArea.Text = txtIdRevision.Text = txtCdgIntEmisor.Text = txtCdgIntRec.Text = txtOficina.Text = txtNombreEmisor.Text = txtEmailEmisor.Text = txtTelEmisor.Text = txtNombreReceptor.Text = txtEmailReceptor.Text = txtTelReceptor.Text = ddlTipoEmisor.SelectedValue = ddlTipoReceptor.SelectedValue = "";

            }
        }
        catch (Exception) {
            txtNoInterno.Text = txtIdArea.Text = txtIdRevision.Text = txtCdgIntEmisor.Text = txtCdgIntRec.Text = txtOficina.Text = txtNombreEmisor.Text = txtEmailEmisor.Text = txtTelEmisor.Text = txtNombreReceptor.Text = txtEmailReceptor.Text = txtTelReceptor.Text = ddlTipoEmisor.SelectedValue = ddlTipoReceptor.SelectedValue = "";
        }
    }
    protected void lnkGuardar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        bool actualizado = actualizaParametros();
        if (actualizado)
        {
            if (Convert.ToInt32(Request.QueryString["add"]) != 0)
            {
                try
                {
                    int documento = 0;
                    try { documento = Convert.ToInt32(Request.QueryString["add"]); }
                    catch (Exception ex) { }
                    if (documento != 0)
                    {
                        FacturacionElectronica.GeneracionDocumentos factura = new FacturacionElectronica.GeneracionDocumentos();
                        FacturacionElectronica.Facturas facturas = new FacturacionElectronica.Facturas();
                        factura.idCfd = documento;
                        facturas.idCfd = documento;
                        factura.obtieneEncabezadoFactura();
                        if (Convert.ToBoolean(factura.info[0]))
                        {
                            DataSet encabezado = (DataSet)factura.info[1];
                            factura.obtieneDetalleFactura();
                            if (Convert.ToBoolean(factura.info[0]))
                            {
                                DataSet detalle = (DataSet)factura.info[1];
                                //Timbrado
                                facturas.obtieneTimbrado();
                                if (Convert.ToBoolean(facturas.info[0]))
                                {
                                    object[] timbre = null;
                                    DataSet iTim = (DataSet)facturas.info[1];
                                    foreach (DataRow fTim in iTim.Tables[0].Rows)
                                    {
                                        timbre = fTim.ItemArray;
                                        break;
                                    }


                                    DataTable dt = new DataTable();
                                    dt.Columns.Add("renglon");
                                    dt.Columns.Add("folio");
                                    try { dt = (DataTable)Session["folios"]; }
                                    catch (Exception)
                                    {
                                        dt = new DataTable();
                                        dt.Columns.Add("renglon");
                                        dt.Columns.Add("folio");
                                    }
                                    if (dt == null)
                                    {
                                        dt = new DataTable();
                                        dt.Columns.Add("renglon");
                                        dt.Columns.Add("folio");
                                    }



                                    if (timbre != null)
                                    {
                                        object[] enc = encabezado.Tables[0].Rows[0].ItemArray;
                                        string ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + Convert.ToString(enc[0]).Trim() + "/" + Convert.ToString(enc[1]).Trim());
                                        string rutaXml = Convert.ToString(ruta + "\\" + timbre[4].ToString() + ".xml");
                                        FileInfo archivo = new FileInfo(rutaXml);
                                        if (archivo.Exists)
                                        {
                                            archivo.Delete();
                                            ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + Convert.ToString(enc[0]).Trim() + "/" + Convert.ToString(enc[1]).Trim());

                                            FacturacionElectronica.GeneracionDocumentos genera = new FacturacionElectronica.GeneracionDocumentos();
                                            genera.idCfd = documento;
                                            genera.generaDoctoTimbrado();


                                            String xmlAddena = factura.generaAddendaQ(encabezado.Tables[0], detalle.Tables[0],dt, rbTipo.SelectedValue, txtPeriodo.Text);
                                            if (xmlAddena != "")
                                            {
                                                XmlDocument doc = new XmlDocument();
                                                doc.Load(rutaXml);
                                                XmlNode comprobante = doc.DocumentElement;
                                                //XDocument docAdd = XDocument.Parse(xmlAddena);
                                                XmlDocument docAddenda = new XmlDocument();
                                                var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true, IgnoreComments = true };
                                                using (var xmlReader = XmlReader.Create(new StringReader(xmlAddena),settings))
                                                {
                                                    docAddenda.Load(xmlReader);
                                                }

                                                XmlElement addenda = doc.CreateElement("cfdi", "Addenda", "http://www.sat.gob.mx/cfd/3");
                                                addenda.InnerXml = docAddenda.OuterXml;
                                                comprobante.AppendChild(addenda);
                                                doc.Save(rutaXml);
                                                lblError.Text = "La adenda del documento indicado se generó correctamente";
                                                lnkGuardar.Visible = false;
                                            }
                                        }
                                        else {
                                            
                                            ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + Convert.ToString(enc[0]).Trim() + "/" + Convert.ToString(enc[1]).Trim());

                                            FacturacionElectronica.GeneracionDocumentos genera = new FacturacionElectronica.GeneracionDocumentos();
                                            genera.idCfd = documento;
                                            genera.generaDoctoTimbrado();
                                            rutaXml = ruta + "\\" + Convert.ToString(timbre[4]) +"-"+ Request.QueryString["add"].ToString()+ ".xml";
                                            String xmlAddena = factura.generaAddendaQ(encabezado.Tables[0], detalle.Tables[0], dt, rbTipo.SelectedValue, txtPeriodo.Text);
                                            if (xmlAddena != "")
                                            {
                                                XmlDocument doc = new XmlDocument();
                                                doc.Load(rutaXml);
                                                XmlNode comprobante = doc.DocumentElement;
                                                //XDocument docAdd = XDocument.Parse(xmlAddena);
                                                XmlDocument docAddenda = new XmlDocument();
                                                var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true, IgnoreComments = true };
                                                using (var xmlReader = XmlReader.Create(new StringReader(xmlAddena), settings))
                                                {
                                                    docAddenda.Load(xmlReader);
                                                }

                                                XmlElement addenda = doc.CreateElement("cfdi", "Addenda", "http://www.sat.gob.mx/cfd/3");
                                                addenda.InnerXml = docAddenda.OuterXml;
                                                comprobante.AppendChild(addenda);
                                                doc.Save(rutaXml);
                                                lblError.Text = "La adenda del documento indicado se generó correctamente";
                                                lnkGuardar.Visible = false;

                                            }

                                        }
                                        FileInfo archExc = new FileInfo(rutaXml);
                                        if (archExc.Exists)
                                        {
                                            Response.ContentType = "text/xml";
                                            Response.ContentEncoding = System.Text.Encoding.UTF8;
                                            Response.AddHeader("Content-Disposition",string.Format("attachment; filename="+archExc.Name));
                                            Response.TransmitFile(rutaXml);
                                            Response.End();
                                        }
                                    }
                                    else
                                        lblError.Text = "Error: No se encontró el documento indicado  o no se encuentra timbrado";
                                }
                                else
                                    lblError.Text = "Error: " + Convert.ToString(facturas.info[1]);
                            }
                            else
                                lblError.Text = "Error: " + Convert.ToString(factura.info[1]);
                        }
                        else
                            lblError.Text = "Error: " + Convert.ToString(factura.info[1]);
                    }
                    else
                        lblError.Text = "Error: No se indicó un documento";
                }
                catch (Exception ex) { lblError.Text = "Error al generar addenda: " + ex.Message; }
            }
            else
            {
                lblError.Text = "Se han actualizado los parametros de la addenda";
                lnkGuardar.Visible = false;
            }
        }
    }

    private bool actualizaParametros()
    {
        decimal[] montos = new decimal[2] { 0, 0 };
        string[] datos = new string[6] { "X", "0000-00-00", "", "X", "0000-00-00", "" };
        montos = validaMontos();
        datos = validaDatos();
        FacturacionElectronica.AddendaQualitas parametros = new FacturacionElectronica.AddendaQualitas();
        parametros.id = 1;
        parametros.actualiza(txtNoInterno.Text, txtIdArea.Text, txtIdRevision.Text, txtCdgIntEmisor.Text, txtCdgIntRec.Text, txtOficina.Text, txtNombreEmisor.Text, txtEmailEmisor.Text, txtTelEmisor.Text, txtNombreReceptor.Text, txtEmailReceptor.Text, txtTelReceptor.Text, ddlTipoEmisor.SelectedValue, ddlTipoReceptor.SelectedValue, montos[0], datos[0], datos[1], datos[2], montos[1], datos[3], datos[4], datos[5], Convert.ToInt32(ddlTipoCliente.SelectedValue));
        if (!Convert.ToBoolean(parametros.info[0]))        
            lblError.Text = "Error: " + Convert.ToString(parametros.info[1]);
        
        return Convert.ToBoolean(parametros.info[0]);
    }

    private string[] validaDatos()
    {
        string[] datos = new string[6] { "X", "0000-00-00", "", "X", "0000-00-00", "" };
        if (txtBancoDeduc.Text == "")
            datos[0] = "X";
        else
            datos[0] = txtBancoDeduc.Text;
        if (txtFechaDeduc.Text == "")
            datos[1] = "0000-00-00";
        else
        {
            try { datos[1] = Convert.ToDateTime(txtFechaDeduc.Text).ToString("yyyy-MM-dd"); } catch (Exception ex) { datos[1] = "0000-00-00"; }
        }
        if (txtFolioDeduc.Text == "")
            datos[2] = "";
        else
            datos[2] = txtFolioDeduc.Text;
        if (txtBancoDeme.Text == "")
            datos[3] = "X";
        else
            datos[3] = txtBancoDeme.Text;
        if (txtFechaDeme.Text == "")
            datos[4] = "0000-00-00";
        else
        {
            try { datos[4] = Convert.ToDateTime(txtFechaDeme.Text).ToString("yyyy-MM-dd"); } catch (Exception ex) { datos[4] = "0000-00-00"; }
        }        
        if (txtFolioDeme.Text == "")
            datos[5] = "";
        else
            datos[5] = txtFolioDeme.Text;

        return datos;
    }

    private decimal[] validaMontos()
    {
        decimal[] retorno = new decimal[2] { 0, 0 };
        try { retorno[0] = Convert.ToDecimal(txtDeducible.Text); } catch (Exception) { retorno[0] = 0; }
        try { retorno[1] = Convert.ToDecimal(txtDemerito.Text); } catch (Exception) { retorno[1] = 0; }
        return retorno;
    }

    protected void lnkRegresa_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Request.QueryString["add"]) != 0)
            Response.Redirect("FacturasOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
        else
            Response.Redirect("Ordenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkCancelar_Click(object sender, EventArgs e)
    {

    }

    protected void lnkAgregarFolio_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("renglon");
        dt.Columns.Add("folio");
        try { dt = (DataTable)Session["folios"]; }
        catch (Exception) {
            dt = new DataTable();
            dt.Columns.Add("renglon");
            dt.Columns.Add("folio");
        }
        if (dt == null) {
            dt = new DataTable();
            dt.Columns.Add("renglon");
            dt.Columns.Add("folio");
        }
        DataRow row = dt.NewRow();
        int i = 0;
        try
        {
            foreach (DataRow r in dt.Rows)
            {
                i = Convert.ToInt32(r[0]);
            }
        }
        catch (Exception) { }
        i++;
        row[0] = i;
        row[1] = txtFolio.Text.PadLeft(12, '0');
        dt.Rows.Add(row);
        Session["folios"] = dt;
        GridView1.DataSource = dt;
        GridView1.DataBind();
        txtFolio.Text = "";
    }
}