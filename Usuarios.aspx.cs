using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Usuarios : System.Web.UI.Page 
{
    CatErrores err = new CatErrores();
    CatUsuarios catUsers = new CatUsuarios();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                grvUsuarios.DataBind();
            }
            catch (Exception ex) {
                lblError.Text = "Se produjo el siguiente error: " + ex.Message;
                lblError.CssClass = "errores fondoBlanco";                
            }
        }
    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            lblError.Text = "";
            lblError.CssClass = "errores";            
            try
            {
                object[] valores = catUsers.existeUsuario(txtNick.Text);
                if (Convert.ToBoolean(valores[0]))
                {
                    if (!Convert.ToBoolean(valores[1]))
                    {
                        SqlDataSource1.Insert();
                        grvUsuarios.DataBind();
                        lblError.Text = "Usuario " + txtNick.Text.ToUpper() + " Agregado Exitósamente";
                        lblError.CssClass = "text-success textoBold";                        
                        txtNick.Text = txtNombreAlta.Text = txtPass.Text = "";
                        grvUsuarios.PageIndex = grvUsuarios.PageCount;
                    }
                    else
                    {
                        lblError.Text = err.mensajeErrorCatUsuarios(200);
                        lblError.CssClass = "errores";                        
                    }
                }
                else
                {
                    lblError.Text = valores[1].ToString();
                    lblError.CssClass = "errores";                    
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Se produjo el siguiente error: " + ex.Message;
                lblError.CssClass = "errores";                
            }
        }        
    }
   
    protected void grvUsuarios_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grvUsuarios.EditIndex = 1;
        lblError.Text = "";
        lblError.CssClass = "errores";        
    }
    protected void grvUsuarios_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grvUsuarios.EditIndex = -1;
        lblError.Text = "";
        lblError.CssClass = "errores";        
    }
    protected void grvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        lblError.CssClass = "errores";
        lblError.Text = "";        
    }

    protected void grvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError.CssClass = "errores";
        if (e.CommandName == "Update") {
            try
            {
                SqlDataSource1.Update();
                grvUsuarios.DataBind();
                lblError.Text = "Usuario Actualizado Exitósamente";
                lblError.CssClass = "text-success textoBold";                
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Usuarios.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "baja" )
        {
            int idUsuario = Convert.ToInt32(e.CommandArgument);
            bool baja = catUsers.altaBajaUsuario(idUsuario,'B');
            if (baja)
                grvUsuarios.DataBind();
            else
            {
                grvUsuarios.DataBind();
                lblError.Text = "El usuario no se logro dar de baja, verifique su conexión e intentelo nuevamente mas tarde";
            }
        }
        else if (e.CommandName == "alta")
        {
            int idUsuario = Convert.ToInt32(e.CommandArgument);
            bool baja = catUsers.altaBajaUsuario(idUsuario, 'A');
            if (baja)
                grvUsuarios.DataBind();
            else
            {
                grvUsuarios.DataBind();
                lblError.Text = "El usuario no se logro dar de alta, verifique su conexión e intentelo nuevamente mas tarde";
            }
        }
    }

    protected void grvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                var btnAlta = e.Row.FindControl("btnAlta") as LinkButton;
                var btnBaja = e.Row.FindControl("btnBaja") as LinkButton;
                char estatus = catUsers.obtieneEstatusUser(Convert.ToInt32(grvUsuarios.DataKeys[e.Row.RowIndex].Value));
                if (estatus == 'B')
                {
                    btnBaja.Visible = false;
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                }
                else
                    btnAlta.Visible = false;
                if (grvUsuarios.EditIndex == -1)
                {                    
                    object[] valores = catUsers.tieneRelacion(Convert.ToInt32(grvUsuarios.DataKeys[e.Row.RowIndex].Value));
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
            catch (Exception)
            {
                
            }
        }
    }
    protected void grvUsuarios_Sorting(object sender, GridViewSortEventArgs e)
    {
        lblError.Text = "";
        lblError.CssClass = "errores";        
    }

    protected void grvUsuarios_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblError.CssClass = "errores";
        try
        {
            object[] valores = catUsers.tieneRelacion(Convert.ToInt32(grvUsuarios.DataKeys[e.RowIndex].Value));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "No se puede eliminar el usuario, esta relacionado con proceso";
                }
            }
            else
                lblError.Text = "El usuario no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception)
        {
            e.Cancel = true;
            lblError.Text = "El usuario no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }
}