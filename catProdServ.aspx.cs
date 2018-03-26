using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Telerik.Web.UI;
using E_Utilities;

public partial class catProdServ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        string script = "abreModEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
        ProdServ edt = new ProdServ();
        string codigo = Convert.ToString(RadGrid1.SelectedValues["ClaveProdServ"]);
        edt.codigo = codigo;
        edt.obtieneservEdit();

        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];

            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtClaveEDT.Text = r1[0].ToString();
                txtdesEDT.Text = r1[1].ToString();
                txtivaEDT.Text = r1[2].ToString();
                txtepsEDT.Text = r1[3].ToString();
                txtComplementoEDT.Text = r1[4].ToString();
                txtComplementoEDT.Text = txtComplementoEDT2.Text;
                txtComplementoEDT.Visible = false;
            }
        }

    }

    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAgregar.Visible = true;
        btnEditar.Visible = true;
        btnEliminar.Visible = true;
        btnSelec.Visible = true;
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        ProdServ edt = new ProdServ();
        string codigo = Convert.ToString(RadGrid1.SelectedValues["ClaveProdServ"]);
        edt.codigo = codigo;
        edt.eliminaMoneda();
        RadGrid1.DataBind();
    }

    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        ProdServ agr = new ProdServ();
        agr.codigo = txt_clave.Text;
        agr.Descripcion = txtdes.Text;
        agr.iva = txtiva.Text;
        agr.eps = txteps.Text;
        agr.complemento = txtcomplemento.Text;
        agr.agregarServicio();
        RadGrid1.DataBind();
        string script = "cierraNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

    protected void BtnActualizar_Click(object sender, EventArgs e)
    {
        ProdServ agr = new ProdServ();
        agr.codigo = txtClaveEDT.Text;
        agr.Descripcion = txtdesEDT.Text;
        agr.iva = txtivaEDT.Text;
        agr.eps = txtepsEDT.Text;
        agr.complemento = txtComplementoEDT2.Text;
        agr.editaMoneda();
        RadGrid1.DataBind();
        string script = "cierraModEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }
}