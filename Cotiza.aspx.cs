using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Cotiza : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            PanelImgZoom.Visible = PanelMascara.Visible = false;
            cargaInfo();
        }
    }

    private void cargaInfo()
    {
        try
        {
            int[] sesiones = obtieneSesiones();

            string argumentos = "Folio:";
            DatosVehiculos vehiculos = new DatosVehiculos();
            object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculoCot(sesiones[0], sesiones[1], sesiones[2], sesiones[3], sesiones[4]);
            if (Convert.ToBoolean(vehiculo[0]))
            {
                DataSet valores = (DataSet)vehiculo[1];
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    argumentos = argumentos.Trim() + " " + fila[3].ToString().ToUpper() + " / " + fila[1].ToString().ToUpper() + " / No. Serie:" + fila[4].ToString().ToUpper();
                    lblPropveedor.Text = fila[2].ToString().ToUpper();
                    break;
                }
            }
            lblOrdenSelect.Text = argumentos;

            datosCotizaProv cotizacion = new datosCotizaProv();
            object[] horas = cotizacion.obtieneHrsMaxTaller(sesiones[1], sesiones[2]);
            int horasT = 0;
            if (Convert.ToBoolean(horas[0]))
            {
                horasT = Convert.ToInt32(horas[1]);
                object[] checaCotizacion = cotizacion.verificaCotizacionCotiza(sesiones, horasT);
                if (Convert.ToBoolean(checaCotizacion[0]))
                {
                    bool activo = Convert.ToBoolean(checaCotizacion[1]);
                    if (activo)
                    {
                        lblMensaje.Visible = false;
                        Panel1.Visible = Panel2.Visible = lnkGuardar.Visible = true;
                        lblMensaje.Text = "";
                        lblError.Text = "";
                        RefProvCotiza datosRefacc = new RefProvCotiza();
                        List<RefProvCotiza> refacciones = new List<RefProvCotiza>();
                        datosRefacc._orden = sesiones[0];
                        datosRefacc._empresa = sesiones[1];
                        datosRefacc._taller = sesiones[2];
                        datosRefacc._cotizacion = sesiones[3];
                        datosRefacc._proveedor = sesiones[4];
                        refacciones = datosRefacc.obtieneRefaccionesCotiza();
                        ListView1.DataSource = refacciones;
                        ListView1.DataBind();
                        DataListFotosDanos.DataBind();
                        DataListFotosDanos.Visible = true;
                        Label321.Visible = txtFechaFirma.Visible = true;
                    }
                    else
                    {
                        Refacciones datosRefacc = new Refacciones();
                        List<Refacciones> refacciones = new List<Refacciones>();
                        datosRefacc._orden = sesiones[0];
                        datosRefacc._empresa = sesiones[1];
                        datosRefacc._taller = sesiones[2];
                        datosRefacc._cotizacion = sesiones[3];
                        datosRefacc._proveedor = sesiones[4];
                        refacciones = datosRefacc.obtieneRefaccionesCotiza();

                        foreach (Refacciones refacc in refacciones)
                        {
                            object[] cotizado = cotizacion.actualizaRefaccionCotiza(sesiones, refacc);
                        }

                        lblMensaje.Visible = true;
                        Panel1.Visible = Panel2.Visible = lnkGuardar.Visible = false;
                        Label321.Visible = txtFechaFirma.Visible = false ;
                        lblMensaje.CssClass = "text-danger t18 textoBold";
                        lblMensaje.Text = "Ya se ha registrado la cotización o a caducado el tiempo máximo de " + horasT.ToString() + " horas para realizar la cotización";
                        lblError.Text = "";
                    }
                }
                else
                {
                    lblError.Text = "Error al cargar la información: Se ha producido un error al cargar la información por favor contacte a su cliente para verificar este portal o bien para buscar otra opción para la cotización. Detalle del Error:" + checaCotizacion[1].ToString();
                }
            }
            else
                lblError.Text = "Error al cargar la información: Se ha producido un error al cargar la información por favor contacte a su cliente para verificar este portal o bien para buscar otra opción para la cotización. Detalle del Error:" + horas[1].ToString();
            
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Cotiza.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[5] { 0, 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["o"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["t"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["c"]);
            sesiones[4] = Convert.ToInt32(Request.QueryString["p"]);            
        }
        catch (Exception x)
        {
            sesiones = new int[5] { 0, 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Cotiza.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
    protected void lnkGuardar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int[]  sesiones = obtieneSesiones();
        int contador=0;
        Refacciones datosRefacc = new Refacciones();
        List<Refacciones> refacciones = new List<Refacciones>();           
        foreach (ListViewDataItem refaccion in ListView1.Items) {
            Label id = refaccion.FindControl("lblIdRefaccion") as Label;
            Label cantidad = refaccion.FindControl("lblCantidad") as Label;
            Label descripcion = refaccion.FindControl("lblRefaccion") as Label;
            TextBox costo = refaccion.FindControl("txtContoUnitario") as TextBox;
            TextBox descuento = refaccion.FindControl("txtPorcDesc") as TextBox;
            CheckBox existencia = refaccion.FindControl("chkExistencia") as CheckBox;
            TextBox dias = refaccion.FindControl("txtDias") as TextBox;
            DropDownList ddlProced = refaccion.FindControl("ddlProcedencia") as DropDownList;

            decimal costoU = 0, porcentaje = 0, impDesc = 0, importe = 0;
            int diasEnt = 0;
            try
            {
                try { costoU = Convert.ToDecimal(costo.Text); } catch (Exception) { costoU = 0; }
                try { porcentaje = Convert.ToDecimal(descuento.Text); } catch (Exception) { porcentaje = 0; }
                importe = Convert.ToInt32(cantidad.Text) * costoU;
                if (porcentaje != 0)
                {
                    impDesc = (porcentaje / 100) * importe;
                    importe = importe - impDesc;
                }
                else
                {
                    impDesc = 0;
                }
                try { diasEnt = Convert.ToInt32(dias.Text); } catch (Exception) { diasEnt = 0; }
                                
                refacciones.Add(new Refacciones() { _refaccion = Convert.ToInt32(id.Text), _cantidad = Convert.ToInt32(cantidad.Text), _descripcion = descripcion.Text, _costo = costoU, _porcentajeDescuento = porcentaje, _importeDescuento = impDesc, _importe = importe, _existencia = existencia.Checked, _dias = diasEnt, _procedencia = int.Parse(ddlProced.SelectedValue) });
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;                
                refacciones = null;
                break;
            }
        }
        if (refacciones != null)
        {
            datosCotizaProv cotizacion = new datosCotizaProv();
            DateTime limite = Convert.ToDateTime(txtFechaFirma.SelectedDate);
            foreach (Refacciones refacc in refacciones)
            {
                object[] cotizado = cotizacion.actualizaRefaccionCotizaRef(sesiones, refacc,limite.ToString("yyyy-MM-dd"));
                if(Convert.ToBoolean(cotizado[1]) == true)
                {
                    contador = contador + 1;
                }
            }
            lblMensaje.Visible = true;
            Panel1.Visible = Panel2.Visible = lnkGuardar.Visible = false;
            lblMensaje.CssClass = "text-success t18 textoBold";
            lblMensaje.Text = "Se ha registrado la cotización exitosamente. Espere confirmación de compra vía correo electrónico de su cliente.";
            lblError.Text = "";
        }
        if (contador != 0)
        {
            /*
            datosCotizaProv coti = new datosCotizaProv();
            DateTime limite = Convert.ToDateTime(txtFechaFirma.SelectedDate);
            object[] cotizadoFechaLimite = coti.actualizaFechaLimiteCotizacion(sesiones,limite.ToString("yyyy-MM-dd"));*/
        }
    }

    protected void DataListFotosDanos_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "zoom")
        {
            PanelMascara.Visible = true;
            PanelImgZoom.Visible = true;
            string[] valores = e.CommandArgument.ToString().Split(';');
            int id_empresa = Convert.ToInt32(valores[0]);
            int id_taller = Convert.ToInt32(valores[1]);
            int no_orden = Convert.ToInt32(valores[2]);
            int consecutivo = Convert.ToInt32(valores[3]);
            int proceso = Convert.ToInt32(valores[4]);
            imgZoom.ImageUrl = "~/ImgEmpresas.ashx?id=" + id_empresa + ";" + id_taller + ";" + no_orden + ";" + consecutivo + ";" + proceso;
        }
    }

    protected void btnCerrarImgZomm_Click(object sender, EventArgs e)
    {
        PanelImgZoom.Visible = false;
        PanelMascara.Visible = false;
    }
}