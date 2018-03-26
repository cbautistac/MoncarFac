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

public partial class catImpuestos : System.Web.UI.Page
{
    Ejecuciones busca = new Ejecuciones();

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
        
        }
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        ImpuestosRT edt = new ImpuestosRT();
        int id = Convert.ToInt32(RadGrid1.SelectedValues["ID_Ret"]);
        edt.idRedEdita = id;
        edt.obtieneRetencion();
        if (Convert.ToBoolean(edt.retorno[0]))
        {
            DataSet ds1 = (DataSet)edt.retorno[1];

            foreach (DataRow r1 in ds1.Tables[0].Rows)
            {
                txtClave.Text = Convert.ToInt32(r1[0]).ToString();
                txtNombre.Text = r1[1].ToString();
                txtDescripcion.Text = r1[2].ToString();
                txtTasa.Text = r1[3].ToString();
                txtCuenta.Text = r1[4].ToString();
            }
            UnidadImpuestoR.Title = "Edita Impuesto de Retencion";
            UnidadImpuestoR.VisibleOnPageLoad = true;
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
        ImpuestosRT eli = new ImpuestosRT();
        int id = Convert.ToInt32(RadGrid1.SelectedValues["ID_Ret"]);
        eli.idRedEdita = id;
        eli.eliminaImpuestoR();
        if (Convert.ToBoolean(eli.retorno[1]))
        {
            RadGrid1.DataBind();
        }
        else
            lblError.Text = "Error al eliminar el impuesto";
    }

    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        
        ImpuestosRT agr = new ImpuestosRT();  
        agr.Nombre = txtNombre.Text;
        agr.Descripcion = txtDescripcion.Text;
        agr.tasa = Convert.ToDecimal(txtTasa.Text);
        agr.Cuenta = txtCuenta.Text;
        if (UnidadImpuestoR.Title == "Nuevo Impuesto de Retencion")
        {
            agr.agregarImpuestoR();
            if (Convert.ToBoolean(agr.retorno[1]))
            {
                RadGrid1.DataBind();
                UnidadImpuestoR.VisibleOnPageLoad = false;
            }
            else
                lblError.Text = "Error al agregar el impuesto de traslado";
        }
        else
        {
            agr.idRedEdita = Convert.ToInt32(txtClave.Text);
            agr.editaImpuestoR();
            if(Convert.ToBoolean(agr.retorno[1]))
            {
                RadGrid1.DataBind();
                UnidadImpuestoR.VisibleOnPageLoad = false;
            }
        }
    }
    private void borraCamposRetenido()
    {
        txtNombre.Text = txtClave.Text = txtDescripcion.Text = txtTasa.Text = txtCuenta.Text = "";
    }

    



    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        borraCamposRetenido();
        object[] id = busca.scalarToInt("(select isnull((select top 1 id_ret from ImpRetenidos_f order by ID_Ret desc),0)+1)");
        txtClave.Text = id[1].ToString();
        UnidadImpuestoR.Title = "Nuevo Impuesto de Retencion";
        UnidadImpuestoR.VisibleOnPageLoad = true;
    }

    protected void btnEditar_Click2(object sender, EventArgs e)
    {
        

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
       // agr.codigo = txtUnidadSat.Text;
      //  agr.Nombre = txtNombre.Text;
     //   agr.Descripcion = txtDesSat.Text;
     //   DateTime fecha_in = Convert.ToDateTime(txtFecha_ini.SelectedDate);
     //   agr.fechaini = fecha_in.ToString("yyyy/MM/dd");
    //    DateTime Fecha_fin = Convert.ToDateTime(txtFecha_fin.SelectedDate);
    //    agr.fechafin = Fecha_fin.ToString("yyyy/MM/dd");
    //    agr.simbolo = txtSimbolo.Text;
        agr.agregarUnidadSat();
        RadGrid2.DataBind();
        UnidadSAT.VisibleOnPageLoad = false;
    }

    protected void AgregarSat_Click(object sender, EventArgs e)
    {
        UnidadSAT.VisibleOnPageLoad = true;
    }
}