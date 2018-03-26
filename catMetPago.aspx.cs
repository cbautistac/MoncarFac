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

public partial class catMetPago : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        string script = "abreModEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
        MetPag edt = new MetPag();
        string codigo = Convert.ToString(RadGrid1.SelectedValues["Clave"]);
        edt.codigo = codigo;
        edt.obtieneMetodoEdit();

        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];

            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtClaveMonEdt.Text = r1[0].ToString();
                txtDesEdt.Text = r1[1].ToString();

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
        MetPag edt = new MetPag();
        string codigo = Convert.ToString(RadGrid1.SelectedValues["Clave"]);
        edt.codigo = codigo;
        edt.eliminaMetodo();
        RadGrid1.DataBind();
    }

    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        MetPag agr = new MetPag();
        agr.codigo = txtClaveMon.Text;
        agr.Descripcion = txtDes.Text;
        agr.agregarMetodo();
        RadGrid1.DataBind();
        string script = "cierraNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

    protected void BtnActualizar_Click(object sender, EventArgs e)
    {
        MetPag agr = new MetPag();
        agr.codigo = txtClaveMonEdt.Text;
        agr.Descripcion = txtDesEdt.Text;
        agr.editaMetodo();
        RadGrid1.DataBind();
        string script = "cierraModEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

}