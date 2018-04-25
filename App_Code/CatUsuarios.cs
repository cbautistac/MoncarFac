using System;

using System.Data;

using E_Utilities;

/// <summary>
/// Descripción breve de CatUsuarios
/// </summary>
public class CatUsuarios
{
    Ejecuciones ejecuta = new Ejecuciones();
    
    Fechas fechas = new Fechas();
    public CatUsuarios()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public object[] existeUsuario(string nick)
    {
        string sql = "select count(*) from Usuarios where upper(nick)='" + nick.ToUpper() + "'";
        return ejecuta.scalarToBool(sql);
    }

    public int existenParametros()
    {
        int contados = 0;
        string sql = "select isnull(count(*),0) from parametros_email";
        object[] ejecutado = ejecuta.scalarToInt(sql);
        bool existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            contados = Convert.ToInt32(ejecutado[1]);
        else
            contados = 0;
        return contados;
    }

    public object[] tieneRelacion(int idUsuario)
    {
        string sql = "select sum(tabla.registros) as relaciones from(SELECT count(*) as registros FROM Usuarios_Perfiles WHERE id_usuario=" + idUsuario + " union all select count(*) as registros from Usuarios_Permisos where id_usuario=" + idUsuario + " union all select count(*) as registros from Usuarios_Taller where id_usuario=" + idUsuario + " union all select count(*) as registros from empleados where id_usuario=" + idUsuario + " union all select count(*) as registros from Bitacora_Orden_Localizacion where id_usuario=" + idUsuario + " union all select count(*) as registros from Bitacora_Orden_Comentarios where id_usuario=" + idUsuario + " union all select count(*) as registros from Bitacora_Orden_Avance where id_usuario=" + idUsuario + ") as tabla";
        return ejecuta.scalarToBool(sql);
    }

    public string obtieneContraseña(string usuario)
    {
        string sql = "select contrasena from usuarios where nick='" + usuario + "'";
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] existeCorreo(string usuario, string correo)
    {
        string sql = "select isnull(count(*),0) from usuarios where nick='" + usuario + "' and correo='" + correo + "'";
        return ejecuta.scalarToInt(sql);
    }

    public object[] validaUsuarioLog(string user, string pass)
    {
        string sql = "select count(*) from Usuarios where upper(nick)='" + user.ToUpper() + "' and contrasena = '" + pass + "'";
        return ejecuta.scalarToBoolLog(sql);
    }

    public object[] existeUsuarioLog(string nick)
    {
        string sql = "select count(*) from Usuarios where upper(nick)='" + nick.ToUpper() + "'";
        return ejecuta.scalarToBoolLog(sql);
    }

    public bool eliminaEmpresaTaller(string sEmpresa, string sTaller)
    {
        bool eliminado = false;
        string sql = "DELETE FROM Empresas_Taller where id_empresa=" + sEmpresa.Trim() + " and id_taller=" + sTaller.Trim();
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        try
        {
            eliminado = Convert.ToBoolean(ejecutado[0]);
            if (eliminado)
            {
                eliminado = Convert.ToBoolean(ejecutado[1]);
            }
        }
        catch (Exception)
        {
            return false;
        }
        return eliminado;
    }

    public DataSet obtieneUsuarios(string correo)
    {
        DataSet data = new DataSet();
        string sql = "select nick from usuarios where correo='" + correo + "'";
        object[] ejecutado = ejecuta.dataSet(sql);
        bool existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            data = (DataSet)ejecutado[1];
        return data;
    }

    public bool eliminaUsuarioTaller(string sEmpresa, string sTaller, string sUsuario)
    {
        bool eliminado = false;
        string sql = "delete from Usuarios_Taller where id_usuario=" + sUsuario.Trim() + " and id_empresa=" + sEmpresa.Trim() + " and id_taller=" + sTaller.Trim();
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        try
        {
            eliminado = Convert.ToBoolean(ejecutado[0]);
            if (eliminado)
            {
                eliminado = Convert.ToBoolean(ejecutado[1]);
            }
        }
        catch (Exception )
        {
            return false;
        }
        return eliminado;
    }

