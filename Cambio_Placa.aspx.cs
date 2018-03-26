using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cambio_Placa : System.Web.UI.Page
{
    DatosUtilerias datosUti = new DatosUtilerias();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            txtNoOrden.Text = "";
            txtPlaca.Text = "";
            lblError.Text = "";
            obtieneSessiones();
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblError.CssClass = "errores";
        if (txtPlaca.Text.Trim() != "")
        {
            int noOrden = Convert.ToInt32(txtNoOrden.Text);
            int[] sesiones = obtieneSessiones();
            int idEmpresa = sesiones[0];
            int idTaller = sesiones[1];
            bool existe = datosUti.existePlacaVehiculo(txtPlaca.Text.Trim().ToUpper());
            if (existe)
            {
                lblError.Text = "Ya existe un vehículo relacionado a la placa que digitó. Verifique e intente de nuevo.";
            }
            else {
                int totalOrdenes = datosUti.obtieneOrdenesRelacionas(idEmpresa, idTaller, lblPlacaVieja.Text);
                if (totalOrdenes == 1) { 
                    Recepciones recepcion = new Recepciones();
                    object[] datosVehi = recepcion.obtieneInfoVehiculo(lblPlacaVieja.Text);
                    if (Convert.ToBoolean(datosVehi[0]))
                    {
                        DataSet Dv = (DataSet)datosVehi[1];
                        int marca = 0, vehiculo = 0, unidad = 0, id = 0;
                        foreach (DataRow fila in Dv.Tables[0].Rows) {
                            marca = Convert.ToInt32(fila[0].ToString());
                            vehiculo = Convert.ToInt32(fila[1].ToString());
                            unidad = Convert.ToInt32(fila[2].ToString());
                            id = Convert.ToInt32(fila[3].ToString());
                        }
                        bool actualizado = datosUti.actualizaPlacaVehiculo(marca, vehiculo, unidad, id, txtPlaca.Text.Trim().ToUpper());
                        if (actualizado)
                        {
                            bool actualizados = datosUti.actualizaPlaca(noOrden, idEmpresa, idTaller, txtPlaca.Text);
                            if (actualizados)
                            {
                                txtPlaca.Visible = false;
                                btnGuardar.Visible = false;
                                txtNoOrden.Text = "";
                                txtPlaca.Text = "";
                                lblPlacaVieja.Text = "";
                                lblError.CssClass = "text-success textoBold";
                                lblError.Text = "La placa en la orden se actualizó exitósamente";
                            }                                
                            else
                                lblError.Text = "La placa " + txtPlaca.Text + " solo se actualizó en el vehículo; No se logró actualizar correctamente en la orden indicada, intentelo nuevamente";
                            
                        }
                        else
                            lblError.Text = "No se pudo actualizar la orden, intente de nuevo";

                    }
                    else {
                        lblError.Text = "No se encontró el vehículo de la orden indicada";
                    }

                }
                else if (totalOrdenes == 0)
                {
                    lblError.Text = "No es posible actualizar la información solicitada ya que no existe órdenes relacionas.";
                }
                else {
                    lblError.Text = "No se puede actualizar la placa ya que se encuentra relacionada con otras ordenes";
                }


                
            }

            
        }
        else
            lblError.Text = "Necesita colocar la placa";
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (txtNoOrden.Text.Trim() != "")
        {
            int noOrden = 0;
            try
            {
                noOrden = Convert.ToInt32(txtNoOrden.Text);
                int[] sesiones = obtieneSessiones();
                int idEmpresa = sesiones[0];
                int idTaller = sesiones[1];
                bool existe = datosUti.existePlaca(noOrden, idEmpresa, idTaller);
                if (existe)
                {
                    string placaVieja = datosUti.obtienePlaca(noOrden, idEmpresa, idTaller);
                    lblPlacaVieja.Text = placaVieja.Trim();
                    txtPlaca.Text = placaVieja.Trim();
                    txtPlaca.Visible = true;
                    btnGuardar.Visible = true;
                }
                else
                {
                    lblError.Text = "El No. de Orden no existe ";
                    txtPlaca.Visible = false;
                    btnGuardar.Visible = false;
                    txtNoOrden.Text = "";
                    txtPlaca.Text = "";

                }
            }
            catch (Exception)
            {
                lblError.Text = "El No. de Orden esta formado solo de numeros";
                txtPlaca.Visible = false;
                btnGuardar.Visible = false;
                txtNoOrden.Text = "";
                txtPlaca.Text = "";
            }
        }
        else
        {
            lblError.Text = "Necesita colocar el No. de la Orden";
            txtPlaca.Visible = false;
            btnGuardar.Visible = false;
            txtNoOrden.Text = "";
            txtPlaca.Text = "";
        }
    }

    private int[] obtieneSessiones()
    {
        int[] sesiones = new int[2];
        try
        {
            sesiones[0] = Convert.ToInt32(Session["empresa"]);
            sesiones[1]= Convert.ToInt32(Session["taller"]);
        }
        catch (Exception ex)
        {
            txtPlaca.Visible = false;
            btnGuardar.Visible = false;
            txtNoOrden.Text = "";
            txtPlaca.Text = "";
            sesiones[0] = 0;
            sesiones[1] = 0;
            Session["paginaOrigen"] = "Cambio_Placa.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }
}