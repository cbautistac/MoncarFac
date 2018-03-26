using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;
using Telerik.Web.UI;

public partial class RecepcionOrden : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    Fechas fechas = new Fechas();

    protected void Page_Load(object sender, EventArgs e)
    {
        obtieneSesiones();
        if (!IsPostBack)
        {            
            cargaDatosOrden();
            checaInventario();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus== "S" || estatus == "C")
                    lnkGuardarRecepcion.Visible = false;
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

    private void checaInventario()
    {
        int[] sesiones = obtieneSesiones();
        object[] inventario = datosOrdenes.obtieneInfoInventario(sesiones[2], sesiones[3], sesiones[4]);
        if (Convert.ToBoolean(inventario[0])) {
            if (Convert.ToBoolean(inventario[1]))
            {
                actualizaProceso(2);
                actualizaLocalizacion();
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

    protected void lnkGuardarRecepcion_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0 || sesiones[4] == 0)
            Response.Redirect("Default.aspx");
        lblErrorRecepcion.Text = "";

        TextBox[] textboxes ={txtTorreOrden,txtKmOrden,txtEmpresaGruaEnOrden,txtEmpresaGruaSalOrden,txtGruaEntOrden,txtGruaSalOrden,txtOperadorGruaEnOrden,txtOperadorGruaSalOrden,
                            txtNombreOrden,txtApPatOrden,txtCalleOrden,txtNoExtOrden,txtColoniaOrden,txtMunOrden,txtEdoOrden,txtCpOrden,txtTel1Orden,txtTel2Orden,txtTel3Orden,
                            txtCorreoOrden,txtContactoOrden,txtSiniestroOrden,txtFechaSiniestro,txtPolizaOrden,txtReporteOrden,txtFolioOrden,txtAjustadorOrden,txtDeducibleOrden,txtPorcDeducible,txtDemerito};

        for (int i = 0; i < textboxes.Length; i++)
        {
            if (textboxes[i].Text == "")
            {
                switch (i)
                {
                    case 1:
                        textboxes[i].Text = "0";
                        break;
                    case 15:
                        textboxes[i].Text = "0";
                        break;
                    case 27:
                        textboxes[i].Text = "0";
                        break;
                    case 28:
                        textboxes[i].Text = "0";
                        break;
                    case 22:
                        textboxes[i].Text = "1900-01-01";
                        break;
                    default:
                        textboxes[i].Text = "N/A";
                        break;
                }
            }
        }

        object[] orden = new object[45]{
                ddlToOrden.SelectedValue,
                ddlClienteOrden.SelectedValue,
                ddlTsOrden.SelectedValue,
                ddlValOrden.SelectedValue,
                ddlTaOrden.SelectedValue,
                ddlLocOrden.SelectedValue,                
                txtTorreOrden.Text,
                ddlGasOrden.SelectedValue,
                txtKmOrden.Text,
                txtObsOrden.Text,
                cbxCproblemaOrden.Checked,
                ddlCategoriaOrden.SelectedValue,
                txtNombreOrden.Text,
                txtApPatOrden.Text,
                txtApMatOrden.Text,
                txtCalleOrden.Text,
                txtNoExtOrden.Text,
                txtNoIntOrden.Text,
                txtColoniaOrden.Text,
                txtMunOrden.Text,
                txtEdoOrden.Text,
                txtCpOrden.Text,
                txtTel1Orden.Text,
                txtTel2Orden.Text,
                txtTel3Orden.Text,
                txtCorreoOrden.Text,
                txtContactoOrden.Text,
                txtSiniestroOrden.Text,
                txtPolizaOrden.Text,
                cbxFlotillaOrden.Checked,
                txtReporteOrden.Text,
                txtFolioOrden.Text,
                txtFechaSiniestro.Text,
                txtAjustadorOrden.Text,
                txtDeducibleOrden.Text,
                txtGruaEntOrden.Text,
                txtGruaSalOrden.Text,
                ddlAseguradoraOrden.SelectedValue,
                txtOperadorGruaEnOrden.Text,
                txtOperadorGruaSalOrden.Text,
                txtEmpresaGruaEnOrden.Text,
                txtEmpresaGruaSalOrden.Text,
                ddlPerfil.SelectedValue,
                txtPorcDeducible.Text,
                txtDemerito.Text
        };
        try
        {
            int noOrden = Convert.ToInt32(sesiones[4].ToString());
            int idEmpresa = Convert.ToInt32(sesiones[2].ToString());
            int idTaller = Convert.ToInt32(sesiones[3].ToString());
            object[] existInv = recepciones.existeInventario(noOrden, idEmpresa, idTaller);
            if (Convert.ToBoolean(existInv[0]))
                if (Convert.ToInt32(existInv[1]) != 0)
                    existInv = recepciones.actualizaInvGas(noOrden, idEmpresa, idTaller, Convert.ToInt32(orden[7]));

            object[] actualizaOrden = recepciones.actualizaOrden(sesiones[2], sesiones[3], sesiones[4].ToString(), orden);
            if (Convert.ToBoolean(actualizaOrden[0]))
            {
                if (lblLocIni.Text != ddlLocOrden.SelectedValue)
                {
                    object[] agregaBitacoraLocalizacion = recepciones.agregaBitacoraLocalizaciones(sesiones, ddlLocOrden.SelectedValue, sesiones[4].ToString());
                }
                else
                    lblErrorRecepcion.Text = "Se actualizaron los datos de la orden: " + sesiones[4].ToString() + ".";

                if (lblPerfilIni.Text != ddlPerfil.SelectedValue)
                {
                    object[] agregaBitacoraPerfiles = recepciones.agregaBitacoraPerfiles(sesiones, ddlPerfil.SelectedValue, sesiones[4].ToString());
                }

                else
                    lblErrorRecepcion.Text = "Se actualizaron los datos de la orden: " + sesiones[4].ToString() + ".";

                actualizaProceso(1);
                actualizaLocalizacion();

            }
            else
                lblErrorRecepcion.Text = "Error al actualizar orden: " + sesiones[4].ToString() + ": " + actualizaOrden[1].ToString();

        }
        catch (Exception ex)
        {
            lblErrorRecepcion.Text = "Se produjo el siguiente error: " + ex.Message.ToString() + ".";
        }
    }

    private void actualizaProceso(int seccion)
    {
        int[] sesiones = obtieneSesiones();
        int compleato = 0;
        string error = "";
        if (seccion != 2)
        {
            TextBox[] textboxes ={txtTorreOrden,txtKmOrden,txtEmpresaGruaEnOrden,txtEmpresaGruaSalOrden,txtGruaEntOrden,txtGruaSalOrden,txtOperadorGruaEnOrden,txtOperadorGruaSalOrden,
                            txtNombreOrden,txtApPatOrden,txtCalleOrden,txtNoExtOrden,txtColoniaOrden,txtMunOrden,txtEdoOrden,txtCpOrden,txtTel1Orden,txtTel2Orden,txtTel3Orden,
                            txtCorreoOrden,txtContactoOrden,txtSiniestroOrden,txtFechaSiniestro,txtPolizaOrden,txtReporteOrden,txtFolioOrden,txtAjustadorOrden,txtDeducibleOrden,txtPorcDeducible,txtDemerito};

            RadComboBox[] combos = { ddlToOrden, ddlClienteOrden, ddlTsOrden, ddlValOrden, ddlTaOrden, ddlPerfil, ddlLocOrden, ddlCategoriaOrden, ddlAseguradoraOrden };

            
            int campos = textboxes.Length + combos.Length;
            int contador = 0;
            for (int i = 0; i < textboxes.Length; i++)
            {
                if (textboxes[i].Text != "")
                    contador++;
            }

            for (int i = 0; i < combos.Length; i++)
            {
                if (combos[i].SelectedValue != "")
                    contador++;
            }

            if (contador < campos)
                compleato = 0;
            else if (contador == campos)
                compleato = 1;

            error = "Aun no se completa la información de la orden";
        }
        else { compleato = 1; error = "Aun hace falta completar información del inventario"; }

        object[] actualizaOrden = recepciones.actualizaProcesosOrden(sesiones[2], sesiones[3], sesiones[4], compleato, seccion);
        if (!Convert.ToBoolean(actualizaOrden[0]))
            lblErrorRecepcion.Text = error;
        

    }

    private void cargaDatosOrden()
    {
        lblErrorRecepcion.Text = "";
        try
        {
            int empresa = Convert.ToInt32(Request.QueryString["e"]);
            int taller = Convert.ToInt32(Request.QueryString["t"]);
            int orden = Convert.ToInt32(Request.QueryString["o"]);

            object[] datosOrden = recepciones.obtieneInfoOrden(orden, empresa, taller);
            if (Convert.ToBoolean(datosOrden[0]))
            {
                DataSet ordenDatos = (DataSet)datosOrden[1];
                int avance = 0;
                foreach (DataRow filaOrd in ordenDatos.Tables[0].Rows)
                {
                    avance = Convert.ToInt32(Convert.ToDecimal(filaOrd[36].ToString()));
                    ddlToOrden.SelectedValue = filaOrd[4].ToString();
                    ddlClienteOrden.SelectedValue = filaOrd[5].ToString();
                    ddlTsOrden.SelectedValue = filaOrd[7].ToString();
                    ddlValOrden.SelectedValue = filaOrd[8].ToString();
                    ddlTaOrden.SelectedValue = filaOrd[9].ToString();
                    ddlLocOrden.SelectedValue = filaOrd[44].ToString();
                    lblLocIni.Text = filaOrd[44].ToString();
                    txtTorreOrden.Text = filaOrd[10].ToString().Trim();
                    try
                    {
                        ddlGasOrden.SelectedValue = filaOrd[12].ToString();
                    }
                    catch (Exception ex)
                    {
                        ddlGasOrden.SelectedValue = "0";
                    }
                    txtKmOrden.Text = filaOrd[13].ToString().Trim();
                    txtObsOrden.Text = filaOrd[14].ToString().Trim();
                    cbxCproblemaOrden.Checked = convierteStringtoBool(filaOrd[15].ToString());
                    ddlCategoriaOrden.SelectedValue = convierteStringtoInt(filaOrd[16].ToString());
                    txtNombreOrden.Text = filaOrd[17].ToString().Trim();
                    txtApPatOrden.Text = filaOrd[18].ToString().Trim();
                    txtApMatOrden.Text = filaOrd[19].ToString().Trim();
                    txtCalleOrden.Text = filaOrd[20].ToString().Trim();
                    txtNoExtOrden.Text = filaOrd[21].ToString().Trim();
                    txtNoIntOrden.Text = filaOrd[22].ToString().Trim();
                    txtColoniaOrden.Text = filaOrd[23].ToString().Trim();
                    txtMunOrden.Text = filaOrd[24].ToString().Trim();
                    txtEdoOrden.Text = filaOrd[25].ToString().Trim();
                    txtCpOrden.Text = filaOrd[26].ToString().Trim();
                    txtTel1Orden.Text = filaOrd[27].ToString().Trim();
                    txtTel2Orden.Text = filaOrd[28].ToString().Trim();
                    txtTel3Orden.Text = filaOrd[29].ToString().Trim();
                    txtCorreoOrden.Text = filaOrd[30].ToString().Trim();
                    txtContactoOrden.Text = filaOrd[31].ToString().Trim();
                    txtSiniestroOrden.Text = filaOrd[32].ToString().Trim();
                    txtPolizaOrden.Text = filaOrd[33].ToString().Trim();
                    cbxFlotillaOrden.Checked = convierteStringtoBool(filaOrd[34].ToString());
                    txtReporteOrden.Text = filaOrd[37].ToString().Trim();
                    txtFolioOrden.Text = filaOrd[38].ToString().Trim();
                    DateTime fecha;
                    try { fecha = Convert.ToDateTime(filaOrd[39].ToString()); }
                    catch (Exception) { fecha = fechas.obtieneFechaLocal(); }
                    txtFechaSiniestro.Text = fecha.ToString("yyyy-MM-dd");
                    txtAjustadorOrden.Text = filaOrd[40].ToString().Trim();
                    txtDeducibleOrden.Text = filaOrd[41].ToString().Trim();
                    txtGruaEntOrden.Text = filaOrd[42].ToString().Trim();
                    txtGruaSalOrden.Text = filaOrd[43].ToString().Trim();
                    ddlAseguradoraOrden.SelectedValue = convierteStringtoInt(filaOrd[45].ToString());
                    txtEmpresaGruaEnOrden.Text = filaOrd[55].ToString().Trim();
                    txtEmpresaGruaSalOrden.Text = filaOrd[56].ToString().Trim();
                    txtOperadorGruaEnOrden.Text = filaOrd[57].ToString().Trim();
                    txtOperadorGruaSalOrden.Text = filaOrd[58].ToString().Trim();
                    ddlPerfil.SelectedValue = filaOrd[59].ToString();
                    lblPerfilIni.Text = filaOrd[59].ToString();
                    txtPorcDeducible.Text = filaOrd[62].ToString();
                    txtDemerito.Text = filaOrd[63].ToString();
                }
            }
            else
            {
                pnlRecepcion.Visible = false;
                lblErrorRecepcion.Text = "No se pudo cargar la información de la orden: " + orden.ToString() + ", por favor intente mas tarde";
            }

        }
        catch (Exception ex)
        {
            lblErrorRecepcion.Text = "Error: " + ex.Message;
        }
    }

    private object convierteBooltoInt(string valor)
    {
        int retorno = 0;
        try
        {
            bool dato = Convert.ToBoolean(valor);
            if (dato)
                retorno = 1;
            else
                retorno = 0;
        }
        catch (Exception)
        {
            retorno = 0;
        }
        return retorno;
    }

    private string convierteStringtoInt(string valor)
    {
        string retorno = "0";
        try
        {
            if (valor == "")
            {
                retorno = "0";
            }
            else
            {
                retorno = valor;
            }
        }
        catch (Exception)
        {
            retorno = "0";
        }
        return retorno;
    }

    private bool convierteStringtoBool(string valor)
    {
        bool retorno = false;
        try
        {
            if (valor == "null")
            {
                retorno = false;
            }
            else
            {
                if (valor == "False")
                    retorno = false;
                else
                    retorno = true;
            }
        }
        catch (Exception)
        {
            retorno = false;
        }
        return retorno;
    }
    /*
    protected void txtNombreOrden_TextChanged(object sender, EventArgs e)
    {
        txtContactoOrden.Text = txtNombreOrden.Text + " " + txtApPatOrden.Text + " " + txtApMatOrden.Text;
    }
    protected void txtApPatOrden_TextChanged(object sender, EventArgs e)
    {
        txtContactoOrden.Text = txtNombreOrden.Text + " " + txtApPatOrden.Text + " " + txtApMatOrden.Text;
    }
    protected void txtApMatOrden_TextChanged(object sender, EventArgs e)
    {
        txtContactoOrden.Text = txtNombreOrden.Text + " " + txtApPatOrden.Text + " " + txtApMatOrden.Text;
    }
    */
    private void actualizaLocalizacion()
    {
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idUsuario = Convert.ToInt32(Request.QueryString["u"]);
        object[] controlParametros = recepciones.cuentaRecepcionRealizada(noOrden, idEmpresa, idTaller);
        int recepRealizados = 0;
        int regBitLocInt = 0;
        if (Convert.ToBoolean(controlParametros[0]))
            recepRealizados = Convert.ToInt32(controlParametros[1]);
        if (recepRealizados == 3)
        {
            object[] regBitLoc = recepciones.obtieneRegBitacoraLoc(noOrden, idEmpresa, idTaller);
            if (Convert.ToBoolean(regBitLoc[0]))
            {
                regBitLocInt = Convert.ToInt32(regBitLoc[1]);
                if (regBitLocInt == 1)
                {
                    object[] actualizaLocalizacion = recepciones.actualizaLocRec(noOrden, idEmpresa, idTaller, idUsuario);
                    if (Convert.ToBoolean(actualizaLocalizacion[0]))
                        if (Convert.ToBoolean(actualizaLocalizacion[1])) { }
                        else
                            lblErrorRecepcion.Text = "Hubo un error en la conexión, ";
                    else
                        lblErrorRecepcion.Text = "Ocurrio un error inesperado: " + actualizaLocalizacion[1].ToString();
                }
            }
            else
                lblErrorRecepcion.Text = "Ocurrio un error inesperado: " + regBitLoc[1].ToString();
        }
    }
}