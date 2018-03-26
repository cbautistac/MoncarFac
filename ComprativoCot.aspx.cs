using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.IO;
using E_Utilities;

public partial class ComprativoCot : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    datosCotizaProv cotizacion = new datosCotizaProv();
    Permisos permisos = new Permisos();
    Fechas fechas = new Fechas();
    int cot = 0, contCot = 0, piezas = 0;
    decimal totalCompra, totalVenta, totalUtilidad;
    int proveedores;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargaDatosPie();
            PanelMascara.Visible = false;
            int[] sesiones = obtieneSesiones();
            object[] proveePen = cotizacion.obtieneProveedoresPendientes(sesiones);
            if (Convert.ToBoolean(proveePen[0]))
            {
                proveedores = Convert.ToInt32(proveePen[1]);
                if (proveedores > 0)
                {
                    bool activo = validaCotizacionesHrs();
                    if (activo)
                    {
                        lblProveedoresPendientes.Visible = true;
                        lblProveedoresPendientes.Text = "Existe(n) " + proveedores.ToString() + " proveedore(s) pendiente(s) de registrar su cotización respectiva";
                        lnkAutTodo.Visible = false;
                    }
                    else
                    {                        
                        Refacciones datosRefacc = new Refacciones();
                        List<Refacciones> refacciones = new List<Refacciones>();
                        datosRefacc._orden = sesiones[4];
                        datosRefacc._empresa = sesiones[2];
                        datosRefacc._taller = sesiones[3];
                        datosRefacc._cotizacion = sesiones[6];
                        refacciones = datosRefacc.obtieneRefacciones();
                        foreach (Refacciones refacc in refacciones)
                        {
                            object[] cotizado = cotizacion.actualizaRefaccion(sesiones, refacc);
                        }
                        procesaRefacciones();
                    }
                }
                else
                    lblProveedoresPendientes.Visible = false;
            }
            if (proveedores == 0)
                procesaRefacciones();

            if (Request.QueryString["a"] == "0")
            {
                lblProveedoresPendientes.Visible = false;
                lnkComparativo.Visible = false;
                lnkComprar.Visible = true;
                pnlComprasEnviadas.Visible = true;
                Label11.Visible = txtPorcGral.Visible = lnkAplicaSobreCosto.Visible = false;
            }
            else
            {
                string estatusCot = cotizacion.obtieneEstatusCotizacion(sesiones);
                if (estatusCot == "CA")
                    lnkComparativo.Visible = false;
                lnkComprar.Visible = false;
                pnlComprasEnviadas.Visible = false;
            }
            lblVisiblePorc.Text = "false";
            grdRefacciones.DataBind();
            if (Convert.ToBoolean(lblVisiblePorc.Text))
            {
                PanelPorcentaje.Visible = false;
                //inactivaControlesGrid();
                pnlNuevoProv.Visible = false;
                lnkAgregarProveedor.Visible = false;
            }
            if (proveedores > 0)
                lnkAutTodo.Visible = false;
            else
                controlAccesos();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    inactivaControlesGrid();
                    lnkRefrescar.Visible = pnlNuevoProv.Visible = false;
                    grdRefacciones.Columns[17].Visible = grdRefacciones.Columns[18].Visible = false;
                    lnkAutTodo.Visible = PanelPorcentaje.Visible = lnkComprar.Visible = lnkAgregarProveedor.Visible = false;
                    pnlComprasEnviadas.Visible = false;
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

    private void inactivaControlesGrid()
    {
        foreach (GridItem r in RadGrid1.Items)
        {

            r.Enabled = false;
            //DataRowView filas = (DataRowView)e.Item.DataItem;
            //DataRow r = filas.Row;

            LinkButton[] botones = {
            r.FindControl("lnkProveedor1") as LinkButton,r.FindControl("lnkProveedor2") as LinkButton,r.FindControl("lnkProveedor3") as LinkButton,
            r.FindControl("lnkProveedor4") as LinkButton,r.FindControl("lnkProveedor5") as LinkButton,r.FindControl("lnkProveedor6") as LinkButton,
            r.FindControl("lnkProveedor7") as LinkButton,r.FindControl("lnkProveedor8") as LinkButton,r.FindControl("lnkProveedor9") as LinkButton,
            r.FindControl("lnkProveedor10") as LinkButton,r.FindControl("lnkProveedor11") as LinkButton,r.FindControl("lnkProveedor12") as LinkButton,
            r.FindControl("lnkProveedor13") as LinkButton,r.FindControl("lnkProveedor14") as LinkButton,r.FindControl("lnkProveedor15") as LinkButton,
            r.FindControl("lnkProveedor16") as LinkButton,r.FindControl("lnkProveedor17") as LinkButton,r.FindControl("lnkProveedor18") as LinkButton,
            r.FindControl("lnkProveedor19") as LinkButton,r.FindControl("lnkProveedor20") as LinkButton};

            Panel[] paneles = {r.FindControl("pp1") as Panel,r.FindControl("pp2") as Panel,r.FindControl("pp3") as Panel,r.FindControl("pp4") as Panel,
            r.FindControl("pp5") as Panel,r.FindControl("pp6") as Panel,r.FindControl("pp7") as Panel,r.FindControl("pp8") as Panel,
            r.FindControl("pp9") as Panel,r.FindControl("pp10") as Panel,r.FindControl("pp11") as Panel,r.FindControl("pp12") as Panel,
            r.FindControl("pp13") as Panel,r.FindControl("pp14") as Panel,r.FindControl("pp15") as Panel,r.FindControl("pp16") as Panel,
            r.FindControl("pp17") as Panel,r.FindControl("pp18") as Panel,r.FindControl("pp19") as Panel,r.FindControl("pp20") as Panel};

            for (int i = 0; i < botones.Length; i++)
            {
                botones[i].Enabled = paneles[i].Enabled = false;
            }
        }
    }

    private void procesaRefacciones()
    {      
        
        int[] sesiones = obtieneSesiones();
        object[] obtieneRefacciones = cotizacion.obtieneRefacciones(sesiones);
        if (Convert.ToBoolean(obtieneRefacciones[0]))
        {
            DataSet refacciones = (DataSet)obtieneRefacciones[1];
            if (refacciones.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in refacciones.Tables[0].Rows)
                {
                    object[] actualizado = cotizacion.actualizaRefacciones(Convert.ToInt32(row[0].ToString()), sesiones);
                }
            }
        }
    }

    private bool validaCotizacionesHrs()
    {
        bool activo = true;
        int[] sesiones = obtieneSesiones();
        datosCotizaProv cotizacion = new datosCotizaProv();
        object[] horas = cotizacion.obtieneHrsMaxTaller(sesiones[2], sesiones[3]);
        int horasT = 0;
        if (Convert.ToBoolean(horas[0]))
        {
            horasT = Convert.ToInt32(horas[1]);
            object[] checaCotizacion = cotizacion.verificaCotizacion(sesiones, horasT);
            if (Convert.ToBoolean(checaCotizacion[0]))
                activo = Convert.ToBoolean(checaCotizacion[1]);
            else
                activo = true;
        }
        else
            activo = true;
        return activo;
    }

    private void controlAccesos()
    {
        int[] sesiones = obtieneSesiones();
        permisos.idUsuario = sesiones[0];
        permisos.obtienePermisos();
        bool[] permisosUsuario = permisos.permisos;
        permisos.permisos = permisosUsuario;
        permisos.permiso = 76;
        permisos.tienePermisoIndicado();
        lnkAutTodo.Visible = false;
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
                lblTit.Text = "Cotización con Folio: " + Request.QueryString["fc"];
            }
        }
    }

    protected void lnkCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            DataView vista = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            DataTable dt = vista.ToTable();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            int cont = 0;
            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                if (fila[11].ToString() == "AU")
                    cont++;
            }
            if (ds.Tables[0].Rows.Count != 0)
            {
                if (cont == ds.Tables[0].Rows.Count)
                {
                    SqlDataSource1.UpdateCommand = "UPDATE [cotizacion_encabezado] SET estatus = 'AU' WHERE [no_orden] = " + Request.QueryString["o"] + " and [id_empresa]=" + Request.QueryString["e"] + " and [id_taller]=" + Request.QueryString["t"] + " and id_cotizacion=" + Request.QueryString["c"] + " AND ESTATUS<>'CA'";
                    SqlDataSource1.Update();
                    actualizaFase();
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
        }
        finally
        {
            Response.Redirect("Cotizacion.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&a=" + Request.QueryString["a"]);
        }
    }

    /*private void cargaGridProveedores(int[] sesiones)
    {
        object[] info = cotizacion.obtieneCotizacionesProveedores(sesiones);
        if (Convert.ToBoolean(info[0]))
        {
            DataSet datos = (DataSet)info[1];
            grdComparativo.DataSource = datos;
        }
        else
        {
            grdComparativo.DataSource = null;
        }
        grdComparativo.DataBind();
    }*/
    protected void lnkProveedor_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] argumentos = btn.CommandArgument.ToString().Split(new char[] { ';' });
        int[] sesines = obtieneSesiones();
        object[] actualizado = cotizacion.actualizaRefaccionCotizada(sesines, argumentos);
        if (Convert.ToBoolean(actualizado[0]))
        {
            if (Convert.ToBoolean(actualizado[1]))
            {
                grdRefacciones.DataBind();
                //PanelMascara.Visible = false;
                string script1 = "alert('La refacción se a actualizado al proveedor seleccionado')";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "informacion", script1, true);
                /*string script1 = "cierraWinComp()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);*/
                actualizaFase();
            }
            else
                lblError.Text = "Error al actualizar la información de la refacción: " + Convert.ToString(actualizado[1]);
        }
        else {
            lblError.Text = "Error al actualizar la información de la refacción: " + Convert.ToString(actualizado[1]);
        }
    }
    protected void grdRefacciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int[] sesiones = obtieneSesiones();
            if (e.Row.RowType == DataControlRowType.Header)
            {
                cot = contCot = piezas = 0;
                totalCompra = totalVenta = totalUtilidad = 0;
                object[] proveePen = cotizacion.obtieneProveedoresPendientes(sesiones);
                if (Convert.ToBoolean(proveePen[0]))
                {
                    proveedores = Convert.ToInt32(proveePen[1]);
                    if (proveedores > 0)
                    {
                        lblProveedoresPendientes.Visible = true;
                        lblProveedoresPendientes.Text = "Existe(n) " + proveedores.ToString() + " proveedore(s) pendiente(s) de registrar su cotización respectiva";
                        lnkAutTodo.Visible = false;
                    }
                    else
                    {
                        lblProveedoresPendientes.Visible = false;
                        controlAccesos();
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnSelecciona = e.Row.FindControl("lnkSeleccionar") as LinkButton;
                LinkButton btnEdita = e.Row.FindControl("lnkEditar") as LinkButton;
                LinkButton btnAutoriza = e.Row.FindControl("lnkAutorizar") as LinkButton;
                string estatus = DataBinder.Eval(e.Row.DataItem, "refEstatus").ToString();
                string estatusRef = DataBinder.Eval(e.Row.DataItem, "estatusRef").ToString();
                string costo = DataBinder.Eval(e.Row.DataItem, "refCosto").ToString();
                string importeCosto = DataBinder.Eval(e.Row.DataItem, "importeCosto").ToString();
                string importeVenta = DataBinder.Eval(e.Row.DataItem, "importeVenta").ToString();
                string utilidad = DataBinder.Eval(e.Row.DataItem, "utilidad").ToString();
                string cantidad = DataBinder.Eval(e.Row.DataItem, "refCantidad").ToString();                
                decimal costoD = Convert.ToDecimal(costo);
                decimal utilidadD = Convert.ToDecimal(utilidad);
                decimal impCosto = Convert.ToDecimal(importeCosto);
                decimal impVenta = Convert.ToDecimal(importeVenta);
                int cantPiezas = Convert.ToInt32(cantidad);

                bool visiblePorc = Convert.ToBoolean(lblVisiblePorc.Text);
                //checar q en au oculte el porcentaje 

                if (Request.QueryString["a"].ToString() == "1")
                {
                    if (costoD == 0)
                    {
                        btnAutoriza.Visible = false;
                        if (estatusRef == "CAN")
                        {
                            btnEdita.Visible = btnAutoriza.Visible = false;
                        }
                    }
                    else if (estatus == "AU" || estatus == "CA")
                    {
                        lblVisiblePorc.Text = "true";
                        btnEdita.Visible = btnAutoriza.Visible = false;
                        if (estatus == "AU")
                            cot++;
                    }
                    else if (estatusRef == "CAN")
                    {
                        btnEdita.Visible = btnAutoriza.Visible = false;
                    }
                    else
                    {
                        if (costoD != 0)
                            contCot++;
                    }

                    if (estatusRef == "CAN")
                    {
                        btnEdita.Visible = btnAutoriza.Visible = false;
                    }

                    if (proveedores > 0)
                        btnAutoriza.Visible = false;
                    else
                    {
                        permisos.idUsuario = sesiones[0];
                        permisos.obtienePermisos();
                        bool[] permisosUsuario = permisos.permisos;
                        permisos.permisos = permisosUsuario;
                        permisos.permiso = 76;
                        permisos.tienePermisoIndicado();
                        if (!permisos.tienePermiso)
                            btnAutoriza.Visible = false;
                        else
                        {
                            if (estatus == "AU" || estatus == "CA")
                                btnAutoriza.Visible = false;
                            else
                                btnAutoriza.Visible = true;
                        }
                    }
                    if (impVenta == 0)
                        btnAutoriza.Visible = false;
                }
                else {
                    btnEdita.Visible = btnAutoriza.Visible = false;
                }
                if (Request.QueryString["a"] == "0")
                {
                    Label11.Visible = txtPorcGral.Visible = lnkAplicaSobreCosto.Visible = false;
                    grdRefacciones.Columns[16].Visible = false;
                    grdRefacciones.Columns[18].Visible = false;
                    grdRefacciones.Columns[9].Visible = false;
                    grdRefacciones.Columns[10].Visible = false;
                    grdRefacciones.Columns[11].Visible = false;
                    grdRefacciones.Columns[12].Visible = false;
                }
                else
                {
                    grdRefacciones.Columns[12].Visible = true;
                    if (utilidadD < 0)
                        e.Row.Cells[12].CssClass = "alert-danger";
                    else if (utilidadD > 0)
                        e.Row.Cells[12].CssClass = "alert-success";
                    else
                        e.Row.Cells[12].CssClass = "alert-warning";
                }
                totalCompra = totalCompra + impCosto;
                totalVenta = totalVenta + impVenta;
                totalUtilidad = totalUtilidad + utilidadD;
                piezas = piezas + cantPiezas;
                string cotCancel = cotizacion.obtieneEstatusCotizacion(sesiones);
                if (cotCancel == "CA")
                {
                    LinkButton lnkAutorizar = e.Row.FindControl("lnkAutorizar") as LinkButton;
                    lnkAutorizar.Visible = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (Request.QueryString["a"] == "1")
                {
                    lnkComprar.Visible = false;
                    pnlComprasEnviadas.Visible = false;
                    if (contCot > 0)
                    {
                        PanelPorcentaje.Visible = true;
                        RadGrid1.Enabled = true;
                        pnlNuevoProv.Visible = true;
                        lnkAgregarProveedor.Visible = false;
                        controlAccesos();
                    }
                    else
                    {
                        lnkAutTodo.Visible = false;
                        PanelPorcentaje.Visible = false;
                        RadGrid1.Enabled = true;
                        pnlNuevoProv.Visible = false;
                        lnkAgregarProveedor.Visible = false;
                    }
                }
                else {
                    if (cot > 0)
                    {
                        lnkComprar.Visible = true;
                        pnlComprasEnviadas.Visible = true;
                    }
                    lnkAutTodo.Visible = false;
                }
                Label lblTcompra = e.Row.FindControl("lblTotalCompra") as Label;
                Label lblTVenta = e.Row.FindControl("lblTotalVenta") as Label;
                Label lblTUtilidad = e.Row.FindControl("lblTotalutilidad") as Label;
                Label lblTRef = e.Row.FindControl("lblTotRef") as Label;
                Label lblPorcUtil = e.Row.FindControl("lblPorcUtilidad") as Label;
                lblTcompra.Text = totalCompra.ToString("C2");
                lblTVenta.Text = totalVenta.ToString("C2");
                lblTUtilidad.Text = totalUtilidad.ToString("C2");
                if (totalVenta == 0)
                    totalVenta = totalCompra;
                lblPorcUtil.Text = ((totalUtilidad / totalCompra) * 100).ToString("00.00") + " %";
                lblTRef.Text = "Refacciones solicitadas: " + piezas.ToString();
                e.Row.CssClass = "alert-info textoBold";
                if (Request.QueryString["a"] == "0")
                {
                    lblPorcUtil.Visible = false;
                }
                else
                {
                    lblPorcUtil.Visible = true;
                    if (totalUtilidad < 0)
                        e.Row.Cells[12].CssClass = e.Row.Cells[14].CssClass = "alert-danger";
                    else if (totalUtilidad > 0)
                        e.Row.Cells[12].CssClass = e.Row.Cells[14].CssClass = "alert-success";
                    else
                        e.Row.Cells[12].CssClass = e.Row.Cells[14].CssClass = "alert-warning";
                }
                actualizaTotales();
            }
        }
        catch (Exception) { }
    }


    protected void grdRefacciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            string refefaccion = ((Label)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("lblRef")).Text;
            string cant = ((Label)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("Label1")).Text;
            string desc = ((Label)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("Label2")).Text;
            string cu = ((Label)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("Label7")).Text;
            string pd = ((Label)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("Label8")).Text;
            string estatus = ((Label)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("Label6")).Text;
            string montoS = ((TextBox)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("txtPrecioMod")).Text;
            string porcScS = ((TextBox)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("txtPorcSob")).Text;
            string estatusRef = ((DropDownList)grdRefacciones.Rows[grdRefacciones.EditIndex].FindControl("ddlEstatus")).SelectedValue;
            decimal monto = 0;
            decimal porcSc = 0;
            decimal costoUni = 0;
            decimal porcDesc = 0;


            int idREF = 0;
            try
            {
                if (porcScS == "")
                    porcSc = 0;

                monto = Convert.ToDecimal(montoS);
                idREF = Convert.ToInt32(refefaccion);
                if (porcScS != "0")
                    porcSc = Convert.ToDecimal(porcScS);
                else
                    porcSc = 0;

                if (porcSc > 100)
                    porcSc = 100;

                costoUni = convierteAdigitos(cu);
                porcDesc = Convert.ToDecimal(pd);

                if (porcDesc != 0)
                    costoUni = costoUni - (costoUni * (porcDesc / 100));

                if (porcSc > 0)
                    monto = costoUni + (costoUni * (porcSc / 100));


                SqlDataSource1.UpdateCommand = "UPDATE [Refacciones_Orden] SET refPrecioVenta = " + monto.ToString() + ",refEstatusSolicitud=" + estatusRef + " , refPorcentSobreCosto=" + porcSc.ToString() + " WHERE [refOrd_Id] = " + idREF.ToString() + " AND [ref_no_orden] = " + Request.QueryString["o"] + " and [ref_id_empresa]=" + Request.QueryString["e"] + " and [ref_id_taller]=" + Request.QueryString["t"];
                grdRefacciones.EditIndex = -1;
                grdRefacciones.DataBind();
                llenarBitacora("Edito", "ED", desc, Convert.ToInt32(cant), estatus);
                actualizaFase();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                monto = 0;
                idREF = 0;
            }
        }
    }

    private decimal convierteAdigitos(string importe)
    {

        decimal total = 0;
        string valor = "";
        for (int j = 0; j < importe.Length; j++)
        {
            if (char.IsDigit(importe[j]))
                valor = valor.Trim() + importe[j];
            else
            {
                if (importe[j] == '.')
                    valor = valor.Trim() + importe[j];
            }
        }
        importe = valor.Trim();
        if (importe == "" || importe == "" || importe == "0")
            total = 0;
        else
        {
            try
            {
                total = Convert.ToDecimal(importe);
            }
            catch (Exception) { total = 0; }
        }

        return total;

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
    protected void lnkAutorizar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] argumentos = btn.CommandArgument.ToString().Split(new char[] { ';' });
        int refaccion = Convert.ToInt32(argumentos[0]);
        try
        {
            SqlDataSource1.UpdateCommand = "UPDATE [Refacciones_Orden] SET refEstatus = 'AU' WHERE [refOrd_Id] = " + refaccion.ToString() + " AND [ref_no_orden] = " + Request.QueryString["o"] + " and [ref_id_empresa]=" + Request.QueryString["e"] + " and [ref_id_taller]=" + Request.QueryString["t"];
            SqlDataSource1.Update();
            grdRefacciones.SelectedIndex = -1;
            grdRefacciones.DataBind();
            lnkComprar.Visible = true;
            pnlComprasEnviadas.Visible = true;
            actualizaTotales();
            llenarBitacora("Autoriza", "AU", argumentos[1], Convert.ToInt32(argumentos[2]), argumentos[3]);
            actualizaFase();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
        }
    }

    protected void lnkAutTodo_Click(object sender, EventArgs e)
    {
        /* modificacion AGD 27jul2017-1940pm
        try
        {
            DataView vista = (DataView)SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            DataTable dt = vista.ToTable();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                if (fila[11].ToString() != "AU")
                {
                    object[] autorizado = cotizacion.autorizaRefaccion(Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"], Request.QueryString["c"], fila[0].ToString());
                    //SqlDataSource1.UpdateCommand = "UPDATE Refacciones_Orden SET refEstatus = 'AU' WHERE refOrd_Id = " + fila[0].ToString() + " AND ref_no_orden = " + Request.QueryString["o"] + " and ref_id_empresa=" + Request.QueryString["e"] + " and ref_id_taller=" + Request.QueryString["t"] + " and id_cotizacion=" + Request.QueryString["c"];
                    //SqlDataSource1.Update();
                    if(Convert.ToBoolean(autorizado[0]))                        
                        llenarBitacora("Autoriza", "AU", fila[2].ToString(), Convert.ToInt32(fila[1].ToString()), "");
                }
            }
            grdRefacciones.SelectedIndex = -1;
            grdRefacciones.DataBind();
            actualizaTotales();
            actualizaFase();
            PanelPorcentaje.Visible = false;
            RadGrid1.Enabled = true;
            pnlNuevoProv.Visible = false;
            lnkAgregarProveedor.Visible = false;

            if (Request.QueryString["a"] == "0")
            {
                lnkComprar.Visible = true;
                pnlComprasEnviadas.Visible = true;
            }

            grdRefacciones.SelectedIndex = -1;
            grdRefacciones.DataBind();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
            lnkComprar.Visible = false;
            pnlComprasEnviadas.Visible = false;
        }
        finally { actualizaTotales(); }
        */
    }

    private void actualizaTotales()
    {
        decimal totMo = 0, totRef = 0, totalOrden = 0;
        TotalesOrden totales = new TotalesOrden();
        // Total Mano Obra
        totales.Empresa = Convert.ToInt32(Request.QueryString["e"]);
        totales.Taller = Convert.ToInt32(Request.QueryString["t"]);
        totales.Orden = Convert.ToInt32(Request.QueryString["o"]);
        totales.obtieneTotalManoObra();
        totMo = totales.ManoObra;


        // Total Refacciones
        totales.obtieneTotalManoRefacciones();
        totRef = totales.Refacciones;

        totalOrden = totMo + totRef;
        totales.Importe = totalOrden;
        totales.actualizaTotales();
        lblTotOrden.Text = totalOrden.ToString("C2");
    }
    protected void lnkComprar_Click(object sender, EventArgs e)
    {
        //Session["P"] = 1;
        //txtContraseñaLog.Text = txtUsuarioLog.Text = lblErrorLog.Text = "";
        //string script = "abreWinAuto()";
        //ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
        //PanelMascara.Visible = PanelPopUpPermiso.Visible = true;
        lblError.Text = "";
        int[] sesiones = obtieneSesiones();
        OrdenCompra ordenCompra = new OrdenCompra();
        ordenCompra.sesiones = sesiones;
        ordenCompra.acceso = Convert.ToInt32(Request.QueryString["a"]);
        object[] proveedores = ordenCompra.obtieneProveedores();
        Envio_Mail correo = new Envio_Mail();
        Refacciones refacciones = new Refacciones();
        if (Convert.ToBoolean(proveedores[0]))
        {
            DataSet datos = (DataSet)proveedores[1];
            int ordenesGeneradas = 0;
            int proveedoresCompras = 0;
            
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                proveedoresCompras++;
                ordenCompra.proveedor = Convert.ToInt32(fila[0].ToString());
                ordenCompra.autoriza = sesiones[0];
                object[] orden = ordenCompra.generaOrden();

                if (Convert.ToBoolean(orden[0]))
                {
                    int ordenGenerada = Convert.ToInt32(orden[1]);
                    if (ordenGenerada == -2)
                        lblError.Text = "La orden a generar no cuenta con proveedores para su envío, por lo cual no es posible generarla, verfique su información e intente de nuevo";
                    else if (ordenGenerada == -1)
                        lblError.Text = "La orden a generar no cuenta con refacciones con los argumentos válidos, por lo cual no es posible generarla, verfique su información e intente de nuevo";
                    else if (ordenGenerada == -3)
                        lblError.Text = "Ya se ha generado la orden correspondiente a uno o más proveedores";
                    else
                    {
                        ordenCompra.actualizaRefacciones(Convert.ToInt32(fila[0].ToString()), ordenGenerada);
                        refacciones._orden = Convert.ToInt32(Request.QueryString["o"]);
                        refacciones._empresa = Convert.ToInt32(Request.QueryString["e"]);
                        refacciones._taller = Convert.ToInt32(Request.QueryString["t"]);
                        refacciones._proveedor = Convert.ToInt32(fila[0].ToString());
                        string fechaPromesa = refacciones.obtieneFechaMinEntEstimada();
                        string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Orden.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + ordenGenerada.ToString() + "&p=" + fila[0].ToString() + "&s=SOL";

                        DateTime fechaEntrega = fechas.obtieneFechaLocal();
                        string valorFecha = "";
                        try { fechaEntrega = Convert.ToDateTime(fechaPromesa); valorFecha = " Recuerde que debe entregar las refacciones y/o piezas a mas tardar el " + fechaEntrega.ToString("d MMM yyyy") + ", fecha especificada en su cotizaci&oacute;n; "; } catch (Exception) { valorFecha = " Recuerde que debe entregar las refacciones y/o piezas en las fechas definidas en su cotizaci&oacute;n; "; }

                        string mensaje = "<h3>Orden de Compra</h3><p>Estimado proveedor haga clic en el siguiente link para consultar la orden de compra de la o las refacciones que se solicitan. <br/><br/><strong>" + valorFecha + " tambi&eacute;n recuerde que al entregar debe acompa&ntilde;ar con la factura correspondiente.</strong></p><br/>" +
                            "<a href='" + url + "' target='_blank'>Consultar Orden de Compra</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...</p>";
                        string asunto = "Orden de Compra";
                        object[] correoEnviado = correo.obtieneDatosServidor("", fila[2].ToString(), mensaje, "", asunto, null, Convert.ToInt32(Request.QueryString["e"]), "", "");
                        ordenesGeneradas++;
                        CotizacionesEnviadas cotizacionEnviada = new CotizacionesEnviadas();

                        if (Convert.ToBoolean(correoEnviado[0]))
                        {
                            bool enviado = Convert.ToBoolean(correoEnviado[0]);
                            cotizacionEnviada.orden = sesiones[4];
                            cotizacionEnviada.empresa = sesiones[2];
                            cotizacionEnviada.taller = sesiones[3];
                            cotizacionEnviada.cotizacion = ordenGenerada;
                            cotizacionEnviada.proveedor = Convert.ToInt32(fila[0].ToString());
                            cotizacionEnviada.enviado = enviado;
                            cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                            cotizacionEnviada.correo = fila[2].ToString();
                            cotizacionEnviada.usuario = sesiones[0];
                            string motivo = "";
                            if (!enviado)
                                motivo = Convert.ToString(correoEnviado[1]);
                            cotizacionEnviada.motivo = motivo;
                            cotizacionEnviada.insertaEnvioCompra();
                        }
                        else
                        {
                            cotizacionEnviada.orden = sesiones[4];
                            cotizacionEnviada.empresa = sesiones[2];
                            cotizacionEnviada.taller = sesiones[3];
                            cotizacionEnviada.cotizacion = ordenGenerada;
                            cotizacionEnviada.proveedor = Convert.ToInt32(fila[0].ToString());
                            cotizacionEnviada.enviado = false;
                            cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                            cotizacionEnviada.usuario = sesiones[0];
                            cotizacionEnviada.correo = fila[2].ToString();
                            cotizacionEnviada.motivo = Convert.ToString(correoEnviado[1]);
                            cotizacionEnviada.insertaEnvioCompra();
                        }
                    }
                }
            }
            if(lblError.Text=="")
                lblError.Text = "Se ha generado la orden de compra correspondiente para el o los proveedores indicados en esta cotización, asi se han enviado " + ordenesGeneradas.ToString() + " correos electrónicos de " + proveedoresCompras.ToString() + " proveedores a informar";
            else
                lblError.Text = lblError.Text + ". Se ha generado la orden de compra correspondiente para el o los proveedores indicados en esta cotización, asi se han enviado " + ordenesGeneradas.ToString() + " correos electrónicos de " + proveedoresCompras.ToString() + " proveedores a informar";
            //PanelMascara.Visible = PanelPopUpPermiso.Visible = false;
            //string script = "cierraWinAuto()";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
            RadGrid3.DataBind();
            pnlComprasEnviadas.Visible = true;
        }
        else
            lblError.Text = "Error: " + Convert.ToString(proveedores[1]);

        grdRefacciones.SelectedIndex = -1;
        grdRefacciones.DataBind();
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

                if (faseSActual < 5)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 5);
                }
            }
        }
        catch (Exception) { }

    }
    protected void btnCerrarComp_Click(object sender, EventArgs e)
    {
       PanelMascara.Visible = false;
        grdRefacciones.SelectedIndex = -1;
        grdRefacciones.DataBind();
    }
    protected void lnkAplicaSobreCosto_Click(object sender, EventArgs e)
    {
        decimal porcentaje = 0;
        if (txtPorcGral.Text == "")
            porcentaje = 0;
        else {
            try { porcentaje = Convert.ToDecimal(txtPorcGral.Text); }
            catch (Exception) { porcentaje = 0; }
        }
        if (porcentaje != 0)
        {
            int[] sesiones = obtieneSesiones();
            OrdenCompra ordenCompra = new OrdenCompra();
            ordenCompra.sesiones = sesiones;
            int contRefacciones, contActual;
            contActual = contRefacciones = 0;
            object[] refacciones = ordenCompra.obtieneRefaccionesCot(sesiones);
            if (Convert.ToBoolean(refacciones[0]))
            {
                DataSet dato = (DataSet)refacciones[1];
                foreach (DataRow fila in dato.Tables[0].Rows)
                {
                    int id = Convert.ToInt32(fila[0].ToString());
                    int cantidad = Convert.ToInt32(fila[1].ToString());
                    decimal costo = Convert.ToDecimal(fila[6].ToString());
                    decimal importeVentaUnitario = costo + (costo * (porcentaje / 100));
                    contRefacciones++;
                    string status = Convert.ToString(fila[11]);
                    if (status != "AU" && status != "CA")
                    {
                        object[] actualizado = ordenCompra.actualizaPrecioVenta(sesiones, id, porcentaje, importeVentaUnitario);
                        if (Convert.ToBoolean(actualizado[0]))
                            contActual++;
                    }
                }
                grdRefacciones.SelectedIndex = -1;
                grdRefacciones.DataBind();
            }
            lblError.Text = "Se actualizaron los precios de venta de " + contActual.ToString() + " refacciones de " + contRefacciones.ToString() + " para actualizar";
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static List<string> obtieneProveedores(string prefixText)
    {
        List<string> lista = new List<string>();

        SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["Taller"].ToString());
        try
        {
            conexionBD.Open();
            SqlCommand cmd = new SqlCommand("select rtrim(ltrim(upper(razon_social))) from cliprov where tipo='P' and razon_social like '%" + prefixText + "%'", conexionBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            IDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(lector.GetString(0));
            }
            lector.Close();
        }
        catch (Exception) { }
        finally { conexionBD.Close(); }
        return lista;
    }
    protected void lnkAgregarProveedor_Click(object sender, EventArgs e)
    {
        Session["P"] = 2;
        txtContraseñaLog.Text = txtUsuarioLog.Text = lblErrorLog.Text = "";        
        //PanelPopUpPermiso.Visible = true;
        string script1 = "cierraWinComp()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);
        string script = "abreWinAuto()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
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
            /* Generacion de orden de compra  */
            if (Session["P"].ToString() == "1")
            {
                int[] sesiones = obtieneSesiones();
                OrdenCompra ordenCompra = new OrdenCompra();
                ordenCompra.sesiones = sesiones;
                ordenCompra.acceso = Convert.ToInt32(Request.QueryString["a"]);
                object[] proveedores = ordenCompra.obtieneProveedores();
                Envio_Mail correo = new Envio_Mail();
                Refacciones refacciones = new Refacciones();
                if (Convert.ToBoolean(proveedores[0]))
                {
                    DataSet datos = (DataSet)proveedores[1];
                    int ordenesGeneradas = 0;
                    int proveedoresCompras = 0;
                    foreach (DataRow fila in datos.Tables[0].Rows)
                    {
                        proveedoresCompras++;
                        ordenCompra.proveedor = Convert.ToInt32(fila[0].ToString());
                        ordenCompra.autoriza = autoriza.IdUsuario;
                        object[] orden = ordenCompra.generaOrden();

                        if (Convert.ToBoolean(orden[0]))
                        {
                            int ordenGenerada = Convert.ToInt32(orden[1]);
                            if (ordenGenerada == -2)
                                lblError.Text = "La orden a generar no cuenta con proveedores para su envío, por lo cual no es posible generarla, verfique su información e intente de nuevo";
                            else if (ordenGenerada == -1)
                                lblError.Text = "La orden a generar no cuenta con refacciones con los argumentos válidos, por lo cual no es posible generarla, verfique su información e intente de nuevo";
                            else if (ordenGenerada == -3)
                                lblError.Text = "Ya se ha generado la orden correspondiente a uno o más proveedores";
                            else
                            {
                                ordenCompra.actualizaRefacciones(Convert.ToInt32(fila[0].ToString()), ordenGenerada);
                                refacciones._orden = Convert.ToInt32(Request.QueryString["o"]);
                                refacciones._empresa= Convert.ToInt32(Request.QueryString["e"]);
                                refacciones._taller = Convert.ToInt32(Request.QueryString["t"]);
                                refacciones._proveedor= Convert.ToInt32(fila[0].ToString());
                                string fechaPromesa = refacciones.obtieneFechaMinEntEstimada();
                                string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Orden.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + ordenGenerada.ToString() + "&p=" + fila[0].ToString() + "&s=SOL";

                                DateTime fechaEntrega = fechas.obtieneFechaLocal();
                                string valorFecha = "";
                                try { fechaEntrega = Convert.ToDateTime(fechaPromesa); valorFecha = " Recuerde que debe entregar las refacciones y/o piezas a mas tardar el "+ fechaEntrega.ToString("d MMM yyyy") +", fecha especificada en su cotizaci&oacute;n; ";  } catch (Exception) { valorFecha = " Recuerde que debe entregar las refacciones y/o piezas en las fechas definidas en su cotizaci&oacute;n; "; }

                                string mensaje = "<h3>Orden de Compra</h3><p>Estimado proveedor haga clic en el siguiente link para consultar la orden de compra de la o las refacciones que se solicitan. <br/><br/><strong>" + valorFecha + " tambi&eacute;n recuerde que al entregar debe acompa&ntilde;ar con la factura correspondiente.</strong></p><br/>" +
                                    "<a href='" + url + "' target='_blank'>Consultar Orden de Compra</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...<br/><b>LOS MONTOS REFLEJADOS EN LA PRESENTE SUBASTA DEBERAN SER RESPETADOS AL MOMENTO DEL SURTIMIENTO</b></p>";
                                string asunto = "Orden de Compra";
                                correo.obtieneDatosServidor("", fila[2].ToString(), mensaje, "", asunto, null, Convert.ToInt32(Request.QueryString["e"]), "", "");
                                ordenesGeneradas++;
                            }
                        }
                    }
                    lblError.Text = "Se ha generado la orden de compra correspondiente para el o los proveedores indicados en esta cotización, asi se han enviado " + ordenesGeneradas.ToString() + " correos electrónicos de " + proveedoresCompras.ToString() + " proveedores a informar";
                    //PanelMascara.Visible = PanelPopUpPermiso.Visible = false;
                    string script = "cierraWinAuto()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
                }
                else
                    lblError.Text = "Error: " + Convert.ToString(proveedores[1]);

                grdRefacciones.SelectedIndex = -1;
                grdRefacciones.DataBind();
            }
            else if (Session["P"].ToString() == "2")
            {
                /*  Comprarativo de Proveedores  */                
                //PanelPopUpPermiso.Visible = false;
                
                string script = "cierraWinAuto()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
                lblErrorNuevo.Text = "";
                int idCliprov = 0;
                int[] sesiones = obtieneSesiones();
                object[] datoProv = cotizacion.obtieneIdProveedor(txtProveedorAdd.Text);
                if (Convert.ToBoolean(datoProv[0]))
                {
                    try { idCliprov = Convert.ToInt32(datoProv[1]); }
                    catch (Exception) { idCliprov = 0; }
                }
                decimal costo, descuento;
                int dias, cantidad, id, existe = 0;
                if (idCliprov != 0)
                {                   

                    try
                    {
                        costo = Convert.ToDecimal(radCostoUnitario.Value);
                        string[] infoRefacc = ddlRefaccionesNueva.SelectedItem.Text.Split(new char[] { ' ' });
                        string cantidadRef = infoRefacc[0].Substring(1, infoRefacc[0].LastIndexOf(')') - 1);
                        lblCantidad.Text = cantidadRef;
                        lblIdRef.Text = ddlRefaccionesNueva.SelectedValue;
                        cantidad = Convert.ToInt32(lblCantidad.Text);
                        id = Convert.ToInt32(lblIdRef.Text);
                        lblRefaccion.Text = "";
                        int cont = 0;
                        foreach (string dato in infoRefacc)
                        {
                            if (cont > 0)
                                lblRefaccion.Text = lblRefaccion.Text + dato.ToString() + " ";
                            cont++;
                        }
                        lblRefaccion.Text = lblRefaccion.Text.Trim();

                        if (radPorcDesc.Value.ToString() == "")
                            descuento = 0;
                        else
                            descuento = Convert.ToDecimal(radPorcDesc.Value);
                        if (chkExistencia.Checked)
                            existe = 1;
                        else
                            existe = 0;
                        if (radDias.Value.ToString() == "")
                            dias = -1;
                        else
                            dias = Convert.ToInt32(radDias.Value);

                        if (costo == 0)
                            lblErrorNuevo.Text = "Debe indicar un costo unitario";
                        else
                        {
                            if (dias == -1)
                                lblErrorNuevo.Text = "Debe indicar los dias de entrega";
                            else
                            {
                                object[] existeProv = cotizacion.existeProveedorCotizacion(sesiones, idCliprov, id);
                                bool exisProv = true;
                                if (Convert.ToBoolean(existeProv[0]))
                                    exisProv = Convert.ToBoolean(existeProv[1]);
                                else
                                    exisProv = true;
                                if (!exisProv)
                                {
                                    decimal importe = cantidad * (costo - (costo * (descuento / 100)));
                                    decimal importeDescuento = (costo * (descuento / 100)) * cantidad;
                                    object[] agregaCosto = cotizacion.agregaProveedorCotizacion(sesiones, id, lblRefaccion.Text, cantidad, idCliprov, costo, descuento, importeDescuento, importe, existe, dias);
                                    if (Convert.ToBoolean(agregaCosto[0]))
                                    {
                                        //cargaGridProveedores(sesiones);
                                        cargaComparativo(sesiones);
                                        txtProveedorAdd.Text = "";//txtDias.Text = "";
                                        radDias.Value = radPorcDesc.Value = radCostoUnitario.Value = 0;
                                        //txtPorcDesc.Text = txtContoUnitario.Text = Convert.ToDecimal("0.00").ToString();
                                        chkExistencia.Checked = false;
                                    }
                                    else
                                        lblErrorNuevo.Text = "Error: " + Convert.ToString(agregaCosto[1]);
                                }
                                else
                                    lblErrorNuevo.Text = "El proveedor " + txtProveedorAdd.Text + " ya existe en la cotización de la refacción";
                            }
                        }
                    }
                    catch (Exception ex) { lblErrorNuevo.Text = "Error: " + ex.Message; }
                    finally
                    {
                        string script1 = "abreWinComp()";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);
                    }
                }
                else {
                    try
                    {
                        costo = Convert.ToDecimal(radCostoUnitario.Value);
                        string[] infoRefacc = ddlRefaccionesNueva.SelectedItem.Text.Split(new char[] { ' ' });
                        string cantidadRef = infoRefacc[0].Substring(1, infoRefacc[0].LastIndexOf(')') - 1);
                        lblCantidad.Text = cantidadRef;
                        lblIdRef.Text = ddlRefaccionesNueva.SelectedValue;
                        cantidad = Convert.ToInt32(lblCantidad.Text);
                        id = Convert.ToInt32(lblIdRef.Text);
                        lblRefaccion.Text = "";
                        int cont = 0;
                        foreach (string dato in infoRefacc)
                        {
                            if (cont > 0)
                                lblRefaccion.Text = lblRefaccion.Text + dato.ToString() + " ";
                            cont++;
                        }
                        lblRefaccion.Text = lblRefaccion.Text.Trim();

                        if (radPorcDesc.Value.ToString() == "")
                            descuento = 0;
                        else
                            descuento = Convert.ToDecimal(radPorcDesc.Value);
                        if (chkExistencia.Checked)
                            existe = 1;
                        else
                            existe = 0;
                        if (radDias.Value.ToString() == "")
                            dias = -1;
                        else
                            dias = Convert.ToInt32(radDias.Value);

                        if (costo == 0)
                            lblErrorNuevo.Text = "Debe indicar un costo unitario";
                        else
                        {
                            if (dias == -1)
                                lblErrorNuevo.Text = "Debe indicar los dias de entrega";
                            else
                            {
                                object[] existeProv = cotizacion.existeProveedorCotizacion(sesiones, idCliprov, id);
                                bool exisProv = true;
                                if (Convert.ToBoolean(existeProv[0]))
                                    exisProv = Convert.ToBoolean(existeProv[1]);
                                else
                                    exisProv = true;

                                decimal importe = cantidad * (costo - (costo * (descuento / 100)));
                                decimal importeDescuento = (costo * (descuento / 100)) * cantidad;

                                if (!exisProv)
                                {
                                    object[] agregaCosto = cotizacion.agregaProveedorCotizacion(sesiones, id, lblRefaccion.Text, cantidad, idCliprov, costo, descuento, importeDescuento, importe, existe, dias);
                                    if (Convert.ToBoolean(agregaCosto[0]))
                                    {
                                        //cargaGridProveedores(sesiones);
                                        cargaComparativo(sesiones);
                                        txtProveedorAdd.Text = /*txtDias.Text =*/ "";
                                        //txtPorcDesc.Text = txtContoUnitario.Text = Convert.ToDecimal("0.00").ToString();
                                        radDias.Value = radPorcDesc.Value = radCostoUnitario.Value = 0;
                                        chkExistencia.Checked = false;
                                    }
                                    else
                                        lblErrorNuevo.Text = "Error: " + Convert.ToString(agregaCosto[1]);
                                }
                                else
                                {
                                    object[] agregaCosto = cotizacion.actualizaProveedorCotizacion(sesiones, id, lblRefaccion.Text, cantidad, idCliprov, costo, descuento, importeDescuento, importe, existe, dias);
                                    if (Convert.ToBoolean(agregaCosto[0]))
                                    {
                                        //cargaGridProveedores(sesiones);
                                        cargaComparativo(sesiones);
                                        txtProveedorAdd.Text = /*txtDias.Text = */"";
                                        //txtPorcDesc.Text = txtContoUnitario.Text = Convert.ToDecimal("0.00").ToString();
                                        radDias.Value = radPorcDesc.Value = radCostoUnitario.Value = 0;
                                        chkExistencia.Checked = false;
                                    }
                                    else
                                        lblErrorNuevo.Text = "Error: " + Convert.ToString(agregaCosto[1]);
                                }
                            }
                        }
                    }
                    catch (Exception ex) { lblErrorNuevo.Text = "Error: " + ex.Message; }
                    finally
                    {
                        string script1 = "abreWinComp()";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);
                    }
                }

                
            }
        }
        else
            lblErrorLog.Text = "Error: " + autoriza.Error;

        grdRefacciones.DataBind();

    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["P"].ToString() == "1")
            {
                string script = "cierraWinAuto()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
                //PanelMascara.Visible = PanelPopUpPermiso.Visible = false;
            }
            else if (Session["P"].ToString() == "2")
            {
                string script1 = "abreWinComp()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacionCerrado", script1, true);
                string script = "cierraWinAuto()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
                //PanelPopUpPermiso.Visible = false;
            }
        }
        catch (Exception) {
            string script = "cierraWinAuto()";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "autorizacion", script, true);
        }
    }

    protected void lnkRefrescar_Click(object sender, EventArgs e)
    {
        Response.Redirect("ComprativoCot.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&c=" + Request.QueryString["c"] + "&fc=" + Request.QueryString["fc"] + "&a=" + Request.QueryString["a"]);
    }

    protected void lnkComparativo_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();     
        lblErrorNuevo.Text = "";
        txtProveedorAdd.Text = "";
        //txtPorcDesc.Text = txtContoUnitario.Text = Convert.ToDecimal("0").ToString();
        //txtDias.Text = "";
        radDias.Value = radPorcDesc.Value = radCostoUnitario.Value = 0;
        chkExistencia.Checked = false;
        cargaComparativo(sesiones);

        string script = "abreWinComp()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "comparativo", script, true);
    }

    private void cargaComparativo(int[] sesiones)
    {
        object[] info = cotizacion.generaComparativo(sesiones);
        if (Convert.ToBoolean(info[0]))
        {
            DataSet datos = (DataSet)info[1];
            int proveedores = Convert.ToInt32(info[0]);
            int cont = 0;
            for (int i = 2; i < RadGrid1.Columns.Count-1; i++)
            {
                if (cont < proveedores)
                    RadGrid1.Columns[i].Visible = true;
                else
                    RadGrid1.Columns[i].Visible = false;
                cont++;
            }
            RadGrid1.Columns[0].ItemStyle.CssClass = RadGrid1.Columns[1].ItemStyle.CssClass = "ancho20";
            RadGrid1.DataSource = datos.Tables[0];

            //grdComparativo.DataSource = datos;
        }
        else
        {
            RadGrid1.DataSource = null;
            //grdComparativo.DataSource = null;
        }
        RadGrid1.DataBind();
        //inactivaControlesGrid();
        //grdComparativo.DataBind();        
    }


    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //DataRowView filas = (DataRowView)e.Item.DataItem;
            //DataRow r = filas.Row;
            var estatus = DataBinder.Eval(e.Item.DataItem, "refEstatus").ToString();

            LinkButton btnSelecciona1 = e.Item.FindControl("lnkProveedor1") as LinkButton;
            LinkButton btnSelecciona2 = e.Item.FindControl("lnkProveedor2") as LinkButton;
            LinkButton btnSelecciona3 = e.Item.FindControl("lnkProveedor3") as LinkButton;
            LinkButton btnSelecciona4 = e.Item.FindControl("lnkProveedor4") as LinkButton;
            LinkButton btnSelecciona5 = e.Item.FindControl("lnkProveedor5") as LinkButton;
            LinkButton btnSelecciona6 = e.Item.FindControl("lnkProveedor6") as LinkButton;
            LinkButton btnSelecciona7 = e.Item.FindControl("lnkProveedor7") as LinkButton;
            LinkButton btnSelecciona8 = e.Item.FindControl("lnkProveedor8") as LinkButton;
            LinkButton btnSelecciona9 = e.Item.FindControl("lnkProveedor9") as LinkButton;
            LinkButton btnSelecciona10 = e.Item.FindControl("lnkProveedor10") as LinkButton;
            LinkButton btnSelecciona11 = e.Item.FindControl("lnkProveedor11") as LinkButton;
            LinkButton btnSelecciona12 = e.Item.FindControl("lnkProveedor12") as LinkButton;
            LinkButton btnSelecciona13 = e.Item.FindControl("lnkProveedor13") as LinkButton;
            LinkButton btnSelecciona14 = e.Item.FindControl("lnkProveedor14") as LinkButton;
            LinkButton btnSelecciona15 = e.Item.FindControl("lnkProveedor15") as LinkButton;
            LinkButton btnSelecciona16 = e.Item.FindControl("lnkProveedor16") as LinkButton;
            LinkButton btnSelecciona17 = e.Item.FindControl("lnkProveedor17") as LinkButton;
            LinkButton btnSelecciona18 = e.Item.FindControl("lnkProveedor18") as LinkButton;
            LinkButton btnSelecciona19 = e.Item.FindControl("lnkProveedor19") as LinkButton;
            LinkButton btnSelecciona20 = e.Item.FindControl("lnkProveedor20") as LinkButton;

            Panel pp1 = e.Item.FindControl("pp1") as Panel;
            Panel pp2 = e.Item.FindControl("pp2") as Panel;
            Panel pp3 = e.Item.FindControl("pp3") as Panel;
            Panel pp4 = e.Item.FindControl("pp4") as Panel;
            Panel pp5 = e.Item.FindControl("pp5") as Panel;
            Panel pp6 = e.Item.FindControl("pp6") as Panel;
            Panel pp7 = e.Item.FindControl("pp7") as Panel;
            Panel pp8 = e.Item.FindControl("pp8") as Panel;
            Panel pp9 = e.Item.FindControl("pp9") as Panel;
            Panel pp10 = e.Item.FindControl("pp10") as Panel;
            Panel pp11 = e.Item.FindControl("pp11") as Panel;
            Panel pp12 = e.Item.FindControl("pp12") as Panel;
            Panel pp13 = e.Item.FindControl("pp13") as Panel;
            Panel pp14 = e.Item.FindControl("pp14") as Panel;
            Panel pp15 = e.Item.FindControl("pp15") as Panel;
            Panel pp16 = e.Item.FindControl("pp16") as Panel;
            Panel pp17 = e.Item.FindControl("pp17") as Panel;
            Panel pp18 = e.Item.FindControl("pp18") as Panel;
            Panel pp19 = e.Item.FindControl("pp19") as Panel;
            Panel pp20 = e.Item.FindControl("pp20") as Panel;

            string costo1 = DataBinder.Eval(e.Item.DataItem, "costo_unitario1").ToString();
            string costo2 = DataBinder.Eval(e.Item.DataItem, "costo_unitario2").ToString();
            string costo3 = DataBinder.Eval(e.Item.DataItem, "costo_unitario3").ToString();
            string costo4 = DataBinder.Eval(e.Item.DataItem, "costo_unitario4").ToString();
            string costo5 = DataBinder.Eval(e.Item.DataItem, "costo_unitario5").ToString();
            string costo6 = DataBinder.Eval(e.Item.DataItem, "costo_unitario6").ToString();
            string costo7 = DataBinder.Eval(e.Item.DataItem, "costo_unitario7").ToString();
            string costo8 = DataBinder.Eval(e.Item.DataItem, "costo_unitario8").ToString();
            string costo9 = DataBinder.Eval(e.Item.DataItem, "costo_unitario9").ToString();
            string costo10 = DataBinder.Eval(e.Item.DataItem, "costo_unitario10").ToString();
            string costo11 = DataBinder.Eval(e.Item.DataItem, "costo_unitario11").ToString();
            string costo12 = DataBinder.Eval(e.Item.DataItem, "costo_unitario12").ToString();
            string costo13 = DataBinder.Eval(e.Item.DataItem, "costo_unitario13").ToString();
            string costo14 = DataBinder.Eval(e.Item.DataItem, "costo_unitario14").ToString();
            string costo15 = DataBinder.Eval(e.Item.DataItem, "costo_unitario15").ToString();
            string costo16 = DataBinder.Eval(e.Item.DataItem, "costo_unitario16").ToString();
            string costo17 = DataBinder.Eval(e.Item.DataItem, "costo_unitario17").ToString();
            string costo18 = DataBinder.Eval(e.Item.DataItem, "costo_unitario18").ToString();
            string costo19 = DataBinder.Eval(e.Item.DataItem, "costo_unitario19").ToString();
            string costo20 = DataBinder.Eval(e.Item.DataItem, "costo_unitario20").ToString();

            decimal cost1 = Convert.ToDecimal(costo1);
            decimal cost2 = Convert.ToDecimal(costo2);
            decimal cost3 = Convert.ToDecimal(costo3);
            decimal cost4 = Convert.ToDecimal(costo4);
            decimal cost5 = Convert.ToDecimal(costo5);
            decimal cost6 = Convert.ToDecimal(costo6);
            decimal cost7 = Convert.ToDecimal(costo7);
            decimal cost8 = Convert.ToDecimal(costo8);
            decimal cost9 = Convert.ToDecimal(costo9);
            decimal cost10 = Convert.ToDecimal(costo10);
            decimal cost11 = Convert.ToDecimal(costo11);
            decimal cost12 = Convert.ToDecimal(costo12);
            decimal cost13 = Convert.ToDecimal(costo13);
            decimal cost14 = Convert.ToDecimal(costo14);
            decimal cost15 = Convert.ToDecimal(costo15);
            decimal cost16 = Convert.ToDecimal(costo16);
            decimal cost17 = Convert.ToDecimal(costo17);
            decimal cost18 = Convert.ToDecimal(costo18);
            decimal cost19 = Convert.ToDecimal(costo19);
            decimal cost20 = Convert.ToDecimal(costo20);

            if (cost1 <= 0) btnSelecciona1.Enabled = pp1.Visible = false; else btnSelecciona1.Enabled = pp1.Visible = true;
            if (cost2 <= 0) btnSelecciona2.Enabled = pp2.Visible = false; else btnSelecciona2.Enabled = pp2.Visible = true;
            if (cost3 <= 0) btnSelecciona3.Enabled = pp3.Visible = false; else btnSelecciona3.Enabled = pp3.Visible = true;
            if (cost4 <= 0) btnSelecciona4.Enabled = pp4.Visible = false; else btnSelecciona4.Enabled = pp4.Visible = true;
            if (cost5 <= 0) btnSelecciona5.Enabled = pp5.Visible = false; else btnSelecciona5.Enabled = pp5.Visible = true;
            if (cost6 <= 0) btnSelecciona6.Enabled = pp6.Visible = false; else btnSelecciona6.Enabled = pp6.Visible = true;
            if (cost7 <= 0) btnSelecciona7.Enabled = pp7.Visible = false; else btnSelecciona7.Enabled = pp7.Visible = true;
            if (cost8 <= 0) btnSelecciona8.Enabled = pp8.Visible = false; else btnSelecciona8.Enabled = pp8.Visible = true;
            if (cost9 <= 0) btnSelecciona9.Enabled = pp9.Visible = false; else btnSelecciona9.Enabled = pp9.Visible = true;
            if (cost10 <= 0) btnSelecciona10.Enabled = pp10.Visible = false; else btnSelecciona10.Enabled = pp10.Visible = true;
            if (cost11 <= 0) btnSelecciona11.Enabled = pp11.Visible = false; else btnSelecciona11.Enabled = pp11.Visible = true;
            if (cost12 <= 0) btnSelecciona12.Enabled = pp12.Visible = false; else btnSelecciona12.Enabled = pp12.Visible = true;
            if (cost13 <= 0) btnSelecciona13.Enabled = pp13.Visible = false; else btnSelecciona13.Enabled = pp13.Visible = true;
            if (cost14 <= 0) btnSelecciona14.Enabled = pp14.Visible = false; else btnSelecciona14.Enabled = pp14.Visible = true;
            if (cost15 <= 0) btnSelecciona15.Enabled = pp15.Visible = false; else btnSelecciona15.Enabled = pp15.Visible = true;
            if (cost16 <= 0) btnSelecciona16.Enabled = pp16.Visible = false; else btnSelecciona16.Enabled = pp16.Visible = true;
            if (cost17 <= 0) btnSelecciona17.Enabled = pp17.Visible = false; else btnSelecciona17.Enabled = pp17.Visible = true;
            if (cost18 <= 0) btnSelecciona18.Enabled = pp18.Visible = false; else btnSelecciona18.Enabled = pp18.Visible = true;
            if (cost19 <= 0) btnSelecciona19.Enabled = pp19.Visible = false; else btnSelecciona19.Enabled = pp19.Visible = true;
            if (cost20 <= 0) btnSelecciona20.Enabled = pp20.Visible = false; else btnSelecciona20.Enabled = pp20.Visible = true;


            if (estatus == "AU")
                e.Item.Enabled = false;

            //nuevo Por Alfredo Gonzalez
            //esto quita la seleccion de botones 
            //dentro del comparativo 21jul2017-19:30pm
            btnSelecciona1.Enabled = false;
            btnSelecciona2.Enabled = false;
            btnSelecciona3.Enabled = false;
            btnSelecciona4.Enabled = false;
            btnSelecciona5.Enabled = false;
            btnSelecciona6.Enabled = false;
            btnSelecciona7.Enabled = false;
            btnSelecciona8.Enabled = false;
            btnSelecciona9.Enabled = false;
            btnSelecciona10.Enabled = false;
            btnSelecciona11.Enabled = false;
            btnSelecciona12.Enabled = false;
            btnSelecciona13.Enabled = false;
            btnSelecciona14.Enabled = false;
            btnSelecciona15.Enabled = false;
            btnSelecciona16.Enabled = false;
            btnSelecciona17.Enabled = false;
            btnSelecciona18.Enabled = false;
            btnSelecciona19.Enabled = false;
            btnSelecciona20.Enabled = false;
        }
    }

    protected void lnkImprimeComparativo_Click(object sender, EventArgs e)
    {
        ImpresionComparativo imprime = new ImpresionComparativo();
        Recepciones recepciones = new Recepciones();

        string archivo = imprime.GenComparativo(obtieneSesiones(),0);
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

    protected void lnkReenviar_Click(object sender, EventArgs e)
    {
        try
        {
            Envio_Mail correo = new Envio_Mail();
            Refacciones refacciones = new Refacciones();
            LinkButton btn = (LinkButton)sender;
            int[] sesiones = obtieneSesiones();
            string[] parametros = btn.CommandArgument.ToString().Split(new char[] { ';' });
            int cotizacion = Convert.ToInt32(parametros[0]);
            int proveedor = Convert.ToInt32(parametros[1]);
            string correos = parametros[2];

            refacciones._orden = Convert.ToInt32(Request.QueryString["o"]);
            refacciones._empresa = Convert.ToInt32(Request.QueryString["e"]);
            refacciones._taller = Convert.ToInt32(Request.QueryString["t"]);
            refacciones._proveedor = proveedor;
            string fechaPromesa = refacciones.obtieneFechaMinEntEstimada();
            string url = ConfigurationManager.AppSettings["direccion"].ToString() + "/Orden.aspx?o=" + sesiones[4] + "&e=" + sesiones[2] + "&t=" + sesiones[3] + "&c=" + cotizacion.ToString() + "&p=" + proveedor.ToString() + "&s=SOL";

            DateTime fechaEntrega = new E_Utilities.Fechas().obtieneFechaLocal();
            string valorFecha = "";
            try { fechaEntrega = Convert.ToDateTime(fechaPromesa); valorFecha = " Recuerde que debe entregar las refacciones y/o piezas a mas tardar el " + fechaEntrega.ToString("d MMM yyyy") + ", fecha especificada en su cotizaci&oacute;n; "; } catch (Exception) { valorFecha = " Recuerde que debe entregar las refacciones y/o piezas en las fechas definidas en su cotizaci&oacute;n; "; }

            string mensaje = "<h3>Orden de Compra</h3><p>Estimado proveedor haga clic en el siguiente link para consultar la orden de compra de la o las refacciones que se solicitan. <br/><br/><strong>" + valorFecha + " tambi&eacute;n recuerde que al entregar debe acompa&ntilde;ar con la factura correspondiente.</strong></p><br/>" +
                "<a href='" + url + "' target='_blank'>Consultar Orden de Compra</a><br/><br/><p>Por su compresi&oacute;n y atenci&oacute;n muchas gracias...</p><br/><p><b>LOS MONTOS REFLEJADOS EN LA PRESENTE SUBASTA DEBERAN SER RESPETADOS AL MOMENTO DEL SURTIMIENTO</b></p>";
            string asunto = "Orden de Compra";
            object[] correoEnviado = correo.obtieneDatosServidor("", correos, mensaje, "", asunto, null, Convert.ToInt32(Request.QueryString["e"]), "", "");


            CotizacionesEnviadas cotizacionEnviada = new CotizacionesEnviadas();

            if (Convert.ToBoolean(correoEnviado[0]))
            {
                bool enviado = Convert.ToBoolean(correoEnviado[0]);
                cotizacionEnviada.orden = sesiones[4];
                cotizacionEnviada.empresa = sesiones[2];
                cotizacionEnviada.taller = sesiones[3];
                cotizacionEnviada.cotizacion = cotizacion;
                cotizacionEnviada.proveedor = proveedor;
                cotizacionEnviada.enviado = enviado;
                cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                cotizacionEnviada.correo = correos;
                cotizacionEnviada.usuario = sesiones[0];
                string motivo = "";
                if (!enviado)
                    motivo = Convert.ToString(correoEnviado[1]);
                cotizacionEnviada.motivo = motivo;
                cotizacionEnviada.actualizaEnvio();
            }
            else
            {
                cotizacionEnviada.orden = sesiones[4];
                cotizacionEnviada.empresa = sesiones[2];
                cotizacionEnviada.taller = sesiones[3];
                cotizacionEnviada.cotizacion = cotizacion;
                cotizacionEnviada.proveedor = proveedor;
                cotizacionEnviada.enviado = false;
                cotizacionEnviada.fecha = new E_Utilities.Fechas().obtieneFechaLocal();
                cotizacionEnviada.usuario = sesiones[0];
                cotizacionEnviada.correo = correos;
                cotizacionEnviada.motivo = Convert.ToString(correoEnviado[1]);
                cotizacionEnviada.actualizaEnvio();
            }            
            RadGrid3.DataBind();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error de reenvio. Detalle: " + ex.Message;
        }
    }
}