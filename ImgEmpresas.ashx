﻿<%@ WebHandler Language="C#" Class="ImgEmpresas" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class ImgEmpresas : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            string[] datos = context.Request.QueryString["id"].ToString().Split(';');
            int idEmpresaI = Convert.ToInt32(datos[0]);
            int idTallerI = Convert.ToInt32(datos[1]);
            int consecutivoI = Convert.ToInt32(datos[3]);
            int noOrdenI = Convert.ToInt32(datos[2]);
            int procesoI = Convert.ToInt32(datos[4]);
            Image imagen = GetImagen(idEmpresaI,idTallerI,consecutivoI,noOrdenI,procesoI);
            context.Response.ContentType = "image/jpeg";
            if (imagen != null)
                imagen.Save(context.Response.OutputStream, ImageFormat.Jpeg);
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("&nbsp;");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private Image GetImagen(int idEmpresa,int idTaller,int consecutivo,int noOrden,int proceso)
    {
        Image logo = null;
        MemoryStream memoryStream = new MemoryStream();
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Taller"].ToString();
        string sql = "select imagen from Fotografias_Orden where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " and consecutivo=" + consecutivo.ToString() + " and proceso=" + proceso.ToString();
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
        catch (Exception x )
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
        catch (Exception x )
        {
            logo = null;
        }
        return logo;
    }
}