    public bool altaBajaUsuario(int idUsuario, char estatus)
    {
        string sql = "update Usuarios set estatus='" + estatus + "' where id_usuario=" + idUsuario.ToString();
        object[] ejecutado = ejecuta.scalarToBool(sql);
        bool existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            return true;
        else
            return false;
    }

    public object[] validaUsuario(string user, string pass)
    {
        string sql = "select count(*) from Usuarios where upper(nick)='" + user.ToUpper() + "' and contrasena = '" + pass + "'";
        return ejecuta.scalarToBool(sql);
    }

    public bool eliminaPerfilUsuarios(string sUsuario, string sPerfil)
    {
        bool eliminado = false;
        string sql = "DELETE FROM Usuarios_Perfiles where id_usuario=" + sUsuario + " and id_perfil=" + sPerfil;
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        try
        {
            eliminado = Convert.ToBoolean(ejecutado[0]);
            if (eliminado)
            {
                eliminado = Convert.ToBoolean(ejecutado[1]);
            }
        }
        catch (Exception )
        {
            return false;
        }
        return eliminado;
    }

    public char obtieneEstatusUser(int usuario)
    {
        char resultado = 'A';
        string sql = "select estatus from Usuarios where id_usuario=" + usuario.ToString();
        resultado = Convert.ToChar(ejecuta.scalarToStringSimple(sql));
        return resultado;
    }

    public object[] obtienePerfiles(int idUsuario)
    {
        string sql = "select up.id_perfil,p.nombre_perfil from Usuarios_Perfiles up inner join Perfiles p on p.id_perfil=up.id_perfil where up.id_usuario=" + idUsuario.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneIdUsuario(string usuario)
    {
        string sql = "select id_usuario from Usuarios where upper(nick)='" + usuario.ToUpper() + "'";
        return ejecuta.scalarToInt(sql);
    }

    public object[] obtieneTalleresUsuario(int idUsuario)
    {
        string sql = "select distinct ut.id_empresa, ut.id_taller,t.nombre_taller from Usuarios_Taller ut inner join Talleres t on t.id_taller=ut.id_taller where ut.id_usuario=" + idUsuario.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] obtieneEmpresasTallerUsuario(int idUsuario, int empresa)
    {
        string sql = "select distinct ut.id_empresa,e.razon_social from Usuarios_Taller ut inner join Empresas e on e.id_empresa=ut.id_empresa where ut.id_usuario=" + idUsuario.ToString();
        return ejecuta.dataSet(sql);
    }

    public int obtieneCumpleañeros()
    {
        int regreso = 0;
        /*
        string sql = "select isnull(count(*),0) from empleados where datepart(dd,fecha_de_nacimiento)=datepart(dd,getdate()) and datepart(mm,fecha_de_nacimiento)=datepart(mm,getdate())";
        object[] ejecutado = ejecutaRH.scalarToInt(sql);
        bool existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            regreso = Convert.ToInt32(ejecutado[1]);
        else
            regreso = 0;*/
        return regreso;
    }

    public bool actualizaEmpresaTaller(string sEmpresa, string sTaller, string topeEco, string topeRef)
    {
        bool eliminado = false;
        decimal topeEc = Convert.ToDecimal(topeEco);
        int topeRe = Convert.ToInt32(topeRef);

        string sql = "Update Empresas_Taller set tope_economico=" + topeEc.ToString() + ", tope_refacciones=" + topeRe.ToString() + " where id_empresa=" + sEmpresa.Trim() + " and id_taller=" + sTaller.Trim();
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        try
        {
            eliminado = Convert.ToBoolean(ejecutado[0]);
            if (eliminado)
            {
                eliminado = Convert.ToBoolean(ejecutado[1]);
            }
        }
        catch (Exception)
        {
            return false;
        }
        return eliminado;
    }

