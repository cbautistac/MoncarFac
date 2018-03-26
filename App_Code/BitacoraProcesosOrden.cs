using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de BitacoraProcesosOrden
/// </summary>
public class BitacoraProcesosOrden
{

    int _orden;
    int _taller;
    int _empresa;
    string _proceso;
    string _usuario;
    string _obervaciones;
    string _dato1;
    string _dato2;
    string _dato3;
    string _dato4;
    string _dato5;
    string _dato6;
    bool _registrado;

	public BitacoraProcesosOrden()
	{
        _obervaciones = _usuario = _proceso = _dato1 = _dato2 = _dato3 = _dato4 = _dato5 = _dato6 = string.Empty;
        _orden = _taller = _empresa = 0;
        _registrado = false;
	}

    public int Orden { set { _orden = value; } }
    public int Taller { set { _taller = value; } }
    public int Empresa { set { _empresa = value; } }
    public string Proceso { set { _proceso = value; } }
    public string Usuario { set { _usuario = value; } }
    public string Dato1 { set { _dato1 = value; } }
    public string Dato2 { set { _dato2 = value; } }
    public string Dato3 { set { _dato3 = value; } }
    public string Dato4 { set { _dato4 = value; } }
    public string Dato5 { set { _dato5 = value; } }
    public string Dato6 { set { _dato6 = value; } }    
    public bool Registrado { set { _registrado = value; } }

    private void armaObservacion() {
        switch (_proceso) { 
            default:
                _obervaciones = "";
                break;
        }
    }

    public void agregaBitacora() {
        armaObservacion();
        //insert

    }


}