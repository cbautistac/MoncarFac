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

public partial class CatFormaPago : System.Web.UI.Page
{

    CatalFormaPago datos = new CatalFormaPago();
    Ejecuciones ejecuta = new Ejecuciones();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void lnkAgregar_Click(object sender, EventArgs e)
    {

        lblTitulo.Text = "Agrega Forma de Pago";
        string script = "abreNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);

        txtDesc.Text = "";
        txtBancarizado.Text = "";
        txtNumOp.Text = "";
        txtRFCCO.Text = "";
        txtCtaOrd.Text = "";
        txtPtnCtaO.Text = "";
        txtRFCCtaBen.Text = "";
        txtCtaBen.Text = "";
        txtTipoCadena.Text = "";
        txtNomCtaExtra.Text = "";

    }
    protected void lnkEditar_Click(object sender, EventArgs e)
    {
        CatalFormaPago edt = new CatalFormaPago();
        string cd = Convert.ToString(RadGrid1.SelectedValues["ClaveFormaPago"]);
        edt.cd = cd;
        edt.cargardatos();

        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];
            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtDesc.Text = r1[1].ToString();
                txtBancarizado.Text = r1[2].ToString();
                txtNumOp.Text = r1[3].ToString();
                txtRFCCO.Text = r1[4].ToString();
                txtCtaOrd.Text = r1[5].ToString();
                txtPtnCtaO.Text = r1[6].ToString();
                txtRFCCtaBen.Text = r1[7].ToString();
                txtCtaBen.Text = r1[8].ToString();
                txtTipoCadena.Text = r1[9].ToString();
                txtNomCtaExtra.Text = r1[10].ToString();
            }
            lblTitulo.Text = "Edita Forma de Pago";
            string script = "abreNewEmi()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
        }
    }
    protected void lnkSeleccionar_Click(object sender, EventArgs e)
    {

    }
    protected void lnkBorrar_Click(object sender, EventArgs e)
    {
        CatalFormaPago dlt = new CatalFormaPago();
        string cd = Convert.ToString(RadGrid1.SelectedValues["id_Banco"]);
        dlt.cd = cd;
        dlt.elminarFormPag();
        RadGrid1.DataBind();
    }
    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        lnkEditar.Visible = true;
        lnkBorrar.Visible = true;
        lnkSeleccionar.Visible = true;
    }
    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        if (lblTitulo.Text == "Agrega Forma de Pago")
        {

            int idForPag =Convert.ToInt32( RadGrid1.SelectedValues["ClaveFormaPago"]);
            string descri = txtDesc.Text;
            string bancariza = txtBancarizado.Text;
            string NumOp = txtNumOp.Text;
            string rfcEmCtO = txtRFCCO.Text;
            string CtaOrd = txtCtaOrd.Text;
            string PtnCtaOr = txtPtnCtaO.Text;
            string rfcEmCtB = txtRFCCtaBen.Text;
            string CtaBen = txtCtaBen.Text;
            string TipCadPag = txtTipoCadena.Text;
            string NomCtaExtra = txtNomCtaExtra.Text;
            string PtnCtaBen = txtPtnCtaBen.Text;
            bool agregar = datos.agregaNuevaFormaPago(descri, PtnCtaBen, bancariza, NumOp, rfcEmCtO, CtaOrd, PtnCtaOr, rfcEmCtB, CtaBen, TipCadPag, NomCtaExtra);
            if (agregar)
            {
                txtDesc.Text = "";
                txtBancarizado.Text = "";
                txtNumOp.Text = "";
                txtRFCCO.Text = "";
                txtCtaOrd.Text = "";
                txtPtnCtaO.Text = "";
                txtRFCCtaBen.Text = "";
                txtCtaBen.Text = "";
                txtTipoCadena.Text = "";
                txtNomCtaExtra.Text = "";
                RadGrid1.DataBind();
            }
        }
        else
        {
            int idForPag = Convert.ToInt32(RadGrid1.SelectedValues["ClaveFormaPago"]);
            string descri = txtDesc.Text;
            string bancariza = txtBancarizado.Text;
            string NumOp = txtNumOp.Text;
            string rfcEmCtO = txtRFCCO.Text;
            string CtaOrd = txtCtaOrd.Text;
            string PtnCtaOr = txtPtnCtaO.Text;
            string rfcEmCtB = txtRFCCtaBen.Text;
            string CtaBen = txtCtaBen.Text;
            string PtnCtaBen = txtPtnCtaBen.Text;
            string TipCadPag = txtTipoCadena.Text;
            string NomCtaExtra = txtNomCtaExtra.Text;
            object[] existe = ejecuta.scalarToInt("select count(*)  from formapago_fsat where ClaveFormaPago=" + idForPag);
            if (Convert.ToInt32(existe[1]) == 1)
            {
                lblErrorNuevo.Text = "Esa Clave ya existe, intente con otro";
            }
            else
            {
                datos.editDatos(Convert.ToString(idForPag), descri, bancariza, NumOp, rfcEmCtO, CtaOrd, PtnCtaOr, rfcEmCtB, CtaBen,PtnCtaBen, TipCadPag, NomCtaExtra);
                if (Convert.ToBoolean(datos.retorno[1]))
                {
                    txtDesc.Text = "";
                    txtBancarizado.Text = "";
                    txtNumOp.Text = "";
                    txtRFCCO.Text = "";
                    txtCtaOrd.Text = "";
                    txtPtnCtaO.Text = "";
                    txtRFCCtaBen.Text = "";
                    txtCtaBen.Text = "";
                    txtTipoCadena.Text = "";
                    txtNomCtaExtra.Text = "";
                    RadGrid1.DataBind();
                }
            }
        }
    }
}