using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ImprimeReporteFactuPA
/// </summary>
public class ImprimeReporteFactuPA
{
    public int usuario { get; set; }
    public int empresa { get; set; }
    public int taller { get; set; }
    public int opcion { get; set; }
    public string archivo { get; set; }
    public ImprimeReporteFactuPA()
    {
        usuario = empresa = taller = opcion = 0;
        archivo = string.Empty;
    }
    public void generaReporte(int id)
    {
        try
        {
            ReporFactuPA facturacion = new ReporFactuPA();
            facturacion.empresa = empresa;
            facturacion.taller = taller;
            facturacion.usuario = usuario;
            facturacion.obtieneEnc(id);
            object[] factu = facturacion.retorno;
            if (Convert.ToBoolean(factu[0]))
            {
                DataTable info = (DataTable)factu[1];
                if (info.Rows.Count > 0)
                {
                    var libroExcel = new XLWorkbook();
                    var hoja = libroExcel.Worksheets.Add("Por Pagar");

                    int filas = 1;
                    //hoja.Cell(filas, 1).SetValue("RFC").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Range(filas, 1, filas, 2).Merge().SetValue("Proveedor").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 3).SetValue("Facturas Pendientes").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 4).SetValue("Monto Pendiente").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 5).SetValue("Facturas Pagadas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 6).SetValue("Monto Pagadas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 7).SetValue("Facturas Programadas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 8).SetValue("Monto Programadas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 9).SetValue("Facturas Canceladas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 10).SetValue("Monto Canceladas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 11).SetValue("Facturas Vencidas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    hoja.Cell(filas, 12).SetValue("Monto Vencidas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                    foreach (DataRow r in info.Rows)
                    {
                        filas++;
                        //hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Range(filas, 1, filas, 2).Merge().SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 4).SetValue(Convert.ToDecimal(r[3]).ToString("N2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 5).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 6).SetValue(Convert.ToDecimal(r[5]).ToString("N2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 7).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 8).SetValue(Convert.ToDecimal(r[7]).ToString("N2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 9).SetValue(Convert.ToString(r[8])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[9]).ToString("N2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 11).SetValue(Convert.ToString(r[10])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[11]).ToString("N2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(11).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                        filas++;
                        filas++;
                        filas++;
                        hoja.Cell(filas, 1).SetValue("Factura").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 2).SetValue("Fecha de Revisión").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 3).SetValue("Fecha Programada de Pago").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 4).SetValue("Fecha de Pago").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 5).SetValue("Monto").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 6).SetValue("Orden").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 7).SetValue("Tienda/Taller").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 8).SetValue("Tipo Vehiculo").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 9).SetValue("Marca").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 10).SetValue("Unidad").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 11).SetValue("Modelo").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 12).SetValue("Color").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 13).SetValue("Placas").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 14).SetValue("No. Siniestro").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 15).SetValue("Estatus").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        facturacion.obtieneDet(Convert.ToInt32(r[0]));
                        decimal porPagar = 0;
                        if (Convert.ToBoolean(facturacion.retorno[0]))
                        {
                            DataTable detalle = (DataTable)facturacion.retorno[1];
                            foreach (DataRow d in detalle.Rows)
                            {
                                filas++;
                                string fecharev = "";
                                string fechaPorg = "";
                                string fechaPag = "";

                                try { fecharev = Convert.ToDateTime(d[1]).ToString("yyyy-MM-dd"); } catch (Exception) { fecharev = ""; }
                                try { fechaPorg = Convert.ToDateTime(d[2]).ToString("yyyy-MM-dd"); } catch (Exception) { fechaPorg = ""; }
                                try { fechaPag = Convert.ToDateTime(d[3]).ToString("yyyy-MM-dd"); } catch (Exception) { fechaPag = ""; }
                                string monto = "0.00";
                                try { monto = Convert.ToDecimal(d[4]).ToString("N2"); } catch (Exception) { monto = "0.00"; }
                                switch (Convert.ToString(d[15]))
                                {
                                   
                                    case "PEN":
                                        hoja.Cell(filas, 1).SetValue(Convert.ToString(d[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 2).SetValue(fecharev).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 3).SetValue(fechaPorg).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 4).SetValue(fechaPag).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 5).SetValue(monto).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 6).SetValue(Convert.ToString(d[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 7).SetValue(Convert.ToString(d[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 8).SetValue(Convert.ToString(d[8])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 9).SetValue(Convert.ToString(d[9])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 10).SetValue(Convert.ToString(d[10])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 11).SetValue(Convert.ToString(d[11]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 12).SetValue(Convert.ToString(d[12]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 13).SetValue(Convert.ToString(d[13]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 14).SetValue(Convert.ToString(d[14]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        hoja.Cell(filas, 15).SetValue(Convert.ToString(d[7]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightBlue);
                                        try { porPagar = porPagar + Convert.ToDecimal(d[4]); } catch (Exception) { }
                                        break;
                                    case "PAG":
                                        hoja.Cell(filas, 1).SetValue(Convert.ToString(d[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 2).SetValue(fecharev).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 3).SetValue(fechaPorg).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 4).SetValue(fechaPag).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 5).SetValue(monto).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 6).SetValue(Convert.ToString(d[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 7).SetValue(Convert.ToString(d[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 8).SetValue(Convert.ToString(d[8])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 9).SetValue(Convert.ToString(d[9])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 10).SetValue(Convert.ToString(d[10])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 11).SetValue(Convert.ToString(d[11]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 12).SetValue(Convert.ToString(d[12]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 13).SetValue(Convert.ToString(d[13]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 14).SetValue(Convert.ToString(d[14]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        hoja.Cell(filas, 15).SetValue(Convert.ToString(d[7]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen);
                                        break;
                                    case "PRO":
                                        hoja.Cell(filas, 1).SetValue(Convert.ToString(d[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 2).SetValue(fecharev).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 3).SetValue(fechaPorg).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 4).SetValue(fechaPag).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 5).SetValue(monto).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 6).SetValue(Convert.ToString(d[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 7).SetValue(Convert.ToString(d[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 8).SetValue(Convert.ToString(d[8])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 9).SetValue(Convert.ToString(d[9])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 10).SetValue(Convert.ToString(d[10])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 11).SetValue(Convert.ToString(d[11]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 12).SetValue(Convert.ToString(d[12]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 13).SetValue(Convert.ToString(d[13]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 14).SetValue(Convert.ToString(d[14]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        hoja.Cell(filas, 15).SetValue(Convert.ToString(d[7]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.CelestialBlue);
                                        try { porPagar = porPagar + Convert.ToDecimal(d[4]); } catch (Exception) { }
                                        break;
                                    case "CAN":
                                        hoja.Cell(filas, 1).SetValue(Convert.ToString(d[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 2).SetValue(fecharev).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 3).SetValue(fechaPorg).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 4).SetValue(fechaPag).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 5).SetValue(monto).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 6).SetValue(Convert.ToString(d[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 7).SetValue(Convert.ToString(d[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 8).SetValue(Convert.ToString(d[8])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 9).SetValue(Convert.ToString(d[9])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 10).SetValue(Convert.ToString(d[10])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 11).SetValue(Convert.ToString(d[11]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 12).SetValue(Convert.ToString(d[12]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 13).SetValue(Convert.ToString(d[13]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 14).SetValue(Convert.ToString(d[14]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        hoja.Cell(filas, 15).SetValue(Convert.ToString(d[7]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.OrangeRed);
                                        break;
                                    case "VEN":
                                        hoja.Cell(filas, 1).SetValue(Convert.ToString(d[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 2).SetValue(fecharev).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 3).SetValue(fechaPorg).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 4).SetValue(fechaPag).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 5).SetValue(monto).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 6).SetValue(Convert.ToString(d[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 7).SetValue(Convert.ToString(d[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 8).SetValue(Convert.ToString(d[8])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 9).SetValue(Convert.ToString(d[9])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 10).SetValue(Convert.ToString(d[10])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 11).SetValue(Convert.ToString(d[11]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 12).SetValue(Convert.ToString(d[12]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 13).SetValue(Convert.ToString(d[13]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 14).SetValue(Convert.ToString(d[14]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        hoja.Cell(filas, 15).SetValue(Convert.ToString(d[7]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.YellowProcess);
                                        try { porPagar = porPagar + Convert.ToDecimal(d[4]); } catch (Exception) { }
                                        break;
                                    default:
                                        hoja.Cell(filas, 1).SetValue(Convert.ToString(d[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 2).SetValue(fecharev).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 3).SetValue(fechaPorg).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 4).SetValue(fechaPag).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 5).SetValue(monto).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 6).SetValue(Convert.ToString(d[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 7).SetValue(Convert.ToString(d[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 8).SetValue(Convert.ToString(d[8])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 9).SetValue(Convert.ToString(d[9])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 10).SetValue(Convert.ToString(d[10])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 11).SetValue(Convert.ToString(d[11]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 12).SetValue(Convert.ToString(d[12]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 13).SetValue(Convert.ToString(d[13]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 14).SetValue(Convert.ToString(d[14]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        hoja.Cell(filas, 15).SetValue(Convert.ToString(d[7]).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                        try { porPagar = porPagar + Convert.ToDecimal(d[4]); } catch (Exception) { }
                                        break;
                                }
                            }
                        }
                        filas++;
                        hoja.Range(filas, 1, filas, 14).Merge().SetValue("Total Por Pagar: ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        hoja.Cell(filas, 15).SetValue(porPagar.ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                        filas++;
                        filas++;
                    }
                    string ruta = HttpContext.Current.Server.MapPath("~/TMP");
                    string archivoGuardado = ruta + "\\Cuentas por Pagar.xlsx";

                    if (!Directory.Exists(ruta))
                        Directory.CreateDirectory(ruta);

                    FileInfo docto = new FileInfo(archivoGuardado);
                    if (docto.Exists)
                        docto.Delete();

                    libroExcel.SaveAs(archivoGuardado);
                    archivo = archivoGuardado;
                }
                else
                    archivo = "";
            }
            else
                archivo = "";
        }
        catch (Exception ex) { archivo = ""; }
    }
}