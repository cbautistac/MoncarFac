using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatBancos
/// </summary>
public class CatalBancos
{
    Ejecuciones ejecuta = new Ejecuciones();
    private string sql;
    public object []retorno;
    public string cd { get; set; }

	public CatalBancos()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public bool agregaNuevoBanco(string RFC, string nomBank, string claveBank)
    {
        sql = "insert into cat_bancos_f(id_Banco,Clave_Banco, RFC_Banco,Nombre_Banco)" +
              " values ((select isnull((select top 1 id_Banco from cat_bancos_f order by id_Banco desc),0)+1),'"+claveBank+"','"+nomBank+"','"+RFC+"')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }
    public void cargardatos()
    {
        sql = "select * from cat_bancos_f where id_Banco="+cd;
        retorno = ejecuta.dataSet(sql);
    }
    public void editDatos(string RFC, string nomBank, int claveBank, int IdBanco)
    {
        sql = "update cat_bancos_f set Clave_Banco='" + claveBank + "', RFC_Banco='" + RFC + "', Nombre_Banco='" + nomBank + "' where id_Banco='" + IdBanco + "'";
        retorno[1] = ejecuta.insertUpdateDelete(sql);
    }

    public void elminarBanco()
    {
        sql="delete from cat_bancos_f where id_banco='"+cd+"'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}