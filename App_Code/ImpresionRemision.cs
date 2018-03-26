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

/// <summary>
/// Descripción breve de ImpresionRemision
/// </summary>
public class ImpresionRemision
{
    Ejecuciones ejecuta = new Ejecuciones();
    Cadenas cad = new Cadenas();
    
    string sql;
    bool resultado;
    object[] datosEjecuta = new object[2];
    public ImpresionRemision()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GenRepOrdTrabajo(int empresa, int taller, int orden, string nombre_taller, string usuario, int idRemision, char tipo)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = "";
        if (tipo == 'R')
        {
            documento.AddTitle("OrdenRemision_E" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString());
            archivo = ruta + "\\" + "OrdenRemision_E" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString() + ".pdf";
        }
        else
        {
            documento.AddTitle("OrdenSalidaSinCargo_E" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString());
            archivo = ruta + "\\" + "OrdenSalidaSinCargo_E" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString() + ".pdf";
        }
        documento.AddCreator("MoncarWeb");

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

            string imagepath = HttpContext.Current.Server.MapPath("img/");
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo.png");
            gif.WidthPercentage = 40f;

            PdfPTable tablaEncabezado = new PdfPTable(2);
            tablaEncabezado.SetWidths(new float[] { 4f, 6f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;

            PdfPCell titu = new PdfPCell(new Phrase("N° Orden: " + orden, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 2;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(gif);
            tablaEncabezado.AddCell(titu);
            documento.Add(tablaEncabezado);

            DatosCab(documento, empresa, taller, orden, nombre_taller, usuario, idRemision, tipo);
            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void DatosCab(Document document, int empresa, int taller, int orden, string nombre_taller, string usuario, int idremision, char tipo)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        //Obtener Datos Aseguradora
        decimal totalMO = 0;
        decimal totalRef = 0;
        decimal totalPresupuesto = 0;
        Vehiculos CaracAseg = new Vehiculos();
        object[] camposOrden = CaracAseg.DatosAseguradora(orden, empresa, taller);
        DataSet datos = new DataSet();
        try { datos = (DataSet)camposOrden[1]; }
        catch (Exception) { datos = null; }
        if (datos != null)
        {
            int i = 0;
            foreach (DataRow valores in datos.Tables[0].Rows)
            {
                if (i < 1)
                {
                    Paragraph fecharep = new Paragraph("Fecha Ingreso: " + Convert.ToDateTime(valores[17]).ToString("dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                    fecharep.Alignment = Element.ALIGN_RIGHT;
                    document.Add(fecharep);
                    if (tipo == 'R')
                    {
                        Paragraph rep = new Paragraph("REMISION", FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                        rep.Alignment = Element.ALIGN_CENTER;
                        document.Add(rep);
                    }
                    else {
                        Paragraph rep = new Paragraph("SALIDA SIN CARGO", FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                        rep.Alignment = Element.ALIGN_CENTER;
                        document.Add(rep);
                    }

                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(valores[1].ToString(), _standardFont1));
                    document.Add(new Paragraph(valores[2].ToString(), _standardFont));
                    document.Add(new Paragraph("Col. " + valores[3].ToString(), _standardFont));
                    document.Add(new Paragraph("Del. " + valores[4].ToString(), _standardFont));
                    document.Add(new Paragraph(" ", _standardFont));
                    document.Add(new Paragraph("Presente", _standardFont2));
                    document.Add(new Paragraph("Atendiendo a sus apreciables ordenes nos permitimos presentar la siguiente remisión para la reparación de " + valores[5].ToString().ToUpper(), _standardFont));
                    document.Add(new Paragraph(" ", _standardFont));
                    document.Add(new Paragraph("Modelo: " + valores[6].ToString() + " con N° de placas " + valores[7].ToString(), _standardFont));
                    document.Add(new Paragraph("Propiedad de: " + valores[8].ToString(), _standardFont));
                    document.Add(new Paragraph("N° Póliza: " + valores[9].ToString(), _standardFont));
                    document.Add(new Paragraph("N° Siniestro: " + valores[10].ToString(), _standardFont));
                }
                i++;
            }
        }
        document.Add(new Paragraph(" ", estilo2));

        object[] camposRem = CaracAseg.datosEncabezadoRemisionSS(idremision, tipo, empresa, taller, orden);
        DataSet datosRmSS = new DataSet();
        try { datosRmSS = (DataSet)camposRem[1]; }
        catch (Exception) { datosRmSS = null; }
        if (datosRmSS != null) {
            document.Add(new Paragraph(" "));
            foreach (DataRow r in datosRmSS.Tables[0].Rows) {
                if (tipo == 'R')
                {
                    document.Add(new Paragraph("No. Remisión : " + r[0].ToString(), _standardFont));
                    totalMO = Convert.ToDecimal(r[4]);
                    totalRef = Convert.ToDecimal(r[5]);
                    totalPresupuesto = Convert.ToDecimal(r[6]);
                }
                else
                {
                    document.Add(new Paragraph("No. Salida Sin Cargo : " + r[0].ToString(), _standardFont));
                    totalMO = Convert.ToDecimal(r[4]);
                    totalRef = Convert.ToDecimal(r[5]);
                    totalPresupuesto = Convert.ToDecimal(r[6]);
                }
                document.Add(new Paragraph("Fecha: " + Convert.ToDateTime(Convert.ToString(r[1])+" "+Convert.ToString(r[2])).ToString("dd/MM/yyyy HH:mm:ss"), _standardFont));
                document.Add(new Paragraph("Usuario: " + r[3].ToString(), _standardFont));             
            }
            
        }
        document.Add(new Paragraph(" "));
        Mano_Obra(document, empresa, taller, orden, totalMO.ToString("C2"), tipo, idremision);
        Refacciones(document, empresa, taller, orden, totalRef.ToString("C2"), tipo, idremision);
        TotaPresupuesto(document, totalPresupuesto);

        PdfPTable tablaEmpty = new PdfPTable(1);
        tablaEmpty.WidthPercentage = 100f;
        tablaEmpty.DefaultCell.Border = 0;
        PdfPCell cellEmpty = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
        cellEmpty.Border = 0;
        tablaEmpty.AddCell(cellEmpty);

        document.Add(tablaEmpty); document.Add(tablaEmpty); document.Add(tablaEmpty); document.Add(tablaEmpty);
        
    }
    
    private void TotaPresupuesto(Document document, decimal totalPresupuesto)
    {

        PdfPTable tableTot = new PdfPTable(2);
        tableTot.WidthPercentage = 100f;
        tableTot.SetWidths(new float[] { 8f, 2f });

        PdfPCell cell = new PdfPCell(new Phrase("Total:", FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cell.BorderWidth = 0;
        cell.Padding = 5;
        cell.PaddingTop = 3;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
        PdfPCell cell2 = new PdfPCell(new Phrase(totalPresupuesto.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cell2.BorderWidth = 0;
        cell2.Padding = 5;
        cell2.PaddingTop = 3;
        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
        tableTot.AddCell(cell);
        tableTot.AddCell(cell2);
        Convertidores conversor = new Convertidores();
        conversor._importe = totalPresupuesto.ToString();

        cell2 = new PdfPCell(new Phrase("Importe con letra: "+ conversor.convierteMontoEnLetras().ToUpper().Trim(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cell2.BorderWidth = 0;
        cell2.PaddingRight = 8;
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        tableTot.AddCell(cell2);
        document.Add(tableTot);
    }

    private void Refacciones(Document document, int empresa, int taller, int orden, string totalRef, char tipo, int idremision)
    {
        // Tipo de Fuentes

        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();

        object[] camposManObra = CarManObra.DatosRefaccionesRemSS(idremision,tipo);

        if (Convert.ToBoolean(camposManObra[0]))
        {
            DataSet datos = (DataSet)camposManObra[1];
            if (datos.Tables[0].Rows.Count != 0)
            {
                PdfPTable tableR = new PdfPTable(1);
                PdfPCell cellR = new PdfPCell(new Phrase("R E F A C C I O N E S", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cellR.BorderWidth = 0;
                cellR.Padding = 5;
                cellR.PaddingTop = 3;
                cellR.HorizontalAlignment = Element.ALIGN_CENTER;
                cellR.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableR.AddCell(cellR);
                tableR.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);
                tableR.HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(tableR);

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 98f;
                table.SetWidths(new float[] { 8f, 2f });
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(fila[0].ToString() + "     " + fila[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.BorderWidth = 0;
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    decimal monto = 0;
                    /*if (tipo == 'R')
                    {*/
                    /*try {*/  monto = Convert.ToDecimal(fila[2]); /*} catch (Exception) { monto = 0; }*/
                    //}
                    PdfPCell cell2 = new PdfPCell(new Phrase(monto.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell2.BorderWidth = 0;
                    cell2.Padding = 2;
                    cell2.PaddingTop = 2;
                    cell2.PaddingBottom = 2;
                    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                    table.AddCell(cell2);
                }
                PdfPCell cellx = new PdfPCell(new Phrase("Total Refacciones:", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cellx.Border = 0;
                cellx.Padding = 2;
                cellx.PaddingTop = 2;
                cellx.PaddingBottom = 2;
                cellx.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell cell2x = new PdfPCell(new Phrase(totalRef, FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cell2x.Border = 0;
                cell2x.Padding = 2;
                cell2x.PaddingTop = 2;
                cell2x.PaddingBottom = 2;
                cell2x.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellx);
                table.AddCell(cell2x);

                table.HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(table);
            }
        }
    }

    private void Mano_Obra(Document document, int empresa, int taller, int orden, string totalMO, char tipo, int idremision)
    {
        // Tipo de Fuentes

        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();

        object[] camposManObra = CarManObra.DatosManoObraGrupoRemSS(idremision, tipo);

        if (Convert.ToBoolean(camposManObra[0]))
        {
            DataSet datos = (DataSet)camposManObra[1];

            if (datos.Tables[0].Rows.Count != 0)
            {
                PdfPTable tableT = new PdfPTable(1);
                PdfPCell cellT = new PdfPCell(new Phrase("M A N O   D E   O B R A", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cellT.BorderWidth = 0;
                cellT.Padding = 5;
                cellT.PaddingTop = 3;
                cellT.HorizontalAlignment = Element.ALIGN_CENTER;
                cellT.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableT.AddCell(cellT);
                tableT.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);
                tableT.HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(tableT);

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 98f;
                table.SetWidths(new float[] { 8f, 2f });
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(fila[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.Border = 0;
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    decimal monto = 0;
                    /*if (tipo == 'R')
                    {*/
                    try { monto = Convert.ToDecimal(fila[2]); } catch (Exception) { monto = 0; }
                    //}                    
                    PdfPCell cell2 = new PdfPCell(new Phrase(monto.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell2.Border = 0;
                    cell2.Padding = 2;
                    cell2.PaddingTop = 2;
                    cell2.PaddingBottom = 2;
                    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                    table.AddCell(cell2);
                    obtieneManosObra(document, empresa, taller, orden, fila[0].ToString(), table, tipo, idremision);
                }
                //table.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);

                PdfPCell cellx = new PdfPCell(new Phrase("Total Mano de Obra:", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cellx.Border = 0;
                cellx.Padding = 2;
                cellx.PaddingTop = 2;
                cellx.PaddingBottom = 2;
                cellx.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell cell2x = new PdfPCell(new Phrase(totalMO, FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cell2x.Border = 0;
                cell2x.Padding = 2;
                cell2x.PaddingTop = 2;
                cell2x.PaddingBottom = 2;
                cell2x.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cellx);
                table.AddCell(cell2x);

                table.HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(table);
            }
        }
    }

    private void obtieneManosObra(Document document, int empresa, int taller, int orden, string grupo, PdfPTable tabla, char tipo, int idRemision)
    {
        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();

        object[] camposManObra = CarManObra.DatosManoObraGrupoDetalleRemSS(idRemision, tipo, grupo);

        if (Convert.ToBoolean(camposManObra[0]))
        {
            DataSet datos = (DataSet)camposManObra[1];
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                PdfPCell cell = new PdfPCell(new Phrase(fila[0].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.BorderWidth = 0;
                cell.Padding = 2;
                cell.PaddingTop = 2;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                decimal monto = 0;
                PdfPCell cell2 = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                /*if (tipo == 'R')
                {*/
                try { monto = Convert.ToDecimal(fila[1]); } catch (Exception) { monto = 0; }
                //}
                cell2 = new PdfPCell(new Phrase(monto.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell2.BorderWidth = 0;
                cell2.Padding = 2;
                cell2.PaddingTop = 2;
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                tabla.AddCell(cell);
                tabla.AddCell(cell2);
            }
        }
    }       
}