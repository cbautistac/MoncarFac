using E_Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de imprimeestatusf
/// </summary>
public class imprimeestatusf
{
    private PdfPCell celda;
    Fechas fechas = new Fechas();
    public imprimeestatusf()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }


    public string imprimeestatusfa(string fechaIni, string fechaFin)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER.Rotate());
        documento.AddTitle(" Reporte Estatus Facturas");
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\estatusfacturas_" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
            Directory.CreateDirectory(ruta);

        FileInfo docto = new FileInfo(archivo);
        if (docto.Exists)
            docto.Delete();
        if (archivo.Trim() != "")
        {

            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);

            // Abrir documento.
            documento.Open();

            //Insertar logo o imagen  

            string imagepath = HttpContext.Current.Server.MapPath("img/");
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo.png");
            gif.WidthPercentage = 5f;


            PdfPTable tablaEncabezado = new PdfPTable(2);
            tablaEncabezado.SetWidths(new float[] { 4f, 6f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;


            PdfPCell titu = new PdfPCell(new Phrase("Moncar Aztahucan S.A de C.V. " + Environment.NewLine + Environment.NewLine + " Reporte Facturas ", FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 1;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablaEncabezado.AddCell(gif);
            tablaEncabezado.AddCell(titu);
            documento.Add(tablaEncabezado);


            datosestatusfacturas(documento, fechaIni, fechaFin);

            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }

        return archivo;
    }

    private void datosestatusfacturas(Document document, string fechaini, string fechafin)
    {
        // Tipo de Font que vamos utilizar        
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        Paragraph paragraph = new Paragraph();
        
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        document.Add(paragraph);

        PdfPTable tbldatos2 = new PdfPTable(6);
        tbldatos2.WidthPercentage = 100f;

        PdfPCell fecha = new PdfPCell(new Paragraph("Fecha", fuenteB));
        PdfPCell uuid = new PdfPCell(new Paragraph("UUID", fuenteB));       
        PdfPCell referen = new PdfPCell(new Paragraph("Referencia externa", fuenteB));
        PdfPCell rfc = new PdfPCell(new Paragraph("Emitida al RFC", fuenteB));
        PdfPCell prove = new PdfPCell(new Paragraph("Proveedor", fuenteB));
        PdfPCell estat = new PdfPCell(new Paragraph("Estatus", fuenteB));



        fecha.HorizontalAlignment = Element.ALIGN_LEFT;
        fecha.Padding = 2;
        fecha.Border = 0;
        fecha.BackgroundColor = referen.BackgroundColor = rfc.BackgroundColor = estat.BackgroundColor = prove.BackgroundColor = uuid.BackgroundColor = BaseColor.LIGHT_GRAY;
        uuid.HorizontalAlignment = Element.ALIGN_CENTER;
        uuid.Padding = 2;
        uuid.Border = 0;      
        referen.HorizontalAlignment = Element.ALIGN_CENTER;
        referen.Padding = 2;
        referen.Border = 0;
        rfc.HorizontalAlignment = Element.ALIGN_CENTER;
        rfc.Padding = 2;
        rfc.Border = 0;
        prove.HorizontalAlignment = Element.ALIGN_CENTER;
        prove.Padding = 2;
        prove.Border = 0;
        estat.HorizontalAlignment = Element.ALIGN_CENTER;
        estat.Padding = 2;
        estat.Border = 0;




        tbldatos2.AddCell(fecha);
        tbldatos2.AddCell(uuid);       
        tbldatos2.AddCell(referen);
        tbldatos2.AddCell(rfc);        
        tbldatos2.AddCell(prove);
        tbldatos2.AddCell(estat);


        document.Add(tbldatos2);


        estatusfac est = new estatusfac();
        est.estatus = "";
        est.fechaini = fechaini;
        est.fechafin = fechafin;
        object[] infoestatusfact = est.obtieneestatusfactur();
        if (Convert.ToBoolean(infoestatusfact[0]))
        {
            DataSet ds = (DataSet)infoestatusfact[1];

            PdfPTable detalle = new PdfPTable(6);
            detalle.WidthPercentage = 100f;


            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                PdfPCell celda = new PdfPCell();

                try { celda = new PdfPCell(new Paragraph(Convert.ToDateTime(fila[3]).ToString("dd/MM/yyyy"), fuente2)); }
                catch (Exception)
                {
                    celda = new PdfPCell(new Paragraph("", fuente2));
                }
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToString(fila[1]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);                

                celda = new PdfPCell(new Paragraph(Convert.ToString(fila[2]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);
                celda = new PdfPCell(new Paragraph(Convert.ToString(fila[4]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);               
                celda = new PdfPCell(new Paragraph(Convert.ToString(fila[5]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);
                celda = new PdfPCell(new Paragraph(Convert.ToString(fila[7]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);
            }
            document.Add(detalle);
        }
    }

    }






