using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Empresas_Talleres : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                lblError.Text = "";
                lblOrden.Text = (Convert.ToInt32(DateTime.Now.ToString("yy")) * 10000).ToString();
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Empresas_Talleres.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    private void llenaElementos()
    {
        ddlEmpresas.Items.Clear();
        ddlTalleres.Items.Clear();
        ddlEmpresas.DataBind();
        ddlTalleres.DataBind();
        GridView1.DataBind();
        txtTopeEco.Text = txTopetRef.Text = "";
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            try
            {
                lblError.Text = "";
                string[] valores = e.CommandArgument.ToString().Split(';');
                CatUsuarios datosU = new CatUsuarios();
                string sEmpresa = valores[0];
                string sTaller = valores[1];
                bool eliminado = datosU.eliminaEmpresaTaller(sEmpresa, sTaller);
                if (eliminado)
                {
                    llenaElementos();
                }
                else
                    lblError.Text = "No se logro eliminar la relación";
                llenaElementos();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Empresas_Talleres.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
        else if (e.CommandName == "Update")
        {
            try
            {
                lblError.Text = "";
                string[] valores = e.CommandArgument.ToString().Split(';');
                CatUsuarios datosU = new CatUsuarios();
                string sEmpresa = valores[0];
                string sTaller = valores[1];

                TextBox topeEco = GridView1.Rows[GridView1.EditIndex].FindControl("txtTopeEcoM") as TextBox;
                TextBox topeRef = GridView1.Rows[GridView1.EditIndex].FindControl("txTopetRefM") as TextBox;
                TextBox txtTiempoMaxCot = GridView1.Rows[GridView1.EditIndex].FindControl("txtTiempoMaxCot") as TextBox;
                int didasMaxCot = Convert.ToInt32(txtTiempoMaxCot.Text);
                decimal topeEc = Convert.ToDecimal(topeEco.Text);
                int topeRe = Convert.ToInt32(topeRef.Text);
                if (didasMaxCot > 0)
                    SqlDataSource3.UpdateCommand = "Update Empresas_Taller set tiempo_max_cot=" + didasMaxCot + ", tope_economico=" + topeEc.ToString() + ", tope_refacciones=" + topeRe.ToString() + " where id_empresa=" + sEmpresa.Trim() + " and id_taller=" + sTaller.Trim();
                else
                    lblError.Text = "Debe ingresar una cantidad de horas máximo para la cotización y mayor a 1";
                GridView1.EditIndex = -1;
                llenaElementos();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Empresas_Talleres.aspx";
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
                    CatEmpresas catEmp = new CatEmpresas();
                    int empresa = Convert.ToInt32(GridView1.DataKeys[e.Row.RowIndex].Values[0].ToString());
                    int taller = Convert.ToInt32(GridView1.DataKeys[e.Row.RowIndex].Values[1].ToString());
                    object[] relacion = catEmp.tieneRelacionEmpresasTaller(empresa, taller);
                    if (Convert.ToBoolean(relacion[0]))
                    {
                        if (Convert.ToBoolean(relacion[1]))
                            btnBtnEliminar.Visible = false;
                        else
                            btnBtnEliminar.Visible = true;
                    }
                    else
                        btnBtnEliminar.Visible = false;
                }
                else {

                }
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Empresas_Talleres.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            CatEmpresas catEmp = new CatEmpresas();
            int empresa = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0].ToString());
            int taller = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[1].ToString());
            object[] valores = catEmp.tieneRelacionEmpresasTaller(empresa, taller);
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    lblError.Text = "No se elimino correctamente el registro ya que esta siendo utilizado en otro proceso";
                    e.Cancel = true;
                }
            }
            else
                lblError.Text = "El registro no se elimino correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Empresas_Talleres.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (Page.IsValid)
        {
            decimal eco = 0;
            int refac = 0;
            int maxd = 0;
            try { eco = Convert.ToDecimal(txtTopeEco.Text); }
            catch (Exception) { eco = 0; }
            try { refac = Convert.ToInt32(txTopetRef.Text); }
            catch (Exception) { refac = 0; }

            try { maxd = Convert.ToInt32(txtDiasMaxCot.Text); }
            catch (Exception) { maxd = 0; }

            if (eco != 0)
            {
                if (refac != 0)
                {
                    if (maxd > 0)
                    {
                        try
                        {
                            SqlDataSource3.Insert();
                            llenaElementos();
                        }
                        catch (Exception ex)
                        {
                            Session["errores"] = ex.Message;
                            Session["paginaOrigen"] = "Empresas_Talleres.aspx";
                            Response.Redirect("AppErrorLog.aspx");
                        }
                    }
                    else
                        lblError.Text = "Debe indicar un valor de máximo de horas para la cotización y mayor a cero";
                }
                else
                    lblError.Text = "Debe indicar un valor de tope máximo de refacciones correcto y mayor a cero";
            }
            else
                lblError.Text = "Debe indicar un valor de tope económico correcto y mayor a cero";
        }
    }
}