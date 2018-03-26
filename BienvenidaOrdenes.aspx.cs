using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class BienvenidaOrdenes : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            cargaDatosPie();
            lblTítulo.Text = "Seleccione una opción para mostrar información";
            lblTítulo.Visible = true;
        }
    }

    private void cargaDatosPie()
    {
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
        object[] datosOrden = recepciones.obtieneInfoOrdenPie(orden, empresa, taller);        
        if (Convert.ToBoolean(datosOrden[0]))
        {
            DataSet ordenDatos = (DataSet)datosOrden[1];
            foreach (DataRow filaOrd in ordenDatos.Tables[0].Rows)
            {
                ddlToOrden.Text = filaOrd[0].ToString();
                ddlClienteOrden.Text = filaOrd[1].ToString();
                ddlTsOrden.Text = filaOrd[2].ToString();
                ddlValOrden.Text = filaOrd[3].ToString();
                ddlTaOrden.Text = filaOrd[4].ToString();
                ddlLocOrden.Text = filaOrd[5].ToString();
                RadRadialGauge1.Pointer.Value = Convert.ToDecimal(filaOrd[6].ToString()) + Convert.ToDecimal("0.5");

                switch (Convert.ToInt32(filaOrd[6].ToString())) { 
                    case 1:
                        e1.CssClass = "pad1m textoBold colorBlanco t14";
                        break;
                    case 2:
                        e2.CssClass = "pad1m textoBold colorBlanco t14";
                        break;
                    case 3:
                        e3.CssClass = "pad1m textoBold colorBlanco t14";
                        break;
                    case 4:
                        e4.CssClass = "pad1m textoBold t14";
                        break;
                    case 5:
                        e5.CssClass = "pad1m textoBold t14";
                        break;
                    case 6:
                        e6.CssClass = "pad1m textoBold t14";
                        break;
                    case 7:
                        e7.CssClass = "pad1m textoBold t14";
                        break;
                    case 8:
                        e8.CssClass = "pad1m textoBold t14";
                        break;
                    default:
                        break;
                }

                //imgFase.ImageUrl = "~/img/taco" + filaOrd[6].ToString() + ".png";
                lblAvanceOrden.Text = "Avance Orden: " + filaOrd[8].ToString() + "%";                
                lblSiniestro.Text = filaOrd[9].ToString();
                lblDeducible.Text = Convert.ToDecimal(filaOrd[10].ToString()).ToString("C2");
                lblTotOrden.Text = Convert.ToDecimal(filaOrd[11].ToString()).ToString("C2");
                ddlPerfil.Text = filaOrd[13].ToString();
                lblPerfil.Text = "Perfil: " + filaOrd[13].ToString();
                try
                {
                    DateTime fechaEntrega = Convert.ToDateTime(filaOrd[14].ToString());
                    if (fechaEntrega.ToString("yyyy-MM-dd") == "1900-01-01")
                        lblEntregaEstimada.Text = "No establecida";
                    else
                        lblEntregaEstimada.Text = filaOrd[14].ToString();
                }
                catch (Exception) {
                    lblEntregaEstimada.Text = "No establecida";
                }
                
                lblPorcDedu.Text = filaOrd[16].ToString() + "%";
            }
        }
    }
    protected void lnkMano_Click(object sender, EventArgs e)
    {
        lblTítulo.Text = "Mano Obra";
        lblTítulo.Visible = true;
        pnlMano.Visible = true;
        pnlRefacciones.Visible = pnlLlamadas.Visible = pnlComentarios.Visible = pnlOperacion.Visible = false;
    }
    protected void lnkRef_Click(object sender, EventArgs e)
    {
        lblTítulo.Text = "Refacciones";
        lblTítulo.Visible = true;
        pnlRefacciones.Visible = true;
        pnlMano.Visible = pnlLlamadas.Visible = pnlComentarios.Visible = pnlOperacion.Visible = false;
    }
    protected void lnkComen_Click(object sender, EventArgs e)
    {
        lblTítulo.Text = "Comentarios";
        lblTítulo.Visible = true;
        pnlComentarios.Visible = true;
        pnlRefacciones.Visible = pnlLlamadas.Visible = pnlMano.Visible = pnlOperacion.Visible = false;
    }
    protected void lnkOperacion_Click(object sender, EventArgs e)
    {
        lblTítulo.Text = "Operación";
        lblTítulo.Visible = true;
        pnlOperacion.Visible = true;
        pnlRefacciones.Visible = pnlLlamadas.Visible = pnlComentarios.Visible = pnlMano.Visible = false;
    }
    protected void lnkLlamada_Click(object sender, EventArgs e)
    {
        lblTítulo.Text = "Llamadas";
        lblTítulo.Visible = true;
        pnlLlamadas.Visible = true;
        pnlRefacciones.Visible = pnlMano.Visible = pnlComentarios.Visible = pnlOperacion.Visible = false;
    }
}