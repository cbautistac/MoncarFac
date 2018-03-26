using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Cuentas : System.Web.UI.MasterPage
{
    Recepciones recepciones = new Recepciones();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            controlAccesos();
            cargaInfoEnc();
            lblversion.Text = ConfigurationManager.AppSettings["version"].ToString();
        }
    }

    private void cargaInfoEnc()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0)
            Response.Redirect("Default.aspx");
        try
        {
            lblEmpresa.Text = recepciones.obtieneNombreEmpresa(Request.QueryString["e"]);
            lblUser.Text = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);
            lblTallerSesion.Text = recepciones.obtieneNombreTaller(Request.QueryString["t"]);
        }
        catch (Exception)
        {

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

    protected void lnkCerrarSesion_Click(object sender, EventArgs e)
    {
        CatUsuarios usuarios = new CatUsuarios();
        object[] actualizaAcceso = usuarios.actualizaBitacoraAcceso(Convert.ToInt32(Request.QueryString["u"]), "I");
        if (Convert.ToBoolean(actualizaAcceso[0]))
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void lnkRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Ordenes.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkControlCostos_Click(object sender, EventArgs e)
    {
        Response.Redirect("ControlCostos.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
    protected void lnkPagoOperarios_Click(object sender, EventArgs e)
    {
        Response.Redirect("PagoOperarios.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkFacturasPagar_Click(object sender, EventArgs e)
    {
        Response.Redirect("FacturasPorPagar.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkCuentasCc_Click(object sender, EventArgs e)
    {
        Response.Redirect("CuentasCobrar.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    private void controlAccesos()
    {
        Permisos permisos = new Permisos();
        int[] sesiones = obtieneSesiones();
        permisos.idUsuario = sesiones[0];
        //permisos.obtienePermisos();
        //bool[] permisosUsuario = permisos.permisos;


        HtmlControl[] controles = {
        (HtmlControl)this.FindControl("mCuentasCP"),                
        (HtmlControl)this.FindControl("subCostos"),
        (HtmlControl)this.FindControl("subPagoOper"),
        (HtmlControl)this.FindControl("mCuentasCC")};

        int[] codigos = { 99, 100, 101, 102 };

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

    protected void lnkFacturas_Click(object sender, EventArgs e)
    {
        Response.Redirect("ComparativoFacturas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkComparativoCostos_Click(object sender, EventArgs e)
    {
        Response.Redirect("ComparativoCostos.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkFacturasCC_Click(object sender, EventArgs e)
    {
        Response.Redirect("ComparativoFacturasCC.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkFacturasExternas_Click(object sender, EventArgs e)
    {
        Response.Redirect("FacturasExternas.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }

    protected void lnkOperarios_Click(object sender, EventArgs e)
    {
        Response.Redirect("OperariosTickets.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"]);
    }
}
