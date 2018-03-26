using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class ConsultaNotificaciones : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();

    protected void Page_Load(object sender, EventArgs e)
    {
        cargaInfo();
        if (!IsPostBack)
        {
            fechas.fecha = fechas.obtieneFechaLocal();
            fechas.tipoFormato = 4;
            string fechaRetorno = fechas.obtieneFechaConFormato();            
            txtFechaIni.Text = fechaRetorno;
            GridView1.DataBind();
        }
    }
    protected void chkLeido_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox check = (CheckBox)sender;
        int valor = Convert.ToInt32(check.ToolTip.ToString());
        int notificacion =valor;        
        Notificaciones noti = new Notificaciones();
        noti.Fecha = Convert.ToDateTime(txtFechaIni.Text);
        noti.Estatus = "V";
        noti.Entrada = notificacion;
        noti.Empresa = Convert.ToInt32(Request.QueryString["e"]);
        noti.Taller = Convert.ToInt32(Request.QueryString["t"]);
        noti.actualizaEstado();
        GridView1.DataBind();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string estatus = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
            if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
            {
                var check = e.Row.Cells[7].Controls[1].FindControl("chkLeido") as CheckBox;
                if (estatus == "P")
                {
                    check.Checked = false;
                    check.Visible = true;
                    e.Row.CssClass = "alert-info";
                }
                else {
                    check.Checked = true;
                    check.Visible = false;
                }
            }
        }
    }
    
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void lnkRegresar_Click(object sender, EventArgs e)
    {
        if(Request.QueryString["pag"]=="BienvenidaOrdenes")
            Response.Redirect(Request.QueryString["pag"] + ".aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
        else
            Response.Redirect(Request.QueryString["pag"] + ".aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
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

    private void cargaInfo()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0)
            Response.Redirect("Default.aspx");
        try
        {
            lblEmpresa.Text = recepciones.obtieneNombreEmpresa(Request.QueryString["e"]);
            lblUser.Text = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);
            lblTallerSesion.Text = recepciones.obtieneNombreTaller(Request.QueryString["t"]);
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}