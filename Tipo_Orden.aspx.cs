using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tipo_Orden : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                GridView1.DataBind();
                lblErrorNew.Text = "";
                lblError.Text = "";
            }
            catch (Exception x)
            {
                Session["errores"] = x.Message;
                Session["paginaOrigen"] = "Tipo_Orden.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Orden.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete")
        {
            try
            {
                SqlDataSource1.Delete();
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Tipo_Orden.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (GridView1.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionTipoOrden(Convert.ToInt32(GridView1.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Tipo_Orden.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                SqlDataSource1.Insert();
                GridView1.DataBind();
                txtDescripcionNew.Text = txtImporteHrsNew.Text = txtImporteHojaNew.Text = txtImportePintNew.Text = "";
                lblErrorNew.Text = "";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Tipo_Orden.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}