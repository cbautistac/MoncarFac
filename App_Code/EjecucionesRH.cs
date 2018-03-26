using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using E_Utilities;

/// <summary>
/// Descripción breve de EjecucionesRH
/// </summary>
public class EjecucionesRH
{

    SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["RecursosHumanos"].ToString());
    SqlCommand cmd;
    DataSet ds;
    SqlDataAdapter da;
    SqlDataReader dr;
    CatErrores errores = new CatErrores();
    public EjecucionesRH()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public object[] scalarToInt(string sql)
    {
        object[] valor = new object[2] { false, "0" };
        try
        {
            int retorno = 0;
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToInt32(cmd.ExecuteScalar());
            valor[0] = true;
            valor[1] = retorno.ToString();

        }
        catch (Exception x)
        {
            valor[0] = false;
            valor[1] = errores.mensajeError(100);
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }

    public object[] scalarToBool(string sql)
    {
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
        catch (Exception x)
        {
            valor[0] = false;
            valor[1] = errores.mensajeError(101);
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }

    internal object[] scalarToBoolLog(string sql)
    {
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
            valor[1] = errores.mensajeError(101) + " " + ex.Message;
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }

    private bool retornaLogico(int valor)
    {
        if (valor > 0)
            return true;
        else
            return false;
    }

    public object[] dataSet(string sql)
    {
        object[] valor = new object[2] { false, false };
        ds = new DataSet();
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            valor[0] = true;
            valor[1] = ds;

        }
        catch (Exception x)
        {
            valor[0] = false;
            valor[1] = errores.mensajeError(102);
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }

    internal object[] exeStoredOrden(string sql, object[] datosOrden)
    {
        object[] retorno = new object[2] { false, "" };
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idEmpresa", datosOrden[0]);
            cmd.Parameters.AddWithValue("@idTaller", datosOrden[1]);
            cmd.Parameters.AddWithValue("@placa", datosOrden[2]);
            cmd.Parameters.AddWithValue("@tOrden", datosOrden[3]);
            cmd.Parameters.AddWithValue("@cliente", datosOrden[4]);
            cmd.Parameters.AddWithValue("@tServicio", datosOrden[5]);
            cmd.Parameters.AddWithValue("@tValuacion", datosOrden[6]);
            cmd.Parameters.AddWithValue("@tAsegurado", datosOrden[7]);
            cmd.Parameters.AddWithValue("@idEmpleado", datosOrden[8]);
            cmd.Parameters.AddWithValue("@idMarca", datosOrden[9]);
            cmd.Parameters.AddWithValue("@idTvehiculo", datosOrden[10]);
            cmd.Parameters.AddWithValue("@idTUnidad", datosOrden[11]);
            cmd.Parameters.AddWithValue("@idVehiculo", datosOrden[12]);
            cmd.Parameters.AddWithValue("@idUsuario", datosOrden[13]);
            cmd.Parameters.AddWithValue("@localizacion", datosOrden[14]);

            SqlParameter ordenObten = new SqlParameter("@respuesta", 0);
            ordenObten.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ordenObten);
            cmd.ExecuteNonQuery();
            string ordenRetornada = cmd.Parameters["@respuesta"].Value.ToString();
            retorno[0] = true;
            retorno[1] = ordenRetornada;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = errores.mensajeError(104); }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    internal object[] insertUpdateDelete(string sql)
    {
        object[] retorno = new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = errores.mensajeError(103); }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    internal string scalarToStringSimple(string sql)
    {
        string valor = "";
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            valor = Convert.ToString(cmd.ExecuteScalar());
        }
        catch (Exception x)
        {
            valor = "";
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }

    internal object[] insertAdjuntos(string sql, byte[] imagen)
    {
        object[] retorno = new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.Parameters.AddWithValue("imagen", imagen);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = errores.mensajeError(103); }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

}