using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de Calendarizacion
/// </summary>
public class Calendarizacion
{
    int _empresa;
    int _empleado;
    int _taller;
    object[] _datos;
    string _ini;
    string _fin;
    Ejecuciones ejecuta = new Ejecuciones();
	public Calendarizacion()
	{
        _empleado = _empresa =_taller = 0;
        _datos = new object[2];
        _ini = _fin = "1900-01-01";
	}

    public int Empresa { set { _empresa = value; } }
    public int Empleado { set { _empleado = value; } }
    public object[] Datos { get { return _datos; } }
    public string FIni { set { _ini = value; } }
    public string FFin { set { _fin = value; } }
    public int Taller { set { _taller = value; } }


    public void obtieneOrdenesEmpleado() {
        string sql = string.Format("select tabla.orden, tabla.fecha from (select o.id_taller,t.nombre_taller, t.identificador+'-'+cast(o.no_orden as varchar) as orden," +
 "case o.estatus when 'A' then convert(char(10),o.fecha_ini_prog,126) when 'I' then convert(char(10),o.fecha_ini,126) else '' end as fecha " +
 "from Operativos_Orden o " +
 "inner join talleres t on t.id_taller=o.id_taller " +
 "where o.IdEmp={0} and o.id_empresa={1} and (o.estatus<>'T' and o.estatus<>'M')) as tabla where cast( tabla.fecha as date) between '{2}' and '{3}' order by tabla.fecha ", _empleado, _empresa, _ini, _fin);
        //"where o.IdEmp={0} and o.id_empresa={1} and (o.estatus<>'T' and o.estatus<>'M') and o.id_taller={4}) as tabla where cast( tabla.fecha as date) between '{2}' and '{3}' order by tabla.fecha ", _empleado, _empresa, _ini, _fin, _taller);
        _datos = ejecuta.dataSet(sql);
    }


}