    public object[] obtieneDiasFechasEntrega(int idEmpresa, int idTaller)
    {
        string sql = "select count(distinct ref_no_orden) from Refacciones_Orden " +
                     "where ref_id_empresa =" + idEmpresa.ToString() + " and ref_id_taller =" + idTaller.ToString() + " and refestatus = 'AU' and reffechentregaEst='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'";
        return ejecuta.scalarToInt(sql);
    }

    public string obtieneEstatusAcceso(int idUsuario)
    {
        string sql = "select estatus_sistema from usuarios where id_usuario=" + idUsuario.ToString();
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] actualizaBitacoraAcceso(int idUsuario, string actIn)
    {
        string sql = "";
        if (actIn == "A")
            sql = "insert into bitacora_acceso(id_usuario,id_bit_acceso,entrada) values(" + idUsuario.ToString() + ",(" +
                "select isnull((select top 1 b.id_bit_acceso from bitacora_acceso b where b.id_usuario=" + idUsuario.ToString() + " order by b.id_bit_acceso desc),0)+1" +
                "),'" + Convert.ToDateTime(fechas.obtieneFechaLocal().ToString()).ToString("s") + "')";
        else if (actIn == "I")
            sql = "update bitacora_acceso set salida='" + Convert.ToDateTime(fechas.obtieneFechaLocal().ToString()).ToString("s") + "' where id_usuario=" + idUsuario.ToString() + " and id_bit_acceso=" +
               "(select isnull((select top 1 b.id_bit_acceso from bitacora_acceso b where b.id_usuario=" + idUsuario.ToString() + " order by b.id_bit_acceso desc),0))";
        sql += " update usuarios set estatus_sistema='" + actIn + "' where id_usuario=" + idUsuario.ToString();
        return ejecuta.insertUpdateDelete(sql);
    }

    public string obtieneNickName(string usuario)
    {
        string sql = "select nick from usuarios where id_usuario=" + usuario;
        return ejecuta.scalarToStringSimple(sql);
    }

    public object[] obtieneRefaccionesVencidas(int idEmpresa, int idTaller)
    {
        string sql = "select count(distinct ref_no_orden) from Refacciones_Orden " +
                     "where ref_id_empresa =" + idEmpresa.ToString() + " and ref_id_taller =" + idTaller.ToString() + " and refestatus = 'AU' and reffechentregaEst<'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'";
        return ejecuta.scalarToInt(sql);
    }

    public int obtieneTransitoProgramado(int idUsuario, int idEmpresa, int idTaller)
    {
        string sql = "select count(*) from seguimiento_orden where id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and no_orden = 160016 and f_retorno_transito = '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "'";
        return Convert.ToInt32(ejecuta.scalarToStringSimple(sql));
    }

    public object[] obtieneOrdenesPorEntregar(int idEmpresa, int idTaller)
    {
        string sql = "select " +
            "cast((select count(*) as entrega_hoy from seguimiento_orden " +
            "where f_entrega_estimada = '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' and no_orden in (" +
            "select no_orden from seguimiento_orden where id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and f_confirmacion = '1900-01-01' or f_confirmacion is null and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + ") " +
            "and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " or " +
            "f_confirmacion = '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' " +
            "and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + ")as varchar(10))+' Vehiculo(s) por entregar y ' + " +
            "cast((select count(*) as entrega_vencida from seguimiento_orden  " +
            "where f_entrega_estimada < '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' and f_entrega_estimada > '1900-01-01' and no_orden in (" +
            "select no_orden from seguimiento_orden where id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and f_confirmacion = '1900-01-01' or f_confirmacion is null and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + ") and no_orden not in (" +
            "select no_orden from seguimiento_orden where id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " and f_confirmacion> '1900-01-01') " +
            "and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + " or " +
            "f_confirmacion < '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' and f_confirmacion> '1900-01-01' " +
            "and id_empresa = " + idEmpresa.ToString() + " and id_taller = " + idTaller.ToString() + ")as varchar(10))+' Vehiculo(s) vencidos por entregar'";
        return ejecuta.scalarToString(sql);
    }
}