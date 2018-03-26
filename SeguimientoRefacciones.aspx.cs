using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;

public partial class SeguimientoRefacciones : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargaDatosPie();
        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[6] { 0, 0, 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);
            sesiones[4] = Convert.ToInt32(Request.QueryString["o"]);
            sesiones[5] = Convert.ToInt32(Request.QueryString["f"]);
        }
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    private void cargaDatosPie()
    {
        int[] sesiones = obtieneSesiones();
        object[] datosOrden = recepciones.obtieneInfoOrdenPie(sesiones[4], sesiones[2], sesiones[3]);
        if (Convert.ToBoolean(datosOrden[0]))
        {
            DataSet ordenDatos = (DataSet)datosOrden[1];
            foreach (DataRow filaOrd in ordenDatos.Tables[0].Rows)
            {
                ddlToOrden.Text = filaOrd[0].ToString();
                ddlClienteOrden.Text = filaOrd[1].ToString();
                ddlTsOrden.Text = filaOrd[2].ToString();
                ddlValOrden.Text = filaOrd[3].ToString();
                ddlTaOrden.Text = filaOrd[4].ToString();
                ddlLocOrden.Text = filaOrd[5].ToString();
                ddlPerfil.Text = filaOrd[13].ToString();
                lblSiniestro.Text = filaOrd[9].ToString();
                lblDeducible.Text = Convert.ToDecimal(filaOrd[10].ToString()).ToString("C2");
                lblTotOrden.Text = Convert.ToDecimal(filaOrd[11].ToString()).ToString("C2");
                try
                {
                    DateTime fechaEntrega = Convert.ToDateTime(filaOrd[14].ToString());
                    if (fechaEntrega.ToString("yyyy-MM-dd") == "1900-01-01")
                        lblEntregaEstimada.Text = "No establecida";
                    else
                        lblEntregaEstimada.Text = filaOrd[14].ToString();
                }
                catch (Exception)
                {
                    lblEntregaEstimada.Text = "No establecida";
                }
                lblPorcDedu.Text = filaOrd[16].ToString() + "%";
            }
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update") {
            string refaccion = ((Label)GridView1.Rows[GridView1.EditIndex].FindControl("Label2")).Text;
            string fecha = ((TextBox)GridView1.Rows[GridView1.EditIndex].FindControl("txtFecha")).Text;
            DateTime fechaValida = fechas.obtieneFechaLocal();
            try
            {
                fechaValida = Convert.ToDateTime(fecha);
                //llenarBitacora("Edito", proceso, descripcion.Text, Convert.ToInt32(cantidad.Text), estatus.SelectedItem.ToString());
                SqlDataSource1.UpdateCommand = "UPDATE [Refacciones_Orden] SET refFechEntregaReal = '" + fechaValida.ToString("yyyy-MM-dd") + "', refestatusSolicitud=2 WHERE [refOrd_Id] = " + refaccion.ToString() + " AND [ref_no_orden] = " + Request.QueryString["o"] + " and [ref_id_empresa]=" + Request.QueryString["e"] + " and [ref_id_taller]=" + Request.QueryString["t"];
                GridView1.EditIndex = -1;
                GridView1.DataBind();
                actualizaFase();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;                
            }
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btn = e.Row.FindControl("lnkEdita") as LinkButton;
            string estatus = DataBinder.Eval(e.Row.DataItem, "refEstatus").ToString();
            
            if (estatus == "CO")
                btn.Visible = false;
        }
    }

    private void actualizaFase()
    {
        int faseSActual = 1;
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
        try
        {
            object[] datos = recepciones.obtieneInfoOrden(orden, empresa, taller);
            if (Convert.ToBoolean(datos[0]))
            {
                DataSet valores = (DataSet)datos[1];
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    faseSActual = Convert.ToInt32(fila[53].ToString());
                }

                if (faseSActual < 4)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 4);
                }
            }
        }
        catch (Exception) { }

    }
}