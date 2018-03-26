using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SqlServer.Server;
using E_Utilities;
/// <summary>
/// Descripción breve de BaseDatos
/// </summary>
public class BaseDatos
{
    public SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
    SqlCommand cmd;
	public BaseDatos()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] intToBool(string sql) {
        object[] valor = new object[2] { false, false };
        try
        {
            int retorno = 0;
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToInt32(cmd.ExecuteScalar());
            valor[0] = true;
            valor[1] = retornaLogico(retorno);

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

    private object retornaLogico(int retorno)
    {
        if (retorno > 0)
            return true;
        else
            return false;
    }

    internal object[] scalarString(string sql)
    {
        object[] valor = new object[2] { false, "" };
        try
        {
            string retorno = "";
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToString(cmd.ExecuteScalar());
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

    public object[] insertUpdateDelete(string query)
    {
        object[] retorno = new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception ex) { retorno[0] = false; retorno[1] = ex.Message; }
        finally
        {
            conexionBD.Close();
        }
        return retorno;
    }

    public object[] scalarInt(string query)
    {
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

    public object[] scalarToDecimal(string sql)
    {
        object[] valor = new object[2] { false, 0 };
        try
        {
            decimal retorno = 0;
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToDecimal(cmd.ExecuteScalar());
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

    private string convierteMontos(string importe)
    {
        if (importe != "")
        {
            importe = importe.Replace('$', ',');
            importe = importe.Replace(',', ' ');
            string valor = "";
            for (int j = 0; j < importe.Length; j++)
            {
                if (char.IsDigit(importe[j]))
                    valor = valor.Trim() + importe[j];
                else
                {
                    if (importe[j] == '.')
                        valor = valor.Trim() + importe[j];
                }
            }
            importe = valor.Trim();
        }
        else
            importe = "0";
        return importe;
    }

    public object[] scalarData(string sql)
    {
        object[] retorno = new object[2];
        try
        {
            DataSet datas = new DataSet();
            cmd = new SqlCommand(sql, conexionBD);
            SqlDataAdapter sdata = new SqlDataAdapter(cmd);
            sdata.Fill(datas);
            retorno[0] = true;
            retorno[1] = datas;
        }
        catch (Exception ex)
        {
            retorno[0] = false;
            retorno[1] = ex.Message;
        }
        return retorno;
    }

    public object[] generaCaja(int _pv, string _usuario, DateTime fecha, string _acceso, string sql)
    {
        object[] valor = new object[2] { false, "" };        
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;            
            cmd.Parameters.AddWithValue("@idPunto", _pv).SqlDbType = SqlDbType.SmallInt;                        
            cmd.Parameters.AddWithValue("@usuario", _usuario).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@acceso", Convert.ToChar(_acceso)).SqlDbType = SqlDbType.Char;
            cmd.Parameters.AddWithValue("@fecha", fecha).SqlDbType = SqlDbType.DateTime;
            cmd.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            int caja = Convert.ToInt32(cmd.Parameters["@respuesta"].Value);

            valor[0] = true;
            valor[1] = caja;

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

    public object[] generaMovimiento(int _pv, string _usuario, DateTime fecha, string _acceso, string estatus, string sql)
    {
        object[] valor = new object[2] { false, "" };
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPunto", _pv).SqlDbType = SqlDbType.SmallInt;
            cmd.Parameters.AddWithValue("@usuario", _usuario).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@acceso", Convert.ToChar(_acceso)).SqlDbType = SqlDbType.Char;
            cmd.Parameters.AddWithValue("@estatus", Convert.ToChar(estatus)).SqlDbType = SqlDbType.Char;
            cmd.Parameters.AddWithValue("@fecha", fecha).SqlDbType = SqlDbType.DateTime;
            cmd.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();            
            valor[0] = true;
            if (cmd.Parameters["@respuesta"].Value.ToString() == "")
                valor[1] = true;
            else
                valor[1] = cmd.Parameters["@respuesta"].Value.ToString();
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

    internal object[] generaCierre(int _punto, string _usuario, string _acceso, string estatus, int _caja, DateTime _fecha, string sql)
    {
        object[] valor = new object[2] { false, "" };
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPunto", _punto).SqlDbType = SqlDbType.SmallInt;
            cmd.Parameters.AddWithValue("@usuario", _usuario).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@acceso", Convert.ToChar(_acceso)).SqlDbType = SqlDbType.Char;
            cmd.Parameters.AddWithValue("@estatus", Convert.ToChar(estatus)).SqlDbType = SqlDbType.Char;
            cmd.Parameters.AddWithValue("@fecha", _fecha).SqlDbType = SqlDbType.DateTime;
            cmd.Parameters.AddWithValue("@caja", _caja).SqlDbType = SqlDbType.Int;
            cmd.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            valor[0] = true;
            if (cmd.Parameters["@respuesta"].Value.ToString() == "")
                valor[1] = true;
            else
                valor[1] = cmd.Parameters["@respuesta"].Value.ToString();
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

    internal object[] generaCierreDia(int _punto, string _usuario, int _caja, DateTime _fecha, string sql)
    {
        object[] valor = new object[2] { false, "" };
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPunto", _punto).SqlDbType = SqlDbType.SmallInt;
            cmd.Parameters.AddWithValue("@usuario", _usuario).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@fecha", _fecha).SqlDbType = SqlDbType.DateTime;
            cmd.Parameters.AddWithValue("@caja", _caja).SqlDbType = SqlDbType.Int;
            cmd.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            valor[0] = true;
            if (cmd.Parameters["@respuesta"].Value.ToString() == "")
                valor[1] = true;
            else
                valor[1] = cmd.Parameters["@respuesta"].Value.ToString();
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

    internal object[] scalarToInt(string query)
    {
        object[] valor = new object[2] { false, 0 };
        try
        {
            int retorno = 0;
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

    public int obtieneDetalleEntradaInv(int idFolioInventEntrada, int entDetID, string idProducto, int isla, int cantidad)
    {
        int retorno = 0;
        try
        {
            string sql = "eliminaDetalleEntrada";
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@entFolioID", idFolioInventEntrada).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@entDetID", entDetID).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@idArticulo", idProducto).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@idAlmacen", isla).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@inventarioDescuento", cantidad).SqlDbType = SqlDbType.Decimal;
            cmd.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            if (cmd.Parameters["@respuesta"].Value.ToString() == "1")
                return 1;
            else
            {
                try { int sinError = Convert.ToInt32(cmd.Parameters["@respuesta"].Value.ToString()); }
                catch (Exception ex) { string error = cmd.Parameters["@respuesta"].Value.ToString(); }
                return 0;
            }
        }
        catch (Exception ex)
        {

        }
        return retorno;
    }
    /*
    // db Accecom, sp GeneraVenta
    internal object[] generaTicket(object[] ticket, string sql)
    {
        object[] valor = new object[2] { false, "" };
        List<Venta> articulos = new List<Venta>();
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            articulos = (List<Venta>)ticket[14];


            cmd.Parameters.AddWithValue("@idPunto", Convert.ToInt16(ticket[0])).SqlDbType = SqlDbType.SmallInt;
            cmd.Parameters.AddWithValue("@idAlmacen", Convert.ToInt16(ticket[1])).SqlDbType = SqlDbType.SmallInt;
            cmd.Parameters.AddWithValue("@caja", Convert.ToInt16(ticket[2])).SqlDbType = SqlDbType.SmallInt;
            cmd.Parameters.AddWithValue("@usuario", Convert.ToString(ticket[3])).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@subtotal", Convert.ToDecimal(ticket[4])).SqlDbType = SqlDbType.Decimal;
            cmd.Parameters.AddWithValue("@iva", Convert.ToDecimal(ticket[5])).SqlDbType = SqlDbType.Decimal;
            cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(ticket[6])).SqlDbType = SqlDbType.Decimal;
            cmd.Parameters.AddWithValue("@porcIva", Convert.ToDecimal(ticket[7])).SqlDbType = SqlDbType.Decimal;
            cmd.Parameters.AddWithValue("@fechaVenta", Convert.ToDateTime(ticket[8])).SqlDbType = SqlDbType.DateTime;
            cmd.Parameters.AddWithValue("@formaPago", Convert.ToChar(ticket[10])).SqlDbType = SqlDbType.Char;
            cmd.Parameters.AddWithValue("@referenciaPago", Convert.ToString(ticket[11])).SqlDbType = SqlDbType.Char;
            cmd.Parameters.AddWithValue("@banco", Convert.ToString(ticket[12])).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@notas", Convert.ToString(ticket[13])).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@tarjeta", Convert.ToString(ticket[15])).SqlDbType = SqlDbType.Char;
            DataTable dt = new DataTable();
            dt.Columns.Add("renglon");
            dt.Columns.Add("id_refaccion");
            dt.Columns.Add("descripcion");
            dt.Columns.Add("cantidad");
            dt.Columns.Add("venta_unitaria");
            dt.Columns.Add("importe");

            foreach (Venta articulo in articulos)
            {

                object[] valores = { Convert.ToInt16(articulo.renglon), articulo.clave, articulo.producto, Convert.ToInt16(articulo.cantidad), Convert.ToDecimal(convierteMontos(articulo.precio)), Convert.ToDecimal(convierteMontos(articulo.total)) };
                dt.Rows.Add(valores);
            }


            cmd.Parameters.AddWithValue("@detalleVenta", dt);

            cmd.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            int ticketGen = Convert.ToInt32(cmd.Parameters["@respuesta"].Value);

            valor[0] = true;
            valor[1] = ticketGen;

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


    internal object[] generaOrden(string sql, string[] argmentosBasicos, List<OrdenCompra> list)
    {
        object[] valor = new object[2] { false, "" };
        List<OrdenCompra> ordenesDet = new List<OrdenCompra>();
        conexionBD.Open();
        cmd = new SqlCommand(sql, conexionBD);
        cmd.CommandType = CommandType.StoredProcedure;
        ordenesDet = (List<OrdenCompra>)list;

        cmd.Parameters.AddWithValue("@idAlmacen", Convert.ToInt16(argmentosBasicos[0])).SqlDbType = SqlDbType.SmallInt;
        cmd.Parameters.AddWithValue("@usuario", Convert.ToString(argmentosBasicos[1])).SqlDbType = SqlDbType.VarChar;
        cmd.Parameters.AddWithValue("@fecha", Convert.ToDateTime(argmentosBasicos[2])).SqlDbType = SqlDbType.Date;
        cmd.Parameters.AddWithValue("@hora", Convert.ToDateTime(argmentosBasicos[3])).SqlDbType = SqlDbType.DateTime;
        DataTable dt = new DataTable();
        dt.Columns.Add("no_detalle");
        dt.Columns.Add("solicitud");

        foreach (OrdenCompra articulo in ordenesDet)
        {
            object[] valores = { Convert.ToInt32(articulo.renglon), articulo.texto };
            dt.Rows.Add(valores);
        }

        cmd.Parameters.AddWithValue("@detalleOrden", dt);
        cmd.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
        try { cmd.ExecuteNonQuery(); }
        catch (Exception ex) { }
        int ticketGen = 0;
        string respuesta = "";
        try
        {
            ticketGen = Convert.ToInt32(cmd.Parameters["@respuesta"].Value);
            valor[0] = true;
            valor[1] = ticketGen;
        }
        catch (Exception ex)
        {
            respuesta = cmd.Parameters["@respuesta"].Value.ToString();
            valor[0] = true;
            valor[1] = respuesta + " " + ex.Message;
        }
        return valor;
    }
    */        
}