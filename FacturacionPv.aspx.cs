using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FacturacionPv : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnFacturar_Click(object sender, EventArgs e)
    {
        string[] argumentos = ((Button)sender).CommandArgument.ToString().Split(new char[] { ';' });
        Response.Redirect("FacturacionGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=0" + "&tck=" + argumentos[0] + "&c=" + argumentos[1]);
    }

    protected void grdFactPend_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdDetalle.DataBind();
    }

    protected void btnFacturar2_Click(object sender, EventArgs e)
    {
        string[] argumentos = ((Button)sender).CommandArgument.ToString().Split(new char[] { '|' });
        Response.Redirect("FacturacionGral.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&fact=0" + "&tck=" + argumentos[0] + "&c=" + argumentos[1]);
    }

    protected void grdFactMasivas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdFactMasivas.DataBind();
    }
}