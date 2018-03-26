using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Usuarios_Taller : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {                
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Usuarios_Taller.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    private void llenaElementos()
    {
        GridView1.DataBind();
        ddlEmpresas.Items.Clear();
        ddlTalleres.Items.Clear();
        ddlUsuario.Items.Clear();
        ddlEmpresas.DataBind();
        ddlTalleres.DataBind();
        ddlUsuario.DataBind();
    }

    protected void btnAgregar_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource3.Insert();
                llenaElementos();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Usuarios_Taller.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            try
            {
                string[] valores = e.CommandArgument.ToString().Split(';');
                CatUsuarios datosU = new CatUsuarios();
                string sEmpresa = valores[0];
                string sTaller = valores[1];
                string sUsuario = valores[2];
                bool eliminado = datosU.eliminaUsuarioTaller(sEmpresa,sTaller,sUsuario);
                if (eliminado)
                {
                    llenaElementos();
                }
                else
                    lblError.Text = "La relacion usuario-taller no se logro eliminar, intentelo nuevamnte y/o verifique su conexión";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Usuarios_Taller.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource3.Insert();
                llenaElementos();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Usuarios_Taller.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}