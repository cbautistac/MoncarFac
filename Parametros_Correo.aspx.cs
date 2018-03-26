using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Parametros_Correo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblHabilitado.Text = "0";
        }
    }

    protected void btnNuevoParametro_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrores.Text = "";
            SqlDataSource1.Insert();
            GridParametros.DataBind();
            txtContraseña.Text = txtHost.Text = txtPuerto.Text = txtUsuario.Text = "";
        }
        catch (Exception x)
        {
            lblErrores.Text = "Ocurrio un error inseperado: " + x.Message;
            GridParametros.DataSource = null;
            GridParametros.DataBind();
        }
    }

    protected void chkSSLHabilitado_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSSLHabilitado.Checked)
            lblHabilitado.Text = "1";
        else
            lblHabilitado.Text = "0";
    }
}