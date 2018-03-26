using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de ReporFactu
/// </summary>
public class ReporFactu
{
    Ejecuciones ejecuta = new Ejecuciones();
    BaseDatos ejecutaa = new BaseDatos();
    public int usuario { get; set; }
    public int empresa { get; set; }
    public int taller { get; set; }
    public object[] retorno { get; set; }
    public string Ini { get; set; }
    public string Fin { get; set; }
    private string sql;
    public ReporFactu()
    {
        usuario = empresa = taller = 0;
        retorno = new object[2] { false, "" };
        Ini = Fin = sql = string.Empty;
    }
    public void obtieneCFD()
    {        
        DataTable dt = new DataTable();
        dt.Columns.Add("RFC");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("UUID");
        dt.Columns.Add("Referencia");
       /* dt.Columns.Add("Factura");
        dt.Columns.Add("Fecha_Crea");
        dt.Columns.Add("Fecha_Genera");
        dt.Columns.Add("Fecha_Cancela");
        dt.Columns.Add("Neto");
        dt.Columns.Add("Descuento_Global");
        dt.Columns.Add("Subtotal");
        dt.Columns.Add("Descuento");
        dt.Columns.Add("Traslados");
        dt.Columns.Add("Retenciones");
        dt.Columns.Add("Total");
        dt.Columns.Add("Taller-Tienda");
        dt.Columns.Add("Orden-Ticket");
        dt.Columns.Add("Marca");
        dt.Columns.Add("Modelo");
        dt.Columns.Add("Color");
        dt.Columns.Add("Placas");
        dt.Columns.Add("Sinistero");
        dt.Columns.Add("Monto_Pago");
        dt.Columns.Add("Fecha_Pago");
        dt.Columns.Add("Notas");
        dt.Columns.Add("Estatus");*/

        obtieneFacturas();
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
                    row[3] = "";
                    row[4] = Convert.ToString(f[3]);
                    row[5] = Convert.ToString(f[4]);
                    row[6] = Convert.ToString(f[5]);
                    row[7] = Convert.ToString(f[6]);
                    row[8] = Convert.ToDecimal(f[9]);
                    row[9] = Convert.ToDecimal(f[8]) + Convert.ToDecimal(f[10]);
                    row[10] = Convert.ToDecimal(f[9]) - (Convert.ToDecimal(f[8]) + Convert.ToDecimal(f[10]));
                    row[11] = Convert.ToDecimal(f[10]);
                    row[12] = Convert.ToDecimal(f[11]);
                    row[13] = Convert.ToDecimal(f[12]);
                    row[14] = Convert.ToDecimal(f[13]);
                    //Taller-Tienda
                    try
                    {
                        if (Convert.ToInt32(f[26]) > 0)
                            row[15] = obtieneNombreTienda(Convert.ToInt32(f[26]));
                        else
                            row[15] = obtieneNombreTaller(Convert.ToInt32(f[19]));
                    }
                    catch (Exception) { row[15] = obtieneNombreTienda(Convert.ToInt32("0")); }
                    //orden ticket 
                    try { row[16] = Convert.ToInt32(f[20]); } catch (Exception) { row[16] = Convert.ToInt32("0"); }
                    if (Convert.ToInt32(row[15]) > 0) { }
                    else
                    {
                        obtieneInformacion(Convert.ToInt32(f[18]), Convert.ToInt32(f[19]), Convert.ToInt32(f[20]));
                        if (Convert.ToBoolean(retorno[0]))
                        {
                            DataSet infoVehi = (DataSet)retorno[1];
                            if (infoVehi.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dv in infoVehi.Tables[0].Rows)
                                {
                                    row[17] = Convert.ToString(dv[2]) + " " + Convert.ToString(dv[3]);
                                    row[18] = Convert.ToString(dv[4]);
                                    row[19] = Convert.ToString(dv[5]);
                                    row[20] = Convert.ToString(dv[6]);
                                    row[21] = Convert.ToString(dv[7]);
                                    break;
                                }
                            }
                            else
                                row[17] = row[18] = row[19] = row[20] = row[21] = string.Empty;
                        }
                        else
                            row[17] = row[18] = row[19] = row[20] = row[21] = string.Empty;
                    }

