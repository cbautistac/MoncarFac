using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipo_Cargo_Interno : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvCargos.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Cargo_Interno.aspx";
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
                grvCargos.DataBind();
                txtAltaCargo.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Cargo_Interno.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvCargos_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvCargos.EditIndex = 1;
    }
    protected void grvCargos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvCargos.EditIndex = -1;
    }
    protected void grvCargos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvCargos.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Cargo_Interno.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete")
        {
            try
            {
                SqlDataSource1.Delete();
                grvCargos.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Cargo_Interno.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}