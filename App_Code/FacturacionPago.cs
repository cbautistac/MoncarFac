using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using E_Utilities;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using TimbradoRV;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Ecc;
using MessagingToolkit.QRCode.Codec.Data;
using MessagingToolkit.QRCode.Codec.Util;
using System.Drawing;
using System.Drawing.Imaging;
using WSGeneraCFDI;
using System.Text.RegularExpressions;

public class FacturacionPago
{
    public class Ejecucion
    {
        public string baseDatos;
        private SqlConnection conexionBD;
        private SqlCommand cmd;
        private DataSet ds;
        private SqlDataAdapter da;

        public object[] dataSet(string sql)
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings[baseDatos].ToString());
            object[] valor = new object[2] { false, false };
            ds = new DataSet();
            try
            {
                conexionBD.Open();
                cmd = new SqlCommand(sql, conexionBD);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                valor[0] = true;
                valor[1] = ds;

            }
            catch (Exception x)
            {
                valor[0] = false;
                valor[1] = x.Message;
            }
            finally
            {
                conexionBD.Close();
            }
            return valor;
        }
        internal object[] scalarToInt(string sql)
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings[baseDatos].ToString());
            object[] valor = new object[2] { false, "0" };
            try
            {
                int retorno = 0;
                conexionBD.Open();
                cmd = new SqlCommand(sql, conexionBD);
                retorno = Convert.ToInt32(cmd.ExecuteScalar());
                valor[0] = true;
                valor[1] = retorno.ToString();

            }
            catch (Exception x)
            {
                valor[0] = false;
                valor[1] = x.Message;
            }
            finally
            {
                conexionBD.Close();
            }
            return valor;
        }
        internal object[] scalarToDecimal(string sql)
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings[baseDatos].ToString());
            object[] valor = new object[2] { false, 0 };
            try
            {
                decimal retorno = 0;
                conexionBD.Open();
                cmd = new SqlCommand(sql, conexionBD);
                retorno = Convert.ToDecimal(cmd.ExecuteScalar());
                valor[0] = true;
                valor[1] = retorno;

            }
            catch (Exception x)
            {
                valor[0] = false;
                valor[1] = x.Message;
            }
            finally
            {
                conexionBD.Close();
            }
            return valor;
        }
        internal object[] scalarToString(string sql)
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings[baseDatos].ToString());
            object[] valor = new object[2] { false, "" };
            try
            {
                string retorno = "";
                conexionBD.Open();
                cmd = new SqlCommand(sql, conexionBD);
                retorno = Convert.ToString(cmd.ExecuteScalar());
                valor[0] = true;
                valor[1] = retorno.ToString();

            }
            catch (Exception x)
            {
                valor[0] = false;
                valor[1] = x.Message;
            }
            finally
            {
                conexionBD.Close();
            }
            return valor;
        }
        internal object[] insertUpdateDelete(string sql)
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings[baseDatos].ToString());
            object[] retorno = new object[2];
            try
            {
                conexionBD.Open();
                cmd = new SqlCommand(sql, conexionBD);
                cmd.ExecuteNonQuery();
                retorno[0] = true;
                retorno[1] = true;
            }
            catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
            finally
            {
                //conexionBD.Dispose();
                conexionBD.Close();
            }
            return retorno;
        }
        internal object[] insertAdjuntos(string sql, byte[] imagen)
        {
            conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings[baseDatos].ToString());
            object[] retorno = new object[2];
            try
            {
                conexionBD.Open();
                cmd = new SqlCommand(sql, conexionBD);
                cmd.Parameters.AddWithValue("imagen", imagen);
                cmd.ExecuteNonQuery();
                retorno[0] = true;
                retorno[1] = true;
            }
            catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
            finally
            {
                //conexionBD.Dispose();
                conexionBD.Close();
            }
            return retorno;
        }
    }

    public class Emisores
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idEmisor { get; set; }
        public object[] info { get; set; }

        public void obtieneInfoEmisor()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select e.IdEmisor,e.EmRFC,e.EmNombre,isnull(e.EmCalle,''),isnull(e.EmNoExt,''),isnull(e.EmNoInt,''),isnull(e.cve_pais,0),isnull((select p.desc_pais from Paises_f p where p.cve_pais=e.cve_pais),'') as EmPais,isnull(e.ID_Estado,0),isnull((select edos.nom_edo from Estados_f edos where edos.cve_pais=e.cve_pais and edos.cve_edo=e.ID_Estado),'') as EmEdo,isnull(e.ID_Del_Mun,0),isnull((select d.Desc_Del_Mun from DelegacionMunicipio_f d where d.cve_pais=e.cve_pais and d.ID_Estado=e.ID_Estado and d.ID_Del_Mun=e.ID_Del_Mun),'') as EmDelMun,isnull(e.ID_Colonia,0),isnull((select c.Desc_Colonia from Colonias_f c where c.cve_pais=e.cve_pais and c.ID_Estado=e.ID_Estado and c.ID_Del_Mun=e.ID_Del_Mun and c.ID_Colonia=e.ID_Colonia and c.ID_Cod_Pos=e.ID_Cod_Pos),'SANTA MARÍA AZTAHUACAN') as EmColonia,isnull(e.ID_Cod_Pos,''),isnull(e.EmLocalidad,''),isnull(e.EmReferencia,''),isnull(e.EmExCalle,''),isnull(e.EmExNoExt,''),isnull(e.EmExNoInt,''),isnull(e.EmExcve_pais,0),isnull((select p.desc_pais from Paises_f p where p.cve_pais=e.EmExcve_pais) ,'')as EmExPais,isnull(e.EmExID_Estado,0),isnull((select edos.nom_edo from Estados_f edos where edos.cve_pais=e.EmExcve_pais and edos.cve_edo=e.EmExID_Estado),'') as EmExEdo,isnull(e.EmExID_Del_Mun,0),isnull((select d.Desc_Del_Mun from DelegacionMunicipio_f d where d.cve_pais=e.EmExcve_pais and d.ID_Estado=e.EmExID_Estado and d.ID_Del_Mun=e.EmExID_Del_Mun),'') as EmExDelMun,isnull(e.EmExID_Colonia,0),isnull((select c.Desc_Colonia from Colonias_f c where c.cve_pais=e.EmExcve_pais and c.ID_Estado=e.EmExID_Estado and c.ID_Del_Mun=e.EmExID_Del_Mun and c.ID_Colonia=e.EmExID_Colonia and c.ID_Cod_Pos=e.EmExID_Cod_Pos),'SANTA MARÍA AZTAHUACAN') as EmExCodPos,isnull(e.EmExID_Cod_Pos,''),isnull(e.EmExLocalidad,''),isnull(e.EmExReferencia,''),isnull(e.EmCorreo_TipoServidor,1),isnull(e.EmCorreo_Servidor,''),isnull(e.EmCorreo_Usuario,''),isnull(e.EmCorreo_Contra,''),isnull(e.EmCorreo_Cuenta,''),isnull(e.EmCorreo_CC,''),isnull(e.EmCorreo_CCO,''),isnull(e.EmRuta_Logo,''),isnull(e.EmRuta_RFC_CBB,''),isnull(e.EmSwCfd_l_Cbb,''),isnull(e.EmRegimen,''),isnull(e.EmTipoPoliza,''),isnull(e.EmCuentaCancelacion,''),isnull(e.EmCuentaAjuste,''),isnull(e.EmCuentaIngreso,''),isnull(e.EmDescCorta,''),isnull(EmMsgCorreo,''),isnull(e.Id_Matriz,0) from emisores_f e where e.IdEmisor=" + idEmisor.ToString();
            info = ejecuta.dataSet(sql);
        }

        public void existeEmisor(string rfc)
        {
            ejecuta.baseDatos = "PVW";
            sql = "select isnull((select TOP 1 idEmisor from emisores_f where rtrim(ltrim(emRfc))='" + rfc.ToUpper().Trim() + "'),0) as emisor";
            info = ejecuta.scalarToInt(sql);
        }


        public void agregarEmisor(object[] datosUi)
        {
            Fechas fechas = new Fechas();
            Certificados certificadoFactura = new Certificados();
            int compleados = 0;
            object[] retorno = new object[] { false, compleados };
            try
            {
                string rfc = datosUi[0] as string;
                string razon = datosUi[1] as string;
                string nombre = datosUi[2] as string;
                string apPat = datosUi[3] as string;
                string apMat = datosUi[4] as string;
                string calle = datosUi[5] as string;
                string noExt = datosUi[6] as string;
                string noInt = datosUi[7] as string;
                Telerik.Web.UI.RadComboBox ddlPais = datosUi[8] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlEstado = datosUi[9] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlMunicipio = datosUi[10] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlColonia = datosUi[11] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlCodigo = datosUi[12] as Telerik.Web.UI.RadComboBox;
                string localidad = datosUi[13] as string;
                string referencia = datosUi[14] as string;
                string calleExt = datosUi[15] as string;
                string noExtEx = datosUi[16] as string;
                string noIntEx = datosUi[17] as string;
                Telerik.Web.UI.RadComboBox ddlPaisEx = datosUi[18] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlEstadoEx = datosUi[19] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlMunicipioEx = datosUi[20] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlColoniaEx = datosUi[21] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlCodigoEx = datosUi[22] as Telerik.Web.UI.RadComboBox;
                string localidadExt = datosUi[23] as string;
                string referenciaExt = datosUi[24] as string;
                string tipoServidor = datosUi[25] as string;
                string servidor = datosUi[26] as string;
                string usuario = datosUi[27] as string;
                string contraseña = datosUi[28] as string;
                string correo = datosUi[29] as string;
                string correoCC = datosUi[30] as string;
                string correoCCO = datosUi[31] as string;
                bool ssl = Convert.ToBoolean(datosUi[32]);
                string puerto = datosUi[33] as string;
                string nomCorto = datosUi[34] as string;
                string tel1 = datosUi[35] as string;
                string tel2 = datosUi[36] as string;
                Telerik.Web.UI.RadAsyncUpload AsyncUploadLogo = datosUi[37] as Telerik.Web.UI.RadAsyncUpload;
                Telerik.Web.UI.RadAsyncUpload AsyncUploadCer = datosUi[38] as Telerik.Web.UI.RadAsyncUpload;
                Telerik.Web.UI.RadAsyncUpload AsyncUploadKey = datosUi[39] as Telerik.Web.UI.RadAsyncUpload;
                Telerik.Web.UI.RadAsyncUpload AsyncUploadPfx = datosUi[40] as Telerik.Web.UI.RadAsyncUpload;
                string passLlave = datosUi[41] as string;
                string vigencia = datosUi[42] as string;
                string userWs = datosUi[43] as string;
                string passWs = datosUi[44] as string;
                string msgCorreo = datosUi[45] as string;

                object[] archivoLogo = procesaArchivo(AsyncUploadLogo, rfc);
                object[] archivoCer = procesaArchivo(AsyncUploadCer, rfc);
                object[] archivoKey = procesaArchivo(AsyncUploadKey, rfc);
                object[] archivoPfx = procesaArchivo(AsyncUploadPfx, rfc);

                int id_emisor = 0;
                existeEmisor(rfc);
                if (Convert.ToBoolean(info[0]))
                    id_emisor = Convert.ToInt32(info[1]);
                if (idEmisor == 0)
                {
                    ejecuta.baseDatos = "PVW";
                    // llena Emisores
                    sql = string.Format("insert into Emisores_f(id_empresa,idEmisor,EmRFC,EmNombre,EmCalle,EmNoExt,EmNoInt,cve_pais,ID_Estado,ID_Del_Mun,ID_Cod_Pos,ID_Colonia,EmLocalidad,EmReferencia," +
        "EmExCalle,EmExNoExt,EmExNoInt,EmExcve_pais,EmExID_Estado,EmExID_Del_Mun,EmExID_Cod_Pos,EmExID_Colonia,EmExLocalidad,EmExReferencia," +
        "EmCorreo_TipoServidor,EmCorreo_Servidor,EmCorreo_Usuario,EmCorreo_Contra,EmCorreo_Cuenta,EmCorreo_CC,EmCorreo_CCO,EmRuta_Logo,EmDescCorta,EmMsgCorreo) " +
        "values(1,(select isnull ((select top 1 idEmisor from emisores_f order by IdEmisor desc),0)+1),'{0}','{1}','{2}','{3}','{4}',{5},{6},{7},'{8}',{9},'{10}','{11}','{12}','{13}','{14}',{15},{16},{17},'{18}',{19},'{20}','{21}',{22},'{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}')",
        rfc.ToUpper().Trim(), razon.ToUpper().Trim(), calle.ToUpper().Trim(), noExt.ToUpper().Trim(), noInt.ToUpper().Trim(), ddlPais.SelectedItem.Value, ddlEstado.SelectedItem.Value, ddlMunicipio.SelectedItem.Value, ddlCodigo.SelectedItem.Value, ddlColonia.SelectedItem.Value, localidad.ToUpper().Trim(), referencia.ToUpper().Trim(),
        calleExt.ToUpper().Trim(), noExtEx.ToUpper().Trim(), noIntEx.ToUpper().Trim(), ddlPaisEx.SelectedItem.Value, ddlEstadoEx.SelectedItem.Value, ddlMunicipioEx.SelectedItem.Value, ddlCodigoEx.SelectedItem.Value, ddlColoniaEx.SelectedItem.Value, localidadExt.ToUpper().Trim(), referenciaExt.ToUpper().Trim(),
        tipoServidor, servidor, usuario, contraseña, correo, correoCC, correoCCO, Convert.ToString(archivoLogo[0]), nomCorto.ToUpper().Trim(), msgCorreo);
                    info = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(info[0]))
                    {
                        compleados++;
                        existeEmisor(rfc);
                        if (Convert.ToBoolean(info[0]))
                            id_emisor = Convert.ToInt32(info[1]);
                        if (id_emisor != 0)
                        {
                            DateTime fechaVigencia = Convert.ToDateTime("1900-01-01");
                            try { fechaVigencia = Convert.ToDateTime(vigencia); }
                            catch (Exception) { fechaVigencia = Convert.ToDateTime("1900-01-01"); }
                            DateTime fechaActual = Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"));

                            //llena Certificados
                            if (fechaVigencia > fechaActual)
                            {/*
                                sql = string.Format("insert into Certificados_f(IdEmisor,CertDescrip,CertRutaCert,CertRutaLlave,CertContra,CertFechaCaduca) VALUES({0},'{1}','{2}','{3}','{4}','{5}')", id_emisor, "Certificado Vigente", rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoCer[4]), rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoKey[4]), passLlave, vigencia);
                                info = ejecuta.insertUpdateDelete(sql);
                                if (Convert.ToBoolean(info[0]))
                                    compleados++;*/
                            }

                            /*
                            Ejecucion ejecutaTaller = new Ejecucion();

                            ejecutaTaller.baseDatos = "Taller";
                            byte[] logo = (byte[])archivoLogo[3];
                            int puertoServ = 0;
                            try { puertoServ = Convert.ToInt32(puerto); }
                            catch (Exception) { puertoServ = 587; }
                            int sslServ = 0;
                            if (ssl)
                                sslServ = 1;

                            // llena empresas,email y facturacion

                            if (logo != null)
                            {
                                sql = string.Format("insert into empresas(id_empresa,razon_social,rfc,calle,num_ext,num_int,colonia,cp,municipio,estado,pais,tel_principal,tel_oficina,logo) values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',@imagen) " +
                                    "insert into parametros_email values({0},'{13}','{14}','{15}',{16},{17},{18}) " +
                                    "insert into parametros_facturacion(id_empresa,id_parametros,rutaCertificado,rutaKey,rutaPfx,contrasenaLlave,finVigencia,usuarioWS,psWS) values({0},1,'{19}','{20}','{21}','{22}','{23}','{24}','{25}')",
                                    id_emisor, razon.ToUpper().Trim(), rfc.ToUpper().Trim(), calle.ToUpper().Trim(), noExt.ToUpper().Trim(), noInt.ToUpper().Trim(), ddlColonia.SelectedItem.Text.ToUpper().Trim(), ddlCodigo.SelectedItem.Value, ddlMunicipio.SelectedItem.Text.ToUpper().Trim(),
                                    ddlEstado.SelectedItem.Text.ToUpper().Trim(), ddlPais.SelectedItem.Text.ToUpper().Trim(), tel1.Trim(), tel2.Trim(), correo.Trim(), contraseña.Trim(), servidor.Trim(), puertoServ.ToString(), tipoServidor.ToString(), sslServ.ToString(),
                                    rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoCer[4]), rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoKey[4]), rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoPfx[4]), passLlave, vigencia, userWs, passWs);
                                ejecutaTaller.insertAdjuntos(sql, logo);
                            }
                            else
                            {
                                sql = string.Format("insert into empresas(id_empresa,razon_social,rfc,calle,num_ext,num_int,colonia,cp,municipio,estado,pais,tel_principal,tel_oficina) values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}') " +
                                    "insert into parametros_email values({0},'{13}','{14}','{15}',{16},{17},{18}) " +
                                    "insert into parametros_facturacion(id_empresa,id_parametros,rutaCertificado,rutaKey,rutaPfx,contrasenaLlave,finVigencia,usuarioWS,psWS) values({0},1,'{19}','{20}','{21}','{22}','{23}','{24}','{25}')",
                                    id_emisor, razon.ToUpper().Trim(), rfc.ToUpper().Trim(), calle.ToUpper().Trim(), noExt.ToUpper().Trim(), noInt.ToUpper().Trim(), ddlColonia.SelectedItem.Text.ToUpper().Trim(), ddlCodigo.SelectedItem.Value, ddlMunicipio.SelectedItem.Text.ToUpper().Trim(),
                                    ddlEstado.SelectedItem.Text.ToUpper().Trim(), ddlPais.SelectedItem.Text.ToUpper().Trim(), tel1.Trim(), tel2.Trim(), correo.Trim(), contraseña.Trim(), servidor.Trim(), puertoServ.ToString(), tipoServidor.ToString(), sslServ.ToString(),
                                    rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoCer[4]), rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoKey[4]), rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoPfx[4]), passLlave, vigencia, userWs, passWs);
                                ejecutaTaller.insertUpdateDelete(sql);
                            }
                            if (Convert.ToBoolean(info[0]))
                                compleados++;*/
                        }

                    }
                    else
                        info = new object[] { false, compleados };
                }
                else
                    info = new object[] { false, "Error: El emisor ya se encuentra registrado, por favor verifique la información" };
            }
            catch (Exception ex) { info = new object[] { false, "Error: " + ex.Message }; }
        }

        private object[] procesaArchivo(Telerik.Web.UI.RadAsyncUpload AsyncUpload, string rfc)
        {
            object[] retorno = new object[5] { "", "", "", null, "" };

            byte[] imagen = null;
            string filename = "";
            string extension = "";
            string ruta = HttpContext.Current.Server.MapPath("~/RequisitosFiscales/" + rfc.ToUpper().Trim());
            try
            {


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
                        //if (File.Exists(archivo))
                        //    file.Delete();


                        Telerik.Web.UI.UploadedFile up = AsyncUpload.UploadedFiles[i];
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
                    retorno[4] = filename;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string ruta1 = HttpContext.Current.Server.MapPath("~/TMP/" + rfc.ToUpper().Trim() + "/" + filename);
                    ruta = HttpContext.Current.Server.MapPath("~/RequisitosFiscales/" + rfc.ToUpper().Trim() + "/" + filename);
                    string[] segmenatado = filename.Split(new char[] { '.' });
                    FileInfo archivito = new FileInfo(ruta1);
                    if (archivito.Exists)
                    {
                        archivito.CopyTo(ruta, true);
                        retorno[0] = ruta;
                        retorno[1] = segmenatado[0];
                        retorno[2] = segmenatado[1];
                        retorno[3] = imagen;
                        retorno[4] = filename;
                    }
                }
                catch (Exception ex1) { retorno = new object[5] { "", "", "", null, "" }; }
            }
            return retorno;
        }

        private bool validaArchivo(string extencion)
        {
            string[] extenciones = { "jpeg", "jpg", "png", "gif", "bmp", "tiff", "cer", "key", "pfx" };
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

        public void actualizaEmisor(object[] datosUi)
        {
            Fechas fechas = new Fechas();
            Certificados certificadoFactura = new Certificados();
            int compleados = 0;
            object[] retorno = new object[] { false, compleados };
            try
            {
                string rfc = datosUi[0] as string;
                string razon = datosUi[1] as string;
                string calle = datosUi[2] as string;
                string noExt = datosUi[3] as string;
                string noInt = datosUi[4] as string;
                Telerik.Web.UI.RadComboBox ddlPais = datosUi[5] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlEstado = datosUi[6] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlMunicipio = datosUi[7] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlColonia = datosUi[8] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlCodigo = datosUi[9] as Telerik.Web.UI.RadComboBox;
                string localidad = datosUi[10] as string;
                string referencia = datosUi[11] as string;
                string calleExt = datosUi[12] as string;
                string noExtEx = datosUi[13] as string;
                string noIntEx = datosUi[14] as string;
                Telerik.Web.UI.RadComboBox ddlPaisEx = datosUi[15] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlEstadoEx = datosUi[16] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlMunicipioEx = datosUi[17] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlColoniaEx = datosUi[18] as Telerik.Web.UI.RadComboBox;
                Telerik.Web.UI.RadComboBox ddlCodigoEx = datosUi[19] as Telerik.Web.UI.RadComboBox;
                string localidadExt = datosUi[20] as string;
                string referenciaExt = datosUi[21] as string;
                string tipoServidor = datosUi[22] as string;
                string servidor = datosUi[23] as string;
                string usuario = datosUi[24] as string;
                string contraseña = datosUi[25] as string;
                string correo = datosUi[26] as string;
                string correoCC = datosUi[27] as string;
                string correoCCO = datosUi[28] as string;
                bool ssl = Convert.ToBoolean(datosUi[29]);
                string puerto = datosUi[30] as string;
                string nomCorto = datosUi[31] as string;
                string tel1 = datosUi[32] as string;
                string tel2 = datosUi[33] as string;
                Telerik.Web.UI.RadAsyncUpload AsyncUploadLogo = datosUi[34] as Telerik.Web.UI.RadAsyncUpload;
                Telerik.Web.UI.RadAsyncUpload AsyncUploadCer = datosUi[35] as Telerik.Web.UI.RadAsyncUpload;
                Telerik.Web.UI.RadAsyncUpload AsyncUploadKey = datosUi[36] as Telerik.Web.UI.RadAsyncUpload;
                Telerik.Web.UI.RadAsyncUpload AsyncUploadPfx = datosUi[37] as Telerik.Web.UI.RadAsyncUpload;
                string passLlave = datosUi[38] as string;
                string vigencia = datosUi[39] as string;
                string userWs = datosUi[40] as string;
                string passWs = datosUi[41] as string;
                string msgCorreo = datosUi[42] as string;
                string rutaCer = datosUi[43] as string;
                string rutaKey = datosUi[44] as string;
                string rutaPfx = datosUi[45] as string;
                int idEmisor = Convert.ToInt32(datosUi[46]);
                string rutaLogo = datosUi[47] as string;

                object[] archivoLogo = procesaArchivo(AsyncUploadLogo, rfc);
                object[] archivoCer = procesaArchivo(AsyncUploadCer, rfc);
                object[] archivoKey = procesaArchivo(AsyncUploadKey, rfc);
                object[] archivoPfx = procesaArchivo(AsyncUploadPfx, rfc);

                string rC = "", rK = "", rP = "", rL = "";
                if (Convert.ToString(archivoLogo[0]) != "")
                    rL = rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoLogo[4]);
                else
                    rL = rutaLogo;

                if (Convert.ToString(archivoCer[0]) != "")
                    rC = rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoCer[4]);
                else
                    rC = rutaCer;

                if (Convert.ToString(archivoKey[0]) != "")
                    rK = rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoKey[4]);
                else
                    rK = rutaKey;

                if (Convert.ToString(archivoPfx[0]) != "")
                    rP = rfc.ToUpper().Trim() + "\\" + Convert.ToString(archivoPfx[4]);
                else
                    rP = rutaPfx;


                if (idEmisor != 0)
                {
                    ejecuta.baseDatos = "PVW";
                    // llena Emisores
                    sql = string.Format("update emisores set EmRFC='{0}',EmNombre='{1}',EmCalle='{2}',EmNoExt='{3}',EmNoInt='{4}',cve_pais={5},ID_Estado={6},ID_Del_Mun={7},ID_Cod_Pos='{8}',ID_Colonia={9},EmLocalidad='{10}',EmReferencia='{11}'," +
        "EmExCalle='{12}',EmExNoExt='{13}',EmExNoInt='{14}',EmExcve_pais={15},EmExID_Estado={16},EmExID_Del_Mun={17},EmExID_Cod_Pos='{18}',EmExID_Colonia={19},EmExLocalidad='{20}',EmExReferencia='{21}'," +
        "EmCorreo_TipoServidor={22},EmCorreo_Servidor='{23}',EmCorreo_Usuario='{24}',EmCorreo_Contra='{25}',EmCorreo_Cuenta='{26}',EmCorreo_CC='{27}',EmCorreo_CCO='{28}',EmRuta_Logo='{29}',EmDescCorta='{30}',EmMsgCorreo='{31}' where idEmisor={32}",
        rfc.ToUpper().Trim(), razon.ToUpper().Trim(), calle.ToUpper().Trim(), noExt.ToUpper().Trim(), noInt.ToUpper().Trim(), ddlPais.SelectedItem.Value, ddlEstado.SelectedItem.Value, ddlMunicipio.SelectedItem.Value, ddlCodigo.SelectedItem.Value, ddlColonia.SelectedItem.Value, localidad.ToUpper().Trim(), referencia.ToUpper().Trim(),
        calleExt.ToUpper().Trim(), noExtEx.ToUpper().Trim(), noIntEx.ToUpper().Trim(), ddlPaisEx.SelectedItem.Value, ddlEstadoEx.SelectedItem.Value, ddlMunicipioEx.SelectedItem.Value, ddlCodigoEx.SelectedItem.Value, ddlColoniaEx.SelectedItem.Value, localidadExt.ToUpper().Trim(), referenciaExt.ToUpper().Trim(),
        tipoServidor, servidor, usuario, contraseña, correo, correoCC, correoCCO, rL, nomCorto.ToUpper().Trim(), msgCorreo, idEmisor);
                    info = ejecuta.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(info[0]))
                    {
                        compleados++;

                        DateTime fechaVigencia = Convert.ToDateTime("1900-01-01");
                        try { fechaVigencia = Convert.ToDateTime(vigencia); }
                        catch (Exception) { fechaVigencia = Convert.ToDateTime("1900-01-01"); }
                        DateTime fechaActual = Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"));

                        //llena Certificados

                        sql = string.Format("update Certificados set CertDescrip='{1}',CertRutaCert='{2}',CertRutaLlave='{3}',CertContra='{4}',CertFechaCaduca='{5}' where idEmisor={0}", idEmisor, "Certificado Vigente", rC, rK, passLlave, fechaVigencia.ToString("yyyy-MM-dd"));
                        info = ejecuta.insertUpdateDelete(sql);
                        if (Convert.ToBoolean(info[0]))
                            compleados++;


                        Ejecucion ejecutaTaller = new Ejecucion();

                        ejecutaTaller.baseDatos = "Taller";
                        byte[] logo = (byte[])archivoLogo[3];
                        int puertoServ = 0;
                        try { puertoServ = Convert.ToInt32(puerto); }
                        catch (Exception) { puertoServ = 587; }
                        int sslServ = 0;
                        if (ssl)
                            sslServ = 1;

                        // llena empresas,email y facturacion

                        if (logo != null)
                        {
                            sql = string.Format("update empresas set razon_social='{1}',rfc='{2}',calle='{3}',num_ext='{4}',num_int='{5}',colonia='{6}',cp='{7}',municipio='{8}',estado='{9}',pais='{10}',tel_principal='{11}',tel_oficina='{12}',logo=@imagen where id_empresa={0} " +
                                "update parametros_email set usuario='{13}', contrasena='{14}', host='{15}', puerto={16}, tipo_servidor={17}, ssl_habilitado={18} where clave={0} " +
                                "update parametros_facturacion set rutaCertificado='{19}',rutaKey='{20}',rutaPfx='{21}',contrasenaLlave='{22}',finVigencia='{23}',usuarioWS='{24}',psWS='{25}' where id_empresa={0} and id_parametros=1",
                                idEmisor, razon.ToUpper().Trim(), rfc.ToUpper().Trim(), calle.ToUpper().Trim(), noExt.ToUpper().Trim(), noInt.ToUpper().Trim(), ddlColonia.SelectedItem.Text.ToUpper().Trim(), ddlCodigo.SelectedItem.Value, ddlMunicipio.SelectedItem.Text.ToUpper().Trim(),
                                ddlEstado.SelectedItem.Text.ToUpper().Trim(), ddlPais.SelectedItem.Text.ToUpper().Trim(), tel1.Trim(), tel2.Trim(), correo.Trim(), contraseña.Trim(), servidor.Trim(), puertoServ.ToString(), tipoServidor.ToString(), sslServ.ToString(),
                                rC, rK, rP, passLlave, fechaVigencia.ToString("yyyy-MM-dd HH:mm:ss"), userWs, passWs);
                            ejecutaTaller.insertAdjuntos(sql, logo);
                        }
                        else
                        {
                            sql = string.Format("update empresas set razon_social='{1}',rfc='{2}',calle='{3}',num_ext='{4}',num_int='{5}',colonia='{6}',cp='{7}',municipio='{8}',estado='{9}',pais='{10}',tel_principal='{11}',tel_oficina='{12}' where id_empresa={0} " +
                                "update parametros_email set usuario='{13}', contrasena='{14}', host='{15}', puerto={16}, tipo_servidor={17}, ssl_habilitado={18} where clave={0} " +
                                "update parametros_facturacion set rutaCertificado='{19}',rutaKey='{20}',rutaPfx='{21}',contrasenaLlave='{22}',finVigencia='{23}',usuarioWS='{24}',psWS='{25}' where id_empresa={0} and id_parametros=1",
                                idEmisor, razon.ToUpper().Trim(), rfc.ToUpper().Trim(), calle.ToUpper().Trim(), noExt.ToUpper().Trim(), noInt.ToUpper().Trim(), ddlColonia.SelectedItem.Text.ToUpper().Trim(), ddlCodigo.SelectedItem.Value, ddlMunicipio.SelectedItem.Text.ToUpper().Trim(),
                                ddlEstado.SelectedItem.Text.ToUpper().Trim(), ddlPais.SelectedItem.Text.ToUpper().Trim(), tel1.Trim(), tel2.Trim(), correo.Trim(), contraseña.Trim(), servidor.Trim(), puertoServ.ToString(), tipoServidor.ToString(), sslServ.ToString(),
                                rC, rK, rP, passLlave, fechaVigencia.ToString("yyyy-MM-dd HH:mm:ss"), userWs, passWs);
                            ejecutaTaller.insertUpdateDelete(sql);
                        }
                        if (Convert.ToBoolean(info[0]))
                            compleados++;


                    }
                    else
                        info = new object[] { false, compleados };
                }
                else
                    info = new object[] { false, "Error: No se actualizó el emisor" };
            }
            catch (Exception ex) { info = new object[] { false, "Error: " + ex.Message }; }
        }
    }



    public class Receptores
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idReceptor { get; set; }
        public int idReceptorElegido { get; set; }
        public object[] info { get; set; }

        public void obtieneInfoReceptor()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select e.IdRecep,e.ReRFC,e.ReNombre,isnull(e.ReCalle,''),isnull(e.ReNoExt,''),isnull(e.ReNoInt,''),isnull(e.Recve_pais,0),isnull((select p.desc_pais from Paises_f p where p.cve_pais=e.Recve_pais),'') as EmPais,isnull(e.Re_ID_Estado,0),isnull((select edos.nom_edo from Estados_f edos where edos.cve_pais=e.Recve_pais and edos.cve_edo=e.Re_ID_Estado),'') as EmEdo,isnull(e.Re_ID_Del_Mun,0),isnull((select d.Desc_Del_Mun from DelegacionMunicipio_f d where d.cve_pais=e.Recve_pais and d.ID_Estado=e.Re_ID_Estado and d.ID_Del_Mun=e.Re_ID_Del_Mun),'') as EmDelMun,isnull(e.ReID_Colonia,0),isnull((select c.Desc_Colonia from Colonias_f c where c.cve_pais=e.Recve_pais and c.ID_Estado=e.Re_ID_Estado and c.ID_Del_Mun=e.Re_ID_Del_Mun and c.ID_Colonia=e.ReID_Colonia and right('00000'+ltrim(rtrim(c.ID_Cod_Pos)),5)=right('00000'+ltrim(rtrim(e.ReID_Cod_Pos)),5)),'SANTA MARÍA AZTAHUACAN') as EmColonia,isnull(e.ReID_Cod_Pos,''),isnull(e.ReLocalidad,''),isnull(e.ReReferencia,''),isnull(e.ReCorreo,''),isnull(e.ReCorreoCC,''),isnull(e.ReCorreoCCO,''),isnull(e.ReCtaCxc,''),isnull(e.IdMatriz,0),isnull(e.recorreo,'') as correo,isnull(e.recorreocc,'') as correoCC,isnull(e.recorreocco,'') as correoCCO from Receptores_f e where e.IdRecep=" + idReceptor.ToString();
            info = ejecuta.dataSet(sql);
        }
        public void obtieneInfoReceptor1()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select e.IdRecep,e.ReRFC,e.ReNombre,isnull(e.ReCalle,''),isnull(e.ReNoExt,''),isnull(e.ReNoInt,''),isnull(e.Recve_pais,0),isnull((select p.desc_pais from Paises_f p where p.cve_pais=e.Recve_pais),'') as EmPais,isnull(e.Re_ID_Estado,0),isnull((select edos.nom_edo from Estados_f edos where edos.cve_pais=e.Recve_pais and edos.cve_edo=e.Re_ID_Estado),'') as EmEdo,isnull(e.Re_ID_Del_Mun,0),isnull((select d.Desc_Del_Mun from DelegacionMunicipio_f d where d.cve_pais=e.Recve_pais and d.ID_Estado=e.Re_ID_Estado and d.ID_Del_Mun=e.Re_ID_Del_Mun),'') as EmDelMun,isnull(e.ReID_Colonia,0),isnull((select c.Desc_Colonia from Colonias_f c where c.cve_pais=e.Recve_pais and c.ID_Estado=e.Re_ID_Estado and c.ID_Del_Mun=e.Re_ID_Del_Mun and c.ID_Colonia=e.ReID_Colonia and right('00000'+ltrim(rtrim(c.ID_Cod_Pos)),5)=right('00000'+ltrim(rtrim(e.ReID_Cod_Pos)),5)),'') as EmColonia,isnull(e.ReID_Cod_Pos,''),isnull(e.ReLocalidad,''),isnull(e.ReReferencia,''),isnull(e.ReCorreo,''),isnull(e.ReCorreoCC,''),isnull(e.ReCorreoCCO,''),isnull(e.ReCtaCxc,''),isnull(e.IdMatriz,0),isnull(e.recorreo,'') as correo,isnull(e.recorreocc,'') as correoCC,isnull(e.recorreocco,'') as correoCCO from Receptores_f e where e.IdRecep=" + idReceptorElegido.ToString();
            info = ejecuta.dataSet(sql);
        }

        public void obtieneUnReceptorXElidReceptor()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select ReRFC, IdMatriz, ReNombre, ReCorreo, ReCorreoCC, ReCorreoCCO, ReCalle, ReNoExt, ReNoInt, ReLocalidad, ReReferencia  from Receptores_f" +
                " where IdRecep = " + idReceptorElegido);
            info = ejecuta.dataSet(sql);
        }

        public void agregarActualizarReceptor(string rfc, string nombre, string calle, string ext, string inte, string localidad, string referencia, string correo, string pais, string estado, string municipio, string colonia, string cp, string cc, string cco)
        {
            ejecuta.baseDatos = "PVW";
            existeReceptor(rfc);
            if (Convert.ToBoolean(info[0]))
            {
                int existe = Convert.ToInt32(info[1]);
                if (existe > 0)
                {
                    obtieneIdReceptor(rfc);
                    if (Convert.ToBoolean(info[0]))
                    {
                        int id = Convert.ToInt32(info[1]);
                        sql = string.Format("update receptores_f set reRFC='{0}', reNombre='{1}', reCorreo='{2}',reCalle='{3}',reNoExt='{4}',reNoInt='{5}', reLocalidad='{6}',reReferencia='{7}',Recve_pais={8} ,Re_ID_Estado={9},Re_ID_Del_Mun={10},ReID_Colonia={11},ReID_Cod_Pos='{12}',ReCorreoCC='{14}',ReCorreoCCO='{15}' where idRecep={13}", rfc.ToUpper().Trim(), nombre.Trim().ToUpper(), correo.Trim().ToLower(), calle.ToUpper().Trim(), ext.Trim().ToUpper(), inte.ToUpper().Trim(), localidad.Trim().ToUpper(), referencia.ToUpper().Trim(), pais, estado, municipio, colonia, cp, id, cc.Trim().ToLower(), cco.Trim().ToLower());
                        info = ejecuta.insertUpdateDelete(sql);
                    }
                }
                else
                {
                    sql = string.Format("insert into Receptores_f (ReRFC,ReNombre,ReCorreo,ReCalle,ReNoExt,ReNoInt,ReLocalidad,ReReferencia,Recve_pais,Re_ID_Estado,Re_ID_Del_Mun,ReID_Colonia,ReID_Cod_Pos,recorreocc,recorreocco) values" +
                        "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},{10},{11},'{12}','{13}','{14}')",
                        rfc.ToUpper().Trim(), nombre.Trim().ToUpper(), correo.Trim().ToLower(), calle.ToUpper().Trim(), ext.Trim().ToUpper(), inte.ToUpper().Trim(), localidad.Trim().ToUpper(), referencia.ToUpper().Trim(), pais, estado, municipio, colonia, cp, cc.Trim().ToLower(), cco.Trim().ToLower());
                    info = ejecuta.insertUpdateDelete(sql);
                }
            }
        }

        public void existeReceptor(string rfc)
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select count(*) from Receptores_f where RTRIM(LTRIM(reRFC))='{0}'", rfc.ToUpper().Trim());
            info = ejecuta.scalarToInt(sql);
        }



        public void obtieneIdReceptor(string rfc)
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select idRecep from Receptores_f where RTRIM(LTRIM(reRFC))='{0}'", rfc.Trim().ToUpper());
            info = ejecuta.scalarToInt(sql);
        }

        public void tieneRelacion()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select count(*) from recepcion_pagos_f where idrecep={0}", idReceptor);
            info = ejecuta.scalarToInt(sql);
        }

        public void eliminar()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("delete from Receptores_f where idrecep={0}", idReceptor);
            info = ejecuta.insertUpdateDelete(sql);
        }
    }

    public class Unidades
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idUnidad { get; set; }
        public string descUnid { get; set; }
        public object[] info { get; set; }

        public void obtieneIdUnidad()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select top 1 idUnid from unidades where UnidDesc like '%" + descUnid + "%'";
            info = ejecuta.scalarToInt(sql);
        }
        public void obtieneDescripcionUnidad()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select UnidDesc from unidades_f where idUnid =" + idUnidad;
            info = ejecuta.scalarToString(sql);
        }
    }

    public class Certificados
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idCertificado { get; set; }
        public object[] info { get; set; }

        public void obtieneIdCertificadoVigente(int idEmisor)
        {
            ejecuta.baseDatos = "PVW";
            sql = "select isnull((select top 1 idCertifica from Certificados where idEmisor=" + idEmisor.ToString() + " order by idcertifica desc),0)";
            info = ejecuta.scalarToInt(sql);
        }

        public void obtieneCertificado()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select * from Certificados where idCertifica=" + idCertificado.ToString();
            info = ejecuta.dataSet(sql);
        }
    }

    public class AddendaQualitas
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int id { get; set; }
        public object[] info { get; set; }


        public void obtieneParametros()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select * from Addenda_Qualitas where No_Registro=" + id.ToString();
            info = ejecuta.dataSet(sql);
        }


        public void actualiza(string NoInterno, string idArea, string idRevision, string cdgEmisor, string cdgReceptor, string oficina, string nombreE, string emailE, string telE, string nombreR, string emailR, string telR, string tipoE, string tipoR, decimal deducible, string bancoDeduc, string fechaDeduc, string folioDeduc, decimal demerito, string bancoDeme, string fechaDeme, string folioDeme, int tipoCliente)
        {
            ejecuta.baseDatos = "PVW";
            try
            {
                sql = string.Format("insert into addenda_qualitas values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',{15},'{16}','{17}','{18}',{19},'{20}','{21}','{22}',{23})",
                    id, NoInterno.PadLeft(2, '0'), idArea.PadLeft(3, '0'), idRevision.PadLeft(3, '0'), cdgEmisor.PadLeft(5, '0'), tipoE, nombreE, emailE, telE, tipoR, nombreR, emailR, telR, oficina.PadLeft(3, '0'), cdgReceptor, deducible.ToString("F2"), bancoDeduc, fechaDeduc, folioDeduc, demerito.ToString("F2"), bancoDeme, fechaDeme, folioDeme, tipoCliente);
                info = ejecuta.insertUpdateDelete(sql);
                if (!Convert.ToBoolean(info[0]))
                {
                    sql = string.Format("update addenda_qualitas set no_interno='{1}',id_area='{2}',id_revision='{3}',cdgintemisor='{4}',cetipo='{5}',cenombre='{6}',ceemail='{7}',cetelefono='{8}',crtipo='{9}',crnombre='{10}',cremail='{11}',crtelefono='{12}',oficinaentrega='{13}',cdgintrecep='{14}',mDeducible={15},bancoDeducible='{16}',fechaDeducible='{17}',folioFichaDeducible='{18}',mDemerito={19},bancoDemerito='{20}',fechaDemerito='{21}',folioFichaDemerito='{22}',tipoCliente={23} where no_registro={0}",
                    id, NoInterno.PadLeft(2, '0'), idArea.PadLeft(3, '0'), idRevision.PadLeft(3, '0'), cdgEmisor.PadLeft(5, '0'), tipoE, nombreE, emailE, telE, tipoR, nombreR, emailR, telR, oficina.PadLeft(3, '0'), cdgReceptor, deducible.ToString("F2"), bancoDeduc, fechaDeduc, folioDeduc, demerito.ToString("F2"), bancoDeme, fechaDeme, folioDeme, tipoCliente);
                    info = ejecuta.insertUpdateDelete(sql);
                }
            }
            catch (Exception ex)
            {
                info[0] = false;
                info[1] = ex.Message;
            }
        }
    }

    public class ImpRetenciones
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idRetencion { get; set; }
        public object[] info { get; set; }

        public void obtieneImpuesto()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select retNombre from ImpRetenidos where id_ret=" + idRetencion;
            info = ejecuta.scalarToString(sql);
        }
    }

    public class ImpTrasladados
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idTrasladado { get; set; }
        public object[] info { get; set; }

        public void obtieneImpuesto()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select trasNombre from ImpTrasladado where id_tras=" + idTrasladado;
            info = ejecuta.scalarToString(sql);
        }

        public void obtieneTasaImpuesto()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select TrasTasa from ImpTrasladado where id_tras=" + idTrasladado;
            info = ejecuta.scalarToDecimal(sql);
        }
    }

    public class TiposDocumentos
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idTipoDoc { get; set; }
        public object[] info { get; set; }

        public void obtieneIdEsquema()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select idEsquema from tipodoc where idTipoDoc=" + idTipoDoc.ToString();
            info = ejecuta.scalarToInt(sql);
        }
    }

    public class Esquemas
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idEsquema { get; set; }
        public object[] info { get; set; }

        public void obtieneTipoCombrobante()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select EsTipoComprobante from EsquemaDoc where idEsquema=" + idEsquema.ToString();
            info = ejecuta.scalarToString(sql);
        }
    }

    public class Moneda
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idMoneda { get; set; }
        public object[] info { get; set; }

        public void obtieneDescripcionMoneda()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select mondescrip from Moneda where idMoneda=" + idMoneda.ToString();
            info = ejecuta.scalarToString(sql);
        }
    }

    public class Facturas
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idCfd { get; set; }
        public object[] info { get; set; }
        public string estatus { get; set; }

        public void obtieneEncabezado()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select idcfd,IdTipoDoc,IdEmisor,IdRecep,IdMoneda,encformapago,EncCondicionesPago,EncMetodoPago,EncDescGlob,EncDescGlobImp,EncMotivoDescuento,EncEstatus,cast(EncTipoCambio as decimal(5,4)) AS EncTipoCambio,EncNota,EncReferencia,EncNumCtaPago,isnull(EncRegimen,'') as EncRegimen,isnull(EncLugarExpedicion,'') as EncLugarExpedicion,EncFolioImpresion,EncSerieImpresion,encemRfc,encrerfc,encsubtotal,encdesc,encImptras,encimpret,enctotal,EncDescMO,EncDescRefaccion,tipo,UsoCFDi_SAT,TipoDocumento from recepcion_pagos_f where idcfdant=" + idCfd.ToString();
            info = ejecuta.dataSet(sql);
        }
        public void obtieneDetalle()
        {
            ejecuta.baseDatos = "PVW";
            //recuperar el uuid, de la factura timbrada.
            //sql = "select IdConcepto,DetDesc,DetCantidad,IdUnid,DetValorUnit,cast(Subtotal as decimal(15,5)) as Subtotal,DetPorcDesc,DetImpDesc,cast(Subtotal as decimal(15,5)),IdTras3,IdTras2,cast(DetImpTras3 as decimal(15,5)) as DetImpTras3,cast(DetImpTras2 as decimal(15,4)) as DetImpTras2,IdRet1,IdRet2,cast(DetImpRet1 as decimal(15,5)) as DetImpRet1,cast(DetImpRet2 as decimal(15,5)) as DetImpRet2,cast(Total as decimal(15,5)) as Total,ClaveProdServ,ClaveUnidad_SAT  from DetCFD_f where IdCfd=" + idCfd.ToString() + " order by IdDetCfd asc";
            sql = "select idcfd,idemisor,idrecep,EncFolioUUID,Right(EncReferencia,5) as FolioFactura,EncCertificado as CertificadoSAT,(cast(EncFechaGenera as varchar)+'T'+cast(EncHoraGenera as varchar)) as Horatimbrado, EncTotal from enccfd_f where IdCfd=" + idCfd.ToString();
            info = ejecuta.dataSet(sql);
        }

        public void obtieneTimbrado()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select top 1 IdCfd,IdTimbre,noCertificadoSat,fechaTimbrado,uuid,selloSat,selloCFD,rutaArchivo,cadenaOriginal,noCertificadoCfd,qr from recepcionpagos_f where IdCfd=" + idCfd.ToString() + " order by idtimbre desc";
            info = ejecuta.dataSet(sql);
        }

        public void obtieneEncabezadoPAGO()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select idcfd,IdTipoDoc,IdEmisor,IdRecep,IdMoneda,encformapago,EncCondicionesPago,EncMetodoPago,EncDescGlob,EncDescGlobImp,EncMotivoDescuento,EncEstatus,cast(EncTipoCambio as decimal(5,4)) AS EncTipoCambio,EncNota,EncReferencia,EncNumCtaPago,isnull(EncRegimen,'') as EncRegimen,isnull(EncLugarExpedicion,'') as EncLugarExpedicion,EncFolioImpresion,EncSerieImpresion,encemRfc,encrerfc,encsubtotal,encdesc,encImptras,encimpret,enctotal,EncDescMO,EncDescRefaccion,tipo,UsoCFDi_SAT,TipoDocumento,UUIDFactura,nocertificadoOrg from recepcion_pagos_f where idcfdant=" + idCfd.ToString();
            info = ejecuta.dataSet(sql);
        }
        public void obtieneDetallePAGO()
        {
            ejecuta.baseDatos = "PVW";
            //recuperar el uuid, de la factura timbrada.
            //sql = "select IdConcepto,DetDesc,DetCantidad,IdUnid,DetValorUnit,cast(Subtotal as decimal(15,5)) as Subtotal,DetPorcDesc,DetImpDesc,cast(Subtotal as decimal(15,5)),IdTras3,IdTras2,cast(DetImpTras3 as decimal(15,5)) as DetImpTras3,cast(DetImpTras2 as decimal(15,4)) as DetImpTras2,IdRet1,IdRet2,cast(DetImpRet1 as decimal(15,5)) as DetImpRet1,cast(DetImpRet2 as decimal(15,5)) as DetImpRet2,cast(Total as decimal(15,5)) as Total,ClaveProdServ,ClaveUnidad_SAT  from DetCFD_f where IdCfd=" + idCfd.ToString() + " order by IdDetCfd asc";
            //sql = "select idcfd,idemisor,idrecep,EncFolioUUID,Right(EncReferencia,5) as FolioFactura,EncCertificado as CertificadoSAT,(cast(EncFechaGenera as varchar)+'T'+cast(EncHoraGenera as varchar)) as Horatimbrado, EncTotal from recepcion_pagos_f where idcfdant=" + idCfd.ToString();
            sql = "select UUIDFactura,Folio,Parcialidad,SaldoAnterior,SaldoPagado,SaldoActual from recepcion_pagos_f where idcfdant=" + idCfd.ToString();
            info = ejecuta.dataSet(sql);
        }

        public void obtieneTimbradoPAGO()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select top 1 IdCfd,IdTimbre,noCertificadoSat,fechaTimbrado,uuid,selloSat,selloCFD,rutaArchivo,cadenaOriginal,noCertificadoCfd,qr from RecepcionPagos_f where IdCfd=" + idCfd.ToString() + " order by idtimbre desc";
            info = ejecuta.dataSet(sql);
        }

        public void actualizaEstatusFactura()
        {
            ejecuta.baseDatos = "PVW";
            string cambio = "";
            if (estatus == "C")
                cambio = ",estatus='CAN' ";
            DateTime fecha = new E_Utilities.Fechas().obtieneFechaLocal();
            sql = "update recepcion_pagos_f set EncEstatus='" + estatus + "',encfechacancel='" + fecha.ToString("yyyy-MM-dd") + "',enchoracancel='" + fecha.ToString("HH:mm:ss") + "' where IdCfd=" + idCfd.ToString() + " update facturas set estatus_factura='" + estatus + "'" + cambio + " where idcfd=" + idCfd.ToString();
            info = ejecuta.insertUpdateDelete(sql);
        }
    }

    public class GeneracionDocumentos
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int idCfd { get; set; }
        public string erroresXml { get; set; }
        private object[] infoEnc;
        private object[] infoDet;
        public object[] info { get; set; }
        public bool conAddenda { get; set; }

        public void obtieneEncabezadoFactura()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select e.encemrfc,e.EncReRFC, e.EncFecha,e.EncHora,isnull((select lower(ed.EsTipoComprobante) " +
" from EsquemaDoc_f ed where ed.IdEsquema in(select idesquema from TipoDoc_f td where td.IdTipoDoc = e.idtipodoc)),'') as tipoComprobante, " +
" isnull(e.EncFormaPago,'') as EncFormaPago,ISNULL(e.EncCondicionesPago,'') as EncCondicionesPago,cast(e.EncSubTotal as decimal(15,2)) as EncSubTotal, " +
" cast(e.EncDescGlobImp as decimal(15,2))+cast(e.EncDesc as decimal(15,2)) as EncDescuento,cast(e.EncTipoCambio as decimal(5,2)) as EncTipoCambio, " +
" (select Mon_Desc from Moneda_f where IdMoneda=e.IdMoneda) as EncMoneda,cast(e.EncTotal as decimal(15,2)) as EncTotal,isnull(e.EncMetodoPago,'') as EncMetodoPago " +
" ,e.EncLugarExpedicion,isnull(e.EncNumCtaPago,'') as EncNumCtaPago,isnull(e.EncMotivoDescuento,'') as EncMotivoDescuento,isnull(e.EncSerieImpresion,'') as EncSerieImp, " +
" isnull(e.EncFolioImpresion,'') as EncFolioImp,isnull(e.EncEmNombre,'') as EncEmNombre,isnull(e.EncEmCalle,'') as EncEmCalle,isnull(e.EncEmNoExt,'') as EncEmNoExt, " +
" isnull(e.EncEmNoInt,'') as EncEmNoInt,isnull((select Desc_Colonia from Colonias_f where cve_pais=e.EncEmPais and ID_Estado=e.EncEmEstado and ID_Del_Mun=e.EncEmDelMun " +
" and ID_Colonia=e.EncEmColonia and ID_Cod_Pos=e.EncEmCP),'') as EncEmColonia,isnull(e.EncEmLocalidad,'') as EncEmLocalidad,isnull(e.EncEmReferenc,'') as EncEmReferencia, " +
" isnull((select Desc_Del_Mun from DelegacionMunicipio_f where cve_pais=e.EncEmPais and ID_Estado=e.EncEmEstado and ID_Del_Mun=e.EncEmDelMun),'') as EncEmDelMun, " +
" isnull((select nom_edo from Estados_f  where cve_pais=e.EncEmPais and cve_edo=e.EncEmEstado),'') as EncEmEstado, " +
" isnull((select desc_pais from Paises_f where cve_pais=e.EncEmPais),'') as EncEmPais,isnull(e.EncEmCP,'') as EncEmCp,isnull(e.EncEmExCalle,'') as EncEmExCalle, " +
" isnull(e.EncEmExNoExt,'') as EncEmExNoExt,isnull(e.EncEmExNoInt,'') as EncEmExNoInt, " +
" isnull((select Desc_Colonia from Colonias_f where cve_pais=e.EncEmExPais and ID_Estado=e.EncEmExEstado and ID_Del_Mun=e.EncEmExDelMun and ID_Colonia=e.EncEmExColonia " +
" and ID_Cod_Pos=e.EncEmExCP),'') as EncEmExColonia,isnull(e.EncEmExLocalidad,'') as EncEmExLocalidad,isnull(e.EncEmExReferenc,'') as EncEmExReferencia, " +
" isnull((select Desc_Del_Mun from DelegacionMunicipio_f where cve_pais=e.EncEmExPais and ID_Estado=e.EncEmExEstado and ID_Del_Mun=e.EncEmExDelMun),'') as EncEmExDelMun, " +
" isnull((select nom_edo from Estados_f  where cve_pais=e.EncEmExPais and cve_edo=e.EncEmExEstado),'') as EncEmExEstado, " +
" isnull((select desc_pais from Paises_f where cve_pais=e.EncEmExPais),'') as EncEmExPais,isnull(e.EncEmExCP,'') as EncEmExCp,ISNULL(e.EncRegimen,'') as EncRegimen, " +
" isnull(e.EncReNombre,'') as EncReNombre,isnull(e.EncReCalle,'') as EncReCalle,isnull(e.EncReNoExt,'') as EncEmReExt,isnull(e.EncReNoInt,'') as EncReNoInt, " +
" isnull(e.EncReColonia,'') as EncReColonia,isnull(e.EncReLocalidad,'') as EncReLocalidad,isnull(e.EncReReferenc,'') as EncReReferencia,isnull(e.EncReDelMun,'') as EncReDelMun, " +
" isnull(e.EncReEstado,'') as EncReEstado,isnull(e.EncRePais,'') as EncRePais,isnull(e.EncReCP,'') as EncReCp,cast(e.EncImpRet as decimal(15,2)) as EncRetenciones, " +
" cast(e.EncImpTras as decimal(15,2)) as EncTraslados,isnull(e.EncReferencia,'') as EncReferencia,isnull(e.EncFechaGenera,'1900-01-01') as EncFechaGenera, " +
" isnull(e.EncHoraGenera,'00:00:00') as EncHoraGenera,(SELECT Clave_Mon from Moneda_f where idMoneda=e.IdMoneda)  as MonAbrev, cast(e.EncDescGlob as decimal(5,2)) AS porcDesc, " +
" isnull((select distinct top 1 i.trasnombre from DetCFD_f d inner join ImpTrasladado_f i on i.id_tras=d.idtras3 where d.idcfd=e.idcfd),'IVA') AS impuesto, " +
" isnull((select distinct top 1 i.trastasa from DetCFD_f d inner join ImpTrasladado_f i on i.id_tras=d.idtras3 where d.idcfd=e.idcfd),0) AS tasaImpuesto, " +
" isnull(e.encserieimpresion,'') as encserieimpresion,isnull(e.encfoliouuid,'') as uuid,isnull(e.EncSello,'') as sello,isnull(e.nocertificadoorg,'') as noCertificado, " +
" isnull(e.certificado,'') as certificado,e.idcfd,e.tipo from recepcion_pagos_f e where e.IdCfd=" + idCfd.ToString();
            infoEnc = ejecuta.dataSet(sql);
            info = infoEnc;
        }

        public void obtieneDetalleFactura()
        {
            ejecuta.baseDatos = "PVW";
            sql = "select * from DetCFD_f  where IdCfd=" + idCfd.ToString() + " order by IdDetCfd asc";
            infoDet = ejecuta.dataSet(sql);
            info = infoDet;
        }

        public void generaDoctoTimbrado()
        {
            string ruta = "";
            try
            {
                obtieneEncabezadoFactura();
                if (Convert.ToBoolean(infoEnc[0]))
                {
                    DataSet encabezadoFac = (DataSet)infoEnc[1];
                    DataTable dtEncabezado = encabezadoFac.Tables[0];
                    if (dtEncabezado.Rows.Count > 0)
                    {
                        object[] encabezado = dtEncabezado.Rows[0].ItemArray;
                        obtieneDetalleFactura();
                        if (Convert.ToBoolean(infoDet[0]))
                        {
                            DataSet detalleFac = (DataSet)infoDet[1];
                            DataTable dtDetalle = detalleFac.Tables[0];
                            if (dtDetalle.Rows.Count > 0)
                            {

                                object[] timbre = null;
                                try
                                {
                                    obtieneDocumentoTimbrado2(encabezado[61].ToString());
                                    if (Convert.ToBoolean(info[0]))
                                    {
                                        DataSet infoTimbre = (DataSet)info[1];
                                        DataTable dtTimbre = infoTimbre.Tables[0];

                                        if (dtTimbre.Rows.Count > 0)
                                        {
                                            timbre = dtTimbre.Rows[0].ItemArray;
                                        }
                                    }
                                }
                                catch (Exception) { timbre = null; }



                                cfdv32.Comprobante comprobante = new cfdv32.Comprobante();
                                comprobante = generaComprobante(encabezado, dtDetalle, true, timbre);
                                ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + encabezado[0].ToString().Trim().ToUpper() + "/" + encabezado[1].ToString().Trim().ToUpper() + "/");

                                if (!Directory.Exists(ruta))
                                    Directory.CreateDirectory(ruta);

                                XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
                                xmlNameSpace.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
                                //xmlNameSpace.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                                string archivo = ruta + Convert.ToString(timbre[0]);
                                FileInfo file = new FileInfo(archivo + ".xml");

                                XmlTextWriter xmlTextWriter = new XmlTextWriter(archivo + ".xml", Encoding.UTF8);
                                xmlTextWriter.Formatting = Formatting.Indented;
                                XmlSerializer xs = new XmlSerializer(typeof(cfdv32.Comprobante));

                                xs.Serialize(xmlTextWriter, comprobante, xmlNameSpace);
                                xmlTextWriter.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                erroresXml = "Error: " + ex.Message;
                info[0] = false;
                info[1] = erroresXml;
            }
        }


        public void generaDocto()
        {
            string ruta = "";
            try
            {
                obtieneEncabezadoFactura();
                if (Convert.ToBoolean(infoEnc[0]))
                {
                    DataSet encabezadoFac = (DataSet)infoEnc[1];
                    DataTable dtEncabezado = encabezadoFac.Tables[0];
                    if (dtEncabezado.Rows.Count > 0)
                    {
                        object[] encabezado = dtEncabezado.Rows[0].ItemArray;
                        obtieneDetalleFactura();
                        if (Convert.ToBoolean(infoDet[0]))
                        {
                            DataSet detalleFac = (DataSet)infoDet[1];
                            DataTable dtDetalle = detalleFac.Tables[0];
                            if (dtDetalle.Rows.Count > 0)
                            {
                                cfdv32.Comprobante comprobante = new cfdv32.Comprobante();
                                comprobante = generaComprobante(encabezado, dtDetalle, false, null);
                                ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + encabezado[0].ToString().Trim().ToUpper() + "/" + encabezado[1].ToString().Trim().ToUpper() + "/");

                                if (!Directory.Exists(ruta))
                                    Directory.CreateDirectory(ruta);
                                /*
                                if (conAddenda)
                                {
                                    XmlElement[] addendas = new XmlElement[1];
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(generaAddendaQ(dtEncabezado, dtDetalle));
                                    XmlElement add = doc.DocumentElement;

                                    addendas[0] = add;

                                    cfdv32.ComprobanteAddenda adden = new cfdv32.ComprobanteAddenda();
                                    adden.Any = addendas;
                                    comprobante.Addenda = adden;
                                }
                                */
                                XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
                                xmlNameSpace.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
                                //xmlNameSpace.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");                                
                                //xmlNameSpace.Add("schemaLocation", "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd");

                                string archivo = ruta + Convert.ToString(encabezado[16]);
                                FileInfo file = new FileInfo(archivo + ".xml");

                                XmlTextWriter xmlTextWriter = new XmlTextWriter(archivo + ".xml", Encoding.UTF8);
                                xmlTextWriter.Formatting = Formatting.Indented;
                                XmlSerializer xs = new XmlSerializer(typeof(cfdv32.Comprobante));

                                xs.Serialize(xmlTextWriter, comprobante, xmlNameSpace);
                                xmlTextWriter.Close();



                                Emisores emisor = new Emisores();
                                emisor.existeEmisor(encabezado[0].ToString().Trim().ToUpper());
                                int idEmisorComprobante = 0;
                                try
                                {
                                    if (Convert.ToBoolean(emisor.info[0]))
                                        idEmisorComprobante = Convert.ToInt32(emisor.info[1]);
                                }
                                catch (Exception) { idEmisorComprobante = 0; }
                                if (idEmisorComprobante != 0)
                                {
                                    //Parametros de Facturacion
                                    ParametrosFacturacion parametrosFactura = new ParametrosFacturacion();
                                    object[] infoParametrosFac = null;
                                    parametrosFactura.id_empresa = idEmisorComprobante;
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

                                    if (infoParametrosFac != null)
                                    {
                                        //Genera Cadena

                                        string rutaXlst = HttpContext.Current.Server.MapPath("~/cadenaoriginal_3_2.xslt");
                                        string rutaXml = archivo + ".xml";
                                        string rutaCadena = archivo + ".txt";
                                        string rutaRequisitos = HttpContext.Current.Server.MapPath("~/RequisitosFiscales");


                                        //Metodo 1 para cadena original
                                        /*XslCompiledTransform transformador = new XslCompiledTransform();
                                        transformador.Load(rutaXlst);
                                        transformador.Transform(rutaXml, rutaCadena);*/


                                        //Metodo 2 para cadena original
                                        //Cargar el XML
                                        StreamReader reader = new StreamReader(rutaXml);
                                        XPathDocument myXPathDoc = new XPathDocument(reader);

                                        //Cargando el XSLT
                                        XslCompiledTransform myXslTrans = new XslCompiledTransform();
                                        myXslTrans.Load(rutaXlst);

                                        StringWriter str = new StringWriter();
                                        XmlTextWriter myWriter = new XmlTextWriter(str);

                                        //Aplicando transformacion
                                        myXslTrans.Transform(myXPathDoc, null, myWriter);

                                        //Resultado
                                        string cadOrig = str.ToString();



                                        // Sella documento

                                        ProcesamientoCertificados procCert = new ProcesamientoCertificados();
                                        object[] sellado = procCert.xmlSellado(cadOrig, rutaXml, rutaRequisitos + "\\" + Convert.ToString(infoParametrosFac[2]), rutaRequisitos + "\\" + Convert.ToString(infoParametrosFac[3]), rutaRequisitos + "\\" + Convert.ToString(infoParametrosFac[4]), Convert.ToString(infoParametrosFac[5]));
                                        if (Convert.ToBoolean(sellado[0]))
                                        {

                                            XmlDocument xmlDoc = new XmlDocument();
                                            xmlDoc.Load(rutaXml);
                                            string xml = xmlDoc.OuterXml;
                                            FacturacionElectronica.Timbrado timbrar = new FacturacionElectronica.Timbrado();
                                            timbrar.rfc = encabezado[0].ToString().Trim().ToUpper();
                                            timbrar.usuario = Convert.ToString(infoParametrosFac[8]);
                                            timbrar.password = Convert.ToString(infoParametrosFac[9]);
                                            timbrar.xmlTimbrar = xml;
                                            timbrar.urlDocumento = rutaXml;
                                            timbrar.documento = idCfd;
                                            timbrar.rfcReceptor = encabezado[1].ToString().Trim().ToUpper();
                                            timbrar.certificadoEmisor = Convert.ToString(sellado[2]);
                                            timbrar.timbrar();
                                            object[] returnTimbrado = timbrar.info;
                                            if (Convert.ToBoolean(returnTimbrado[0]))
                                            {
                                                erroresXml = Convert.ToInt32(returnTimbrado[1]).ToString();
                                                info[0] = true;
                                                info[1] = erroresXml;
                                            }
                                            else
                                            {
                                                erroresXml = Convert.ToString(returnTimbrado[1]);
                                                info[0] = false;
                                                info[1] = erroresXml;
                                            }
                                        }
                                        else
                                        {
                                            erroresXml = "Error al sellar el documento. Detalle: " + Convert.ToString(sellado[1]);
                                            info[0] = false;
                                            info[1] = erroresXml;
                                        }


                                        //Codigo a eliminar  Emulador de factura
                                        /*XmlDocument xmlDoc = new XmlDocument();
                                        xmlDoc.Load(rutaXml);
                                        string xml = xmlDoc.OuterXml;
                                        FacturacionElectronica.Timbrado timbrar = new FacturacionElectronica.Timbrado();
                                        timbrar.rfc = encabezado[0].ToString().Trim().ToUpper();
                                        timbrar.usuario = Convert.ToString(infoParametrosFac[8]);
                                        timbrar.password = Convert.ToString(infoParametrosFac[9]);
                                        timbrar.xmlTimbrar = xml;
                                        timbrar.urlDocumento = rutaXml;
                                        timbrar.documento = idCfd;
                                        timbrar.rfcReceptor = encabezado[1].ToString().Trim().ToUpper();
                                        timbrar.timbrar();
                                        object[] returnTimbrado = timbrar.info;
                                        if (Convert.ToBoolean(returnTimbrado[0]))
                                        {
                                            erroresXml = Convert.ToInt32(returnTimbrado[1]).ToString();
                                            info[0] = true;
                                            info[1] = erroresXml;
                                        }
                                        else
                                        {
                                            erroresXml = Convert.ToString(returnTimbrado[1]);
                                            info[0] = false;
                                            info[1] = erroresXml;
                                        }*/

                                    }
                                    else
                                    {
                                        erroresXml = "Error al generar el documento, la información de parametros de facturación no es correcta o no se encuentra actualizada";
                                        info[0] = false;
                                        info[1] = erroresXml;
                                    }
                                }
                                else
                                {
                                    erroresXml = "Error al generar el documento, el emisor no fue encontrado";
                                    info[0] = false;
                                    info[1] = erroresXml;
                                }
                            }
                            else
                            {
                                erroresXml = "Error: El documento no cuenta con conceptos, proceda a capturar al menos un concepto.";
                                info[0] = false;
                                info[1] = erroresXml;
                            }
                        }
                        else
                        {
                            erroresXml = "Error al obtener los datos de encabezado del documento. Detalle: " + Convert.ToString(infoDet[1]);
                            info[0] = false;
                            info[1] = erroresXml;
                        }
                    }
                    else
                    {
                        erroresXml = "Error: El documento no cuenta con datos de encabezado, por favor proceda a generar un documento nuevo.";
                        info[0] = false;
                        info[1] = erroresXml;
                    }
                }
                else
                {
                    erroresXml = "Error: No fue posible obtener el encabezado del documento debido a que se produjó un error de conexión o de sintáxis al obtener la información. " + Convert.ToString(infoEnc[1]);
                    info[0] = false;
                    info[1] = erroresXml;
                }
            }
            catch (Exception ex)
            {
                erroresXml = "Error: " + ex.Message;
                info[0] = false;
                info[1] = erroresXml;
            }
        }


        public void actualizaTimbrado(string certificado, string fecha, string uuid, string selloSat, string selloCFD, byte[] qr, string ruta, string cadena, string certificadoCfd, string certificadoEmisor)
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("insert into Timbrado values({0},(select isnull((select top 1 idtimbre from timbrado_f where idCfd={0} order by idtimbre desc),0)+1),'{1}','{2}','{3}','{4}','{5}',@imagen,'{6}','{7}','{8}') " +
            "update recepcion_pagos_f set EncFolioUUID='{3}', EncSello='{5}',EncCertificado='{1}',EncTimbre='{4}', encestatus='T',certificado='{9}' where IdCfd={0}",
                idCfd, certificado, fecha, uuid, selloSat, selloCFD, ruta, cadena, certificadoCfd, certificadoEmisor);
            infoEnc = ejecuta.insertAdjuntos(sql, qr);
            info = infoEnc;
        }

        public string generaAddendaQ(DataTable encabezado, DataTable detalle, DataTable folios, string tipo, string periodo)
        {
            string addend = "";
            const string quote = "\"";
            try
            {
                object[] cfdiEncabezado = encabezado.Rows[0].ItemArray;
                string[] referncia = Convert.ToString(cfdiEncabezado[53]).Split(new char[] { '-' });

                Convertidores conversores = new Convertidores();
                conversores._importe = Convert.ToString(cfdiEncabezado[11]);
                string totalLetras = conversores.convierteMontoEnLetras();

                AddendaQualitas addendaQ = new AddendaQualitas();
                addendaQ.id = 1;
                addendaQ.obtieneParametros();
                object[] infoAdd = addendaQ.info;
                DataSet datosAddenda = null;
                if (Convert.ToBoolean(infoAdd[0]))
                    datosAddenda = (DataSet)infoAdd[1];

                object[] parametros = datosAddenda.Tables[0].Rows[0].ItemArray;

                int empresa = 0;
                int taller = 0;
                int orden = 0;
                string datoOrden = Convert.ToString(cfdiEncabezado[60]);
                try
                {
                    string[] argumentos = datoOrden.Split(new char[] { '-' });
                    //Empresa
                    empresa = Convert.ToInt32(Regex.Replace(argumentos[0], @"[^\d]", "")); //Convert.ToInt32(argumentos[0].Substring(2, argumentos[0].Length-1));                    
                    //Taller
                    taller = Convert.ToInt32(Regex.Replace(argumentos[1], @"[^\d]", ""));
                    //Orden
                    orden = Convert.ToInt32(Regex.Replace(argumentos[3], @"[^\d]", ""));
                }
                catch (Exception ex)
                {
                    empresa = taller = orden = 0;
                    //empresa = taller = 1;
                    //orden = 160018;
                }

                if (orden == 0 || empresa == 0 || taller == 0) { addend = ""; }
                else
                {
                    object[] ordenVehiculo = null;
                    Recepciones recepcion = new Recepciones();
                    object[] infoOrdenVehiculo = recepcion.obtieneInfoOrdenAddendaQualitas(empresa, taller, orden);
                    if (Convert.ToBoolean(infoOrdenVehiculo[0]))
                    {
                        DataSet dInfoVehi = (DataSet)infoOrdenVehiculo[1];
                        foreach (DataRow filaVehi in dInfoVehi.Tables[0].Rows)
                        {
                            ordenVehiculo = filaVehi.ItemArray;
                            break;
                        }
                    }
                    if (ordenVehiculo != null)
                    {
                        string anio = periodo.ToString().Substring(2, 2);
                        string valorNumero, valorReporte, valorSiniestro;
                        valorNumero = Convert.ToString(ordenVehiculo[0]).Trim();
                        valorReporte = Convert.ToString(ordenVehiculo[1]).Trim();
                        valorSiniestro = Convert.ToString(ordenVehiculo[3]).Trim();
                        string numero = "";
                        try { numero = valorNumero.Substring(0, 10); } catch (Exception) { numero = valorNumero; }
                        string reporte = "";
                        try { reporte = ("04" + anio + valorReporte).Substring(0, 11); } catch (Exception) { reporte = ("04" + anio + valorReporte); }
                        string sinies = "";
                        try { sinies = ("04" + anio + valorSiniestro).Substring(0, 11); } catch (Exception) { sinies = ("04" + anio + valorSiniestro); }
                        addend = "<ECFD version=" + quote + "1.0" + quote + ">" +
                                    "<Documento ID=" + quote + "T33CFDI" + referncia[1].Trim() + quote + ">" +
                                        "<Encabezado>" +
                                            "<IdDoc>" +
                                                "<NroAprob>00000</NroAprob>" +
                                                "<AnoAprob>0000</AnoAprob>" +
                                                "<Tipo>33</Tipo>" +
                                                "<Serie>0</Serie>" +
                                                "<Folio>" + referncia[1].Trim() + "</Folio>" +
                                                "<Estado>ORIGINAL</Estado>" +
                                                "<NumeroInterno>" + Convert.ToString(parametros[1]).Trim().PadLeft(2, '0') + "</NumeroInterno>" +
                                                "<FechaEmis>" + Convert.ToDateTime(cfdiEncabezado[54]).ToString("yyyy-MM-dd") + "T" + Convert.ToDateTime(cfdiEncabezado[55]).ToString("HH:mm:ss") + "</FechaEmis>" +
                                                "<FormaPago>" + validaCadenas(Convert.ToString(cfdiEncabezado[5])).Trim() + "</FormaPago>" +
                                                "<Area>" +
                                                    "<IdArea>" + Convert.ToString(parametros[2]).Trim().PadLeft(3, '0') + "</IdArea>" +
                                                    "<IdRevision>" + Convert.ToString(parametros[3]).Trim().PadLeft(3, '0') + "</IdRevision>" +
                                                "</Area>" +
                                            "</IdDoc>" +
                                            "<ExEmisor>" +
                                                "<RFCEmisor>" + validaCadenas(Convert.ToString(cfdiEncabezado[0])).Trim() + "</RFCEmisor>" +
                                                "<NmbEmisor>" + validaCadenas(Convert.ToString(cfdiEncabezado[18])).Trim() + "</NmbEmisor>" +
                                                "<CodigoExEmisor>" +
                                                    "<TpoCdgIntEmisor>EXT</TpoCdgIntEmisor>" +
                                                    "<CdgIntEmisor>" + Convert.ToString(parametros[4]).Trim().PadLeft(5, '0') + "</CdgIntEmisor>" +
                                                "</CodigoExEmisor>" +
                                                "<DomFiscal>" +
                                                    "<Calle>" + validaCadenas(Convert.ToString(cfdiEncabezado[19])).Trim() + "</Calle>" +
                                                    "<NroExterior>" + validaCadenas(Convert.ToString(cfdiEncabezado[20])).Trim() + "</NroExterior>";

                        if (validaCadenas(Convert.ToString(cfdiEncabezado[21])).Trim() != "")
                            addend = addend + "<NroInterior>" + validaCadenas(Convert.ToString(cfdiEncabezado[21])).Trim() + "</NroInterior>";


                        addend = addend + "<Colonia>" + validaCadenas(Convert.ToString(cfdiEncabezado[22])).Trim() + "</Colonia>" +
                            "<Municipio>" + validaCadenas(Convert.ToString(cfdiEncabezado[25])).Trim() + "</Municipio>" +
                            "<Estado>" + validaCadenas(Convert.ToString(cfdiEncabezado[26])).Trim() + "</Estado>" +
                            "<Pais>" + validaCadenas(Convert.ToString(cfdiEncabezado[27])).Trim() + "</Pais>" +
                            "<CodigoPostal>" + validaCadenas(Convert.ToString(cfdiEncabezado[28])).Trim().PadLeft(5, '0') + "</CodigoPostal>" +
                        "</DomFiscal>" +
                        "<LugarExped>" +
                            "<Calle>" + validaCadenas(Convert.ToString(cfdiEncabezado[29])).Trim() + "</Calle>" +
                            "<NroExterior>" + validaCadenas(Convert.ToString(cfdiEncabezado[30])).Trim() + "</NroExterior>";

                        if (validaCadenas(Convert.ToString(cfdiEncabezado[31])).Trim() != "")
                            addend = addend + "<NroInterior>" + validaCadenas(Convert.ToString(cfdiEncabezado[31])).Trim() + "</NroInterior>";

                        addend = addend + "<Colonia>" + validaCadenas(Convert.ToString(cfdiEncabezado[32])).Trim() + "</Colonia>" +
                            "<Municipio>" + validaCadenas(Convert.ToString(cfdiEncabezado[35])).Trim() + "</Municipio>" +
                            "<Estado>" + validaCadenas(Convert.ToString(cfdiEncabezado[36])).Trim() + "</Estado>" +
                            "<Pais>" + validaCadenas(Convert.ToString(cfdiEncabezado[37])).Trim() + "</Pais>" +
                            "<CodigoPostal>" + validaCadenas(Convert.ToString(cfdiEncabezado[38])).Trim().PadLeft(5, '0') + "</CodigoPostal>" +
                        "</LugarExped>" +
                        "<ContactoEmisor>" +
                            "<Tipo>" + validaCadenas(Convert.ToString(parametros[5])).Trim().ToLower() + "</Tipo>" +
                            "<Nombre>" + validaCadenas(Convert.ToString(parametros[6])).Trim() + "</Nombre>" +
                            "<eMail>" + validaCadenas(Convert.ToString(parametros[7])).Trim() + "</eMail>" +
                            "<Telefono>" + validaCadenas(Convert.ToString(parametros[8])).Trim() + "</Telefono>" +
                        "</ContactoEmisor>" +
                    "</ExEmisor>" +
                    "<ExReceptor>" +
                        "<RFCRecep>" + validaCadenas(Convert.ToString(cfdiEncabezado[1])).Trim() + "</RFCRecep>" +
                        "<NmbRecep>" + validaCadenas(Convert.ToString(cfdiEncabezado[40])).Trim() + "</NmbRecep>" +
                        "<CodigoExReceptor>" +
                            "<TpoCdgIntRecep>INT</TpoCdgIntRecep>" +
                            "<CdgIntRecep>" + validaCadenas(Convert.ToString(parametros[14])).Trim() + "</CdgIntRecep>" +
                        "</CodigoExReceptor>" +
                        "<DomFiscalRcp>" +
                            "<Calle>" + validaCadenas(Convert.ToString(cfdiEncabezado[41])).Trim() + "</Calle>" +
                            "<NroExterior>" + validaCadenas(Convert.ToString(cfdiEncabezado[42])).Trim() + "</NroExterior>";

                        if (validaCadenas(Convert.ToString(cfdiEncabezado[43])).Trim() != "")
                            addend = addend + "<NroInterior>" + validaCadenas(Convert.ToString(cfdiEncabezado[43])).Trim() + "</NroInterior>";

                        addend = addend + "<Colonia>" + validaCadenas(Convert.ToString(cfdiEncabezado[44])).Trim() + "</Colonia>" +
                            "<Municipio>" + validaCadenas(Convert.ToString(cfdiEncabezado[47])).Trim() + "</Municipio>" +
                            "<Estado>" + validaCadenas(Convert.ToString(cfdiEncabezado[48])).Trim() + "</Estado>" +
                            "<Pais>" + validaCadenas(Convert.ToString(cfdiEncabezado[49])).Trim() + "</Pais>" +
                            "<CodigoPostal>" + validaCadenas(Convert.ToString(cfdiEncabezado[50])).Trim().PadLeft(5, '0') + "</CodigoPostal>" +
                        "</DomFiscalRcp>" +
                        "<LugarRecep>" +
                            "<Calle>" + validaCadenas(Convert.ToString(cfdiEncabezado[41])).Trim() + "</Calle>" +
                            "<NroExterior>" + validaCadenas(Convert.ToString(cfdiEncabezado[42])).Trim() + "</NroExterior>";

                        if (validaCadenas(Convert.ToString(cfdiEncabezado[43])).Trim() != "")
                            addend = addend + "<NroInterior>" + validaCadenas(Convert.ToString(cfdiEncabezado[43])).Trim() + "</NroInterior>";

                        addend = addend + "<Colonia>" + validaCadenas(Convert.ToString(cfdiEncabezado[44])).Trim() + "</Colonia>" +
                            "<Municipio>" + validaCadenas(Convert.ToString(cfdiEncabezado[47])).Trim() + "</Municipio>" +
                            "<Estado>" + validaCadenas(Convert.ToString(cfdiEncabezado[48])).Trim() + "</Estado>" +
                            "<Pais>" + validaCadenas(Convert.ToString(cfdiEncabezado[49])).Trim() + "</Pais>" +
                            "<CodigoPostal>" + validaCadenas(Convert.ToString(cfdiEncabezado[50])).Trim().PadLeft(5, '0') + "</CodigoPostal>" +
                        "</LugarRecep>" +
                        "<ContactoReceptor>" +
                            "<Tipo>" + validaCadenas(Convert.ToString(parametros[9])).Trim().ToLower() + "</Tipo>" +
                            "<Nombre>" + validaCadenas(Convert.ToString(parametros[10])).Trim() + "</Nombre>" +
                            "<eMail>" + validaCadenas(Convert.ToString(parametros[11])).Trim() + "</eMail>" +
                            "<Telefono>" + validaCadenas(Convert.ToString(parametros[12])).Trim() + "</Telefono>" +
                        "</ContactoReceptor>" +
                    "</ExReceptor>" +
                    "<Totales>" +
                        "<Moneda>" + validaCadenas(Convert.ToString(cfdiEncabezado[56])).Trim() + "</Moneda>" +
                        /*
                        "<IndLista></IndLista>" +
                        "<TipoLista></TipoLista>" +
                        */
                        "<SubTotal>" + Convert.ToDecimal(Convert.ToString(cfdiEncabezado[7])).ToString("F2") + "</SubTotal>" +
                        "<MntDcto>" + Convert.ToDecimal(Convert.ToString(cfdiEncabezado[8])).ToString("F2") + "</MntDcto>" +
                        "<PctDcto>" + Convert.ToDecimal(Convert.ToString(cfdiEncabezado[57])).ToString("F2") + "</PctDcto>" +
                        "<MntBase>" + (Convert.ToDecimal(Convert.ToString(cfdiEncabezado[7])) - Convert.ToDecimal(Convert.ToString(cfdiEncabezado[8]))).ToString("F2") + "</MntBase>" +
                        "<MntImp>" + Convert.ToDecimal(Convert.ToString(cfdiEncabezado[52])).ToString("F2") + "</MntImp>" +
                        "<VlrPagar>" + Convert.ToDecimal(Convert.ToString(cfdiEncabezado[11])).ToString("F2") + "</VlrPagar>" +
                        "<VlrPalabras>" + totalLetras.ToUpper().Trim() + "</VlrPalabras>" +
                    "</Totales>" +
                    "<ExImpuestos>" +
                        "<TipoImp>" + Convert.ToString(cfdiEncabezado[58]) + "</TipoImp>" +
                        "<TasaImp>" + Convert.ToInt32(Convert.ToString(cfdiEncabezado[59])).ToString().PadLeft(2, '0') + "</TasaImp>" +
                        "<MontoImp>" + Convert.ToDecimal(Convert.ToString(cfdiEncabezado[52])).ToString("F2") + "</MontoImp>" +
                    "</ExImpuestos>" +
                    "<Poliza>" +
                        "<Tipo>autos</Tipo>" +
                        "<Numero>" + numero.Trim().PadLeft(10, '0') + "</Numero>" +
                        "<INC>0000</INC>" +
                        "<TpoCliente>" + Convert.ToString(parametros[23]).Trim().PadLeft(1, '0') + "</TpoCliente>" +
                        "<NroReporte>" + reporte.Trim().PadLeft(11, '0') + "</NroReporte>" +
                        "<NroSint>" + sinies.Trim().PadLeft(11, '0') + "</NroSint>" +
                    "</Poliza>" +
                    "<Vehiculo>" +
                        "<Tipo>PARTICULAR</Tipo>" +
                        "<Marca>" + Convert.ToString(ordenVehiculo[4]).Trim() + "</Marca>" +
                        "<Modelo>" + Convert.ToString(ordenVehiculo[5]).Trim() + "</Modelo>" +
                        "<Ano>" + Convert.ToString(ordenVehiculo[6]).Trim().PadLeft(4, '0') + "</Ano>" +
                        "<Color>" + Convert.ToString(ordenVehiculo[8]).Trim() + "</Color>" +
                        "<NroSerie>" + Convert.ToString(ordenVehiculo[9]).Trim() + "</NroSerie>" +
                        "<Placa>" + Convert.ToString(ordenVehiculo[7]).Trim() + "</Placa>" +
                    "</Vehiculo>" +
                "</Encabezado>";

                        decimal montoMo = 0;
                        decimal motnoRef = 0;

                        int cont = 1;
                        foreach (DataRow fila in detalle.Rows)
                        {

                            Unidades unidad = new Unidades();
                            unidad.idUnidad = Convert.ToInt32(fila[4]);
                            unidad.obtieneDescripcionUnidad();
                            string unidadConcepto = "";
                            if (Convert.ToBoolean(unidad.info[0]))
                                unidadConcepto = Convert.ToString(unidad.info[1]);

                            try
                            {
                                string[] split = Convert.ToString(fila[3]).Trim().Split(new char[] { '-' });
                                if (split[0].Substring(0, 2) == "MO")
                                    montoMo = montoMo + Convert.ToDecimal(fila[19]);
                                else if (split[0].Substring(0, 3) == "REF")
                                    motnoRef = motnoRef + Convert.ToDecimal(fila[19]);
                            }
                            catch (Exception) { }

                            //Foreach por cada concepto
                            addend = addend + "<Detalle>" +
                                 "<NroLinDet>" + cont.ToString() + "</NroLinDet>" +
                                 "<CdgItem>" +
                                     "<TpoCodigo>INT</TpoCodigo>" +
                                     "<VlrCodigo>" + Convert.ToString(fila[3]).Trim() + "</VlrCodigo>" +
                                 "</CdgItem>" +
                                 "<DscLang>ES</DscLang>" +
                                 "<DscItem>" + Convert.ToString(fila[21]).Trim() + "</DscItem>" +
                                 "<QtyItem>" + Convert.ToDecimal(Convert.ToString(fila[5]).Trim()).ToString("F2") + "</QtyItem>" +
                                 "<UnmdItem>" + unidadConcepto.Trim() + "</UnmdItem>" +
                                 "<PrcNetoItem>" + Convert.ToDecimal(fila[6]).ToString("F2") + "</PrcNetoItem>" +
                                 "<MontoNetoItem>" + Convert.ToDecimal(fila[19]).ToString("F2") + "</MontoNetoItem>" +
                             "</Detalle>";
                            cont++;
                        }
                        addend = addend + "<Referencia>" +
                             "<NroLinRef>1</NroLinRef>" +
                             "<TpoDocRef>FE</TpoDocRef>" +
                             "<SerieRef>0</SerieRef>" +
                             "<FolioRef>" + referncia[1].Trim() + "</FolioRef>" +
                             "<RazonRef>Esta factura sustituye a esta factura</RazonRef>" +
                         "</Referencia>" +
                         "<TimeStamp>" + Convert.ToDateTime(cfdiEncabezado[54]).ToString("yyyy-MM-dd") + "T" + Convert.ToDateTime(cfdiEncabezado[55]).ToString("HH:mm:ss") + "</TimeStamp>" +
                     "</Documento>" +
                     "<Personalizados>";
                        string montoManoObraString = "";
                        if (tipo == "M" || tipo == "G") montoManoObraString = montoMo.ToString("F2"); else montoManoObraString = "0.00";
                        string montoRefString = "";
                        if (tipo == "R" || tipo == "G") montoRefString = motnoRef.ToString("F2"); else montoRefString = "0.00";

                        addend = addend + "<campoString name=\"montoManoObra\">" + montoManoObraString + "</campoString>" +
                         "<campoString name=\"montoRefacciones\">" + montoRefString + "</campoString>" +
                         "<campoString name=\"fechaFiniquito\">" + Convert.ToDateTime(cfdiEncabezado[54]).ToString("yyyy-MM-dd") + "T" + Convert.ToDateTime(cfdiEncabezado[55]).ToString("HH:mm:ss") + "</campoString>" +
                         "<campoString name=\"fechaEntregaRefacciones\">" + Convert.ToDateTime(cfdiEncabezado[54]).ToString("yyyy-MM-dd") + "T" + Convert.ToDateTime(cfdiEncabezado[55]).ToString("HH:mm:ss") + "</campoString>" +
                         "<campoString name=\"oficinaEntregaFactura\">" + Convert.ToString(parametros[13]).Trim().PadLeft(3, '0') + "</campoString>" +
                         "<campoString name=\"UUID\">" + Convert.ToString(cfdiEncabezado[61]) + "</campoString>";

                        foreach (DataRow r in folios.Rows)
                        {
                            addend = addend + "<campoString name =\"folioElectronico\">" + r[1].ToString().Trim() + "</campoString>";
                        }

                        string[] montos = new string[2] { "0.00", "0.00" };
                        try { montos[0] = Convert.ToDecimal(parametros[15]).ToString("F2"); } catch (Exception) { montos[0] = "0.00"; }
                        try { montos[1] = Convert.ToDecimal(parametros[19]).ToString("F2"); } catch (Exception) { montos[1] = "0.00"; }

                        addend = addend + "<campoString name=\"folioElectronico\">" + Convert.ToString(ordenVehiculo[10]).Trim().PadLeft(12, '0') + "</campoString>" +

                         "<campoString name=\"montoDeducible\">" + montos[0] + "</campoString>" +
                         "<campoString name=\"bancoDepositoDeducible\">" + Convert.ToString(parametros[16]).Trim() + "</campoString>" +
                         "<campoString name=\"fechaDepositoDeducible\">" + Convert.ToString(parametros[17]).Trim() + "</campoString>" +
                         "<campoString name=\"folioFicha_ReferenciaDeducible\">" + Convert.ToString(parametros[18]).Trim() + "</campoString>" +
                         "<campoString name=\"montoDemerito_Recupero\">" + montos[1] + "</campoString>" +
                         "<campoString name=\"bancoDepositoDemerito_Recupero\">" + Convert.ToString(parametros[20]).Trim() + "</campoString>" +
                         "<campoString name=\"fechaDepositoDemerito_Recupero\">" + Convert.ToString(parametros[21]).Trim() + "</campoString>" +
                         "<campoString name=\"folioFicha_ReferenciaDemerito\">" + Convert.ToString(parametros[22]).Trim() + "</campoString>" +
                         "<campoString name=\"numeroTramite\">0</campoString>" +
                         "<campoString name=\"Default1\">string</campoString>" +
                         "<campoString name=\"Default2\">string</campoString>" +
                     "</Personalizados>" +
                 "</ECFD>";
                    }
                }
            }
            catch (Exception ex) { addend = ""; }
            return addend;
        }

        private cfdv32.Comprobante generaComprobante(object[] encabezado, DataTable dtDetalle, bool docTimbrado, object[] timbre)
        {
            Unidades unidad = new Unidades();

            cfdv32.Comprobante comprobante = new cfdv32.Comprobante();
            try
            {
                comprobante.version = "3.2";
                comprobante.fecha = Convert.ToDateTime(Convert.ToDateTime(encabezado[54]).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(encabezado[55]).ToString("HH:mm:ss"));

                string lugarExpedicion = "";
                switch (Convert.ToString(encabezado[4]).ToLower().Trim())
                {
                    case "egreso":
                        comprobante.tipoDeComprobante = cfdv32.ComprobanteTipoDeComprobante.egreso;
                        break;
                    case "ingreso":
                        comprobante.tipoDeComprobante = cfdv32.ComprobanteTipoDeComprobante.ingreso;
                        break;
                    case "traslado":
                        comprobante.tipoDeComprobante = cfdv32.ComprobanteTipoDeComprobante.traslado;
                        break;
                    default:
                        break;
                }

                if (Convert.ToString(encabezado[66]) == "NC")
                    comprobante.tipoDeComprobante = cfdv32.ComprobanteTipoDeComprobante.egreso;

                if (docTimbrado)
                {
                    comprobante.sello = encabezado[62].ToString().Trim();
                    comprobante.noCertificado = encabezado[63].ToString().Trim();
                    comprobante.certificado = encabezado[64].ToString().Trim();// Convert.ToString(encabezado[64]).Trim();
                }
                else
                    comprobante.sello = comprobante.noCertificado = comprobante.certificado = "";


                comprobante.formaDePago = validaCadenas(Convert.ToString(encabezado[5]));
                if (Convert.ToString(encabezado[6]) != "")
                    comprobante.condicionesDePago = validaCadenas(Convert.ToString(encabezado[6]));

                comprobante.subTotal = Convert.ToDecimal(Convert.ToDecimal(encabezado[7]).ToString("F2"));
                decimal descuento = 0;
                try { descuento = Convert.ToDecimal(Convert.ToDecimal(encabezado[8]).ToString("F2")); } catch (Exception) { descuento = 0; }
                if (descuento != 0)
                {
                    comprobante.descuentoSpecified = true;
                    comprobante.descuento = descuento;
                }

                if (Convert.ToString(encabezado[9]) != "")
                    comprobante.TipoCambio = Convert.ToDecimal(encabezado[9]).ToString("F2");
                if (Convert.ToString(encabezado[10]) != "")
                    comprobante.Moneda = validaCadenas(Convert.ToString(encabezado[10]));
                comprobante.total = Convert.ToDecimal(Convert.ToDecimal(encabezado[11]).ToString("F2"));
                comprobante.metodoDePago = validaCadenas(Convert.ToString(encabezado[12]));
                if (Convert.ToString(encabezado[13]) != "")
                    comprobante.LugarExpedicion = validaCadenas(Convert.ToString(encabezado[13]));
                if (Convert.ToString(encabezado[14]) != "")
                    comprobante.NumCtaPago = validaCadenas(Convert.ToString(encabezado[14]));
                if (descuento != 0)
                {
                    if (Convert.ToString(encabezado[15]) != "")
                        comprobante.motivoDescuento = validaCadenas(Convert.ToString(encabezado[15]));
                    else
                        comprobante.motivoDescuento = "Descuento Global";
                }
                /*
                if (Convert.ToString(encabezado[16]) != "")
                {
                    string[] serieImp = validaCadenas(Convert.ToString(encabezado[53])).Split(new char[] { '-' });
                    string serieAgregar = "";
                    
                    try { serieAgregar = serieImp[1] ; } catch (Exception) { serieAgregar = validaCadenas(serieImp[0]); }
                    
                    comprobante.serie = validaCadenas(serieAgregar);
                }*/
                if (Convert.ToString(encabezado[17]) != "")
                {
                    string[] referncia = Convert.ToString(encabezado[53]).Split(new char[] { '-' });
                    try
                    {
                        if (referncia[1] != "")
                            comprobante.folio = validaCadenas(Convert.ToString(referncia[1]));
                    }
                    catch (Exception) { if (referncia[0] != "") comprobante.folio = validaCadenas(Convert.ToString(referncia[0])); }
                }

                //Emisor
                cfdv32.ComprobanteEmisor cfdiEmisor = new cfdv32.ComprobanteEmisor();
                cfdiEmisor.rfc = validaCadenas(Convert.ToString(encabezado[0])).Trim();
                if (Convert.ToString(encabezado[18]) != "")
                    cfdiEmisor.nombre = validaCadenas(Convert.ToString(encabezado[18])).Trim();

                cfdv32.t_UbicacionFiscal ubicacionEmisor = new cfdv32.t_UbicacionFiscal();
                ubicacionEmisor.calle = validaCadenas(Convert.ToString(encabezado[19]));
                ubicacionEmisor.noExterior = validaCadenas(Convert.ToString(encabezado[20]));
                if (validaCadenas(Convert.ToString(encabezado[21])) != "")
                    if (validaCadenas(Convert.ToString(encabezado[21])) != "0")
                        ubicacionEmisor.noInterior = validaCadenas(Convert.ToString(encabezado[21]));
                ubicacionEmisor.colonia = validaCadenas(Convert.ToString(encabezado[22]));
                if (Convert.ToString(encabezado[23]) != "")
                    ubicacionEmisor.localidad = validaCadenas(Convert.ToString(encabezado[23]));
                if (Convert.ToString(encabezado[24]) != "")
                    ubicacionEmisor.referencia = validaCadenas(Convert.ToString(encabezado[24]));
                ubicacionEmisor.municipio = validaCadenas(Convert.ToString(encabezado[25]));
                ubicacionEmisor.estado = validaCadenas(Convert.ToString(encabezado[26]));
                ubicacionEmisor.pais = validaCadenas(Convert.ToString(encabezado[27]));
                ubicacionEmisor.codigoPostal = validaCadenas(Convert.ToString(encabezado[28])).PadLeft(5, '0');
                cfdiEmisor.DomicilioFiscal = ubicacionEmisor;

                cfdv32.t_Ubicacion ubicacionExpedicion = new cfdv32.t_Ubicacion();
                ubicacionExpedicion.calle = validaCadenas(Convert.ToString(encabezado[29]));
                ubicacionExpedicion.noExterior = validaCadenas(Convert.ToString(encabezado[30]));
                if (validaCadenas(Convert.ToString(encabezado[31])) != "")
                    if (validaCadenas(Convert.ToString(encabezado[31])) != "0")
                        ubicacionExpedicion.noInterior = validaCadenas(Convert.ToString(encabezado[31]));
                ubicacionExpedicion.colonia = validaCadenas(Convert.ToString(encabezado[32]));
                if (Convert.ToString(encabezado[33]) != "")
                    ubicacionExpedicion.localidad = validaCadenas(Convert.ToString(encabezado[33]));
                if (Convert.ToString(encabezado[34]) != "")
                    ubicacionExpedicion.referencia = validaCadenas(Convert.ToString(encabezado[34]));
                ubicacionExpedicion.municipio = validaCadenas(Convert.ToString(encabezado[35]));
                ubicacionExpedicion.estado = validaCadenas(Convert.ToString(encabezado[36]));
                ubicacionExpedicion.pais = validaCadenas(Convert.ToString(encabezado[37]));
                ubicacionExpedicion.codigoPostal = validaCadenas(Convert.ToString(encabezado[38])).PadLeft(5, '0');
                cfdiEmisor.ExpedidoEn = ubicacionExpedicion;

                lugarExpedicion = lugarExpedicion.Trim() + validaCadenas(Convert.ToString(encabezado[35])) + ", " + validaCadenas(Convert.ToString(encabezado[36]));

                cfdv32.ComprobanteEmisorRegimenFiscal[] emisorRegimen = new cfdv32.ComprobanteEmisorRegimenFiscal[1];
                emisorRegimen[0] = new cfdv32.ComprobanteEmisorRegimenFiscal();
                emisorRegimen[0].Regimen = validaCadenas(Convert.ToString(encabezado[39]));

                comprobante.Emisor = new cfdv32.ComprobanteEmisor();
                comprobante.Emisor.nombre = cfdiEmisor.nombre;
                comprobante.Emisor.rfc = cfdiEmisor.rfc;
                comprobante.Emisor.DomicilioFiscal = cfdiEmisor.DomicilioFiscal;
                comprobante.Emisor.ExpedidoEn = cfdiEmisor.ExpedidoEn;
                comprobante.Emisor.RegimenFiscal = new cfdv32.ComprobanteEmisorRegimenFiscal[1];
                comprobante.Emisor.RegimenFiscal[0] = new cfdv32.ComprobanteEmisorRegimenFiscal();
                comprobante.Emisor.RegimenFiscal[0].Regimen = emisorRegimen[0].Regimen;

                //Receptor
                cfdv32.ComprobanteReceptor cfdiReceptor = new cfdv32.ComprobanteReceptor();
                cfdiReceptor.rfc = validaCadenas(Convert.ToString(encabezado[1]));
                cfdiReceptor.nombre = validaCadenas(Convert.ToString(encabezado[40]));

                cfdv32.t_Ubicacion ubicacionReceptor = new cfdv32.t_Ubicacion();
                ubicacionReceptor.calle = validaCadenas(Convert.ToString(encabezado[41]));
                ubicacionReceptor.noExterior = validaCadenas(Convert.ToString(encabezado[42]));
                if (validaCadenas(Convert.ToString(encabezado[43])) != "")
                    if (validaCadenas(Convert.ToString(encabezado[43])) != "0")
                        ubicacionReceptor.noInterior = validaCadenas(Convert.ToString(encabezado[43]));
                ubicacionReceptor.colonia = validaCadenas(Convert.ToString(encabezado[44]));
                if (Convert.ToString(encabezado[45]) != "")
                    ubicacionReceptor.localidad = validaCadenas(Convert.ToString(encabezado[45]));
                if (Convert.ToString(encabezado[46]) != "")
                    ubicacionReceptor.referencia = validaCadenas(Convert.ToString(encabezado[46]));
                ubicacionReceptor.municipio = validaCadenas(Convert.ToString(encabezado[47]));
                ubicacionReceptor.estado = validaCadenas(Convert.ToString(encabezado[48]));
                ubicacionReceptor.pais = validaCadenas(Convert.ToString(encabezado[49]));
                ubicacionReceptor.codigoPostal = validaCadenas(Convert.ToString(encabezado[50])).PadLeft(5, '0');
                cfdiReceptor.Domicilio = ubicacionReceptor;

                comprobante.Receptor = new cfdv32.ComprobanteReceptor();
                comprobante.Receptor.rfc = cfdiReceptor.rfc.Trim();
                comprobante.Receptor.nombre = cfdiReceptor.nombre.Trim();
                comprobante.Receptor.Domicilio = cfdiReceptor.Domicilio;

                cfdv32.ComprobanteConcepto[] conceptos = new cfdv32.ComprobanteConcepto[dtDetalle.Rows.Count];
                int totalConceptos = 0;
                int traslados = 0;
                int retenciones = 0;

                //Conceptos
                foreach (DataRow concepto in dtDetalle.Rows)
                {
                    conceptos[totalConceptos] = new cfdv32.ComprobanteConcepto();
                    conceptos[totalConceptos].cantidad = Convert.ToDecimal(Convert.ToDecimal(concepto[5]).ToString("F2"));
                    unidad.idUnidad = Convert.ToInt32(concepto[4]);
                    unidad.obtieneDescripcionUnidad();
                    object[] infoUnidad = unidad.info;
                    string descripcionUnidad = "No Definido";
                    if (Convert.ToBoolean(infoUnidad[0]))
                        descripcionUnidad = Convert.ToString(infoUnidad[1]);
                    conceptos[totalConceptos].unidad = validaCadenas(descripcionUnidad);
                    conceptos[totalConceptos].descripcion = validaCadenas(Convert.ToString(concepto[21]));
                    conceptos[totalConceptos].valorUnitario = Convert.ToDecimal(Convert.ToDecimal(concepto[6]).ToString("F2"));
                    conceptos[totalConceptos].importe = Convert.ToDecimal((Convert.ToDecimal(concepto[5]) * Convert.ToDecimal(concepto[6])).ToString("F2"));
                    if (Convert.ToString(concepto[3]) != "")
                        conceptos[totalConceptos].noIdentificacion = Convert.ToString(concepto[3]);

                    if (Convert.ToInt32(concepto[7]) != 0)
                        traslados++;
                    if (Convert.ToInt32(concepto[9]) != 0)
                        traslados++;
                    if (Convert.ToInt32(concepto[11]) != 0)
                        traslados++;
                    if (Convert.ToInt32(concepto[13]) != 0)
                        retenciones++;
                    if (Convert.ToInt32(concepto[15]) != 0)
                        retenciones++;

                    totalConceptos++;
                }

                comprobante.Conceptos = conceptos;

                //Impuestos
                decimal impRetenidos = Convert.ToDecimal(encabezado[51]);
                decimal impTrasladados = Convert.ToDecimal(encabezado[52]);

                cfdv32.ComprobanteImpuestos cfdiImpuestos = new cfdv32.ComprobanteImpuestos();
                if (retenciones != 0)
                {
                    cfdiImpuestos.totalImpuestosRetenidos = impRetenidos;
                    comprobante.Impuestos = new cfdv32.ComprobanteImpuestos();
                    comprobante.Impuestos.totalImpuestosRetenidosSpecified = true;
                    comprobante.Impuestos.totalImpuestosRetenidos = Convert.ToDecimal(cfdiImpuestos.totalImpuestosRetenidos.ToString("F2"));
                }
                if (traslados != 0)
                {
                    cfdiImpuestos.totalImpuestosTrasladados = impTrasladados;
                    comprobante.Impuestos = new cfdv32.ComprobanteImpuestos();
                    comprobante.Impuestos.totalImpuestosTrasladadosSpecified = true;
                    comprobante.Impuestos.totalImpuestosTrasladados = Convert.ToDecimal(cfdiImpuestos.totalImpuestosTrasladados.ToString("F2"));
                }

                //Retenciones
                cfdv32.ComprobanteImpuestosRetencion[] impuestosRetenidos = new cfdv32.ComprobanteImpuestosRetencion[retenciones];
                int contReten = 0;
                if (retenciones != 0)
                {
                    ImpRetenciones impRetenciones = new ImpRetenciones();
                    string impuesto = "";
                    foreach (DataRow reten in dtDetalle.Rows)
                    {
                        if (Convert.ToInt32(reten[13]) != 0)
                        {
                            impuestosRetenidos[contReten] = new cfdv32.ComprobanteImpuestosRetencion();
                            impRetenciones.idRetencion = Convert.ToInt32(reten[13]);
                            impRetenciones.obtieneImpuesto();
                            object[] imp = impRetenciones.info;
                            if (Convert.ToBoolean(imp[0]))
                                impuesto = Convert.ToString(imp[1]);

                            impuestosRetenidos[contReten].importe = Convert.ToDecimal(Convert.ToDecimal(reten[14]).ToString("F2"));
                            if (cfdv32.ComprobanteImpuestosRetencionImpuesto.ISR.ToString() == impuesto)
                                impuestosRetenidos[contReten].impuesto = cfdv32.ComprobanteImpuestosRetencionImpuesto.ISR;
                            else
                                impuestosRetenidos[contReten].impuesto = cfdv32.ComprobanteImpuestosRetencionImpuesto.IVA;
                            contReten++;
                        }
                        if (Convert.ToInt32(reten[15]) != 0)
                        {
                            impuestosRetenidos[contReten] = new cfdv32.ComprobanteImpuestosRetencion();
                            impRetenciones.idRetencion = Convert.ToInt32(reten[15]);
                            impRetenciones.obtieneImpuesto();
                            object[] imp = impRetenciones.info;
                            if (Convert.ToBoolean(imp[0]))
                                impuesto = Convert.ToString(imp[1]);

                            impuestosRetenidos[contReten].importe = Convert.ToDecimal(Convert.ToDecimal(reten[16]).ToString("F2"));
                            if (cfdv32.ComprobanteImpuestosRetencionImpuesto.ISR.ToString() == impuesto)
                                impuestosRetenidos[contReten].impuesto = cfdv32.ComprobanteImpuestosRetencionImpuesto.ISR;
                            else
                                impuestosRetenidos[contReten].impuesto = cfdv32.ComprobanteImpuestosRetencionImpuesto.IVA;
                            contReten++;
                        }
                    }
                    comprobante.Impuestos.Retenciones = impuestosRetenidos;
                }

                //Traslados
                cfdv32.ComprobanteImpuestosTraslado[] impuestosTrasladados = new cfdv32.ComprobanteImpuestosTraslado[traslados];
                int contTras = 0;
                if (traslados != 0)
                {
                    ImpTrasladados impTras = new ImpTrasladados();
                    string impuesto = "";
                    decimal tasa = 0;
                    foreach (DataRow tras in dtDetalle.Rows)
                    {
                        if (Convert.ToInt32(tras[7]) != 0)
                        {
                            impuestosTrasladados[contTras] = new cfdv32.ComprobanteImpuestosTraslado();
                            impTras.idTrasladado = Convert.ToInt32(tras[7]);
                            impTras.obtieneImpuesto();
                            object[] imp = impTras.info;
                            if (Convert.ToBoolean(imp[0]))
                                impuesto = Convert.ToString(imp[1]);
                            impTras.obtieneTasaImpuesto();
                            object[] impT = impTras.info;
                            if (Convert.ToBoolean(impT[0]))
                                tasa = Convert.ToDecimal(Convert.ToDecimal(impT[1]).ToString("F2"));

                            impuestosTrasladados[contTras].importe = Convert.ToDecimal(Convert.ToDecimal(tras[8]).ToString("F2"));
                            impuestosTrasladados[contTras].tasa = tasa;
                            if (cfdv32.ComprobanteImpuestosTrasladoImpuesto.IEPS.ToString() == impuesto)
                                impuestosTrasladados[contTras].impuesto = cfdv32.ComprobanteImpuestosTrasladoImpuesto.IEPS;
                            else
                                impuestosTrasladados[contTras].impuesto = cfdv32.ComprobanteImpuestosTrasladoImpuesto.IVA;
                            contTras++;
                        }
                        if (Convert.ToInt32(tras[9]) != 0)
                        {
                            impuestosTrasladados[contTras] = new cfdv32.ComprobanteImpuestosTraslado();
                            impTras.idTrasladado = Convert.ToInt32(tras[9]);
                            impTras.obtieneImpuesto();
                            object[] imp = impTras.info;
                            if (Convert.ToBoolean(imp[0]))
                                impuesto = Convert.ToString(imp[1]);
                            impTras.obtieneTasaImpuesto();
                            object[] impT = impTras.info;
                            if (Convert.ToBoolean(impT[0]))
                                tasa = Convert.ToDecimal(Convert.ToDecimal(impT[1]).ToString("F2"));

                            impuestosTrasladados[contTras].importe = Convert.ToDecimal(Convert.ToDecimal(tras[10]).ToString("F2"));
                            impuestosTrasladados[contTras].tasa = tasa;
                            if (cfdv32.ComprobanteImpuestosTrasladoImpuesto.IEPS.ToString() == impuesto)
                                impuestosTrasladados[contTras].impuesto = cfdv32.ComprobanteImpuestosTrasladoImpuesto.IEPS;
                            else
                                impuestosTrasladados[contTras].impuesto = cfdv32.ComprobanteImpuestosTrasladoImpuesto.IVA;
                            contTras++;
                        }
                        if (Convert.ToInt32(tras[11]) != 0)
                        {
                            impuestosTrasladados[contTras] = new cfdv32.ComprobanteImpuestosTraslado();
                            impTras.idTrasladado = Convert.ToInt32(tras[11]);
                            impTras.obtieneImpuesto();
                            object[] imp = impTras.info;
                            if (Convert.ToBoolean(imp[0]))
                                impuesto = Convert.ToString(imp[1]);
                            impTras.obtieneTasaImpuesto();
                            object[] impT = impTras.info;
                            if (Convert.ToBoolean(impT[0]))
                                tasa = Convert.ToDecimal(Convert.ToDecimal(impT[1]).ToString("F2"));

                            impuestosTrasladados[contTras].importe = Convert.ToDecimal(Convert.ToDecimal(tras[12]).ToString("F2"));
                            impuestosTrasladados[contTras].tasa = tasa;
                            if (cfdv32.ComprobanteImpuestosTrasladoImpuesto.IEPS.ToString() == impuesto)
                                impuestosTrasladados[contTras].impuesto = cfdv32.ComprobanteImpuestosTrasladoImpuesto.IEPS;
                            else
                                impuestosTrasladados[contTras].impuesto = cfdv32.ComprobanteImpuestosTrasladoImpuesto.IVA;
                            contTras++;
                        }
                    }
                    comprobante.Impuestos.Traslados = impuestosTrasladados;

                    //Timbre
                    if (docTimbrado)
                    {
                        XmlElement[] timbres = new XmlElement[1];
                        XmlDocument doc = new XmlDocument();
                        string tfdTimbrado = "";
                        tfdTimbrado = "<tfd:TimbreFiscalDigital xmlns:tfd=\"http://www.sat.gob.mx/TimbreFiscalDigital\"  version=\"1.0\" UUID=\"" + Convert.ToString(timbre[0]).Trim() + "\" FechaTimbrado=\"" + Convert.ToString(timbre[5]).Trim() + "\" selloCFD=\"" + Convert.ToString(timbre[7]).Trim() + "\" noCertificadoSAT=\"" + Convert.ToString(timbre[4]).Trim() + "\" selloSAT=\"" + Convert.ToString(timbre[6]).Trim() + "\"/>";
                        doc.LoadXml(tfdTimbrado);
                        XmlElement add = doc.DocumentElement;

                        XmlAttribute schemaLocation = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                        schemaLocation.Value = "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/TimbreFiscalDigital/TimbreFiscalDigital.xsd";
                        add.SetAttributeNode(schemaLocation);

                        //XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
                        //xmlNameSpace.Add("schemaLocation", "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/TimbreFiscalDigital/TimbreFiscalDigital.xsd");                        
                        timbres[0] = add;
                        cfdv32.ComprobanteComplemento complementoTombre = new cfdv32.ComprobanteComplemento();
                        complementoTombre.Any = timbres;
                        comprobante.Complemento = complementoTombre;
                    }

                }
            }
            catch (Exception ex) { }
            return comprobante;
        }

        private string validaCadenas(string cadena)
        {
            string cadenaCorregida = cadena.Trim();
            cadenaCorregida = cadenaCorregida.Replace("&", "&amp;");
            cadenaCorregida = cadenaCorregida.Replace("\"", "&quot;");
            cadenaCorregida = cadenaCorregida.Replace("<", "&lt;");
            cadenaCorregida = cadenaCorregida.Replace(">", "&gt;");
            cadenaCorregida = cadenaCorregida.Replace("'", "&apos;");
            cadenaCorregida = cadenaCorregida.Replace("|", "");
            cadenaCorregida = cadenaCorregida.Replace("  ", " ");
            cadenaCorregida = cadenaCorregida.Replace("\"", "");
            return cadenaCorregida;
        }

        public void actualizaFechaGeneracion(int documento, DateTime fechaGeneracion)
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("update recepcion_pagos_f set EncFechaGenera='{0}', EncHoraGenera='{1}' where IdCfd={2}", fechaGeneracion.ToString("yyyy-MM-dd"), fechaGeneracion.ToString("HH:mm:ss"), documento);
            info = ejecuta.insertUpdateDelete(sql);
        }

        public void cancelaDocumento()
        {
            try
            {
                obtieneDocumentoTimbrado();
                if (Convert.ToBoolean(info[0]))
                {
                    object[] informacionDocto = null;
                    DataSet datosDoc = (DataSet)info[1];
                    foreach (DataRow fila in datosDoc.Tables[0].Rows)
                    {
                        informacionDocto = fila.ItemArray;
                        break;
                    }
                    if (informacionDocto != null)
                    {
                        //GeneraCFD.GeneraCFD cancela = new GeneraCFD.GeneraCFD();
                        RVCFDI.GeneraCFDI cancela = new RVCFDI.GeneraCFDI();

                        string uuidCancelar = Convert.ToString(informacionDocto[0]);
                        string rfc = Convert.ToString(informacionDocto[2]);
                        string rutaXml = Convert.ToString(informacionDocto[1]);

                        //Parametros de Facturacion
                        ParametrosFacturacion parametrosFactura = new ParametrosFacturacion();
                        object[] infoParametrosFac = null;
                        parametrosFactura.id_empresa = Convert.ToInt32(Convert.ToString(informacionDocto[3]));
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
                        if (infoParametrosFac != null)
                        {
                            string UUID = uuidCancelar; //UUID del CFDI a cancelar.
                            string RFC_Emisor = rfc; //RFC del Emisor.                                                        
                            string Path_XMLCancelacion = rutaXml;//Ruta del XML a cancelar.
                            string Path_Key = Convert.ToString(infoParametrosFac[3]); //Ruta del archivo de llave privada.
                            string Path_Certificado = Convert.ToString(infoParametrosFac[2]); //Ruta del certificado de sello digital.
                            string ClaveKey = Convert.ToString(infoParametrosFac[5]); //Contraseña de la llave privada.
                            string Usuario_WS = Convert.ToString(infoParametrosFac[8]); //Usuario del Servicio Web.
                            string Contrasena_WS = Convert.ToString(infoParametrosFac[9]); //Contraseña del Servicio Web.
                            string URL_WS = "";
                            int enProduccion = Convert.ToInt32(ConfigurationManager.AppSettings["enProduccion"].ToString());
                            if (enProduccion == 0)
                                URL_WS = ConfigurationManager.AppSettings["urlTimbradoPrueba"].ToString();
                            else
                                URL_WS = ConfigurationManager.AppSettings["urlTimbrado"].ToString();
                            //Ruta del Servicio Web de cancelación.                            
                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(Path_XMLCancelacion);
                                string xml = doc.OuterXml;
                                bool cancelado = false;
                                try
                                {
                                    cancela.cancelar(xml, Usuario_WS, Contrasena_WS, URL_WS);
                                    cancelado = true;
                                }
                                catch (Exception ex) { cancelado = false; }
                                if (cancelado)
                                {
                                    //cancela.Cancelar_CFDI(UUID, RFC_Emisor, Path_XMLCancelacion, Path_Key, Path_Certificado, ClaveKey, Usuario_WS, Contrasena_WS, URL_WS);
                                    Facturas facturas = new Facturas();
                                    facturas.idCfd = idCfd;
                                    facturas.estatus = "C";
                                    facturas.actualizaEstatusFactura();
                                    object[] actualizado = facturas.info;

                                    info[0] = true;
                                    info[1] = true;
                                }
                                else
                                {
                                    info[0] = false;
                                    info[1] = "No se pudo cancelar el documento";
                                }
                            }
                            catch (Exception x)
                            {
                                info[0] = false;
                                info[1] = x.Message;
                            }
                        }
                        else
                        {
                            info[0] = false;
                            info[1] = "No se encontraron parametros de facturación o bien algunos datos están incorrectos";
                        }
                    }
                    else
                    {
                        Facturas facturas = new Facturas();
                        facturas.idCfd = idCfd;
                        facturas.estatus = "C";
                        facturas.actualizaEstatusFactura();
                        info = facturas.info;
                    }
                }
                else
                {
                    Facturas facturas = new Facturas();
                    facturas.idCfd = idCfd;
                    facturas.estatus = "C";
                    facturas.actualizaEstatusFactura();
                    info = facturas.info;
                }
            }
            catch (Exception ex)
            {
                info[0] = false;
                info[1] = ex.Message;
            }
        }

        public bool verificaDocumentoTimbrado(int documento)
        {
            bool timbrado = false;
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select COUNT(*) from recepcion_pagos_f where idcfd={0} and encestatus='T'", idCfd);
            object[] ret = ejecuta.scalarToInt(sql);
            try
            {
                if (Convert.ToBoolean(ret[0]))
                {
                    if (Convert.ToInt32(ret[1]) != 0)
                        timbrado = true;
                    else
                        timbrado = false;
                }
            }
            catch (Exception) { timbrado = false; }
            return timbrado;
        }

        public void obtieneDocumentoTimbrado()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select top 1 t.uuid,t.rutaArchivo,e.EncEmRFC,em.idemisor,t.noCertificadoSat,t.fechatimbrado,t.sellosat,t.sellocfd,t.cadenaOriginal,t.nocertificadocfd from Timbrado t inner join recepcion_pagos_f e on e.idcfd = t.idcfd inner join emisores em on em.emrfc=e.EncEmRFC where t.idcfd={0}", idCfd);
            info = ejecuta.dataSet(sql);
        }

        public void obtieneDocumentoTimbrado2(string uuid)
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select top 1 t.uuid,t.rutaArchivo,e.EncEmRFC,em.idemisor,t.noCertificadoSat,t.fechatimbrado,t.sellosat,t.sellocfd,t.cadenaOriginal,t.nocertificadocfd from Timbrado t inner join recepcion_pagos_f e on e.idcfd = t.idcfd inner join emisores em on em.emrfc=e.EncEmRFC where t.idcfd={0} and t.uuid='{1}'", idCfd, uuid);
            info = ejecuta.dataSet(sql);
        }

        internal void agregaDocumento(int documento, string xmlTimbrar, string archivoTimbrado)
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("insert into documentos values({0},(SELECT ISNULL((SELECT TOP 1 CONSECUTIVO FROM DOCUMENTOS WHERE IDCFD ={0} ORDER BY CONSECUTIVO DESC),0))+1,'{1}','{2}')", documento, xmlTimbrar, archivoTimbrado);
            info = ejecuta.insertUpdateDelete(sql);
        }

        internal void actualizaTimbradoPrb(string ruta)
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("insert into Timbrado select {0},idTimbre,noCertificadoSat,fechaTimbrado,uuid,selloSat,selloCFD,qr,'{1}',cadenaOriginal,noCertificadoCfd from timbrado where idcfd=11 " +
            "update recepcion_pagos_f set EncFolioUUID=(select top 1 uuid from timbrado where idcfd={0}), EncSello=(select top 1 selloCfd from timbrado where idcfd={0}),EncCertificado=(select top 1 noCertificadoSat from timbrado where idcfd={0}),EncTimbre=(select top 1 selloSat from timbrado where idcfd={0}), encestatus='T' where IdCfd={0}",
                idCfd, ruta);
            infoEnc = ejecuta.insertUpdateDelete(sql);
            info = infoEnc;
        }
    }

    public class CatalogosFacturacion
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public int id { get; set; }
        public string idPais { get; set; }
        public string idEstado { get; set; }
        public string idMunicipio { get; set; }
        public string idColonia { get; set; }
        public string descripcion { get; set; }
        public object[] info { get; set; }

        public void obtieneIdPais()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select cve_pais from paises_f where desc_pais='{0}'", descripcion);
            info = ejecuta.scalarToInt(sql);
        }

        public void obtieneIdEstado()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select cve_edo from estados_f where nom_edo='{0}' and cve_pais={1}", descripcion, idPais);
            info = ejecuta.scalarToInt(sql);
        }

        public void obtieneIdMunicipio()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select ID_Del_Mun from DelegacionMunicipio_f where Desc_Del_Mun='{0}' and cve_pais={1} and id_estado={2}", descripcion, idPais, idEstado);
            info = ejecuta.scalarToInt(sql);
        }

        public void obtieneIdColonia()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select id_colonia from Colonias_f where cve_pais={1} and ID_Estado={2} and ID_Del_Mun={3} and Desc_Colonia='{0}'", descripcion, idPais, idEstado, idMunicipio);
            info = ejecuta.scalarToInt(sql);
        }

        public void obtieneIdCp()
        {
            ejecuta.baseDatos = "PVW";
            sql = string.Format("select ID_COD_POS from Colonias_f where cve_pais={1} and ID_Estado={2} and ID_Del_Mun={3} and ID_Colonia={0}", idColonia, idPais, idEstado, idMunicipio);
            info = ejecuta.scalarToInt(sql);
        }
    }

    public class ProcesamientoCertificados
    {

        // obtiene sello
        private string obtieneSello(string cadenaO, string password, string pfx)
        {
            string sello = "";
            try
            {
                X509Certificate2 _MiCertificado = new X509Certificate2(pfx, password);
                RSACryptoServiceProvider RSA1 = (RSACryptoServiceProvider)_MiCertificado.PrivateKey;
                SHA1CryptoServiceProvider hasher = new SHA1CryptoServiceProvider();
                byte[] bytesFirmados = RSA1.SignData(System.Text.Encoding.UTF8.GetBytes(cadenaO), hasher);
                sello = Convert.ToBase64String(bytesFirmados);
            }
            catch { sello = ""; }
            return sello;
        }

        //genera e inserta sello en xml
        public object[] xmlSellado(string cadenaOriginal, string archvio, string rutaCer, string rutaKey, string rutaPfx, string passwordKey)
        {
            object[] retorno = new object[3];
            XmlDocument xmlDoc = new XmlDocument();
            //string cadenaOriginal = cargaArchivo(rutaNominasSinTimbrar + "CadenaOriginal.txt");

            string selloValor = obtieneSello(cadenaOriginal.Trim(), passwordKey, rutaPfx);
            string certificado = obtieneCertificado(rutaCer, passwordKey);
            string noCertificado = obtieneNoCertificado(rutaCer, passwordKey);

            if (cadenaOriginal == "" || selloValor == "" || certificado == "" || noCertificado == "")
            {
                retorno[0] = false;
                retorno[1] = "Uno o mas archivos de encriptación son incorrectos";
                retorno[2] = "";
            }
            else
            {
                try
                {
                    xmlDoc.Load(archvio);
                    XmlAttribute schema = xmlDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    schema.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
                    xmlDoc.DocumentElement.SetAttributeNode(schema);
                    xmlDoc.ChildNodes[1].Attributes["sello"].Value = selloValor;
                    xmlDoc.ChildNodes[1].Attributes["certificado"].Value = certificado;
                    xmlDoc.ChildNodes[1].Attributes["noCertificado"].Value = noCertificado.Trim();
                    xmlDoc.Save(archvio);

                    using (TextWriter writer = new StreamWriter(archvio, false, new UTF8Encoding(false)))
                    {
                        xmlDoc.Save(writer);
                        retorno[0] = true;
                        retorno[1] = "";
                        retorno[2] = certificado;
                    }
                }
                catch (Exception e)
                {
                    retorno[0] = false;
                    retorno[1] = e.Message;
                    retorno[2] = "";
                }
            }
            return retorno;
        }

        public string obtieneCertificado(string rutaCertificado, string Password)
        {
            string certificado = "";
            try
            {
                X509Certificate2 certEmisor = new X509Certificate2(); // Generas un objeto del tipo de certificado
                byte[] byteCertData = ReadFile(rutaCertificado.Trim()); // Manda llamar la funcion Readfile para cargar el archivo .cer 
                certEmisor.Import(byteCertData); // Importa los datos del certificado qeu acabas de leer
                certificado = Convert.ToBase64String(certEmisor.GetRawCertData()); // Conviertelos a Base64
            }
            catch { certificado = ""; }
            return certificado;
        }

        // Funcion para leer archivo
        internal static byte[] ReadFile(string rutaArchivo)
        {
            FileStream f = new FileStream(rutaArchivo.Trim(), FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }

        public string obtieneNoCertificado(string rutaCertificado, string Password)
        {
            string numero = "";
            string noCertificado = "";
            try
            {
                X509Certificate2 serie = new X509Certificate2();
                byte[] numeroSerie = ReadFile(rutaCertificado.Trim());
                serie.Import(numeroSerie);
                numero = serie.GetSerialNumberString();
                int contador = 0;
                while (contador < (numero.Length) / 2)
                {
                    noCertificado += numero.Substring((contador * 2) + 1, 1);
                    contador++;
                }
            }
            catch (Exception) { noCertificado = ""; }
            return noCertificado;
        }

        public DateTime[] obtieneVigencia(string rutaCertificado, string Password)
        {
            DateTime[] fechaVigencia = new DateTime[2];
            X509Certificate2 vigencia = new X509Certificate2();
            byte[] vigenciaCer = ReadFile(rutaCertificado.Trim());
            vigencia.Import(vigenciaCer);
            fechaVigencia[1] = vigencia.NotAfter;
            fechaVigencia[0] = vigencia.NotBefore;
            return fechaVigencia;
        }
    }

    public class Timbrado
    {
        Ejecucion ejecuta = new Ejecucion();
        private string sql;
        public string rfc { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string xmlTimbrar { get; set; }
        public string urlDocumento { get; set; }
        public string rfcReceptor { get; set; }
        public string certificadoEmisor { get; set; }
        public int documento { get; set; }
        public object[] info { get; set; }
        com.formulasistemas.www.ManejadordeTimbres foliosWSFormula = new com.formulasistemas.www.ManejadordeTimbres();

        public void timbrar()
        {
            try
            {
                int empresaActiva = foliosWSFormula.ObtieneEstatus(rfc);
                if (empresaActiva != 6)
                {
                    info[0] = false;
                    info[1] = "Error al timbrar documento: La empresa con R.F.C. " + rfc.Trim().ToUpper() + " esta dada de baja o no existe registrada en el catálogo de empresas de su proveedor de servicios; por favor contáctelo para resolver este error";
                }
                else
                {
                    int foliosDisponibles = 0;
                    foliosDisponibles = foliosWSFormula.ObtieneFoliosDisponibles(rfc);
                    if (foliosDisponibles == 0)
                    {
                        info[0] = false;
                        info[1] = "Error al timbrar documento: La empresa con R.F.C. " + rfc.Trim().ToUpper() + " no cuenta con folios disponibles; por favor a su proveedor de servicio de timbrado para solicitar más folios";
                    }
                    else
                    {
                        info = obtieneTimbre(usuario, password, xmlTimbrar, urlDocumento, documento, rfc, rfcReceptor, certificadoEmisor);
                        int timbrado = 0;
                        if (Convert.ToBoolean(info[0]))
                        {
                            timbrado = foliosWSFormula.Timbrar(rfc);
                            if (timbrado == 4)
                            {
                                info[0] = true;
                                info[1] = foliosDisponibles - 1;
                            }
                        }
                        else
                        {
                            info[0] = false;
                            info[1] = info[1].ToString();
                        }

                    }
                }
            }
            catch (Exception ex) { info[0] = false; info[1] = ex.Message; }
        }

        public void obtieneFoliosDisponibles()
        {
            int foliosDisponibles = 0;
            try
            {
                foliosDisponibles = foliosWSFormula.ObtieneFoliosDisponibles(rfc);
                info[0] = true;
                info[1] = foliosDisponibles;
            }
            catch (Exception ex) { info[0] = false; info[1] = ex.Message; }
        }

        private object[] obtieneTimbre(string usuario, string password, string xmlTimbrar, string urlDocumento, int documento, string rfcEmisor, string rfcReceptor, string certificadoEmisor)
        {
            XmlDocument timbrado = new XmlDocument();
            object[] timbrados = new object[2] { false, "" };
            //TimbradoRV.Timbrar servicioWs = new TimbradoRV.Timbrar();
            bool haSidoTimbrado = false;
            string errorTimbrado = "";
            try
            {

                int enProduccion = Convert.ToInt32(ConfigurationManager.AppSettings["enProduccion"].ToString());
                /*if (enProduccion == 0)
                    servicioWs.URL = ConfigurationManager.AppSettings["urlTimbradoPrueba"].ToString();
                else
                    servicioWs.URL = ConfigurationManager.AppSettings["urlTimbrado"].ToString();
                */
                string archivoTimbrar = urlDocumento.Trim();
                string archivoTimbrado = "";

                if (enProduccion == 1)
                {
                    WSGeneraCFDI.ServiceSoapClient servicio;
                    WSGeneraCFDI.StructCfd objResponse;

                    objResponse = new WSGeneraCFDI.StructCfd();
                    servicio = new WSGeneraCFDI.ServiceSoapClient();

                    string base64 = CodificarABase64(xmlTimbrar);
                    objResponse = servicio.GetTicketSC(usuario, password, base64);

                    if (objResponse.state == 0)
                    {
                        string xmlRetorno = DecodificarBase64ACadena(objResponse.Cfdi);
                        archivoTimbrado = xmlRetorno;
                        timbrado.LoadXml(xmlRetorno);
                        timbrado.Save(archivoTimbrar);
                        haSidoTimbrado = true;
                        errorTimbrado = "";
                    }
                    else
                    {
                        if (objResponse.state == -82)
                        {
                            string xmlRetorno = DecodificarBase64ACadena(objResponse.Cfdi);
                            archivoTimbrado = xmlRetorno;
                            timbrado.LoadXml(xmlRetorno);
                            timbrado.Save(archivoTimbrar);
                            haSidoTimbrado = true;
                            errorTimbrado = "";
                        }
                        else
                        {
                            archivoTimbrado = DecodificarBase64ACadena(objResponse.Cfdi);
                            errorTimbrado = objResponse.Descripcion;
                            haSidoTimbrado = false;
                        }
                    }
                    objResponse = null;

                    GC.Collect();
                }
                else
                {

                    WSGeneraCFDIprb.ServiceSoapClient servicio;
                    WSGeneraCFDIprb.StructCfd objResponse;

                    objResponse = new WSGeneraCFDIprb.StructCfd();
                    servicio = new WSGeneraCFDIprb.ServiceSoapClient();

                    string base64 = CodificarABase64(xmlTimbrar);
                    objResponse = servicio.GetTicketSC(usuario, password, base64);
                    if (objResponse.state == 0)
                    {
                        string xmlRetorno = DecodificarBase64ACadena(objResponse.Cfdi);
                        archivoTimbrado = xmlRetorno;
                        timbrado.LoadXml(xmlRetorno);
                        timbrado.Save(archivoTimbrar);
                        haSidoTimbrado = true;
                    }
                    else
                    {
                        if (objResponse.state == -82)
                        {
                            string xmlRetorno = DecodificarBase64ACadena(objResponse.Cfdi);
                            archivoTimbrado = xmlRetorno;
                            timbrado.LoadXml(xmlRetorno);
                            timbrado.Save(archivoTimbrar);
                            haSidoTimbrado = true;
                        }
                        else
                        {
                            archivoTimbrado = DecodificarBase64ACadena(objResponse.Cfdi);
                            errorTimbrado = objResponse.Descripcion;
                            haSidoTimbrado = false;
                        }
                    }
                    objResponse = null;

                    GC.Collect();
                }

                /*haSidoTimbrado = true;
                GeneracionDocumentos agregaDcto = new GeneracionDocumentos();
                agregaDcto.agregaDocumento(documento, xmlTimbrar, archivoTimbrado);*/

                if (!haSidoTimbrado)
                {

                    int idNodoFactura = 0;
                    int contNodos = 0;
                    if (timbrado.DocumentElement.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode nodo in timbrado.DocumentElement.ChildNodes)
                        {
                            if (nodo.Name == "cfdi:Complemento")
                            {
                                idNodoFactura = contNodos;
                                break;
                            }
                            contNodos++;
                        }
                    }
                    if (idNodoFactura != 0)
                    {
                        int nodoIndex = 0;
                        int contNodoChild = 0;
                        if (timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes.Count > 0)
                        {
                            foreach (XmlNode nodoChild in timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes)
                            {
                                if (nodoChild.Name == "tfd:TimbreFiscalDigital")
                                {
                                    nodoIndex = contNodoChild;
                                    break;
                                }
                                contNodoChild++;
                            }
                            string uuid = timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes[nodoIndex].Attributes["UUID"].Value.ToString();
                            string fechaTimbre = timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes[nodoIndex].Attributes["FechaTimbrado"].Value.ToString();
                            string certificadoSat = timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes[nodoIndex].Attributes["noCertificadoSAT"].Value.ToString();
                            string selloSat = timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes[nodoIndex].Attributes["selloSAT"].Value.ToString();
                            string version = timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes[nodoIndex].Attributes["version"].Value.ToString();
                            string selloCFD = timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes[nodoIndex].Attributes["selloCFD"].Value.ToString();
                            string certificadoCfd = timbrado.DocumentElement.Attributes["noCertificado"].Value.ToString();
                            string cadenaOriginal = cadenaOriginalCFDI(version, uuid, fechaTimbre, selloCFD, certificadoSat);
                            string textoCodigo = "?re=" + timbrado.DocumentElement.ChildNodes[0].Attributes["rfc"].Value.ToString() + "&rr=" + timbrado.DocumentElement.ChildNodes[1].Attributes["rfc"].Value.ToString() + "&tt=" + timbrado.DocumentElement.Attributes["total"].Value.ToString() + "&id=" + timbrado.DocumentElement.ChildNodes[idNodoFactura].ChildNodes[nodoIndex].Attributes["UUID"].Value.ToString();
                            Image codigoQr = generaQr(textoCodigo.Trim(), textoCodigo.Trim().Length);
                            MemoryStream ms = new MemoryStream();
                            codigoQr.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                            string ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + rfcEmisor.Trim().ToUpper() + "/" + rfcReceptor.Trim().ToUpper() + "/");
                            if (!Directory.Exists(ruta))
                                Directory.CreateDirectory(ruta);

                            codigoQr.Save(ruta + "\\" + uuid.Trim() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                            byte[] arregloQr = ms.ToArray();




                            GeneracionDocumentos genera = new GeneracionDocumentos();
                            genera.idCfd = documento;
                            genera.actualizaTimbrado(certificadoSat, fechaTimbre, uuid, selloSat, selloCFD, arregloQr, archivoTimbrar, cadenaOriginal, certificadoCfd, certificadoEmisor);
                            if (Convert.ToBoolean(genera.info[0]))
                            {
                                timbrados[0] = true;
                                timbrados[1] = "";
                            }
                            else
                            {
                                timbrados[0] = true;
                                timbrados[1] = "Se ha timbrado el documento correctamente, pero se genero un problema al actualizar su estatus, por favor contacte a su proveedor de servicio de timbrado para que le actualice su información aciendole llegar esta cadena: " + cadenaOriginal;
                            }
                        }
                        else
                        {
                            timbrados[0] = false;
                            timbrados[1] = "El documento no cuenta con la sección relacionada al timbrado";
                        }
                    }
                    else
                    {
                        timbrados[0] = false;
                        timbrados[1] = "El documento no cuenta con la sección relacionada al timbrado";
                        /*GeneracionDocumentos genera = new GeneracionDocumentos();
                        genera.idCfd = documento;
                        genera.actualizaTimbradoPrb(archivoTimbrar);
                        if (Convert.ToBoolean(genera.info[0]))
                        {
                            timbrados[0] = true;
                            timbrados[1] = "";
                        }
                        else
                        {
                            timbrados[0] = true;
                            timbrados[1] = "Se ha timbrado el documento correctamente, pero se genero un problema al actualizar su estatus, por favor contacte a su proveedor de servicio de timbrado para que le actualice su información aciendole llegar esta cadena: ";
                        }*/
                    }
                }
                else
                {
                    /*GeneracionDocumentos genera = new GeneracionDocumentos();
                    genera.idCfd = documento;
                    genera.actualizaTimbradoPrb(archivoTimbrar);
                    if (Convert.ToBoolean(genera.info[0]))
                    {
                        timbrados[0] = true;
                        timbrados[1] = "";
                    }
                    else
                    {
                        timbrados[0] = false;
                        timbrados[1] = "Se ha timbrado el documento correctamente, pero se genero un problema al actualizar su estatus, por favor contacte a su proveedor de servicio de timbrado para que le actualice su información aciendole llegar esta cadena: ";
                    }*/
                    timbrados[0] = false;
                    timbrados[1] = errorTimbrado;
                }



                /*

                servicioWs.Usuario = usuario;
                servicioWs.Contrasena = password;
                
                servicioWs.CFDorigen = archivoTimbrar;
                servicioWs.CFDdestino = archivoTimbrar;
                servicioWs.Tipo = Timbrar.tipos.Timbrar_CFDI;
                servicioWs.ConsumirServicio();
                if (servicioWs.Status == Timbrar.estado.Terminado)
                {
                    timbrado.Load(archivoTimbrar);
                   
                }
                else
                {                    
                    timbrados[0] = false;
                    timbrados[1] = servicioWs.MensajeDeError;                    
                }
                //Libera memoria
                servicioWs.Dispose();
                GC.Collect();*/
            }
            catch (Exception e)
            {
                timbrados[0] = false;
                timbrados[1] = e.Message;
            }
            return timbrados;
        }

        private string CodificarABase64(string strXML)
        {
            byte[] utf8Bytes = UTF8Encoding.UTF8.GetBytes(strXML);
            return Convert.ToBase64String(utf8Bytes);
        }

        private string DecodificarBase64ACadena(string strXMLCodificado)
        {
            try
            {
                byte[] decbuff = Convert.FromBase64String(strXMLCodificado);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            catch
            {
                return "";
            }
        }

        //obtiene cadena original CFDI
        private string cadenaOriginalCFDI(string version, string uuid, string fecha, string sello, string certificado)
        {
            string cadena = "";
            cadena = "||" + version.Trim() + "|" + uuid.Trim() + "|" + fecha.Trim() + "|" + sello.Trim() + "|" + certificado.Trim() + "||";
            return cadena;
        }

        // genera imagen codigo qr
        private Image generaQr(string entrada, int lenCadena)
        {
            string encriptar = entrada.Trim();
            QRCodeEncoder cod = new QRCodeEncoder();
            cod.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            cod.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            cod.QRCodeVersion = 0;
            Bitmap imagen = cod.Encode(encriptar.Trim(), Encoding.UTF8);
            return imagen;
        }

    }


}