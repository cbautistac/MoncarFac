using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
/// <summary>
/// Descripción breve de ReporFactuPA
/// </summary>
public class ReporFactuPA
{
    Ejecuciones ejecuta = new Ejecuciones();
    BaseDatos ejecutaa = new BaseDatos();
    public int usuario { get; set; }
    public int empresa { get; set; }
    public int taller { get; set; }
    public object[] retorno { get; set; }
    private string sql;
    public ReporFactuPA()
    {
        usuario = empresa = taller = 0;
        retorno = new object[2] { false, "" };
        sql = string.Empty;
    }
    internal void obtieneEnc(int id)
    {
        // Obtiene Encabezado
        DataTable dt = new DataTable();
        dt.Columns.Add("id");//0
        dt.Columns.Add("Cliente/Proveedor"); //1
        dt.Columns.Add("Facturas Pendientes"); //2
        dt.Columns.Add("Monto Pendiente"); //3
        dt.Columns.Add("Facturas Pagadas"); //4
        dt.Columns.Add("Monto Pagadas"); //5
        dt.Columns.Add("Facturas Programadas"); //6
        dt.Columns.Add("Monto Programadas"); //7
        dt.Columns.Add("Facturas Canceladas"); //8
        dt.Columns.Add("Monto Canceladas"); //9
        dt.Columns.Add("Facturas Vencidas"); //10
        dt.Columns.Add("Monto Vencidas"); //11

        obtieneFacturas(id);
        if (Convert.ToBoolean(retorno[0]))
        {
            DataSet datosFacturas = (DataSet)retorno[1];
            if (datosFacturas.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow f in datosFacturas.Tables[0].Rows)
                {
                    DataRow row = dt.NewRow();
                    row[0] = Convert.ToString(f[0]);
                    row[1] = Convert.ToString(f[1]);
                    row[2] = Convert.ToString(f[2]);
                    row[3] = Convert.ToDecimal(f[3]);
                    row[4] = Convert.ToString(f[4]);
                    row[5] = Convert.ToDecimal(f[5]);
                    row[6] = Convert.ToString(f[6]);
                    row[7] = Convert.ToDecimal(f[7]);
                    row[8] = Convert.ToString(f[8]);
                    row[9] = Convert.ToDecimal(f[9]);
                    row[10] = Convert.ToString(f[10]);
                    row[11] = Convert.ToDecimal(f[11]);
                    dt.Rows.Add(row);
                }

                retorno = new object[2] { true, dt };
            }
            else
                retorno = new object[2] { false, "No existen Facturas a exportar" };
        }
        else
            retorno = new object[2] { false, Convert.ToString(retorno[1]) };
    }
    public void obtieneDet(int id)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Factura"); //0
        dt.Columns.Add("Fecha de Revision"); //1
        dt.Columns.Add("Fecha Programada de Pago"); //2
        dt.Columns.Add("Fecha de Pago"); //3
        dt.Columns.Add("Monto"); //4
        dt.Columns.Add("Orden"); //5
        dt.Columns.Add("Tienda/Taller"); //6
        dt.Columns.Add("Estatus"); //7
        dt.Columns.Add("Tipo Vehiculo"); //8
        dt.Columns.Add("Marca"); //9
        dt.Columns.Add("Unidad"); //10
        dt.Columns.Add("Modelo"); //11
        dt.Columns.Add("Color"); //12
        dt.Columns.Add("Placas"); //13
        dt.Columns.Add("No. Siniestro"); //14
        dt.Columns.Add("estatusbd"); //15


        obtieneInfoDet(id);
        if (Convert.ToBoolean(retorno[0]))
        {
            DataSet datosFacturas = (DataSet)retorno[1];
            if (datosFacturas.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow f in datosFacturas.Tables[0].Rows)
                {
                    DataRow row = dt.NewRow();
                    row[0] = Convert.ToString(f[0]);
                    row[1] = Convert.ToString(f[1]);
                    row[2] = Convert.ToString(f[2]);
                    row[3] = Convert.ToString(f[3]);
                    try { row[4] = Convert.ToDecimal(f[12]).ToString("N2"); } catch (Exception) { row[4] = Convert.ToDecimal(0).ToString("N2"); }
                    row[5] = Convert.ToString(f[9]);

                    Recepciones recepciones = new Recepciones();
                    string taller = recepciones.obtieneNombreTaller(Convert.ToString(f[8]));
                    row[6] = taller;
                    string estatusFacturas = Convert.ToString(f[6]);
                    string estatus = "X";

                    if (estatusFacturas == "PAG")
                        estatus = "PAGADA";
                    else if (estatusFacturas == "REV")
                        estatus = "REVISADA";
                    else if (estatusFacturas == "CAN")
                        estatus = "CANCELADA";
                    else if (estatusFacturas == "PRO")
                    {
                        DateTime fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                        DateTime fechaActual = new E_Utilities.Fechas().obtieneFechaLocal();
                        try { fecha = Convert.ToDateTime(f[2]); } catch (Exception) { fecha= new E_Utilities.Fechas().obtieneFechaLocal(); }
                        TimeSpan ts = fechaActual - fecha;
                        int dias = ts.Days;
                        if (dias > 0)
                        {
                            estatus = "VENCIDA";
                            estatusFacturas = "VEN";
                        }
                        else
                        {
                            estatus = "PROGRAMADA";
                            estatusFacturas = "PRO";
                        }
                    }
                    else if (estatusFacturas == "PEN")
                        estatus = "PENDIENTE";
                    else
                        estatus = "";

                    row[7] = estatus;
                    row[15] = estatusFacturas;
                    ReporFactu factu = new ReporFactu();
                    factu.obtieneInformacion(Convert.ToInt32(f[7]), Convert.ToInt32(f[8]), Convert.ToInt32(f[9]));
                    if (Convert.ToBoolean(factu.retorno[0]))
                    {
                        DataSet infoVehi = (DataSet)factu.retorno[1];
                        if (infoVehi.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dv in infoVehi.Tables[0].Rows)
                            {
                                row[8] = Convert.ToString(dv[1]);
                                row[9] = Convert.ToString(dv[2]);
                                row[10] = Convert.ToString(dv[3]);
                                row[11] = Convert.ToString(dv[4]);
                                row[12] = Convert.ToString(dv[5]);
                                row[13] = Convert.ToString(dv[6]);
                                row[14] = Convert.ToString(dv[7]);
                                break;
                            }
                        }
                        else
                            row[8] = row[9] = row[10] = row[11] = row[12] = row[13] = row[14] = string.Empty;
                    }
                    else
                        row[8] = row[9] = row[10] = row[11] = row[12] = row[13] = row[14] = string.Empty;
                    dt.Rows.Add(row);
                }
            }
            retorno = new object[] { true, dt };
        }
        else
            retorno = new object[] { false, "" };
    }


    internal void obtieneDetTot()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Total a Pagar"); //0

        obtieneTot();

        if (Convert.ToBoolean(retorno[0]))
        {
            DataSet total = (DataSet)retorno[1];
            if (total.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow f in total.Tables[0].Rows)
                {
                    DataRow row = dt.NewRow();
                    row[0] = Convert.ToString(f[0]);
                }
            }
        }
    }
    private void obtieneFacturas(int id)
    {
        string condicion = "";
        if (id != 0)
            condicion = " where t.id_cliprov = " + id + " ";
        sql = " select t.id_cliprov,t.razon_social, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov AND estatus = 'PEN' and factura = t.factura  order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = t.id_cliprov AND f.estatus = 'PEN') as t group by factura, FechaRevision, FechaProgPago, FechaPago, id_cliprov, folio, estatus, id_empresa, id_taller, no_orden, clv_politica, razon_social) as r ) as Pendientes, " + 
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov AND estatus = 'PEN' and factura = t.factura  order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = t.id_cliprov AND f.estatus = 'PEN') as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r ) as MoPendientes, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as Pagadas, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as MoPagadas, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as Programadas, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as MoProgramadas, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' and estatus = 'CAN' and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' and f.estatus = 'CAN' and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as Canceladas, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' and estatus = 'CAN' and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' and f.estatus = 'CAN' and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as MoCanceladas, " +
" (select count(*) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and datediff(day, fechaprogpago, '2017-05-18') > 0 and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and datediff(day, f.fechaprogpago, '2017-05-18') > 0 and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as Vencidas, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and datediff(day, fechaprogpago, '2017-05-18') > 0 and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and datediff(day, f.fechaprogpago, '2017-05-18') > 0 and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as MoVencidas " +
" from(select fa.id_cliprov, fa.razon_social from facturas fa where fa.tipocuenta = 'PA' group by fa.id_cliprov, fa.razon_social) as t "+condicion+" order by t.id_cliprov";
        retorno = ejecutaa.scalarData(sql);
    }

    private void obtieneInfoDet(int id)
    {
        sql = "select * from(select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov AND estatus = 'PEN' and factura = t.factura  order by 1 desc ) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = id_cliprov AND f.estatus = 'PEN') as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social " +
            " union all " +
" select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND(FechaPago IS NOT NULL or fechaPago is null and estatus = 'PAG') and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc ) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND(f.FechaPago IS NOT NULL or f.fechaPago is null and f.estatus = 'PAG') and f.id_cliprov = id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social" +
" union all " +
" select  t.factura,t.FechaRevision,t.FechaProgPago,t.FechaPago,t.id_cliprov,t.folio,'PRO' AS estatus,t.id_empresa,t.id_taller,t.no_orden,t.clv_politica,t.razon_social,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc ) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social" +
" union all " +
" select *,(select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' and estatus = 'CAN' and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc ) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' and f.estatus = 'CAN' and f.id_cliprov = id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social) as t WHERE t.id_cliprov="+id;
        retorno = ejecutaa.scalarData(sql);
    }
    private void obtieneTot()
    {
        sql = "select t.id_cliprov,t.razon_social, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where tipocuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NULL AND FechaRevision IS NULL and id_cliprov = t.id_cliprov AND estatus = 'PEN' and factura = t.factura  order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NULL AND f.FechaRevision IS NULL and f.id_cliprov = t.id_cliprov AND f.estatus = 'PEN') as t group by factura, FechaRevision, FechaProgPago, FechaPago, id_cliprov, folio, estatus, id_empresa, id_taller, no_orden, clv_politica, razon_social) as r ) as MoPenndientes, " +
" (select isnull(sum(monto_pagar), 0) from(select *, (select top 1 case when monto_pagar = 0 then monto else monto_pagar END as monto_pagar from facturas where TipoCuenta = 'PA' AND FechaPago IS NULL AND FechaProgPago IS NOT NULL and id_cliprov = t.id_cliprov and factura = t.factura order by 1 desc) as monto_pagar from(select f.factura, f.FechaRevision, f.FechaProgPago, f.FechaPago, f.id_cliprov, f.folio, estatus, f.id_empresa, f.id_taller, f.no_orden, f.clv_politica, f.razon_social from facturas f where f.TipoCuenta = 'PA' AND f.FechaPago IS NULL AND f.FechaProgPago IS NOT NULL and f.id_cliprov = t.id_cliprov) as t group by factura,FechaRevision,FechaProgPago,FechaPago,id_cliprov,folio,estatus,id_empresa,id_taller,no_orden,clv_politica,razon_social)as r) as MoProgramadas " +
" from(select fa.id_cliprov, fa.razon_social from facturas fa where fa.tipocuenta = 'PA' group by fa.id_cliprov, fa.razon_social) as t order by t.id_cliprov";
    }
}