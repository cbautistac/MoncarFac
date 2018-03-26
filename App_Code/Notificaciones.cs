using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Notificaciones
/// </summary>
public class Notificaciones
{

    DateTime _fecha;
    Fechas fechas = new Fechas();
    string _usuario;
    string _origen;
    int _clasificacion;
    string _notificacion;
    int _dias;
    object[] _retorno;
    string _estatus;
    int _punto;
    string _articulo;
    int _entrada;
    int _caja;
    string _extra;
    int _empresa;
    int _taller;
    
	public Notificaciones()
	{
        _usuario = _notificacion =_origen = _estatus = _articulo =_extra = string.Empty;
        _clasificacion = _punto = _caja = _entrada = _empresa = _taller = 0;
        _fecha = fechas.obtieneFechaLocal();
        _retorno = new object[2];
	}

    public int Dias { set { _dias = value; } }
    public DateTime Fecha { set {_fecha=value;} }
    public string Usuario { set {_usuario=value;} }
    public string Articulo { set { _articulo = value; } }// Valor para la orden
    public string Origen { set { _origen = value; } }
    public int Clasificacion { set { _clasificacion = value; } }
    public int Punto { set { _punto = value; } }
    public int Caja { set { _caja = value; } }
    public int Entrada { set { _entrada = value; } }
    public int Empresa { set { _empresa = value; } }
    public int Taller { set { _taller = value; } }
    public object[] Retorno { get { return _retorno; } }
    public string Estatus { set { _estatus = value; } }
    public string Extra { set { _extra = value; } }// valor para datos extras


    public void agregaNotificacion() {
        Ejecuciones data = new Ejecuciones();
        object[] datos = new object[2];
        string sql = string.Format("insert into dbo.Notificaciones values({0},{1},(select isnull((select top 1 id_notificacion from notificaciones where id_empresa={0} and id_taller={1} and fecha='{2}' order by id_notificacion desc),0)+1),'{2}','{3}',{4},'{5}',{6},'{7}','{8}')", _empresa, _taller, _fecha.ToString("yyyy-MM-dd"), _fecha.ToString("HH:mm:ss"), _usuario, _origen, _clasificacion, _notificacion, _estatus);
        _retorno = data.insertUpdateDelete(sql);
    }


    public void armaNotificacion() {
        Recepciones data = new Recepciones();
        string nombreTaller = data.obtieneNombreTaller(_taller.ToString());
               
        switch (_clasificacion) { 
            case 1:
                //Recepcion                
                _notificacion = "La Nueva Orden " + _articulo + " esta lista para presupuesto";
                break;
            case 2:
                //Levantamiento Daños (asignacion)                
                _notificacion = "La Orden " + _articulo + " esta lista para iniciar asignación de operarios";
                break;
            case 3:
                //Levantamiento Daños (asignacion)                
                _notificacion = "La Orden " + _articulo + " esta lista para cotizar refacciones";
                break;
            case 4:
                //Asignacion Realizada
                _notificacion = "La Orden " + _articulo + " ya cuenta con los operarios asignados";
                break;
            case 5:
                //Cotizacion Realizada
                _notificacion = "La Orden " + _articulo + " ya cuenta con cotización realizada";
                break;
            case 6:
                //AVISO DE LOCALIZACION ORDEN (TERMINADO, PAGO DAÑOS, PERDIDA TOTAL)
                _notificacion = "La Orden " + _articulo + " se encuentra en localización " + _extra;
                break;
            case 7:
                //AVISO DE SALIDA (PAGO DAÑOS, PERDIDA TOTAL)
                _notificacion = "El vehículo de la orden " + _articulo + " ha salido por " + _extra;
                break;      
            case 8:
                // Aviso de finalizacion de asignacion
                _notificacion = "La Orden " + _articulo + " ha finalizado la asignación de operarios";
                break;
            case 9:
                // Aviso de calidad
                _notificacion = "La Orden " + _articulo + " ha finalizado la operación de " + _extra + " por favor proceda a su inspección de calidad";
                break;
            case 10:
                // Aviso de inconformidad
                _notificacion = "La Orden " + _articulo + " presenta una inconformidad: " + _extra + ", por favor de seguimiento a la misma.";
                break;
            case 11:
                // Aviso de entrega refacción hoy
                _notificacion = "Cuenta con entrega de refacciones para: " + _extra + " orden(es) el dia de hoy, por favor de seguimiento a la misma.";
                break;
            case 12:
                // Aviso de entrega refacción hoy
                _notificacion = "Cuenta con: " + _extra + " orden(es) con refacciones pendientes de entrega a la fecha promesa del proveedor, por favor de seguimiento a la misma.";
                break;
            case 13:
                // retorno de transito recibido hoy
                _notificacion = "La Orden: " + _extra + " cuenta con retorno de tránsito,esta listo para asignación.";
                break;
            case 14:
                // retorno de transito para el dia de hoy
                _notificacion = "Cuenta con: " + _extra + " vehículo(s) con fecha programada para retorno de tránsito para el dia de hoy.";
                break;
            case 15:
                // refaccion cancelada en valuacion
                _notificacion = "La refacción: " + _extra + " fue cancelada por el usuario" + _usuario + ", el dia " + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + " a las " + fechas.obtieneFechaLocal().ToString("HH:mm:dd") + ".";
                break;
            case 16:
                // vehiculos por entregar y vencidos
                _notificacion = "Cuenta con: " + _extra + " al dia de hoy.";
                break;
            case 17:
                {
                    // Bitacora de llamadas pendientes
                    this.obtPrefijoTaller();
                    _notificacion = "La orden " + _retorno[1] + _articulo + " tiene una llamada \"" + _extra + "\" sin atender.";
                    break;
                }
            case 18:
                // vehiculos por entregar y vencidos
                _notificacion = "La Orden: " + _articulo + " tiene la primer llamada pendiente.";
                break;
            case 19:
                // vehiculos por entregar y vencidos
                _notificacion = "La refaccion: " + _articulo + " le quedan " + _dias + " dias para ser entregada. Orden Compra: " + _extra;
                break;
            default:
                _notificacion = "";
                break;
        }
    }


    public void actualizaEstado() {
        Ejecuciones data = new Ejecuciones();
        object[] datos = new object[2];
        string sql = string.Format("update notificaciones set estatus='{0}' where fecha='{1}' and id_notificacion={2} and id_empresa={3} and id_taller={4}", _estatus, _fecha.ToString("yyyy-MM-dd"), _entrada, _empresa, _taller);
        _retorno = data.insertUpdateDelete(sql);
    }

    public void obtieneNotificacionesPendientes() {
        Ejecuciones data = new Ejecuciones();
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from notificaciones where fecha='{1}' and estatus='{0}' and id_empresa={2} and id_taller={3}", _estatus, _fecha.ToString("yyyy-MM-dd"), _empresa, _taller);
        _retorno = data.scalarToInt(sql);
    }

    private void obtPrefijoTaller()
    {
        Ejecuciones eje = new Ejecuciones();
        object[] datos = new object[2];
        string sqlObtPref = "SELECT identificador FROM Talleres WHERE id_taller=" + _taller;
        _retorno = eje.scalarToString(sqlObtPref);
    }
    public void existeNotificacion()
    {
        Ejecuciones data = new Ejecuciones();
        object[] datos = new object[2];
        string sql = string.Format("select Count(*) from notificaciones where id_empresa=" + _empresa + " and id_taller=" + _taller + " and notificacion='"+ _notificacion + "' and estatus='P'");
        _retorno = data.scalarToInt(sql);
    }
}