using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatalImpuesto
/// </summary>
public class CatalImpuesto
{
    Ejecuciones ejecuta = new Ejecuciones();
    private string sql;
    public object[] retorno;
    public string cd { get; set; }
	public CatalImpuesto()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public void cargardatos()
    {
        sql = "select * from impuesto_fsat where ClaveImpuesto=" + cd;
        retorno = ejecuta.dataSet(sql);
    }
    public void elminarImpuesto()
    {
        sql = "delete from impuesto_fsat where ClaveImpuesto='" + cd + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public bool agregaNuevoImpuesto(string ClavImp, string Desc, string Retenido, string Trasl, string LocFed, string EntAp)
    {
        sql = "insert into impuesto_fsat(ClaveImpuesto,Descripcion, Retencion,Traslado, LocalFederal, EntidadAplica) values ((select isnull((select top 1 ClaveImpuesto from impuesto_fsat order by ClaveImpuesto desc),0)+1), '" + Desc + "', '" + Retenido + "', '" + Trasl + "', '" + LocFed + "', '" + EntAp + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }
    public void editDatos(string ClavImp, string Desc, string Retenido, string Trasl, string LocFed, string EntAp, int impuClave)
    {
        sql = "update impuesto_fsat set Descripcion='" + Desc + "', Retencion='" + Retenido + "', Traslado='" + Trasl + "',LocalFederal='" + LocFed + "', EntidadAplica='" + EntAp + "' where ClaveImpuesto='" + impuClave + "'";
        retorno[1] = ejecuta.insertUpdateDelete(sql);
    }

}