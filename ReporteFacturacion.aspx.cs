using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReporteFacturacion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            DateTime fechaIni = new E_Utilities.Fechas().obtieneFechaLocal();
            DateTime fechaFin = new E_Utilities.Fechas().obtieneFechaLocal();
            
            txtFechaIni.Text = fechaIni.ToString("yyyy-MM-dd");
            txtFechaFin.Text = fechaFin.ToString("yyyy-MM-dd");
            lnkDescarga.Visible = false;
            cargaInfo();
        }
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
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "ReporteFacturacion.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
    protected void lnkDescarga_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int[] sesiones = obtieneSesiones();
        ImprimirReporteFactu facturacion = new ImprimirReporteFactu();
        facturacion.usuario = sesiones[0];
        facturacion.empresa = sesiones[2];
        facturacion.taller = sesiones[3];
        facturacion.Ini = txtFechaIni.Text;
        facturacion.Fin = txtFechaFin.Text;
        facturacion.generaReporte();
        string archivo = facturacion.archivo;
        if (archivo != "")
        {
            try
            {
                FileInfo docto = new FileInfo(archivo);
                if (docto.Exists)
                {
                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + docto.Name);
                    Response.WriteFile(archivo);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al accesar al archivo en el servidor. Detalle: " + ex.Message;
            }
        }
        else
            lblError.Text = "No se puedo generar el documento por favor vuelva a intentar";
    }


    private DataTable generaTablaVacia() {
        DataTable dt = new DataTable();
        dt.Columns.Add("RFC");
        dt.Columns.Add("Nombre");
        dt.Columns.Add("UUID");
        dt.Columns.Add("Folio");
        dt.Columns.Add("Factura");
        dt.Columns.Add("Fecha_Crea");
        dt.Columns.Add("Fecha_Genera");
        dt.Columns.Add("Fecha_Cancela");
        dt.Columns.Add("Neto");
        dt.Columns.Add("Descuento_Global");
        dt.Columns.Add("Subtotal");
        dt.Columns.Add("Descuento");
        dt.Columns.Add("Traslados");
        dt.Columns.Add("Retenciones");
        dt.Columns.Add("Total");
        dt.Columns.Add("Taller_Tienda");
        dt.Columns.Add("Orden_Ticket");
        dt.Columns.Add("Marca");
        dt.Columns.Add("Modelo");
        dt.Columns.Add("Color");
        dt.Columns.Add("Placas");
        dt.Columns.Add("Sinistero");
        dt.Columns.Add("Monto_Pago");
        dt.Columns.Add("Fecha_Pago");
        dt.Columns.Add("Notas");
        dt.Columns.Add("Estatus");
        return dt;
    }

    private void cargaInfo() {
        DateTime fechaIni = Convert.ToDateTime(txtFechaIni.Text);
        DateTime fechaFIn = Convert.ToDateTime(txtFechaFin.Text);

        if (fechaFIn < fechaIni)
            lblError.Text = "Debe indicar una fecha final mayor a la inicial";
        else if (fechaIni > fechaFIn)
            lblError.Text = "Debe indicar una fecha inicial menor a la final";
        else
        {
            int[] sesiones = obtieneSesiones();
            ReporFactu facturacion = new ReporFactu();
            facturacion.empresa = sesiones[2];
            facturacion.taller = sesiones[3];
            facturacion.usuario = sesiones[0];
            facturacion.Ini = fechaIni.ToString("yyyy-MM-dd");
            facturacion.Fin = fechaFIn.ToString("yyyy-MM-dd");
            facturacion.gravaPrametros();
            facturacion.obtieneCFD();
            object[] retorno = facturacion.retorno;
            if (Convert.ToBoolean(retorno[0]))
            {
                try
                {
                    DataTable dt = (DataTable)retorno[1];
                    RadGrid1.DataSource = dt;
                    RadGrid1.DataBind();
                    lnkDescarga.Visible = true;
                }
                catch (Exception ex) { lblError.Text = "Error al obtener información. " + ex.Message; RadGrid1.DataSource = null; RadGrid1.DataBind(); lnkDescarga.Visible = false; }
            }
            else
            {
                lblError.Text = Convert.ToString(retorno[1]);
                RadGrid1.DataSource = generaTablaVacia(); RadGrid1.DataBind();
                lnkDescarga.Visible = false;
            }
        }
    }

    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
   /*     if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
        {
            string estatus = DataBinder.Eval(e.Item.DataItem, "Estatus").ToString();
            switch (estatus)
            {
                case "C":
                    e.Item.BackColor = System.Drawing.Color.Red;
                    break;
                case "P":
                    e.Item.BackColor = System.Drawing.Color.Yellow;
                    break;
                default:
                    e.Item.BackColor = System.Drawing.Color.White;
                    break;
            }
        }*/
    }

    protected void lnkTodo_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int[] sesiones = obtieneSesiones();
        ImprimirReporteFactu facturacion = new ImprimirReporteFactu();
        facturacion.usuario = sesiones[0];
        facturacion.empresa = sesiones[2];
        facturacion.taller = sesiones[3];
        facturacion.Ini = "2016-01-01";
        facturacion.Fin = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");

        ReporFactu rep = new ReporFactu();
        rep.empresa = facturacion.empresa;
        rep.taller =facturacion.taller;
        rep.usuario = facturacion.usuario;
        rep.Ini = facturacion.Ini;
        rep.Fin = facturacion.Fin;
        rep.gravaPrametros();
        facturacion.generaReporte();
        string archivo = facturacion.archivo;
        if (archivo != "")
        {
            try
            {
                FileInfo docto = new FileInfo(archivo);
                if (docto.Exists)
                {
                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + docto.Name);
                    Response.WriteFile(archivo);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al accesar al archivo en el servidor. Detalle: " + ex.Message;
            }
        }
        else
            lblError.Text = "No se puedo generar el documento por favor vuelva a intentar";
    }

    protected void lnkGeneraInfo_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        cargaInfo();
    }
}