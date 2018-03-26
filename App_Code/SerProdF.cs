using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de SerProdF
/// </summary>
public class SerProdF
{
    Ejecuciones ejecuta = new Ejecuciones();
    public string codigo { get; set; }
    public int codigoSat { get; set; }
    public string Descripcion { get; set; }
    public int  unidad { get; set; }
    public string  claveunidad { get; set; }
    public float valunit { get; set; }

    public object[] retorno;
    private string sql;
    public SerProdF()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
    public void agregarProductoF()
    {
        sql = "insert into conceptos_f values ('" + codigo + "',"+codigoSat+",'" + Descripcion + "','" + claveunidad + "'," + unidad + ","+valunit+",0,0,0,0,0,'','')";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void obtieneProdSerEdit()
    {
        sql = "select * from conceptos_f where IdConcepto='" + codigo + "'";
        retorno = ejecuta.dataSet(sql);
    }
    public void editaProducto()
    {
        sql = "UPDATE conceptos_f " +
                " SET  IdConcepto='" + codigo + "', IdEmisor=" + codigoSat + ", CoDescrip='" + Descripcion + "', IdCoMaster='" + claveunidad + "',IdUnid="+unidad+ ",CoValorUnit="+valunit+ " where IdConcepto='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void eliminaProducto()
    {
        sql = "delete from conceptos_f where IdConcepto='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

}