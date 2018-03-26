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

public partial class CatBancos : System.Web.UI.Page
{
    CatalBancos datos = new CatalBancos();
    Ejecuciones ejecuta = new Ejecuciones();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        if(lblTitulo.Text=="Agrega Banco")
        {

        
        string RFC = txtRfcBanco.Text;
        string nomBank = txtBanco.Text;
        string claveBank = txtIdBanco.Text;
        bool agregar = datos.agregaNuevoBanco(RFC, nomBank, claveBank);
        if (agregar)
        {
            txtRfcBanco.Text = "";
            txtBanco.Text = "";
            txtIdBanco.Text = "";
            RadGrid1.DataBind();
        }
        }
        else
        {
            string RFC = txtRfcBanco.Text;
            string nomBank = txtBanco.Text;
            int claveBank = Convert.ToInt32(txtIdBanco.Text);
            int IdBanco =Convert.ToInt32(RadGrid1.SelectedValues["id_Banco"]);
            object[] existe = ejecuta.scalarToInt("select count(*)  from cat_bancos_f where Clave_Banco=" + claveBank);
            if(Convert.ToInt32( existe[1])==1)
            {
                lblErrorNuevo.Text = "Esa Clave ya existe, intente con otro";
            }
            else { 
            datos.editDatos(RFC, nomBank, claveBank, IdBanco);
            if (Convert.ToBoolean(datos.retorno[1]))
            {
                txtRfcBanco.Text = "";
                txtBanco.Text = "";
                txtIdBanco.Text = "";
                RadGrid1.DataBind();
            }
            }
        }

    }

    protected void lnkEditar_Click(object sender, EventArgs e)
    {
        
        CatalBancos edt = new CatalBancos();
        string cd = Convert.ToString(RadGrid1.SelectedValues["id_Banco"]);
        edt.cd = cd;
        edt.cargardatos();

        if(Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];
            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtRfcBanco.Text = r1[1].ToString() ;
                txtBanco.Text = r1[2].ToString() ;
                txtIdBanco.Text = r1[3].ToString() ;
            }
            lblTitulo.Text= "Edita Bancos";
            string script = "abreNewEmi()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
        }
    }
    
    protected void lnkBorrar_Click(object sender, EventArgs e)
    {
        CatalBancos dlt = new CatalBancos();
        string cd = Convert.ToString(RadGrid1.SelectedValues["id_Banco"]);
        dlt.cd = cd ;
        dlt.elminarBanco();
        RadGrid1.DataBind();

    }
    protected void lnkBorrar_Click1(object sender, EventArgs e)
    {

    }
    protected void lnkSeleccionar_Click(object sender, EventArgs e)
    {

    }
    protected void lnkAgregar_Click(object sender, EventArgs e)
    {
        lblTitulo.Text = "Agrega Banco";
        string script = "abreNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
    }
    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkEditar.Visible = true;
        lnkBorrar.Visible = true;
        lnkSeleccionar.Visible = true;
    }
}