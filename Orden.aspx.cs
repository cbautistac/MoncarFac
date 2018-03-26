using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class Orden : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PanelMascara.Visible = false;
            PanelImgZoom.Visible = false;
            cargaInfo();
        }
    }

    private void cargaInfo()
    {
        if (Request.QueryString["s"] == "REC")
        {            
            GridView1.Columns[12].Visible = GridView1.Columns[13].Visible = true;
        }
        else
        {            
            GridView1.Columns[12].Visible = GridView1.Columns[13].Visible = false;
        }
        try
        {
            int[] sesiones = obtieneSesiones();

            string argumentos = "Folio:";
            DatosVehiculos vehiculos = new DatosVehiculos();
            object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculoOrd(sesiones[0], sesiones[1], sesiones[2], sesiones[3], sesiones[4]);
            if (Convert.ToBoolean(vehiculo[0]))
            {
                DataSet valores = (DataSet)vehiculo[1];
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    argumentos = argumentos.Trim() + " " + fila[3].ToString().ToUpper() + " / " + fila[1].ToString().ToUpper();
                    lblPropveedor.Text = fila[2].ToString().ToUpper();
                    break;
                }
            }
            lblOrdenSelect.Text = argumentos;
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[5] { 0, 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["o"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["t"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["c"]);
            sesiones[4] = Convert.ToInt32(Request.QueryString["p"]);
        }
        catch (Exception x)
        {
            sesiones = new int[5] { 0, 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    protected void lnkImprimeOrden_Click(object sender, EventArgs e)
    {
        ImpresionOrdenCompra imprime = new ImpresionOrdenCompra();
        int noOrden;
        int idEmpresa;
        int idTaller;
        int idCotizacion;
        noOrden = Convert.ToInt32(Request.QueryString["o"]);
        idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        idTaller = Convert.ToInt32(Request.QueryString["t"]);
        idCotizacion = Convert.ToInt32(Request.QueryString["c"]);

        string archivo = imprime.GenRepOrdTrabajo(noOrden, idEmpresa, idTaller, idCotizacion, 'O', Convert.ToInt32(Request.QueryString["u"]), 1, Request.QueryString["s"]);
        try
        {
            if (archivo != "")
            {
                FileInfo filename = new FileInfo(archivo);
                if (filename.Exists)
                {    
                    /*Response.Clear();
                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", filename.Name));
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(Server.MapPath(Path.Combine("~/files/" + filename.Name)));
                    Response.End();*/

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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string c = DataBinder.Eval(e.Row.DataItem, "id_refaccion").ToString();
            SqlDsFotosRef.SelectParameters["id_refaccion"].DefaultValue = DataBinder.Eval(e.Row.DataItem, "id_refaccion").ToString();
        }
    }

    protected void Image1_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imagenSel = (ImageButton)sender;
        PanelMascara.Visible = true;
        PanelImgZoom.Visible = true;
        imgZoom.ImageUrl = imagenSel.ImageUrl;
    }

    protected void btnCerrarImgZomm_Click(object sender, EventArgs e)
    {
        PanelMascara.Visible = false;
        PanelImgZoom.Visible = false;
    }
}