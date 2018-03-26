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
using iTextSharp.text.pdf.parser;
using iTextSharp.text.io;
using System.Diagnostics;
using System.IO;


public class OrdenTrabajo
{
    Ejecuciones ejecuta = new Ejecuciones();

    public OrdenTrabajo()
    {
    }

    public string GenRepOrdTrabajo(int empresa, int taller, int orden, string nombre_taller, string usuario)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        documento.AddTitle("OrdenTrabajo" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "OrdenTrabajo" + empresa.ToString() + "_T" + taller.ToString() + "_Orden" + orden.ToString() + ".pdf";

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

            Paragraph titu = new Paragraph("ORDEN TRABAJO", FontFactory.GetFont("ARIAL", 13, iTextSharp.text.Font.BOLD));
            titu.Alignment = Element.ALIGN_CENTER;
            documento.Add(titu);

            string fecha = "Fecha: " + DateTime.Today.ToString();

            Paragraph num_orden = new Paragraph("N° Orden: " + orden, FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
            num_orden.Alignment = Element.ALIGN_RIGHT;
            documento.Add(num_orden);
            
            
            documento.Add(new Paragraph(" "));
            DatosCab(documento, empresa, taller, orden, nombre_taller, usuario);
            documento.Add(new Paragraph(" "));           
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();

        }
        return archivo;
    }


    private void DatosCab(Document document, int empresa, int taller, int orden, string nombre_taller, string usuario)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        //Obtener Datos Aseguradora
        
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
                    PdfPTable tablaEncabezado = new PdfPTable(2);
                    tablaEncabezado.WidthPercentage = 100f;
                    tablaEncabezado.DefaultCell.Border = 0;
                    PdfPTable tablaDatos = new PdfPTable(1);
                    tablaDatos.WidthPercentage = 100f;
                    tablaDatos.DefaultCell.Border = 0;
                    PdfPTable tablaEmpleados = new PdfPTable(1);
                    tablaEmpleados.WidthPercentage = 100f;
                    tablaEmpleados.DefaultCell.Border = 0;

                    //PdfPCell torre = new PdfPCell(new );

                    /*Paragraph torre = new Paragraph("Torre: " + valores[0].ToString(), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    torre.Alignment = Element.ALIGN_RIGHT;
                    document.Add(torre);
                    document.Add(new Paragraph(" "));*/
                    Paragraph fecharep = new Paragraph("Fecha Ingreso: " + Convert.ToDateTime(valores[17]).ToString("dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                    fecharep.Alignment = Element.ALIGN_RIGHT;
                    document.Add(fecharep);

                    if (valores[18].ToString() != "" && valores[18].ToString() != "1900-01-01" && valores[18] != null)
                    {
                        Paragraph fechatra = new Paragraph("Fecha Reingreso: " + Convert.ToDateTime(valores[18]).ToString("dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
                        fechatra.Alignment = Element.ALIGN_RIGHT;
                        document.Add(fechatra);
                    }


                    document.Add(new Paragraph(" "));

                    tablaDatos.AddCell(new Paragraph(" "));
                    tablaDatos.AddCell(new Paragraph(valores[1].ToString(), _standardFont1));                    
                    tablaDatos.AddCell(new Paragraph(" ", _standardFont));                    
                    tablaDatos.AddCell(new Paragraph("Vehículo: " + valores[5].ToString().ToUpper(), _standardFont));                    
                    tablaDatos.AddCell(new Paragraph("Modelo: " + valores[6].ToString(), _standardFont));
                    tablaDatos.AddCell(new Paragraph("Placas: " + valores[7].ToString(), _standardFont2));
                    tablaDatos.AddCell(new Paragraph("Propiedad de: " + valores[8].ToString(), _standardFont));
                    tablaDatos.AddCell(new Paragraph("N° Póliza: " + valores[9].ToString(), _standardFont));
                    tablaDatos.AddCell(new Paragraph("N° Siniestro: " + valores[10].ToString(), _standardFont));
                    tablaDatos.AddCell(new Paragraph("Fecha Promesa: " + Convert.ToDateTime(valores[11].ToString()).ToString("dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    Operativos(tablaEmpleados, empresa, taller, orden);
                    tablaEncabezado.AddCell(tablaDatos);
                    tablaEncabezado.AddCell(tablaEmpleados);
                    document.Add(tablaEncabezado);
                }
                i++;
            }
        }
        document.Add(new Paragraph(" ", estilo2));
        Mano_Obra(document, empresa, taller, orden);
        Refacciones(document, empresa, taller, orden);
    }

    private void Operativos(PdfPTable tablaEmpleados, int empresa, int taller, int orden)
    {
        // Tipo de Fuentes

        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


        //Obtener Datos Mano de Obra
        Vehiculos CarManObra = new Vehiculos();

        object[] camposManObra = CarManObra.DatosOperativos(orden, empresa, taller);

        if (Convert.ToBoolean(camposManObra[0]))
        {
            DataSet datos = (DataSet)camposManObra[1];
            if (datos.Tables[0].Rows.Count != 0)
            {
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                tablaEmpleados.AddCell(new Paragraph(" ", _standardFont));
                PdfPTable tableo = new PdfPTable(1);
                PdfPCell cello = new PdfPCell(new Phrase("O P E R A T I V O S", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                cello.BorderWidth = 0;
                cello.Padding = 5;
                cello.PaddingTop = 3;
                cello.HorizontalAlignment = Element.ALIGN_CENTER;
                cello.BackgroundColor = BaseColor.LIGHT_GRAY;
                tableo.AddCell(cello);
                tableo.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);
                tableo.HorizontalAlignment = Element.ALIGN_CENTER;
                tablaEmpleados.AddCell(tableo);

                PdfPTable table = new PdfPTable(1);
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(fila[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.BorderWidth = 0;
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                }
                table.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                tablaEmpleados.AddCell(table);
            }
        }
    }

    private void Refacciones(Document document, int empresa, int taller, int orden)
    {
        // Tipo de Fuentes

        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


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

                PdfPTable table = new PdfPTable(1);
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    PdfPCell cell=null;
                    if (fila[4].ToString() == "RP")
                    {
                        cell = new PdfPCell(new Phrase(fila[1].ToString() + "     " + fila[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                        cell.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                    }
                    else
                        cell = new PdfPCell(new Phrase(fila[1].ToString() + "     " + fila[2].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell.BorderWidth = 0;
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                }
                table.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(table);
            }
        }
    }


    private void Mano_Obra(Document document, int empresa, int taller, int orden)
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

                PdfPTable table = new PdfPTable(1);
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(fila[1].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                    cell.BorderWidth = 0;
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    obtieneManosObra(document, empresa, taller, orden, fila[0].ToString(), table);
                }
                table.SetWidthPercentage(new float[1] { 598f }, PageSize.LETTER);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(table);
            }
        }
    }

    private void obtieneManosObra(Document document, int empresa, int taller, int orden, string grupo,PdfPTable tabla)
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
                tabla.AddCell(cell);
            }            
        }
    }
}
        
