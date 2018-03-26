using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatMon
/// </summary>
public class CatMon
{
    Ejecuciones ejecuta = new Ejecuciones();
    public string codigo { get; set; }
    public string Descripcion { get; set; }
    public int Decimales { get; set; }
    public decimal Porcentaje { get; set; }

    public object[] retorno;
    private string sql;
    public CatMon()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public void agregarMoneda()
    {
        sql = "insert into moneda_f values ('"+codigo+"','"+Descripcion+"',"+Decimales+","+Porcentaje+")";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

  
        public void obtieneMonedaEdit()
    {
        sql = "select * from moneda_f where Clave_Mon='"+codigo+"'";
        retorno = ejecuta.dataSet(sql);
    }

    public void editaMoneda()
    {
        sql = "UPDATE moneda_f " +
                " SET  Clave_Mon='" + codigo + "', Mon_Desc='"+Descripcion+"', Decimales="+Decimales+", Porcentaje_var="+Porcentaje+ " where Clave_Mon='"+codigo+"'" ;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void eliminaMoneda()
    {
        sql = "delete from moneda_f where Clave_Mon='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

}