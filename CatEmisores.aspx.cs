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

public partial class CatEmisores : System.Web.UI.Page
{

    Ejecuciones ejecuta = new Ejecuciones();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        lblErrorNuevo.Text = "";
        try
        {
            FacturacionElectronica.Emisores emisor = new FacturacionElectronica.Emisores();
            Fechas fechas = new Fechas();
            FacturacionElectronica.ProcesamientoCertificados procCert = new FacturacionElectronica.ProcesamientoCertificados();
            object[] certificado = procesaArchivo(RadAsyncUploadCer, txtRfc.Text.ToUpper().Trim());
            DateTime[] fechasVigencia = procCert.obtieneVigencia(Convert.ToString(certificado[0]), txtPassLlave.Text);
            bool certificadoValido = false;
            string ruta = HttpContext.Current.Server.MapPath("~/TMP/" + txtRfc.Text.ToUpper().Trim());
            try
            {
                DateTime fechaIni, fechaFin;
                DateTime fechaActual = fechas.obtieneFechaLocal();
                fechaIni = fechasVigencia[0];
                fechaFin = fechasVigencia[1];

                if (fechaFin <= fechaActual)
                {
                    certificadoValido = false;
                    lblErrorModifica.Text = "El certificado ya no se encuentra vigente";
                }
                else if (fechaFin > fechaActual)
                    certificadoValido = true;

            }
            catch (Exception ex)
            {
                certificadoValido = false;
                lblErrorNuevo.Text = "Error: " + ex.Message;
            }
            finally
            {
                // Si el directorio no existe, eliminarlo
                if (!Directory.Exists(ruta))
                    Directory.Delete(ruta);
            }

            if (certificadoValido)
            {
                object[] datosUi = { txtRfc.Text, txtRazon.Text, "", "", "", txtCalle.Text, txtNoExt.Text, txtNoInt.Text, ddlPais, ddlEstado, ddlMunicipio, ddlColonia, ddlCodigo, txtLocalidad.Text, txtReferencia.Text, txtCalleEx.Text, txtNoExtEx.Text, txtNoIntEx.Text, ddlPaisEx, ddlEstadoEx, ddlMunicipioEx, ddlColoniaEx, ddlCodigoEx, txtLocalidadEx.Text, txtReferenciaEx.Text, ddlServidor.SelectedValue, txtServidor.Text, txtUsuario.Text, txtContrasena.Text, txtCorreo.Text, txtCorreoCC.Text, txtCorreoCCO.Text, chkCifrado.Checked, txtPuerto.Text, txtNomCorto.Text, txtTel1.Text, txtTel2.Text, AsyncUpload1, RadAsyncUploadCer, RadAsyncUploadKey, RadAsyncUploadPfx, txtPassLlave.Text, fechasVigencia[1].ToString("yyyy-MM-dd HH:mm:ss"), txtUserWs.Text, txtPassWs.Text, txtMsgCorreo.Text };
                emisor.agregarEmisor(datosUi);
                object[] agregar = emisor.info;
                if (Convert.ToBoolean(agregar[0]))
                {
                    if (Convert.ToInt32(agregar[1]) < 2)
                        lblError.Text = "Se agregó el emisor " + txtRazon.Text.ToUpper().Trim();
                    else
                        lblError.Text = "";
                    string script = "cierraNewEmi()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
                    RadGrid1.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorNuevo.Text = "Error al agregar empresa: " + ex.Message;
        }
    }

    private object[] procesaArchivo(Telerik.Web.UI.RadAsyncUpload AsyncUpload, string rfc)
    {
        object[] retorno = new object[4] { "", "", "", null };

        byte[] imagen = null;
        try
        {
            string filename = "";
            string extension = "";
            string ruta = HttpContext.Current.Server.MapPath("~/TMP/" + rfc.ToUpper().Trim());

            // Si el directorio no existe, crearlo
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);


            int documentos = AsyncUpload.UploadedFiles.Count;
            string[] archivosAborrar = new string[documentos];

            for (int i = 0; i < documentos; i++)
            {
                filename = AsyncUpload.UploadedFiles[i].FileName;
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


                    Telerik.Web.UI.UploadedFile up = AsyncUpload.UploadedFiles[i];
                    ruta = HttpContext.Current.Server.MapPath("~/TMP/" + rfc.ToUpper().Trim());
                    if (!Directory.Exists(ruta))
                        Directory.CreateDirectory(ruta);
                    archivo = String.Format("{0}\\{1}", ruta, filename);
                    up.SaveAs(archivo);
                    archivosAborrar[i] = archivo;
                    imagen = File.ReadAllBytes(archivo);
                }
                else
                    imagen = null;

                retorno[0] = archivo;
                retorno[1] = segmenatado[0];
                retorno[2] = extension;
                retorno[3] = imagen;
            }
        }
        catch (Exception ex) { retorno = new object[4] { "", "", "", null }; }
        return retorno;
    }

    private bool validaArchivo(string extencion)
    {
        string[] extenciones = { "cer", "key", "pfx" };
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

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        int idEmisor=0;
        try { idEmisor = Convert.ToInt32(RadGrid1.SelectedValues["idEmisor"]); }
        catch (Exception) { idEmisor = 0; }
        if(idEmisor != 0)
        {
            string sql = "delete emisores_f where idEmisor=" + idEmisor;
            object[] eliminar = ejecuta.scalarToString(sql); 
            if(eliminar[1].ToString()== "")
            {
                string sql2 = " delete Certificados_f where idEmisor="+ idEmisor;
                object[] borraCert = ejecuta.scalarToBool(sql2);
                RadGrid1.DataBind();
                lblError.Text = "Emisor eliminado correctamente";
            }
            else
            lblError.Text = "Error al eliminar el emisor";
        }
        else
        lblError.Text = "Debe seleccionar un emisor";
    }
    protected void lnkEdita_Click(object sender, EventArgs e)
    {

    }
    protected void lnkCancelaEdit_Click(object sender, EventArgs e)
    {
    }
    private void cargaInformacion(int id)
    { 
    
    }

    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnBorrar.Visible = true;
        btnEditar.Visible = true;
        btnSeleccionar.Visible = true;
    }

    protected void rbtnPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRazon.Text = "";
        string persona = rbtnPersona.SelectedValue.ToString();
        if (persona == "M")
            txtRfc.MaxLength = 12;
        else if (persona == "F")
            txtRfc.MaxLength = 13;
    }

    protected void ddlPais_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlEstado.Text = "";
        ddlEstado.SelectedIndex = -1;
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }
    protected void ddlEstado_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }
    protected void ddlMunicipio_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }
    protected void ddlColonia_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try { ddlCodigo.SelectedIndex = 0; }
        catch (Exception)
        {
            ddlCodigo.Text = "";
            ddlCodigo.SelectedIndex = -1;
        }
    }
    protected void ddlPaisEx_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlEstadoEx.Text = "";
        ddlEstadoEx.SelectedIndex = -1;
        ddlMunicipioEx.Text = "";
        ddlMunicipioEx.SelectedIndex = -1;
        ddlColoniaEx.Text = "";
        ddlColoniaEx.SelectedIndex = -1;
        ddlCodigoEx.Text = "";
        ddlCodigoEx.SelectedIndex = -1;
    }
    protected void ddlEstadoEx_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlMunicipioEx.Text = "";
        ddlMunicipioEx.SelectedIndex = -1;
        ddlColoniaEx.Text = "";
        ddlColoniaEx.SelectedIndex = -1;
        ddlCodigoEx.Text = "";
        ddlCodigoEx.SelectedIndex = -1;
    }
    protected void ddlMunicipioEx_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlColoniaEx.Text = "";
        ddlColoniaEx.SelectedIndex = -1;
        ddlCodigoEx.Text = "";
        ddlCodigoEx.SelectedIndex = -1;
    }
    protected void ddlColoniaEx_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try { ddlCodigoEx.SelectedIndex = 0; }
        catch (Exception)
        {
            ddlCodigoEx.Text = "";
            ddlCodigoEx.SelectedIndex = -1;
        }
    }

    protected void rbtnPersonaMod_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRazonMod.Text = "";
        string persona = rbtnPersonaMod.SelectedValue.ToString();
        if (persona == "M")
            txtRfcMod.MaxLength = 12;
        else if (persona == "F")
            txtRfcMod.MaxLength = 13;
    }
    protected void ddlPaisMod_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlEstadoMod.Text = "";
        ddlEstadoMod.SelectedIndex = -1;
        ddlMunicipioMod.Text = "";
        ddlMunicipioMod.SelectedIndex = -1;
        ddlColoniaMod.Text = "";
        ddlColoniaMod.SelectedIndex = -1;
        ddlCodigoMod.Text = "";
        ddlCodigoMod.SelectedIndex = -1;
    }
    protected void ddlEstadoMod_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlMunicipioMod.Text = "";
        ddlMunicipioMod.SelectedIndex = -1;
        ddlColoniaMod.Text = "";
        ddlColoniaMod.SelectedIndex = -1;
        ddlCodigoMod.Text = "";
        ddlCodigoMod.SelectedIndex = -1;
    }
    protected void ddlMunicipioMod_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlColoniaMod.Text = "";
        ddlColoniaMod.SelectedIndex = -1;
        ddlCodigoMod.Text = "";
        ddlCodigoMod.SelectedIndex = -1;
    }
    protected void ddlColoniaMod_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try { ddlCodigoMod.SelectedIndex = 0; }
        catch (Exception)
        {
            ddlCodigoMod.Text = "";
            ddlCodigoMod.SelectedIndex = -1;
        }
    }
    protected void ddlPaisModEx_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlEstadoModEx.Text = "";
        ddlEstadoModEx.SelectedIndex = -1;
        ddlMunicipioModEx.Text = "";
        ddlMunicipioModEx.SelectedIndex = -1;
        ddlColoniaModEx.Text = "";
        ddlColoniaModEx.SelectedIndex = -1;
        ddlCodigoModEx.Text = "";
        ddlCodigoModEx.SelectedIndex = -1;
    }
    protected void ddlEstadoModEx_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlMunicipioModEx.Text = "";
        ddlMunicipioModEx.SelectedIndex = -1;
        ddlColoniaModEx.Text = "";
        ddlColoniaModEx.SelectedIndex = -1;
        ddlCodigoModEx.Text = "";
        ddlCodigoModEx.SelectedIndex = -1;
    }
    protected void ddlMunicipioModEx_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlColoniaModEx.Text = "";
        ddlColoniaModEx.SelectedIndex = -1;
        ddlCodigoModEx.Text = "";
        ddlCodigoModEx.SelectedIndex = -1;
    }
    protected void ddlColoniaModEx_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        try { ddlCodigoModEx.SelectedIndex = 0; }
        catch (Exception)
        {
            ddlCodigoModEx.Text = "";
            ddlCodigoModEx.SelectedIndex = -1;
        }
    }
}