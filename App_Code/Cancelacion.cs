using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Cancelacion
/// </summary>
public class Cancelacion
{
    Ejecuciones ejecuta = new Ejecuciones();
    public int factura { get; set; }

    public object[] retorno;
    private string sql;
    public Cancelacion()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public void editaEstatus()
    {
        sql = "UPDATE encCFD_f " +
                " SET  encestatus='C' where idCfd="+factura ;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

}