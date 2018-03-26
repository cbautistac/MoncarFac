using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de AvancesOperacion
/// </summary>
public class AvancesOperacion
{

    public int id { get; set; }
    public string grupo { get; set; }
    public bool chk25 { get; set; }
    public bool chk50 { get; set; }
    public bool chk75 { get; set; }
    public bool chk100 { get; set; }
    public bool chkVoBo { get; set; }
    public int empresa { get; set; }
    public int taller { get; set; }
    public int orden { get; set; }

    Ejecuciones ejecuta = new Ejecuciones();

	public AvancesOperacion()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public List<AvancesOperacion> obtieneGrupos()
    {
        List<AvancesOperacion> grupos = new List<AvancesOperacion>();
        datosCotizaProv cotizacion = new datosCotizaProv();
        object[] gruposObtenidos = obtieneInfo(empresa, taller, orden);
        if (Convert.ToBoolean(gruposObtenidos[0]))
        {
            DataSet refCot = (DataSet)gruposObtenidos[1];
            foreach (DataRow fila in refCot.Tables[0].Rows)
            {
                grupos.Add(new AvancesOperacion() { 
                    id = Convert.ToInt32(fila[0].ToString()), 
                    grupo = fila[1].ToString(), 
                    chk25 = Convert.ToBoolean(fila[2]),
                    chk50 = Convert.ToBoolean(fila[3]),
                    chk75 = Convert.ToBoolean(fila[4]),
                    chk100 = Convert.ToBoolean(fila[5]),
                    chkVoBo = Convert.ToBoolean(fila[6])
                });
            }
        }
        else
            grupos = null;

        return grupos;
    }

    private object[] obtieneInfo(int empresa, int taller, int orden)
    {
        string sql = "select distinct mo.id_grupo_op,upper(g.descripcion_go) as descripcion,"
        + "isnull((select s.p25 from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as p25,"
        + "isnull((select s.p50 from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as p50,"
        + "isnull((select s.p75 from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as p75,"
        + "isnull((select s.p100 from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as p100,"
        + "isnull((select s.terminado from Seguimiento_Operacion s where s.id_empresa=mo.id_empresa and s.id_taller=mo.id_taller and s.no_orden=mo.no_orden and s.id_grupo_op=mo.id_grupo_op),0) as VoBo "
        + "from Mano_Obra mo "
        + "inner join Grupo_Operacion g on g.id_grupo_op=mo.id_grupo_op "
        + "where mo.id_empresa=" + empresa.ToString() + " and mo.id_taller=" + taller.ToString() + " and mo.no_orden=" + orden.ToString(); //and mo.id_grupo_op in (1,2,3)";
        return ejecuta.dataSet(sql);
    }

}