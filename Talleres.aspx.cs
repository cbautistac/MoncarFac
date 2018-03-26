using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Talleres : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvTalleres.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Talleres.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTalleres_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvTalleres.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Talleres.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete")
        {
            try
            {
                SqlDataSource1.Delete();
                grvTalleres.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Talleres.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        
    }

    protected void grvTalleres_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvTalleres.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionTalleres(Convert.ToInt32(grvTalleres.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Talleres.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvTalleres_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvTalleres.EditIndex = e.NewEditIndex;
        grvTalleres.DataBind();
    }

    protected void grvTalleres_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionTalleres(Convert.ToInt32(grvTalleres.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    lblError.Text = "No se pudo eliminar el taller por que esta siendo utilizado en otro proceso";
                    e.Cancel = true;
                }
            }
            else
                lblError.Text = "El taller no se elimino correctamente verifique su conexion e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "El taller no se elimino correctamente verifique su conexion e intentelo nuevamente mas tarde";
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvTalleres.DataBind();
                txtAltaTaller.Text = "";
                txtAltaPrefijo.Text = "";
            }
            catch (Exception ex)
            {
                lblError.Text = "Se produjo el siguiente error: " + ex.Message;
            }
        }
    }
}