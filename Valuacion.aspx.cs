using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Telerik.Web.UI;

public partial class Valuacion : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Permisos permisos = new Permisos();

    int piezas = 0;
    decimal totalCompra, totalVenta, totalUtilidad;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cargaDatosPie();
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {                   
                   //grdRefacOrd.Columns[14].Visible = grdRefacOrd.Columns[15].Visible = GridView1.Columns[5].Visible = false;
                    
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
            }
        }
    }

    protected void grdRefacOrd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                piezas = 0;
                totalCompra = totalVenta = totalUtilidad = 0;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
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

                LinkButton lblEstatusRefEdit = e.Row.FindControl("lblEstatusRefEdit") as LinkButton;
                LinkButton btnEditar = e.Row.FindControl("lnkEditarR") as LinkButton;
                LinkButton btnEliminar = e.Row.FindControl("lnkEliminarR") as LinkButton;
                DropDownList ddlEstatusRefEdit = e.Row.FindControl("ddlEstatusRefEdit") as DropDownList;
                Label lblIdEstRef = e.Row.FindControl("lblIdEstRef") as Label;
                Label lblEstatus = e.Row.FindControl("lblEstatus") as Label;
                string estatusRef = DataBinder.Eval(e.Row.DataItem, "refEstatus").ToString();
                string importeCosto = DataBinder.Eval(e.Row.DataItem, "importe_compra").ToString();
                string importeVenta = DataBinder.Eval(e.Row.DataItem, "importe").ToString();
                string utilidad = DataBinder.Eval(e.Row.DataItem, "utilidad").ToString();
                string cantidad = DataBinder.Eval(e.Row.DataItem, "refCantidad").ToString();
                string estatusRefaccion = lblIdEstRef.Text;
                string status = lblEstatus.Text;

                string idCfd = DataBinder.Eval(e.Row.DataItem, "idCfd").ToString();

                int idFactura = 0;
                try { if (idCfd == "") idFactura = 0; else idFactura = Convert.ToInt32(idCfd); } catch (Exception) { idFactura = -1; }
                /*
                if (idFactura != 0)
                    e.Row.CssClass = "alert-info";
                */

                /*
                if (status == "AU" && estatusRefaccion == "3")
                {
                    btnEditar.Visible = btnEliminar.Visible = false;
                }
                else if (estatusRefaccion == "2")
                {
                    if (modo != "Edit")
                        btnEliminar.Visible = false;
                }
                */
                decimal utilidadD = Convert.ToDecimal(utilidad);
                decimal impCosto = Convert.ToDecimal(importeCosto);
                decimal impVenta = Convert.ToDecimal(importeVenta);
                int cantPiezas = Convert.ToInt32(cantidad);
                if (utilidadD < 0)
                    e.Row.Cells[10].CssClass = "alert-danger";
                else if (utilidadD > 0)
                    e.Row.Cells[10].CssClass = "alert-success";
                else
                    e.Row.Cells[10].CssClass = "alert-warning";
                totalCompra = totalCompra + impCosto;
                totalVenta = totalVenta + impVenta;
                totalUtilidad = totalUtilidad + utilidadD;
                piezas = piezas + cantPiezas;
                /*
                if (modo == "Edit")
                {
                    string estatus = estatusRef;
                    if (estatus != "NA" && estatus != "AU")
                        ddlEstatusRefEdit.SelectedValue = estatus;
                }

                if (estatusRef == "FA")
                {
                    btnEditar.Visible = btnEliminar.Visible = false;
                }
                else
                {
                    if (idFactura != 0)
                        btnEditar.Visible = btnEliminar.Visible = false;
                }

                if (modo != "Edit")
                {
                    int[] sesiones = obtieneSesiones();
                    permisos.idUsuario = sesiones[0];
                    /*permisos.obtienePermisos();
                    bool[] permisosUsuario = permisos.permisos;
                    permisos.permisos = permisosUsuario;*/
                permisos.permiso = 72;
                permisos.tienePermisoIndicado();
                /*
                    if (!permisos.tienePermiso)
                        btnEliminar.Visible = false;
                    else
                        btnEliminar.Visible = true;
                 

                    if (idFactura != 0)
                        btnEditar.Visible = btnEliminar.Visible = false;
                 */

            }

        
            else if (e.Row.RowType == DataControlRowType.Footer)//
            {
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
                //lblPorcUtil.Text = ((totalUtilidad / totalVenta) * 100).ToString("00.00") + " %";
                lblTRef.Text = "Refacciones solicitadas: " + piezas.ToString();
                e.Row.CssClass = "alert-info textoBold";
                /*if (totalUtilidad < 0)
                    e.Row.Cells[10].CssClass = e.Row.Cells[11].CssClass = "alert-danger";
                else if (totalUtilidad > 0)
                    e.Row.Cells[10].CssClass = e.Row.Cells[11].CssClass = "alert-success";
                else
                    e.Row.Cells[10].CssClass = e.Row.Cells[11].CssClass = "alert-warning";*/
            }
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    protected void grdRefacOrd_RowCommand(object sender, GridViewCommandEventArgs e)
    {/*
        if (e.CommandName == "Update")
        {
            string id = e.CommandArgument.ToString();
            Label descripcion = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("Label311") as Label;
            Label cantidad = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("Label12") as Label;
            TextBox precio = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("txtPrecioM") as TextBox;
            DropDownList estatus = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("ddlEstatusRefEdit") as DropDownList;
            DropDownList estatussOLICITUD = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("ddlEstatusRefaccion") as DropDownList;
            TextBox porcScS = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("txtPorcSob") as TextBox;
            Label cu = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("Label6") as Label;

            string costoUnitario = cu.Text;
            decimal costo = 0;
            decimal precioVenta = 0;
            decimal sobre = 0;
            try
            {
                costo = convierteAdigitos(costoUnitario);

                if (porcScS.Text == "")
                    sobre = 0;
                else
                    sobre = Convert.ToDecimal(porcScS.Text);

                if (sobre > 100)
                    sobre = 100;


                if (precio.Text == "")
                    precioVenta = 0;
                else
                {
                    if (sobre > 0)
                        precioVenta = costo + (costo * (sobre / 100));
                    else
                        precioVenta = convierteAdigitos(precio.Text);
                }

                string proceso = "ED";
                llenarBitacora("Edito", proceso, descripcion.Text, Convert.ToInt32(cantidad.Text), estatus.SelectedItem.ToString());
                sqlDSRefOrden.UpdateCommand = "UPDATE [Refacciones_Orden] SET refestatus='" + estatus.SelectedValue + "',refEstatusSolicitud=" + estatussOLICITUD.SelectedValue + "  WHERE [refOrd_Id] = " + id + " AND [ref_no_orden] = " + Request.QueryString["o"] + " and [ref_id_empresa]=" + Request.QueryString["e"] + " and [ref_id_taller]=" + Request.QueryString["t"];
                grdRefacOrd.EditIndex = -1;
                grdRefacOrd.DataBind();
                actualizaTotales();
                actualizaFase();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al actualizar la refacción: " + ex.Message;
            }
            finally { actualizaTotales(); actualizaFase(); }
        }
        if (e.CommandName == "Delete")
        {
            try
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                Label lblRefaccion = row.FindControl("Label31") as Label;
                Label lblCantidad = row.FindControl("Label2") as Label;
                Label lblEstatusTex = row.FindControl("Label13") as Label;
                string id = e.CommandArgument.ToString();
                string proceso = "EL";
                llenarBitacora("Eliminó", proceso, lblRefaccion.Text, Convert.ToInt32(lblCantidad.Text), lblEstatusTex.Text);
                sqlDSRefOrden.DeleteCommand = "DELETE FROM [Refacciones_Orden] WHERE [refOrd_Id] = " + id + "  AND [ref_no_orden] = " + Request.QueryString["o"] + " and [ref_id_empresa]=" + Request.QueryString["e"] + " and [ref_id_taller]=" + Request.QueryString["t"];
                grdRefacOrd.DataBind();
                actualizaTotales();
                actualizaFase();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al eliminar la refacción: " + ex.Message;
            }
            finally
            {
                actualizaTotales();
                actualizaFase();
            }
        }
        */
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

    protected void grdRefacOrd_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        actualizaTotales();
        actualizaFase();
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

    protected void lnkImprimir_Click(object sender, EventArgs e)
    {
        ImpresionValuacion imprimeVal = new ImpresionValuacion();
        int noOrden;
        int idEmpresa;
        int idTaller;
        noOrden = Convert.ToInt32(Request.QueryString["o"]);
        idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        idTaller = Convert.ToInt32(Request.QueryString["t"]);

        string archivo = imprimeVal.GenRepValuacion(noOrden, idEmpresa, idTaller);
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

    protected void lnkConsultaNoAuto_Click(object sender, EventArgs e)
    {
        GridView1.DataBind();
        string script = "abreWin()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "noautorizdas", script, true);
    }
    protected void lnkReautoriza_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument.ToString();
            SqlDataSource2.UpdateCommand = "UPDATE [Refacciones_Orden] SET refEstatusSolicitud=1  WHERE [refOrd_Id] = " + id + " AND [ref_no_orden] = " + Request.QueryString["o"] + " and [ref_id_empresa]=" + Request.QueryString["e"] + " and [ref_id_taller]=" + Request.QueryString["t"];
            SqlDataSource2.Update();
            GridView1.DataBind();
        }
        catch (Exception) { }
        finally
        {
            //grdRefacOrd.DataBind();
            actualizaTotales();
            actualizaFase();
        }
    }

    

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            
            var estatus = e.Item.FindControl("lblEstatusProveedorEdita") as Label;
            string est = estatus.Text;
            var boton = e.Item.FindControl("btnCambiaProveedor") as LinkButton;
            
            if (est != "No Aplica")
            {
                boton.Visible = false;
                boton.CssClass = "btn btn-success t14";
               
            }
            else
            {
                boton.CssClass = "btn btn-info t14";
            }
            
              
        }
    }

    protected void lnkCambiarProveedor_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int argumento = Convert.ToInt32(btn.CommandArgument.ToString());
        int[] sesiones = obtieneSesiones();
        int refordid = Convert.ToInt32(argumento);
        Recepciones cambia = new Recepciones();
        object [] resultado = cambia.traeRefOrdId(sesiones[2], sesiones[3], sesiones[4],refordid);
        lblRefaccion.Text = resultado[1].ToString();
        string script = "abreWin2()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
    }

    protected void btnCambiaProveedor_Click(object sender, EventArgs e)
    {
        int[] sesiones = obtieneSesiones();
        Recepciones cambia = new Recepciones();
        int refaccion, proveedor;
        try { refaccion = Convert.ToInt32(lblRefaccion.Text); }
        catch (Exception) { refaccion = 0; }
        try { proveedor = Convert.ToInt32(cmbProveedorEditar.SelectedValue); }
        catch (Exception) { proveedor = 0; }
        object[] resultado = cambia.cambiaProveedor(sesiones[2],sesiones[3],sesiones[4],refaccion,proveedor);

        cmbProveedorEditar.SelectedValue = "0";
        RadGrid1.DataBind();
        string script = "cierraWin2()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "modales", script, true);
    }
}