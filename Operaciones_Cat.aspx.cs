using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Operaciones_Cat : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Gridoperaciones.DataBind();
                lblError.Text = "";
                txtDescripcionAdd.Text = "";
            }
        }
        catch (Exception x)
        {
            lblError.Text = "Se produjo ún error: " + x.Message;
        }
    }
    protected void Gridoperaciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                Gridoperaciones.DataBind();
                lblError.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Usuarios.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    
    protected void Gridoperaciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (Gridoperaciones.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionOp(Convert.ToInt32(Gridoperaciones.DataKeys[e.Row.RowIndex].Value.ToString()));
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
            catch (Exception x )
            {

            }
        }
    }

    protected void Gridoperaciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionOp(Convert.ToInt32(Gridoperaciones.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "La operación no se puede eliminar ya que esta siendo utilizada en otro proceso";
                }
            }
            else
                lblError.Text = "La operacion no se elimino correctamente, verifique su conexión e intentelo nuevamnete mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "La operacion no se elimino correctamente, verifique su conexión e intentelo nuevamnete mas tarde";
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {
            SqlDataSource1.Insert();
            Gridoperaciones.DataBind();
            txtDescripcionAdd.Text = "";
            lblError.Text = "";
        }
        catch (Exception x)
        {
            lblError.Text = "Se produjo ún error en la inseción: " + x.Message;
        }
    }
}