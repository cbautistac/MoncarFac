using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Drawing;
using System.Data;
using System.IO;
using E_Utilities;

public partial class Asignacion : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    ManoObraOrden datosMO = new ManoObraOrden();
    Catalogos datosCat = new Catalogos();
    Fechas fechas = new Fechas();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            cargaDatosPie();
            cargaFechaPromesa();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    txtOperativo.Visible = false;
                    Label7.Visible = false;
                    txtFechaIniProg.Visible = false;
                    lnkFechaIniProg.Visible = Label11.Visible = false;
                    txtMonto.Visible = Panel4.Visible = false;
                    lnkBuscar.Visible = lnkFechaPromesa.Visible = Label5.Visible = false;
                    lnkAgregarOperario.Visible = false;
                    grdOperativosOrden.Columns[7].Visible = grdOperativosOrden.Columns[8].Visible = false;
                    Label14.Visible = false;
                    lnkNotificar.Visible = false;
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

    private void cargaFechaPromesa()
    {
        string existe = "";
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        try
        {
            existe = datosMO.obtieneFechaPromesa(noOrden, idTaller, idEmpresa);
            string[] promesas = existe.Split(';');
            txtFechaPromesa.Text = promesas[0];
            txtHoraPromesa.Text = promesas[1];
        }
        catch (Exception x)
        {
            txtFechaPromesa.Text = "";
            txtHoraPromesa.Text = "";
        }
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
            sesiones = new int[6] { 0, 0, 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
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
                lblFaseIni.Text = filaOrd[6].ToString();
                lblLocalizacionIni.Text = filaOrd[7].ToString();
                lblAvanceIni.Text = filaOrd[8].ToString();
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
            }
        }
    }

    protected void lnkOperario_Click(object sender, EventArgs e)//juan-9-12-2015
    {
        try
        {
            LinkButton lnkOperario = (LinkButton)sender;
            string[] argumentos = lnkOperario.CommandArgument.ToString().Split(';');
            
            GridOperarios.DataBind();            
        }
        catch (Exception) { }
    }

    protected void GridOperarios_RowDataBound(object sender, GridViewRowEventArgs e)//juan-9-12-2015
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPichonera = e.Row.FindControl("lblPichonera") as Label;
            Label lblAsigOperMO = e.Row.FindControl("lblAsigOperMO") as Label;
            LinkButton lnkAsigOperMO = e.Row.FindControl("lnkAsigOperMO") as LinkButton;
            int disponibles = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "resta").ToString());
            try
            {
                if (disponibles < 1)
                    lblAsigOperMO.ForeColor = Color.Red;
                else
                    lblAsigOperMO.ForeColor = Color.Empty;
            }
            catch (Exception ex) { }
        }
    }

    protected void lnkAsigOperMO_Click(object sender, EventArgs e)
    {
        LinkButton lnkAsigOperMO = (LinkButton)sender;
        string[] datoOperario = lnkAsigOperMO.CommandArgument.ToString().Split(new char[] { ';' });
        
        int cajones = Convert.ToInt32(datoOperario[2]);
        if (cajones > 0)
        {
            lblIdOperativo.Text = datoOperario[0];
            txtOperativo.Text = datoOperario[1];
            Panel2.Visible = false;
            Panel3.Visible = false;
            Label2.Visible = false;
            Panel1.Visible = true;
            lnkCancelaVista.Visible = false;
        }
        else
            lblError.Text = "No es posible asignarle mas trabajo al operario ya que llego a su límite de capacidad";
    }

    private void acutalizaFase()
    {
        int faseSActual = Convert.ToInt32(lblFaseIni.Text);
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

                if (faseSActual < 3)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 3);
                }
            }
        }
        catch (Exception) { }
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
        catch (Exception ex) { return false; }
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
        catch (Exception ex)
        {
            try
            {
                if (tiempo.Length == 7)
                    tiempo = tiempo.PadLeft(8, '0');
                DateTime.ParseExact(tiempo, "HH:mm:ss", CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception ex1) { return false; }
        }
    }

    protected void GridManoObra_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            try
            {
                
                GridOperarios.Visible = true;
                string[] argumentos = e.CommandArgument.ToString().Split(';');
                
                Session["consecutivoMo"] = argumentos[2];
                
                int noOrden, idEmpresa, idTaller, idConsecutivoMO;
                noOrden = idEmpresa = idTaller = idConsecutivoMO = 0;
                noOrden = Convert.ToInt32(Request.QueryString["o"]);
                idEmpresa = Convert.ToInt32(argumentos[0]);
                idTaller = Convert.ToInt32(argumentos[1]);
                idConsecutivoMO = Convert.ToInt32(argumentos[2]);
                bool asignado = datosMO.operadorMOAsignado(noOrden, idEmpresa, idTaller, idConsecutivoMO);
                if (asignado)
                {
                   

                    // verificar si mano de obra esta en t si lo operativos asignaods = operativos asignados en estatus t

                
                    GridOperarios.Visible = true;
                    GridOperarios.DataBind();
                }
                else
                {
                    GridOperarios.Visible = true;
                    GridOperarios.DataBind();
                }
            }
            catch (Exception ex) { }
            
        }
    }

    protected void GridAsiganados_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            
            GridOperarios.DataBind();
        }
    }

    protected void GridAsiganados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOperativoAsig = e.Row.FindControl("lblOperativoAsig") as Label;
            Label lblIdEmpleadoAsig = e.Row.FindControl("lblIdEmpleadoAsig") as Label;
            Label lblEstatusMO = e.Row.FindControl("lblEstatusMO") as Label;
            Label lblEstatusDes = e.Row.FindControl("lblEstatusDes") as Label;
            LinkButton btnAsignar = e.Row.FindControl("btnAsignar") as LinkButton;
            LinkButton btnReasignar = e.Row.FindControl("btnReasignar") as LinkButton;
            string nomOperario = "", estatus = "";
            int idEmpleado = 0;
            estatus = lblEstatusMO.Text;
            try
            {

                idEmpleado = Convert.ToInt32(lblIdEmpleadoAsig.Text);
                if (idEmpleado != 0)
                {
                    nomOperario = datosMO.obtieneNombreEmpleadoRH(idEmpleado);
                    if (nomOperario == "")
                    {
                        lblOperativoAsig.Text = "Sin operario asignado";
                        btnReasignar.Visible = false;
                    }
                    else
                    {
                        lblOperativoAsig.Text = nomOperario;
                        if (estatus == "T")
                            btnReasignar.Visible = false;
                        else
                            btnReasignar.Visible = true;
                    }
                }
                else
                {
                    lblOperativoAsig.Text = "Sin operario asignado";
                    btnReasignar.Visible = false;
                }
            }
            catch (Exception)
            {
                lblOperativoAsig.Text = "Sin operario asignado";
                btnReasignar.Visible = false;
            }
        }
    }

    protected void GridManoObra_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var btnEliminar = e.Row.Cells[3].Controls[1].FindControl("btnAsignacion") as LinkButton;
            string mo = DataBinder.Eval(e.Row.DataItem, "consecutivo").ToString();
            if (datosMO.obtieneOperariosMo(Convert.ToInt32(Request.QueryString["o"]), Convert.ToInt32(Request.QueryString["t"]), Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(mo)) > 0)
            {
                int operariosTerminados = datosMO.obtieneOperariosTerminado(Convert.ToInt32(Request.QueryString["o"]), Convert.ToInt32(Request.QueryString["t"]), Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(mo));

                if (operariosTerminados == 0)
                    btnEliminar.Visible = true;
                else
                    btnEliminar.Visible = true;
            }
            else
                btnEliminar.Visible = true;
        }
    }

    protected void lnkBuscar_Click(object sender, EventArgs e)
    {
        GridOperarios.DataBind();
        Panel2.Visible = true;        
        Panel3.Visible = false;
        Label2.Visible = true;
        Panel1.Visible = false;
        lnkCancelaVista.Visible = true;
        txtBuscaOperario.Text = "";
        lblError.Text = "";
    }
    protected void lnkDetalleOperario_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] datos = btn.CommandArgument.Split(new char[]{';'});
        Panel3.Visible = true;
        lblOrdAsig.Text = "Ordenes Asignadas de " + datos[1];
        SqlDataSource2.SelectCommand = "select o.no_orden,t.nombre_taller,o.id_asignacion,convert(char(10),o.fecha_ini_prog,126)+' '+convert(char(8),o.hora_ini_prog,108) as fecha_ini_prog,convert(char(10),o.fecha_ini,126)+' '+convert(char(8),o.hora_ini,108) as fecha_ini,convert(char(10),o.fecha_fin,126)+' '+convert(char(8),o.hora_fin,108) as fecha_fin,o.estatus from operativos_orden o inner join talleres t on t.id_taller=o.id_taller where o.idEmp=" + datos[0] + " and (o.fecha_fin is null or Convert(char(10),o.fecha_fin,126)='' or Convert(char(10),o.fecha_fin,126)='1900-01-01' or o.estatus<>'T') order by t.id_taller asc, o.no_orden desc,o.id_asignacion asc";
        grdDetalle.DataBind();
    }
    protected void lnkAgregarOperario_Click(object sender, EventArgs e)
    {
        string idGops = "";

        List<string> values = new List<string>();
        foreach (ListItem Item in chkGopsOper.Items)
        {
            if (Item.Selected)
                values.Add(Item.Value);
        }
        values.ToArray();

        for (int iConta = 0; iConta < values.Count; iConta++)
        {
            if (iConta == values.Count-1)
                idGops += values[iConta];
            else
                idGops += values[iConta] + "-";
        }
        string[] seleccionados = idGops.Split('-');
        seleccionados = seleccionados.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        if (seleccionados.Length != 0)
        {
            lblError.Text = "";
            int[] sesiones = obtieneSesiones();
            if (txtFechaPromesa.Text != "")
            {
                /*if (txtHoraPromesa.Text != "")
                {*/
                    if (txtOperativo.Text != "" && lblIdOperativo.Text != "")
                    {
                        if (txtFechaIniProg.Text != "")
                        {
                            /*if (txtHoraIniProg.Text != "")
                            {*/
                                DateTime fecha;
                                try
                                {
                                    object[] verifica = datosMO.existeAsignadoAorden(sesiones[2], sesiones[3], sesiones[4], lblIdOperativo.Text);
                                    if (Convert.ToBoolean(verifica[0]))
                                    {
                                        if (Convert.ToBoolean(verifica[1]))
                                            lblError.Text = "El operativo " + txtOperativo.Text + " ya se encuentra asignado a la orden";
                                        else
                                        {
                                            fecha = Convert.ToDateTime(txtFechaIniProg.Text); //+ " " + txtHoraIniProg.Text);
                                            decimal monto = 0;
                                            try
                                            {
                                                if (txtMonto.Text == "")
                                                    monto = 0;
                                                else
                                                    monto = Convert.ToDecimal(txtMonto.Text);
                                            }
                                            catch (Exception) { monto = 0; }
                                            //meter fecha promesa a seguimiento orden usando base de arriba
                                            object[] asignado = datosMO.asignaOperario(sesiones[0], sesiones[2], sesiones[3], sesiones[4], lblIdOperativo.Text, fecha, monto, idGops);
                                            if (Convert.ToBoolean(asignado[0]))
                                            {
                                                int noOrden = Convert.ToInt32(Request.QueryString["o"]);
                                                int idTaller = Convert.ToInt32(Request.QueryString["t"]);
                                                int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                                                string fechaPromesa = txtFechaPromesa.Text;
                                                string horaPromesa = txtHoraPromesa.Text;
                                                asignado = datosMO.actualizarTiempoEstimadoEntrega(noOrden, idEmpresa, idTaller, fechaPromesa, horaPromesa);
                                                txtFechaIniProg.Text = txtHoraIniProg.Text = txtOperativo.Text = lblIdOperativo.Text = txtMonto.Text = "";
                                                grdOperativosOrden.DataBind();
                                                Panel2.Visible = Panel3.Visible = false;
                                                Label2.Visible = false;
                                                lnkCancelaVista.Visible = false;
                                                Panel1.Visible = true;
                                                acutalizaFase();
                                            }
                                            else
                                                lblError.Text = "Error al asignar el operario: " + asignado[1].ToString();
                                        }
                                    }
                                    else
                                        lblError.Text = "Error al asignar el operario: " + verifica[1].ToString();
                                }
                                catch (Exception) { lblError.Text = "Debe indicar una hora de inicio programada válida"; }
                            /*}
                            else
                                lblError.Text = "Debe indicar la hora de inicio programada";*/
                        }
                        else
                            lblError.Text = "Debe indicar la fecha de inicio programada";
                    }
                    else
                        lblError.Text = "Debe seleccionar un operario";
                /*}
                else
                    lblError.Text = "Debe indicar una hora de entrega promesa";*/
            }
            else
                lblError.Text = "Debe indicar una fecha de entrega promesa";
        }
        else
            lblError.Text = "Debe Seleccionar al menos un grupo de operacion a trabajar";
    }
    protected void lnkCancelaVista_Click(object sender, EventArgs e)
    {
        Panel1.Visible = true;
        Panel2.Visible = Panel3.Visible = Label2.Visible = lnkCancelaVista.Visible = false;
        lblError.Text = "";
    }
    protected void lnkBuscarOpe_Click(object sender, EventArgs e)
    {
        if(txtBuscaOperario.Text!="")
            SqlDataSourceOperadoresRH.SelectCommand = "select (rtrim(ltrim(e.Nombres))+' '+rtrim(ltrim(e.Apellido_Paterno))+' '+isnull(rtrim(ltrim(e.Apellido_Materno)),'')) as nombre,e.clv_pichonera,e.IdEmp,(e.clv_pichonera - (select count(*) from Operativos_Orden oo where oo.IdEmp=e.IdEmp and oo.estatus<>'T'))as resta from Empleados e where e.status_empleado!='B' and e.tipo_empleado in ('EX','IN') AND (rtrim(ltrim(e.Nombres)) LIKE '%" + txtBuscaOperario.Text.ToUpper().Trim() + "%' or rtrim(ltrim(e.Apellido_Paterno)) like '%" + txtBuscaOperario.Text.ToUpper().Trim() + "%' or isnull(rtrim(ltrim(e.Apellido_Materno)),'') like '%" + txtBuscaOperario.Text.ToUpper().Trim() + "%') order by resta desc";        
        GridOperarios.DataBind();
        Panel1.Visible = false;
        Panel2.Visible = Panel3.Visible = Label2.Visible = lnkCancelaVista.Visible = true;
    }
    protected void hpkLimpiar_Click(object sender, EventArgs e)
    {
        txtBuscaOperario.Text = "";
        SqlDataSourceOperadoresRH.SelectCommand = "select (rtrim(ltrim(e.Nombres))+' '+rtrim(ltrim(e.Apellido_Paterno))+' '+isnull(rtrim(ltrim(e.Apellido_Materno)),'')) as nombre,e.clv_pichonera,e.IdEmp,(e.clv_pichonera - (select count(*) from Operativos_Orden oo where oo.IdEmp=e.IdEmp and oo.estatus<>'T'))as resta from Empleados e where e.status_empleado!='B' and e.tipo_empleado in ('EX','IN') order by resta desc";
        GridOperarios.DataBind();
        Panel1.Visible = false;
        Panel2.Visible = Panel3.Visible = Label2.Visible = lnkCancelaVista.Visible = true;
    }


    protected void grdOperativosOrden_RowDataBound(object sender, GridViewRowEventArgs e)
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
                //meter lkabel Label 
                //lblGopsIds = e.Row.FindControl("lblGopsIds") as Label; 
                //en edit item para leer los gopsids y seleccionar los checks
                Label lblGopsIds = e.Row.FindControl("lblGopsIds") as Label;
                string[] gops = lblGopsIds.Text.Split('-');
                CheckBoxList chkGopsEdit = e.Row.FindControl("chkGopsEdit") as CheckBoxList;
                foreach (ListItem Item in chkGopsEdit.Items)
                {
                    for (int iContFor = 0; iContFor < chkGopsEdit.Items.Count; iContFor++)
                    {
                        for (int iContForIf = 0; iContForIf < gops.Length; iContForIf++)
                        {
                            if (chkGopsEdit.Items[iContFor].Value == gops[iContForIf])
                            {
                                chkGopsEdit.Items[iContFor].Selected = true;
                            }
                        }
                    }
                }
            }
            if (modo != "Edit")
            {
                string status = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
                LinkButton btnEliminar = e.Row.FindControl("lnkElimina") as LinkButton;
                LinkButton btnEditar = e.Row.FindControl("lnkEdita") as LinkButton;

                Label lblGopsIds = e.Row.FindControl("lblGopsIds") as Label;
                Label lblGopsText = e.Row.FindControl("lblGopsText") as Label;
                string[] gops = lblGopsIds.Text.Split('-');
                string gopsText = "Sin Grupos Asignados";
                try
                {
                    string temFor = "";
                    if (Convert.ToInt32(gops[0]) > 0)
                    {
                        gopsText = "";
                        for (int iCont = 0; iCont < gops.Length; iCont++)
                        {
                            temFor = datosCat.obtieneNombreGrupoOperacion(gops[iCont]);
                            if (iCont == gops.Length - 1)
                                gopsText += temFor;
                            else
                                gopsText += temFor + ",";
                        }
                    }
                    else
                        gopsText = "Sin Grupos Asignados";
                }
                catch (Exception ex)
                {

                }
                lblGopsText.Text = gopsText;
                /*if (status == "A")
                    btnEliminar.Visible = btnEditar.Visible = true;
                else
                    btnEliminar.Visible = btnEditar.Visible = false;*/
            }
        }
    }
    protected void grdOperativosOrden_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            TextBox fecha = grdOperativosOrden.Rows[grdOperativosOrden.EditIndex].FindControl("txtFechaIniProgM") as TextBox;
            TextBox hora = grdOperativosOrden.Rows[grdOperativosOrden.EditIndex].FindControl("txtHoraIniProgM") as TextBox;
            TextBox montoTxt = grdOperativosOrden.Rows[grdOperativosOrden.EditIndex].FindControl("txtMontoM") as TextBox;

            CheckBoxList chkGopsEdit = grdOperativosOrden.Rows[grdOperativosOrden.EditIndex].FindControl("chkGopsEdit") as CheckBoxList;

            string[] argumentos = e.CommandArgument.ToString().Split(new char[] { ';' });
            string id = argumentos[0];
            string empleado = argumentos[1];
            if (fecha.Text != "")
            {
                if (hora.Text != "")
                {
                    try
                    {
                        DateTime fechaIni = Convert.ToDateTime(fecha.Text + " " + hora.Text);
                        int[] sesiones = obtieneSesiones();

                        //terminar par mandar parametros del check al update
                        decimal monto = 0;
                        try
                        {
                            if (montoTxt.Text == "")
                                monto = 0;
                            else
                                monto = Convert.ToDecimal(montoTxt.Text);
                        }
                        catch (Exception) { monto = 0; }
                        string idGops = "";
                        List<string> values = new List<string>();

                        foreach (ListItem Item in chkGopsEdit.Items)
                        {
                            if (Item.Selected)
                                values.Add(Item.Value);
                        }
                        values.ToArray();
                        for (int iConta = 0; iConta < values.Count; iConta++)
                        {
                            if (iConta == values.Count - 1)
                                idGops += values[iConta];
                            else
                                idGops += values[iConta] + "-";
                        }
                        string[] seleccionados = idGops.Split('-');
                        seleccionados = seleccionados.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                        fechas.fecha = fechas.obtieneFechaLocal();
                        fechas.tipoFormato = 4;
                        string fechaRetorno = fechas.obtieneFechaConFormato();
                        fechas.tipoFormato = 6;
                        string horaRetorno = fechas.obtieneFechaConFormato();

                        SqlDataSource1.UpdateCommand = "update operativos_orden set fecha_ini_prog='" + fechaIni.ToString("yyyy-MM-dd") + "',hora_ini_prog='" + fechaIni.ToString("HH:mm:ss") + "',fecha_ult_modifica='" + fechaRetorno + "',hora_ult_modifica='" + horaRetorno + "',id_usuario_ult_modifi=" + sesiones[0].ToString() + ",monto=" + monto.ToString() + ",idgops='" + idGops + "' where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_asignacion=" + id + " and idEmp=" + empleado;
                        SqlDataSource1.Update();
                        grdOperativosOrden.EditIndex = -1;
                        grdOperativosOrden.DataBind();
                        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
                        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
                        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
                        string fechaPromesa = txtFechaPromesa.Text;
                        string horaPromesa = txtHoraPromesa.Text;
                        object[] asignado = datosMO.actualizarTiempoEstimadoEntrega(noOrden, idEmpresa, idTaller, fechaPromesa, horaPromesa);
                        acutalizaFase();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error al actualizar el operario: " + ex.Message;
                    }
                }
                else
                    lblError.Text = "Debe indicar la hora de inicio programada";
            }
            else
                lblError.Text = "Debe indicar la fecha de inicio programada";
        }
        if (e.CommandName == "Delete") {
            string[] argumentos = e.CommandArgument.ToString().Split(new char[] { ';' });
            string id = argumentos[0];
            string empleado = argumentos[1];
            try {
                int[] sesiones = obtieneSesiones();
                SqlDataSource1.DeleteCommand = "delete from operativos_orden where id_empresa=" + sesiones[2].ToString() + " and id_taller=" + sesiones[3].ToString() + " and no_orden=" + sesiones[4].ToString() + " and id_asignacion=" + id + " and idEmp=" + empleado;                                
                grdOperativosOrden.DataBind();
                acutalizaFase();
            }
            catch (Exception ex) { lblError.Text = "Error al eliminar el operativo: " + ex.Message; }
        }
    }

    protected void btnImprime_Click(object sender, EventArgs e)
    {
        OrdenTrabajo imprime = new OrdenTrabajo();
        Recepciones recepciones = new Recepciones();

        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);

        string nomTaller = recepciones.obtieneNombreTaller(Request.QueryString["t"]);
        string usuario = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);


        string archivo = imprime.GenRepOrdTrabajo(empresa, taller, orden, nomTaller, usuario);
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

    protected void lnkNotificar_Click(object sender, EventArgs e)
    {
        Notificaciones notifi = new Notificaciones();
        notifi.Articulo = Request.QueryString["o"];
        notifi.Empresa = Convert.ToInt32(Request.QueryString["e"]);
        notifi.Taller = Convert.ToInt32(Request.QueryString["t"]);
        notifi.Usuario = Request.QueryString["u"];
        notifi.Fecha = fechas.obtieneFechaLocal();
        notifi.Estatus = "P";
        notifi.Clasificacion = 8;
        notifi.Origen = "O";
        notifi.armaNotificacion();
        notifi.agregaNotificacion();
        acutalizaFase();
    }
}
