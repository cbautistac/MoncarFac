using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Recupera_Contrasena : System.Web.UI.Page
{
    Envio_Mail datosCorreo = new Envio_Mail();
    CatUsuarios datosUsuarios = new CatUsuarios();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErrores.Text = "";
        lblParametros.Text = "";
        int parametros = datosUsuarios.existenParametros();
        if (parametros > 0)
            lblParametros.Visible = false;
        else
        {
            lblParametros.Text = "No se han ingresado parámetros para mandar su contraseña por correo, contacte al administrador e informele de su situación";
            PanelContraseña.Visible = false;
            lblParametros.CssClass = "centrado";
        }
    }

    protected void btnAceptar_Click(object sender, ImageClickEventArgs e)
    {
        lblErrores.Text = "";
        string usuario = txtNick.Text;
        string correo = txtCorreo.Text;
        object[] valores = datosUsuarios.existeUsuario(usuario);
        bool existe = Convert.ToBoolean(valores[0]);
        if (existe)
        {
            existe = Convert.ToBoolean(valores[1]);
            if (existe)
            {
                string contraseña = datosUsuarios.obtieneContraseña(usuario);
                object[] enviado = new object[] { false, "" };
                valores = datosUsuarios.existeCorreo(usuario, correo);
                existe = Convert.ToBoolean(valores[0]);
                if (existe)
                {
                    int correos = Convert.ToInt32(valores[1]);
                    if (correos==1)
                    {
                        enviado = datosCorreo.obtieneDatosServidor(usuario, correo, "", contraseña,"Recuperación de Contraseña",null,1, "", "");
                        if (Convert.ToBoolean(enviado[0]))
                            lblErrores.Text = "La contraseña fue enviada exitosamente al correo " + correo + " del Usuario " + usuario;
                        else
                            lblErrores.Text = "El envio no se logro exitosamente, verifique su conexión e intentelo nuevamente mas tarde. Detalle: " + Convert.ToString(enviado[1]);
                        lblSeleccione.Visible = false;
                        btnAceptarUsuarios.Visible = false;
                        ddlUsuarios.Visible = false;
                    }
                    else if (correos == 0)
                    {
                        string mensaje = "El correo no corresponde con el registrado, si usted no es el propietario de la cuenta evite hacer mal uso del sistema";
                        enviado = datosCorreo.obtieneDatosServidor(usuario, correo, mensaje, contraseña,"Recuperación de Contraseña",null,1, "", "");
                        if (Convert.ToBoolean(enviado[0]))
                            lblErrores.Text = "La contraseña fue enviada exitosamente al correo " + correo + " del Usuario " + usuario;
                        else
                            lblErrores.Text = "El envio no se logro exitosamente, verifique su conexión e intentelo nuevamente mas tarde. Detalle: " + Convert.ToString(enviado[1]);
                        lblSeleccione.Visible = false;
                        btnAceptarUsuarios.Visible = false;
                        ddlUsuarios.Visible = false;
                    }
                    else if (correos>1)
                    {
                        lblSeleccione.Visible = true;
                        btnAceptarUsuarios.Visible = true;
                        ddlUsuarios.Visible = true;
                        DataSet data = datosUsuarios.obtieneUsuarios(correo);
                        try { ddlUsuarios.DataSource = data; }
                        catch (Exception) { data = null; }
                        ddlUsuarios.DataSource = data;
                        ddlUsuarios.DataValueField = "nick";
                        ddlUsuarios.DataTextField = "nick";
                        ddlUsuarios.DataBind();
                        ddlUsuarios.Items.Insert(0, new ListItem("Seleccione un Usuario", ""));
                    }
                }
                else
                    lblErrores.Text = "La recuperación no se logro correctamete, verifique su conexión e intentelo nuevamente mas tarde";
            }
            else
                lblErrores.Text = "El usuario que ingreso no existe, verifique sus datos e intentelo nuevamente";
        }
        else
            lblErrores.Text = "La recuperación no se logro correctamete, verifique su conexión e intentelo nuevamente mas tarde";
    }

    protected void btnAceptarUsuarios_Click(object sender, ImageClickEventArgs e)
    {
        lblErrores.Text = "";
        string correo = txtCorreo.Text;
        string usuario = ddlUsuarios.SelectedValue;
        string contraseña = datosUsuarios.obtieneContraseña(usuario);
        object[] enviado = new object[] { false, "" };
        object[] valores = datosUsuarios.existeCorreo(usuario, correo);
        enviado = datosCorreo.obtieneDatosServidor(usuario, correo, "", contraseña, "Recuperación de Contraseña",null,1, "", "");
        if (Convert.ToBoolean(enviado[0]))
            lblErrores.Text = "La contraseña fue enviada exitosamente al correo " + correo + " del Usuario " + usuario;
        else
            lblErrores.Text = "El envio no se logro exitosamente, verifique su conexión e intentelo nuevamente mas tarde. Detalle: " + Convert.ToString(enviado[1]);
        lblSeleccione.Visible = false;
        btnAceptarUsuarios.Visible = false;
        ddlUsuarios.Visible = false;
    }
}