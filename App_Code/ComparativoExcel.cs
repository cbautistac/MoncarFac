using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ComparativoExcel
/// </summary>
public class ComparativoExcel
{
    public string formatoColor { get; set; }
    public string empresa { get; set; }
    public string taller { get; set; }
    public string orden { get; set; }
    public int opcion { get; set; }
    public string archivo { get; set; }

    public ComparativoExcel()
    {
        empresa = taller = orden = archivo = "";
        opcion = 0;
    }
    public void generaArchivos()
    {
        switch (opcion)
        {
            case 1:
                //Mapfre
                generaMapfre();
                break;
            case 2:
                //generico
                generaComparativoGnerico();
                break;
            case 3:
                //Complemento mo
                generaComplementoMO();
                break;
            case 4:
                //Complemento ref
                generaComplementoREF();
                break;
            case 5:
                //Afirme
                generaAfime();
                break;
            default:
                break;
        }
    }

    private void generaAfime()
    {
        try
        {
            DataSet datos = new DataSet();
            Ejecuciones ejecuta = new Ejecuciones();
            Recepciones recepciones = new Recepciones();
            try
            {
                string sql = "select distinct d.id_cliprov,c.razon_social from cotizacion_detalle d inner join cliprov c on c.id_cliprov = d.id_cliprov and c.tipo = 'P' inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D')";
                object[] info = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(info[0]))
                    datos = (DataSet)info[1];
            }
            catch (Exception ex) { datos = new DataSet(); }
            string identificador = ejecuta.scalarToStringSimple("select identificador from talleres where id_taller=" + taller.ToString());
            int numProveedores = datos.Tables[0].Rows.Count;
            object[] infoProveedores = new object[numProveedores];
            string[] proveedorNom = new string[numProveedores];
            object[] columna = new object[numProveedores];
            object[] columnaB = new object[numProveedores];
            decimal[] totalProveedor = new decimal[numProveedores];
            int celdafinal = (numProveedores * 2) + 5;
            var libroExcel = new XLWorkbook();
            var hoja = libroExcel.Worksheets.Add("COMPARATIVO");
            hoja.SetShowGridLines(false);
            hoja.Column(1).Width = 15;
            hoja.Column(2).Width = 35;
            hoja.Column(3).Width = 10;
            hoja.Column(celdafinal).Width = 35;
            hoja.Column(celdafinal - 1).Width = 25;

            hoja.Range(1, 1, 1, 3).Merge().SetValue("SEGUROS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(15).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
            hoja.Range(2, 1, 4, 3).Merge().SetValue("AFIRME").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(48).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);

            hoja.Range(5, 1, 5, celdafinal).Merge().SetValue("COMPARATIVO DE REFACCIONES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(20).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
            hoja.Range(6, 1, 6, 2).Merge().SetValue("CONTROL DE TALLER").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
            hoja.Range(7, 1, 7, 2).Merge().SetValue(identificador.ToUpper() + orden.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);

            //datos encabezado
            object[] datosVehiculo = recepciones.obtieneDatosVehiculoExel(orden, empresa, taller);
            string[] vehiculo = new string[11];
            try
            {
                if (Convert.ToBoolean(datosVehiculo[0]))
                {
                    DataSet dataEnc = (DataSet)datosVehiculo[1];
                    foreach (DataRow fila in dataEnc.Tables[0].Rows)
                    {
                        hoja.Cell(10, 1).SetValue("Siniestro:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(10, 2).SetValue(fila[3]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(10, 4).SetValue("Marca:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(10, 5).SetValue(fila[0]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(10, 9).SetValue("Puertas:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(10, 10).SetValue(fila[9]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(10, 17).SetValue("Control de Taller:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(10, 18).SetValue(identificador.ToUpper() + orden.ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);

                        hoja.Cell(11, 1).SetValue("Reporte:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(11, 2).SetValue(fila[5]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(11, 4).SetValue("Tipo:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(11, 5).SetValue(fila[1]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(11, 9).SetValue("Transmisión:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(11, 10).SetValue(fila[10]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);

                        hoja.Cell(12, 1).SetValue("Asegurado:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(12, 2).SetValue(fila[11]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(12, 4).SetValue("Año:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(12, 5).SetValue(fila[6]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(12, 9).SetValue("Origen Vehículo:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(12, 10).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);

                        hoja.Cell(13, 1).SetValue("Póliza:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(13, 2).SetValue(fila[4]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(13, 4).SetValue("Placas:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(13, 5).SetValue(fila[8]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(13, 9).SetValue("Serie Vehículo:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(13, 10).SetValue(fila[2]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);

                        hoja.Cell(14, 1).SetValue("Riesgo:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(14, 2).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(14, 4).SetValue("Color:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(14, 5).SetValue(fila[7]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(14, 9).SetValue("Vidrios:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(14, 10).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);

                        hoja.Cell(15, 1).SetValue("Taller:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(15, 2).SetValue(identificador.ToUpper()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(15, 4).SetValue("Ajustador:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(15, 5).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(15, 9).SetValue("Clima:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                        hoja.Cell(15, 10).SetValue(fila[13]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
                    }
                }
                //fin datos encabezado

                hoja.Range(18, 4, 18, celdafinal).Merge().SetValue("Proveedores").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                hoja.Cell(19, 1).SetValue("CANTIDAD").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                hoja.Cell(19, 2).SetValue("REFACCIONES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                hoja.Cell(19, 3).SetValue("ORIGEN").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            }
            catch (Exception ex) { }
            int inicio = 4;
            int cont = 0;
            try
            {
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    //llena proveedores
                    hoja.Column(inicio).Width = 35;
                    hoja.Column(inicio + 1).Width = 10;
                    hoja.Column(inicio).Width = 20;
                    hoja.Range(19, inicio, 19, inicio + 1).Merge().SetValue(fila[1].ToString().ToUpper()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    proveedorNom[cont] = fila[1].ToString();
                    inicio += 2;
                    infoProveedores[cont] = fila[0];
                    columna[cont] = inicio - 2;
                    columnaB[cont] = Convert.ToInt32(columna[cont]) + 1;
                    cont++;
                }
                hoja.Cell(19, inicio).SetValue("COSTO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                hoja.Cell(19, inicio + 1).SetValue("PROVEEDOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            }
            catch (Exception ex) { }
            //llena refacciones y montos
            try
            {
                string sql = "select distinct d.id_cotizacion,d.id_cotizacion_detalle,d.cantidad,r.refDescripcion from Cotizacion_Detalle d inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D')";
                object[] info = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(info[0]))
                    datos = (DataSet)info[1];
            }
            catch (Exception ex) { datos = new DataSet(); }

            int filaDetalle = 20, refaccionRenglon = 0;
            decimal total = 0;
            int TotRefacciones = datos.Tables[0].Rows.Count;
            decimal[] montosMin = new decimal[TotRefacciones];
            string[] proveedorMin = new string[TotRefacciones];
            DataTable ProvsMontMin = new DataTable();
            try
            {
                ProvsMontMin.Columns.Add("Proveedor");
                ProvsMontMin.Columns.Add("Monto");
                foreach (DataRow infoRef in datos.Tables[0].Rows)
                {
                    DataRow rowProvs = ProvsMontMin.NewRow();
                    hoja.Cell(filaDetalle, 1).SetValue(infoRef[2].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Cell(filaDetalle, 2).SetValue(infoRef[3].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Cell(filaDetalle, 3).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    DataSet detalle = new DataSet();
                    decimal[] valores = new decimal[numProveedores];
                    try
                    {
                        string sql = "select d.id_cliprov,d.importe,case d.existencia when 1 then 'SI' else 'No' end as existencia,DATEADD(day, d.dias_entrega, d.fecha_captura)as fecha_entrega from Cotizacion_Detalle d inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D') and d.id_cotizacion_detalle=" + infoRef[1].ToString();
                        object[] info = ejecuta.dataSet(sql);
                        if (Convert.ToBoolean(info[0]))
                            detalle = (DataSet)info[1];
                    }
                    catch (Exception ex) { detalle = new DataSet(); }

                    int tamañoImporte = 0; try
                    {
                        foreach (DataRow configura in detalle.Tables[0].Rows)
                        {
                            for (int i = 0; i < columna.Length; i++)
                            {
                                if (configura[0].ToString() == infoProveedores[i].ToString())
                                {
                                    hoja.Cell(filaDetalle, Convert.ToInt32(columna[i])).SetValue(Convert.ToDecimal(configura[1]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    hoja.Cell(filaDetalle, Convert.ToInt32(columnaB[i])).SetValue(" ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                                    decimal monto = 0;
                                    try { monto = Convert.ToDecimal(configura[1]); } catch (Exception) { }
                                    totalProveedor[i] += Convert.ToDecimal(configura[1]);
                                    valores[i] += Convert.ToDecimal(configura[1]);
                                    if (valores[i] != 0)
                                        tamañoImporte++;
                                    break;
                                }
                            }
                            for (int k = 0; k < infoProveedores.Length; k++)
                            {
                                hoja.Cell(filaDetalle, Convert.ToInt32(columna[k])).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            }
                        }
                    }
                    catch (Exception ex) { }
                    decimal[] importes = new decimal[tamañoImporte];
                    decimal importesDec = 0;
                    int contador = 0, intProvMin = 0;
                    try
                    {
                        for (int iv = 0; iv < valores.Length; iv++)
                        {
                            if (valores[iv] != 0)
                            {
                                importes[contador] = valores[iv];
                                contador++;
                            }
                        }
                        for (intProvMin = 0; intProvMin < valores.Length; intProvMin++)
                        {
                            if (importes.Length == 0)
                                break;
                            else if (valores[intProvMin] == importes.Min())
                                break;
                        }
                        if (importes.Length > 0)
                            importesDec = importes.Min();

                    }
                    catch (Exception ex) { }
                    hoja.Cell(filaDetalle, inicio - 1).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Cell(filaDetalle, inicio).SetValue(importesDec.ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Cell(filaDetalle, inicio + 1).SetValue(proveedorNom[intProvMin]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    if (importes.Length == 0)
                    {
                        total += 0;
                        montosMin[refaccionRenglon] = 0;
                        proveedorMin[refaccionRenglon] = proveedorNom[intProvMin];
                        rowProvs[0] = proveedorNom[intProvMin];
                        rowProvs[1] = 0;
                    }
                    else
                    {
                        total = total + importes.Min();
                        montosMin[refaccionRenglon] = importes.Min();
                        proveedorMin[refaccionRenglon] = proveedorNom[intProvMin];
                        rowProvs[0] = proveedorNom[intProvMin];
                        rowProvs[1] = importes.Min();
                    }
                    filaDetalle++;
                    ProvsMontMin.Rows.Add(rowProvs);
                    refaccionRenglon++;
                }
            }
            catch (Exception ex) { }
            hoja.Column(inicio).Width = 15;
            hoja.Range(filaDetalle, 2, filaDetalle, 3).Merge().SetValue("TOTAL").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(filaDetalle, 3).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            try
            {
                for (int contaTotales = 0; contaTotales < columna.Length; contaTotales++)
                {
                    hoja.Cell(filaDetalle, Convert.ToInt32(columna[contaTotales])).SetValue(totalProveedor[contaTotales].ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Cell(filaDetalle, Convert.ToInt32(columnaB[contaTotales])).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                }
            }
            catch (Exception ex) { }
            hoja.Cell(filaDetalle, inicio).SetValue(total.ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            try
            {
                string sql = "select distinct d.id_cliprov,c.razon_social from cotizacion_detalle d inner join cliprov c on c.id_cliprov = d.id_cliprov and c.tipo = 'P' inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D')";
                object[] info = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(info[0]))
                    datos = (DataSet)info[1];
            }
            catch (Exception ex) { datos = new DataSet(); }
            inicio = 4;
            cont = 0;
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                //llena proveedores ultimos pie
                hoja.Column(inicio).Width = 20;
                hoja.Range(filaDetalle + 1, inicio, filaDetalle + 1, inicio + 1).Merge().SetValue(fila[1].ToString().ToUpper()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                inicio += 2;
                infoProveedores[cont] = fila[0];
                columna[cont] = inicio - 2;
                columnaB[cont] = Convert.ToInt32(columna[cont]) + 1;
                cont++;
            }
            //termina llenado refacciones y montos
            filaDetalle += 2;
            hoja.Range(filaDetalle, 1, filaDetalle, 2).Merge().SetValue("Complementarias").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
            filaDetalle++;
            hoja.Range(filaDetalle, celdafinal - 3, filaDetalle, celdafinal - 2).Merge().SetValue("SUBTOTAL").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Cell(filaDetalle, celdafinal - 1).SetValue(total.ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(filaDetalle, celdafinal).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            filaDetalle++;

            try
            {
                string sql = "select distinct d.id_cliprov,c.razon_social from cotizacion_detalle d inner join cliprov c on c.id_cliprov = d.id_cliprov and c.tipo = 'P' inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D')";
                object[] info = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(info[0]))
                    datos = (DataSet)info[1];
            }
            catch (Exception ex) { datos = new DataSet(); }
            for (int renglons = 0; renglons < 5; renglons++)
            {
                hoja.Column(inicio).Width = 20;
                hoja.Cell(filaDetalle, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                hoja.Cell(filaDetalle, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                hoja.Cell(filaDetalle, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                inicio = 4;
                cont = 0;
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    hoja.Cell(filaDetalle, inicio).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    hoja.Cell(filaDetalle, inicio + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(11).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    inicio += 2;
                }
                hoja.Cell(filaDetalle, celdafinal - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                hoja.Cell(filaDetalle, celdafinal).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightCyan).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                filaDetalle++;
            }
            filaDetalle++;
            hoja.Range(filaDetalle, 1, filaDetalle, 2).Merge().SetValue("Comentarios:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
            filaDetalle++;

            hoja.Range(filaDetalle, 1, filaDetalle, celdafinal).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetBottomBorder(XLBorderStyleValues.Medium);
            filaDetalle++;
            hoja.Range(filaDetalle, 1, filaDetalle, celdafinal).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetBottomBorder(XLBorderStyleValues.Medium);
            filaDetalle++;
            hoja.Range(filaDetalle, 1, filaDetalle, celdafinal).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetBottomBorder(XLBorderStyleValues.Medium);
            filaDetalle += 4;

            hoja.Range(filaDetalle, celdafinal - 1, filaDetalle, celdafinal).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetTopBorder(XLBorderStyleValues.Medium);
            filaDetalle++;
            hoja.Row(filaDetalle).Height = 25;
            hoja.Range(filaDetalle, celdafinal - 1, filaDetalle, celdafinal).Merge().SetValue("Alejandro Lorea Rodriguez" + Environment.NewLine + "Supervisor Agencia y/o Talleres").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor);

            filaDetalle += 13;
            hoja.Cell(filaDetalle, celdafinal - 2).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Cell(filaDetalle, celdafinal - 1).SetValue("COSTO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(filaDetalle, celdafinal).SetValue("PROVEEDOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGreen).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            filaDetalle++;
            decimal sumMontProv = 0, totalTablita = 0;
            string proveArrastre = proveedorMin[0];
            string[] minimosProvArr = new string[ProvsMontMin.Rows.Count];
            int contTablaProvs = 0;
            foreach (DataRow fila in ProvsMontMin.Rows)
            {
                sumMontProv = 0;
                int nuevo = 0;
                for (int renglons = 0; renglons < ProvsMontMin.Rows.Count; renglons++)
                {
                    if (Convert.ToString(fila[0]) == proveedorMin[renglons])
                    {
                        sumMontProv += Convert.ToDecimal(fila[1]);
                        proveArrastre = proveedorMin[renglons];
                        proveedorMin[renglons] = "";
                        nuevo = 1;
                    }
                }
                if (nuevo != 0)
                {
                    minimosProvArr[contTablaProvs] = proveArrastre + "&" + sumMontProv.ToString();
                    contTablaProvs++;
                }
            }
            minimosProvArr = minimosProvArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int renglons = 0; renglons < minimosProvArr.Length; renglons++)
            {
                string[] splitMinimosProvArr = minimosProvArr[renglons].Split('&');
                hoja.Cell(filaDetalle, celdafinal - 2).SetValue("PROV " + (renglons+1).ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.None);
                hoja.Cell(filaDetalle, celdafinal - 1).SetValue(Convert.ToDecimal(splitMinimosProvArr[1]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                hoja.Cell(filaDetalle, celdafinal).SetValue(splitMinimosProvArr[0]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                totalTablita += Convert.ToDecimal(splitMinimosProvArr[1]);
                filaDetalle++;
            }
            hoja.Cell(filaDetalle, celdafinal - 1).SetValue(totalTablita.ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(filaDetalle, celdafinal).SetValue("TOTAL").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.NoColor).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            string ruta = HttpContext.Current.Server.MapPath("~/files");
            string archivoGuardado = ruta + "\\" + "ComparativoRefaccionesAfirme_" + orden.ToString() + ".xlsx";
            //si no existe la carpeta temporal la creamos 
            if (!(Directory.Exists(ruta)))
                Directory.CreateDirectory(ruta);

            libroExcel.SaveAs(archivoGuardado);
            archivo = archivoGuardado;
        }
        catch (Exception ex) { archivo = ex.Message; }
    }

    private void generaComplementoREF()
    {
        try
        {
            var libroExcel = new XLWorkbook();
            var hoja = libroExcel.Worksheets.Add("Complemento Refacciones");
            hoja.SetShowGridLines(false);
            hoja.Column(1).Width = 6.89;
            hoja.Column(5).Width = 6.89;
            hoja.Column(2).Width = 13.89;
            hoja.Column(6).Width = 13.89;
            hoja.Column(3).Width = 30.11;
            hoja.Column(7).Width = 30.11;
            hoja.Column(4).Width = 12.89;
            hoja.Column(8).Width = 12.89;

            hoja.Range(2, 1, 2, 4).Merge().SetValue("COMPLEMENTO DE REFACCIONES:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(14).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(3, 1, 3, 2).Merge().SetValue("PROVEEDOR o AGENCIA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);

            hoja.Range(5, 7, 5, 8).Merge().SetValue("SINIESTRO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(6, 1, 6, 2).Merge().SetValue("POLIZA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(6, 3).SetValue("INCISO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(6, 4).SetValue("SINIESTRO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(6, 5, 6, 6).Merge().SetValue("FECHA VALUACION").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(8, 1, 8, 2).Merge().SetValue("MARCA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(8, 3).SetValue("MODELO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(8, 4, 8, 6).Merge().SetValue("TIPO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(8, 7).SetValue("PLACAS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(8, 8).SetValue("COLOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            Recepciones recepciones = new Recepciones();
            object[] datosVehiculo = recepciones.obtieneDatosVehiculoExel(orden, empresa, taller);
            string[] vehiculo = new string[11];
            if (Convert.ToBoolean(datosVehiculo[0]))
            {
                DataSet dataVehiculo = (DataSet)datosVehiculo[1];
                foreach (DataRow row in dataVehiculo.Tables[0].Rows)
                {
                    vehiculo[0] = row[0].ToString();//marca
                    vehiculo[1] = row[1].ToString();//tipo
                    vehiculo[2] = row[6].ToString();//modelo
                    vehiculo[3] = row[2].ToString();//serie
                    vehiculo[4] = row[8].ToString();//placas
                    vehiculo[5] = row[3].ToString();//siniestro
                    vehiculo[6] = row[11].ToString();//aseguradora
                    vehiculo[7] = row[7].ToString();//color
                    vehiculo[8] = row[4].ToString();//poliza
                    vehiculo[9] = row[14].ToString();//fecha valuacion
                    vehiculo[10] = row[2].ToString();//serie
                }
            }
            //siniestro
            hoja.Range(6, 7, 7, 8).Merge().SetValue(vehiculo[5]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //poliza
            hoja.Range(7, 1, 7, 2).Merge().SetValue(vehiculo[8]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //inciso
            hoja.Cell(7, 3).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //siniestro
            hoja.Cell(7, 4).SetValue(vehiculo[5]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //fecha valuacion
            hoja.Range(7, 5, 7, 6).Merge().SetValue(Convert.ToDateTime(vehiculo[9]).ToString("yyyy-MM-dd")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //marca
            hoja.Range(9, 1, 9, 2).Merge().SetValue(vehiculo[0]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //modelo
            hoja.Cell(9, 3).SetValue(vehiculo[2]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //tipo
            hoja.Range(9, 4, 9, 6).Merge().SetValue(vehiculo[1]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //placas
            hoja.Cell(9, 7).SetValue(vehiculo[4]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //color
            hoja.Cell(9, 8).SetValue(vehiculo[7]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            hoja.Range(10, 1, 10, 2).Merge().SetValue("SERIE").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(10, 3).SetValue("RIESGO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(10, 4, 10, 6).Merge().SetValue("VALUADOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(10, 7).SetValue("TALLER O AGENCIA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(10, 8).SetValue("AJUSTADOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            hoja.Range(11, 1, 11, 2).Merge().SetValue(vehiculo[10]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(11, 3).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(11, 4, 11, 6).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(11, 7).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(11, 8).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            hoja.Cell(13, 1).SetValue("CANTIDAD").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(13, 2, 13, 3).Merge().SetValue("DESCRIPCION").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(13, 4).SetValue("PRECIO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(13, 5).SetValue("CANTIDAD").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(13, 6, 13, 7).Merge().SetValue("DESCRIPCION").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(13, 8).SetValue("PRECIO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            int renglon = 14, renglonB = 14;

            object[] datosREF = recepciones.obtieneRefacciones(orden, empresa, taller);
            decimal monto = 0, total = 0;
            int renglones = 0, renglonFor = 0;
            if (Convert.ToBoolean(datosREF[0]))
            {
                DataSet refacciones = (DataSet)datosREF[1];
                renglones = refacciones.Tables[0].Rows.Count;
                if (renglones > 0)
                {
                    int renglonesMit = 39;
                    if (renglones > 50)
                        renglonesMit = renglones / 2 + 15;
                    foreach (DataRow row in refacciones.Tables[0].Rows)
                    {
                        try { monto = Convert.ToDecimal(row[2]); } catch (Exception) { monto = 0; }
                        if (renglonFor == 0)
                        {
                            hoja.Cell(renglon, 1).SetValue(row[0].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                            hoja.Range(renglon, 2, renglon, 3).Merge().SetValue(row[1].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                            hoja.Cell(renglon, 4).SetValue("$ " + monto.ToString("F2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                            if (renglon == renglonesMit && renglones < 50)
                            {
                                renglonFor = 1;
                                renglon--;
                            }
                            else if (renglon == 35 && renglones > 51)
                            {
                                renglonFor = 1;
                                renglon--;
                            }
                            renglon++;
                        }
                        else
                        {
                            hoja.Cell(renglonB, 5).SetValue(row[0].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                            hoja.Range(renglonB, 6, renglon, 7).Merge().SetValue(row[1].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                            hoja.Cell(renglonB, 8).SetValue("$ " + monto.ToString("F2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                            renglonB++;
                        }
                        total += monto;
                    }
                    if (renglon > renglonB)
                    {
                        int intRenglonB = renglonB;
                        int intRenglon = renglon;
                        for (renglonB = intRenglonB; renglonB < intRenglon; renglonB++)
                        {
                            hoja.Cell(renglonB, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                            hoja.Range(renglonB, 6, renglon, 7).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                            hoja.Cell(renglonB, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                        }
                        hoja.Cell(renglon - 1, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                        hoja.Range(renglon - 1, 6, renglon - 1, 7).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                        hoja.Cell(renglon - 1, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    }
                }
                else
                {
                    hoja.Cell(renglon, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    hoja.Range(renglon, 2, renglon, 3).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    hoja.Cell(renglon, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    hoja.Cell(renglon, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    hoja.Range(renglon, 6, renglon, 7).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    hoja.Cell(renglon, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    renglon++;
                }
            }
            hoja.Range(renglon, 1, renglon, 6).Merge().SetValue("TOTAL AUTORIZADO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(14).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 7, renglon, 8).Merge().SetValue("$ " + total.ToString("F2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(14).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            renglon += 2;
            hoja.Cell(renglon, 1).SetValue("NOTA:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 2, renglon, 8).Merge().SetValue("EN CASO DE CAMBIO DE PROVEEDOR NOTIFICAR AL DEPARTAMENTO DE COTIZACION ANTES DE FACTURAR.").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Cell(renglon, 1).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 2, renglon, 8).Merge().SetValue("LOS PRECIOS ASENTADOS EN EL VALE DEBEN DE COINCIDIR CON LA FACTURA.").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.Yellow).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon += 7;
            hoja.Range(renglon, 1, renglon, 2).Merge().SetValue("____________________________").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 3, renglon, 5).Merge().SetValue("___________________________________").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 6, renglon, 8).Merge().SetValue("______________________________________").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 1, renglon, 2).Merge().SetValue("SELLO DE RECIBIDO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 3, renglon, 5).Merge().SetValue("FIRMA Y SELLO DEL VALUADOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 6, renglon, 8).Merge().SetValue("FIRMA Y SELLO DEL COTIZADOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon += 3;
            hoja.Range(renglon, 1, renglon, 11).Merge().SetValue("Centro de Valuacion Aeropuerto I ASISTENCIA PUBLICA 164 Col. FEDERAL Del. VENUSTIANO CARRANZA D.F.15700        TEL (5) 785-90-35   FAX: (5) 785-00-66    ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(7).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 1, renglon, 11).Merge().SetValue("Centro de Valuacion Aeropuerto II BOULEVARD PUERTO AEREO # 151 Col. SANTA CRUZ AVIACION Del. VENUSTIANO CARRANZA TEL. (5) 133-35-60 FAX (5) 786-00-66").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(7).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 1, renglon, 11).Merge().SetValue("Centro de Valuacion Sur  AV. DIVISION DEL NORTE # 3054 Col. ATLANTIDA. Del. COYOACAN TEL (5) 484-06.80 FAX (5) 322-80-72").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(7).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 1, renglon, 11).Merge().SetValue("Centro de Valuacion Norte   Av. GUSTAVO BAZ 180 Col. LA ESCUELA, TLALNEPANTLA Edo Mex.  D.F.54090 TEL. Y FAX  (5) 365-00-84    ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(7).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            /*hoja.Cell(3, 1).SetValue("POLIZA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Double);
            hoja.Cell(3, 1).SetValue("POLIZA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Double);
            */

            string ruta = HttpContext.Current.Server.MapPath("~/files");
            string archivoGuardado = ruta + "\\" + "ComplementoRefacciones_" + orden.ToString() + ".xlsx";
            //si no existe la carpeta temporal la creamos 
            if (!(Directory.Exists(ruta)))
                Directory.CreateDirectory(ruta);

            libroExcel.SaveAs(archivoGuardado);
            archivo = archivoGuardado;
        }
        catch (Exception ex) { archivo = ex.Message; }
    }

    private void generaComplementoMO()
    {
        try
        {
            var libroExcel = new XLWorkbook();
            var hoja = libroExcel.Worksheets.Add("Complemento Mano de Obra");
            hoja.SetShowGridLines(false);
            hoja.Column(1).Width = 18.78;
            hoja.Column(2).Width = 2.67;
            hoja.Column(3).Width = 2.67;
            hoja.Column(4).Width = 2.67;
            hoja.Column(5).Width = 8.33;
            hoja.Column(6).Width = 2.67;
            hoja.Column(7).Width = 2.67;
            hoja.Column(8).Width = 8.33;
            hoja.Column(9).Width = 8.33;
            hoja.Column(10).Width = 23.67;
            hoja.Column(11).Width = 23.67;
            //hoja.Row(1).Height = 25;

            hoja.Range(2, 1, 2, 8).Merge().SetValue("COMPLEMENTO DE MANO DE OBRA:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(14).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Cell(3, 1).SetValue("AGENCIA O TALLER").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);

            hoja.Range(5, 10, 5, 11).Merge().SetValue("SINIESTRO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(6, 1).SetValue("POLIZA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(6, 2, 6, 4).Merge().SetValue("INCISO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(6, 5, 6, 7).Merge().SetValue("SINIESTRO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(6, 8, 6, 9).Merge().SetValue("FECHA VALUACION").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(8, 1, 8, 2).Merge().SetValue("MARCA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(8, 3, 8, 5).Merge().SetValue("MODELO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(8, 6, 8, 9).Merge().SetValue("TIPO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(8, 10).SetValue("PLACAS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(8, 11).SetValue("COLOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            Recepciones recepciones = new Recepciones();
            object[] datosVehiculo = recepciones.obtieneDatosVehiculoExel(orden, empresa, taller);
            string[] vehiculo = new string[11];
            if (Convert.ToBoolean(datosVehiculo[0]))
            {
                DataSet dataVehiculo = (DataSet)datosVehiculo[1];
                foreach (DataRow row in dataVehiculo.Tables[0].Rows)
                {
                    vehiculo[0] = row[0].ToString();//marca
                    vehiculo[1] = row[1].ToString();//tipo
                    vehiculo[2] = row[6].ToString();//modelo
                    vehiculo[3] = row[2].ToString();//serie
                    vehiculo[4] = row[8].ToString();//placas
                    vehiculo[5] = row[3].ToString();//siniestro
                    vehiculo[6] = row[11].ToString();//aseguradora
                    vehiculo[7] = row[7].ToString();//color
                    vehiculo[8] = row[4].ToString();//poliza
                    vehiculo[9] = row[14].ToString();//fecha valuacion
                    vehiculo[10] = row[2].ToString();//serie
                }
            }
            hoja.Range(6, 10, 7, 11).Merge().SetValue(vehiculo[5]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(7, 1).SetValue(vehiculo[8]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(7, 2, 7, 4).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(7, 5, 7, 7).Merge().SetValue(vehiculo[5]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(7, 8, 7, 9).Merge().SetValue(Convert.ToDateTime(vehiculo[9]).ToString("yyyy-MM-dd")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(9, 1, 9, 2).Merge().SetValue(vehiculo[0]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(9, 3, 9, 5).Merge().SetValue(vehiculo[2]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(9, 6, 9, 9).Merge().SetValue(vehiculo[1]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(9, 10).SetValue(vehiculo[4]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(9, 11).SetValue(vehiculo[7]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            hoja.Range(10, 1, 10, 3).Merge().SetValue("SERIE").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(10, 4, 10, 6).Merge().SetValue("RIESGO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(10, 7, 10, 9).Merge().SetValue("VALUADOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(10, 10).SetValue("FECHA DE COMPLEMENTO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(10, 11).SetValue("DIAS TRANSCURRIDOS").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            hoja.Range(11, 1, 11, 3).Merge().SetValue(vehiculo[10]).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(11, 4, 11, 6).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Range(11, 7, 11, 9).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(11, 10).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(11, 11).SetValue("0").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.GreenYellow).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            hoja.Range(13, 1, 13, 10).Merge().SetValue("TRABAJOS A REALIZAR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(13, 11).SetValue("$ MONTO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            int renglon = 14;

            object[] datosMO = recepciones.obtieneManoObra(orden, empresa, taller);
            decimal monto = 0, total = 0;
            if (Convert.ToBoolean(datosMO[0]))
            {
                DataSet manoObra = (DataSet)datosMO[1];
                foreach (DataRow row in manoObra.Tables[0].Rows)
                {
                    try { monto = Convert.ToDecimal(row[7]); } catch (Exception) { monto = 0; }
                    hoja.Range(renglon, 1, renglon, 10).Merge().SetValue(row[8].ToString() + " " + row[9].ToString() + " " + row[6].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    hoja.Cell(renglon, 11).SetValue("$ " + monto.ToString("F2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
                    total += monto;
                    renglon++;
                }
            }

            hoja.Range(renglon, 1, renglon, 10).Merge().SetValue("TOTAL AUTORIZADO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(14).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Cell(renglon, 11).SetValue("$ " + total.ToString("F2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(14).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            renglon += 7;
            hoja.Range(renglon, 1, renglon, 3).Merge().SetValue("____________________________").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 5, renglon, 9).Merge().SetValue("___________________________________").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 10, renglon, 11).Merge().SetValue("______________________________________").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Bottom).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 1, renglon, 3).Merge().SetValue("SELLO CAPTURA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 5, renglon, 9).Merge().SetValue("SUPERVISOR DE VALUACION").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(renglon, 10, renglon, 11).Merge().SetValue("FIRMA Y SELLO DEL VALUADOR").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 5, renglon, 9).Merge().SetValue("GINA AYALA // OSCAR RUIZ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon += 3;
            hoja.Range(renglon, 1, renglon, 11).Merge().SetValue("Centro de Valuacion Aeropuerto I ASISTENCIA PUBLICA 164 Col. FEDERAL Del. VENUSTIANO CARRANZA D.F.15700        TEL (5) 785-90-35   FAX: (5) 785-00-66    ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(7).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 1, renglon, 11).Merge().SetValue("Centro de Valuacion Aeropuerto II BOULEVARD PUERTO AEREO # 151 Col. SANTA CRUZ AVIACION Del. VENUSTIANO CARRANZA TEL. (5) 133-35-60 FAX (5) 786-00-66").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(7).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 1, renglon, 11).Merge().SetValue("Centro de Valuacion Sur  AV. DIVISION DEL NORTE # 3054 Col. ATLANTIDA. Del. COYOACAN TEL (5) 484-06.80 FAX (5) 322-80-72").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(7).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            renglon++;
            hoja.Range(renglon, 1, renglon, 11).Merge().SetValue("Centro de Valuacion Norte   Av. GUSTAVO BAZ 180 Col. LA ESCUELA, TLALNEPANTLA Edo Mex.  D.F.54090 TEL. Y FAX  (5) 365-00-84    ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(7).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            /*hoja.Cell(3, 1).SetValue("POLIZA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Double);
            hoja.Cell(3, 1).SetValue("POLIZA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Double);
            */

            string ruta = HttpContext.Current.Server.MapPath("~/files");
            string archivoGuardado = ruta + "\\" + "ComplementoManoObra_" + orden.ToString() + ".xlsx";
            //si no existe la carpeta temporal la creamos 
            if (!(Directory.Exists(ruta)))
                Directory.CreateDirectory(ruta);

            libroExcel.SaveAs(archivoGuardado);
            archivo = archivoGuardado;
        }
        catch (Exception ex) { archivo = ex.Message; }
    }

    private void generaComparativoGnerico()
    {
        try
        {

            var libroExcel = new XLWorkbook();
            var hoja = libroExcel.Worksheets.Add("Comparativo");
            DataSet datos = new DataSet();
            Ejecuciones ejecuta = new Ejecuciones();
            Recepciones recepciones = new Recepciones();
            try
            {
                string sql = "select distinct d.id_cliprov,c.razon_social from cotizacion_detalle d inner join cliprov c on c.id_cliprov = d.id_cliprov and c.tipo = 'P' inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D')";
                object[] info = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(info[0]))
                    datos = (DataSet)info[1];
            }
            catch (Exception ex) { datos = new DataSet(); }

            int numProveedores = datos.Tables[0].Rows.Count;
            object[] infoProveedores = new object[numProveedores];
            object[] columna = new object[numProveedores];
            int celdafinal = numProveedores + 3;
            XLColor azulFuerte = XLColor.FromArgb(255, 255, 255);
            string nomDocExc = "Genrico";
            if (formatoColor != "")
            {

                if (formatoColor == "M")
                {
                    azulFuerte = XLColor.FromArgb(0, 32, 96);//multiva
                    nomDocExc = "Multiva";
                }
                else if (formatoColor == "A")
                {
                    azulFuerte = XLColor.FromArgb(245, 134, 44);//atlas
                    nomDocExc = "Atlas";
                }
                else if (formatoColor == "H")
                {
                    azulFuerte = XLColor.FromArgb(0, 129, 71);//hdi
                    nomDocExc = "HDI";
                }
            }

            hoja.Column(2).Width = 43.33;
            hoja.Column(1).Width = 12.33;
            hoja.Row(1).Height = 25;
            hoja.Row(13).Height = 25;
            hoja.Row(14).Height = 25;

            hoja.Range(1, 1, 1, celdafinal).Merge().SetValue("TALLER MONCAR AZTAHUACAN").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(14).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(azulFuerte).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(2, 1, 2, celdafinal).Merge().SetValue("PRESUPUESTO REFACCIONES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Cell(3, 2).SetValue("Calle Hidalgo No. 171 Mz- 3B ").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(9).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Range(3, 3, 3, 6).Merge().SetValue("Telefonos: 5690 1128, 5693 5996").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(9).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);

            object[] datosVehiculo = recepciones.obtieneDatosVehiculoExel(orden, empresa, taller);
            string[] vehiculo = new string[7];
            if (Convert.ToBoolean(datosVehiculo[0]))
            {
                DataSet dataVehiculo = (DataSet)datosVehiculo[1];
                foreach (DataRow row in dataVehiculo.Tables[0].Rows)
                {
                    vehiculo[0] = row[1].ToString();//marca
                    vehiculo[1] = row[0].ToString();//tipo
                    vehiculo[2] = row[6].ToString();//modelo
                    vehiculo[3] = row[2].ToString();//serie
                    vehiculo[4] = row[8].ToString();//placas
                    vehiculo[5] = row[3].ToString();//siniestro
                    vehiculo[6] = row[11].ToString();//aseguradora
                }
            }

            hoja.Cell(4, 1).SetValue("MARCA:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(5, 1).SetValue("TIPO:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(6, 1).SetValue("MODELO:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(7, 1).SetValue("SERIE:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(8, 1).SetValue("PLACAS:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(9, 1).SetValue("SINIESTRO:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(10, 1).SetValue("ASEGURADORA:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            hoja.Cell(4, 2).SetValue(vehiculo[0].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(5, 2).SetValue(vehiculo[1].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(6, 2).SetValue(vehiculo[2].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(7, 2).SetValue(vehiculo[3].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(8, 2).SetValue(vehiculo[4].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(9, 2).SetValue(vehiculo[5].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            hoja.Cell(10, 2).SetValue(vehiculo[6].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            hoja.Range(11, celdafinal - 2, 13, celdafinal).Merge().SetValue(orden).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Ahorini").Font.SetFontSize(54).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Cell(14, 1).SetValue("Cantidad").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(azulFuerte).Border.SetOutsideBorder(XLBorderStyleValues.None);
            hoja.Cell(14, 2).SetValue("Refacciones").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(azulFuerte).Border.SetOutsideBorder(XLBorderStyleValues.None);

            int inicio = 3;
            int cont = 0;
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                //llena proveedores
                hoja.Column(inicio).Width = 20;
                hoja.Cell(14, inicio).SetValue(fila[1].ToString().ToUpper()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(azulFuerte).Border.SetOutsideBorder(XLBorderStyleValues.None);
                inicio++;
                infoProveedores[cont] = fila[0];
                columna[cont] = inicio - 1;
                cont++;
            }
            hoja.Cell(14, inicio).SetValue("COSTO" + Environment.NewLine + "MAS BAJO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(azulFuerte).Border.SetOutsideBorder(XLBorderStyleValues.None);
            try
            {
                string sql = "select distinct d.id_cotizacion_detalle,d.cantidad,r.refDescripcion from Cotizacion_Detalle d inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D')";
                object[] info = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(info[0]))
                    datos = (DataSet)info[1];
            }
            catch (Exception ex) { datos = new DataSet(); }

            int filaDetalle = 15;
            decimal total = 0;
            foreach (DataRow infoRef in datos.Tables[0].Rows)
            {
                hoja.Cell(filaDetalle, 1).SetValue(infoRef[1].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                hoja.Cell(filaDetalle, 2).SetValue(infoRef[2].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                DataSet detalle = new DataSet();
                decimal[] valores = new decimal[numProveedores];
                try
                {
                    string sql = "select d.id_cliprov,d.importe,case d.existencia when 1 then 'SI' else 'No' end asexistencia,DATEADD(day, d.dias_entrega, d.fecha_captura)as fecha_entrega from Cotizacion_Detalle d inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D') and d.id_cotizacion_detalle=" + infoRef[0].ToString();
                    object[] info = ejecuta.dataSet(sql);
                    if (Convert.ToBoolean(info[0]))
                        detalle = (DataSet)info[1];
                }
                catch (Exception ex) { detalle = new DataSet(); }

                int tamañoImporte = 0;
                foreach (DataRow configura in detalle.Tables[0].Rows)
                {
                    for (int i = 0; i < columna.Length; i++)
                    {
                        if (configura[0].ToString() == infoProveedores[i].ToString())
                        {
                            hoja.Cell(filaDetalle, Convert.ToInt32(columna[i])).SetValue(Convert.ToDecimal(configura[1]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            valores[i] = Convert.ToDecimal(configura[1]);
                            if (valores[i] != 0)
                                tamañoImporte++;
                            break;
                        }
                    }
                    for (int k = 0; k < infoProveedores.Length; k++)
                    {
                        hoja.Cell(filaDetalle, Convert.ToInt32(columna[k])).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    }
                }

                decimal[] importes = new decimal[tamañoImporte];

                int contador = 0;
                for (int iv = 0; iv < valores.Length; iv++)
                {
                    if (valores[iv] != 0)
                    {
                        importes[contador] = valores[iv];
                        contador++;
                    }
                }

                hoja.Cell(filaDetalle, inicio - 1).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                hoja.Cell(filaDetalle, inicio).SetValue(importes.Min().ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                total = total + importes.Min();
                filaDetalle++;
            }
            hoja.Column(inicio).Width = 15;
            hoja.Cell(filaDetalle, inicio).SetValue(total.ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            string ruta = HttpContext.Current.Server.MapPath("~/files");
            string archivoGuardado = ruta + "\\" + nomDocExc + "_" + orden.ToString() + ".xlsx";
            //si no existe la carpeta temporal la creamos 
            if (!(Directory.Exists(ruta)))
                Directory.CreateDirectory(ruta);

            libroExcel.SaveAs(archivoGuardado);
            archivo = archivoGuardado;
        }
        catch (Exception ex) { archivo = ex.Message; }
    }

    private void generaMapfre()
    {
        try
        {
            DataSet datos = new DataSet();
            Ejecuciones ejecuta = new Ejecuciones();
            Recepciones recepciones = new Recepciones();
            try
            {
                string sql = "select distinct d.id_cliprov,c.razon_social from cotizacion_detalle d inner join cliprov c on c.id_cliprov = d.id_cliprov and c.tipo = 'P' inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D')";
                object[] info = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(info[0]))
                    datos = (DataSet)info[1];
            }
            catch (Exception ex) { datos = new DataSet(); }

            int numProveedores = datos.Tables[0].Rows.Count;
            object[] infoProveedores = new object[numProveedores];
            object[] columna = new object[numProveedores];
            int celdafinal = (numProveedores * 5) + 4;

            var libroExcel = new XLWorkbook();
            var hoja = libroExcel.Worksheets.Add("Comparativo Mapfre");

            hoja.Range(1, 1, 2, celdafinal).Merge().SetValue("TALLER: MONCAR AZTAHUACAN").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(14).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red);
            hoja.Range(3, 1, 3, celdafinal).Merge().SetValue("PRESUPUESTO DE REFACCIONES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);
            hoja.Range(4, 2, 4, celdafinal).Merge().SetValue("Calle Hidalgo No. 171 Mz-3B").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(9).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White);

            hoja.Range(5, 1, 5, 4).Merge().SetValue("MAPFRE TEPEYAC SA").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(5, 5).SetValue("MARCA:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(5, 10).SetValue("AUT,STD,TRIPTONIC:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            hoja.Cell(6, 1).SetValue("PLAZA:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Range(6, 2, 6, 4).Merge().SetValue("Distrito Federal").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(6, 5).SetValue("TIPO:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(6, 10).SetValue("ELEC. O MANUAL:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Range(6, 11, 6, celdafinal).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            hoja.Cell(7, 1).SetValue("POLIZA:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(7, 5).SetValue("SUB TIPO:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Range(7, 6, 7, 9).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(7, 10).SetValue("2 O 4 PTAS:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            hoja.Cell(8, 1).SetValue("SINIESTRO:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(8, 5).SetValue("AÑO:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(8, 10).SetValue("A/AC:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            hoja.Cell(9, 1).SetValue("ASEG-TERC:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(9, 5).SetValue("COLOR:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(9, 10).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Range(9, 11, 9, celdafinal).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            hoja.Cell(10, 1).SetValue("SERIE:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(10, 5).SetValue("PINTURA:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Range(10, 6, 10, 9).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(10, 10).SetValue("FECHA INGRESO:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            hoja.Cell(11, 1).SetValue("PLACAS:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(11, 5).SetValue("CILINDROS:").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Cell(11, 10).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Range(11, 11, 11, celdafinal).Merge().SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            object[] infoOrd = recepciones.obtieneDatosVehiculoExel(orden, empresa, taller);
            if (Convert.ToBoolean(infoOrd[0]))
            {
                DataSet infoOrden = (DataSet)infoOrd[1];
                foreach (DataRow filas in infoOrden.Tables[0].Rows)
                {
                    hoja.Range(5, 6, 5, 9).Merge().SetValue(filas[0].ToString().ToUpper().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(5, 11, 5, celdafinal).Merge().SetValue(filas[10].ToString().ToUpper().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(6, 6, 6, 9).Merge().SetValue(filas[1].ToString().ToUpper().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(7, 2, 7, 4).Merge().SetValue(filas[4].ToString().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(7, 11, 7, celdafinal).Merge().SetValue(filas[9].ToString().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(8, 2, 8, 4).Merge().SetValue(filas[3].ToString().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(8, 6, 8, 9).Merge().SetValue(filas[6].ToString().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    string acc = "NO";
                    if (filas[13].ToString().Trim() == "0" || filas[13].ToString().Trim().ToUpper() == "FALSE")
                        acc = "NO";
                    else
                        acc = "SI";
                    hoja.Range(8, 11, 8, celdafinal).Merge().SetValue(acc).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(9, 2, 9, 4).Merge().SetValue(filas[11].ToString().Trim().ToUpper()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(9, 6, 9, 9).Merge().SetValue(filas[7].ToString().Trim().ToUpper()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(10, 2, 10, 4).Merge().SetValue(filas[2].ToString().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(10, 11, 10, celdafinal).Merge().SetValue(Convert.ToDateTime(filas[15]).ToShortDateString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(11, 2, 11, 4).Merge().SetValue(filas[8].ToString().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    hoja.Range(11, 6, 11, 9).Merge().SetValue(filas[12].ToString().Trim()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(8).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                }
            }

            hoja.Range(12, 1, 14, celdafinal).Merge().SetValue(orden).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Ahorini").Font.SetFontSize(54).Font.SetBold(true).Font.SetFontColor(XLColor.Red).Fill.SetBackgroundColor(XLColor.White);

            hoja.Range(15, 1, 16, celdafinal).Merge().SetValue("PROVEEDORES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            hoja.Range(17, 1, 18, 1).Merge().SetValue("CANT.").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Range(17, 2, 18, 2).Merge().SetValue("REFACCIONES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            /* PROVEEDORES */
            hoja.Column(1).Width = 12.14;
            hoja.Column(2).Width = 50;
            int alternado = 0;
            int inicio = 3;
            int inicioAlter = 4;
            int inicioExist = 5;
            int inicioFecha = 6;
            int inicioComent = 7;
            int cont = 0;

            if (numProveedores != 0)
            {
                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    hoja.Column(inicio).Width = 10;
                    hoja.Column(inicioAlter).Width = 15;
                    hoja.Column(inicioExist).Width = 10;
                    hoja.Column(inicioFecha).Width = 10;
                    hoja.Column(inicioComent).Width = 20;
                    if (alternado == 0)
                    {
                        hoja.Range(17, inicio, 18, inicio).Merge().SetValue("Origen" + Environment.NewLine + "O,TW,H,U").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Cell(17, inicioAlter).SetValue(fila[1].ToString().ToUpper()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Cell(18, inicioAlter).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Range(17, inicioExist, 18, inicioExist).Merge().SetValue("Existencia").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Range(17, inicioFecha, 18, inicioFecha).Merge().SetValue("Fecha" + Environment.NewLine + "Entrega").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Range(17, inicioComent, 18, inicioComent).Merge().SetValue("Comentario").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        alternado = 1;
                    }
                    else
                    {
                        hoja.Range(17, inicio, 18, inicio).Merge().SetValue("Origen" + Environment.NewLine + "O,TW,H,U").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Cell(17, inicioAlter).SetValue(fila[1].ToString().ToUpper()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Cell(18, inicioAlter).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Range(17, inicioExist, 18, inicioExist).Merge().SetValue("Existencia").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Range(17, inicioFecha, 18, inicioFecha).Merge().SetValue("Fecha" + Environment.NewLine + "Entrega").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Range(17, inicioComent, 18, inicioComent).Merge().SetValue("Comentario").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        alternado = 0;
                    }
                    inicio += 5;
                    inicioAlter += 5;
                    inicioExist += 5;
                    inicioFecha += 5;
                    inicioComent += 5;
                    infoProveedores[cont] = fila[0];
                    columna[cont] = inicio - 5;
                    cont++;
                }
            }
            else
            {
                inicio = 3;
                inicioAlter = 4;
                inicioExist = 5;
                inicioFecha = 6;
                inicioComent = 7;
            }

            hoja.Column(inicio).Width = 20;
            hoja.Column(inicioAlter).Width = 15;
            hoja.Range(17, inicio, 18, inicio).Merge().SetValue("COMENTARIOS" + Environment.NewLine + "GENERALES").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Red).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            hoja.Range(17, inicioAlter, 18, inicioAlter).Merge().SetValue("COSTO" + Environment.NewLine + "MAS BAJO").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.Black).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            // Detalle
            datos = new DataSet();

            try
            {
                string sql = "select distinct d.id_cotizacion,d.id_cotizacion_detalle,d.cantidad,r.refDescripcion from Cotizacion_Detalle d inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D')";
                object[] info = ejecuta.dataSet(sql);
                if (Convert.ToBoolean(info[0]))
                    datos = (DataSet)info[1];
            }
            catch (Exception ex) { datos = new DataSet(); }

            int filaDetalle = 19;
            decimal total = 0;
            foreach (DataRow infoRef in datos.Tables[0].Rows)
            {
                hoja.Cell(filaDetalle, 1).SetValue(infoRef[2].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                hoja.Cell(filaDetalle, 2).SetValue(infoRef[3].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(false).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                DataSet detalle = new DataSet();
                decimal[] valores = new decimal[numProveedores];
                try
                {
                    string sql = "select d.id_cliprov,d.importe,case d.existencia when 1 then 'SI' else 'No' end asexistencia,DATEADD(day, d.dias_entrega, d.fecha_captura)as fecha_entrega from Cotizacion_Detalle d inner join Refacciones_Orden r on r.ref_id_empresa = d.id_empresa and r.ref_id_taller = d.id_taller and r.ref_no_orden = d.no_orden and r.refOrd_Id = d.id_cotizacion_detalle and r.refEstatusSolicitud not in(10) and r.refestatus not in('CA') where d.no_orden = " + orden + " and d.id_taller = " + taller + " and d.id_empresa = " + empresa + " and d.estatus_proveedor not in('D') and d.id_cotizacion_detalle=" + infoRef[1].ToString();
                    object[] info = ejecuta.dataSet(sql);
                    if (Convert.ToBoolean(info[0]))
                        detalle = (DataSet)info[1];
                }
                catch (Exception ex) { detalle = new DataSet(); }

                int tamañoImporte = 0;
                foreach (DataRow configura in detalle.Tables[0].Rows)
                {
                    for (int i = 0; i < columna.Length; i++)
                    {
                        if (configura[0].ToString() == infoProveedores[i].ToString())
                        {
                            hoja.Cell(filaDetalle, Convert.ToInt32(columna[i])).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            hoja.Cell(filaDetalle, Convert.ToInt32(columna[i]) + 1).SetValue(Convert.ToDecimal(configura[1]).ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            hoja.Cell(filaDetalle, Convert.ToInt32(columna[i]) + 2).SetValue(configura[2].ToString()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            hoja.Cell(filaDetalle, Convert.ToInt32(columna[i]) + 3).SetValue(Convert.ToDateTime(configura[3]).ToString("yyy-MM-dd")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            hoja.Cell(filaDetalle, Convert.ToInt32(columna[i]) + 4).SetValue("").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                            valores[i] = Convert.ToDecimal(configura[1]);
                            if (valores[i] != 0)
                                tamañoImporte++;
                            break;
                        }
                    }
                    for (int k = 0; k < infoProveedores.Length; k++)
                    {
                        hoja.Cell(filaDetalle, Convert.ToInt32(columna[k])).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Cell(filaDetalle, Convert.ToInt32(columna[k]) + 1).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Cell(filaDetalle, Convert.ToInt32(columna[k]) + 2).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Cell(filaDetalle, Convert.ToInt32(columna[k]) + 3).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                        hoja.Cell(filaDetalle, Convert.ToInt32(columna[k]) + 4).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    }
                }

                decimal[] importes = new decimal[tamañoImporte];

                int contador = 0;
                for (int iv = 0; iv < valores.Length; iv++)
                {
                    if (valores[iv] != 0)
                    {
                        importes[contador] = valores[iv];
                        contador++;
                    }
                }

                hoja.Cell(filaDetalle, inicioAlter - 1).Style.Fill.SetBackgroundColor(XLColor.White).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                hoja.Cell(filaDetalle, inicioAlter).SetValue(importes.Min().ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(10).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                total = total + importes.Min();
                filaDetalle++;
            }

            hoja.Cell(filaDetalle, inicioAlter).SetValue(total.ToString("C2")).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right).Alignment.SetVertical(XLAlignmentVerticalValues.Center).Font.SetFontName("Arial").Font.SetFontSize(12).Font.SetBold(true).Font.SetFontColor(XLColor.Black).Fill.SetBackgroundColor(XLColor.LightGray).Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            string ruta = HttpContext.Current.Server.MapPath("~/files");
            string archivoGuardado = ruta + "\\" + "ComparativoMapfre_" + orden.ToString() + ".xlsx";
            //si no existe la carpeta temporal la creamos 
            if (!(Directory.Exists(ruta)))
                Directory.CreateDirectory(ruta);

            libroExcel.SaveAs(archivoGuardado);
            archivo = archivoGuardado;
        }
        catch (Exception ex) { archivo = ex.Message; }
    }
}