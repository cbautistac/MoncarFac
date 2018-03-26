using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using E_Utilities;

/// <summary>
/// Descripción breve de Envio_Mail
/// </summary>
public class Envio_Mail
{
    Ejecuciones ejecuta = new Ejecuciones();
	public Envio_Mail()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public object[] obtieneDatosServidor(string usuariocorreo, string correo, string mensaje ,string contraseña, string asunto, ListBox adjuntos, int empresa, string CC, string CCO)
    {
        object[] enviado = new object[] { false, "" };
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Taller"].ToString();
        string sql = "select isnull(count(*),0) from parametros_email where clave=" + empresa;
        object[] valores = ejecuta.scalarToInt(sql);
        bool existe = Convert.ToBoolean(valores[0]);
        if (existe)
        {
            int parametros = Convert.ToInt32(valores[1]);
            if (parametros != 0)
            {
                if (parametros == 1)
                    sql = "select * from Parametros_Email where clave=" + empresa;
                else if (parametros > 1)
                    sql = "select * from Parametros_Email where clave=" + empresa;
                try
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(sql, conexion);
                    SqlDataReader lectura = cmd.ExecuteReader();
                    string usuariohost = "", contrasena = "", host = "";
                    int puerto = 0;
                    int ssl = 0;
                    object[] param = new object[5];
                    while (lectura.Read())
                    {
                        //perfil = lectura.GetInt16(0);
                        /*usuariohost = lectura.GetString(1);
                        contrasena = lectura.GetString(2);
                        host = lectura.GetString(3);
                        puerto = lectura.GetInt32(4);
                        ssl = lectura.GetInt16(6);*/
                        param[0] = lectura.GetValue(1);
                        param[1] = lectura.GetValue(2);
                        param[2] = lectura.GetValue(3);
                        param[3] = lectura.GetValue(4);
                        param[4] = lectura.GetValue(6);
                    }
                    usuariohost = param[0].ToString();
                    contrasena = param[1].ToString();
                    host = param[2].ToString();
                    puerto = Convert.ToInt32(param[3]);
                    ssl = Convert.ToInt32(param[4]);
                    enviado = EnviarCorreo(usuariocorreo, correo, usuariohost, contrasena, puerto, ssl, host, mensaje, contraseña, asunto, adjuntos, CC, CCO);
                }
                catch (Exception x)
                {
                    enviado[0] = false;
                    enviado[1] = x.Message;
                }
            }
            else
            {
                enviado[0] = false;
                enviado[1] = "No existen parametros definidos para enviar correos";
            }
        }
        else
        {
            enviado[0] = false;
            enviado[1] = "No existen parametros definidos para enviar correos";
        }
        conexion.Dispose();
        conexion.Close();
        return enviado;
    }

    private object[] EnviarCorreo(string usuariocorreo, string correo, string usuariohost, string contrasena, int puerto, int ssl, string host, string mensaje, string contraseña, string asunto, ListBox adjuntos, string CC, string CCO)
    {
        object[] envio = new object[] { false, "" };
        /*-------------------------MENSAJE DE CORREO----------------------*/
        //Creamos un nuevo Objeto de mensaje
        System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
        //Direccion de correo electronico a la que queremos enviar el mensaje
        mmsg.To.Add(correo);
        //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario
        //Asunto
        string usuario = usuariocorreo;
        mmsg.Subject = asunto;// "Asunto del correo";
        mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
        if (CC!="")
            mmsg.CC.Add(CC);
        if (CCO != "")
            mmsg.Bcc.Add(CCO);
        //Cuerpo del Mensaje
        mmsg.Body = mensaje;//Texto del contenio del mensaje de correo
        if (adjuntos != null)
        {
            //Adjuntos
            foreach (ListItem l in adjuntos.Items)
            {
                mmsg.Attachments.Add(new System.Net.Mail.Attachment(l.Value));
            }
        }


        mmsg.BodyEncoding = System.Text.Encoding.UTF8;
        mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML
        //Correo electronico desde la que enviamos el mensaje
        mmsg.From = new System.Net.Mail.MailAddress(usuariohost);//"juan@formulasistemas.com");//"micuenta@servidordominio.com");
        /*-------------------------CLIENTE DE CORREO----------------------*/
        //Creamos un objeto de cliente de correo
        System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
        //Hay que crear las credenciales del correo emisor
        cliente.Credentials = new System.Net.NetworkCredential(usuariohost, contrasena);//"juan@formulasistemas.com", "juanFS2014");//"micuenta@servidordominio.com", "micontraseña");
        //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
        ///*
        cliente.Port = puerto;
        bool ssl_obtenido = false;
        if (ssl == 1)
            ssl_obtenido = true;
        cliente.EnableSsl = ssl_obtenido;
        //*/

        cliente.Host = host; //"mail.formulasistemas.com";// "mail.servidordominio.com"; //Para Gmail "smtp.gmail.com";


        /*-------------------------ENVIO DE CORREO----------------------*/

        try
        {
            //Enviamos el mensaje      
            cliente.Send(mmsg);
            envio[0] = true;
            envio[1] = "";
        }
        catch (System.Net.Mail.SmtpException ex)
        {
            envio[0] = false;//Aquí gestionamos los errores al intentar enviar el correo
            envio[1] = ex.Message;
        }
        return envio;
    }

    public object[] obtieneParametros(int clave) {        
        object[] retorno = new object[2] { false, "" };
        string sql = "select * from parametros_email where clave=" + clave;
        return retorno = ejecuta.dataSet(sql);                
    }

}