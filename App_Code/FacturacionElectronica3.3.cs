using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de FacturacionElectronica3
/// </summary>
public class FacturacionElectronica3
{
    Ejecuciones ejecuta = new Ejecuciones();
    public int empresa { get; set; }
    public int taller { get; set; }
    public int idCFD { get; set; }
    public int idEmisor { get; set; }
    public int idReceptor {get;set;}
    private string sql;
    public object[] retorno;

	public FacturacionElectronica3()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public void obtieneDatosEncabezado()
    {
        //sql = "select encfolioimpresion,case encFormaPago when '1' then '01' when '2' then '02' when '3' then '03' when '4' then '04' when '5' then '05' when '6' then '06' when '7' then '07' when '8' then '08' when '9' then '09' else EncFormaPago end as encformapago  ,EncCondicionesPago,encsubtotal-encdescglobimp,encdesc,enctotal,IdMoneda,EncTipoCambio,TIPO,encmetodopago,EncEmCP,idRecep,encImpTras,encdescglobimp from enccfd_f where IdCfd=" + idCFD;
        sql = "select encreferencia,case encFormaPago when '1' then '01' when '2' then '02' when '3' then '03' when '4' then '04' when '5' then '05' when '6' then '06' when '7' then '07' when '8' then '08' when '9' then '09' else EncFormaPago end as encformapago  ,EncCondicionesPago,encsubtotal-encdescglobimp,encdesc,enctotal,IdMoneda,EncTipoCambio,TIPO,encmetodopago,EncEmCP,idRecep,encImpTras,encdescglobimp,encrerfc from enccfd_f where IdCfd=" + idCFD;
        retorno = ejecuta.dataSet(sql);
    }
    public void obtieneInfoConceptos()
    {
        sql = "select  a.claveprodServ,a.IdConcepto,a.DetCantidad,a.ClaveUnidad_SAT, b.Nombre,a.DetDesc,cast(a.DetValorUnit as decimal(15,4)) as DetValorUnit,  cast (a.subtotal as decimal(15,4)) as subtotal,a.detimpdesc,cast( a.DetImpTras3 as decimal(15,4)) as DetImpTras3, case IdTras3 when '1' then '001' when '2' then '002' when '3' then '003' else '' end as idTras3  from DetCFD_f a  left join c_unidad_f b on b.ClaveUnidad = a.ClaveUnidad_SAT where idCfd=" + idCFD;
        retorno = ejecuta.dataSet(sql);
    }
    public void obtieneInfoEmisor()
    {
        sql = "select EncEmRFC,EncEmNombre,EncRegimen from EncCFD_f where idCfd=" + idCFD + " and  IdEmisor=" + idEmisor;
        retorno = ejecuta.dataSet(sql);
    }

    public void obtieneInfoReceptor()
    {
        sql = "select f.ReRFC,f.ReNombre,a.UsoCFDi_SAT from EncCFD_f a inner join Receptores_f f on f.IdRecep = a.IdRecep where a.idCfd=" + idCFD + " and  a.IdRecep=" + idReceptor;
        //sql = "select EncReRFC, EncReNombre,UsoCFDi_SAT from EncCFD_f where idCfd=" + idCFD + " and  IdRecep=" + idReceptor;
        retorno = ejecuta.dataSet(sql);
    }
    public void obtieneImpConceptos()
    {
        sql = "select case IdTras3 when '1' then '001' when '2' then '002' when '3' then '003' else '' end as idTras3,cast(DetImpTras3 as decimal(15,4)) from DetCFD_f where idCfd=" + idCFD;
        retorno = ejecuta.dataSet(sql);
    }
    public void actualizaFactura(string UUID, string fecha,string hora, string selloSAT, string certificadoSAT, string timbreSAT,string certificado, string noCertificado)
    {
        //sql = "update EncCFD_f set encEstatus='T', noCertificadoOrg='"+txtNoCertificado+"', certificado='', encSello='"+SelloCFDI+"', encCertificado='', encTimbre='', encFolioUUID='" + UUID + "', encFechaGenera='"+fecha+"', encHoraGenera='"+hora+"' where IdCfd=" + idCFD;
        //sql = " EncCFD_f where idCfd=" + idCFD;
        sql = "update EncCFD_f set EncFolioUUID='"+UUID+"', EncFechaGenera='"+fecha+"', EncHoraGenera='"+hora+"',EncSello='"+selloSAT+"', EncCertificado='"+certificadoSAT+"',EncTimbre='"+timbreSAT+"',EncEstatus='T',certificado='"+certificado+"',nocertificadoOrg='"+noCertificado+"' where IdCFD=" + idCFD;
        retorno = ejecuta.insertUpdateDelete(sql);
    }
    public void actualizaTimbrado(int IdCDF, string certificado, string fecha, string uuid, string selloSat, string selloCFD, byte[] qr, string ruta, string cadena, string certificadoCfd)
    {
        sql = "insert into timbrado_f values (" + IdCDF + ",(select isnull((select top 1 idtimbre from timbrado_f where idCfd=" + IdCDF + " order by idtimbre desc),0)+1),'" + certificado + "','" + fecha + "','" + uuid + "','" + selloSat + "','" + selloCFD + "','@imagen','" + ruta + "','" + cadena + "','" + certificadoCfd + "')";
        retorno = ejecuta.insertAdjuntos(sql,qr);
        //retorno = ejecuta.insertUpdateDelete(sql);
    }
}