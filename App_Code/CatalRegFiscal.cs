using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatalRegFiscal
/// </summary>
public class CatalRegFiscal
{
    Ejecuciones ejecuta = new Ejecuciones();
    private string sql;
    public object[] retorno;
    public string cd { get; set; }
	public CatalRegFiscal()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public bool agregaRegFisc(string desc, string fisc, string moral, string clveReg)
    {
        sql = "insert into RegimenFiscal_FSAT (ClaveRegimen, Descripcion, Fisica, Moral) values ('" + clveReg + "', '" + desc + "', '" + fisc + "', '" + moral + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }
    public void cargardatos()
    {
        sql = "select * from RegimenFiscal_FSAT where ClaveRegimen=" + cd;
        retorno = ejecuta.dataSet(sql);
    }
    public void editDatos(string clveReg, string desc, string fisc, string moral, string claveRF)
    {
        sql = "update RegimenFiscal_FSAT set  Descripcion='" + desc + "', Fisica='" + fisc + "', Moral='" + moral + "' where ClaveRegimen='" + claveRF + "'";
        retorno[1] = ejecuta.insertUpdateDelete(sql);
    }
    public void elminarRegimen()
    {
        sql = "delete from RegimenFiscal_FSAT where ClaveRegimen='" + cd + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}