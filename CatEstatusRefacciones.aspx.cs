using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatEstatusRefacciones : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GridGrupoOp.DataBind();
                lblError.Text = "";
                txtDescripcionAdd.Text = "";
            }
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "CatEstatusRefacciones.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
    protected void GridGrupoOp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            try
            {
                SqlDataSource1.Update();
                GridGrupoOp.DataBind();
                lblError.Text = "";
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "CatEstatusRefacciones.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Delete") { }
    }

    protected void GridGrupoOp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string modo = e.Row.RowState.ToString();
            string[] valores = null;
            try { valores = modo.Split(new char[] { ',' }); }
            catch (Exception) { modo = e.Row.ToString(); }
            if (valores != null)
            {
                for (int i = 0; i < valores.Length; i++)
                {
                    if (valores[i].Trim() == "Edit")
                    {
                        modo = "Edit";
                        break;
                    }
                    else
                        modo = valores[i].Trim();
                }
            }

            if (modo != "Edit")
            {
                var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
                var btnBtnEditar = e.Row.FindControl("btnEditar") as LinkButton;
                try
                {
                    if (GridGrupoOp.EditIndex == -1)
                    {
                        if (Convert.ToInt32(GridGrupoOp.DataKeys[e.Row.RowIndex].Value.ToString()) < 6 || Convert.ToInt32(GridGrupoOp.DataKeys[e.Row.RowIndex].Value.ToString())==11)
                        {
                            btnBtnEditar.Visible = btnBtnEliminar.Visible = false;
                        }
                        else
                        {
                            Catalogos cat = new Catalogos();
                            object[] valoresR = cat.tieneRelacionGrupoOp(Convert.ToInt32(GridGrupoOp.DataKeys[e.Row.RowIndex].Value.ToString()));
                            if (Convert.ToBoolean(valoresR[0]))
                            {
                                if (Convert.ToBoolean(valoresR[1]))
                                    btnBtnEliminar.Visible = false;
                                else
                                    btnBtnEliminar.Visible = true;
                            }
                            else
                                btnBtnEliminar.Visible = false;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(GridGrupoOp.DataKeys[e.Row.RowIndex].Value.ToString()) < 6 || Convert.ToInt32(GridGrupoOp.DataKeys[e.Row.RowIndex].Value.ToString()) == 11)
                        {
                            btnBtnEditar.Visible = btnBtnEliminar.Visible = false;
                        }
                        else
                        {
                            Catalogos cat = new Catalogos();
                            object[] valoresR = cat.tieneRelacionEstatus(Convert.ToInt32(GridGrupoOp.DataKeys[e.Row.RowIndex].Value.ToString()));
                            if (Convert.ToBoolean(valoresR[0]))
                            {
                                if (Convert.ToBoolean(valoresR[1]))
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
                    Session["paginaOrigen"] = "CatEstatusRefacciones.aspx";
                    Response.Redirect("AppErrorLog.aspx");
                }
            }
            else
            {

            }
        }
    }

    protected void GridGrupoOp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Catalogos cat = new Catalogos();
            object[] valores = cat.tieneRelacionGrupoOp(Convert.ToInt32(GridGrupoOp.DataKeys[e.RowIndex].Value.ToString()));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "El estatus no se puede eliminar ya que se encuentra ocupado en otro proceso";
                }
            }
            else
                lblError.Text = "El estatus no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x)
        {
            e.Cancel = true;
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {
            SqlDataSource1.Insert();
            GridGrupoOp.DataBind();
            lblError.Text = "";
            txtDescripcionAdd.Text = "";
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "CatEstatusRefacciones.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
}