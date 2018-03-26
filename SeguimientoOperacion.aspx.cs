using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class SeguimientoOperacion : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    CalificaOperarioDatos operaDatos = new CalificaOperarioDatos();
    Fechas fechas = new Fechas();
    Permisos permisos = new Permisos();
    int gruposOrden, gruposTerminados, contadorCheck100;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargaDatosPie();
            gruposOrden = gruposTerminados = 0;
            obtieneGrupos();
            Session["cien"] = "0";
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    lnkGuardar.Visible = lnkPantallas.Visible = false;                    
                    Label13.Visible = rbtGOP.Visible = false;
                    txtAvance.Enabled = ddlPerfil.Enabled = ddlLocalizacion.Enabled = ListGrupos.Enabled = false;
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
            Session["paginaOrigen"] = "Remisiones.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
    private void habilitaChecks(CheckBox[] chks, int gop)
    {
        
            CheckBox chk25Enables = chks[0] as CheckBox;
            CheckBox chk50Enables = chks[1] as CheckBox;
            CheckBox chk75Enables = chks[2] as CheckBox;
            CheckBox chk100Enables = chks[3] as CheckBox;
            CheckBox chkTerEnables = chks[4] as CheckBox;
            if (chk25Enables.Checked)
            {
                enablesChecks(chks, gop, true, false, false, false, false);
                if (chk50Enables.Checked)
                {
                    enablesChecks(chks, gop, false, true, true, false, false);
                    if (chk75Enables.Checked)
                    {
                        enablesChecks(chks, gop, false, false, true, true, false);
                        if (chk100Enables.Checked)
                        {
                            enablesChecks(chks, gop, false, false, false, true, true);
                            if (chkTerEnables.Checked)
                            {
                                enablesChecks(chks, gop, false, false, false, false, true);
                            }
                            else
                                enablesChecks(chks, gop, false, false, false, true, true);
                        }
                        else
                        {
                            enablesChecks(chks, gop, false, false, true, true, false);
                            chkTerEnables.Checked = false;
                        }
                    }
                    else
                        enablesChecks(chks, gop, false, true, true, false, false);
                }
                else
                    enablesChecks(chks, gop, true, true, false, false, false);
            }
            else
                enablesChecks(chks, gop, true, false, false, false, false);
    }

    private void enablesChecks(object[] chks, int gop, bool v1, bool v2, bool v3, bool v4, bool v5)
    {
        ((CheckBox)chks[0]).Enabled = v1;
        ((CheckBox)chks[1]).Enabled = v2;
        ((CheckBox)chks[2]).Enabled = v3;
        ((CheckBox)chks[3]).Enabled = v4;
        ((CheckBox)chks[4]).Enabled = v5;
    }

    private void obtieneGrupos()
    {
        int noOrden = 0;
        int idEmpresa = 0;
        int idTaller = 0;

        try
        {
            noOrden = Convert.ToInt32(Request.QueryString["o"]);
            idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            idTaller = Convert.ToInt32(Request.QueryString["t"]);
            AvancesOperacion datos = new AvancesOperacion();
            List<AvancesOperacion> group = new List<AvancesOperacion>();
            datos.orden = noOrden;
            datos.empresa = idEmpresa;
            datos.taller = idTaller;
            group = datos.obtieneGrupos();
            ListGrupos.DataSource = group;
            ListGrupos.DataBind();
        }
        catch (Exception ex)
        {
            ListGrupos.DataSource = null;
            ListGrupos.DataBind();
            lblErroresOO.Text = "Se produjo un error inesperado: " + ex.Message.ToString();
        }
        finally {
            Cronos cronologico = new Cronos();
            cronologico.Empresa = idEmpresa;
            cronologico.Taller = idTaller;
            cronologico.Orden = noOrden;
            cronologico.obtieneFechas();
            string[] datos = cronologico.Fechas;
            try { lblFestimada.Text = "Fecha Promesa: " + convierteFecha(datos[15]) + " " + Convert.ToDateTime(datos[16]).ToString("HH:mm:ss"); } catch (Exception) { lblFestimada.Text = "Fecha Promesa: "; }
            try { lblFpactada.Text = "Fecha Pactada: " + convierteFecha(datos[19]) + " " + Convert.ToDateTime(datos[20]).ToString("HH:mm:ss"); } catch (Exception) { lblFestimada.Text = "Fecha Pactada: "; }
            try { lblFConfirmacion.Text = "Fecha Confirmación: " + convierteFecha(datos[23]) + " " + Convert.ToDateTime(datos[24]).ToString("HH:mm:ss"); } catch (Exception) { lblFestimada.Text = "Fecha Confirmación: "; }

            /*if (txtfTerminado.Text == "" || txtfTerminado.Text == "1900-01-01")
            {
                lnkGuardar.Visible = true;
                ListGrupos.Enabled = true;
            }
            else
            {
                ListGrupos.Enabled = false;
                lnkGuardar.Visible = false;
            }*/
            ddlPerfil.SelectedValue = lblPerfilIni.Text;
            ddlLocalizacion.SelectedValue = lblLocalizacionIni.Text;
            txtAvance.Text = lblAvanceIni.Text;
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
                lblPerfilPie.Text = filaOrd[13].ToString();
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
                lblFaseIni.Text = filaOrd[6].ToString();
                lblLocalizacionIni.Text = filaOrd[7].ToString();
                lblAvanceIni.Text = filaOrd[8].ToString();
                lblPerfilIni.Text = filaOrd[12].ToString();
            }
        }
    }

    private string convierteFecha(string valor)
    {
        string retorno = ""; ;
        DateTime fecha = Convert.ToDateTime("1900-01-01");
        try
        {
            fecha = Convert.ToDateTime(valor);
            if (fecha.ToString("yyyy-MM-dd") == "1900-01-01")
                retorno = "";
            else
                retorno = fecha.ToString("yyyy-MM-dd");            
        }
        catch (Exception) { retorno = ""; }
        return retorno;
    }

    protected void lnkGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            gruposOrden = gruposTerminados = 0;
            foreach (ListViewDataItem grupo in ListGrupos.Items)
            {
                Label id = grupo.FindControl("lblIdGrupo") as Label;
                Label descrip = grupo.FindControl("lblGrupo") as Label;
                CheckBox p25 = grupo.FindControl("chkP25") as CheckBox;
                CheckBox p50 = grupo.FindControl("chkP50") as CheckBox;
                CheckBox p75 = grupo.FindControl("chkP75") as CheckBox;
                CheckBox p100 = grupo.FindControl("chkP100") as CheckBox;
                CheckBox Vobo = grupo.FindControl("chkVoBo") as CheckBox;
                CheckBox[] cheks = new CheckBox[5] { p25, p50, p75, p100, Vobo };
                guardaOpSeg(Convert.ToInt32(id.Text), cheks, descrip.Text,grupo);
                string texto = actualizaFechaTermino();
                lblErroresOO.Text += texto;
                gruposOrden++;
                if (Vobo.Checked)
                    gruposTerminados++;
            }

            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            int idUsuario = Convert.ToInt32(Request.QueryString["u"]);

            decimal porcentaje = Convert.ToDecimal(txtAvance.Text);
            

            Recepciones recepciones = new Recepciones();

            object[] obtieneAvance = recepciones.obtieneAvance(idEmpresa, idTaller, noOrden);
            if (Convert.ToBoolean(obtieneAvance[0]))
            {
                porcentaje = Convert.ToDecimal(obtieneAvance[1]);
                txtAvance.Text = porcentaje.ToString();
                lblAvanceIni.Text = porcentaje.ToString();
            }

            lblPerfilIni.Text = ddlPerfil.SelectedValue;
            lblLocalizacionIni.Text= ddlLocalizacion.SelectedValue;

            object[] actualizados = actualizados = recepciones.actualizaOrdenDatos(idEmpresa, idTaller, noOrden, ddlLocalizacion.SelectedValue, txtAvance.Text, ddlPerfil.SelectedValue);
            if (Convert.ToBoolean(actualizados[0]))
            {
                BitacorasComentarios bitacora = new BitacorasComentarios();
                bitacora.Empresa = idEmpresa;
                bitacora.Taller = idTaller;
                bitacora.Orden = noOrden;
                bitacora.Usuario = idUsuario;

                if (ddlLocalizacion.SelectedValue != lblLocalizacionIni.Text)
                {
                    bitacora.Bitacora = 2;
                    bitacora.Valor = ddlLocalizacion.SelectedValue;
                    bitacora.agregaRegistro();
                    actualizados = bitacora.Afectado;
                    if (Convert.ToBoolean(actualizados[0])) { }
                }

                BitacorasComentarios bitacoraA = new BitacorasComentarios();
                bitacoraA.Empresa = idEmpresa;
                bitacoraA.Taller = idTaller;
                bitacoraA.Orden = noOrden;
                bitacoraA.Bitacora = 1;
                bitacoraA.Usuario = idUsuario;
                bitacoraA.Valor = porcentaje.ToString();
                bitacoraA.agregaRegistro();
                object[] afectado = bitacora.Afectado;                
            }

            int registros = ListGrupos.Items.Count;
            
            foreach (ListViewItem item in ListGrupos.Items)
            {
                DropDownList ddlCalificacion = item.FindControl("ddlCalificacion") as DropDownList;
                LinkButton lnkCalifica = item.FindControl("lnkCalifica") as LinkButton;
                noOrden = Convert.ToInt32(Request.QueryString["o"]);
                idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                idTaller = Convert.ToInt32(Request.QueryString["t"]);
                string[] grupos = lnkCalifica.CommandArgument.ToString().Split(new char[] { '-' });
                int idGops = Convert.ToInt32(grupos[0]);
                int calificacion = Convert.ToInt32(ddlCalificacion.SelectedValue);
                bool actualizado = operaDatos.actualizaCalificacion(noOrden, idEmpresa, idTaller, idGops, calificacion);
            }
            
            obtieneGrupos();
            cargaDatosPie();
            actualizaFase();
        }
        catch (Exception ex) { lblErroresOO.Text = "Error: " + ex.Message; }
    }

    private string actualizaFechaTermino()
    {
        string fechaActualizada = "";
        Cronos datosCronos = new Cronos();
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        datosCronos.obtieneIncompletos(noOrden, idEmpresa, idTaller);
        object[] actualizado = datosCronos.Actualizacion;
        if ((bool)actualizado[0])
        {
            int incompletos = Convert.ToInt32(actualizado[1]);
            if (incompletos == 0)
            {
                datosCronos.existeFechaTermino(noOrden.ToString(), idEmpresa.ToString(), idTaller.ToString());
                actualizado = datosCronos.Actualizacion;
                if ((bool)actualizado[0])
                {
                    int existeTermino = Convert.ToInt32(actualizado[1]);
                    if (existeTermino == 0)
                    {
                        datosCronos.actualizaTerminado(noOrden.ToString(), idEmpresa.ToString(), idTaller.ToString(), Request.QueryString["u"]);
                        actualizado = datosCronos.Actualizacion;
                        if ((bool)actualizado[0])
                            if ((bool)actualizado[1])
                                fechaActualizada = ", La fecha de termino fue actualizada exitosamente";
                    }
                }
            }
        }
        return fechaActualizada;
    }

    private void guardaFechas(string fechaTocado, string fechaTerminado, int gruposOrden, int gruposTerminados)
    {
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        DateTime fecha1, fecha2;
        try {
            fecha1 = Convert.ToDateTime(fechaTocado);
        }
        catch (Exception) { fecha1 = Convert.ToDateTime("1900-01-01"); }
        try
        {
            fecha2 = Convert.ToDateTime(fechaTerminado);
        }
        catch (Exception) { fecha2 = Convert.ToDateTime("1900-01-01"); }

        object[] actualizado = operaDatos.actualizaCronos(noOrden, idEmpresa, idTaller, fecha1.ToString("yyyy-MM-dd"), fecha2.ToString("yyyy-MM-dd"));

    }

    private void guardaOpSeg(int gop, CheckBox[] chks, string descrip, ListViewDataItem grupo)
    {
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        int idUsuario = Convert.ToInt32(Request.QueryString["u"]);
        string operacion = descrip.ToUpper();        
        CheckBox chk25InsUpd = chks[0] as CheckBox;
        CheckBox chk50InsUpd = chks[1] as CheckBox;
        CheckBox chk75InsUpd = chks[2] as CheckBox;
        CheckBox chk100InsUpd = chks[3] as CheckBox;
        CheckBox chkTerInsUpd = chks[4] as CheckBox;
        object[] existeResul = operaDatos.existeSegOp(noOrden, idEmpresa, idTaller, gop);
        string usuarioN = operaDatos.obtieneNombreUsuario(idUsuario);
        string observacion = "El usuario " + usuarioN + " modifico " + operacion + " 25% " + textoToF(chk25InsUpd.Checked) + " 50% " + textoToF(chk50InsUpd.Checked) + " 75% " + textoToF(chk75InsUpd.Checked) + " 100% " + textoToF(chk100InsUpd.Checked) + " VoBo " + textoToF(chkTerInsUpd.Checked);
        if ((bool)existeResul[0])
        {
            bool existe = Convert.ToBoolean(existeResul[1]);
            if (!existe)
            {
                object[] registrado = operaDatos.insertaSegOp(noOrden, idEmpresa, idTaller, gop, chk25InsUpd.Checked, chk50InsUpd.Checked, chk75InsUpd.Checked, chk100InsUpd.Checked, chkTerInsUpd.Checked, idUsuario, observacion);
                if ((bool)registrado[0])
                    if ((bool)registrado[1])
                    {
                        lblErroresOO.Text = "Se guardaron los cambios exitosamente";
                        ListGrupos.DataBind();
                    }
                    else
                        lblErroresOO.Text = "Hubo un error al guardar los cambios, verifique su conexión e intentelo nuevamente";
                else
                    lblErroresOO.Text = "Se produjo un error inseperado: " + registrado[1].ToString();
            }
            else
            {
                object[] actualizado = operaDatos.actualizaSegOp(noOrden, idEmpresa, idTaller, gop, chk25InsUpd.Checked, chk50InsUpd.Checked, chk75InsUpd.Checked, chk100InsUpd.Checked, chkTerInsUpd.Checked, idUsuario, observacion);
                if ((bool)actualizado[0])
                    if ((bool)actualizado[1])
                        lblErroresOO.Text = "Se guardaron los cambios exitosamente";
                    else
                        lblErroresOO.Text = "Hubo un error al guardar los cambios, verifique su conexión e intentelo nuevamente";
                else
                    lblErroresOO.Text = "Se produjo un error inseperado: " + actualizado[1].ToString();
            }
        }

        if (chk100InsUpd.Checked)
        {
            Notificaciones notifi = new Notificaciones();
            notifi.Articulo = Request.QueryString["o"];
            notifi.Empresa = Convert.ToInt32(Request.QueryString["e"]);
            notifi.Taller = Convert.ToInt32(Request.QueryString["t"]);
            notifi.Usuario = Request.QueryString["u"];
            notifi.Fecha = fechas.obtieneFechaLocal();
            notifi.Estatus = "P";
            notifi.Clasificacion = 9;
            notifi.Origen = "O";
            notifi.Extra = operacion;
            notifi.armaNotificacion();
            notifi.agregaNotificacion();
        }

        if (chkTerInsUpd.Checked) {
            try
            {
                DropDownList ddlCalificacion = grupo.FindControl("ddlCalificacion") as DropDownList;
                LinkButton lnkCalifica = grupo.FindControl("lnkCalifica") as LinkButton;
                
                int idGops = Convert.ToInt32(lnkCalifica.CommandArgument);
                int calificacion = Convert.ToInt32(ddlCalificacion.SelectedValue);
                bool actualizado = operaDatos.actualizaCalificacion(noOrden, idEmpresa, idTaller, idGops, calificacion);                
            }
            catch (Exception ex)
            {
                lblErroresOO.Text = "Ocurrio un error al guardar la calificacion: " + ex.Message.ToString();
            }
        }
        actualizaFase();
    }

    private string textoToF(bool checado)
    {
        if (checado)
            return "Si";
        else
            return "No";
    }


    protected void ListGrupos_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try {
            Label id = e.Item.FindControl("lblIdGrupo") as Label;
            CheckBox p25 = e.Item.FindControl("chkP25") as CheckBox;
            CheckBox p50 = e.Item.FindControl("chkP50") as CheckBox;
            CheckBox p75 = e.Item.FindControl("chkP75") as CheckBox;
            CheckBox p100 = e.Item.FindControl("chkP100") as CheckBox;
            CheckBox Vobo = e.Item.FindControl("chkVoBo") as CheckBox;
            DropDownList ddlCalificacion = e.Item.FindControl("ddlCalificacion") as DropDownList;
            LinkButton lnkCalifica = e.Item.FindControl("lnkCalifica") as LinkButton;
            p25.Enabled = p50.Enabled = p75.Enabled = p100.Enabled = Vobo.Enabled = false;
            gruposOrden++;
            string noOrden="";
            string idEmpresa="";
            string idTaller = "";
            if (Vobo.Checked)
            {
                
                string idCalificacion = "0";
                try
                {
                    noOrden = Request.QueryString["o"];
                    idEmpresa = Request.QueryString["e"];
                    idTaller = Request.QueryString["t"];
                    idCalificacion = operaDatos.obtieneCalificacionGO(id.Text, noOrden, idEmpresa, idTaller);
                }
                catch (Exception)
                {
                    Response.Redirect("Default.aspx");
                }

                gruposTerminados++;
                ddlCalificacion.SelectedValue = idCalificacion;
                ddlCalificacion.Visible = true;
                lnkCalifica.Visible = false;

                bool existen = false;
                object[] existeOp = operaDatos.existeOperarioasignado(Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"], id.Text);
                if (Convert.ToBoolean(existeOp[0]))
                    existen = Convert.ToBoolean(existeOp[1]);
                else
                    existen = false;
                ddlCalificacion.Enabled = existen;
                //lnkCalifica.Visible = existen;

            }
            else
                ddlCalificacion.Visible = lnkCalifica.Visible = false;
            CheckBox[] cheks = new CheckBox[5] { p25, p50, p75, p100, Vobo };
            habilitaChecks(cheks, Convert.ToInt32(id.Text));

            controlAccesos(Vobo);
        }
        catch (Exception ex) { lblErroresOO.Text = "Error: " + ex.Message; }
    }


    protected void CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gruposOrden = gruposTerminados = 0;
            foreach (ListViewDataItem grupo in ListGrupos.Items) {
                Label id = grupo.FindControl("lblIdGrupo") as Label;
                CheckBox p25 = grupo.FindControl("chkP25") as CheckBox;
                CheckBox p50 = grupo.FindControl("chkP50") as CheckBox;
                CheckBox p75 = grupo.FindControl("chkP75") as CheckBox;
                CheckBox p100 = grupo.FindControl("chkP100") as CheckBox;
                CheckBox Vobo = grupo.FindControl("chkVoBo") as CheckBox;
                DropDownList ddlCalificacion = grupo.FindControl("ddlCalificacion") as DropDownList;
                LinkButton lnkCalifica = grupo.FindControl("lnkCalifica") as LinkButton;
                CheckBox[] cheks = new CheckBox[5] { p25, p50, p75, p100, Vobo };
                habilitaChecks(cheks, Convert.ToInt32(id.Text));
                gruposOrden++;
                if (Vobo.Checked)
                {
                    gruposTerminados++;
                    ddlCalificacion.Visible = true;
                    lnkCalifica.Visible = false;
                    bool existen = false;
                    object[] existeOp = operaDatos.existeOperarioasignado(Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"], id.Text);
                    if (Convert.ToBoolean(existeOp[0]))
                        existen = Convert.ToBoolean(existeOp[1]);
                    else
                        existen = false;
                    ddlCalificacion.Enabled = existen;
                    lnkCalifica.Visible = false;
                }
                else
                    ddlCalificacion.Visible = lnkCalifica.Visible = false;
            }
            /*if (gruposOrden == gruposTerminados)
                Label9.Visible = txtfTerminado.Visible = lnkTerminado.Visible = true;
            else
            {
                Label9.Visible = txtfTerminado.Visible = lnkTerminado.Visible = false;
                txtfTerminado.Text = "";
            }*/
        }
        catch (Exception ex) { lblErroresOO.Text = "Error: " + ex.Message; }
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

                if (faseSActual < 6)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 6);
                }
            }
        }
        catch (Exception) { }

    }

    protected void lnkPantallas_Click(object sender, EventArgs e)
    {
        lblErroresOO.Text = "";
        LinkButton lnkPantallas = (LinkButton)sender;
        if (rbtGOP.SelectedValue != "")
        {
            fechas.fecha = fechas.obtieneFechaLocal();
            fechas.tipoFormato = 4;
            string fechaRetorno = fechas.obtieneFechaConFormato();
            fechas.tipoFormato = 6;
            string horaRetorno = fechas.obtieneFechaConFormato();

            int noOrden = Convert.ToInt32(Request.QueryString["o"]);
            int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
            int idTaller = Convert.ToInt32(Request.QueryString["t"]);
            int idGop = Convert.ToInt32(rbtGOP.SelectedValue);
            int idUsuario = Convert.ToInt32(Request.QueryString["u"]);
            string fecha = fechaRetorno;
            string hora = horaRetorno;
            bool inserta = false;
            object[] ejecutado = new object[2];
            ejecutado = operaDatos.insertaBitVistaPatio(noOrden, idEmpresa, idTaller, idGop, idUsuario, fecha, hora);
            if ((bool)ejecutado[0])
                inserta = Convert.ToBoolean(ejecutado[1]);
            else
                inserta = false;
            if (inserta)
            {
                lblErroresOO.Text="La seleccion de vista prioritaria a sido modificada con exito a: "+rbtGOP.SelectedItem.Text;
                /*string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/PantallaPatio.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&gop=" + idGop.ToString() + "&op=2";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "');", true);*/
            }
            else
                lblErroresOO.Text = "Ocurrio un error inesperado: " + ejecutado[1].ToString();
        }
        else
            lblErroresOO.Text = "Nesecita seleccionar un Grupo para generar la visualización";
    }

    private void controlAccesos(CheckBox chk)
    {
        permisos.idUsuario = Convert.ToInt32(Request.QueryString["u"]);
        /*permisos.obtienePermisos();
        bool[] permisosUsuario = permisos.permisos;
        permisos.permisos = permisosUsuario;*/
        permisos.permiso = 79;
        permisos.tienePermisoIndicado();
        if (!permisos.tienePermiso)
            chk.Visible = false;
        else
            chk.Visible = true;
    }

    protected void ListGrupos_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if(e.CommandName=="califica")
        {
            lblErroresOO.Text = "";
            try
            {
                DropDownList ddlCalificacion = e.Item.FindControl("ddlCalificacion") as DropDownList;
                LinkButton lnkCalifica = e.Item.FindControl("lnkCalifica") as LinkButton;
                int noOrden = Convert.ToInt32(Request.QueryString["o"]);
                int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                int idTaller = Convert.ToInt32(Request.QueryString["t"]);
                int idGops = Convert.ToInt32(lnkCalifica.CommandArgument);
                int calificacion = Convert.ToInt32(ddlCalificacion.SelectedValue);
                bool actualizado = operaDatos.actualizaCalificacion(noOrden, idEmpresa, idTaller, idGops, calificacion);
                if (actualizado)
                    lblErroresOO.Text = "Calificación actualizada exitosamente.";
                else
                    lblErroresOO.Text = "Ocurrio un error al guardar la calificacion.";
            }
            catch (Exception ex)
            {
                lblErroresOO.Text = "Ocurrio un error al guardar la calificacion: " + ex.Message.ToString();
            }
        }
    }
}