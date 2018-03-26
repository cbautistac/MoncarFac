using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AsignacionPermisos : System.Web.UI.Page
{
    DatosPermisos datosPerm = new DatosPermisos();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void cargaDatos()
    {
        GridAsignados.DataBind();
        GridPermisos.DataBind();
    }

    protected void lnkAsignar_Click(object sender, EventArgs e)
    {
        LinkButton lnkAsignar = (LinkButton)sender;
        int idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
        int idPermiso = Convert.ToInt32(lnkAsignar.CommandArgument);
        object[] ejecutado = datosPerm.asignaPermiso(idUsuario, idPermiso);
        if ((bool)ejecutado[0])
            if (Convert.ToBoolean(ejecutado[1]))
            {
                cargaDatos();
                lblError.Text = "Permiso asignado satisfactoriamente";
            }
            else
                lblError.Text = "No se logro asignar el permiso, verifique su conexión e intentelo nuevamnete";
        else
            lblError.Text = "Ocurrio un error inesperado: " + ejecutado[1].ToString();
    }

    protected void lnkDesAsignar_Click(object sender, EventArgs e)
    {
        LinkButton lnkDesAsignar = (LinkButton)sender;
        int idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
        int idPermiso = Convert.ToInt32(lnkDesAsignar.CommandArgument);
        object[] ejecutado = datosPerm.desasignaPermiso(idUsuario, idPermiso);
        if ((bool)ejecutado[0])
            if (Convert.ToBoolean(ejecutado[1]))
            {
                cargaDatos();
                lblError.Text = "Permiso desasignado satisfactoriamente";
            }
            else
                lblError.Text = "No se logro desasignar el permiso, verifique su conexión e intentelo nuevamnete";
        else
            lblError.Text = "Ocurrio un error inesperado: " + ejecutado[1].ToString();
    }

    protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargaDatos();
    }
}