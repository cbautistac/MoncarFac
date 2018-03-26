using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    int usuario, perfil, taller, empresa;
    DataSet talleres;
    Fechas fechas = new Fechas();
    CatUsuarios usuarios = new CatUsuarios();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            pnlperfiles.Visible = false;
            pnlTalleres.Visible = false;
            mask.Visible = false;
            usuario = 0;
            perfil = 0;
            taller = 0;
            empresa = 0;
            lblversion.Text= ConfigurationManager.AppSettings["version"].ToString();
            Session["user"] = Session["perfil"] = Session["empresa"] = Session["taller"] = null;
        }
        else if (Page.Request.Params["__EVENTTARGET"] == "cierraSesion")
        {
            int usu = int.Parse(Page.Request.Params["__EVENTARGUMENT"].ToString());
            cierraSesionUsu(usu);
        }
    }

    private void checaTalleresUsuario(DataSet talleres)
    {
        if (talleres.Tables[0].Rows.Count > 1)
        {
            pnlTalleres.Visible = true;
            if (!mask.Visible)
            {
                mask.Visible = true;
            }
        }
        else {
            pnlperfiles.Visible = false;
            pnlTalleres.Visible = false;
            mask.Visible = false;
            foreach (DataRow fila in talleres.Tables[0].Rows) {
                empresa = Convert.ToInt32(fila[0]);
                taller = Convert.ToInt32(fila[1]);
            }
            if (taller != 0)
            {
                llamaPagina(perfil);
            }
            else
                lblErrorLog.Text = "El usuario que acaba de ingresar no cuenta con algun taller asignado, contacte al administrador para que le otorgue el o los necesarios para sus labores";
        }
    }

    private DataSet obtieneTalleresUsuario(int usuario)
    {
        DataSet retorno = new DataSet();
        object[] valores = new object[2];
        valores = usuarios.obtieneTalleresUsuario(usuario);
        if (Convert.ToBoolean(valores[0]))
        {
            retorno = (DataSet)valores[1];
        }
        else
        {
            lblErrorLog.Text = valores[1].ToString();
            retorno = null;          
        }
        return retorno;
    }
    
    private void llamaPagina(int perfil)
    {
        if (usuario == 0)
        {
            object[] idUsuario = usuarios.obtieneIdUsuario(txtUsuarioLog.Text);
            if (Convert.ToBoolean(idUsuario[0]))
            {
                usuario = Convert.ToInt32(idUsuario[1]);
                lblUsuario.Text = usuario.ToString();
            }
        }
        if (usuario != 0)
        {
            Session["user"] = usuario;
            Session["perfil"] = perfil;
            Response.Redirect("Ordenes.aspx?u=" + usuario + "&p=" + perfil + "&e=" + "1" + "&t=" + "1");
        }
    }

    protected void lnkRecPas_Click(object sender, EventArgs e)
    {
        Response.Redirect("Recupera_Contrasena.aspx");
    }

    protected void btnLog1_Click(object sender, EventArgs e)
    {
        lblErrorLog.Text = "";

        object[] verificaUsuario = usuarios.existeUsuarioLog(txtUsuarioLog.Text);
        if (Convert.ToBoolean(verificaUsuario[0]))
        {
            if (Convert.ToBoolean(verificaUsuario[1]))
            {
                object[] userValido = usuarios.validaUsuarioLog(txtUsuarioLog.Text, txtContraseñaLog.Text);
                if (Convert.ToBoolean(userValido[0]))
                {
                    if (Convert.ToBoolean(userValido[1]))
                    {
                        object[] idUsuario = usuarios.obtieneIdUsuario(txtUsuarioLog.Text);
                        if (Convert.ToBoolean(idUsuario[0]))
                        {
                            usuario = Convert.ToInt32(idUsuario[1]);
                            string statAccs = usuarios.obtieneEstatusAcceso(usuario);
                        if (statAccs != "A" || usuario == 1)
                        {
                            object[] actualizaAcceso = usuarios.actualizaBitacoraAcceso(usuario, "A");
                            if (Convert.ToBoolean(actualizaAcceso[0]))
                            {
                                if (Convert.ToBoolean(actualizaAcceso[1]))
                                {
                                    if (usuario == 1)
                                    {
                                        Session["user"] = usuario;
                                        Session["perfil"] = 1;
                                        Session["empresa"] = empresa;
                                        Session["taller"] = taller;
                                        Response.Redirect("BienvenidaAdmon.aspx");
                                    }
                                    else
                                    {
                                        lblUsuario.Text = usuario.ToString();
                                        object[] perfilesUsuario = usuarios.obtienePerfiles(usuario);
                                        if (Convert.ToBoolean(perfilesUsuario[0]))
                                        {
                                            DataSet info = (DataSet)perfilesUsuario[1];
                                            talleres = new DataSet();
                                            
                                                perfil = 0;
                                                // brinca a usuarios taller
                                                foreach (DataRow fila in info.Tables[0].Rows)
                                                {
                                                    perfil = Convert.ToInt32(fila[0].ToString());
                                                    lblArrastrePerfil.Text = perfil.ToString();
                                                }
                                                if (perfil != 0 & perfil != 1)
                                                {
                                                    llamaPagina(2);
                                                }
                                                else if (perfil != 0 & perfil == 1)
                                                {
                                                    llamaPagina(1);
                                                }
                                                else
                                                    lblErrorLog.Text = "El usuario que acaba de ingresar no cuenta con algun perfil, contacte al administrador para que le otorgue los necesarios para sus labores";
                                            
                                        }
                                        else
                                            lblErrorLog.Text = perfilesUsuario[1].ToString();
                                    }
                                }
                                else
                                    lblErrorLog.Text = "Ocurrio un error inesperado: " + actualizaAcceso[1].ToString();
                            }
                            else
                                lblErrorLog.Text = "Ocurrio un error inesperado: " + idUsuario[1].ToString();
                        }
                        else {
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "jsCierraSes", "cierraSesion(" + usuario + ");", true);
                            UpdatePanel1.Update();
                        }
                    }
                        else
                            lblErrorLog.Text = idUsuario[1].ToString();
                    }
                    else
                        lblErrorLog.Text = "El Usuario y/o Contraseña no son correctos.";
                }
                else
                    lblErrorLog.Text = userValido[1].ToString();
            }
            else
                lblErrorLog.Text = "El Usuario no existe.";
        }
        else
            lblErrorLog.Text = verificaUsuario[1].ToString();
    }
    
    protected void btnAceptarPerfil_Click1(object sender, EventArgs e)
    {
        pnlperfiles.Visible = false;
        perfil = Convert.ToInt32(rdbPerfil.SelectedValue);
        lblArrastrePerfil.Text = perfil.ToString();
        if (usuario == 0)
        {
            object[] idUsuario = usuarios.obtieneIdUsuario(txtUsuarioLog.Text);
            if (Convert.ToBoolean(idUsuario[0]))
            {
                usuario = Convert.ToInt32(idUsuario[1]);
                lblUsuario.Text = usuario.ToString();
            }
        }
        if (perfil == 1)
        {
            Session["user"] = usuario;
            Session["perfil"] = perfil;
            Session["empresa"] = empresa;
            Session["taller"] = taller;
            Response.Redirect("~/BienvenidaAdmon.aspx", true);
        }
        else
        {
            if (talleres == null)
            {
                talleres = obtieneTalleresUsuario(usuario);
            }
            else
            {
                if (talleres.Tables[0].Rows.Count == 0)
                    talleres = obtieneTalleresUsuario(usuario);
            }
            checaTalleresUsuario(talleres);
        }
        
    }
    protected void btnCancelarPerfil_Click1(object sender, EventArgs e)
    {
        object[] idUsuario = usuarios.obtieneIdUsuario(txtUsuarioLog.Text);
        if (Convert.ToBoolean(idUsuario[0]))
        {
            usuario = Convert.ToInt32(idUsuario[1]);
        }
        object[] actualizaAcceso = usuarios.actualizaBitacoraAcceso(usuario, "I");
        pnlTalleres.Visible = false;
        pnlperfiles.Visible = false;
        mask.Visible = false;
    }
    protected void btnAceptarTaller_Click1(object sender, EventArgs e)
    {
        empresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
        taller = Convert.ToInt32(ddlTaller.SelectedValue);
        pnlTalleres.Visible = false;
        mask.Visible = false;
        perfil = Convert.ToInt32(lblArrastrePerfil.Text);
        llamaPagina(perfil);
    }
    protected void btnCancelarTaller_Click1(object sender, EventArgs e)
    {
        object[] idUsuario = usuarios.obtieneIdUsuario(txtUsuarioLog.Text);
        if (Convert.ToBoolean(idUsuario[0]))
        {
            usuario = Convert.ToInt32(idUsuario[1]);
        }
        object[] actualizaAcceso = usuarios.actualizaBitacoraAcceso(usuario, "I");
        pnlTalleres.Visible = false;
        pnlperfiles.Visible = false;
        mask.Visible = false;
    }

    private void cumpleaños()
    {
        int cumpleañeros = usuarios.obtieneCumpleañeros();
        if (cumpleañeros > 0)
        {
            GridCumpleañeros.DataBind();
            PanelCumple.Visible = true;
            PanelMaskCumple.Visible = true;
        }
        else
        {
            PanelCumple.Visible = false;
            PanelMaskCumple.Visible = false;
        }
    }

    protected void btnSalirCumple_Click(object sender, EventArgs e)
    {
        PanelCumple.Visible = false;
        PanelMaskCumple.Visible = false;
    }

    private void notifica(int idEmpresa, int idTaller, int idUsuario)
    {
        try
        {
            DateTime fechaHoy = fechas.obtieneFechaLocal();
            object[] ordenesArr = usuarios.obtieneDiasFechasEntrega(idEmpresa, idTaller);
            if ((bool)ordenesArr[0])
            {
                int ordenes = Convert.ToInt32(ordenesArr[1]);
                if (ordenes != 0)
                    generanotificacion(idEmpresa, idTaller, idUsuario, ordenes.ToString(), 11);
                //meter refacciones con fecha d entrega caducas
                //notificar
                ordenesArr = usuarios.obtieneRefaccionesVencidas(idEmpresa, idTaller);
                if ((bool)ordenesArr[0])
                {
                    ordenes = Convert.ToInt32(ordenesArr[1]);
                    if (ordenes != 0)
                        generanotificacion(idEmpresa, idTaller, idUsuario, ordenes.ToString(), 12);
                    int transRetProg = usuarios.obtieneTransitoProgramado(idUsuario, idEmpresa, idTaller);
                    if (transRetProg > 0)
                        generanotificacion(idEmpresa, idTaller, idUsuario, transRetProg.ToString(), 14);
                }
                else
                    lblErrorLog.Text = "Ocurrio un error inesperado: " + ordenesArr[1].ToString();

            }
            else
                lblErrorLog.Text = "Ocurrio un error inesperado: " + ordenesArr[1].ToString();
        }
        catch (Exception ex) { }
    }

    private void notificaOrdenEntregaYVencida(int idEmpresa, int idTaller, int idUsuario)
    {
        try
        {
            DateTime fechaHoy = fechas.obtieneFechaLocal();
            object[] ordenesArr = usuarios.obtieneOrdenesPorEntregar(idEmpresa, idTaller);
            if ((bool)ordenesArr[0])
            {
                string ordenes = ordenesArr[1].ToString();
                generanotificacion(idEmpresa, idTaller, idUsuario, ordenes, 16);
            }
            else
                lblErrorLog.Text = "Ocurrio un error inesperado: " + ordenesArr[1].ToString();
        }
        catch (Exception ex) { }
    }

    private void generanotificacion(int idEmpresa, int idTaller, int idUsuario, string ordenes, int intMensaje)
    {
        try
        {
            Notificaciones notifi = new Notificaciones();
            notifi.Extra = ordenes.ToString();
            notifi.Empresa = idEmpresa;
            notifi.Taller = idTaller;
            notifi.Usuario = idUsuario.ToString();
            notifi.Fecha = fechas.obtieneFechaLocal();
            notifi.Estatus = "P";
            notifi.Clasificacion = intMensaje;
            notifi.Origen = "O";
            notifi.armaNotificacion();
            notifi.agregaNotificacion();
        }
        catch (Exception ex)
        {
            string error = "";
        }
    }

    private void cierraSesionUsu(int usu)
    {
        usuarios.actualizaBitacoraAcceso(usu, "I");
        lblErrorLog.Text = "Sesión cerrada, ingresa de nuevo Usuario y Contraseña para acceder.";
    }
}