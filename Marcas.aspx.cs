using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Marcas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvMarcas.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Marcas.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void grvMarcas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvMarcas.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Marcas.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvMarcas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvMarcas.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionMarcas(Convert.ToInt32(grvMarcas.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Marcas.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvMarcas_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionMarcas(Convert.ToInt32(grvMarcas.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "La marca no se puede eliminar ya que esta siendo utilizada por otro proceso";
                }
            }
            else
                lblError.Text = "No se elimino la marca correctamente, verifique su conexión e intentelo nuevamnte mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "No se elimino la marca correctamente, verifique su conexión e intentelo nuevamnte mas tarde";
        }
    }

    protected void btnAceptarNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                SqlDataSource1.Insert();
                grvMarcas.DataBind();
                txtDescripcionNew.Text = "";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Marcas.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}