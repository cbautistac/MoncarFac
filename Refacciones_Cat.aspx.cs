using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Refacciones_Cat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GridRefacciones.DataBind();
                lblError.Text = "";
                txtDescripcionAdd.Text = "";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Refacciones_Cat.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
    protected void GridRefacciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                GridRefacciones.DataBind();
                lblError.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Refacciones_Cat.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void GridRefacciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (GridRefacciones.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionRefacciones(Convert.ToInt32(GridRefacciones.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Refacciones_Cat.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void GridRefacciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionRefacciones(Convert.ToInt32(GridRefacciones.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "No se pudo eliminar la refacción por que se encuntra vinculada en otro proceso";
                }
            }
            else
                lblError.Text = "no se pudo eliminar correctamente la refacción, verifique su conexion e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "no se pudo eliminar correctamente la refacción, verifique su conexion e intentelo nuevamente mas tarde";
        }
    }
    
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {
            SqlDataSource1.Insert();
            GridRefacciones.DataBind();
            txtDescripcionAdd.Text = "";
            lblError.Text = "";
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Refacciones_Cat.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}