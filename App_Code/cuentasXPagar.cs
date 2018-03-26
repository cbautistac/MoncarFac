using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using E_Utilities;


/// <summary>
/// Descripción breve de cuentasXPagar
/// </summary>
public class cuentasXPagar
{
    ////////////////////////////////////////////////
    //Alfredo Gonzalez Davila || 04ago2017 
    ////////////////////////////////////////////////

    Ejecuciones ejecuta = new Ejecuciones();
    string sql = "";
    //
    private SqlConnection conexionBD;
    private SqlCommand cmd;
    private SqlDataAdapter da;
    private DataSet ds;

    // Definicion de propiedades publicas    
    public int no_orden { get; set; }
    public int id_empresa { get; set; }
    public int id_taller { get; set; }
    public int id_orden { get; set; }
    public int id_cotizacion { get; set; }
    public int id_cliprov { get; set; }
    public string folioCotizacion { get; set; }
    public string folio_orden { get; set; }
    public string factura { get; set; }
    public string folioFacturaRealDelProveedor { get; set; }
    public object[] retorno { get; set; }
    //constructor
    public cuentasXPagar()
    {
        //TODO: Agregar aquí la lógica del constructor
    }
    public void actualizaFolioRealFacturaEnOrdenCompraEncabezado()
    {
        string sql = "update Orden_Compra_Encabezado set factura = " + "'" + folioFacturaRealDelProveedor + "'" + " where Factura='" + factura +
            "' and id_cliprov=" + id_cliprov + " and id_taller=" + id_taller + " and id_empresa=" + id_empresa + " and no_orden=" + no_orden;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void validaFolioEnFacturas()
    {
        //se comenta por Ordenes de Edgar Palacios. ||07Ago2017|16:40pm (conversacion Telefonica)
        //Se explica la situacion de la posibilidad de contar con el mismo numero de factura 2 proveedores diferentes
        //pero se da la indicacion (Edgar Palacios ) de preguntar solo por el folio capturado. si ya existe no hay 
        //posibilidad de usar el mismo folio. usuario devera de capturar uno diferente(.)
        //string sql = "select count(factura) from facturas where factura='" + factura + "'" + " and  id_cliprov= " + id_cliprov;
        string sql = "select count(factura) from facturas_f where factura='" + factura + "'" ;
        retorno = scalarInt(sql);
    }

    
    //
    internal object[] insertUpdateDelete(string sql)
    {
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        object[] retorno = new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
        finally
        {
            conexionBD.Close();
        }
        return retorno;
    }
    //
    public object[] scalarInt(string query)
    {
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        object[] valor = new object[2] { false, "" };
        try
        {
            int retorno = -10;
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            retorno = Convert.ToInt32(cmd.ExecuteScalar());
            valor[0] = true;
            valor[1] = retorno;

        }
        catch (Exception ex)
        {
            valor[0] = false;
            valor[1] = ex.Message;
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }
    //
}
    