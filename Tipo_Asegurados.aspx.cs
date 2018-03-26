using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipo_Asegurados : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvAsegurados.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Asegurados.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }

    }
    
    protected void grvAsegurados_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvAsegurados.EditIndex = 1;
    }
    protected void grvAsegurados_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvAsegurados.EditIndex = -1;
    }
    protected void grvAsegurados_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvAsegurados.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Asegurados.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete")
        {
            try
            {
                SqlDataSource1.Delete();
                grvAsegurados.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Asegurados.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvAsegurados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvAsegurados.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionTipoAsegurado(Convert.ToInt32(grvAsegurados.DataKeys[e.Row.RowIndex].Value.ToString()));
                    if (Convert.ToBoolean(valores[0]))
                    {
                        if (Convert.ToBoolean(valores[1]))
                            btnBtnEliminar.Visible = false;
                        else
                            btnBtnEliminar.Visible = true;
                    }
                    else
                        btnBtnEliminar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Asegurados.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvAsegurados_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionTipoAsegurado(Convert.ToInt32(grvAsegurados.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "El tipo no se elimino por que esta relacionado con otro proceso";
                }
            }
            else
                lblError.Text = "El tipo no se eslimino correctamente, verifique su conexión e intentelo nuevamente";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "El tipo no se eslimino correctamente, verifique su conexión e intentelo nuevamente";
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvAsegurados.DataBind();
                txtAltaAsegurado.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Asegurados.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}