using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class AppErrorLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["errores"] == null)
                Response.Redirect("Default.aspx");
        }
        if (this.Session["errores"] != null)
            Label2.Text = "Se produjo el siguiente error en la página " + Session["paginaOrigen"].ToString() + ": " + this.Session["errores"].ToString();
        else
        {
            Label2.Text = "Error no Controlado, vuelva a iniciar sesión";
        }
    }
    protected void btnContinuar_Click(object sender, ImageClickEventArgs e)
    {
        if (Label2.Text != "Error no Controlado, vuelva a iniciar sesión")
        {
            if (Session["user"] != null)
            {
                if (Session["paginaOrigen"] != null)
                    Response.Redirect(Session["paginaOrigen"].ToString());
                else
                    Response.Redirect("Default.aspx");
            }else
                Response.Redirect("Default.aspx");
        }
        else
            Response.Redirect("Default.aspx");
    }
}