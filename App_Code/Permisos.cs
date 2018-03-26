using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

/// <summary>
/// Descripción breve de Permisos
/// </summary>
public class Permisos
{
    Ejecuciones ejecuta = new Ejecuciones();
    public bool[] permisos { get; set; }
    public int permiso { get; set; }
    public int idUsuario { get; set; }
    public bool tienePermiso { get; set; }

    public Permisos()
    {

    }

    private int obtienePermisosTotales()
    {
        string sql = "Select count(*) from permisos";
        int retorno = 0;
        try
        {
            object[] datos = ejecuta.scalarToInt(sql);
            if (Convert.ToBoolean(datos[0]))
                retorno = Convert.ToInt32(datos[1]);
            else
                retorno = 0;
        }
        catch (Exception) { retorno = 0; }
        return retorno;
    }

    private void llenaPermisos()
    {
        permisos = new bool[obtienePermisosTotales()];
        for (int i = 0; i < permisos.Length; i++)
        {
            permisos[i] = false;
        }
    }

    public void obtienePermisos()
    {
        llenaPermisos();
        string sql = "Select id_permiso from usuarios_permisos where id_usuario=" + idUsuario.ToString();
        SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["Taller"].ToString());
        try
        {            
            conexionBD.Open();

            SqlCommand cmd = new SqlCommand(sql, conexionBD);
            SqlDataReader lectura = cmd.ExecuteReader();
            while (lectura.Read())
            {
                permisos[lectura.GetInt32(0) - 1] = true;
            }
        }
        catch (Exception)
        {
            llenaPermisos();
        }
        finally {
            conexionBD.Close();
            conexionBD.Dispose();
        }
    }

    public void tienePermisoBuscado() {
        for (int i = 0; i < permisos.Length; i++)
        {
            if (permiso - 1 == i)
            {
                tienePermiso = permisos[i];
                break;
            }
        }
    }

    public void tienePermisoIndicado() {
        string sql = "Select count(*) from usuarios_permisos where id_usuario=" + idUsuario.ToString() + " and id_permiso=" + permiso.ToString();
        int retorno = 0;
        try
        {
            object[] datos = ejecuta.scalarToInt(sql);
            if (Convert.ToBoolean(datos[0]))
            {
                retorno = Convert.ToInt32(datos[1]);
                if (retorno != 0)
                    tienePermiso = true;
                else
                    tienePermiso = false;
            }
            else
            {                
                tienePermiso = false;
            }
        }
        catch (Exception) {tienePermiso = false; }
    }
}