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
/// Descripción breve de ImprimeOrdenes
/// </summary>
public class ImprimeOrdenes
{
    Ejecuciones ejecuta = new Ejecuciones();
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();
    public ImprimeOrdenes()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GeneraReporte(string idEmpresa, string idTaller, string condicion, string fechaIni, string fechaFin, string cliente)
    {
        float margen = 2;
        float margenes = 1;
        // Crear documento        
        Document documento = new Document(iTextSharp.text.PageSize.LETTER.Rotate(), 0.5f, 0.5f, 1f, 1f);
        documento.AddTitle("Ordenes_E" + idEmpresa.ToString() + "_T" + idTaller.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "Ordenes_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
            Directory.CreateDirectory(ruta);

        FileInfo archivoReporte = new FileInfo(archivo);
        if (archivoReporte.Exists)
            archivoReporte.Delete();


        if (archivo.Trim() != "")
        {
            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            string empresa = ejecuta.scalarToStringSimple("select razon_social from empresas where id_empresa=" + idEmpresa.ToString());
            string taller = ejecuta.scalarToStringSimple("select nombre_taller from talleres where id_taller=" + idTaller);
            // Abrir documento.
            documento.Open();
            /*
            string imagepath = HttpContext.Current.Server.MapPath("img/");
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo.png");
            gif.WidthPercentage = 40f;
            */
            PdfPTable tablaEncabezado = new PdfPTable(1);
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;

            PdfPCell titu = new PdfPCell(new Phrase("Ordenes en Proceso", FontFactory.GetFont("ARIAL", 15, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 1;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(titu);
            titu = new PdfPCell(new Phrase(taller, FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 1;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(titu);
            titu = new PdfPCell(new Phrase("Periodo del " + Convert.ToDateTime(fechaIni).ToString("dd/MM/yyyy") + " al " + Convert.ToDateTime(fechaFin).ToString("dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 13, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 1;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(titu);
            tablaEncabezado.AddCell(" ");
            documento.Add(tablaEncabezado);

            DatosCab(documento, idEmpresa, idTaller, condicion, fechaIni, fechaFin, cliente);
            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void DatosCab(Document document, string idEmpresa, string idTaller, string condicion, string fechaIni, string fechaFin, string cliente)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font _standardFont9 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont9B = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont11B = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont11 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        
        //Obtener Datos Aseguradora       
        PdfPTable tablaEncabezadoValuacion = new PdfPTable(15);
        //tablaEncabezadoValuacion.SetWidths(new float[] { });
        tablaEncabezadoValuacion.WidthPercentage = 100f;
        tablaEncabezadoValuacion.DefaultCell.Border = 0;
        PdfPCell cellTitu = new PdfPCell();
        string[] titulos = new string[15] { "Orden", "Fecha Recepción", "Compañía", "Vehículo", "Color", "Placas", "Siniestro", "% Avance", "Fecha Estimada", "Fecha Asignación", "Fecha Entrega Real", "Días Atraso", "Estatus", "Localización", "Perfil" };
        for (int i = 0; i < 15; i++) {
            cellTitu = new PdfPCell(new Phrase(titulos[i], FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidthTop = cellTitu.BorderWidthBottom = 0;
            cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
            cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTitu.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablaEncabezadoValuacion.AddCell(cellTitu);
        }        
        document.Add(tablaEncabezadoValuacion);


        object[] ordens = recepciones.obtieneReporteOrdenes2(idEmpresa, idTaller, condicion, fechaIni, fechaFin, fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss"));
              

        if (Convert.ToBoolean(ordens[0]))
        {
            DataSet info = (DataSet)ordens[1];
            foreach (DataRow filaOrd in info.Tables[0].Rows)
            {
                PdfPTable tabla = new PdfPTable(15);
                tabla.DefaultCell.Border = 0;
                tabla.WidthPercentage = 100f;

                PdfPCell cell = new PdfPCell();
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[0]), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[7]), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[5]), FontFactory.GetFont(FontFactory.HELVETICA, 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[1]), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[2]), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[3]), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[8]), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[12])+"%", FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(validafecha(Convert.ToString(filaOrd[13])), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(validafecha(Convert.ToString(filaOrd[14])), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(validafecha(Convert.ToString(filaOrd[15])), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[16]), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[17]).ToUpper(), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[4]).ToUpper(), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);
                cell = new PdfPCell(new Phrase(Convert.ToString(filaOrd[10]).ToUpper(), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                tabla.AddCell(cell);

                document.Add(tabla);
                
            }
        }


        /*object[] estatusOrdenes = recepciones.obtieneEstatusOrdenes(idEmpresa, idTaller, condicion, fechaIni, fechaFin);
        if (Convert.ToBoolean(estatusOrdenes[0])) {
            DataSet infoEstatus = (DataSet)estatusOrdenes[1];
            foreach (DataRow filaWEstatus in infoEstatus.Tables[0].Rows) {
                Paragraph estatus = new Paragraph(Convert.ToString(filaWEstatus[1]).ToUpper(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                estatus.Alignment = Element.ALIGN_LEFT;
                document.Add(estatus);
                string valorEstatus = Convert.ToString(filaWEstatus[0]);
                object[] localizaciones = recepciones.obtieneLocalizacionesOrdenes(idEmpresa, idTaller, condicion, fechaIni, fechaFin, valorEstatus);
                if (Convert.ToBoolean(localizaciones[0]))
                {
                    DataSet infoLoc = (DataSet)localizaciones[1];
                    foreach (DataRow filaLoc in infoLoc.Tables[0].Rows) {
                        Paragraph localizacion = new Paragraph(Convert.ToString(filaLoc[1]).ToUpper(), FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                        localizacion.Alignment = Element.ALIGN_LEFT;
                        document.Add(localizacion);
                        string valorLoc = Convert.ToString(filaLoc[0]);
                        object[] perfiles = recepciones.obtienePefilesOrdenes(idEmpresa, idTaller, condicion, fechaIni, fechaFin, valorEstatus, valorLoc);
                        if (Convert.ToBoolean(perfiles[0]))
                        {
                            DataSet infoPerfiles = (DataSet)perfiles[1];
                            foreach (DataRow filaPerf in infoPerfiles.Tables[0].Rows)
                            {
                                Paragraph perfil = new Paragraph(Convert.ToString(filaPerf[1]).ToUpper(), FontFactory.GetFont("ARIAL", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                                perfil.Alignment = Element.ALIGN_LEFT;
                                document.Add(perfil);
                                string valorPefil = Convert.ToString(filaPerf[0]);
                                object[] ordens = recepciones.obtieneReporteOrdenes(idEmpresa, idTaller, condicion, fechaIni, fechaFin, valorEstatus, valorLoc, valorPefil,fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss"), cliente);

                                PdfPTable tablaDetalle = new PdfPTable(12);
                                tablaDetalle.DefaultCell.Border = 0;
                                tablaDetalle.WidthPercentage = 100f;

                                if (Convert.ToBoolean(ordens[0]))
                                {
                                    DataSet info = (DataSet)ordens[1];
                                    foreach (DataRow filaOrd in info.Tables[0].Rows) {
                                        PdfPCell cell = new PdfPCell();
                                        string[] detalle = new string[12] {
                                            Convert.ToString(filaOrd[0]),
                                            Convert.ToString(filaOrd[7]) ,
                                            Convert.ToString(filaOrd[5]) ,
                                            Convert.ToString(filaOrd[1]) ,
                                            Convert.ToString(filaOrd[2]) ,
                                            Convert.ToString(filaOrd[3]) ,
                                            Convert.ToString(filaOrd[8]) ,
                                            Convert.ToString(filaOrd[12])+"%" ,
                                            Convert.ToString(filaOrd[13]) ,
                                            Convert.ToString(filaOrd[14]) ,
                                            Convert.ToString(filaOrd[15]) ,
                                            Convert.ToString(filaOrd[16]) };
                                        for (int i = 0; i < 12; i++)
                                        {
                                            cell = new PdfPCell(new Phrase(detalle[i], FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                                            cell.Border = 0;
                                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            tablaDetalle.AddCell(cell);
                                        }
                                        document.Add(tablaDetalle);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }*/

    }

    private string validafecha(string fecha)
    {
        if (fecha == "1900-01-01")
            return "";
        else
            return fecha;
    }
}