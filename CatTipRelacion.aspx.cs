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

public partial class CatTipRelacion : System.Web.UI.Page
{
    CatalTipRelacion datos = new CatalTipRelacion();
    Ejecuciones ejecuta = new Ejecuciones();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        if (lblTitulo.Text == "Agrega Tipo Factor")
        {


            string ClavTipRel = txtClaveTipR.Text;
            string Desc = txtDesc.Text;
            bool agregar = datos.agregaNuevoFactor(ClavTipRel, Desc);
            if (agregar)
            {

                txtClaveTipR.Text = "";
                txtDesc.Text = "";
                string script = "cierraNewEmi()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);

                RadGrid1.DataBind();
            }
        }
        else
        {
            string ClavTipFac = txtClaveTipR.Text;
            string Desc = txtDesc.Text;
            int TipFacClave = Convert.ToInt32(RadGrid1.SelectedValues["ClaveTipo"]);
            object[] existe = ejecuta.scalarToInt("select * from TipoRelacion_FSAT where Descripcion='" + Desc + "'");
            if (Convert.ToInt32(existe[1]) == 1)
            {
                lblErrorNuevo.Text = "Esa Clave ya existe, intente con otro";
            }
            else
            {
                datos.editDatos(ClavTipFac, Desc, TipFacClave);
                if (Convert.ToBoolean(datos.retorno[1]))
                {
                    txtClaveTipR.Text = "";
                    txtDesc.Text = "";

                    string script = "cierraNewEmi()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
                    RadGrid1.DataBind();
                }

            }
        }  
    }

    protected void lnkAgregar_Click(object sender, EventArgs e)
    {
        lblTitulo.Text = "Agrega Tipo Factor";
        string script = "abreNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
    }
    protected void lnkEditar_Click(object sender, EventArgs e)
    {
        CatalTipRelacion edt = new CatalTipRelacion();
        string cd = Convert.ToString(RadGrid1.SelectedValues["ClaveTipo"]);
        edt.cd = cd;
        edt.cargardatos();

        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];
            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtClaveTipR.Text = r1[0].ToString();
                txtDesc.Text = r1[1].ToString();
            }
            lblTitulo.Text = "Edita Tipo Factor";
            string script = "abreNewEmi()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
            txtClaveTipR.Enabled = false;
        }
    }
    protected void lnkBorrar_Click(object sender, EventArgs e)
    {
        CatalTipRelacion dlt = new CatalTipRelacion();
        string cd = Convert.ToString(RadGrid1.SelectedValues["ClaveTipo"]);
        dlt.cd = cd;
        dlt.elminarFactTipo();
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
}