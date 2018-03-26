using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de ParametrosFacturacion
/// </summary>
public class ParametrosFacturacion
{
    Ejecuciones ejecuta = new Ejecuciones();
    public int id_empresa { get; set; }
    public object[] info { get; set; }

	public ParametrosFacturacion()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public void obtieneParametos() {
        string sql = "select * from parametros_facturacion where id_empresa=" + id_empresa;
        info = ejecuta.dataSet(sql);
    }
}