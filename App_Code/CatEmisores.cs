

/// <summary>
/// Descripción breve de CatEmisores
/// </summary>
public class CatEmisores
{
    private string sql;
    public object[] retorno;
    Ejecuciones ejecuta = new Ejecuciones();


	public CatEmisores()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    
    public void borrarEmisor(int idEmisor)
    {
        sql ="delete emisores_f where idEmisor="+idEmisor;
        retorno = ejecuta.scalarToBool(sql);
    }
}