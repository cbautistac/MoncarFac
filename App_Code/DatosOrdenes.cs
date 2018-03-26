using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de DatosOrdenes
/// </summary>
public class DatosOrdenes
{
    Ejecuciones ejecuta = new Ejecuciones();
	public DatosOrdenes()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public bool agregaFotoDanos(int idEmpresa, int idTaller, int noOrden, string nombre, byte[] imagen, int proceso)
    {
        bool insertado = false;
        string sql = "insert into Fotografias_Orden values(" + idEmpresa.ToString() + "," + idTaller.ToString() + "," + noOrden.ToString() + ",(select isnull((select top 1 consecutivo from fotografias_orden where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " and proceso=" + proceso.ToString() + " order by consecutivo desc),0)+1)," + proceso.ToString() + ",'" + nombre + "','ruta',@imagen)";
        object[] ejecutado = ejecuta.insertAdjuntos(sql, imagen);
        try
        {
            bool fueInsertado = Convert.ToBoolean(ejecutado[0]);
            if (fueInsertado)
                insertado = true;
            else
                insertado = false;
        }
        catch (Exception x )
        {
            insertado = false;
        }
        return insertado;
    }

    public bool eliminarFotoDanos(int id_empresa, int id_taller, int no_orden, int consecutivo, int proceso)
    {
        bool eliminado = false;
        string sql = "delete from Fotografias_Orden where id_empresa=" + id_empresa.ToString() + " and id_taller=" + id_taller.ToString() + " and no_orden=" + no_orden.ToString() + " and consecutivo=" + consecutivo.ToString() + " and proceso=" + proceso.ToString();

        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        try
        {
            bool fueInsertado = Convert.ToBoolean(ejecutado[0]);
            if (fueInsertado)
                eliminado = true;
            else
                eliminado = false;
        }
        catch (Exception x )
        {
            eliminado = false;
        }
        return eliminado;
    }

    public bool obtieneEstatusLlamada(int noOrden,int idEmpresa,int idTaller)
    {
        string sql = "select count(*) from Llamadas_Orden " +
                     "where id_empresa = " + idEmpresa.ToString() + " and id_taller =" + idTaller.ToString() + " and no_orden =" + noOrden.ToString() + " and estatus != 'P' ";
        object[] ejecutado = ejecuta.scalarToInt(sql);
        try
        {
            bool existe = Convert.ToBoolean(ejecutado[0]);
            if (existe)
            {
                int atendido = Convert.ToInt32(ejecutado[1]);
                if (atendido > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public object[] obtieneInfoInventario(int empresa, int taller, int orden)
    {
        object[] retorno = new object[2] { false, false };
        try
        {
            string sql = "select ((case i.izq when 1 then 1 else 0 end) + (case i.der when 1 then 1 else 0 end) + (case i.fron when 1 then 1 else 0 end) + (case i.post when 1 then 1 else 0 end) + (case i.int when 1 then 1 else 0 end) + (case i.caj when 1 then 1 else 0 end) + (case i.gen when 1 then 1 else 0 end)) from inventario_vehiculo i where i.no_orden=" + orden.ToString() + " and i.id_empresa=" + empresa.ToString() + " and i.id_taller=" + taller.ToString();
            object[] inventario = ejecuta.scalarToInt(sql);
            if (Convert.ToBoolean(inventario[0]))
            {
                if (Convert.ToInt32(inventario[1]) < 7)
                {
                    retorno[0] = true;
                    retorno[1] = false;
                }
                else
                {
                    sql = "select count(*) from fotografias_orden where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " and proceso=1";
                    object[] fotos = ejecuta.scalarToInt(sql);
                    if (Convert.ToBoolean(fotos[0]))
                    {
                        if (Convert.ToInt32(fotos[1]) > 4)
                        {
                            retorno[0] = true;
                            retorno[1] = true;
                        }
                        else
                        {
                            retorno[0] = true;
                            retorno[1] = false;
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            retorno[0] = false;
            retorno[1] = false;
        }

        return retorno;
        
    }

    public object[] obtieneFotos(int empresa, int taller, int orden, int proceso)
    {
        string sql = "select nombre_imagen,imagen from Fotografias_Orden where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " and proceso=" + proceso.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneFotosBienvenida(int empresa, int taller, int orden, int proceso)
    {
        string sql = "select top 10 nombre_imagen,imagen from Fotografias_Orden where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString() + " and no_orden=" + orden.ToString() + " and proceso=" + proceso.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] agregaRegistroPintura(int[] sesiones, DateTime solicitud, DateTime recepcion, string operario, string recibe, string entrega, string observacion, string detalle)
    {
        string orden = sesiones[4].ToString() + "-RP";
        string sql = "declare @orden int " +//meter campo nuevo
"SET @orden = (select isnull((SELECT TOP 1  id_solicitud FROM Registro_Pinturas WHERE ANO=" + solicitud.Year.ToString() + " ORDER BY id_solicitud DESC),0)+1) " +
"insert into Registro_Pinturas (ano,id_solicitud,folio_solicitud,id_empresa,id_taller,no_orden,fecha_solicitud,hora_solicitud,fecha_recepcion,hora_recepcion,dias_igualacion,recibe,entrega_muestra,id_usuario_registro,idEmp,desc_solicitud,detalle) " +
"values(" + solicitud.Year.ToString() + ",@orden,'" + orden + "'+CAST(@orden AS CHAR(10))," + sesiones[2].ToString() + "," + sesiones[3].ToString() + "," + sesiones[4].ToString() + ",'" + solicitud.ToString("yyyy-MM-dd") + "','" + solicitud.ToString("HH:mm:ss") + "','" + recepcion.ToString("yyyy-MM-dd") + "','" + recepcion.ToString("HH:mm:ss") + "',0,'" + recibe + "','" + entrega + "'," + sesiones[0].ToString() + "," + operario + ",'" + observacion + "','" + detalle + "')";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaOrdenPintura(string ano, string orden, DateTime igualacion, DateTime termiando, DateTime entregado, int diasIgualacion, string recibe, string entrega, string operario,string ticket)
    {
        string sql = "update registro_pinturas set fecha_igualacion='" + igualacion.ToString("yyyy-MM-dd") + "', fecha_terminado='" + termiando.ToString("yyyy-MM-dd") + "', hora_terminado='" + termiando.ToString("HH:mm:ss") + "', fecha_entrega='" + entregado.ToString("yyyy-MM-dd") + "', hora_entrega='" + entregado.ToString("HH:mm:ss") + "', dias_igualacion=" + diasIgualacion.ToString() + ", entrega_real='" + recibe + "', id_usuario_entrega='" + entrega + "',estatus='T',idemp=" + operario + ",ticket=" + ticket + ",total=(select total+iva from venta_enc where ticket in (select ticket from registro_pinturas where id_solicitud=" + orden + " and ano=" + ano + ")) WHERE ano=" + ano + " and id_solicitud=" + orden;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaEstatusOrdenPintura(string[] argumentos, string estatus)
    {
        string sql = "update registro_pinturas set estatus='" + estatus + "' where ano=" + argumentos[0] + " and id_solicitud=" + argumentos[1];
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] agregaDetallePintura(string ano, string orden, string cantidad, string descripcion, string unidad, decimal importe)
    {
        string sql = "insert into Detalle_Registro_Pintura values(" + ano + "," + orden + ",(select isnull((select top 1 id_detalle from Detalle_Registro_Pintura where ano=" + ano + " and id_solicitud=" + orden + " order by id_detalle desc),0)+1),'" + cantidad + "','" + unidad + "','" + descripcion + "'," + importe.ToString() + ")";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] eliminarDetallePintura(string ano, string orden, string id)
    {
        string sql = "delete from detalle_registro_pintura where ano=" + ano + " and id_solicitud=" + orden + " and id_detalle=" + id;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] actualizaMontoRegPintura(string ano, string orden, decimal importeTotal)
    {
        string sql = "update registro_pinturas set total=" + importeTotal.ToString() + " where ano=" + ano + " and id_solicitud=" + orden;
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneInfoSolicitudPintura(string ano, string orden)
    {
        string sql = "select convert(char(10),fecha_igualacion,126),convert(char(10),fecha_terminado,126),convert(char(8),hora_terminado,108),convert(char(10),fecha_entrega,126),convert(char(8),hora_entrega,108),entrega_real,id_usuario_entrega,idemp,isnull(ticket,0) as ticket from registro_pinturas where ano=" + ano + " and id_solicitud=" + orden;
        return ejecuta.dataSet(sql);
    }

    public bool agregaFotoRefacciones(int idEmpresa, int idTaller, int noOrden, string nombre, byte[] imagen, string id)
    {
        bool insertado = false;
        string sql = "insert into Fotografias_Orden values(" + idEmpresa.ToString() + "," + idTaller.ToString() + "," + noOrden.ToString() + ",(select isnull((select top 1 consecutivo from fotografias_orden where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " and proceso=5 order by consecutivo desc),0)+1),5,'" + nombre + "','ruta',@imagen) "+
            "insert into fotos_refacciones values(" + noOrden.ToString() + "," + idEmpresa.ToString() + "," + idTaller.ToString() + "," + id + ",(select isnull((select top 1 id_fotografia from Fotos_refacciones where no_orden=" + noOrden.ToString() + " and id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and id_refaccion=" + id + " order by id_fotografia desc),0)+1),'" + nombre + "','ruta',@imagen)";
        object[] ejecutado = ejecuta.insertAdjuntos(sql, imagen);
        try
        {
            bool fueInsertado = Convert.ToBoolean(ejecutado[0]);
            if (fueInsertado)
                insertado = true;
            else
                insertado = false;
        }
        catch (Exception x)
        {
            insertado = false;
        }
        return insertado;
    }

    public object[] eliminaFotoRefaccion(string[] imagenURL)
    {
        string sql = "delete from fotos_refacciones where no_orden =" + imagenURL[2] + " and id_empresa =" + imagenURL[0] + " and id_taller =" + imagenURL[1] + " and id_refaccion =" + imagenURL[3] + " and id_fotografia =" + imagenURL[4];
        return ejecuta.insertUpdateDelete(sql);
    }
}