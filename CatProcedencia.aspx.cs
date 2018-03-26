using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatProcedencia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        SqlDsCatProcedencia.InsertParameters["proc_Descrip"].DefaultValue = txtProcDescrip.Text.Trim();
        SqlDsCatProcedencia.Insert();
        txtProcDescrip.Text = string.Empty;
    }

    protected void grdProc_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Ejecuciones ejec = new Ejecuciones();
        string sqlValDep = "SELECT ref_no_orden FROM Refacciones_Orden WHERE id_Procedencia=" + e.Keys[0].ToString();
        if ((bool)ejec.scalarToBool(sqlValDep)[1])
        {
            lblError.Text = "No se puede eliminar la procedencia, está en uso.";
            e.Cancel = true;
        }
    }

    protected void grdProc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblError.Text = "";
    }
}