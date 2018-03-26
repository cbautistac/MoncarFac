using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de BitacorasComentarios
/// </summary>
public class BitacorasComentarios
{
    int _empresa;
    int _taller;
    int _orden;
    int _usuario;
    string _valor;
    int _bitacora;
    object[] _afectado;

    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
	public BitacorasComentarios()
	{
        _empresa = _taller = _orden = _usuario = _bitacora = 0;
        _valor = string.Empty;
        _afectado=new object[2];
	}

    public int Empresa { set { _empresa = value; } }
    public int Taller { set { _taller = value; } }
    public int Orden { set { _orden = value; } }
    public int Usuario { set { _usuario = value; } }
    public int Bitacora { set { _bitacora = value; } }
    public string Valor { set { _valor = value; } }
    public object[] Afectado { set { _afectado = value; } get { return _afectado; } }


    public void agregaRegistro() {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();

        string sql = "";
        switch (_bitacora) { 
            case 1:
                // Bitacora Avances
                sql = string.Format( "insert into bitacora_orden_avance values({2},{0},{1},(select isnull((select top 1 id_avance from bitacora_orden_avance where id_empresa={0} and id_taller={1} and no_orden={2} order by id_avance desc),0)+1),{3},'{4}',{5})",_empresa,_taller,_orden,Convert.ToDecimal(_valor),fechaRetorno,_usuario);
                break;
            case 2:
                // Bitacora Localizaciones
                sql = string.Format("insert into bitacora_orden_localizacion values({2},{0},{1},(select isnull((select top 1 id_registro from bitacora_orden_localizacion where id_empresa={0} and id_taller={1} and no_orden={2} order by id_registro desc),0)+1),{3},'{4}','{5}',{6}) " +
                    "update Ordenes_Reparacion set id_localizacion={3} where id_empresa={0} and id_taller={1} and no_orden={2}", _empresa, _taller, _orden, Convert.ToInt32(_valor), fechaRetorno, horaRetorno, _usuario);
                break;
            case 3:
                // Bitacora Comentarios
                sql = string.Format("insert into bitacora_orden_comentarios values({2},{0},{1},(select isnull((select top 1 id_observacion from bitacora_orden_comentarios where id_empresa={0} and id_taller={1} and no_orden={2} order by id_observacion desc),0)+1),'{3}','{4}','{5}',{6})", _empresa, _taller, _orden, _valor, fechaRetorno, horaRetorno, _usuario);
                break;
            case 4:
                //Bitacora de perfiles
                sql = string.Format("insert into bitacora_orden_perfiles values({2},{0},{1},(select isnull((select top 1 id_registro from bitacora_orden_perfiles where id_empresa={0} and id_taller={1} and no_orden={2} order by id_registro desc),0)+1),{3},'{4}','{5}',{6}) "+
                    "update Ordenes_Reparacion set id_perfilOrden={3} where id_empresa={0} and id_taller={1} and no_orden={2}" , _empresa, _taller, _orden, Convert.ToInt32(_valor), fechaRetorno, horaRetorno, _usuario);
                break;
            default:
                
                break;
        }

        _afectado = ejecuta.insertUpdateDelete(sql);
        /*if (Convert.ToInt32(_valor) == 100)
            actualizaFechaTermino();*/
    }

    private void actualizaFechaTermino()
    {
        Cronos datosCronos = new Cronos();
        datosCronos.obtieneIncompletos(_orden, _empresa, _taller);
        object[] actualizado = datosCronos.Actualizacion;
        if ((bool)actualizado[0])
        {
            int incompletos = Convert.ToInt32(actualizado[1]);
            if (incompletos == 0)
            {
                datosCronos.existeFechaTermino(_orden.ToString(), _empresa.ToString(), _taller.ToString());
                actualizado = datosCronos.Actualizacion;
                if ((bool)actualizado[0])
                {
                    int existeTermino = Convert.ToInt32(actualizado[1]);
                    if (existeTermino == 0)
                    {
                        datosCronos.actualizaTerminado(_orden.ToString(), _empresa.ToString(), _taller.ToString(), _usuario.ToString());
                        actualizado = datosCronos.Actualizacion;
                    }
                }
            }
        }
    }

    public bool actualizaRetornoTransitoProg()
    {
        DateTime fechaProgRetTrans = fechas.obtieneFechaLocal();
        fechaProgRetTrans = fechaProgRetTrans.AddDays(8);
        string sql = "update seguimiento_orden set f_prog_retorno_tran='" + fechaProgRetTrans.ToString("yyyy-MM-dd") + "' ,h_prog_retorno_tran='"+ fechaProgRetTrans.ToString("HH:mm:ss") + "' " +
            "where no_orden =" + _orden.ToString() + " and id_empresa =" + _empresa.ToString() + " and id_taller =" + _taller.ToString();
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            if ((bool)ejecutado[1])
                return true;
            else
                return false;
        else
            return false;
    }
}