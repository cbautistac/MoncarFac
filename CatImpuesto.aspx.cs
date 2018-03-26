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

public partial class CatImpuesto : System.Web.UI.Page
{
    CatalImpuesto datos = new CatalImpuesto();
    Ejecuciones ejecuta = new Ejecuciones();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void lnkAgregar_Click(object sender, EventArgs e)
    {
        lblTitulo.Text = "Agrega Impuesto";
        string script = "abreNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
        txtClaveImp.Enabled = false;
    }
    protected void lnkEditar_Click(object sender, EventArgs e)
    {
        CatalImpuesto edt = new CatalImpuesto();
        string cd = Convert.ToString(RadGrid1.SelectedValues["ClaveImpuesto"]);
        edt.cd = cd;
        edt.cargardatos();

        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];
            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtClaveImp.Text = r1[0].ToString();
                txtDesc.Text = r1[1].ToString();
                txtRet.Text = r1[2].ToString();
                txtTras.Text = r1[3].ToString();
                txtLocFed.Text = r1[4].ToString();
                txtEntApl .Text = r1[5].ToString();
            }
            lblTitulo.Text = "Edita Impuesto";
            string script = "abreNewEmi()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
            txtClaveImp.Enabled = false;
        }
    }
    protected void lnkBorrar_Click(object sender, EventArgs e)
    {
        CatalImpuesto dlt = new CatalImpuesto();
        string cd = Convert.ToString(RadGrid1.SelectedValues["ClaveImpuesto"]);
        dlt.cd = cd;
        dlt.elminarImpuesto();
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
        if (lblTitulo.Text == "Agrega Impuesto")
        {


            string ClavImp = txtClaveImp.Text;
            string Desc = txtDesc.Text = "";
            string Retenido = txtRet.Text = "";
            string Trasl = txtTras.Text ="" ;
            string LocFed =txtLocFed.Text ="" ;
            string EntAp = txtEntApl.Text = "";
            bool agregar = datos.agregaNuevoImpuesto(ClavImp, Desc, Retenido, Trasl, LocFed, EntAp);
            if (agregar)
            {

                 txtClaveImp.Text ="";
                txtDesc.Text = "";
                txtRet.Text = "" ;
                txtTras.Text ="";
                txtLocFed.Text ="";
                txtEntApl .Text = "";
            }
        }
        else
        {
            string ClavImp = txtClaveImp.Text;
            string Desc = txtDesc.Text = "";
            string Retenido = txtRet.Text = "";
            string Trasl = txtTras.Text ="" ;
            string LocFed =txtLocFed.Text ="" ;
            string EntAp = txtEntApl.Text = "";
            int impuClave = Convert.ToInt32(RadGrid1.SelectedValues["ClaveImpuesto"]);
            object[] existe = ejecuta.scalarToInt("select count(*)  from impuesto_fsat where ClaveImpuesto=" + impuClave);
            if (Convert.ToInt32(existe[1]) == 1)
            {
                lblErrorNuevo.Text = "Esa Clave ya existe, intente con otro";
            }
            else
            {
                datos.editDatos(ClavImp, Desc, Retenido, Trasl, LocFed, EntAp, impuClave);
                if (Convert.ToBoolean(datos.retorno[1]))
                {
                    txtClaveImp.Text = "";
                    txtDesc.Text = "";
                    txtRet.Text = "";
                    txtTras.Text = "";
                    txtLocFed.Text = "";
                    txtEntApl.Text = "";
                }
            }
        }  
    }
}