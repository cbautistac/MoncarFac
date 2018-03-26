using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Autorizaciones
/// </summary>
public class Autorizaciones
{
    Ejecuciones ejecuta = new Ejecuciones();

    public string nick { get; set; }
    public string contrasena { get; set; }
    public int permiso { get; set; }
    string _error;
    bool _valido;
    int _id;

	public Autorizaciones()
	{
        _error = string.Empty;
        _valido = false;
        _id = 0;
	}

    public string Error {
        get { return _error; }
    }

    public int IdUsuario {
        get { return _id; }
    }

    public bool Valido {
        get { return _valido; }
    }

    private void obtieneError(int codigo) {
        switch (codigo) { 
            case 1:
                _error = "El usuario no existe";
                break;
            case 2:
                _error = "Usuario o Contraseña incorrectos";
                break;
            case 3:
                _error = "El usuario no cuenta con el permiso necesario";
                break;
            default:
                _error = "";
                break;
        }
    }

    public void validaUsuario() {

        try
        {
            CatUsuarios usuarios = new CatUsuarios();
            object[] usuario = usuarios.obtieneIdUsuario(nick);
            if (Convert.ToBoolean(usuario[0]))
            {
                _id = Convert.ToInt32(usuario[1]);
                if (_id != 0)
                {
                    object[] validaUser = usuarios.validaUsuarioLog(nick, contrasena);
                    if (Convert.ToBoolean(validaUser[0]))
                    {
                        if (Convert.ToBoolean(validaUser[1]))
                        {
                            object[] tienePermisos = tienePermiso();
                            if (Convert.ToBoolean(tienePermisos[0]))
                            {
                                if (Convert.ToBoolean(tienePermisos[1]))
                                {
                                    _valido = true;
                                    obtieneError(0);
                                }
                                else
                                {
                                    _valido = false;
                                    obtieneError(3);
                                }
                            }
                            else
                            {
                                _valido = false;
                                obtieneError(3);
                            }
                        }
                        else
                        {
                            _valido = false;
                            obtieneError(2);
                        }
                    }
                    else
                    {
                        _valido = false;
                        obtieneError(2);
                    }
                }
                else {
                    _valido = false;
                    obtieneError(1);
                }
            }
            else
            {
                _valido = false;
                obtieneError(1);
            }
        }
        catch (Exception ex) {
            _valido = false;
            _error = "Error: " + ex.Message;
        }
    }

    public object[] tienePermiso() {
        string sql = "select count(*) from usuarios_permisos where id_usuario=" + _id.ToString() + " and id_permiso=" + permiso.ToString();
        return ejecuta.scalarToBoolLog(sql);
    }

}