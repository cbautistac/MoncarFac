using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de BitPerfiles
/// </summary>
public class BitPerfiles
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    string sql;
    object[] ejec = new object[2];
    public BitPerfiles()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public DataSet llenaPerfilesList(int idEmpresa, int idTaller, int[] perfiles)
    {
        DataSet data = null;
        for(int conta =0; conta <perfiles.Length; conta++)
        {
            if (conta < perfiles.Length-1)
            {
                sql += "select ('" + perfiles[conta].ToString() + "')as id_perfilOrden, " +
                    "(select descripcion from perfilesordenes where id_perfilOrden = " + perfiles[conta].ToString() + ") as perfil, " +
                    "(select count(id_perfilOrden) as vehiculos " +
                    "from Ordenes_Reparacion " +
                    "where status_orden = 'A' and id_perfilOrden = " + perfiles[conta].ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + ")as vehiculos " +
                    "union all ";
            }
            else
            {
                sql += "select ('" + perfiles[conta].ToString() + "')as id_perfilOrden, " +
                    "(select descripcion from perfilesordenes where id_perfilOrden = " + perfiles[conta].ToString() + ") as perfil, " +
                    "(select count(id_perfilOrden) as vehiculos " +
                    "from Ordenes_Reparacion " +
                    "where status_orden = 'A' and id_perfilOrden = " + perfiles[conta].ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + ")as vehiculos ";
            }
        }
        ejec = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(ejec[0]))
            data = (DataSet)ejec[1];
        return data;
    }

    public DataSet obtieneAseguradorasPerfil(int idEmpresa, int idTaller, int perfil)
    {
        DataSet data = null;
        sql = "select orp.id_perfilOrden,orp.id_cliprov,c.razon_social as cliente,count(orp.id_cliprov) as total,c.rgb_r,c.rgb_g,c.rgb_b" +
              " from Ordenes_Reparacion orp" +
              " inner join cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = 'C'" +
              " inner join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden" +
              " where orp.status_orden='A' and orp.id_perfilOrden = " + perfil.ToString() + " and orp.id_empresa = " + idEmpresa.ToString() + " and orp.id_taller = " + idTaller.ToString() +
              " group by orp.id_cliprov,c.razon_social,c.rgb_r,c.rgb_g,c.rgb_b,orp.id_perfilOrden";
        ejec = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(ejec[0]))
            data = (DataSet)ejec[1];
        return data;
    }
}