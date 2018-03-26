using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Errores
/// </summary>
public class CatErrores
{
	public CatErrores()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public string mensajeError(int claveError)
    {
        string mensaje = "";
        switch (claveError)
        { 
            case 100:
                mensaje = "No se pudo realizar la conversión del elemento scalar en el método ScalarToInt a tipo entero.";
                break;
            case 101:
                mensaje = "No se pudo realizar la conversión del elemento scalar en el método ScalarToBoolean a tipo booleano.";
                break;
            case 102:
                mensaje = "No se pudo realizar asignacion del dataset en el método dataset() de la clase Ejecuciones.";
                break;
            case 103:
                mensaje = "No se pudo realizar la inserción, eliminación o actualización solicitada debido a ún error de sintaxis en la función enviada al método insertUpdateDelete() de la clase Ejecuciones";
                break;
            case 104:
                mensaje = "Se produjo ún error de sintaxis en el proceso de generacion de la Orden de la clase Ejecuciones";
                break;
            case 105:
                mensaje = "Se produjo ún error de sintaxis en el proceso de generacion de Cotización de la clase Ejecuciones";
                break;
            default:
                mensaje = "Proceso realizado exitosamente";
                break;
        }
        return mensaje;
    }

    public string mensajeErrorCatUsuarios(int claveError)
    {
        string mensaje = "";
        switch (claveError)
        {
            case 200:
                mensaje = "El Usuario que intenta agregar ya existe, por favor verifique su información.";
                break;
            default:
                mensaje = "Proceso realizado exitosamente.";
                break;
        }
        return mensaje;
    }

}