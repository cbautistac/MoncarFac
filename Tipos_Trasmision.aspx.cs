using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipos_Trasmision : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvTransmisiones.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipos_Trasmision.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTransmisiones_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvTransmisiones.EditIndex = 1;
    }
    protected void grvUsuarios_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvTransmisiones.EditIndex = -1;
    }
    protected void grvTransmisiones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvTransmisiones.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipos_Trasmision.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void grvTransmisiones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvTransmisiones.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionTransmision(Convert.ToInt32(grvTransmisiones.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Tipos_Trasmision.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void grvTransmisiones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionTransmision(Convert.ToInt32(grvTransmisiones.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "No se puede eliminar el tipo de transmisión, esta relacionado con proceso";
                }
            }
            else
                lblError.Text = "El tipo de transmisión no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "El tipo de transmisión no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvTransmisiones.DataBind();
                txtAltaTransmision.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipos_Trasmision.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}