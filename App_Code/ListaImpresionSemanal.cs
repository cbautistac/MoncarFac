
using E_Utilities;
/// <summary>
/// Descripción breve de ListaImpresionSemanal
/// </summary>
public class ListaImpresionSemanal
{
    private string _dia;
    private string _diaSemana;
    private string _no_orden;
    private string _tipo_auto;
    private string _placas;
    private string _color;
    private string _cliente;
    private string _id_localizacion;
    private string _localizacion;
    private string _fecha;
    private string _avance_orden;
    private string _fase_orden;
    private string _f_terminado;
    private string _f_tocado;
    private string _f_entrega;
    private string _f_entrega_estimada;

    public ListaImpresionSemanal()
    {
        _dia= string.Empty;
        _diaSemana = string.Empty;
        _no_orden = string.Empty;
        _tipo_auto = string.Empty;
        _placas = string.Empty;
        _color = string.Empty;
        _cliente = string.Empty;
        _id_localizacion = string.Empty;
        _localizacion = string.Empty;
        _fecha = string.Empty;
        _avance_orden = string.Empty;
        _fase_orden = string.Empty;
        _f_terminado = string.Empty;
        _f_tocado = string.Empty;
        _f_entrega = string.Empty;
        _f_entrega_estimada = string.Empty;
    }

    public string Dia
    {
        get
        {
            return _dia;
        }

        set
        {
            _dia = value;
        }
    }

    public string DiaSemana
    {
        get
        {
            return _diaSemana;
        }

        set
        {
            _diaSemana = value;
        }
    }

    public string No_orden
    {
        get
        {
            return _no_orden;
        }

        set
        {
            _no_orden = value;
        }
    }

    public string Tipo_auto
    {
        get
        {
            return _tipo_auto;
        }

        set
        {
            _tipo_auto = value;
        }
    }

    public string Placas
    {
        get
        {
            return _placas;
        }

        set
        {
            _placas = value;
        }
    }

    public string Color
    {
        get
        {
            return _color;
        }

        set
        {
            _color = value;
        }
    }

    public string Cliente
    {
        get
        {
            return _cliente;
        }

        set
        {
            _cliente = value;
        }
    }

    public string Id_localizacion
    {
        get
        {
            return _id_localizacion;
        }

        set
        {
            _id_localizacion = value;
        }
    }

    public string Localizacion
    {
        get
        {
            return _localizacion;
        }

        set
        {
            _localizacion = value;
        }
    }

    public string Fecha
    {
        get
        {
            return _fecha;
        }

        set
        {
            _fecha = value;
        }
    }

    public string Avance_orden
    {
        get
        {
            return _avance_orden;
        }

        set
        {
            _avance_orden = value;
        }
    }

    public string Fase_orden
    {
        get
        {
            return _fase_orden;
        }

        set
        {
            _fase_orden = value;
        }
    }

    public string F_terminado
    {
        get
        {
            return _f_terminado;
        }

        set
        {
            _f_terminado = value;
        }
    }

    public string F_tocado
    {
        get
        {
            return _f_tocado;
        }

        set
        {
            _f_tocado = value;
        }
    }

    public string F_entrega
    {
        get
        {
            return _f_entrega;
        }

        set
        {
            _f_entrega = value;
        }
    }

    public string F_entrega_estimada
    {
        get
        {
            return _f_entrega_estimada;
        }

        set
        {
            _f_entrega_estimada = value;
        }
    }
}