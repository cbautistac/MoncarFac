﻿using System;
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
/// Descripción breve de ImpresionPresupuesto
/// </summary>
public class ImpresionPresupuesto
{
    Ejecuciones ejecuta = new Ejecuciones();
    string sql;
    bool resultado;
    object[] datosEjecuta = new object[2];
    decimal totalPresu;
    public ImpresionPresupuesto()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GenRepOrdTrabajo(int empresa, int taller, int orden, string nombre_taller, string usuario, int idFirmante, char detalle)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        documento.AddTitle("OrdenPresupuesto_E" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "OrdenPresupuesto_E" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
            Directory.CreateDirectory(ruta);

        FileInfo docto = new FileInfo(archivo);
        if (docto.Exists)
            docto.Delete();


        if (archivo.Trim() != "")
        {
            actualizaTotales(empresa, taller, orden);
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
            
            DatosCab(documento, empresa, taller, orden, nombre_taller, usuario, idFirmante, detalle);
            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void DatosCab(Document document, int empresa, int taller, int orden, string nombre_taller, string usuario, int idFirmante, char detalle)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        //Obtener Datos Aseguradora
        decimal totalMO = 0;
        decimal totalRef = 0;
        totalPresu = 0;
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
                    //try { totalMO = Convert.ToDecimal(valores[12]); } catch (Exception) { totalMO = 0; }
                    //try{ totalRef = Convert.ToDecimal(valores[13]); } catch (Exception) { totalRef = 0; }
                    //try{totalPresupuesto = Convert.ToDecimal(valores[14]); } catch (Exception) { totalPresupuesto = 0; }

                    Paragraph fecharep = new Paragraph("Fecha Ingreso: " + Convert.ToDateTime(valores[17]).ToString("dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                    fecharep.Alignment = Element.ALIGN_RIGHT;
                    document.Add(fecharep);
                    document.Add(new Paragraph(" "));

                    document.Add(new Paragraph(valores[1].ToString(), _standardFont1));
                    document.Add(new Paragraph(valores[2].ToString(), _standardFont));
                    document.Add(new Paragraph("Col. " + valores[3].ToString(), _standardFont));
                    document.Add(new Paragraph("Del. " + valores[4].ToString(), _standardFont));
                    document.Add(new Paragraph(" ", _standardFont));
                    document.Add(new Paragraph("Presente", _standardFont2));
                    document.Add(new Paragraph("Atendiendo a sus apreciables ordenes nos permitimos presentar el siguiente presupuesto para la reparación de " + valores[5].ToString().ToUpper(), _standardFont));
                    document.Add(new Paragraph(" ", _standardFont));
                    document.Add(new Paragraph("Modelo: " + valores[6].ToString() + " con N° de placas " + valores[7].ToString(), _standardFont));
                    document.Add(new Paragraph("Propiedad de: " + valores[8].ToString(), _standardFont));
                    document.Add(new Paragraph("N° Póliza: " + valores[9].ToString(), _standardFont));
                    document.Add(new Paragraph("N° Siniestro: " + valores[10].ToString(), _standardFont));
                    if (valores[19].ToString() != "")
                        document.Add(new Paragraph("N° Económico: " + valores[19].ToString(), _standardFont));
                }
                i++;
            } 
        }
        document.Add(new Paragraph(" ", estilo2));
        Mano_Obra(document, empresa, taller, orden, totalMO, detalle);
        Refacciones(document, empresa, taller, orden, totalRef);
        TotaPresupuesto(document, totalPresu);

        PdfPTable tablaEmpty = new PdfPTable(1);
        tablaEmpty.WidthPercentage = 100f;
        tablaEmpty.DefaultCell.Border = 0;
        PdfPCell cellEmpty = new PdfPCell(new Phrase(" ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
        cellEmpty.Border = 0;
        tablaEmpty.AddCell(cellEmpty);

        document.Add(tablaEmpty); document.Add(tablaEmpty); document.Add(tablaEmpty); document.Add(tablaEmpty);
         
        UltimoDocumento(document, idFirmante, orden, taller, empresa);
        DocumentoFooter(document, empresa);
    }

    private void DocumentoFooter(Document document, int idEmpresa)
    {
        string sql = "select 'Calle '+calle+' '+ltrim(rtrim(num_ext))+' '+ltrim(rtrim(num_int))+' '+colonia+' Deleg. '+municipio+' '+pais+' '+estado+' '+cp+' Tels. '+ltrim(rtrim(tel_principal))+' E-mail: mon-car@moncar.com.mx - www.moncar.com.mx' from empresas where id_empresa =" + idEmpresa.ToString();
        string footerDoc = "Calle Hidalgo L 171, Mza. 3-B Sta. Ma. Aztahuacan Deleg. Iztapalapa, México, D.F. Tel: 56935996 moncarsa.com.mx";//ejecuta.scalarToStringSimple(sql);
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
    
    private void UltimoDocumento(Document document, int idFirmante, int noOrden, int idTaller, int idEmpresa)
    {
        sql = "select firmante from firmantes where id_firma=" + idFirmante;
        string firmante = ejecuta.scalarToStringSimple(sql);

        sql= "select puesto from firmantes where id_firma=" + idFirmante;
        string puesto = ejecuta.scalarToStringSimple(sql);

        sql = "select observaciones from ordenes_reparacion where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString();
        string observacionesOrden = ejecuta.scalarToStringSimple(sql);

        PdfPTable tableFin = new PdfPTable(2);
        PdfPTable tableFirma = new PdfPTable(1);
        tableFin.WidthPercentage = 100f;
        tableFin.SetWidths(new float[] { 4f, 6f });
        tableFirma.WidthPercentage = 100f;
        tableFin.DefaultCell.Border = 0;
        tableFirma.DefaultCell.Border = 0;

        PdfPCell cell1 = new PdfPCell(new Phrase("Atentamente", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
        cell1.HorizontalAlignment = 1;
        cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
        cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
        cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase("Observaciones:", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
        cell4.Border = 0;
        cell4.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell cell5 = new PdfPCell(new Phrase(firmante, FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cell5.Border = 0;
        cell5.HorizontalAlignment = 1;
        PdfPCell cell6 = new PdfPCell(new Phrase(observacionesOrden, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
        cell6.Border = 0;
        cell6.HorizontalAlignment = Element.ALIGN_LEFT;
        cell6.PaddingLeft = 1;
        PdfPCell cell7 = new PdfPCell(new Phrase(puesto.ToUpper(), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
        cell7.Border = 0;
        cell7.HorizontalAlignment = 1;
        tableFirma.AddCell(cell5);
        tableFirma.AddCell(cell7);
        tableFin.AddCell(cell1);
        tableFin.AddCell(cell2);
        tableFin.AddCell(cell3);
        tableFin.AddCell(cell4);
        tableFin.AddCell(tableFirma);
        tableFin.AddCell(cell6);
        document.Add(tableFin);
    }

    private void TotaPresupuesto(Document document, decimal totalPresupuesto)
    {

        PdfPTable tableTot = new PdfPTable(2);
        tableTot.WidthPercentage = 100f;
        tableTot.SetWidths(new float[] { 8f, 2f });

        PdfPCell cell = new PdfPCell(new Phrase("Total de este presupuesto:", FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
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
        cell2 = new PdfPCell(new Phrase("Nota: Los precios anteriores no incluyen I.V.A.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cell2.BorderWidth = 0;
        cell2.PaddingRight = 8;
        cell2.Colspan = 2;
        cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
        tableTot.AddCell(cell2);
        document.Add(tableTot);
    }

    private void Refacciones(Document document, int empresa, int taller, int orden, decimal totalRef)
    {
        // Tipo de Fuentes

        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();

        object[] camposManObra = CarManObra.DatosRefacciones(orden, empresa, taller);

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
                    PdfPCell cell = new PdfPCell(new Phrase(fila[1].ToString() + "     " + fila[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.BorderWidth = 0;
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    decimal monto = 0;
                    try { monto = Convert.ToDecimal(fila[3]); } catch (Exception) { monto = 0; }
                    PdfPCell cell2 = new PdfPCell(new Phrase(monto.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell2.BorderWidth = 0;
                    cell2.Padding = 2;
                    cell2.PaddingTop = 2;
                    cell2.PaddingBottom = 2;
                    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                    table.AddCell(cell2);
                    totalRef = totalRef + monto;
                }
                totalPresu = totalPresu + totalRef;
                PdfPCell cellx = new PdfPCell(new Phrase("Total Refacciones:", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cellx.Border = 0;
                cellx.Padding = 2;
                cellx.PaddingTop = 2;
                cellx.PaddingBottom = 2;
                cellx.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell cell2x = new PdfPCell(new Phrase(totalRef.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
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

    private void Mano_Obra(Document document, int empresa, int taller, int orden, decimal totalMO, char detalle)
    {
        // Tipo de Fuentes

        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();

        object[] camposManObra = CarManObra.DatosManoObraGrupo(orden, empresa, taller);

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
                    try { monto = Convert.ToDecimal(fila[2]); } catch (Exception) { monto = 0; }
                    PdfPCell cell2 = new PdfPCell(new Phrase(monto.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell2.Border = 0;
                    cell2.Padding = 2;
                    cell2.PaddingTop = 2;
                    cell2.PaddingBottom = 2;
                    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cell);
                    table.AddCell(cell2);                    
                    obtieneManosObra(document, empresa, taller, orden, fila[0].ToString(), table, detalle);
                    totalMO = totalMO + monto;
                }
                totalPresu = totalPresu + totalMO;
                //table.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);

                PdfPCell cellx = new PdfPCell(new Phrase("Total Mano de Obra:", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cellx.Border = 0;
                cellx.Padding = 2;
                cellx.PaddingTop = 2;
                cellx.PaddingBottom = 2;
                cellx.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell cell2x = new PdfPCell(new Phrase(totalMO.ToString("C"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
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

    private void obtieneManosObra(Document document, int empresa, int taller, int orden, string grupo, PdfPTable tabla, char detalle)
    {

        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();

        object[] camposManObra = CarManObra.DatosManoObraGrupoDetalle(orden, empresa, taller, grupo);

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
                if (detalle == 'S')
                {
                    try { monto = Convert.ToDecimal(fila[4]); }
                    catch (Exception) { monto = 0; }
                    cell2 = new PdfPCell(new Phrase(monto.ToString("c2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));                    
                }
                cell2.BorderWidth = 0;
                cell2.Padding = 2;
                cell2.PaddingTop = 2;
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                tabla.AddCell(cell);                
                tabla.AddCell(cell2);
            }
        }
    }

    private void actualizaTotales(int empresa, int taller, int orden)
    {
        decimal totMo = 0, totRef = 0, totalOrden = 0;
        TotalesOrden totales = new TotalesOrden();
        // Total Mano Obra
        totales.Empresa = empresa;
        totales.Taller = taller;
        totales.Orden = orden;
        totales.obtieneTotalManoObra();
        totMo = totales.ManoObra;


        // Total Refacciones
        totales.obtieneTotalManoRefacciones();
        totRef = totales.Refacciones;

        totalOrden = totMo + totRef;
        totales.Importe = totalOrden;
        totales.actualizaTotales();        
    }
}