using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipo_Rin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvRin.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Rin.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvRin_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvRin.EditIndex = 1;
    }
    protected void grvRin_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvRin.EditIndex = -1;
    }
    protected void grvRin_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvRin.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Rin.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvRin_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvRin.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionRin(Convert.ToInt32(grvRin.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Tipo_Rin.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvRin_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionRin(Convert.ToInt32(grvRin.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "No se puede eliminar el tipo de rin, esta siendo utilizado en otro proceso";
                }
            }
            else
                lblError.Text = "No se elimino correctamente el tipo de rin, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x)
        {
            e.Cancel = true;
            lblError.Text = "No se elimino correctamente el tipo de rin, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvRin.DataBind();
                txtAltaRin.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Rin.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}