                    row[22] = Convert.ToDecimal(f[13]);                    
                    row[23] = Convert.ToString(f[16]);
                    string estatusFact = Convert.ToString(f[14]);
                    string estatusFacturas= Convert.ToString(f[17]);
                    string estatus = "X";
                    if (estatusFact == "T")
                    {
                        if (estatusFacturas == "PAG")
                            estatus = "P";
                        else
                            estatus = "T";
                    }
                    else if (estatusFact == "C")
                        estatus = "C";
                    else
                        estatus = "X";

                    row[24] = string.Empty;
                    row[25] = estatus;
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

    private string obtieneNombreTaller(int taller)
    {
        sql = "select nombre_taller from talleres where id_taller=" + taller;
        return ejecuta.scalarToStringSimple(sql);
    }

    private string obtieneNombreTienda(int tienda)
    {
        sql = "select nombre_pv from punto_venta where id_punto=" + tienda + " and id_almacen=" + tienda;
        return ejecuta.scalarToStringSimple(sql);
    }

    private void obtieneFacturas()
    {
        obtieneFechasParametros();
        sql = "select distinct e.encReRFC,e.encReNombre,e.EncFolioUUID,e.EncReferencia,convert(char(10),e.EncFechaGenera,120)+' '+convert(char(8),e.enchoragenera,120) as EncFechaGenera,substring(t.fechatimbrado, 1, 10)+' '+substring(t.fechatimbrado, 12, 8) as fechatimbrado,convert(char(10), e.encfechaCancel, 120) + ' ' + convert(char(8), e.enchoracancel, 120) as fechaCancela,(e.encsubtotal + e.encdescglobimp) as neto,e.encdescglobimp as dctoGlob,e.encsubtotal as subtotal,e.encimptras as traslados,e.encimpret as retenciones,e.enctotal as total,e.encestatus as estatusFact  from EncCFD_f e left join receptores_f r on r.idrecep=e.idrecep LEFT JOIN TIMBRADO_f T ON T.IDCFD = E.IDCFD where e.encestatus='T' and  e.EncFecha between '" + Ini + "' and '" + Fin + "'";
        retorno = ejecutaa.scalarData(sql);
    }

    internal void obtieneInformacion(int empresa, int taller , int orden)
    {
        sql = "select orp.no_orden,tv.descripcion as tipo_vehiculo,m.descripcion as marca,tu.descripcion as unidad,cast(v.modelo as char(4)) as modelo,rtrim(ltrim(v.color_ext)) as color,rtrim(ltrim(orp.placas)) as placas,orp.no_siniestro from Ordenes_Reparacion orp left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad where orp.id_empresa = " + empresa + " and orp.id_taller = " + taller + " and orp.no_orden =" + orden;
        retorno = ejecuta.dataSet(sql);
    }

    public void gravaPrametros() {
        sql = "Select count(*) from tmp_exporta_facturas where id_usuario=" + usuario;
        retorno = ejecuta.scalarToBool(sql);
        if (Convert.ToBoolean(retorno[0])) {
            if (Convert.ToBoolean(retorno[1]))
            {
                sql = "update tmp_exporta_facturas set fecha_ini='" + Ini + "',fecha_fin='" + Fin + "' where id_usuario=" + usuario;
                ejecuta.insertUpdateDelete(sql);
            }
            else {
                sql = "insert into tmp_exporta_facturas values(" + usuario + ",'" + Ini + "','" + Fin + "')";
                ejecuta.insertUpdateDelete(sql);
            }
        }
    }

    private void obtieneFechasParametros()
    {
        sql = "select * from tmp_exporta_facturas where id_usuario=" + usuario;
        retorno = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(retorno[0]))
        {
            DataSet fechas = (DataSet)retorno[1];
            if (fechas.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in fechas.Tables[0].Rows)
                {
                    Ini = Convert.ToString(r[1]);
                    Fin = Convert.ToString(r[2]);
                }
            }
            else
                Ini = Fin = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
        }
        else
            Ini = Fin = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
    }

}