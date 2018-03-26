using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;

public partial class FotosOrden : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    private string nombArchImg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateProgress1.DynamicLayout = true;
        foreach (DataListItem item in DataListFotosDanos.Items)
        {
            LinkButton btnGuardaImg = (LinkButton)item.FindControl("btnGuardaImg");
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnGuardaImg);
        }
        
        obtieneSesiones();
        if (!IsPostBack)
        {
            PanelImgZoom.Visible = PanelMascara.Visible = false;

            cargaDatosPie();
            lblErrorFotos.Text = "";
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
                ddlPerfil.Text = filaOrd[13].ToString();
                lblSiniestro.Text = filaOrd[9].ToString();
                lblDeducible.Text = Convert.ToDecimal(filaOrd[10].ToString()).ToString("C2");
                lblTotOrden.Text = Convert.ToDecimal(filaOrd[11].ToString()).ToString("C2");
                try
                {
                    DateTime fechaEntrega = Convert.ToDateTime(filaOrd[14].ToString());
                    if (fechaEntrega.ToString("yyyy-MM-dd") == "1900-01-01")
                        lblEntregaEstimada.Text = "No establecida";
                    else
                        lblEntregaEstimada.Text = filaOrd[14].ToString();
                }
                catch (Exception)
                {
                    lblEntregaEstimada.Text = "No establecida";
                }
                lblPorcDedu.Text = filaOrd[16].ToString() + "%";
            }
        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[6] { 0, 0, 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);
            sesiones[4] = Convert.ToInt32(Request.QueryString["o"]);
            sesiones[5] = Convert.ToInt32(Request.QueryString["f"]);
        }
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    protected void btnAddFotoDanos_Click(object sender, EventArgs e)
    {
        UpdatePanel1.Update();
        lblErrorFotos.Text = "";
        lblErrorFotos.CssClass = "errores";

        bool archivoValido = false;
        byte[] imagen = null;
        string[] archivosAborrar = null;
        //string extension = "";
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
            archivosAborrar = new string[documentos];

            for (int i = 0; i < documentos; i++)
            {
                filename = AsyncUpload1.UploadedFiles[i].FileName;
                string[] segmenatado = filename.Split(new char[] { '.' });

                archivoValido = validaArchivo(segmenatado[1]);
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

                    int[] sesiones = obtieneSesiones();
                    int idEmpresa = sesiones[2];
                    int idTaller = sesiones[3];
                    int proceso = Convert.ToInt32(ddlProcesoFotoFiltro.SelectedValue);
                    int noOrden = sesiones[4];

                    string nombre = filename;
                    bool agregado = datosOrdenes.agregaFotoDanos(idEmpresa, idTaller, noOrden, nombre, imagen, proceso);
                    if (agregado)
                        guardados++;
                }
                else
                    imagen = null;
            }
            lblErrorFotos.CssClass = "text-success";
            lblErrorFotos.Text = "Se guardaron " + guardados.ToString() + " imagenes de " + documentos.ToString() + " seleccionadas.";
            for (int j = 0; j < archivosAborrar.Length; j++)
            {
                FileInfo archivoBorrar = new FileInfo(archivosAborrar[j]);
                archivoBorrar.Delete();
            }

            DataListFotosDanos.DataBind();
        }
        catch (Exception ex) { lblErrorFotos.CssClass = "errores"; lblErrorFotos.Text = "Error: " + ex.Message; }
        finally { UpdatePanel1.Update(); }
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

    protected void DataListFotosDanos_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "zoom")
        {
            lblErrorFotos.Text = "";
            PanelMascara.Visible = true;
            PanelImgZoom.Visible = true;
            string[] valores = e.CommandArgument.ToString().Split(';');
            int id_empresa = Convert.ToInt32(valores[0]);
            int id_taller = Convert.ToInt32(valores[1]);
            int no_orden = Convert.ToInt32(valores[2]);
            int consecutivo = Convert.ToInt32(valores[3]);
            int proceso = Convert.ToInt32(valores[4]);
            imgZoom.ImageUrl = "~/ImgEmpresas.ashx?id=" + id_empresa + ";" + id_taller + ";" + no_orden + ";" + consecutivo + ";" + proceso;
        }
        else if (e.CommandName == "Delete")
        {

            lblErrorFotos.Text = "";
            try
            {
                lblErrorFotos.Text = "";
                string[] valores = e.CommandArgument.ToString().Split(';');
                int id_empresa = Convert.ToInt32(valores[0]);
                int id_taller = Convert.ToInt32(valores[1]);
                int no_orden = Convert.ToInt32(valores[2]);
                int consecutivo = Convert.ToInt32(valores[3]);
                int proceso = Convert.ToInt32(valores[4]);
                bool eliminado = datosOrdenes.eliminarFotoDanos(id_empresa, id_taller, no_orden, consecutivo, proceso);
                if (eliminado)
                {
                    DataListFotosDanos.DataBind();
                }
                else
                    lblErrorFotos.Text = "La imagen no se logro eliminar, verifique su conexión e intentelo nuevamente mas tarde";
            }
            catch (Exception x)
            {
                lblErrorFotos.Text = "La carga de las imagenes no se logro por ún error inesperado: " + x.Message;
            }
            finally { UpdatePanel1.Update(); }
        }
        else if (e.CommandName == "GuardaImg")
        {
            string[] valores = e.CommandArgument.ToString().Split(';');
            int id_empresa = Convert.ToInt32(valores[0]);
            int id_taller = Convert.ToInt32(valores[1]);
            int no_orden = Convert.ToInt32(valores[2]);
            int consecutivo = Convert.ToInt32(valores[3]);
            int proceso = Convert.ToInt32(valores[4]);
            byte[] arrFile = getImagen(id_empresa, id_taller, consecutivo, no_orden, proceso);
            string url = Request.Url.Segments[1];
            string url2 = System.Configuration.ConfigurationManager.AppSettings["direccion"] + "/TMP/";
            var contDisp = new System.Net.Mime.ContentDisposition { FileName = nombArchImg, Inline = false };
            Response.AddHeader("content-disposition", "attachment; filename=" + nombArchImg);
            Response.ContentType = "application/octet-stream";
            Response.ContentType = "image/JPEG";
            Response.Buffer = true;
            Response.Clear();
            Response.BinaryWrite(arrFile);
            Response.Flush();
            Response.End();
            /*Alx: Para descargar desde un archivo almacenado en el servidor
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "image/jpeg";
            response.AddHeader("Content-Disposition", "attachment; filename=" + nomArchivo + ";");
            response.TransmitFile(Server.MapPath("~/TMP/" + nomArchivo));
            response.Flush();
            response.End();*/
        }
    }

    protected void btnCerrarImgZomm_Click(object sender, EventArgs e)
    {
        PanelImgZoom.Visible = false;
        PanelMascara.Visible = false;
    }
    protected void ddlProcesoFotoFiltro_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorFotos.Text = "";
        DataListFotosDanos.DataBind();
    }

    protected void btnGuardaImg_Click(object sender, EventArgs e)
    {
        LinkButton btnSaveIme = (LinkButton)sender;
        string[] valores = btnSaveIme.CommandArgument.ToString().Split(';');
        int id_empresa = Convert.ToInt32(valores[0]);
        int id_taller = Convert.ToInt32(valores[1]);
        int no_orden = Convert.ToInt32(valores[2]);
        int consecutivo = Convert.ToInt32(valores[3]);
        int proceso = Convert.ToInt32(valores[4]);

        byte[] arrFile = getImagen(id_empresa, id_taller, consecutivo, no_orden, proceso);
        //Alx: para leer la imagen desde el servidor
        //byte[] arrFile = System.IO.File.ReadAllBytes(@"C:\inetpub\wwwroot\e-Taller_axl\TMP\" + nombArchImg);
        if (arrFile != null)
        {
            var contDisp = new System.Net.Mime.ContentDisposition { FileName = nombArchImg, Inline = false };

            Response.ClearContent();
            Response.AddHeader("content-disposition", contDisp.ToString());
            Response.ContentType = "application/octet-stream";
            Response.ContentType = "image/JPEG";
            Response.Buffer = false;
            Response.Clear();
            Response.BinaryWrite(arrFile);
            Response.Flush();
            Response.End();

            /*Alx: Para descargar desde un archivo almacenado en el servidor
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "image/jpeg";
            response.AddHeader("Content-Disposition", "attachment; filename=" + nomArchivo + ";");
            response.TransmitFile(Server.MapPath("~/TMP/" + nomArchivo));
            response.Flush();
            response.End();*/
        }
    }

    private byte[] getImagen(int idEmpresa, int idTaller, int consecutivo, int noOrden, int proceso)
    {
        System.Data.SqlClient.SqlConnection conexion = new System.Data.SqlClient.SqlConnection();
        conexion.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Taller"].ToString();
        string sql = "select nombre_imagen, imagen from Fotografias_Orden where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " and consecutivo=" + consecutivo.ToString() + " and proceso=" + proceso.ToString();
        byte[] imgStream = null;
        try
        {
            conexion.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, conexion);
            System.Data.SqlClient.SqlDataReader lectura = cmd.ExecuteReader();
            if (lectura.HasRows)
            {
                lectura.Read();
                imgStream = (byte[])lectura["imagen"];
                nombArchImg = lectura["nombre_imagen"].ToString();
                /*Alx: Guarda el archivo en disco del lado del servidor
                string r = MapPath("TMP/");
                FileStream fs = new FileStream(r + nombArchImg, FileMode.Create);
                fs.Write(imgStream, 0, imgStream.Length);
                fs.Close();
                */
            }
        }
        catch (Exception ex)
        {
            lblErrorFotos.Text = "La imagen tiene un problema, por lo que no es posible descargarla. Error: " + ex.Message;
        }
        finally
        {
            conexion.Dispose();
            conexion.Close();
        }

        return imgStream;
    }
}