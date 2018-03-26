using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using E_Utilities;
using TimbradoRV;
using System.IO;
using System.Drawing;
using MessagingToolkit.QRCode.Codec;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;



public partial class FacturacionGral : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();
    public DataTable dt;
    private bool bolErrConcpto = false;
    private int pasos;
    string status;
    int idReceptor_Elegido;
    decimal totalLinea;
    protected void Page_Load(object sender, EventArgs e)
    {
        obtieneSesiones();

        if (!IsPostBack)
        {
            if (Convert.ToInt32(Request.QueryString["fact"]) == 0)
            {
                ddlMetodoPagoSAT.SelectedValue = "PUE";
            }
            lblIdEmisor.Text = lblEmisorFacturas.Text = "1";
            ddlMonedaSAT.SelectedValue = "MXN";
            //string idRecep = obtieneIdReceptor(Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"]);
            /*Alx: Carga datos de venta_det*/
            if(lblReceptorFactura.Text == "")
            {
                lblReceptorFactura.Text = "0";
            }
            lblAbreviatura.Text = "MXN";
            lblModo.Text = "C";
            string ticket = string.Empty;
            try
            {
                ticket = Request.QueryString["tck"].ToString();
            }
            catch { }
            if (!string.IsNullOrEmpty(ticket))
            {
                lblReceptorFactura.Text = Request.QueryString["c"];
                string[] valoresTicket = ticket.Split(new char[] { ';' });
                if (valoresTicket.Length == 1)
                    cargaDatosTicket(int.Parse(ticket), int.Parse(Request.QueryString["p"].ToString()));
                else
                    cargaDatosTickets(valoresTicket, Convert.ToInt32(Request.QueryString["p"]));

            }


            lblError.Text = "";
            grdEmisores.SelectedIndex = 0;
            //PopUpEmisores
            cargaDatos();
            llenaInfoEncFactura();
            //grdReceptores.SelectedIndex = 0;

            //cargaDatosRecep();
            /*
            com.formulasistemas.www.ManejadordeTimbres folios = new com.formulasistemas.www.ManejadordeTimbres();
            int foliosDisponibles = folios.ObtieneFoliosDisponibles("CBM140917L5A");
            lblError.Text = foliosDisponibles.ToString() + " Folios Disponibles";
            */
            status = "P";

            dt = new DataTable();

            dt.Columns.Add(new DataColumn("Concepto", typeof(string)));
            dt.Columns.Add(new DataColumn("Importe", typeof(string)));
            dt.Columns.Add(new DataColumn("SubTotal", typeof(string)));
            dt.Columns.Add(new DataColumn("Imp. Tras.", typeof(string)));
            dt.Columns.Add(new DataColumn("Imp. Ret.", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(string)));
            dt.Columns.Add(new DataColumn("Select", typeof(string)));

            dt = AddRow(dt);

            //genera n filas
            DataRow dr = default(DataRow);
            for (int i = 1; i <= 2; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            }

            if (Convert.ToInt32(Request.QueryString["fact"]) == 0)
            {
                 
                txtCondicionesPago.Text = "CONTADO";
                txtCtaPago.Text = "";
                txtTipoCambio.Text = "1.00";
                string notasFact = "";
                string referenciasFact = "";

                txtNotaFac.Text = notasFact.Trim();

                int ticketF = 0;
                try { ticketF = Convert.ToInt32(Request.QueryString["tck"].ToString()); } catch (Exception) { ticketF = 0; }
                if (ticketF != 0)
                    txtReferenciasFac.Text = "T" + Request.QueryString["p"].ToString() + "-Tk" + Request.QueryString["tck"].ToString();
                else
                    txtReferenciasFac.Text = referenciasFact;
                lnkTimbrar.Visible = false;

            }
            else
            {
                if (pasos == 0)
                {
                    cargaDatosFacturaPrevia(Convert.ToInt32(Request.QueryString["fact"]));
                    llenaInfoEncFactura();
                    lnkTimbrar.Visible = true;    
                }
            }


            if (status != "P")
            {
                lnkBuscar.Visible = false;
                lnkBuscaRec.Visible = false;
                lnkBuscaMonedas.Visible = false;
                multiPagina.PageViews[3].Enabled = multiPagina.PageViews[4].Enabled = false;
                fvwResumen.Enabled = false;
                ddlFormaPagoSAT.Enabled = txtCondicionesPago.Enabled = ddlMetodoPagoSAT.Enabled = ddlRegimenSAT.Enabled = txtCtaPago.Enabled = true;
                lnkTimbrar.Visible = false;
                
            }

        }
    }

    private void cargaDatosTickets(string[] valoresTicket, int idPunto)
    {
        int IDEmisor = Convert.ToInt32(lblEmisorFacturas.Text);
        int IdRecep = Convert.ToInt32(lblReceptorFactura.Text);
        List<VentaDet> arts = new List<VentaDet>();
        object[] datosVta = VentaDet.datosVentaM(valoresTicket, idPunto, idPunto);
        if (!Convert.ToBoolean(datosVta[0]))
            lblMnsjs.Text = "Error: " + datosVta[1].ToString();
        else
        {
            int i = 1;
            dt = ((DataSet)datosVta[1]).Tables[0];
            foreach (DataRow rd in dt.Rows)
            {
                rd[0] = i;
                i++;
            }

            bool esOk = true;
            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString))
            {
                try
                {
                    conLoc.Open();
                    string qryBorra = "DELETE FROM DocumentoCfdi_f WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;
                    SqlCommand comLoc = new SqlCommand(qryBorra, conLoc);
                    comLoc.ExecuteNonQuery();

                    if (Convert.ToBoolean(dt.Rows[0][9]))
                    {
                        decimal TporcDctoGlob = 0, TvalUnit = 0, Timpte = 0, TtotDcto = 0, Tsubtotal = 0, Tiva = 0, TTotal = 0;
                        foreach (DataRow r in dt.Rows)
                        {
                            decimal porcDctoGlob = Convert.ToDecimal(r[8].ToString());
                            decimal cant = Convert.ToDecimal(r[3]);
                            decimal valUnit = 0.00M;
                            decimal impte = 0.00M;
                            decimal totDcto = 0.00M;
                            decimal subTot = 0.00M;
                            decimal iva16 = 0.00M;
                            decimal Total = 0.00M;

                            /*if (r[12].ToString() != "0.00")
                            {
                                valUnit = Convert.ToDecimal(r[4]);
                                impte = valUnit * cant;
                                totDcto = (valUnit * (Convert.ToDecimal(r[6]) / 100)) * cant; //   Convert.ToDecimal(r[7]) * cant;
                                subTot = impte - totDcto; //Convert.ToDecimal(r[5]);
                                if (porcDctoGlob > 0)
                                    subTot = subTot - ((subTot * porcDctoGlob) / 100);
                                iva16 = subTot;
                                Total = subTot;
                                valUnit = Convert.ToDecimal(valUnit.ToString("F2"));
                                TporcDctoGlob = TporcDctoGlob + porcDctoGlob;
                                TvalUnit = TvalUnit + valUnit;
                                Timpte = Timpte + impte;
                                TtotDcto = TtotDcto + totDcto;
                                Tsubtotal = Tsubtotal + subTot;
                                Tiva = Tiva + iva16;
                                TTotal = TTotal + Total;

                            }
                            else
                            {
                            */
                                valUnit = Convert.ToDecimal(r[4]) / Convert.ToDecimal(1.16);
                                impte = valUnit * cant;
                                totDcto = (valUnit * (Convert.ToDecimal(r[6]) / 100)) * cant; //   Convert.ToDecimal(r[7]) * cant;
                                subTot = impte - totDcto; //Convert.ToDecimal(r[5]);
                                if (porcDctoGlob > 0)
                                    subTot = subTot - ((subTot * porcDctoGlob) / 100);
                                iva16 = subTot * 0.16M;
                                Total = subTot + iva16;
                                valUnit = Convert.ToDecimal(valUnit.ToString("F2"));
                                TporcDctoGlob = TporcDctoGlob + porcDctoGlob;
                                TvalUnit = TvalUnit + valUnit;
                                Timpte = Timpte + impte;
                                TtotDcto = TtotDcto + totDcto;
                                Tsubtotal = Tsubtotal + subTot;
                                Tiva = Tiva + iva16;
                                TTotal = TTotal + Total;
                            //}


                            int clavesat;
                            clavesat = Convert.ToInt32(r[10]);
                            string unidadsat = Convert.ToString(r[11]);

                            string qryInserta = "INSERT INTO DocumentoCfdi_f (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera,ddlClaveProdSat,ddlClaveUnidadSat) " +
                                "VALUES (1,'" + IDEmisor + "' , '" + IdRecep + "', 'VPV1', 'Venta Productos Varios', 1, 1, " + Math.Round(TvalUnit, 2) + ", " + Math.Round(Timpte, 2) + ", 0, " + Math.Round(TtotDcto, 2) + ", " + Math.Round(Tsubtotal, 2) + "," + 2 + ", " + 5 + ", " + Math.Round(Tiva, 2) + ", " + 0 + ", " + 1 + ", " + 2 + ", " + 0 + ", " + 0 + ", " + Math.Round(TTotal, 2) + ", convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120))," + clavesat + ",'" + unidadsat + "')";
                            comLoc.CommandText = qryInserta;
                            using (comLoc)
                            {
                                comLoc.CommandText = qryInserta;
                                int ok = comLoc.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {

                        decimal porcDctoGlob = Convert.ToDecimal(dt.Rows[0][8].ToString());
                        foreach (DataRow r in dt.Rows)
                        {
                            decimal valUnit = Convert.ToDecimal(r[4]) / Convert.ToDecimal(1.16);
                            decimal cant = Convert.ToDecimal(r[3]);
                            decimal impte = valUnit * cant;
                            decimal totDcto = (valUnit * (Convert.ToDecimal(r[6]) / 100)) * cant; //   Convert.ToDecimal(r[7]) * cant;
                            decimal subTot = impte - totDcto; //Convert.ToDecimal(r[5]);
                            if (porcDctoGlob > 0)
                                subTot = subTot - ((subTot * porcDctoGlob) / 100);
                            decimal iva16 = subTot * 0.16M;
                            decimal Total = subTot + iva16;
                            valUnit = Convert.ToDecimal(valUnit.ToString("F2"));
                            string clavesat = r[11].ToString();
                            string unidadsat = r[12].ToString();


                            string qryInserta = "INSERT INTO DocumentoCfdi_f (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera, ddlClaveProdSat, ddlClaveUnidadSat) " +
                                "VALUES (" + r[0] + ",'" + IDEmisor + "' , '" + IdRecep + "', '" + r[1].ToString() + "', '" + r[2].ToString() + "', " + r[3].ToString() + ", " + 1 + ", " + Math.Round(valUnit, 2) + ", " + Math.Round(impte, 2) + ", " + r[6].ToString() + ", " + Math.Round(totDcto, 2) + ", " + Math.Round(subTot, 2) + "," + 2 + ", " + 5 + ", " + Math.Round(iva16, 2) + ", " + 0 + ", " + 1 + ", " + 2 + ", " + 0 + ", " + 0 + ", " + Math.Round(Total, 2) + ", convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120),'" + clavesat + "','" + unidadsat + "')";
                            comLoc.CommandText = qryInserta;
                            using (comLoc)
                            {
                                comLoc.CommandText = qryInserta;
                                int ok = comLoc.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    esOk = false;
                    lblMnsjs.Text = "Error LocalDB insersion detalle: " + ex.Source + " - " + ex.Message;
                }
                finally { conLoc.Close(); }
            }
            if (esOk)
            {
                ((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text = dt.Rows[0][8].ToString();
                //txtPctjeDsctoGlb.Text = dt.Rows[0][8].ToString();
                PctjeDsctoGlb = Convert.ToDecimal(dt.Rows[0][8].ToString());
                LlenaInfoDetalle(IDEmisor, IdRecep);
            }
        }
    }

    private string obtieneIdReceptor(string empresa, string taller, string orden)
    {
        string id = "0";
        try
        {
            object[] datosOrden = recepciones.obtieneInfoOrden(Convert.ToInt32(orden), Convert.ToInt32(empresa), Convert.ToInt32(taller));
            if (Convert.ToBoolean(datosOrden[0]))
            {
                DataSet infoOrd = (DataSet)datosOrden[1];
                int idCliprov = 0;
                foreach (DataRow fila in infoOrd.Tables[0].Rows)
                {
                    idCliprov = Convert.ToInt32(fila[5].ToString());
                }
                CatClientes catClientes = new CatClientes();
                object[] datosCliente = catClientes.obtieneRfc(idCliprov);
                if (Convert.ToBoolean(datosCliente[0]))
                {
                    string rfc = Convert.ToString(datosCliente[1]);
                    FacturacionElectronica.Receptores receptor = new FacturacionElectronica.Receptores();
                    receptor.obtieneIdReceptor(rfc);
                    object[] infoReceptor = receptor.info;
                    if (Convert.ToBoolean(infoReceptor[0]))
                        id = Convert.ToString(infoReceptor[1]);
                    else
                        id = "0";
                }
                else
                    id = "0";
            }
            else
                id = "0";
        }
        catch (Exception ex) { id = "0"; }
        return id;
    }

    //Metodos Generales    
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
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "FacturacionGral.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }


    // Ventana Emisores
    private void cargaDatos()
    {
        lblErrorEmisores.Text = "";
        string sql = "select IdEmisor,EmRFC,EmNombre from emisores_f ";
        string condicion = "";
        if (txtBusqueda.Text != "")
            condicion = " where EmRFC like '%" + txtBusqueda.Text.Trim() + "%' or EmNombre like '%" + txtBusqueda.Text.Trim() + "%'";
        sql = sql + condicion;
        SqlDataSource1.SelectCommand = sql.Trim();
        grdEmisores.DataBind();
        cargaInfoEmisor();
    }

    protected void lnkBusqueda_Click(object sender, EventArgs e)
    {
        grdEmisores.SelectedIndex = 0;
        cargaDatos();
    }

    private void cargaInfoEmisor()
    {
        Label[] etiquetas = { lblEmiNomCto, lblCalleEmi, lblNoExtEmi, lblNoIntEmi,lblColoniaEmi,lblDelMunEmi,lblEdoEmi,lblPaisEmi,
                                            lblCpEmi,lblLocalidadEmi,lblReferenciaEmi,lblCalleEx,lblNoExtEx,lblNoIntEx,lblColoniaEx,lblDelMunEx,
                                            lblEdoEx, lblPaisEx, lblCpEx, lblLocalidadEx, lblReferenciaEx };
        try
        {
            FacturacionElectronica.Emisores Emisor = new FacturacionElectronica.Emisores();
            Emisor.idEmisor = Convert.ToInt32(grdEmisores.DataKeys[grdEmisores.SelectedIndex].Value.ToString());
            Emisor.obtieneInfoEmisor();
            object[] info = Emisor.info;
            if (Convert.ToBoolean(info[0]))
            {
                DataSet valores = (DataSet)info[1];
                if (valores.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow fila in valores.Tables[0].Rows)
                    {
                        for (int i = 0; i < fila.ItemArray.Length; i++)
                        {
                            if (fila[i].ToString() == "")
                                fila[i] = "...";
                        }
                        lblIdEmisor.Text = fila[0].ToString();
                        etiquetas[0].Text = fila[46].ToString();
                        etiquetas[1].Text = fila[3].ToString();
                        etiquetas[2].Text = fila[4].ToString();
                        etiquetas[3].Text = fila[5].ToString();
                        etiquetas[4].Text = fila[13].ToString();
                        etiquetas[5].Text = fila[11].ToString();
                        etiquetas[6].Text = fila[9].ToString();
                        etiquetas[7].Text = fila[7].ToString();
                        etiquetas[8].Text = fila[14].ToString();
                        etiquetas[9].Text = fila[15].ToString();
                        etiquetas[10].Text = fila[16].ToString();
                        etiquetas[11].Text = fila[17].ToString();
                        etiquetas[12].Text = fila[18].ToString();
                        etiquetas[13].Text = fila[19].ToString();
                        etiquetas[14].Text = fila[27].ToString();
                        etiquetas[15].Text = fila[25].ToString();
                        etiquetas[16].Text = fila[23].ToString();
                        etiquetas[17].Text = fila[21].ToString();
                        etiquetas[18].Text = fila[28].ToString();
                        etiquetas[19].Text = fila[29].ToString();
                        etiquetas[20].Text = fila[30].ToString();
                    }
                }
                else
                    lblErrorEmisores.Text = "No se encontró ningún emisor";
            }
            else
                lblErrorEmisores.Text = "Error: " + Convert.ToString(info[1]);
        }
        catch (Exception ex)
        {
            lblErrorEmisores.Text = "Error: " + ex.Message;
        }
    }

    protected void lnkLimpiar_Click(object sender, EventArgs e)
    {
        txtBusqueda.Text = "";
        cargaDatos();
    }

    protected void grdEmisores_PageIndexChanged(object sender, EventArgs e)
    {
        grdEmisores.SelectedIndex = 0;
        cargaDatos();
    }

    protected void grdEmisores_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargaDatos();
    }

    protected void lnkSeleccionarEmisor_Click(object sender, EventArgs e)
    {
        lblEmisorFacturas.Text = lblIdEmisor.Text;
        llenaInfoEncFactura();
        string script = "cierraWinEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

    //Ventana Receptores
    private void cargaDatosRecep()
    {
        lblErrorRec.Text = "";
        string sql = "select IdRecep,ReRFC,ReNombre from receptores_f ";
        string condicion = "";
        /*if (txtBuscarRec.Text != "")
            condicion = " where ReRFC like '%" + txtBuscarRec.Text.Trim() + "%' or ReNombre like '%" + txtBuscarRec.Text.Trim() + "%'";*/
        sql = sql + condicion;
        FacturacionElectronica.Ejecucion ejecutaFact = new FacturacionElectronica.Ejecucion();
        ejecutaFact.baseDatos = "PVW";

        object[] info = ejecutaFact.dataSet(sql);
        if (Convert.ToBoolean(info[0]))
        {
            DataSet infoRecp = (DataSet)info[1];
            grdReceptores.DataSource = infoRecp;
        }
        else
            grdReceptores.DataSource = null;
        grdReceptores.DataBind();
        cargaInfoReceptor();
    }

    private void cargaInfoReceptor()
    {
        Label[] etiquetas = { lblCalleRec, lblNoExtRec, lblNoIntRec,lblColoniaRec,lblDelMunRec,lblEdoRec,lblPaisRec,
                              lblCpRec,lblLocalidadRec,lblReferenciaRec,lblCorreoRec,lblCorreoCCRec, lblCorreoCCORec};
        try
        {
            FacturacionElectronica.Receptores Receptor = new FacturacionElectronica.Receptores();
            if (lblReceptorFactura.Text != "0" && lblReceptorFactura.Text == grdReceptores.SelectedValues["IdRecep"].ToString()) //grdReceptores.DataKeys[grdReceptores.SelectedIndex].Value.ToString())
                Receptor.idReceptor = Convert.ToInt32(lblReceptorFactura.Text);
            else
                Receptor.idReceptor = Convert.ToInt32(grdReceptores.SelectedValues["IdRecep"].ToString());//.DataKeys[grdReceptores.SelectedIndex].Value.ToString());
            Receptor.obtieneInfoReceptor();
            object[] info = Receptor.info;
            if (Convert.ToBoolean(info[0]))
            {
                DataSet valores = (DataSet)info[1];
                if (valores.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow fila in valores.Tables[0].Rows)
                    {
                        for (int i = 0; i < fila.ItemArray.Length; i++)
                        {
                            if (fila[i].ToString() == "")
                                fila[i] = "...";
                        }
                        lblIdReceptor.Text = fila[0].ToString();
                        txtRfc.Text = fila[1].ToString();
                        if (txtRfc.Text.Trim().Length == 12)
                            rbtnPersona.SelectedValue = "M";
                        else
                            rbtnPersona.SelectedValue = "F";

                        if (rbtnPersona.SelectedValue == "M")
                            txtRfc.MaxLength = 12;
                        else
                            txtRfc.MaxLength = 13;

                        txtRazonNew.Text = fila[2].ToString();
                        ddlPais.SelectedValue = fila[6].ToString();
                        ddlEstado.SelectedValue = fila[8].ToString();
                        ddlMunicipio.SelectedValue = fila[10].ToString();
                        ddlColonia.SelectedValue = fila[12].ToString();

//                        ddlCodigo.SelectedValue = fila[14].ToString();
                       


                        etiquetas[0].Text = txtCalle.Text = fila[3].ToString(); //Calle
                        etiquetas[1].Text = txtNoExt.Text = fila[4].ToString(); //Exterior
                        etiquetas[2].Text = txtNoIntMod.Text = fila[5].ToString(); //Interior
                        etiquetas[3].Text = fila[13].ToString(); //Colonia
                        etiquetas[4].Text = fila[11].ToString(); //Delegacion
                        etiquetas[5].Text = fila[9].ToString(); //Estado
                        etiquetas[6].Text = fila[5].ToString(); //Pais
                        etiquetas[7].Text = fila[14].ToString(); //CP
                        etiquetas[8].Text = txtLocalidad.Text = fila[15].ToString(); //Localidades
                        etiquetas[9].Text = txtReferenciaMod.Text = fila[16].ToString(); //Referencia
                        etiquetas[10].Text = txtCorreo.Text = fila[17].ToString(); //Correo
                        etiquetas[11].Text = txtCorreoCC.Text = fila[18].ToString(); //CorreoCC
                        etiquetas[12].Text = txtCorreoCCO.Text = fila[19].ToString(); //CorreoCCO
                    }
                }
                else
                    lblErrorRec.Text = "No se encontró ningún receptor";
            }
            else
                lblErrorRec.Text = "Error: " + Convert.ToString(info[1]);
        }
        catch (Exception ex)
        {
            lblErrorRec.Text = "Error: " + ex.Message;
        }
    }

    protected void lnkBusquedaRec_Click(object sender, EventArgs e)
    {
        //grdReceptores.SelectedIndex = 0;
        //cargaDatosRecep();        
    }

    protected void lnkLimpiarRec_Click(object sender, EventArgs e)
    {
        //txtBuscarRec.Text = "";
        //cargaDatosRecep();        
    }

    protected void grdReceptores_PageIndexChanged(object sender, EventArgs e)
    {
        //grdReceptores.SelectedIndex = 0;
        // cargaDatosRecep();        
    }

    protected void grdReceptores_SelectedIndexChanged(object sender, EventArgs e)
    {
        //cargaDatosRecep();
    }

    protected void lnkSeleccionarReceptor_Click(object sender, EventArgs e)
    {
        this.idReceptor_Elegido = Convert.ToInt32(lblIdReceptor.Text);
        //dibujaReceptorSeleccionado();
        lblReceptorFactura.Text = lblIdReceptor.Text;
        llenaInfoEncFactura();// linea original


        string script = "cierraWinRec();";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

    // Alfredo 29jun2017- 17:28pm
    //  /////////////////////////////////////////////////////////////////////////////////////////
    private void dibujaReceptorSeleccionado()
    {
        //this.idReceptor_Elegido = 0;

        // guardo lo que trae el Label, si es diferente a 0
        if (Convert.ToInt32(this.idReceptor_Elegido) > 0)
        {
            this.idReceptor_Elegido = Convert.ToInt32(lblIdReceptor.Text);

            // Creamos un objeto receptores para que por medio del metodo " obtieneUnReceptorXElidReceptor "
            // obtengamos un data set con la informacion a desplegar | dibujar
            FacturacionElectronica.Receptores idRecElegido = new FacturacionElectronica.Receptores();

            idRecElegido.idReceptorElegido = this.idReceptor_Elegido;
            idRecElegido.obtieneInfoReceptor1();
            object[] infoRecElegido = idRecElegido.info;
            if (Convert.ToBoolean(infoRecElegido[0]))
            {
                // ok tengo un dataset, debo de crear un objeto para vaciar los datos
                //Label[] etiquetasRecElegido = { lblRfcReceptor, lblNombreReceptor, lblCalleRecFac, lblNoExtRecFac, lblNoIntRecFac, lblColoniaRecFac,
                //                     lblDelMunRecFac, lblEdoRecFac, lblPaisRecFac, lblCpRecFac, lblLocalidadRecFac, lblReferenciaRecFac };

                Label[] etiquetasRecElegido = {lblReceptorFactura, lblRfcReceptor, lblNombreReceptor, lblCalleRecFac, lblNoExtRecFac, lblNoIntRecFac, lblColoniaRecFac,
                                                lblDelMunRecFac, lblEdoRecFac, lblPaisRecFac, lblCpRecFac, lblLocalidadRecFac, lblReferenciaRecFac };

                DataSet valoresReceptorElegido = (DataSet)infoRecElegido[1];
                if (valoresReceptorElegido.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow fila in valoresReceptorElegido.Tables[0].Rows)
                    {
                        for (int i = 0; i < fila.ItemArray.Length; i++)
                        {
                            if (fila[i].ToString() == "")
                                fila[i] = "...";
                        }

                        etiquetasRecElegido[0].Text = fila[2].ToString(); //Receptor
                        etiquetasRecElegido[1].Text = fila[1].ToString(); //RFC
                        etiquetasRecElegido[2].Text = fila[2].ToString(); //Nombre
                        etiquetasRecElegido[3].Text = fila[3].ToString(); //Calle
                        etiquetasRecElegido[4].Text = fila[4].ToString(); //NoExterior
                        etiquetasRecElegido[5].Text = fila[5].ToString(); //Nointerior
                        etiquetasRecElegido[6].Text = fila[13].ToString(); //Colonia
                        etiquetasRecElegido[7].Text = fila[11].ToString(); //Delegacion
                        etiquetasRecElegido[8].Text = fila[9].ToString(); //Estado
                        etiquetasRecElegido[9].Text = fila[5].ToString(); //Pais
                        etiquetasRecElegido[10].Text = fila[14].ToString(); //CP
                        etiquetasRecElegido[11].Text =  fila[15].ToString(); //Localidad
                        etiquetasRecElegido[12].Text =  fila[16].ToString(); //Referencias
                    }
                }
                else
                {
                    lblError.Text = "No se ha indicado un receptor para facturar";
                }

            }
        }



        if (this.idReceptor_Elegido != 0)
        {

        }


    }

    //  /////////////////////////////////////////////////////////////////////////////////////////

    // Datos de Factura
    private void llenaInfoEncFactura()
    {
        lblError.Text = "";
        try
        {
            int idEmisor = Convert.ToInt32(lblIdEmisor.Text);
            int idReceptor = 0;
            try { idReceptor = Convert.ToInt32(lblReceptorFactura.Text); }
            catch (Exception) { idReceptor = 0; }
            Label[] etiquetas = { lblRfcEmisor, lblRazonEmisor, lblRFCEmiExFac, lblNombreEmiExFac, lblCalleEmiFac, lblNoExtEmiFac, lblNoIntEmiFac, lblColoniaEmiFac, lblDelMunEmiFac, lblEdoEmiFac, lblPaisEmiFac, lblCpEmiFac, lblLocalidadEmiFac, lblReferenciaEmiFac, lblCalleEmiExFac, lblNoExtEmiExFac, lblNoIntEmiExFac, lblColoniaEmiExFac, lblDelMunEmiExFac, lblEdoEmiExFac, lblPaisEmiExFac, lblCpEmiExFac, lblLocalidadEmiExFac, lblReferenciaEmiExFac };
            Label[] etiquetasRec = { lblRfcReceptor, lblNombreReceptor, lblCalleRecFac, lblNoExtRecFac, lblNoIntRecFac, lblColoniaRecFac, lblDelMunRecFac, lblEdoRecFac, lblPaisRecFac, lblCpRecFac, lblLocalidadRecFac, lblReferenciaRecFac };
            try
            {
                //Emisor
                FacturacionElectronica.Emisores Emisor = new FacturacionElectronica.Emisores();
                Emisor.idEmisor = idEmisor;
                Emisor.obtieneInfoEmisor();
                object[] info = Emisor.info;
                if (Convert.ToBoolean(info[0]))
                {
                    DataSet valores = (DataSet)info[1];
                    if (valores.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow fila in valores.Tables[0].Rows)
                        {
                            for (int i = 0; i < fila.ItemArray.Length; i++)
                            {
                                if (fila[i].ToString() == "")
                                    fila[i] = "...";
                            }

                            etiquetas[0].Text = etiquetas[2].Text = fila[1].ToString();
                            etiquetas[1].Text = etiquetas[3].Text = fila[2].ToString();
                            etiquetas[4].Text = fila[3].ToString();
                            etiquetas[5].Text = fila[4].ToString();
                            etiquetas[6].Text = fila[5].ToString();
                            etiquetas[7].Text = fila[13].ToString();
                            etiquetas[8].Text = fila[11].ToString();
                            etiquetas[9].Text = fila[9].ToString();
                            etiquetas[10].Text = fila[7].ToString();
                            etiquetas[11].Text = fila[14].ToString();
                            etiquetas[12].Text = fila[15].ToString();
                            etiquetas[13].Text = fila[16].ToString();
                            etiquetas[14].Text = fila[17].ToString();
                            etiquetas[15].Text = fila[18].ToString();
                            etiquetas[16].Text = fila[19].ToString();
                            etiquetas[17].Text = fila[27].ToString();
                            etiquetas[18].Text = fila[25].ToString();
                            etiquetas[19].Text = fila[23].ToString();
                            etiquetas[20].Text = fila[21].ToString();
                            etiquetas[21].Text = fila[28].ToString();
                            etiquetas[22].Text = fila[29].ToString();
                            etiquetas[23].Text = fila[30].ToString();
                        }
                    }
                    else
                        lblError.Text = "No se ha indicado un emisor para facturar";
                }
                else
                    lblError.Text = "Error Emisor: " + Convert.ToString(info[1]);

                //Receptor
                FacturacionElectronica.Receptores Receptor = new FacturacionElectronica.Receptores();
                if (this.idReceptor_Elegido > 0)
                {
                    Receptor.idReceptor = this.idReceptor_Elegido;
                }
                else
                {
                    //return;
                }

                Receptor.idReceptor = idReceptor;
                Receptor.obtieneInfoReceptor();
                object[] infoRec = Receptor.info;
                if (Convert.ToBoolean(infoRec[0]))
                {
                    DataSet valoresRec = (DataSet)infoRec[1];
                    if (valoresRec.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow fila in valoresRec.Tables[0].Rows)
                        {
                            for (int i = 0; i < fila.ItemArray.Length; i++)
                            {
                                if (fila[i].ToString() == "")
                                    fila[i] = "...";
                            }
                            etiquetasRec[0].Text = fila[1].ToString();
                            etiquetasRec[1].Text = fila[2].ToString();
                            etiquetasRec[2].Text = fila[3].ToString();
                            etiquetasRec[3].Text = fila[4].ToString();
                            etiquetasRec[4].Text = fila[5].ToString();
                            etiquetasRec[5].Text = fila[13].ToString();
                            etiquetasRec[6].Text = fila[11].ToString();
                            etiquetasRec[7].Text = fila[9].ToString();
                            etiquetasRec[8].Text = fila[7].ToString();
                            etiquetasRec[9].Text = fila[14].ToString();
                            etiquetasRec[10].Text = fila[15].ToString();
                            etiquetasRec[11].Text = fila[16].ToString();
                        }
                    }
                    else
                        lblError.Text = "No se ha indicado un receptor para facturar";
                }
                else
                    lblError.Text = "Error Receptor: " + Convert.ToString(infoRec[1]);

            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
            }
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    //Conceptos
    private DataTable AddRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();
        
        dr["Concepto"] = "";
        dr["Importe"] = "";
        dr["SubTotal"] = "";
        dr["Imp. Tras."] = "";
        dr["Imp. Ret."] = "";
        dr["Total"] = "";
        dr["Select"] = "";
        dt.Rows.Add(dr);
        return dt;
    }

    protected void btnAgrFila_Click1(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dt"];
        //grdDocu.Rebind();
    }

    protected void grdDocu_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!IsPostBack)
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("IdFila", typeof(string)));
            dt.Columns.Add(new DataColumn("Concepto", typeof(string)));
            dt.Columns.Add(new DataColumn("Importe", typeof(string)));
            dt.Columns.Add(new DataColumn("SubTotal", typeof(string)));
            dt.Columns.Add(new DataColumn("Imp. Tras.", typeof(string)));
            dt.Columns.Add(new DataColumn("Imp. Ret.", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(string)));
            dt.Columns.Add(new DataColumn("Select", typeof(string)));

            dt = AddRow(dt);
            ViewState["dt"] = dt;
            grdDocu.DataSource = dt;
            Session["info"] = dt;
        }
        else if (!bolErrConcpto)
        {
            try { dt = (DataTable)Session["info"]; }
            catch (Exception) { dt = null; }
            if (dt != null)
            {
                DataRow dr = default(DataRow);
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                ViewState["dt"] = dt;
                grdDocu.DataSource = dt;
                Session["info"] = dt;
            }
        }
    }

    protected void grdDocu_PreRender(object sender, EventArgs e)
    {
        //GridTableView masterTable = (sender as RadGrid).MasterTableView; // comentado AGD
        //GridColumn ConceptoColumn = masterTable.GetColumnSafe("TemplateColumn2") as GridColumn; // comentado AGD
        //TextBox txtIdent = masterTable.GetBatchColumnEditor("txtIdent") as TextBox;
        //TextBox txtConcepto = masterTable.GetBatchColumnEditor("txtConcepto") as TextBox;
        //RadNumericTextBox radnumCantidad = masterTable.GetBatchColumnEditor("radnumCantidad") as RadNumericTextBox;
        //DropDownList ddlUnidad = masterTable.GetBatchColumnEditor("ddlUnidad") as DropDownList;
    }

    protected void grdDocu_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (status != "P")
        {
            Dictionary<string, string>[] valores = new Dictionary<string, string>[]
            {

            };
            int IDEmisor = Convert.ToInt32(lblEmisorFacturas.Text);
            int IdRecep = Convert.ToInt32(lblReceptorFactura.Text);
            string IdctrlPostBack = getPostBackControlName();
            string ctrlPostBack = "";
            if (!string.IsNullOrEmpty(IdctrlPostBack))
                ctrlPostBack = IdctrlPostBack.Substring(IdctrlPostBack.LastIndexOf('$') + 1);
            int noFilas = grdDocu.MasterTableView.Items.Count;
            GridDataItem ultFila = grdDocu.MasterTableView.Items[noFilas - 1];
            string strIdentConcpto = ((TextBox)ultFila.FindControl("txtIdent")).Text.Trim();
            string strConcepto = ((TextBox)ultFila.FindControl("txtConcepto")).Text.Trim();

            if (e.CommandName.Equals("InitInsert") && (!string.IsNullOrEmpty(strIdentConcpto) || !string.IsNullOrEmpty(strConcepto)))
            {
                string command = e.CommandName;
                string args = e.CommandArgument.ToString();
                if (status != "T" && status != "C")
                {
                    using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString))
                    {
                        try
                        {
                            conLoc.Open();
                            string qryBorra = "DELETE FROM DocumentoCfdi_f WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;
                            string qryInserta = "INSERT INTO DocumentoCfdi_f (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera,ddlClaveProdSAT,ddlClaveUnidadSAT) " +
                                "VALUES (@IdFila, @IdEmisor, @IdRecep, @txtIdent, @txtConcepto, @radnumCantidad, @ddlUnidad, @txtValUnit, @lblImporte, @txtPtjeDscto, @txtDscto, @lblSubTotal, @ddlIvaTras, @ddlIeps, @lblIvaTras, @lblIeps, @ddlIvaRet, @ddlIsrRet, @lblIvaRet, @lblIsrRet, @lblTotal, @EncFechaGenera,@ddlClaveProdSAT,@ddlClaveUnidadSAT)";
                             /*
                            string qryInserta = "INSERT INTO DocumentoCfdi_f (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera) " +
                                "VALUES (@IdFila, @IdEmisor, @IdRecep, @txtIdent, @txtConcepto, @radnumCantidad, @ddlUnidad, @txtValUnit, @lblImporte, @txtPtjeDscto, @txtDscto, @lblSubTotal, @ddlIvaTras, @ddlIeps, @lblIvaTras, @lblIeps, @ddlIvaRet, @ddlIsrRet, @lblIvaRet, @lblIsrRet, @lblTotal, @EncFechaGenera)";
                             * */
                            SqlCommand comLoc = new SqlCommand(qryBorra, conLoc);
                            using (comLoc)
                            {

                                int filasElim = comLoc.ExecuteNonQuery();
                                comLoc.CommandText = qryInserta;
                                comLoc.Parameters.Add("IdFila", SqlDbType.SmallInt).Direction = ParameterDirection.Input;
                                comLoc.Parameters.Add("IdEmisor", SqlDbType.SmallInt).Direction = ParameterDirection.Input;
                                comLoc.Parameters.Add("IdRecep", SqlDbType.SmallInt).DbType = DbType.Int16;
                                comLoc.Parameters.Add("txtIdent", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("txtConcepto", SqlDbType.NVarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("radnumCantidad", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("ddlUnidad", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("txtValUnit", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("lblImporte", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("txtPtjeDscto", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("txtDscto", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("lblSubTotal", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("ddlIvaTras", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("ddlIeps", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("lblIvaTras", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("lblIeps", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("ddlIvaRet", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("ddlIsrRet", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("lblIvaRet", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("lblIsrRet", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.Add("lblTotal", SqlDbType.VarChar).DbType = DbType.String;
                                comLoc.Parameters.AddWithValue("EncFechaGenera", fechas.obtieneFechaLocal()).DbType = DbType.DateTime;
                                comLoc.Parameters.Add("ddlClaveProdSAT", SqlDbType.Int).DbType = DbType.Int32;
                                comLoc.Parameters.Add("ddlClaveUnidadSAT", SqlDbType.VarChar).DbType = DbType.String;
                                //comLoc.Parameters.AddWithValue("EncFechaGenera", Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss"))).DbType = DbType.DateTime;

                                foreach (GridDataItem fila in grdDocu.Items)
                                {

                                    int IdFila = fila.ItemIndex;
                                    string txtIdent = ((TextBox)fila.FindControl("txtIdent")).Text;
                                    string txtConcepto = ((TextBox)fila.FindControl("txtConcepto")).Text.Trim();
                                    string radnumCantidad = ((RadNumericTextBox)fila.FindControl("radnumCantidad")).Value.ToString();
                                    string ddlUnidad = ((DropDownList)fila.FindControl("ddlUnidad")).SelectedValue;
                                    string txtValUnit = ((TextBox)fila.FindControl("txtValUnit")).Text.Trim();
                                    string lblImporte = ((Label)fila.FindControl("lblImporte")).Text;
                                    string txtPtjeDscto = ((TextBox)fila.FindControl("txtPtjeDscto")).Text.Trim();
                                    string txtDscto = ((TextBox)fila.FindControl("txtDscto")).Text.Trim();
                                    string lblSubTotal = ((Label)fila.FindControl("lblSubTotal")).Text;
                                    string ddlIvaTras = ((RadDropDownList)fila.FindControl("ddlIvaTras")).SelectedValue;
                                    string lblIvaTras = ((Label)fila.FindControl("lblIvaTras")).Text;
                                    string ddlIeps = ((RadDropDownList)fila.FindControl("ddlIeps")).SelectedValue;
                                    string lblIeps = ((Label)fila.FindControl("lblIeps")).Text;
                                    string ddlIvaRet = ((RadDropDownList)fila.FindControl("ddlIvaRet")).SelectedValue;
                                    string lblIvaRet = ((Label)fila.FindControl("lblIvaRet")).Text;
                                    string ddlIsrRet = ((RadDropDownList)fila.FindControl("ddlIsrRet")).SelectedValue;
                                    string lblIsrRet = ((Label)fila.FindControl("lblIsrRet")).Text;
                                    string lblTotal = ((Label)fila.FindControl("lblTotalCpto")).Text;
                                    string ddlClaveProdSAT = ((DropDownList)fila.FindControl("ddlClaveProdSAT")).SelectedValue;
                                    string ddlClaveUnidadSAT = ((DropDownList)fila.FindControl("ddlClaveUnidadSAT")).SelectedValue;

                                    comLoc.Parameters["IdFila"].Value = IdFila;
                                    comLoc.Parameters["IdEmisor"].Value = IDEmisor;
                                    comLoc.Parameters["IdRecep"].Value = IdRecep;
                                    comLoc.Parameters["txtIdent"].Value = txtIdent;
                                    comLoc.Parameters["txtConcepto"].Value = txtConcepto;
                                    comLoc.Parameters["radnumCantidad"].Value = radnumCantidad;
                                    comLoc.Parameters["ddlUnidad"].Value = ddlUnidad;
                                    comLoc.Parameters["txtValUnit"].Value = txtValUnit;
                                    comLoc.Parameters["lblImporte"].Value = lblImporte;
                                    comLoc.Parameters["txtPtjeDscto"].Value = txtPtjeDscto;
                                    comLoc.Parameters["txtDscto"].Value = txtDscto;
                                    comLoc.Parameters["lblSubTotal"].Value = lblSubTotal;
                                    comLoc.Parameters["ddlIvaTras"].Value = ddlIvaTras;
                                    comLoc.Parameters["ddlIeps"].Value = ddlIeps;
                                    comLoc.Parameters["lblIvaTras"].Value = lblIvaTras;
                                    comLoc.Parameters["lblIeps"].Value = lblIeps;
                                    comLoc.Parameters["ddlIvaRet"].Value = ddlIvaRet;
                                    comLoc.Parameters["ddlIsrRet"].Value = ddlIsrRet;
                                    comLoc.Parameters["lblIvaRet"].Value = lblIvaRet;
                                    comLoc.Parameters["lblIsrRet"].Value = lblIsrRet;
                                    comLoc.Parameters["lblTotal"].Value = lblTotal;
                                    comLoc.Parameters["ddlClaveProdSAT"].Value = ddlClaveProdSAT.Trim();
                                    comLoc.Parameters["ddlClaveUnidadSAT"].Value = ddlClaveUnidadSAT;

                                    int ok = comLoc.ExecuteNonQuery();

                                }
                                dt = new DataTable();
                                dt.Columns.Add(new DataColumn("IdFila", typeof(string)));
                                dt.Columns.Add(new DataColumn("Concepto", typeof(string)));
                                dt.Columns.Add(new DataColumn("Importe", typeof(string)));
                                dt.Columns.Add(new DataColumn("SubTotal", typeof(string)));
                                dt.Columns.Add(new DataColumn("Imp. Tras.", typeof(string)));
                                dt.Columns.Add(new DataColumn("Imp. Ret.", typeof(string)));
                                dt.Columns.Add(new DataColumn("Total", typeof(string)));
                                dt.Columns.Add(new DataColumn("Select", typeof(string)));

                                dt = AddRow(dt);
                            }
                            conLoc.Close();
                        }
                        catch (Exception ex)
                        {
                            lblMnsjs.Text = "Error LocalDB insersion tmp: " + ex.Source + " - " + ex.Message;
                        }
                    }
                }
                else
                    lblMnsjs.Text = "No es posible agregar conceptos cuando la factura ya esta timbrada o cancelada";
            }
            else if (e.CommandName.Equals("InitInsert") && (string.IsNullOrEmpty(strIdentConcpto) || string.IsNullOrEmpty(strConcepto)))
            {
                e.Canceled = true;
                bolErrConcpto = true;
                string strErr = "alert('El Identificador ya la descripción del Concepto deben ser capturados.');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "AlertScritpt", strErr, true);
            }
            else if (ctrlPostBack == "SaveChangesButton")
            {
                if (status != "T" && status != "C")
                {
                    if (((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text == "" && Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text) != 0 || ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text == "" && Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text) != 0)
                    {
                        string mensaje = string.Format("alert('Debe indicar el motivo de descuento');");
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Scritpt", mensaje, true);
                    }
                    else
                    {
                        docuCfdi docCfd = new docuCfdi(int.Parse(lblEmisorFacturas.Text), int.Parse(lblReceptorFactura.Text), 1);
                        docCfd.IdMoneda = ddlMonedaSAT.Text;
                        docCfd.strEmRfc = lblRfcEmisor.Text;
                        docCfd.IdTipoDoc = 2;
                        string strReRfcNom = lblRfcReceptor.Text;
                        docCfd.strReRfc = strReRfcNom.Substring(0, 13).Trim();
                        docCfd.decEncDescGlob = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);
                        docCfd.decEncDescGlobImp = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text);
                        docCfd.decEncSubTotal = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblSubTotBru")).Text);
                        docCfd.decEncDesc = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotDscto")).Text);
                        docCfd.decEncImpTras = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblImpTras")).Text);
                        docCfd.decEncImpRet = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblImpRet")).Text);
                        docCfd.decEncTotal = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotalGral")).Text);
                        docCfd.strEncMotDesc = ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text;
                        docCfd.charEncEstatus = 'P';
                        docCfd.strEncFormaPago = ddlFormaPagoSAT.Text.ToUpper(); //Forma de Pago SAT
                        docCfd.strEncMetodoPago = ddlMetodoPagoSAT.Text.ToUpper(); //Metodo Pago SAT
                        docCfd.strEncCondicionesPago = txtCondicionesPago.Text.ToUpper();
                        docCfd.strEncRegimen = ddlRegimenSAT.Text.ToUpper(); //Regimen Fiscal SAT
                        docCfd.strEncNumCtaPago = txtCtaPago.Text.ToUpper();
                        docCfd.floEncTipoCambio = float.Parse(txtTipoCambio.Text);
                        docCfd.strEncNota = txtNotaFac.Text;
                        docCfd.idUsoCFDI = ddlUsoCFDI.SelectedValue;
                        docCfd.tipoDocumento = cmbTipoDocumento.SelectedValue;
                        string lugarExpedicion = "";

                        lugarExpedicion = lugarExpedicion.Trim() + lblCalleEmiExFac.Text.Trim().ToUpper() + " No. Ext. " + lblNoExtEmiExFac.Text.Trim().ToUpper();
                        if (lblNoIntEmiExFac.Text != "")
                            lugarExpedicion = lugarExpedicion.Trim() + " No. Int. " + lblNoIntEmiExFac.Text.Trim().ToUpper();
                        lugarExpedicion = lugarExpedicion + ", Col. " + lblColoniaEmiExFac.Text.Trim().ToUpper() + ", C.P. " + lblCpEmiExFac.Text.Trim().ToUpper() + ", " + lblDelMunEmiExFac.Text.Trim().ToUpper() + ", " + lblEdoEmiExFac.Text.Trim().ToUpper() + ", " + lblPaisEmiExFac.Text.Trim().ToUpper();

                        docCfd.strEncLugarExpedicion = lugarExpedicion.Trim();

                        object[] infoFacura = recepciones.obtieneUltimaFacturaTaller("1","1");
                        float folioFactura = 0;
                        if (Convert.ToBoolean(infoFacura[0]))
                        {
                            int ticketF = 0;
                            try { ticketF = Convert.ToInt32(Request.QueryString["tck"].ToString()); } catch (Exception) { ticketF = 0; }
                            if (ticketF != 0)
                                docCfd.strEncReferencia = "T" + Request.QueryString["p"].ToString() + "-Tk" + Request.QueryString["tck"].ToString() + "-" + Convert.ToInt32(infoFacura[1]).ToString();
                            else
                            {
                                if (txtReferenciasFac.Text != "")
                                {
                                    string textitu = txtReferenciasFac.Text;
                                    string test1 = textitu.Substring(1, 6).ToString();
                                    docCfd.strEncReferencia = txtReferenciasFac.Text;

                                    //docCfd.strEncFolioImp = Convert.ToInt32(test1);
                                }
                                else
                                {
                                    docCfd.strEncFolioImp = float.Parse(Convert.ToInt32(infoFacura[1]).ToString());
                                    docCfd.strEncReferencia = "T" + Request.QueryString["p"].ToString() + "-Tk0-" + Convert.ToInt32(infoFacura[1]).ToString();
                                }
                            }

                            
                            bool pagadosTk = docCfd.strEncReferencia.Contains("Tk");

                            folioFactura = float.Parse(Convert.ToInt32(infoFacura[1]).ToString());
                            docCfd.strEncRegimen = ddlRegimenSAT.Text; //REGIMEN FISCAL SAT
                            docCfd.idCfdAnt = Convert.ToInt32(Request.QueryString["fact"]);
                            try
                            {
                                object[] prefijoTaller = recepciones.obtienePrefijoTaller(Request.QueryString["t"]);
                                if (Convert.ToBoolean(prefijoTaller[0]))
                                    docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-T" + Request.QueryString["t"] + "-TFG" + Convert.ToInt32(infoFacura[1]).ToString();
                                else
                                    docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-TFG";
                            }
                            catch (Exception)
                            {
                                docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-T" + Request.QueryString["t"] + "-TFG" + Convert.ToInt32(infoFacura[1]).ToString();
                            }

                            docCfd.tipoFactura = ddlTipoFactura.SelectedValue;

                            List<detDocCfdi> lstDetCfd = new List<detDocCfdi>();
                            foreach (GridDataItem fila in grdDocu.Items)
                            {
                                RadDropDownList ddlTras1 = (RadDropDownList)fila.FindControl("ddlIvaTras");
                                RadDropDownList ddlTras2 = (RadDropDownList)fila.FindControl("ddlIeps");
                                RadDropDownList ddlRet1 = (RadDropDownList)fila.FindControl("ddlIvaRet");
                                RadDropDownList ddlRet2 = (RadDropDownList)fila.FindControl("ddlIsrRet");
                                DropDownList ddlProdServ = (DropDownList)fila.FindControl("ddlClaveProdSAT");
                                DropDownList ddlCveUnidad = (DropDownList)fila.FindControl("ddlClaveUnidadSAT");
                                lstDetCfd.Add(new detDocCfdi()
                                {
                                    IdDetCfd = fila.ItemIndex + 1,
                                    IdEmisor = Convert.ToInt16(IDEmisor),
                                    IdConcepto = ((TextBox)fila.FindControl("txtIdent")).Text,
                                    DetDesc = ((TextBox)fila.FindControl("txtConcepto")).Text.Trim(),
                                    DetCantidad = Convert.ToInt16(((RadNumericTextBox)fila.FindControl("radnumCantidad")).Value),
                                    IdUnid = Convert.ToInt16(((DropDownList)fila.FindControl("ddlUnidad")).SelectedValue),
                                    DetValorUni = Convert.ToDecimal(((TextBox)fila.FindControl("txtValUnit")).Text),
                                    //IdTras1 = string.IsNullOrEmpty(ddlTras1.SelectedValue) ? short.Parse("0") : Convert.ToInt16(ddlTras1.SelectedValue),
                                    DetImpTras1 = 0,
                                    IdTras2 = string.IsNullOrEmpty(ddlTras2.SelectedValue) ? short.Parse("0") : Convert.ToInt16(ddlTras2.SelectedValue),
                                    DetImpTras2 = Convert.ToDecimal(((Label)fila.FindControl("lblIeps")).Text),
                                    IdTras3 = string.IsNullOrEmpty(ddlTras1.SelectedValue) ? short.Parse("0") : Convert.ToInt16(ddlTras1.SelectedValue),
                                    DetImpTras3 = Convert.ToDecimal(((Label)fila.FindControl("lblIvaTras")).Text),
                                    IdRet1 = string.IsNullOrEmpty(ddlRet1.SelectedValue) ? Int16.Parse("0") : Convert.ToInt16(ddlRet1.SelectedValue),
                                    DetImpRet1 = Convert.ToDecimal(((Label)fila.FindControl("lblIvaRet")).Text),
                                    IdRet2 = string.IsNullOrEmpty(ddlRet2.SelectedValue) ? short.Parse("0") : Convert.ToInt16(ddlRet2.SelectedValue),
                                    DetImpRet2 = Convert.ToDecimal(((Label)fila.FindControl("lblIsrRet")).Text),
                                    DetPorcDesc = Convert.ToDecimal(((TextBox)fila.FindControl("txtPtjeDscto")).Text.Trim()),
                                    DetImpDesc = Convert.ToDecimal(((TextBox)fila.FindControl("txtDscto")).Text.Trim()),
                                    Subtotal = Convert.ToDecimal(((Label)fila.FindControl("lblSubTotal")).Text),
                                    Total = Convert.ToDecimal(((Label)fila.FindControl("lblTotalCpto")).Text),
                                    CoCuentaPredial = null,
                                    CveProdServSAT = ddlProdServ.SelectedValue,
                                    CveUnidadSAT = ddlCveUnidad.SelectedValue
                                });
                            }
                            object[] result = docuCfdi.guardaEncCfdi(docCfd, lstDetCfd);
                            string scriptMnsj;
                            if (Convert.ToBoolean(result[0]))
                            {
                                if (Convert.ToInt32(result[1].ToString()) > 0)
                                {
                                    ticketF = 0;
                                    DateTime fecha = fechas.obtieneFechaLocal();
                                    try { ticketF = Convert.ToInt32(Request.QueryString["tck"].ToString()); } catch (Exception) { ticketF = 0; }
                                    if (ticketF != 0)
                                    {
                                        object[] actualizaFacturado = VentaDet.actualizaFacturado(ticketF, Request.QueryString["p"].ToString(), Convert.ToInt32(result[1].ToString()));
                                        object[] infoTk = VentaDet.obtieneFechaTicket(ticketF, Request.QueryString["p"].ToString());
                                        if (Convert.ToBoolean(infoTk[0]))
                                            fecha = Convert.ToDateTime(infoTk[1]);
                                        else
                                            fecha = fechas.obtieneFechaLocal();
                                    }
                                    else
                                    {
                                        try
                                        {
                                            string[] ticketsFact = Request.QueryString["tck"].ToString().Split(new char[] { ';' });
                                            foreach (string ticketEncontrado in ticketsFact)
                                            {
                                                object[] actualizaFacturado = VentaDet.actualizaFacturado(Convert.ToInt32(ticketEncontrado), Request.QueryString["p"].ToString(), Convert.ToInt32(result[1].ToString()));
                                            }
                                            fecha = fechas.obtieneFechaLocal();
                                        }
                                        catch (Exception) { }
                                    }

                                    docCfd.IdCfd = Convert.ToInt32(result[1].ToString());
                                    docCfd.actualizaTipoFactura();


                                    Facturas facturas = new Facturas();
                                    facturas.folio = Convert.ToInt32(folioFactura);
                                    facturas.tipoCuenta = "CC";
                                    facturas.factura = docCfd.strEncReferencia;
                                    CatClientes clientes = new CatClientes();
                                    string politica = clientes.obtieneClavePoliticaCliente(lblReceptorFactura.Text);
                                    int diasPlazo = clientes.obtieneDiasPolitica(lblReceptorFactura.Text);
                                    FacturacionElectronica.Receptores recp = new FacturacionElectronica.Receptores();
                                    recp.idReceptor = Convert.ToInt32(lblReceptorFactura.Text);
                                    recp.obtieneInfoReceptor();
                                    object[] retorno = recp.info;
                                    try
                                    {
                                        DataSet infoRec = (DataSet)retorno[1];
                                        foreach (DataRow i in infoRec.Tables[0].Rows)
                                        {
                                            facturas.razon_social = Convert.ToString(i[2]);
                                            break;
                                        }
                                    }
                                    catch (Exception ex) { facturas.razon_social = ""; }
                                    int tickets = 0;
                                    try
                                    {
                                        tickets = Convert.ToInt32(Request.QueryString["tck"]);
                                    }
                                    catch (Exception)
                                    {
                                        tickets = 0;
                                    }
                                    if (tickets == 0)
                                    {
                                        facturas.fechaRevision = fecha;
                                        facturas.fechaProgPago = fechas.obtieneFechaLocal().AddDays(Convert.ToDouble(diasPlazo));
                                        facturas.id_cliprov = Convert.ToInt32(lblReceptorFactura.Text);
                                        facturas.formaPago = "E";
                                        facturas.politica = politica;
                                        if (pagadosTk)
                                            facturas.estatus = "PAG";
                                        else
                                            facturas.estatus = "PEN";
                                        facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
                                        facturas.taller = Convert.ToInt32(Request.QueryString["t"]);
                                        facturas.tipoCargo = "I";
                                        facturas.Importe = docCfd.decEncTotal;
                                        facturas.orden = Convert.ToInt32(Request.QueryString["o"]);
                                    }
                                    else
                                    {
                                        if (ticketF != 0)
                                        {
                                            facturas.fechaRevision = fecha;
                                            facturas.fechaProgPago = fecha;
                                            facturas.id_cliprov = Convert.ToInt32(lblReceptorFactura.Text);
                                            facturas.formaPago = "E";
                                            facturas.politica = politica;
                                            if (pagadosTk)
                                                facturas.estatus = "PAG";
                                            else
                                                facturas.estatus = "PEN";
                                            facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
                                            facturas.taller = Convert.ToInt32(Request.QueryString["p"].ToString());
                                            facturas.tipoCargo = "I";
                                            facturas.Importe = docCfd.decEncTotal;
                                            facturas.orden = ticketF;
                                            facturas.fechaPago = fecha;
                                        }
                                        else
                                        {
                                            facturas.fechaRevision = fecha;
                                            facturas.fechaProgPago = fecha;
                                            facturas.id_cliprov = Convert.ToInt32(lblReceptorFactura.Text);
                                            facturas.formaPago = "E";
                                            facturas.politica = politica;
                                            if (pagadosTk)
                                                facturas.estatus = "PAG";
                                            else
                                                facturas.estatus = "PEN";
                                            facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
                                            facturas.taller = Convert.ToInt32(Request.QueryString["p"].ToString());
                                            facturas.tipoCargo = "I";
                                            facturas.Importe = docCfd.decEncTotal;
                                            facturas.orden = ticketF;
                                            facturas.fechaPago = fecha;
                                        }
                                    }

                                    if (Request.QueryString["refct"] == "0" || Request.QueryString["refct"] == "1")
                                    {
                                        facturas.idCfd = Convert.ToInt32(result[1].ToString());
                                        facturas.generaFacturaCC();
                                    }
                                    else
                                    {
                                        Ejecuciones ej = new Ejecuciones();
                                        object[] existeCoso = ej.scalarToInt("select count(*)  from facturas where folio=" + facturas.folio + " and factura='"+facturas.factura+"' and id_cliprov=" + facturas.id_cliprov);
                                        if (Convert.ToInt32(Request.QueryString["fact"]) == 0 || Convert.ToInt32(existeCoso[1]) == 0)
                                        {
                                            try { facturas.idCfd = Convert.ToInt32(result[1].ToString()); }
                                            catch (Exception) { facturas.idCfd = Convert.ToInt32(Request.QueryString["fact"]); }
                                            facturas.generaFacturaCC();
                                        }
                                        else
                                        {
                                            facturas.idCfd = Convert.ToInt32(Request.QueryString["fact"]);
                                            facturas.actualizaFacturaCC();
                                        }
                                    }
                                    object[] facturasInternas = facturas.retorno;
                                    /*if (!Convert.ToBoolean(facturasInternas[0]))
                                        facturas.actualizaFactura();*/

                                }
                                if (Convert.ToBoolean(result[0]))
                                {
                                    scriptMnsj = string.Format("alert('Se ha guardado el documento: {0}');", result[1].ToString());
                                    int tck, c;
                                    try { tck = Convert.ToInt32(Request.QueryString["tck"]); }
                                    catch (Exception) { tck = 0; }
                                    try { c = Convert.ToInt32(Request.QueryString["c"]); }
                                    catch (Exception) { c = 0; }
                                    if(tck != 0 && c !=0)
                                    {
                                        Response.Redirect("FacturacionGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=" + result[1].ToString() + "&tck=" + tck + "&c=" + c);
                                    }
                                    else
                                        Response.Redirect("FacturacionGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=" + result[1].ToString()); 
                                }
                                else
                                    scriptMnsj = string.Format("alert('Hubo un problema al guardar el documento: {0}');", result[1].ToString());
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Scritpt", scriptMnsj, true);
                            }

                            else
                            {
                                scriptMnsj = string.Format("alert('Hubo un problema al guardar el documento: {0}');", Convert.ToString(infoFacura[1]));
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Scritpt", scriptMnsj, true);
                            }

                        }
                        else
                        {
                            string scriptMnsj = string.Format("alert('No es posible guardar los cambios ya que la factura se encuentra timbrada o cancelada');");
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alertas", scriptMnsj, true);
                        }
                    }
                }
            }
        }
    }

    protected void grdDocu_ItemDataBound(object sender, GridItemEventArgs e)
    {
        string IdctrlPostBack = getPostBackControlName();
        string ctrlPostBack = "";
        if (!string.IsNullOrEmpty(IdctrlPostBack))
            ctrlPostBack = IdctrlPostBack.Substring(IdctrlPostBack.LastIndexOf('$') + 1);

        if (e.Item is GridDataItem && (ctrlPostBack == "InitInsertButton" || ctrlPostBack == "AddNewRecordButton"))
        {
            if (status != "T" && status != "C")
            {
                int IdEmisor = Convert.ToInt32(lblEmisorFacturas.Text);
                int IdRecep = Convert.ToInt32(lblReceptorFactura.Text);
                int ItemIdx = e.Item.ItemIndex;

                PctjeDsctoGlb = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);

                SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString);
                string qrySelect = "SELECT IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera,ddlClaveProdSAT,ddlClaveUnidadSAT FROM DocumentoCfdi_f " +
                    "WHERE ((IdFila = " + ItemIdx + ") AND (IdEmisor = " + IdEmisor + ") AND (IdRecep = " + IdRecep + "))";

                SqlCommand comLoc = new SqlCommand(qrySelect, conLoc);
                conLoc.Open();

                SqlDataReader dr = comLoc.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int IdFila = dr.GetInt16(0);
                        ((TextBox)e.Item.FindControl("txtIdent")).Text = dr["txtIdent"].ToString();
                        ((TextBox)e.Item.FindControl("txtConcepto")).Text = dr["txtConcepto"].ToString();
                        ((RadNumericTextBox)e.Item.FindControl("radnumCantidad")).Value = Convert.ToDouble(dr["radnumCantidad"].ToString());
                        DropDownList ddlUni = (DropDownList)e.Item.FindControl("ddlUnidad");
                        valorCombo(ddlUni, dr["ddlUnidad"].ToString());
                        ((TextBox)e.Item.FindControl("txtValUnit")).Text = this.getValue5Decimals(Convert.ToDecimal(dr["txtValUnit"].ToString())).ToString();
                        ((Label)e.Item.FindControl("lblImporte")).Text = dr["lblImporte"].ToString();
                        ((TextBox)e.Item.FindControl("txtPtjeDscto")).Text = dr["txtPtjeDscto"].ToString();
                        ((TextBox)e.Item.FindControl("txtDscto")).Text = dr["txtDscto"].ToString();

                        decimal ImporteCpto = Convert.ToDecimal(((Label)e.Item.FindControl("lblImporte")).Text);
                        decimal desctoCpto = decimal.Parse(((TextBox)e.Item.FindControl("txtDscto")).Text);
                        if (ImporteCpto != 0)
                            subTotalCncpto = ((ImporteCpto - desctoCpto));
                        if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
                        {
                            dtoGlobConcepto = this.getValue5Decimals(((subTotalCncpto * PctjeDsctoGlb) / 100));
                            subTotalCncpto = subTotalCncpto - dtoGlobConcepto;
                            ((Label)e.Item.FindControl("lblDtoGlobalConcepto")).Text = dtoGlobConcepto.ToString("F2");
                        }

                        ((Label)e.Item.FindControl("lblSubTotal")).Text = dr["lblSubTotal"].ToString();
                        RadDropDownList ddlIvaTras = (RadDropDownList)e.Item.FindControl("ddlIvaTras");
                        ddlIvaTras.SelectedValue = dr["ddlIvaTras"].ToString();
                        //valorCombo(ddlImp, dr["ddlImp"].ToString());
                        RadDropDownList ddlIeps = (RadDropDownList)e.Item.FindControl("ddlIeps");
                        ddlIeps.SelectedValue = dr["ddlIeps"].ToString();
                        //valorCombo(ddlIeps, dr["ddlIeps"].ToString());
                        ((Label)e.Item.FindControl("lblIvaTras")).Text = dr["lblIvaTras"].ToString();
                        ((Label)e.Item.FindControl("lblIeps")).Text = dr["lblIeps"].ToString();
                        RadDropDownList ddlIvaRet = (RadDropDownList)e.Item.FindControl("ddlIvaRet");
                        ddlIvaRet.SelectedValue = dr["ddlIvaRet"].ToString();
                        //valorCombo(ddlImp, dr["ddlImp"].ToString());
                        RadDropDownList ddlIsrRet = (RadDropDownList)e.Item.FindControl("ddlIsrRet");
                        ddlIeps.SelectedValue = dr["ddlIsrRet"].ToString();
                        //valorCombo(ddlIeps, dr["ddlIeps"].ToString());
                        ((Label)e.Item.FindControl("lblIvaRet")).Text = dr["lblIvaRet"].ToString();
                        ((Label)e.Item.FindControl("lblIsrRet")).Text = dr["lblIsrRet"].ToString();
                        ((Label)e.Item.FindControl("lblTotalCpto")).Text = dr["lblTotal"].ToString();
                        DropDownList ddlClaveProdSAT = (DropDownList)e.Item.FindControl("ddlClaveProdSAT");
                        ddlIeps.SelectedValue = dr["ddlClaveProdSAT"].ToString();
                        DropDownList ddlClaveUnidadSAT = (DropDownList)e.Item.FindControl("ddlClaveUnidadSAT");
                        ddlIeps.SelectedValue = dr["ddlClaveUnidadSAT"].ToString();
                    }
                }
                else
                {

                }
                dr.Close();
                conLoc.Close();
            }
        }
        else if (e.Item is GridDataItem && (ctrlPostBack == ""))
        {
            if (status != "P")
            {
                ((TextBox)e.Item.FindControl("txtIdent")).Enabled = false;
                ((TextBox)e.Item.FindControl("txtConcepto")).Enabled = false;
                ((RadNumericTextBox)e.Item.FindControl("radnumCantidad")).Enabled = false;
                DropDownList ddlUni = (DropDownList)e.Item.FindControl("ddlUnidad");
                RadDropDownList ddlIvaTras = (RadDropDownList)e.Item.FindControl("ddlIvaTras");
                RadDropDownList ddlIeps = (RadDropDownList)e.Item.FindControl("ddlIeps");
                RadDropDownList ddlIvaRet = (RadDropDownList)e.Item.FindControl("ddlIvaRet");
                RadDropDownList ddlIsrRet = (RadDropDownList)e.Item.FindControl("ddlIsrRet");
                ddlUni.Enabled = ddlIvaTras.Enabled = ddlIeps.Enabled = ddlIvaRet.Enabled = ddlIsrRet.Enabled = false;
                ((TextBox)e.Item.FindControl("txtValUnit")).Enabled = false;
                ((TextBox)e.Item.FindControl("txtPtjeDscto")).Enabled = false;
                ((TextBox)e.Item.FindControl("txtDscto")).Enabled = false;
                DropDownList ddlClaveProdSAT = (DropDownList)e.Item.FindControl("ddlClaveProdSAT");
                DropDownList ddlClaveUnidadSAT = (DropDownList)e.Item.FindControl("ddlClaveUnidadSAT");
                ddlClaveProdSAT.Enabled = ddlClaveUnidadSAT.Enabled = false;
            }
        }
    }

    private void valorCombo(DropDownList ddl, string valorSelect)
    {
        for (int i = 0; i < ddl.Items.Count; i++)
        {
            if (ddl.Items[i].Value == valorSelect)
                ddl.Items[i].Selected = true;
        }
    }

    protected void grdDocu_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridCommandItem)
        {
            Button btn = (Button)e.Item.FindControl("AddNewRecordButton");
            btn.Attributes.Add("onClick", "return leeConcpto()");
            LinkButton lbtn = (LinkButton)e.Item.FindControl("InitInsertButton");
            lbtn.Attributes.Add("onClick", "return leeConcpto()");
        }
    }

    private string getPostBackControlName()
    {
        Control ctrl = null;
        //first we will check the "__EVENTTARGET" because if post back made by the controls
        //which used "_doPostBack" function also available in Request.Form collection.

        string ctrlname = Page.Request.Params.Get("__EVENTTARGET");
        if (!string.IsNullOrEmpty(ctrlname))
        {
            ctrl = Page.FindControl(ctrlname);
        }
        return ctrlname;
    }

    //variable de Item (concepto)
    decimal Importe = 0;
    decimal valUnit = 0;
    decimal ptjeDscto = 0;
    decimal Dscto = 0;
    decimal dtoGlobConcepto = 0;
    decimal subTotalCncpto = 0;
    decimal ivaTras = 0;
    decimal iepsTras = 0;
    decimal ivaRet = 0;
    decimal TotalConcepto = 0;
    decimal isrRet = 0;
    //variables Totales 
    decimal subTotBrut = 0;
    decimal totDscto = 0;
    decimal PctjeDsctoGlb = 0;
    decimal totDsctoGbl = 0;
    decimal subTotNeto = 0;
    decimal totImpTras = 0;
    decimal totImpRet = 0;

    private void CalculaSubTotal(GridDataItem Item)
    {
        decimal descuento = decimal.Parse(((TextBox)Item.FindControl("txtDscto")).Text);
        //si hay descuento global, lo resta también al concepto
        PctjeDsctoGlb = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);
        if (Importe == 0)
            Importe = Convert.ToDecimal(((Label)Item.FindControl("lblImporte")).Text);
        subTotalCncpto = (Importe - descuento);
        if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
        {
            dtoGlobConcepto = this.getValue5Decimals(((subTotalCncpto * PctjeDsctoGlb) / 100));
            subTotalCncpto = subTotalCncpto - dtoGlobConcepto;            
            ((Label)Item.FindControl("lblDtoGlobalConcepto")).Text = dtoGlobConcepto.ToString("F2");
        }
        ((Label)Item.FindControl("lblSubTotal")).Text = subTotalCncpto.ToString("F2");
        calculaIvaTras(Item);
        calculaIepsTras(Item);
        calculaIvaRet(Item);
        calculaIsrRet(Item);
        CalculaSubTotBruto(subTotalCncpto);
    }

    private void calculaIsrRet(GridDataItem Item)
    {
        isrRet = Convert.ToDecimal(((Label)Item.FindControl("lblIsrRet")).Text);
        if (subTotalCncpto != 0 && isrRet != 0)
        {
            BaseDatos bd = new BaseDatos();
            RadDropDownList ddlIsrRet = (RadDropDownList)Item.FindControl("ddlIsrRet");
            string qryValIva = "SELECT RetTasa FROM ImpRetenidos WHERE Id_Ret ='" + ddlIsrRet.SelectedValue + "'";
            object[] valIsr = bd.scalarToDecimal(qryValIva);
            ivaTras = Convert.ToDecimal(((Label)Item.FindControl("lblIvaTras")).Text);
            iepsTras = Convert.ToDecimal(((Label)Item.FindControl("lblIeps")).Text);
            ivaRet = Convert.ToDecimal(((Label)Item.FindControl("lblIvaRet")).Text);
            isrRet = (subTotalCncpto * Convert.ToDecimal(valIsr[1])) / 100;
            ((Label)Item.FindControl("lblIsrRet")).Text = isrRet.ToString("F2");
        }
        totalLinea = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet);
        ((Label)Item.FindControl("lblTotalCpto")).Text = (totalLinea).ToString("F2");
    }

    private void calculaIvaRet(GridDataItem Item)
    {
        ivaRet = Convert.ToDecimal(((Label)Item.FindControl("lblIvaRet")).Text);
        if (subTotalCncpto != 0 && ivaRet != 0)
        {
            BaseDatos bd = new BaseDatos();
            RadDropDownList ddlIvaRet = (RadDropDownList)Item.FindControl("ddlIvaRet");
            string qryValIva = "SELECT RetTasa FROM ImpRetenidos WHERE Id_Ret ='" + ddlIvaRet.SelectedValue + "'";
            object[] valIva = bd.scalarToDecimal(qryValIva);
            isrRet = Convert.ToDecimal(((Label)Item.FindControl("lblIsrRet")).Text);
            ivaTras = Convert.ToDecimal(((Label)Item.FindControl("lblIvaTras")).Text);
            iepsTras = Convert.ToDecimal(((Label)Item.FindControl("lblIeps")).Text);
            ivaRet = (subTotalCncpto * Convert.ToDecimal(valIva[1])) / 100;
            ((Label)Item.FindControl("lblIvaRet")).Text = ivaRet.ToString("F2");
        }
        totalLinea = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet);
        ((Label)Item.FindControl("lblTotalCpto")).Text = (totalLinea).ToString("F2");
    }

    private void calculaIepsTras(GridDataItem item)
    {
        iepsTras = Convert.ToDecimal(((Label)item.FindControl("lblIeps")).Text);
        if (iepsTras != 0 && subTotalCncpto != 0 && ivaTras == 0)
        {
            BaseDatos bd = new BaseDatos();
            RadDropDownList ddlIepsTras = (RadDropDownList)item.FindControl("ddlIeps");
            string qryValIeps = "SELECT TrasTasa FROM ImpTrasladado_f WHERE Id_Tras ='" + ddlIepsTras.SelectedValue + "'";
            object[] valorIeps = bd.scalarToDecimal(qryValIeps);
            iepsTras = (subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100;
            ((Label)item.FindControl("lblIeps")).Text = iepsTras.ToString("F2");
        }
        else if (iepsTras != 0 && subTotalCncpto != 0 && ivaTras != 0)
        {
            BaseDatos bd = new BaseDatos();
            RadDropDownList ddlIvaTras = (RadDropDownList)item.FindControl("ddlIvaTras");
            string qryValIva = "SELECT TrasTasa FROM ImpTrasladado_f WHERE Id_Tras ='" + ddlIvaTras.SelectedValue + "'";
            object[] valorIva = bd.scalarInt(qryValIva);
            ivaTras =  this.getValue5Decimals((this.getValue5Decimals((subTotalCncpto * Convert.ToDecimal(valorIva[1]))) / 100));

            RadDropDownList ddlIepsTras = (RadDropDownList)item.FindControl("ddlIeps");
            string qryValIeps = "SELECT TrasTasa FROM ImpTrasladado_f WHERE Id_Tras ='" + ddlIepsTras.SelectedValue + "'";
            object[] valorIeps = bd.scalarToDecimal(qryValIeps);
            iepsTras = this.getValue5Decimals(((subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100));
            ivaTras = ivaTras + this.getValue5Decimals((this.getValue5Decimals((ivaTras * Convert.ToDecimal(valorIeps[1]))) / 100)); //(subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100;
            
            ((Label)item.FindControl("lblIvaTras")).Text = ivaTras.ToString("F2");
            ((Label)item.FindControl("lblIeps")).Text = iepsTras.ToString("F2");
        }
        totalLinea = (subTotalCncpto + ivaTras + iepsTras);
        ((Label)item.FindControl("lblTotalCpto")).Text = (totalLinea).ToString("F2");
    }

    private void calculaIvaTras(GridDataItem item)
    {
        ivaTras = Convert.ToDecimal(((Label)item.FindControl("lblIvaTras")).Text);
        if (subTotalCncpto == 0)
            subTotalCncpto = Convert.ToInt32 (unidadcant.Text);
        //subTotalCncpto = Convert.ToDecimal(((Label)item.FindControl("txtValUnit")).Text) * int.Parse(((RadNumericTextBox)item.FindControl("radnumCantidad")).Value.ToString());
        if (subTotalCncpto != 0)
        {
            BaseDatos bd = new BaseDatos();
            RadDropDownList ddlIvaTras = (RadDropDownList)item.FindControl("ddlIvaTras");
            string qryValIva = "SELECT TrasTasa FROM ImpTrasladado_f WHERE Id_Tras ='" + ddlIvaTras.SelectedValue + "'";
            object[] valorIva = bd.scalarInt(qryValIva);
            ivaTras = this.getValue5Decimals((this.getValue5Decimals((subTotalCncpto * Convert.ToDecimal(valorIva[1]))) / 100));
            ((Label)item.FindControl("lblIvaTras")).Text = ivaTras.ToString("F2");
        }
        totalLinea = (subTotalCncpto + ivaTras);
        ((Label)item.FindControl("lblTotalCpto")).Text = (totalLinea).ToString("F2");
    }

    protected void radnumCantidad_TextChanged(object sender, EventArgs e)
    {
        RadNumericTextBox radnumCant = (RadNumericTextBox)sender;
        GridTableCell cell = (GridTableCell)radnumCant.Parent;
        int intCant = int.Parse(radnumCant.Value.ToString());
        unidadcant.Text = intCant.ToString();
        valUnit = decimal.Parse(((TextBox)cell.FindControl("txtValUnit")).Text);
        if (valUnit != 0)
        {
            Importe = intCant * valUnit;
            ((Label)cell.FindControl("lblImporte")).Text = Importe.ToString("F2");
            GridDataItem Item = (GridDataItem)cell.Parent;
            CalculaSubTotal(Item);
        }
    }
    public void cantidadtexto(){
        GridTableCell cell = (GridTableCell)unidadcant.Parent;
        int intCant = Convert.ToInt32(unidadcant.Text);
        valUnit = decimal.Parse(((TextBox)cell.FindControl("txtValUnit")).Text);
        if (valUnit != 0)
        {
            Importe = intCant * valUnit;
            ((Label)cell.FindControl("lblImporte")).Text = Importe.ToString("F2");
            GridDataItem Item = (GridDataItem)cell.Parent;
            CalculaSubTotal(Item);
        }
        }

    protected void txtValUnit_TextChanged(object sender, EventArgs e)
    {
        TextBox txtValUnit = (TextBox)sender;
        GridTableCell cell = (GridTableCell)txtValUnit.Parent;
        valUnit = decimal.Parse(txtValUnit.Text);
       int intCant = int.Parse(((RadNumericTextBox)cell.FindControl("radnumCantidad")).Value.ToString());
        //int intCantwero = Convert.ToInt32 (unidadcant.Text);
        Importe = intCant * valUnit;
        ((Label)cell.FindControl("lblImporte")).Text = Importe.ToString("F2");
        GridDataItem Item = (GridDataItem)cell.Parent;
        CalculaSubTotal(Item);
    }

    protected void txtPtjeDscto_TextChanged(object sender, EventArgs e)
    {
        TextBox txtPtje = (TextBox)sender;
        GridDataItem Item = (GridDataItem)txtPtje.Parent.Parent;
        Importe = decimal.Parse(((Label)Item.FindControl("lblImporte")).Text);
        ptjeDscto = decimal.Parse(txtPtje.Text);
        if (Importe != 0)
        {
            Dscto = (Importe * ptjeDscto) / 100;
            subTotalCncpto = Importe - Dscto;
            ((TextBox)Item.FindControl("txtDscto")).Text = Dscto.ToString("F2");
            CalculaSubTotal(Item);
        }
    }

    protected void txtDscto_TextChanged(object sender, EventArgs e)
    {
        TextBox txtDscto = (TextBox)sender;
        GridDataItem Item = (GridDataItem)txtDscto.Parent.Parent;
        decimal Importe = decimal.Parse(((Label)Item.FindControl("lblImporte")).Text);
        decimal Dscto = decimal.Parse(txtDscto.Text);
        decimal Ptje = 0;
        Ptje = (Dscto * 100) / Importe;
        ((TextBox)Item.FindControl("txtPtjeDscto")).Text = Ptje.ToString("F2");
        CalculaSubTotal(Item);
    }

    protected void ddlIvaTras_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        BaseDatos bd = new BaseDatos();
        string qryValIva = "SELECT TrasTasa FROM ImpTrasladado_f WHERE Id_Tras ='" + e.Value + "'";
        object[] valIva = bd.scalarInt(qryValIva);
        RadDropDownList ddlIvaTras = (RadDropDownList)sender;
        GridDataItem Item = (GridDataItem)ddlIvaTras.Parent.Parent;
        ivaTras = Convert.ToDecimal(((Label)Item.FindControl("lblIvaTras")).Text);
        iepsTras = Convert.ToDecimal(((Label)Item.FindControl("lblIeps")).Text);
        subTotalCncpto = Convert.ToDecimal(((Label)Item.FindControl("lblSubTotal")).Text);
        if (subTotalCncpto != 0)
            ivaTras = (subTotalCncpto * Convert.ToDecimal(valIva[1])) / 100;
        if (iepsTras != 0)
        {
            RadDropDownList ddlIepsTras = (RadDropDownList)Item.FindControl("ddlIeps");
            string qryValIeps = "SELECT TrasTasa FROM ImpTrasladado_f WHERE Id_Tras ='" + ddlIepsTras.SelectedValue + "'";
            object[] valorIeps = bd.scalarToDecimal(qryValIeps);
            iepsTras = (subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100;
            ivaTras = ivaTras + ((ivaTras * Convert.ToDecimal(valorIeps[1])) / 100);
        }
        ivaRet = Convert.ToDecimal(((Label)Item.FindControl("lblIvaRet")).Text);
        isrRet = Convert.ToDecimal(((Label)Item.FindControl("lblIsrRet")).Text);
        ((Label)Item.FindControl("lblIvaTras")).Text = ivaTras.ToString("F2");
        ((Label)Item.FindControl("lblIeps")).Text = iepsTras.ToString("F2");
        totalLinea = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet);
        ((Label)Item.FindControl("lblTotalCpto")).Text = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet).ToString("F2");
        calculaTotales();
    }

    protected void ddlIeps_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        BaseDatos bd = new BaseDatos();
        string qryValIeps = "SELECT TrasTasa FROM ImpTrasladado_f WHERE Id_Tras ='" + e.Value + "'";
        object[] valorIeps = bd.scalarToDecimal(qryValIeps);
        RadDropDownList ddlIeps = (RadDropDownList)sender;
        GridDataItem Item = (GridDataItem)ddlIeps.Parent.Parent;
        ivaTras = Convert.ToDecimal(((Label)Item.FindControl("lblIvaTras")).Text);
        iepsTras = Convert.ToDecimal(((Label)Item.FindControl("lblIeps")).Text);
        subTotalCncpto = Convert.ToDecimal(((Label)Item.FindControl("lblSubTotal")).Text);
        if (subTotalCncpto != 0)
            iepsTras = (subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100;
        if (ivaTras != 0)
        {
            RadDropDownList ddlIvaTras = (RadDropDownList)Item.FindControl("ddlIvaTras");
            string qryValIva = "SELECT TrasTasa FROM ImpTrasladado_f WHERE Id_Tras ='" + ddlIvaTras.SelectedValue + "'";
            object[] valorIva = bd.scalarInt(qryValIva);
            ivaTras = (subTotalCncpto * Convert.ToDecimal(valorIva[1])) / 100;
            ivaTras = ivaTras + ((ivaTras * Convert.ToDecimal(valorIeps[1])) / 100);
        }

        ivaRet = Convert.ToDecimal(((Label)Item.FindControl("lblIvaRet")).Text);
        isrRet = Convert.ToDecimal(((Label)Item.FindControl("lblIsrRet")).Text);
        ((Label)Item.FindControl("lblIvaTras")).Text = ivaTras.ToString("F2");
        ((Label)Item.FindControl("lblIeps")).Text = iepsTras.ToString("F2");
        totalLinea = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet);
        ((Label)Item.FindControl("lblTotalCpto")).Text = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet).ToString("F2");
        calculaTotales();
    }

    protected void ddlIvaRet_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        BaseDatos bd = new BaseDatos();
        string qryValIva = "SELECT RetTasa FROM ImpRetenidos WHERE Id_Ret ='" + e.Value + "'";
        object[] valIva = bd.scalarToDecimal(qryValIva);
        RadDropDownList ddlIvaRet = (RadDropDownList)sender;
        GridDataItem Item = (GridDataItem)ddlIvaRet.Parent.Parent;
        Label lblTotalCpto = (Label)Item.FindControl("lblTotalCpto");
        subTotalCncpto = Convert.ToDecimal(((Label)Item.FindControl("lblSubTotal")).Text);
        isrRet = Convert.ToDecimal(((Label)Item.FindControl("lblIsrRet")).Text);
        ivaTras = Convert.ToDecimal(((Label)Item.FindControl("lblIvaTras")).Text);
        iepsTras = Convert.ToDecimal(((Label)Item.FindControl("lblIeps")).Text);
        TotalConcepto = Convert.ToDecimal(lblTotalCpto.Text);
        if (subTotalCncpto != 0 && !string.IsNullOrEmpty(valIva[1].ToString()))
            ivaRet = (subTotalCncpto * Convert.ToDecimal(valIva[1])) / 100;
        decimal totImpRet = ivaRet + isrRet;
        totalLinea = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet);
        lblTotalCpto.Text = (subTotalCncpto + ivaTras + iepsTras - totImpRet).ToString("F2");
        ((Label)Item.FindControl("lblIvaRet")).Text = ivaRet.ToString("F2");
        calculaTotales();
    }

    protected void ddlIsrRet_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        BaseDatos bd = new BaseDatos();
        string qryValIva = "SELECT RetTasa FROM ImpRetenidos WHERE Id_Ret ='" + e.Value + "'";
        object[] valIsr = bd.scalarToDecimal(qryValIva);
        RadDropDownList ddlIsrRet = (RadDropDownList)sender;
        GridDataItem Item = (GridDataItem)ddlIsrRet.Parent.Parent;
        ivaRet = Convert.ToDecimal(((Label)Item.FindControl("lblIvaRet")).Text);
        Label lblTotalCpto = (Label)Item.FindControl("lblTotalCpto");
        subTotalCncpto = Convert.ToDecimal(((Label)Item.FindControl("lblSubTotal")).Text);
        ivaTras = Convert.ToDecimal(((Label)Item.FindControl("lblIvaTras")).Text);
        iepsTras = Convert.ToDecimal(((Label)Item.FindControl("lblIeps")).Text);
        TotalConcepto = Convert.ToDecimal(lblTotalCpto.Text);
        if (subTotalCncpto != 0 && !string.IsNullOrEmpty(valIsr[1].ToString()))
            isrRet = (subTotalCncpto * Convert.ToDecimal(valIsr[1])) / 100;
        ((Label)Item.FindControl("lblIsrRet")).Text = isrRet.ToString("F2");
        decimal totImpRet = ivaRet + isrRet;
        totalLinea = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet);
        lblTotalCpto.Text = (subTotalCncpto + ivaTras + iepsTras - totImpRet).ToString("F2");
        calculaTotales();
    }

    private void CalculaSubTotBruto(decimal subTotConcepto)
    {
        subTotBrut = 0;
        foreach (GridDataItem fila in grdDocu.Items)
        {
            subTotBrut = subTotBrut + Convert.ToDecimal(((Label)fila.FindControl("lblImporte")).Text);
        }
        //lblSubTotBru.Text = subTotBrut.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblSubTotBru")).Text = subTotBrut.ToString("F2");
        calculaTotalDescuento();
    }

    private void calculaTotalDescuento()
    {
        totDscto = totDsctoGbl = 0;
        foreach (GridDataItem fila in grdDocu.Items)
        {
            //Cálculo del descuento total INDIVIDUAL (por concepto)
            totDscto = totDscto + Convert.ToDecimal(((TextBox)fila.FindControl("txtDscto")).Text);
            //Cálculo el descuento total GLOBAL
            if (PctjeDsctoGlb != 0)
            {
                Importe = Convert.ToDecimal(((Label)fila.FindControl("lblImporte")).Text);
                Dscto = Convert.ToDecimal(((TextBox)fila.FindControl("txtDscto")).Text);

                if (Importe != 0)
                    subTotalCncpto = ((Importe - Dscto));
                else
                    subTotalCncpto = 0;

                if (subTotalCncpto != 0)
                {
                    totDsctoGbl = totDsctoGbl + ((subTotalCncpto * PctjeDsctoGlb) / 100);
                }
            }
        }

        //lblTotDscto.Text = totDscto.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblTotDscto")).Text = totDscto.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text = totDsctoGbl.ToString("F2");
        calculaSubTotNeto();
    }

    protected void txtPctjeDsctoGlb_TextChanged(object sender, EventArgs e)
    {
        //PctjeDsctoGlb = Convert.ToDecimal(txtPctjeDsctoGlb.Text);
        PctjeDsctoGlb = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);
        decimal subTotBru_menos_Dscto = 0;
        decimal desctoCpto = 0;
        decimal ImporteCpto = 0;
        //vuelve a calcular subtotales e impuesto trasladado por item (concepto)
        foreach (GridDataItem item in grdDocu.Items)
        {
            ImporteCpto = Convert.ToDecimal(((Label)item.FindControl("lblImporte")).Text);
            desctoCpto = decimal.Parse(((TextBox)item.FindControl("txtDscto")).Text);
            dtoGlobConcepto = 0;
            if (ImporteCpto != 0)
                //Creo que aqui esta el erro??
                subTotalCncpto = ((ImporteCpto - desctoCpto));
            if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
            {
                dtoGlobConcepto = ((subTotalCncpto * PctjeDsctoGlb) / 100);
                subTotalCncpto = subTotalCncpto - ((subTotalCncpto * PctjeDsctoGlb) / 100);
            }
            string global = dtoGlobConcepto.ToString("F2");
            string sub = subTotalCncpto.ToString("F2");
            ((Label)item.FindControl("lblDtoGlobalConcepto")).Text = global;//dtoGlobConcepto.ToString("F2");
            ((Label)item.FindControl("lblSubTotal")).Text = sub;//subTotalCncpto.ToString("F2");
            calculaIvaTras(item);
            calculaIepsTras(item);
            calculaIvaRet(item);
            calculaIsrRet(item);
        }
        if (PctjeDsctoGlb != 0)
        {
            //txtMotivoDscto.Visible = true;
            //lblMotDscto.Visible = true;
            ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = true;
            ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = true;
            totDscto = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotDscto")).Text);
            subTotBrut = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblSubTotBru")).Text);
            subTotBru_menos_Dscto = subTotBrut - totDscto;

            totDsctoGbl = ((subTotBru_menos_Dscto * PctjeDsctoGlb) / 100);
        }
        else
        {
            //txtMotivoDscto.Visible = false;
            //lblMotDscto.Visible = false;
            ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = false;
            ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = false;
        }

        //lblDsctoGlb.Text = totDsctoGbl.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text = totDsctoGbl.ToString("F2");
        calculaSubTotNeto();

    }

    private void calculaSubTotNeto()
    {
        subTotBrut = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblSubTotBru")).Text);
        totDscto = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotDscto")).Text);
        totDsctoGbl = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text);
        subTotNeto = ((subTotBrut - totDsctoGbl) - totDscto);
        //lblSubTotNeto.Text = subTotNeto.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblSubTotNeto")).Text = subTotNeto.ToString("F2");
        calculaTotales();
    }

    private void calculaTotales()
    {
        decimal impRetIvaCpto = 0;
        decimal impRetIsrCpto = 0;
        decimal subTotalTras = 0, totImpRet = 0, totImpTras = 0;
        //decimal trasyRetAdicionales = 0;
        foreach (GridDataItem fila in grdDocu.Items)
        {
            totImpTras = totImpTras + Convert.ToDecimal(((Label)fila.FindControl("lblIvaTras")).Text) + Convert.ToDecimal(((Label)fila.FindControl("lblIeps")).Text);
            impRetIvaCpto = Convert.ToDecimal(((Label)fila.FindControl("lblIvaRet")).Text);
            impRetIsrCpto = Convert.ToDecimal(((Label)fila.FindControl("lblIsrRet")).Text);
            totImpRet = totImpRet + (impRetIvaCpto + impRetIsrCpto);
        }
        ((Label)fvwResumen.Row.FindControl("lblImpTras")).Text = totImpTras.ToString("F2");
        Label lblSubTotNet = (Label)fvwResumen.Row.FindControl("lblSubTotNeto");
        if (Convert.ToDecimal(lblSubTotNet.Text) != 0 & subTotNeto == 0)
            subTotNeto = Convert.ToDecimal(lblSubTotNet.Text);
        subTotalTras = subTotNeto + totImpTras;
        ((Label)fvwResumen.Row.FindControl("lblSubTotTras")).Text = subTotalTras.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblImpRet")).Text = totImpRet.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblSubTotRet")).Text = (subTotalTras - totImpRet).ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblTotalGral")).Text = (subTotalTras - totImpRet).ToString("F2");
    }

    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new System.EventHandler(this.Page_Load);
    }

    private void LlenaInfoDetalle(int IDEmisor, int IdRecep)
    {
        DataSet ds = new DataSet();
        try
        {
            string qrySelect = "";
            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString))
            {
                try
                {
                    conLoc.Open();
                    qrySelect = "select * FROM DocumentoCfdi_f WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;

                    using (conLoc)
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand(qrySelect, conLoc);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(ds);
                        }
                        catch (Exception ex)
                        {
                            ds = null;
                        }
                        finally
                        {
                            conLoc.Close();
                        }
                    }
                }
                catch (Exception ex) { ds = null; lblError.Text = "Error: " + ex.Message; }
            }


            dt = ds.Tables[0];
            grdDocu.DataSource = dt;
            Session["info"] = dt;
            grdDocu.DataBind();


            if (grdDocu.Items.Count > 0)
            {
                int filasDt = 0;
                int filaIns = 0;
                foreach (GridDataItem fila in grdDocu.Items)
                {
                    filaIns = 0;
                    foreach (DataRow dato in dt.Rows)
                    {
                        if (filaIns == filasDt)
                        {
                            ((TextBox)fila.FindControl("txtIdent")).Text = dato[3].ToString();
                            ((TextBox)fila.FindControl("txtConcepto")).Text = dato[4].ToString();
                            ((RadNumericTextBox)fila.FindControl("radnumCantidad")).Value = Convert.ToDouble(dato[5].ToString());
                            ((DropDownList)fila.FindControl("ddlUnidad")).SelectedValue = dato[6].ToString();
                            ((TextBox)fila.FindControl("txtValUnit")).Text = Convert.ToDecimal(dato[7].ToString()).ToString("F2");
                            ((Label)fila.FindControl("lblImporte")).Text = Convert.ToDecimal(dato[8].ToString()).ToString("F2");
                            ((TextBox)fila.FindControl("txtPtjeDscto")).Text = Convert.ToDecimal(dato[9].ToString()).ToString("F2");
                            ((TextBox)fila.FindControl("txtDscto")).Text = Convert.ToDecimal(dato[10].ToString()).ToString("F2");
                            ((Label)fila.FindControl("lblSubTotal")).Text = Convert.ToDecimal(dato[11].ToString()).ToString("F2");    //asdsad
                            ((RadDropDownList)fila.FindControl("ddlIvaTras")).SelectedValue = dato[12].ToString();
                            ((Label)fila.FindControl("lblIvaTras")).Text = Convert.ToDecimal(dato[14].ToString()).ToString("F2");
                            ((RadDropDownList)fila.FindControl("ddlIeps")).SelectedValue = dato[13].ToString();
                            ((Label)fila.FindControl("lblIeps")).Text = dato[15].ToString();
                            ((RadDropDownList)fila.FindControl("ddlIvaRet")).SelectedValue = dato[16].ToString();
                            ((Label)fila.FindControl("lblIvaRet")).Text = dato[18].ToString();
                            ((RadDropDownList)fila.FindControl("ddlIsrRet")).SelectedValue = dato[17].ToString();
                            ((Label)fila.FindControl("lblIsrRet")).Text = dato[19].ToString();
                            ((Label)fila.FindControl("lblTotalCpto")).Text = Convert.ToDecimal(dato[20].ToString()).ToString("F2");
                            try { ((DropDownList)fila.FindControl("ddlClaveProdSAT")).SelectedValue = dato[22].ToString(); }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlClaveProdSAT")).SelectedValue = 0.ToString(); }
                            try { ((DropDownList)fila.FindControl("ddlClaveUnidadSAT")).SelectedValue = dato[23].ToString(); }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlClaveUnidadSAT")).SelectedValue = ""; }

                            /*Alx: para calcular descuento global por concepto cuando se llena con datos externos (venta_det)*/
                            //el cálculo solo aplica para ventas hechas desde punto de venta
                            decimal desctoCpto = Convert.ToDecimal(dato[9]);
                            decimal impCpto = Convert.ToDecimal(dato[8]);
                            subTotalCncpto = impCpto - ((impCpto * desctoCpto) / 100);
                            if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
                            {
                                dtoGlobConcepto = ((subTotalCncpto * PctjeDsctoGlb) / 100);
                                ((Label)fila.FindControl("lblDtoGlobalConcepto")).Text = dtoGlobConcepto.ToString("F2");
                            }
                            /*Alx*/
                        }
                        filaIns++;
                    }
                    filasDt++;
                }
            }
            else
            {
                dt = ds.Tables[0];
                grdDocu.DataSource = dt;
                Session["info"] = dt;
                DataRow dr = default(DataRow);
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                grdDocu.DataBind();
            }

            CalculaSubTotBruto(0);
        }
        catch (Exception ex)
        {
            ds = null; lblError.Text = "Error: " + ex.Message;
            dt = ds.Tables[0];
            grdDocu.DataSource = dt;
            Session["info"] = dt;
            DataRow dr = default(DataRow);
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            grdDocu.DataBind();
        }
    }

    private void cargaDatosFacturaPrevia(int idCfd)
    {
        lblError.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            int IDEmisor = 0;
            int IdRecep = 0;
            FacturacionElectronica.Facturas factura = new FacturacionElectronica.Facturas();
            factura.idCfd = idCfd;
            factura.obtieneEncabezado();
            object[] facturaAnterior = factura.info;
            try
            {
                if (Convert.ToBoolean(facturaAnterior[0]))
                {
                    DataSet dsEnc = (DataSet)facturaAnterior[1];
                    foreach (DataRow ro in dsEnc.Tables[0].Rows)
                    {
                        lblEmisorFacturas.Text = ro[2].ToString();
                        lblReceptorFactura.Text = ro[3].ToString();
                        IDEmisor = Convert.ToInt32(ro[2].ToString());
                        IdRecep = Convert.ToInt32(ro[3].ToString());
                        ddlMonedaSAT.SelectedValue = ro[4].ToString();
                        ddlFormaPagoSAT.SelectedValue = ro[5].ToString();
                        txtCondicionesPago.Text = ro[6].ToString();
                        ddlMetodoPagoSAT.SelectedValue = ro[7].ToString();
                        PctjeDsctoGlb = Convert.ToDecimal(ro[8].ToString());
                        ((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text = ro[8].ToString();
                        ((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text = Convert.ToDecimal(ro[9].ToString()).ToString("F2");
                        ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text = ro[10].ToString();
                        if (PctjeDsctoGlb != 0)
                        {
                            ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = true;
                            ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = true;
                        }

                        /*txtPctjeDsctoGlb.Text = ro[8].ToString();
                        lblDsctoGlb.Text = Convert.ToDecimal(ro[9].ToString()).ToString("F2");
                        txtMotivoDscto.Text = ro[10].ToString();*/
                        status = ro[11].ToString();
                        txtTipoCambio.Text = (Convert.ToDecimal(ro[12].ToString())).ToString();
                        txtNotaFac.Text = ro[13].ToString();
                        txtReferenciasFac.Text = ro[14].ToString();
                        txtCtaPago.Text = ro[15].ToString();
                        ddlRegimenSAT.SelectedValue = ro[16].ToString();
                        string[] folio = ro[14].ToString().Split(new char[] { '-' });
                        txtFolioImp.Text = folio[1];
                        ddlTipoFactura.SelectedValue = ro[29].ToString();
                        ddlUsoCFDI.SelectedValue = ro[30].ToString().Trim();
                        cmbTipoDocumento.SelectedValue = ro[31].ToString();
                    }
                }

            }
            catch (Exception) { IdRecep = IDEmisor = 0; }

            if (IDEmisor != 0 && IdRecep != 0)
            {
                factura.obtieneDetalle();
                object[] conceptos = factura.info;
                int filasElim = 0;
                using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString))
                {
                    try
                    {
                        conLoc.Open();
                        string qryBorra = "DELETE FROM DocumentoCfdi_f WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;
                        SqlCommand comLoc = new SqlCommand(qryBorra, conLoc);
                        using (comLoc)
                        {
                            filasElim = comLoc.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex) { filasElim = 0; lblError.Text = "Error: " + ex.Message; }
                }
                if (filasElim > -1)
                {
                    if (Convert.ToBoolean(conceptos[0]))
                    {
                        DataSet conceptosFacturar = (DataSet)conceptos[1];
                        int filas = 1;
                        foreach (DataRow r in conceptosFacturar.Tables[0].Rows)
                        {
                            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString))
                            {
                                try
                                {
                                    conLoc.Open();
                                    string qryInserta = "INSERT INTO DocumentoCfdi_f (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera,ddlClaveProdSAT,ddlClaveUnidadSAT) " +
                                        "VALUES (" + filas + ",'" + IDEmisor + "' , '" + IdRecep + "', '" + r[0].ToString() + "', '" + r[1].ToString() + "', " + r[2].ToString() + ", " + r[3].ToString() + ", " + Math.Round(Convert.ToDecimal(r[4].ToString()), 2) + ", " + Math.Round(Math.Round(Convert.ToDecimal(r[4].ToString()), 2) * Convert.ToDecimal(r[2].ToString()), 2) + ", " + r[6].ToString() + ", " + Math.Round(Convert.ToDecimal(r[7].ToString()), 2) + ", " + r[8].ToString() + "," + r[9].ToString() + ", " + r[10].ToString() + ", " + Math.Round(Convert.ToDecimal(r[11].ToString()), 2) + ", " + r[12].ToString() + ", " + r[13].ToString() + ", " + r[14].ToString() + ", " + r[15].ToString() + ", " + r[16].ToString() + ", " + Math.Round(Convert.ToDecimal(r[17].ToString()), 2) + ",convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120),'" + r[18].ToString() +"','" + r[19].ToString() + "')";
                                    SqlCommand comLoc = new SqlCommand(qryInserta, conLoc);
                                    using (comLoc)
                                    {
                                        comLoc.CommandText = qryInserta;
                                        int ok = comLoc.ExecuteNonQuery();
                                    }
                                    conLoc.Close();
                                }
                                catch (Exception ex)
                                {
                                    lblMnsjs.Text = "Error LocalDB insersion detalle: " + ex.Source + " - " + ex.Message;
                                }
                                filas++;
                            }
                        }
                    }
                    else
                        lblError.Text = "Error: " + Convert.ToString(conceptos[1]);


                    LlenaInfoDetalle(IDEmisor, IdRecep);
                    pasos++;
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.ToString();
        }


    }

    /*
    protected void lnkTimbrar_Click(object sender, EventArgs e)
    {
        try
        {
            com.formulasistemas.www.ManejadordeTimbres folios = new com.formulasistemas.www.ManejadordeTimbres();
            int foliosDisponibles = folios.ObtieneFoliosDisponibles(lblRfcEmisor.Text.Trim().ToUpper());
            if (foliosDisponibles > 0)
            {
                lblError.Text = "Folios Disponibles: " + foliosDisponibles.ToString();

                int documento = Convert.ToInt32(Request.QueryString["fact"]);
                if (documento == 0)
                {
                    int IDEmisor = Convert.ToInt32(lblEmisorFacturas.Text);
                    docuCfdi docCfd = new docuCfdi(int.Parse(lblEmisorFacturas.Text), int.Parse(lblReceptorFactura.Text), 1);
                    docCfd.IdMoneda = Convert.ToInt32(lblIdMonedaFac.Text);
                    docCfd.strEmRfc = lblRfcEmisor.Text;
                    string strReRfcNom = lblRfcReceptor.Text;
                    docCfd.strReRfc = strReRfcNom.Substring(0, 13).Trim();
                    docCfd.decEncDescGlob = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);
                    docCfd.decEncDescGlobImp = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text);
                    docCfd.decEncSubTotal = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblSubTotBru")).Text);
                    docCfd.decEncDesc = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotDscto")).Text);
                    docCfd.decEncImpTras = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblImpTras")).Text);
                    docCfd.decEncImpRet = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblImpRet")).Text);
                    docCfd.decEncTotal = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotalGral")).Text);
                    docCfd.strEncMotDesc = ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text;
                    docCfd.charEncEstatus = 'P';
                    docCfd.strEncFormaPago = txtFormaPago.Text.ToUpper();
                    docCfd.strEncMetodoPago = txtMetodoPago.Text.ToUpper();
                    docCfd.strEncCondicionesPago = txtCondicionesPago.Text.ToUpper();
                    docCfd.strEncRegimen = txtRegimenFac.Text.ToUpper();
                    docCfd.strEncNumCtaPago = txtCtaPago.Text.ToUpper();
                    docCfd.floEncTipoCambio = float.Parse(txtTipoCambio.Text);
                    docCfd.strEncNota = txtNotaFac.Text;
                    object[] infoFacura = recepciones.obtieneUltimaFacturaTaller(Request.QueryString["e"], Request.QueryString["t"]);
                    if (Convert.ToBoolean(infoFacura[0]))
                    {
                        docCfd.strEncReferencia = txtReferenciasFac.Text + "-" + Convert.ToInt32(infoFacura[1]).ToString();
                        docCfd.strEncFolioImp = float.Parse(Convert.ToInt32(infoFacura[1]).ToString());
                        docCfd.strEncRegimen = txtRegimenFac.Text;
                        docCfd.idCfdAnt = Convert.ToInt32(Request.QueryString["fact"]);
                        try
                        {
                            object[] prefijoTaller = recepciones.obtienePrefijoTaller(Request.QueryString["t"]);
                            if (Convert.ToBoolean(prefijoTaller[0]))
                                docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-T" + Request.QueryString["t"] + "-TFG" + Convert.ToInt32(infoFacura[1]).ToString();
                            else
                                docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-TFG" + Convert.ToInt32(infoFacura[1]).ToString();
                        }
                        catch (Exception)
                        {
                            docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-T" + Request.QueryString["t"] + "-TFG" + Convert.ToInt32(infoFacura[1]).ToString();
                        }

                        List<detDocCfdi> lstDetCfd = new List<detDocCfdi>();
                        foreach (GridDataItem fila in grdDocu.Items)
                        {
                            RadDropDownList ddlTras1 = (RadDropDownList)fila.FindControl("ddlIvaTras");
                            RadDropDownList ddlTras2 = (RadDropDownList)fila.FindControl("ddlIeps");
                            RadDropDownList ddlRet1 = (RadDropDownList)fila.FindControl("ddlIvaRet");
                            RadDropDownList ddlRet2 = (RadDropDownList)fila.FindControl("ddlIsrRet");
                            lstDetCfd.Add(new detDocCfdi()
                            {
                                IdDetCfd = fila.ItemIndex + 1,
                                IdEmisor = Convert.ToInt16(IDEmisor),
                                IdConcepto = ((TextBox)fila.FindControl("txtIdent")).Text,
                                DetDesc = ((TextBox)fila.FindControl("txtConcepto")).Text.Trim(),
                                DetCantidad = Convert.ToInt16(((RadNumericTextBox)fila.FindControl("radnumCantidad")).Value),
                                IdUnid = Convert.ToInt16(((DropDownList)fila.FindControl("ddlUnidad")).SelectedValue),
                                DetValorUni = Convert.ToDecimal(((TextBox)fila.FindControl("txtValUnit")).Text),
                                //IdTras1 = string.IsNullOrEmpty(ddlTras1.SelectedValue) ? short.Parse("0") : Convert.ToInt16(ddlTras1.SelectedValue),
                                DetImpTras1 = 0,
                                IdTras2 = string.IsNullOrEmpty(ddlTras2.SelectedValue) ? short.Parse("0") : Convert.ToInt16(ddlTras2.SelectedValue),
                                DetImpTras2 = Convert.ToDecimal(((Label)fila.FindControl("lblIeps")).Text),
                                IdTras3 = string.IsNullOrEmpty(ddlTras1.SelectedValue) ? short.Parse("0") : Convert.ToInt16(ddlTras1.SelectedValue),
                                DetImpTras3 = Convert.ToDecimal(((Label)fila.FindControl("lblIvaTras")).Text),
                                IdRet1 = string.IsNullOrEmpty(ddlRet1.SelectedValue) ? Int16.Parse("0") : Convert.ToInt16(ddlRet1.SelectedValue),
                                DetImpRet1 = Convert.ToDecimal(((Label)fila.FindControl("lblIvaRet")).Text),
                                IdRet2 = string.IsNullOrEmpty(ddlRet2.SelectedValue) ? short.Parse("0") : Convert.ToInt16(ddlRet2.SelectedValue),
                                DetImpRet2 = Convert.ToDecimal(((Label)fila.FindControl("lblIsrRet")).Text),
                                DetPorcDesc = Convert.ToDecimal(((TextBox)fila.FindControl("txtPtjeDscto")).Text.Trim()),
                                DetImpDesc = Convert.ToDecimal(((TextBox)fila.FindControl("txtDscto")).Text.Trim()),
                                Subtotal = Convert.ToDecimal(((Label)fila.FindControl("lblSubTotal")).Text),
                                Total = Convert.ToDecimal(((Label)fila.FindControl("lblTotalCpto")).Text),
                                CoCuentaPredial = null
                            });
                        }
                        object[] result = docuCfdi.guardaEncCfdi(docCfd, lstDetCfd);
                        if (Convert.ToBoolean(result[0]))
                            documento = Convert.ToInt32(result[1]);
                    }
                    else
                        lblError.Text = "Error: " + Convert.ToString(infoFacura[1]);
                }

                if (documento != 0)
                {
                    FacturacionElectronica.Facturas factura = new FacturacionElectronica.Facturas();
                    FacturacionElectronica.GeneracionDocumentos genera = new FacturacionElectronica.GeneracionDocumentos();
                    genera.idCfd = documento;
                    genera.actualizaFechaGeneracion(documento, fechas.obtieneFechaLocal());
                    object[] actualizadoFact = genera.info;
                    if (Convert.ToBoolean(actualizadoFact[0]))
                    {
                        genera.generaDocto();
                        if (!Convert.ToBoolean(genera.info[0]))
                            lblError.Text = "Error: " + Convert.ToString(genera.info[1]);
                        else
                        {
                            lblError.Text = Convert.ToString(genera.info[1]);
                            foliosDisponibles = folios.ObtieneFoliosDisponibles(lblRfcEmisor.Text.Trim().ToUpper());
                            lblError.Text = lblError.Text + " Folios Disponibles: " + foliosDisponibles.ToString();
                            lnkTimbrar.Visible = false;
                        }
                    }
                    else
                        lblError.Text = "Error: " + Convert.ToString(actualizadoFact[1]);
                }
                else
                    lblError.Text = "Error: No se ha indicado un documento para timbrar, o bien genere un documento nuevo";
            }
            else
                lblError.Text = "Ya no cuenta con folios disponibles para el emisor indicado";
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }
     */

    protected void lnkImprimir_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
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
                            string ruta = HttpContext.Current.Server.MapPath("~/files");
                            try { filename.CopyTo(ruta + "\\" + filename.Name); }
                            catch(Exception)
                            {
                                FileInfo file = new FileInfo(ruta + "\\" + filename.Name);
                                if(file.Exists)
                                {
                                    file.Delete();
                                }
                                filename.CopyTo(ruta + "\\" + filename.Name);
                            }
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

    protected void lnkRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("FacturasGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void cargaDatosTicket(int ticket, int idPunto)
    {
        int IDEmisor = Convert.ToInt32(lblEmisorFacturas.Text);
        int IdRecep = Convert.ToInt32(lblReceptorFactura.Text);
        List<VentaDet> arts = new List<VentaDet>();
        object[] datosVta = VentaDet.datosVenta(ticket, idPunto, idPunto);
        if (!Convert.ToBoolean(datosVta[0]))
            lblMnsjs.Text = "Error: " + datosVta[1].ToString();
        else
        {
            dt = ((DataSet)datosVta[1]).Tables[0];
            bool esOk = true;
            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString))
            {
                try
                {
                    conLoc.Open();
                    string qryBorra = "DELETE FROM DocumentoCfdi_f WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;
                    SqlCommand comLoc = new SqlCommand(qryBorra, conLoc);
                    comLoc.ExecuteNonQuery();

                    decimal porcDctoGlob = Convert.ToDecimal(dt.Rows[0][8].ToString());
                    foreach (DataRow r in dt.Rows)
                    {
                        /*decimal valUnit = Convert.ToDecimal(r[4]) / Convert.ToDecimal(1.16);
                        decimal cant = Convert.ToDecimal(r[3]);
                        decimal impte = valUnit * cant;
                        decimal totDcto = (valUnit * (Convert.ToDecimal(r[6]) / 100)) * cant; //   Convert.ToDecimal(r[7]) * cant;
                        decimal subTot = impte - totDcto; //Convert.ToDecimal(r[5]);
                        if (porcDctoGlob > 0)
                            subTot = subTot - ((subTot * porcDctoGlob) / 100);
                        decimal iva16 = subTot * 0.16M;
                        decimal Total = subTot + iva16;
                        int clavesat;
                        clavesat = Convert.ToInt32(r[10]);
                        string unidadsat = Convert.ToString(r[11]);
                        valUnit = Convert.ToDecimal(valUnit.ToString("F2"));*/

                        //decimal porcDctoGlob = Convert.ToDecimal(r[8].ToString());
                        decimal cant = Convert.ToDecimal(r[3]);
                        decimal valUnit = 0.00M;
                        decimal impte = 0.00M;
                        decimal totDcto = 0.00M;
                        decimal subTot = 0.00M;
                        decimal iva16 = 0.00M;
                        decimal Total = 0.00M;
                        int tieneiva = 0;

                        /*if (r[12].ToString() == "0.00")
                        {
                            valUnit = Convert.ToDecimal(r[4]);
                            impte = valUnit * cant;
                            totDcto = (valUnit * (Convert.ToDecimal(r[6]) / 100)) * cant; //   Convert.ToDecimal(r[7]) * cant;
                            subTot = impte - totDcto; //Convert.ToDecimal(r[5]);
                            if (porcDctoGlob > 0)
                                subTot = subTot - ((subTot * porcDctoGlob) / 100);
                            iva16 = 0.0M;
                            Total = subTot;
                            valUnit = Convert.ToDecimal(valUnit.ToString("F2"));
                            /*TporcDctoGlob = TporcDctoGlob + porcDctoGlob;
                            TvalUnit = TvalUnit + valUnit;
                            Timpte = Timpte + impte;
                            TtotDcto = TtotDcto + totDcto;
                            Tsubtotal = Tsubtotal + subTot;
                            Tiva = Tiva + iva16;
                            TTotal = TTotal + Total;
                            tieneiva = 1;
                        }
                        else
                        {
                        */
                            valUnit = Convert.ToDecimal(r[4]) / Convert.ToDecimal(1.16);
                            impte = valUnit * cant;
                            totDcto = (valUnit * (Convert.ToDecimal(r[6]) / 100)) * cant; //   Convert.ToDecimal(r[7]) * cant;
                            subTot = impte - totDcto; //Convert.ToDecimal(r[5]);
                            if (porcDctoGlob > 0)
                                subTot = subTot - ((subTot * porcDctoGlob) / 100);
                            iva16 = subTot * 0.16M;
                            Total = subTot + iva16;
                            valUnit = Convert.ToDecimal(valUnit.ToString("F2"));
                            /*TporcDctoGlob = TporcDctoGlob + porcDctoGlob;
                            TvalUnit = TvalUnit + valUnit;
                            Timpte = Timpte + impte;
                            TtotDcto = TtotDcto + totDcto;
                            Tsubtotal = Tsubtotal + subTot;
                            Tiva = Tiva + iva16;
                            TTotal = TTotal + Total;*/
                            //tieneiva = 2;
                        //}
                        int clavesat;
                        clavesat = Convert.ToInt32(r[10]);
                        string unidadsat = Convert.ToString(r[11]);

                        string qryInserta = "INSERT INTO DocumentoCfdi_f (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera,ddlClaveProdSat,ddlClaveUnidadSat) " +
                            "VALUES (" + r[0] + ",'" + IDEmisor + "' , '" + IdRecep + "', '" + r[1].ToString() + "', '" + r[2].ToString() + "', " + r[3].ToString() + ", " + 1 + ", " + Math.Round(valUnit, 2) + ", " + Math.Round(impte, 2) + ", " + r[6].ToString() + ", " + Math.Round(totDcto, 2) + ", " + Math.Round(subTot, 2) + "," + tieneiva + ", " + 5 + ", " + Math.Round(iva16, 2) + ", " + 0 + ", " + 1 + ", " + 2 + ", " + 0 + ", " + 0 + ", " + Math.Round(Total, 2) + ", convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120)," + clavesat + ",'" + unidadsat + "')";
                        comLoc.CommandText = qryInserta;
                        using (comLoc)
                        {
                            comLoc.CommandText = qryInserta;
                            int ok = comLoc.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    esOk = false;
                    lblMnsjs.Text = "Error LocalDB insersion detalle: " + ex.Source + " - " + ex.Message;
                }
                finally { conLoc.Close(); }
            }
            if (esOk)
            {
                ((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text = dt.Rows[0][8].ToString();
                //txtPctjeDsctoGlb.Text = dt.Rows[0][8].ToString();
                PctjeDsctoGlb = Convert.ToDecimal(dt.Rows[0][8].ToString());
                LlenaInfoDetalle(IDEmisor, IdRecep);
            }
        }
    }

    protected void lnkAgregaRec_Click(object sender, EventArgs e)
    {
        lblErrorActuraliza.Text = "";
        rbtnPersona.SelectedValue = "M";
        lblModo.Text = "C";
        txtRfc.Text = txtRazonNew.Text = "";
        txtCalle.Text = txtNoExt.Text = txtNoIntMod.Text = "";
        
        ddlPais.SelectedValue = ddlEstado.SelectedValue = ddlMunicipio.SelectedValue = ddlColonia.SelectedValue = ddlCodigo.SelectedValue = "";
        txtReferenciaMod.Text = txtLocalidad.Text = txtCorreo.Text = txtCorreoCC.Text = txtCorreoCCO.Text = "";
        txtRfc.MaxLength = 12;
        string script1 = "abreWinCatRec()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "AgregaCliente", script1, true);
    }

    protected void lnkEditaRec_Click(object sender, EventArgs e)
    {
        lblErrorActuraliza.Text = "";
        cargaInfoReceptor();
        lblModo.Text = "M";
        txtRfc.Enabled = false;
        rbtnPersona.Enabled = false;
        string script1 = "abreWinCatRec()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "ModificaCliente", script1, true);
    }

    protected void lnkEliminaRec_Click(object sender, EventArgs e)
    {
        lblErrorActuraliza.Text = "";
        FacturacionElectronica.Receptores Receptor = new FacturacionElectronica.Receptores();
        Receptor.idReceptor = Convert.ToInt32(lblIdReceptor.Text);
        Receptor.tieneRelacion();
        object[] relacionado = Receptor.info;
        if (Convert.ToBoolean(relacionado[0]))
        {
            int relaciones = Convert.ToInt32(relacionado[1]);
            if (relaciones == 0)
            {
                Receptor.eliminar();
                object[] eliminado = Receptor.info;
                if (Convert.ToBoolean(eliminado[0]))
                {
                    if (Convert.ToBoolean(eliminado[1]))
                        grdReceptores.DataBind();
                    else
                        lblErrorRec.Text = "No es posible eliminar el receptor. " + eliminado[1].ToString();
                }
                else
                    lblErrorRec.Text = "No es posible eliminar el receptor. " + eliminado[1].ToString();
            }
            else
                lblErrorActuraliza.Text = "El cliente no se puede eliminar debido a que ya cuenta facturas relacionadas";
        }
        else
            lblErrorActuraliza.Text = "Error al eliminar. " + relacionado[1].ToString();
    }

    protected void rbtnPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        string persona = rbtnPersona.SelectedValue.ToString();
        if (persona == "M")
            txtRfc.MaxLength = 12;
        else if (persona == "F")
            txtRfc.MaxLength = 13;
    }

    protected void ddlPais_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlEstado.Text = "";
        ddlEstado.SelectedIndex = -1;
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    protected void ddlEstado_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    protected void ddlMunicipio_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    protected void ddlColonia_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    protected void lnkAceptarModificacion_Click(object sender, EventArgs e)
    {
        lblErrorActuraliza.Text = "";
        FacturacionElectronica.Receptores Receptor = new FacturacionElectronica.Receptores();
        if (lblModo.Text == "C")
            Receptor.idReceptor = 0;
        else
            Receptor.idReceptor = Convert.ToInt32(lblIdReceptor.Text);
        Receptor.existeReceptor(txtRfc.Text.ToUpper());
        object[] existeReceptor = Receptor.info;
        if (Convert.ToBoolean(existeReceptor[0]))
        {
            if (Convert.ToBoolean(existeReceptor[1]))
            {
                Receptor.agregarActualizarReceptor(txtRfc.Text, txtRazonNew.Text, txtCalle.Text, txtNoExt.Text, txtNoIntMod.Text, txtLocalidad.Text, txtReferenciaMod.Text, txtCorreo.Text, ddlPais.SelectedValue, ddlEstado.SelectedValue, ddlMunicipio.SelectedValue, ddlColonia.SelectedValue, ddlCodigo.SelectedValue, txtCorreoCC.Text, txtCorreoCCO.Text);
                if (Convert.ToBoolean(Receptor.info[0]))
                {
                    lblErrorActuraliza.Text = "El receptor fue actualizado existósamente";
                    string script1 = "cierraWinCatRec()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraCatCliente", script1, true);
                }
                else
                    lblErrorActuraliza.Text = "Error al actualizar los datos. " + Receptor.info[1].ToString();
            }
            else
                lblErrorActuraliza.Text = "El Receptro indicado no existe o sus datos no son corrector.";
        }
        else
            lblErrorActuraliza.Text = "Error al actualizar los datos del cliente. " + existeReceptor[1].ToString();
    }

    protected void lnkElimina_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string id = btn.CommandArgument.ToString();

        DataTable ds = new DataTable();
        if (Session["info"] != null)
        {
            ds = (DataTable)Session["info"];
            foreach (DataRow r in ds.Rows)
            {
                if (r[0].ToString() == id)
                {
                    ds.Rows.Remove(r);
                    break;
                }
            }
            if (ds.Rows.Count == 0)
            {
                dt = new DataTable();
                dt.Columns.Add(new DataColumn("IdFila", typeof(string)));
                dt.Columns.Add(new DataColumn("Concepto", typeof(string)));
                dt.Columns.Add(new DataColumn("Importe", typeof(string)));
                dt.Columns.Add(new DataColumn("SubTotal", typeof(string)));
                dt.Columns.Add(new DataColumn("Imp. Tras.", typeof(string)));
                dt.Columns.Add(new DataColumn("Imp. Ret.", typeof(string)));
                dt.Columns.Add(new DataColumn("Total", typeof(string)));
                dt.Columns.Add(new DataColumn("Select", typeof(string)));
                dt = AddRow(dt);
            }
            else
                dt = ds;
            grdDocu.DataSource = dt;
            grdDocu.DataBind();

            int filasDt = 0;
            int filaIns = 0;
            foreach (GridDataItem fila in grdDocu.Items)
            {
                filaIns = 0;
                foreach (DataRow dato in dt.Rows)
                {
                    if (filaIns == filasDt)
                    {
                        try
                        {
                            ((TextBox)fila.FindControl("txtIdent")).Text = dato[3].ToString();
                            ((TextBox)fila.FindControl("txtConcepto")).Text = dato[4].ToString();
                            ((RadNumericTextBox)fila.FindControl("radnumCantidad")).Value = Convert.ToDouble(dato[5].ToString());
                            try
                            {
                                ((DropDownList)fila.FindControl("ddlUnidad")).SelectedValue = dato[6].ToString();
                            }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlUnidad")).SelectedIndex = -1; }
                            ((TextBox)fila.FindControl("txtValUnit")).Text = Convert.ToDecimal(dato[7].ToString()).ToString("F2");
                            ((Label)fila.FindControl("lblImporte")).Text = Convert.ToDecimal(dato[8].ToString()).ToString("F2");
                            ((TextBox)fila.FindControl("txtPtjeDscto")).Text = Convert.ToDecimal(dato[9].ToString()).ToString("F2");
                            ((TextBox)fila.FindControl("txtDscto")).Text = Convert.ToDecimal(dato[10].ToString()).ToString("F2");
                            ((Label)fila.FindControl("lblSubTotal")).Text = Convert.ToDecimal(dato[11].ToString()).ToString("F2");
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIvaTras")).SelectedValue = dato[12].ToString();
                            }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlIvaTras")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIvaTras")).Text = Convert.ToDecimal(dato[14].ToString()).ToString("F2");
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIeps")).SelectedValue = dato[13].ToString();
                            }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlIeps")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIeps")).Text = dato[15].ToString();
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIvaRet")).SelectedValue = dato[16].ToString();
                            }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlIvaRet")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIvaRet")).Text = dato[18].ToString();
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIsrRet")).SelectedValue = dato[17].ToString();
                            }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlIsrRet")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIsrRet")).Text = dato[19].ToString();
                            ((Label)fila.FindControl("lblTotalCpto")).Text = Convert.ToDecimal(dato[20].ToString()).ToString("F2");
                        }
                        catch (Exception) { }
                    }
                    filaIns++;
                }
                filasDt++;
            }
            CalculaSubTotBruto(0);
        }
        else
            lblError.Text = "No es posible eliminar el registro, por favor vuelva a entrar a facturacion y seleccione la factura indicada o bien agregue un nuevo documento";
    }

    protected void grdReceptores_PageIndexChanged1(object sender, EventArgs e)
    {
        //grdReceptores.SelectedIndex = 0;
        //cargaDatosRecep();
    }

    protected void grdReceptores_SelectedIndexChanged1(object sender, EventArgs e)
    {
        lblIdReceptor.Text = grdReceptores.SelectedValues["IdRecep"].ToString();
        cargaInfoReceptor();
    }

    protected void lnkBuscaRec_Click(object sender, EventArgs e)
    {
        grdReceptores.DataBind();

    }

    protected void lnkTimbrar_Click(object sender, EventArgs e)
    {
        Ejecuciones obtienestatus = new Ejecuciones();
        object[] st = obtienestatus.scalarToString("select encestatus from EncCFD_f where idcfd =" + Convert.ToInt32(Request.QueryString["fact"]));
        status = st[1].ToString();
        if (status != "C" || status != "T")
        {
            object[] info = new object[2] { false, "" };//////////////borrar
            Ejecuciones bd = new Ejecuciones();
            info[0] = this.timbradoCFDI33();
            // Crea instancia

            //com.formulasistemas.www.ManejadordeTimbres foliosWSFormula = new com.formulasistemas.www.ManejadordeTimbres();
            /*object[] info = new object[2] { false, "" };
            string rfc = "CBM140917L5A";
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
                        info[0] = this.timbradoCFDI33();
                        int timbrado = 0;

                        if (Convert.ToBoolean(info[0]))
                        {
                            timbrado = foliosWSFormula.Timbrar(rfc);
                        }
                        foliosDisponibles = foliosWSFormula.ObtieneFoliosDisponibles(rfc);
                    }
                }
            }
            catch (Exception ex)
            {
            }*/

        }
        else if (status == "C")
        {
            lblError.Text = "No se puede timbrar la factura ya que se encuentra cancelada";
        }
        else
        {
            lblError.Text = "No se puede timbrar la factura ya que esta ha sido timbrada anteriormente";
        }
    }


    private bool timbradoCFDI33()
    {
        bool timbrar = true;
        bool retorno = false;

        RVCFDI33.GeneraCFDI objCfdi = new RVCFDI33.GeneraCFDI();
        // Agrega el certificado
        //object[] certificado = bd.scalarToString("select certRutaCert from certificados_f where idEmisor=" + lblIdEmisor.Text);
        //object[] llave = bd.scalarToString("select certRutaLlave from certificados_f where idEmisor=" + lblIdEmisor.Text);
        string rutaCer = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/00001000000408933431.cer");
        string rutaKey = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/CSD_MATRIZ_CBM140917L5A_20180111_165535.key");
        //string rutaCer = certificado[1].ToString();
        //string rutaKey = llave[1].ToString();
        //string[] NoCertificadoOrgRuta = certificado[1].ToString().Split(new char[] { '\\' });
        //string[] NoCertificadoOrg = NoCertificadoOrgRuta[6].ToString().Split(new char[] { '.' });
        //string NoCert = NoCertificadoOrg[0].ToString();
        objCfdi.agregarCertificado(rutaCer);
        string RECEPTOR = "";
        string EMISOR = "";
        int idreceptor = 0;
        string txtXML;
        #region Complementos

        switch ("Ninguno")
        {

            #region Ninguno
            case "Ninguno":
                string serie;
                string RFC = "";
                string Folio, FormaDePago, CondicionesDePago;
                double SubTotal, DescuentoTotal, total;
                decimal subtotal2;
                string Moneda, TipoCambio, TipoDeComprobante, MetodoPago, LugarExpedicion, Confirmacion;
                double imptrastot = 0;
                double descuentoGlobal = 0;
                //Obtenemos la informacion del Encabezado del XML
                FacturacionElectronica3 comprobante = new FacturacionElectronica3();
                comprobante.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                comprobante.obtieneDatosEncabezado();
                if (Convert.ToBoolean(comprobante.retorno[0]))
                {
                    DataSet DatosComprobante = (DataSet)comprobante.retorno[1];
                    foreach (DataRow InfoComprobante in DatosComprobante.Tables[0].Rows)
                    {
                        RFC = InfoComprobante[14].ToString();
                        if (RFC == "MTE440316E54 ")
                        {
                            serie = "";
                        }
                        else
                        {
                            serie = "A";
                        }
                        Folio = InfoComprobante[0].ToString();
                        string[] folio2 = Folio.ToString().Split(new char[] { '-' });
                        Folio = folio2[1];
                        FormaDePago = InfoComprobante[1].ToString();
                        CondicionesDePago = "01";
                        SubTotal = Convert.ToDouble(InfoComprobante[3]);
                        subtotal2 = Convert.ToDecimal(InfoComprobante[3]);
                        SubTotal = Convert.ToDouble(subtotal2);
                        DescuentoTotal = Convert.ToDouble(InfoComprobante[4]);
                        Moneda = InfoComprobante[6].ToString();
                        TipoCambio = InfoComprobante[7].ToString();
                        total = Convert.ToDouble(InfoComprobante[5]);
                        TipoDeComprobante = "I";
                        MetodoPago = InfoComprobante[9].ToString();
                        LugarExpedicion = InfoComprobante[10].ToString();
                        Confirmacion = "";
                        //Agrega nodo Comprobande al XML
                        Fechas fecha = new Fechas();
                        DateTime fechaHoy = fecha.obtieneFechaLocal();
                        string dia, hora;
                        dia = fechaHoy.ToString("yyyy-MM-dd");
                        hora = fechaHoy.ToString("HH:mm:ss");
                        idreceptor = Convert.ToInt32(InfoComprobante[11]);
                        imptrastot = Convert.ToDouble(InfoComprobante[12]);
                        descuentoGlobal = Convert.ToDouble(InfoComprobante[13]);
                        objCfdi.agregarComprobante33(serie, Folio, dia + "T" + hora, FormaDePago, CondicionesDePago, SubTotal, DescuentoTotal, Moneda, TipoCambio, total, TipoDeComprobante, MetodoPago, LugarExpedicion, Confirmacion);
                    }//
                }

                //Obtenemos la informacion del Emisor al XML
                FacturacionElectronica3 emisor = new FacturacionElectronica3();
                emisor.idEmisor = Convert.ToInt32(lblIdEmisor.Text);
                emisor.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                emisor.obtieneInfoEmisor();
                if (Convert.ToBoolean(emisor.retorno[0]))
                {
                    DataSet DatosEmisor = (DataSet)emisor.retorno[1];
                    foreach (DataRow InfoEmisor in DatosEmisor.Tables[0].Rows)
                    {
                        //Agrega nodo Emisor al XML
                        EMISOR = InfoEmisor[0].ToString().Trim().ToUpper();
                        objCfdi.agregarEmisor(InfoEmisor[0].ToString().Trim(), InfoEmisor[1].ToString().Trim(), InfoEmisor[2].ToString().Trim());
                    }
                }
                //Obtenemos la informacion del Receptor XML
                FacturacionElectronica3 receptor = new FacturacionElectronica3();
                receptor.idReceptor = idreceptor;
                receptor.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                receptor.obtieneInfoReceptor();
                if (Convert.ToBoolean(receptor.retorno[0]))
                {
                    DataSet DatosReceptor = (DataSet)receptor.retorno[1];
                    foreach (DataRow InfoReceptor in DatosReceptor.Tables[0].Rows)
                    {
                        //Agrega nodo Receptor al XML
                        RECEPTOR = InfoReceptor[0].ToString();
                        objCfdi.agregarReceptor(InfoReceptor[0].ToString().Trim(), InfoReceptor[1].ToString().Trim(), "", "", InfoReceptor[2].ToString().Trim());
                    }
                }

                //Se obtienen los datos de los conceptos
                FacturacionElectronica3 conceptos = new FacturacionElectronica3();
                conceptos.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                conceptos.obtieneInfoConceptos();
                if (Convert.ToBoolean(conceptos.retorno[0]))
                {
                    string ClaveProdServ, NoIdentificacion, ClaveUnidad, Unidad, Descripcion = "";
                    double Cantidad, ValorUnitario, Importe, Descuento, ImpuestosRetenidos, ImpTrasladados = 0;
                    int contador_impuestos = 0;
                    DataSet DatosConceptos = (DataSet)conceptos.retorno[1];
                    //Tabla para impuestos de traslado
                    DataTable tbImp = new DataTable();
                    tbImp.Columns.Add(new DataColumn("contador", typeof(int)));
                    tbImp.Columns.Add(new DataColumn("importe", typeof(double)));
                    tbImp.Columns.Add(new DataColumn("impuesto", typeof(string)));
                    tbImp.Columns.Add(new DataColumn("tasa", typeof(string)));
                    tbImp.Columns.Add(new DataColumn("tasaOcuota", typeof(double)));
                    tbImp.Columns.Add(new DataColumn("total", typeof(double)));

                    foreach (DataRow InfoConceptos in DatosConceptos.Tables[0].Rows)
                    {
                        ClaveProdServ = InfoConceptos[0].ToString();
                        NoIdentificacion = InfoConceptos[1].ToString();
                        Cantidad = Convert.ToDouble(InfoConceptos[2]);
                        ClaveUnidad = InfoConceptos[3].ToString().Trim();
                        Unidad = InfoConceptos[4].ToString().Trim();
                        Descripcion = InfoConceptos[5].ToString();
                        if (descuentoGlobal != 0)
                        {
                            ValorUnitario = Convert.ToDouble(InfoConceptos[7]);
                            ValorUnitario = ValorUnitario / Cantidad;
                        }
                        else
                            ValorUnitario = Convert.ToDouble(InfoConceptos[6]);
                        Importe = Convert.ToDouble(InfoConceptos[7]);
                        Descuento = Convert.ToDouble(InfoConceptos[8]);
                        //Agrega nodo Conceptos con el impuesto de Traslado (Si es que tiene) al XML
                        objCfdi.agregarConcepto(ClaveProdServ, NoIdentificacion, Cantidad, ClaveUnidad, Unidad, Descripcion, ValorUnitario, ValorUnitario * Cantidad, Descuento);
                        if (Convert.ToDecimal(InfoConceptos[9]) != 0 && InfoConceptos[10].ToString() == "002" || InfoConceptos[10].ToString() == "003")
                        {
                            contador_impuestos = contador_impuestos + 1;
                            double tasaOcuota;
                            double impuesto;
                            string tasa;
                            if (InfoConceptos[10].ToString() == "002")
                            {
                                tasa = "Tasa";
                                tasaOcuota = 0.1600;
                                impuesto = Importe * tasaOcuota;
                            }
                            else
                            {
                                tasa = "Tasa";
                                tasaOcuota = 0.160000;
                                impuesto = Importe * tasaOcuota;
                            }
                            ImpTrasladados = ImpTrasladados + impuesto;


                            objCfdi.agregarImpuestoConceptoTraslado(Importe, InfoConceptos[10].ToString().Trim(), "Tasa", tasaOcuota, Convert.ToDouble(InfoConceptos[9]));

                            DataRow dr = tbImp.NewRow();
                            dr["contador"] = contador_impuestos;
                            dr["importe"] = Importe;
                            //dr["impuesto"] = InfoConceptos[10].ToString().Trim(); Original
                            dr["impuesto"] = InfoConceptos[9].ToString().Trim();
                            dr["tasa"] = tasa;
                            dr["tasaOcuota"] = tasaOcuota;
                            dr["total"] = impuesto;
                            tbImp.Rows.Add(dr);
                        }
                    }
                    if (contador_impuestos != 0)
                    {
                        string impuestoIVA = "002";
                        string TipoFactorIVA = "Tasa";
                        double ImporteImpuestoIVA = 0;
                        double tasaOcuota = 0.160000;
                        foreach (DataRow tbimpuestos in tbImp.Rows)
                        {
                            ImporteImpuestoIVA = ImporteImpuestoIVA + Convert.ToDouble(tbimpuestos[5]);
                            //objCfdi.agregarImpuestoConceptoTraslado(Convert.ToDouble(tbimpuestos[1]), tbimpuestos[2].ToString(), tbimpuestos[3].ToString(), Convert.ToDouble(tbimpuestos[4]),Convert.ToDouble(tbimpuestos[5]));
                        }

                        objCfdi.agregarImpuestos(0, imptrastot);

                        objCfdi.agregarTraslado(impuestoIVA, TipoFactorIVA, tasaOcuota, imptrastot);

                        /*
                        conceptos.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                        conceptos.obtieneImpConceptos();
                        if (Convert.ToBoolean(conceptos.retorno[0]))
                        {
                            //Obtenemos datos de impuestos de traslados y retenciones
                            DataSet DatosImpConceptos = (DataSet)conceptos.retorno[1];
                            foreach (DataRow InfoImpConceptos in DatosImpConceptos.Tables[0].Rows)
                            {
                                Impuesto = InfoImpConceptos[0].ToString();
                                TipoFactor = "Tasa";
                                TasaoCuota = 0.160000;
                                ImporteImpuesto = Convert.ToDouble(InfoImpConceptos[1]);
                                //Agrega noto Impuestos translado y retenciones al XML
                                if (Convert.ToDecimal(InfoImpConceptos[1]) != 0)
                                {
                                    objCfdi.agregarTraslado(Impuesto, TipoFactor, TasaoCuota, ImporteImpuesto);
                                }
                            }
                        }*/
                    }
                }


                /*
                objCfdi.agregarImpuestoConceptoTraslado(1, "002", "Tasa", 0.160000, 0.16);
                objCfdi.agregarInformacionAduanera("13  47  3160  3001698");
                */

                objCfdi.AgregarInfoAdicional("NoProveedor", "OrdenCompra", "Telefono", "SitioWeb", "Pagare");

                break;
            #endregion

            #region IEDU
            case "Educativas":
                objCfdi.agregarComprobante33("A", "6172", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "01", "01", 1, 0, "MXN", "1", 1.16, "I", "PUE", "39300", "");
                //objCfdi.agregarCfdiRelacionados("04");
                //objCfdi.agregarCfdiRelacionado("0a8e8af1-c7af-4925-a389-bfd1be89c99e");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("XAXX010101000", "Cliente general", "", "", "P01");
                objCfdi.agregarConcepto("01010101", "NoId", 1, "EA", "Pieza", "Producto Generico", 1, 1, 0);
                objCfdi.agregarImpuestoConceptoTraslado(1, "002", "Tasa", 0.160000, 0.16);
                //objCfdi.agregarInformacionAduanera("13  47  3160  3001698");
                objCfdi.agregarEducativas("Juan Manuel Calderón Martínez", "LANA841219HGRLRN14", "Primaria", "1023456789", "CAMJ841219I33");
                //
                objCfdi.agregarImpuestos(0, 0.16);
                objCfdi.agregarTraslado("002", "Tasa", 0.160000, 0.16);
                //objCfdi.agregarRetencion(
                break;
            #endregion

            #region NotariosPublicos
            case "Notarios Públicos":
                objCfdi.agregarComprobante33("A", "6172", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "01", "01", 1, 0, "MXN", "1", 1, "I", "PUE", "39300", "");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("XAXX010101000", "Cliente general", "", "", "P01");
                objCfdi.agregarConcepto("01010101", "NoId", 1, "EA", "Pieza", "Producto Generico", 1, 1, 0);
                objCfdi.agregarNotariosPublicos("1.0");
                objCfdi.agregarDescInmueble("01", "Calle", "", "", "", "", "", "Acapulco", 12, "MEX", "39715");
                objCfdi.agregarDatosOperacion(15, "2017-10-23", 10, 10, 1.16);
                objCfdi.agregarDatosNotario("CAMJ841219HGRLRN14", 5, 12, "");
                objCfdi.agregarDatosEnajenante("No");
                objCfdi.agregarDatosUnEnajenante("Cliente genérico", "Generico", "Genérico", "LANA7008173R5", "LANA700817HGRLRN14");
                objCfdi.agregarDatosAdquiriente("No");
                objCfdi.agregarDatosUnAdquiriente("Cliente genérico", "Generico", "Genérico", "LANA7008173R5", "LANA700817HGRLRN14");
                break;
            #endregion

            #region SPEI
            case "SPEI":
                objCfdi.agregarComprobante33("A", "6172", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "01", "", 1, 0, "MXN", "1", 1, "I", "PUE", "39300", "");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("CAMJ841219I33", "Cliente general", "", "", "P01");
                objCfdi.agregarConcepto("01010101", "NoId", 1, "EA", "Pieza", "Producto Generica", 1, 1, 0);
                //
                objCfdi.agregarSPEITercero(System.DateTime.Now.ToString("yyyy-MM-dd"), "14:05:04", "12345", "jhjhjhjhjh", "000000000000000", "jhjhjhjhjhjhjh");
                objCfdi.agregarSPEIOrdenante("Bancomer", "Juan Manuel", "12", "00000000000", "CAMJ841219I33");
                objCfdi.agregarSPEIBeneficiario("Banamex", "Juan Manuel", "12", "0000000002", "CAMJ841219I33", "Prueba", 0.16, 1);
                break;
            #endregion

            #region ValesDespensa
            case "Vales de Despensa":
                // Cfdi 3.3
                objCfdi.agregarComprobante33("A", "6172", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "01", "", 1, 0, "MXN", "1", 1.16, "I", "PUE", "39300", "");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("CAMJ841219I33", "Cliente general", "", "", "P01");
                objCfdi.agregarConcepto("01010101", "NoId", 1, "EA", "Pieza", "Producto Generica", 1, 1, 0);
                // Complemento
                objCfdi.agregarValesDespensa("monedero electrónico", "C1215456380", "12345678901", 20);
                objCfdi.agregarConceptoValesDespensa("123456789", "2017-09-22T13:18:00", "CAMJ841219I33", "CAMJ841219HGRLRN14", "Juan Manuel Calderón", "72015612300", 10);
                objCfdi.agregarConceptoValesDespensa("234567890", "2017-09-22T13:18:00", "CAMJ841219I33", "CAMJ841219HGRLRN14", "Juan Manuel Calderón", "72015612300", 10);
                break;
            #endregion

            #region ImpuestosLocales
            case "Impuestos Locales":
                // Cfdi 3.3
                objCfdi.agregarComprobante33("A", "6172", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "01", "", 1, 0, "MXN", "1", 1.16, "I", "PUE", "39300", "");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("CAMJ841219I33", "Cliente general", "", "", "P01");
                objCfdi.agregarConcepto("01010101", "NoId", 1, "EA", "Pieza", "Producto Generica", 1, 1, 0);
                // Complemento
                objCfdi.agregarImpuestosLocales("1.0", 0, 0.16);
                objCfdi.agregarTrasladosLocales("IVA Local", 0.16, 0.16);
                break;
            #endregion

            #region Pagos 1.0
            case "Pago10":
                objCfdi.agregarComprobante33("Pago", "1", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "", "", 0, 0, "XXX", "", 0, "P", "", "39300", "");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("XAXX010101000", "Cliente general", "", "", "P01");
                //
                objCfdi.agregarConcepto("84111506", "", 1, "ACT", "", "Pago", 0, 0, 0);
                // aquí empezamos con el Complemento de Pagos
                objCfdi.agregarPago10("2017-07-18T12:00:00", "02", "MXN", 0, 3, "", "", "", "12345678901", "", "", "", "", "", "");
                objCfdi.agregarPago10DoctoRelacionado("39BF5250-E071-4DDB-828D-6669E1C1C886", "", "", "MXN", 0, "PPD", 1, 1000, 1, 999);
                //objCfdi.agregarPago10DoctoRelacionado("39BF5250-E071-4DDB-828D-6669E1C1C886", "", "", "MXN", 0, "PPD", 1, 1000, 1, 999);
                //objCfdi.agregarPago10Impuestos(0, 10);
                //objCfdi.agregarPago10Traslado("002", "Tasa", 0.1600, 5);
                //objCfdi.agregarPago10Traslado("003", "Tasa", 0.1600, 5);
                break;
            #endregion

            #region EstadoDeCuentaGasolinas
            case "Estado De Cuenta Combustible":
                objCfdi.agregarComprobante33("A", "6172", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "01", "01", 1, 0, "MXN", "1", 1.16, "I", "PUE", "39300", "");
                objCfdi.agregarCfdiRelacionados("04");
                objCfdi.agregarCfdiRelacionado("0a8e8af1-c7af-4925-a389-bfd1be89c99e");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("XAXX010101000", "Cliente general", "", "", "P01");
                //
                objCfdi.agregarConcepto("01010101", "NoId", 1, "EA", "Pieza", "Producto Generica", 1, 1, 0);
                objCfdi.agregarImpuestoConceptoTraslado(1, "002", "Tasa", 0.160000, 0.16);
                //
                objCfdi.agregarImpuestos(0, 0.16);
                objCfdi.agregarTraslado("002", "Tasa", 0.160000, 0.16);
                objCfdi.agregarEstadoCuentaCombustible11("Tarjeta", "14BRC00043", 1, 1.16);
                objCfdi.agregarConceptoEstadoCuentaCombustible("531714663", "2017-05-12T12:00:00", "NEP1004207LA", "0000108043", "", 1, "32011", "Litros", "Magna", "9655118", 1, 1);
                objCfdi.agregarTrasladoEstadoCuentaCombustible("IVA", 0.1600, 0.16);
                break;
            #endregion

            #region Nomina12
            case "Nómina12":
                // Cfdi 3.3
                objCfdi.Decimales = 2;
                objCfdi.agregarComprobante33("Serie", "Folio", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "99", "", 1.00, 0, "MXN", "1", 1.00, "N", "PUE", "39715", "");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("CAMJ841219I33", "Juan Manuel Calderón", "", "", "P01");
                objCfdi.agregarConcepto("84111505", "", 1, "ACT", "", "Pago de nómina", 1.00, 1.00, 0);
                // Complemento
                objCfdi.agregarNomina12("1.2", "O", "2016-11-15", "2016-11-01", "2017-01-15", "0.001", "1.00", "", "");
                objCfdi.agregarNominaEmisor12("", "C1215456387", "");
                objCfdi.agregarNominaReceptor12("HETJ880528HGRRLL13", "123456789012345", "2017-01-01", "P3M21D", "01", "", "01", "02", "EMP13", "DOCUMENTOS DIGITALES", "DESARROLLADOR JR", "1", "01", "002", "1234567890", "1.00", "1.00", "DIF");
                objCfdi.agregarNominaPercepciones12("1.00", null, null, "1.00", "0");
                objCfdi.agregarNominaPercepcionesPercepcion12("001", "001", "Sueldo", "1.00", "0");                    // Cfdi 3.3
                                                                                                                       //// Complemento
                                                                                                                       //objCfdi.agregarNomina12("1.2", "O", "2016-11-15", "2016-11-01", "2017-01-15", "0.001", "1.00", "", "");
                                                                                                                       //objCfdi.agregarNominaEmisor12("", "C1215456387", "");
                                                                                                                       //objCfdi.agregarNominaReceptor12("HETJ880528HGRRLL13", "123456789012345", "2017-01-01", "P3M21D", "01", "", "01", "02", "EMP13", "DOCUMENTOS DIGITALES", "DESARROLLADOR JR", "1", "01", "002", "1234567890", "1.00", "1.00", "DIF");
                                                                                                                       //objCfdi.agregarNominaPercepciones12("1.00", null, null, "1.00", "0");
                                                                                                                       //objCfdi.agregarNominaPercepcionesPercepcion12("001", "001", "Sueldo", "1.00", "0");     
                break;
            #endregion

            #region Areolineas
            case "Aerolíneas":
                // Cfdi 3.3
                objCfdi.agregarComprobante33("A", "6172", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "01", "", 1, 0, "MXN", "1", 1, "I", "PUE", "39300", "");
                objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                objCfdi.agregarReceptor("CAMJ841219I33", "Cliente general", "", "", "P01");
                objCfdi.agregarConcepto("01010101", "NoId", 1, "EA", "Pieza", "Producto Generica", 1, 1, 0);
                // Complemento
                objCfdi.agregarAerolineas(10);
                objCfdi.agregarOtrosCargos(10);
                objCfdi.agregarCargo("001", 10);
                break;
            #endregion

            default:
                //MessageBox.Show("Este complemento no está habilitado todavía");
                break;
        }
        #endregion

        #region GeneraXML
        //objCfdi.CargaXslt();
        //objCfdi.GeneraXML(rutaKey, "12345678a"); Certificadoprueba
        //objCfdi.GeneraXML(rutaKey, "ForSis2017");
        objCfdi.GeneraXML(rutaKey, "Burgos1s");

        //objCfdi.GeneraXML(rutaKey, "ForSis2017");

        if (objCfdi.MensajeError != "")
        {
            //txtXML.Text = "";
            //MessageBox.Show("Ocurrió un error al crear el XML: " + objCfdi.MensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblError.Text = "Ocurrió un error al crear el XML: " + objCfdi.MensajeError;
            retorno = false;
        }
        // Escribe el previo del XML
        txtXML = objCfdi.Xml;

        string Directorio = HttpContext.Current.Server.MapPath("~/Comprobantes");
        Directorio = Directorio + "/" + EMISOR + "/" + RECEPTOR + "/";

        if (!Directory.Exists(Directorio))
            Directory.CreateDirectory(Directorio);
        DateTime fechaDia = fechas.obtieneFechaLocal();
        string ArchivoXMLSinTimbre = RECEPTOR + "-" + Request.QueryString["fact"].ToString();
        System.IO.File.WriteAllText(Directorio + "\\" + ArchivoXMLSinTimbre + ".xml", txtXML, System.Text.UTF8Encoding.UTF8);

        //return;
        // Lo timbra

        if (timbrar)
        {
            //objCfdi.TimbrarCfdiArchivo(Directorio + NombreArchivo + ".xml", "fgomez", "12121212", "http://generacfdi.com.mx/rvltimbrado/service1.asmx?WSDL",Directorio, "Timbrado.xml", false);
            //objCfdi.TimbrarCfdi("fgomez", "12121212", "http://generacfdi.com.mx/rvltimbrado/service1.asmx?WSDL", false); //TIMBRADO PRUEBA
            objCfdi.TimbrarCfdi("CBM140917L5A", "Burgos1s", "http://generacfdi.com.mx/rvltimbrado/service1.asmx?WSDL", true);
            // Verifica el error
            if (objCfdi.MensajeError == "")
            {
                txtXML = objCfdi.XmlTimbrado;
                // Coloca datos del timbrado
                string UUID = objCfdi.UUID;
                string FechaTimbrado = objCfdi.FechaTimbrado;
                string[] fecha = FechaTimbrado.Split(new char[] { 'T' }); ;
                string SelloSAT = objCfdi.SelloSat;
                string certificadoSAT = objCfdi.NoCertificadoPac;
                string SelloCFDI = objCfdi.SelloCfdi;
                string T_Certificado = objCfdi.Certificado;
                string noCertificado = objCfdi.NoCertificado;

                string ArchivoXMLTimbrado = objCfdi.UUID + "-" + Request.QueryString["fact"].ToString();

                //Guardamos XML Timbrado con el nombre del timbre
                System.IO.File.WriteAllText(Directorio + "\\" + ArchivoXMLTimbrado + ".xml", txtXML, System.Text.UTF8Encoding.UTF8);
                FacturacionElectronica3 guarda = new FacturacionElectronica3();
                guarda.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                guarda.actualizaFactura(UUID, fecha[0], fecha[1], SelloSAT, certificadoSAT, SelloCFDI, T_Certificado, noCertificado);
                string directorioTimbrado = Directorio + "\\" + ArchivoXMLTimbrado + ".xml";
                string cadenaOriginal = objCfdi.CadenaOriginal;
                //Damos valores para generar el QR
                objCfdi.GenerarQrCodeArchivo(Directorio + "\\" + objCfdi.UUID + "-" + Request.QueryString["fact"].ToString() + ".jpg");

                byte[] QR = objCfdi.ConvertirQrCode("Hola");
                guarda.actualizaTimbrado(Convert.ToInt32(Request.QueryString["fact"]), certificadoSAT, FechaTimbrado, UUID, SelloSAT, SelloCFDI, QR, directorioTimbrado, cadenaOriginal, noCertificado);
                if (Convert.ToBoolean(guarda.retorno[1]))
                {
                    lblError.Text = "Factura timbrada correctamente";
                    lnkBuscar.Visible = false;
                    lnkBuscaRec.Visible = false;
                    lnkBuscaMonedas.Visible = false;
                    multiPagina.PageViews[3].Enabled = multiPagina.PageViews[4].Enabled = false;
                    fvwResumen.Enabled = false;
                    ddlFormaPagoSAT.Enabled = txtCondicionesPago.Enabled = ddlMetodoPagoSAT.Enabled = ddlRegimenSAT.Enabled = txtCtaPago.Enabled = true;
                    lnkTimbrar.Visible = false;
                    //Response.Redirect("FacturacionGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=" + Request.QueryString["fact"]); 
                    retorno = true;
                }
            }
            else
            {
                lblError.Text = "Ocurrió un error al timbrar la factura'" + objCfdi.MensajeError;
                retorno = false;
            }
        }
        #endregion
        return retorno;
    }

    //private void btnCancelar_Click(object sender, EventArgs e)
    //{
    //    System.Windows.Forms.OpenFileDialog ArchivoXML = new System.Windows.Forms.OpenFileDialog();
    //    // Obtenemos el archivo XML
    //    ArchivoXML.Filter = "archivo(s) XML (.xml)|*.xml";
    //    if (ArchivoXML.ShowDialog() == System.Windows.Forms.DialogResult.OK)
    //    {
    //        // Crea el objeto
    //        RVCFDI33.RVCancelacion.Cancelacion objCan = new RVCFDI33.RVCancelacion.Cancelacion();
    //        // Timbra el archivo
    //        string CadenaXML = System.IO.File.ReadAllText(ArchivoXML.FileName);
    //        string clave = "12345678a";
    //        string UUID = LeerValorXML(CadenaXML, "UUID", "TimbreFiscalDigital");
    //        string ArchivoCancelacion="";
    //        ArchivoCancelacion = HttpContext.Current.Server.MapPath(ArchivoCancelacion);//Application.StartupPath + "\\Cancelacion.xml";
    //        string rutaCer = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/00001000000406147836.cer");
    //        string rutaKey = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/CSD_DEL_ORIENTE_MCA9505036Z2_20170511_143948.key");
    //        if (UUID == "")
    //        {
    //            lblError.Text = ("Esta factura no ha sido timbrar Cfdi no timbrado");
    //            return;
    //        }
    //        objCan.crearXMLCancelacionArchivo(rutaCer, rutaKey, clave, UUID, ArchivoCancelacion);
    //        if (objCan.MensajeDeError != "")
    //        {
    //            lblError.Text = ("Ocurrió un error al creal la cancelación: " + objCan.MensajeDeError);
    //            return;
    //        }

    //        objCan.enviarCancelacionArchivo(ArchivoCancelacion, "fgomez", "12121212", "http://generacfdi.com.mx/rvltimbrado/service1.asmx?WSDL", false);
    //        // Verifica el resultado
    //        if (objCan.MensajeDeError == "")
    //            lblError.Text = ("Se canceló el XML con éxito");
    //        else
    //             lblError.Text = ("Ocurrió un error al cancelar el XML: " + objCan.MensajeDeError);
    //    }
    //    // Libera memoria
    //    ArchivoXML.Dispose();
    //    GC.Collect();
    //}

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
    protected void lnkDescargarXML_Click(object sender, EventArgs e)
    {
        try
        {
            RVCFDI33.GeneraCFDI objCfdi = new RVCFDI33.GeneraCFDI();
            Ejecuciones eje = new Ejecuciones();
            object[] UUID = eje.scalarToString("select top 1 uuid from timbrado_f where idcfd=" + Request.QueryString["fact"].ToString() + " order by idtimbre desc");
            object[] emisor = eje.scalarToString("select EncEmRFC from enccfd_f where idcfd=" + Request.QueryString["fact"].ToString());
            object[] receptor = eje.scalarToString("select EncReRFC from enccfd_f where idcfd=" + Request.QueryString["fact"].ToString());
            string Directorio = HttpContext.Current.Server.MapPath("~/Comprobantes");
            Directorio = Directorio + "\\" + emisor[1].ToString() + "\\" + receptor[1].ToString();
            //string ruta = Directorio + "\\" + receptor[1].ToString() + "-" + Request.QueryString["fact"].ToString() + ".xml";
            string ruta = Directorio;
            string nombre = receptor[1].ToString() + "-" + Request.QueryString["fact"].ToString() + ".xml";

            string rutaDestino = HttpContext.Current.Server.MapPath("~/TMP");
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(ruta + "\\" + nombre);
            string hola = xDoc.InnerXml;

            FileInfo archivoTMP = new FileInfo(rutaDestino + "\\" + nombre);

            if (archivoTMP.Exists)
                archivoTMP.Delete();
            System.IO.File.WriteAllText(rutaDestino + "\\" + nombre, hola, System.Text.UTF8Encoding.UTF8);

            lnkDescarga_Click(archivoTMP, rutaDestino, nombre);
        }
        catch (Exception) { lblError.Text = "Primero debe timbrar para generar y descargar el XML"; }
    }

    protected void lnkDescarga_Click(FileInfo archivo, string ruta, string nombre)
    {
        try
        {
            Response.Clear();
            FileInfo doc = new FileInfo(archivo.FullName);
            Response.AddHeader("content-disposition", "attachment;filename=" + doc.Name);
            Response.WriteFile(doc.Directory + "\\" + doc.Name);
            Response.End();
        }
        catch (Exception)
        {
            lblError.Text = "Primero debe timbrar para generar y descargar el XML";
        }
        /*
        Response.ContentType = "text/xml";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AppendHeader("NombreCabecera", "MensajeCabecera");
        Response.TransmitFile(ruta);visual
        Response.AddHeader("Content-Disposition",
        string.Format("attachment; filename = \"{0}\"", System.IO.Path.GetFileName(nombre)));
        Response.End();*/
    }

    protected void lnkDescargaTimbrado_Click(object sender, EventArgs e)
    {
        Ejecuciones eje = new Ejecuciones();
        object[] UUID = eje.scalarToString("select top 1 uuid from timbrado_f where idcfd=" + Request.QueryString["fact"].ToString() + " order by idtimbre desc");
        object[] emisor = eje.scalarToString("select EncEmRFC from enccfd_f where idcfd=" + Request.QueryString["fact"].ToString());
        object[] receptor = eje.scalarToString("select EncReRFC from enccfd_f where idcfd=" + Request.QueryString["fact"].ToString());
        string Directorio = HttpContext.Current.Server.MapPath("~/Comprobantes");
        Directorio = Directorio + "\\" + emisor[1].ToString() + "\\" + receptor[1].ToString();

        //string ruta = Directorio + "\\" + receptor[1].ToString() + "-" + Request.QueryString["fact"].ToString() + ".xml";
        string ruta = Directorio;

        string rutaDestino = HttpContext.Current.Server.MapPath("~/TMP"); ;
        /*
        if (archivo.Exists)
        archivo.Delete();*/

        if (UUID[1].ToString() != "")
        {
            string nombreTimbrado = UUID[1].ToString() + "-" + Request.QueryString["fact"].ToString() + ".xml";
            System.IO.File.Copy(ruta + "\\" + nombreTimbrado, rutaDestino + "\\" + nombreTimbrado, true);
            lnkDescarga_ClickTimbrado(rutaDestino, nombreTimbrado);
        }
        else
            lblError.Text = "Primero debe timbrar para generar y descargar el XML";

    }

    protected void lnkDescarga_ClickTimbrado(string ruta, string nombre)
    {
        try
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + nombre);
            Response.WriteFile(ruta + "\\" + nombre);
            Response.End();
        }
        catch (Exception)
        {
            lblError.Text = "Primero debe timbrar para generar y descargar el XML";
        }
    }

    private decimal getValue5Decimals(decimal valor)
    {
        return Convert.ToDecimal(valor.ToString("F2"));
    }
}