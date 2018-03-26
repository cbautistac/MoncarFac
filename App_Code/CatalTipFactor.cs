using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatalTipFactor
/// </summary>
public class CatalTipFactor
{
    Ejecuciones ejecuta = new Ejecuciones();
    private string sql;
    public object[] retorno;
    public string cd { get; set; }
	public CatalTipFactor()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public void cargardatos()
    {
        sql = "select * from TipoFactor_FSAT where ClaveTipo=" + cd;
        retorno = ejecuta.dataSet(sql);
    }
    public void elminarFactTipo()
    {
        sql = "delete from TipoFactor_FSAT where ClaveTipo='" + cd + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public bool agregaNuevoFactor(string ClavTipFac, string Desc)
    {
        sql = "insert into TipoFactor_FSAT(ClaveTipo,Descripcion) values ((select isnull((select top 1 ClaveTipo from TipoFactor_FSAT order by ClaveTipo desc),0)+1), '" + Desc + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }
    public void editDatos(string ClavTipFac, string Desc, int TipFacClave)
    {
        sql = "update TipoFactor_FSAT set Descripcion='" + Desc + "' where ClaveTipo='" + TipFacClave + "'";
        retorno[1] = ejecuta.insertUpdateDelete(sql);
    }
}