using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class Administracion : System.Web.UI.MasterPage
{
    Recepciones recepciones = new Recepciones();
    Permisos permisos = new Permisos();
    protected void Page_Load(object sender, EventArgs e)
    {
        cargaInfo();
        controlAccesos();
        lblversion.Text = ConfigurationManager.AppSettings["version"].ToString();
    }

    private void cargaInfo()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] != 1)
        {
            if (sesiones[0] == 0 || sesiones[1] == 0)
                Response.Redirect("Default.aspx");
        }
        else{
            if (sesiones[0] == 0)
                Response.Redirect("Default.aspx");
        }
        try
        {            
            lblUser.Text = recepciones.obtieneNombreUsuario(sesiones[0].ToString());            
            lblEmpresa.Text = recepciones.obtieneNombreEmpresa(sesiones[2].ToString());            
            lblTallerSesion.Text = recepciones.obtieneNombreTaller(sesiones[3].ToString());
        }
        catch (Exception )
        {
            
        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[4] { 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Session["user"].ToString());
            sesiones[1] = Convert.ToInt32(Session["perfil"].ToString());
            sesiones[2] = Convert.ToInt32(Session["empresa"].ToString());
            sesiones[3] = Convert.ToInt32(Session["taller"].ToString());
        }
        catch (Exception x )
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Administracion";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    private void controlAccesos() {
        int[] sesiones = obtieneSesiones();         
        permisos.idUsuario = sesiones[0];
        //permisos.obtienePermisos();
        //bool[] permisosUsuario = permisos.permisos;


        HtmlControl[] controles = {
        (HtmlControl)this.FindControl("madmon"),
        (HtmlControl)this.FindControl("mgenrales"),
        (HtmlControl)this.FindControl("mvehiculos"),
        (HtmlControl)this.FindControl("mpersonal"),
        (HtmlControl)this.FindControl("mutil"),
        (HtmlControl)this.FindControl("mclientes"),
        (HtmlControl)this.FindControl("mproveedores"),
            (HtmlControl)this.FindControl("subperfil"),
            (HtmlControl)this.FindControl("submenu"),
            (HtmlControl)this.FindControl("subsubm"),
            (HtmlControl)this.FindControl("subrmsub"),
            (HtmlControl)this.FindControl("subropme"),
            (HtmlControl)this.FindControl("subrprop"),
            (HtmlControl)this.FindControl("subparamcorr"),
            (HtmlControl)this.FindControl("subasigper"),
                (HtmlControl)this.FindControl("subValua"),
                (HtmlControl)this.FindControl("subPerfiles"),
                (HtmlControl)this.FindControl("subloca"),
                (HtmlControl)this.FindControl("subtasegu"),
                (HtmlControl)this.FindControl("subserv"),
                (HtmlControl)this.FindControl("subord"),
                (HtmlControl)this.FindControl("subpol"),
                (HtmlControl)this.FindControl("subgrpo"),
                (HtmlControl)this.FindControl("suboper"),
                (HtmlControl)this.FindControl("subestatus"),
                (HtmlControl)this.FindControl("subref"),
                (HtmlControl)this.FindControl("subcatcli"),
                (HtmlControl)this.FindControl("subcalifop"),
                    (HtmlControl)this.FindControl("submarca"),
                    (HtmlControl)this.FindControl("subtipovehi"),
                    (HtmlControl)this.FindControl("subunidad"),
                    (HtmlControl)this.FindControl("subtrans"),
                    (HtmlControl)this.FindControl("subtrac"),
                    (HtmlControl)this.FindControl("subgas"),
                    (HtmlControl)this.FindControl("subrin"),
                    (HtmlControl)this.FindControl("subvehi"),
                        (HtmlControl)this.FindControl("subpuestos"),
                        (HtmlControl)this.FindControl("subempleados"),
                            (HtmlControl)this.FindControl("subplaca"),
        (HtmlControl)this.FindControl("subZonas"),
        (HtmlControl)this.FindControl("subRefProc"),
        (HtmlControl)this.FindControl("mControl")};

        int[] codigos = { 2, 11, 25, 34, 37, 39, 40, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 26, 27, 28, 29, 30, 31, 32, 33, 35, 36, 38, 111, 110, 112 };

        //permisos.permisos = permisosUsuario;
        for(int i =0; i<codigos.Length;i++){
            permisos.permiso = codigos[i];
            permisos.tienePermisoIndicado();
            if (!permisos.tienePermiso)
                controles[i].Attributes["style"] = "display:none;";
            else
                controles[i].Attributes["style"] = "";
        }
    }

    protected void lnkCerrarSesion_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        CatUsuarios usuarios = new CatUsuarios();
        object[] actualizaAcceso = usuarios.actualizaBitacoraAcceso(sesiones[0], "I");
        if (Convert.ToBoolean(actualizaAcceso[0]))
        {
            Response.Redirect("Default.aspx");
        }
    }
}
