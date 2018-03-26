using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Categoria_Cliente : System.Web.UI.Page
{
    CatClientes datos = new CatClientes();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void GridCatClientes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName== "Update")
        {
            try
            {
                SqlDataSource1.Update();
                GridCatClientes.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Categoria_Cliente.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName== "Delete") { }
    }

    protected void GridCatClientes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                if (GridCatClientes.EditIndex == -1)
                {
                    Catalogos cat = new Catalogos();
                    object[] valores = cat.tieneRelacionCategoCliente(Convert.ToInt32(GridCatClientes.DataKeys[e.Row.RowIndex].Value.ToString()));
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
                Session["paginaOrigen"] = "Categoria_Cliente.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void GridCatClientes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionCategoCliente(Convert.ToInt32(GridCatClientes.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "La categoria del cliente no se puede eliminar ya que esta relacionado con otro proceso";
                }
            }
            else
                lblError.Text = "La categoria del cliente no se elimino exitosamente, verifique su conexión e intentelo nuevamente";
        }
        catch (Exception x )
        {
            e.Cancel=true;
        }
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        bool entero = false;
        try
        {
            lblError.Text = "";
            int descMoInt = Convert.ToInt32(txtDesMo.Text);
            entero = true;
        }
        catch (Exception x)
        {
            lblError.Text = "En el campo Desc Mo solo puede introducir numeros con dos decimales";
            entero = false;
        }
        if (entero)
        {
            try
            {
                lblError.Text = "";
                int descRefInt = Convert.ToInt32(txtDesRef.Text);
                entero = true;
            }
            catch (Exception x)
            {
                lblError.Text = "En el campo Desc Ref solo puede introducir numeros con dos decimales";
                entero = false;
            }
            if (entero)
            {
                try
                {
                    if (txtDesc.Text != "" && txtDesMo.Text != "" && txtDesRef.Text != "" && txtPrefijo.Text != "")
                    {
                        bool insertado = datos.insertaCatCliente(txtDesc.Text, txtDesMo.Text, txtDesRef.Text, txtPrefijo.Text);
                        if (insertado)
                        {
                            txtDesc.Text = txtDesMo.Text = txtDesRef.Text = txtPrefijo.Text = lblError.Text = "";
                            /*SqlDataSource1.Insert();*/
                            GridCatClientes.DataBind();
                        }
                    }
                    else
                        lblError.Text = "Necesita llenar todos los campos";
                }
                catch (Exception ex)
                {
                    Session["errores"] = ex.Message;
                    Session["paginaOrigen"] = "Categoria_Cliente.aspx";
                    Response.Redirect("AppErrorLog.aspx");
                }
            }
        }
    }
}