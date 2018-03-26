using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using Telerik.Web.UI;

public partial class RefaccionesPendientes : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void cargaDatos()
    {
        RadGrid1.DataSource = cargaFechas();
        RadGrid1.DataBind();
    }

    private DataTable cargaFechas()
    {
        DataTable dt = new DataTable();
        dt = null;
        string sql = string.Format("select distinct r.ref_no_orden," +
                                   "(select tv.descripcion+' '+m.descripcion+' '+tu.descripcion+' '+ltrim(rtrim(upper(v.color_ext)))+' '+cast(v.modelo as varchar) " +
                                   "from Ordenes_Reparacion orp " +
                                   "left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller " +
                                   "left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
                                   "left join Marcas m on m.id_marca = orp.id_marca " +
                                   "left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
                                   "left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
                                   "left join Localizaciones l on l.id_localizacion = orp.id_localizacion " +
                                   "left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
                                   "left join PerfilesOrdenes po on po.id_perfilOrden = orp.id_perfilOrden " +
                                   "where orp.no_orden = r.ref_no_orden and orp.id_empresa = " + Request.QueryString["e"] + " and orp.id_taller = " + Request.QueryString["t"] + ") as vehiculo, " +
                                   "(select c.razon_social  " +
                                   "from Ordenes_Reparacion orp " +
                                   "left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller " +
                                   "left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
                                   "left join Marcas m on m.id_marca = orp.id_marca " +
                                   "left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
                                   "left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
                                   "left join Localizaciones l on l.id_localizacion = orp.id_localizacion " +
                                   "left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
                                   "left join PerfilesOrdenes po on po.id_perfilOrden = orp.id_perfilOrden " +
                                   "where orp.no_orden = r.ref_no_orden and orp.id_empresa = " + Request.QueryString["e"] + " and orp.id_taller = " + Request.QueryString["t"] + ") as razon_social, " +
                                   "(select po.descripcion " +
                                   "from Ordenes_Reparacion orp " +
                                   "left join Seguimiento_Orden so on so.no_orden = orp.no_orden and orp.id_empresa = so.id_empresa and orp.id_taller = so.id_taller " +
                                   "left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
                                   "left join Marcas m on m.id_marca = orp.id_marca " +
                                   "left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
                                   "left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
                                   "left join Localizaciones l on l.id_localizacion = orp.id_localizacion " +
                                   "left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
                                   "left join PerfilesOrdenes po on po.id_perfilOrden = orp.id_perfilOrden " +
                                   "where orp.no_orden = r.ref_no_orden and orp.id_empresa = " + Request.QueryString["e"] + " and orp.id_taller = " + Request.QueryString["t"] + ") as perfil, " +

                                   "(select count(*) " +
                                   "from Refacciones_Orden " +
                                   "inner join cliprov c on c.id_cliprov = refproveedor and c.tipo = 'P' " +
                                   "where ref_no_orden = r.ref_no_orden and ref_id_empresa = " + Request.QueryString["e"] + " and ref_id_taller = " + Request.QueryString["t"] +
                                   " and refestatus = 'AU' and reffechentregaEst < '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "') as refacciones," +
                                   "(select fase_orden from ordenes_reparacion where id_empresa=r.ref_id_empresa and id_taller=r.ref_id_taller and no_orden=r.ref_no_orden)as fase_orden from Refacciones_Orden r " +
                                   "where r.ref_id_empresa =" + Request.QueryString["e"] + " and r.ref_id_taller = " + Request.QueryString["t"] +
                                   " and r.refestatus = 'AU' and r.reffechentregaEst < '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'");
        dt = GetDataTable(sql);
        return dt;
    }

    public DataTable GetDataTable(string query)
    {
        String ConnString = ConfigurationManager.ConnectionStrings["Taller"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnString);
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = new SqlCommand(query, conn);
        DataTable myDataTable = new DataTable();
        conn.Open();
        try
        {
            adapter.Fill(myDataTable);
        }
        catch(Exception x)
        {

        }
        finally
        {
            conn.Close();
        }
        return myDataTable;
    }

    protected void RadGrid1_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        switch (e.DetailTableView.Name)
        {
            case "ingreso":
                {
                    string CustomerID = dataItem.GetDataKeyValue("ref_no_orden").ToString();
                    e.DetailTableView.DataSource = GetDataTable(string.Format(
                        "select c.razon_social,ref_no_orden,refdescripcion,reffechsolicitud,reffechentregaest from Refacciones_Orden inner join cliprov c on c.id_cliprov=refproveedor and c.tipo='P' " +
                        "where ref_id_empresa = " + Request.QueryString["e"].ToString() + " and ref_id_taller = " + Request.QueryString["t"].ToString() + " and ref_no_orden = " + CustomerID +
                        " and refestatus = 'AU' and reffechentregaEst < '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'"));
                    break;
                }
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!e.IsFromDetailTable)
            (sender as RadGrid).DataSource = cargaFechas();
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //RadGrid1.MasterTableView.Items[0].Expanded = true;
            //RadGrid1.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
        }
    }

    protected void btnOrden_Click(object sender, EventArgs e)
    {
        LinkButton lknReferencia = (LinkButton)sender;
        int orden = Convert.ToInt32(lknReferencia.Text);
        int fase = Convert.ToInt32(lknReferencia.CommandArgument.ToString());
        Response.Redirect("BienvenidaOrdenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + orden + "&f=" + fase);
    }

    protected void lnkRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ordenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
}