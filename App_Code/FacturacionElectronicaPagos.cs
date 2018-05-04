﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de FacturacionElectronicaPagos
/// </summary>
public class FacturacionElectronicaPagos
{
    Ejecuciones ejecuta = new Ejecuciones();
    public int empresa { get; set; }
    public int taller { get; set; }
    public int idCFD { get; set; }
    public int idEmisor { get; set; }
    public int idReceptor { get; set; }
    private string sql;
    public object[] retorno;

    public FacturacionElectronicaPagos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
    public void obtieneDatosEncabezado()
    {
        //sql = "select encfolioimpresion,case encFormaPago when '1' then '01' when '2' then '02' when '3' then '03' when '4' then '04' when '5' then '05' when '6' then '06' when '7' then '07' when '8' then '08' when '9' then '09' else EncFormaPago end as encformapago  ,EncCondicionesPago,encsubtotal-encdescglobimp,encdesc,enctotal,IdMoneda,EncTipoCambio,TIPO,encmetodopago,EncEmCP,idRecep,encImpTras,encdescglobimp from enccfd_f where IdCfd=" + idCFD;
        sql = "select encreferencia,case when LEN(encformapago)=1 then '0'+encformapago else Encformapago end as EncFormaPago,EncCondicionesPago,encsubtotal-encdescglobimp,encdesc,enctotal,IdMoneda,EncTipoCambio,TIPO,encmetodopago,EncEmCP,idRecep,encImpTras,encdescglobimp,encrerfc from enccfd_f where IdCfd=" + idCFD;
        retorno = ejecuta.dataSet(sql);
    }
    public void obtieneInfoConceptos()
    {
        sql = "select  a.claveprodServ,a.IdConcepto,a.DetCantidad,a.ClaveUnidad_SAT, b.Nombre,a.DetDesc,cast(a.DetValorUnit as decimal(15,4)) as DetValorUnit,  cast (a.subtotal as decimal(15,4)) as subtotal,a.detimpdesc,cast( a.DetImpTras3 as decimal(15,4)) as DetImpTras3, case IdTras3 when '1' then '001' when '2' then '002' when '3' then '003' else '' end as idTras3  from DetCFD_f a  left join c_unidad_f b on b.ClaveUnidad = a.ClaveUnidad_SAT where idCfd=" + idCFD;
        retorno = ejecuta.dataSet(sql);
    }

    public void obtieneUUIDFOLIO() {
        //sql = "select UUIDFactura,Folio,Parcialidad,SaldoAnterior,SaldoPagado,Saldoactual,case when LEN(encformapago)=1 then '0'+encformapago else Encformapago end as EncFormaPago,encmetodopago from Recepcion_Pagos_F where idcfdant=" + idCFD;
        sql = "select IdConcepto as UUID,IdUnid as Folio, DetCantidad as Parcialidad,DetValorUnit as SaldoAnterior,IdTras1 as ImportePagado,DetImpTras1 as SaldoActual,(select EncFormaPago from Recepcion_Pagos_F where idcfd='"+idCFD+"') as EncFormaPago,(select encmetodopago from Recepcion_Pagos_F where idcfd='"+idCFD+"')as encmetodopago from DetPagos_f where idcfd='"+idCFD+"'";
        retorno = ejecuta.dataSet(sql);
    }

