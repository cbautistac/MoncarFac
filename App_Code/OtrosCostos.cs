using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de OtrosCostos
/// </summary>
public class OtrosCostos
{
    Fechas fechas = new Fechas();
    public int orden { get; set; }
    public int taller { get; set; }
    public int empresa { get; set; }
    public int renglon { get; set; }
    public DateTime fecha { get; set; }
    public string descripcion { get; set; }
    public decimal proveedor { get; set; }
    public double cantidad { get; set; }
    public double costo { get; set; }
    public double importe { get; set; }
    public string area { get; set; }
    public string factura { get; set; }
    public double descuento { get; set; }
    public  string notaCredito { get; set; }
    public object[] retorno { get; set; }
    public int pago { get; set; }    

    public OtrosCostos()
	{
        orden = taller = empresa = renglon = 0;
        descripcion = area = factura = notaCredito = string.Empty;
        proveedor = 0;
        cantidad = costo = importe = descuento = 0;
        pago = -1;
        fecha = fechas.obtieneFechaLocal();
        retorno = new object[2] { false, "" };
	}

    public void generaOtroCosto() {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = string.Format("declare @renglon int set @renglon = (select isnull((select top 1 renglon from otros_costos where id_empresa={2} and id_taller={1} and no_orden={0} order by renglon desc),0))+1 " +
            "insert into otros_costos values({0},{1},{2},@renglon,'{3}','{4}',{5},{6},{7},{8},'{9}','{10}',{11},'{12}',{13}) select @renglon", orden, taller, empresa, fecha.ToString("yyyy-MM-dd"), descripcion, proveedor, cantidad, costo, importe, area, factura, descuento, notaCredito, pago);
        try
        {
            retorno = ejecuta.scalarToInt(sql);
            int renglon = 0;
            if (Convert.ToBoolean(retorno[0]))
                renglon = Convert.ToInt32(retorno[1]);

            if (renglon != 0)
            {
                Facturas facturas = new Facturas();
                facturas.folio = orden;
                facturas.tipoCuenta = "PA";
                facturas.factura = factura;
                CatClientes clientes = new CatClientes();
                string politica = clientes.obtieneClavePolitica(proveedor);
                if (area != "CA")
                    facturas.estatus = "PEN";
                else
                    facturas.estatus = "PAG";
                facturas.id_cliprov = Convert.ToInt32(proveedor);
                facturas.politica = politica;
                
                facturas.empresa = empresa;
                facturas.taller = taller;
                facturas.tipoCargo = "C";
                facturas.orden = orden;
                facturas.razon_social = obtieneRazonSocial(Convert.ToInt32(proveedor), "P");
                Recepciones recepciones = new Recepciones();
                facturas.monto = recepciones.obtieneMonto(empresa, taller, orden, factura, proveedor.ToString());
                facturas.generaFactura();
                

                object[] retornos = facturas.retorno;
                if (Convert.ToBoolean(retornos[0]))
                {
                    retorno[0] = true;
                    retorno[1] = "";
                }
                else
                {
                    eliminaOtroCosto();
                    retorno[0] = false;
                    retorno[1] = retornos[1];
                }
            }
            else {
                retorno[0] = false;
                retorno[1] = "No es posible generar el registro";
            }
        }
        catch (Exception ex) {
            retorno[0] = false;
            retorno[1] = ex.Message;
        }
    }

    public void obtieneInfoOtrosCostosCCyCP()
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = "select * from otros_costos where no_orden = " + orden + " and id_empresa = " + empresa + " and id_taller = " + taller + " and factura = '" + factura + "' and proveedor =" + proveedor;
        retorno = ejecuta.dataSet(sql);
    }

    public void eliminaOtroCosto() {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = "delete from otros_costros where renglon=" + renglon + " and id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void obtieneInfoOtroCosto()
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = "select * from otros_costos where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden + " AND Renglon=" + renglon + " and Area_de_Aplicacion='" + area + "'";
        retorno = ejecuta.dataSet(sql);
    }

    public void actualizaOtroCosto()
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = string.Format("update otros_costos set fecha='{3}', descripcion='{4}', proveedor={5}, cantidad={6}, costo_unitario={7}, importe={8}, factura='{10}', descuento={11}, id_nota_credito='{12}', pago={14} where no_orden={0} and id_taller={1} and id_empresa={2} and renglon={13} and area_de_aplicacion='{9}'", orden, taller, empresa, fecha.ToString("yyyy-MM-dd"), descripcion, proveedor, cantidad, costo, importe, area, factura, descuento, notaCredito, renglon, pago);
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    internal object[] obtienecajachica(int empresa, int taller, int ordenT)
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = string.Format("Select OC.renglon, OC.fecha, OC.descripcion, OC.proveedor, ltrim(rtrim(C.razon_social)) as razon_social, oc.cantidad, oc.Costo_Unitario, oc.Importe, oc.Factura, oc.Descuento, oc.id_nota_credito " +
                                   "from otros_costos OC LEFT JOIN CLIPROV C ON c.id_cliprov = Cast(oc.proveedor AS INT) anD c.tipo = 'P' " +
                                   "where OC.id_empresa = {1} and OC.id_taller = {1} and OC.no_orden = {2} and OC.area_de_aplicacion = 'CA' ", empresa, taller, ordenT);

        return ejecuta.dataSet(sql);
    }

    internal object[] obtienealmacen(int empresa, int taller, int ordenT)
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = string.Format("SELECT venta_det.renglon, venta_enc.ticket, venta_enc.fecha_venta, LTRIM(Cliprov.razon_social), venta_det.cantidad, venta_det.descripcion, venta_det.venta_unitaria, venta_det.valor_descuento, venta_det.importe " +
                                   "FROM venta_det INNER JOIN Registro_Pinturas AS rp ON venta_det.ticket = rp.ticket INNER JOIN venta_enc ON venta_det.ticket = venta_enc.ticket INNER JOIN Cliprov ON venta_enc.id_prov = Cliprov.id_cliprov AND Cliprov.tipo = 'P' " +
                                   "WHERE(rp.id_empresa = {1}) AND(rp.id_taller = {1}) AND(rp.no_orden = {2}) AND(venta_enc.Area_Aplicacion = 'Al') ORDER BY venta_enc.fecha_venta ", empresa, taller, ordenT);

        return ejecuta.dataSet(sql);
    }

    private string obtieneRazonSocial(int idCliprov, string tipo)
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string razonsocial = "";
        string sql = "select razon_social from cliprov where id_cliprov=" + idCliprov.ToString() + " and tipo='"+tipo+"'";
        object[] ejecutado = ejecuta.scalarToString(sql);
        if ((bool)ejecutado[0])
            razonsocial = ejecutado[1].ToString();
        else
            razonsocial = "";
        return razonsocial;
    }
}