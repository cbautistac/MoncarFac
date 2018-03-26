using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ComparativoCostos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cargaInfo();
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[4] { 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);
        }
        catch (Exception x)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }


    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is Telerik.Web.UI.GridDataItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;
            var lblFechaIni = r[11].ToString();
            var btnOrden = r[4].ToString();
            var status = r[15].ToString();
            try
            {
                string fechaIni = Convert.ToString(lblFechaIni);
                int noOrden = Convert.ToInt32(btnOrden);
                var btn = e.Item.Controls[6].FindControl("btnOrden") as LinkButton;
                if (Convert.ToString(status) == "A")
                    btn.CssClass = "btn btn-primary textoBold colorBlanco";
                else if (Convert.ToString(status) == "T")
                    btn.CssClass = "btn btn-info textoBold colorBlanco";
                else if (Convert.ToString(status) == "C")
                    btn.CssClass = "btn btn-default textoBold colorBlanco";
                else if (Convert.ToString(status) == "R")
                    btn.CssClass = "btn btn-success textoBold colorBlanco";
                else if (Convert.ToString(status) == "F")
                    btn.CssClass = "btn btn-warning textoBold colorBlanco";
                else if (Convert.ToString(status) == "S")
                    btn.CssClass = "btn btn-danger textoBold colorBlanco";

            }
            catch (Exception ex)
            {

            }
        }
    }

    private void cargaInfo()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0)
            Response.Redirect("Default.aspx");
        try
        {

            SqlDataSource1.SelectCommand = "select orp.id_empresa,em.razon_social as empresa, orp.id_taller,t.nombre_taller as taller, orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion as descripcion,upper(v.color_ext) as color_ext,orp.placas,l.descripcion as localizacion,C.razon_social,orp.fase_orden, so.f_recepcion, orp.no_siniestro,v.modelo,po.descripcion as perfil,orp.status_orden"
            + " from Ordenes_Reparacion orp"
            + " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller"
            + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
            + " left join Marcas m on m.id_marca=orp.id_marca"
            + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
            + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
            + " left join Localizaciones l on l.id_localizacion=orp.id_localizacion"
            + " left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C'"
            + " left join PerfilesOrdenes po on po.id_perfilOrden=orp.id_perfilOrden "
            + " left join Empresas em on em.id_empresa=orp.id_empresa "
            + " left join Talleres t on t.id_taller=orp.id_taller "
            + " order by orp.id_empresa,orp.id_taller,orp.no_orden desc";
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    protected void btnOrden_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        LinkButton lknReferencia = (LinkButton)sender;
        string[] argumentos = lknReferencia.CommandArgument.ToString().Split(new char[] { ';' });

        string orden = argumentos[0];
        string taller = argumentos[1];
        string empresa = argumentos[2];
        SqlDsCompDet.SelectParameters["noOrden"].DefaultValue = orden;
        SqlDsCompDet.SelectParameters["empID"].DefaultValue = empresa;
        SqlDsCompDet.SelectParameters["tallerID"].DefaultValue = taller;
        RadGrid2.DataBind();        
        lblError.Text = "";
        /*cargaInfoOrden();
        cargaDatosPie();
        RadTabStrip1.SelectedIndex = 0;
        RadMultiPage1.SelectedIndex = 0;*/
        
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", "abreWinCtrl()", true);
        
    }
    
}