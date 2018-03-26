using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Clasificacion_Empleados : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvClasificacion.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Clasificacion_Empleados.aspx";
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
                grvClasificacion.DataBind();
                txtAltaClasificacion.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Clasificacion_Empleados.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void grvClasificacion_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvClasificacion.EditIndex = 1;
    }
    protected void grvClasificacion_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvClasificacion.EditIndex = -1;
    }
    protected void grvClasificacion_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvClasificacion.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Usuarios.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete")
        {
            try
            {
                SqlDataSource1.Delete();
                grvClasificacion.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Clasificacion_Empleados.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}