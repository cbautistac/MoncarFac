using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatEmpleados : System.Web.UI.Page
{
    DatosEmpleados datos = new DatosEmpleados();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridEmpleados.DataBind();
            ddlManoObra.DataBind();
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string nombre = txtNombre.Text;
        string paterno = txtAPaterno.Text;
        string materno = txtAMaterno.Text;
        string puesto = ddlManoObra.SelectedValue;
        string pichonera = txtPichonera.Text;
        string tipo = ddlTipo.SelectedValue;
        bool agregado = datos.agregaNuevoEmpleado(nombre, paterno, materno, puesto, pichonera, tipo);
        if (agregado)
        {
            txtAMaterno.Text = "";
            txtAPaterno.Text = "";
            txtNombre.Text = "";
            txtPichonera.Text = "";
            lblError.Text = "";
            GridEmpleados.DataBind();
        }
        else
            lblError.Text = "No se logro insertar el empleado, verifique su conexión e intentelo nuevamnete";
    }
    
    protected void GridEmpleados_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridEmpleados.EditIndex = e.NewEditIndex;
        GridEmpleados.DataBind();
    }

    protected void GridEmpleados_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        e.Cancel = true;
        GridEmpleados.EditIndex = -1;
        GridEmpleados.DataBind();
    }

    protected void GridEmpleados_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblError.Text = "";
        int idEmp = Convert.ToInt32(GridEmpleados.DataKeys[e.RowIndex].Value);
        GridViewRow row = (GridViewRow)GridEmpleados.Rows[e.RowIndex];
        TextBox txtNombreEdit = row.FindControl("txtNombreEdit") as TextBox;
        TextBox txtPaternoEdit = row.FindControl("txtPaternoEdit") as TextBox;
        TextBox txtMaternoEdit = row.FindControl("txtMaternoEdit") as TextBox;
        TextBox txtPichoneraEdit = row.FindControl("txtPichoneraEdit") as TextBox;
        DropDownList ddlPuestoEdit = row.FindControl("ddlPuestoEdit") as DropDownList;
        DropDownList ddlTipoEdit = row.FindControl("ddlTipoEdit") as DropDownList;

        string nombre = txtNombreEdit.Text;
        string paterno = txtPaternoEdit.Text;
        string materno = txtMaternoEdit.Text;
        string puesto = ddlPuestoEdit.SelectedValue;
        string pichonera = txtPichoneraEdit.Text;
        string tipo = ddlTipoEdit.SelectedValue;
        bool actualizado = datos.actualizaEmpleado(idEmp, nombre, paterno, materno, puesto, pichonera, tipo);
        if (!actualizado)
        {
            lblError.Text = "No se logro actualizar el empleado, verifique su conexión e intentelo nuevamente";
            e.Cancel = true;
            GridEmpleados.EditIndex = -1;
            GridEmpleados.DataBind();
        }
    }

    protected void GridEmpleados_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        GridEmpleados.EditIndex = -1;
        GridEmpleados.DataBind();
    }

    protected void GridEmpleados_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Updates")
        {
            lblError.Text = "";
            int idEmp = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            TextBox txtNombreEdit = GridEmpleados.Rows[gvr.RowIndex].FindControl("txtNombreEdit") as TextBox;
            TextBox txtPaternoEdit = GridEmpleados.Rows[gvr.RowIndex].FindControl("txtPaternoEdit") as TextBox;
            TextBox txtMaternoEdit = GridEmpleados.Rows[gvr.RowIndex].FindControl("txtMaternoEdit") as TextBox;
            TextBox txtPichoneraEdit = GridEmpleados.Rows[gvr.RowIndex].FindControl("txtPichoneraEdit") as TextBox;
            DropDownList ddlPuestoEdit = GridEmpleados.Rows[gvr.RowIndex].FindControl("ddlPuestoEdit") as DropDownList;
            DropDownList ddlTipoEdit = GridEmpleados.Rows[gvr.RowIndex].FindControl("ddlTipoEdit") as DropDownList;

            string nombre = txtNombreEdit.Text;
            string paterno = txtPaternoEdit.Text;
            string materno = txtMaternoEdit.Text;
            string puesto = ddlPuestoEdit.SelectedValue;
            string pichonera = txtPichoneraEdit.Text;
            string tipo = ddlTipoEdit.SelectedValue;
            bool actualizado = datos.actualizaEmpleado(idEmp, nombre, paterno, materno, puesto, pichonera, tipo);
            if (!actualizado)
            {
                lblError.Text = "No se logro actualizar el empleado, verifique su conexión e intentelo nuevamente";
                GridEmpleados.EditIndex = -1;
                GridEmpleados.DataBind();
            }
            else
            {
                GridEmpleados.EditIndex = -1;
                GridEmpleados.DataBind();
            }
        }
        /*else if (e.CommandName == "Delete") {
            lblError.Text = "";
            int idEmp = Convert.ToInt32(e.CommandArgument);
            bool relacionado = datos.tieneRelacionEmpleado(idEmp.ToString());
            if (!relacionado) {
                bool actualizado = datos.eliminaEmpleado(idEmp);
                if (!actualizado)
                {
                    lblError.Text = "No se logro eliminar el empleado, verifique su conexión e intentelo nuevamente";                    
                    GridEmpleados.DataBind();
                }else
                    GridEmpleados.DataBind();
            }
            
        }*/
    }
    protected void GridEmpleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridEmpleados.PageIndex = e.NewPageIndex;
        GridEmpleados.DataBind();
    }
    protected void GridEmpleados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        { 
            string modo = e.Row.RowState.ToString();
            string[] valores = null;
            try { valores = modo.Split(new char[] { ',' }); }
            catch (Exception) { modo = e.Row.ToString(); }
            if (valores != null) {
                for (int i = 0; i < valores.Length; i++) {
                    if (valores[i].Trim() == "Edit")
                    {
                        modo = "Edit";
                        break;
                    }
                    else
                        modo = valores[i].Trim();
                }
            }

            if (modo == "Edit")
            {
                string puesto = DataBinder.Eval(e.Row.DataItem, "Puesto").ToString();
                string tipo_empleado = DataBinder.Eval(e.Row.DataItem, "tipo_empleado").ToString();
                DropDownList ddlPuestoEdit = e.Row.FindControl("ddlPuestoEdit") as DropDownList;
                DropDownList ddlTipoEdit = e.Row.FindControl("ddlTipoEdit") as DropDownList;
                ddlPuestoEdit.DataBind();

                try { ddlPuestoEdit.SelectedValue = puesto; }
                catch (Exception) { }
                try { ddlTipoEdit.SelectedValue = tipo_empleado; }
                catch (Exception) { }
            }
            else {
                LinkButton btnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
                string empleado = DataBinder.Eval(e.Row.DataItem, "idEmp").ToString();
                bool relacionado = datos.tieneRelacionEmpleado(empleado);
                if (relacionado)
                    btnEliminar.Visible = false;
                else
                    btnEliminar.Visible = true;
            }
        }
    }
}