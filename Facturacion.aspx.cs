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

public partial class Facturacion_Facturacion : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();
    Ejecuciones ejecuta = new Ejecuciones();
    public DataTable dt;
    private bool bolErrConcpto = false;
    private int pasos;
    string status;
    string noReceptor;
    protected void Page_Load(object sender, EventArgs e)
    {
        obtieneSesiones();

        if (!IsPostBack)
        {
            try
            {
                lblModo.Text = "C";
                lblIdEmisor.Text = lblEmisorFacturas.Text = lblIdMonedaFac.Text = "1";
                string idRecep = obtieneIdReceptor(Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"]);
                lblReceptorFactura.Text = idRecep;
                cargaDatosPie();
                lblError.Text = "";
                grdEmisores.SelectedIndex = 0;
                //PopUpEmisores
                cargaDatos();
                llenaInfoEncFactura();                
                grdReceptores.SelectedIndex = 0;
                cargaDatosRecep();

                com.formulasistemas.www.ManejadordeTimbres folios = new com.formulasistemas.www.ManejadordeTimbres();
                int foliosDisponibles = folios.ObtieneFoliosDisponibles(lblRfcEmisor.Text.Trim().ToUpper());
                lblError.Text = foliosDisponibles.ToString() + " Folios Disponibles";

                status = "P";

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

                //genera n filas
                DataRow dr = default(DataRow);
                for (int i = 1; i <= 2; i++)
                {
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }

                if (Convert.ToInt32(Request.QueryString["fact"]) == 0)
                {
                    pnlOperacionesFact.Visible = true;
                    txtFormaPago.Text = "PAGO EN UNA SOLA EXHIBICION";
                    txtCondicionesPago.Text = "CONTADO";
                    txtMetodoPago.Text = "NO IDENTIFICADO";
                    txtRegimenFac.Text = "PERSONAS MORALES DEL REGIMEN GENERAL";
                    txtCtaPago.Text = "";
                    lblDescripcionMoneda.Text = "PESOS";
                    lblAbreviatura.Text = "MN";
                    txtTipoCambio.Text = "1.00";
                    string notasFact = "";
                    string referenciasFact = "";
                    txtGlobal1.Text = txtGlobal2.Text = "";
                    txtGlobal1.Visible = txtGlobal2.Visible = false;
                    try { ddlDesglose.SelectedValue = "1"; } catch (Exception) { ddlDesglose.SelectedIndex = -1; }
                    try
                    {
                        int empresa = Convert.ToInt32(Request.QueryString["e"]);
                        int taller = Convert.ToInt32(Request.QueryString["t"]);
                        int orden = Convert.ToInt32(Request.QueryString["o"]);
                        object[] datosOrden = recepciones.obtieneInfoOrdenPie(orden, empresa, taller);
                        object[] prefijoTaller = recepciones.obtienePrefijoTaller(Request.QueryString["t"]);
                        DatosVehiculos vehi = new DatosVehiculos();
                        object[] datosVehiculo = vehi.obtieneDatosBasicosVehiculoNotaFactura(orden, empresa, taller);
                        if (Convert.ToBoolean(prefijoTaller[0]))
                            referenciasFact = Convert.ToString(prefijoTaller[1]).Trim().ToUpper() + Request.QueryString["o"].Trim();
                        if (Convert.ToBoolean(datosOrden[0]))
                        {
                            object[] infoOrden = null;
                            DataSet ordenDatos = (DataSet)datosOrden[1];
                            foreach (DataRow fila in ordenDatos.Tables[0].Rows)
                            {
                                infoOrden = fila.ItemArray;
                            }
                            if (Convert.ToBoolean(datosVehiculo[0]))
                            {
                                object[] datosVehiculoOrden = null;
                                DataSet infoVehi = (DataSet)datosVehiculo[1];
                                foreach (DataRow filav in infoVehi.Tables[0].Rows)
                                {
                                    datosVehiculoOrden = filav.ItemArray;
                                }
                                if (datosVehiculoOrden != null && infoOrden != null)
                                {
                                    notasFact = string.Format("ORDEN " + Convert.ToString(prefijoTaller[1]).Trim().ToUpper() + Request.QueryString["o"].Trim() + "{0}" +
                                    "UNIDAD " + Convert.ToString(datosVehiculoOrden[1]).Trim().ToUpper() + " " + Convert.ToString(datosVehiculoOrden[2]).ToUpper().ToUpper().Trim() + " MODELO " + Convert.ToString(datosVehiculoOrden[3]).ToUpper().ToUpper().Trim() + " PLACAS " + Convert.ToString(datosVehiculoOrden[4]).ToUpper().ToUpper().Trim() + "{0}" +
                                    "NO. PÓLIZA " + Convert.ToString(infoOrden[17]).ToUpper().Trim() + " NO. SINIESTRO " + Convert.ToString(infoOrden[9]).ToUpper().Trim() + "{0}" +
                                    "PROPIEDAD DE " + Convert.ToString(datosVehiculoOrden[5]).Trim().ToUpper() + "{0}" +
                                    "NO. SERIE " + Convert.ToString(datosVehiculoOrden[6]).Trim().ToUpper() + "{0}" + "NO. PROVEEDOR " + noReceptor, Environment.NewLine);
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                    txtNotaFac.Text = notasFact.Trim();
                    txtReferenciasFac.Text = referenciasFact;
                    txtFolioImp.Text = Request.QueryString["o"];
                    lnkTimbrar.Visible = false;
                }
                else
                {
                    pnlOperacionesFact.Visible = false;
                    if (pasos == 0)
                    {
                        cargaDatosFacturaPrevia(Convert.ToInt32(Request.QueryString["fact"]));
                        llenaInfoEncFactura();
                        lnkTimbrar.Visible = true;
                    }
                }

                if (status != "P" )
                {

                    if (Request.QueryString["fact"] != "0" && Request.QueryString["refct"] != "1")
                    {
                        pnlOperacionesFact.Visible = false;
                        lnkBuscar.Visible = false;
                        lnkBuscaRec.Visible = false;
                        lnkBuscaMonedas.Visible = false;
                        multiPagina.PageViews[3].Enabled = multiPagina.PageViews[4].Enabled = false;
                        fvwResumen.Enabled = false;
                        txtFormaPago.Enabled = txtCondicionesPago.Enabled = txtMetodoPago.Enabled = txtRegimenFac.Enabled = txtCtaPago.Enabled = true;
                        lnkTimbrar.Visible = false;
                        grdDocu.MasterTableView.Columns[6].Visible = false;
                        grdDocu.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                        grdDocu.MasterTableView.CommandItemSettings.ShowSaveChangesButton = false;
                        grdDocu.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
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
                    noReceptor = catClientes.obtieneNumeroProveedor(idCliprov, "P");
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


    // Ventana Emisores
    private void cargaDatos()
    {
        lblErrorEmisores.Text = "";
        string sql = "select IdEmisor,EmRFC,EmNombre from emisores ";
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
        string sql = "select IdRecep,ReRFC,ReNombre from receptores ";
        string condicion = "";
        if (txtBuscarRec.Text != "")
            condicion = " where ReRFC like '%" + txtBuscarRec.Text.Trim() + "%' or ReNombre like '%" + txtBuscarRec.Text.Trim() + "%'";
        sql = sql + condicion;
        FacturacionElectronica.Ejecucion ejecutaFact = new FacturacionElectronica.Ejecucion();
        ejecutaFact.baseDatos = "PVW";

        object[] info = ejecutaFact.dataSet(sql);
        if (Convert.ToBoolean(info[0])) {
            DataSet infoRecp = (DataSet)info[1];
            grdReceptores.DataSource = infoRecp;
        }else
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
            if (lblReceptorFactura.Text != "0" && lblReceptorFactura.Text== grdReceptores.DataKeys[grdReceptores.SelectedIndex].Value.ToString())
                Receptor.idReceptor = Convert.ToInt32(lblReceptorFactura.Text);
            else
                Receptor.idReceptor = Convert.ToInt32(grdReceptores.DataKeys[grdReceptores.SelectedIndex].Value.ToString());
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
                        if (txtRfc.Text.Length == 12)
                            rbtnPersona.SelectedValue = "M";
                        else
                            rbtnPersona.SelectedValue = "F";

                        if (rbtnPersona.SelectedValue == "M")
                            txtRfc.MaxLength = 12;
                        else
                            txtRfc.MaxLength = 13;

                        txtRazonNew.Text = fila[2].ToString();
                        



                        try { ddlPais.SelectedValue = fila[6].ToString(); } catch (Exception) {
                            ddlPais.Items.Add(new RadComboBoxItem(fila[6].ToString(), fila[6].ToString()));
                            ddlPais.SelectedValue = fila[6].ToString();
                        }
                        try
                        {
                            ddlEstado.SelectedValue = fila[8].ToString();
                        }
                        catch (Exception) {
                            ddlEstado.Items.Add(new RadComboBoxItem(fila[8].ToString(), fila[8].ToString()));
                            ddlEstado.SelectedValue = fila[8].ToString();
                        }
                        try
                        {
                            ddlMunicipio.SelectedValue = fila[10].ToString();
                        }
                        catch (Exception) {
                            ddlMunicipio.Items.Add(new RadComboBoxItem(fila[10].ToString(), fila[10].ToString()));
                            ddlMunicipio.SelectedValue = fila[10].ToString();
                        }
                        try { ddlColonia.SelectedValue = fila[12].ToString(); } catch (Exception) {
                            ddlColonia.Items.Add(new RadComboBoxItem(fila[12].ToString(), fila[12].ToString()));
                            ddlColonia.SelectedValue = fila[12].ToString();
                        }
                        try { ddlCodigo.SelectedValue = fila[14].ToString(); } catch (Exception) {
                            ddlCodigo.Items.Add(new RadComboBoxItem(fila[14].ToString(), fila[14].ToString()));
                            ddlCodigo.SelectedValue = fila[14].ToString();
                        }

                        etiquetas[0].Text = txtCalle.Text = fila[3].ToString();
                        etiquetas[1].Text = txtNoExt.Text = fila[4].ToString();
                        etiquetas[2].Text=txtNoIntMod.Text = fila[5].ToString();
                        etiquetas[3].Text = fila[13].ToString();
                        etiquetas[4].Text = fila[11].ToString();
                        etiquetas[5].Text = fila[9].ToString();
                        etiquetas[6].Text = fila[5].ToString();
                        etiquetas[7].Text = fila[14].ToString();
                        etiquetas[8].Text = txtLocalidad.Text= fila[15].ToString();
                        etiquetas[9].Text =txtReferenciaMod.Text= fila[16].ToString();
                        etiquetas[10].Text=txtCorreo.Text = fila[17].ToString();
                        etiquetas[11].Text=txtCorreoCC.Text = fila[18].ToString();
                        etiquetas[12].Text=txtCorreoCCO.Text = fila[19].ToString();
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

    protected void PreventErrorOnbinding(object sender, EventArgs e)
    {
        RadComboBox cb = sender as RadComboBox;
        cb.DataBinding -= new EventHandler(PreventErrorOnbinding);
        cb.AppendDataBoundItems = true;

        try
        {
            cb.DataBind();
            cb.Items.Clear();
        }
        catch (ArgumentOutOfRangeException)
        {
            cb.Items.Clear();
            cb.ClearSelection();
            RadComboBoxItem cbI = new RadComboBoxItem("", "0");
            cbI.Selected = true;
            cb.Items.Add(cbI);
        }
    }
    protected void lnkBusquedaRec_Click(object sender, EventArgs e)
    {
        grdReceptores.SelectedIndex = 0;
        cargaDatosRecep();
    }
    protected void lnkLimpiarRec_Click(object sender, EventArgs e)
    {
        txtBuscarRec.Text = "";
        cargaDatosRecep();
    }
    protected void grdReceptores_PageIndexChanged(object sender, EventArgs e)
    {
        grdReceptores.SelectedIndex = 0;
        cargaDatosRecep();
    }
    protected void grdReceptores_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargaDatosRecep();
    }
    protected void lnkSeleccionarReceptor_Click(object sender, EventArgs e)
    {
        lblReceptorFactura.Text = lblIdReceptor.Text;
        llenaInfoEncFactura();
        string script = "cierraWinRec()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }
    
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
        finally {
            if (PctjeDsctoGlb != 0)
            {
                ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = true;
                ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = true;
            }
        }
    }
    
    //Conceptos

    private DataTable AddRow(DataTable dt)
    {
        DataRow dr = dt.NewRow();
        dr["idFila"] = "";
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
            dt.Columns.Add(new DataColumn("idFila", typeof(string)));
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

            /**Factura Nueva sin info*/
            if (rdlOpcionesFactura.SelectedValue == "0")
            {
                dt = (DataTable)ViewState["dt"];
                DataRow dr = default(DataRow);
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                //ViewState["dt"] = dt;
                grdDocu.DataSource = dt;
                Session["info"] = dt;
            }
            else
            {
                int[] sesiones = obtieneSesiones();
                try
                {
                    try { dt = (DataTable)Session["info"]; } catch (Exception) { dt = null; }
                    if (dt != null)
                    {
                        DataRow dr = default(DataRow);
                        dr = dt.NewRow();
                        dt.Rows.Add(dr);
                        ViewState["dt"] = dt;
                        grdDocu.DataSource = dt;
                        Session["info"] = dt;
                    }
                    /*
                    //carga info al agregar new item
                    object[] conceptos = recepciones.obtieneInfoFacturar(sesiones, rdlOpcionesFactura.SelectedValue);
                    if (Convert.ToBoolean(conceptos[0]))
                    {
                        DataSet conceptosFacturar = (DataSet)conceptos[1];
                        dt = conceptosFacturar.Tables[0];
                        
                        DataRow dr = default(DataRow);
                        dr = dt.NewRow();
                        dt.Rows.Add(dr);

                        ViewState["dt"] = dt;
                        grdDocu.DataSource = dt;                        
                        Session["info"] = dt;
                    }*/
                }
                catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
            }
        }
    }

    protected void grdDocu_PreRender(object sender, EventArgs e)
    {
        GridTableView masterTable = (sender as RadGrid).MasterTableView;
        GridColumn ConceptoColumn = masterTable.GetColumnSafe("TemplateColumn2") as GridColumn;        
    }

    protected void grdDocu_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (status != "P")
        {
            Dictionary<string, string>[] valores = new Dictionary<string, string>[] { };
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
                    using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
                    {
                        try
                        {
                            conLoc.Open();
                            string qryBorra = "DELETE FROM DocumentoCfdi WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;
                            string qryInserta = "INSERT INTO DocumentoCfdi (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera) " +
                                "VALUES (@IdFila, @IdEmisor, @IdRecep, @txtIdent, @txtConcepto, @radnumCantidad, @ddlUnidad, @txtValUnit, @lblImporte, @txtPtjeDscto, @txtDscto, @lblSubTotal, @ddlIvaTras, @ddlIeps, @lblIvaTras, @lblIeps, @ddlIvaRet, @ddlIsrRet, @lblIvaRet, @lblIsrRet, @lblTotal, @EncFechaGenera) ";
                                
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
                if (status != "T" && status != "C" || Request.QueryString["refct"] == "1")
                {
                    if (((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text == "" && Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text) != 0 || ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text == "" && Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text) != 0)
                    {
                        string mensaje = string.Format("alert('Debe indicar el motivo de descuento');");
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Scritpt", mensaje, true);
                    }
                    else
                    {
                        docuCfdi docCfd = new docuCfdi(int.Parse(lblEmisorFacturas.Text), int.Parse(lblReceptorFactura.Text), 1);
                        //docCfd.IdMoneda = Convert.ToInt32(lblIdMonedaFac.Text);
                        docCfd.strEmRfc = lblRfcEmisor.Text;
                        string strReRfcNom = lblRfcReceptor.Text;
                        docCfd.IdTipoDoc = 2;
                        docCfd.strReRfc = strReRfcNom.Substring(0, 13).Trim();
                        docCfd.decEncDescGlob = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);
                        docCfd.decEncDescMO = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtMODesGlob")).Text);
                        docCfd.decEncDescRefaccion = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtRefDesGlob")).Text);
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
                        string lugarExpedicion = "";

                        lugarExpedicion = lugarExpedicion.Trim() + lblCalleEmiExFac.Text.Trim().ToUpper() + " No. Ext. " + lblNoExtEmiExFac.Text.Trim().ToUpper();
                        if (lblNoIntEmiExFac.Text != "")
                            lugarExpedicion = lugarExpedicion.Trim() + " No. Int. " + lblNoIntEmiExFac.Text.Trim().ToUpper();
                        lugarExpedicion = lugarExpedicion + ", Col. " + lblColoniaEmiExFac.Text.Trim().ToUpper() + ", C.P. " + lblCpEmiExFac.Text.Trim().ToUpper() + ", " + lblDelMunEmiExFac.Text.Trim().ToUpper() + ", " + lblEdoEmiExFac.Text.Trim().ToUpper() + ", " + lblPaisEmiExFac.Text.Trim().ToUpper();

                        docCfd.strEncLugarExpedicion = lugarExpedicion.Trim();

                        if (Request.QueryString["refct"] == "1" || Request.QueryString["refct"] == "0")
                        {
                            object[] infoFacura = recepciones.obtieneUltimaFacturaTaller(Request.QueryString["e"], Request.QueryString["t"]);
                            if (Convert.ToBoolean(infoFacura[0]))
                                docCfd.strEncReferencia = txtReferenciasFac.Text + "-" + Convert.ToInt32(infoFacura[1]).ToString();
                            else
                                docCfd.strEncReferencia = txtReferenciasFac.Text;
                        }
                        else if (Request.QueryString["refct"] == "2")
                        {
                            docCfd.strEncReferencia = txtReferenciasFac.Text;
                        }
                        docCfd.strEncFolioImp = float.Parse(Request.QueryString["o"]);
                        docCfd.strEncRegimen = txtRegimenFac.Text;
                        if (Request.QueryString["refct"] == "1" || Request.QueryString["refct"] == "0")
                            docCfd.idCfdAnt = 0;
                        else
                            docCfd.idCfdAnt = Convert.ToInt32(Request.QueryString["fact"]);

                        try
                        {
                            object[] prefijoTaller = recepciones.obtienePrefijoTaller(Request.QueryString["t"]);
                            if (Convert.ToBoolean(prefijoTaller[0]))
                                docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-T" + Request.QueryString["t"] + "-OT-" + Convert.ToString(prefijoTaller[1]).Trim() + Request.QueryString["o"] + "-TF" + rdlOpcionesFactura.SelectedValue;
                            else
                                docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-TF" + rdlOpcionesFactura.SelectedValue;
                        }
                        catch (Exception)
                        {
                            docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-T" + Request.QueryString["t"] + "-OT-" + Request.QueryString["o"] + "-TF" + rdlOpcionesFactura.SelectedValue;
                        }

                        docCfd.tipoFactura = ddlTipoFactura.SelectedValue;

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
                        string scriptMnsj;
                        if (Convert.ToBoolean(result[0]))
                        {
                            Refacciones refacciones = new Refacciones();
                            refacciones.actualizaFacturados(result[1].ToString(), Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"], "P");
                            if (Convert.ToInt32(result[1].ToString()) > 0)
                            {
                                docCfd.IdCfd = Convert.ToInt32(result[1].ToString());
                                docCfd.actualizaTipoFactura();

                                Facturas facturas = new Facturas();
                                facturas.folio = Convert.ToInt32(Request.QueryString["o"]);
                                facturas.tipoCuenta = "CC";
                                facturas.factura = docCfd.strEncReferencia;
                                CatClientes clientes = new CatClientes();
                                string politica = clientes.obtieneClavePoliticaCliente(lblReceptorFactura.Text);
                                int diasPlazo = clientes.obtieneDiasPolitica(lblReceptorFactura.Text);
                                facturas.fechaRevision = fechas.obtieneFechaLocal();
                                facturas.fechaProgPago = fechas.obtieneFechaLocal().AddDays(Convert.ToDouble(diasPlazo));
                                facturas.id_cliprov = Convert.ToInt32(lblReceptorFactura.Text);
                                facturas.formaPago = "E";
                                facturas.politica = politica;
                                facturas.estatus = "PEN";
                                facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
                                facturas.taller = Convert.ToInt32(Request.QueryString["t"]);
                                facturas.tipoCargo = "I";
                                facturas.Importe = docCfd.decEncTotal;
                                facturas.orden = Convert.ToInt32(Request.QueryString["o"]);
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

                                if (Request.QueryString["refct"] == "0" || Request.QueryString["refct"] == "1")
                                {
                                    facturas.idCfd = Convert.ToInt32(result[1].ToString());
                                    facturas.generaFacturaCC();
                                }
                                else
                                {
                                    facturas.idCfd = Convert.ToInt32(Request.QueryString["fact"]);
                                    facturas.actualizaFacturaCC();
                                }
                                object[] facturasInternas = facturas.retorno;
                                /*if (!Convert.ToBoolean(facturasInternas[0]))
                                    facturas.actualizaFactura();*/
                                Operarios operarios = new Operarios();
                                int[] sessiones = obtieneSesiones();
                                operarios.empresa = sessiones[2];
                                operarios.taller = sessiones[3];
                                operarios.orden = sessiones[4];
                                operarios.liberarCajones();
                                object[] op = operarios.retorno;
                                if (Convert.ToBoolean(op[0])) {
                                    DataSet info = (DataSet)op[1];
                                    foreach (DataRow r in info.Tables[0].Rows) {
                                        string[] valoresOperarios = new string[5] { Convert.ToString(r[0]), Convert.ToString(r[1]), Convert.ToString(r[2]), Convert.ToString(r[3]), Convert.ToString(r[4]) };
                                        operarios.liberar(sessiones, valoresOperarios);
                                    }
                                }

                            }
                            scriptMnsj = string.Format("alert('Se ha guardado el documento: {0}');", result[1].ToString());
                            Response.Redirect("FacturasOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
                        }
                        else
                            scriptMnsj = string.Format("alert('Hubo un problema al guardar el documento: {0}');", result[1].ToString());
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

    protected void grdDocu_ItemDataBound(object sender, GridItemEventArgs e)
    {
        string IdctrlPostBack = getPostBackControlName();
        string ctrlPostBack = "";
        if (!string.IsNullOrEmpty(IdctrlPostBack))
            ctrlPostBack = IdctrlPostBack.Substring(IdctrlPostBack.LastIndexOf('$') + 1);
        if (e.Item is GridDataItem)
        {
            string encSerie = obtieneEncSerie();
            if (encSerie != "0")
            {
                DataRowView filas = (DataRowView)e.Item.DataItem;
                DataRow r = filas.Row;
                string sql = "";
                int existe = 0;
                SqlConnection conexionBDeFactura = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
                int countR = 0;
                if (e.Item is GridDataItem && (ctrlPostBack == "InitInsertButton" || ctrlPostBack == "AddNewRecordButton"))
                    countR = 0;
                else
                    countR = 3;
                if (r[countR].ToString().Split('-')[0] == "MO")
                {
                    sql = "select count(*) " +
                        "from EncCFD e, DetCFD d " +
                        "where e.EncSerieImpresion like '%" + encSerie + "%' and d.IdCfd=e.IdCfd and e.EncEstatus<>'C' and d.IdConcepto like 'MO%' and d.DetDesc like '%" + r[4].ToString() + "%'";
                    try
                    {
                        conexionBDeFactura.Open();
                        SqlCommand cmd = new SqlCommand(sql, conexionBDeFactura);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                        conexionBDeFactura.Close();
                    }
                    catch (Exception)
                    {
                        existe = 0;
                        conexionBDeFactura.Close();
                    }
                }
                else if (r[countR].ToString().Split('-')[0] == "REF")
                {
                    string idordId = r[countR].ToString().Split('-')[1];

                    sql = "select count(*) " +
                    "from EncCFD e, DetCFD d " +
                    "where e.EncSerieImpresion like '%" + encSerie + "%' and d.IdCfd=e.IdCfd and e.EncEstatus<>'C' and d.IdConcepto like 'REF%' AND SUBSTRING(d.IdConcepto,5,26)= '" + idordId + "' ";
                    try
                    {
                        conexionBDeFactura.Open();
                        SqlCommand cmd = new SqlCommand(sql, conexionBDeFactura);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                        conexionBDeFactura.Close();
                    }
                    catch (Exception)
                    {
                        existe = 0;
                        conexionBDeFactura.Close();
                    }
                }
                if (existe > 0)
                    e.Item.CssClass = "alert-danger";
            }
        }

        if (e.Item is GridDataItem && (ctrlPostBack == "InitInsertButton" || ctrlPostBack == "AddNewRecordButton"))
        {
            if (status != "T" && status != "C")
            //if (status != "T")
            {
                int IdEmisor = Convert.ToInt32(lblEmisorFacturas.Text);
                int IdRecep = Convert.ToInt32(lblReceptorFactura.Text);
                int ItemIdx = e.Item.ItemIndex;

                PctjeDsctoGlb = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);

                SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString);
                string qrySelect = "SELECT IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera FROM DocumentoCfdi " +
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
                        ((TextBox)e.Item.FindControl("txtValUnit")).Text = dr["txtValUnit"].ToString();
                        ((Label)e.Item.FindControl("lblImporte")).Text = dr["lblImporte"].ToString();
                        ((TextBox)e.Item.FindControl("txtPtjeDscto")).Text = dr["txtPtjeDscto"].ToString();
                        ((TextBox)e.Item.FindControl("txtDscto")).Text = dr["txtDscto"].ToString();

                        decimal ImporteCpto = Convert.ToDecimal(((Label)e.Item.FindControl("lblImporte")).Text);
                        decimal desctoCpto = decimal.Parse(((TextBox)e.Item.FindControl("txtDscto")).Text);
                        if (ImporteCpto != 0)
                            subTotalCncpto = ((ImporteCpto - desctoCpto));
                        if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
                        {
                            dtoGlobConcepto = ((subTotalCncpto * PctjeDsctoGlb) / 100);
                            subTotalCncpto = subTotalCncpto - ((subTotalCncpto * PctjeDsctoGlb) / 100);
                            ((Label)e.Item.FindControl("lblDtoGlobalConcepto")).Text = dtoGlobConcepto.ToString("F2");
                        }

                        ((Label)e.Item.FindControl("lblSubTotal")).Text = dr["lblSubTotal"].ToString();
                        RadDropDownList ddlIvaTras = (RadDropDownList)e.Item.FindControl("ddlIvaTras");
                        try { ddlIvaTras.SelectedValue = dr["ddlIvaTras"].ToString(); } catch (Exception) { ddlIvaTras.SelectedIndex = -1; }
                        //valorCombo(ddlImp, dr["ddlImp"].ToString());
                        RadDropDownList ddlIeps = (RadDropDownList)e.Item.FindControl("ddlIeps");
                        try { ddlIeps.SelectedValue = dr["ddlIeps"].ToString(); } catch (Exception) { ddlIeps.SelectedIndex = -1; }
                        //valorCombo(ddlIeps, dr["ddlIeps"].ToString());
                        ((Label)e.Item.FindControl("lblIvaTras")).Text = dr["lblIvaTras"].ToString();
                        ((Label)e.Item.FindControl("lblIeps")).Text = dr["lblIeps"].ToString();
                        RadDropDownList ddlIvaRet = (RadDropDownList)e.Item.FindControl("ddlIvaRet");
                        try { ddlIvaRet.SelectedValue = dr["ddlIvaRet"].ToString(); } catch (Exception) { ddlIvaRet.SelectedIndex = -1; }
                        //valorCombo(ddlImp, dr["ddlImp"].ToString());
                        RadDropDownList ddlIsrRet = (RadDropDownList)e.Item.FindControl("ddlIsrRet");
                        try { ddlIeps.SelectedValue = dr["ddlIsrRet"].ToString(); } catch (Exception) { ddlIeps.SelectedIndex = -1; }
                        //valorCombo(ddlIeps, dr["ddlIeps"].ToString());
                        ((Label)e.Item.FindControl("lblIvaRet")).Text = dr["lblIvaRet"].ToString();
                        ((Label)e.Item.FindControl("lblIsrRet")).Text = dr["lblIsrRet"].ToString();
                        ((Label)e.Item.FindControl("lblTotalCpto")).Text = dr["lblTotal"].ToString();

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
            if (status != "P" && Request.QueryString["refct"] != "1")
            //if (status != "P" && status != "C")
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
            }
        }
    }

    private string obtieneEncSerie()
    {
        string serie = "";
        string noOrden = Request.QueryString["o"];
        object[] taller = recepciones.obtienePrefijoTaller(Request.QueryString["t"]);
        if ((bool)taller[0])
        {
            string identifTaller = Convert.ToString(taller[1]);
            serie = identifTaller.Trim() + noOrden;
            return serie;
        }
        else
            return "0";
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
            dtoGlobConcepto = ((subTotalCncpto * PctjeDsctoGlb) / 100);
            subTotalCncpto = subTotalCncpto - ((subTotalCncpto * PctjeDsctoGlb) / 100);
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
        ((Label)Item.FindControl("lblTotalCpto")).Text = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet).ToString("F2");
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
        ((Label)Item.FindControl("lblTotalCpto")).Text = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet).ToString("F2");
    }
    private void calculaIepsTras(GridDataItem item)
    {
        iepsTras = Convert.ToDecimal(((Label)item.FindControl("lblIeps")).Text);
        if (iepsTras != 0 && subTotalCncpto != 0 && ivaTras == 0)
        {
            BaseDatos bd = new BaseDatos();
            RadDropDownList ddlIepsTras = (RadDropDownList)item.FindControl("ddlIeps");
            string qryValIeps = "SELECT TrasTasa FROM ImpTrasladado WHERE Id_Tras ='" + ddlIepsTras.SelectedValue + "'";
            object[] valorIeps = bd.scalarToDecimal(qryValIeps);
            iepsTras = (subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100;
            ((Label)item.FindControl("lblIeps")).Text = iepsTras.ToString("F2");
        }
        else if (iepsTras != 0 && subTotalCncpto != 0 && ivaTras != 0)
        {
            BaseDatos bd = new BaseDatos();
            RadDropDownList ddlIvaTras = (RadDropDownList)item.FindControl("ddlIvaTras");
            string qryValIva = "SELECT TrasTasa FROM ImpTrasladado WHERE Id_Tras ='" + ddlIvaTras.SelectedValue + "'";
            object[] valorIva = bd.scalarInt(qryValIva);
            ivaTras = (subTotalCncpto * Convert.ToDecimal(valorIva[1])) / 100;

            RadDropDownList ddlIepsTras = (RadDropDownList)item.FindControl("ddlIeps");
            string qryValIeps = "SELECT TrasTasa FROM ImpTrasladado WHERE Id_Tras ='" + ddlIepsTras.SelectedValue + "'";
            object[] valorIeps = bd.scalarToDecimal(qryValIeps);
            iepsTras = (subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100;
            ivaTras = ivaTras + ((ivaTras * Convert.ToDecimal(valorIeps[1])) / 100); //(subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100;

            ((Label)item.FindControl("lblIvaTras")).Text = ivaTras.ToString("F2");
            ((Label)item.FindControl("lblIeps")).Text = iepsTras.ToString("F2");
        }
        ((Label)item.FindControl("lblTotalCpto")).Text = (subTotalCncpto + ivaTras + iepsTras).ToString("F2");
    }
    private void calculaIvaTras(GridDataItem item)
    {
        ivaTras = Convert.ToDecimal(((Label)item.FindControl("lblIvaTras")).Text);
        if (subTotalCncpto == 0)
            subTotalCncpto = Convert.ToDecimal(((Label)item.FindControl("txtValUnit")).Text) * int.Parse(((RadNumericTextBox)item.FindControl("radnumCantidad")).Value.ToString());

        if (subTotalCncpto != 0)
        {
            BaseDatos bd = new BaseDatos();
            RadDropDownList ddlIvaTras = (RadDropDownList)item.FindControl("ddlIvaTras");
            string qryValIva = "SELECT TrasTasa FROM ImpTrasladado WHERE Id_Tras ='" + ddlIvaTras.SelectedValue + "'";
            object[] valorIva = bd.scalarInt(qryValIva);
            ivaTras = (subTotalCncpto * Convert.ToDecimal(valorIva[1])) / 100;
            ((Label)item.FindControl("lblIvaTras")).Text = ivaTras.ToString("F2");
        }
        ((Label)item.FindControl("lblTotalCpto")).Text = (subTotalCncpto + ivaTras).ToString("F2");
    }

    protected void radnumCantidad_TextChanged(object sender, EventArgs e)
    {
        RadNumericTextBox radnumCant = (RadNumericTextBox)sender;
        GridTableCell cell = (GridTableCell)radnumCant.Parent;
        int intCant = int.Parse(radnumCant.Value.ToString());
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
        string qryValIva = "SELECT TrasTasa FROM ImpTrasladado WHERE Id_Tras ='" + e.Value + "'";
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
            string qryValIeps = "SELECT TrasTasa FROM ImpTrasladado WHERE Id_Tras ='" + ddlIepsTras.SelectedValue + "'";
            object[] valorIeps = bd.scalarToDecimal(qryValIeps);
            iepsTras = (subTotalCncpto * Convert.ToDecimal(valorIeps[1])) / 100;
            ivaTras = ivaTras + ((ivaTras * Convert.ToDecimal(valorIeps[1])) / 100);
        }
        ivaRet = Convert.ToDecimal(((Label)Item.FindControl("lblIvaRet")).Text);
        isrRet = Convert.ToDecimal(((Label)Item.FindControl("lblIsrRet")).Text);
        ((Label)Item.FindControl("lblIvaTras")).Text = ivaTras.ToString("F2");
        ((Label)Item.FindControl("lblIeps")).Text = iepsTras.ToString("F2");
        ((Label)Item.FindControl("lblTotalCpto")).Text = (subTotalCncpto + ivaTras + iepsTras - ivaRet - isrRet).ToString("F2");
        calculaTotales();
    }
    protected void ddlIeps_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        BaseDatos bd = new BaseDatos();
        string qryValIeps = "SELECT TrasTasa FROM ImpTrasladado WHERE Id_Tras ='" + e.Value + "'";
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
            string qryValIva = "SELECT TrasTasa FROM ImpTrasladado WHERE Id_Tras ='" + ddlIvaTras.SelectedValue + "'";
            object[] valorIva = bd.scalarInt(qryValIva);
            ivaTras = (subTotalCncpto * Convert.ToDecimal(valorIva[1])) / 100;
            ivaTras = ivaTras + ((ivaTras * Convert.ToDecimal(valorIeps[1])) / 100);
        }

        ivaRet = Convert.ToDecimal(((Label)Item.FindControl("lblIvaRet")).Text);
        isrRet = Convert.ToDecimal(((Label)Item.FindControl("lblIsrRet")).Text);
        ((Label)Item.FindControl("lblIvaTras")).Text = ivaTras.ToString("F2");
        ((Label)Item.FindControl("lblIeps")).Text = iepsTras.ToString("F2");
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
        lblTotalCpto.Text = (subTotalCncpto + ivaTras + iepsTras - totImpRet).ToString("F2");
        calculaTotales();
    }

    private void CalculaSubTotBruto(decimal subTotConcepto)
    {
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
        totDscto = 0;
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
                subTotalCncpto = ((ImporteCpto - desctoCpto));
            if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
            {
                dtoGlobConcepto = ((subTotalCncpto * PctjeDsctoGlb) / 100);
                subTotalCncpto = subTotalCncpto - ((subTotalCncpto * PctjeDsctoGlb) / 100);
            }
            ((Label)item.FindControl("lblDtoGlobalConcepto")).Text = dtoGlobConcepto.ToString("F2");
            ((Label)item.FindControl("lblSubTotal")).Text = subTotalCncpto.ToString("F2");
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

    protected void lnkCargarInfo_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblMnsjs.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            int IDEmisor = Convert.ToInt32(lblEmisorFacturas.Text);
            int IdRecep = Convert.ToInt32(lblReceptorFactura.Text);
            
            int filasElim = 0;
            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
            {
                try
                {
                    conLoc.Open();
                    string qryBorra = "DELETE FROM DocumentoCfdi WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;
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
                int facturaPrevia = 0;
                try { facturaPrevia = Convert.ToInt32(Request.QueryString["prev"]); } catch (Exception) { facturaPrevia = 0; }
                if (facturaPrevia == 0)
                {
                    object[] conceptos = recepciones.obtieneInfoFacturar(sesiones, rdlOpcionesFactura.SelectedValue);
                    if (Convert.ToBoolean(conceptos[0]))
                    {
                        DataSet conceptosFacturar = (DataSet)conceptos[1];
                        int filas = 1;
                        foreach (DataRow r in conceptosFacturar.Tables[0].Rows)
                        {
                            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
                            {
                                try
                                {
                                    FacturacionElectronica.Unidades unidad = new FacturacionElectronica.Unidades();
                                    unidad.descUnid = r[3].ToString().ToUpper().Trim();
                                    unidad.obtieneIdUnidad();
                                    object[] unidadades = unidad.info;
                                    int idUnidad = 0;
                                    try
                                    {
                                        if (Convert.ToBoolean(unidadades[0]))
                                            idUnidad = Convert.ToInt32(unidadades[1]);
                                        else
                                            idUnidad = 0;
                                    }
                                    catch (Exception) { idUnidad = 0; }

                                    conLoc.Open();
                                    string qryInserta = "INSERT INTO DocumentoCfdi (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera) " +
                                        "VALUES (" + filas + ",'" + IDEmisor + "' , '" + IdRecep + "', '" + r[0].ToString().Trim() + "', '" + r[1].ToString().Trim() + "', " + r[2].ToString() + ", " + idUnidad + ", " + Convert.ToDecimal(r[4]).ToString("F2") + ", " + Convert.ToDecimal(r[5]).ToString("F2") + ", 0, 0, " + Convert.ToDecimal(r[5]).ToString("F2") + ",2, 0, " + r[7].ToString() + ", 0, 0, 0, 0, 0, " + r[8].ToString() + ", convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120))";
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
                                    lblMnsjs.Text = "Error LocalDB insersion detalle tipo factura: " + ex.Source + " - " + ex.Message;
                                }
                                filas++;
                            }
                        }
                    }
                    else
                        lblError.Text = "Error: " + Convert.ToString(conceptos[1]);
                }
                else {
                    object[] conceptos = recepciones.obtieneInfoFacturarPrev(sesiones[0], IDEmisor, IdRecep);
                    
                }
                LlenaInfoDetalle(IDEmisor, IdRecep);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.ToString();
        }

    }

    private void LlenaInfoDetalle(int IDEmisor, int IdRecep)
    {
        DataSet ds = new DataSet();
        try
        {
            string qrySelect = "";
            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
            {
                try
                {
                    conLoc.Open();
                    if (rdlOpcionesFactura.SelectedValue == "1")
                    {
                        if (ddlDesglose.SelectedValue == "2") { qrySelect = string.Format("SELECT 1 as IdFila,IdEmisor,IdRecep,'GG-01','{0}',1,5,SUM(CAST(lblImporte AS decimal(15,2))),SUM(CAST(lblImporte AS decimal(15,2))),SUM(CAST(txtPtjeDscto AS decimal(5,2))),SUM(CAST(txtDscto AS decimal(15,2))),SUM(CAST(lblSubTotal AS decimal(15,2))),2,0,SUM(CAST(lblIvaTras AS decimal(15,2))),SUM(CAST(lblIeps AS decimal(15,2))),0,0,SUM(CAST(lblIvaRet AS decimal(15,2))),SUM(CAST(lblIsrRet AS decimal(15,2))),SUM(CAST(lblTotal AS decimal(15,2))),convert(char(10),EncFechaGenera,120) as EncFechaGenera  FROM DocumentoCfdi  where idemisor={1} and idrecep={2}  GROUP BY IdEmisor,IdRecep,convert(char(10),EncFechaGenera,120)", txtGlobal1.Text, IDEmisor, IdRecep); }
                        else if (ddlDesglose.SelectedValue == "3") { qrySelect = string.Format("SELECT 1 as IdFila,IdEmisor,IdRecep,'MO-01','{0}',1,5,SUM(CAST(txtValUnit AS decimal(15,2))),SUM(CAST(lblImporte AS decimal(15,2))),SUM(CAST(txtPtjeDscto AS decimal(5,2))),SUM(CAST(txtDscto AS decimal(15,2))),SUM(CAST(lblSubTotal AS decimal(15,2))),2,0,SUM(CAST(lblIvaTras AS decimal(15,2))),SUM(CAST(lblIeps AS decimal(15,2))),0,0,SUM(CAST(lblIvaRet AS decimal(15,2))),SUM(CAST(lblIsrRet AS decimal(15,2))),SUM(CAST(lblTotal AS decimal(15,2))),convert(char(10),EncFechaGenera,120) as EncFechaGenera FROM DocumentoCfdi where idemisor={1} and idrecep={2} AND SUBSTRING(txtIdent,1,2)='MO' GROUP BY IdEmisor,IdRecep,convert(char(10),EncFechaGenera,120) UNION ALL SELECT 2,IdEmisor,IdRecep,'REF-01','{3}',1,14,SUM(CAST(lblImporte AS decimal(15,2))),SUM(CAST(lblImporte AS decimal(15,2))),SUM(CAST(txtPtjeDscto AS decimal(5,2))),SUM(CAST(txtDscto AS decimal(15,2))),SUM(CAST(lblSubTotal AS decimal(15,2))),2,0,SUM(CAST(lblIvaTras AS decimal(15,2))),SUM(CAST(lblIeps AS decimal(15,2))),0,0,SUM(CAST(lblIvaRet AS decimal(15,2))),SUM(CAST(lblIsrRet AS decimal(15,2))),SUM(CAST(lblTotal AS decimal(15,2))),convert(char(10),EncFechaGenera,120) as EncFechaGenera FROM DocumentoCfdi where idemisor={1} and idrecep={2} AND SUBSTRING(txtIdent,1,3)='REF' GROUP BY IdEmisor,IdRecep,convert(char(10),EncFechaGenera,120)", txtGlobal1.Text, IDEmisor, IdRecep, txtGlobal2.Text); }
                        else { qrySelect = "select * FROM DocumentoCfdi WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep; }
                    }
                    else if (rdlOpcionesFactura.SelectedValue == "2")
                    {
                        if (ddlDesglose.SelectedValue == "2")
                        {
                            qrySelect = string.Format("SELECT 1 as IdFila,IdEmisor,IdRecep,'MO-01','{0}',1,5,SUM(CAST(txtValUnit AS decimal(15,2))),SUM(CAST(lblImporte AS decimal(15,2))),SUM(CAST(txtPtjeDscto AS decimal(5,2))),SUM(CAST(txtDscto AS decimal(15,2))),SUM(CAST(lblSubTotal AS decimal(15,2))),2,0,SUM(CAST(lblIvaTras AS decimal(15,2))),SUM(CAST(lblIeps AS decimal(15,2))),0,0,SUM(CAST(lblIvaRet AS decimal(15,2))),SUM(CAST(lblIsrRet AS decimal(15,2))),SUM(CAST(lblTotal AS decimal(15,2))),convert(char(10),EncFechaGenera,120) as EncFechaGenera  FROM DocumentoCfdi  where idemisor={1} and idrecep={2}  GROUP BY IdEmisor,IdRecep,convert(char(10),EncFechaGenera,120)", txtGlobal1.Text, IDEmisor, IdRecep);
                        }
                        else { qrySelect = "select * FROM DocumentoCfdi WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep + " AND SUBSTRING(txtIdent,1,2)='MO'"; }
                    }
                    else if (rdlOpcionesFactura.SelectedValue == "3")
                    {
                        if (ddlDesglose.SelectedValue == "2")
                        {
                            qrySelect = string.Format("SELECT 1 as IdFila,IdEmisor,IdRecep,'REF-01','{0}',1,14,SUM(CAST(lblImporte AS decimal(15,2))),SUM(CAST(lblImporte AS decimal(15,2))),SUM(CAST(txtPtjeDscto AS decimal(5,2))),SUM(CAST(txtDscto AS decimal(15,2))),SUM(CAST(lblSubTotal AS decimal(15,2))),2,0,SUM(CAST(lblIvaTras AS decimal(15,2))),SUM(CAST(lblIeps AS decimal(15,2))),0,0,SUM(CAST(lblIvaRet AS decimal(15,2))),SUM(CAST(lblIsrRet AS decimal(15,2))),SUM(CAST(lblTotal AS decimal(15,2))),convert(char(10),EncFechaGenera,120) as EncFechaGenera  FROM DocumentoCfdi  where idemisor={1} and idrecep={2}  GROUP BY IdEmisor,IdRecep,convert(char(10),EncFechaGenera,120)", txtGlobal2.Text, IDEmisor, IdRecep);
                        }
                        else { qrySelect = "select * FROM DocumentoCfdi WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep + " AND SUBSTRING(txtIdent,1,3)='REF'"; }
                    }
                    else
                        qrySelect = "select * FROM DocumentoCfdi WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;

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
            //carga grid al cargar toda la info btn 
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
                            try { ((RadDropDownList)fila.FindControl("ddlIvaTras")).SelectedValue = dato[12].ToString(); }
                            catch (Exception) { ((RadDropDownList)fila.FindControl("ddlIvaTras")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIvaTras")).Text = Convert.ToDecimal(dato[14].ToString()).ToString("F2");
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIeps")).SelectedValue = dato[13].ToString();
                            }
                            catch (Exception) { ((RadDropDownList)fila.FindControl("ddlIeps")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIeps")).Text = dato[15].ToString();
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIvaRet")).SelectedValue = dato[16].ToString();
                            }
                            catch (Exception) { ((RadDropDownList)fila.FindControl("ddlIvaRet")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIvaRet")).Text = dato[18].ToString();
                            try
                            { ((RadDropDownList)fila.FindControl("ddlIsrRet")).SelectedValue = dato[17].ToString(); }
                            catch (Exception) { ((RadDropDownList)fila.FindControl("ddlIsrRet")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIsrRet")).Text = dato[19].ToString();
                            ((Label)fila.FindControl("lblTotalCpto")).Text = Convert.ToDecimal(dato[20].ToString()).ToString("F2");

                            decimal desctoCpto = Convert.ToDecimal(dato[9]);
                            decimal impCpto = Convert.ToDecimal(dato[8]);
                            subTotalCncpto = impCpto - ((impCpto * desctoCpto) / 100);
                            if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
                            {
                                dtoGlobConcepto = ((subTotalCncpto * PctjeDsctoGlb) / 100);
                                ((Label)fila.FindControl("lblDtoGlobalConcepto")).Text = dtoGlobConcepto.ToString("F2");
                            }
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
        finally {
            if (PctjeDsctoGlb != 0)
            {
                ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = true;
                ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = true;
            }
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
                        lblIdMonedaFac.Text = ro[4].ToString();
                        txtFormaPago.Text = ro[5].ToString();
                        txtCondicionesPago.Text = ro[6].ToString();
                        txtMetodoPago.Text = ro[7].ToString();

                        PctjeDsctoGlb = Convert.ToDecimal(ro[8].ToString());
                        ((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text = ro[8].ToString();
                        ((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text = Convert.ToDecimal(ro[9].ToString()).ToString("F2");
                        ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Text = ro[10].ToString();
                        if (PctjeDsctoGlb != 0)
                        {
                            ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = true;
                            ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = true;
                        }
                        
                        //txtPctjeDsctoGlb.Text = ro[8].ToString();
                        //lblDsctoGlb.Text = Convert.ToDecimal(ro[9].ToString()).ToString("F2");
                        //txtMotivoDscto.Text = ro[10].ToString();
                        status = ro[11].ToString();
                        txtTipoCambio.Text = (Convert.ToDecimal(ro[12].ToString())).ToString();
                        txtNotaFac.Text = ro[13].ToString();
                        if (Request.QueryString["refct"] == "1")
                            txtReferenciasFac.Text = obtieneEncSerie();
                        else
                            txtReferenciasFac.Text = ro[14].ToString();
                        txtCtaPago.Text = ro[15].ToString();
                        txtRegimenFac.Text = ro[16].ToString();
                        txtFolioImp.Text = ro[18].ToString();
                        ddlTipoFactura.SelectedValue = ro[29].ToString();
                    }
                }
            }
            catch (Exception) { IdRecep = IDEmisor = 0; }

            if (IDEmisor != 0 && IdRecep != 0)
            {
                int filasElim = 0;
                using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
                {
                    try
                    {
                        conLoc.Open();
                        string qryBorra = "DELETE FROM DocumentoCfdi WHERE IdEmisor = " + IDEmisor + " AND IdRecep = " + IdRecep;
                        SqlCommand comLoc = new SqlCommand(qryBorra, conLoc);
                        using (comLoc)
                        {
                            filasElim = comLoc.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex) { filasElim = 0; lblError.Text = "Error: " + ex.Message; }
                }

                if (Request.QueryString["refct"] == "1")
                {
                    if (filasElim > -1)
                    {
                        Recepciones recepciones = new Recepciones();
                        object[] conceptos = recepciones.obtieneInfoFacturarPrev(sesiones[0], IDEmisor, IdRecep);
                        LlenaInfoDetalle(IDEmisor, IdRecep);
                        pasos++;
                    }
                }
                else {
                    factura.obtieneDetalle();
                    object[] conceptos = factura.info;
                    if (filasElim > -1)
                    {

                        if (Convert.ToBoolean(conceptos[0]))
                        {
                            DataSet conceptosFacturar = (DataSet)conceptos[1];
                            int filas = 1;
                            foreach (DataRow r in conceptosFacturar.Tables[0].Rows)
                            {
                                using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
                                {
                                    try
                                    {
                                        conLoc.Open();
                                        string qryInserta = "INSERT INTO DocumentoCfdi (IdFila, IdEmisor, IdRecep, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera) " +
                                            "VALUES (" + filas + ",'" + IDEmisor + "' , '" + IdRecep + "', '" + r[0].ToString() + "', '" + r[1].ToString() + "', " + r[2].ToString() + ", " + r[3].ToString() + ", " + Math.Round(Convert.ToDecimal(r[4].ToString()), 2) + ", " + Math.Round(Math.Round(Convert.ToDecimal(r[4].ToString()), 2) * Convert.ToDecimal(r[2].ToString()), 2) + ", " + r[6].ToString() + ", " + Math.Round(Convert.ToDecimal(r[7].ToString()), 2) + ", " + r[8].ToString() + "," + r[9].ToString() + ", " + r[10].ToString() + ", " + r[11].ToString() + ", " + r[12].ToString() + ", " + r[13].ToString() + ", " + r[14].ToString() + ", " + r[15].ToString() + ", " + r[16].ToString() + ", " + r[17].ToString() + ",convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120))";
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
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.ToString();
        }


    }

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
                    //docCfd.IdMoneda = Convert.ToInt32(lblIdMonedaFac.Text);
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
                        docCfd.strEncFolioImp = float.Parse(Request.QueryString["o"]);
                        docCfd.strEncRegimen = txtRegimenFac.Text;
                        docCfd.idCfdAnt = Convert.ToInt32(Request.QueryString["fact"]);
                        try
                        {
                            object[] prefijoTaller = recepciones.obtienePrefijoTaller(Request.QueryString["t"]);
                            if (Convert.ToBoolean(prefijoTaller[0]))
                                docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-T" + Request.QueryString["t"] + "-OT-" + Convert.ToString(prefijoTaller[1]).Trim() + Request.QueryString["o"] + "-TF" + rdlOpcionesFactura.SelectedValue;
                            else
                                docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-TF" + rdlOpcionesFactura.SelectedValue;
                        }
                        catch (Exception)
                        {
                            docCfd.strEncSerieImp = "E" + Request.QueryString["e"] + "-T" + Request.QueryString["t"] + "-OT-" + Request.QueryString["o"] + "-TF" + rdlOpcionesFactura.SelectedValue;
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
                        {
                            documento = Convert.ToInt32(result[1]);
                            if (documento > 0)
                            {
                                Facturas facturas = new Facturas();
                                facturas.folio = Convert.ToInt32(Request.QueryString["o"]);
                                facturas.tipoCuenta = "CC";
                                facturas.factura = txtReferenciasFac.Text;
                                CatClientes clientes = new CatClientes();
                                string politica = clientes.obtieneClavePoliticaCliente(lblReceptorFactura.Text);
                                int diasPlazo = clientes.obtieneDiasPolitica(lblReceptorFactura.Text);
                                facturas.fechaRevision = fechas.obtieneFechaLocal();
                                facturas.fechaProgPago = fechas.obtieneFechaLocal().AddDays(Convert.ToDouble(diasPlazo));
                                facturas.id_cliprov = Convert.ToInt32(lblReceptorFactura.Text);
                                facturas.formaPago = "E";
                                facturas.politica = politica;
                                facturas.estatus = "PEN";
                                facturas.empresa = Convert.ToInt32(Request.QueryString["e"]); 
                                facturas.taller = Convert.ToInt32(Request.QueryString["t"]); 
                                facturas.tipoCargo = "I";
                                facturas.Importe = docCfd.decEncTotal;
                                facturas.orden = Convert.ToInt32(Request.QueryString["o"]);
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
                                if (Request.QueryString["refct"] == "0" || Request.QueryString["refct"] == "1")
                                {
                                    facturas.idCfd = documento;
                                    facturas.generaFacturaCC();
                                }
                                else
                                {
                                    facturas.idCfd = Convert.ToInt32(Request.QueryString["fact"]);
                                    facturas.actualizaFacturaCC();
                                }
                                object[] facturasInternas = facturas.retorno;

                            }
                        }
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
                        actualizadoFact = genera.info;
                        if (!Convert.ToBoolean(actualizadoFact[0]))
                            lblError.Text = "Error de Timbrado: " + Convert.ToString(actualizadoFact[1]);
                        else
                        {
                            bool timbrado = genera.verificaDocumentoTimbrado(documento);
                            if (timbrado)
                            {
                                //lblError.Text= Convert.ToString(actualizadoFact[1]);
                                Refacciones refacciones = new Refacciones();
                                refacciones.actualizaFacturados(documento.ToString(), Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"], "T");
                                try
                                {
                                    lblError.Text = Convert.ToString(actualizadoFact[1]);
                                }
                                catch (Exception EX) { lblError.Text = "Se ha presentado el siguiente error: " + EX.Message + "; más sin embargo no afecto para que el documento se timbrara."; }
                                folios.Timbrar(lblRfcEmisor.Text.Trim().ToUpper());
                                foliosDisponibles = folios.ObtieneFoliosDisponibles(lblRfcEmisor.Text.Trim().ToUpper());
                                lblError.Text = lblError.Text + Environment.NewLine + "Folios Disponibles: " + foliosDisponibles.ToString();
                                lblError.Text = lblError.Text.Replace(Environment.NewLine, "<br/>");
                                grdDocu.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                grdDocu.MasterTableView.CommandItemSettings.ShowSaveChangesButton = false;
                                grdDocu.MasterTableView.Columns[6].Visible = false;
                                acutalizaFase();
                                lnkTimbrar.Visible = false;
                            }
                            else 
                                lblError.Text = "Error: no fue posible timbrar el documento intentelo de nuevo, si el problema persiste contacte al administrador del sistema";
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

    private string obtieneRazonSocial(int idCliprov, string tipo)
    {
        string razonsocial = "";
        string sql = "select razon_social from cliprov where id_cliprov=" + idCliprov.ToString() + " and tipo='"+tipo+"'";
        object[] ejecutado = ejecuta.scalarToString(sql);
        if ((bool)ejecutado[0])
            razonsocial = ejecutado[1].ToString();
        else
            razonsocial = "";
        return razonsocial;
    }

    private void acutalizaFase()
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

                if (faseSActual < 8)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 8);
                }
            }
        }
        catch (Exception) { }

    }
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
    protected void rdlOpcionesFactura_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlDesglose.SelectedValue = "1";
        txtGlobal1.Text = txtGlobal2.Text = "";
        txtGlobal1.Visible = txtGlobal2.Visible = false;
        lblError.Text = "";
    }
    protected void ddlDesglose_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (ddlDesglose.SelectedValue == "1")
        {
            txtGlobal1.Text = txtGlobal2.Text = "";
            txtGlobal1.Visible = txtGlobal2.Visible = false;
        }
        else if (ddlDesglose.SelectedValue == "2")
        {
            if (rdlOpcionesFactura.SelectedValue == "1")
            {
                string texto = "";
                try
                {
                    int empresa = Convert.ToInt32(Request.QueryString["e"]);
                    int taller = Convert.ToInt32(Request.QueryString["t"]);
                    int orden = Convert.ToInt32(Request.QueryString["o"]);
                    object[] datosOrden = recepciones.obtieneInfoOrdenPie(orden, empresa, taller);

                    DatosVehiculos vehi = new DatosVehiculos();
                    object[] datosVehiculo = vehi.obtieneDatosBasicosVehiculoNotaFactura(orden, empresa, taller);
                    if (Convert.ToBoolean(datosOrden[0]))
                    {
                        object[] infoOrden = null;
                        DataSet ordenDatos = (DataSet)datosOrden[1];
                        foreach (DataRow fila in ordenDatos.Tables[0].Rows)
                        {
                            infoOrden = fila.ItemArray;
                        }
                        if (Convert.ToBoolean(datosVehiculo[0]))
                        {
                            object[] datosVehiculoOrden = null;
                            DataSet infoVehi = (DataSet)datosVehiculo[1];
                            foreach (DataRow filav in infoVehi.Tables[0].Rows)
                            {
                                datosVehiculoOrden = filav.ItemArray;
                            }
                            if (datosVehiculoOrden != null && infoOrden != null)
                            {
                                texto = string.Format("Por la reparación (Global) efectuada a la unidad " +
                                Convert.ToString(datosVehiculoOrden[1]).Trim().ToUpper() + " " + Convert.ToString(datosVehiculoOrden[2]).ToUpper().ToUpper().Trim() + " modelo " + Convert.ToString(datosVehiculoOrden[3]).ToUpper().ToUpper().Trim() + " con placas " + Convert.ToString(datosVehiculoOrden[4]).ToUpper().ToUpper().Trim() +
                                " Propiedad de " + Convert.ToString(datosVehiculoOrden[5]).Trim().ToUpper());
                            }
                            else
                                texto = "";
                        }
                        else
                            texto = "";
                    }
                    else
                        texto = "";
                }
                catch (Exception ex) { }

                txtGlobal1.Text = texto;
                txtGlobal1.Visible = true;
                txtGlobal2.Text = "";
                txtGlobal2.Visible = false;
            }
            else if (rdlOpcionesFactura.SelectedValue == "2")
            {
                txtGlobal1.Text = "MANO DE OBRA DE ACUERDO A LA VALUACIÓN";
                txtGlobal1.Visible = true;
                txtGlobal2.Text = "";
                txtGlobal2.Visible = false;
            }
            else if (rdlOpcionesFactura.SelectedValue == "3")
            {
                txtGlobal2.Text = "LISTADO DE REFACCIONES";
                txtGlobal2.Visible = true;
                txtGlobal1.Text = "";
                txtGlobal1.Visible = false;
            }
        }
        else if (ddlDesglose.SelectedValue == "3")
        {
            if (rdlOpcionesFactura.SelectedValue == "1")
            {
                txtGlobal1.Text = "MANO DE OBRA";
                txtGlobal1.Visible = true;
                txtGlobal2.Text = "REFACCIONES";
                txtGlobal2.Visible = true;
            }
            else {
                txtGlobal1.Text = txtGlobal2.Text = "";
                txtGlobal1.Visible = txtGlobal2.Visible = false;
                lblError.Text = "Esta opción no puede ser elegible con el tipo de desglose indicado";
            }
        }
    }

    protected void txtMODesGlob_TextChanged(object sender, EventArgs e)
    {
        decimal descMO = 0;
        decimal totalCto = 0;

        try { descMO = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtMODesGlob")).Text); }
        catch (Exception ex) { descMO = 0; }

        //PctjeDsctoGlb = Convert.ToDecimal(txtPctjeDsctoGlb.Text);
        PctjeDsctoGlb = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);
        decimal subTotBru_menos_Dscto = 0;
        decimal desctoCpto = 0;
        //vuelve a calcular subtotales e impuesto trasladado por item (concepto)
        foreach (GridDataItem item in grdDocu.Items)
        {
            TextBox id = (TextBox)item.FindControl("txtIdent");
            string[] idSplit = id.Text.ToString().Split('-');
            if (idSplit[0] == "MO")
            {
                TextBox txtPtjeDscto = (TextBox)item.FindControl("txtPtjeDscto");
                TextBox txtDscto = (TextBox)item.FindControl("txtDscto");
                Label lblImporte = (Label)item.FindControl("lblImporte");
                try { totalCto = Convert.ToDecimal(lblImporte.Text); } catch (Exception) { totalCto = 0; }
                if (totalCto != 0)
                {
                    txtPtjeDscto.Text = descMO.ToString("F2");
                    txtDscto.Text = (totalCto * (descMO / 100)).ToString("F2");
                    desctoCpto = decimal.Parse(((TextBox)item.FindControl("txtDscto")).Text);
                    dtoGlobConcepto = 0;
                    if (totalCto != 0 && totalCto > 0)
                        subTotalCncpto = ((totalCto - desctoCpto));
                    if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
                    {
                        dtoGlobConcepto = ((subTotalCncpto * PctjeDsctoGlb) / 100);
                        subTotalCncpto = subTotalCncpto - ((subTotalCncpto * PctjeDsctoGlb) / 100);
                    }
                    ((Label)item.FindControl("lblDtoGlobalConcepto")).Text = dtoGlobConcepto.ToString("F2");
                    ((Label)item.FindControl("lblSubTotal")).Text = subTotalCncpto.ToString("F2");
                    calculaIvaTras(item);
                    calculaIepsTras(item);
                    calculaIvaRet(item);
                    calculaIsrRet(item);

                    //txtMotivoDscto.Visible = true;
                    //lblMotDscto.Visible = true;
                    ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = true;
                    ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = true;
                    totDscto = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotDscto")).Text);
                    subTotBrut = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblSubTotBru")).Text);
                    subTotBru_menos_Dscto = subTotBrut - totDscto;

                    totDsctoGbl = ((subTotBru_menos_Dscto * PctjeDsctoGlb) / 100);

                    calculaTotalDescuento();
                }
            }
        }

        //lblDsctoGlb.Text = totDsctoGbl.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text = totDsctoGbl.ToString("F2");
        calculaSubTotNeto();
    }

    protected void txtRefDesGlob_TextChanged(object sender, EventArgs e)
    {
        decimal descREF = 0;
        decimal totalCto = 0;

        try { descREF = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtRefDesGlob")).Text); }
        catch (Exception ex) { descREF = 0; }
        //PctjeDsctoGlb = Convert.ToDecimal(txtPctjeDsctoGlb.Text);
        PctjeDsctoGlb = Convert.ToDecimal(((TextBox)fvwResumen.Row.FindControl("txtPctjeDsctoGlb")).Text);
        decimal subTotBru_menos_Dscto = 0;
        decimal desctoCpto = 0;
        //vuelve a calcular subtotales e impuesto trasladado por item (concepto)
        foreach (GridDataItem item in grdDocu.Items)
        {
            TextBox id = (TextBox)item.FindControl("txtIdent");
            string[] idSplit = id.Text.ToString().Split('-');
            if (idSplit[0] == "REF")
            {
                TextBox txtPtjeDscto = (TextBox)item.FindControl("txtPtjeDscto");
                TextBox txtDscto = (TextBox)item.FindControl("txtDscto");
                Label lblImporte = (Label)item.FindControl("lblImporte");
                try { totalCto = Convert.ToDecimal(lblImporte.Text); } catch (Exception) { totalCto = 0; }
                if (totalCto != 0)
                {
                    txtPtjeDscto.Text = descREF.ToString("F2");
                    txtDscto.Text = (totalCto * (descREF / 100)).ToString("F2");

                    desctoCpto = decimal.Parse(((TextBox)item.FindControl("txtDscto")).Text);
                    dtoGlobConcepto = 0;
                    if (totalCto != 0 && totalCto > 0)
                        subTotalCncpto = ((totalCto - desctoCpto));
                    if (PctjeDsctoGlb != 0 && subTotalCncpto != 0)
                    {
                        dtoGlobConcepto = ((subTotalCncpto * PctjeDsctoGlb) / 100);
                        subTotalCncpto = subTotalCncpto - ((subTotalCncpto * PctjeDsctoGlb) / 100);
                    }
                    ((Label)item.FindControl("lblDtoGlobalConcepto")).Text = dtoGlobConcepto.ToString("F2");
                    ((Label)item.FindControl("lblSubTotal")).Text = subTotalCncpto.ToString("F2");
                    calculaIvaTras(item);
                    calculaIepsTras(item);
                    calculaIvaRet(item);
                    calculaIsrRet(item);

                    //txtMotivoDscto.Visible = true;
                    //lblMotDscto.Visible = true;
                    ((TextBox)fvwResumen.Row.FindControl("txtMotivoDscto")).Visible = true;
                    ((Label)fvwResumen.Row.FindControl("lblMotDscto")).Visible = true;
                    totDscto = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblTotDscto")).Text);
                    subTotBrut = Convert.ToDecimal(((Label)fvwResumen.Row.FindControl("lblSubTotBru")).Text);
                    subTotBru_menos_Dscto = subTotBrut - totDscto;

                    totDsctoGbl = ((subTotBru_menos_Dscto * PctjeDsctoGlb) / 100);

                    calculaTotalDescuento();
                }
            }
        }

        //lblDsctoGlb.Text = totDsctoGbl.ToString("F2");
        ((Label)fvwResumen.Row.FindControl("lblDsctoGlb")).Text = totDsctoGbl.ToString("F2");
        calculaSubTotNeto();
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
                                ((RadDropDownList)fila.FindControl("ddlIvaTras")).SelectedValue = dato[12].ToString(); }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlIvaTras")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIvaTras")).Text = Convert.ToDecimal(dato[14].ToString()).ToString("F2");
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIeps")).SelectedValue = dato[13].ToString(); }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlIeps")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIeps")).Text = dato[15].ToString();
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIvaRet")).SelectedValue = dato[16].ToString(); }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlIvaRet")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIvaRet")).Text = dato[18].ToString();
                            try
                            {
                                ((RadDropDownList)fila.FindControl("ddlIsrRet")).SelectedValue = dato[17].ToString(); }
                            catch (Exception) { ((DropDownList)fila.FindControl("ddlIsrRet")).SelectedIndex = -1; }
                            ((Label)fila.FindControl("lblIsrRet")).Text = dato[19].ToString();
                            ((Label)fila.FindControl("lblTotalCpto")).Text = Convert.ToDecimal(dato[20].ToString()).ToString("F2");
                        }
                        catch (Exception) {  }
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

    protected void lnkAceptarModificacion_Click(object sender, EventArgs e)
    {
        lblErrorActuraliza.Text = "";
        FacturacionElectronica.Receptores Receptor = new FacturacionElectronica.Receptores();       
        Receptor.idReceptor = Convert.ToInt32(lblIdReceptor.Text);
        Receptor.existeReceptor(txtRfc.Text.ToUpper());
        object[] existeReceptor = Receptor.info;
        if (Convert.ToBoolean(existeReceptor[0]))
        {
            int existe = Convert.ToInt32(existeReceptor[1]);            
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
            lblErrorActuraliza.Text = "Error al actualizar los datos del cliente. " + existeReceptor[1].ToString();
    }

    protected void lnkAgregaRec_Click(object sender, EventArgs e)
    {
        lblErrorActuraliza.Text = "";
        rbtnPersona.SelectedValue = "M";
        txtRfc.Text = txtRazonNew.Text = "";
        lblModo.Text = "C";
        txtCalle.Text = txtNoExt.Text = txtNoIntMod.Text = "";
        ddlPais.SelectedIndex = ddlEstado.SelectedIndex = ddlMunicipio.SelectedIndex = ddlColonia.SelectedIndex = ddlCodigo.SelectedIndex = -1;
        txtReferenciaMod.Text = txtLocalidad.Text = txtCorreo.Text = txtCorreoCC.Text = txtCorreoCCO.Text = "";
        txtRfc.MaxLength = 12;        
        string script1 = "abreWinCatRec()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "AgregaCliente", script1, true);
    }

    protected void lnkEditaRec_Click(object sender, EventArgs e)
    {
        lblErrorActuraliza.Text = "";
        lblModo.Text = "M";
        txtRfc.Enabled = false;
        cargaInfoReceptor();        
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
            if (relaciones == 0) {
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
}