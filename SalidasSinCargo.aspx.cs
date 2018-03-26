using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Data;
using Telerik.Web.UI;
using System.IO;

public partial class SalidasSinCargo : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();
    decimal totalMo, totalRef, totRemision;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargaDatosPie();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "C")
                {
                    RadGrid1.MasterTableView.CommandItemSettings.ShowSaveChangesButton = false;
                    RadGrid1.MasterTableView.CommandItemSettings.ShowCancelChangesButton = false;
                    RadGrid2.MasterTableView.CommandItemSettings.ShowSaveChangesButton = false;
                    RadGrid2.MasterTableView.CommandItemSettings.ShowCancelChangesButton = false;
                    lnkGeneraRemision.Visible = false;
                }
            }
            else
                Response.Redirect("BienvenidaOrdenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
        }
    }

    private string obtieneEstatus()
    {
        string estatus = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            object[] infoEstatus = recepciones.obtieneEstatusOrden(sesiones[2].ToString(), sesiones[3].ToString(), sesiones[4].ToString());
            if (Convert.ToBoolean(infoEstatus[0]))
                estatus = Convert.ToString(infoEstatus[1]);
            else
                estatus = "";
        }
        catch (Exception) { estatus = ""; }
        return estatus;
    }

    private void cargaDatosPie()
    {
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
        object[] datosOrden = recepciones.obtieneInfoOrdenPie(orden, empresa, taller);
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
            Session["paginaOrigen"] = "Remisiones.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
    
    protected void lnkGeneraRemision_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            int[] sesiones = obtieneSesiones();
            object[] completada = recepciones.obtieneTodoAplicadoSalida(sesiones);
            int completados = RadGrid1.Items.Count + RadGrid2.Items.Count;
            if (Convert.ToBoolean(completada[0]))
            {
                if (Convert.ToInt32(completada[1]) != completados)
                    lblError.Text = "No es posible generar la salida ya que existen conceptos de mano de obra o de refacciones aun sin aplicar";
                else
                {
                    if (completados == 0)
                        lblError.Text = "No se puede generar la salida ya que no existen conceptos de mano de obra o de refacciones registrados";
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("registro");
                        dt.Columns.Add("clasificacion");
                        dt.Columns.Add("id_concepto");
                        dt.Columns.Add("descripcion");
                        dt.Columns.Add("cantidad");
                        dt.Columns.Add("costo_unitario");
                        dt.Columns.Add("porc_sobre_costo");
                        dt.Columns.Add("sobre_costo");
                        dt.Columns.Add("venta_unitaria");
                        dt.Columns.Add("importe");
                        int registros = 1;
                        decimal totalManoObra = 0;
                        decimal totalRefacciones = 0;
                        decimal totalTotal = 0;


                        DataSourceSelectArguments args = new DataSourceSelectArguments();
                        DataView view = (DataView)SqlDataSource1.Select(args);
                        DataTable dts = view.ToTable();

                        foreach (DataRow r in dts.Rows)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = registros;
                            dr[1] = "M";
                            dr[2] = Convert.ToInt32(r[0].ToString());
                            dr[3] = r[1].ToString();
                            dr[4] = 1;
                            dr[5] = dr[6] = dr[7] = 0;
                            dr[8] = dr[9] = Convert.ToDecimal(r[2]);
                            dt.Rows.Add(dr);
                            totalManoObra = totalManoObra + Convert.ToDecimal(dr[9]);
                            registros++;
                        }

                        DataSourceSelectArguments args2 = new DataSourceSelectArguments();
                        DataView view2 = (DataView)SqlDataSource2.Select(args2);
                        DataTable dts2 = view2.ToTable();
                        registros = 1;

                        foreach (DataRow r2 in dts2.Rows)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = registros;
                            dr[1] = "R";
                            dr[2] = Convert.ToInt32(r2[0]);
                            dr[3] = r2[2].ToString();
                            dr[4] = Convert.ToInt32(r2[1]);
                            dr[5] = Convert.ToDecimal(r2[3]);
                            dr[6] = Convert.ToDecimal(r2[4]);
                            dr[7] = Convert.ToDecimal(r2[3]) * (Convert.ToDecimal(r2[4]) / 100);
                            dr[8] = Convert.ToDecimal(r2[5]);
                            dr[9] = Convert.ToDecimal(r2[6]);
                            dt.Rows.Add(dr);
                            totalRefacciones = totalRefacciones + Convert.ToDecimal(dr[9]);
                            registros++;
                        }

                        totalTotal = totalManoObra + totalRefacciones;
                        string tipo = "S";
                        object[] generado = recepciones.generarRemisionSS(sesiones, dt, totalManoObra, totalRefacciones, totalTotal, tipo, txtObs.Text);
                        if (Convert.ToBoolean(generado[0]))
                        {
                            string[] argumentos = Convert.ToString(generado[1]).Split(new char[] { ';' });
                            lblError.Text = "Se ha generado la salida " + argumentos[0] + " con número: " + argumentos[1];
                            RadGrid1.Rebind();
                            RadGrid2.Rebind();
                            RadGrid3.Rebind();
                            txtObs.Text = "";
                            Operarios operarios = new Operarios();
                            int[] sessiones = obtieneSesiones();
                            operarios.empresa = sessiones[2];
                            operarios.taller = sessiones[3];
                            operarios.orden = sessiones[4];
                            operarios.liberarCajones();
                            object[] op = operarios.retorno;
                            if (Convert.ToBoolean(op[0]))
                            {
                                DataSet info = (DataSet)op[1];
                                foreach (DataRow r in info.Tables[0].Rows)
                                {
                                    string[] valoresOperarios = new string[5] { Convert.ToString(r[0]), Convert.ToString(r[1]), Convert.ToString(r[2]), Convert.ToString(r[3]), Convert.ToString(r[4]) };
                                    operarios.liberar(sessiones, valoresOperarios);
                                }
                            }
                        }
                        else
                        {
                            RadGrid3.Rebind();
                            RadGrid1.Rebind();
                            RadGrid2.Rebind();
                            //lblError.Text = "No se pudo generar la remisión. Detalle: " + Convert.ToString(generado[1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex) { lblError.Text = "Error al generar la salida. Detalle: " + ex.Message; }
    }

    private decimal convierteAdigitos(string importe)
    {

        decimal total = 0;
        string valor = "";
        for (int j = 0; j < importe.Length; j++)
        {
            if (char.IsDigit(importe[j]))
                valor = valor.Trim() + importe[j];
            else
            {
                if (importe[j] == '.')
                    valor = valor.Trim() + importe[j];
            }
        }
        importe = valor.Trim();
        if (importe == "" || importe == "" || importe == "0")
            total = 0;
        else
        {
            try
            {
                total = Convert.ToDecimal(importe);
            }
            catch (Exception) { total = 0; }
        }

        return total;

    }

    protected void lnkImprime_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        ImpresionRemision imprime = new ImpresionRemision();
        Recepciones recepciones = new Recepciones();

        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);

        string nomTaller = recepciones.obtieneNombreTaller(Request.QueryString["t"]);
        string usuario = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);
        int remision = Convert.ToInt32(RadGrid3.SelectedValue);
        if (remision != 0)
        {
            char detalle = 'S';
            string archivo = imprime.GenRepOrdTrabajo(empresa, taller, orden, nomTaller, usuario, remision, detalle);
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
            lblError.Text = "Debe seleccionar una salida sin cargo para imprimirla";
    }
}