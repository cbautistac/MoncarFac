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
using System.Globalization;

/// <summary>
/// Descripción breve de ImpresionOrdenCompra
/// </summary>
public class ImpresionOrdenCompra
{
    Ejecuciones ejecuta = new Ejecuciones();
    DatosImpresionOrdenCompra datosImpresion = new DatosImpresionOrdenCompra();
    string sql;
    bool resultado;
    object[] datosEjecuta = new object[2];
    public ImpresionOrdenCompra()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
    /*public string GenRepOrdTrabajo(int noOrden, int idEmpresa, int idTaller, int idCotizacion, char tipoImpresion, int idUsuario, int tamaño)*/
    public string GenRepOrdTrabajo(int noOrden, int idEmpresa, int idTaller, int idCotizacion, char tipoImpresion, int idUsuario, int tamaño, string estatus)
    {         
        string archivo = "";
        Document documento = new Document();
        // Crear documento
        if (tipoImpresion == 'T')
            archivo = formatoTiket(documento, noOrden, idEmpresa, idTaller, idCotizacion, idUsuario, tamaño);
        else if (tipoImpresion == 'O')
            archivo = formatoOrden(documento, noOrden, idEmpresa, idTaller, idCotizacion,estatus);
        return archivo;
    }

    private string formatoOrden(Document documento, int noOrden, int idEmpresa, int idTaller, int idCotizacion, string estatus)
    {
        documento = new Document(iTextSharp.text.PageSize.LETTER);
        if(estatus=="REC")
            documento.AddTitle("OrdenCompra_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + "_OC" + idCotizacion.ToString());
        else
            documento.AddTitle("OrdenCompra_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + "_OCS" + idCotizacion.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = "";
        if (estatus == "REC")
            archivo = ruta + "\\" + "OrdenCompra_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + "_OC" + idCotizacion.ToString() + ".pdf";
        else
            archivo = ruta + "\\" + "OrdenCompra_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + "_OCS" + idCotizacion.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

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
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo_direccion(2).jpg");
            gif.WidthPercentage = 40f;

            PdfPTable tablaEncabezado = new PdfPTable(2);
            tablaEncabezado.SetWidths(new float[] { 4f, 6f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;

            PdfPCell titu = new PdfPCell(new Phrase("N° Orden: " + noOrden, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 2;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(gif);
            tablaEncabezado.AddCell(titu);
            documento.Add(tablaEncabezado);

            DatosCab(documento, idEmpresa, idTaller, noOrden, idCotizacion,estatus);
            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void DatosCab(Document document, int idEmpresa, int idTaller, int noOrden, int idCotizacion, string estatus)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        //Obtener Datos Aseguradora
        object[] camposOrden = datosImpresion.encabezadoOrdenCompra(noOrden, idEmpresa, idTaller, idCotizacion);
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
                    //debug para ver q cae encabezado
                    string folioOrden, fecha, proveedor, importe;
                    folioOrden = fecha = proveedor = importe = "";

                    folioOrden = valores[1].ToString();
                    fecha = valores[4].ToString();
                    proveedor =valores[7].ToString();
                    importe = valores[5].ToString();

                    Paragraph fecharep = new Paragraph("Fecha Solicitud: " + fecha, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                    fecharep.Alignment = Element.ALIGN_RIGHT;
                    document.Add(fecharep);

                    if (estatus == "REC")
                    {
                        Paragraph fechaent = new Paragraph("Fecha Entrega: " + valores[8].ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                        fechaent.Alignment = Element.ALIGN_RIGHT;
                        document.Add(fechaent);
                    }
                    document.Add(new Paragraph(" "));

                    document.Add(new Paragraph("Folio: "+folioOrden, _standardFont1));
                    document.Add(new Paragraph("Proveedor: "+proveedor, _standardFont));
                    if(estatus=="REC")
                        document.Add(new Paragraph("Entregó: " + valores[9].ToString().ToUpper(), _standardFont));
                    document.Add(new Paragraph(" "));

                    string argumentos = "";
                    DatosVehiculos vehiculos = new DatosVehiculos();
                    object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculoCot(noOrden, idEmpresa, idTaller, idCotizacion, Convert.ToInt32(valores[6].ToString()));
                    if (Convert.ToBoolean(vehiculo[0]))
                    {
                        DataSet valores1 = (DataSet)vehiculo[1];
                        foreach (DataRow fila in valores1.Tables[0].Rows)
                        {
                            argumentos = argumentos.Trim() + " " + fila[1].ToString().ToUpper();                            
                            break;
                        }
                    }
                    document.Add(new Paragraph("Vehículo: " + argumentos, _standardFont));
                }
                i++;
            }
        }
        document.Add(new Paragraph(" ", estilo2));
        Mano_Obra(document, idEmpresa, idTaller, noOrden, idCotizacion, estatus);
    }

    private void Mano_Obra(Document document, int idEmpresa, int idTaller, int noOrden, int idCotizacion, string estatus)
    {
        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();
        //checar de donde sale el devolucion
        object[] camposManObra = datosImpresion.detalleOrdenCompra(noOrden, idEmpresa, idTaller, idCotizacion);

        if (Convert.ToBoolean(camposManObra[0]))
        {
            DataSet datos = (DataSet)camposManObra[1];

            if (datos.Tables[0].Rows.Count != 0)
            {
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font _standardFonTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                //checar como caen los datos en debug
                PdfPTable tableT = new PdfPTable(1);
                PdfPCell cellT = new PdfPCell(new Phrase("O R D E N   D E   C O M P R A", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cellT.BorderWidth = 0;
                cellT.Padding = 5;
                cellT.PaddingTop = 3;
                cellT.HorizontalAlignment = Element.ALIGN_CENTER;
                cellT.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableT.AddCell(cellT);
                tableT.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);
                tableT.HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(tableT);

                PdfPTable table = new PdfPTable(9);
                //table.WidthPercentage = 100f;
                //table.SetWidths(new float[] { 1f, 2f, 1f, 1f, 1f, 1f, 1f, 2f });
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 5, 20, 10, 10, 10, 10, 10, 20, 15 });
                string[] titulosTabla = { "Cantidad", "Refacción", "Cost. Unit.", "% Dto.", "Impte. Dto.", "Importe", "Estatus", "Observaciones","Procedencia" };

                for (int iCont = 0; iCont < titulosTabla.Length; iCont++)
                {
                    if (estatus != "REC")
                        titulosTabla[6] = titulosTabla[7] = "";
                    PdfPCell cell = new PdfPCell(new Phrase(titulosTabla[iCont].ToString(), _standardFonTitle));
                    cell.Border = 0;
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    if (iCont == 1 || iCont == 7)
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    else
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }
                decimal importeTotal = 0;
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    string cantidad, refaccion, costUnit, desc, imprtDesc, importe, estatusRef, observacion, entrega, procedencia;
                    cantidad = refaccion = costUnit = desc = imprtDesc = importe = estatusRef = observacion = entrega = procedencia = "";
                    cantidad = fila[3].ToString();
                    refaccion = fila[2].ToString();
                    costUnit = fila[4].ToString();
                    desc = fila[5].ToString();
                    imprtDesc = fila[6].ToString();
                    importe = fila[7].ToString();
                    estatusRef = fila[11].ToString();
                    observacion = fila[12].ToString();
                    entrega = fila[15].ToString();
                    procedencia = fila[16].ToString();
                    if (estatusRef == "Surtido")
                        importeTotal = importeTotal + Convert.ToDecimal(importe);

                    string[] detalleCampos = { cantidad, refaccion, costUnit, desc, imprtDesc, importe, estatusRef, observacion, procedencia };
                    for (int iCont = 0; iCont < detalleCampos.Length; iCont++)
                    {
                        if (estatus != "REC")
                            detalleCampos[6] = detalleCampos[7] = "";
                        PdfPCell cell = new PdfPCell(new Phrase(detalleCampos[iCont].ToString(), _standardFont));
                        cell.Border = 0;
                        cell.Padding = 2;
                        cell.PaddingTop = 2;
                        cell.PaddingBottom = 2;
                        if (iCont == 1 || iCont == 7)
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                }
                document.Add(table);
                //pintar total al final suma de importes menos de los cancelados o en devolucion
                if (estatus == "REC")
                {
                    PdfPCell cellTot = new PdfPCell(new Phrase("Importe Total sin I.V.A.: " + (string.Format("{0:C}", importeTotal)), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_RIGHT;
                    PdfPTable tableTotal = new PdfPTable(1);
                    tableTotal.WidthPercentage = 100f;
                    tableTotal.AddCell("");
                    tableTotal.AddCell(cellTot);
                    //document.Add(table);
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));

                    document.Add(tableTotal);
                }

               
            }
        }
    }

    private string formatoTiket(Document documento, int noOrden, int idEmpresa, int idTaller, int idCotizacion, int idUsuario, int tamaño)
    {
        float anchoTablaGIF = 0;
        if (tamaño == 2)
        {
            documento = new Document(iTextSharp.text.PageSize.LETTER);
            anchoTablaGIF = 40F;
        }
        else if (tamaño == 1)
        {
            documento = new Document(iTextSharp.text.PageSize.B6);
            anchoTablaGIF = 40f;
        }
        documento.AddTitle("TicketOrdenCompraE" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + "_OC" + idCotizacion.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "TicketOrdenCompraE" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + "_OC" + idCotizacion.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

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
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo_direccion(2).jpg");

            PdfPTable tablaEncabezado = new PdfPTable(1);
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.HorizontalAlignment = Element.ALIGN_CENTER;
            tablaEncabezado.WidthPercentage = anchoTablaGIF;
            PdfPCell imagenGif = new PdfPCell(gif);
            imagenGif.HorizontalAlignment = Element.ALIGN_CENTER;
            imagenGif.Border = 0;
            tablaEncabezado.AddCell(imagenGif);
            documento.Add(tablaEncabezado);
            documento.Add(new Paragraph(" "));
            DatosCabTicket(documento, idEmpresa, idTaller, noOrden, idCotizacion, idUsuario, tamaño);
            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void DatosCabTicket(Document documento, int idEmpresa, int idTaller, int noOrden, int idCotizacion, int idUsuario, int tamaño)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font();
        iTextSharp.text.Font _standardFont2 = new iTextSharp.text.Font();
        iTextSharp.text.Font _standardFont1 = new iTextSharp.text.Font();
        if (tamaño == 1)
        {
            _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            _standardFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        }
        else if (tamaño == 2)
        {
            _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            _standardFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        }
        string folioOrden, fecha, proveedor, importe, entrego, factura, cliente;
        folioOrden = fecha = proveedor = importe = entrego = factura = cliente = "";
        int idUserAut = 0;
        //Obtener Datos Aseguradora
        object[] camposOrden = datosImpresion.encabezadoOrdenCompra(noOrden, idEmpresa, idTaller, idCotizacion);
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
                    folioOrden = valores[1].ToString();
                    fecha = valores[4].ToString();
                    proveedor = valores[7].ToString();
                    Decimal importeTot = Convert.ToDecimal(valores[14]) ;
                    importe = valores[5].ToString();
                    entrego= valores[9].ToString();
                    idUserAut = Convert.ToInt32(valores[10].ToString());
                    cliente = valores[13].ToString();
                    factura = valores[11].ToString();
                    PdfPTable tableT1 = new PdfPTable(1);
                    tableT1.WidthPercentage = 100f;

                    PdfPCell cellT1 = new PdfPCell(new Phrase("Folio: " + folioOrden, _standardFont1));
                    cellT1.Border = 0;
                    cellT1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableT1.AddCell(cellT1);
                    PdfPCell cellF1 = new PdfPCell(new Phrase("Factura: " + factura, _standardFont1));
                    cellF1.Border = 0;
                    cellF1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableT1.AddCell(cellF1);
                    cellT1 = new PdfPCell(new Phrase("Proveedor: " + proveedor, _standardFont1));
                    cellT1.Border = 0;
                    cellT1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableT1.AddCell(cellT1);
                    cellT1 = new PdfPCell(new Phrase("No. Orden: " + noOrden.ToString(), _standardFont1));
                    cellT1.Border = 0;
                    cellT1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableT1.AddCell(cellT1);
                    cellT1 = new PdfPCell(new Phrase("Cliente: " + cliente.ToString(), _standardFont1));
                    cellT1.Border = 0;
                    cellT1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableT1.AddCell(cellT1);
                    //agregamos importe 
                    Decimal importeImprime = Convert.ToDecimal(importe);
                    
                    cellT1 = new PdfPCell(new Phrase("Importe Total: " + importeTot.ToString("C2"), _standardFont1));
                    cellT1.Border = 0;
                    cellT1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableT1.AddCell(cellT1);

                    /*documento.Add(new Paragraph("Folio: " + folioOrden, _standardFont1));
                    documento.Add(new Paragraph("Proveedor: " + proveedor, _standardFont));
                    documento.Add(new Paragraph("No. Orden: " + noOrden.ToString(), _standardFont));*/
                    string argumentos = "";
                    DatosVehiculos vehiculos = new DatosVehiculos();
                    object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculoCot(noOrden, idEmpresa, idTaller, idCotizacion, Convert.ToInt32(valores[6].ToString()));
                    if (Convert.ToBoolean(vehiculo[0]))
                    {
                        DataSet valores1 = (DataSet)vehiculo[1];
                        foreach (DataRow fila in valores1.Tables[0].Rows)
                        {
                            argumentos = argumentos.Trim() + " " + fila[1].ToString().ToUpper();
                            break;
                        }
                    }
                    cellT1 = new PdfPCell(new Phrase("Vehículo: " + argumentos, _standardFont1));
                    cellT1.Border = 0;
                    cellT1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableT1.AddCell(cellT1);
                    cellT1 = new PdfPCell(new Phrase("Fecha Entrega: " + valores[8].ToString(), _standardFont1));
                    cellT1.Border = 0;
                    cellT1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableT1.AddCell(cellT1);
                    /*documento.Add(new Paragraph("Vehículo: " + argumentos, _standardFont));
                    Paragraph fechaent = new Paragraph("Fecha Entrega: " + valores[8].ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));*/
                    documento.Add(tableT1);
                    documento.Add(new Paragraph(" "));
                }
                i++;
            }
        }
        Mano_ObraTicket(documento, idEmpresa, idTaller, noOrden, idCotizacion, entrego, idUsuario, idUserAut, tamaño);
    }

    private void Mano_ObraTicket(Document documento, int idEmpresa, int idTaller, int noOrden, int idCotizacion, string entrego, int idUsuario, int idUserAut, int tamaño)
    {
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font();
        iTextSharp.text.Font _standardFonTitle = new iTextSharp.text.Font();
        if (tamaño==1)
        {
            _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            _standardFonTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        }
        else if (tamaño == 2)
        {
            _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            _standardFonTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        }
        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();
        //checar de donde sale el devolucion
        object[] camposManObra = datosImpresion.detalleOrdenCompraImpresion(noOrden, idEmpresa, idTaller, idCotizacion);

        if (Convert.ToBoolean(camposManObra[0]))
        {
            DataSet datos = (DataSet)camposManObra[1];

            if (datos.Tables[0].Rows.Count != 0)
            {
                //checar como caen los datos en debug
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 65f;
                table.SetWidths(new float[] { 2f, 3f, 2f, 2f, 3f });
                string[] titulosTabla = { "Cantidad", "Refacción", "Estatus", "Importe", "Observaciones" };

                for (int iCont = 0; iCont < titulosTabla.Length; iCont++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(titulosTabla[iCont].ToString(), _standardFonTitle));
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    if (iCont == 1 || iCont == 3)
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    else
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    string cantidad, refaccion, estatusRef, importe, observacion, entrega;
                    cantidad = refaccion = estatusRef = observacion = entrega = "";
                    cantidad = fila[3].ToString();

                    Decimal importeRef = Convert.ToDecimal(fila[7]);
                    Decimal cantidadDe = Convert.ToDecimal(fila[3]);
                    Decimal totalLinea = 0;
                    importe = importeRef.ToString("C2");
                    totalLinea = importeRef ;
                    //importe = importeRef.ToString * cantidad;
                    //total linea es el costo unitario por la cantidad que solicitan
                    refaccion = fila[2].ToString();
                    estatusRef = fila[11].ToString();
                    observacion = fila[12].ToString();
                    entrega = fila[15].ToString();
                    Decimal importDes = Convert.ToDecimal(fila[6]);

                    string[] detalleCampos = { cantidad, refaccion, estatusRef,totalLinea.ToString(), observacion };
                    for (int iCont = 0; iCont < detalleCampos.Length; iCont++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(detalleCampos[iCont].ToString(), _standardFont));
                        cell.Border = 0;
                        cell.Padding = 2;
                        cell.PaddingTop = 2;
                        cell.PaddingBottom = 2;
                        if (iCont == 1 || iCont == 3 || iCont == 4)
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                }
                //pintar al final firmantes
                string nomUsuarioRec = ejecuta.scalarToStringSimple("select nombre_usuario from usuarios where id_usuario=" + idUsuario.ToString());
                string nomUsuarioAut = ejecuta.scalarToStringSimple("select nombre_usuario from usuarios where id_usuario=" + idUserAut.ToString());
                int cuadros = 0;
                float[] wids = new float[2];
                if (tamaño == 1)
                {
                    cuadros = 1;
                    wids = new float[] { 10f };
                }
                else if (tamaño == 2)
                {
                    cuadros = 2;
                    wids = new float[] { 5f, 5f };
                }
                PdfPTable tableFirmas = new PdfPTable(cuadros);
                tableFirmas.WidthPercentage = 100f;
                tableFirmas.SetWidths(wids);

                if (tamaño == 2)
                {
                    PdfPCell cellLinea = new PdfPCell(new Phrase(" ", _standardFont));
                    cellLinea.BorderWidthTop = 1;
                    cellLinea.BorderWidthLeft = 0;
                    cellLinea.BorderWidthRight = 0;
                    cellLinea.BorderWidthBottom = 0;
                    cellLinea.Padding = 2;
                    cellLinea.Colspan = 2;
                    cellLinea.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellLinea);

                    PdfPCell cellEspacio = new PdfPCell(new Phrase(" ", _standardFont));
                    cellEspacio.Border = 0;
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);

                    PdfPCell cellTot = new PdfPCell(new Phrase("___________________________________", _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    cellTot = new PdfPCell(new Phrase("___________________________________", _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    cellTot = new PdfPCell(new Phrase("Recibió: " + nomUsuarioRec, _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    cellTot = new PdfPCell(new Phrase("Entregó: " + entrego, _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);

                    cellTot = new PdfPCell(new Phrase("___________________________________", _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.Colspan = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    cellTot = new PdfPCell(new Phrase("Autorizo: " + nomUsuarioAut, _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.Colspan = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);
                }
                else if (tamaño == 1)
                {
                    PdfPCell cellEspacio = new PdfPCell(new Phrase(" ", _standardFont));
                    cellEspacio.Border = 0;
                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    
                    PdfPCell cellTot = new PdfPCell(new Phrase("___________________", _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    cellTot = new PdfPCell(new Phrase("Recibió: " + nomUsuarioRec, _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                    
                    cellTot = new PdfPCell(new Phrase("___________________", _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);
                    
                    cellTot = new PdfPCell(new Phrase("Entregó: " + entrego, _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);

                    cellTot = new PdfPCell(new Phrase("___________________", _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.Colspan = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    cellTot = new PdfPCell(new Phrase("Autorizo: " + nomUsuarioAut, _standardFont));
                    cellTot.Border = 0;
                    cellTot.Padding = 2;
                    cellTot.PaddingTop = 2;
                    cellTot.PaddingBottom = 2;
                    cellTot.Colspan = 2;
                    cellTot.HorizontalAlignment = Element.ALIGN_CENTER;
                    tableFirmas.AddCell(cellTot);

                    tableFirmas.AddCell(cellEspacio);
                    tableFirmas.AddCell(cellEspacio);
                }

                documento.Add(table);
                documento.Add(tableFirmas);
            }
        }
    }
}