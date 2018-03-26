using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using E_Utilities;
using System.Globalization;
using System.Configuration;
using Telerik.Web.UI;

public partial class LevantamientoOrden : System.Web.UI.Page
{
    Recepciones recepciones = new Recepciones();
    Fechas fechas = new Fechas();
    Cadenas cadenas = new Cadenas();
    Permisos permisos = new Permisos();
    int terminados;
    decimal totMo, totRef;

    protected void Page_Load(object sender, EventArgs e)
    {
        obtieneSesiones();
        if (!IsPostBack)
        {
            cargaDatosPie();
            controlAccesos();
            txtRefaccion.Text = "";
            txtMontoNew.Value = 0;
            txtMontoNew.Text = "0";
            txtRefCant.Value = 1;
            txtPrecio.Text = "0";
            txtPrecio.Value = 0;
            lblErrorNew.Text = "La cancelación de la refacción solo es posible desde Valuación";
            lnkEditar.Visible = false;
            string estatus = obtieneEstatus();
            if (estatus != "")
            {
                if (estatus == "R" || estatus == "T" || estatus == "F" || estatus == "S" || estatus == "C")
                {
                    btnAceptarNew.Visible = false;
                    chkRefaccion.Visible = false;
                    btnAddRefac.Visible = false;
                    GridView2.Columns[10].Visible= GridView2.Columns[11].Visible = false;
                   // grdRefacOrd.Columns[7].Visible = grdRefacOrd.Columns[8].Visible = false;
                    lnkCotizar.Visible = lnkEditar.Visible = lnkTerminado.Visible = false;
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

    protected void btnAceptarNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtRefaccion.Text == "")
                txtRefaccion.Text = ddlOpNew.SelectedItem.Text;
            if (txtMontoNew.Text != "")
            {
                decimal monto = 0;
                try
                {
                    monto = Convert.ToDecimal(txtMontoNew.Text);
                    //if (monto != 0)
                    //{
                        SqlDataSourceDanos.Insert();
                        GridView2.DataBind();
                        if (ddlOpNew.SelectedItem.Text.ToLower() == txtRefaccion.Text.ToLower())
                            txtRefaccion.Text = "";
                        acutalizaFase();
                        txtMontoNew.Value = 0;
                        txtMontoNew.Text = "0";
                        lblErrorNew.Text = "";
                        chkRefaccion.Checked = false;
                        RadTabStrip1.SelectedIndex = 0;
                        RadMultiPage1.SelectedIndex = 0;
                    //}
                    //else
                        //lblErrorNew.Text = "El monto no puede ser cero";
                }
                catch (Exception ex) { lblErrorNew.Text = "Error: " + ex.Message; }
            }
            else
                lblErrorNew.Text = "Debe indicar el monto";            
        }
        catch (Exception x)
        {
            lblErrorNew.Text = "Se produjo ún error inesperado: " + x.Message.ToString();
        }
        finally
        {
            actualizaTotales();
        }
    }

    private void llenarBitacora(string proceso, string estatusProceso, string refaccion, int piezas, string estatusRefText)
    {
        bool inserta = false;
        int noOrden = Convert.ToInt32(Request.QueryString["o"]);
        int idEmpresa = Convert.ToInt32(Request.QueryString["e"]);
        int idTaller = Convert.ToInt32(Request.QueryString["t"]);
        int idUsuario = Convert.ToInt32(Request.QueryString["u"]);
        string usuario = recepciones.obtieneNombreUsuario(idUsuario.ToString());
        string observaciones = "El usuario " + usuario + " " + proceso + " la refacción: " + refaccion + ", " + piezas.ToString() + " pieza(s) para realizar " + estatusRefText + "Compra";
        inserta = recepciones.llenaBitacoraRef(noOrden, idEmpresa, idTaller, idUsuario, observaciones, estatusProceso);
    }

    private void acutalizaFase()
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

                if (faseSActual < 2)
                {
                    recepciones.actualizaFaseOrden(orden, taller, empresa, 2);
                }
            }
        }
        catch (Exception) { }

    }

    protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        CatClientes datos = new CatClientes();
        if (e.CommandName == "Delete")
        {
            try
            {
                string consecutivo = e.CommandArgument.ToString();
                SqlDataSourceDanos.DeleteCommand = "delete from mano_obra where id_empresa=" + Request.QueryString["e"] + " and id_taller=" + Request.QueryString["t"] + " and no_orden=" + Request.QueryString["o"] + " and consecutivo=" + consecutivo;
                GridView2.DataBind();
                actualizaTotales();
            }
            catch (Exception x)
            {
                lblErrorNew.Text = "Se produjo ún error inesperado: " + x.Message.ToString();
            }
            finally { actualizaTotales(); acutalizaFase(); }
        }
        else if (e.CommandName == "Update")
        {
            string idGOS = "";
            string idOPS = "";
            string idRefS = "";
            string montoS = "";
            idGOS = ((DropDownList)GridView2.Rows[GridView2.EditIndex].FindControl("ddlGoEditDanos")).SelectedValue;
            idOPS = ((DropDownList)GridView2.Rows[GridView2.EditIndex].FindControl("ddlOpEditDanos")).SelectedValue;
            idRefS = ((TextBox)GridView2.Rows[GridView2.EditIndex].FindControl("txtRefaccionMod")).Text;
            montoS = ((RadNumericTextBox)GridView2.Rows[GridView2.EditIndex].FindControl("txtMontoEdit")).Text;
            decimal monto = 0;
            if (idRefS != "")
            {
                if (montoS != "")
                {
                    try
                    {
                        monto = Convert.ToDecimal(montoS);
                        //if (monto != 0)
                        //{
                            try
                            {
                                string consecutivo = e.CommandArgument.ToString();
                                int idGO = Convert.ToInt32(idGOS);
                                int idOP = Convert.ToInt32(idOPS);
                                decimal montoA = Convert.ToDecimal(montoS);
                                SqlDataSourceDanos.UpdateCommand = "update mano_obra set id_grupo_op=@id_grupo_op,id_operacion=@id_operacion,id_refaccion=lower(@id_refaccion),monto_mo=@monto where id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and consecutivo=@consecutivo";
                                SqlDataSourceDanos.UpdateParameters.Add("id_grupo_op", idGO.ToString());
                                SqlDataSourceDanos.UpdateParameters.Add("id_operacion", idOP.ToString());
                                SqlDataSourceDanos.UpdateParameters.Add("id_refaccion", idRefS.ToString());
                                SqlDataSourceDanos.UpdateParameters.Add("id_empresa", Request.QueryString["e"]);
                                SqlDataSourceDanos.UpdateParameters.Add("id_taller", Request.QueryString["t"]);
                                SqlDataSourceDanos.UpdateParameters.Add("no_orden", Request.QueryString["o"]);
                                SqlDataSourceDanos.UpdateParameters.Add("monto", monto.ToString());
                                SqlDataSourceDanos.UpdateParameters.Add("consecutivo", consecutivo.ToString());
                                try
                                {
                                    SqlDataSourceDanos.Update();
                                    GridView2.EditIndex = -1;
                                    GridView2.DataBind();
                                }
                                catch (Exception ex)
                                {
                                    lblErrorNew.Text = "Error al actualizar la mano de obra: " + ex.Message;
                                }
                            }
                            catch (Exception x)
                            {
                                lblErrorNew.Text = "Se produjo ún error inesperado: " + x.Message.ToString();
                            }
                        //}
                        //else
                        //{
                          //  lblErrorNew.Text = "El monto no puede ser cero";
                        //}
                    }
                    catch (Exception ex) { lblErrorNew.Text = "Error: " + ex.Message; monto = 0; }
                    finally { actualizaTotales(); acutalizaFase(); }
                }
                else
                {
                    lblErrorNew.Text = "Debe indicar el monto";
                    monto = 0;
                }
            }
            else
                lblErrorNew.Text = "Debe indicar la descripción";
        }
    }

    protected void btnAddRefac_Click(object sender, EventArgs e)
    {
        decimal precioVenta = 0;
        try
        {
            if (txtPrecio.Text == "")
                precioVenta = 0;
            else
                precioVenta = Convert.ToDecimal(txtPrecio.Text);
            string estatusRef = ddlEstatusRefaccion.SelectedValue.ToString();

            Catalogos cat = new Catalogos();
            object[] topes = cat.obtieneTopes(Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(Request.QueryString["t"]));
            decimal topeEconomico = 0;
            int topeRefacciones = 0;
            if (Convert.ToBoolean(topes[0]))
            {
                DataSet info = (DataSet)topes[1];
                foreach (DataRow fila in info.Tables[0].Rows)
                {
                    topeEconomico = Convert.ToDecimal(fila[0].ToString());
                    topeRefacciones = Convert.ToInt32(fila[1].ToString());
                }
            }

            if (topeEconomico != 0 && topeRefacciones != 0)
            {
                object[] totales = recepciones.obtieneTotalesPresupuesto(Convert.ToInt32(Request.QueryString["e"]), Convert.ToInt32(Request.QueryString["t"]), Convert.ToInt32(Request.QueryString["o"]));
                decimal importes = 0;
                int refaccionesIngresadas = 0;
                if (Convert.ToBoolean(totales[0]))
                {
                    DataSet info = (DataSet)totales[1];
                    foreach (DataRow fila in info.Tables[0].Rows)
                    {
                        refaccionesIngresadas = Convert.ToInt32(fila[0].ToString());
                        importes = Convert.ToDecimal(fila[1].ToString());
                    }
                }

                if (refaccionesIngresadas + 1 > topeRefacciones || importes + (Convert.ToInt32(txtRefCant.Value) * precioVenta) > topeEconomico)
                {
                    Session["autoriza"] = 1;
                    txtContraseñaLog.Text = txtUsuarioLog.Text = lblErrorLog.Text = "";
                    PanelMask.Visible = PanelPopUpPermiso.Visible = true;
                }
                else {

                    string cadena = txtRefDesc.Text.ToLower();
                    string letra = cadena.Substring(0, 1).ToUpper();
                    cadena = letra + cadena.Substring(1, cadena.Length - 1);
                    string proceso = "AL";
                    llenarBitacora("Agrego", proceso, txtRefDesc.Text, Convert.ToInt32(txtRefCant.Value), ddlEstatusRefaccion.SelectedItem.ToString());
                    sqlDSRefOrden.InsertCommand = "INSERT INTO [Refacciones_Orden] ([refDescripcion], [refCantidad], [refObservaciones], refProveedor, refCosto, [ref_no_orden], ref_id_empresa, ref_id_taller, refPorcentSobreCosto, refEstatus, refEstatusSolicitud, refPrecioVenta,precio_venta_ini,precio_venta_proove,estatus_presupuesto) VALUES ('" + cadena + "', " + txtRefCant.Text + ", '', 0, 0, " + Request.QueryString["o"] + ", " + Request.QueryString["e"] + ", " + Request.QueryString["t"] + ", 0, '" + estatusRef + "',1," + precioVenta.ToString() + "," + precioVenta.ToString() + "," + precioVenta.ToString() + ",'P')";
                    sqlDSRefOrden.Insert();
                    grdRefacOrd.DataBind();
                    acutalizaFase();
                    txtRefDesc.Text = "";
                    txtRefCant.Value = 1;
                    txtPrecio.Value = 0;
                    txtPrecio.Text = "0";
                    RadTabStrip1.SelectedIndex = 1;
                    RadMultiPage1.SelectedIndex = 1;
                }
            }
            else
            {
                string cadena = txtRefDesc.Text.ToLower();
                string letra = cadena.Substring(0, 1).ToUpper();
                cadena = letra + cadena.Substring(1, cadena.Length - 1);
                string proceso = "AL";
                llenarBitacora("Agrego", proceso, txtRefDesc.Text, Convert.ToInt32(txtRefCant.Value), ddlEstatusRefaccion.SelectedItem.ToString());
                sqlDSRefOrden.InsertCommand = "INSERT INTO [Refacciones_Orden] ([refDescripcion], [refCantidad], [refObservaciones], refProveedor, refCosto, [ref_no_orden], ref_id_empresa, ref_id_taller, refPorcentSobreCosto, refEstatus, refEstatusSolicitud, refPrecioVenta,precio_venta_ini,precio_venta_proove,estatus_presupuesto) VALUES ('" + cadena + "', " + txtRefCant.Text + ", '', 0, 0, " + Request.QueryString["o"] + ", " + Request.QueryString["e"] + ", " + Request.QueryString["t"] + ", 0, '" + estatusRef + "',1," + precioVenta.ToString() + "," + precioVenta.ToString() + "," + precioVenta.ToString() + ",'P')";
                sqlDSRefOrden.Insert();
                grdRefacOrd.DataBind();
                acutalizaFase();
                txtRefDesc.Text = "";
                txtRefCant.Value = 1;
                txtPrecio.Value = 0;
                txtPrecio.Text = "0";
                RadTabStrip1.SelectedIndex = 1;
                RadMultiPage1.SelectedIndex = 1;
            }
        }
        catch (Exception x)
        {
            lblErrorNew.Text = "Se produjo ún error al agregar refacción: " + x.Message.ToString();
        }
        finally { actualizaTotales(); acutalizaFase(); }
    }

    protected void btnImprime_Click(object sender, EventArgs e)
    {
        ImpresionPresupuesto imprime = new ImpresionPresupuesto();
        Recepciones recepciones = new Recepciones();

        int empresa = Convert.ToInt32(Request.QueryString["e"]);
        int taller = Convert.ToInt32(Request.QueryString["t"]);
        int orden = Convert.ToInt32(Request.QueryString["o"]);

        string nomTaller = recepciones.obtieneNombreTaller(Request.QueryString["t"]);
        string usuario = recepciones.obtieneNombreUsuario(Request.QueryString["u"]);

        char detalle = Convert.ToChar(ddlDetalle.SelectedValue);
        string archivo = imprime.GenRepOrdTrabajo(empresa, taller, orden, nomTaller, usuario, Convert.ToInt32(ddlFirmantes.SelectedValue), detalle);
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
            lblErrorNew.Text = "Error al descargar el archivo: " + ex.Message;
        }
    }

    protected void chkRefaccion_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRefaccion.Checked)
        {
            cadenas.cadena = txtRefaccion.Text.ToLower();
            cadenas.conversion = 3;
            cadenas.convierteCadena();            
            txtRefDesc.Text = cadenas.cadena;
        }
        else
            txtRefDesc.Text = "";
    }

    protected void grdRefacOrd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblErrorNew.Text = "";
        if (e.CommandName == "Update")
        {
            string id = e.CommandArgument.ToString();
            TextBox descripcion = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("TextBox1") as TextBox;
            RadNumericTextBox cantidad = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("txtEdtRefCant") as RadNumericTextBox;
            RadNumericTextBox precio = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("txtPrecioM") as RadNumericTextBox;
            RadNumericTextBox precioUnit = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("txtEdtRefCosto") as RadNumericTextBox;
            DropDownList estatus = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("ddlEstatusRefEdit") as DropDownList;
            DropDownList ddlRefProcEd = grdRefacOrd.Rows[grdRefacOrd.EditIndex].FindControl("ddlRefProcEd") as DropDownList;
            if (1 == 1/*estatus.SelectedValue != "CA"*/)
            {
                decimal precioVenta = 0;
                try
                {
                    if (precioUnit.Text == "")
                        precioVenta = 0;
                    else
                        precioVenta = Convert.ToDecimal(precioUnit.Text);

                    string proceso = "ED";

                    string cadena = descripcion.Text.ToLower();
                    string letra = cadena.Substring(0, 1).ToUpper();
                    cadena = letra + cadena.Substring(1, cadena.Length-1);
                    cadenas.cadena = cadena;
                    //cadenas.conversion = 3;
                    //cadenas.convierteCadena();
                    llenarBitacora("Edito", proceso, cadenas.cadena, Convert.ToInt32(cantidad.Text),"");
                    sqlDSRefOrden.UpdateCommand = "UPDATE [Refacciones_Orden] SET [refDescripcion] = '" + cadenas.cadena + "', [refCantidad] = " + cantidad.Text + ", refPrecioVenta = " + precioVenta.ToString() + ",precio_venta_ini='" + precioUnit.Text + "' WHERE [refOrd_Id] = " + id + " AND [ref_no_orden] = " + Request.QueryString["o"] + " and [ref_id_empresa]=" + Request.QueryString["e"] + " and [ref_id_taller]=" + Request.QueryString["t"];
                    grdRefacOrd.EditIndex = -1;
                    grdRefacOrd.DataBind();
                    actualizaTotales();
                    acutalizaFase();
                }
                catch (Exception ex)
                {
                    lblErrorNew.Text = "Error al actualizar la refacción: " + ex.Message;
                }
                finally { actualizaTotales(); acutalizaFase(); }
                grdRefacOrd.DataBind();
            }
            else
            {
                lblErrorNew.Text = "La cancelación solo es posible desde Valuación";
                grdRefacOrd.EditIndex = -1;
                grdRefacOrd.DataBind();
                actualizaTotales();
                acutalizaFase();
            }
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
                object[] existeRelaciones = recepciones.existeRelacionRefaccion(Request.QueryString["e"], Request.QueryString["t"], Request.QueryString["o"], id);
                if (Convert.ToBoolean(existeRelaciones[0]))
                {
                    bool relacionado = Convert.ToBoolean(existeRelaciones[1]);
                    if (!relacionado)
                    {
                        string proceso = "EL";
                        llenarBitacora("Eliminó", proceso, lblRefaccion.Text, Convert.ToInt32(lblCantidad.Text), lblEstatusTex.Text);
                        sqlDSRefOrden.DeleteCommand = "DELETE FROM [Refacciones_Orden] WHERE [refOrd_Id] = " + id + "  AND [ref_no_orden] = " + Request.QueryString["o"] + " and [ref_id_empresa]=" + Request.QueryString["e"] + " and [ref_id_taller]=" + Request.QueryString["t"];
                        grdRefacOrd.DataBind();
                        actualizaTotales();
                        acutalizaFase();
                    }
                    else
                        lblErrorNew.Text = "No es posible eliminar la refaccion ya que se encuentra relacionada con una cotizacion y/o una orden de compra";
                }
                else
                    lblErrorNew.Text = "No es posible eliminar la refaccion ya que se encuentra relacionada con una cotizacion y/o una orden de compra";
            }
            catch (Exception ex)
            {
                lblErrorNew.Text = "Error al eliminar la refacción: " + ex.Message;
            }
            finally {
                actualizaTotales();
                acutalizaFase();
            }
        }
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
        acutalizaFase();
    }

    protected void GridView2_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        actualizaTotales();
        acutalizaFase();
    }

    protected void grdRefacOrd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header) { terminados = 0; totRef = 0; }
            else if(e.Row.RowType == DataControlRowType.EmptyDataRow) { terminados = 0; totRef = 0; }
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
                if (modo == "Edit")
                {
                    string estatus = lblEstatusRefEdit.Text;
                    if (estatus != "NA" && estatus != "AU")
                        ddlEstatusRefEdit.SelectedValue = estatus;
                }
                string estatusRef = DataBinder.Eval(e.Row.DataItem, "refEstatus").ToString();
                string estatusPresupuesto = DataBinder.Eval(e.Row.DataItem, "estatus_presupuesto").ToString();
                string idCfd = DataBinder.Eval(e.Row.DataItem, "idCfd").ToString();

                int idFactura = 0;
                try { if (idCfd == "") idFactura = 0; else idFactura = Convert.ToInt32(idCfd); } catch (Exception) { idFactura = -1; }
                /*
                if (idFactura != 0)
                    e.Row.CssClass = "alert-info";
                */
                string aplicaRem = DataBinder.Eval(e.Row.DataItem, "aplica_rem").ToString();
                string aplicaSS = DataBinder.Eval(e.Row.DataItem, "aplica_ss").ToString();
                string monto = DataBinder.Eval(e.Row.DataItem, "refPrecioVenta").ToString();
                string cantidad = DataBinder.Eval(e.Row.DataItem, "refCantidad").ToString();

                bool aplicadoRem = false;
                bool aplicadoSS = false;
                /*
                if (aplicaRem.ToUpper() == "TRUE" || aplicaRem == "1")
                    aplicadoRem = true;

                if (aplicaSS.ToUpper() == "TRUE" || aplicaSS == "1")
                    aplicadoSS = true;

                if (estatusRef == "FA" || aplicadoRem || aplicadoSS)
                {
                    btnEditar.Visible = btnEliminar.Visible = false;
                }
                else {
                    if (idFactura != 0)
                        btnEditar.Visible = btnEliminar.Visible = false;
                }
                if (estatusPresupuesto == "T")
                {
                    btnEditar.Visible = btnEliminar.Visible = false;
                    terminados++;
                }
                else {
                    if(idFactura!=0)
                        btnEditar.Visible = btnEliminar.Visible = false;
                }
                */
                decimal improte = Convert.ToDecimal(monto) * Convert.ToDecimal(cantidad);
                try { totRef = totRef + Convert.ToDecimal(improte); } catch (Exception) { totRef = totRef + 0; }
            }
            else if (e.Row.RowType == DataControlRowType.Footer) {
                /*
                if (terminados > 0)
                    lnkEditar.Visible = true;
                else
                    lnkEditar.Visible = false;
                */
                lblTotalRefacciones.Text = "Total Refacciones: " + totRef.ToString("C2");
            }
        }
        catch (Exception) { }
    }

    protected void lnkCotizar_Click(object sender, EventArgs e)
    {
        Response.Redirect("CotizaRefProv.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"] + "&c=0");
    }
    

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        lblErrorLog.Text = "";
        Autorizaciones autoriza = new Autorizaciones();
        autoriza.nick = txtUsuarioLog.Text;
        autoriza.contrasena = txtContraseñaLog.Text;
        autoriza.permiso = 1;
        autoriza.validaUsuario();
        if (autoriza.Valido)
        {
            autorizacion();
        }
        else
            lblErrorLog.Text = "Error: " + autoriza.Error;
    }

    private void autorizacion()
    {
        int opcion = 0;
        try
        {
            opcion = Convert.ToInt32(Session["autoriza"].ToString());
        }
        catch (Exception) { opcion = 0; }
        switch (opcion)
        {
            case 1:
                {
                    decimal precioVenta = 0;
                    if (txtPrecio.Text == "")
                        precioVenta = 0;
                    else
                        precioVenta = Convert.ToDecimal(txtPrecio.Text);
                    string estatusRef = ddlEstatusRefaccion.SelectedValue.ToString();
                    string proceso = "AL";
                    cadenas.cadena = txtRefDesc.Text.ToLower();
                    cadenas.conversion = 3;
                    cadenas.convierteCadena();
                    llenarBitacora("Agrego", proceso, txtRefDesc.Text, Convert.ToInt32(txtRefCant.Value), ddlEstatusRefaccion.SelectedItem.ToString());
                    sqlDSRefOrden.InsertCommand = "INSERT INTO [Refacciones_Orden] ([refDescripcion], [refCantidad], [refObservaciones], refProveedor, refCosto, [ref_no_orden], ref_id_empresa, ref_id_taller, refPorcentSobreCosto, refEstatus, refEstatusSolicitud, refPrecioVenta,precio_venta_ini,precio_venta_proove,estatus_presupuesto) VALUES ('" + cadenas.cadena + "', " + txtRefCant.Text + ", '', 0, 0, " + Request.QueryString["o"] + ", " + Request.QueryString["e"] + ", " + Request.QueryString["t"] + ", 0, '" + estatusRef + "',1," + precioVenta.ToString() + "," + precioVenta.ToString() + "," + precioVenta.ToString() + ",'P')";
                    sqlDSRefOrden.Insert();
                    grdRefacOrd.DataBind();
                    actualizaTotales();
                    acutalizaFase();
                    txtRefDesc.Text = "";
                    txtRefCant.Value = 1;
                    txtPrecio.Value = 0;
                    txtPrecio.Text = "0";
                    PanelMask.Visible = PanelPopUpPermiso.Visible = false;
                }
                break;
            case 2:
                {
                    try
                    {
                        int[] sesiones = obtieneSesiones();
                        object[] actualizados = recepciones.actualizaPresupuestos(sesiones, "P");
                        if (Convert.ToBoolean(actualizados[0]))
                        {
                            if (Convert.ToBoolean(actualizados[1]))
                            {
                                grdRefacOrd.DataBind();
                                GridView2.DataBind();
                                notificar();
                                PanelMask.Visible = PanelPopUpPermiso.Visible = false;
                            }
                            else
                                lblErrorNew.Text = "Error: " + Convert.ToString(actualizados[1]);
                        }
                        else
                            lblErrorNew.Text = "Error: " + Convert.ToString(actualizados[1]);
                    }
                    catch (Exception ex) { lblErrorNew.Text = "Error: " + ex.Message; }
                }
                break;
            case 0:
                lblErrorLog.Text = "Se perdio la conexión, recargue la página e intentenlo nuevamente";
                break;
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        PanelMask.Visible = PanelPopUpPermiso.Visible = false;
    }

    protected void lnkTerminado_Click(object sender, EventArgs e)
    {
        try {
            int[] sesiones = obtieneSesiones();
            object[] actualizados = recepciones.actualizaPresupuestos(sesiones,"T");
            if (Convert.ToBoolean(actualizados[0]))
            {
                if (Convert.ToBoolean(actualizados[1]))
                {
                    grdRefacOrd.DataBind();
                    GridView2.DataBind();
                    notificar();
                    lnkEditar.Visible = true;
                }
                else
                    lblErrorNew.Text = "Error: " + Convert.ToString(actualizados[1]);
            }
            else
                lblErrorNew.Text = "Error: " + Convert.ToString(actualizados[1]);
        }
        catch (Exception ex) { lblErrorNew.Text = "Error: " + ex.Message; }
    }

    private void notificar()
    {
        Notificaciones notifi = new Notificaciones();
        notifi.Articulo = Request.QueryString["o"];
        notifi.Empresa = Convert.ToInt32(Request.QueryString["e"]);
        notifi.Taller = Convert.ToInt32(Request.QueryString["t"]);
        notifi.Usuario = Request.QueryString["u"];
        notifi.Fecha = fechas.obtieneFechaLocal();
        notifi.Estatus = "P";
        notifi.Clasificacion = 2;
        notifi.Origen = "O";
        notifi.armaNotificacion();
        notifi.agregaNotificacion();
        notifi.Clasificacion = 3;
        notifi.Origen = "O";
        notifi.armaNotificacion();
        notifi.agregaNotificacion();
    }

    private void controlAccesos()
    {
        int[] sesiones = obtieneSesiones();
        permisos.idUsuario = sesiones[0];
        /*permisos.obtienePermisos();
        bool[] permisosUsuario = permisos.permisos;
        permisos.permisos = permisosUsuario;*/
        permisos.permiso = 67;
        permisos.tienePermisoIndicado();
        if (!permisos.tienePermiso)
            lnkTerminado.Visible = false;
        else
            lnkTerminado.Visible = true;

        permisos.permiso = 68;
        permisos.tienePermisoIndicado();
        if (!permisos.tienePermiso)
            lnkCotizar.Visible = false;
        else
            lnkCotizar.Visible = true;

    }
       
    protected void lnkEditar_Click(object sender, EventArgs e)
    {
        Session["autoriza"] = 2;
        int usuID = int.Parse(Request.QueryString["u"]);
        permisos.idUsuario = usuID;
        permisos.permiso = 106;
        permisos.tienePermisoIndicado();
        if (permisos.tienePermiso)
        {
            try
            {
                int[] sesiones = obtieneSesiones();
                object[] actualizados = recepciones.actualizaPresupuestos(sesiones, "P");
                if (Convert.ToBoolean(actualizados[0]))
                {
                    if (Convert.ToBoolean(actualizados[1]))
                    {
                        grdRefacOrd.DataBind();
                        GridView2.DataBind();
                        //notificar();
                    }
                    else
                        lblErrorNew.Text = "Error: " + Convert.ToString(actualizados[1]);
                }
                else
                    lblErrorNew.Text = "Error: " + Convert.ToString(actualizados[1]);
            }
            catch (Exception ex) { lblErrorNew.Text = "Error: " + ex.Message; }
        }
        else
        {
            txtContraseñaLog.Text = txtUsuarioLog.Text = lblErrorLog.Text = "";
            PanelMask.Visible = PanelPopUpPermiso.Visible = true;
        }
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {            
            if (e.Row.RowType == DataControlRowType.Header) { terminados = 0; totMo = 0; }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow) { terminados = 0; totMo = 0; }
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
                LinkButton btnEditar = e.Row.FindControl("lnkEditar") as LinkButton;
                LinkButton btnEliminar = e.Row.FindControl("lnkEliminar") as LinkButton;
                
                string estatusRef = DataBinder.Eval(e.Row.DataItem, "statusCfd").ToString();
                string estatusPresupuesto = DataBinder.Eval(e.Row.DataItem, "estatus_presupuesto").ToString();
                string aplicaRem = DataBinder.Eval(e.Row.DataItem, "aplica_rem").ToString();
                string aplicaSS = DataBinder.Eval(e.Row.DataItem, "aplica_ss").ToString();
                string monto = DataBinder.Eval(e.Row.DataItem, "monto_mo").ToString();

                string idCfd = DataBinder.Eval(e.Row.DataItem, "idCfd").ToString();

                int idFactura = 0;
                try { if (idCfd == "") idFactura = 0; else idFactura = Convert.ToInt32(idCfd); } catch (Exception) { idFactura = -1; }
                /*if (idFactura != 0)
                    e.Row.CssClass = "alert-info";
                */
                bool aplicadoRem = false;
                bool aplicadoSS = false;
                /*
                if (aplicaRem.ToUpper() == "TRUE" || aplicaRem == "1")
                    aplicadoRem = true;

                if (aplicaSS.ToUpper() == "TRUE" || aplicaSS == "1")
                    aplicadoSS = true;
                
                if (estatusRef == "FA" || aplicadoRem || aplicadoSS)
                {
                    btnEditar.Visible = btnEliminar.Visible = false;
                }
                else {
                    if (idFactura != 0)
                        btnEditar.Visible = btnEliminar.Visible = false;
                }
                if (estatusPresupuesto == "T")
                {
                    btnEditar.Visible = btnEliminar.Visible = false;
                    terminados++;
                }
                else {
                    if (idFactura != 0)
                        btnEditar.Visible = btnEliminar.Visible = false;
                }
                */
                try { totMo = totMo + Convert.ToDecimal(monto); } catch (Exception) { totMo = totMo + 0; }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                /*
                if (terminados > 0)
                    lnkEditar.Visible = true;
                else
                    lnkEditar.Visible = false;
                */
                lblTotalManoObra.Text = "Total de Mano Obra: " + totMo.ToString("C2");

            }
        }
        catch (Exception) { }
    }
}