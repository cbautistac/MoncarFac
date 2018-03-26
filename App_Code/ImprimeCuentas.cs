using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ImprimeCuentas
/// </summary>
public class ImprimeCuentas
{
    E_Utilities.Fechas fechas = new E_Utilities.Fechas();

    public ImprimeCuentas()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }


    public string generaReporte(DataTable datos, string tipo) {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER.Rotate());
        documento.AddTitle(" Informe de Costo por Unidad ");
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = "";
        if(tipo=="PA")
            archivo = ruta + "\\CuentasPagar_" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + ".pdf";
        else
            archivo = ruta + "\\CuentasCobrar_" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + ".pdf";

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
            tablaEncabezado.SetWidths(new float[] { 2f, 8f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;


            PdfPCell titu = new PdfPCell(new Phrase("Moncar Aztahucan S.A de C.V. ", FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 1;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablaEncabezado.AddCell(gif);
            tablaEncabezado.AddCell(titu);
            documento.Add(tablaEncabezado);

            /*

            Datosencab(documento, empresa, taller, orden);
            datosManoObra(documento, orden, empresa, taller);
            datosRefaccion(documento, orden, empresa, taller);
            datospintura(documento, orden, empresa, taller);
            datosCC(documento, orden, empresa, taller);
            datosalmacen(documento, orden, empresa, taller);
            datosCostoFijo(documento, orden, empresa, taller);
            datosTotalReparacion(documento);
            
             */

            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();


        }
        return archivo;

    }
}