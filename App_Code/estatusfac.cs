using E_Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System;
using System.Data.SqlClient;
using System.Configuration;

public class estatusfac
{

    string sql;
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    object[] ejecutado = new object[2];
    public string estatus { get; set; }
    public string fechaini { get; set; }
    public string fechafin { get; set; }

    public estatusfac()
    {
        estatus = fechafin = fechaini = string.Empty;
    }


    internal object[] obtieneestatusfactur()
    {
        string condicion = "";
        if (estatus != "")
            //condicion = " where encestatus = '" + estatus + "' ";
            condicion = " where encestatus in (" + estatus + ") ";
        string sql = string.Format("select e.idCfd,e.EncFolioUUID,e.EncReferencia,convert(char(10),e.EncFechaGenera,120) as EncFechaGenera,e.EncReRFC,case when e.EncReNombre is null then (select renombre from receptores_f where idrecep=e.idrecep) when e.encrenombre='' then (select renombre from receptores_f where idrecep=e.idrecep) else e.encrenombre end as EncReNombre,e.EncEstatus, " +
"case e.encestatus when 'P' then 'En Captura' when 'E' then 'En Tránsito' when 'T' then 'Timbrado' when 'R' then 'Rechazado' when 'C' then 'Cancelado'	 else 'Otro' end as est from EncCFD_f e where encfecha between '{0}' and '{1}' order by e.idcfd desc ", fechaini, fechafin);
        SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        DataSet ds = new DataSet();
        object[] valor = new object[2] { false, "" };
        try
        {
            conexionBD.Open();
            SqlCommand cmd = new SqlCommand(sql, conexionBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            valor[0] = true;
            valor[1] = ds;

        }
        catch (Exception x)
        {
            valor[0] = false;
            valor[1] = x.Message;
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }
    
}