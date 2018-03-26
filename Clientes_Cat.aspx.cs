using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;
using System.Drawing;
using System.IO;
using Telerik.Web.UI;

public partial class Clientes_Cat : System.Web.UI.Page
{

    CatClientes datos = new CatClientes();
    Fechas fechas = new Fechas();

    protected void Page_Load(object sender, EventArgs e)
    {

    } 
    protected void lnkAgregar_Click(object sender, EventArgs e)
    {
        bool insertado = false;
        lblError.Text = ""; 
        try
        {
            string persona = rbtnPersona.SelectedValue.ToString();
            string rfc = txtRfc.Text.ToUpper();
            string razon = txtRazonNew.Text.ToUpper();
            string nombre = txtNombre.Text.ToUpper();
            string ap = txtApPat.Text.ToUpper();
            string am = txtApMat.Text.ToUpper();
            string correo = txtEmail.Text.ToLower();
            bool aseguradora = chkAseguradora.Checked;
            string fecha;
            DateTime fechaNacimiento;
            if (persona == "M")
            { 
                if (validaRfc(rfc, persona))
                {
                    //fecha = rfc.Substring(7, 2) + "/" + rfc.Substring(5, 2) + "/" + rfc.Substring(3, 2);
                    //fechaNacimiento = Convert.ToDateTime(fecha);
                    /*
                    try
                    {
                        string numeros = rfc.Substring(3, 6); // aqui es par aobtener el el fomato del rfc de las fechas yymmdd
                        int año = Convert.ToInt32(numeros.Substring(0, 2));
                        int mes = Convert.ToInt32(numeros.Substring(2, 2));
                        int dia = Convert.ToInt32(numeros.Substring(4, 2));

                        fechaNacimiento = Convert.ToDateTime(dia.ToString().PadLeft(2, '0') + "/" + mes.ToString().PadLeft(2, '0') + "/" + año.ToString());
                        
                    }
                    catch (Exception) { fechaNacimiento = new E_Utilities.Fechas().obtieneFechaLocal(); }
                    */
                    rfc = " " + rfc;
                    fecha = rfc.Substring(8, 2) + "/" + rfc.Substring(6, 2) + "/" + rfc.Substring(4, 2);
                    try { fechaNacimiento = Convert.ToDateTime(fecha); }
                    catch (Exception) { fechaNacimiento = new E_Utilities.Fechas().obtieneFechaLocal();  }
                    insertado = datos.insertaClienteBasicos(rfc, razon, nombre, ap, am, aseguradora, persona, fechaNacimiento.ToString("yyyy-MM-dd"), "C",correo);
                    if (insertado)
                    {
                        txtApMat.Text = txtApPat.Text = txtNombre.Text = txtRazonNew.Text = txtRfc.Text = txtEmail.Text = "";
                        rbtnPersona.SelectedValue = "F";
                        txtRfc.MaxLength = 13;
                        txtApMat.Visible = true;
                        txtApPat.Visible = true;
                        txtNombre.Visible = true;
                        txtRazonNew.Visible = false;
                        RequiredFieldValidator2.Enabled = false;
                        RequiredFieldValidator3.Enabled = true;
                        RequiredFieldValidator4.Enabled = true;
                        chkAseguradora.Checked = false;
                        GridClientes.DataBind();
                    }
                    else
                        lblError.Text = "No se agrego al cliente, intente más tarde.";

                }
                else
                    lblError.Text = "El RFC no tiene el formato correcto, verifique";

            }
            else if (persona == "F")
            {
                if (validaRfc(rfc, persona))
                {
                    fecha = rfc.Substring(8, 2) + "/" + rfc.Substring(6, 2) + "/" + rfc.Substring(4, 2);
                    try { fechaNacimiento = Convert.ToDateTime(fecha); }
                    catch (Exception) { fechaNacimiento = new E_Utilities.Fechas().obtieneFechaLocal(); }
                    razon = nombre.Trim() + " " + ap.Trim() + " " + am.Trim();
                    insertado = datos.insertaClienteBasicos(rfc, razon, nombre, ap, am, aseguradora, persona, fechaNacimiento.ToString("yyyy-MM-dd"), "C",correo);
                    if (insertado)
                    {
                        txtApMat.Text = txtApPat.Text = txtNombre.Text = txtRazonNew.Text = txtRfc.Text = "";
                        rbtnPersona.SelectedValue = "F";
                        txtRfc.MaxLength = 13;
                        txtApMat.Visible = true;
                        txtApPat.Visible = true;
                        txtNombre.Visible = true;
                        txtRazonNew.Visible = false;
                        RequiredFieldValidator2.Enabled = false;
                        RequiredFieldValidator3.Enabled = true;
                        RequiredFieldValidator4.Enabled = true;
                        chkAseguradora.Checked = false;
                        GridClientes.DataBind();
                    }
                    else
                        lblError.Text = "No se agrego al cliente, intente más tarde.";
                }
                else
                    lblError.Text = "El RFC no tiene el formato correcto, verifique";
            }

        }
        catch (Exception x)
        {
            Session["errores"] = x.Message;
            Session["paginaOrigen"] = "Clientes_Cat.aspx";
            Response.Redirect("AppErrorLog.aspx");
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
        string rfcsinmod = rfc;
        if (persona == "M")
        {
            rfc = "A" + rfc;
            /*
            try
            {
                string numeros = rfc.Substring(3, 6); // aqui es par aobtener el el fomato del rfc de las fechas yymmdd
                int año = Convert.ToInt32(numeros.Substring(0, 2));
                int mes = Convert.ToInt32(numeros.Substring(2, 2));
                int dia = Convert.ToInt32(numeros.Substring(4, 2));

                fechaNacimieto = Convert.ToDateTime(dia.ToString().PadLeft(2, '0') + "/" + mes.ToString().PadLeft(2, '0') + "/" + año.ToString());
                valido = true;
            } catch (Exception) { valido = false; }
            */

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
                    fecha = rfc.Substring(8, 2) + "/" + rfc.Substring(6, 2) + "/" + rfc.Substring(4, 2);
                    try { fechaNacimieto = Convert.ToDateTime(fecha); valido = true; }
                    catch (Exception x) { valido = false; }
                }
            }
            else
                valido = false;
            
            if (seccion1 && seccion2)
            {
                
                object[] rfcNoExiste = clientes.existeRFCcleinte(rfcsinmod, "C");
                if (Convert.ToBoolean(rfcNoExiste[0]))
                {
                  
                    if (Convert.ToBoolean(rfcNoExiste[1]))
                        valido = false;
                    else
                        valido = true;
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
                    fecha = rfc.Substring(8, 2) + "/" + rfc.Substring(6, 2) + "/" + rfc.Substring(4, 2);
                    try { fechaNacimieto = Convert.ToDateTime(fecha); valido = true; }
                    catch (Exception x) { valido = false; }
                }
            }
            else
                valido = false;

            if (seccion1 && seccion2)
            {
                object[] rfcNoExiste = clientes.existeRFCcleinte(rfc, "C");
                if (Convert.ToBoolean(rfcNoExiste[0]))
                {
                    if (Convert.ToBoolean(rfcNoExiste[1]))
                        valido = false;
                    else
                        valido = true;
                }
            }
        }
        else
            valido = false;
        lblFecha.Text = fechaNacimieto.ToString();
        return valido;
    }

    protected void rbtnPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtApMat.Text = txtApPat.Text = txtNombre.Text = txtRazonNew.Text = "";
        string persona = rbtnPersona.SelectedValue.ToString();
        if (persona == "M")
        {
            txtRfc.MaxLength = 12;
            txtApMat.Visible = false;
            txtApPat.Visible = false;
            txtNombre.Visible = false;
            txtRazonNew.Visible = true;
            RequiredFieldValidator2.Enabled = true;
            RequiredFieldValidator3.Enabled = false;
            RequiredFieldValidator4.Enabled = false;
        }
        else if (persona == "F")
        {
            txtRfc.MaxLength = 13;
            txtApMat.Visible = true;
            txtApPat.Visible = true;
            txtNombre.Visible = true;
            txtRazonNew.Visible = false;
            RequiredFieldValidator2.Enabled = false;
            RequiredFieldValidator3.Enabled = true;
            RequiredFieldValidator4.Enabled = true;
        }
    }

    protected void GridCatClientes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete") { }
        else if (e.CommandName == "Select")
        {
            PanelDetalle.Visible = true;
            PanelGrid.Visible = false;
            PanelAdd.Visible = false;
        }
        else if (e.CommandName == "baja")
        {
            int idCliprov = Convert.ToInt32(e.CommandArgument);
            bool baja = datos.altaBajaCliprov(idCliprov, "B", "C");
            if (baja)
                GridClientes.DataBind();
            else
            {
                GridClientes.DataBind();
                lblError.Text = "El Cliente no se logro dar de baja, verifique su conexión e intentelo nuevamente mas tarde";
            }
        }
        else if (e.CommandName == "alta")
        {
            int idCliprov = Convert.ToInt32(e.CommandArgument);
            bool baja = datos.altaBajaCliprov(idCliprov, "A", "C");
            if (baja)
                GridClientes.DataBind();
            else
            {
                GridClientes.DataBind();
                lblError.Text = "El Cliente no se logro dar de alta, verifique su conexión e intentelo nuevamente mas tarde";
            }
        }
    }

    protected void lnkCerrarDetalle_Click(object sender, EventArgs e)
    {
        PanelDetalle.Visible = false;
        PanelGrid.Visible = true;
        PanelAdd.Visible = true;
        GridClientes.DataBind();
    }

    protected void GridClientes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("lnkEliminar") as LinkButton;
            try
            {
                var btnAlta = e.Row.FindControl("lnkAlta") as LinkButton;
                var btnBaja = e.Row.FindControl("lnkBaja") as LinkButton;
                string estatus = datos.obtieneEstatusCliprov(Convert.ToInt32(GridClientes.DataKeys[e.Row.RowIndex].Values[0].ToString()), "C");
                if (estatus == "B")
                {
                    btnBaja.Visible = false;
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                }
                else
                    btnAlta.Visible = false;
                if (GridClientes.EditIndex == -1)
                {
                    CatClientes catClient = new CatClientes();
                    object[] valores = catClient.tieneRelacion(Convert.ToInt32(GridClientes.DataKeys[e.Row.RowIndex].Values[0].ToString()), "C");
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
            }
            catch (Exception x)
            {
                Session["errores"] = x.Message;
                Session["paginaOrigen"] = "Clientes_Cat.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        if (DetailsView1.CurrentMode == DetailsViewMode.Edit)
        {
            RadioButtonList rdlPersonaMod = DetailsView1.FindControl("rdlPersonaMod") as RadioButtonList;
            TextBox txtNombreMod = DetailsView1.FindControl("txtNombreMod") as TextBox;
            TextBox txtApMod = DetailsView1.FindControl("txtApMod") as TextBox;
            TextBox txtAmMod = DetailsView1.FindControl("txtAmMod") as TextBox;
            RequiredFieldValidator RequiredFieldValidator8 = DetailsView1.FindControl("RequiredFieldValidator8") as RequiredFieldValidator;
            RequiredFieldValidator RequiredFieldValidator9 = DetailsView1.FindControl("RequiredFieldValidator9") as RequiredFieldValidator;
            RegularExpressionValidator RegularExpressionValidator11 = DetailsView1.FindControl("RegularExpressionValidator11") as RegularExpressionValidator;
            Telerik.Web.UI.RadNumericTextBox rgb_r = DetailsView1.FindControl("rgb_r_mod") as Telerik.Web.UI.RadNumericTextBox;
            Telerik.Web.UI.RadNumericTextBox rgb_g = DetailsView1.FindControl("rgb_g_mod") as Telerik.Web.UI.RadNumericTextBox;
            Telerik.Web.UI.RadNumericTextBox rgb_b = DetailsView1.FindControl("rgb_b_mod") as Telerik.Web.UI.RadNumericTextBox;
            Telerik.Web.UI.RadColorPicker paleta = DetailsView1.FindControl("RadColorPicker1") as Telerik.Web.UI.RadColorPicker;
            Telerik.Web.UI.RadComboBox ddlPais = DetailsView1.FindControl("ddlPais") as Telerik.Web.UI.RadComboBox;
            Telerik.Web.UI.RadComboBox ddlEstado = DetailsView1.FindControl("ddlEstado") as Telerik.Web.UI.RadComboBox;
            Telerik.Web.UI.RadComboBox ddlMunicipio = DetailsView1.FindControl("ddlMunicipio") as Telerik.Web.UI.RadComboBox;
            Telerik.Web.UI.RadComboBox ddlColonia = DetailsView1.FindControl("ddlColonia") as Telerik.Web.UI.RadComboBox;
            Telerik.Web.UI.RadComboBox ddlCodigo = DetailsView1.FindControl("ddlCodigo") as Telerik.Web.UI.RadComboBox;

            string pais = DataBinder.Eval(DetailsView1.DataItem, "pais").ToString().ToUpper();
            string estado = DataBinder.Eval(DetailsView1.DataItem, "estado").ToString().ToUpper();
            string municipio = DataBinder.Eval(DetailsView1.DataItem, "municipio").ToString().ToUpper();
            string colonia = DataBinder.Eval(DetailsView1.DataItem, "colonia").ToString().ToUpper();
            string cp = DataBinder.Eval(DetailsView1.DataItem, "cp").ToString();

            ddlPais.SelectedValue = obtieneIds(1, pais, "0", "0", "0", "0");
            ddlEstado.DataBind();
            ddlEstado.SelectedValue = obtieneIds(2, estado, ddlPais.SelectedValue, "0", "0", "0");
            ddlMunicipio.DataBind();
            ddlMunicipio.SelectedValue = obtieneIds(3, municipio, ddlPais.SelectedValue, ddlEstado.SelectedValue, "0", "0");
            ddlColonia.DataBind();
            ddlColonia.SelectedValue = obtieneIds(4, colonia, ddlPais.SelectedValue, ddlEstado.SelectedValue, ddlMunicipio.SelectedValue, "0");
            ddlCodigo.DataBind();
            ddlCodigo.SelectedValue = obtieneIds(5, cp, ddlPais.SelectedValue, ddlEstado.SelectedValue, ddlMunicipio.SelectedValue, ddlColonia.SelectedValue);


            string rojo = DataBinder.Eval(DetailsView1.DataItem, "rgb_r").ToString();
            string verde = DataBinder.Eval(DetailsView1.DataItem, "rgb_g").ToString();
            string azul = DataBinder.Eval(DetailsView1.DataItem, "rgb_b").ToString();

            rgb_r.Value = Convert.ToInt32(rojo);
            rgb_g.Value = Convert.ToInt32(verde);
            rgb_b.Value = Convert.ToInt32(azul);

            paleta.SelectedColor = Color.FromArgb(Convert.ToInt32(rojo), Convert.ToInt32(verde), Convert.ToInt32(azul));

            
            string persona = rdlPersonaMod.SelectedValue.ToString();
            if (persona == "F")
            {
                txtNombreMod.Enabled = true;
                txtApMod.Enabled = true;
                txtAmMod.Enabled = true;
                RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{4}[0-9]{6}[0-9A-Za-z]{3}$";
            }
            else if (persona == "M")
            {
                txtNombreMod.Enabled = false;
                txtApMod.Enabled = false;
                txtAmMod.Enabled = false;
                RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{3}[0-9]{6}[0-9A-Za-z]{3}$";
                RequiredFieldValidator8.Enabled = false;
                RequiredFieldValidator9.Enabled = false;

            }
        }
    }

    private string obtieneIds(int ddl, string cadena, string idPais, string idEstado, string idMunicipio, string idColonia)
    {
        string id = "0";
        object[] valores = new object[2];
        FacturacionElectronica.CatalogosFacturacion catDatos = new FacturacionElectronica.CatalogosFacturacion();
        catDatos.descripcion = cadena;
        switch (ddl) { 
            case 1:                
                catDatos.obtieneIdPais();
                valores = catDatos.info;
                if (Convert.ToBoolean(valores[0]))
                    id = Convert.ToString(valores[1]);
                else
                    id = "0";
                break;
            case 2:
                catDatos.idPais = idPais;
                catDatos.obtieneIdEstado();
                valores = catDatos.info;
                if (Convert.ToBoolean(valores[0]))
                    id = Convert.ToString(valores[1]);
                else
                    id = "0";
                break;
            case 3:
                catDatos.idPais = idPais;
                catDatos.idEstado = idEstado;
                catDatos.obtieneIdMunicipio();
                valores = catDatos.info;
                if (Convert.ToBoolean(valores[0]))
                    id = Convert.ToString(valores[1]);
                else
                    id = "0";
                break;
            case 4:
                catDatos.idPais = idPais;
                catDatos.idEstado = idEstado;
                catDatos.idMunicipio = idMunicipio;
                catDatos.obtieneIdColonia();
                valores = catDatos.info;
                if (Convert.ToBoolean(valores[0]))
                    id = Convert.ToString(valores[1]);
                else
                    id = "0";
                break;
            case 5:
                catDatos.idPais = idPais;
                catDatos.idEstado = idEstado;
                catDatos.idMunicipio = idMunicipio;
                catDatos.idColonia = idColonia;
                catDatos.obtieneIdCp();
                valores = catDatos.info;
                if (Convert.ToBoolean(valores[0]))
                    id = Convert.ToString(valores[1]);
                else
                    id = "0";
                break;
            default:
                id = "0";
                break;
        }
        return id;
    }

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        TextBox txtfechaMod = DetailsView1.FindControl("txtfechaMod") as TextBox;
        try
        {
            DateTime fechaValida = Convert.ToDateTime(txtfechaMod.Text);
            if (fechaValida > fechas.obtieneFechaLocal())
            {
                e.Cancel = true;
                lblError.Text = "La fecha de nacimiento no puede ser mayor a la de hoy";
            }            
        }
        catch (Exception x)
        {
            e.Cancel = true;
        }
    }

    protected void rdlPersonaMod_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rdlPersonaMod = DetailsView1.FindControl("rdlPersonaMod") as RadioButtonList;
        TextBox txtNombreMod = DetailsView1.FindControl("txtNombreMod") as TextBox;
        TextBox txtApMod = DetailsView1.FindControl("txtApMod") as TextBox;
        TextBox txtAmMod = DetailsView1.FindControl("txtAmMod") as TextBox;
        RequiredFieldValidator RequiredFieldValidator8 = DetailsView1.FindControl("RequiredFieldValidator8") as RequiredFieldValidator;
        RequiredFieldValidator RequiredFieldValidator9 = DetailsView1.FindControl("RequiredFieldValidator9") as RequiredFieldValidator;
        RegularExpressionValidator RegularExpressionValidator11 = DetailsView1.FindControl("RegularExpressionValidator11") as RegularExpressionValidator;
        string persona = rdlPersonaMod.SelectedValue.ToString();
        if (persona == "F")
        {
            txtNombreMod.Enabled = true;
            txtApMod.Enabled = true;
            txtAmMod.Enabled = true;
            RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{4}[0-9]{6}[0-9A-Za-z]{3}$";
        }
        else if (persona == "M")
        {
            txtNombreMod.Enabled = false;
            txtApMod.Enabled = false;
            txtAmMod.Enabled = false;
            RegularExpressionValidator11.ValidationExpression = "^[A-Za-z]{3}[0-9]{6}[0-9A-Za-z]{3}$";
            RequiredFieldValidator8.Enabled = false;
            RequiredFieldValidator9.Enabled = false;
        }
    }


    protected void GridClientes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            if (GridClientes.EditIndex == -1)
            {
                CatClientes catClient = new CatClientes();
                object[] valores = catClient.tieneRelacion(Convert.ToInt32(GridClientes.DataKeys[e.RowIndex].Values[0].ToString()), GridClientes.DataKeys[e.RowIndex].Values[1].ToString());
                if (Convert.ToBoolean(valores[0]))
                {
                    if (!Convert.ToBoolean(valores[1])) { }
                    else
                    {
                        e.Cancel = true;
                        lblError.Text = "No se puede eliminar el cliente ya que esta relacionado con otro proceso";
                    }
                }
                else
                    lblError.Text = "No se logro eliminar correctamente el cliente, verifique su conexión e intentelo nuevamente mas tarde";
            }
        }
        catch (Exception x)
        {
            e.Cancel = true;
        }
    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        CatClientes catClien = new CatClientes();
        FacturacionElectronica.Receptores receptor = new FacturacionElectronica.Receptores();
        TextBox txtRfc = DetailsView1.FindControl("txtRFCMod") as TextBox;
        TextBox txtNombre = DetailsView1.FindControl("txtRazonMod") as TextBox;
        TextBox txtCalle = DetailsView1.FindControl("txtCalleMod") as TextBox;
        TextBox txtNoExt = DetailsView1.FindControl("txtNoExtMod") as TextBox;
        TextBox txtNoInt = DetailsView1.FindControl("txtNoIntMod") as TextBox;
        TextBox txtLocalidad = DetailsView1.FindControl("txtLocalidadMod") as TextBox;
        TextBox txtReferencia = DetailsView1.FindControl("txtReferenciaMod") as TextBox;
        TextBox txtCorreo = DetailsView1.FindControl("txtCorreoMod") as TextBox;

        Telerik.Web.UI.RadNumericTextBox rgb_r = DetailsView1.FindControl("rgb_r_mod") as Telerik.Web.UI.RadNumericTextBox;
        Telerik.Web.UI.RadNumericTextBox rgb_g = DetailsView1.FindControl("rgb_g_mod") as Telerik.Web.UI.RadNumericTextBox;
        Telerik.Web.UI.RadNumericTextBox rgb_b = DetailsView1.FindControl("rgb_b_mod") as Telerik.Web.UI.RadNumericTextBox;
        Telerik.Web.UI.RadColorPicker paleta = DetailsView1.FindControl("RadColorPicker1") as Telerik.Web.UI.RadColorPicker;
        Telerik.Web.UI.RadAsyncUpload AsyncUpload1 = DetailsView1.FindControl("AsyncUpload1") as Telerik.Web.UI.RadAsyncUpload;
        Telerik.Web.UI.RadComboBox ddlPais = DetailsView1.FindControl("ddlPais") as Telerik.Web.UI.RadComboBox;
        Telerik.Web.UI.RadComboBox ddlEstado = DetailsView1.FindControl("ddlEstado") as Telerik.Web.UI.RadComboBox;
        Telerik.Web.UI.RadComboBox ddlMunicipio = DetailsView1.FindControl("ddlMunicipio") as Telerik.Web.UI.RadComboBox;
        Telerik.Web.UI.RadComboBox ddlColonia = DetailsView1.FindControl("ddlColonia") as Telerik.Web.UI.RadComboBox;
        Telerik.Web.UI.RadComboBox ddlCodigo = DetailsView1.FindControl("ddlCodigo") as Telerik.Web.UI.RadComboBox;


        int valor = paleta.SelectedColor.ToArgb();
        rgb_r.Value = paleta.SelectedColor.R;
        rgb_g.Value = paleta.SelectedColor.G;
        rgb_b.Value = paleta.SelectedColor.B;

        byte[] imagen = null;
        try
        {
            string filename = "";
            string extension = "";
            string ruta = Server.MapPath("~/TMP");

            // Si el directorio no existe, crearlo
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);


            int documentos = AsyncUpload1.UploadedFiles.Count;
            int guardados = 0;
            string[] archivosAborrar = new string[documentos];

            for (int i = 0; i < documentos; i++)
            {
                filename = AsyncUpload1.UploadedFiles[i].FileName;
                string[] segmenatado = filename.Split(new char[] { '.' });

                bool archivoValido = validaArchivo(segmenatado[1]);
                extension = segmenatado[1];
                string archivo = String.Format("{0}\\{1}", ruta, filename);

                FileInfo file = new FileInfo(archivo);
                if (archivoValido)
                {

                    // Verificar que el archivo no exista
                    if (File.Exists(archivo))
                        file.Delete();


                    Telerik.Web.UI.UploadedFile up = AsyncUpload1.UploadedFiles[i];
                    up.SaveAs(archivo);
                    archivosAborrar[i] = archivo;
                    imagen = File.ReadAllBytes(archivo);
                }
                else
                    imagen = null;
            }

            catClien.actualizaImagen(Convert.ToInt32(GridClientes.DataKeys[GridClientes.SelectedRow.RowIndex].Values[0].ToString()), imagen);
            catClien.actualizaDireccion(Convert.ToInt32(GridClientes.DataKeys[GridClientes.SelectedRow.RowIndex].Values[0].ToString()), ddlPais.SelectedItem, ddlEstado.SelectedItem, ddlMunicipio.SelectedItem, ddlColonia.SelectedItem, ddlCodigo.SelectedItem);
            receptor.agregarActualizarReceptor(txtRfc.Text, txtNombre.Text, txtCalle.Text, txtNoExt.Text, txtNoInt.Text, txtLocalidad.Text, txtReferencia.Text, txtCorreo.Text, ddlPais.SelectedValue, ddlEstado.SelectedValue, ddlMunicipio.SelectedValue, ddlColonia.SelectedValue, ddlCodigo.SelectedValue, "", "");
            for (int j = 0; j < archivosAborrar.Length; j++)
            {
                FileInfo archivoBorrar = new FileInfo(archivosAborrar[j]);
                archivoBorrar.Delete();
            }            
        }
        catch (Exception ex) { }
        catClien.actualizaColores(Convert.ToInt32(GridClientes.DataKeys[GridClientes.SelectedRow.RowIndex].Values[0].ToString()),rgb_r,rgb_g,rgb_b);
        DetailsView1.DataBind();
    }

    private bool validaArchivo(string extencion)
    {
        string[] extenciones = { "jpeg", "jpg", "png", "gif", "bmp", "tiff" };
        bool valido = false;
        for (int i = 0; i < extenciones.Length; i++)
        {
            if (extencion.ToUpper() == extenciones[i].ToUpper())
            {
                valido = true;
                break;
            }
        }
        return valido;
    }

    protected void ddlPais_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox ddlestados = DetailsView1.FindControl("ddlEstado") as RadComboBox;
        ddlestados.Text = "";
        ddlestados.SelectedIndex = -1;
        RadComboBox ddlMunicipio = DetailsView1.FindControl("ddlMunicipio") as RadComboBox;
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        RadComboBox ddlColonia = DetailsView1.FindControl("ddlColonia") as RadComboBox;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        RadComboBox ddlCp = DetailsView1.FindControl("ddlCodigo") as RadComboBox;
        ddlCp.Text = "";
        ddlCp.SelectedIndex = -1;
    }

    protected void ddlEstado_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox ddlMunicipio = DetailsView1.FindControl("ddlMunicipio") as RadComboBox;
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        RadComboBox ddlColonia = DetailsView1.FindControl("ddlColonia") as RadComboBox;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        RadComboBox ddlCp = DetailsView1.FindControl("ddlCodigo") as RadComboBox;
        ddlCp.Text = "";
        ddlCp.SelectedIndex = -1;
    }

    protected void ddlMunicipio_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox ddlColonia = DetailsView1.FindControl("ddlColonia") as RadComboBox;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        RadComboBox ddlCp = DetailsView1.FindControl("ddlCodigo") as RadComboBox;
        ddlCp.Text = "";
        ddlCp.SelectedIndex = -1;
    }

    protected void ddlColonia_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox ddlCp = DetailsView1.FindControl("ddlCodigo") as RadComboBox;
        try { ddlCp.SelectedIndex = 0; }
        catch (Exception)
        {
            ddlCp.Text = "";
            ddlCp.SelectedIndex = -1;
        }
    }
}