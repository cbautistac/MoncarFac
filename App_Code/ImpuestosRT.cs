using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ImpuestosRT
/// </summary>
public class ImpuestosRT
{
    Ejecuciones ejecuta = new Ejecuciones();
    public string codigo { get; set; }
    public string Descripcion { get; set; }
    public string Nombre { get; set; }
    public string Cuenta { get; set; }
    public string simbolo { get; set; }
    public decimal tasa { get; set; }
    public decimal Porcentaje { get; set; }
    public int idRedEdita { get; set; }

    public object[] retorno;
    private string sql;
    public ImpuestosRT()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public void agregarImpuestoR()
    {
        sql = "insert into impRetenidos_f values ((select isnull((select top 1 id_ret from ImpRetenidos_f order by ID_Ret desc),0)+1),'"+Nombre+"','"+Descripcion+"',"+tasa+","+Cuenta+")";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void eliminaImpuestoR()
    {
        sql = "delete impRetenidos_f where id_ret=" + idRedEdita;
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void editaImpuestoR()
    {
        sql = "update impRetenidos_f set retnombre='"+Nombre+"', retdescrip='"+Descripcion+"', rettasa="+tasa+", retcuentaconta="+Cuenta+" where id_ret=" + idRedEdita;
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void obtieneRetencion()
    {
        sql = "select * from impRetenidos_f where id_ret=" + idRedEdita;
        retorno = ejecuta.dataSet(sql);
    }
}