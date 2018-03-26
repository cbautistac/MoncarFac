using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipo_Vehiculo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvVehiculo.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Vehiculo.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void grvVehiculo_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvVehiculo.EditIndex = 1;        
    }
    protected void grvVehiculo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvVehiculo.EditIndex = -1;
    }
    protected void grvVehiculo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update") {
            try
            {
                SqlDataSource1.Update();
                grvVehiculo.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Vehiculo.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvVehiculo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvVehiculo.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionTipoVehiculo(Convert.ToInt32(grvVehiculo.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Tipo_Vehiculo.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvVehiculo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionTipoVehiculo(Convert.ToInt32(grvVehiculo.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "No se puede eliminar el tipo de vehiculo, esat siendo utilizado por otro proceso";
                }
            }
            else
                lblError.Text = "El tipo de vehiculo no se elimino correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "El tipo de vehiculo no se elimino correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvVehiculo.DataBind();
                txtAltaVehiculo.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Vehiculo.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}

