using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatEmpresas
/// </summary>
public class CatEmpresas
{
    Ejecuciones ejecuta = new Ejecuciones();
	public CatEmpresas()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public object[] tieneRelacion(int idEmpresas)
    {
        string sql = "select sum(tabla.registros) as relaciones "+
                     "from(" +
                     "SELECT count(*) as registros FROM Usuarios_Taller WHERE id_taller = " + idEmpresas + " " +
                     "union all select count(*) as registros from empresas_taller where id_taller = " + idEmpresas + " " +
                     "union all select count(*) as registros from Bitacora_Orden_Localizacion where id_taller = " + idEmpresas + " " +
                     "union all select count(*) as registros from Bitacora_Orden_Comentarios where id_taller = " + idEmpresas + " " +
                     "union all select count(*) as registros from Bitacora_Orden_Avance where id_taller = " + idEmpresas + " " +
                     ") as tabla";
        return ejecuta.scalarToBool(sql);
    }
    public object[] tieneRelacionEmpresasTaller(int empresa, int taller)
    {
        string sql = "select COUNT(*) as registros from Ordenes_Reparacion where id_empresa=" + empresa.ToString() + " and id_taller=" + taller.ToString();
        return ejecuta.scalarToBool(sql);
    }
}