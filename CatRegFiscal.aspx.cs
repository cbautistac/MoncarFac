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

public partial class CatRegFiscal : System.Web.UI.Page
{
    CatalRegFiscal datos = new CatalRegFiscal();
    Ejecuciones ejecuta = new Ejecuciones();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void lnkAgregar_Click(object sender, EventArgs e)
    {
        lblTitulo.Text = "Agrega Regimen Fiscal";
        string script = "abreNewEmi()";
        txtClaveReg.Enabled = true;
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
        txtDesc.Text = "";
        txtFisc.Text = "";
        txtMoral.Text = "";
        txtClaveReg.Text = "";
    }
    protected void lnkEditar_Click(object sender, EventArgs e)
    {
        CatalRegFiscal edt = new CatalRegFiscal();
        string cd = Convert.ToString(RadGrid1.SelectedValues["ClaveRegimen"]);
        edt.cd = cd;
        edt.cargardatos();

        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];
            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtClaveReg.Text = r1[0].ToString();
                txtDesc.Text = r1[1].ToString();
                txtFisc.Text = r1[2].ToString();
                txtMoral.Text = r1[3].ToString();
            }
            lblTitulo.Text = "Edita Regimen Fiscal";
            string script = "abreNewEmi()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
            txtClaveReg.Enabled = false;
        }
    }
    protected void lnkBorrar_Click(object sender, EventArgs e)
    {
        CatalRegFiscal dlt = new CatalRegFiscal();
        string cd = Convert.ToString(RadGrid1.SelectedValues["ClaveRegimen"]);
        dlt.cd = cd;
        dlt.elminarRegimen();
        RadGrid1.DataBind();
    }
    protected void lnkSeleccionar_Click(object sender, EventArgs e)
    {

    }
    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkEditar.Visible = true;
        lnkBorrar.Visible = true;
        lnkSeleccionar.Visible = true;
    }
    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        if (lblTitulo.Text == "Agrega Regimen Fiscal")
        {

            string clveReg = txtClaveReg.Text;
            string desc = txtDesc.Text;
            string fisc = txtFisc.Text;
            string moral = txtMoral.Text;
            bool agregar = datos.agregaRegFisc(clveReg, desc, fisc, moral);
            if (agregar)
            {
                txtDesc.Text = "";
                txtFisc.Text = "";
                txtMoral.Text = "";
                txtClaveReg.Text = "";
                RadGrid1.DataBind();
            }
        }
        else
        {
            string desc = txtDesc.Text;
            string fisc = txtFisc.Text;
            string moral = txtMoral.Text;
            string clveReg = txtClaveReg.Text;
            int claveRF = Convert.ToInt32(RadGrid1.SelectedValues["ClaveRegimen"]);
            object[] existe = ejecuta.scalarToInt("select count(*)  from RegimenFiscal_FSAT where ClaveRegimen=" + claveRF);
            if (Convert.ToInt32(existe[1]) == 1)
            {
                lblErrorNuevo.Text = "Esa Clave ya existe, intente con otro";
            }
            else
            {
                datos.editDatos(clveReg, desc, fisc, moral, Convert.ToString(claveRF));
                if (Convert.ToBoolean(datos.retorno[1]))
                {
                    txtDesc.Text = "";
                    txtFisc.Text = "";
                    txtMoral.Text = "";
                    txtClaveReg.Text = "";
                    RadGrid1.DataBind();
                }
            }
        }
    }
}