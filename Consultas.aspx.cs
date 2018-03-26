using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class Consultas : System.Web.UI.Page
{
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    Fechas fechas = new Fechas();

    protected void Page_Load(object sender, EventArgs e)
    {
        string qs = Request.QueryString["c"];
        short intTipoCons = short.Parse(qs);
        if (!IsPostBack)
        {
            string Titulo = "Elije consulta";
            switch (intTipoCons) {
                case 1:
                    Titulo = "Órdenes en Tránsito";
                    break;
                case 2:
                    Titulo = "Órdenes con Garatía";
                    break;
                case 3:
                    Titulo = "Órdenes en Terminado";
                    break;
                case 4:
                    Titulo = "Órdenes por Entregar";
                    break;
            }
            litTit.Text = Titulo;
            pnlRefOrd.Visible = PanelMascara.Visible = false;     
            cargaInfo(intTipoCons);
        }
    }

    private void cargaInfo(Int16 tipoCons)
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0)
            Response.Redirect("Default.aspx");

        string condQry = "";
        switch (tipoCons)
        {
            case 1: //para en tránsito perfilOrden = 2 y sin fecha
                condQry = " AND orp.id_perfilOrden=2 AND (so.f_retorno_transito IS NULL OR so.f_retorno_transito='1900-01-01') ";
                break;
            case 2:
                condQry = " AND orp.id_perfilOrden=3 ";
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = false;
                break;
            case 3:
                condQry = " AND so.f_terminado > '1900-01-01' AND (so.f_entrega IS NULL OR so.f_retorno_transito='1900-01-01') ";
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = false;
                break;
            case 4:
                condQry = " AND so.f_terminado > '1900-01-01' AND so.f_entrega_estimada > '1900-01-01' ";
                GridView1.Columns[GridView1.Columns.Count - 1].Visible = false;
                break;
        }
        try
        {
            SqlDataSource1.SelectParameters.Clear();

            if (txtFiltro.Text == "")
            {
                SqlDataSource1.SelectCommand = "select orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion as descripcion,upper(v.color_ext) as color_ext,orp.placas,l.descripcion as localizacion,C.razon_social,orp.fase_orden, so.f_recepcion, orp.no_siniestro,v.modelo,po.descripcion as perfil, CAST(so.f_entrega_estimada as varchar(10)) + ' ' + CAST(so.h_estrega_estimada as varchar(8)) AS entEstimada, CAST(so.f_pactada as varchar(10)) + ' ' + CAST(so.h_pactada as varchar(8)) AS f_pactada"
                                                        + " from Ordenes_Reparacion orp"
                                                        + " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller"
                                                        + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                                                        + " left join Marcas m on m.id_marca=orp.id_marca"
                                                        + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
                                                        + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                                                        + " left join Localizaciones l on l.id_localizacion=orp.id_localizacion"
                                                        + " left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C'"
                                                        + " left join PerfilesOrdenes po on po.id_perfilOrden=orp.id_perfilOrden"
                                                        + " where orp.id_empresa=" + sesiones[2].ToString() + " and orp.id_taller=" + sesiones[3].ToString() + condQry + " order by orp.no_orden desc";
            }
            else
            {
                SqlDataSource1.SelectCommand = "select orp.no_orden,tv.descripcion+' '+m.descripcion+' '+tu.descripcion as descripcion,upper(v.color_ext) as color_ext,orp.placas,l.descripcion as localizacion,C.razon_social,orp.fase_orden, so.f_recepcion, orp.no_siniestro,v.modelo,po.descripcion as perfil, CAST(so.f_entrega_estimada as varchar(10)) + ' ' + CAST(so.h_estrega_estimada as varchar(8)) AS entEstimada, CAST(so.f_pactada as varchar(10)) + ' ' + CAST(so.h_pactada as varchar(8)) AS f_pactada"
                                                        + " from Ordenes_Reparacion orp"
                                                        + " left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa=so.id_empresa and orp.id_taller=so.id_taller"
                                                        + " left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo"
                                                        + " left join Marcas m on m.id_marca=orp.id_marca"
                                                        + " left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo"
                                                        + " left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad"
                                                        + " left join Localizaciones l on l.id_localizacion=orp.id_localizacion"
                                                        + " left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C'"
                                                        + " left join PerfilesOrdenes po on po.id_perfilOrden=orp.id_perfilOrden"
                                                        + " where orp.id_empresa=" + sesiones[2].ToString() + " and orp.id_taller=" + sesiones[3].ToString() + condQry + " and (orp.no_orden like '%" + txtFiltro.Text + "%' or c.razon_social like '%" + txtFiltro.Text + "%' or orp.placas like '%" + txtFiltro.Text + "%' or m.descripcion like '%" + txtFiltro.Text + "%' or tv.descripcion like '%" + txtFiltro.Text + "%' or tu.descripcion like '%" + txtFiltro.Text + "%' or v.color_ext like '%" + txtFiltro.Text + "%' or orp.no_siniestro like '%" + txtFiltro.Text + "%' or v.modelo like '%" + txtFiltro.Text + "%' or l.descripcion like '%" + txtFiltro.Text + "%' or po.descripcion like '%" + txtFiltro.Text + "%') order by orp.no_orden desc";

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

    protected void lnkRegresarOrdenes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ordenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.DataItemIndex == 0)
                lblIdOrden.Text = DataBinder.Eval(e.Row.DataItem, "no_orden").ToString();
            string fPactada = DataBinder.Eval(e.Row.DataItem, "f_pactada").ToString();
            if (fPactada.Contains("1900"))
                e.Row.Cells[11].Text = "";
            if (DataBinder.Eval(e.Row.DataItem, "entEstimada").ToString().Contains("1900"))
                e.Row.Cells[10].Text = "";
            var lblFechaIni = e.Row.Cells[1].Controls[1].FindControl("lblFechaRecp") as Label;
            var lbtnNoOrden = e.Row.Cells[1].Controls[1].FindControl("no_orden") as LinkButton;
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

    protected void lnkLimpiar_Click(object sender, EventArgs e)
    {
        txtFiltro.Text = "";
        cargaInfo(Int16.Parse(Request.QueryString["c"].ToString()));
    }

    protected void btnOrden_Click(object sender, EventArgs e)
    {
        LinkButton lknReferencia = (LinkButton)sender;
        int orden = Convert.ToInt32(lknReferencia.Text);
        int fase = Convert.ToInt32(lknReferencia.CommandArgument.ToString());
        Response.Redirect("BienvenidaOrdenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + orden + "&f=" + fase);
    }

    protected void lnkSeleccionar_Click(object sender, EventArgs e)
    {
        //int[] sesiones = obtieneSesiones();
        LinkButton btn = (LinkButton)sender;
        //string[] argumentos = btn.CommandArgument.ToString().Split(new char[] { ';' });

        pnlRefOrd.Visible = PanelMascara.Visible = true;
    }
    protected void btnCerrarComp_Click(object sender, EventArgs e)
    {
        pnlRefOrd.Visible = PanelMascara.Visible = false;     
    }
}