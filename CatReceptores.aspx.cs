using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.IO;
using E_Utilities;


public partial class CatReceptores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
        
        }
    }

    protected void lnkAbreAgrega_Click(object sender, EventArgs e)
    {
        borraCampos();
        modalNuevo.Title = "Nuevo Receptor";
        lblEditaAgrega.Text = "Agrega";
        string script = "abreNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
    }

    protected void lnkGuardaEdita_Click(object sender, EventArgs e)
    {
        Receptores ag = new Receptores();
        ag.rfc = txtRFC.Text;
        ag.razonSocial = txtNombre.Text;
        ag.correo = txtCorreo.Text;
        ag.correoCC = txtCoreoCC.Text;
        ag.correoCCO = txtCorreoCCO.Text;
        ag.calle = txtCalle.Text;
        ag.nExt = txtNoExt.Text;
        ag.nInt = txtNoInt.Text;
        ag.localidad = txtLocalidad.Text;
        ag.referencia = txtReferencia.Text;
        ag.cuenta = txtCuenta.Text;
        ag.pais = Convert.ToInt32(ddlPais.SelectedValue);
        ag.municipio = Convert.ToInt32(ddlMunicipio.SelectedValue);
        ag.estado = Convert.ToInt32(ddlEstado.SelectedValue);
        ag.colonia = Convert.ToInt32(ddlColonia.SelectedValue);
        ag.cp = Convert.ToInt32(ddlCP.SelectedValue);
        if (lblEditaAgrega.Text == "Agrega")
        {
            ag.agregaReceptor();
            if(Convert.ToBoolean(ag.retorno[1]))
            {
                lblError.Text = "Se agrego exitosamente el receptor";
                RadGrid1.DataBind();
                borraCampos();
                string script = "cierraNewEmi()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
            }
            else
            {
                lblError.Text = "Error al agregar el receptor:" + ag.retorno[1].ToString();
            }
        }
        else
        {
            ag.idReceptor = Convert.ToInt32(RadGrid1.SelectedValues["IdRecep"]);
        }
    }

    private void borraCampos()
    {
    
    }
    
    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnBorrar.Visible = true;
        btnEditar.Visible = true;
        btnSeleccionar.Visible = true;
    }
    
    protected void lnkCerrar_Click(object sender, EventArgs e)
    {
        borraCampos();
        string script = "cierraNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        Receptores br = new Receptores();
        br.idReceptor = Convert.ToInt32(RadGrid1.SelectedValues["IdRecep"]);
        br.borrarReceptor();
        if(Convert.ToBoolean(br.retorno[1]))
        {
            RadGrid1.DataBind();
            lblError.Text = "Receptor eliminado correctamente";
        }
        else
            lblError.Text="Error al eliminar el receptor";
    }
}