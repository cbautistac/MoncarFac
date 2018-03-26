using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Web.UI.HtmlControls;
using System.Configuration;

public partial class AdmOrdenes : System.Web.UI.MasterPage
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();
    Permisos permisos = new Permisos();
    protected void Page_Load(object sender, EventArgs e)
    {
        cargaNotificaciones();
        if (!IsPostBack)
        {
            
            lblversion.Text = ConfigurationManager.AppSettings["version"].ToString();
        }
        
    }

    private void cargaInfoEnc()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 )
            Response.Redirect("Default.aspx");
        try
        {
            lblEmpresa.Text = recepciones.obtieneNombreEmpresa(Request.QueryString["e"]);
            lblUser.Text = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);
            lblTallerSesion.Text = recepciones.obtieneNombreTaller(Request.QueryString["t"]);
        }
        catch (Exception) { 

        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[4] { 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
        }
        catch (Exception x)
        {
            sesiones = new int[2] { 0, 0};
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    protected void lnkRecarga_Click(object sender, EventArgs e)
    {
        cargaNotificaciones();
        DataList2.DataBind();
    }
    private void cargaNotificaciones()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0)
            Response.Redirect("Default.aspx");
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
    protected void lnkTodas_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaNotificaciones.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&pag=Ordenes");
    }
    
    protected void lnkFacturacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("FacturasGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkAddenda_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdendaQualitas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&add=0");
    }
    protected void lnkCuentas_Click(object sender, EventArgs e)
    {
        Response.Redirect("BienvenidaCuentas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    private void controlAccesos()
    {
        int[] sesiones = obtieneSesiones();
        permisos.idUsuario = sesiones[0];
        //permisos.obtienePermisos();
        //bool[] permisosUsuario = permisos.permisos;


        HtmlControl[] controles = {
        (HtmlControl)this.FindControl("mConsultas"),
        (HtmlControl)this.FindControl("mreportes"),
        (HtmlControl)this.FindControl("actNot"),
        (HtmlControl)this.FindControl("mnot"),
        (HtmlControl)this.FindControl("mpantallaspatiomenu"),
            (HtmlControl)this.FindControl("subPersonal"),
            (HtmlControl)this.FindControl("subCalendar"),
                (HtmlControl)this.FindControl("subIngresos"),
                (HtmlControl)this.FindControl("subBitAsig"),
                (HtmlControl)this.FindControl("subBitVal"),
                (HtmlControl)this.FindControl("subBitLlam"),
                (HtmlControl)this.FindControl("subPint"),
                    (HtmlControl)this.FindControl("subConsTrans"),
                    (HtmlControl)this.FindControl("subConsGar"),
                    (HtmlControl)this.FindControl("subConsTerm"),
                    (HtmlControl)this.FindControl("subConsEntregas"),
                        (HtmlControl)this.FindControl("progSem"),
        (HtmlControl)this.FindControl("mFactura"),
        (HtmlControl)this.FindControl("refaccionesPendientes"),
        (HtmlControl)this.FindControl("prePendiente"),
        (HtmlControl)this.FindControl("mCuentas"),
        (HtmlControl)this.FindControl("sunOrden"),
        (HtmlControl)this.FindControl("subTran"),
        (HtmlControl)this.FindControl("subPerfil")
        };

        int[] codigos = { 41, 44, 45, 51, 56, 58, 58, 59, 42, 43, 46, 47, 48, 49, 50, 52, 53, 54, 55, 57, 83, 96, 97, 98, 103, 104, 105 };

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
    
    protected void lnkPuntoVenta_Click(object sender, EventArgs e)
    {
        Response.Redirect("FacturacionPv.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }


    protected void lnkOrdenes_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ordenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkOrdenesRep_Click(object sender, EventArgs e)
    {
        Response.Redirect("RepOrdenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkReporte_Click(object sender, EventArgs e)
    {
        Response.Redirect("RepFacturas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkFacturasExternas_Click(object sender, EventArgs e)
    {
        Response.Redirect("FacturasExternas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkExportado_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReporteFacturacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoReceptores_Click(object sender, EventArgs e)
    {
       Response.Redirect("CatReceptores.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoEmisores_Click(object sender, EventArgs e)
    {
       Response.Redirect("CatEmisores.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoUsuarios_Click(object sender, EventArgs e)
    {
       Response.Redirect("CatUsuarios.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoImpuesto_Click(object sender, EventArgs e)
    {
        Response.Redirect("catImpuestos.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoMetodoPago_Click(object sender, EventArgs e)
    {
        Response.Redirect("catMetPago.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoMoneda_Click(object sender, EventArgs e)
    {
        Response.Redirect("catMoneda.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoProdServ_Click(object sender, EventArgs e)
    {
        Response.Redirect("catProdServ.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoUnidad_Click(object sender, EventArgs e)
    {
        Response.Redirect("catUnidad.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoBancos_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatBancos.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoFormaPago_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatFormaPago.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoImpuestos_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatImpuesto.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoRegimenFiscal_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatRegFiscal.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatTasaCuota_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatTasaCuota.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoTipoFactor_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatTipFactor.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoTipoRelacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatTipRelacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkCatalogoServprod_f_Click(object sender, EventArgs e)
    {
        Response.Redirect("ServProd_F.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
}
