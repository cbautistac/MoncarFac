<%@ WebHandler Language="C#" Class="LogoEmpresas" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class LogoEmpresas : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Clear();
        if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            string id = context.Request.QueryString["id"].ToString();
            Image imagen = GetImagen(id);
            context.Response.ContentType = "image/png";
            if (imagen != null)
                imagen.Save(context.Response.OutputStream, ImageFormat.Png);
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("&nbsp;");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private Image GetImagen(string id)
    {
        Image logo = null;
        MemoryStream memoryStream = new MemoryStream();
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Taller"].ToString();
        string sql = "select logo from Empresas where id_empresa=" + id;
        try
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader lectura = cmd.ExecuteReader();
            if (lectura.HasRows)
            {
                lectura.Read();
                byte[] imagenPerfil = (byte[])lectura[0];
                memoryStream = new MemoryStream(imagenPerfil, false);
            }
        }
        catch (Exception x)
        {

        }
        finally
        {
            conexion.Dispose();
            conexion.Close();
        }
        try
        {
            logo = Image.FromStream(memoryStream);
        }
        catch (Exception x)
        {
            logo = null;
        }
        return logo;
    }

}