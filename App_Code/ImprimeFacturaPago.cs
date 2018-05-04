using System;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.io;
using System.Diagnostics;
using System.IO;
using E_Utilities;
using ClosedXML.Excel;
/// <summary>
/// Descripción breve de ImprimeFacturaPago
/// </summary>
public class ImprimeFacturaPago
{
    Ejecuciones ejecuta = new Ejecuciones();
    BaseDatos data = new BaseDatos();
    Fechas fechas = new Fechas();
    FacturacionElectronica.Receptores receptores = new FacturacionElectronica.Receptores();
    FacturacionElectronica.Emisores emisores = new FacturacionElectronica.Emisores();
    string sql;
    bool resultado;
    object[] ejecutados = new object[2];
    public string archivo { get; set; }
    public string usuario { get; set; }
    public string estatusFact { get; set; }
    public string Ini { get; set; }
    public string Fin { get; set; }

    public ImprimeFacturaPago()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GenFactura(int idCfd, object[] encabezado, DataTable detalle, object[] timbre)
    {
        Ejecuciones busca = new Ejecuciones();
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        //documento.AddTitle("ComparativoCotizacion_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString());

        string[] nombre = new string[2] { "", "" };
        try
        {
            FileInfo fact = new FileInfo(Convert.ToString(timbre[4]));
            nombre = fact.Name.ToString().Split(new char[] { '.' });
            documento.AddTitle("Factura_" + nombre[0]);

        }
        catch (Exception)
        {
            string ra = HttpContext.Current.Server.MapPath("~/tmp");
            if (!(Directory.Exists(ra)))
                Directory.CreateDirectory(ra);
            string arch = ra + "\\" + "Factura_" + Convert.ToString(encabezado[0]).Trim();
            FileInfo fact = new FileInfo(arch);
            nombre = fact.Name.ToString().Split(new char[] { '.' });
            documento.AddTitle("Factura_" + Convert.ToString(encabezado[0]).Trim());
        }
        documento.AddCreator("MoncarWeb");
        string ruta = "";

        if (encabezado[11].ToString() == "C")
            ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + Convert.ToString(encabezado[20]).Trim() + "/" + Convert.ToString(encabezado[21]).Trim() + "/Cancelados");
        else
            ruta = HttpContext.Current.Server.MapPath("~/Comprobantes/" + Convert.ToString(encabezado[20]).Trim() + "/" + Convert.ToString(encabezado[21]).Trim());



        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
            Directory.CreateDirectory(ruta);
        string archivo = ruta + "\\" + nombre[0].Trim() + ".pdf";
        if (archivo.Trim() != "")
        {
            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            // Abrir documento.
            documento.Open();

            string imagepath = HttpContext.Current.Server.MapPath("img/");
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo.png");
            gif.WidthPercentage = 15f;

            //encabezado
            PdfPTable tablaEncabezado = new PdfPTable(3);
            tablaEncabezado.SetWidths(new float[] { 2f, 4f, 4f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;

            BaseColor grisClaro = new BaseColor(221, 221, 221);
            BaseColor rojoOscuro = new BaseColor(192, 0, 0);
            PdfPCell tituPagina;
            tituPagina = new PdfPCell(gif);
            tituPagina.Rowspan = 4;
            tituPagina.Border = 0;
            tablaEncabezado.AddCell(tituPagina);

            if (encabezado[11].ToString() == "C")
            {
                tituPagina = new PdfPCell(new Phrase("CANCELADO", FontFactory.GetFont("ARIAL", 15, iTextSharp.text.Font.BOLD, new BaseColor(255, 255, 255))));
                tituPagina.HorizontalAlignment = Element.ALIGN_CENTER;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tituPagina.Colspan = 2;
                tituPagina.BackgroundColor = new BaseColor(192, 0, 0);
                tablaEncabezado.AddCell(tituPagina);
            }
            else
            {
                tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 15, iTextSharp.text.Font.BOLD)));
                tituPagina.HorizontalAlignment = 2;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tituPagina.Colspan = 2;
                tablaEncabezado.AddCell(tituPagina);
            }

