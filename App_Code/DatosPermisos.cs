using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de DatosPermisos
/// </summary>
public class DatosPermisos
{
    Ejecuciones ejecuta = new Ejecuciones();
    string sql;
    object[] resultado = new object[2];

    public DatosPermisos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public object[] obtienePermisosUsuario(int idUsuario)
    {
        sql = "select id_permiso from usuarios_permisos where id_usuario=" + idUsuario.ToString();
        return ejecuta.dataSet(sql);
    }

    public object[] asignaPermiso(int idUsuario, int idPermiso)
    {
        sql = "insert into usuarios_permisos values (" + idUsuario.ToString() + "," + idPermiso.ToString() + ")";
        return ejecuta.insertUpdateDelete(sql);
    }

    public object[] desasignaPermiso(int idUsuario, int idPermiso)
    {
        sql = "delete from usuarios_permisos  where id_permiso=" + idPermiso.ToString() + " and id_usuario=" + idUsuario.ToString();
        return ejecuta.insertUpdateDelete(sql);
    }
}