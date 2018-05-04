using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ProdServ
/// </summary>
public class ProdServ
{

    Ejecuciones ejecuta = new Ejecuciones();
    public string codigo { get; set; }
    public string Descripcion { get; set; }
    public string fechaini { get; set; }
    public string fechafin { get; set; }
    public string iva { get; set; }
    public string eps { get; set; }
    public string complemento { get; set; }
    public int fac { get; set; }



    public object[] retorno;
    private string sql;
    public ProdServ()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public void agregarServicio()
    {
        sql = "insert into c_ProdServ_f values ('" + codigo + "','" + Descripcion + "','"+iva+"','"+eps+"','"+complemento+"')";
        retorno = ejecuta.insertUpdateDelete(sql);
    }


    public void obtieneservEdit()
    {
        sql = "select * from c_ProdServ_f where ClaveProdServ='" + codigo + "'";
        retorno = ejecuta.dataSet(sql);
    }

    public void editaMoneda()
    {
        sql = "UPDATE c_ProdServ_f " +
                " SET  ClaveProdServ='" + codigo + "', Descripcion='" + Descripcion + "',IncIVATraslado='"+iva+ "',IncEPSTraslado='"+eps+ "',ComplementoIncluir='"+complemento+"' where ClaveProdServ='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void eliminaMoneda()
    {
        sql = "delete from c_ProdServ_f where ClaveProdServ='" + codigo + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void cambiaEstatus()
    {
        sql = "update encCfd_f set encestatus='C' where idCFD=" + fac;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void cambiaEstatusCompPago()
    {
        sql = "update Recepcion_pagos_F set encestatus='C' where idCFD=" + fac;
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}