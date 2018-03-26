using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RepFacturas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            ddlEstatus.SelectedValue = "P";
            txtFechaIni.Text = new E_Utilities.Fechas().obtieneFechaLocal().AddMonths(-1).ToString("yyyy-MM-dd");
            txtFechaFin.Text = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
            RadGrid1.DataBind();
        }
    }

    protected void lnkBuscar_Click(object sender, EventArgs e)
    {
        RadGrid1.DataBind();
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[4] { 0, 0, 0, 0};
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
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
    protected void lnkImprime_Click(object sender, EventArgs e)
    {
        lblError.Text = "";        
        imprimeestatusf imprimir = new imprimeestatusf();
        int[] sesiones = obtieneSesiones();

        string archivo = imprimir.imprimeestatusfa(txtFechaIni.Text,txtFechaFin.Text);
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
}