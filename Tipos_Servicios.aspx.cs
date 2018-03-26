using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipos_Servicios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvTipoServicios.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipos_Servicios.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void grvTipoServicios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvTipoServicios.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipos_Servicios.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTipoServicios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvTipoServicios.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionServicios(Convert.ToInt32(grvTipoServicios.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Tipos_Servicios.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTipoServicios_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionServicios(Convert.ToInt32(grvTipoServicios.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "No se puede eliminar el tipo de servicio, esta relacionado con proceso";
                }
            }
            else
                lblError.Text = "El tipo de servicio no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "El tipo de servicio no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }

    protected void btnAceptarNew_Click1(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                SqlDataSource1.Insert();
                grvTipoServicios.DataBind();
                txtDescripcionNew.Text = txtPrefijoNew.Text = "";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Tipos_Servicios.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}