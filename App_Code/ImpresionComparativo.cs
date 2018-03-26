
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
/// Descripción breve de ImpresionComparativo
/// </summary>
public class ImpresionComparativo
{
    Ejecuciones ejecuta = new Ejecuciones();
    string sql;
    bool resultado;
    object[] ejecutados = new object[2];

    public ImpresionComparativo()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GenComparativo(int[] sessiones, int opcion)
    {
        int noOrden = sessiones[4];
        int idEmpresa = sessiones[2];
        int idTaller = sessiones[3];
        int idUsuario = sessiones[0];
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        documento.AddTitle("ComparativoCotizacion_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "ComparativoCotizacion_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + ".pdf";
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
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "logo.png");
            gif.WidthPercentage = 15f;

            PdfPTable tablaEncabezado = new PdfPTable(3);
            tablaEncabezado.SetWidths(new float[] { 2.5f, 5f, 2.5f });
            tablaEncabezado.DefaultCell.Border = 0;
            tablaEncabezado.WidthPercentage = 100f;

            string prefijoTaller = ejecuta.scalarToStringSimple("select ltrim(rtrim(identificador)) from talleres where id_taller=" + idTaller.ToString());

            PdfPCell tituPagina = new PdfPCell(new Phrase("Comparativo de Refacciones", FontFactory.GetFont("ARIAL", 16, iTextSharp.text.Font.BOLD)));
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

            Mano_Obra(documento, idEmpresa, idTaller, noOrden, sessiones, opcion);
            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void Mano_Obra(Document documento, int idEmpresa, int idTaller, int noOrden, int[] sessiones, int opcion)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente10 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente8 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente6 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente4 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente3 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        //Obtener Datos Comparativo
        datosCotizaProv cotizacion = new datosCotizaProv();
        if (opcion!=1)
            sql = "select rank() over(order by id_cliprov),id_cliprov from Cotizacion_Detalle where id_empresa=" + sessiones[2].ToString() + " and id_taller=" + sessiones[3].ToString() + " and no_orden=" + sessiones[4].ToString() + " and id_cotizacion=" + sessiones[6].ToString() + " group by id_cliprov";
        else if (opcion == 1)
            sql = "select rank() over(order by id_cliprov),id_cliprov from Cotizacion_Detalle where id_empresa=" + sessiones[2].ToString() + " and id_taller=" + sessiones[3].ToString() + " and no_orden=" + sessiones[4].ToString() + " group by id_cliprov";
        DataSet proveedoresCotizacion = new DataSet();
        object[] proveedoresCotizantes = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(proveedoresCotizantes[0]))
        {
            proveedoresCotizacion = (DataSet)proveedoresCotizantes[1];
        }

