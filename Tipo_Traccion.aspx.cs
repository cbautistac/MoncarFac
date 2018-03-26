using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipo_Traccion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvTraccion.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Traccion.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void grvTraccion_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvTraccion.EditIndex = 1;
    }
    protected void grvTraccion_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvTraccion.EditIndex = -1;
    }
    protected void grvTraccion_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvTraccion.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Traccion.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTraccion_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvTraccion.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionTranccion(Convert.ToInt32(grvTraccion.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Tipo_Traccion.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTraccion_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
                Catalogos cat = new Catalogos();
                object[] valores = cat.tieneRelacionTranccion(Convert.ToInt32(grvTraccion.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    lblError.Text = "No se puede eliminar el tipo de transacción, esta siendo utilizado por otro proceso";
                    e.Cancel = true;
                }
            }
            else
                lblError.Text = "No se elimino correctamente el tipo de tracción, verifique su conexión e intentelo nuevam,ente mas tarde";
            }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "No se elimino correctamente el tipo de tracción, verifique su conexión e intentelo nuevam,ente mas tarde";
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvTraccion.DataBind();
                txtAltaTraccion.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Traccion.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}