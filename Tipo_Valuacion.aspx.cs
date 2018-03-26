using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipo_Valuacion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvTipoValuacion.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Valuacion.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void grvTipoValuacion_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvTipoValuacion.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Valuacion.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTipoValuacion_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvTipoValuacion.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionValuacion(Convert.ToInt32(grvTipoValuacion.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Tipo_Valuacion.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTipoValuacion_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionValuacion(Convert.ToInt32(grvTipoValuacion.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    lblError.Text = "No se pudo eliminar el tipo de valiación, esta siendo utilizado por otro proceso";
                    e.Cancel = true;
                }
            }
            else
                lblError.Text = "No se elimino correctamente el tipo de valiación, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            lblError.Text = "No se elimino correctamente el tipo de valiación, verifique su conexión e intentelo nuevamente mas tarde";
            e.Cancel = true;
        }
    }

    protected void btnAceptarNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                SqlDataSource1.Insert();
                grvTipoValuacion.DataBind();
                txtDescripcionNew.Text = txtPrefijoNew.Text = "";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Tipo_Valuacion.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}