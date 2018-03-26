using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipo_Unidad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvTipoUnidad.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Unidad.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void grvTipoUnidad_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvTipoUnidad.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Unidad.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTipoUnidad_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvTipoUnidad.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();

                    int marca = Convert.ToInt32(grvTipoUnidad.DataKeys[e.Row.RowIndex].Values[0].ToString());
                    int tVehiculo = Convert.ToInt32(grvTipoUnidad.DataKeys[e.Row.RowIndex].Values[1].ToString());
                    int unidad = Convert.ToInt32(grvTipoUnidad.DataKeys[e.Row.RowIndex].Values[2].ToString());
                    object[] valores = cat.tieneRelacionTipoUnidad(marca, tVehiculo, unidad);
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
                Session["paginaOrigen"] = "Tipo_Unidad.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTipoUnidad_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            int marca = Convert.ToInt32(grvTipoUnidad.DataKeys[e.RowIndex].Values[0].ToString());
            int tVehiculo = Convert.ToInt32(grvTipoUnidad.DataKeys[e.RowIndex].Values[1].ToString());
            int unidad = Convert.ToInt32(grvTipoUnidad.DataKeys[e.RowIndex].Values[2].ToString());
            object[] valores = cat.tieneRelacionTipoUnidad(marca, tVehiculo, unidad);
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "No se elimino el tipo de unidad, esta siendo utilizado por otro proceso";
                }
            }
            else
                lblError.Text = "El tipo de unidad no se elimino correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "El tipo de unidad no se elimino correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }

    protected void btnAceptarNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                SqlDataSource1.Insert();
                grvTipoUnidad.DataBind();
                txtDescripcionNew.Text = "";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Tipo_Unidad.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}