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


public partial class FComprobantePagos : System.Web.UI.Page
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
            if (lblReceptorFactura.Text == "")
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



            lblError.Text = "";
            grdEmisores.SelectedIndex = 0;
            //PopUpEmisores
            cargaDatos();
            llenaInfoEncFactura();
            //grdReceptores.SelectedIndex = 0;

            //cargaDatosRecep();

            com.formulasistemas.www.ManejadordeTimbres folios = new com.formulasistemas.www.ManejadordeTimbres();
            int foliosDisponibles = folios.ObtieneFoliosDisponibles("MCA9505036Z2");
            lblError.Text = foliosDisponibles.ToString() + " Folios Disponibles";

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
                    lnkTimbrar.Visible = false;
                }
            }


            if (existe(Convert.ToInt32(Request.QueryString["fact"])))
            {
                lnkBuscar.Visible = false;
                lnkBuscaRec.Visible = false;
                lnkBuscaMonedas.Visible = false;
                multiPagina.PageViews[3].Enabled = multiPagina.PageViews[4].Enabled = true;
                fvwResumen.Enabled = false;
                ddlFormaPagoSAT.Enabled = txtCondicionesPago.Enabled = ddlMetodoPagoSAT.Enabled = ddlRegimenSAT.Enabled = txtCtaPago.Enabled = true;
                //lnkTimbrar.Visible = true;
            }



            //if (estaTimbrada(Convert.ToInt32(Request.QueryString["fact"]))){
            //    lnkBuscar.Visible = false;
            //    lnkBuscaRec.Visible = false;
            //    lnkBuscaMonedas.Visible = false;
            //    multiPagina.PageViews[3].Enabled = multiPagina.PageViews[4].Enabled = true;
            //    fvwResumen.Enabled = false;
            //    ddlFormaPagoSAT.Enabled = txtCondicionesPago.Enabled = ddlMetodoPagoSAT.Enabled = ddlRegimenSAT.Enabled = txtCtaPago.Enabled = true;
            //    lnkTimbrar.Visible = false;
            //    grdDocu.Enabled = false;
            //}

        }
    }


    private bool existe(int fact)
    {
        BaseDatos bd = new BaseDatos();
        string query = "select count(*) from recepcion_pagos_f where idcfdAnt='" + fact + "'";
        object[] a = bd.scalarInt(query);
        return Convert.ToBoolean(a[1]);
    }

    private bool estaTimbrada(int fact)
    {
        BaseDatos bd = new BaseDatos();
        string query = "select count(*) from recepcion_pagos_f where idcfdAnt='" + fact + "' and encestatus='T'";
        object[] a = bd.scalarInt(query);
        return Convert.ToBoolean(a[1]);
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
                        etiquetasRecElegido[11].Text = fila[15].ToString(); //Localidad
                        etiquetasRecElegido[12].Text = fila[16].ToString(); //Referencias
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


    //Guardar el Documento en la BD
    protected void grdDocu_ItemCommand(object sender, GridCommandEventArgs e)
    {
        BaseDatos bd = new BaseDatos();

        string delete = "delete from recepcion_pagos_f WHERE IdEmisor = " + lblEmisorFacturas.Text + " AND IdRecep = " + lblReceptorFactura.Text;
        bd.insertUpdateDelete(delete);

        if (status != "P"){

            int IDEmisor = Convert.ToInt32(lblEmisorFacturas.Text);
            int IdRecep = Convert.ToInt32(lblReceptorFactura.Text);
            string IdctrlPostBack = getPostBackControlName();
            string ctrlPostBack = "";
            if (!string.IsNullOrEmpty(IdctrlPostBack))
                ctrlPostBack = IdctrlPostBack.Substring(IdctrlPostBack.LastIndexOf('$') + 1);
            int noFilas = grdDocu.MasterTableView.Items.Count;
            GridDataItem ultFila = grdDocu.MasterTableView.Items[noFilas - 1];
            string Parcialidad = ((RadDropDownList)ultFila.FindControl("ddlParcialidad")).SelectedValue;
            //string strConcepto = ((TextBox)ultFila.FindControl("txtConcepto")).Text.Trim();

            //if(ddlParcialidad.sele)

            if (ctrlPostBack == "SaveChangesButton")
            {
                
                if (status != "T" && status != "C")
                {
                    
                        docuCfdi docCfd = new docuCfdi(int.Parse(lblEmisorFacturas.Text), int.Parse(lblReceptorFactura.Text), 1);
                        docCfd.IdMoneda = ddlMonedaSAT.Text;
                        docCfd.strEmRfc = lblRfcEmisor.Text;
                        docCfd.IdTipoDoc = 2;
                        string strReRfcNom = lblRfcReceptor.Text;
                        docCfd.strReRfc = strReRfcNom.Substring(0, 13).Trim();
                        docCfd.decEncTotal = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotal")).Text);
                        docCfd.charEncEstatus = 'P';
                        docCfd.strEncFormaPago = ddlFormaPagoSAT.Text.ToUpper(); //Forma de Pago SAT
                        docCfd.strEncMetodoPago = ddlMetodoPagoSAT.Text.ToUpper(); //Metodo Pago SAT
                        docCfd.strEncCondicionesPago = txtCondicionesPago.Text.ToUpper();
                        docCfd.strEncRegimen = ddlRegimenSAT.Text.ToUpper(); //Regimen Fiscal SAT
                        docCfd.strEncNumCtaPago = txtCtaPago.Text.ToUpper();
                        docCfd.floEncTipoCambio = float.Parse(txtTipoCambio.Text);
                        docCfd.strEncNota = txtNotaFac.Text;
                        docCfd.idUsoCFDI = ddlUsoCFDI.SelectedValue;
                        docCfd.tipoDocumento = cmbTipoDocumento.Text;
                        string lugarExpedicion = "";

                        lugarExpedicion = lugarExpedicion.Trim() + lblCalleEmiExFac.Text.Trim().ToUpper() + " No. Ext. " + lblNoExtEmiExFac.Text.Trim().ToUpper();
                        if (lblNoIntEmiExFac.Text != "")
                            lugarExpedicion = lugarExpedicion.Trim() + " No. Int. " + lblNoIntEmiExFac.Text.Trim().ToUpper();
                        lugarExpedicion = lugarExpedicion + ", Col. " + lblColoniaEmiExFac.Text.Trim().ToUpper() + ", C.P. " + lblCpEmiExFac.Text.Trim().ToUpper() + ", " + lblDelMunEmiExFac.Text.Trim().ToUpper() + ", " + lblEdoEmiExFac.Text.Trim().ToUpper() + ", " + lblPaisEmiExFac.Text.Trim().ToUpper();

                        docCfd.strEncLugarExpedicion = lugarExpedicion.Trim();

                    object[] infoFacura = { 1, 1 };// recepciones.obtieneUltimaFacturaTaller("1", "1");
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
                            object[] prefijoTaller = { 0, 0 };// recepciones.obtienePrefijoTaller(Request.QueryString["t"]);
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
                            RadDropDownList parcialidad = (RadDropDownList)fila.FindControl("ddlParcialidad");
                            //RadDropDownList ddlTras2 = (RadDropDownList)fila.FindControl("ddlIeps");
                            //RadDropDownList ddlRet1 = (RadDropDownList)fila.FindControl("ddlIvaRet");
                            //RadDropDownList ddlRet2 = (RadDropDownList)fila.FindControl("ddlIsrRet");
                            //DropDownList ddlProdServ = (DropDownList)fila.FindControl("ddlClaveProdSAT");
                            //DropDownList ddlCveUnidad = (DropDownList)fila.FindControl("ddlClaveUnidadSAT");

                            detDocCfdi asd = new detDocCfdi();
                            asd.IdDetCfd = fila.ItemIndex + 1;
                            asd.IdEmisor = Convert.ToInt16(IDEmisor);
                            asd.UUID = ((TextBox)fila.FindControl("txtUUID")).Text;
                            asd.Folio = ((TextBox)fila.FindControl("txtFoliot")).Text;
                            asd.Parcialidad = parcialidad.SelectedValue;
                            asd.SaldoAnt = ((TextBox)fila.FindControl("txtSaldoAnterior")).Text;
                            asd.SaldoPagado = ((TextBox)fila.FindControl("txtIportePagado")).Text;
                            asd.SaldoAct = ((Label)fila.FindControl("lblSaldoActual")).Text;
                            asd.Total = Convert.ToDecimal(((Label)fila.FindControl("lblSaldoActual")).Text);
                            asd.ProductoSAT = "84111506";
                            asd.ClaveUnidadSAT = "ACT";
                            asd.idcfdAnterior = Request.QueryString["fact"];

                            lstDetCfd.Add(asd);
                            //Falta Guardarlos en la nueva tabla Recepcion_pagos_f
                        }

                        object[] result =  docuCfdi.GuardaRecepcionPago(docCfd, lstDetCfd);
                            string scriptMnsj;
                            //if (Convert.ToBoolean(result[0]))
                            //{
                            //    if (Convert.ToInt32(result[1].ToString()) > 0)
                            //    {
                            //        ticketF = 0;
                            //        DateTime fecha = fechas.obtieneFechaLocal();
                            //        try { ticketF = Convert.ToInt32(Request.QueryString["tck"].ToString()); } catch (Exception) { ticketF = 0; }
                            //        if (ticketF != 0)
                            //        {
                            //        object[] actualizaFacturado = VentaDet.actualizaFacturado(ticketF, Request.QueryString["p"].ToString(), Convert.ToInt32(result[1].ToString()));
                            //            object[] infoTk = VentaDet.obtieneFechaTicket(ticketF, Request.QueryString["p"].ToString());
                            //            if (Convert.ToBoolean(infoTk[0]))
                            //                fecha = Convert.ToDateTime(infoTk[1]);
                            //            else
                            //                fecha = fechas.obtieneFechaLocal();
                            //        }
                            //        else
                            //        {
                            //            try
                            //            {
                            //                string[] ticketsFact = Request.QueryString["tck"].ToString().Split(new char[] { ';' });
                            //                foreach (string ticketEncontrado in ticketsFact)
                            //                {
                            //                    object[] actualizaFacturado = VentaDet.actualizaFacturado(Convert.ToInt32(ticketEncontrado), Request.QueryString["p"].ToString(), Convert.ToInt32(result[1].ToString()));
                            //                }
                            //                fecha = fechas.obtieneFechaLocal();
                            //            }
                            //            catch (Exception) { }
                            //        }

                            //        docCfd.IdCfd = Convert.ToInt32(result[1].ToString());
                            //        docCfd.actualizaTipoFactura();


                            //        Facturas facturas = new Facturas();
                            //        facturas.folio = Convert.ToInt32(folioFactura);
                            //        facturas.tipoCuenta = "CC";
                            //        facturas.factura = docCfd.strEncReferencia;
                            //        CatClientes clientes = new CatClientes();
                            //        string politica = clientes.obtieneClavePoliticaCliente(lblReceptorFactura.Text);
                            //        int diasPlazo = clientes.obtieneDiasPolitica(lblReceptorFactura.Text);
                            //        FacturacionElectronica.Receptores recp = new FacturacionElectronica.Receptores();
                            //        recp.idReceptor = Convert.ToInt32(lblReceptorFactura.Text);
                            //        recp.obtieneInfoReceptor();
                            //        object[] retorno = recp.info;
                            //        try
                            //        {
                            //            DataSet infoRec = (DataSet)retorno[1];
                            //            foreach (DataRow i in infoRec.Tables[0].Rows)
                            //            {
                            //                facturas.razon_social = Convert.ToString(i[2]);
                            //                break;
                            //            }
                            //        }
                            //        catch (Exception ex) { facturas.razon_social = ""; }
                            //        int tickets = 0;
                            //        try
                            //        {
                            //            tickets = Convert.ToInt32(Request.QueryString["tck"]);
                            //        }
                            //        catch (Exception)
                            //        {
                            //            tickets = 0;
                            //        }
                            //        if (tickets == 0)
                            //        {
                            //            facturas.fechaRevision = fecha;
                            //            facturas.fechaProgPago = fechas.obtieneFechaLocal().AddDays(Convert.ToDouble(diasPlazo));
                            //            facturas.id_cliprov = Convert.ToInt32(lblReceptorFactura.Text);
                            //            facturas.formaPago = "E";
                            //            facturas.politica = politica;
                            //            if (pagadosTk)
                            //                facturas.estatus = "PAG";
                            //            else
                            //                facturas.estatus = "PEN";
                            //            facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
                            //            facturas.taller = Convert.ToInt32(Request.QueryString["t"]);
                            //            facturas.tipoCargo = "I";
                            //            facturas.Importe = docCfd.decEncTotal;
                            //            facturas.orden = Convert.ToInt32(Request.QueryString["o"]);
                            //        }
                            //        else
                            //        {
                            //            if (ticketF != 0)
                            //            {
                            //                facturas.fechaRevision = fecha;
                            //                facturas.fechaProgPago = fecha;
                            //                facturas.id_cliprov = Convert.ToInt32(lblReceptorFactura.Text);
                            //                facturas.formaPago = "E";
                            //                facturas.politica = politica;
                            //                if (pagadosTk)
                            //                    facturas.estatus = "PAG";
                            //                else
                            //                    facturas.estatus = "PEN";
                            //                facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
                            //                facturas.taller = Convert.ToInt32(Request.QueryString["p"].ToString());
                            //                facturas.tipoCargo = "I";
                            //                facturas.Importe = docCfd.decEncTotal;
                            //                facturas.orden = ticketF;
                            //                facturas.fechaPago = fecha;
                            //            }
                            //            else
                            //            {
                            //                facturas.fechaRevision = fecha;
                            //                facturas.fechaProgPago = fecha;
                            //                facturas.id_cliprov = Convert.ToInt32(lblReceptorFactura.Text);
                            //                facturas.formaPago = "E";
                            //                facturas.politica = politica;
                            //                if (pagadosTk)
                            //                    facturas.estatus = "PAG";
                            //                else
                            //                    facturas.estatus = "PEN";
                            //                facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
                            //                facturas.taller = Convert.ToInt32(Request.QueryString["p"].ToString());
                            //                facturas.tipoCargo = "I";
                            //                facturas.Importe = docCfd.decEncTotal;
                            //                facturas.orden = ticketF;
                            //                facturas.fechaPago = fecha;
                            //            }
                            //        }

                            //        if (Request.QueryString["refct"] == "0" || Request.QueryString["refct"] == "1")
                            //        {
                            //            facturas.idCfd = Convert.ToInt32(result[1].ToString());
                            //            //facturas.generaFacturaCC();
                            //        }
                            //        else
                            //        {
                            //            Ejecuciones ej = new Ejecuciones();
                            //            object[] existeCoso = ej.scalarToInt("select count(*)  from facturas where folio=" + facturas.folio + " and factura='" + facturas.factura + "' and id_cliprov=" + facturas.id_cliprov);
                            //            if (Convert.ToInt32(Request.QueryString["fact"]) == 0 || Convert.ToInt32(existeCoso[1]) == 0)
                            //            {
                            //                try { facturas.idCfd = Convert.ToInt32(result[1].ToString()); }
                            //                catch (Exception) { facturas.idCfd = Convert.ToInt32(Request.QueryString["fact"]); }
                            //                //facturas.generaFacturaCC();
                            //            }
                            //            else
                            //            {
                            //                facturas.idCfd = Convert.ToInt32(Request.QueryString["fact"]);
                            //                //facturas.actualizaFacturaCC();
                            //            }
                            //        }
                            //        object[] facturasInternas = facturas.retorno;
                            //        /*if (!Convert.ToBoolean(facturasInternas[0]))
                            //            facturas.actualizaFactura();*/

                            //    }
                            //    if (Convert.ToBoolean(result[0]))
                            //    {
                            //        scriptMnsj = string.Format("alert('Se ha guardado el documento: {0}');", result[1].ToString());
                            //        int tck, c;
                            //        try { tck = Convert.ToInt32(Request.QueryString["tck"]); }
                            //        catch (Exception) { tck = 0; }
                            //        try { c = Convert.ToInt32(Request.QueryString["c"]); }
                            //        catch (Exception) { c = 0; }
                            //        if (tck != 0 && c != 0)
                            //        {
                            //            Response.Redirect("FacturacionGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=" + result[1].ToString() + "&tck=" + tck + "&c=" + c);
                            //        }
                            //        else
                            //            Response.Redirect("FacturacionGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=" + result[1].ToString());
                            //    }
                            //    else
                            //        scriptMnsj = string.Format("alert('Hubo un problema al guardar el documento: {0}');", result[1].ToString());
                            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Scritpt", scriptMnsj, true);
                            //}
                            
                            //else
                            //{
                            //    scriptMnsj = string.Format("alert('Hubo un problema al guardar el documento: {0}');", Convert.ToString(infoFacura[1]));
                            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Scritpt", scriptMnsj, true);
                            //}

                        }
                        else
                        {
                            string scriptMnsj = string.Format("alert('No es posible guardar los cambios ya que la factura se encuentra timbrada o cancelada');");
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alertas", scriptMnsj, true);
                        }
                    
                }
                string scriptError = string.Format("alert('La Factura se guardo Correctamente');");
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alertas", scriptError, true);
                lnkTimbrar.Visible = true;
            }
        }
    }

    protected void grdDocu_ItemDataBound(object sender, GridItemEventArgs e)
    {
        /*string IdctrlPostBack = getPostBackControlName();
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
        }*/
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

    

    protected void ddlIvaTras_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {

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
                    qrySelect = "select * FROM RecepcionPagos_f_temp WHERE IdEmisor = " + IDEmisor + " AND IdReceptor = " + IdRecep+ " and len(noCertificadoCfd)=0";

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

            decimal total = 0;

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
                            ((TextBox)fila.FindControl("txtUUID")).Text = dato[7].ToString();
                            ((TextBox)fila.FindControl("txtFoliot")).Text = dato[1].ToString();
                            //((Label)fila.FindControl("lblImporte")).Text = Convert.ToDecimal(dato[8].ToString()).ToString("F2");
                            //((TextBox)fila.FindControl("txtPtjeDscto")).Text = Convert.ToDecimal(dato[9].ToString()).ToString("F2");
                            //iNSERTAR CAMPO PARA SELECCION DE MONEDA
                            ((RadDropDownList)fila.FindControl("ddlParcialidad")).SelectedValue = dato[18].ToString();
                            ((TextBox)fila.FindControl("txtSaldoAnterior")).Text = dato[15].ToString();
                            ((TextBox)fila.FindControl("txtIportePagado")).Text = dato[16].ToString();
                            ((Label)fila.FindControl("lblSaldoActual")).Text = dato[17].ToString();
                            total = total + Convert.ToDecimal(dato[16].ToString());
                        }
                        filaIns++;
                    }
                    filasDt++;
                }
                ((Label)fvwResumen.Row.FindControl("lblTotal")).Text = total.ToString();
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

            //CalculaSubTotBruto(0);
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
            FacturacionPago.Facturas factura = new FacturacionPago.Facturas();
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
                        //PctjeDsctoGlb = Convert.ToDecimal(ro[8].ToString());
                        //((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text = ro[8].ToString();
                        //((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text = Convert.ToDecimal(ro[9].ToString()).ToString("F2");
                        //((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text = ro[10].ToString();
                        //if (PctjeDsctoGlb != 0)
                        //{
                        //    ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = true;
                        //    ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = true;
                        //}

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
                        //string qryBorra = "DELETE FROM Documentocfdi_f WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;
                        string qryBorra = "Delete From RecepcionPagos_f_temp WHERE IdEmisor = " + IDEmisor + " AND IdReceptor = " + IdRecep;
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
                                    SqlCommand comLoc = new SqlCommand();
                                    BaseDatos bd = new BaseDatos();
                                    string qryInserta = "";
                                    object[] v = bd.scalarInt("Select count(*) from RecepcionPagos_f WHERE IdEmisor = " + IDEmisor + " AND IdReceptor = " + IdRecep+" and len(noCertificadoCfd)=0");
                                    if (!Convert.ToBoolean(v[1]))
                                    {
                                        //string qryInserta = "INSERT INTO Documentocfdi_f (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera,ddlClaveProdSAT,ddlClaveUnidadSAT) " +
                                        //    "VALUES (" + filas + ",'" + IDEmisor + "' , '" + IdRecep + "', '" + r[0].ToString() + "', '" + r[1].ToString() + "', " + r[2].ToString() + ", " + r[3].ToString() + ", " + Math.Round(Convert.ToDecimal(r[4].ToString()), 2) + ", " + Math.Round(Math.Round(Convert.ToDecimal(r[4].ToString()), 2) * Convert.ToDecimal(r[2].ToString()), 2) + ", " + r[6].ToString() + ", " + Math.Round(Convert.ToDecimal(r[7].ToString()), 2) + ", " + r[8].ToString() + "," + r[9].ToString() + ", " + r[10].ToString() + ", " + Math.Round(Convert.ToDecimal(r[11].ToString()), 2) + ", " + r[12].ToString() + ", " + r[13].ToString() + ", " + r[14].ToString() + ", " + r[15].ToString() + ", " + r[16].ToString() + ", " + Math.Round(Convert.ToDecimal(r[17].ToString()), 2) + ",convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120),'" + r[18].ToString() + "','" + r[19].ToString() + "')";
                                        qryInserta = "insert into RecepcionPagos_f_temp values('" + r[0].ToString() + "','" + r[1].ToString() + "','" + IDEmisor + "','" + IdRecep + "',(select count(idtimbre)+1 from recepcionpagos_f where IdEmisor =" + IDEmisor + " and IdReceptor=" + IdRecep + " and folio='" + r[1].ToString() + "'),'','','" + r[2].ToString() + "','','','','','','','MXN','" + r[3].ToString() + "','" + r[4].ToString() + "','" + r[5].ToString() + "','" + r[6].ToString() + "');" +
                                            "insert into RecepcionPagos_f (IdCfd,Folio,IdEmisor,IdReceptor,IdTimbre,noCertificadoSat,fechaTimbrado,uuid,selloSat,selloCFD,qr,rutaArchivo,cadenaOriginal,noCertificadoCfd,Moneda,SaldoAnterior,SaldoActual,Total,Parcialidad) " +
                                            "select * from RecepcionPagos_f_temp where idcfd='" + r[0].ToString() + "' and idemisor='" + IDEmisor + "' and idreceptor='" + IdRecep + "'";
                                        comLoc = new SqlCommand(qryInserta, conLoc);
                                    }
                                    else{
                                        qryInserta = "insert into RecepcionPagos_f_temp values('" + r[0].ToString() + "','" + r[1].ToString() + "','" + IDEmisor + "','" + IdRecep + "',(select count(idtimbre)+1 from recepcionpagos_f where IdEmisor =" + IDEmisor + " and IdReceptor=" + IdRecep + " and folio='" + r[1].ToString() + "'),'','','" + r[2].ToString() + "','','','','','','','MXN','" + r[3].ToString() + "','" + r[4].ToString() + "','" + r[5].ToString() + "','" + r[6].ToString() + "');";
                                        comLoc = new SqlCommand(qryInserta, conLoc);
                                    }
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

    

    protected void lnkImprimir_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            FacturacionPago.Facturas factura = new FacturacionPago.Facturas();
            ImprimeFacturaPago imprime = new ImprimeFacturaPago();
            int documento = Convert.ToInt32(Request.QueryString["fact"]);
            if (documento == 0)
            { }
            else
            {
                object[] encabezado = null, timbre = null;
                DataTable detalle = null;
                //Encabezado
                factura.idCfd = documento;
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
                            catch (Exception)
                            {
                                FileInfo file = new FileInfo(ruta + "\\" + filename.Name);
                                if (file.Exists)
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
        FacturacionPago.Receptores Receptor = new FacturacionPago.Receptores();
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
        FacturacionPago.Receptores Receptor = new FacturacionPago.Receptores();
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
            //CalculaSubTotBruto(0);
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
        object[] st = obtienestatus.scalarToString("select encestatus from recepcion_pagos_f where idcfdAnt =" + Convert.ToInt32(Request.QueryString["fact"]));
        status = st[1].ToString();
        if (status != "C" || status != "T")
        {
            object[] info = new object[2] { false, "" };//////////////borrar
            Ejecuciones bd = new Ejecuciones();
            info[0] = this.timbradoCFDI33();
            com.formulasistemas.www.ManejadordeTimbres foliosWSFormula = new com.formulasistemas.www.ManejadordeTimbres();
            //object[] info = new object[2] { false, "" };
              string rfc = "MCA9505036Z2";
              //try
              //{
              //    int empresaActiva = foliosWSFormula.ObtieneEstatus(rfc);
              //    if (empresaActiva != 6)
              //    {
              //        info[0] = false;
              //        info[1] = "Error al timbrar documento: La empresa con R.F.C. " + rfc.Trim().ToUpper() + " esta dada de baja o no existe registrada en el catálogo de empresas de su proveedor de servicios; por favor contáctelo para resolver este error";
              //    }
              //    else
              //    {
              //        int foliosDisponibles = 0;
              //        foliosDisponibles = foliosWSFormula.ObtieneFoliosDisponibles(rfc);
              //        if (foliosDisponibles == 0)
              //        {
              //            info[0] = false;
              //            info[1] = "Error al timbrar documento: La empresa con R.F.C. " + rfc.Trim().ToUpper() + " no cuenta con folios disponibles; por favor a su proveedor de servicio de timbrado para solicitar más folios";
              //        }
              //        else
              //        {
              //           //////////////////////aquiiiiiiiiiiiiiii
              //            int timbrado = 0;

              //            if (Convert.ToBoolean(info[0]))
              //            {
              //                timbrado = foliosWSFormula.Timbrar(rfc);                           
              //            }
              //            foliosDisponibles = foliosWSFormula.ObtieneFoliosDisponibles(rfc);
              //        }
              //    }
              //}
              //catch (Exception ex)
              //{
              //}




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
    static string fechaactual;

    private bool timbradoCFDI33()
    {
        bool timbrar = true;
        bool retorno = false;
        // Crea instancia
        RVCFDI33.GeneraCFDI objCfdi = new RVCFDI33.GeneraCFDI();
        // Agrega el certificado
        //object[] certificado = bd.scalarToString("select certRutaCert from certificados_f where idEmisor=" + lblIdEmisor.Text);
        //object[] llave = bd.scalarToString("select certRutaLlave from certificados_f where idEmisor=" + lblIdEmisor.Text);
        string rutaCer = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/00001000000406147836.cer");    //Certificado Moncar
        string rutaKey = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/CSD_DEL_ORIENTE_MCA9505036Z2_20170511_143948.key");    //Key Moncar
        //string rutaCer = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/CSD_Pruebas_CFDI_LAN7008173R5.cer");
        //string rutaKey = HttpContext.Current.Server.MapPath("~/Comprobantes/Certificados/CSD_Pruebas_CFDI_LAN7008173R5.key");
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

        switch ("Pago10")
        {
            #region Pagos 1.0
            case "Pago10":

                string serie;
                string RFC = "";
                string Folio, FormaDePago, CondicionesDePago, TipoDoc;
                double SubTotal, DescuentoTotal, total;
                decimal subtotal2;
                string Moneda, TipoCambio, TipoDeComprobante, MetodoPago, LugarExpedicion, Confirmacion;
                double imptrastot = 0;
                double descuentoGlobal = 0;
                //Obtenemos la informacion del Encabezado del XML
                FacturacionElectronicaPagos comprobante = new FacturacionElectronicaPagos();
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
                        TipoDoc = InfoComprobante[8].ToString();
                        if (TipoDoc == "NC")
                        {
                            TipoDeComprobante = "E";
                        }
                        else
                        {
                            TipoDeComprobante = "I";
                        }
                        MetodoPago = InfoComprobante[9].ToString();
                        LugarExpedicion = InfoComprobante[10].ToString();
                        if (LugarExpedicion.Length <= 4)

                            LugarExpedicion = "0" + LugarExpedicion;

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
                        objCfdi.agregarComprobante33(serie, Folio, dia + "T" + hora, "", "", 0, 0, "XXX", "", 0, "P", "", LugarExpedicion, Confirmacion);
                    }//
                }

                //objCfdi.agregarComprobante33("Pago", "1", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), "", "", 0, 0, "MXN", "", 0, "P", "", "39300", "");

                //Obtenemos la informacion del emisor XML
                FacturacionElectronicaPagos emisora = new FacturacionElectronicaPagos();
                emisora.idEmisor = Convert.ToInt32(lblIdEmisor.Text);
                emisora.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                emisora.obtieneInfoEmisor();
                if (Convert.ToBoolean(emisora.retorno[0]))
                {
                    DataSet DatosEmisor = (DataSet)emisora.retorno[1];
                    foreach (DataRow InfoEmisor in DatosEmisor.Tables[0].Rows)
                    {
                        //Agrega nodo Emisor al XML
                        EMISOR = InfoEmisor[0].ToString().Trim().ToUpper();
                        objCfdi.agregarEmisor(InfoEmisor[0].ToString().Trim(), InfoEmisor[1].ToString().Trim(), InfoEmisor[2].ToString().Trim());
                    }
                }
                //Obtenemos la informacion del Receptor XML
                FacturacionElectronicaPagos receptora = new FacturacionElectronicaPagos();
                receptora.idReceptor = idreceptor;
                receptora.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                receptora.obtieneInfoReceptor();
                if (Convert.ToBoolean(receptora.retorno[0]))
                {
                    DataSet DatosReceptor = (DataSet)receptora.retorno[1];
                    foreach (DataRow InfoReceptor in DatosReceptor.Tables[0].Rows)
                    {
                        //Agrega nodo Receptor al XML
                        RECEPTOR = InfoReceptor[0].ToString();
                        objCfdi.agregarReceptor(InfoReceptor[0].ToString().Trim(), InfoReceptor[1].ToString().Trim(), "", "", "P01");
                    }
                }


                //objCfdi.agregarEmisor("LAN7008173R5", "CINDEMEX SA DE CV", "601");
                //objCfdi.agregarReceptor("XAXX010101000", "Cliente general", "", "", "P01");
                //

                //Se obtienen los datos de los conceptos
                FacturacionElectronicaPagos conceptos = new FacturacionElectronicaPagos();
                conceptos.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                conceptos.obtieneUUIDFOLIO();
                string UUIDF = "", Fol = "", Par = "", SAnt = "", SP = "", SACT = "", formapago = "", metodpago = "";
                if (Convert.ToBoolean(conceptos.retorno[0]))
                {
                    DataSet DatosConceptos = (DataSet)conceptos.retorno[1];

                    foreach (DataRow InfoConceptos in DatosConceptos.Tables[0].Rows)
                    {
                        //Agrega nodo Conceptos con el impuesto de Traslado (Si es que tiene) al XML
                        objCfdi.agregarConcepto("84111506", "", 1, "ACT", "", "Pago", 0, 0, 0);
                        UUIDF = InfoConceptos[0].ToString();
                        Fol = InfoConceptos[1].ToString();
                        Par = InfoConceptos[2].ToString();
                        SAnt = InfoConceptos[3].ToString();
                        SP = InfoConceptos[4].ToString();
                        SACT = InfoConceptos[5].ToString();
                        formapago = InfoConceptos[6].ToString();
                        metodpago = InfoConceptos[7].ToString();
                    }
                }

                //objCfdi.agregarConcepto("84111506", "", 1, "ACT", "", "Pago", 0, 0, 0);
                // aquí empezamos con el Complemento de Pagos

                fechaactual = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("hh:mm:ss");



                objCfdi.agregarPago10(fechaactual, formapago, "MXN", 0, Convert.ToDouble(SP), "", "", "", "", "", "", "", "", "", "");     //Ingresar los valores generados por el concepto de pago
                string PPDPPID = "";
                if (Convert.ToBoolean(Convert.ToInt32(metodpago)))
                    PPDPPID = "PID";
                else
                    PPDPPID = "PPD";

                objCfdi.agregarPago10DoctoRelacionado(UUIDF, "", Fol, "MXN", 0, PPDPPID, Convert.ToInt32(Par), Convert.ToDouble(SAnt), Convert.ToDouble(SP), Convert.ToDouble(SACT));    //Ingresar UUID de la factura previa 
                //objCfdi.agregarPago10DoctoRelacionado("39BF5250-E071-4DDB-828D-6669E1C1C886", "", "", "MXN", 0, "PPD", 1, 1000, 1, 999);
                //objCfdi.agregarPago10Impuestos(0, 10);
                //objCfdi.agregarPago10Traslado("002", "Tasa", 0.1600, 5);
                //objCfdi.agregarPago10Traslado("003", "Tasa", 0.1600, 5);
                break;
                #endregion
        }
        #endregion

        #region GeneraXML
        //objCfdi.CargaXslt();
        //objCfdi.GeneraXML(rutaKey, "12345678a"); //Certificadoprueba
        //objCfdi.GeneraXML(rutaKey, "ForSis2017");
        objCfdi.GeneraXML(rutaKey, "MONCAR2017");

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
        //EMISOR = "LAN7008173R5";
        //RECEPTOR = "XAXX010101000";

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
            objCfdi.TimbrarCfdi("MCA9505036Z2", "K2C694v6", "http://generacfdi.com.mx/rvltimbrado/service1.asmx?WSDL", true);
            //objCfdi.TimbrarCfdi("MCA9505036Z2", "K2C694v6", "http://generacfdi.com.mx/rvltimbrado/service1.asmx?WSDL", true);
            // Verifica el error
            if (objCfdi.MensajeError == "")
            {
                txtXML = objCfdi.XmlTimbrado;
                // Coloca datos del timbrado
                string UUID = objCfdi.UUID;
                //string FechaTimbrado = objCfdi.FechaTimbrado;
                string[] fecha = fechaactual.Split(new char[] { 'T' }); ;
                string SelloSAT = objCfdi.SelloSat;
                string certificadoSAT = objCfdi.NoCertificadoPac;
                string SelloCFDI = objCfdi.SelloCfdi;
                string T_Certificado = objCfdi.Certificado;
                string noCertificado = objCfdi.NoCertificado;

                string ArchivoXMLTimbrado = objCfdi.UUID + "-" + Request.QueryString["fact"].ToString();

                //Guardamos XML Timbrado con el nombre del timbre
                System.IO.File.WriteAllText(Directorio + "\\" + ArchivoXMLTimbrado + ".xml", txtXML, System.Text.UTF8Encoding.UTF8);
                FacturacionElectronicaPagos guarda = new FacturacionElectronicaPagos();
                guarda.idCFD = Convert.ToInt32(Request.QueryString["fact"]);
                bool actualizado = guarda.actualizaFactura(UUID, fecha[0], fecha[1], SelloSAT, certificadoSAT, SelloCFDI, T_Certificado, noCertificado);
                string directorioTimbrado = Directorio + "\\" + ArchivoXMLTimbrado + ".xml";
                string cadenaOriginal = objCfdi.CadenaOriginal;
                //Damos valores para generar el QR
                objCfdi.GenerarQrCodeArchivo(Directorio + "\\" + objCfdi.UUID + "-" + Request.QueryString["fact"].ToString() + ".jpg");

                byte[] QR = objCfdi.ConvertirQrCode("Hola");



                object[] baseD = new Ejecuciones().dataSet("select idemisor,idrecep,Folio,SaldoAnterior,SaldoPagado,SaldoActual,Parcialidad  from recepcion_pagos_f where idcfdant='" + Convert.ToInt32(Request.QueryString["fact"]) + "'");
                DataSet a = (DataSet)baseD[1];
                foreach (DataRow Info in a.Tables[0].Rows)
                {
                    guarda.actualizaTimbrado(Convert.ToInt32(Request.QueryString["fact"]), Info[0].ToString(), Info[1].ToString(), certificadoSAT, fechaactual, UUID, SelloSAT, SelloCFDI, QR, directorioTimbrado, cadenaOriginal, noCertificado, Info[2].ToString(), Info[3].ToString(), Info[4].ToString(), Info[5].ToString(), Info[6].ToString());
                }



                //if (Convert.ToBoolean(guarda.retorno[1]))
                if (actualizado)
                {
                    lblError.Text = "Factura timbrada correctamente";
                    lnkBuscar.Visible = false;
                    lnkBuscaRec.Visible = false;
                    lnkBuscaMonedas.Visible = false;
                    multiPagina.PageViews[3].Enabled = multiPagina.PageViews[4].Enabled = false;
                    fvwResumen.Enabled = false;
                    grdDocu.Enabled = false;
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

    protected void ddlFormaPagoSAT_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LabelFormaPago.Text = ddlFormaPagoSAT.Text;
        ((Label)fvwResumen.Row.FindControl("LabelFormaPago")).Text = ddlFormaPagoSAT.SelectedValue;
    }

    protected void ddlParcialidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected void txtIportePagado_TextChanged(object sender, EventArgs e)
    {
        decimal a = 0;
        decimal total = 0;
        foreach (GridDataItem item in grdDocu.Items) {
            a = Convert.ToDecimal(((TextBox)item.FindControl("txtSaldoAnterior")).Text) - Convert.ToDecimal(((TextBox)item.FindControl("txtIportePagado")).Text);
            ((Label)item.FindControl("lblSaldoActual")).Text = a.ToString();
            total = Convert.ToDecimal(((TextBox)item.FindControl("txtIportePagado")).Text);
        }

        ((Label)fvwResumen.Row.FindControl("lblTotal")).Text = total.ToString();
    }
}