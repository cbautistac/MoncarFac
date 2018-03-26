using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de TotalesOrden
/// </summary>
/// 

public class TotalesOrden
{
    Ejecuciones ejecuta = new Ejecuciones();
    int _empresa;
    int _taller;
    int _orden;
    object[] _retorno;
    decimal _importes;
    decimal _mo;
    decimal _ref;

	public TotalesOrden()
	{
        _empresa = _taller = _orden = 0;
        _retorno = new object[2];
        _importes=_ref=_mo = 0;
	}

    public int Empresa { set { _empresa = value; } }
    public int Taller { set { _taller = value; } }
    public int Orden { set { _orden = value; } }
    public object[] Retorno { get { return _retorno; } }
    public decimal Importe { set { _importes = value; } get { return _importes; } }
    public decimal Refacciones { set { _ref = value; } get { return _ref; } }
    public decimal ManoObra { set { _mo = value; } get { return _mo; } }

    public void obtieneTotalManoObra() {
        string sql = string.Format("select isnull(sum(isnull(monto_mo,0)),0) from Mano_Obra where id_empresa={0} and id_taller={1} and no_orden={2}", _empresa, _taller, _orden);
        _retorno = ejecuta.scalarToDecimal(sql);
        try { _mo = Convert.ToDecimal(_retorno[1].ToString()); }
        catch (Exception) { _mo = 0; }

    }

    public void obtieneTotalManoRefacciones()
    {
        //string sql = string.Format("select isnull(sum(isnull(refPrecioVenta,0)*refCantidad),0) as importe from Refacciones_Orden where ref_id_empresa={0} and ref_id_taller={1} and ref_no_orden={2} and refestatussolicitud<>11 and proceso is null", _empresa, _taller, _orden);
        string sql = string.Format("select isnull(sum(isnull(refPrecioVenta,0)*refCantidad),0) as importe from Refacciones_Orden where ref_id_empresa={0} and ref_id_taller={1} and ref_no_orden={2} and proceso is null", _empresa, _taller, _orden);
        _retorno = ejecuta.scalarToDecimal(sql);
        try { _ref = Convert.ToDecimal(_retorno[1].ToString()); }
        catch (Exception) { _ref = 0; }
    }

    public void actualizaTotales() {
        string sql = string.Format("update ordenes_reparacion set total_mo={0}, total_ref={1}, total_orden={2} where id_empresa={3} and id_taller={4} and no_orden={5}", _mo, _ref, _importes, _empresa, _taller, _orden);
        _retorno = ejecuta.insertUpdateDelete(sql);
    }

}