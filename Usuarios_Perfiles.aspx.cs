using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Usuarios_Perfiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                llenaElementos();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Usuarios_Perfiles.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        string id_usuario = ddlUsuario.SelectedValue;
        string id_perfil = ddlPerfil.SelectedValue;
        SqlDataSource1.Insert();
        llenaElementos();
    }

    private void llenaElementos()
    {
        grvUPerfiles.DataBind();
        ddlPerfil.Items.Clear();
        ddlUsuario.Items.Clear();
        ddlUsuario.DataBind();
        ddlPerfil.DataBind();
    }
    
    protected void btnActualizar_Click(object sender, EventArgs e)
    {
        llenaElementos();
    }
    
    protected void grvUPerfiles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Delete")
            {
                string[] datos = e.CommandArgument.ToString().Split(';');
                string sUsuario = datos[0];
                string sPerfil = datos[1];
                CatUsuarios datosU = new CatUsuarios();
                bool eliminado = datosU.eliminaPerfilUsuarios(sUsuario, sPerfil);
                if (eliminado)
                {
                    llenaElementos();
                }
                else
                    lblError.Text = "No se logro eliminar el perfil";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Usuarios_Perfiles.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}