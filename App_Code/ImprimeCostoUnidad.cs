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
/// Descripción breve de ImprimirInfoCostoUnidad
/// </summary>
public class ImprimeCostoUnidad
{
    Fechas fechas = new Fechas();

    private decimal totalReparacion;
    private decimal totalUtilidad;
    private decimal porcentajetotal;
    private decimal totals;
    public ImprimeCostoUnidad()
    {
        totalReparacion = 0;
        totalUtilidad = 0;
        totals = 0;
        porcentajetotal = 0;
    }

    public string imprimeCostoUnidad(int empresa, int taller, int orden)
    {


        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER.Rotate());
        documento.AddTitle(" Comparativo  Costos ");
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\ComparativoCostos" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + ".pdf";

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


            PdfPCell titu = new PdfPCell(new Phrase("Moncar Aztahucan S.A de C.V. " + Environment.NewLine + Environment.NewLine + "Comparativo Costos", FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 1;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_MIDDLE;
            tablaEncabezado.AddCell(gif);
            tablaEncabezado.AddCell(titu);
            documento.Add(tablaEncabezado);

            Datosencab(documento, empresa, taller, orden);

            //datosManoObra(documento, orden, empresa, taller);
            datosRefaccion(documento, orden, empresa, taller);
            //datospintura(documento, orden, empresa, taller);
            //datosCC(documento, orden, empresa, taller);
            //datosalmacen(documento, orden, empresa, taller);
            //datosCostoFijo(documento, orden, empresa, taller);
            datosTotalReparacion(documento);

            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();


        }
        return archivo;


    }

    private void datosTotalReparacion(Document documento)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

        PdfPTable tbldatos2 = new PdfPTable(1);
        tbldatos2.WidthPercentage = 100f;
        /*
        PdfPCell fecha = new PdfPCell(new Paragraph("TOTAL REPARACION: " + totalReparacion.ToString("C2"), fuente1));
        fecha.HorizontalAlignment = Element.ALIGN_RIGHT;
        fecha.PaddingTop = 10;
        fecha.Border = 0;
        tbldatos2.AddCell(fecha);*/

        PdfPCell utilidad = new PdfPCell(new Paragraph("TOTAL UTILIDAD: " + totalUtilidad.ToString("C2"), fuente1));
        utilidad.HorizontalAlignment = Element.ALIGN_RIGHT;
        utilidad.PaddingTop = 10;
        utilidad.Border = 0;
        tbldatos2.AddCell(utilidad);


