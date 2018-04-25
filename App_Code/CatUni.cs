using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatUni
/// </summary>
public class CatUni
{
    Ejecuciones ejecuta = new Ejecuciones();
    public string codigo { get; set; }
    public string Descripcion { get; set; }
    public string Nombre { get; set; }
    public string fechaini { get; set; }
    public string fechafin { get; set; }
    public string simbolo { get; set; }
    public int Decimales { get; set; }
    public decimal Porcentaje { get; set; }
    public string Temp { set; get; }

    public object[] retorno;
    private string sql;

    public CatUni()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public void agregarUnidad()
    {
        sql = "insert into unidades_f values ('" + codigo + "','" + Descripcion + "')";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void obtieneunidadEdit()
    {
        sql = "select * from unidades_f where idUnid='" + codigo + "'";
        retorno = ejecuta.dataSet(sql);
    }

    public void editaUnidad()
    {
        sql = "UPDATE unidades_f " +
                " SET  idUnid='" + codigo + "', Uniddesc='" + Descripcion + "' where idUnid='" + codigo + "' ";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void eliminaUnidad()
    {
        sql = "delete from unidades_f where idUnid='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void agregarUnidadSat()
    {
        sql = "insert into c_unidad_f values ('" + codigo + "','" + Nombre + "','"+Descripcion+"','"+simbolo+"')";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void editaUnidadSat()
    {
        sql = "UPDATE c_unidad_f " +
                " SET  ClaveUnidad='" + codigo + "', Nombre='" + Nombre + "',Descripcion='" + Descripcion + "', Simbolo='" + simbolo + "' where ClaveUnidad='" + Temp + "' ";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void obtieneunidadEditSat()
    {
        sql = "select ClaveUnidad, RTRIM(Nombre) as Nombre, RTRIM(Descripcion) as Descripcion, RTRIM(Simbolo) as Simbolo from c_unidad_f where ClaveUnidad='" + codigo + "'";
        retorno = ejecuta.dataSet(sql);
    }
    public void eliminaUnidadSat()
    {
        sql = "delete from c_unidad_f where ClaveUnidad='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}