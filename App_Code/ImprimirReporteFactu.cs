using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ImprimirReporteFactu
/// </summary>
public class ImprimirReporteFactu
{
    public int usuario { get; set; }
    public int empresa { get; set; }
    public int taller { get; set; }
    public int opcion { get; set; }
    public string archivo { get; set; }
    public string Ini { get; set; }
    public string Fin { get; set; }
    public ImprimirReporteFactu()
    {
        usuario = empresa = taller = opcion = 0;
        archivo = Ini = Fin = string.Empty;
    }
    public void generaReporte()
    {
        try
        {
            ReporFactu facturacion = new ReporFactu();
            facturacion.empresa = empresa;
            facturacion.taller = taller;
            facturacion.usuario = usuario;
            facturacion.Ini = Ini;
            facturacion.Fin = Fin;
            facturacion.obtieneCFD();
            object[] factu = facturacion.retorno;
            if (Convert.ToBoolean(factu[0]))
            {
                DataTable info = (DataTable)factu[1];
                if (info.Rows.Count > 0)
                {
                    var libroExcel = new XLWorkbook();
                    var hoja = libroExcel.Worksheets.Add("Facturas");

                    int filas = 1;
                    hoja.Cell(filas, 1).SetValue("RFC").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 2).SetValue("NOMBRE").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 3).SetValue("UUID").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 4).SetValue("FOLIO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 5).SetValue("FACTURA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 6).SetValue("FECHA GENERA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 7).SetValue("FECHA TIMBRADO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 8).SetValue("FECHA CANCELA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 9).SetValue("NETO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 10).SetValue("DESCUENTO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 11).SetValue("SUBTOTAL").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);                    
                    hoja.Cell(filas, 12).SetValue("TRASLADOS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 13).SetValue("RETENCIONES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 14).SetValue("TOTAL").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 15).SetValue("TALLER/TIENDA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 16).SetValue("ORDEN/TICKET").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 17).SetValue("VEHICULO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 18).SetValue("MODELO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 19).SetValue("COLOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 20).SetValue("PLACAS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 21).SetValue("SINIESTRO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 22).SetValue("MONTO PAGO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 23).SetValue("FECHA PAGO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    hoja.Cell(filas, 24).SetValue("NOTAS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black);
                    foreach (DataRow r in info.Rows)
                    {
                        
                        filas++;
                        switch (Convert.ToString(r[25])) {
                            case "C":
                                hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 2).SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 4).SetValue(Convert.ToString(r[3])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 5).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 6).SetValue(Convert.ToString(r[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 7).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 8).SetValue(Convert.ToString(r[7])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 9).SetValue(Convert.ToDecimal(r[8]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[9]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 11).SetValue(Convert.ToDecimal(r[10]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);                                
                                hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[12]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 13).SetValue(Convert.ToDecimal(r[13]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 14).SetValue(Convert.ToDecimal(r[14]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 15).SetValue(Convert.ToString(r[15])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 16).SetValue(Convert.ToString(r[16])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 17).SetValue(Convert.ToString(r[17])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 18).SetValue(Convert.ToString(r[18])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 19).SetValue(Convert.ToString(r[19])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 20).SetValue(Convert.ToString(r[20])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 21).SetValue(Convert.ToString(r[21])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 22).SetValue(Convert.ToDecimal(r[22]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 23).SetValue(Convert.ToString(r[23])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                hoja.Cell(filas, 24).SetValue(Convert.ToString(r[24])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Red);
                                break;
                            case "T":
                                hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 2).SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 4).SetValue(Convert.ToString(r[3])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 5).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 6).SetValue(Convert.ToString(r[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 7).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 8).SetValue(Convert.ToString(r[7])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 9).SetValue(Convert.ToDecimal(r[8]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[9]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 11).SetValue(Convert.ToDecimal(r[10]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[12]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 13).SetValue(Convert.ToDecimal(r[13]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 14).SetValue(Convert.ToDecimal(r[14]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 15).SetValue(Convert.ToString(r[15])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 16).SetValue(Convert.ToString(r[16])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 17).SetValue(Convert.ToString(r[17])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 18).SetValue(Convert.ToString(r[18])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 19).SetValue(Convert.ToString(r[19])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 20).SetValue(Convert.ToString(r[20])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 21).SetValue(Convert.ToString(r[21])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 22).SetValue(Convert.ToDecimal(r[22]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 23).SetValue(Convert.ToString(r[23])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 24).SetValue(Convert.ToString(r[24])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                break;
                            case "P":
                                hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 2).SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 4).SetValue(Convert.ToString(r[3])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 5).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 6).SetValue(Convert.ToString(r[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 7).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 8).SetValue(Convert.ToString(r[7])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 9).SetValue(Convert.ToDecimal(r[8]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[9]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 11).SetValue(Convert.ToDecimal(r[10]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[12]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 13).SetValue(Convert.ToDecimal(r[13]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 14).SetValue(Convert.ToDecimal(r[14]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 15).SetValue(Convert.ToString(r[15])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 16).SetValue(Convert.ToString(r[16])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 17).SetValue(Convert.ToString(r[17])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 18).SetValue(Convert.ToString(r[18])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 19).SetValue(Convert.ToString(r[19])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 20).SetValue(Convert.ToString(r[20])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 21).SetValue(Convert.ToString(r[21])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 22).SetValue(Convert.ToDecimal(r[22]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 23).SetValue(Convert.ToString(r[23])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                hoja.Cell(filas, 24).SetValue(Convert.ToString(r[24])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow);
                                break;
                            default:
                                hoja.Cell(filas, 1).SetValue(Convert.ToString(r[0])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 2).SetValue(Convert.ToString(r[1])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 3).SetValue(Convert.ToString(r[2])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 4).SetValue(Convert.ToString(r[3])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 5).SetValue(Convert.ToString(r[4])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 6).SetValue(Convert.ToString(r[5])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 7).SetValue(Convert.ToString(r[6])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 8).SetValue(Convert.ToString(r[7])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 9).SetValue(Convert.ToDecimal(r[8]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 10).SetValue(Convert.ToDecimal(r[9]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 11).SetValue(Convert.ToDecimal(r[10]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 12).SetValue(Convert.ToDecimal(r[12]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 13).SetValue(Convert.ToDecimal(r[13]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 14).SetValue(Convert.ToDecimal(r[14]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 15).SetValue(Convert.ToString(r[15])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 16).SetValue(Convert.ToString(r[16])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 17).SetValue(Convert.ToString(r[17])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 18).SetValue(Convert.ToString(r[18])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 19).SetValue(Convert.ToString(r[19])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 20).SetValue(Convert.ToString(r[20])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 21).SetValue(Convert.ToString(r[21])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 22).SetValue(Convert.ToDecimal(r[22]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 23).SetValue(Convert.ToString(r[23])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                hoja.Cell(filas, 24).SetValue(Convert.ToString(r[24])).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black);
                                break;
                        }
                    }
                    string ruta = HttpContext.Current.Server.MapPath("~/TMP");
                    string archivoGuardado = ruta + "\\ExportadoFacturas_" + Ini + "_" + Fin + ".xlsx";

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
                archivo = Convert.ToString(factu[1]);

        }
        catch (Exception ex) { archivo = ex.Message; }
    }
}
