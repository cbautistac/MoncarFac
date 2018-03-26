using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Operarios
/// </summary>
public class Operarios
{
    Fechas fechas = new Fechas();
    public int orden { get; set; }
    public int empresa { get; set; }
    public int taller { get; set; }
    public int empleado { get; set; }
    public int asignacion { get; set; }
    public decimal montoPagado { get; set; }
    public int usuario { get; set; }
    public int pagado { get; set; }
    public object[] retorno { get; set; }

	public Operarios()
	{
        orden = empresa = taller = empleado = asignacion = usuario = pagado = 0;
        montoPagado = 0;
        retorno = new object[] { false, "" };
	}

    public void actualizaPagado() {
        
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = "";
        if (pagado == 1)
            sql = "update operativos_orden set monto_pagado=" + montoPagado.ToString("F2") + ", pagado=" + pagado + ", fechaPago=Convert(datetime,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120), id_usuario_pago=" + usuario + " where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden + " and idEmp=" + empleado; //+ " and id_asignacion=" + asignacion;
        else
            sql = "update operativos_orden set monto_pagado=null, pagado=null, fechaPago=null, id_usuario_pago=null where id_empresa=" + empresa + " and id_taller=" + taller + " and no_orden=" + orden + " and idEmp=" + empleado; //+ " and id_asignacion=" + asignacion;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void liberarCajones()
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string sql = "select idemp,isnull(convert(char(10),fecha_ini,120),'') as  fecha_ini,isnull(convert(char(8),hora_ini,120),'') as hora_ini,isnull(convert(char(10),fecha_fin,120),'') as fecha_fin,isnull(convert(char(8),hora_fin,120),'') as hora_fin,estatus from operativos_orden where no_orden = " + orden + " and id_empresa = " + empresa + " and id_taller = " + taller;
        retorno = ejecuta.dataSet(sql);
    }

    public void liberar(int[] sessiones, string[] valoresOperarios)
    {
        Ejecuciones ejecuta = new Ejecuciones();
        string fechaIni = "", hiora_ini = "", fechafin = "", horafin = "";
        fechaIni = obtienefechaini(valoresOperarios[1].Trim(),0);
        hiora_ini = obtienefechaini(valoresOperarios[2].Trim(), 1);
        fechafin = obtienefechaini(valoresOperarios[3].Trim(), 0);
        horafin = obtienefechaini(valoresOperarios[4].Trim(), 1);
        string sql = "update operativos_orden set fecha_ini='" + fechaIni + "', hora_ini='" + hiora_ini + "', fecha_fin='" + fechafin + "',hora_fin='" + horafin + "',estatus='T' where id_empresa=" + sessiones[2] + " and id_taller=" + sessiones[3] + " and no_orden=" + orden + " and idEmp=" + valoresOperarios[0] + " update empleados set clv_pichonera=clv_pichonera+1 where idemp=" + valoresOperarios[0];
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    private string obtienefechaini(string v,int opcion)
    {
        string retorno;
        if (opcion == 0)
        {
            if (v == "")
                retorno = new E_Utilities.Fechas().obtieneFechaLocal().ToString("yyyy-MM-dd");
            else
                retorno = Convert.ToDateTime(v).ToString("yyyy-MM-dd");
        }
        else {
            if (v == "")
                retorno = new E_Utilities.Fechas().obtieneFechaLocal().ToString("HH:mm:ss");
            else
                try { retorno = Convert.ToDateTime(v).ToString("HH:mm:ss"); } catch (Exception) { retorno= new E_Utilities.Fechas().obtieneFechaLocal().ToString("HH:mm:ss"); }
        }
        return retorno;
    }
}