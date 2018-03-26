using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.IO;

public partial class RepOrdenes : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFechaIni.SelectedDate = fechas.obtieneFechaLocal();
            txtFechaFin.SelectedDate = txtFechaIni.SelectedDate;
        }
    }

    protected void lnkBuscar_Click(object sender, EventArgs e)
    {
        string empresa = Request.QueryString["e"];
        string taller = Request.QueryString["t"];
        string condicion = "";
        string estatus = armaEstatus();
        string localizacion= armaValores(chkLocalizacion);
        string perfil = armaValores(chkPerfiles);

        if (estatus != "")
            condicion = " and orp.status_orden in (" + estatus + ") ";
        if (localizacion != "")
            condicion = condicion + " and orp.id_localizacion in (" + localizacion + ") ";
        if (perfil != "")
            condicion = condicion + " and orp.id_perfilOrden in (" + perfil + ") ";
        if (ddlClienteNuevo.SelectedValue != "")
            condicion = condicion + " and orp.id_cliprov = " + ddlClienteNuevo.SelectedValue + " ";


        ImprimeOrdenes impresion = new ImprimeOrdenes();
        string archivo = impresion.GeneraReporte(empresa, taller, condicion, Convert.ToDateTime(txtFechaIni.SelectedDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtFechaFin.SelectedDate).ToString("yyyy-MM-dd"),ddlClienteNuevo.SelectedValue);
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

    private string armaValores(CheckBoxList chkList)
    {
        string valores = "";
        for (int i = 0; i < chkList.Items.Count; i++) {
            if(chkList.Items[i].Selected)
                valores = valores + chkList.Items[i].Value + ",";
            
        }
        if(valores!="")
            valores = valores.Substring(0, valores.Length - 1);
        return valores;
    }

    private string armaEstatus()
    {
        string valores = "";
        for (int i = 0; i < chkEstatus.Items.Count; i++) {
            if(chkEstatus.Items[i].Selected)
                valores = valores + "'" + chkEstatus.Items[i].Value + "',";
            
        }
        if(valores!="")
            valores = valores.Substring(0, valores.Length - 1);
        return valores;
    }
}