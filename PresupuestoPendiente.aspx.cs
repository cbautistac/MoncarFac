using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;


public partial class PresupuestoPendiente : System.Web.UI.Page
{
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    Fechas fechas = new Fechas();
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

    private void cargaInfo()
    {

        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0)
            Response.Redirect("Default.aspx");
        try
        {

            SqlDataSource1.SelectParameters.Clear();

            if (txtFiltro.Text == "")
            {
                SqlDataSource1.SelectCommand = "select orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion as descripcion,upper(v.color_ext) as color_ext,orp.placas,l.descripcion as localizacion,C.razon_social,orp.fase_orden, so.f_recepcion, orp.no_siniestro,v.modelo,po.descripcion as perfil"
                                                        + " from Ordenes_Reparacion orp"
                                                        + " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller"
                                                        + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                                                        + " left join Marcas m on m.id_marca=orp.id_marca"
                                                        + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
                                                        + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                                                        + " left join Localizaciones l on l.id_localizacion=orp.id_localizacion"
                                                        + " left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C'"
                                                        + " left join PerfilesOrdenes po on po.id_perfilOrden=orp.id_perfilOrden"
                                                        + " where orp.id_empresa=" + sesiones[2].ToString() + " and orp.id_taller=" + sesiones[3].ToString() + " and orp.status_orden='" + ddlEstatus.SelectedValue + "' "
                                                        + " and (select isnull((select sum(tabla.registros) as registros from(select count(*) as registros, no_orden as orden from mano_obra where no_orden in (select no_orden from ordenes_reparacion where id_empresa = orp.id_empresa and id_taller = orp.id_taller) and id_empresa = orp.id_empresa and id_taller = orp.id_taller group by id_empresa, id_taller, no_orden"
                                                        + " union all select count(*) as registros, ref_no_orden as orden from refacciones_orden where ref_no_orden in (select no_orden from ordenes_reparacion where id_empresa = orp.id_empresa and id_taller = orp.id_taller) and ref_id_empresa = orp.id_empresa and ref_id_taller = orp.id_taller group by ref_id_empresa, ref_id_taller, ref_no_orden) as tabla where tabla.orden = orp.no_orden group by tabla.orden),0))= 0 "
                                                        + " order by orp.no_orden desc";
            }
            else
            {
                SqlDataSource1.SelectCommand = "select orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion as descripcion,upper(v.color_ext) as color_ext,orp.placas,l.descripcion as localizacion,C.razon_social,orp.fase_orden, so.f_recepcion, orp.no_siniestro,v.modelo,po.descripcion as perfil"
                                                        + " from Ordenes_Reparacion orp"
                                                        + " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller"
                                                        + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                                                        + " left join Marcas m on m.id_marca=orp.id_marca"
                                                        + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
                                                        + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                                                        + " left join Localizaciones l on l.id_localizacion=orp.id_localizacion"
                                                        + " left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C'"
                                                        + " left join PerfilesOrdenes po on po.id_perfilOrden=orp.id_perfilOrden "
                                                        + " where orp.id_empresa=" + sesiones[2].ToString() + " and orp.id_taller=" + sesiones[3].ToString() + " and orp.status_orden='" + ddlEstatus.SelectedValue + "' "
                                                        + " and (select isnull((select sum(tabla.registros) as registros from(select count(*) as registros, no_orden as orden from mano_obra where no_orden in (select no_orden from ordenes_reparacion where id_empresa = orp.id_empresa and id_taller = orp.id_taller) and id_empresa = orp.id_empresa and id_taller = orp.id_taller group by id_empresa, id_taller, no_orden"
                                                        + " union all select count(*) as registros, ref_no_orden as orden from refacciones_orden where ref_no_orden in (select no_orden from ordenes_reparacion where id_empresa = orp.id_empresa and id_taller = orp.id_taller) and ref_id_empresa = orp.id_empresa and ref_id_taller = orp.id_taller group by ref_id_empresa, ref_id_taller, ref_no_orden) as tabla where tabla.orden = orp.no_orden group by tabla.orden),0))= 0 "
                                                        + " and (orp.no_orden like '%" + txtFiltro.Text + "%' or c.razon_social like '%" + txtFiltro.Text + "%' or orp.placas like '%" + txtFiltro.Text + "%' or m.descripcion like '%" + txtFiltro.Text + "%' or tv.descripcion like '%" + txtFiltro.Text + "%' or tu.descripcion like '%" + txtFiltro.Text + "%' or v.color_ext like '%" + txtFiltro.Text + "%' or orp.no_siniestro like '%" + txtFiltro.Text + "%' or v.modelo like '%" + txtFiltro.Text + "%' or l.descripcion like '%" + txtFiltro.Text + "%' or po.descripcion like '%" + txtFiltro.Text + "%') order by orp.no_orden desc";

            }
            GridView1.DataBind();

        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }
    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargaInfo();
    }
    protected void lnkBuscar_Click(object sender, EventArgs e)
    {
        cargaInfo();
    }
    protected void lnkLimpiar_Click(object sender, EventArgs e)
    {
        txtFiltro.Text = "";
        cargaInfo();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string fase = DataBinder.Eval(e.Row.DataItem, "fase_orden").ToString();
            var img = e.Row.Cells[10].Controls[1].FindControl("imgFase") as System.Web.UI.WebControls.Image;
            var lblFechaIni = e.Row.Cells[1].Controls[1].FindControl("lblFechaRecp") as Label;
            var lbtnNoOrden = e.Row.Cells[1].Controls[1].FindControl("no_orden") as LinkButton;
            img.ImageUrl = "img/fase_" + fase + ".png";
            try
            {
                string fechaIni = lblFechaIni.Text;
                int noOrden = Convert.ToInt32(lbtnNoOrden.Text);
                bool llamadaPendiente = checaEstatusLlamada(noOrden);
                if (!llamadaPendiente)
                {
                    DateTime fechaActual = fechas.obtieneFechaLocal();
                    DateTime fechaIngreso = Convert.ToDateTime(fechaIni);
                    TimeSpan hrsDif = fechaActual - fechaIngreso;
                    int hrsDiferencia = Convert.ToInt32(hrsDif.TotalHours);
                    if (hrsDiferencia < 49)
                    { }
                    else if (hrsDiferencia < 72 && hrsDiferencia > 48)
                    {
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.Yellow;
                    }
                    else if (hrsDiferencia > 72)
                    {
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {

            }
        }
    }

    private bool checaEstatusLlamada(int noOrden)
    {
        bool pendiente = false;
        try
        {
            int[] sesiones = obtieneSesiones();
            int idEmpresa = sesiones[2];
            int idTaller = sesiones[3];
            bool estatus = datosOrdenes.obtieneEstatusLlamada(noOrden, idEmpresa, idTaller);
            if (estatus)
                pendiente = true;
            else
                pendiente = false;
        }
        catch (Exception ex)
        {
            pendiente = false;
        }
        return pendiente;
    }

    protected void btnOrden_Click(object sender, EventArgs e)
    {
        LinkButton lknReferencia = (LinkButton)sender;
        int orden = Convert.ToInt32(lknReferencia.Text);
        int fase = Convert.ToInt32(lknReferencia.CommandArgument.ToString());        
        Response.Redirect("LevantamientoOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + orden + "&f=" + fase);
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        cargaInfo();
    }
}