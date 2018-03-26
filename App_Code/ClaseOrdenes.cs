using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ClaseOrdenes
/// </summary>
public class ClaseOrdenes
{
    public ClaseOrdenes()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public static void actualizaManoObra(int idEmpresa, int idTaller, int noOrden, int consecutivo, int idGO, int idOp, int idRef)
    {
        string sql = "UPDATE Mano_Obra set id_grupo_op=, id_operacion=, id_refaccion=, monto_mo=@monto_mo WHERE id_empresa=@id_empresa and id_taller=@id_taller and no_orden=@no_orden and consecutivo=@consecutivo";
        Ejecuciones ejecuta = new Ejecuciones();
        ejecuta.insertUpdateDelete(sql);
    }
}