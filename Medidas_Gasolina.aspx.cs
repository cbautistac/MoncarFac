using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Medidas_Gasolina : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvGasolina.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Medidas_Gasolina.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void grvGasolina_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvGasolina.EditIndex = 1;
    }
    protected void grvGasolina_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvGasolina.EditIndex = -1;
    }
    protected void grvGasolina_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvGasolina.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Medidas_Gasolina.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }        
    }

    protected void grvGasolina_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvGasolina.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionGasolina(Convert.ToInt32(grvGasolina.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Medidas_Gasolina.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvGasolina_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionGasolina(Convert.ToInt32(grvGasolina.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "El registro no se puede eliminar por que esta siendo utilizado en otro proceso";
                }
            }
            else
                lblError.Text = "El registro no se elimino correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "El registro no se elimino correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                SqlDataSource1.Insert();
                grvGasolina.DataBind();
                txtAltaGasolina.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Medidas_Gasolina.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
}