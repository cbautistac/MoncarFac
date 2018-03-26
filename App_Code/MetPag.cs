using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de MetPag
/// </summary>
public class MetPag
{
    Ejecuciones ejecuta = new Ejecuciones();
    public string codigo { get; set; }
    public string Descripcion { get; set; }

    public object[] retorno;
    private string sql;
    public MetPag()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public void agregarMetodo()
    {
        sql = "insert into c_MetodoPago values ('" + codigo + "','" + Descripcion + "')";
        retorno = ejecuta.insertUpdateDelete(sql);
    }


    public void obtieneMetodoEdit()
    {
        sql = "select * from c_MetodoPago where Clave='" + codigo + "'";
        retorno = ejecuta.dataSet(sql);
    }

    public void editaMetodo()
    {
        sql = "UPDATE c_MetodoPago " +
                " SET  Clave='" + codigo + "', Descripcion='" + Descripcion + "' where Clave='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void eliminaMetodo()
    {
        sql = "delete from c_MetodoPago where Clave='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}