    public void ObtienePago10(){
        sql = "select EncFecha,EncHora, EncFormaPago,SaldoAnterior, EncTipoCambio from recepcion_pagos_f where idcfd='" + idCFD + "'";
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
    public bool actualizaFactura(string UUID, string fecha, string hora, string selloSAT, string certificadoSAT, string timbreSAT, string certificado, string noCertificado)
    {
        bool fact = false;
        //sql = "update EncCFD_f set encEstatus='T', noCertificadoOrg='"+txtNoCertificado+"', certificado='', encSello='"+SelloCFDI+"', encCertificado='', encTimbre='', encFolioUUID='" + UUID + "', encFechaGenera='"+fecha+"', encHoraGenera='"+hora+"' where IdCfd=" + idCFD;
        //sql = " EncCFD_f where idCfd=" + idCFD;
        try
        {
            sql = "update recepcion_pagos_f set EncFolioUUID='" + UUID + "', EncFechaGenera='" + fecha + "', EncHoraGenera='" + hora + "',EncSello='" + selloSAT + "', EncCertificado='" + certificadoSAT + "',EncTimbre='" + timbreSAT + "',EncEstatus='T',certificado='" + certificado + "',nocertificadoOrg='" + noCertificado + "' where idcfd=" + idCFD;
            retorno = ejecuta.insertUpdateDelete(sql);
            fact = true;
        }
        catch (Exception ) { fact = false; };
        return fact;
    }

    public void actualizaTimbrado(int IdCDF,string emisor,string receptor, string certificado, string fecha, string uuid, string selloSat, string selloCFD, byte[] qr, string ruta, string cadena, string certificadoCfd, string Folio, string SAnt,string SPag, string STotal, string Parci)
    {
        //BaseDatos bd = new BaseDatos();
        //object[] a = bd.scalarToInt("select count(*) from recepcionpagos_f where idcfd='" + idCFD + "'");
        //if()
        
        sql = "declare @IDTIM int, @Saldo varchar(5); set @IDTIM = (select isnull((select top 1 idtimbre from RecepcionPagos_f where idCfd = 1 order by idtimbre desc),0)+1); set @Saldo=(select SaldoActual from recepcion_pagos_f where idcfd='"+idCFD+"');" +
            "insert into RecepcionPagos_f values (" + IdCDF + ",'" + Folio + "'," + emisor + "," + receptor + ",@IDTIM,'" + certificado + "','" + fecha + "','" + uuid + "','" + selloSat + "','" + selloCFD + "','@imagen','" + ruta + "','" + cadena + "','" + certificadoCfd + "','MXN','"+SAnt+"','"+SPag+"','"+STotal+"','"+Parci+"');" +
            "update Recepcion_Pagos_F set IdTimbreActual=@IDTIM, SaldoAnterior=@Saldo, SaldoPagado=0, SaldoActual=0 where IdCfd='" + idCFD + "';" +
            "insert into detallepagos_f (IdDetCfd,IdCfd,IdEmisor,IdConcepto,IdUnid,DetCantidad,DetValorUnit,IdTras1,DetImpTras1,IdTras2,DetImpTras2,IdTras3,DetImpTras3,IdRet1,DetImpRet1,IdRet2,DetImpRet2,DetPorcDesc,DetImpDesc,Subtotal,Total,DetDesc,CoCuentaPredial,DetArticuloSAP,ClaveUnidad_SAT,ClaveProdServ) " +
            "select *from DetPagos_f where idcfd='"+idCFD+ "'; update detallepagos_f set IDTimbre=@IDTIM where idcfd="+idCFD+" and idtimbre is null;";
        //sql = "update recepcionpagos_f set noCertificadoSat='" + certificado + "',fechaTimbrado='" + fecha + "',uuid='" + uuid + "',selloSat='" + selloSat + "',selloCFD='" + selloCFD + "',qr='@imagen',rutaArchivo='" + ruta + "',cadenaoriginal='" + cadena + "',noCertificadoCfd='" + certificadoCfd + "',SaldoAnterior='" + SAnt + "',SaldoActual='" + SPag + "',Total='" + STotal + "',Parcialidad='" + Parci + "' where idcfd=" + idCFD + " and idtimbre=(select top 1 (idtimbre)from RecepcionPagos_f where idcfd='"+idCFD+"' order by idtimbre desc)";
        //retorno = ejecuta.insertAdjuntos(sql, qr);
        retorno = ejecuta.insertUpdateDelete(sql);
        if(Convert.ToBoolean(retorno[1]))
            actualizaDetalles();
    }

    public void actualizaDetalles() {
        Ejecuciones eje = new Ejecuciones();
        object[] act = eje.dataSet("Select iddetcfd, idunid,DetImpTras1 from detpagos_F where idcfd="+idCFD+"");

        string a = "";
        DataSet val = (DataSet)act[1];
        int indice = 1;
        foreach (DataRow fila in val.Tables[0].Rows){
            for (int i = 0; i < 1; i++)
            {
                eje.insertUpdateDelete("update DetPagos_f set DetValorUnit='" + fila[2] + "', IdTras1=0, detimptras1=0 where IdCfd='" + idCFD + "' and IdDetCfd='" + indice + "' and IdUnid='" + fila[1] + "'");
                //a = "update DetPagos_f set DetValorUnit='" + fila[2] + "', IdTras1=0, detimptras1=0 where IdCfd='" + idCFD + "' and IdDetCfd='" + indice + "' and IdUnid='" + fila[1] + "'";
            }
            indice++;
        }
    }
}