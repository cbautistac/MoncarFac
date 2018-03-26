using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Politicas_Pago : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    protected void GridPoliticasPag_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName== "Update")
        {
            try
            {
                lblError.Text = "";
                SqlDataSource1.Update();
                GridPoliticasPag.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Politicas_Pago.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void GridPoliticasPag_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (GridPoliticasPag.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionPoliticaPago(Convert.ToInt32(GridPoliticasPag.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Politicas_Pago.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void GridPoliticasPag_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (GridPoliticasPag.EditIndex == -1)
            {
                Catalogos cat = new Catalogos();
                object[] valores = cat.tieneRelacionPoliticaPago(Convert.ToInt32(GridPoliticasPag.DataKeys[e.RowIndex].Value.ToString()));
                if (Convert.ToBoolean(valores[0]))
                {
                    if (!Convert.ToBoolean(valores[1])) { }
                    else
                    {
                        e.Cancel = true;
                        lblError.Text = "No se pudo eliminar la pilitica, esta siendo utilizada por oto proceso";
                    }
                }
                else
                    lblError.Text = "no se pudo eliminar el proceso correctamente, verifique su conexión e intentelo nuevamente mas tarde";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "no se pudo eliminar el proceso correctamente, verifique su conexión e intentelo nuevamente mas tarde";
            e.Cancel = true;
        }
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtClvPolitica.Text != "" && txtDescripcion.Text != "" && txtDescripcionDescuento.Text != "" && txtDescuento.Text != "")
            {
                SqlDataSource1.Insert();
                GridPoliticasPag.DataBind();
                txtClvPolitica.Text = txtDescripcion.Text = txtDescripcionDescuento.Text = txtDiasPlazo.Text = txtDescuento.Text = lblError.Text = "";
            }
            else
                lblError.Text = "Necesita llenar todos los campos";
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Politicas_Pago.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}