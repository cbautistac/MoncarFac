using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OperariosTickets : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnPagar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        LinkButton btnPagar = (LinkButton)sender;
        string[] argumentos = btnPagar.CommandArgument.ToString().Split(new char[] { ';' });
        try
        {
            int orden = Convert.ToInt32(argumentos[0]);
            int empresa = Convert.ToInt32(argumentos[1]);
            int taller = Convert.ToInt32(argumentos[2]);
            int empleado = Convert.ToInt32(argumentos[3]);
            int asignacion = Convert.ToInt32(argumentos[4]);
            decimal montoPagado = Convert.ToDecimal(argumentos[5]);
            int usuario = Convert.ToInt32(Request.QueryString["u"]);
            Operarios operario = new Operarios();
            operario.empresa = empresa;
            operario.taller = taller;
            operario.orden = orden;
            operario.empleado = empleado;
            operario.asignacion = asignacion;
            operario.usuario = usuario;
            operario.montoPagado = montoPagado;
            operario.pagado = 1;
            operario.actualizaPagado();
            object[] actualizado = operario.retorno;
            if (Convert.ToBoolean(actualizado[0]))
            {
                lblError.Text = "El operario a sido actualizado";
                RadGrid2.DataBind();
            }
            else
                lblError.Text = "Error al actualizar al operario: " + actualizado[1].ToString();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al colocar como pagado al operario: " + ex.Message;
        }
    }

    protected void btnCanPagar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        LinkButton btnPagar = (LinkButton)sender;
        string[] argumentos = btnPagar.CommandArgument.ToString().Split(new char[] { ';' });
        try
        {
            int orden = Convert.ToInt32(argumentos[0]);
            int empresa = Convert.ToInt32(argumentos[1]);
            int taller = Convert.ToInt32(argumentos[2]);
            int empleado = Convert.ToInt32(argumentos[3]);
            int asignacion = Convert.ToInt32(argumentos[4]);
            decimal montoPagado = Convert.ToDecimal(argumentos[5]);
            int usuario = Convert.ToInt32(Request.QueryString["u"]);
            Operarios operario = new Operarios();
            operario.empresa = empresa;
            operario.taller = taller;
            operario.orden = orden;
            operario.empleado = empleado;
            operario.asignacion = asignacion;
            operario.usuario = usuario;
            operario.montoPagado = 0;
            operario.pagado = 0;
            operario.actualizaPagado();
            object[] actualizado = operario.retorno;
            if (Convert.ToBoolean(actualizado[0]))
            {
                lblError.Text = "El operario a sido actualizado";                
                RadGrid2.DataBind();
            }
            else
                lblError.Text = "Error al actualizar al operario: " + actualizado[1].ToString();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al colocar como no pagado al operario: " + ex.Message;
        }
    }

    protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;
            
            
            var pagado = Convert.ToBoolean(r[13]);
            try
            {
                
                var btnPago = e.Item.Controls[0].FindControl("btnPagar") as LinkButton;
                var btnCanPago = e.Item.Controls[0].FindControl("btnCanPagar") as LinkButton;
               
                if (pagado)
                {
                    btnPago.Visible = false;
                    btnCanPago.Visible = true;
                }
                else
                {
                    btnPago.Visible = true;
                    btnCanPago.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}