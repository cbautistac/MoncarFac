using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de AlUsuarios
/// </summary>
public class AlUsuarios
{
    public string nombre { get; set; }
    public string nick { get; set; }
    public string contraseña { get; set; }
    public int id_usuario { get; set; }
    private string sql;
    public object[] retorno;
    public int puestirri { get; set; }
    public int id_empresa { get; set; }

    Ejecuciones ejecuta = new Ejecuciones();



    public AlUsuarios()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
    //agregar a un usuario 
    public void Agrega_Usuario()
    {
        sql = "insert into usuarios values ((select isnull((select top 1 id_usuario from usuarios order by id_usuario desc),0)+1),'" + nick + "','" + contraseña + "','" + nombre + "','A','A')";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void obtieneUsuarioaEditar()
    {
        sql = "select * from usuarios where id_usuario=" + id_usuario ;
        retorno = ejecuta.dataSet(sql);
    }
    public void eliminarUsuario()
    {
        sql = "delete from usuarios where id_usuario=" + id_usuario ;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void actualizaUsuario()
    {
        sql = "update usuarios set nick='" + nick + "' , contrasena='" + contraseña + "', nombre_usuario='" + nombre + "' where id_usuario=" + id_usuario;
        retorno = ejecuta.insertUpdateDelete(sql);
    }


}