using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class Operacion : System.Web.UI.Page
{
    ManoObraOrden datosMO = new ManoObraOrden();
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargaDatosPie();
            lblErroresOO.Text = "";
            obtieneSesiones();
            GridAsignaciones.Visible = true;
            GridAsignaciones.DataBind();
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

    protected void btnBuscarAsignaciones_Click(object sender, EventArgs e)//juan-14-12-15
    {
        GridOperarios.DataBind();
        GridOperarios.Visible = false;
        if (Request.QueryString["o"].Trim() != "")
        {
            lblErroresOO.Text = "";
            int noOrden, idTaller, idEmpresa;
            noOrden = idTaller = idEmpresa = 0;
            bool error = false;
            try
            {
                noOrden = Convert.ToInt32(Request.QueryString["o"]);
                try
                {
                    idTaller = Convert.ToInt32(Request.QueryString["t"]);
                    idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                }
                catch (Exception) { error = true; }
                if (!error)
                {
                    bool existe = datosMO.existeNoOrden(noOrden, idTaller, idEmpresa);
                    if (existe)
                    {
                        GridAsignaciones.Visible = true;
                        GridAsignaciones.DataBind();
                    }
                    else
                    {
                        GridAsignaciones.Visible = false;
                        lblErroresOO.Text = "El número de orden no existe.";
                    }
                }
            }
            catch (Exception ex)
            {
                GridAsignaciones.Visible = false;
                lblErroresOO.Text = "El número de orden solo puede estar compuesto por numeros.";
            }
        }
        else
        {
            GridAsignaciones.Visible = false;
            lblErroresOO.Text = "Necesita colocar un numero de orden.";
        }
    }

    protected void GridAsignaciones_RowCommand(object sender, GridViewCommandEventArgs e)//juan-14-12-15
    {
        lblErroresOO.Text = "";
        int noOrden, idTaller, idEmpresa, idEmp, idAsignacion, idConsecutivoMo;        
        noOrden = idTaller = idEmpresa = idEmp = idAsignacion = idConsecutivoMo = 0;
        string[] args = e.CommandArgument.ToString().Split(';');       
        if (args.Length == 6)
        {
            noOrden = Convert.ToInt32(args[0]);
            idEmpresa = Convert.ToInt32(args[1]);
            idTaller = Convert.ToInt32(args[2]);
            idEmp = Convert.ToInt32(args[3]);
            idAsignacion = Convert.ToInt32(args[4]);
            idConsecutivoMo = Convert.ToInt32(args[5]);
            GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);           
            GridAsignaciones.DataBind();
        }
    }

    private bool validaFechaFin(string fechaIni, string fechaFin)//22-12-2015
    {
        try
        {
            DateTime ini = Convert.ToDateTime(fechaIni);
            DateTime fin = Convert.ToDateTime(fechaFin);
            if (fin > ini)
                return false;
            else
                return true;
        }
        catch (Exception) { return true; }
    }

    private bool tiempoMenorActual(string tiempo)
    {
        try
        {
            DateTime tiempoTxt = Convert.ToDateTime(tiempo);
            if (tiempoTxt < fechas.obtieneFechaLocal())
                return true;
            else
                return false;
        }
        catch (Exception) { return false; }
    }

    private bool tiempoNull(string tiempo)
    {
        try
        {
            if (tiempo != "" && tiempo != null)
                return true;
            else
                return false;
        }
        catch (Exception) { return false; }
    }

    private bool validaTiempo(string tiempo)
    {
        try
        {
            DateTime.ParseExact(tiempo, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return true;
        }
        catch (Exception)
        {
            try
            {
                if (tiempo.Length == 7)
                    tiempo = tiempo.PadLeft(8, '0');
                DateTime.ParseExact(tiempo, "HH:mm:ss", CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception) { return false; }
        }
    }

    protected void GridOperarios_RowDataBound(object sender, GridViewRowEventArgs e)//juan-9-12-2015
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPichonera = e.Row.FindControl("lblPichonera") as Label;
            Label lblAsigOperMO = e.Row.FindControl("lblAsigOperMO") as Label;
            LinkButton lnkAsigOperMO = e.Row.FindControl("lnkAsigOperMO") as LinkButton;
            int maxAsig = 0;
            int asignaciones = 0;
            int idEmp = 0;
            try
            {
                idEmp = Convert.ToInt32(lnkAsigOperMO.CommandArgument);
                if (lblPichonera.Text != null && lblPichonera.Text != "")
                {
                    asignaciones = datosMO.obtieneAsignaciones(idEmp);
                    maxAsig = Convert.ToInt32(lblPichonera.Text);
                    if (asignaciones < maxAsig && maxAsig != 0)
                        lblAsigOperMO.ForeColor = System.Drawing.Color.Empty;
                    else
                        lblAsigOperMO.ForeColor = System.Drawing.Color.Red;
                }
                else
                    lblAsigOperMO.ForeColor = System.Drawing.Color.Red;
            }
            catch (Exception) { }
        }
    }

    protected void lnkAsigOperMO_Click(object sender, EventArgs e)//juan-9-12-2015
    {
        lblErroresOO.Text = "";
        LinkButton lnkAsigOperMO = (LinkButton)sender;
        bool asigna, actualizado, asignado;
        asigna = actualizado = asignado = false;

        int noOrden, idEmpresa, idTaller, IdEmp, idConsecutivoMO, idAsignacion;
        noOrden = idEmpresa = idTaller = IdEmp = idConsecutivoMO = idAsignacion = 0;
        try
        {
            noOrden = Convert.ToInt32(Request.QueryString["o"]);
            idEmpresa = Convert.ToInt32(lblEmpresaMO.Text);
            idTaller = Convert.ToInt32(lblTallerMO.Text);
            IdEmp = Convert.ToInt32(lnkAsigOperMO.CommandArgument);
            idConsecutivoMO = Convert.ToInt32(lblConsecutivoMO.Text);
            idAsignacion = Convert.ToInt32(lblIdAsignacion.Text);
            asignado = datosMO.operadorMOAsignado(noOrden, idEmpresa, idTaller, idConsecutivoMO);
            if (!asignado)
            {
                asigna = datosMO.asignaOperadorMO(noOrden, idEmpresa, idTaller, IdEmp, idConsecutivoMO);
                if (asigna)
                {
                    GridAsignaciones.DataBind();
                    GridAsignaciones.Visible = true;
                    GridOperarios.DataBind();
                    GridOperarios.Visible = false;
                }
                else
                    lblErroresOO.Text = "Hubo un error inesperdo al asignar el operador, verifique su conexión e intentelo nuevamnete.";
            }
            else
            {
                actualizado = datosMO.actualizaOperadorMO(noOrden, idEmpresa, idTaller, IdEmp, idConsecutivoMO, idAsignacion);
                if (actualizado)
                {
                    GridAsignaciones.DataBind();
                    GridAsignaciones.Visible = true;
                    GridOperarios.DataBind();
                    GridOperarios.Visible = false;
                }
                else
                    lblErroresOO.Text = "Hubo un error inesperdo al actualizar el operador, verifique su conexión e intentelo nuevamnete.";
            }
        }
        catch (Exception)
        { }
    }

    protected void GridAsignaciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                Label lblEstatusDes = e.Row.FindControl("lblEstatusDes") as Label;
                Label lblOperNomDes = e.Row.FindControl("lblOperNomDes") as Label;
                Label lblIdEmp = e.Row.FindControl("lblIdEmpDes") as Label;
                Label lblIdMODes = e.Row.FindControl("lblIdMODes") as Label;
                TextBox txtFechaIniDes = e.Row.FindControl("txtFechaIniDes") as TextBox;
                TextBox txtHoraIniDes = e.Row.FindControl("txtHoraIniDes") as TextBox;
                TextBox txtFechaFinDes = e.Row.FindControl("txtFechaFinDes") as TextBox;
                TextBox txtHoraFinDes = e.Row.FindControl("txtHoraFinDes") as TextBox;
                Label lblFechaIniDes = e.Row.FindControl("lblFechaIniDes") as Label;
                Label lblHoraIniDes = e.Row.FindControl("lblHoraIniDes") as Label;
                Label lblFechaFinDes = e.Row.FindControl("lblFechaFinDes") as Label;
                Label lblHoraFinDes = e.Row.FindControl("lblHoraFinDes") as Label;
                LinkButton btnIniCancelarOper = e.Row.FindControl("btnIniCancelarOper") as LinkButton;
                LinkButton btnIniTiempo = e.Row.FindControl("btnIniTiempo") as LinkButton;
                LinkButton btnFinCancelarOper = e.Row.FindControl("btnFinCancelarOper") as LinkButton;
                LinkButton btnFinTiempo = e.Row.FindControl("btnFinTiempo") as LinkButton;
                LinkButton btnReasignaOper = e.Row.FindControl("btnReasignaOper") as LinkButton;
                LinkButton lnkIni = e.Row.FindControl("lnkFini") as LinkButton;
                LinkButton lnkFin = e.Row.FindControl("lnkFin") as LinkButton;
                string nomOperario = "", estatus = "";
                int idEmpleado = 0;
                estatus = lblEstatusDes.Text;
                int noOrden, idTaller, idEmpresa, idMO;
                noOrden = idTaller = idEmpresa = idMO = 0;
                noOrden = Convert.ToInt32(Request.QueryString["o"]);
                idTaller = Convert.ToInt32(Request.QueryString["t"]);
                idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                idMO = Convert.ToInt32(lblIdMODes.Text);
                int operariosTotalesTeminado = datosMO.obtieneOperariosTerminado(noOrden, idTaller, idEmpresa, idMO);
                if (operariosTotalesTeminado == 0)
                    estatus = "TT";
                try
                {
                    switch (estatus)
                    {
                        case "A":
                            txtFechaIniDes.Visible = true;
                            txtHoraIniDes.Visible = true;
                            txtFechaFinDes.Visible = false;
                            txtHoraFinDes.Visible = false;
                            lblFechaFinDes.Visible = true;
                            lblHoraFinDes.Visible = true;
                            lblFechaIniDes.Visible = false;
                            lblHoraIniDes.Visible = false;
                            btnIniTiempo.Visible = true;
                            btnIniCancelarOper.Visible = false;
                            btnFinCancelarOper.Visible = false;
                            btnFinTiempo.Visible = false;
                            btnReasignaOper.Visible = false;
                            lnkIni.Visible = true;
                            lnkFin.Visible = false;
                            break;
                        case "I":
                            txtFechaIniDes.Visible = false;
                            txtHoraIniDes.Visible = false;
                            txtFechaFinDes.Visible = true;
                            txtHoraFinDes.Visible = true;
                            lblFechaFinDes.Visible = false;
                            lblHoraFinDes.Visible = false;
                            lblFechaIniDes.Visible = true;
                            lblHoraIniDes.Visible = true;
                            btnIniTiempo.Visible = false;
                            btnIniCancelarOper.Visible = true;
                            btnFinCancelarOper.Visible = false;
                            btnFinTiempo.Visible = true;
                            btnReasignaOper.Visible = true;
                            lnkIni.Visible = false;
                            lnkFin.Visible = true;
                            break;
                        case "T":
                            txtFechaIniDes.Visible = false;
                            txtHoraIniDes.Visible = false;
                            txtFechaFinDes.Visible = false;
                            txtHoraFinDes.Visible = false;
                            lblFechaFinDes.Visible = true;
                            lblHoraFinDes.Visible = true;
                            lblFechaIniDes.Visible = true;
                            lblHoraIniDes.Visible = true;
                            btnIniTiempo.Visible = false;
                            btnIniCancelarOper.Visible = false;
                            btnFinCancelarOper.Visible = true;
                            btnFinTiempo.Visible = false;
                            btnReasignaOper.Visible = false;
                            lnkIni.Visible = false;
                            lnkFin.Visible = false;
                            break;
                        case "TT":
                            txtFechaIniDes.Visible = false;
                            txtHoraIniDes.Visible = false;
                            txtFechaFinDes.Visible = false;
                            txtHoraFinDes.Visible = false;
                            lblFechaFinDes.Visible = true;
                            lblHoraFinDes.Visible = true;
                            lblFechaIniDes.Visible = true;
                            lblHoraIniDes.Visible = true;
                            btnIniTiempo.Visible = false;
                            btnIniCancelarOper.Visible = false;
                            btnFinCancelarOper.Visible = false;
                            btnFinTiempo.Visible = false;
                            btnReasignaOper.Visible = false;
                            lnkIni.Visible = false;
                            lnkFin.Visible = false;
                            break;
                        default:
                            break;
                    }
                    idEmpleado = Convert.ToInt32(lblIdEmp.Text);
                    if (idEmpleado != 0)
                    {
                        nomOperario = datosMO.obtieneNombreEmpleadoRH(idEmpleado);
                        if (nomOperario == "")
                            lblOperNomDes.Text = "Sin operario asignado";
                        else
                            lblOperNomDes.Text = nomOperario;
                    }
                    else
                        lblOperNomDes.Text = "Sin operario asignado";
                }
                catch (Exception) { lblOperNomDes.Text = "Sin operario asignado"; }
            }
            catch (Exception ex) { }
        }
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
            }
        }
    }
}