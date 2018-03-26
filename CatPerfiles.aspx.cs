using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatPerfiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvLocalizaciones.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "CatPerfiles.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvLocalizaciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                grvLocalizaciones.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "CatPerfiles.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete") { }
    }

    protected void grvLocalizaciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (grvLocalizaciones.EditIndex == -1)
                {
                    if (Convert.ToInt32(grvLocalizaciones.DataKeys[e.Row.RowIndex].Value.ToString()) < 8)
                    {
                        btnBtnEliminar.Visible = false;
                        var btnEditar = e.Row.FindControl("btnEditar") as LinkButton;
                        btnEditar.Visible = false;
                    }
                    else
                    {
                        Catalogos cat = new Catalogos();
                        object[] valores = cat.tieneRelacionPerfilesOrden(Convert.ToInt32(grvLocalizaciones.DataKeys[e.Row.RowIndex].Value.ToString()));
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
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "CatPerfiles.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void grvLocalizaciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionLocalizaciones(Convert.ToInt32(grvLocalizaciones.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "El Perfil no se puede eliminar, esta siendo utilizado en otro proceso";
                }
            }
            else
                lblError.Text = "El Perfil no se pudo eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x)
        {
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
                grvLocalizaciones.DataBind();
                txtDescripcionNew.Text = "";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "CatPerfiles.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}