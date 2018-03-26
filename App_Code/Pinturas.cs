using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;

public class Pinturas
{
    string sql;
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    object[] ejecutado = new object[2];
    public Pinturas()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    internal object[] obtienepintura(int empresa, int taller, int ordenT) 
    {
        string sql = string.Format("select venta_det.renglon, venta_enc.ticket, venta_enc.fecha_venta, LTRIM(Cliprov.razon_social), venta_det.cantidad, venta_det.descripcion, venta_det.venta_unitaria, venta_det.valor_descuento, venta_det.importe "+
                                   "from venta_det inner join Registro_Pinturas as rp on venta_det.ticket = rp.ticket inner join venta_enc ON venta_det.ticket = venta_enc.ticket inner join Cliprov ON venta_enc.id_prov = Cliprov.id_cliprov AND Cliprov.tipo = 'P' "+
                                   "where (rp.id_empresa = {1})  AND(rp.id_taller = {1}) AND(rp.no_orden = {2}) AND(venta_enc.Area_Aplicacion = 'Pn') ORDER BY venta_enc.fecha_venta ", empresa, taller, ordenT);
        return ejecuta.dataSet(sql);
    }



}