        int proveedoresTotalCotz = proveedoresCotizacion.Tables[0].Rows.Count;
        //meter proveedoresTotalCotz en la generacion de columnas para saber los proveedores a pintar
        object[] camposManObra = new object[2];
        if (opcion!=1)
            camposManObra = cotizacion.generaComparativo(sessiones);
        else if (opcion == 1)
            camposManObra = cotizacion.generaComparativoGral(sessiones);
        if (Convert.ToBoolean(camposManObra[0]))
        {
            DataSet datos = (DataSet)camposManObra[1];
            if (datos.Tables[0].Rows.Count != 0)
            {
                documento.Add(new Paragraph(" "));
                PdfPTable encaTa = new PdfPTable(10);
                encaTa.SetWidths(new float[] { 10, 10, 30, 10, 5, 5, 5, 5, 10,10 });
                //encaTa.DefaultCell.Border = 0;
                encaTa.WidthPercentage = 100f;
                encaTa.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell refac = new PdfPCell(new Phrase("Refacción", fuente8));
                refac.HorizontalAlignment = 1;
                //refac.Border = 0;
                refac.BackgroundColor = BaseColor.LIGHT_GRAY;
                refac.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(refac);

                PdfPCell Cant = new PdfPCell(new Phrase("Cantidad", fuente8));
                Cant.HorizontalAlignment = 1;
                //Cant.Border = 0;
                Cant.BackgroundColor = BaseColor.LIGHT_GRAY;
                Cant.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(Cant);

                PdfPCell Prov = new PdfPCell(new Phrase("Proveedor", fuente8));
                Prov.HorizontalAlignment = 1;
                //Prov.Border = 0;
                Prov.BackgroundColor = BaseColor.LIGHT_GRAY;
                Prov.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(Prov);

                PdfPCell cu = new PdfPCell(new Phrase("C.U.", fuente8));
                cu.HorizontalAlignment = 1;
                //cu.Border = 0;
                cu.BackgroundColor = BaseColor.LIGHT_GRAY;
                cu.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(cu);

                PdfPCell porce = new PdfPCell(new Phrase("P%", fuente8));
                porce.HorizontalAlignment = 1;
                //porce.Border = 0;
                porce.BackgroundColor = BaseColor.LIGHT_GRAY;
                porce.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(porce);

                PdfPCell desc = new PdfPCell(new Phrase("Desc", fuente8));
                desc.HorizontalAlignment = 1;
                //desc.Border = 0;
                desc.BackgroundColor = BaseColor.LIGHT_GRAY;
                desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(desc);

                PdfPCell exit = new PdfPCell(new Phrase("Exist", fuente8));
                exit.HorizontalAlignment = 1;
                //exit.Border = 0;
                exit.BackgroundColor = BaseColor.LIGHT_GRAY;
                exit.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(exit);

                PdfPCell Ent = new PdfPCell(new Phrase("Días", fuente8));
                Ent.HorizontalAlignment = 1;
                //Ent.Border = 0;
                Ent.BackgroundColor = BaseColor.LIGHT_GRAY;
                Ent.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(Ent);

                PdfPCell Cf = new PdfPCell(new Phrase("Cost. Final", fuente8));
                Cf.HorizontalAlignment = 1;
                //Cf.Border = 0;
                Cf.BackgroundColor = BaseColor.LIGHT_GRAY;
                Cf.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(Cf);
                PdfPCell Fl = new PdfPCell(new Phrase("Fecha Límite", fuente8));
                Fl.HorizontalAlignment = 1;
                //Cf.Border = 0;
                Fl.BackgroundColor = BaseColor.LIGHT_GRAY;
                Fl.VerticalAlignment = Element.ALIGN_MIDDLE;
                encaTa.AddCell(Fl);

                documento.Add(encaTa);
                int refaccionesTotales = datos.Tables[0].Rows.Count;
               
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    


                        int rs = 5, cunit = 6, pd = 7, id = 8, im = 9, ex = 10, ds = 11, cant = 2, descri = 3,fechLimi=13;
                    

                        for (int cont = 0; cont < proveedoresTotalCotz; cont++)
                        {
                            if (fila[im].ToString() != "0" && fila[im].ToString() != "0.00")
                            {
                                PdfPTable encaTa2 = new PdfPTable(10);
                                encaTa2.SetWidths(new float[] { 10, 10, 30, 10, 5, 5, 5, 5, 10,10 });
                                //encaTa.DefaultCell.Border = 0;
                                encaTa2.WidthPercentage = 100f;
                                encaTa2.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell refac1 = new PdfPCell(new Phrase("" + fila[descri].ToString(), fuente6));
                                refac1.HorizontalAlignment = Element.ALIGN_CENTER;
                                //refac1.Border = 0;
                                refac1.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(refac1);

                                PdfPCell Cant2 = new PdfPCell(new Phrase("" + fila[cant].ToString(), fuente6));
                                Cant2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Cant2.Border = 0;
                                Cant2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(Cant2);

                                PdfPCell Prov2 = new PdfPCell(new Phrase("" + fila[rs].ToString(), fuente6));
                                Prov2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Prov2.Border = 0;
                                Prov2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(Prov2);

                                PdfPCell cu2 = new PdfPCell(new Phrase("" + fila[cunit].ToString(), fuente6));
                                cu2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //cu2.Border = 0;
                                cu2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(cu2);

                                PdfPCell porce2 = new PdfPCell(new Phrase("" + fila[pd].ToString(), fuente6));
                                porce2.HorizontalAlignment = Element.ALIGN_CENTER;
                                // porce2.Border = 0;
                                porce2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(porce2);

                                PdfPCell desc2 = new PdfPCell(new Phrase("" + fila[id].ToString(), fuente6));
                                desc2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //desc2.Border = 0;
                                desc2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(desc2);

                                string truFal = "";
                                if (fila[ex].ToString() == "True")
                                    truFal = "Si";
                                else
                                    truFal = "No";
                                PdfPCell exit2 = new PdfPCell(new Phrase("" + truFal, fuente6));
                                exit2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //exit2.Border = 0;
                                exit2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(exit2);

                                PdfPCell Ent2 = new PdfPCell(new Phrase("" + fila[ds].ToString(), fuente6));
                                Ent2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Ent2.Border = 0;
                                Ent2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(Ent2);

                                PdfPCell Cf2 = new PdfPCell(new Phrase("" + fila[im].ToString(), fuente6));
                                Cf2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Cf2.Border = 0;
                                Cf2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(Cf2);

                                string fechi = fila[fechLimi].ToString();

                                if (fechi == "1900-01-01")
                                {
                                    fechi = "";
                                }
                                PdfPCell fL2 = new PdfPCell(new Phrase("" + fechi, fuente6));
                                fL2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //Cf2.Border = 0;
                                fL2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                encaTa2.AddCell(fL2);

                                documento.Add(encaTa2);
                            }

                            rs += 10; cunit += 10; pd += 10; id += 10; im += 10; ex += 10; ds += 10; fechLimi += 10;
                        }
                    

                }
                /*PdfPTable tablaEncabezadoFila = new PdfPTable(3);
                tablaEncabezadoFila.SetWidths(new float[] { 10F, 10F, (proveedoresTotalCotz*20f) });
                tablaEncabezadoFila.WidthPercentage = 100f;
                PdfPTable tablaTitFila = new PdfPTable(proveedoresTotalCotz+2);

                float[] arrAnchos = new float[proveedoresTotalCotz+2];
                arrAnchos[0] = arrAnchos[1] = 10f;
                for (int contaFor=2;contaFor<arrAnchos.Length;contaFor++)
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
                documento.Add(tablaTitFila);*/
            }
        
        }
    }
}