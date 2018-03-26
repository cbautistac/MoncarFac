using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Puestos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvPuestos.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Puestos.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void grvUsuarios_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvPuestos.EditIndex = 1;
    }
    protected void grvUsuarios_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvPuestos.EditIndex = -1;
    }
    protected void grvPuestos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvPuestos.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Puestos.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete")
        {
            try
            {
                SqlDataSource1.Delete();
                grvPuestos.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Puestos.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvPuestos.DataBind();
                txtAltaPuesto.Text = "";
            }

            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Puestos.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }



    protected void grvPuestos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string id = DataBinder.Eval(e.Row.DataItem, "id_puesto").ToString();
            var btnEditar = e.Row.Cells[2].Controls[1].FindControl("btnEditar") as LinkButton;
            var btnEliminar = e.Row.Cells[3].Controls[1].FindControl("btnEliminar") as LinkButton;
            if (id == "26")
                btnEditar.Visible = btnEliminar.Visible = false;
        }
    }
}