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

/// <summary>
/// Descripción breve de ImprimirValuacionGral
/// </summary>
public class ImprimirValuacionGral
{
    Ejecuciones ejecuta = new Ejecuciones();
    string sql;
    bool resultado;
    object[] ejecutados = new object[2];

    public ImprimirValuacionGral()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string generaComparativoGeneral(int[] sessiones)
    {
        int noOrden = sessiones[4];
        int idEmpresa = sessiones[2];
        int idTaller = sessiones[3];
        int idUsuario = sessiones[0];
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LEGAL.Rotate());
        documento.AddTitle("ComparativoCotizacion_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "ValuacionGeneral_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + ".pdf";
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
            gif.WidthPercentage = 15f;

            PdfPTable tablaEncabezado = new PdfPTable(3);
            tablaEncabezado.SetWidths(new float[] { 2.5f, 5f, 2.5f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;

            string prefijoTaller = ejecuta.scalarToStringSimple("select ltrim(rtrim(identificador)) from talleres where id_taller=" + idTaller.ToString());

            PdfPCell tituPagina = new PdfPCell(new Phrase("Valuación", FontFactory.GetFont("ARIAL", 16, iTextSharp.text.Font.BOLD)));
            tituPagina.HorizontalAlignment = 1;
            tituPagina.VerticalAlignment = 1;
            tituPagina.Border = 0;
            tituPagina.VerticalAlignment = Element.ALIGN_BOTTOM;

            PdfPCell titu = new PdfPCell(new Phrase("N° Orden: " + prefijoTaller + " " + noOrden, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 2;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(gif);
            tablaEncabezado.AddCell(tituPagina);
            tablaEncabezado.AddCell(titu);

            documento.Add(tablaEncabezado);

            generaValuacion(documento, idEmpresa, idTaller, noOrden, sessiones);
            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void generaValuacion(Document documento, int idEmpresa, int idTaller, int noOrden, int[] sessiones)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        //Obtener Datos Comparativo
        datosCotizaProv cotizacion = new datosCotizaProv();
        sql = obtieneQuery(noOrden, idEmpresa, idTaller);
        DataSet proveedoresCotizacion = new DataSet();
        object[] proveedoresCotizantes = ejecuta.dataSet(sql);

        if (Convert.ToBoolean(proveedoresCotizantes[0]))
            proveedoresCotizacion = (DataSet)proveedoresCotizantes[1];

        PdfPTable tablaEncabezadoValuacion = new PdfPTable(13);
        //tablaEncabezadoValuacion.SetWidths(new float[] { });
        tablaEncabezadoValuacion.WidthPercentage = 100f;

        PdfPCell cellTitu = new PdfPCell();

        cellTitu = new PdfPCell(new Phrase("Cant.", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Refacción", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Proveedor", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Cost. Unit.", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("% Dto.", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Importe Dto.", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Importe Compra", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("% S.C.", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Precio Unit. Autorizado", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Importe Autorizado", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Utilidad", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Est. Presupuesto", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase("Est. Refacción", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidth = 0;
        cellTitu.Padding = 5;
        cellTitu.PaddingTop = 3;
        cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
        cellTitu.BackgroundColor = BaseColor.LIGHT_GRAY;
        tablaEncabezadoValuacion.AddCell(cellTitu);
        decimal utilidad = 0;
        decimal importeAutorizado = 0;
        decimal importeCompra = 0;
        decimal totalUtilidad = 0;
        decimal totalImporteAutorizado = 0;
        decimal totalImporteCompra = 0;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        foreach (DataRow fila in proveedoresCotizacion.Tables[0].Rows)
        {
            //"Cant."
            cellTitu = new PdfPCell(new Phrase(fila[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //refaccion
            cellTitu = new PdfPCell(new Phrase(fila[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //proveedor
            cellTitu = new PdfPCell(new Phrase(fila[5].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_CENTER;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //costo unitario
            cellTitu = new PdfPCell(new Phrase(Convert.ToDecimal(fila[6]).ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //%descuento
            cellTitu = new PdfPCell(new Phrase(Convert.ToDecimal(fila[7]).ToString("F2")+" %", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //importe descuento
            cellTitu = new PdfPCell(new Phrase(Convert.ToDecimal(fila[8]).ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //importe compra
            try { importeCompra = Convert.ToDecimal(fila[9]); } catch (Exception) { importeCompra = 0; } finally { totalImporteCompra += importeCompra; }
            cellTitu = new PdfPCell(new Phrase(importeCompra.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //%SC
            cellTitu = new PdfPCell(new Phrase(Convert.ToDecimal(fila[16]).ToString("F2")+" %", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //"Precio Unit. Autorizado."
            cellTitu = new PdfPCell(new Phrase(Convert.ToDecimal(fila[3]).ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //"Importe Autorizado"
            try { importeAutorizado = Convert.ToDecimal(fila[10]); } catch (Exception) { importeAutorizado = 0; } finally { totalImporteAutorizado += importeAutorizado; }
            cellTitu = new PdfPCell(new Phrase(Convert.ToDecimal(fila[10]).ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //"Utilidad"
            try { utilidad = Convert.ToDecimal(fila[15]); } catch (Exception) { utilidad = 0; } finally { totalUtilidad += utilidad; }
            if(utilidad==0)
                cellTitu = new PdfPCell(new Phrase(utilidad.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.YELLOW)));
            else if (utilidad > 0)
                cellTitu = new PdfPCell(new Phrase(utilidad.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.GREEN)));
            else if (utilidad < 0)
                cellTitu = new PdfPCell(new Phrase(utilidad.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.RED)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //"Est. Presupuesto."
            cellTitu = new PdfPCell(new Phrase(fila[19].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
            //"Est. Refacción"
            cellTitu = new PdfPCell(new Phrase(fila[14].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
            cellTitu.BorderWidth = 0;
            cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablaEncabezadoValuacion.AddCell(cellTitu);
        }
        cellTitu = new PdfPCell(new Phrase("Total:", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        tablaEncabezadoValuacion.AddCell(cellTitu);
        cellTitu = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        tablaEncabezadoValuacion.AddCell(cellTitu);
        cellTitu = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        tablaEncabezadoValuacion.AddCell(cellTitu);
        cellTitu = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        tablaEncabezadoValuacion.AddCell(cellTitu);
        cellTitu = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop= 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        tablaEncabezadoValuacion.AddCell(cellTitu);
                
        cellTitu = new PdfPCell(new Phrase(totalImporteCompra.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        tablaEncabezadoValuacion.AddCell(cellTitu);
        cellTitu = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        tablaEncabezadoValuacion.AddCell(cellTitu);
                
        cellTitu = new PdfPCell(new Phrase(totalImporteAutorizado.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        if (totalUtilidad == 0)
            cellTitu = new PdfPCell(new Phrase(totalUtilidad.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.YELLOW)));
        else if (totalUtilidad > 0)
            cellTitu = new PdfPCell(new Phrase(totalUtilidad.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.GREEN)));
        else if (totalUtilidad < 0)
            cellTitu = new PdfPCell(new Phrase(totalUtilidad.ToString("C2"), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.RED)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        decimal porcUtilidad = 0;
        porcUtilidad = ((totalUtilidad / totalImporteCompra) * 100);

        if (porcUtilidad == 0)
            cellTitu = new PdfPCell(new Phrase(porcUtilidad.ToString("F2") + " %", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.YELLOW)));
        else if (porcUtilidad > 0)
            cellTitu = new PdfPCell(new Phrase(porcUtilidad.ToString("F2") + " %", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.GREEN)));
        else if (porcUtilidad < 0)
            cellTitu = new PdfPCell(new Phrase(porcUtilidad.ToString("F2") + " %", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.RED)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        cellTitu.HorizontalAlignment = Element.ALIGN_RIGHT;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        cellTitu = new PdfPCell(new Phrase(" ", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
        cellTitu.BorderWidthTop = 1;
        cellTitu.BorderWidthBottom = cellTitu.BorderWidthLeft = cellTitu.BorderWidthRight = 0;
        tablaEncabezadoValuacion.AddCell(cellTitu);

        documento.Add(tablaEncabezadoValuacion);
    }

    private string obtieneQuery(int noOrden, int idEmpresa, int idTaller)
    {
        sql = "declare @orden int "+
              "declare @empresa int " +
              "declare @taller int " +
              "declare @acceso int " +
              " set @orden =" +noOrden+
              " set @empresa =" +idEmpresa+
              " set @taller =" +idTaller+
              " set @acceso = 1 " +
              "select r.refOrd_Id,r.refCantidad,r.refDescripcion,r.refPrecioVenta, " +
              "case @acceso when 0 then " +
              "case r.refEstatus when 'AU' THEN r.refProveedor else " +
              "(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
              "else r.refProveedor end  as refProveedor, " +
              "(select razon_social from Cliprov where tipo = 'P' and id_cliprov in (select case @acceso when 0 then " +
              "case r.refEstatus when 'AU' THEN r.refProveedor else " +
              "(select isnull((SELECT id_cliprov from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
              "else r.refProveedor end)) as razon_social, " +
              "case @acceso when 0 then " +
              "case r.refEstatus when 'AU' THEN r.refCosto else " +
              "(select isnull((SELECT costo_unitario from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
              "else r.refCosto end  as refCosto, " +
              "case @acceso when 0 then " +
              "case r.refEstatus when 'AU' THEN " +
              "(SELECT isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor), 0)) " +
              "else " +
              "(select isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
              "else " +
              "(SELECT isnull((SELECT porc_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),0)) end " +
              "as porc_desc, " +
              "case @acceso when 0 then " +
              "case r.refEstatus when 'AU' THEN " +
              "(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor), 0)) " +
              "else " +
              "(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
              "else " +
              "(SELECT isnull((SELECT importe_desc from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),0)) end " +
              "as importeDesc, " +
              "case @acceso when 0 then " +
              "case r.refEstatus when 'AU' THEN " +
              "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor), 0)) " +
              "else " +
              "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
              "else " +
              "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),0)) end " +
              "as importeCosto, " +
              "isnull((r.refCantidad * r.refPrecioVenta), 0) as importeVenta,r.refEstatus, " +
              "isnull((SELECT estatus from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),'CAN') as estatusRef,r.refEstatusSolicitud, " +
              "(select staDescripcion from rafacciones_estatus where staRefId = r.refEstatusSolicitud) as descripEstatus, " +
              "(isnull((r.refCantidad * r.refPrecioVenta), 0) - " +
              "(select case @acceso when 0 then " +
              "case r.refEstatus when 'AU' THEN " +
              "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor), 0)) " +
              "else " +
              "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.id_cliprov_cotizacion),0)) end " +
              "else " +
              "(SELECT isnull((SELECT importe from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = ref_id_taller and id_cotizacion = r.id_cotizacion_autorizada and id_cotizacion_detalle = r.refOrd_id and id_cliprov = r.refProveedor),0)) end) " +
              ") as utilidad, " +
              "r.refPorcentSobreCosto as porcSobre, " +
              "(select staDescripcion from rafacciones_estatus where starefid = r.refEstatusSolicitud) as estatusSoli, " +
              "r.refEstatusSolicitud, " +
              "case r.refEstatus when 'NA' then 'No Aplica' when 'EV' then 'Evaluación' when 'RP' THEN 'Reparación' when 'CO' then 'Compra' when 'CA' THEN 'Cancelada' when 'AP' then 'Aplicada' when 'AU' then 'Autorizada' else '' end as estatus " +
              "from Refacciones_Orden r " +
              "left " +
              "join Cliprov c on c.id_cliprov = r.refProveedor and c.tipo = 'P' " +
              "where r.ref_no_orden = @orden and r.ref_id_empresa = @empresa and r.ref_id_taller = @taller " +
              "and r.refEstatusSolicitud <> 11 and r.proceso is null and refOrd_Id in (select distinct id_cotizacion_detalle from Cotizacion_Detalle where no_orden = r.ref_no_orden and id_empresa = r.ref_id_empresa and id_taller = r.ref_id_taller)";
        return  sql;
    }
}