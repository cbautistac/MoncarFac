using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Descargas : System.Web.UI.Page
{
    //Recepciones recepciones = new Recepciones();
    protected void Page_Load(object sender, EventArgs e)
    {
        //cargaInfo();
        //ShowPdf1.FilePath = "files/" + Request.QueryString["filename"];
        HtmlControl pdf = (HtmlControl)this.FindControl("pdf");
        pdf.Attributes.Add("src", "files/" + Request.QueryString["filename"]);//+"#toolbar=0&navpanes=0&scrollbar=0");
    }

    /*/private void cargaInfo()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0 || sesiones[4] == 0)
            Response.Redirect("Default.aspx");
        try
        {
            lblEmpresa.Text = recepciones.obtieneNombreEmpresa(Request.QueryString["e"]);
            lblUser.Text = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);
            lblTallerSesion.Text = recepciones.obtieneNombreTaller(Request.QueryString["t"]);            
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    protected void lnkDescagra_Click(object sender, EventArgs e)
    {
        string filename = Request.QueryString["filename"].ToString();

        Response.Clear();
        Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", filename));
        Response.ContentType = "application/pdf";
        Response.WriteFile(Server.MapPath(Path.Combine("~/files", filename)));
        Response.End();        
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
        catch (Exception x)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
    protected void lnkRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.QueryString["pag"] + ".aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }*/
}