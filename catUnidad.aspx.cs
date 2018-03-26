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

public partial class catUnidad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        modalModifica.VisibleOnPageLoad = true;
        CatUni edt = new CatUni();
        string codigo = Convert.ToString(RadGrid1.SelectedValues["idunid"]);
        edt.codigo = codigo;
        edt.obtieneunidadEdit();

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
        CatUni edt = new CatUni();
        string codigo = Convert.ToString(RadGrid1.SelectedValues["idunid"]);
        edt.codigo = codigo;
        edt.eliminaUnidad();
        RadGrid1.DataBind();
    }

    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        CatUni agr = new CatUni();
        agr.codigo = txtClaveMon.Text;
        agr.Descripcion = txtDes.Text;
        agr.agregarUnidad();
        RadGrid1.DataBind();
        UnidadEmisor.VisibleOnPageLoad = false;
    }

    protected void BtnActualizar_Click(object sender, EventArgs e)
    {
        CatUni agr = new CatUni();
        agr.codigo = txtClaveMonEdt.Text;
        agr.Descripcion = txtDesEdt.Text;

        agr.editaUnidad();
        RadGrid1.DataBind();
        modalModifica.VisibleOnPageLoad = false;
    }



    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        UnidadEmisor.VisibleOnPageLoad = true;
    }

    protected void btnEditar_Click2(object sender, EventArgs e)
    {
        UnidadSATMOD.VisibleOnPageLoad = true;
        CatUni edt = new CatUni();
        string codigo = Convert.ToString(RadGrid2.SelectedValues["ClaveUnidad"]);
        edt.codigo = codigo;
        edt.obtieneunidadEditSat();

        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];

            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                TextBox1.Text = r1[0].ToString();
                txtNombreEDT.Text = r1[1].ToString();
                txtDescEDT.Text = r1[2].ToString();
                txtFechainiEDT.SelectedDate = Convert.ToDateTime(r1[3]);
                txtFechafinEDT.SelectedDate = Convert.ToDateTime(r1[4]);
                txtSimboloEDT.Text = r1[5].ToString();



            }
        }

    }
    protected void btnEliminar_Click2(object sender, EventArgs e)
    {
        CatUni edt = new CatUni();
        string codigo = Convert.ToString(RadGrid2.SelectedValues["ClaveUnidad"]);
        edt.codigo = codigo;
        edt.eliminaUnidadSat();
        RadGrid2.DataBind();
    }

   

    protected void BtnActualizar_Click2(object sender, EventArgs e)
    {
        CatUni agr = new CatUni();
        agr.codigo = TextBox1.Text;
        agr.Nombre = txtNombreEDT.Text;
        agr.Descripcion = txtDescEDT.Text;
        DateTime fecha_in = Convert.ToDateTime(txtFechainiEDT.SelectedDate);
        agr.fechaini = fecha_in.ToString("yyyy/MM/dd");
        DateTime Fecha_fin = Convert.ToDateTime(txtFechafinEDT.SelectedDate);
        agr.fechafin = Fecha_fin.ToString("yyyy/MM/dd");
        agr.simbolo = txtSimboloEDT.Text;
        agr.editaUnidadSat();
        RadGrid2.DataBind();
        UnidadSATMOD.VisibleOnPageLoad = false;
    }


    protected void RadGrid2_SelectedIndexChanged(object sender, EventArgs e)
    {
        AgregarSat.Visible = true;
        LinkButton7.Visible = true;
        LinkButton8.Visible = true;
        LinkButton9.Visible = true;
    }

    protected void lnkAgregarNuevoSAT_Click(object sender, EventArgs e)
    {
        CatUni agr = new CatUni();
        agr.codigo = txtUnidadSat.Text;
        agr.Nombre = txtNombre.Text;
        agr.Descripcion = txtDesSat.Text;
        DateTime fecha_in = Convert.ToDateTime(txtFecha_ini.SelectedDate);
        agr.fechaini = fecha_in.ToString("yyyy/MM/dd");
        DateTime Fecha_fin = Convert.ToDateTime(txtFecha_fin.SelectedDate);
        agr.fechafin = Fecha_fin.ToString("yyyy/MM/dd");
        agr.simbolo = txtSimbolo.Text;
        agr.agregarUnidadSat();
        RadGrid2.DataBind();
        UnidadSAT.VisibleOnPageLoad = false;
    }

    protected void AgregarSat_Click(object sender, EventArgs e)
    {
        UnidadSAT.VisibleOnPageLoad = true;
    }
}