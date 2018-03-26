using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ControlAccesos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkCerrarSesionUss_Click(object sender, EventArgs e)
    {
        LinkButton lnkSalir = sender as LinkButton;
        int idUsuario = Convert.ToInt32(lnkSalir.CommandArgument);
        CatUsuarios usuarios = new CatUsuarios();
        object[] actualizaAcceso = usuarios.actualizaBitacoraAcceso(idUsuario, "I");
        if (Convert.ToBoolean(actualizaAcceso[1]))
        {
            grvUsuarios.DataBind();
        }
        else
            lblError.Text = "No se logro cerrar la sesión del usuario seleccionado, vuelva a intentar";

    }
}