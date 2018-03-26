using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CatUsuarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    private void borraCampos()
    {
        txtContraseña.Text = txtNombre.Text = txtUsuario.Text = "";
    }
    protected void lnkEliminar_Click(object sender, EventArgs e)
    {
        AlUsuarios pera = new AlUsuarios();
        pera.id_usuario = Convert.ToInt32(RadGrid1.SelectedValues["id_usuario"]);
        pera.id_empresa = Convert.ToInt32(Request.QueryString["e"]);
        pera.eliminarUsuario();
        if(Convert.ToBoolean(pera.retorno[1]))
        {
            lblErrorAfuera.Text = "Se elimino el usuario exitosamente";
            RadGrid1.DataBind();
            lnkAbreEdicion.Visible = true;
            lnkEliminar.Visible = true;
        }
        else
        {
            lblErrorAfuera.Text = "Error al eliminar al usuario:" + pera.retorno[1];

        }

    }
    protected void lnkAbreAgrega_Click(object sender, EventArgs e)
    {
        borraCampos();
        lblIdUsuario.Text = "";
        string script = "abreNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
    }

    protected void lnkCerrar_Click(object sender, EventArgs e)
    {
        borraCampos();
        string script = "cierraNewEmi()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
    }

    protected void lnkAgrega_Click(object sender, EventArgs e)
    {
        AlUsuarios pera = new AlUsuarios();
        pera.nombre = txtNombre.Text;
        pera.nick = txtUsuario.Text;
        pera.contraseña = txtContraseña.Text;
        pera.id_empresa = Convert.ToInt32(Request.QueryString["e"]);
        if (lblIdUsuario.Text == "")
        {
            pera.Agrega_Usuario();
            if (Convert.ToBoolean(pera.retorno[0]))
            {
                lblErrorAfuera.Text = "El usuario se agrego exitosamente";
                RadGrid1.DataBind();
                borraCampos();
                lnkAbreEdicion.Visible = true;
                lnkEliminar.Visible = true;
                string script = "cierraNewEmi()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
            }
            else
            {
                lblErrorAdentro.Text = "Error al agregar al usuario:" + pera.retorno[1];

            }
        }
        else
        {
            pera.id_usuario = Convert.ToInt32(lblIdUsuario.Text);
            pera.actualizaUsuario();
            if (Convert.ToBoolean(pera.retorno[0]))
            {
                lblErrorAfuera.Text = "El usuario se actualizo exitosamente";
                RadGrid1.DataBind();
                borraCampos();
                lnkAbreEdicion.Visible = true;
                lnkEliminar.Visible = true;
                string script = "cierraNewEmi()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "cierra", script, true);
            }
            else
            {
                lblErrorAdentro.Text = "Error al actualizar al usuario:" + pera.retorno[1];

            }
        }
    }

    protected void lnkAbreEdicion_Click(object sender, EventArgs e)
    {
        int usuario;
        usuario = Convert.ToInt32(RadGrid1.SelectedValues["id_usuario"]);
        AlUsuarios edita = new AlUsuarios();
        edita.id_usuario = usuario;
        edita.id_empresa = Convert.ToInt32(Request.QueryString["e"]);
        edita.obtieneUsuarioaEditar();
        if (Convert.ToBoolean(edita.retorno[0]))
        {
            DataSet ds = (DataSet)edita.retorno[1];
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                lblIdUsuario.Text = r[1].ToString();
                txtUsuario.Text = r[2].ToString();
                txtContraseña.Text = r[3].ToString();
                txtNombre.Text = r[4].ToString();
            }
            string script = "abreNewEmi()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "abre", script, true);
        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[4] { 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[0] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["t"]);
        }
        catch (Exception x)
        {
            sesiones = new int[4] { 0, 0,0,0};
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
    protected void RadGrid_1OnSelectedIndexChangued(object sender, EventArgs e)
    {
        lnkAbreEdicion.Visible = true;
        lnkEliminar.Visible = true;
    }
}