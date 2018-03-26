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

public partial class ServProd_F : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnEditar_Click(object sender, EventArgs e)
    {
        string script = "abreModEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
        SerProdF edt = new SerProdF();
        string codigo = Convert.ToString(RadGrid1.SelectedValues["IdConcepto"]);
        edt.codigo = codigo;
        edt.obtieneProdSerEdit();

        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];

            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtClaveProdEDT.Text = r1[0].ToString();
                txtClaveProdSatEDT.Text = r1[1].ToString();
                txtDesEDT.Text = r1[2].ToString();
                cmb_unidadEDT.SelectedValue = Convert.ToString(r1[4]);
                txtClaveunidadSatEDT.Text = r1[3].ToString();
                txtPreciounitaEDT.Text = r1[5].ToString();

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
        SerProdF edt = new SerProdF();
        string codigo = Convert.ToString(RadGrid1.SelectedValues["IdConcepto"]);
        edt.codigo = codigo;
        edt.eliminaProducto();
        RadGrid1.DataBind();
    }

    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        SerProdF agr = new SerProdF();
        agr.codigo = txtClaveProd.Text;
        agr.codigoSat = Convert.ToInt32( txtClaveProdSat.Text);
        agr.Descripcion = txtDes.Text;
        agr.unidad = Convert.ToInt32(cmb_unidad.SelectedValue);
        agr.claveunidad = txtClaveunidadSat.Text;
        agr.valunit = Convert.ToUInt32(txtPreciounita.Text);
        agr.agregarProductoF();
        RadGrid1.DataBind();
        string script = "cierraNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

    protected void BtnActualizar_Click(object sender, EventArgs e)
    {
        SerProdF agr = new SerProdF();
        agr.codigo = txtClaveProdEDT.Text;
        agr.codigoSat = Convert.ToInt32(txtClaveProdSatEDT.Text);
        agr.Descripcion = txtDesEDT.Text;
        agr.unidad = Convert.ToInt32(cmb_unidadEDT.SelectedValue);
        agr.claveunidad = txtClaveunidadSatEDT.Text;
        agr.valunit = Convert.ToUInt32(txtPreciounitaEDT.Text);
        agr.editaProducto();
        RadGrid1.DataBind();
        string script = "cierraModEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

}