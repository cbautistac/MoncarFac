using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Data;
using System.IO;

public partial class PagoDeducible : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            cargaDatosPie();
            cargaDatosOrden();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    txtDeducibleOrden.Enabled= txtDemerito.Enabled = txtNombreOrden.Enabled = txtApPatOrden.Enabled = txtApMatOrden.Enabled = ddlFirmantes.Enabled = ddlForma.Enabled = btnAceptar.Visible = false;
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
            Session["paginaOrigen"] = "PagoDeducible.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    private void cargaDatosOrden()
    {
        lblError.Text = "";
        try
        {
            int empresa = Convert.ToInt32(Request.QueryString["e"]);
            int taller = Convert.ToInt32(Request.QueryString["t"]);
            int orden = Convert.ToInt32(Request.QueryString["o"]);

            object[] datosOrden = recepciones.obtieneInfoOrden(orden, empresa, taller);
            if (Convert.ToBoolean(datosOrden[0]))
            {
                DataSet ordenDatos = (DataSet)datosOrden[1];                
                foreach (DataRow filaOrd in ordenDatos.Tables[0].Rows)
                {                    
                    txtNombreOrden.Text = filaOrd[17].ToString().Trim();
                    txtApPatOrden.Text = filaOrd[18].ToString().Trim();
                    txtApMatOrden.Text = filaOrd[19].ToString().Trim();
                    txtDeducibleOrden.Text = filaOrd[41].ToString().Trim();
                    txtDemerito.Text = filaOrd[63].ToString().Trim();
                }
            }
            else                
                lblError.Text = "No se pudo cargar la información de la orden: " + orden.ToString() + ", por favor intente mas tarde";
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
        }
    }
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try {
            decimal monto = 0, montoDemerito=0;
            try { monto = Convert.ToDecimal(txtDeducibleOrden.Text); }
            catch (Exception) { monto = 0; }
            try { montoDemerito = Convert.ToDecimal(txtDemerito.Text); }
            catch (Exception) { montoDemerito = 0; }

            if (monto != 0)
            {
                int[] sesiones = obtieneSesiones();
                object[] actualizado = recepciones.actualizaOrdenDeducible(sesiones, monto, montoDemerito, txtNombreOrden.Text, txtApPatOrden.Text, txtApMatOrden.Text);
                if (Convert.ToBoolean(actualizado[0])) {
                    CartaDeducible imprime = new CartaDeducible();
                    int empresa = Convert.ToInt32(Request.QueryString["e"]);
                    int taller = Convert.ToInt32(Request.QueryString["t"]);
                    int orden = Convert.ToInt32(Request.QueryString["o"]);

                    string nomTaller = recepciones.obtieneNombreTaller(Request.QueryString["t"]);
                    string usuario = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);
                    actualizaFase();
                                        
                    string archivo = imprime.GenRepOrdTrabajo(empresa, taller, orden, nomTaller, usuario, Convert.ToInt32(ddlFirmantes.SelectedValue), ddlForma.SelectedValue);
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
                    catch (Exception ex)
                    {
                        lblError.Text = "Error al descargar el archivo: " + ex.Message;
                    }
                }
                else
                    lblError.Text = "Error: " + Convert.ToString(actualizado[1]);
            }
            else
                lblError.Text = "El monto de deducible no es válido";
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    private void actualizaFase()
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

                if (faseSActual < 7)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 7);
                }
            }
        }
        catch (Exception) { }

    }
}