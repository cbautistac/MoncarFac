using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using Telerik.Web.UI;
using System.Data;

public partial class PagoOperarios : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        cargaInfo();
        if (!IsPostBack)
        {
            lblAno.Text = fechas.obtieneFechaLocal().Year.ToString();
        }
    }

    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[4] { 0, 0, 0, 0 };
        try
        {
            sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(Request.QueryString["t"]);
        }
        catch (Exception x)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    private void cargaInfo()
    {
        int[] sesiones = obtieneSesiones();
        if (sesiones[0] == 0 || sesiones[1] == 0 || sesiones[2] == 0 || sesiones[3] == 0)
            Response.Redirect("Default.aspx");
        try
        {
            if(lblAno.Text=="")
                lblAno.Text = fechas.obtieneFechaLocal().Year.ToString();
            SqlDataSource1.SelectCommand = "declare @ano int declare @empresa int set @ano=" + lblAno.Text + " set @empresa=" + sesiones[2] + " " +
"select *,(montoAutorizado - montoUsuado) as pagar from(select *, isnull((select sum(monto) from operativos_orden where id_empresa = t.id_empresa and id_taller = t.id_taller and no_orden = t.no_orden and idemp = t.idemp), 0) as montoAutorizado,(select isnull(sum(total), 0) from Registro_Pinturas r where r.id_empresa = t.id_empresa and r.id_taller = t.id_taller and r.no_orden = t.no_orden and r.idEmp = t.IdEmp and r.ano = @ano) as montoUsuado from(" +
"select distinct oo.no_orden, oo.id_taller, t.nombre_taller as taller, oo.id_empresa, em.razon_social as empresa, oo.idemp, ltrim(rtrim(ltrim(rtrim(e.Nombres)) + ' ' + ltrim(rtrim(isnull(e.Apellido_Paterno, ''))) + ' ' + ltrim(rtrim(isnull(e.Apellido_Materno, ''))))) as nombre," +
"p.descripcion as puesto, oo.estatus, orp.avance_orden, (tv.descripcion + ' ' + m.descripcion + ' ' + tu.descripcion) as vehiculo, cast(v.modelo as char(4)) as modelo, rtrim(ltrim(v.color_ext)) as color,v.serie_vin, orp.placas, orp.id_cliprov, c.razon_social, isnull(oo.pagado,0) as pagado,orp.status_orden from operativos_orden oo " +
"inner join Talleres t on t.id_taller = oo.id_taller inner join Empresas em on em.id_empresa = oo.id_empresa inner join empleados e on e.IdEmp = oo.IdEmp left join Puestos p on p.id_puesto = e.Puesto inner join ordenes_reparacion orp on orp.no_orden = oo.no_orden and orp.id_empresa = oo.id_empresa and orp.id_taller = oo.id_taller " +
"left join Vehiculos v on v.id_marca = orp.id_marca and v.id_tipo_vehiculo = orp.id_tipo_vehiculo and v.id_tipo_unidad = orp.id_tipo_unidad and v.id_vehiculo = orp.id_vehiculo left join Marcas m on m.id_marca = orp.id_marca left join Tipo_Vehiculo tv on tv.id_tipo_vehiculo = orp.id_tipo_vehiculo left join Tipo_Unidad tu on tu.id_marca = orp.id_marca and tu.id_tipo_vehiculo = orp.id_tipo_vehiculo and tu.id_tipo_unidad = orp.id_tipo_unidad " +
"left join Cliprov c on c.id_cliprov = orp.id_cliprov and c.tipo = 'C' where oo.id_empresa = @empresa and oo.estatus = 'T') as t group by no_orden,id_taller,taller,id_empresa,empresa,idemp,nombre,puesto,estatus,vehiculo,avance_orden,modelo,color,serie_vin,placas,id_cliprov,razon_social,pagado,status_orden) as tabla";
        }
        catch (Exception ex)
        {
            Session["errores"] = ex.Message;
            Session["paginaOrigen"] = "Ordenes.aspx";
            Response.Redirect("AppErrorLog.aspx");
        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView filas = (DataRowView)e.Item.DataItem;
            DataRow r = filas.Row;            
            var btnOrden = r[0].ToString();
            var status = r[18].ToString();
            var pagado = Convert.ToBoolean(r[17]);
            try
            {                
                int noOrden = Convert.ToInt32(btnOrden);

                var btn = e.Item.Controls[0].FindControl("lblOrden") as Label;                
                var btnPago = e.Item.Controls[0].FindControl("btnPagar") as LinkButton;
                var btnCanPago = e.Item.Controls[0].FindControl("btnCanPagar") as LinkButton;
                if (Convert.ToString(status) == "A")
                    btn.CssClass = "btn btn-primary textoBold";
                else if (Convert.ToString(status) == "T")
                    btn.CssClass = "btn btn-info textoBold";
                else if (Convert.ToString(status) == "C")
                    btn.CssClass = "btn btn-default textoBold";
                else if (Convert.ToString(status) == "R")
                    btn.CssClass = "btn btn-success textoBold";
                else if (Convert.ToString(status) == "F")
                    btn.CssClass = "btn btn-warning textoBold ";
                else if (Convert.ToString(status) == "S")
                    btn.CssClass = "btn btn-danger textoBold ";


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
            decimal montoPagado = Convert.ToDecimal(argumentos[4]);
            int usuario = Convert.ToInt32(Request.QueryString["u"]);
            Operarios operario = new Operarios();
            operario.empresa = empresa;
            operario.taller = taller;
            operario.orden = orden;
            operario.empleado = empleado;            
            operario.usuario = usuario;
            operario.montoPagado = montoPagado;
            operario.pagado = 1;
            operario.actualizaPagado();
            object[] actualizado = operario.retorno;
            if (Convert.ToBoolean(actualizado[0]))
            {
                lblError.Text = "El operario a sido actualizado";
                cargaInfo();
                RadGrid1.DataBind();
            }
            else 
                lblError.Text = "Error al actualizar al operario: " + actualizado[1].ToString();
        }
        catch (Exception ex) {
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
            decimal montoPagado = Convert.ToDecimal(argumentos[4]);
            int usuario = Convert.ToInt32(Request.QueryString["u"]);
            Operarios operario = new Operarios();
            operario.empresa = empresa;
            operario.taller = taller;
            operario.orden = orden;
            operario.empleado = empleado;            
            operario.usuario = usuario;
            operario.montoPagado = 0;
            operario.pagado = 0;
            operario.actualizaPagado();
            object[] actualizado = operario.retorno;
            if (Convert.ToBoolean(actualizado[0]))
            {
                lblError.Text = "El operario a sido actualizado";
                cargaInfo();
                RadGrid1.DataBind();
            }
            else
                lblError.Text = "Error al actualizar al operario: " + actualizado[1].ToString();
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al colocar como no pagado al operario: " + ex.Message;
        }
    }
}