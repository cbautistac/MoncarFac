using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FacturasExternas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            if (radTipo.SelectedValue == "PA")
                lblTipoProveedor.Text = "P";
            else
                lblTipoProveedor.Text = "C";
            limpiaCampos();
            
        }
    }

    private void limpiaCampos()
    {
        txtFactura.Text = "";
        txtOrden.Text = "";
        txtMonto.Text = "";
        ddlProveedor.Items.Clear();
        ddlProveedor.Text = "";
        ddlProveedor.DataBind();
        //ddlTaller.Items.Clear();
        //ddlTaller.DataBind();
    }

    protected void lnkAgregarFactura_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            object[] result = new object[] { false, "" };
            Refacciones refacciones = new Refacciones();
            Facturas facturas = new Facturas();
            facturas.folio = Convert.ToInt32(txtOrden.Text);
            facturas.tipoCuenta = radTipo.SelectedValue;
            facturas.factura = txtFactura.Text;
            CatClientes clientes = new CatClientes();
            string politica = clientes.obtieneClavePoliticaCliente(ddlProveedor.SelectedValue);
            int diasPlazo = clientes.obtieneDiasPolitica(ddlProveedor.SelectedValue);
            facturas.fechaRevision = new E_Utilities.Fechas().obtieneFechaLocal();
            facturas.fechaProgPago = new E_Utilities.Fechas().obtieneFechaLocal().AddDays(Convert.ToDouble(diasPlazo));
            facturas.id_cliprov = Convert.ToInt32(ddlProveedor.SelectedValue);
            facturas.formaPago = "E";
            facturas.politica = politica;
            facturas.estatus = "PEN";
            facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
            facturas.taller = Convert.ToInt32(ddlTaller.SelectedValue);
            facturas.tipoCargo = "I";
            facturas.Importe = Convert.ToDecimal(txtMonto.Text);
            facturas.orden = Convert.ToInt32(txtOrden.Text);
            FacturacionElectronica.Receptores recp = new FacturacionElectronica.Receptores();
            recp.idReceptor = Convert.ToInt32(ddlProveedor.SelectedValue);
            recp.obtieneInfoReceptor();
            int idrecep = 0;
            string rfc = "";
            object[] retorno = recp.info;
            try
            {
                DataSet infoRec = (DataSet)retorno[1];
                foreach (DataRow i in infoRec.Tables[0].Rows)
                {
                    facturas.razon_social = Convert.ToString(i[2]);
                    idrecep = Convert.ToInt32(i[0]);
                    rfc = Convert.ToString(i[1]);
                    break;
                }
            }
            catch (Exception ex) { facturas.razon_social = ""; }
            if (facturas.razon_social == "")
                facturas.razon_social = ddlProveedor.Text.ToUpper();

            //facturas.idCfd = Convert.ToInt32(result[1].ToString());            
            if (facturas.tipoCuenta == "CC") {
                facturas.rfc = rfc.ToUpper().Trim();
                facturas.idRecep = idrecep;                
                facturas.creaEmccfd();
            }
            
            facturas.generaFacturaCC();

            object[] facturasInternas = facturas.retorno;
            if (Convert.ToBoolean(facturasInternas[0]))
            {
                lblError.Text = "Factura agregada existosamente";
                radTipo.SelectedValue = "PA";
                if (radTipo.SelectedValue == "PA")
                    lblTipoProveedor.Text = "P";
                else
                    lblTipoProveedor.Text = "C";
                limpiaCampos();
            }
            else
                lblError.Text = "No se pudo agregar la factura externa. Detalle: " + Convert.ToString(facturasInternas[1]);

        }
        catch (Exception ex) { lblError.Text = "No se pudo agregar la factura externa. Detalle: " + ex.Message; }
    }


    protected void radTipo_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if(radTipo.SelectedValue=="PA")
            lblTipoProveedor.Text = "P";
        else
            lblTipoProveedor.Text = "C";
        ddlProveedor.Items.Clear();
        ddlProveedor.Text = "";
        ddlProveedor.DataBind();
    }
}