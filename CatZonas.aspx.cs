using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatZonas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        SqlDsCatProcedencia.InsertParameters["descripcion"].DefaultValue = txtProcDescrip.Text.Trim();
        SqlDsCatProcedencia.Insert();
        txtProcDescrip.Text = string.Empty;
    }

    protected void grdProc_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Ejecuciones ejec = new Ejecuciones();
        string sqlValDep = "SELECT count(*) FROM cliprov WHERE id_zona=" + e.Keys[0].ToString();
        if ((bool)ejec.scalarToBool(sqlValDep)[1])
        {
            lblError.Text = "No se puede eliminar la ragión, está en uso.";
            e.Cancel = true;
        }
    }

    protected void grdProc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
    }
}