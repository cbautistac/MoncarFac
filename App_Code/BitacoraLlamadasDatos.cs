using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de BitacoraLlamadasDatos
/// </summary>
public class BitacoraLlamadasDatos
{
    Ejecuciones ejecuta = new Ejecuciones();
    Fechas fechas = new Fechas();
    string sql;
    object[] resultado = new object[2];

    public BitacoraLlamadasDatos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool insertaLlamada(int noOrden, int idEmpresa, int idTaller, string tipoLlamada, string fechaLlamada, string horaLlamada, string usuarioLlamada, string observaciones, string comentarioCliente, string clienteLlamo, string contesto, string atendio, string responsable, int respuestaLlamadaConsecutivo, int idUsuarioRegistro, int atendida, string quienAtendio, DateTime fechaAtencion)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string horaRetorno = fechas.obtieneFechaConFormato();

        if (atendida == 1)
            sql = "insert into llamadas_orden " +
                  "(no_orden, id_empresa, id_taller, consecutivo, tipo_llamada, fecha_llamada, hora, usuario_llamada," +
                  "estatus, observaciones, comentarios_cliente, cliente_llamo, contesto, atendio, responsable," +
                  "respuesta_llamada_consecutivo, usuario_registro, fecha_registro, hora_registro,atendida,quienAtendio,fechaAtendio,horaAtendio) " +
                  "values(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + "," +
                  "(select isnull((select top 1 llo.consecutivo from llamadas_orden llo where llo.no_orden = " + noOrden.ToString() + " and llo.id_empresa = " + idEmpresa.ToString() + " and llo.id_taller = " + idTaller.ToString() + " order by llo.consecutivo desc), 0) + 1)," +
                  "'" + tipoLlamada + "','" + fechaLlamada + "','" + horaLlamada + "','" + usuarioLlamada + "','A','" + observaciones + "','" + comentarioCliente + "'," +
                  "'" + clienteLlamo + "','" + contesto + "','" + atendio + "','" + responsable + "'," + respuestaLlamadaConsecutivo.ToString() + "," +
                  idUsuarioRegistro.ToString() + ",'" + fechaRetorno + "','" + horaRetorno + "'," + atendida + ",'" + quienAtendio + "','" + fechaAtencion.ToString("yyyy-MM-dd") + "','" + fechaAtencion.ToString("HH:mm:ss") + "')";
        else
            sql = "insert into llamadas_orden " +
              "(no_orden, id_empresa, id_taller, consecutivo, tipo_llamada, fecha_llamada, hora, usuario_llamada," +
              "estatus, observaciones, comentarios_cliente, cliente_llamo, contesto, atendio, responsable," +
              "respuesta_llamada_consecutivo, usuario_registro, fecha_registro, hora_registro) " +
              "values(" + noOrden.ToString() + ", " + idEmpresa.ToString() + ", " + idTaller.ToString() + "," +
              "(select isnull((select top 1 llo.consecutivo from llamadas_orden llo where llo.no_orden = " + noOrden.ToString() + " and llo.id_empresa = " + idEmpresa.ToString() + " and llo.id_taller = " + idTaller.ToString() + " order by llo.consecutivo desc), 0) + 1)," +
              "'" + tipoLlamada + "','" + fechaLlamada + "','" + horaLlamada + "','" + usuarioLlamada + "','A','" + observaciones + "','" + comentarioCliente + "'," +
              "'" + clienteLlamo + "','" + contesto + "','" + atendio + "','" + responsable + "'," + respuestaLlamadaConsecutivo.ToString() + "," +
              idUsuarioRegistro.ToString() + ",'" + fechaRetorno + "','" + horaRetorno + "')";

        if (respuestaLlamadaConsecutivo != 0)
            sql = sql + " update llamadas_orden set atendida=1, quienatendio='" + quienAtendio + "', fechaatendio='" + fechaAtencion.ToString("yyyy-MM-dd") + "',horaatendio='" + fechaAtencion.ToString("HH:mm:ss") + "' where id_empresa=" + idEmpresa.ToString() +
                " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " and consecutivo" + respuestaLlamadaConsecutivo.ToString() + " if(" + respuestaLlamadaConsecutivo.ToString() + "=1) begin  update seguimiento_orden set f_primer_llamada='" + fechaAtencion.ToString("yyyy-MM-dd") + "' where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " end";
    

        resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }

    public bool actualizaLlamada(int noOrden, int idEmpresa, int idTaller, string tipoLlamada, string fechaLlamada, string horaLlamada, string usuarioLlamada, string observaciones, string comentarioCliente, string llamo, string contesto, string atendio, string responsable, int respuestaLlamadaConsecutivo, int idUsuarioRegistro, string consecutivo, int atendida,string quienAtendio, DateTime fechaAtencion)
    {
        fechas.fecha = fechas.obtieneFechaLocal();
        fechas.tipoFormato = 4;
        string fechaRetorno = fechas.obtieneFechaConFormato();
        fechas.tipoFormato = 6;
        string agregado = "";
        if (atendida == 1)
            agregado = ", fechaAtendio='" + fechaAtencion.ToString("yyyy-MM-dd") + "', horaAtendio='" + fechaAtencion.ToString("HH:mm:ss") + "' ";
        string cronos = "";
        if (consecutivo == "1")
            cronos = " update seguimiento_orden set f_primer_llamada='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString();

        string horaRetorno = fechas.obtieneFechaConFormato();
        sql = "update llamadas_orden set tipo_llamada='" + tipoLlamada + "', fecha_llamada='" + fechaLlamada + "', hora='" + horaLlamada + "', usuario_llamada='" + usuarioLlamada + "'," +
              "estatus='A', observaciones='" + observaciones + "', comentarios_cliente='" + comentarioCliente + "', cliente_llamo='" + llamo + "', contesto='" + contesto + "', atendio='" + atendio + "', responsable='" + responsable + "'," +
              "respuesta_llamada_consecutivo=" + respuestaLlamadaConsecutivo.ToString() + ", usuario_registro=" + idUsuarioRegistro.ToString() + ", fecha_registro='" + fechaRetorno + "', hora_registro= '" + horaRetorno + "', atendida=" + atendida + ", quienAtendio='" + quienAtendio + "' " +
              agregado + " where no_orden = " + noOrden.ToString() + " and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and consecutivo=" + consecutivo + cronos;
        if (respuestaLlamadaConsecutivo != 0)
            sql = sql + " update llamadas_orden set atendida=1, quienatendio='" + quienAtendio + "', fechaatendio='" + fechaAtencion.ToString("yyyy-MM-dd") + "',horaatendio='" + fechaAtencion.ToString("HH:mm:ss") + "' where id_empresa=" + idEmpresa.ToString() +
                " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " and consecutivo" + respuestaLlamadaConsecutivo.ToString() + " if(" + respuestaLlamadaConsecutivo.ToString() + "=1) begin  update seguimiento_orden set f_primer_llamada='" + fechaAtencion.ToString("yyyy-MM-dd") + "' where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " end";


        resultado = ejecuta.insertUpdateDelete(sql);
        if ((bool)resultado[0])
            return Convert.ToBoolean(resultado[1]);
        else
            return false;
    }
}