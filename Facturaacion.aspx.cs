using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using E_Utilities;
using TimbradoRV;
using System.IO;

public partial class Facturaacion : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();
    public DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            borratmp();
            if (Convert.ToInt32(Request.QueryString["fact"]) != 0)
            {
                int[] sesiones = obtieneSesiones();
                FacturacionElectronica.Facturas factura = new FacturacionElectronica.Facturas();
                factura.idCfd = Convert.ToInt32(Request.QueryString["fact"]);
                factura.obtieneDetalle();
                object[] conceptos = factura.info;
                int filasElim = 0;
                using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
                {
                    try
                    {
                        conLoc.Open();
                        string qryBorra = "DELETE FROM DocumentoCfdiTmp WHERE IdUsuario = " + sesiones[0] + " delete from DocumentoCfdiTmp_fact where idusuario= " + sesiones[0];
                        SqlCommand comLoc = new SqlCommand(qryBorra, conLoc);
                        using (comLoc)
                        {
                            filasElim = comLoc.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex) { filasElim = 0; lblError.Text = "Error: " + ex.Message; }
                }
                if (filasElim > -1)
                {
                    if (Convert.ToBoolean(conceptos[0]))
                    {
                        DataSet conceptosFacturar = (DataSet)conceptos[1];
                        int filas = 1;
                        foreach (DataRow r in conceptosFacturar.Tables[0].Rows)
                        {
                            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
                            {
                                try
                                {
                                    conLoc.Open();
                                    string qryInserta = "INSERT INTO DocumentoCfdiTmp (IdFila, IdUsuario, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera) " +
                                        "VALUES (" + filas + "," + sesiones[0] + ", '" + r[0].ToString() + "', '" + r[1].ToString() + "', " + r[2].ToString() + ", " + r[3].ToString() + ", " + Math.Round(Convert.ToDecimal(r[4].ToString()), 2) + ", " + Math.Round(Math.Round(Convert.ToDecimal(r[4].ToString()), 2) * Convert.ToDecimal(r[2].ToString()), 2) + ", " + r[6].ToString() + ", " + Math.Round(Convert.ToDecimal(r[7].ToString()), 2) + ", " + r[8].ToString() + "," + r[9].ToString() + ", " + r[10].ToString() + ", " + r[11].ToString() + ", " + r[12].ToString() + ", " + r[13].ToString() + ", " + r[14].ToString() + ", " + r[15].ToString() + ", " + r[16].ToString() + ", " + r[17].ToString() + ",convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120))";
                                    SqlCommand comLoc = new SqlCommand(qryInserta, conLoc);
                                    using (comLoc)
                                    {
                                        comLoc.CommandText = qryInserta;
                                        int ok = comLoc.ExecuteNonQuery();
                                    }
                                    conLoc.Close();
                                }
                                catch (Exception ex)
                                {
                                    lblError.Text = "Error LocalDB insersion detalle: " + ex.Source + " - " + ex.Message;
                                }
                                filas++;
                            }
                        }
                    }
                    else
                        lblError.Text = "Error: " + Convert.ToString(conceptos[1]);
                }
            }
        }
    }

    private void borratmp()
    {
        int[] sesiones = obtieneSesiones();
        int filasElim = 0;
        using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
        {
            try
            {
                conLoc.Open();
                string qryBorra = "DELETE FROM DocumentoCfdiTmp WHERE IdUsuario = " + sesiones[0] + " delete from DocumentoCfdiTmp_fact where idusuario= " + sesiones[0];
                SqlCommand comLoc = new SqlCommand(qryBorra, conLoc);
                using (comLoc)
                {
                    filasElim = comLoc.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { filasElim = 0; lblError.Text = "Error: " + ex.Message; }
        }
    }

    protected void lnkCargarInfo_Click(object sender, EventArgs e)
    {
        lblError.Text = "";        
        try
        {
            int[] sesiones = obtieneSesiones();
            object[] conceptos = recepciones.obtieneInfoFacturar(sesiones, "1");
            int filasElim = 0;
            using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
            {
                try
                {
                    conLoc.Open();
                    string qryBorra = "DELETE FROM DocumentoCfdiTmp WHERE IdUsuario = " + sesiones[0] + " delete from DocumentoCfdiTmp_fact where idusuario= " + sesiones[0];
                    SqlCommand comLoc = new SqlCommand(qryBorra, conLoc);
                    using (comLoc)
                    {
                        filasElim = comLoc.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { filasElim = 0; lblError.Text = "Error: " + ex.Message; }
            }
            if (filasElim > -1)
            {
                if (Convert.ToBoolean(conceptos[0]))
                {
                    DataSet conceptosFacturar = (DataSet)conceptos[1];
                    int filas = 1;
                    foreach (DataRow r in conceptosFacturar.Tables[0].Rows)
                    {
                        using (SqlConnection conLoc = new SqlConnection(ConfigurationManager.ConnectionStrings["connStringCfdiTemp"].ConnectionString))
                        {
                            try
                            {
                                FacturacionElectronica.Unidades unidad = new FacturacionElectronica.Unidades();
                                unidad.descUnid = r[3].ToString().ToUpper().Trim();
                                unidad.obtieneIdUnidad();
                                object[] unidadades = unidad.info;
                                int idUnidad = 0;
                                try
                                {
                                    if (Convert.ToBoolean(unidadades[0]))
                                        idUnidad = Convert.ToInt32(unidadades[1]);
                                    else
                                        idUnidad = 0;
                                }
                                catch (Exception) { idUnidad = 0; }

                                conLoc.Open();
                                string qryInserta = "INSERT INTO DocumentoCfdiTmp (IdFila, IdUsuario, txtIdent, txtConcepto, radnumCantidad, ddlUnidad, txtValUnit, lblImporte, txtPtjeDscto, txtDscto, lblSubTotal, ddlIvaTras, ddlIeps, lblIvaTras, lblIeps, ddlIvaRet, ddlIsrRet, lblIvaRet, lblIsrRet, lblTotal, EncFechaGenera) " +
                                    "VALUES (" + filas + "," + sesiones[0] + ", '" + r[0].ToString().Trim() + "', '" + r[1].ToString().Trim() + "', " + r[2].ToString() + ", " + idUnidad + ", " + Convert.ToDecimal(r[4]).ToString("F2") + ", " + Convert.ToDecimal(r[5]).ToString("F2") + ", 0, 0, " + Convert.ToDecimal(r[5]).ToString("F2") + ",2, 0, " + r[7].ToString() + ", 0, 0, 0, 0, 0, " + r[8].ToString() + ", convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120))";
                                SqlCommand comLoc = new SqlCommand(qryInserta, conLoc);
                                using (comLoc)
                                {
                                    comLoc.CommandText = qryInserta;
                                    int ok = comLoc.ExecuteNonQuery();
                                }
                                conLoc.Close();
                            }
                            catch (Exception ex)
                            {
                                lblError.Text = "Error LocalDB insersion detalle tipo factura: " + ex.Source + " - " + ex.Message;
                            }
                            filas++;
                        }
                    }
                                        
                    RadGrid1.DataBind();
                    RadGrid2.DataBind();
                }
                else
                    lblError.Text = "Error: " + Convert.ToString(conceptos[1]);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.ToString();
        }
    }
       
    protected void lnkAsignaFacturas_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        LinkButton btn = (LinkButton)sender;
        string argumentos = btn.CommandArgument.ToString();
        bool quitaProd = recepciones.agregaProducto(sesiones[0],argumentos);
        if (!quitaProd)
           lblError.Text = "No se logro agregar el concepto a la factura";
        RadGrid1.DataBind();
        RadGrid2.DataBind();


    }
    protected void lnkDelProv_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        LinkButton lnkDelProv = (LinkButton)sender;
        string argumentos = lnkDelProv.CommandArgument.ToString();        
        bool quitaProd = recepciones.quitaProductoFactura(sesiones[0], argumentos);
        if (!quitaProd)
           lblError.Text = "No se logro quitar el concepto de la factura.";
        RadGrid1.DataBind();
        RadGrid2.DataBind();
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


    protected void lnkAgregarTodo_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();        
        bool quitaProd = recepciones.agregarTodo(sesiones[0]);
        if (!quitaProd)
            lblError.Text = "No se logro agregar el o los conceptos a la factura.";
        RadGrid1.DataBind();
        RadGrid2.DataBind();
    }

    protected void lnkQuitarTodo_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        bool quitaProd = recepciones.quitarTodo(sesiones[0]);
        if (!quitaProd)
            lblError.Text = "No se logro quitar el o los conceptos de la factura.";
        RadGrid1.DataBind();
        RadGrid2.DataBind();
    }

    protected void lnkFacturar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Facturacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&fact=" + Request.QueryString["fact"] + "&refct=" + Request.QueryString["refct"] + "&prev=1");
    }
}