        documento.Add(tbldatos2);
    }

    private void datosCostoFijo(Document documento, int orden, int empresa, int taller)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        decimal costoFijo = 0;
        Recepciones recepciones = new Recepciones();
        object[] datosOrden = recepciones.obtieneInfoOrdenPie(orden, empresa, taller);
        if (Convert.ToBoolean(datosOrden[0]))
        {
            DataSet ordenDatos = (DataSet)datosOrden[1];
            foreach (DataRow filaOrd in ordenDatos.Tables[0].Rows)
            {
                costoFijo = Convert.ToDecimal(filaOrd[18]);
            }
        }

        PdfPTable tbldatos2 = new PdfPTable(1);
        tbldatos2.WidthPercentage = 100f;

        PdfPCell fecha = new PdfPCell(new Paragraph("Total Costo Fijo: " + costoFijo.ToString("C2"), fuente1));
        fecha.HorizontalAlignment = Element.ALIGN_RIGHT;
        fecha.Padding = 2;
        fecha.Border = 0;

        tbldatos2.AddCell(fecha);
        documento.Add(tbldatos2);
        totalReparacion = totalReparacion + costoFijo;
    }

    private void Datosencab(Document document, int empresa, int taller, int ordenT)
    {

        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        PdfPTable tbldatos = new PdfPTable(5);
        tbldatos.WidthPercentage = 100f;


        string[] info = new string[5] { "", "", "", "", "" };

        DatosVehiculos vehiculos = new DatosVehiculos();
        object[] vehiculot = vehiculos.obtieneDatosVehiculoCC(ordenT, empresa, taller);
        if (Convert.ToBoolean(vehiculot[0]))
        {
            DataSet valores = (DataSet)vehiculot[1];
            foreach (DataRow fila in valores.Tables[0].Rows)
            {
                info[0] = Convert.ToString(fila[0]);
                info[1] = Convert.ToString(fila[1]);
                info[2] = Convert.ToString(fila[2]);
                info[3] = Convert.ToString(fila[3]);
                info[4] = Convert.ToString(fila[4]);
            }
        }

        PdfPCell datos = new PdfPCell(new Phrase());

        PdfPCell orden = new PdfPCell(new Paragraph("Orden: " + info[0], fuente1));
        PdfPCell cliente = new PdfPCell(new Paragraph("Cliente: " + info[2], fuente1));
        PdfPCell vehiculo = new PdfPCell(new Paragraph("Vehiculo: " + info[3], fuente1));
        PdfPCell placas = new PdfPCell(new Paragraph("Placas: " + info[1], fuente1));
        PdfPCell tipvalua = new PdfPCell(new Paragraph("Tipo de Valuación: " + info[4], fuente1));


        orden.HorizontalAlignment = Element.ALIGN_LEFT;
        orden.Padding = 9;
        orden.Border = 0;
        cliente.HorizontalAlignment = Element.ALIGN_LEFT;
        cliente.Padding = 9;
        cliente.Border = 0;
        vehiculo.HorizontalAlignment = Element.ALIGN_LEFT;
        vehiculo.Padding = 9;
        vehiculo.Border = 0;
        placas.HorizontalAlignment = Element.ALIGN_LEFT;
        placas.Padding = 9;
        placas.Border = 0;
        tipvalua.HorizontalAlignment = Element.ALIGN_LEFT;
        tipvalua.Padding = 9;
        tipvalua.Border = 0;

        tbldatos.AddCell(orden);
        tbldatos.AddCell(cliente);
        tbldatos.AddCell(vehiculo);
        tbldatos.AddCell(placas);
        tbldatos.AddCell(tipvalua);


        document.Add(tbldatos);

    }

    private void datosManoObra(Document document, int ordenT, int empresa, int taller)
    {
        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        Paragraph paragraph = new Paragraph();
        paragraph.Add(new Phrase("Mano de Obra", fuente1));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        document.Add(paragraph);

        PdfPTable tbldatos2 = new PdfPTable(4);
        tbldatos2.WidthPercentage = 100f;

        PdfPCell fecha = new PdfPCell(new Paragraph("Fecha", fuenteB));
        PdfPCell trabajador = new PdfPCell(new Paragraph("Trabajador a Cargo", fuenteB));
        PdfPCell costoPresup = new PdfPCell(new Paragraph("Precio Autorizado", fuenteB));
        PdfPCell costoReal = new PdfPCell(new Paragraph("Precio Real", fuenteB));
       


        fecha.HorizontalAlignment = Element.ALIGN_LEFT;
        fecha.Padding = 2;
        fecha.Border = 0;
        fecha.BackgroundColor = trabajador.BackgroundColor = costoPresup.BackgroundColor = costoReal.BackgroundColor = BaseColor.LIGHT_GRAY;
        trabajador.HorizontalAlignment = Element.ALIGN_CENTER;
        trabajador.Padding = 2;
        trabajador.Border = 0;
        costoPresup.HorizontalAlignment = Element.ALIGN_CENTER;
        costoPresup.Padding = 2;
        costoPresup.Border = 0;
        costoReal.HorizontalAlignment = Element.ALIGN_CENTER;
        costoReal.Padding = 2;
        costoReal.Border = 0;
        


        tbldatos2.AddCell(fecha);
        tbldatos2.AddCell(trabajador);
        tbldatos2.AddCell(costoPresup);
        tbldatos2.AddCell(costoReal);
        

        document.Add(tbldatos2);

        decimal totalMo = 0;
        
        CostoUnidad mo = new CostoUnidad();

        object[] infoManoObra = mo.obtieneManoObraCc(empresa, taller, ordenT);
        if (Convert.ToBoolean(infoManoObra[0]))
        {
            DataSet ds = (DataSet)infoManoObra[1];

            PdfPTable detalle = new PdfPTable(4);
            detalle.WidthPercentage = 100f;

            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                PdfPCell celda = new PdfPCell(new Paragraph(Convert.ToDateTime(fila[0]).ToString("dd/MM/yyyy"), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToString(fila[1]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[2]).ToString("C2"), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_RIGHT;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[3]).ToString("C2"), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_RIGHT;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

               

                totalMo = totalMo + Convert.ToDecimal(fila[3]);
                
            }
            document.Add(detalle);
        }

        Paragraph total = new Paragraph();
        total.Add(new Phrase("Total Mano de Obra: " + totalMo.ToString("C2"), fuente1));
        total.Alignment = Element.ALIGN_RIGHT;
        document.Add(total);
        totalReparacion = totalReparacion + totalMo;
      
    }

    /*este es el bloque original antes de cambiarlo el 11ago2017||Alfredo Gonzalez Davila
    //
    private void datosRefaccion(Document document, int ordenT, int empresa, int taller)
    {
        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteN = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.RED);
        iTextSharp.text.Font fuenteP = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.GREEN);

        Paragraph paragraph = new Paragraph();
        paragraph.Add(new Phrase("Refacciones", fuente1));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        document.Add(paragraph);

        PdfPTable tbldatos3 = new PdfPTable(6);
        tbldatos3.WidthPercentage = 100f;

        PdfPCell Refac = new PdfPCell(new Paragraph("Refacción", fuenteB));
        PdfPCell Cant = new PdfPCell(new Paragraph("Cantidad", fuenteB));
        PdfPCell prove = new PdfPCell(new Paragraph("Proveedor", fuenteB));
        PdfPCell Precio = new PdfPCell(new Paragraph("Precio Autorizado", fuenteB));
        PdfPCell impor = new PdfPCell(new Paragraph("Precio Compra", fuenteB));
        PdfPCell utilidad = new PdfPCell(new Paragraph("Utilidad", fuenteB));

        Refac.HorizontalAlignment = Element.ALIGN_LEFT;
        Refac.Padding = 2;
        Refac.Border = 0;
        Refac.BackgroundColor = Cant.BackgroundColor = Precio.BackgroundColor = impor.BackgroundColor = prove.BackgroundColor = utilidad.BackgroundColor = BaseColor.LIGHT_GRAY;
        Cant.HorizontalAlignment = Element.ALIGN_CENTER;
        Cant.Padding = 2;
        Cant.Border = 0;
        prove.HorizontalAlignment = Element.ALIGN_CENTER;
        prove.Padding = 2;
        prove.Border = 0;
        Precio.HorizontalAlignment = Element.ALIGN_CENTER;
        Precio.Padding = 2;
        Precio.Border = 0;
        impor.HorizontalAlignment = Element.ALIGN_CENTER;
        impor.Padding = 2;
        impor.Border = 0;
        utilidad.HorizontalAlignment = Element.ALIGN_CENTER;
        utilidad.Padding = 2;
        utilidad.Border = 0;

        tbldatos3.AddCell(Refac);
        tbldatos3.AddCell(Cant);
        tbldatos3.AddCell(prove);
        tbldatos3.AddCell(Precio);
        tbldatos3.AddCell(impor);
        tbldatos3.AddCell(utilidad);

        document.Add(tbldatos3);

        decimal totalRef = 0;
        decimal utilidadRef = 0;
        decimal totalAutorizado =0;        
        decimal a = 0;
        decimal b = 0;
        decimal c = 100;
        decimal operacion1 = 0;
        decimal operacion2 = 0;  
        CostoUnidad Ref = new CostoUnidad();

        object[] inforefacciones = Ref.obtienerefaccionesc(empresa, taller, ordenT);
        if (Convert.ToBoolean(inforefacciones[0]))
        {
            DataSet ds = (DataSet)inforefacciones[1];

            PdfPTable detalle = new PdfPTable(6);
            detalle.WidthPercentage = 100f;

            foreach (DataRow fila in ds.Tables[0].Rows)
            {



                a = Convert.ToDecimal(fila[3]);
                b = Convert.ToDecimal(fila[4]);                
                if (a != 0 && b == 0)
                {
                    operacion2 = 100;
                }
                else
                {
                    if (a == 0)
                    {
                        operacion2 = -100;
                    }
                    else
                    {
                        operacion1 = (a - b) * 100;
                        operacion2 = operacion1 / a;

                    }
                }

                PdfPCell celda = new PdfPCell(new Paragraph(Convert.ToString(fila[0]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[1]).ToString(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToString(fila[2]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[3]).ToString("C2"), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[4]).ToString("C2"), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[5]).ToString("C2") + " | " + Convert.ToDecimal(operacion2).ToString("F2") + "%", fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);                                

                totalAutorizado = totalAutorizado + Convert.ToDecimal(fila[4]);
                totalRef = totalRef + Convert.ToDecimal(fila[4]);
                totals = totals + Convert.ToDecimal(fila[4]);
                utilidadRef = utilidadRef + Convert.ToDecimal(fila[4]);
                
            }
            document.Add(detalle);
        }
        
        Paragraph total = new Paragraph();
        total.Add(new Phrase("Total Refacciones: " + totalAutorizado.ToString("C2"), fuente1));
        total.Alignment = Element.ALIGN_RIGHT;
        document.Add(total);
        decimal opera1 = 0, opera2 = 0;        

        if (totalAutorizado == 0)
        {
            opera2 = -100;
        }
        else
        {
            if (totalAutorizado != 0 && totals == 0)
            {
                opera2 = 100;
            }
            else
            {
                opera1 = (totalAutorizado - totals) * 100;
                opera2 = opera1 / totalAutorizado;
            }
        }    

        if (utilidadRef < 0)
        {
            Paragraph uti = new Paragraph();
            uti.Add(new Phrase("Utilidad Refacciones: " + utilidadRef.ToString("C2") + " | " + Convert.ToDecimal(opera2).ToString("F2") + "%", fuenteN));
            uti.Alignment = Element.ALIGN_RIGHT;
            document.Add(uti);
        }
        if (utilidadRef > 0)
        {
            Paragraph uti = new Paragraph();
            uti.Add(new Phrase("Utilidad Refacciones: " + utilidadRef.ToString("C2") + " | " + Convert.ToDecimal(opera2).ToString("F2") + "%", fuenteP));
            uti.Alignment = Element.ALIGN_RIGHT;
            document.Add(uti);
        }
        if (utilidadRef == 0)
        {
            Paragraph uti = new Paragraph();
            uti.Add(new Phrase("Utilidad Refacciones: " + utilidadRef.ToString("C2") + " | " + Convert.ToDecimal(opera2).ToString("F2") + "%", fuente1));
            uti.Alignment = Element.ALIGN_RIGHT;
            document.Add(uti);
        }



        totalReparacion = totalReparacion + totalRef;
        totalUtilidad = totalUtilidad + utilidadRef;
    }
*/

    /*
     * Alfredo Gonzalez Davila|| 11Ago2017 ||
     * Lo siguiente es una copia de la clase "ImprimeInfoCostoUnidad"
     * ya que a esta fecha(11ago2017|| 1450pm) ya ha quedado corregida
     */
    private void datosRefaccion(Document document, int ordenT, int empresa, int taller)
    {
        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteN = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.RED);
        iTextSharp.text.Font fuenteP = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.GREEN);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        Paragraph paragraph = new Paragraph();
        paragraph.Add(new Phrase("Refacciones", fuente1));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        document.Add(paragraph);

        //correccion||Alfredo Gonzalez Davila ||10 Ago2017
        //falta columna de importe | (cantidad x costo)
        //la utilidad sera: (cantidad x precio autorizado )-(cantidad x costo -->("precio compra"))
        PdfPTable tbldatos3 = new PdfPTable(7);
        tbldatos3.WidthPercentage = 100f;

        PdfPCell Refac = new PdfPCell(new Paragraph("Refacción", fuenteB));
        PdfPCell Cant = new PdfPCell(new Paragraph("Cantidad", fuenteB));
        PdfPCell prove = new PdfPCell(new Paragraph("Proveedor", fuenteB));
        PdfPCell Precio = new PdfPCell(new Paragraph("Precio Autorizado", fuenteB));
        PdfPCell compra = new PdfPCell(new Paragraph("Precio Compra", fuenteB));
        PdfPCell importe = new PdfPCell(new Paragraph("Importe", fuenteB));
        PdfPCell uti = new PdfPCell(new Paragraph("Utilidad", fuenteB));


        Refac.HorizontalAlignment = Element.ALIGN_LEFT;
        Refac.Padding = 2;
        Refac.Border = 0;
        Refac.BackgroundColor = Cant.BackgroundColor = uti.BackgroundColor = Precio.BackgroundColor = compra.BackgroundColor = importe.BackgroundColor = prove.BackgroundColor = BaseColor.LIGHT_GRAY;
        Cant.HorizontalAlignment = Element.ALIGN_CENTER;
        Cant.Padding = 2;
        Cant.Border = 0;
        prove.HorizontalAlignment = Element.ALIGN_CENTER;
        prove.Padding = 2;
        prove.Border = 0;
        Precio.HorizontalAlignment = Element.ALIGN_CENTER;
        Precio.Padding = 2;
        Precio.Border = 0;
        compra.HorizontalAlignment = Element.ALIGN_CENTER;
        compra.Padding = 2;
        compra.Border = 0;
        importe.HorizontalAlignment = Element.ALIGN_CENTER;
        importe.Padding = 2;
        importe.Border = 0;
        uti.HorizontalAlignment = Element.ALIGN_CENTER;
        uti.Padding = 2;
        uti.Border = 0;


        tbldatos3.AddCell(Refac);
        tbldatos3.AddCell(Cant);
        tbldatos3.AddCell(prove);
        tbldatos3.AddCell(Precio);
        tbldatos3.AddCell(compra);
        tbldatos3.AddCell(importe);
        tbldatos3.AddCell(uti);


        document.Add(tbldatos3);

        decimal totalRef = 0;
        Refacciones Ref = new Refacciones();

        object[] inforefacciones = Ref.obtienerefaccionesc(empresa, taller, ordenT);

        decimal totPorcentajeUtilidad = 0;
        decimal totalRefPrecioCompleto = 0;

        if (Convert.ToBoolean(inforefacciones[0]))
        {
            DataSet ds = (DataSet)inforefacciones[1];

            PdfPTable detalle = new PdfPTable(7);
            detalle.WidthPercentage = 100f;
            decimal utilidIndiv = 0;
            decimal ope1 = 0;
            decimal autorizado = 0;
            decimal compra1 = 0;
            decimal importe1 = 0;
            decimal utilidad = 0;
            decimal porcentajeUtilidadIndividual = 0;



            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                //bloque correctivo
                //Alfredo Gonzalez Davila ||10Ago2017
                //En el reporte dse imprime el precio autorizado  en ceros
                //si viene en 0 se igualara el autorizado al precio de compra
                //Instrucciones dadas por Edgar Palacios|09Ago2017|18:20pm
                //////////////////////////////////////////////////////////
                if (Convert.ToDecimal(fila[7]) > 0)
                {
                    autorizado = Convert.ToDecimal(fila[7]);
                }
                else
                {
                    autorizado = Convert.ToDecimal(fila[5]);
                }

                compra1 = Convert.ToDecimal(fila[5]);

                //correccion||Alfredo Gonzalez Davila|10Ago2017
                //no contempla los importes de compra X cantidad

                importe1 = (Convert.ToDecimal(fila[2]) * Convert.ToDecimal(fila[5]));
                utilidad = (autorizado * (Convert.ToDecimal(fila[2]))) - importe1;
                if (importe1 > 0)
                {
                    porcentajeUtilidadIndividual = ((utilidad * 100) / importe1);
                }
                else
                {
                    porcentajeUtilidadIndividual = 100;
                }

                /*
                if (autorizado != 0 && compra1 == 0)
                {
                    utilidIndiv = 100;
                }
                else
                {
                    if (autorizado==0)
                    {
                        utilidIndiv = -100;
                    }
                    else
                    {
                        ope1 = (autorizado - compra1) * 100;
                        utilidIndiv = ope1 / autorizado;

                    }
                }
                
                ope1 = ((autorizado * ( Convert.ToDecimal(fila[2]) )) - (compra1 * (Convert.ToDecimal(fila[2]))) ) * 100;
                utilidIndiv = ope1 / (autorizado * (Convert.ToDecimal(fila[2])));
                */

                PdfPCell celda = new PdfPCell(new Paragraph(Convert.ToString(fila[1]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_LEFT;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[2]).ToString(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToString(fila[4]).ToUpper(), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(autorizado.ToString("C2"), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[5]).ToString("C2"), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                celda = new PdfPCell(new Paragraph(importe1.ToString("C2"), fuente2));
                celda.HorizontalAlignment = Element.ALIGN_CENTER;
                celda.Padding = 0;
                celda.Border = 0;
                detalle.AddCell(celda);

                uti = new PdfPCell(new Paragraph(Convert.ToDecimal(utilidad).ToString("C2") + " | " + porcentajeUtilidadIndividual.ToString("F2") + "%", fuente2));
                uti.HorizontalAlignment = Element.ALIGN_CENTER;
                uti.Padding = 0;
                uti.Border = 0;
                detalle.AddCell(uti);

                //Correccion ||Alfredo Gonzalez Davila||10Ago2017
                //instruccion Original: totalRef = totalRef + Convert.ToDecimal(fila[5]);
                //se hace la correccion ya que no se tima encuenta la columna de cantidad para hacer los calculos
                //Nota: en la variable importe1 traemos la operacion de ( del array fila[5]*[2]), correspondiente 
                //Al importe de compra X cantidad

                totalRef = totalRef + importe1;
                //totals = totals + importe1;
                totalRefPrecioCompleto = totalRefPrecioCompleto + (autorizado * (Convert.ToDecimal(fila[2])));
                totalUtilidad = totalUtilidad + ((autorizado * (Convert.ToDecimal(fila[2]))) - importe1);

            }
            document.Add(detalle);
        }

        Paragraph totalAuto = new Paragraph();
        totalAuto.Add(new Phrase("Total Refacciones Autorizadas: " + totalRefPrecioCompleto.ToString("C2"), fuente1));
        totalAuto.Alignment = Element.ALIGN_RIGHT;
        document.Add(totalAuto);

        Paragraph total = new Paragraph();
        total.Add(new Phrase("Total Refacciones: " + totalRef.ToString("C2"), fuente1));
        total.Alignment = Element.ALIGN_RIGHT;
        document.Add(total);

        decimal opera1 = 0, opera2 = 0;
        opera1 = (totalRef - totals) * 100;
        //opera2 = opera1 / totalRef;
        /*
        decimal opera1=0, opera2=0;

        if (totalRef == 0)
        {
            opera2 = -100;
        }        
        else
        {
            if (totalRef != 0 && totals == 0)
            {
                opera2 = 100;
            }
            else
            {
                opera1 = (totalRef - totals) * 100;
                opera2 = opera1 / totalRef;
            }
        }
     
        if (totalUtilidad < 0)
        {
            Paragraph uts = new Paragraph();
            uts.Add(new Phrase("Utilidad Refacciones: " + totalUtilidad.ToString("C2") + " | " + Convert.ToDecimal(opera2).ToString("F2") + "%", fuenteN));
            uts.Alignment = Element.ALIGN_RIGHT;
            document.Add(uts);
        }
        if (totalUtilidad > 0)
        {
            Paragraph uts = new Paragraph();
            uts.Add(new Phrase("Utilidad Refacciones: " + totalUtilidad.ToString("C2") + " | " + Convert.ToDecimal(opera2).ToString("F2") + "%", fuenteP));
            uts.Alignment = Element.ALIGN_RIGHT;
            document.Add(uts);
        }
        if (totalUtilidad == 0)
        {
            Paragraph uts = new Paragraph();
            uts.Add(new Phrase("Utilidad Refacciones: " + totalUtilidad.ToString("C2") + " | " + Convert.ToDecimal(opera2).ToString("F2") + "%", fuente1));
            uts.Alignment = Element.ALIGN_RIGHT;
            document.Add(uts);
        }
        */
        if ((totalUtilidad > 0) && (totalRefPrecioCompleto >0) )
        {
            totPorcentajeUtilidad = ((totalUtilidad * 100) / totalRefPrecioCompleto);
        }
        else
        {

            totPorcentajeUtilidad = 0;
        }
        

        Paragraph uts = new Paragraph();
        //para pintar el total del costo con respecto  al total autorizado
        //uts.Add(new Phrase("Utilidad Refacciones: " + totalUtilidad.ToString("C2") + " | " + ((totalRef * 100)/ totalRefPrecioCompleto).ToString("F2") + "%", fuente1));

        //para Pintar porcentaje de utilidad total  con respecto a precio autorizado
        uts.Add(new Phrase("Utilidad Refacciones: " + totalUtilidad.ToString("C2") + " | " + totPorcentajeUtilidad.ToString("F2") + "%", fuente1));
        uts.Alignment = Element.ALIGN_RIGHT;
        document.Add(uts);
        totalReparacion = totalReparacion + totalRef;
    }

    private void datospintura(Document document, int ordenT, int empresa, int taller)
    {

        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        Paragraph paragraph = new Paragraph();
        paragraph.Add(new Phrase("Pintura", fuente1));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        document.Add(paragraph);


        PdfPTable tbldatos4 = new PdfPTable(5);
        tbldatos4.WidthPercentage = 100f;

        PdfPCell fecha = new PdfPCell(new Paragraph("Fecha", fuenteB));
        PdfPCell descri = new PdfPCell(new Paragraph("Descripción", fuenteB));
        PdfPCell cantidad = new PdfPCell(new Paragraph("Cantidad", fuenteB));
        PdfPCell costos = new PdfPCell(new Paragraph("Precio Autorizado", fuenteB));
        PdfPCell utilidad = new PdfPCell(new Paragraph("Precio Compra", fuenteB));
       

        fecha.HorizontalAlignment = Element.ALIGN_LEFT;
        fecha.Padding = 2;
        fecha.Border = 0;
        fecha.BackgroundColor = descri.BackgroundColor = cantidad.BackgroundColor = costos.BackgroundColor =  utilidad.BackgroundColor  = BaseColor.LIGHT_GRAY;
        descri.HorizontalAlignment = Element.ALIGN_CENTER;
        descri.Padding = 2;
        descri.Border = 0;
        cantidad.HorizontalAlignment = Element.ALIGN_CENTER;
        cantidad.Padding = 2;
        cantidad.Border = 0;
        costos.HorizontalAlignment = Element.ALIGN_CENTER;
        costos.Padding = 2;
        costos.Border = 0;
        utilidad.HorizontalAlignment = Element.ALIGN_CENTER;
        utilidad.Padding = 2;
        utilidad.Border = 0;
        

        tbldatos4.AddCell(fecha);
        tbldatos4.AddCell(descri);
        tbldatos4.AddCell(cantidad);
        tbldatos4.AddCell(costos);
        tbldatos4.AddCell(utilidad);
        

        document.Add(tbldatos4);


        decimal totalPint = 0;
      

        string sql = "select fecha,nombre,cantidad,montoAutorizado,monto1,(monto1-montoAutorizado) as utilidadRef from tmp_costo_unidad  where id_taller=" + taller + " and id_empresa=" + empresa + " and no_orden=" + ordenT + " and identificador='Pintura'";

        
        DataTable dt = inicializaTbPint(sql);
        PdfPTable detalle = new PdfPTable(5);
        detalle.WidthPercentage = 100f;
        foreach (DataRow fila in dt.Rows)
        {
            PdfPCell celda = new PdfPCell(new Paragraph(Convert.ToDateTime(fila[0]).ToString("dd/MM/yyyy"), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_LEFT;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToString(fila[1]).ToUpper(), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[2]).ToString(), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[3]).ToString("C2"), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[4]).ToString("C2"), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            


            totalPint = totalPint + Convert.ToDecimal(fila[4]);
            
        }
        document.Add(detalle);

        Paragraph total = new Paragraph();
        total.Add(new Phrase("Total Pintura: " + totalPint.ToString("C2"), fuente1));
        total.Alignment = Element.ALIGN_RIGHT;
        document.Add(total);
      
        totalReparacion = totalReparacion + totalPint;

    }

    private DataTable inicializaTbPint(string qry)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fecha", typeof(string));
        dt.Columns.Add("descripcion", typeof(string));        
        dt.Columns.Add("cantidad", typeof(string));               
        dt.Columns.Add("costo autorizado");
        dt.Columns.Add("costo compra");
        dt.Columns.Add("utilidad");

        Ejecuciones ejec = new Ejecuciones();
        DataSet ds = new DataSet();
        if (qry != "")
        {
            ds = (DataSet)ejec.dataSet(qry)[1];
            foreach (DataRow row in ds.Tables[0].Rows)
                dt.Rows.Add(Convert.ToDateTime(row[0]).ToString("yyyy-MM-dd"), row[1], row[2], row[3], row[4],row[5]);
        }

        return dt;
    }
    private DataTable inicializaTbCaja(string qry)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fecha", typeof(string));
        dt.Columns.Add("proveedor", typeof(string));
        dt.Columns.Add("descripcion", typeof(string));
        dt.Columns.Add("cantidad");
        dt.Columns.Add("costo autorizado");
        dt.Columns.Add("costo compra");
        dt.Columns.Add("utilidad");

        Ejecuciones ejec = new Ejecuciones();
        DataSet ds = new DataSet();
        if (qry != "")
        {
            ds = (DataSet)ejec.dataSet(qry)[1];
            foreach (DataRow row in ds.Tables[0].Rows)
                dt.Rows.Add(Convert.ToDateTime(row[0]).ToString("yyyy-MM-dd"), row[1], row[2], row[3], row[4], row[5],row[6]);
        }

        return dt;
    }
    private DataTable inicializaTbAlm(string qry)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fecha", typeof(string));
        dt.Columns.Add("descripcion", typeof(string));
        dt.Columns.Add("cantidad", typeof(string));
        dt.Columns.Add("costo autorizado");
        dt.Columns.Add("costo compra");
        dt.Columns.Add("utilidad");

        Ejecuciones ejec = new Ejecuciones();
        DataSet ds = new DataSet();
        if (qry != "")
        {
            ds = (DataSet)ejec.dataSet(qry)[1];
            foreach (DataRow row in ds.Tables[0].Rows)
                dt.Rows.Add(Convert.ToDateTime(row[0]).ToString("yyyy-MM-dd"), row[1], row[2], row[3], row[4], row[5]);
        }

        return dt;
    }

    private void datosCC(Document document, int ordenT, int empresa, int taller)
    {
        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        Paragraph paragraph = new Paragraph();
        paragraph.Add(new Phrase("Caja Chica", fuente1));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        document.Add(paragraph);

        PdfPTable tbldatos5 = new PdfPTable(6);
        tbldatos5.WidthPercentage = 100f;


        PdfPCell fecha = new PdfPCell(new Paragraph("Fecha", fuenteB));
        PdfPCell Provee = new PdfPCell(new Paragraph("Proveedor", fuenteB));
        PdfPCell Descripcion = new PdfPCell(new Paragraph("Descripcion", fuenteB));
        PdfPCell cantidad = new PdfPCell(new Paragraph("Cantidad", fuenteB));
        PdfPCell costosa = new PdfPCell(new Paragraph("Costo Autorizado", fuenteB));
        PdfPCell costosc = new PdfPCell(new Paragraph("Costo Compra", fuenteB));
        

        fecha.HorizontalAlignment = Element.ALIGN_LEFT;
        fecha.Padding = 2;
        fecha.Border = 0;
        fecha.BackgroundColor = Descripcion.BackgroundColor = cantidad.BackgroundColor = costosa.BackgroundColor = costosc.BackgroundColor  = Provee.BackgroundColor = BaseColor.LIGHT_GRAY;
        Descripcion.HorizontalAlignment = Element.ALIGN_CENTER;
        Descripcion.Padding = 2;
        Descripcion.Border = 0;
        Provee.HorizontalAlignment = Element.ALIGN_LEFT;
        Provee.Padding = 2;
        Provee.Border = 0;
        cantidad.HorizontalAlignment = Element.ALIGN_CENTER;
        cantidad.Padding = 2;
        cantidad.Border = 0;
        costosa.HorizontalAlignment = Element.ALIGN_CENTER;
        costosa.Padding = 2;
        costosa.Border = 0;
        costosc.HorizontalAlignment = Element.ALIGN_CENTER;
        costosc.Padding = 2;
        costosc.Border = 0;
        

        tbldatos5.AddCell(fecha);
        tbldatos5.AddCell(Provee);
        tbldatos5.AddCell(Descripcion);
        tbldatos5.AddCell(cantidad);
        tbldatos5.AddCell(costosa);
        tbldatos5.AddCell(costosc);


        document.Add(tbldatos5);

        decimal totalcaj = 0;
        

        string sql = "select fecha,razon_social,nombre,cantidad,montoAutorizado,monto1,(montoAutorizado-monto1) as utilidad from tmp_costo_unidad where id_taller=" + taller + " and id_empresa=" + empresa + " and no_orden=" + ordenT + " and identificador='Caja Chica'";

        DataTable dt = inicializaTbCaja(sql);

        PdfPTable detalle = new PdfPTable(6);
        detalle.WidthPercentage = 100f;

        foreach (DataRow fila in dt.Rows)
        {
            PdfPCell celda = new PdfPCell(new Paragraph(Convert.ToDateTime(fila[0]).ToString("dd/MM/yyyy"), fuente2));
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

            celda = new PdfPCell(new Paragraph(Convert.ToString(fila[3]).ToUpper(), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[4]).ToString("C2"), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[5]).ToString("C2"), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            

            totalcaj = totalcaj + Convert.ToDecimal(fila[5]);
          
        }
        document.Add(detalle);

        Paragraph total = new Paragraph();
        total.Add(new Phrase("Total Caja Chica: " + totalcaj.ToString("C2"), fuente1));
        total.Alignment = Element.ALIGN_RIGHT;
        document.Add(total);
    
        totalReparacion = totalReparacion + totalcaj;

    }

    private void datosalmacen(Document document, int ordenT, int empresa, int taller)
    {
        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        Paragraph paragraph = new Paragraph();
        paragraph.Add(new Phrase("Almacen", fuente1));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        paragraph.Add(new Phrase(Chunk.NEWLINE));
        document.Add(paragraph);

        PdfPTable tbldatos6 = new PdfPTable(5);
        tbldatos6.WidthPercentage = 100f;

        PdfPCell fecha = new PdfPCell(new Paragraph("Fecha", fuenteB));
        PdfPCell Descripcion = new PdfPCell(new Paragraph("Descripcion", fuenteB));
        PdfPCell cantidad = new PdfPCell(new Paragraph("Cantidad", fuenteB));
        PdfPCell costosa = new PdfPCell(new Paragraph("Costo Autorizado", fuenteB));
        PdfPCell costosc = new PdfPCell(new Paragraph("Costo Compra", fuenteB));
        

        fecha.HorizontalAlignment = Element.ALIGN_LEFT;
        fecha.Padding = 2;
        fecha.Border = 0;
        fecha.BackgroundColor = Descripcion.BackgroundColor = cantidad.BackgroundColor = costosa.BackgroundColor = costosc.BackgroundColor = BaseColor.LIGHT_GRAY;
        Descripcion.HorizontalAlignment = Element.ALIGN_CENTER;
        Descripcion.Padding = 2;
        Descripcion.Border = 0;
        cantidad.HorizontalAlignment = Element.ALIGN_CENTER;
        cantidad.Padding = 2;
        cantidad.Border = 0;
        costosa.HorizontalAlignment = Element.ALIGN_CENTER;
        costosa.Padding = 2;
        costosa.Border = 0;
        costosc.HorizontalAlignment = Element.ALIGN_CENTER;
        costosc.Padding = 2;
        costosc.Border = 0;      

        tbldatos6.AddCell(fecha);
        tbldatos6.AddCell(Descripcion);
        tbldatos6.AddCell(cantidad);
        tbldatos6.AddCell(costosa);
        tbldatos6.AddCell(costosc);

        document.Add(tbldatos6);

        decimal totalalm = 0;
        

        string sql = "select fecha,nombre,cantidad,montoAutorizado,monto1,(montoAutorizado-monto1) as utilidad from tmp_costo_unidad where id_taller=" + taller + " and id_empresa=" + empresa + " and no_orden=" + ordenT + " and identificador='Almacen'";
        DataTable dt = inicializaTbAlm(sql);
        PdfPTable detalle = new PdfPTable(5);
        detalle.WidthPercentage = 100f;

        foreach (DataRow fila in dt.Rows)
        {
            PdfPCell celda = new PdfPCell(new Paragraph(Convert.ToDateTime(fila[0]).ToString("dd/MM/yyyy"), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_LEFT;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToString(fila[1]).ToUpper(), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToString(fila[2]).ToUpper(), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[3]).ToString("C2"), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            celda = new PdfPCell(new Paragraph(Convert.ToDecimal(fila[4]).ToString("C2"), fuente2));
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.Padding = 0;
            celda.Border = 0;
            detalle.AddCell(celda);

            


            totalalm = totalalm + Convert.ToDecimal(fila[4]);
         
        }
        document.Add(detalle);

        Paragraph total = new Paragraph();
        total.Add(new Phrase("Total Almacén: " + totalalm.ToString("C2"), fuente1));
        total.Alignment = Element.ALIGN_RIGHT;
        document.Add(total);        
        totalReparacion = totalReparacion + totalalm;
     
    }
}

