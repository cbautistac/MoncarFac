using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class AdmonOrden : System.Web.UI.MasterPage
{
    Recepciones recepciones = new Recepciones();
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    Fechas fechas = new Fechas();
    Permisos permisos = new Permisos();

    protected void Page_Load(object sender, EventArgs e)
    {
        cargaInfo();
        controlAccesos();
        if (!IsPostBack) {
            ddlTallerCambio.ClearSelection();
            ddlTallerCambio.SelectedValue = Request.QueryString["t"];
            ddlTallerCambio.DataBind();
            
            if (ddlTallerCambio.Items.Count > 1)
                ddlTallerCambio.Visible = true;
            else
                ddlTallerCambio.Visible = false;
            lblversion.Text = ConfigurationManager.AppSettings["version"].ToString();
        }
    }

    private void cargaInfo()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0 || sesiones[4]==0)
            Response.Redirect("Default.aspx");
        try
        {
            lblEmpresa.Text = recepciones.obtieneNombreEmpresa(Request.QueryString["e"]);
            lblUser.Text = recepciones.obtieneNombreUsuario(Request.QueryString["u"]); 
            lblTallerSesion.Text = recepciones.obtieneNombreTaller(Request.QueryString["t"]);
            
            string argumentos = "Orden: " + sesiones[4].ToString();
                DatosVehiculos vehiculos = new DatosVehiculos();
                object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculo(sesiones[4], sesiones[2], sesiones[3]);
                if (Convert.ToBoolean(vehiculo[0]))
                {
                    DataSet valores = (DataSet)vehiculo[1];
                    foreach (DataRow fila in valores.Tables[0].Rows)
                    {
                        argumentos = argumentos.Trim() + " / " + fila[1].ToString().ToUpper();
                        break;
                    }
                }
            
            
            string fase = obtieneEtapa(sesiones[5]);
            lblOrdenSelect.Text = argumentos;
            fechas.fecha = fechas.obtieneFechaLocal();
            fechas.tipoFormato = 4;
            lblFechaActual.Text = fechas.obtieneFechaConFormato();
            Notificaciones noti = new Notificaciones();
            noti.Empresa = sesiones[2];
            noti.Taller = sesiones[3];
            noti.Fecha = Convert.ToDateTime(lblFechaActual.Text);
            noti.Estatus = "P";
            noti.obtieneNotificacionesPendientes();
            object[] pendientes = noti.Retorno;
            if (Convert.ToBoolean(pendientes[0]))
                lblNotifi.Text = Convert.ToString(pendientes[1]);
            else
                lblNotifi.Text = "0";


        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    private string obtieneEtapa(int etapa)
    {
        string fase = "S/E";
        switch (etapa) { 
            case 1:
                fase = "Recepción";
                break;
            case 2:
                fase = "Presupuesto";
                break;
            case 3:
                fase = "Asignación";
                break;
            case 4:
                fase = "Cronológico";
                break;
            case 5:
                fase = "Refacciones";
                break;
            case 6:
                fase = "Operación";
                break;
            case 7:
                fase = "Entrega";
                break;
            case 8:
                fase = "Facturación";
                break;
            default:
                fase = "S/E";
                break;
        }

        return fase;
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
            sesiones = new int[6] { 0, 0, 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    protected void lnkOrdenes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ordenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("BienvenidaOrdenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkRecepcion_Click(object sender, EventArgs e)
    {
        Response.Redirect("RecepcionOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkVehiculo_Click(object sender, EventArgs e)
    {
        Response.Redirect("VehiculoOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkInventario_Click(object sender, EventArgs e)
    {
        Response.Redirect("InventarioOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkDanos_Click(object sender, EventArgs e)
    {
        Response.Redirect("LevantamientoOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkFotos_Click(object sender, EventArgs e)
    {
        Response.Redirect("FotosOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkTodas_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaNotificaciones.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&pag=BienvenidaOrdenes");
    }
    protected void lnkNotificacion_Click(object sender, EventArgs e)
    {
        LinkButton btnNot = (LinkButton)sender;
        int alerta = Convert.ToInt32(btnNot.CommandArgument.ToString());
        Notificaciones noti = new Notificaciones();
        noti.Fecha = Convert.ToDateTime(lblFechaActual.Text);
        noti.Estatus = "V";
        noti.Entrada = alerta;
        noti.Empresa = Convert.ToInt32(Request.QueryString["e"]);
        noti.Taller = Convert.ToInt32(Request.QueryString["t"]);
        noti.actualizaEstado();
        DataList2.DataBind();
        noti.Estatus = "P";
        noti.obtieneNotificacionesPendientes();
        object[] pendientes = noti.Retorno;
        if (Convert.ToBoolean(pendientes[0]))
            lblNotifi.Text = Convert.ToString(pendientes[1]);
        else
            lblNotifi.Text = "0";
    }
    protected void lnkValuacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("Valuacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkAsignacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("Asignacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkBitAvances_Click(object sender, EventArgs e)
    {
        Response.Redirect("BitAvancesOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkBitCome_Click(object sender, EventArgs e)
    {
        Response.Redirect("BitComentariosOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkBitLoc_Click(object sender, EventArgs e)
    {
        Response.Redirect("BitLocalizacionOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkBitPerfil_Click(object sender, EventArgs e)
    {
        Response.Redirect("BitPerfilesOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkRefacciones_Click(object sender, EventArgs e)
    {
        Response.Redirect("Cotizacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&a=1");
    }    
    protected void lnkSegumiento_Click(object sender, EventArgs e)
    {
        Response.Redirect("SeguimientoRefacciones.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkOrdenesCompra_Click(object sender, EventArgs e)
    {
        Response.Redirect("OrdenesCompras.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkCronos_Click(object sender, EventArgs e)
    {
        Response.Redirect("CronosOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkCotizaciones_Click(object sender, EventArgs e)
    {
        Response.Redirect("Cotizacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&a=0");
    }
    protected void lnkOperacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("Operacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkBitLlamadas_Click(object sender, EventArgs e)
    {
        Response.Redirect("BitacoraLlamadasOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkCalOperarios_Click(object sender, EventArgs e)
    {
        Response.Redirect("CalificaOperarios.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkSeguimiento_Click(object sender, EventArgs e)
    {
        Response.Redirect("SeguimientoOperacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkRegPintura_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegistroPinturas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkFacturacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("FacturasOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    /*protected void lnkIngresoCliente_Click(object sender, EventArgs e)
    {
        //Response.Redirect("RegistroPinturas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }*/
    protected void lnkSeguimientoEnt_Click(object sender, EventArgs e)
    {
        Response.Redirect("SeguimientoEntrada.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkInconforme_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inconformidades.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    protected void lnkPagos_Click(object sender, EventArgs e)
    {
        Response.Redirect("PagoDeducible.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
    
    protected void lnkFacturas_Click(object sender, EventArgs e)
    {
        string estatus="P";
        try {  
            object[] retorno = recepciones.obtieneEstatusOrden(Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"]);
            if (Convert.ToBoolean(retorno[0]))
                estatus = Convert.ToString(retorno[1]);
            else
                estatus = "P";
        }
        catch (Exception ex) { estatus = "P"; }
        Response.Redirect("FacturasOrden.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&sta=" + estatus);
    }

    private void controlAccesos()
    {
        int[] sesiones = obtieneSesiones();
        permisos.idUsuario = sesiones[0];
        //permisos.obtienePermisos();
        //bool[] permisosUsuario = permisos.permisos;


        HtmlControl[] controles = {
        (HtmlControl)this.FindControl("mrecep"),
        (HtmlControl)this.FindControl("subDaños"),
        (HtmlControl)this.FindControl("subAsig"),
        (HtmlControl)this.FindControl("mRef"),
        (HtmlControl)this.FindControl("mVal"),
        (HtmlControl)this.FindControl("mOper"),
        (HtmlControl)this.FindControl("mEnt"),
        (HtmlControl)this.FindControl("mFac"),
        (HtmlControl)this.FindControl("mExt"),
            (HtmlControl)this.FindControl("subRecep"),
            (HtmlControl)this.FindControl("subInv"),
            (HtmlControl)this.FindControl("subVeh"),
                (HtmlControl)this.FindControl("subVal"),
                (HtmlControl)this.FindControl("subCot"),
                (HtmlControl)this.FindControl("subOC"),
                    (HtmlControl)this.FindControl("subSeg"),
                    (HtmlControl)this.FindControl("subPint"),
                    (HtmlControl)this.FindControl("subCalOp"),
                        (HtmlControl)this.FindControl("subFotos"),
                        (HtmlControl)this.FindControl("subLlam"),
                        (HtmlControl)this.FindControl("subAva"),
                        (HtmlControl)this.FindControl("subCom"),
                        (HtmlControl)this.FindControl("subLoc"),
                        (HtmlControl)this.FindControl("subPer"),
                        (HtmlControl)this.FindControl("subCron"),                            
                            (HtmlControl)this.FindControl("subSeguimientoEnt"),
                            (HtmlControl)this.FindControl("subInconforme"),
                            (HtmlControl)this.FindControl("subPagos"),(HtmlControl)this.FindControl("mAjus")};

        int[] codigos = { 61, 66, 69, 70, 75, 77, 82, 83, 84, 63, 64, 65, 71, 73, 74, 78, 80, 81, 85, 86, 87, 88, 89, 90, 91, 93, 94, 95, 109 };

        //permisos.permisos = permisosUsuario;
        for (int i = 0; i < codigos.Length; i++)
        {
            permisos.permiso = codigos[i];
            permisos.tienePermisoIndicado();
            if (!permisos.tienePermiso)
                controles[i].Attributes["style"] = "display:none;";
            else
                controles[i].Attributes["style"] = "";
        }

        permisos.permiso = 58;
        permisos.tienePermisoIndicado();
        if (!permisos.tienePermiso)
            not.Visible = false;
        else
            not.Visible = true;

    }
    protected void lnkCierre_Click(object sender, EventArgs e)
    {
        try
        { string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Default.aspx"; Response.Redirect(url); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
    }

    protected void lnkCerrarSesion_Click(object sender, EventArgs e)
    {
        CatUsuarios usuarios = new CatUsuarios();
        object[] actualizaAcceso = usuarios.actualizaBitacoraAcceso(Convert.ToInt32(Request.QueryString["u"]), "I");
        if (Convert.ToBoolean(actualizaAcceso[0]))
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void ddlTallerCambio_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string usuario = Request.QueryString["u"];
            string perfil = Request.QueryString["p"];
            string empresa = Request.QueryString["e"];
            string taller = ddlTallerCambio.SelectedValue;
            Response.Redirect("Ordenes.aspx?u=" + usuario + "&p=" + perfil + "&e=" + empresa + "&t=" + taller, false);
        }
        catch (Exception ex)
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void lnkRemisiones_Click(object sender, EventArgs e)
    {
        Response.Redirect("Remisiones.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }

    protected void lnkSalidasSinCargo_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalidasSinCargo.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }

    protected void lnkAjuste_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ajuste.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
}
