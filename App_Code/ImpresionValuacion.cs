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
/// Descripción breve de ImpresionValuacion
/// </summary>
public class ImpresionValuacion
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    public ImpresionValuacion()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GenRepValuacion(int noOrden, int idEmpresa, int idTaller)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER.Rotate());
        documento.AddTitle("Valuacion_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString());
        documento.AddCreator("MoncarWeb");

        string ruta = HttpContext.Current.Server.MapPath("~/files");
        string archivo = ruta + "\\" + "Valuacion_E" + idEmpresa.ToString() + "_T" + idTaller.ToString() + "_Orden" + noOrden.ToString() + ".pdf";

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
            string empresa = ejecuta.scalarToStringSimple("select razon_social from empresas where id_empresa=" + idEmpresa.ToString());
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

            PdfPCell titu = new PdfPCell(new Phrase(empresa, FontFactory.GetFont("ARIAL", 15, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 1;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(titu);
            tablaEncabezado.AddCell(" ");
            titu = new PdfPCell(new Phrase("Informe de Seguimiento de Refacciones", FontFactory.GetFont("ARIAL", 15, iTextSharp.text.Font.BOLD)));
            titu.HorizontalAlignment = 1;
            titu.Border = 0;
            titu.VerticalAlignment = Element.ALIGN_BOTTOM;
            tablaEncabezado.AddCell(titu);
            tablaEncabezado.AddCell(" ");
            documento.Add(tablaEncabezado);

            DatosCab(documento, idEmpresa, idTaller, noOrden);
            documento.Add(new Paragraph(" "));
            documento.AddCreationDate();
            documento.Add(new Paragraph(""));
            documento.Close();
        }
        return archivo;
    }

    private void DatosCab(Document document, int idEmpresa, int idTaller, int noOrden)
    {
        // Tipo de Fuentes
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font _standardFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        //Obtener Datos Aseguradora
        PdfPTable tablaEncabezado = new PdfPTable(1);
        tablaEncabezado.DefaultCell.Border = 0;
        tablaEncabezado.WidthPercentage = 100f;

        string sql = "select m.descripcion+' '+tu.descripcion+' '+cast(v.modelo as char(4))+' 'as descripcion,rtrim(ltrim(orp.placas)) as placas, c.razon_social as aseguradora,orp.no_siniestro, convert(varchar,so.f_entrega_estimada,103)as f_entrega_estimada from Ordenes_Reparacion orp inner join seguimiento_orden so on so.no_orden = orp.no_orden and so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo left join Marcas m on m.id_marca = orp.id_marca inner join Cliprov c on c.id_cliprov = orp.id_cliprov left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad where orp.id_empresa = " + idEmpresa.ToString() + " and orp.id_taller = " + idTaller.ToString() + " and orp.no_orden =" + noOrden.ToString();
        object[] vehiculo = ejecuta.dataSet(sql);
        string[] camposEncabezado = new string[5];
        if (Convert.ToBoolean(vehiculo[0]))
        {
            DataSet valorex = (DataSet)vehiculo[1];
            foreach (DataRow fila in valorex.Tables[0].Rows)
            {
                for (int contadorEnca = 0; contadorEnca < fila.ItemArray.Length; contadorEnca++)
                {
                    camposEncabezado[contadorEnca] = fila[contadorEnca].ToString().ToUpper();
                }
                break;
            }
        }

        PdfPTable tablaSubEncabezado = new PdfPTable(6);
        tablaSubEncabezado.DefaultCell.Border = 0;
        tablaSubEncabezado.WidthPercentage = 100f;
        string unidad = camposEncabezado[0];
        string placas = camposEncabezado[1];
        string aseguradora = camposEncabezado[2];
        string noSiniestro = camposEncabezado[3];
        string fechaEstimada = camposEncabezado[4];

        PdfPCell cellSubEncab = new PdfPCell(new Phrase("Aseguradora: ", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD)));
        cellSubEncab.HorizontalAlignment = 2;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase(aseguradora, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL)));
        cellSubEncab.HorizontalAlignment = 0;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase("Siniestro: ", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD)));
        cellSubEncab.HorizontalAlignment = 2;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase(noSiniestro, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL)));
        cellSubEncab.HorizontalAlignment = 0;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase("Placas: ", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD)));
        cellSubEncab.HorizontalAlignment = 2;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase(placas, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL)));
        cellSubEncab.HorizontalAlignment = 0;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase("No. Orden: ", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD)));
        cellSubEncab.HorizontalAlignment = 2;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        string prefijoTaller = ejecuta.scalarToStringSimple("select identificador from talleres where id_taller=" + idTaller.ToString());
        cellSubEncab = new PdfPCell(new Phrase(prefijoTaller.Trim()+" "+noOrden.ToString(), FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL)));
        cellSubEncab.HorizontalAlignment = 0;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase("Unidad: ", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD)));
        cellSubEncab.HorizontalAlignment = 2;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase(unidad, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL)));
        cellSubEncab.HorizontalAlignment = 0;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase("Entrega Estimada: ", FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.BOLD)));
        cellSubEncab.HorizontalAlignment = 2;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        cellSubEncab = new PdfPCell(new Phrase(fechaEstimada, FontFactory.GetFont("ARIAL", 11, iTextSharp.text.Font.NORMAL)));
        cellSubEncab.HorizontalAlignment = 0;
        cellSubEncab.Border = 0;
        tablaSubEncabezado.AddCell(cellSubEncab);

        document.Add(tablaSubEncabezado);
        document.Add(new Paragraph(" ", estilo2));
        Datos_valuacion(document, idEmpresa, idTaller, noOrden);
    }

    private void Datos_valuacion(Document document, int idEmpresa, int idTaller, int noOrden)
    {
        // Tipo de Fuentes

        iTextSharp.text.Font estilo2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);


        //Obtener Datos Mano de Obra
        //checar de donde sale el devolucion
        string sql = "SELECT refCantidad,refDescripcion,isnull((case convert(varchar,ro.refFechSolicitud,103) when '1900-01-01' then '' else convert(varchar,ro.refFechSolicitud,103) end),'') as refFechSolicitud,isnull((case convert(varchar,ro.refFechEntregaEst,103) when '1900-01-01' then '' else convert(varchar,ro.refFechEntregaEst,103) end),'') as refFechEntregaEst,isnull((case convert(varchar,ro.refFechEntregaReal,103) when '1900-01-01' then '' else convert(varchar,ro.refFechEntregaReal,103) end),'') as refFechEntregaReal,(SELECT razon_social FROM Cliprov WHERE id_cliprov = ro.refProveedor AND tipo = 'P') AS provRazSoc,rafe.staDescripcion,ro.observacion FROM Refacciones_Orden ro  inner join Rafacciones_Estatus rafe on rafe.staRefID=ro.refEstatusSolicitud WHERE (ref_no_orden = " + noOrden.ToString() + ") and ref_id_empresa = " + idEmpresa.ToString() + " and ref_id_taller =" + idTaller.ToString();
        object[] camposValuacion = ejecuta.dataSet(sql);

        if (Convert.ToBoolean(camposValuacion[0]))
        {
            DataSet datos = (DataSet)camposValuacion[1];

            if (datos.Tables[0].Rows.Count != 0)
            {
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font _standardFonTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                //checar como caen los datos en debug
                PdfPTable table = new PdfPTable(8);
                table.WidthPercentage = 100f;
                table.SetWidths(new float[] { 1f, 2f, 1f, 1f, 1f, 1f, 1f, 1f });
                string[] titulosTabla = { "Cantidad", "Refacción", "Proveedor", "Fecha Solicitud", "Estatus", "Entrega Estimada", "Entrega Real", "Retrazo" };

                for (int iCont = 0; iCont < titulosTabla.Length; iCont++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(titulosTabla[iCont].ToString(), _standardFonTitle));
                    cell.Border = 0;
                    cell.Padding = 2;
                    cell.PaddingTop = 2;
                    cell.PaddingBottom = 2;
                    if (iCont==1 || iCont == 2)
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    else
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                PdfPCell cellLinea = new PdfPCell(new Phrase(" ", _standardFont));
                cellLinea.BorderWidthTop = 1;
                cellLinea.BorderWidthBottom = 0;
                cellLinea.BorderWidthLeft = 0;
                cellLinea.BorderWidthRight = 0;
                cellLinea.Colspan = 8;
                cellLinea.Padding = 2;
                cellLinea.PaddingTop = 2;
                cellLinea.PaddingBottom = 2;
                cellLinea.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cellLinea);

                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    int diasRetrazo = 0;
                    string cantidad, refacción, proveedor, fechaSolicitud, estatus, entregaEstimada, entregaReal, observacionCancel;
                    cantidad = refacción = proveedor = fechaSolicitud = estatus = entregaEstimada = entregaReal = observacionCancel = "";
                    cantidad=fila[0].ToString();
                    refacción = fila[1].ToString();
                    proveedor = fila[5].ToString();
                    fechaSolicitud = fila[2].ToString();
                    estatus = fila[6].ToString();
                    entregaEstimada = fila[3].ToString();
                    entregaReal = fila[4].ToString();
                    observacionCancel= fila[7].ToString();
                    try
                    {
                        DateTime fechaAResta = fechas.obtieneFechaLocal();
                        if (entregaReal != "")
                            fechaAResta = Convert.ToDateTime(entregaReal);

                        TimeSpan retrazoDias = Convert.ToDateTime(entregaEstimada) - fechaAResta;
                        diasRetrazo = retrazoDias.Days;
                        if (fechaAResta < Convert.ToDateTime(entregaEstimada))
                            diasRetrazo = diasRetrazo * -1;

                        //diasRetrazo = Math.Abs(diasRetrazo) + 1;
                    }
                    catch (Exception ex)
                    {
                        diasRetrazo = 0;
                    }

                    string[] detalleCampos = { cantidad, refacción, proveedor, fechaSolicitud, estatus, entregaEstimada, entregaReal, diasRetrazo.ToString() };
                    for (int iCont = 0; iCont < detalleCampos.Length; iCont++)
                    {
                        if (estatus != "Solicitado" && estatus != "Devolucion" && estatus != "Cancelado" && estatus != "Surtido")
                            detalleCampos[2] = "";
                        PdfPCell cell = new PdfPCell(new Phrase(detalleCampos[iCont].ToString(), _standardFont));
                        cell.Border = 0;
                        cell.Padding = 2;
                        cell.PaddingTop = 2;
                        cell.PaddingBottom = 2;
                        if (iCont == 1 || iCont == 2)
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                    if (estatus=="Devolucion" || estatus=="Cancelado" )
                    {
                        if (observacionCancel == "" || observacionCancel == "null")
                            observacionCancel = "Sin justificación";
                        PdfPCell cell = new PdfPCell(new Phrase(estatus + ": " + observacionCancel, _standardFont));
                        cell.Border = 0;
                        cell.Colspan = 8;
                        cell.Padding = 2;
                        cell.PaddingTop = 2;
                        cell.PaddingBottom = 2;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                }
                //pintar total al final suma de importes menos de los cancelados o en devolucion
                PdfPTable tableTotal = new PdfPTable(1);
                tableTotal.WidthPercentage = 100f;
                document.Add(table);
            }
        }
    }
}