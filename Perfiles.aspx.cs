using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Perfiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvPerfiles.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Perfiles.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void btnAgregar_Click(object sender, ImageClickEventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvPerfiles.DataBind();
                txtAltaPerfil.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Perfiles.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void grvPerfiles_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvPerfiles.EditIndex = 1;
    }
    protected void grvPerfiles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvPerfiles.EditIndex = -1;
    }
    protected void grvPerfiles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvPerfiles.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Perfiles.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete")
        {
            try
            {
                SqlDataSource1.Delete();
                grvPerfiles.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Perfiles.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}
