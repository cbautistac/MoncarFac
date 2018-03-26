using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;
using E_Utilities;
using Telerik.Web.UI;

public partial class DetalleOrdenCompra : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    datosCotizaProv cotizacion = new datosCotizaProv();
    Fechas fechas = new Fechas();
    int cot = 0, contCot = 0;
    DatosOrdenes datos = new DatosOrdenes();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PanelMascara.Visible = false;
            PanelImgZoom.Visible = false;
            cargaDatosPie();
            cargaDatosEntrega();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    lnkAceptar.Visible = lnkEliminaFoto.Visible = lnkNotificaOrden.Visible = false;
                    grdRefacciones.Columns[13].Visible = grdRefacciones.Columns[14].Visible = grdRefacciones.Columns[15].Visible = false;
                    Label1.Visible = txtFechaRecepcion.Visible = lnkFechaRecepcion.Visible = txtHoraPact.Visible = false;
                    Label3.Visible = txtNombreEntrega.Visible = Label13.Visible = txtFactura.Visible = false;
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

    private void cargaDatosEntrega()
    {
        if (Request.QueryString["s"] == "CAN" || Request.QueryString["s"] == "ENT")
            lnkImprimeOrden.Visible = false;
        else
            lnkImprimeOrden.Visible = true;
        int[] sesiones = obtieneSesiones();
        object[] datosOrden = recepciones.obtieneInfoOrdenCompra(sesiones[4], sesiones[2], sesiones[3], sesiones[6]);
        if (Convert.ToBoolean(datosOrden[0]))
        {
            DataSet ordenDatos = (DataSet)datosOrden[1];
            foreach (DataRow filaOrd in ordenDatos.Tables[0].Rows)
            {
                txtFechaRecepcion.Text = filaOrd[0].ToString();
                txtHoraPact.Text = filaOrd[1].ToString();
                txtNombreEntrega.Text = filaOrd[2].ToString();
                txtFactura.Text = filaOrd[3].ToString();
                if (txtNombreEntrega.Text == "" || txtHoraPact.Text == "" || txtFechaRecepcion.Text == "" || txtFactura.Text=="")
                {
                    txtHoraPact.Enabled = txtNombreEntrega.Enabled = txtFactura.Enabled = true;
                    lnkFechaRecepcion.Visible = true;
                    lnkNotificaOrden.Visible = false;
                }
                else
                {
                    txtHoraPact.Enabled = txtNombreEntrega.Enabled = txtFactura.Enabled = false;
                    lnkFechaRecepcion.Visible = false;
                    lnkNotificaOrden.Visible = true;
                }
            }
        }
        else
            lblError.Text = "Error: " + datosOrden[1].ToString();
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);
            sesiones[4] = Convert.ToInt32(Request.QueryString["o"]);
            sesiones[5] = Convert.ToInt32(Request.QueryString["f"]);
            sesiones[6] = Convert.ToInt32(Request.QueryString["c"]);
        }
        catch (Exception)
        {
            sesiones = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    private void cargaDatosPie()
    {
        int[] sesiones = obtieneSesiones();
        object[] datosOrden = recepciones.obtieneInfoOrdenPie(sesiones[4], sesiones[2], sesiones[3]);
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
                lblTit.Text = "Orden de Compra con Folio: " + Request.QueryString["fc"];
            }
        }
    }

    protected void lnkCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("OrdenesCompras.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }

    private void llenarBitacora(string proceso, string estatusProceso, string refaccion, int piezas, string estatusRefText)
    {
        bool inserta = false;
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        int idUsuario = Convert.ToInt32(Request.QueryString["u"]);
        string usuario = recepciones.obtieneNombreUsuario(idUsuario.ToString());
        string observaciones = "El usuario " + usuario + " " + proceso + " la refacción: " + refaccion + ", " + piezas.ToString() + " pieza(s) para realizar " + estatusRefText;
        inserta = recepciones.llenaBitacoraRef(noOrden, idEmpresa, idTaller, idUsuario, observaciones, estatusProceso);
    }

    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        LinkButton lnkCal = grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("lnkEntrega") as LinkButton;
        TextBox txtFecha = grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("txtEntrega") as TextBox;

        if (ddl.SelectedValue == "3")
        {
            if (txtFechaRecepcion.Text != "")
                txtFecha.Text = txtFechaRecepcion.Text;
            else
                txtFecha.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            lnkCal.Visible = true;
            txtFecha.Visible = true;
        }
        else
        {
            txtFecha.Text = "";
            txtFecha.Visible = false;
            lnkCal.Visible = false;
        }
    }

    protected void grdRefacciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string modo = e.Row.RowState.ToString();
            string[] valores = null;
            try { valores = modo.Split(new char[] { ',' }); }
            catch (Exception) { modo = e.Row.ToString(); }
            if (valores != null)
            {
                for (int i = 0; i < valores.Length; i++)
                {
                    if (valores[i].Trim() == "Edit")
                    {
                        modo = "Edit";
                        break;
                    }
                    else
                        modo = valores[i].Trim();
                }
            }

            if (modo == "Edit")
            {
                DropDownList ddl = e.Row.FindControl("ddlEstatus") as DropDownList;
                LinkButton lnkCal = e.Row.FindControl("lnkEntrega") as LinkButton;
                TextBox txtFecha = e.Row.FindControl("txtEntrega") as TextBox;
                LinkButton lnkEnviCorreo = e.Row.FindControl("lnkEnviCorreo") as LinkButton;

                try { lnkEnviCorreo.Visible = false; }
                catch (Exception) { }
                if (ddl.SelectedValue == "3")
                {
                    lnkCal.Visible = true;
                    txtFecha.Visible = true;
                }
                else
                {
                    txtFecha.Text = "";
                    txtFecha.Visible = false;
                    lnkCal.Visible = false;
                }
            }
            else
            {
                LinkButton lnkEdit = e.Row.FindControl("lnkEditar") as LinkButton;
                LinkButton lnkEnviCorreo = e.Row.FindControl("lnkEnviCorreo") as LinkButton;
               
                string estatus = DataBinder.Eval(e.Row.DataItem, "estatusRef").ToString();
                int est;
                try {  est = Convert.ToInt32(estatus); }
                catch (Exception) { est = 0; }

                if (est == 3 || est == 5)
                    lnkEdit.Visible = false;
                else
                {
                    lnkEdit.Visible = true;                   
                    if (est==1 || est==2 || est==3||est==5)
                        lnkEnviCorreo.Visible = false;
                    else
                        lnkEnviCorreo.Visible = true;
                }
            }

        }
    }

    protected void lnkAceptar_Click(object sender, EventArgs e)
    {
        try
        {
            string fecha = "";
            string hora = "";

            DateTime fechaRecep;
            try
            {

                fechaRecep = Convert.ToDateTime(txtFechaRecepcion.Text + " " + txtHoraPact.Text);

                fecha = "'" + fechaRecep.ToString("yyyy-MM-dd") + "'";
                hora = "'" + fechaRecep.ToString("HH:mm:ss") + "'";
            }
            catch (Exception) { fecha = "null"; hora = "null"; }


            SqlDataSource1.UpdateCommand = "update orden_compra_encabezado set id_usuario_recibe=" + Request.QueryString["u"] + ", fecha_entrega=" + fecha + ", hora_entrega=" + hora + ", nombre_entrega='" + txtNombreEntrega.Text + "', estatus='CON', factura='"+txtFactura.Text+"' where no_orden=" + Request.QueryString["o"] + " and id_empresa=" + Request.QueryString["e"] + " and id_taller=" + Request.QueryString["t"] + " and id_orden=" + Request.QueryString["c"];
            SqlDataSource1.Update();
            lnkNotificaOrden.Visible = true;
            actualizaFechaEntrega();

            OrdenCompra ordenC = new OrdenCompra();
            object[] datosOrden = ordenC.obtieneInfoOrden(Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(Request.QueryString["t"]), Convert.ToInt32(Request.QueryString["o"]), Convert.ToInt32(Request.QueryString["c"]));
            decimal proveedor = 0;
            decimal importe = 0;
            if (Convert.ToBoolean(datosOrden[0])) {
                DataSet info = (DataSet)datosOrden[1];
                if (info.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow filaInfo in info.Tables[0].Rows)
                    {
                        proveedor = Convert.ToDecimal(filaInfo[0]);
                        importe = Convert.ToDecimal(filaInfo[1]);
                        break;
                    }
                }
            }


            Facturas facturas = new Facturas();
            facturas.folio = Convert.ToInt32(Request.QueryString["o"]);
            facturas.tipoCuenta = "PA";
            facturas.factura = txtFactura.Text;
            CatClientes clientes = new CatClientes();
            string politica = clientes.obtieneClavePolitica(proveedor);
            facturas.id_cliprov = Convert.ToInt32(proveedor);
            facturas.politica = politica;
            facturas.estatus = "PEN";
            facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
            facturas.taller = Convert.ToInt32(Request.QueryString["t"]);
            facturas.tipoCargo = "C";
            decimal importeVal = Convert.ToDecimal(importe);
            importeVal = importeVal + (importeVal * Convert.ToDecimal(0.16));
            facturas.Importe = importeVal;
            facturas.montoPagar = importeVal;
            facturas.orden = Convert.ToInt32(Request.QueryString["o"]);
            facturas.razon_social = obtieneRazonSocial(Convert.ToInt32(proveedor), "P");
            facturas.monto = recepciones.obtieneMonto(Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(Request.QueryString["t"]), Convert.ToInt32(Request.QueryString["o"]), txtFactura.Text, proveedor.ToString());
            facturas.existeFactura();
            if (Convert.ToBoolean(facturas.retorno[0]))
            {
                if (Convert.ToInt32(facturas.retorno[1]) == 0)
                    facturas.generaFactura();                
            }
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
        cargaDatosEntrega();
    }

    private string obtieneRazonSocial(int idCliprov, string tipo)
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string razonsocial = "";
        string sql = "select razon_social from cliprov where id_cliprov=" + idCliprov.ToString() + " and tipo='" + tipo + "'";
        object[] ejecutado = ejecuta.scalarToString(sql);
        if ((bool)ejecutado[0])
            razonsocial = ejecutado[1].ToString();
        else
            razonsocial = "";
        return razonsocial;
    }

    private void actualizaFechaEntrega()
    {
        string noOrden = Request.QueryString["o"];
        string idEmpresa = Request.QueryString["e"];
        string idTaller = Request.QueryString["t"];
        string ultimaFecha = cotizacion.ultimaEntregaActualizada(noOrden, idEmpresa, idTaller);
        DateTime fechaUltimaConv;
        DateTime fecha1900 = Convert.ToDateTime("1900-01-01");
        try
        {
            fechaUltimaConv = Convert.ToDateTime(ultimaFecha);
            if (fechaUltimaConv > fecha1900)
                cotizacion.actualizaCronosEntregas(noOrden, idEmpresa, idTaller, fechaUltimaConv.ToString("yyyy-MM-dd"));
        }
        catch (Exception) { }
    }

    protected void grdRefacciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {


        }
    }

    protected void lnkActualiza_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        DropDownList ddl = grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("ddlEstatus") as DropDownList;
        TextBox obs = grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("txtObservaciones") as TextBox;
        TextBox fEntrega = grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("txtEntrega") as TextBox;
        Label refaccion = grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("lblRefaccionEd") as Label;
        string[] argumentos = lnk.CommandArgument.ToString().Split(new char[] { ';' });
        string id = argumentos[0];
        try
        {
            string fecha = "";
            string hora = "";
            string fechaEnt = fEntrega.Text;
            if (fechaEnt == "")
            {
                fechaEnt = "null";
                DateTime fechaRecep;
                try
                {
                    fechaRecep = Convert.ToDateTime(txtFechaRecepcion.Text + " " + txtHoraPact.Text);
                    fecha = "'" + fechaRecep.ToString("yyyy-MM-dd") + "'";
                    hora = "'" + fechaRecep.ToString("HH:mm:ss") + "'";
                }
                catch (Exception) { fechaEnt = "null"; fecha = "null"; hora = "null"; }
            }
            else
            {
                DateTime fechaEntrega;
                DateTime fechaRecep;
                try
                {
                    fechaEntrega = Convert.ToDateTime(fEntrega.Text);
                    fechaRecep = Convert.ToDateTime(txtFechaRecepcion.Text + " " + txtHoraPact.Text);
                    fechaEnt = "'" + fechaEntrega.ToString("yyyy-MM-dd") + "'";
                    fecha = "'" + fechaRecep.ToString("yyyy-MM-dd") + "'";
                    hora = "'" + fechaRecep.ToString("HH:mm:ss") + "'";
                }
                catch (Exception) { fechaEnt = "null"; fecha = "null"; hora = "null"; }

            }
            SqlDataSource1.UpdateCommand = "update refacciones_orden set refEstatusSolicitud=" + ddl.SelectedValue + ", observacion='" + obs.Text + "', refFechEntregaReal=" + fechaEnt + " where ref_no_orden=" + Request.QueryString["o"] + " and ref_id_empresa=" + Request.QueryString["e"] + " and ref_id_taller=" + Request.QueryString["t"] + " and refOrd_id=" + id + " " +
                " update orden_compra_encabezado set id_usuario_recibe=" + Request.QueryString["u"] + ", fecha_entrega=" + fecha + ", hora_entrega=" + hora + ", nombre_entrega='" + txtNombreEntrega.Text + "', estatus='CON', factura='"+txtFactura.Text+"' where no_orden=" + Request.QueryString["o"] + " and id_empresa=" + Request.QueryString["e"] + " and id_taller=" + Request.QueryString["t"] + " and id_orden=" + Request.QueryString["c"];
            SqlDataSource1.Update();
            actualizaFechaEntrega();

            Facturas facturas = new Facturas();
            facturas.folio = Convert.ToInt32(Request.QueryString["o"]);
            facturas.tipoCuenta = "PA";
            facturas.factura = txtFactura.Text;
            CatClientes clientes = new CatClientes();
            string politica = clientes.obtieneClavePolitica(Convert.ToDecimal(argumentos[1]));
            facturas.id_cliprov = Convert.ToInt32(argumentos[1]);
            facturas.politica = politica;
            facturas.estatus = "PEN";
            facturas.empresa = Convert.ToInt32(Request.QueryString["e"]);
            facturas.taller = Convert.ToInt32(Request.QueryString["t"]);
            facturas.tipoCargo = "C";
            decimal importeVal = Convert.ToDecimal(argumentos[2]);
            importeVal = importeVal + (importeVal * Convert.ToDecimal(0.16));
            facturas.Importe = importeVal;
            facturas.montoPagar = importeVal;            
            facturas.orden = Convert.ToInt32(Request.QueryString["o"]);
            facturas.razon_social = obtieneRazonSocial(Convert.ToInt32(argumentos[1]), "P");
            facturas.monto = recepciones.obtieneMonto(Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(Request.QueryString["t"]), Convert.ToInt32(Request.QueryString["o"]), txtFactura.Text, argumentos[1]);


            facturas.existeFactura();
            if (Convert.ToBoolean(facturas.retorno[0]))
            {
                if (Convert.ToInt32(facturas.retorno[1]) == 0)
                    facturas.generaFactura();                
            }


            if (ddl.SelectedValue == "5")
            {
                try
                {
                    int noOrden = Convert.ToInt32(Request.QueryString["o"]);
                    int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                    int idTaller = Convert.ToInt32(Request.QueryString["t"]);
                    int idUsuario = Convert.ToInt32(Request.QueryString["u"]);

                    Notificaciones notifi = new Notificaciones();
                    notifi.Extra = refaccion.Text + " de la orden " + Request.QueryString["o"] + " de la orden de compra con el folio " + Request.QueryString["fc"];//mensaje
                    notifi.Empresa = idEmpresa;
                    notifi.Taller = idTaller;
                    notifi.Usuario = idUsuario.ToString();
                    notifi.Fecha = fechas.obtieneFechaLocal();
                    notifi.Estatus = "P";
                    notifi.Clasificacion = 15;//numero mensage
                    notifi.Origen = "O";
                    notifi.armaNotificacion();
                    notifi.agregaNotificacion();
                }
                catch (Exception ex) 
                {
                    string error = "";
                }
            }
            grdRefacciones.EditIndex = -1;
            grdRefacciones.DataBind();
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    protected void lnkImprimeOrden_Click(object sender, EventArgs e)
    {

        OrdenCompra ordenCompra = new OrdenCompra();
        ordenCompra.sesiones = obtieneSesiones();

        object[] dato = ordenCompra.ordenAutorizada();
        if (Convert.ToBoolean(dato[0]))
        {
            if (Convert.ToBoolean(dato[1]))
            {
                int ordenGenerada = Convert.ToInt32(Request.QueryString["c"]);
                //IMPRIMIR EL TICKET
                ImpresionOrdenCompra imprime = new ImpresionOrdenCompra();
                int noOrden;
                int idEmpresa;
                int idTaller;
                int idCotizacion;
                noOrden = Convert.ToInt32(Request.QueryString["o"]);
                idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                idTaller = Convert.ToInt32(Request.QueryString["t"]);
                idCotizacion = Convert.ToInt32(Request.QueryString["c"]);
                int tamaño = Convert.ToInt32(ConfigurationManager.AppSettings["tamano"].ToString());

                string archivo = imprime.GenRepOrdTrabajo(noOrden, idEmpresa, idTaller, idCotizacion, 'T', Convert.ToInt32(Request.QueryString["u"]), tamaño, Request.QueryString["s"]);
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
            {
                Permisos permiso = new Permisos();
                permiso.idUsuario = Convert.ToInt32(Request.QueryString["u"]);
                permiso.permiso = 107;
                permiso.tienePermisoIndicado();

                if (permiso.tienePermiso)
                {
                    int ordenGenerada = Convert.ToInt32(Request.QueryString["c"]);
                    //IMPRIMIR EL TICKET
                    ImpresionOrdenCompra imprime = new ImpresionOrdenCompra();
                    int noOrden;
                    int idEmpresa;
                    int idTaller;
                    int idCotizacion;
                    noOrden = Convert.ToInt32(Request.QueryString["o"]);
                    idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                    idTaller = Convert.ToInt32(Request.QueryString["t"]);
                    idCotizacion = Convert.ToInt32(Request.QueryString["c"]);
                    int tamaño = Convert.ToInt32(ConfigurationManager.AppSettings["tamano"].ToString());

                    string archivo = imprime.GenRepOrdTrabajo(noOrden, idEmpresa, idTaller, idCotizacion, 'T', Convert.ToInt32(Request.QueryString["u"]), tamaño, Request.QueryString["s"]);
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
                {
                    txtContraseñaLog.Text = txtUsuarioLog.Text = lblErrorLog.Text = "";
                    PanelMask.Visible = PanelPopUpPermiso.Visible = true;
                }
            }
        }
        else
            lblError.Text = "Error: " + Convert.ToString(dato[1]);
    }

    protected void lnkNotificaOrden_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        OrdenCompra ordenCompra = new OrdenCompra();
        ordenCompra.sesiones = sesiones;
        object[] proveedores = ordenCompra.obtieneCorreoProveedor(sesiones);
        if (Convert.ToBoolean(proveedores[0]))
        {
            DataSet datos = (DataSet)proveedores[1];
            string correo = "";
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                if (fila[0].ToString() == Request.QueryString["pr"])
                {
                    correo = fila[2].ToString();
                    break;
                }
            }

            if (correo != "")
            {
                int ordenGenerada = Convert.ToInt32(Request.QueryString["c"]);
                Envio_Mail correoM = new Envio_Mail();
                string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Orden.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + ordenGenerada.ToString() + "&p=" + Request.QueryString["pr"] + "&s=REC";
                string mensaje = "<h3>Orden de Compra</h3><p>Estimado proveedor haga clic en el siguiente link para consultar la orden de compra de la o las refacciones que han sido entregadas.  Se incluyen refacciones que han sido canceladas por su cliente o devueltas. </p><br/>" +
                    "<p>Recuerde que debe imprimir su orden de compra para que al momento de solicitar su pago, debera presentar la orden de compra impresa asi como el comprobante de entregado de las refacciones. Ambos documnetos son necesarios para efectuar el pago; de otra forma el pago no sera realizado y tendra que acudir con el cliente para hacer la aclaraci&oacute;n necesario.</p><br/>" +
                    "<a href='" + url + "' target='_blank'>Orden de Compra Entregada</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...</p>";
                string asunto = "Orden de Compra Entregada";
                object[] enviado = correoM.obtieneDatosServidor("", correo, mensaje, "", asunto, null, sesiones[2], "", "");
                if (Convert.ToBoolean(enviado[0]))
                    lblError.Text = "La orden de compra fue enviada exitosamente.";
                else
                    lblError.Text = "Ocurrio un error al enviar la orden de compra a los proveedores. Detalle: " + Convert.ToString(enviado[1]);
            }
        }
        else
            lblError.Text = "El proveedor de la orden de compra no cuenta con un correo; solicitelo, actualice la información y vuelva a enviar el comprobante de la orden de compra";
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        Autorizaciones autoriza = new Autorizaciones();
        autoriza.nick = txtUsuarioLog.Text;
        autoriza.contrasena = txtContraseñaLog.Text;
        autoriza.permiso = 1;
        autoriza.validaUsuario();
        if (autoriza.Valido)
        {
            int[] sesiones = obtieneSesiones();
            OrdenCompra ordenCompra = new OrdenCompra();
            ordenCompra.sesiones = sesiones;
            int ordenGenerada = Convert.ToInt32(Request.QueryString["c"]);

            //ACTUALIZAR INFORMACION DE ORDEN DE COMPRA 

            object[] actualizado = ordenCompra.actualizaOrden(sesiones, autoriza.IdUsuario);

            if (Convert.ToBoolean(actualizado[0]))
            {
                //IMPRIMIR EL TICKET
                ImpresionOrdenCompra imprime = new ImpresionOrdenCompra();
                int noOrden;
                int idEmpresa;
                int idTaller;
                int idCotizacion;
                noOrden = Convert.ToInt32(Request.QueryString["o"]);
                idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                idTaller = Convert.ToInt32(Request.QueryString["t"]);
                idCotizacion = Convert.ToInt32(Request.QueryString["c"]);
                int tamaño = Convert.ToInt32(ConfigurationManager.AppSettings["tamano"].ToString());

                string archivo = imprime.GenRepOrdTrabajo(noOrden, idEmpresa, idTaller, idCotizacion, 'T', Convert.ToInt32(Request.QueryString["u"]), tamaño, Request.QueryString["s"]);
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
                    PanelMask.Visible = PanelPopUpPermiso.Visible = false;
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error al descargar el archivo: " + ex.Message;
                }
            }
            else
                lblError.Text = "Error al actualizar la orden de compra: " + Convert.ToString(actualizado[1]);
        }
        else
            lblErrorLog.Text = "Error: " + autoriza.Error;


    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        PanelMask.Visible = PanelPopUpPermiso.Visible = false;
    }

    protected void Fotos(object sender, EventArgs e)
    {
        DatosOrdenes datosOrdenes = new DatosOrdenes();
        LinkButton btn = (LinkButton)sender;
        string id = btn.CommandArgument;
        RadAsyncUpload AsyncUpload1 = grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("AsyncUpload1") as RadAsyncUpload;
        bool archivoValido = false;
        byte[] imagen = null;
        string[] archivosAborrar = null;
        //string extension = "";
        try
        {
            string filename = "";
            string extension = "";
            string ruta = Server.MapPath("~/TMP");
            string nomImgs = "";
            // Si el directorio no existe, crearlo
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
            int documentos = AsyncUpload1.UploadedFiles.Count;
            int guardados = 0;
            archivosAborrar = new string[documentos];
            for (int i = 0; i < documentos; i++)
            {
                filename = AsyncUpload1.UploadedFiles[i].FileName;
                string[] segmenatado = filename.Split(new char[] { '.' });
                archivoValido = validaArchivo(segmenatado[1]);
                extension = segmenatado[1];
                string archivo = String.Format("{0}\\{1}", ruta, filename);
                FileInfo file = new FileInfo(archivo);
                if (archivoValido)
                {
                    // Verificar que el archivo no exista
                    if (File.Exists(archivo))
                        file.Delete();
                    Telerik.Web.UI.UploadedFile up = AsyncUpload1.UploadedFiles[i];
                    up.SaveAs(archivo);
                    archivosAborrar[i] = archivo;
                    imagen = File.ReadAllBytes(archivo);
                    int[] sesiones = obtieneSesiones();
                    int idEmpresa = sesiones[2];
                    int idTaller = sesiones[3];
                    int noOrden = sesiones[4];
                    string nombre = filename;
                    bool agregado = datosOrdenes.agregaFotoRefacciones(idEmpresa, idTaller, noOrden, nombre, imagen, id);
                    if (agregado)
                        guardados++;
                    if (i == documentos - 1)
                        nomImgs += filename;
                    else
                        nomImgs += filename + " - ";
                }
                else
                    imagen = null;
            }
            for (int j = 0; j < archivosAborrar.Length; j++)
            {
                FileInfo archivoBorrar = new FileInfo(archivosAborrar[j]);
                archivoBorrar.Delete();
            }
            lblError.Text = "Se insertaron las imagenes: " + nomImgs;
            //DataListFotosDanos.DataBind();
        }
        catch (Exception ex) { /*lblErrorFotos.CssClass = "errores"; lblErrorFotos.Text = "Error: " + ex.Message;*/ }
    }

    private bool validaArchivo(string extencion)
    {
        string[] extenciones = { "jpeg", "jpg", "png", "gif", "bmp", "tiff" };
        bool valido = false;
        for (int i = 0; i < extenciones.Length; i++)
        {
            if (extencion.ToUpper() == extenciones[i].ToUpper())
            {
                valido = true;
                break;
            }
        }
        return valido;
    }

    protected void Image1_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imagenSel = (ImageButton)sender;
        PanelMascara.Visible = true;
        PanelImgZoom.Visible = true;
        //imgZoom.ImageUrl = "~/ImgEmpresas.ashx?id=" + id_empresa + ";" + id_taller + ";" + no_orden + ";" + consecutivo + ";" + proceso;
        imgZoom.ImageUrl = imagenSel.ImageUrl;
    }

    protected void btnCerrarImgZomm_Click(object sender, EventArgs e)
    {
        PanelMascara.Visible = false;
        PanelImgZoom.Visible = false;
    }

    protected void lnkEliminaFoto_Click(object sender, EventArgs e)
    {
        //DatosOrdenes datos = new DatosOrdenes();
        //imgZoom.ImageUrl = "~/ImgEmpresas.ashx?id=" + id_empresa + ";" + id_taller + ";" + no_orden + ";" + consecutivo + ";" + proceso;
        string[] imagenURLIgual = imgZoom.ImageUrl.Split('=');
        string[] imagenURL = imagenURLIgual[1].Split(';');
        object[] ejecutado = datos.eliminaFotoRefaccion(imagenURL);
        grdRefacciones.DataBind();
        cargaDatosEntrega();
        lblError.Text = "Foto eliminada";
        PanelMascara.Visible = false;
        PanelImgZoom.Visible = false;
    }

    protected void lnkEnviCorreo_Click(object sender, EventArgs e)
    {
        LinkButton lnkEnviCorreo = (LinkButton)sender;
        int[] sesiones = obtieneSesiones();
        OrdenCompra ordenCompra = new OrdenCompra();
        ordenCompra.sesiones = sesiones;
        object[] proveedores = ordenCompra.obtieneCorreoProveedor(sesiones);
        if (Convert.ToBoolean(proveedores[0]))
        {
            DataSet datos = (DataSet)proveedores[1];
            string correo = "";
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                if (fila[0].ToString() == Request.QueryString["pr"])
                {
                    correo = fila[2].ToString();
                    break;
                }
            }

            if (correo != "")
            {
                string vehiculoDatos = "";
                DatosVehiculos vehiculos = new DatosVehiculos();
                object[] vehiculo = vehiculos.obtieneDatosBasicosVehiculo(sesiones[4], sesiones[2], sesiones[3]);
                if (Convert.ToBoolean(vehiculo[0]))
                {
                    DataSet valores = (DataSet)vehiculo[1];
                    foreach (DataRow fila in valores.Tables[0].Rows)
                    {
                        vehiculoDatos = fila[1].ToString().ToUpper();
                        break;
                    }
                }

                Label lblRefaccion = grdRefacciones.Rows[grdRefacciones.TabIndex].FindControl("lblRefaccion") as Label;
                int ordenGenerada = Convert.ToInt32(Request.QueryString["c"]);
                Envio_Mail correoM = new Envio_Mail();
                string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Orden.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + ordenGenerada.ToString() + "&p=" + Request.QueryString["pr"] + "&s=REC";
                string mensaje = "<h3>Información sobre entrega de refacciones</h3><p>Estimado proveedor:<br/> Por medio del presente le informamos que la(s) refaccion(es) entregada(s) no cumplen con las especificaciones de la unidad a reparar, por lo cual le hacemos de su conocimiento que han sido devueltas o canceladas anexando imagen de la misma.</p><br/>" +
                    "<p>Debido a esta situación le sugerimos se contacte con el personal de Moncar Aztahuacan para obtener mas información a cerca de este problema.</p><br/>" +
                    "<p>No Orden: " + Request.QueryString["o"] + ", Vehículo: " + vehiculoDatos + "  Orden de Compra con Folio: " + Request.QueryString["fc"] + ", Refacci&oacute;n: " + lblRefaccion.Text + "</p><br/>" +
                    "<br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...</p>";
                string asunto = "Información sobre entrega de refacciones";
                ListBox imagenes = new ListBox();

                //ObtieneImagenes int noOrden, int idEmpresa, int idTaller, int idRefaccion)
                object[] imags = ordenCompra.obtieneFotosRefaccion(sesiones[4], sesiones[2], sesiones[3], Convert.ToInt32(lnkEnviCorreo.CommandArgument.ToString()));
                if (Convert.ToBoolean(imags[0]))
                {
                    DataSet fotos = (DataSet)imags[1];
                    if (fotos != null)
                    {
                        foreach (DataRow fila in fotos.Tables[0].Rows)
                        {
                            string nombreFoto = fila[0].ToString();
                            byte[] imagenBuffer = (byte[])fila[1];
                            System.IO.MemoryStream st = new System.IO.MemoryStream(imagenBuffer);
                            System.Drawing.Image foto = System.Drawing.Image.FromStream(st);
                            ListItem adjuntosf = new ListItem();
                            adjuntosf.Value = adjuntosf.Text = nombreFoto.Trim();
                            string[] infoImagen = nombreFoto.Split(new char[] { '.' });
                            string extencion = infoImagen[1].ToLower();
                            System.Drawing.Imaging.ImageFormat formato;
                            switch (extencion)
                            {
                                case "jpg":
                                    formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                    break;
                                case "png":
                                    formato = System.Drawing.Imaging.ImageFormat.Png;
                                    break;
                                case "jpeg":
                                    formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                    break;
                                case "gif":
                                    formato = System.Drawing.Imaging.ImageFormat.Gif;
                                    break;
                                case "bmp":
                                    formato = System.Drawing.Imaging.ImageFormat.Bmp;
                                    break;
                                case "tiff":
                                    formato = System.Drawing.Imaging.ImageFormat.Tiff;
                                    break;
                                default:
                                    formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                                    break;
                            }
                            foto.Save(adjuntosf.Text, formato);
                            imagenes.Items.Add(adjuntosf);
                        }
                    }
                    object[] enviado = correoM.obtieneDatosServidor("", correo, mensaje, "", asunto, imagenes, sesiones[2], "", "");
                    if (Convert.ToBoolean(enviado[0]))
                        lblError.Text = "Correo enviado exitosamente.";
                    else
                        lblError.Text = "Ocurrio un error al enviar el correo. Detalle: " + Convert.ToString(enviado[1]);
                }
            }
            else
                lblError.Text = "El proveedor de la orden de compra no cuenta con un correo; solicitelo, actualice la información y vuelva a enviar el comprobante de la orden de compra";
        }
    }

    protected void lnkimptk_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        OrdenCompra folio = new OrdenCompra();
        folio.empresa = sesiones[2];
        folio.taller = sesiones[3];
        folio.orden = sesiones[4];

        LinkButton aut = (LinkButton)sender;
        string[] arg = aut.CommandArgument.ToString().Split(new char[] { ';' });
        int refaccion = Convert.ToInt32(arg[0]);
        folio.refaccion = refaccion;
       
        folio.CambiarEstatusI();
        grdRefacciones.DataBind();

    }

    protected void lnknoimptk_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        OrdenCompra folio = new OrdenCompra();
        folio.empresa = sesiones[2];
        folio.taller = sesiones[3];
        folio.orden = sesiones[4];

        LinkButton aut = (LinkButton)sender;
        string[] arg = aut.CommandArgument.ToString().Split(new char[] { ';' });
        int refaccion = Convert.ToInt32(arg[0]);
        folio.refaccion = refaccion;

        folio.CambiarEstatusN();
        grdRefacciones.DataBind();
    }

    

    
}

