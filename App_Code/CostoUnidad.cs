using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de CostoUnidad
/// </summary>
public class CostoUnidad
{
    Ejecuciones ejecuta = new Ejecuciones();
    public CostoUnidad()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
    public bool quitaProductoImpresion(int empresa, int taller, int no_orden, int id_material, string identificador)
    {
        string sql = "delete from tmp_costo_unidad where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + no_orden + " and id_material=" + id_material + " and identificador='" + identificador + "'";
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
    public bool agregaProductoImpresion(int empresa, int taller, string identificador, string fecha, decimal cantidad, string nombre, decimal montoAutorizado, string razonSocial, decimal monto1, decimal monto2, int no_orden, int id_material)
    {
        string sql = "insert into tmp_costo_unidad values(" + empresa + "," + taller + "," + no_orden + "," + id_material + ",'" + fecha + "','" + identificador + "'," + cantidad + ",'" + nombre + "'," + montoAutorizado + ",'" + razonSocial + "'," + monto1 + "," + monto2 + ",'A')" ;
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
    public bool quitarTodo( int empresa, int taller, int orden )
    {
        string sql = "delete from tmp_costo_unidad where id_taller=" + taller + " and id_empresa=" + empresa + " and no_orden=" + orden;
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public bool agregarTodo(int taller, int empresa, int orden)
    {
        string sql = "";
       // DataTable dt = tablaTodo(sql);

        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
    internal object[] obtieneManoObraCc(int empresa, int taller, int ordenT)
    {
        string sql = string.Format("select fecha, nombre,montoAutorizado,monto1, montoAutorizado-monto1 as utilidad from tmp_costo_unidad  where id_taller=" + taller + " and id_empresa=" + empresa + " and no_orden=" + ordenT + " and identificador='Mano de Obra'");
        return ejecuta.dataSet(sql);
    }
    internal object[] obtienerefaccionesc(int empresa, int taller, int ordenT)
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = string.Format("select nombre,cantidad,razon_social,montoAutorizado,monto1,(montoAutorizado-monto1) as utilidadRef from tmp_costo_unidad  where id_taller=" + taller + " and id_empresa=" + empresa + " and no_orden=" + ordenT + " and identificador='Refacciones'");
        return ejecuta.dataSet(sql);
    }

    public bool AgregaTodoASeleccionar(int empresa, int taller, int orden,  int material, string fecha, string  identificador, decimal cantidad, string nombre, decimal montoA, string razon_social, decimal monto1, decimal monto2)
    {
        string sql = "insert into tmp_costo_unidad_todos values ("+ empresa + "," + taller + "," + orden + "," + material + ",'" + fecha + "','" + identificador + "'," + cantidad + ",'" + nombre + "'," + montoA + ",'" + razon_social + "'," + monto1 + "," + monto2 + ",'P')"; 
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
    public bool tb1atb2(int empresa, int taller, int orden, int material, string fecha, string identificador, decimal cantidad, string nombre, decimal montoA, string razon_social, decimal monto1, decimal monto2)
    {
        string sql = "insert into tmp_costo_unidad values (" + empresa + "," + taller + "," + orden + "," + material + ",'" + fecha + "','" + identificador + "'," + cantidad + ",'" + nombre + "'," + montoA + ",'" + razon_social + "'," + monto1 + "," + monto2 + ",'P')";
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
    public bool borrarTemporal1(int empresa, int taller, int orden)
    {
        string sql = "delete from tmp_costo_unidad_todos where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden;
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
    public bool borrarTemporal2(int empresa, int taller, int orden)
    {
        string sql = "delete from tmp_costo_unidad where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden;
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public bool TerminarImpresion1(int taller, int empresa, int orden)
    {
        string sql1 = "delete from tmp_costo_unidad_todos where id_empresa= " + empresa + " and id_taller=" + taller + " and no_orden=" + orden;       
        object[] resultado = ejecuta.insertUpdateDelete(sql1);   
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;            
    }
    public bool TerminarImpresion2(int taller, int empresa , int orden)
    {        
        string sql = "delete from tmp_costo_unidad where id_empresa= " + empresa + " and id_taller=" + taller + " and no_orden=" + orden;        
        object[] resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
}
