using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatalMetPago
/// </summary>
public class CatalMetPago
{
    Ejecuciones ejecuta = new Ejecuciones();
    private string sql;
    public object[] retorno;
    public string cd { get; set; }
	public CatalMetPago()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public void cargardatos()
    {
        sql = "select * from MetodoPago_fsat where ClaveMetodo=" + cd;
        retorno = ejecuta.dataSet(sql);
    }
    public void elminarMetPago()
    {
        sql = "delete from MetodoPago_fsat where ClaveMetodo='" + cd + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public bool agregaNuevoMetodo(string ClavMet, string Desc)
    {
        sql = "insert into MetodoPago_fsat(ClaveMetodo,Descripcion) values ((select isnull((select top 1 ClaveMetodo from MetodoPago_fsat order by ClaveMetodo desc),0)+1), '" + Desc + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }
    public void editDatos(string ClavTipFac, string Desc, int TipFacClave)
    {
        sql = "update MetodoPago_fsat set Descripcion='" + Desc + "' where ClaveMetodo='" + TipFacClave + "'";
        retorno[1] = ejecuta.insertUpdateDelete(sql);
    }
}