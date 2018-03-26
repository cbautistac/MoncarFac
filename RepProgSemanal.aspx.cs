using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.IO;

public partial class RepProgSemanal : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    ValidaFechas validaciones = new ValidaFechas();
    DatosProgSemanal datos = new DatosProgSemanal();
    DataSet dataImpresion;

    List<ListaImpresionSemanal> listaImpresion;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Chart1.Visible = false;
        }
    }

    private void cargaDatos()
    {
        lblError.Text = "";
        DateTime fechaIni, fechaFin;
        try
        {
            fechaIni = Convert.ToDateTime(txtFechaIni.Text);
            fechaFin = Convert.ToDateTime(txtFechaFin.Text);
            validaciones.fechaInicial = fechaIni;
            validaciones.fechaFinal = fechaFin;
            int valido = validaciones.validaFechas();
            if (valido == 1)
                lblError.Text = "La fecha inicial no puede ser mayor que la final";
            else if (valido == 2)
            {
                TimeSpan ts = fechaFin - fechaIni;
                int dias = ts.Days;
                if (dias < 8)
                {
                    if (obtieneDia(fechaIni.DayOfWeek) != 2)
                        procesaOrdenes();
                }
            }
            else
                lblError.Text = "No es posible validar las fechas, por favor verifique";
        }
        catch (Exception)
        {

        }
    }

    private DataTable obtieneFechas(DateTime fechaIni, DateTime fechaFin)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fecha");
        dt.Columns.Add("dia");
        dt.Columns.Add("dw");
        dt.Columns.Add("diaN");
        dt.Columns.Add("f_div");
        DateTime fecha = fechaIni;
        while (fecha < fechaFin.AddDays(1))
        {
            int dia = obtieneDia(fecha.DayOfWeek);
            string dw = obtieneDiaSemana(fecha.DayOfWeek);
            if (dia != 1)
            {
                DataRow row = dt.NewRow();
                row["fecha"] = fecha.ToString("dd/MM/yyyy");
                row["dia"] = dia;
                row["dw"] = dw;
                row["diaN"] = fecha.Day;
                row["f_div"] = fecha.ToString("yyyyMMdd");
                dt.Rows.Add(row);
            }
            fecha = fecha.AddDays(1);
        }
        return dt;
    }

    private string obtieneDiaSemana(DayOfWeek dayOfWeek)
    {
        string retorno = "LUNES";
        switch (dayOfWeek.ToString().ToUpper())
        {
            case "SUNDAY":
                retorno = "DOMINGO";
                break;
            case "MONDAY":
                retorno = "LUNES";
                break;
            case "TUESDAY":
                retorno = "MARTES";
                break;
            case "WEDNESDAY":
                retorno = "MIERCOLES";
                break;
            case "THURSDAY":
                retorno = "JUEVES";
                break;
            case "FRIDAY":
                retorno = "VIERNES";
                break;
            case "SATURDAY":
                retorno = "SABADO";
                break;
            default:
                break;
        }
        return retorno;
    }

    private int obtieneDia(DayOfWeek dayOfWeek)
    {
        int retorno = 1;
        switch (dayOfWeek.ToString().ToUpper())
        {
            case "SUNDAY":
                retorno = 1;
                break;
            case "MONDAY":
                retorno = 2;
                break;
            case "TUESDAY":
                retorno = 3;
                break;
            case "WEDNESDAY":
                retorno = 4;
                break;
            case "THURSDAY":
                retorno = 5;
                break;
            case "FRIDAY":
                retorno = 6;
                break;
            case "SATURDAY":
                retorno = 7;
                break;
            default:
                break;
        }
        return retorno;
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

    protected void lnkRegresarOrdenes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ordenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkBuscar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        Session["listaImpresion"] = null;
        try
        {
            DateTime fini = Convert.ToDateTime(txtFechaIni.Text);
            DateTime ffin = Convert.ToDateTime(txtFechaFin.Text);
            if (fini < ffin)
            {
                TimeSpan diasDife = ffin - fini;
                int diasDifeInt = diasDife.Days;
                if (diasDifeInt < 7)
                {
                    Session["cliente"] = null;
                    Session["opcion"] = "0";
                    RadGrid1.DataSource = cargaFechas(0, 0);
                    RadGrid1.DataBind();
                    procesaOrdenes();
                    aseguradoras();
                }
                else
                    lblError.Text = "El rango de la fecha seleccionada no puede ser mayor a 7 dias, verifique sus datos.";
            }
            else
                lblError.Text = "La fecha inicial no puede ser mayor a la fecha final, verifique sus datos.";
        }
        catch (Exception)
        { lblError.Text = "Ocurrio un error en la validación de las fechas, verifique sus datos."; }
    }

    private static DataTable GetData(string query)
    {
        string strConnString = ConfigurationManager.ConnectionStrings["Taller"].ConnectionString;
        using (SqlConnection con = new SqlConnection(strConnString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = query;
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }

    protected void btnOrden_Click(object sender, EventArgs e)
    {
        LinkButton lknReferencia = (LinkButton)sender;
        int orden = Convert.ToInt32(lknReferencia.Text);
        int fase = Convert.ToInt32(lknReferencia.CommandArgument.ToString());
        Response.Redirect("BienvenidaOrdenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + orden + "&f=" + fase);
    }

    private void procesaOrdenes()
    {
        Chart1.Visible = false;
        if (txtFechaIni.Text != "" && txtFechaFin.Text != "")
        {
            try
            {
                double ordenesTotales = 0;
                double terminados = 0;
                double procesos = 0;
                double productividad = 0;
                double incumplimiento = 0;
                double proxVencer = 0;
                double otros = 0;

                int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                int idTaller = Convert.ToInt32(Request.QueryString["t"]);
                string fechaIni = txtFechaIni.Text;
                string fechaFin = txtFechaFin.Text;

                Indicadores indicador = new Indicadores();
                object[] datos= indicador.obtieneValores(idEmpresa, idTaller, fechaIni, fechaFin);
                if (Convert.ToBoolean(datos[0])) {
                    DataSet info = (DataSet)datos[1];
                    foreach (DataRow r in info.Tables[0].Rows) {
                        ordenesTotales = Convert.ToDouble(r[0].ToString());
                        terminados = Convert.ToDouble(r[1].ToString());
                        procesos = Convert.ToDouble(r[2].ToString());
                        productividad = Convert.ToDouble(r[3].ToString());
                        incumplimiento = Convert.ToDouble(r[4].ToString());
                        proxVencer = Convert.ToDouble(r[5].ToString());
                    }
                }

                /*productividad = totalesGrafica("productividad");
                terminados = totalesGrafica("terminados");
                procesos = totalesGrafica("procesos");
                otros = totalesGrafica("otros") - productividad - terminados - procesos;
                
                incumplimiento = totalesGrafica("incumplimiento");
                proxVencer = totalesGrafica("proxVencer");*/

                lblVencidos.Text = incumplimiento.ToString();
                lblProxVencer.Text = proxVencer.ToString();
                lblTerminados.Text = terminados.ToString();
                lblEntregados.Text = productividad.ToString();
                lblProceso.Text = procesos.ToString();
                lblTotales.Text = ordenesTotales.ToString();

                double[] yValues = { terminados, procesos, productividad, incumplimiento,proxVencer };
                string[] xValues = { "Terminados", "Proceso", "Entregados", "Vencidos", "Por Vencer" };
                //ordenesTotales = terminados + procesos + productividad + incumplimiento + proxVencer;
                Chart1.Series["Default"].Points.DataBindXY(xValues, yValues);

                Chart1.Series["Default"].Label = "#PERCENT{0.00%}";

                Chart1.Series["Default"].Points[0].Color = Color.FromArgb(63, 212, 59);//TERMINADOS
                Chart1.Series["Default"].Points[1].Color = Color.FromArgb(238, 145, 35);// EN PROCESO
                Chart1.Series["Default"].Points[2].Color = Color.FromArgb(73, 134, 232);//ENTREGADOS
                Chart1.Series["Default"].Points[3].Color = Color.FromArgb(185, 74, 72);//VENCIDOS
                Chart1.Series["Default"].Points[4].Color = Color.FromArgb(192, 152, 83);//PORVENCER

                Chart1.Series["Default"].ChartType = SeriesChartType.Pie;

                Chart1.Series["Default"]["PieLabelStyle"] = "Outside";
                Chart1.Series["Default"]["PieLineColor"] = "Black";

                Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                for (int cont = 0; cont < xValues.Length; cont++)
                {
                    DataPoint pt = Chart1.Series["Default"].Points[cont];
                    pt.LegendText = "#VALX: #VALY Vehiculo(s)";
                    try { Chart1.Legends.Add(pt.ToString()); }
                    catch (Exception x)
                    {
                        string xx = "";
                    }
                }
                if (ordenesTotales != 0)
                    Chart1.Visible = true;
                else
                    Chart1.Visible = false;
            }
            catch (Exception ex)
            {
                Chart1.Visible = false;
            }
        }
    }

    private double totalesGrafica(string ceunta)
    {
        double totales = 0;
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        string fechaIni = txtFechaIni.Text;
        string fechaFin = txtFechaFin.Text;
        try { totales = Convert.ToDouble(datos.obtieneTotales(idEmpresa, idTaller, fechaIni, fechaFin, ceunta)); }
        catch (Exception) { totales = 0; }
        return totales;
    }
    private void aseguradoras()
    {
        //Chart2.Visible = false;
        if (txtFechaIni.Text != "" && txtFechaFin.Text != "")
        {
            try
            {
                int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                int idTaller = Convert.ToInt32(Request.QueryString["t"]);
                string fechaIni = txtFechaIni.Text;
                string fechaFin = txtFechaFin.Text;
                object[] ejecutado = new object[2];
                DataSet aseguradorasData = null;
                ejecutado = datos.optieneAseguradoras(idEmpresa, idTaller, fechaIni, fechaFin);
                if ((bool)ejecutado[0])
                    aseguradorasData = (DataSet)ejecutado[1];
                if (aseguradorasData != null)
                {
                    string aseguradorasStringText = "";
                    string aseguradorasStringInt = "";
                    int contaForeach = 0;
                    Color[] colores = new Color[aseguradorasData.Tables[0].Rows.Count];

                    foreach (DataRow fila in aseguradorasData.Tables[0].Rows)
                    {
                        if (contaForeach == 0)
                        {
                            aseguradorasStringText = fila[2].ToString();
                            aseguradorasStringInt = fila[1].ToString();
                        }
                        else
                        {
                            aseguradorasStringText = aseguradorasStringText + ";" + fila[2].ToString();
                            aseguradorasStringInt = aseguradorasStringInt + ";" + fila[1].ToString();
                        }
                        colores[contaForeach] = Color.FromArgb(Convert.ToInt32(fila[3].ToString()), Convert.ToInt32(fila[4].ToString()), Convert.ToInt32(fila[5].ToString()));
                        contaForeach++;
                    }
                    string[] tempYValues = aseguradorasStringInt.Split(';');
                    string[] xValues = aseguradorasStringText.Split(';');
                    double[] yValues = new double[xValues.Length];
                    for (contaForeach = 0; contaForeach < xValues.Length; contaForeach++)
                    {
                        yValues[contaForeach] = Convert.ToDouble(tempYValues[contaForeach]);
                    }

                    /*Chart2.Series["Default"].Points.DataBindXY(xValues, yValues);

                    Chart2.Series["Default"].Label = "#VALY";

                    Chart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                    Chart2.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                    Chart2.Legends.Clear();

                    for (contaForeach = 0; contaForeach < xValues.Length; contaForeach++)
                        Chart2.Series["Default"].Points[contaForeach].Color = colores[contaForeach];

                    Chart2.Series["Default"].ChartType = SeriesChartType.Bar;

                    if (xValues.Length != 0)
                        Chart2.Visible = true;
                    else
                        Chart2.Visible = false;*/


                    procesaTiles(aseguradorasData);

                }
                //else
                    //Chart2.Visible = false;
            }
            catch (Exception ex)
            {
                //Chart2.Visible = false;
            }
        }
    }

    private void procesaTiles(DataSet aseguradorasData)
    {
        DataList1.DataSource = aseguradorasData;
        DataList1.DataBind();
    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        string rojo = DataBinder.Eval(e.Item.DataItem, "rgb_r").ToString();
        string verde = DataBinder.Eval(e.Item.DataItem, "rgb_g").ToString();
        string azul = DataBinder.Eval(e.Item.DataItem, "rgb_b").ToString();
        HtmlControl indicador = (HtmlControl)e.Item.FindControl("cuadro");
        indicador.Attributes["style"] = "background-color:rgb(" + Convert.ToInt32(rojo) + "," + Convert.ToInt32(verde) + "," + Convert.ToInt32(azul) + ");";
    }

    protected void lnkProceso(object sender, EventArgs e)
    {
        lblError.Text = "";
        LinkButton btn = (LinkButton)sender;
        string[] argumentos = btn.CommandArgument.ToString().Split(new char[] { ';' });
        RadGrid1.DataSource = cargaFechas(Convert.ToInt32(argumentos[0].ToString()), Convert.ToInt32(argumentos[1].ToString()));
        RadGrid1.DataBind();
        procesaOrdenes();
        aseguradoras();
        Session["cliente"] = argumentos[0];
        Session["opcion"] = argumentos[1];
    }

    protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!e.IsFromDetailTable)
            (sender as RadGrid).DataSource = cargaFechas(0, 0);
    }

    private DataTable cargaFechas(int cliente, int proceso)
    {
        DateTime fechaIni, fechaFin;
        DataTable dt = new DataTable();
        DataTable dtd = new DataTable();
        dtd.Columns.Add("fecha");
        dtd.Columns.Add("dw");
        try
        {
            fechaIni = Convert.ToDateTime(txtFechaIni.Text);
            fechaFin = Convert.ToDateTime(txtFechaFin.Text);
            dt = null;
            if (proceso == 0)
            {
                try
                {
                    validaciones.fechaInicial = fechaIni;
                    validaciones.fechaFinal = fechaFin;
                    int valido = validaciones.validaFechas();
                    if (valido == 1)
                        lblError.Text = "La fecha inicial no puede ser mayor que la final";
                    else if (valido == 2)
                    {
                        TimeSpan ts = fechaFin - fechaIni;
                        int dias = ts.Days;
                        if (dias < 8)
                        {
                            if (obtieneDia(fechaIni.DayOfWeek) != 2)
                            {
                                dt = null;
                                procesaOrdenes();
                            }
                            else
                            {
                                DateTime fecha = fechaIni;
                                while (fecha < fechaFin.AddDays(1))
                                {
                                    int dia = obtieneDia(fecha.DayOfWeek);
                                    string dw = obtieneDiaSemana(fecha.DayOfWeek);
                                    if (dia != 1)
                                    {
                                        DataRow row = dtd.NewRow();
                                        row["fecha"] = fecha.ToString("dd/MM/yyyy");
                                        row["dw"] = dw;
                                        dtd.Rows.Add(row);
                                    }
                                    fecha = fecha.AddDays(1);
                                }
                                dt = dtd;
                            }
                        }
                        else
                            dt = null;
                    }
                    else
                        lblError.Text = "No es posible validar las fechas, por favor verifique";
                }
                catch (Exception)
                {
                    dt = null;
                }
            }
            else if (proceso == 1)
            {
                string sql = string.Format("select distinct tabla.dia,case tabla.dia WHEN 1 THEN 'Domingo' WHEN 2 THEN 'Lunes' WHEN 3 THEN 'Martes' WHEN 4 THEN 'Miercoles' WHEN 5 THEN 'Jueves' WHEN 6 THEN 'Viernes' WHEN 7 THEN 'Sabado' else '' end as diaSemana from(" +
    "select case when orp.id_localizacion=3 then datepart(dw,'{2}') when orp.id_localizacion=4 then datepart(dw,so.f_entrega_estimada) when (orp.id_localizacion<>4 or orp.id_localizacion<>3) and so.f_entrega_estimada<'{2}' then datepart(dw,'{2}') else datepart(dw,'{2}') end as dia," +
    "orp.no_orden,upper(tv.descripcion+' '+m.descripcion+' '+tu.descripcion) as tipo_auto,orp.placas,upper(v.color_ext) as color,upper(c.razon_social) as cliente,case  when orp.id_localizacion=4 then so.f_entrega_estimada when orp.id_localizacion=3 then so.f_entrega_estimada " +
    "when (orp.id_localizacion<>4 or orp.id_localizacion<>3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha,orp.id_localizacion,l.descripcion as localizacion,orp.avance_orden,orp.fase_orden,so.f_entrega_estimada,so.f_terminado,so.f_tocado,so.f_entrega,c.id_cliprov " +
    "from ordenes_reparacion orp " +
    "left join seguimiento_orden so on so.id_empresa=orp.id_empresa and so.id_taller=orp.id_taller and so.no_orden=orp.no_orden " +
    "left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo " +
    "left join Marcas m on m.id_marca=orp.id_marca " +
    "left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo " +
    "left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad " +
    "left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C' " +
    "left join localizaciones l on l.id_localizacion=orp.id_localizacion " +
    "where orp.id_empresa={0} and orp.id_taller={1} and orp.status_orden in ('A','T') and ((orp.avance_orden between 80 and 90 and orp.id_localizacion<>4 and so.f_entrega_estimada between '{2}' and '{3}') or so.f_entrega_estimada between '{2}' and '{3}' or so.f_entrega_estimada < '{2}') " +
    ") as tabla where tabla.fecha<>'1900-01-01' and tabla.id_cliprov={4} order by tabla.dia", Request.QueryString["e"], Request.QueryString["t"], txtFechaIni.Text, txtFechaFin.Text, cliente);
                dt = GetData(sql);

                DateTime fecha = Convert.ToDateTime(txtFechaIni.Text);
                while (fecha < fechaFin.AddDays(1))
                {
                    int dia = obtieneDia(fecha.DayOfWeek);
                    string dw = obtieneDiaSemana(fecha.DayOfWeek);
                    if (dia != 1)
                    {
                        bool esDia = false;
                        foreach (DataRow r in dt.Rows)
                        {
                            if (dia == Convert.ToInt32(r[0].ToString()))
                            {
                                esDia = true;
                                break;
                            }
                        }
                        if (esDia)
                        {
                            DataRow row = dtd.NewRow();
                            row["fecha"] = fecha.ToString("dd/MM/yyyy");
                            row["dw"] = dw;
                            dtd.Rows.Add(row);
                        }
                    }
                    fecha = fecha.AddDays(1);
                }
                dt = dtd;
            }
            else if (proceso == 2)
            {

                string condicion = "";
                if (cliente == 1) { condicion = string.Format("and tabla.id_localizacion not in(3,4) and tabla.f_entrega_estimada<'{0}'", txtFechaIni.Text); }
                else if (cliente == 2) { condicion = string.Format("and tabla.f_entrega_estimada = dateadd(d,3,'{0}')", fechas.obtieneFechaLocal().ToString("yyyy-MM-dd")); }
                else if (cliente == 3) { condicion = string.Format("and tabla.id_localizacion=3"); }
                else if (cliente == 4) { condicion = string.Format("and tabla.id_localizacion=4"); }
                else if (cliente == 5) { condicion = string.Format("and tabla.id_localizacion not in (3,4) and (tabla.f_tocado <> '1900-01-01' and tabla.f_tocado is not null) and tabla.f_entrega_estimada between '{0}' and '{1}'", txtFechaIni.Text, txtFechaFin.Text); }

                string sql = string.Format("select distinct tabla.dia,case tabla.dia WHEN 1 THEN 'Domingo' WHEN 2 THEN 'Lunes' WHEN 3 THEN 'Martes' WHEN 4 THEN 'Miercoles' WHEN 5 THEN 'Jueves' WHEN 6 THEN 'Viernes' WHEN 7 THEN 'Sabado' else '' end as diaSemana from(" +
    "select case when orp.id_localizacion=3 then datepart(dw,'{2}') when orp.id_localizacion=4 then datepart(dw,so.f_entrega_estimada) when (orp.id_localizacion<>4 or orp.id_localizacion<>3) and so.f_entrega_estimada<'{2}' then datepart(dw,'{2}') else datepart(dw,'{2}') end as dia," +
    "orp.no_orden,upper(tv.descripcion+' '+m.descripcion+' '+tu.descripcion) as tipo_auto,orp.placas,upper(v.color_ext) as color,upper(c.razon_social) as cliente,case  when orp.id_localizacion=4 then so.f_entrega_estimada when orp.id_localizacion=3 then so.f_entrega_estimada " +
    "when (orp.id_localizacion<>4 or orp.id_localizacion<>3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha,orp.id_localizacion,l.descripcion as localizacion,orp.avance_orden,orp.fase_orden,so.f_entrega_estimada,so.f_terminado,so.f_tocado,so.f_entrega,c.id_cliprov " +
    "from ordenes_reparacion orp " +
    "left join seguimiento_orden so on so.id_empresa=orp.id_empresa and so.id_taller=orp.id_taller and so.no_orden=orp.no_orden " +
    "left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo " +
    "left join Marcas m on m.id_marca=orp.id_marca " +
    "left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo " +
    "left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad " +
    "left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C' " +
    "left join localizaciones l on l.id_localizacion=orp.id_localizacion " +
    "where orp.id_empresa={0} and orp.id_taller={1} and orp.status_orden in ('A','T') and ((orp.avance_orden between 80 and 90 and orp.id_localizacion<>4 and so.f_entrega_estimada between '{2}' and '{3}') or so.f_entrega_estimada between '{2}' and '{3}' or so.f_entrega_estimada < '{2}') " +
    ") as tabla where tabla.fecha<>'1900-01-01' " + condicion + " order by tabla.dia", Request.QueryString["e"], Request.QueryString["t"], txtFechaIni.Text, txtFechaFin.Text);
                dt = GetData(sql);

                DateTime fecha = Convert.ToDateTime(txtFechaIni.Text);
                while (fecha < fechaFin.AddDays(1))
                {
                    int dia = obtieneDia(fecha.DayOfWeek);
                    string dw = obtieneDiaSemana(fecha.DayOfWeek);
                    if (dia != 1)
                    {
                        bool esDia = false;
                        foreach (DataRow r in dt.Rows)
                        {
                            if (dia == Convert.ToInt32(r[0].ToString()))
                            {
                                esDia = true;
                                break;
                            }
                        }
                        if (esDia)
                        {
                            DataRow row = dtd.NewRow();
                            row["fecha"] = fecha.ToString("dd/MM/yyyy");
                            row["dw"] = dw;
                            dtd.Rows.Add(row);
                        }
                    }
                    fecha = fecha.AddDays(1);
                }
                dt = dtd;
            }
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {

        if (RadGrid1.MasterTableView.Items.Count > 0)
        {
            for (int i = 0; i < RadGrid1.MasterTableView.Items.Count; i++)
            {
                RadGrid1.MasterTableView.Items[i].Expanded = true;
            }
        }

    }

    protected void RadGrid1_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        switch (e.DetailTableView.Name)
        {
            case "ingreso":
                {
                    string fecha = dataItem.GetDataKeyValue("fecha").ToString();
                    fecha = Convert.ToDateTime(fecha).ToString("yyyy-MM-dd");
                    int dia = obtieneDia(Convert.ToDateTime(fecha).DayOfWeek);
                    string cliente = (string)Session["cliente"];
                    string opcion = (string)Session["opcion"];
                    string condicion = "";
                    if (cliente != "" && cliente != null)
                    {
                        if (opcion != "" && opcion != null)
                        {
                            if (opcion == "0")
                                condicion = " and tabla.id_cliprov=" + cliente + " ";
                            else
                            {
                                if (cliente == "1") { condicion = string.Format("and tabla.id_localizacion not in(3,4) and tabla.f_entrega_estimada<'{0}'", txtFechaIni.Text); }
                                else if (cliente == "2") { condicion = string.Format("and tabla.f_entrega_estimada = dateadd(d,3,'{0}')", fechas.obtieneFechaLocal().ToString("yyyy-MM-dd")); }
                                else if (cliente == "3") { condicion = string.Format("and tabla.id_localizacion=3"); }
                                else if (cliente == "4") { condicion = string.Format("and tabla.id_localizacion=4"); }
                                else if (cliente == "5") { condicion = string.Format("and tabla.id_localizacion not in (3,4) and (tabla.f_tocado <> '1900-01-01' and tabla.f_tocado is not null) and tabla.f_entrega_estimada between '{0}' and '{1}'", txtFechaIni.Text, txtFechaFin.Text); }
                            }
                        }
                    }
                    //DataTable dataT = new DataTable();
                   /* DataTable dataT = GetData(string.Format("select tabla.dia," +
                        "case tabla.dia WHEN 1 THEN 'Domingo' WHEN 2 THEN 'Lunes' WHEN 3 THEN 'Martes' WHEN 4 THEN 'Miercoles' WHEN 5 THEN 'Jueves' WHEN 6 THEN 'Viernes' WHEN 7 THEN 'Sabado' else '' end as diaSemana," +
                        "tabla.no_orden,tabla.tipo_auto,tabla.placas,tabla.color,tabla.cliente,tabla.id_localizacion,tabla.localizacion,tabla.fecha,tabla.avance_orden,tabla.fase_orden," +
                        "tabla.f_terminado,tabla.f_tocado,tabla.f_entrega,tabla.f_entrega_estimada " +
                        "from(" +
                        "select case when orp.id_localizacion=3 then datepart(dw,'{4}') when orp.id_localizacion=4 then datepart(dw,so.f_entrega_estimada) when (orp.id_localizacion<>4 or orp.id_localizacion<>3) and so.f_entrega_estimada<'{4}' then datepart(dw,'{4}') else datepart(dw,'{4}') end as dia," +
                        "orp.no_orden,upper(tv.descripcion+' '+m.descripcion+' '+tu.descripcion) as tipo_auto,orp.placas," +
                        "upper(v.color_ext) as color,upper(c.razon_social) as cliente," +
                        "case  when orp.id_localizacion=4 then so.f_entrega_estimada when orp.id_localizacion=3 then so.f_entrega_estimada " +
                        "when (orp.id_localizacion<>4 or orp.id_localizacion<>3) then so.f_entrega_estimada else so.f_entrega_estimada end as fecha," +
                        "orp.id_localizacion,l.descripcion as localizacion,orp.avance_orden,orp.fase_orden," +
                        "so.f_entrega_estimada,so.f_terminado,so.f_tocado,so.f_entrega,c.id_cliprov " +
                        "from ordenes_reparacion orp " +
                        "left join seguimiento_orden so on so.id_empresa=orp.id_empresa and so.id_taller=orp.id_taller and so.no_orden=orp.no_orden " +
                        "left join Vehiculos v on v.id_marca=orp.id_marca and v.id_tipo_vehiculo=orp.id_tipo_vehiculo and v.id_tipo_unidad=orp.id_tipo_unidad and v.id_vehiculo=orp.id_vehiculo " +
                        "left join Marcas m on m.id_marca=orp.id_marca " +
                        "left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo=orp.id_tipo_vehiculo " +
                        "left join Tipo_Unidad tu on tu.id_marca=orp.id_marca and tu.id_tipo_vehiculo=orp.id_tipo_vehiculo and tu.id_tipo_unidad=orp.id_tipo_unidad " +
                        "left join Cliprov c on c.id_cliprov=orp.id_cliprov and c.tipo=orp.tipo_cliprov and c.tipo='C' " +
                        "left join localizaciones l on l.id_localizacion=orp.id_localizacion " +
                        "where orp.id_empresa={0} and orp.id_taller={1} " +
                        "and orp.status_orden in ('A','T') and " +
                        "((orp.avance_orden between 80 and 90 and orp.id_localizacion<>4 and so.f_entrega_estimada between '{4}' and '{5}') " +
                        "or so.f_entrega_estimada between '{4}' and '{5}' or so.f_entrega_estimada < '{4}')" +
                        ") as tabla where tabla.dia={3} and tabla.fecha<>'1900-01-01' " + condicion +
                        " order by tabla.dia,tabla.no_orden", Request.QueryString["e"], Request.QueryString["t"], fecha, dia, txtFechaIni.Text, txtFechaFin.Text));

                    */


                    DataTable dataT = GetData(string.Format("declare @fecha date declare @dia int set @fecha = '{2}' set @dia = {3} " +
"select * from(select case when tabla.fecha <= '{4}' then 2 else datepart(dw, tabla.fecha) end as dia," +
"case (case when tabla.fecha <= '{4}' then 2 else datepart(dw, tabla.fecha) end) WHEN 1 THEN 'Domingo' WHEN 2 THEN 'Lunes' WHEN 3 THEN 'Martes' WHEN 4 THEN 'Miercoles' WHEN 5 THEN 'Jueves' WHEN 6 THEN 'Viernes' WHEN 7 THEN 'Sabado' else '' end as diaSemana," +
"tabla.no_orden,tabla.tipo_auto,tabla.placas,tabla.color,tabla.cliente,tabla.id_localizacion,tabla.localizacion,tabla.fecha,tabla.avance_orden,tabla.fase_orden,tabla.f_terminado,tabla.f_tocado,tabla.f_entrega,tabla.f_entrega_estimada " +
"from(select orp.no_orden, upper(tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as tipo_auto, orp.placas, upper(v.color_ext) as color, upper(c.razon_social) as cliente," +
"case when orp.id_localizacion = 4 then so.f_entrega_estimada when orp.id_localizacion = 3 then so.f_entrega_estimada when(orp.id_localizacion <> 4 and orp.id_localizacion <> 3) and so.f_entrega_estimada <= @fecha then so.f_entrega_estimada else '1900-01-01' end as fecha," +
"orp.id_localizacion,l.descripcion as localizacion,orp.avance_orden,orp.fase_orden,so.f_entrega_estimada,so.f_terminado,so.f_tocado,so.f_entrega,c.id_cliprov from ordenes_reparacion orp " +
"left join seguimiento_orden so on so.id_empresa = orp.id_empresa and so.id_taller = orp.id_taller and so.no_orden = orp.no_orden " +
"left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo " +
"left join Marcas m on m.id_marca = orp.id_marca " +
"left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo " +
"left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
"left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = orp.tipo_cliprov and c.tipo = 'C' " +
"left join localizaciones l on l.id_localizacion = orp.id_localizacion " +
"where orp.id_empresa = {0} and orp.id_taller = {1} and orp.status_orden in ('A','T') and ((orp.avance_orden between 80 and 90 and orp.id_localizacion <> 4 and so.f_entrega_estimada between '{4}' and '{5}') or so.f_entrega_estimada between '{4}' and '{5}' or so.f_entrega_estimada < @fecha)) as tabla where " +
"tabla.fecha <> '1900-01-01' " + condicion + ") as tablota where tablota.dia = @dia order by  tablota.dia,tablota.no_orden", Request.QueryString["e"], Request.QueryString["t"], fecha, dia, txtFechaIni.Text, txtFechaFin.Text));




                    e.DetailTableView.DataSource = dataT;





                    llenaDataTableImpresion(dataT);
                    procesaOrdenes();
                    aseguradoras();
                    break;
                }
            default:
                procesaOrdenes();
                aseguradoras();
                break;

        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            procesaOrdenes();
            aseguradoras();
        }
    }

    private void llenaDataTableImpresion(DataTable dataSource)
    {
        ListaImpresionSemanal listaCargaSemanal = new ListaImpresionSemanal();
        List<ListaImpresionSemanal> listaImpresion = new List<ListaImpresionSemanal>();
        if (Session["listaImpresion"] != null)
            listaImpresion = (List<ListaImpresionSemanal>)Session["listaImpresion"];
        string diasemana = "";
        foreach (DataRow row in dataSource.Rows)
        {
            listaCargaSemanal = new ListaImpresionSemanal();
            listaCargaSemanal.Dia = row[0].ToString();
            listaCargaSemanal.DiaSemana = row[1].ToString();
            listaCargaSemanal.No_orden = row[2].ToString();
            listaCargaSemanal.Tipo_auto = row[3].ToString();
            listaCargaSemanal.Placas = row[4].ToString();
            listaCargaSemanal.Color = row[5].ToString();
            listaCargaSemanal.Cliente = row[6].ToString();
            listaCargaSemanal.Id_localizacion = row[7].ToString();
            listaCargaSemanal.Localizacion = row[8].ToString();
            listaCargaSemanal.Fecha = row[9].ToString();
            listaCargaSemanal.Avance_orden = row[10].ToString();
            listaCargaSemanal.Fase_orden = row[11].ToString();
            listaCargaSemanal.F_terminado = row[12].ToString();
            listaCargaSemanal.F_tocado = row[13].ToString();
            listaCargaSemanal.F_entrega = row[14].ToString();
            listaCargaSemanal.F_entrega_estimada = row[15].ToString();
            diasemana = row[1].ToString();
            listaImpresion.Add(listaCargaSemanal);           
        }
        Session["listaImpresion"] = listaImpresion;
    }

    protected void lnkImprimir_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (Session["listaImpresion"] != null)
        {
            listaImpresion = (List<ListaImpresionSemanal>)Session["listaImpresion"];
            if (listaImpresion.Count > 0)
            {
                int[] sesiones = { Convert.ToInt32(Request.QueryString["o"]), Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(Request.QueryString["t"]) };
                DatosImpresionSemanal imprime = new DatosImpresionSemanal();
                string archivo = imprime.GenProgSemanal(sesiones, (List<ListaImpresionSemanal>)Session["listaImpresion"], txtFechaIni.Text, txtFechaFin.Text);
                try
                {
                    if (archivo != "")
                    {
                        FileInfo filename = new FileInfo(archivo);
                        if (filename.Exists)
                        {
                            string url = "Descargas.aspx?filename=" + filename.Name;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, fullscreen=yes,resizable=yes');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error al descargar el archivo: " + ex.Message;
                }
            }
            else
                lblError.Text = "La opción seleccionada no genro elementos para su impresión, verifique la opción a imprimir";
        }
        else
        {
            RadGrid1.DataSource = cargaFechas(0, 0);
            lblError.Text = "La Programación Semanal se reinicio, se perdio la impresión , vuelva a cargar los parametros parametros para generar la Programación Semanal.";
        }
    }
}