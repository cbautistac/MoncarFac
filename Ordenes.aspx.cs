using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Globalization;
using System.Drawing;
using E_Utilities;
using Telerik.Web.UI;

public partial class Ordenes : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    Fechas fechas = new Fechas();
    Permisos permisos = new Permisos();
    Ejecuciones llamada = new Ejecuciones();

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }


    private void limpiaCombo(RadComboBox combo)
    {
        combo.Items.Clear();
        //combo.Items.Add(new ListItem("", ""));
        combo.DataBind();
        //combo.SelectedValue = "";
        combo.SelectedIndex = -1;
    }

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
        catch (Exception x)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
}