            tituPagina = new PdfPCell(new Phrase("COBRO CFDI", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_CENTER;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 2;
            tituPagina.BackgroundColor = grisClaro;
            tablaEncabezado.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("Folio Fiscal:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 1;
            tablaEncabezado.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(timbre[4]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 1;
            tablaEncabezado.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("No de Serie del Certificado del CSD:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 1;
            tablaEncabezado.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(timbre[9]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 1;
            tituPagina.PaddingBottom = 5f;
            tablaEncabezado.AddCell(tituPagina);

            documento.Add(tablaEncabezado);



            //Emisor

            PdfPTable tablaEmisor = new PdfPTable(2);
            tablaEmisor.SetWidths(new float[] { 1.6f, 8.4f });
            tablaEmisor.DefaultCell.Border = 0;
            //tituPagina.Rowspan = 6;
            tablaEmisor.WidthPercentage = 100f;

            emisores.idEmisor = Convert.ToInt32(encabezado[2]);
            emisores.obtieneInfoEmisor();
            object[] emisor = null;
            if (Convert.ToBoolean(emisores.info[0]))
            {
                DataSet infoRec = (DataSet)emisores.info[1];
                foreach (DataRow r in infoRec.Tables[0].Rows)
                {
                    emisor = r.ItemArray;
                    break;
                }
            }

            tituPagina = new PdfPCell(new Phrase("R.F.C.:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEmisor.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(emisor[1]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEmisor.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("Razón Social:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEmisor.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(emisor[2]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEmisor.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("Régimen Fiscal:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEmisor.AddCell(tituPagina);
            object[] regimenFiscal = busca.scalarToString("select Descripcion from RegimenFiscal_FSAT where ClaveRegimen='" + encabezado[16].ToString().Trim() + "'");
            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(encabezado[16]) + " " + regimenFiscal[1].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.PaddingBottom = 5f;
            tablaEmisor.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("Factura:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEmisor.AddCell(tituPagina);

            //factura
            tituPagina = new PdfPCell(new Phrase(encabezado[0].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEmisor.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("Lugar. fecha y hora de emisión:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEmisor.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase(emisor[14].ToString() + " / " + Convert.ToString(timbre[3]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase(emisor[14].ToString() + "  ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 7;
            tituPagina.PaddingBottom = 5f;
            tablaEmisor.AddCell(tituPagina);

            documento.Add(tablaEmisor);

            documento.Add(new Paragraph(" "));

            iTextSharp.text.pdf.draw.LineSeparator line1 = new iTextSharp.text.pdf.draw.LineSeparator(1F, 120.0F, rojoOscuro, Element.ALIGN_CENTER, 1);
            documento.Add(line1);


            //receptor

            tablaEncabezado = new PdfPTable(2);
            tablaEncabezado.SetWidths(new float[] { 1.6f, 8.4f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;

            tituPagina = new PdfPCell(new Phrase("Cliente", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 2;
            tituPagina.BackgroundColor = grisClaro;
            tablaEncabezado.AddCell(tituPagina);

            receptores.idReceptor = Convert.ToInt32(encabezado[3]);
            receptores.obtieneInfoReceptor();
            object[] receptor = null;
            if (Convert.ToBoolean(receptores.info[0]))
            {
                DataSet infoRec = (DataSet)receptores.info[1];
                foreach (DataRow r in infoRec.Tables[0].Rows)
                {
                    receptor = r.ItemArray;
                    break;
                }
            }

            tituPagina = new PdfPCell(new Phrase("R.F.C.:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(encabezado[21]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("Razón Social:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(receptor[2]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("Uso de CFDI:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado.AddCell(tituPagina);

            object[] usoCFDI = busca.scalarToString("select descripcion from usoCFDI_FSAT where ClaveUso='" + encabezado[30].ToString().Trim() + "'");

            try { tituPagina = new PdfPCell(new Phrase(usoCFDI[1].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado.AddCell(tituPagina);

            documento.Add(tablaEncabezado);
            documento.Add(new Paragraph(" "));
            documento.Add(line1);


            PdfPTable tablaEncabezado1 = new PdfPTable(2);
            tablaEncabezado1.SetWidths(new float[] { 1.6f, 8.4f });
            tablaEncabezado1.DefaultCell.Border = 0;
            tablaEncabezado1.WidthPercentage = 100f;

            tituPagina = new PdfPCell(new Phrase("Datos Generales de la Factura.", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 2;
            tituPagina.BackgroundColor = grisClaro;
            tablaEncabezado1.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("CODIGO P/S: ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado1.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase("84111506", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado1.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("Clave Unidad: ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = Element.ALIGN_RIGHT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado1.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase("ACT", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = Element.ALIGN_LEFT;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaEncabezado1.AddCell(tituPagina);

            documento.Add(tablaEncabezado1);

            documento.Add(new Paragraph(" "));
            documento.Add(line1);


            PdfPTable tablaEncabezado2 = new PdfPTable(2);
            tablaEncabezado2.SetWidths(new float[] { 1.6f, 8.4f });
            tablaEncabezado2.DefaultCell.Border = 0;
            tablaEncabezado2.WidthPercentage = 100f;

            tituPagina = new PdfPCell(new Phrase("CFDI Relacionados", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.Colspan = 2;
            tituPagina.BackgroundColor = grisClaro;
            tablaEncabezado2.AddCell(tituPagina);
            documento.Add(tablaEncabezado2);

            documento.Add(new Paragraph(" "));

            //encabezado tablas detalle
            PdfPTable tablaDetalle = new PdfPTable(7);
            tablaDetalle.SetWidths(new float[] { 24f, 10f, 10f, 10f, 8f, 8f, 8f });
            tablaDetalle.DefaultCell.Border = 1;
            tablaDetalle.DefaultCell.BackgroundColor = grisClaro;
            tablaDetalle.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY;
            tablaDetalle.WidthPercentage = 100f;

            


            tituPagina = new PdfPCell(new Phrase("UUID", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingTop = 2f;
            tituPagina.BackgroundColor = grisClaro;
            tituPagina.PaddingBottom = 2f;
            tablaDetalle.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("FOLIO", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingTop = 2f;
            tituPagina.BackgroundColor = grisClaro;
            tituPagina.PaddingBottom = 2f;
            tablaDetalle.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("MONEDA", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingTop = 2f;
            tituPagina.BackgroundColor = grisClaro;
            tituPagina.PaddingBottom = 2f;
            tablaDetalle.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("PARCIALIDAD", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingTop = 2f;
            tituPagina.BackgroundColor = grisClaro;
            tituPagina.PaddingBottom = 2f;
            tablaDetalle.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("SALDO ANTERIOR", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingTop = 2f;
            tituPagina.BackgroundColor = grisClaro;
            tituPagina.PaddingBottom = 2f;
            tablaDetalle.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("IMPORTE PAGADO", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingTop = 2f;
            tituPagina.BackgroundColor = grisClaro;
            tituPagina.PaddingBottom = 2f;
            tablaDetalle.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase("SALDO ACTUAL", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingTop = 2f;
            tituPagina.BackgroundColor = grisClaro;
            tituPagina.PaddingBottom = 2f;
            tablaDetalle.AddCell(tituPagina);

            double subtotal = 0;
            try { subtotal = Convert.ToDouble(Convert.ToString(encabezado[22])); } catch (Exception) { subtotal = 0; }
            double descuentoGlobal = 0;
            try { descuentoGlobal = Convert.ToDouble(Convert.ToString(encabezado[9])); } catch (Exception) { descuentoGlobal = 0; }
            double descuentoMoRef = 0;
            try { descuentoMoRef = Convert.ToDouble(Convert.ToString(encabezado[23])); } catch (Exception) { descuentoMoRef = 0; }
            double descuento = 0;
            try { descuento = descuentoGlobal + descuentoMoRef; } catch (Exception) { descuento = 0; }
            double neto = subtotal - descuento;
            double total=0;
            //try { total = Convert.ToDouble(Convert.ToString(encabezado[26])); } catch (Exception) { total = 0; }
            double iva;
            try { iva = Convert.ToDouble(Convert.ToString(encabezado[24])); } catch (Exception) { iva = 0; }


            foreach (DataRow fila in detalle.Rows){
                //impresion de UUID
                tituPagina = new PdfPCell(new Phrase(fila[0].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                tituPagina.HorizontalAlignment = 1;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tablaDetalle.AddCell(tituPagina);

                //impresion de FOLIO
                tituPagina = new PdfPCell(new Phrase(fila[1].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                tituPagina.HorizontalAlignment = 1;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tablaDetalle.AddCell(tituPagina);

                //impresion de Moneda
                tituPagina = new PdfPCell(new Phrase("Peso", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                tituPagina.HorizontalAlignment = 1;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tablaDetalle.AddCell(tituPagina);

                //impresion de Parcialidad
                tituPagina = new PdfPCell(new Phrase(fila[2].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                tituPagina.HorizontalAlignment = 1;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tablaDetalle.AddCell(tituPagina);

                //impresion de SaldoAnterior
                tituPagina = new PdfPCell(new Phrase(fila[3].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                tituPagina.HorizontalAlignment = 1;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tablaDetalle.AddCell(tituPagina);

                //impresion de SaldoPagado
                tituPagina = new PdfPCell(new Phrase(fila[4].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                tituPagina.HorizontalAlignment = 1;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tablaDetalle.AddCell(tituPagina);

                total += Convert.ToDouble(fila[4]);

                //impresion de SaldoActual 
                tituPagina = new PdfPCell(new Phrase(fila[5].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                tituPagina.HorizontalAlignment = 1;
                tituPagina.VerticalAlignment = 1;
                tituPagina.Border = 0;
                tablaDetalle.AddCell(tituPagina);

            }

            documento.Add(tablaDetalle);

            //linea roja

            documento.Add(new Paragraph(" "));
            documento.Add(line1);
            tablaDetalle = new PdfPTable(2);
            tablaDetalle.SetWidths(new float[] { 70f, 30f });
            tablaDetalle.DefaultCell.Border = 0;
            tablaDetalle.WidthPercentage = 100f;

            //iva = subtotal * 0.16;
            //total = subtotal + iva;
            //total con letra tetxo
            Convertidores conversor = new Convertidores();
            
            //foreach (DataRow fila in detalle.Rows)
            //{
            //    total = Convert.ToDouble(fila[4].ToString());
            //}

            conversor._importe = total.ToString();
            tituPagina = new PdfPCell(new Phrase("Total con letra: " + Environment.NewLine + Environment.NewLine + Environment.NewLine + conversor.convierteMontoEnLetras().ToUpper().Trim(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaDetalle.AddCell(tituPagina);

            PdfPTable tablaImportes = new PdfPTable(2);
            tablaImportes.SetWidths(new float[] { 5f, 5f });
            tablaImportes.DefaultCell.Border = 0;
            tablaImportes.DefaultCell.BackgroundColor = grisClaro;
            tablaImportes.WidthPercentage = 100;

            //total
            tituPagina = new PdfPCell(new Phrase("Total:", FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.BorderColorTop = BaseColor.BLACK;
            tituPagina.BorderWidthTop = 1;
            tituPagina.BackgroundColor = grisClaro;
            tablaImportes.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase(total.ToString("C2"), FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 2;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.BorderColorTop = BaseColor.BLACK;
            tituPagina.BorderWidthTop = 1;
            tituPagina.BackgroundColor = grisClaro;
            tablaImportes.AddCell(tituPagina);

            tablaDetalle.AddCell(tablaImportes);

            documento.Add(tablaDetalle);
            documento.Add(new Paragraph(" "));

            tablaImportes = new PdfPTable(4);
            tablaImportes.SetWidths(new float[] { 25f, 25f, 25f, 25f });
            tablaImportes.DefaultCell.Border = 0;
            //pago en exhibiciones
            object[] FormaPago = busca.scalarToString("select descripcion from c_formapago_f where claveformapago='" + encabezado[5].ToString() + "'");
            try { tituPagina = new PdfPCell(new Phrase(encabezado[5].ToString() + "-" + Convert.ToString(FormaPago[1]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.PaddingBottom = 2f;
            tablaImportes.AddCell(tituPagina);
            //tipo de pago
            try { tituPagina = new PdfPCell(new Phrase("Condiciones de Pago : " + Convert.ToString(encabezado[6]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.PaddingBottom = 2f;
            tablaImportes.AddCell(tituPagina);
            //condicion de pago
            object[] condicionPago = busca.scalarToString("select descripcion from metodopago_fsat where ClaveMetodo='" + encabezado[7].ToString() + "'");
            try { tituPagina = new PdfPCell(new Phrase(encabezado[7].ToString() + "-" + condicionPago[1].ToString(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.PaddingBottom = 2f;
            tablaImportes.AddCell(tituPagina);
            //cuenta de pago
            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(encabezado[15]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.PaddingBottom = 2f;
            tablaImportes.AddCell(tituPagina);

            documento.Add(tablaImportes);

            /*
            string[] decimalesTotalText = total.ToString("C2").Split('.');
            //agrega encabezado tabla detalle y detalle
            
            //tabla totales 
            tablaDetalle = new PdfPTable(5);
            tablaDetalle.SetWidths(new float[] { 15f, 15f, 25f, 20f, 25f });
            tablaDetalle.DefaultCell.Border = 0;
            tablaDetalle.DefaultCell.BackgroundColor = grisClaro;
            tablaDetalle.DefaultCell.BorderColor = BaseColor.LIGHT_GRAY;
            tablaDetalle.WidthPercentage = 110;
            //tabla montos 
            PdfPTable tablaImportes = new PdfPTable(1);
            tablaDetalle.DefaultCell.Border = 0;
            tablaDetalle.DefaultCell.BackgroundColor = grisClaro;
            //total con letra titulo subtotal
            
           

            

            tablaDetalle.AddCell(tablaImportes);
            //monto letras iva
            tablaImportes = new PdfPTable(1);
            tablaDetalle.DefaultCell.BackgroundColor = grisClaro;
           

            tituPagina = new PdfPCell(new Phrase(conversor.convierteMontoEnLetras().ToUpper().Trim(), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 2;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingLeft = 15;
            tituPagina.Border = 0;
            tituPagina.Colspan = 4;
            tablaDetalle.AddCell(tituPagina);
            //Subtotal
            

            tablaDetalle.AddCell(tablaImportes);

            //espacio en blanco ieps retenido
            tablaImportes = new PdfPTable(1);
            tablaImportes.DefaultCell.Border = 0;
            tablaImportes.DefaultCell.BackgroundColor = grisClaro;
            //" "
            tituPagina = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 2;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingLeft = 15;
            tituPagina.Border = 0;
            tituPagina.Colspan = 4;
            tablaDetalle.AddCell(tituPagina);
            

            tablaDetalle.AddCell(tablaImportes);

            //espacio en blanco isr retenido
            tablaImportes = new PdfPTable(1);
            tablaImportes.DefaultCell.Border = 0;
            tablaImportes.DefaultCell.BackgroundColor = grisClaro;
            //" "
            tituPagina = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 2;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingLeft = 15;
            tituPagina.Colspan = 4;
            tituPagina.Border = 0;
            tablaDetalle.AddCell(tituPagina);
            //ISR Detenido
            tituPagina = new PdfPCell(new Phrase("ISR Retenido:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.BackgroundColor = grisClaro;
            tablaImportes.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 2;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.BackgroundColor = grisClaro;
            tablaImportes.AddCell(tituPagina);

            tablaDetalle.AddCell(tablaImportes);

            //espacio en blanco iva retenido
            tablaImportes = new PdfPTable(1);
            tablaImportes.DefaultCell.Border = 0;
            tablaImportes.DefaultCell.BackgroundColor = grisClaro;
            //" "
            tituPagina = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 2;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingLeft = 15;
            tituPagina.Border = 0;
            tituPagina.Colspan = 4;
            tablaDetalle.AddCell(tituPagina);
            //ISR Detenido
            tituPagina = new PdfPCell(new Phrase("IVA Retenido:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.BackgroundColor = grisClaro;
            tablaImportes.AddCell(tituPagina);

            tituPagina = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
            tituPagina.HorizontalAlignment = 2;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.BackgroundColor = grisClaro;
            tablaImportes.AddCell(tituPagina);

            tablaDetalle.AddCell(tablaImportes);

            //tipo de pago total
            tablaImportes = new PdfPTable(2);
            tablaImportes.SetWidths(new float[] { 3f, 7f });
            tablaImportes.DefaultCell.Border = 0;
            tablaImportes.DefaultCell.BackgroundColor = grisClaro;
            //pago en exhibiciones
            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(encabezado[5]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.PaddingLeft = 15;
            tituPagina.Colspan = 2;
            tablaDetalle.AddCell(tituPagina);
            //tipo de pago
            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(encabezado[6]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingLeft = 15;
            tituPagina.Border = 0;
            tablaDetalle.AddCell(tituPagina);
            //condicion de pago
            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(encabezado[7]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.PaddingLeft = 15;
            tituPagina.Border = 0;
            tablaDetalle.AddCell(tituPagina);

            

            tablaDetalle.AddCell(tablaImportes);
            
            //agrega totales
            documento.Add(tablaDetalle);*/
            //br en itext
            documento.Add(new Paragraph(" "));
            //linea roja
            documento.Add(line1);

            //PdfPTable tablanotas = new PdfPTable(1);
            //tablanotas.DefaultCell.Border = 0;
            //tablanotas.WidthPercentage = 100;


            //tituPagina = new PdfPCell(new Phrase("NOTAS: ", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            //tituPagina.HorizontalAlignment = 0;
            //tituPagina.VerticalAlignment = 1;
            //tituPagina.Border = 0;
            //tablanotas.AddCell(tituPagina);
            //notas
            //try
            //{
            //    string porcDesc = "";
            //    if (descuentoGlobal != 0)
            //    {
            //        try
            //        {
            //            decimal porDescInt = Convert.ToDecimal(encabezado[8].ToString());
            //            if (porDescInt > 0)
            //            {
            //                porcDesc = "%" + porDescInt.ToString("F2") + " de descuento por " + encabezado[10].ToString();
            //            }
            //            else
            //                porcDesc = "Descuento por " + encabezado[10].ToString();
            //        }
            //        catch (Exception) { }
            //    }

            //    if (descuentoMoRef != 0)
            //    {
            //        decimal porcMo = 0, porcRef = 0;
            //        try { porcMo = Convert.ToDecimal(encabezado[27]); } catch (Exception) { porcMo = 0; }
            //        try { porcRef = Convert.ToDecimal(encabezado[28]); } catch (Exception) { porcRef = 0; }
            //        if (porcMo > 0)
            //            porcDesc = porcDesc + Environment.NewLine + "%" + porcMo.ToString("F2") + " descuento de Mano de Obra";
            //        if (porcRef > 0)
            //            porcDesc = porcDesc + Environment.NewLine + "%" + porcRef.ToString("F2") + " descuento de Refacciones";
            //    }

            //    tituPagina = new PdfPCell(new Phrase(Convert.ToString(encabezado[13] + " " + porcDesc), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL)));
            //}
            //catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            //tituPagina.HorizontalAlignment = 0;
            //tituPagina.VerticalAlignment = 1;
            //tituPagina.Border = 0;
            //tablanotas.AddCell(tituPagina);
            //documento.Add(tablanotas);



            //tabla sellos
            PdfPTable tablaSellos = new PdfPTable(1);
            tablaSellos.WidthPercentage = 100;
            tablaSellos.DefaultCell.Border = 0;

            //sello digital titulo
            tituPagina = new PdfPCell(new Phrase("Sello Digital del CFDI:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaSellos.AddCell(tituPagina);
            //sello digital #
            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(timbre[6]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaSellos.AddCell(tituPagina);
            //sello sat titulo
            tituPagina = new PdfPCell(new Phrase("Sello del SAT:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaSellos.AddCell(tituPagina);
            //sello sat #
            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(timbre[5]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaSellos.AddCell(tituPagina);
            //agrega tabla sellos doc
            documento.Add(tablaSellos);
            //br en itext
            documento.Add(new Paragraph(" "));
            //tabla qr                         
            PdfPTable tablaQrCadena = new PdfPTable(2);
            tablaQrCadena.SetWidths(new float[] { 20f, 80f });
            tablaQrCadena.WidthPercentage = 100;
            tablaQrCadena.DefaultCell.Border = 1;
            //codigo qr

            byte[] imagenBuffer = null;
            try
            {
                imagenBuffer = (byte[])timbre[10];
                System.IO.MemoryStream st = new System.IO.MemoryStream(imagenBuffer);
                System.Drawing.Image foto = System.Drawing.Image.FromStream(st);

                string nomImg = ruta + "\\" + Convert.ToString(timbre[4]).Trim() + "-" + timbre[0].ToString() + ".jpg";
                string[] infoImagen = nomImg.Split(new char[] { '.' });
                string extencion = infoImagen[2].ToLower();
                System.Drawing.Imaging.ImageFormat formato;
                switch (extencion)
                {
                    case "jpg":
                        formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case "png":
                        formato = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    case "jpeg":
                        formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case "gif":
                        formato = System.Drawing.Imaging.ImageFormat.Gif;
                        break;
                    case "bmp":
                        formato = System.Drawing.Imaging.ImageFormat.Bmp;
                        break;
                    case "tiff":
                        formato = System.Drawing.Imaging.ImageFormat.Tiff;
                        break;
                    default:
                        formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                }
                foto.Save(nomImg, formato);
            }
            catch (Exception)
            {
                imagenBuffer = null;
            }

            try
            {
                gif = iTextSharp.text.Image.GetInstance(ruta + "\\" + Convert.ToString(timbre[4]).Trim() + "-" + timbre[0].ToString() + ".jpg");
                gif.WidthPercentage = 10;
                //agregar qr a doc
                tituPagina = new PdfPCell(gif);
            }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.Rowspan = 4;
            tituPagina.Border = 0;
            tablaQrCadena.AddCell(tituPagina);

            //cadena titulo
            tituPagina = new PdfPCell(new Phrase("Cadena Original del complemento de certificación digital del SAT:", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.BOLD, rojoOscuro)));
            tituPagina.Border = 0;
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tablaQrCadena.AddCell(tituPagina);
            //cadena texto
            try { tituPagina = new PdfPCell(new Phrase(Convert.ToString(timbre[8]), FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaQrCadena.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase("No de Serie del Certificado del SAT: " + Convert.ToString(timbre[2]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaQrCadena.AddCell(tituPagina);

            try { tituPagina = new PdfPCell(new Phrase("Fecha y hora de certificación: " + Convert.ToString(timbre[3]), FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            catch (Exception) { tituPagina = new PdfPCell(new Phrase("", FontFactory.GetFont("ARIAL", 7, iTextSharp.text.Font.NORMAL))); }
            tituPagina.HorizontalAlignment = 0;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaQrCadena.AddCell(tituPagina);

            //agrega tabla QR
            documento.Add(tablaQrCadena);
            //br en itext
            documento.Add(new Paragraph(" "));
            //linea roja
            documento.Add(line1);

            tablaQrCadena = new PdfPTable(1);
            tablaQrCadena.WidthPercentage = 100;
            tablaQrCadena.DefaultCell.Border = 0;
            tituPagina = new PdfPCell(new Phrase("Este documento es una representación impresa de un CFDI", FontFactory.GetFont("ARIAL", 6, iTextSharp.text.Font.BOLD)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tablaQrCadena.AddCell(tituPagina);

            documento.Add(tablaQrCadena);
            documento.AddCreationDate();
            documento.Close();
        }
        return archivo;
    }
    //verifica numero a convertir
    public string montoLetras(string num)
    {
        string res, dec = "";
        Int64 entero;
        int decimales;
        double nro;

        try

        {
            nro = Convert.ToDouble(num);
        }
        catch
        {
            return "";
        }

        entero = Convert.ToInt64(Math.Truncate(nro));
        decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
        if (decimales > 0)
        {
            dec = " CON " + decimales.ToString() + "/100";
        }

        res = toText(Convert.ToDouble(entero)) + dec;
        return res;
    }
    //pasa monto a letra
    private string toText(double value)
    {
        string Num2Text = "";
        value = Math.Truncate(value);
        if (value == 0) Num2Text = "CERO";
        else if (value == 1) Num2Text = "UNO";
        else if (value == 2) Num2Text = "DOS";
        else if (value == 3) Num2Text = "TRES";
        else if (value == 4) Num2Text = "CUATRO";
        else if (value == 5) Num2Text = "CINCO";
        else if (value == 6) Num2Text = "SEIS";
        else if (value == 7) Num2Text = "SIETE";
        else if (value == 8) Num2Text = "OCHO";
        else if (value == 9) Num2Text = "NUEVE";
        else if (value == 10) Num2Text = "DIEZ";
        else if (value == 11) Num2Text = "ONCE";
        else if (value == 12) Num2Text = "DOCE";
        else if (value == 13) Num2Text = "TRECE";
        else if (value == 14) Num2Text = "CATORCE";
        else if (value == 15) Num2Text = "QUINCE";
        else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
        else if (value == 20) Num2Text = "VEINTE";
        else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
        else if (value == 30) Num2Text = "TREINTA";
        else if (value == 40) Num2Text = "CUARENTA";
        else if (value == 50) Num2Text = "CINCUENTA";
        else if (value == 60) Num2Text = "SESENTA";
        else if (value == 70) Num2Text = "SETENTA";
        else if (value == 80) Num2Text = "OCHENTA";
        else if (value == 90) Num2Text = "NOVENTA";
        else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
        else if (value == 100) Num2Text = "CIEN";
        else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
        else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
        else if (value == 500) Num2Text = "QUINIENTOS";
        else if (value == 700) Num2Text = "SETECIENTOS";
        else if (value == 900) Num2Text = "NOVECIENTOS";
        else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
        else if (value == 1000) Num2Text = "MIL";
        else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
        else if (value < 1000000)
        {
            Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
            if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
        }

        else if (value == 1000000) Num2Text = "UN MILLON";
        else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
        else if (value < 1000000000000)
        {
            Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
            if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
        }

        else if (value == 1000000000000) Num2Text = "UN BILLON";
        else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

        else
        {
            Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
            if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        }
        return Num2Text;

    }
    //no se esta usando 
    private void Mano_Obra(Document documento, int idEmpresa, int idTaller, int noOrden, int[] sessiones)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        //Obtener Datos Comparativo
        datosCotizaProv cotizacion = new datosCotizaProv();

        sql = "select rank() over(order by id_cliprov),id_cliprov from Cotizacion_Detalle where id_empresa=" + sessiones[2].ToString() + " and id_taller=" + sessiones[3].ToString() + " and no_orden=" + sessiones[4].ToString() + " and id_cotizacion=" + sessiones[6].ToString() + " group by id_cliprov";
        DataSet proveedoresCotizacion = new DataSet();
        object[] proveedoresCotizantes = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(proveedoresCotizantes[0]))
        {
            proveedoresCotizacion = (DataSet)proveedoresCotizantes[1];
        }

        int proveedoresTotalCotz = proveedoresCotizacion.Tables[0].Rows.Count;
        //meter proveedoresTotalCotz en la generacion de columnas para saber los proveedores a pintar

        object[] camposManObra = cotizacion.generaComparativo(sessiones);
        if (Convert.ToBoolean(camposManObra[0]))
        {
            DataSet datos = (DataSet)camposManObra[1];
            if (datos.Tables[0].Rows.Count != 0)
            {
                PdfPTable tablaEncabezadoFila = new PdfPTable(3);
                tablaEncabezadoFila.SetWidths(new float[] { 10F, 10F, (proveedoresTotalCotz * 20f) });
                tablaEncabezadoFila.WidthPercentage = 100f;
                PdfPTable tablaTitFila = new PdfPTable(proveedoresTotalCotz + 2);

                float[] arrAnchos = new float[proveedoresTotalCotz + 2];
                arrAnchos[0] = arrAnchos[1] = 10f;
                for (int contaFor = 2; contaFor < arrAnchos.Length; contaFor++)
                    arrAnchos[contaFor] = 20f;

                tablaTitFila.SetWidths(arrAnchos);
                tablaTitFila.WidthPercentage = 100f;
                PdfPCell cellTitu = new PdfPCell(new Phrase("Cantidad", FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cellTitu.Border = 0;
                tablaEncabezadoFila.AddCell(cellTitu);
                cellTitu = new PdfPCell(new Phrase("Descripción", FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cellTitu.Border = 0;
                tablaEncabezadoFila.AddCell(cellTitu);
                cellTitu = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cellTitu.Border = 0;
                tablaEncabezadoFila.AddCell(cellTitu);
                documento.Add(tablaEncabezadoFila);
                int refaccionesTotales = datos.Tables[0].Rows.Count;
                int countForeach = 1;
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    PdfPCell cellCantidad = new PdfPCell(new Phrase(fila[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cellCantidad.Border = 0;
                    cellCantidad.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tablaTitFila.AddCell(cellCantidad);
                    PdfPCell cellDescripcion = new PdfPCell(new Phrase(fila[3].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cellDescripcion.Border = 0;
                    cellDescripcion.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tablaTitFila.AddCell(cellDescripcion);
                    int rs = 5, cu = 6, pd = 7, id = 8, im = 9, ex = 10, ds = 11;
                    for (int cont = 0; cont < proveedoresTotalCotz; cont++)
                    {
                        PdfPTable tablaDetalle = new PdfPTable(1);
                        tablaDetalle.DefaultCell.Border = 0;
                        PdfPTable tablaDetalleNumeros = new PdfPTable(3);
                        tablaDetalleNumeros.DefaultCell.Border = 1;
                        if (countForeach == 1)
                        {
                            PdfPCell cellRazon = new PdfPCell(new Phrase(fila[rs].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellRazon.Border = 0;
                            cellRazon.BackgroundColor = BaseColor.LIGHT_GRAY;
                            cellRazon.VerticalAlignment = Element.ALIGN_MIDDLE;
                            tablaDetalle.AddCell(cellRazon);
                        }

                        if (fila[im].ToString() != "0" && fila[im].ToString() != "0.00")
                        {

                            PdfPCell cellCostoUnitario = new PdfPCell(new Phrase(fila[im].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellCostoUnitario.Border = 0;
                            cellCostoUnitario.Rowspan = 5;
                            cellCostoUnitario.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellCostoUnitario.HorizontalAlignment = Element.ALIGN_MIDDLE;
                            tablaDetalleNumeros.AddCell(cellCostoUnitario);

                            PdfPCell cellCostoUnitarioTitu = new PdfPCell(new Phrase("C. U.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellCostoUnitarioTitu.Border = 0;
                            cellCostoUnitarioTitu.HorizontalAlignment = 0;
                            tablaDetalleNumeros.AddCell(cellCostoUnitarioTitu);

                            PdfPCell cellCostoUnitarioMini = new PdfPCell(new Phrase(fila[cu].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellCostoUnitarioMini.Border = 0;
                            cellCostoUnitarioMini.HorizontalAlignment = 2;
                            tablaDetalleNumeros.AddCell(cellCostoUnitarioMini);

                            PdfPCell cellPorcDecto = new PdfPCell(new Phrase("Porc. Dto.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellPorcDecto.Border = 0;
                            cellPorcDecto.HorizontalAlignment = 0;
                            tablaDetalleNumeros.AddCell(cellPorcDecto);

                            PdfPCell cellPorcDectoMini = new PdfPCell(new Phrase(fila[pd].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellPorcDectoMini.Border = 0;
                            cellPorcDectoMini.HorizontalAlignment = 2;
                            tablaDetalleNumeros.AddCell(cellPorcDectoMini);

                            PdfPCell cellDescuento = new PdfPCell(new Phrase("Dto.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellDescuento.Border = 0;
                            cellDescuento.HorizontalAlignment = 0;
                            tablaDetalleNumeros.AddCell(cellDescuento);

                            PdfPCell cellDescuentoMini = new PdfPCell(new Phrase(fila[id].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellDescuentoMini.Border = 0;
                            cellDescuentoMini.HorizontalAlignment = 2;
                            tablaDetalleNumeros.AddCell(cellDescuentoMini);

                            PdfPCell cellExistencia = new PdfPCell(new Phrase("Exist.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellExistencia.Border = 0;
                            cellExistencia.HorizontalAlignment = 0;
                            tablaDetalleNumeros.AddCell(cellExistencia);

                            string truFal = "";
                            if (fila[ex].ToString() == "True")
                                truFal = "Si";
                            else
                                truFal = "No";
                            PdfPCell cellExistenciaMini = new PdfPCell(new Phrase(truFal, FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellExistenciaMini.Border = 0;
                            cellExistenciaMini.HorizontalAlignment = 1;
                            tablaDetalleNumeros.AddCell(cellExistenciaMini);

                            PdfPCell cellDiasEntregaTitu = new PdfPCell(new Phrase("Entrega", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                            cellDiasEntregaTitu.Border = 0;
                            cellDiasEntregaTitu.HorizontalAlignment = 0;
                            tablaDetalleNumeros.AddCell(cellDiasEntregaTitu);

                            PdfPCell cellDiasEntregaMini = new PdfPCell(new Phrase(fila[ds].ToString() + " día(s)", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellDiasEntregaMini.Border = 0;
                            cellDiasEntregaMini.HorizontalAlignment = 1;
                            tablaDetalleNumeros.AddCell(cellDiasEntregaMini);

                            tablaDetalle.AddCell(tablaDetalleNumeros);
                            tablaTitFila.AddCell(tablaDetalle);
                        }
                        else
                        {
                            PdfPCell cellVacio = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                            cellVacio.Border = 0;
                            tablaDetalle.AddCell(cellVacio);
                            tablaTitFila.AddCell(tablaDetalle);
                        }
                        rs += 9; cu += 9; pd += 9; id += 9; im += 9; ex += 9; ds += 9;
                    }

                    countForeach++;
                }
                documento.Add(tablaTitFila);
            }
        }
    }

    public DataSet llenaCatFact(string estatusFact, string Ini, string Fin)
    {
        DataSet datas = new DataSet();

        this.sql = "select distinct e.encReRFC,e.encReNombre,e.EncFolioUUID,e.EncReferencia,convert(char(10),e.EncFechaGenera,120)+' '+convert(char(8),e.enchoragenera,120) as EncFechaGenera,substring(t.fechatimbrado, 1, 10)+' '+substring(t.fechatimbrado, 12, 8) as fechatimbrado,convert(char(10), e.encfechaCancel, 120) + ' ' + convert(char(8), e.enchoracancel, 120) as fechaCancela,(e.encsubtotal + e.encdescglobimp) as neto,e.encdescglobimp as dctoGlob,e.encsubtotal as subtotal,e.encimptras as traslados,e.encimpret as retenciones,e.enctotal as total,e.encestatus as estatusFact  from EncCFD_f e left join receptores_f r on r.idrecep=e.idrecep LEFT JOIN TIMBRADO_f T ON T.IDCFD = E.IDCFD where e.encestatus='" + estatusFact + "' and  e.EncFechaGenera between '" + Ini + "' and '" + Fin + "'";
        object[] ejecutado = data.scalarData(sql);
        if ((bool)ejecutado[0])
            datas = (DataSet)ejecutado[1];
        else
            datas = null;
        return datas;
    }
    public void imprimeExcel()
    {
        try
        {
            ImprimeFacturaPrueba datosProducto = new ImprimeFacturaPrueba();

            DataSet data = new DataSet();

            data = datosProducto.llenaCatFact(estatusFact, Ini, Fin);

            if (data.Tables[0].Rows.Count > 0)
            {
                var libroExcel = new XLWorkbook();
                var hoja = libroExcel.Worksheets.Add("Facturas");

                int filas = 1;

                hoja.Cell(filas, 1).SetValue("RFC").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 2).SetValue("NOMBRE").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 3).SetValue("UUID").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 4).SetValue("FOLIO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 5).SetValue("FACTURA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 6).SetValue("FECHA GENERA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 7).SetValue("FECHA TIMBRADO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 8).SetValue("FECHA CANCELA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 9).SetValue("NETO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 10).SetValue("DESCUENTO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 11).SetValue("SUBTOTAL").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 12).SetValue("TRASLADOS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 13).SetValue("RETENCIONES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                hoja.Cell(filas, 14).SetValue("TOTAL").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                /*   hoja.Cell(filas, 15).SetValue("TALLER/TIENDA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 16).SetValue("ORDEN/TICKET").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 17).SetValue("VEHICULO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 18).SetValue("MODELO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 19).SetValue("COLOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 20).SetValue("PLACAS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 21).SetValue("SINIESTRO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 22).SetValue("MONTO PAGO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 23).SetValue("FECHA PAGO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                   hoja.Cell(filas, 24).SetValue("NOTAS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);*/

                string tiendaTaller = "";
                foreach (DataRow r in data.Tables[0].Rows)
                {
                    string estatusFactura = Convert.ToString(r[13]);
                    string estatus = "X";
                    if (estatusFactura == "T")
                    {

                        estatus = "T";
                    }
                    else if (estatusFactura == "C")
                        estatus = "C";



                    filas++;

                    switch (estatus)
                    {
                        case "C":
                            hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 2).SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 4).SetValue(" ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 5).SetValue(Convert.ToString(r[3])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 6).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 7).SetValue(Convert.ToString(r[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 8).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 9).SetValue(Convert.ToDecimal(r[7]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[8]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 11).SetValue(Convert.ToDecimal(r[9]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[10]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 13).SetValue(Convert.ToDecimal(r[11]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            hoja.Cell(filas, 14).SetValue(Convert.ToDecimal(r[12]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                            /*  hoja.Cell(filas, 15).SetValue(tiendaTaller).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 16).SetValue(Convert.ToString(r[14])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 17).SetValue(Convert.ToString(r[16])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 18).SetValue(Convert.ToString(r[17])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 19).SetValue(Convert.ToString(r[18])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 20).SetValue(Convert.ToString(r[19])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 21).SetValue(Convert.ToString(r[20])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 22).SetValue(Convert.ToDecimal(r[21]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 23).SetValue(Convert.ToString(r[22])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                             hoja.Cell(filas, 24).SetValue(estatus).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);*/
                            break;
                        case "T":
                            hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 2).SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 4).SetValue(" ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 5).SetValue(Convert.ToString(r[3])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 6).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 7).SetValue(Convert.ToString(r[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 8).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 9).SetValue(Convert.ToDecimal(r[7]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[8]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 11).SetValue(Convert.ToDecimal(r[9]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[10]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 13).SetValue(Convert.ToDecimal(r[11]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 14).SetValue(Convert.ToDecimal(r[12]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            /*  hoja.Cell(filas, 15).SetValue(tiendaTaller).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 16).SetValue(Convert.ToString(r[14])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 17).SetValue(Convert.ToString(r[16])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 18).SetValue(Convert.ToString(r[17])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 19).SetValue(Convert.ToString(r[18])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 20).SetValue(Convert.ToString(r[19])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 21).SetValue(Convert.ToString(r[20])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 22).SetValue(Convert.ToDecimal(r[21]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 23).SetValue(Convert.ToString(r[22])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                              hoja.Cell(filas, 24).SetValue(estatus).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);*/
                            break;
                        case "P":
                            hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 2).SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 4).SetValue(" ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 5).SetValue(Convert.ToString(r[3])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 6).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 7).SetValue(Convert.ToString(r[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 8).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 9).SetValue(Convert.ToDecimal(r[7]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[8]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 11).SetValue(Convert.ToDecimal(r[9]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[10]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 13).SetValue(Convert.ToDecimal(r[11]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            hoja.Cell(filas, 14).SetValue(Convert.ToDecimal(r[12]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                            /*    hoja.Cell(filas, 15).SetValue(tiendaTaller).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 16).SetValue(Convert.ToString(r[14])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 17).SetValue(Convert.ToString(r[16])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 18).SetValue(Convert.ToString(r[17])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 19).SetValue(Convert.ToString(r[18])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 20).SetValue(Convert.ToString(r[19])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 21).SetValue(Convert.ToString(r[20])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 22).SetValue(Convert.ToDecimal(r[21]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 23).SetValue(Convert.ToString(r[22])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                              hoja.Cell(filas, 24).SetValue(estatus).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);*/
                            break;
                        default:
                            hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 2).SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 4).SetValue(" ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 5).SetValue(Convert.ToString(r[3])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 6).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 7).SetValue(Convert.ToString(r[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 8).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 9).SetValue(Convert.ToDecimal(r[7]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[8]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 11).SetValue(Convert.ToDecimal(r[9]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[10]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 13).SetValue(Convert.ToDecimal(r[11]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            hoja.Cell(filas, 14).SetValue(Convert.ToDecimal(r[12]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                            /*  hoja.Cell(filas, 15).SetValue(tiendaTaller).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 16).SetValue(Convert.ToString(r[14])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 17).SetValue(Convert.ToString(r[16])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 18).SetValue(Convert.ToString(r[17])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 19).SetValue(Convert.ToString(r[18])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 20).SetValue(Convert.ToString(r[19])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 21).SetValue(Convert.ToString(r[20])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 22).SetValue(Convert.ToDecimal(r[21]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 23).SetValue(Convert.ToString(r[22])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                             hoja.Cell(filas, 24).SetValue(estatus).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);*/
                            break;
                    }
                }
                string ruta = HttpContext.Current.Server.MapPath("~/TMP");
                string archivoGuardado = ruta + "\\Facturas_" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "_" + estatusFact + ".xlsx";

                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                FileInfo docto = new FileInfo(archivoGuardado);
                if (docto.Exists)
                    docto.Delete();

                libroExcel.SaveAs(archivoGuardado);
                archivo = archivoGuardado;
            }
            else
                archivo = "";
        }
        catch (Exception ex) { archivo = ex.Message; }
    }
    private string obtieneNombreTaller(int taller)
    {
        sql = "select nombre_taller from talleres where id_taller=" + taller;
        return ejecuta.scalarToStringSimple(sql);
    }

    private string obtieneNombreTienda(int tienda)
    {
        sql = "select nombre_pv from punto_venta where id_punto=" + tienda + " and id_almacen=" + tienda;
        return ejecuta.scalarToStringSimple(sql);
    }
}