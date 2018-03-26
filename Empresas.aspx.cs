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

public partial class Empresas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Empresas.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }    
    
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnBtnEliminar = e.Row.FindControl("btnEliminar") as LinkButton;
            try
            {
                string modo = e.Row.RowState.ToString();
                string[] valor = null;
                try { valor = modo.Split(new char[] { ',' }); }
                catch (Exception) { modo = e.Row.ToString(); }
                if (valor != null)
                {
                    for (int i = 0; i < valor.Length; i++)
                    {
                        if (valor[i].Trim() == "Edit")
                        {
                            modo = "Edit";
                            break;
                        }
                        else
                            modo = valor[i].Trim();
                    }
                }
                if (modo != "Edit")
                {
                    CatEmpresas catEmp = new CatEmpresas();
                    object[] valores = catEmp.tieneRelacion(Convert.ToInt32(GridView1.DataKeys[e.Row.RowIndex].Value));
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
            catch (Exception ex)
            {
                Session["errores"] = ex.Message;
                Session["paginaOrigen"] = "Empresas.aspx";
                Response.Redirect("AppErrorLog.aspx");
            }
        }
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            CatEmpresas catEmp = new CatEmpresas();
            object[] valores = catEmp.tieneRelacion(Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value));
            if (Convert.ToBoolean(valores[0]))
            {
                if (!Convert.ToBoolean(valores[1])) { }
                else
                {
                    e.Cancel = true;
                    lblError.Text = "La empresa no se puede eliminar ya que esta relacionada en otro proceso";
                }
            }
            else
                lblError.Text = "La empresa no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
        catch (Exception x )
        {
            e.Cancel = true;
            lblError.Text = "La empresa no se logro eliminar correctamente, verifique su conexión e intentelo nuevamente mas tarde";
        }
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
        
    protected void lnkAgregarNuevo_Click(object sender, EventArgs e)
    {
        lblErrorNuevo.Text = "";
        try {
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
            finally {
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
                    if (Convert.ToInt32(agregar[1]) < 3)
                        lblError.Text = "Se agregó una la empresa " + txtRazon.Text.ToUpper().Trim();
                    else
                        lblError.Text = "";
                    string script = "cierraNewEmi()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
                    GridView1.DataBind();
                }
            }
        }
        catch (Exception ex) {
            lblErrorNuevo.Text = "Error al agregar empresa: " + ex.Message;
        }
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int id = Convert.ToInt32(btn.CommandArgument.ToString());
        lblIdEmisorMod.Text = id.ToString();
        lblErrorModifica.Text = "";
        cargaInformacion(id);
        string script = "abreModEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
    }

    protected void lnkEdita_Click(object sender, EventArgs e)
    {        
        lblErrorModifica.Text = "";
        try
        {
            FacturacionElectronica.ProcesamientoCertificados procCert = new FacturacionElectronica.ProcesamientoCertificados();
            Fechas fechas = new Fechas();
            string vigencia = lblVigenciaCertificadoMod.Text;   
            bool certificadoValido = true;
            string ruta = HttpContext.Current.Server.MapPath("~/TMP/" + txtRfcMod.Text.ToUpper().Trim());
            DateTime fechaActual = fechas.obtieneFechaLocal();
            if (RadAsyncUploadModCer != null)
            {
                object[] certificado = procesaArchivo(RadAsyncUploadModCer, txtRfcMod.Text.ToUpper().Trim());
                DateTime[] fechasVigencia = procCert.obtieneVigencia(Convert.ToString(certificado[0]), txtPassLlave.Text);
                try
                {
                    DateTime fechaIni, fechaFin;                    
                    fechaIni = fechasVigencia[0];
                    fechaFin = fechasVigencia[1];

                    if (fechaFin <= fechaActual)
                    {
                        certificadoValido = false;
                        lblErrorModifica.Text = "El certificado ya no se encuentra vigente";
                    }
                    else if (fechaFin > fechaActual)
                    {
                        certificadoValido = true;
                        vigencia = fechaFin.ToString("yyyy-MM-dd HH:mm:ss");
                    }

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
            }
            else {
                if (Convert.ToDateTime(vigencia) <= fechaActual)
                    certificadoValido = false;
                else if (Convert.ToDateTime(vigencia) > fechaActual) 
                    certificadoValido = true;
            }

            if (certificadoValido)
            {
                int id = Convert.ToInt32(lblIdEmisorMod.Text);
                object[] datosUi = { txtRfcMod.Text, txtRazonMod.Text, txtCalleMod.Text, txtNoExtMod.Text, txtNoIntMod.Text, ddlPaisMod, ddlEstadoMod, ddlMunicipioMod, ddlColoniaMod, ddlCodigoMod, txtLocalidadMod.Text, txtReferenciaMod.Text, txtCalleModEx.Text, txtNoExtModEx.Text, txtNoIntModEx.Text, ddlPaisModEx, ddlEstadoModEx, ddlMunicipioModEx, ddlColoniaModEx, ddlCodigoModEx, txtLocalidadModEx.Text, txtReferenciaModEx.Text, ddlServidorMod.SelectedValue, txtServidorMod.Text, txtUsuarioMod.Text, txtContrasenaMod.Text, txtCorreoMod.Text, txtCorreoModCC.Text, txtCorreoModCCO.Text, chkCifradoMod.Checked, txtPuertoMod.Text, txtNomCortoMod.Text, txtTel1Mod.Text, txtTel2Mod.Text, AsyncUploadMod1, RadAsyncUploadModCer, RadAsyncUploadModKey, RadAsyncUploadModPfx, txtPassLlaveMod.Text, vigencia, txtUserWsMod.Text, txtPassWsMod.Text, txtMsgCorreoMod.Text, lblRutaCer.Text, lblRutaKey.Text, lblRutaPfx.Text, id, lblLogo.Text };
                FacturacionElectronica.Emisores emisor = new FacturacionElectronica.Emisores();
                emisor.idEmisor = id;
                emisor.actualizaEmisor(datosUi);
                object[] agregar = emisor.info;
                if (Convert.ToBoolean(agregar[0]))
                {
                    if (Convert.ToInt32(agregar[1]) < 3)
                        lblError.Text = "Se actualizó la empresa " + txtRazonMod.Text.ToUpper().Trim();
                    else
                        lblError.Text = "";
                    string script = "cierraModEmi()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
                    GridView1.EditIndex = -1;
                    GridView1.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorModifica.Text = "Error al actualizar la empresa: " + ex.Message;
        }
    }

    protected void lnkCancelaEdit_Click(object sender, EventArgs e)
    {
        GridView1.EditIndex = -1;
        GridView1.DataBind();
        string script = "cierraModEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

    

    private void cargaInformacion(int id)
    {
        FacturacionElectronica.Emisores emisor = new FacturacionElectronica.Emisores();
        FacturacionElectronica.Certificados certificado = new FacturacionElectronica.Certificados();
        Catalogos empresa = new Catalogos();
        Envio_Mail mail = new Envio_Mail();
        ParametrosFacturacion parametrosFactura = new ParametrosFacturacion();

        object[] infoEmisor = null, infoCertificado = null, infoEmpresa = null, infoMail = null, infoParametrosFac = null;

        //Emisores 
        emisor.idEmisor = id;
        emisor.obtieneInfoEmisor();
        if (Convert.ToBoolean(emisor.info[0]))
        {
            DataSet infoEmi = (DataSet)emisor.info[1];
            foreach (DataRow filaE in infoEmi.Tables[0].Rows)
            {
                infoEmisor = filaE.ItemArray;
                break;
            }
        }
        else
            infoEmisor = null;

        //Certificados
        certificado.obtieneIdCertificadoVigente(id);
        object[] obtieneid = certificado.info;
        int idCertificado = 0;
        if (Convert.ToBoolean(obtieneid[0]))
            idCertificado = Convert.ToInt32(obtieneid[1]);
        certificado.idCertificado = idCertificado;
        certificado.obtieneCertificado();
        if (Convert.ToBoolean(certificado.info[0]))
        {
            DataSet infoCer = (DataSet)certificado.info[1];
            foreach (DataRow filaCer in infoCer.Tables[0].Rows)
            {
                infoCertificado = filaCer.ItemArray;
                break;
            }
        }
        else
            infoCertificado = null;

        //Empresas
        object[] empresaProc = empresa.obtieneEmpresa(id);
        if (Convert.ToBoolean(empresaProc[0]))
        {
            DataSet infoEmp = (DataSet)empresaProc[1];
            foreach (DataRow filaEmp in infoEmp.Tables[0].Rows)
            {
                infoEmpresa = filaEmp.ItemArray;
                break;
            }
        }
        else
            infoEmpresa = null;

        //Email
        object[] emailParam = mail.obtieneParametros(id);
        if (Convert.ToBoolean(emailParam[0]))
        {
            DataSet infoM = (DataSet)emailParam[1];
            foreach (DataRow filaMail in infoM.Tables[0].Rows)
            {
                infoMail = filaMail.ItemArray;
                break;
            }
        }
        else
            infoMail = null;

        //Facturacion
        parametrosFactura.id_empresa = id;
        parametrosFactura.obtieneParametos();
        if (Convert.ToBoolean(parametrosFactura.info[0]))
        {
            DataSet infoF = (DataSet)parametrosFactura.info[1];
            foreach (DataRow filaF in infoF.Tables[0].Rows)
            {
                infoParametrosFac = filaF.ItemArray;
                break;
            }
        }
        else
            infoParametrosFac = null;


        if (infoEmisor != null) {
            if (Convert.ToString(infoEmisor[1]).Length < 13)
                rbtnPersonaMod.SelectedValue = "M";
            else
                rbtnPersonaMod.SelectedValue = "F";

            txtRfcMod.Text = Convert.ToString(infoEmisor[1]).Trim();
            txtRazonMod.Text = Convert.ToString(infoEmisor[2]).Trim();
            txtCalleMod.Text = Convert.ToString(infoEmisor[3]).Trim();
            txtNoExtMod.Text = Convert.ToString(infoEmisor[4]).Trim();
            txtNoIntMod.Text = Convert.ToString(infoEmisor[5]).Trim();
            ddlPaisMod.SelectedValue = Convert.ToString(infoEmisor[6]);
            ddlEstadoMod.DataBind();
            ddlEstadoMod.SelectedValue = Convert.ToString(infoEmisor[8]);
            ddlMunicipioMod.DataBind();
            ddlMunicipioMod.SelectedValue = Convert.ToString(infoEmisor[10]);
            ddlColoniaMod.DataBind();
            ddlColoniaMod.SelectedValue = Convert.ToString(infoEmisor[12]);
            ddlCodigoMod.DataBind();
            ddlCodigoMod.SelectedValue = Convert.ToString(infoEmisor[14]);
            txtLocalidadMod.Text = Convert.ToString(infoEmisor[15]).Trim();
            txtReferenciaMod.Text = Convert.ToString(infoEmisor[16]).Trim();
            txtCalleModEx.Text = Convert.ToString(infoEmisor[17]).Trim();
            txtNoExtModEx.Text = Convert.ToString(infoEmisor[18]).Trim();
            txtNoIntModEx.Text = Convert.ToString(infoEmisor[19]).Trim();
            ddlPaisModEx.SelectedValue = Convert.ToString(infoEmisor[20]);
            ddlEstadoModEx.DataBind();
            ddlEstadoModEx.SelectedValue = Convert.ToString(infoEmisor[22]);
            ddlMunicipioModEx.DataBind();
            ddlMunicipioModEx.SelectedValue = Convert.ToString(infoEmisor[24]);
            ddlColoniaModEx.DataBind();
            ddlColoniaModEx.SelectedValue = Convert.ToString(infoEmisor[26]);
            ddlCodigoModEx.DataBind();
            ddlCodigoModEx.SelectedValue = Convert.ToString(infoEmisor[28]);
            txtLocalidadModEx.Text = Convert.ToString(infoEmisor[29]).Trim();
            txtReferenciaModEx.Text = Convert.ToString(infoEmisor[30]).Trim();
            ddlServidorMod.SelectedValue = Convert.ToString(infoEmisor[31]);
            txtServidorMod.Text = Convert.ToString(infoEmisor[32]).Trim();
            txtUsuarioMod.Text = Convert.ToString(infoEmisor[33]).Trim();
            txtContrasenaMod.Text = Convert.ToString(infoEmisor[34]).Trim();
            txtCorreoMod.Text = Convert.ToString(infoEmisor[35]).Trim();
            txtCorreoModCC.Text = Convert.ToString(infoEmisor[36]).Trim();
            txtCorreoModCCO.Text = Convert.ToString(infoEmisor[37]).Trim();
            lblLogo.Text = Convert.ToString(infoEmisor[38]).Trim();  
            imgLogo.ImageUrl = "~/LogoEmpresas.ashx?id=" + id.ToString();
            txtMsgCorreoMod.Text = Convert.ToString(infoEmisor[47]).Trim();
            txtNomCortoMod.Text = Convert.ToString(infoEmisor[46]).Trim();
        }
        if (infoMail != null) {
            txtPuertoMod.Text = Convert.ToString(infoMail[4]).Trim();
            chkCifradoMod.Checked = Convert.ToBoolean(infoMail[6]);
        }

        if (infoEmpresa != null) {
            txtTel1Mod.Text = Convert.ToString(infoEmpresa[11]).Trim();
            txtTel2Mod.Text = Convert.ToString(infoEmpresa[12]).Trim();
        }

        if (infoParametrosFac != null) {
            lblRutaCer.Text = Convert.ToString(infoParametrosFac[2]).Trim();
            lblRutaKey.Text = Convert.ToString(infoParametrosFac[3]).Trim();
            lblRutaPfx.Text = Convert.ToString(infoParametrosFac[4]).Trim();
            try
            {
                lblVigenciaCertificadoMod.Text = Convert.ToDateTime(Convert.ToString(infoParametrosFac[7]).Trim()).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception) { lblVigenciaCertificadoMod.Text = "No se ha indicado los archivos necesarios para facturación."; }
            txtPassLlaveMod.Text = Convert.ToString(infoParametrosFac[5]).Trim();
            txtUserWsMod.Text = Convert.ToString(infoParametrosFac[8]).Trim();
            txtPassWsMod.Text = Convert.ToString(infoParametrosFac[9]).Trim();
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
       
}