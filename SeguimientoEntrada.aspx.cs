using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;

public partial class SeguimientoEntrada : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    DatosEntrega datosEnt = new DatosEntrega();
    Fechas fecha = new Fechas();
    object[] datosObj = new object[2];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lnkGuarda.Visible = lnkTerminarOrden.Visible = false;
            obtieneSesiones();
            cargaDatosPie();
            cargaDatos();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    ddlLocalizacion.Enabled = DropDownList1.Enabled = txtAvance.Enabled = txtEntrego.Enabled = txtRecibio.Enabled = false;
                    LinkButton1.Visible = LinkButton2.Visible = LinkButton3.Visible = LinkButton4.Visible= lnkGuarda.Visible = lnkTerminarOrden.Visible = false;                    
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

    private void cargaDatosPie()
    {
        Recepciones recepciones = new Recepciones();
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
                txtAvance.Text = filaOrd[8].ToString();
                lblDeducible.Text = Convert.ToDecimal(filaOrd[10].ToString()).ToString("C2");
                lblTotOrden.Text = Convert.ToDecimal(filaOrd[11].ToString()).ToString("C2");

                ddlLocalizacion.SelectedValue = filaOrd[7].ToString();
                DropDownList1.SelectedValue = filaOrd[12].ToString();

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

    private void cargaDatos()
    {
        lblError.Text = "";
        string perfil = DropDownList1.SelectedValue;
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        int idUsuario = Convert.ToInt32(Request.QueryString["u"]);
        txtEntrego.Text = datosEnt.obtieneNombreUsuarioLog(Convert.ToInt32(Request.QueryString["u"]));
        int vistoBuenoFaltantes = 1;
        //lnkGuarda.Visible = lnkTerminarOrden.Visible = false;
        lnkGuarda.Visible = true;
        if (perfil != "10" || perfil != "6")
        {
            datosObj = datosEnt.obtieneVistoBueno(noOrden, idEmpresa, idTaller);
            if ((bool)datosObj[0])
            {
                vistoBuenoFaltantes = Convert.ToInt32(datosObj[1]);
                if (vistoBuenoFaltantes != 0)
                {
                    lnkGuarda.Visible = lnkTerminarOrden.Visible = false;
                    lblError.Text = "Por el tipo de perfil necesita tener en VoBo todas las operaciones";
                }
                datosObj = datosEnt.existeRegClienteEntrega(noOrden, idEmpresa, idTaller);
                if ((bool)datosObj[0])
                {
                    if ((bool)datosObj[1])
                    {
                        string[] infoEntrega = datosEnt.obtieneInfoClienteEntrega(noOrden, idEmpresa, idTaller).Split(';');
                        string nom_cliente = infoEntrega[0];
                        string fecha_entrada = infoEntrega[1];
                        string hora_entrada = infoEntrega[2];
                        string fecha_salida = infoEntrega[3];
                        string hora_salida = infoEntrega[4];
                        string fecha_entrega = infoEntrega[5];
                        string hora_entrega = infoEntrega[6];
                        txtFechaBaja.Text = datosEnt.obtieneFechaBajaPortal(noOrden, idEmpresa, idTaller);
                        txtFechaEntrada.Text = fecha_entrada;
                        txtFechaSalCliente.Text = fecha_salida;
                        txtFechaEntrega.Text = fecha_entrega;
                        if (!string.IsNullOrEmpty(hora_salida))
                            timpSalCliente.SelectedDate = Convert.ToDateTime(hora_salida);
                        //timpSalCliente.SetTime(Convert.ToDateTime(hora_salida).Hour, Convert.ToDateTime(hora_salida).Minute, Convert.ToDateTime(hora_salida).ToString("tt") == "a.m." ? MKB.TimePicker.TimeSelector.AmPmSpec.AM : MKB.TimePicker.TimeSelector.AmPmSpec.PM);
                        if (!string.IsNullOrEmpty(hora_entrada))
                            timpHoraEntrada.SelectedDate = Convert.ToDateTime(hora_entrada);
                        //timpHoraEntrada.SetTime(Convert.ToDateTime(hora_entrada).Hour, Convert.ToDateTime(hora_entrada).Minute, Convert.ToDateTime(hora_entrada).ToString("tt") == "a.m." ? MKB.TimePicker.TimeSelector.AmPmSpec.AM : MKB.TimePicker.TimeSelector.AmPmSpec.PM);
                        if (!string.IsNullOrEmpty(hora_entrega))
                            timpHoraEntrega.SelectedDate = Convert.ToDateTime(hora_entrega);
                            //timpHoraEntrega.SetTime(Convert.ToDateTime(hora_entrega).Hour, Convert.ToDateTime(hora_entrega).Minute, Convert.ToDateTime(hora_entrega).ToString("tt") == "a.m." ? MKB.TimePicker.TimeSelector.AmPmSpec.AM : MKB.TimePicker.TimeSelector.AmPmSpec.PM);
                        txtRecibio.Text = nom_cliente;
                    }
                    else
                    {
                        txtFechaBaja.Text = "";
                        txtFechaEntrega.Text = "";
                        txtFechaSalCliente.Text = "";
                        txtRecibio.Text = "";
                    }
                }
                else
                    lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();
            }
            else
                lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();
        }
    }


    protected void lnkGuarda_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();        
        Permisos permiso = new Permisos();
        permiso.permiso = 108;
        permiso.idUsuario = sesiones[0];
        permiso.tienePermisoIndicado();
        if (permiso.tienePermiso)
        {
            if (txtFechaBaja.Text != "")
                if (txtFechaEntrega.Text != "")
                    if (!timpHoraEntrega.IsEmpty)//(txtHoraEntrega.Text != "")
                        if (txtFechaEntrada.Text != "")
                            if (!timpHoraEntrada.IsEmpty)//(txtHoraEntrada.Text != "")
                                if (txtRecibio.Text != "")
                                    if (txtEntrego.Text != "")
                                    {
                                        int noOrden = sesiones[4];
                                        int idEmpresa = sesiones[2];
                                        int idTaller = sesiones[3];
                                        int idUsuario = sesiones[0];
                                        int idUsuarioAut = sesiones[0];
                                        datosObj = datosEnt.existeRegClienteEntrega(noOrden, idEmpresa, idTaller);
                                        if ((bool)datosObj[0])
                                        {
                                            datosObj = datosEnt.registraOrdenesEntrega(noOrden, idEmpresa, idTaller, idUsuario, idUsuarioAut);
                                            if ((bool)datosObj[0])
                                            {
                                                string strHraEntrega = timpHoraEntrega.SelectedTime.ToString();
                                                string strHraEntrada = timpHoraEntrada.SelectedTime.ToString();
                                                string strHraSalCte = timpSalCliente.SelectedTime.ToString();
                                                if ((bool)datosObj[1])
                                                {
                                                    datosObj = datosEnt.guardaIngresoCliente(noOrden, idEmpresa, idTaller, txtRecibio.Text.ToUpper(), txtFechaEntrada.Text, strHraEntrada, txtFechaSalCliente.Text, strHraSalCte, txtFechaBaja.Text, txtFechaEntrega.Text, strHraEntrega, ddlLocalizacion.SelectedValue,DropDownList1.SelectedValue);
                                                    if ((bool)datosObj[0])
                                                        lblError.Text = "El ingreso fue registrado exitosamente";
                                                    else
                                                        lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();

                                                    actualizaFase();
                                                    cargaDatosPie();
                                                }
                                                else
                                                {
                                                    datosObj = datosEnt.actualizaIngresoCliente(noOrden, idEmpresa, idTaller, txtRecibio.Text.ToUpper(), txtFechaEntrada.Text, strHraEntrada, txtFechaSalCliente.Text, strHraSalCte, txtFechaBaja.Text, txtFechaEntrega.Text, strHraEntrega, ddlLocalizacion.SelectedValue, DropDownList1.SelectedValue);
                                                    if ((bool)datosObj[0])
                                                        lblError.Text = "El ingreso fue registrado exitosamente";
                                                    else
                                                        lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();

                                                    actualizaFase();
                                                    cargaDatosPie();
                                                }
                                            }
                                            else
                                                lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();
                                        }
                                        else
                                            lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();
                                    }
                                    else
                                        lblError.Text = "Necesita colocar el nombre de la persona que entregó el vehículo";
                                else
                                    lblError.Text = "Necesita colocar el nombre de la persona que recibió el vehículo";
                            else
                                lblError.Text = "Necesita colocar una hora de entrada";
                        else
                            lblError.Text = "Necesita colocar una fecha de entrada";
                    else
                        lblError.Text = "Necesita colocar la fecha de entrega";
                else
                    lblError.Text = "Necesita colocar la hora de entrega";
            else
                lblError.Text = "Necesita colocar la fecha de baja del portal";
        }
        else
            PanelMask.Visible = PanelPopUpPermiso.Visible = true;
    }

    protected void lnkTerminarOrden_Click(object sender, EventArgs e)
    {
        obtieneSesiones();
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        datosObj = datosEnt.terminaOrden(noOrden, idEmpresa, idTaller);
    }

    protected void ddlLocalizacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        string localizacion = ddlLocalizacion.SelectedValue;
        if (localizacion != "3" && localizacion != "4")
        {
            ddlLocalizacion.DataBind();
            lblError.Text = "Solo se puede cambiar la localización a terminado o entregado";
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        PanelMask.Visible = PanelPopUpPermiso.Visible = true;
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            lblErrorLog.Text = "";
            Autorizaciones autoriza = new Autorizaciones();
            autoriza.nick = txtUsuarioLog.Text;
            autoriza.contrasena = txtContraseñaLog.Text;
            autoriza.permiso = 1;
            autoriza.validaUsuario();
            if (autoriza.Valido)
            {
                obtieneSesiones();
                int noOrden = Convert.ToInt32(Request.QueryString["o"]);
                int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                int idTaller = Convert.ToInt32(Request.QueryString["t"]);
                int idUsuario = Convert.ToInt32(Request.QueryString["u"]);
                int idUsuarioAut = Convert.ToInt32(datosEnt.obtieneIdUsuarioAut(txtUsuarioLog.Text));
                if (idUsuarioAut == 0)
                    Convert.ToInt32("f");
                lblError.Text = "";
                if (txtFechaBaja.Text != "")
                    if (txtFechaEntrega.Text != "")
                        if (!timpHoraEntrega.IsEmpty)//(txtHoraEntrega.Text != "")
                            if (txtFechaEntrada.Text != "")
                                if (!timpHoraEntrada.IsEmpty)//(txtHoraEntrada.Text != "")
                                    if (txtRecibio.Text != "")
                                        if (txtEntrego.Text != "")
                                        {
                                            datosObj = datosEnt.existeRegClienteEntrega(noOrden, idEmpresa, idTaller);
                                            if ((bool)datosObj[0])
                                            {
                                                datosObj = datosEnt.registraOrdenesEntrega(noOrden, idEmpresa, idTaller, idUsuario, idUsuarioAut);
                                                if ((bool)datosObj[0])
                                                {
                                                    string strHraEntrega = timpHoraEntrega.SelectedTime.ToString();
                                                    string strHraEntrada = timpHoraEntrada.SelectedTime.ToString();
                                                    string strHraSalCte = timpSalCliente.SelectedTime.ToString();
                                                    if ((bool)datosObj[1])
                                                    {
                                                        datosObj = datosEnt.guardaIngresoCliente(noOrden, idEmpresa, idTaller, txtRecibio.Text.ToUpper(), txtFechaEntrada.Text, strHraEntrada, txtFechaSalCliente.Text, strHraSalCte, txtFechaBaja.Text, txtFechaEntrega.Text, strHraEntrega,ddlLocalizacion.SelectedValue,DropDownList1.SelectedValue);
                                                        if ((bool)datosObj[0])
                                                            lblError.Text = "El ingreso fue registrado exitosamente";
                                                        else
                                                            lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();

                                                        actualizaFase();
                                                    }
                                                    else
                                                    {
                                                        datosObj = datosEnt.actualizaIngresoCliente(noOrden, idEmpresa, idTaller, txtRecibio.Text.ToUpper(), txtFechaEntrada.Text, strHraEntrada, txtFechaSalCliente.Text, strHraSalCte, txtFechaBaja.Text, txtFechaEntrega.Text, strHraEntrega, ddlLocalizacion.SelectedValue, DropDownList1.SelectedValue);
                                                        if ((bool)datosObj[0])
                                                            lblError.Text = "El ingreso fue registrado exitosamente";
                                                        else
                                                            lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();

                                                        actualizaFase();
                                                    }
                                                }
                                                else
                                                    lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();
                                            }
                                            else
                                                lblError.Text = "Ocurrio un error inesperado: " + datosObj[1].ToString();
                                        }
                                        else
                                            lblError.Text = "Necesita colocar el nombre de la persona que entregó el vehículo";
                                    else
                                        lblError.Text = "Necesita colocar el nombre de la persona que recibió el vehículo";
                                else
                                    lblError.Text = "Necesita colocar una hora de entrada";
                            else
                                lblError.Text = "Necesita colocar una fecha de entrada";
                        else
                            lblError.Text = "Necesita colocar la fecha de entrega";
                    else
                        lblError.Text = "Necesita colocar la hora de entrega";
                else
                    lblError.Text = "Necesita colocar la fecha de baja del portal";
                PanelMask.Visible = PanelPopUpPermiso.Visible = false;
            }
            else
            {
                lblErrorLog.Text = "Error: " + autoriza.Error;
            }
        }
        catch (Exception)
        {
            lblErrorLog.Text = "Ocurrio un error en la autorización verifique sus datos e intentelo nuevamente";
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        lblErrorLog.Text = "";
        ddlPerfil.DataBind();
        lblError.Text = "Necesita autorización para cambiar el perfil";
        PanelMask.Visible = PanelPopUpPermiso.Visible = false;
    }

    private void actualizaFase()
    {
        int faseSActual = 1;
        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);
        try
        {
            object[] datos = recepciones.obtieneInfoOrden(orden, empresa, taller);
            if (Convert.ToBoolean(datos[0]))
            {
                DataSet valores = (DataSet)datos[1];
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    faseSActual = Convert.ToInt32(fila[53].ToString());
                }

                if (faseSActual < 7)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 7);
                }
            }
        }
        catch (Exception) { }

    }
}