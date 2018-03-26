using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de CatFormaPago
/// </summary>
public class CatalFormaPago
{
    Ejecuciones ejecuta = new Ejecuciones();
    private string sql;
    public object[] retorno;
    public string cd { get; set; }
    public CatalFormaPago()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public bool agregaNuevaFormaPago(string descri, string bancariza, string NumOp, string rfcEmCtO, string CtaOrd, string PtnCtaOr, string rfcEmCtB, string CtaBen, string TipCadPag, string NomCtaExtra, string PtnCtaBen)
    {
        sql = "insert into FormaPago_FSAT (ClaveFormaPago, Descripcion, Bancarizado, NumeroOperacion, RFCEmisorCtaOrden, CuentaOrden, PatronCtaOrden, RFCEmisorCtaBen, CuentaBen, PatronCtaBen, TipoCadenaPago, NombreBancoEmisorCOE)" +
              " values ((select isnull((select top 1 ClaveFormaPago from formapago_fSAT order by ClaveFormaPago desc),0)+1),'" + descri + "','" + bancariza + "','" + NumOp + "','" + rfcEmCtO + "','" + CtaOrd + "','" + PtnCtaOr + "','" + rfcEmCtB + "','" + CtaBen + "','" + PtnCtaBen + "','" + TipCadPag + "','" + NomCtaExtra + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            return Convert.ToBoolean(ejecutado[1]);
        else
            return false;
    }
    public void editDatos(string descri, string bancariza, string NumOp, string rfcEmCtO, string CtaOrd, string PtnCtaOr, string rfcEmCtB, string CtaBen,string PtnCtaBen, string TipCadPag, string NomCtaExtra, string idForPag)
    {
        sql = "update formapago_fsat set Descripcion='" + descri + "', Bancarizado='" + bancariza + "', NumeroOperacion='" + NumOp + "', RFCEmisorCtaOrden='" + rfcEmCtO + "',CuentaOrden='" + CtaOrd + "', PatronCtaOrden='" + PtnCtaOr + "', RFCEmisorCtaBen='" + rfcEmCtB + "', CuentaBen='" + CtaBen + "',PatronCtaBen='" + PtnCtaBen + "', TipoCadenaPago='" + TipCadPag + "', NombreBancoEmisorCOE='" + NomCtaExtra + "' where ClaveFormaPago= '" + idForPag + "'";
        retorno[1] = ejecuta.insertUpdateDelete(sql);
    }
    public void cargardatos()
    {
        sql = "select * from formapago_fsat where ClaveFormaPago=" + cd;
        retorno = ejecuta.dataSet(sql);
    }
    public void elminarFormPag()
    {
        sql = "delete from formapago_fsat where ClaveFormaPago='" + cd + "'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}