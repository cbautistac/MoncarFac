using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Refacciones_Orden : System.Web.UI.Page
{
    DatosOrdenes datosOrdenes = new DatosOrdenes();
    //--ALE 15122015: Variable para controlar grid de Cotizacion de Refacciones Editable
    private bool grdCotizaModoEdicion = true;
    protected bool esModoEdicion
    {
        get { return this.grdCotizaModoEdicion; }
        set { this.grdCotizaModoEdicion = value; }
    }
    //--ALE 15122015
    
    private int empresa, taller, no_orden; 
    protected void Page_Load(object sender, EventArgs e)
    {
        empresa = Convert.ToInt32(Request.QueryString["e"]);
        taller = Convert.ToInt32(Request.QueryString["t"]);
        no_orden = Convert.ToInt32(Request.QueryString["o"]);
        //--ALE 15122015: Si no hay refacciones cotizadas, establece el grid de cotización en modo edición o de lectura en caso contrario
        if (!IsPostBack)
        {
            SqlConnection conn = creaSqlConnection();
            using (conn)
            {
                string qryEmpTall = "SELECT ro.refOrd_Id, ro.refEstatus FROM Refacciones_Orden AS ro " +
                    "INNER JOIN Refacciones_Cotiza AS rc ON ro.refOrd_Id = rc.cot_refOrd_ID AND ro.ref_no_orden = rc.cot_ref_no_orden " +
                    "WHERE [ref_no_orden]=" + no_orden;
                SqlCommand comm = new SqlCommand(qryEmpTall, conn);
                conn.Open();
                SqlDataReader dr = comm.ExecuteReader();

                if (dr.HasRows)
                {
                    this.grdCotizaModoEdicion = false;
                    lblCotiza.Text = "Ver cotización";
                    grdRefCotiza.DataBind();
                }
                else
                {
                    lblCotiza.Text = "Agregar Cotización";
                    this.grdCotizaModoEdicion = true;
                    //grdRefCotiza.DataBind();
                }
                dr.Close();
                conn.Close();
            }
        }
    }

    protected SqlConnection creaSqlConnection()
    {
        SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Taller"].ConnectionString);
        return conn;
    }

    protected void btnCotiza_Click(object sender, EventArgs e)
    {
        grdRefCotiza.Visible = true;
        if (lblCotiza.Text.Contains("Agregar"))
        {
            divRefCotBtns.Visible = true;
        }
        else
        {
            grdRefCotiza.Caption = "Haga Clic en un proveedor de la refacción para cambiar el proveedor sugerido.";
            divRefCotBtns.Visible = false;
        }
    }
    protected void lnkGuardaCot_Click(object sender, EventArgs e)
    {
        int refOrd_Id = 0;
        bool guardoCot = false;
        int idxColumn, idxCeldHead;
        int rprov = 0;
        decimal rcost = 0;
        GridViewRow refCotHeadRow = grdRefCotiza.HeaderRow;
        Literal lblRefOrdID;
        //Busca en cada fila de refacciones el ID y costo, si es != de 0 y se ha seleccionado proveedor los guarda en tbl Refacciones_Cotiza
        foreach (GridViewRow fila in grdRefCotiza.Rows)
        {
            if (fila.RowType == DataControlRowType.DataRow)
            {
                idxColumn = 0;
                lblRefOrdID = (Literal)fila.FindControl("lblRefOrdID");
                refOrd_Id = Convert.ToInt32(lblRefOrdID.Text);
                DataControlFieldHeaderCell cellHeader;
                int colsgrid = fila.Cells.Count;
                int colsHead = refCotHeadRow.Cells.Count;
                foreach (TableCell cell in fila.Cells)//OfType<DataControlFieldCell>())
                {
                    //--para verificar coincidencia de columnas: idxColumn = idxCeldHead
                    cellHeader = (DataControlFieldHeaderCell)refCotHeadRow.Cells[idxColumn];
                    idxCeldHead = refCotHeadRow.Cells.GetCellIndex(cellHeader);
                    //--
                    // busca en celda del encabezado el combo para traer el proveedor seleccionado
                    if (refCotHeadRow.Cells[idxColumn].Controls.OfType<DropDownList>().Count() > 0)
                        rprov = Convert.ToInt32(refCotHeadRow.Cells[idxColumn].Controls.OfType<DropDownList>().First().SelectedValue);

                    if (cell.Controls.OfType<TextBox>().Count() > 0 && decimal.TryParse(cell.Controls.OfType<TextBox>().First().Text.Trim(), out rcost))
                        if (rprov > 0 && rcost > 0)
                        {
                            guardaCotizaciones(refOrd_Id, no_orden, rprov, rcost);
                            guardoCot = true;
                            rcost = rprov = 0;
                        }
                    idxColumn++;
                }
            }
            rcost = rprov = 0;
        }
        if (guardoCot)
        {
            //si se guardaron cotizaciones, se actualiza tbl Refacciones_Orden con el prov con costos mínimos sugeridos
            guardaCotizaciones(refOrd_Id, no_orden, -1, 0);
            grdCotizaModoEdicion = false;
            grdRefacCotizado.DataBind();
            lblCotiza.Text = "Ver Cotización";
        }
        grdRefCotiza.Visible = false;
        divRefCotBtns.Visible = false;
    }

    protected void guardaCotizaciones(int refOrd_Id, int no_orden, int provId, decimal costo)
    {
        using (SqlConnection conn = creaSqlConnection())
        {
            string qryRefaccCot = string.Empty;
            SqlCommand comm = new SqlCommand(qryRefaccCot, conn);
            conn.Open();
            if (provId == -1 && costo == 0)
            {
                int minProv, cot_refOrd_ID;
                decimal minCosto;
                string qryCostoMin = string.Format("SELECT MIN([cotCosto]) AS costoMin, cot_refOrd_ID, rc.cotProv_ID " +
                    "FROM Refacciones_Cotiza AS rc " +
                    "WHERE cot_ref_no_orden = {0}  AND cotCosto = (SELECT MIN(cotCosto) FROM Refacciones_Cotiza AS rc1 WHERE rc1.cot_refOrd_ID = rc.cot_refOrd_ID) " +
                    "GROUP BY rc.cot_refOrd_ID, rc.cotProv_ID ORDER BY rc.cot_refOrd_ID", no_orden);
                comm.CommandText = qryCostoMin;
                SqlDataReader dr = comm.ExecuteReader();
                SqlConnection conRef = creaSqlConnection();
                conRef.Open();
                SqlCommand comRef = new SqlCommand(qryRefaccCot, conRef);
                comRef.Parameters.Add("@refProveedor", SqlDbType.Int).DbType = DbType.Int32;
                comRef.Parameters.Add("@refCosto", SqlDbType.Decimal).Direction = ParameterDirection.Input;
                while (dr.Read())
                {
                    minProv = Convert.ToInt32(dr["cotProv_ID"].ToString());
                    minCosto = Convert.ToDecimal(dr["costoMin"].ToString());
                    cot_refOrd_ID = Convert.ToInt32(dr["cot_refOrd_ID"].ToString());
                    qryRefaccCot = "UPDATE Refacciones_Orden SET refProveedor = @refProveedor, refCosto = @refCosto, refPorcentSobreCosto=" + txtPorcentGlobal.Text.Trim() +
                        "WHERE ref_no_orden=" + no_orden + " AND refOrd_Id=" + cot_refOrd_ID;
                    comRef.CommandText = qryRefaccCot;
                    comRef.Parameters["@refProveedor"].Value = minProv;
                    comRef.Parameters["@refCosto"].Value = minCosto;
                    int result = comRef.ExecuteNonQuery();
                }
                dr.Close();
            }
            else
            {
                qryRefaccCot = "INSERT INTO Refacciones_Cotiza VALUES (@refOrd_Id, @no_orden, @provID, @costo)";
                comm.CommandText = qryRefaccCot;
                comm.Parameters.AddWithValue("@refOrd_Id", refOrd_Id).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@no_orden", no_orden).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@provID", provId).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@costo", costo).DbType = DbType.Decimal;
                comm.ExecuteNonQuery();
            }
            conn.Close();
        }
    }

    protected void lnkAddProv_Click(object sender, EventArgs e)
    {
        /*TemplateField tfield = new TemplateField();
        tfield.HeaderText = "Proveedor";
        DataControlFieldCollection grdDfCollection = grdRefCotiza.Columns.CloneFields();
        grdRefCotiza.Columns.Add(grdDfCollection[6]);
        
        tfield = new TemplateField();
        tfield.HeaderText = "Costo";
        grdRefCotiza.Columns.Add(tfield);
         */
        if (!grdRefCotiza.Columns[8].Visible)
        { 
            grdRefCotiza.Columns[8].Visible = true;
            grdRefCotiza.Columns[9].Visible = true;
        }
        else if (!grdRefCotiza.Columns[10].Visible)
        {
            grdRefCotiza.Columns[10].Visible = true;
            grdRefCotiza.Columns[11].Visible = true;
            lnkAddProv.Enabled = false;
        }
    }
    protected void ddlProvs1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //var x = ((GridViewRow)grdRefacCotizado.SelectedRow).FindControl("textDateSent").ClientID;
        //var y = ((GridViewRow)grdRefacCotizado.Rows[grdRefacCotizado.EditIndex]).FindControl("txtPorcent").ClientID;
        DropDownList ddlProv = (DropDownList)sender;
        DataControlFieldHeaderCell cellHeader = (DataControlFieldHeaderCell)ddlProv.Parent;
        GridViewRow grdRow = (GridViewRow)ddlProv.Parent.Parent;
        int columnIndx = grdRow.Cells.GetCellIndex(cellHeader);
        foreach (GridViewRow r in grdRefCotiza.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow && ddlProv.SelectedIndex > 0)
                r.Cells[columnIndx].Controls.OfType<Label>().First().Text = ddlProv.SelectedItem.Text;
        }
    }
    protected void grdRefacCotizado_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Update"))
            sqlDSRefOrden.UpdateCommand = "UPDATE [Refacciones_Orden] SET refPorcentSobreCosto = @refPorcentSobreCosto, refPrecioVenta = @refPrecioVenta WHERE [refOrd_Id] = @refOrd_Id AND [ref_no_orden] = @ref_no_orden";
        else if (e.CommandName.Contains("cot"))
        {
            string[] args = e.CommandArgument.ToString().Split(';');
            sqlDSRefOrden.UpdateCommand = "UPDATE [Refacciones_Orden] SET refEstatus='" + (e.CommandName.Equals("cotAprobar") ? "AP" : e.CommandName.Equals("cotAutoriza") ? "AU" : "NA") + "' " +
                "WHERE refOrd_Id = " + args[0].ToString() + " AND ref_no_orden =" + args[1].ToString();
            sqlDSRefOrden.Update();
        }
    }

    protected void grdRefCotiza_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && !esModoEdicion)
        {
            /*Alex 16/12/2015: En la fila actual del grid se lee el Id y con este se traen las cotizaciones de la refacción,
             * por cada fila de la consulta corresponde a una columna en el grid, se busca por celda el control del proveedor y del costo
             * asignándoles valores, cuando ya no hay registros en la consulta reincia el proceso en la siguiente fila del grid.
            */
            Literal lblRefOrdID = (Literal)e.Row.FindControl("lblRefOrdID");
            int refOrd_ID = Convert.ToInt32(lblRefOrdID.Text);
            using (SqlConnection conn = creaSqlConnection())
            {
                string qry = "SELECT ro.refDescripcion, rc.cotProv_ID, Cliprov.razon_social, rc.cotCosto FROM Refacciones_Orden AS ro " +
                    "INNER JOIN Refacciones_Cotiza AS rc ON ro.refOrd_Id = rc.cot_refOrd_ID AND ro.ref_no_orden = rc.cot_ref_no_orden " +
                    "INNER JOIN Cliprov ON rc.cotProv_ID = Cliprov.id_cliprov AND Cliprov.tipo = 'P'" +
                    "WHERE rc.cot_refOrd_ID =" + lblRefOrdID.Text + " ORDER BY rc.cot_refOrd_ID";
                conn.Open();
                SqlCommand comm = new SqlCommand(qry, conn);
                SqlDataReader dr = comm.ExecuteReader();
                int rows = dr.RecordsAffected;
                //dr.Read();
                string strProvRs = string.Empty;
                string strCosto = string.Empty;
                string strProvId = string.Empty;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.Controls.OfType<LinkButton>().Count() > 0)
                    {
                        if (dr.Read())
                        {
                            strProvId = dr["cotProv_ID"].ToString();
                            strProvRs = dr["razon_social"].ToString();
                            strCosto = dr["cotCosto"].ToString();
                        }
                        else
                            break;
                        LinkButton lnkRefaccProv = cell.Controls.OfType<LinkButton>().First();
                        lnkRefaccProv.Text = strProvRs;
                        string strComArgs = lnkRefaccProv.CommandArgument;
                        lnkRefaccProv.CommandArgument = strComArgs + ";" + strProvId + ";" + strCosto;
                    }
                    else if (cell.Controls.OfType<Label>().Count() > 0)
                    {
                        cell.Controls.OfType<Label>().First().Text = strCosto;
                    }
                }
                dr.Close();
                conn.Close();
            }
        }
    }
    protected void grdRefCotiza_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            //--Alex 17/12/15: Al seleccionar un proveedor se cambia el provedor sugerido en el grid de refacciones cotizadas (grdRefacCotizado)
            int no_orden = Convert.ToInt32(grdRefCotiza.DataKeys[0].Values[1]);
            string[] args = e.CommandArgument.ToString().Split(';');
            int refOrdID = Convert.ToInt32(args[0].ToString());
            int provId = Convert.ToInt32(args[1].ToString());
            decimal refCosto = Convert.ToDecimal(args[2].ToString());
            using (SqlConnection con = creaSqlConnection())
            {
                string qry = "UPDATE Refacciones_Orden SET refProveedor=" + provId + ", refCosto=" + refCosto +
                    "WHERE refOrd_Id=" + refOrdID + " AND ref_no_orden=" + no_orden;
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                int ok = com.ExecuteNonQuery();
                grdRefacCotizado.DataBind();
                grdRefCotiza.Visible = false;
            }
        }
    }
    protected void ddlRefEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlRefEstatus = (DropDownList)sender;
        DataControlFieldCell cell = (DataControlFieldCell)ddlRefEstatus.Parent;
        GridViewRow grdRow = (GridViewRow)ddlRefEstatus.Parent.Parent;
        TextBox txtRefFechSoli = (TextBox)grdRow.FindControl("txtRefFechSoli");
        if (int.Parse(ddlRefEstatus.SelectedValue) != 2)
        {
            txtRefFechSoli.Enabled = true;
            ((TextBox)grdRow.FindControl("txtRefFechEnt")).Enabled = true;
            ((TextBox)grdRow.FindControl("txtRefFechReal")).Text = "";
        }
        if (int.Parse(ddlRefEstatus.SelectedValue) == 2)
            ((TextBox)grdRow.FindControl("txtRefFechReal")).Enabled = true;
        txtRefFechSoli.Focus();
    }

    protected void lnkGuardaFech_Command(object sender, CommandEventArgs e)
    {
        LinkButton lnkGuardaFech = (LinkButton)sender;
        DataControlFieldCell cell = (DataControlFieldCell)lnkGuardaFech.Parent;
        GridViewRow grdRow = (GridViewRow)lnkGuardaFech.Parent.Parent;

        string[] args = e.CommandArgument.ToString().Split(';');
        DropDownList ddlRefEstatus = (DropDownList)grdRow.FindControl("ddlRefEstatus");
        TextBox txtRefFechSoli = (TextBox)grdRow.FindControl("txtRefFechSoli");
        TextBox txtRefFechEnt = (TextBox)grdRow.FindControl("txtRefFechEnt");
        TextBox txtRefFechReal = (TextBox)grdRow.FindControl("txtRefFechReal");
        
        sqlDSRefOrden.UpdateCommand = "UPDATE [Refacciones_Orden] SET refEstatusSolicitud=@refEstatusSolicitud, refFechSolicitud=@refFechSolicitud, refFechEntregaEst=@refFechEntregaEst, refFechEntregaReal=@refFechEntregaReal " +
            "WHERE refOrd_Id = " + args[0].ToString() + " AND ref_no_orden =" + args[1].ToString();
        sqlDSRefOrden.UpdateParameters["refEstatusSolicitud"].DefaultValue =ddlRefEstatus.SelectedValue;
        sqlDSRefOrden.UpdateParameters.Add("refFechSolicitud", TypeCode.DateTime, txtRefFechSoli.Text);
        sqlDSRefOrden.UpdateParameters.Add("refFechEntregaEst", TypeCode.DateTime, txtRefFechEnt.Text);
        sqlDSRefOrden.UpdateParameters.Add("refFechEntregaReal", TypeCode.DateTime, txtRefFechReal.Text);
        sqlDSRefOrden.Update();
    }
    protected void grdRefacCotizado_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (DataBinder.Eval(e.Row.DataItem, "refEstatus").Equals("AU"))
                ((LinkButton)e.Row.FindControl("lnkEditarR")).Enabled = false;
            DropDownList ddlRefEstatus = (DropDownList)e.Row.FindControl("ddlRefEstatus");
            ddlRefEstatus.SelectedIndex = Convert.ToInt16(DataBinder.Eval(e.Row.DataItem, "refEstatusSolicitud").ToString()) - 1;
            if (!ddlRefEstatus.Enabled)
            {
                ddlRefEstatus.ToolTip = "Para seleccionar estatus, la contización debe estar aprobada o autorizada";
                ((LinkButton)e.Row.FindControl("lnkGuardaFech")).Visible = false;
            }
            if (Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "refPorcentSobreCosto").ToString()) > 0)
                ((LinkButton)e.Row.FindControl("lnkEditarR")).Enabled = false;
        }
    }

    protected void btnCotiza_Click1(object sender, EventArgs e)
    {
        Response.Redirect("CotizaRefProv.aspx?u=" + Request.QueryString["u"] + "&p=" + Request.QueryString["p"] + "&e=" + Request.QueryString["e"] + "&t=" + Request.QueryString["t"] + "&o=" + Request.QueryString["o"] + "&f=" + Request.QueryString["f"]);
    }
}