using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using E_Utilities;

public partial class VehiculoOrden : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    Fechas fechas = new Fechas();    

    protected void Page_Load(object sender, EventArgs e)
    {
        obtieneSesiones();
        if (!IsPostBack)
        {            
            cargaDatosVehiculo();
            checaInventario();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                    lnkGuardarVehiculo.Visible = false;
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
        if (Convert.ToBoolean(inventario[0]))
        {
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
        catch (Exception x)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    protected void lnkGuardarVehiculo_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0 || sesiones[4] == 0)
            Response.Redirect("Default.aspx");
        lblErrorVehiculo.Text = "";

        string[] iniciales = lblIniciales.Text.Split(new char[] { ';' });
        bool cambio = false;
        


        TextBox[] textboxes = { txtModeloOrden, txtSerieOrden, txtMotorOrden, txtColorExtOrden, txtColorIntOrden, txtCilindrosOrden, txtVersionOrden, txtPuertasOrden, txtNoEcoOrden, txtLlantaOrden };
        for (int i = 0; i < textboxes.Length; i++)
        {
            if (textboxes[i].Text == "")
            {
                switch (i)
                {
                    case 0:
                        textboxes[i].Text = fechas.obtieneFechaLocal().Year.ToString().PadLeft(4, '0');
                        break;
                    case 5:
                        textboxes[i].Text = "0";
                        break;
                    case 7:
                        textboxes[i].Text = "0";
                        break;                    
                    default:
                        textboxes[i].Text = "N/A";
                        break;
                }
            }
        }

        DatosVehiculos vehiculos = new DatosVehiculos();
        string[] datos = new string[32] { ddlMarcaOrd.SelectedValue,
                    ddlTvOrden.SelectedValue ,
                    ddlUnidadOrden.SelectedValue ,
                    lblIdVehiculo.Text ,
                    txtModeloOrden.Text ,
                    txtPlacasOrden.Text ,
                    txtSerieOrden.Text ,
                    txtMotorOrden.Text ,
                    txtColorIntOrden.Text,
                    txtColorExtOrden.Text ,
                    ddlTransOrden.SelectedValue  ,
                    ddlTracOrden.SelectedValue  ,
                    txtCilindrosOrden.Text  ,
                    txtVersionOrden.Text  ,
                    txtPuertasOrden.Text  ,
                    txtNoEcoOrden.Text ,
                    ddlRinOrden.SelectedValue ,
                    txtLlantaOrden.Text  ,
                    rblQuemaCocoOrden.SelectedValue ,
                    rdlBolsasOrden.SelectedValue ,
                    rdlAireOrden.SelectedValue ,
                    rdlHidraulica.SelectedValue ,
                    rdlElevadoresOrden.SelectedValue ,
                    rdlEspejosOrden.SelectedValue ,
                    rdlColorEspOrden.SelectedValue ,
                    rdlMoldurasOrden.SelectedValue ,
                    rdlVestidurasOrden.SelectedValue ,
                    rdlCantoneras.SelectedValue ,
                    rdlFarosOrden.SelectedValue ,
                    rdlFaciaOrden.SelectedValue ,
                    rdlCabinaOrden.SelectedValue ,
                    rdlDefensaOrden.SelectedValue  };        

        try
        {
            if (iniciales[0] != datos[0] || iniciales[1] != datos[1] || iniciales[2] != datos[2])
                cambio = true;

            object[] actualizaVehiculo = vehiculos.actualizaVehiculo(txtPlacasOrden.Text, datos, lblIdVehiculo.Text, cambio);
            if (!Convert.ToBoolean(actualizaVehiculo[0]))
            {
                lblErrorVehiculo.Text = "No pudo actualizarse la información del vehículo. Error: " + Convert.ToString(actualizaVehiculo[1]);
            }
            else {
                actualizaProceso(3);
                actualizaLocalizacion();
                Response.Redirect("VehiculoOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
            }
        }
        catch (Exception ex)
        {
            lblErrorVehiculo.Text = "Se produjo el siguiente error: " + ex.Message.ToString() + ".";
        }
    }

    private void actualizaProceso(int seccion)
    {
        int[] sesiones = obtieneSesiones();
        int compleato = 0;
        string error = "";
        if (seccion != 2)
        {
            TextBox[] textboxes = { txtModeloOrden, txtSerieOrden, txtMotorOrden, txtColorExtOrden, txtColorIntOrden, txtCilindrosOrden, txtVersionOrden, txtPuertasOrden, txtNoEcoOrden, txtLlantaOrden };

            DropDownList[] combos = { ddlTransOrden, ddlTracOrden, ddlRinOrden };


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

            error = "Aun no se completa la información del vehículo";
        }
        else { compleato = 1; error = "Aun hace falta completar información del inventario"; }
        object[] actualizaOrden = recepciones.actualizaProcesosOrden(sesiones[2], sesiones[3], sesiones[4], compleato, seccion);
        if (!Convert.ToBoolean(actualizaOrden[0]))
            lblErrorVehiculo.Text = error;

    }

    private void cargaDatosVehiculo()
    {        
        lblErrorVehiculo.Text = "";
        try
        {
            int empresa = Convert.ToInt32(Request.QueryString["e"]);
            int taller = Convert.ToInt32(Request.QueryString["t"]);
            int orden = Convert.ToInt32(Request.QueryString["o"]);

            DatosVehiculos vehiculos = new DatosVehiculos();
            object[] infoVehiculo = vehiculos.obtieneInfoVehiculo(orden, empresa, taller);
            if (Convert.ToBoolean(infoVehiculo[0]))
            {
                DataSet valores = (DataSet)infoVehiculo[1];
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    ddlMarcaOrd.SelectedValue = fila[0].ToString();
                    ddlTvOrden.SelectedValue = fila[1].ToString();
                    ddlUnidadOrden.Items.Clear();
                    ddlUnidadOrden.DataBind();
                    ddlUnidadOrden.SelectedValue = fila[2].ToString();
                    //llenaDdlLinea(fila[0].ToString(), fila[1].ToString(), fila[2].ToString());                    
                    lblIdVehiculo.Text = fila[3].ToString().Trim();
                    txtModeloOrden.Text = fila[4].ToString().Trim();
                    txtPlacasOrden.Text = fila[5].ToString().Trim();
                    txtSerieOrden.Text = fila[6].ToString().Trim();
                    txtMotorOrden.Text = fila[7].ToString().Trim();
                    txtColorIntOrden.Text = fila[8].ToString().Trim();
                    txtColorExtOrden.Text = fila[9].ToString().Trim();
                    ddlTransOrden.SelectedValue = fila[10].ToString();
                    ddlTracOrden.SelectedValue = fila[11].ToString();
                    txtCilindrosOrden.Text = fila[12].ToString().Trim();
                    txtVersionOrden.Text = fila[14].ToString().Trim();
                    txtPuertasOrden.Text = fila[15].ToString().Trim();
                    txtNoEcoOrden.Text = fila[13].ToString().Trim();
                    ddlRinOrden.SelectedValue = fila[16].ToString();
                    txtLlantaOrden.Text = fila[17].ToString().Trim();
                    rblQuemaCocoOrden.SelectedValue = convierteBooltoInt(fila[18].ToString()).ToString();
                    rdlBolsasOrden.SelectedValue = convierteBooltoInt(fila[19].ToString()).ToString();
                    rdlAireOrden.SelectedValue = convierteBooltoInt(fila[20].ToString()).ToString();
                    rdlHidraulica.SelectedValue = convierteBooltoInt(fila[21].ToString()).ToString();
                    rdlElevadoresOrden.SelectedValue = convierteBooltoInt(fila[22].ToString()).ToString();
                    rdlEspejosOrden.SelectedValue = convierteBooltoInt(fila[23].ToString()).ToString();
                    rdlColorEspOrden.SelectedValue = convierteBooltoInt(fila[24].ToString()).ToString();
                    rdlMoldurasOrden.SelectedValue = convierteBooltoInt(fila[25].ToString()).ToString();
                    rdlVestidurasOrden.SelectedValue = convierteBooltoInt(fila[27].ToString()).ToString();
                    rdlCantoneras.SelectedValue = convierteBooltoInt(fila[26].ToString()).ToString();
                    rdlFarosOrden.SelectedValue = convierteBooltoInt(fila[28].ToString()).ToString();
                    rdlFaciaOrden.SelectedValue = convierteBooltoInt(fila[29].ToString()).ToString();
                    rdlCabinaOrden.SelectedValue = convierteBooltoInt(fila[30].ToString()).ToString();
                    rdlDefensaOrden.SelectedValue = convierteBooltoInt(fila[31].ToString()).ToString();
                    lblIniciales.Text = fila[0].ToString() + ";" + fila[1].ToString() + ";" + fila[2].ToString() + ";" + fila[3].ToString();
                }
                
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
            else
            {                
                lblErrorVehiculo.Text = "No se pudo cargar la información del vehículo, por favor intente mas tarde";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    private void llenaDdlLinea(string marca, string vehiculo,string linea)
    {
        ddlUnidadOrden.Items.Clear();
        SqlDataSource12.SelectCommand = "select id_tipo_unidad, descripcion from Tipo_Unidad where id_marca=" + marca + " and id_tipo_vehiculo=" + vehiculo + " and id_tipo_unidad=" + linea;
        ddlUnidadOrden.DataBind();
    }

    private int convierteBooltoInt(string valor)
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
        catch (Exception x)
        {
            retorno = -1;
        }
        return retorno;
    }
    protected void btnImprime_Click(object sender, EventArgs e)
    {
        CaracVehiculos imprime = new CaracVehiculos();
        Recepciones recepciones = new Recepciones();
        
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
        
        string nomTaller =  recepciones.obtieneNombreTaller(Request.QueryString["t"]);
        string usuario = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);
        string contacto = "";
        string correo = "";
        string telefonos = "";
        object[] ordenes = recepciones.obtieneInfoOrden(orden, empresa, taller);
        if (Convert.ToBoolean(ordenes[0]))
        {
            DataSet valores = (DataSet)ordenes[1];
            foreach (DataRow fila in valores.Tables[0].Rows) {
                contacto = Convert.ToString(fila[31]);
                correo = Convert.ToString(fila[30]);

                if (Convert.ToString(fila[27]) != "")
                    telefonos = telefonos.Trim() + Convert.ToString(fila[27]) +"/";
                if (Convert.ToString(fila[28]) != "")
                    telefonos = telefonos.Trim() + Convert.ToString(fila[28]) + "/";
                if (Convert.ToString(fila[29]) != "")
                    telefonos = telefonos.Trim() + Convert.ToString(fila[29]);
            }
        }

        string archivo = imprime.GenerarRep(empresa, taller, orden, nomTaller, usuario, contacto, correo, telefonos);
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
        catch (Exception ex) {
            lblErrorVehiculo.Text = "Error al descargar el archivo: " + ex.Message;
        }

    }

    private void actualizaLocalizacion() {
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
                            lblErrorVehiculo.Text = "Hubo un error en la conexión, ";
                    else
                        lblErrorVehiculo.Text = "Ocurrio un error inesperado: " + actualizaLocalizacion[1].ToString();
                }
            }
            else
                lblErrorVehiculo.Text = "Ocurrio un error inesperado: " + regBitLoc[1].ToString();
        }
    }

    protected void ddlMarcaOrd_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlUnidadOrden.Items.Clear();
        ddlUnidadOrden.Text = "";
        ddlUnidadOrden.DataBind();
    }

    protected void ddlTvOrden_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlUnidadOrden.Items.Clear();
        ddlUnidadOrden.Text = "";
        ddlUnidadOrden.DataBind();
    }
}