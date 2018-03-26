using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de CartaDeducible
/// </summary>
public class CartaDeducible
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    Convertidores conversiones = new Convertidores();
    string sql;
    bool resultado;
    object[] datosEjecuta = new object[2];
    public CartaDeducible()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GenRepOrdTrabajo(int empresa, int taller, int orden, string nombre_taller, string usuario, int idFirmante, string forma)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        documento.AddTitle("CartaDeducible_E" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "CartaDeducible_E" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

        if (archivo.Trim() != "")
        {

            FileInfo doct = new FileInfo(archivo);
            if (doct.Exists)
                doct.Delete();
            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            // Abrir documento.
            documento.Open();

            string imagepath = HttpContext.Current.Server.MapPath("img/");
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo.png");
            gif.WidthPercentage = 40f;

            PdfPTable tablaEncabezado = new PdfPTable(2);
            tablaEncabezado.SetWidths(new float[] { 4f, 6f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;
            PdfPCell titu = new PdfPCell(new Phrase(fechas.obtieneFechaLocal().ToLongDateString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL)));
            titu.HorizontalAlignment = 2;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(gif);
            tablaEncabezado.AddCell(titu);
            documento.Add(tablaEncabezado);

            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));

            DatosVehiculos vehiculos = new DatosVehiculos();
            string marca, tipo, modelo, placas, prefijo;
            marca = tipo = modelo = placas = prefijo = "";

            object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculoCorreo(orden, empresa, taller);
            if (Convert.ToBoolean(vehiculo[0]))
            {
                DataSet valores = (DataSet)vehiculo[1];
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    marca = fila[1].ToString();
                    tipo = fila[2].ToString();
                    modelo = fila[3].ToString();
                    placas = fila[4].ToString();
                    prefijo = fila[5].ToString();
                }
            }
            string cliente = "";
            decimal monto = 0;
            decimal montoDemerito = 0;
            string montoCadena = "";
            string montoCadenaDemerito = "";
            string poliza = "";
            string siniestro = "";
            Recepciones recepciones = new Recepciones();
            object[] datos = recepciones.obtieneInfoOrden(orden, empresa, taller);
            if (Convert.ToBoolean(datos[0]))
            {
                DataSet info = (DataSet)datos[1];
                foreach (DataRow fila in info.Tables[0].Rows)
                {
                    cliente = fila[17].ToString().Trim().ToUpper() + " " + fila[18].ToString().Trim().ToUpper() + " " + fila[19].ToString().Trim().ToUpper();
                    try { monto = Convert.ToDecimal(fila[41].ToString()); }
                    catch (Exception) { monto = 0; }
                    try { montoDemerito = Convert.ToDecimal(fila[63].ToString()); }
                    catch (Exception) { montoDemerito = 0; }

                    conversiones._importe = monto.ToString();
                    montoCadena = conversiones.convierteMontoEnLetras();
                    conversiones._importe = montoDemerito.ToString();
                    montoCadenaDemerito = conversiones.convierteMontoEnLetras();
                    poliza = fila[33].ToString();
                    siniestro = fila[32].ToString();
                }
            }
            string mensaje = "";
            if (forma == "P")
                mensaje = "Recibimos de " + cliente.Trim() + " la cantidad de " + monto.ToString("C2") + " (" + montoCadena + ") por el concepto de pago de reparación de la unidad marca " + marca.Trim() + " " + tipo.Trim() + " modelo " + modelo + ", placas de circulación " + placas.Trim() + ". La cual fue realizada en nuestras instalaciones.";
            else if (forma == "M")
                mensaje = "Recibimos de " + cliente.Trim() + " la cantidad de " + montoDemerito.ToString("C2") + " (" + montoCadenaDemerito + ") por el concepto de pago de demérito de la unidad marca " + marca.Trim() + " " + tipo.Trim() + " modelo " + modelo + ", placas de circulación " + placas.Trim() + ". La cual fue realizada en nuestras instalaciones.";
            else
                mensaje = "Recibimos de " + cliente.Trim() + " la cantidad de " + monto.ToString("C2") + " (" + montoCadena + ") por el concepto de pago de deducible de la unidad marca " + marca.Trim() + " " + tipo.Trim() + " modelo " + modelo + ", placas de circulación " + placas.Trim() + "; con número de póliza " + poliza.Trim() + " y siniestro " + siniestro.Trim() + ". El cual fue reparado en nuestras instalaciones.";
            
            Paragraph detalle = new Paragraph(mensaje, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK));
            detalle.Alignment = Element.ALIGN_LEFT;
            documento.Add(detalle);
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            Paragraph ATT = new Paragraph("Moncar Aztahuacan, S.A. de .C.V.", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            ATT.Alignment = Element.ALIGN_LEFT;
            documento.Add(ATT);
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            sql = "select firmante from firmantes where id_firma=" + idFirmante;
            string firmante = ejecuta.scalarToStringSimple(sql);

            Paragraph ATT1 = new Paragraph(firmante, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            ATT1.Alignment = Element.ALIGN_LEFT;
            documento.Add(ATT1);
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            DocumentoFooter(documento, empresa);

            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void DocumentoFooter(Document document, int idEmpresa)
    {
        string sql = "select 'Calle '+calle+' '+ltrim(rtrim(num_ext))+' '+ltrim(rtrim(num_int))+' '+colonia+' Deleg. '+municipio+' '+pais+' '+estado+' '+cp+' Tels. '+ltrim(rtrim(tel_principal))+' - '+ltrim(rtrim(tel_oficina))+' Fax: '+ltrim(rtrim(fax))+' '+' E-mail: carlosmonrroy@prodigy.net.mx - www.moncar.com.mx' from empresas where id_empresa =" + idEmpresa.ToString();
        string footerDoc = ejecuta.scalarToStringSimple(sql);
        PdfPTable tabla = new PdfPTable(1);
        tabla.WidthPercentage = 100f;
        tabla.DefaultCell.Border = 0;
        PdfPCell pie = new PdfPCell(new Phrase(footerDoc, FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD)));
        pie.Border = 0;
        pie.HorizontalAlignment = 1;
        pie.BackgroundColor = BaseColor.LIGHT_GRAY;
        tabla.AddCell(" ");
        tabla.AddCell(" ");
        tabla.AddCell(" ");
        tabla.AddCell(" ");
        tabla.AddCell(pie);
        document.Add(tabla);
    }
}