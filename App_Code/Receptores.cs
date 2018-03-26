using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Receptores
/// </summary>
public class Receptores
{
    Ejecuciones ejecuta = new Ejecuciones();
    public int idReceptor { get; set; }
    public string rfc { get; set; }
    public string razonSocial { get; set; }
    public string correo { get; set; }
    public string correoCC { get; set; }
    public string correoCCO { get; set; }
    public string calle { get; set; }
    public string nExt { get; set; }
    public string nInt { get; set; }
    public string localidad { get; set; }
    public string referencia { get; set; }
    public string cuenta { get; set; }
    public int pais { get; set; }
    public int estado { get; set; }
    public int municipio { get; set; }
    public int colonia { get; set; }
    public int cp { get; set; }

    public object[] retorno;
    private string sql;

	public Receptores()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public void agregaReceptor()
    {
        sql = "insert  into receptores_f (idRecep,ReRFC,ReNombre,ReCorreo,ReCorreoCC,ReCorreoCCO,ReCalle,ReNoExt,ReNoInt,ReLocalidad,ReReferencia,Recve_pais,Re_ID_Estado,Re_ID_Del_Mun,ReID_Colonia,ReID_Cod_Pos,ReCtaCxc) " +
              " values((select isnull((select top 1 idRecep from Receptores_f order by IdRecep desc),0)+1),'"+rfc.ToUpper()+"','"+razonSocial.ToUpper()+"','"+correo+"','"+correoCC+"','"+correoCCO+"','"+calle.ToUpper()+"','"+nExt.ToUpper()+"','"+nInt.ToUpper()+"','"+localidad.ToUpper()+"','"+referencia.ToUpper()+"',"+pais+","+estado+","+municipio+","+colonia+","+cp+",'"+cuenta.ToUpper()+"') ";
        retorno = ejecuta.insertUpdateDelete(sql); 
    }
    public void borrarReceptor()
    {
        sql = "delete receptores_f where idRecep=" +idReceptor;
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}