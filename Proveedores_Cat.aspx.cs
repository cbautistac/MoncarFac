using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using Telerik.Web.UI;
using System.Data;

public partial class Proveedores_Cat : System.Web.UI.Page
{
    CatClientes datos = new CatClientes();
    Fechas fechas = new Fechas();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblProveedor.Text = "";
            limpiaCampos();
        }
    }
        
    private bool validaRfc(string rfc, string persona)
    {
        bool valido = false;
        bool seccion1 = true;
        bool seccion2 = true;
        DateTime fechaNacimieto = fechas.obtieneFechaLocal();
        string fecha;
        CatClientes clientes = new CatClientes();

        if (persona == "M")
        {
            for (int j = 0; j < 3; j++)
            {
                if (!char.IsLetter(rfc[j]))
                    seccion1 = false;
            }
            if (seccion1)
            {
                for (int j = 3; j < 9; j++)
                {
                    if (!char.IsDigit(rfc[j]))
                        seccion2 = false;
                }

                if (seccion2)
                {
                    //fecha = rfc.Substring(5, 2) + "/" + rfc.Substring(7, 2) + "/" + rfc.Substring(3, 2);
                    int añoRfc = Convert.ToInt32(rfc.Substring(3, 2));
                    int añoActual = Convert.ToInt32(fechas.obtieneFechaLocal().Year.ToString().Substring(2, 2));
                    if (añoRfc>0 && añoRfc<=añoActual)
                        fecha="20" + rfc.Substring(3, 2) + "-" + rfc.Substring(5, 2) + "-" + rfc.Substring(7, 2);
                    else
                        fecha="19" + rfc.Substring(3, 2) + "-" + rfc.Substring(5, 2) + "-" + rfc.Substring(7, 2);


                    //fecha = rfc.Substring(3, 2) + "-" + rfc.Substring(5, 2) + "-" + rfc.Substring(7, 2);
                    try { fechaNacimieto = Convert.ToDateTime(fecha); valido = true; }
                    catch (Exception x) { valido = false; }
                }
            }
            else
                valido = false;

            if (seccion1 && seccion2)
            {
                if (lblProveedor.Text == "")
                {
                    object[] rfcNoExiste = clientes.existeRFCcleinte(rfc, "P");
                    if (Convert.ToBoolean(rfcNoExiste[0]))
                    {
                        if (Convert.ToBoolean(rfcNoExiste[1]))
                            valido = false;
                        else
                            valido = true;
                    }
                }
                else {
                    object[] rfcNoExiste = clientes.existeRFCcleinteModifica(rfc, "P", Convert.ToInt32(lblProveedor.Text));
                    if (Convert.ToBoolean(rfcNoExiste[0]))
                    {
                        if (Convert.ToBoolean(rfcNoExiste[1]))
                            valido = false;
                        else
                            valido = true;
                    }
                }
            }
        }
        else if (persona == "F")
        {
            for (int j = 0; j < 4; j++)
            {
                if (!char.IsLetter(rfc[j]))
                    seccion1 = false;
            }
            if (seccion1)
            {
                for (int j = 4; j < 10; j++)
                {
                    if (!char.IsDigit(rfc[j]))
                        seccion2 = false;
                }

                if (seccion2)
                {
                    //fecha = rfc.Substring(6, 2) + "/" + rfc.Substring(8, 2) + "/" + rfc.Substring(4, 2);
                    int añoRfc = Convert.ToInt32(rfc.Substring(4, 2));
                    int añoActual = Convert.ToInt32(fechas.obtieneFechaLocal().Year.ToString().Substring(2, 2));
                    if (añoRfc > 0 && añoRfc <= añoActual)
                        fecha = "20" + rfc.Substring(4, 2) + "-" + rfc.Substring(6, 2) + "-" + rfc.Substring(8, 2);
                    else
                        fecha = "19" + rfc.Substring(4, 2) + "-" + rfc.Substring(6, 2) + "-" + rfc.Substring(8, 2);
                    try { fechaNacimieto = Convert.ToDateTime(fecha); valido = true; }
                    catch (Exception x) { valido = false; }
                }
            }
            else
                valido = false;

            if (seccion1 && seccion2)
            {
                if (lblProveedor.Text == "")
                {
                    object[] rfcNoExiste = clientes.existeRFCcleinte(rfc, "P");
                    if (Convert.ToBoolean(rfcNoExiste[0]))
                    {
                        if (Convert.ToBoolean(rfcNoExiste[1]))
                            valido = false;
                        else
                            valido = true;
                    }
                }
                else
                {
                    object[] rfcNoExiste = clientes.existeRFCcleinteModifica(rfc, "P", Convert.ToInt32(lblProveedor.Text));
                    if (Convert.ToBoolean(rfcNoExiste[0]))
                    {
                        if (Convert.ToBoolean(rfcNoExiste[1]))
                            valido = false;
                        else
                            valido = true;
                    }
                }
            }
        }
        else
            valido = false;
        //lblFecha.Text = fechaNacimieto.ToString();
        return valido;
    }

    

    protected void rdlPersonaMod_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlPersonaMod.SelectedValue == "F")
        {
            txtRFCMod.MaxLength = 13;
            txtApMod.Visible = true;
            txtAmMod.Visible = true;
            txtNombreMod.Visible = true;
            txtRazonMod.Visible = false;
            RequiredFieldValidator7.Enabled = false;
            RequiredFieldValidator8.Enabled = true;
            RequiredFieldValidator9.Enabled = true;
            //RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{4}[0-9]{6}[0-9A-Za-z]{3}$";
        }
        else
        {
            txtRFCMod.MaxLength = 12;
            txtApMod.Visible = false;
            txtAmMod.Visible = false;
            txtNombreMod.Visible = false;
            txtRazonMod.Visible = true;
            RequiredFieldValidator7.Enabled = true;
            RequiredFieldValidator8.Enabled = false;
            RequiredFieldValidator9.Enabled = false;
            //RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{3}[0-9]{6}[0-9A-Za-z]{3}$";
        }
    }
       

    private void limpiaCampos()
    {
        lblProveedor.Text = "";
        rdlPersonaMod.SelectedValue = "F";
        if (rdlPersonaMod.SelectedValue == "F") {
            txtRFCMod.MaxLength = 13;
            txtApMod.Visible = true;
            txtAmMod.Visible = true;
            txtNombreMod.Visible = true;
            txtRazonMod.Visible = false;
            RequiredFieldValidator7.Enabled = false;
            RequiredFieldValidator8.Enabled = true;
            RequiredFieldValidator9.Enabled = true;
            //RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{4}[0-9]{6}[0-9A-Za-z]{3}$";
        }
        else {
            txtRFCMod.MaxLength = 12;
            txtApMod.Visible = false;
            txtAmMod.Visible = false;
            txtNombreMod.Visible = false;
            txtRazonMod.Visible = true;
            RequiredFieldValidator7.Enabled = true;
            RequiredFieldValidator8.Enabled = false;
            RequiredFieldValidator9.Enabled = false;
            //RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{3}[0-9]{6}[0-9A-Za-z]{3}$";
        }
        rdlSexoMod.SelectedValue = "M";
        txtRFCMod.Text = txtfechaMod.Text = txtRazonMod.Text = txtNombreMod.Text = txtApMod.Text = txtAmMod.Text = txtCalleMod.Text = txtNoExtMod.Text = txtNoIntMod.Text = txtColoniaMod.Text = txtCpMod.Text = txtMunicipMod.Text = txtEstadoMod.Text = txtPaisMod.Text = txtTel1Mod.Text = txtTel2Mod.Text = txtTel3Mod.Text = txtCorreoMod.Text = txtDescMod.Text = "";
        ddlPoliticaMod.Items.Clear();
        ddlPoliticaMod.DataBind();
        ddlZona.Items.Clear();
        ddlZona.DataBind();
        ddlRevisionMod.SelectedValue = ddlCobranzaMod.SelectedValue = ddlPoliticaMod.SelectedValue = ddlZona.SelectedValue = "0";
        chbAseguradoraMod.Checked = false;
    }

   
    private void cargaInfoProveedor()
    {
        DataSet valores = new CatClientes().llenaDetalle(lblProveedor.Text, "P");
        foreach (DataRow r in valores.Tables[0].Rows) {
            rdlPersonaMod.SelectedValue = Convert.ToString(r[2]);
            txtRFCMod.Text = Convert.ToString(r[3]);
            rdlSexoMod.SelectedValue = Convert.ToString(r[4]);
            DateTime fecha = DateTime.Now;
            try { fecha = Convert.ToDateTime(r[5]); } catch (Exception) { fecha = DateTime.Now; }
            txtfechaMod.Text = fecha.ToString("yyyy-MM-dd");            
            txtRazonMod.Text= Convert.ToString(r[7]);
            txtNombreMod.Text= Convert.ToString(r[8]);
            txtApMod.Text= Convert.ToString(r[9]);
            txtAmMod.Text= Convert.ToString(r[10]);
            txtCalleMod.Text= Convert.ToString(r[11]);
            txtNoExtMod.Text = Convert.ToString(r[12]);
            txtNoIntMod.Text= Convert.ToString(r[13]);
            txtColoniaMod.Text= Convert.ToString(r[14]);
            txtCpMod.Text= Convert.ToString(r[15]);
            //txtCpMod.Value = Convert.ToDouble(Convert.ToString(r[15]).Trim());
            txtMunicipMod.Text= Convert.ToString(r[16]);
            txtEstadoMod.Text= Convert.ToString(r[17]);
            txtPaisMod.Text= Convert.ToString(r[18]);
            try { txtTel1Mod.Text = Convert.ToString(r[19]); }
            catch (Exception) { txtTel1Mod.Text =  null ; }
            //txtTel1Mod.Value = Convert.ToDouble(Convert.ToString(r[19]).Trim());
            txtTel2Mod.Text= Convert.ToString(r[20]);
            //txtTel2Mod.Value = Convert.ToDouble(Convert.ToString(r[20]).Trim());
            txtTel3Mod.Text= Convert.ToString(r[21]);
            //txtTel3Mod.Value = Convert.ToDouble(Convert.ToString(r[21]).Trim());
            ddlRevisionMod.SelectedValue = Convert.ToString(r[22]);
            ddlCobranzaMod.SelectedValue= Convert.ToString(r[23]);
            ddlPoliticaMod.SelectedValue= Convert.ToString(r[24]);
            chbAseguradoraMod.Checked = Convert.ToBoolean(r[25]);
            txtDescMod.Text = Convert.ToString(r[26]);
            //txtDescMod.Value = Convert.ToDouble(Convert.ToString(r[26]).Trim());
            ddlZona.SelectedValue = Convert.ToString(r[28]);
            txtCorreoMod.Text = Convert.ToString(r[29]);
            txtNoProveedor.Text = Convert.ToString(r[30]);
            break;
        }
        if (rdlPersonaMod.SelectedValue == "F")
        {
            txtRFCMod.MaxLength = 13;
            txtApMod.Visible = true;
            txtAmMod.Visible = true;
            txtNombreMod.Visible = true;
            txtRazonMod.Visible = false;
            RequiredFieldValidator7.Enabled = false;
            RequiredFieldValidator8.Enabled = true;
            RequiredFieldValidator9.Enabled = true;
            //RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{4}[0-9]{6}[0-9A-Za-z]{3}$";
        }
        else
        {
            txtRFCMod.MaxLength = 12;
            txtApMod.Visible = false;
            txtAmMod.Visible = false;
            txtNombreMod.Visible = false;
            txtRazonMod.Visible = true;
            RequiredFieldValidator7.Enabled = true;
            RequiredFieldValidator8.Enabled = false;
            RequiredFieldValidator9.Enabled = false;
            //RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{3}[0-9]{6}[0-9A-Za-z]{3}$";
        }
    }

    protected void lnkAgregar_Click(object sender, EventArgs e)
    {
        string script = "abreWinCtrl()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType ==  GridItemType.Item || e.Item.ItemType==GridItemType.AlternatingItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;
            var btnBtnEliminar = e.Item.FindControl("lnkEliminar") as LinkButton;            
            try
            {
                var btnAlta = e.Item.FindControl("lnkActivar") as LinkButton;
                var btnBaja = e.Item.FindControl("lnkInactiva") as LinkButton;
                int id = Convert.ToInt32(r[0]);
                string estatus = datos.obtieneEstatusCliprov(id, "P");
                if (estatus == "B")
                {
                    btnBaja.Visible = false;
                    e.Item.CssClass = "alert-danger";
                }
                else
                    btnAlta.Visible = false;

                CatClientes catClient = new CatClientes();
                object[] valores = catClient.tieneRelacionProv(id, "P");
                if (Convert.ToBoolean(valores[0]))
                {
                    if (Convert.ToBoolean(valores[1]))
                        btnBtnEliminar.Visible = false;
                    else
                        btnBtnEliminar.Visible = true;
                }
                else
                    btnBtnEliminar.Visible = false;
                
            }
            catch (Exception x)
            {
                Session["errores"] = x.Message;
                Session["paginaOrigen"] = "Proveedores_Cat.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }

    protected void lnkActivar_Click(object sender, EventArgs e)
    {
        lblErrorGral.Text = "";
        LinkButton btn = (LinkButton)sender;
        int idCliprov = Convert.ToInt32(btn.CommandArgument);
        bool baja = datos.altaBajaCliprov(idCliprov, "A", "P");
        if (baja)
            RadGrid1.DataBind();
        else
        {
            RadGrid1.DataBind();
            lblErrorGral.Text = "El Proveedor no se logro reactivar, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }

    protected void lnkInactiva_Click(object sender, EventArgs e)
    {
        lblErrorGral.Text = "";
        LinkButton btn = (LinkButton)sender;
        int idCliprov = Convert.ToInt32(btn.CommandArgument);
        bool baja = datos.altaBajaCliprov(idCliprov, "B", "P");
        if (baja)
            RadGrid1.DataBind();
        else
        {
            RadGrid1.DataBind();
            lblErrorGral.Text = "El Proveedor no se logro inactivar, verifique su conexión e intentelo nuevamente mas tarde";
        }
    }

    protected void lnkCancelar_Click(object sender, EventArgs e)
    {
        limpiaCampos();
        string script = "cierraWinCtrl()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
    }

    protected void lnkEliminar_Click(object sender, EventArgs e)
    {
        lblErrorGral.Text = "";
        LinkButton btn = (LinkButton)sender;
        int id = Convert.ToInt32(btn.CommandArgument);
        CatClientes catClient = new CatClientes();
        object[] valores = catClient.tieneRelacion(id, "P");
        if (Convert.ToBoolean(valores[0])) {
            bool relacionado = Convert.ToBoolean(valores[1]);
            if (!relacionado)
            {
                object[] actualizado = catClient.eliminaClienteProveedor(id, "P");
                if (Convert.ToBoolean(actualizado[0]))
                    RadGrid1.DataBind();
                else
                    lblErrorGral.Text = "Error al eliminar el proveedor. Detalle: " + Convert.ToString(actualizado[1]); ;
            }
            else
                lblErrorGral.Text = "El proveedor no puede ser eliminado ya que se encuentra relacionado a una Cotización, una Orden de Compra, una Refacción o una Factura";
        }else
            lblErrorGral.Text = "Error al eliminar el proveedor. Detalle: "+Convert.ToString(valores[1]); ;
    }

    protected void lnkAceptar_Click(object sender, EventArgs e)
    {        
        lblError.Text = "";
        try
        {
            string persona = rdlPersonaMod.SelectedValue.ToString();
            string rfc = txtRFCMod.Text.ToUpper();
            string razon = txtRazonMod.Text;
            string nombre = txtNombreMod.Text;
            string ap = txtApMod.Text;
            string am = txtAmMod.Text;
            string correo = txtCorreoMod.Text;
            bool aseguradora = chbAseguradoraMod.Checked;
            string fecha;
            DateTime fechaNacimiento = DateTime.Now;
            int edad = 0;
            bool valido = false;
            if (persona == "M")
            {
                if (validaRfc(rfc, persona))
                {
                    //fecha = rfc.Substring(7, 2) + "/" + rfc.Substring(5, 2) + "/" + rfc.Substring(3, 2);
                    int añoRfc = Convert.ToInt32(rfc.Substring(3, 2));
                    int añoActual = Convert.ToInt32(fechas.obtieneFechaLocal().Year.ToString().Substring(2, 2));
                    if (añoRfc > 0 && añoRfc <= añoActual)
                        fecha = "20" + rfc.Substring(3, 2) + "-" + rfc.Substring(5, 2) + "-" + rfc.Substring(7, 2);
                    else
                        fecha = "19" + rfc.Substring(3, 2) + "-" + rfc.Substring(5, 2) + "-" + rfc.Substring(7, 2);
                    fechaNacimiento = Convert.ToDateTime(fecha);
                    edad = calculaEdad(fechaNacimiento);
                    valido = true;                    
                }
                else
                    lblError.Text = "El RFC no tiene el formato correcto o ya se encuentra dado de alta, verifique";

            }
            else if (persona == "F")
            {
                if (validaRfc(rfc, persona))
                {
                    int añoRfc = Convert.ToInt32(rfc.Substring(4, 2));
                    int añoActual = Convert.ToInt32(fechas.obtieneFechaLocal().Year.ToString().Substring(2, 2));
                    if (añoRfc > 0 && añoRfc <= añoActual)
                        fecha = "20" + rfc.Substring(4, 2) + "-" + rfc.Substring(6, 2) + "-" + rfc.Substring(8, 2);
                    else
                        fecha = "19" + rfc.Substring(4, 2) + "-" + rfc.Substring(6, 2) + "-" + rfc.Substring(8, 2);
                    fechaNacimiento = Convert.ToDateTime(fecha);
                    razon = nombre.Trim() + " " + ap.Trim() + " " + am.Trim();
                    edad = calculaEdad(fechaNacimiento);
                    valido = true;                    
                }
                else
                    lblError.Text = "El RFC no tiene el formato correcto o ya se encuentra dado de alta, verifique";
            }

            if (valido) {
                CatClientes catClient = new CatClientes();
                int idPorveedor = 0;
                try { idPorveedor = Convert.ToInt32(lblProveedor.Text); } catch (Exception) { idPorveedor = 0; }
                object[] actualizado = catClient.insertaActualizaCliente(idPorveedor, "P", rdlPersonaMod.SelectedValue, rfc, rdlSexoMod.SelectedValue, fechaNacimiento.ToString("yyyy-MM-dd"), edad, razon, nombre, ap, am, txtCalleMod.Text, txtNoExtMod.Text, txtNoIntMod.Text, txtColoniaMod.Text, txtCpMod.Text, txtMunicipMod.Text, txtEstadoMod.Text, txtPaisMod.Text, txtTel1Mod.Text, txtTel2Mod.Text, txtTel3Mod.Text, Convert.ToInt32(ddlRevisionMod.SelectedValue), Convert.ToInt32(ddlCobranzaMod.SelectedValue), Convert.ToInt32(ddlPoliticaMod.SelectedValue), chbAseguradoraMod.Checked, Convert.ToDecimal(txtDescMod.Value), null, txtCorreoMod.Text, 0, 0, 0, null, "", "", Convert.ToInt32(ddlZona.SelectedValue), lblProveedor.Text, txtNoProveedor.Text);
                if (Convert.ToBoolean(actualizado[0]))
                {
                    RadGrid1.DataBind();
                    lblError.Text = "";
                    limpiaCampos();
                    string script = "cierraWinCtrl()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
                }
                else
                    lblError.Text = "No se pudo guardar o actualizar información. Detalle: " + Convert.ToString(actualizado[1]);
            }
        }
        catch (Exception x)
        {
            Session["errores"] = x.Message;
            Session["paginaOrigen"] = "Proveedores_Cat.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    private int calculaEdad(DateTime fechaNacimiento)
    {
        int edadEnDias = 0;
        if (fechaNacimiento.ToString("yyyy-MM-dd") == "1900-01-01" || fechaNacimiento.ToString("yyyy-MM-dd") == "1901-01-01")
            edadEnDias = 0;
        else
        {
            try
            {
                DateTime nacimiento = new DateTime(fechaNacimiento.Year, fechaNacimiento.Month, fechaNacimiento.Day);
                int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
                edadEnDias = edad;
            }
            catch (Exception) { edadEnDias = 0; }
        }
        return edadEnDias;
    }

    protected void lnkEditar_Click(object sender, EventArgs e)
    {
        lblErrorGral.Text = "";
        lblError.Text = "";
        LinkButton btn = (LinkButton)sender;
        int idCliprov = Convert.ToInt32(btn.CommandArgument);
        lblProveedor.Text = idCliprov.ToString();
        cargaInfoProveedor();
        string script = "abreWinCtrl()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
    }
}