using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Utilities;
using System.Configuration;

public class docuCfdi
{
    private static BaseDatos datos = new BaseDatos();
    private static SqlConnection conn = datos.conexionBD;
    private static SqlCommand comm = new SqlCommand();
    private static Fechas fechas = new Fechas();

    


    public int IdCfd { get; set; }
    public int IdEmisor { get; set; }
    public int IdRecep { get; set; }
    public int IdTipoDoc { get; set; }
    public string IdMoneda { get; set; }
    public string strEmRfc { get; set; }
    public string strReRfc { get; set; }
    public DateTime dteFecha {
        get { return fechas.obtieneFechaLocal(); }
    }
    public string strHora
    {
        get { return fechas.obtieneFechaLocal().ToString("HH:mm:ss"); }
    }

    public DateTime dteEncFechaGen { get; set; }
    public string strEncHoraGen { get; set; }
    public DateTime dteEncFechaCancel { get; set; }
    public string strEncHoraCancel { get; set; }
    public string strEncSello { get; set; }
    public string strEncCert { get; set; }
    public string strEncTimbre { get; set; }
    public string strEncFormaPago { get; set; }
    public string strEncCondicionesPago { get; set; }
    public string strEncMetodoPago { get; set; }
    public decimal decEncDescGlob { get; set; }
    public decimal decEncDescMO { get; set; }
    public decimal decEncDescRefaccion { get; set; }
    public decimal decEncDescGlobImp { get; set; }
    public decimal decEncSubTotal { get; set; }
    public decimal decEncDesc { get; set; }
    public decimal decEncImpTras { get; set; }
    public decimal decEncImpRet { get; set; }
    public decimal decEncTotal { get; set; }
    public char charEncEstatus { get; set; }
    public string strEncMotDesc { get; set; }
    public float floEncTipoCambio { get; set; }
    public string strEncNota { get; set; }
    public string strEncReferencia { get; set; }
    public string strEncNumCtaPago { get; set; }
    public string strEncRegimen { get; set; }
    public string strEncLugarExpedicion { get; set; }
    public float strEncFolioImp { get; set; }
    public string strEncSerieImp { get; set; }
    public int idCfdAnt { get; set; }
    public string tipoFactura { get; set; }
    public string idUsoCFDI { get; set; }
    public string tipoDocumento { get; set; }

    public docuCfdi()
    {
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
}

    public docuCfdi(int intIdEm, int intIdRe, int intIdTipoDoc)
    {

        IdEmisor = intIdEm;
        IdRecep = intIdRe;
        IdTipoDoc = intIdTipoDoc;
    }

    public static object[] guardaEncCfdi(docuCfdi dcfd, List<detDocCfdi> lstDet)
    {
        int CfdiID = -1;
        object[] result = new object[2] { false, "" };
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        if (conn.State == ConnectionState.Open)
            conn.Close();

        comm = new SqlCommand();

        using (comm)
        {
            try
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "docCfdiInserta";
                comm.Parameters.Add("@IdCfd", SqlDbType.Int).Direction = ParameterDirection.Output;
                comm.Parameters.AddWithValue("@idEmisor", dcfd.IdEmisor).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@idRecep", dcfd.IdRecep).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@IdTipoDoc", dcfd.IdTipoDoc).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@IdMoneda", dcfd.IdMoneda).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncEmRfc", dcfd.strEmRfc).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncReRfc", dcfd.strReRfc).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncFecha", dcfd.dteFecha).DbType = DbType.Date;
                comm.Parameters.AddWithValue("@EncHora", dcfd.strHora).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncSello", dcfd.strEncSello).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncCertificado", dcfd.strEncCert).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncTimbre", dcfd.strEncTimbre).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncFormaPago", dcfd.strEncFormaPago).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncCondicionesPago", dcfd.strEncCondicionesPago).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncMetodoPago", dcfd.strEncMetodoPago).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncDescGlob", dcfd.decEncDescGlob).SqlDbType = SqlDbType.Real;
                comm.Parameters.AddWithValue("@EncDescMO", dcfd.decEncDescMO).SqlDbType = SqlDbType.Real;
                comm.Parameters.AddWithValue("@EncDescRefaccion", dcfd.decEncDescRefaccion).SqlDbType = SqlDbType.Real;
                comm.Parameters.AddWithValue("@EncDescGlobImp", dcfd.decEncDescGlobImp).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncSubTotal", dcfd.decEncSubTotal).SqlDbType = SqlDbType.Decimal;
                comm.Parameters.AddWithValue("@EncDesc", dcfd.decEncDesc).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncImpTras", dcfd.decEncImpTras).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncImpRet", dcfd.decEncImpRet).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncTotal", dcfd.decEncTotal).SqlDbType = SqlDbType.Decimal;
                comm.Parameters.AddWithValue("@EncMotivoDescuento", dcfd.strEncMotDesc).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncTipoCambio", dcfd.floEncTipoCambio).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncNota", dcfd.strEncNota).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncReferencia", dcfd.strEncReferencia).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncNumCtaPago", dcfd.strEncNumCtaPago).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncRegimen", dcfd.strEncRegimen).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncLugarExpedicion", dcfd.strEncLugarExpedicion).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncFolioImpresion", dcfd.strEncFolioImp).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncSerieImpresion", dcfd.strEncSerieImp).DbType = DbType.String;
                comm.Parameters.AddWithValue("@idCfdAnt", dcfd.idCfdAnt).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@idUsoCFDI", dcfd.idUsoCFDI).DbType = DbType.String;
                comm.Parameters.AddWithValue("@tipoDocumento", dcfd.tipoDocumento).DbType = DbType.String;

                DataTable dt = new DataTable();
                dt.Columns.Add("IdDetCfd", typeof(int));
                dt.Columns.Add("IdEmisor", typeof(int));
                dt.Columns.Add("IdConcepto", typeof(string));
                dt.Columns.Add("IdUnid", typeof(Int16));
                dt.Columns.Add("DetCantidad", typeof(Int16));
                dt.Columns.Add("CoValorUnit", typeof(float));
                dt.Columns.Add("IdTras1", typeof(Int16));
                dt.Columns.Add("DetImpTras1", typeof(float));
                dt.Columns.Add("IdTras2", typeof(Int16));
                dt.Columns.Add("DetImpTras2" , typeof(float));
                dt.Columns.Add("IdTras3", typeof(Int16));
                dt.Columns.Add("DetImpTras3", typeof(decimal));
                dt.Columns.Add("IdRet1", typeof(Int16));
                dt.Columns.Add("DetImpRet1", typeof(float));
                dt.Columns.Add("IdRet2", typeof(Int16));
                dt.Columns.Add("DetImpRet2", typeof(float));
                dt.Columns.Add("DetPorcDesc", typeof(decimal));
                dt.Columns.Add("DetImpDesc", typeof(decimal));
                dt.Columns.Add("Subtotal", typeof(decimal));
                dt.Columns.Add("Total", typeof(decimal));
                dt.Columns.Add("DetDesc", typeof(string));
                dt.Columns.Add("CoCuentaPredial", typeof(string));
                dt.Columns.Add("CveProdSerSAT", typeof(string));
                dt.Columns.Add("CveUnidadSAT", typeof(string));

                foreach (detDocCfdi det in lstDet)
                {

                    object[] valores = {det.IdDetCfd, det.IdEmisor, det.IdConcepto, det.IdUnid, det.DetCantidad, det.DetValorUni, det.IdTras1, det.DetImpTras1, 
                        det.IdTras2, det.DetImpTras2, det.IdTras3, det.DetImpTras3, det.IdRet1, det.DetImpRet1, det.IdRet2, det.DetImpRet2, 
                        det.DetPorcDesc, det.DetImpDesc, det.Subtotal, det.Total, det.DetDesc, det.CoCuentaPredial, det.CveProdServSAT, det.CveUnidadSAT };
                    dt.Rows.Add(valores);
                }

                comm.Parameters.AddWithValue("@tbDetConceptos", dt);
                comm.Parameters.Add("@respuesta", SqlDbType.VarChar, 256).Direction = ParameterDirection.Output;

                int filAfec = comm.ExecuteNonQuery();
                
                string resp = comm.Parameters["@IdCfd"].Value.ToString();
                CfdiID = Convert.ToInt32(comm.Parameters["@IdCfd"].Value);
                result[0] = true;
                if (CfdiID != -1)
                    result[1] = CfdiID;
                else
                    result[1] = resp;
            }
            catch(Exception ex)
            {
                result[1] = ex.Message;
            }
            finally
            {
                comm.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        return result;
    }


    public static object[] RecePago(docuCfdi dcfd, List<detDocCfdi> lstDet)
    {
        int CfdiID = -1;
        object[] result = new object[2] { false, "" };
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        if (conn.State == ConnectionState.Open)
            conn.Close();

        comm = new SqlCommand();

        using (comm)
        {
            try
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "ComprobantePago";
                comm.Parameters.Add("@IdCfd", SqlDbType.Int).Direction = ParameterDirection.Output;
                comm.Parameters.AddWithValue("@idEmisor", dcfd.IdEmisor).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@idRecep", dcfd.IdRecep).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@IdTipoDoc", dcfd.IdTipoDoc).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@IdMoneda", dcfd.IdMoneda).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncEmRfc", dcfd.strEmRfc).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncReRfc", dcfd.strReRfc).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncFecha", dcfd.dteFecha).DbType = DbType.Date;
                comm.Parameters.AddWithValue("@EncHora", dcfd.strHora).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncSello", dcfd.strEncSello).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncCertificado", dcfd.strEncCert).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncTimbre", dcfd.strEncTimbre).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncFormaPago", dcfd.strEncFormaPago).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncCondicionesPago", dcfd.strEncCondicionesPago).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncMetodoPago", dcfd.strEncMetodoPago).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncDescGlob", dcfd.decEncDescGlob).SqlDbType = SqlDbType.Real;
                comm.Parameters.AddWithValue("@EncDescMO", dcfd.decEncDescMO).SqlDbType = SqlDbType.Real;
                comm.Parameters.AddWithValue("@EncDescRefaccion", dcfd.decEncDescRefaccion).SqlDbType = SqlDbType.Real;
                comm.Parameters.AddWithValue("@EncDescGlobImp", dcfd.decEncDescGlobImp).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncSubTotal", dcfd.decEncSubTotal).SqlDbType = SqlDbType.Decimal;
                comm.Parameters.AddWithValue("@EncDesc", dcfd.decEncDesc).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncImpTras", dcfd.decEncImpTras).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncImpRet", dcfd.decEncImpRet).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncTotal", dcfd.decEncTotal).SqlDbType = SqlDbType.Decimal;
                comm.Parameters.AddWithValue("@EncMotivoDescuento", dcfd.strEncMotDesc).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncTipoCambio", dcfd.floEncTipoCambio).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncNota", dcfd.strEncNota).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncReferencia", dcfd.strEncReferencia).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncNumCtaPago", dcfd.strEncNumCtaPago).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncRegimen", dcfd.strEncRegimen).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncLugarExpedicion", dcfd.strEncLugarExpedicion).DbType = DbType.String;
                comm.Parameters.AddWithValue("@EncFolioImpresion", dcfd.strEncFolioImp).SqlDbType = SqlDbType.Float;
                comm.Parameters.AddWithValue("@EncSerieImpresion", dcfd.strEncSerieImp).DbType = DbType.String;
                comm.Parameters.AddWithValue("@idCfdAnt", dcfd.idCfdAnt).DbType = DbType.Int32;
                comm.Parameters.AddWithValue("@idUsoCFDI", dcfd.idUsoCFDI).DbType = DbType.String;
                comm.Parameters.AddWithValue("@tipoDocumento", dcfd.tipoDocumento).DbType = DbType.String;

                DataTable dt = new DataTable();
                dt.Columns.Add("IdDetCfd", typeof(int));
                dt.Columns.Add("IdEmisor", typeof(int));
                dt.Columns.Add("IdConcepto", typeof(string));
                dt.Columns.Add("IdUnid", typeof(string));
                dt.Columns.Add("DetCantidad", typeof(string));
                dt.Columns.Add("CoValorUnit", typeof(string));
                dt.Columns.Add("IdTras1", typeof(string));
                dt.Columns.Add("DetImpTras1", typeof(string));
                dt.Columns.Add("IdTras2", typeof(string));
                dt.Columns.Add("DetImpTras2", typeof(string));
                dt.Columns.Add("IdTras3", typeof(string));
                dt.Columns.Add("DetImpTras3", typeof(string));
                dt.Columns.Add("IdRet1", typeof(string));
                dt.Columns.Add("DetImpRet1", typeof(string));
                dt.Columns.Add("IdRet2", typeof(string));
                dt.Columns.Add("DetImpRet2", typeof(string));
                dt.Columns.Add("DetPorcDesc", typeof(string));
                dt.Columns.Add("DetImpDesc", typeof(string));
                dt.Columns.Add("Subtotal", typeof(string));
                dt.Columns.Add("Total", typeof(string));
                dt.Columns.Add("DetDesc", typeof(string));
                dt.Columns.Add("CoCuentaPredial", typeof(string));
                dt.Columns.Add("CveProdSerSAT", typeof(string));
                dt.Columns.Add("CveUnidadSAT", typeof(string));

                foreach (detDocCfdi det in lstDet)
                {
                    //asd.IdDetCfd = fila.ItemIndex + 1;
                    //asd.IdEmisor = Convert.ToInt16(IDEmisor);
                    //asd.UUID = ((TextBox)fila.FindControl("txtUUID")).Text;
                    //asd.Folio = ((TextBox)fila.FindControl("txtFoliot")).Text;
                    //asd.Parcialidad = parcialidad.SelectedValue;
                    //asd.SaldoAnt = ((TextBox)fila.FindControl("txtSaldoAnterior")).Text;
                    //asd.SaldoPagado = ((TextBox)fila.FindControl("txtIportePagado")).Text;
                    //asd.SaldoAct = ((Label)fila.FindControl("lblSaldoActual")).Text;
                    //asd.Total = Convert.ToDecimal(((Label)fila.FindControl("lblSaldoActual")).Text);
                    //asd.FECHA = ((TextBox)fila.FindControl("txtFecha")).Text;
                    //asd.HORA = ((TextBox)fila.FindControl("txtHora")).Text;
                    //asd.ProductoSAT = "84111506";
                    //asd.ClaveUnidadSAT = "ACT";
                    //asd.idcfdAnterior = Request.QueryString["fact"];

                    object[] valores = {det.IdDetCfd, det.IdEmisor, det.UUID, det.Folio, det.Parcialidad, det.SaldoAnt, det.SaldoPagado, det.SaldoAct,
                        det.FECHA, det.HORA, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, det.ProductoSAT, det.ClaveUnidadSAT };
                    dt.Rows.Add(valores);
                }

                comm.Parameters.AddWithValue("@tbDetConceptos", dt);
                comm.Parameters.Add("@respuesta", SqlDbType.VarChar, 256).Direction = ParameterDirection.Output;

                int filAfec = comm.ExecuteNonQuery();

                string resp = comm.Parameters["@IdCfd"].Value.ToString();
                CfdiID = Convert.ToInt32(comm.Parameters["@IdCfd"].Value);
                result[0] = true;
                if (CfdiID != -1)
                    result[1] = CfdiID;
                else
                    result[1] = resp;
            }
            catch (Exception ex)
            {
                result[1] = ex.Message;
            }
            finally
            {
                comm.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        return result;
    }

    public static object[] GuardaRecepcionPago(docuCfdi dcfd, List<detDocCfdi> lstDet)
    {
        //int CfdiID = -1;
        //object[] result = new object[2] { false, "" };
        //conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        //if (conn.State == ConnectionState.Open)
        //    conn.Close();

        //comm = new SqlCommand();

        //using (comm)
        //{
        string Insertar = "";
        object[] re = new object[1];
            try
            {
            BaseDatos bd = new BaseDatos();

            string valores = "", up="";
            foreach (detDocCfdi a in lstDet) {
                valores = ",'" + a.Folio + "','" + a.Parcialidad + "','" + a.SaldoAnt + "','" + a.SaldoPagado + "','" + a.SaldoAct + "','" + a.UUID + "','" + a.ProductoSAT + "','" + a.ClaveUnidadSAT + "'";
                up = a.Folio + "," + a.Parcialidad + "," + a.SaldoAnt + "," + a.SaldoPagado + "," + a.SaldoAct;
            }

            string[] x = up.Split(',');

            object[] ab = bd.scalarInt("Select count(*) from recepcion_pagos_f where idemisor='" + dcfd.IdEmisor + "' and idrecep='" + dcfd.IdRecep + "'");

            if (Convert.ToInt32(ab[1]) == 1)
            {
                Insertar = "update recepcion_pagos_f set Folio='" + x[0] + "', Parcialidad='" + x[1] + "', SaldoAnterior='" + x[2] + "', SaldoPagado='" + x[3] + "', SaldoActual='" + x[4] + "' where idemisor='" + dcfd.IdEmisor + "' and idrecep='" + dcfd.IdRecep + "';" +
                    "update recepcionpagos_f set Parcialidad='"+x[1]+"', SaldoAnterior='"+x[2]+"', SaldoActual='"+x[3]+"', Total='"+x[4] + "' where idemisor='" + dcfd.IdEmisor + "' and idreceptor='" + dcfd.IdRecep + "';";
                re = bd.insertUpdateDelete(Insertar);
            }
            else
            {
                object[] idcfd = bd.scalarInt("select count(*)+1 from recepcion_pagos_f");

                Insertar = "insert into recepcion_pagos_f (idcfd,idemisor,idrecep,idtipodoc,idmoneda,EncEmRfc,EncReRfc,EncFecha,EncHora,EncSello,EncCertificado,EncTimbre,EncFormaPago,EncCondicionesPago,EncMetodoPago," +
                "EncTipoCambio,EncNota,EncReferencia,EncNumCtaPago,EncRegimen,EncLugarExpedicion,EncFolioImpresion,EncSerieImpresion,usocfdi_sat,tipoDocumento,Folio,Parcialidad," +
                "SaldoAnterior, SaldoPagado, SaldoActual, UUIDFactura,ProductoSAT,ClaveUnidadSAT,encestatus,idcfdAnt)" +
                "values ('" + idcfd[1].ToString() + "','" + dcfd.IdEmisor + "'" +
                ",'" + dcfd.IdRecep + "'" +
                ",'" + dcfd.IdTipoDoc + "'" +
                ",'" + dcfd.IdMoneda + "'" +
                ",'" + dcfd.strEmRfc + "'" +
                ",'" + dcfd.strReRfc + "'" +
                ",'" + dcfd.dteFecha.ToShortDateString() + "'" +
                ",'" + dcfd.strHora + "'" +
                ",'" + dcfd.strEncSello + "'" +
                ",'" + dcfd.strEncCert + "'" +
                ",'" + dcfd.strEncTimbre + "'" +
                ",'" + dcfd.strEncFormaPago + "'" +
                ",'" + dcfd.strEncCondicionesPago + "'" +
                ",'" + dcfd.strEncMetodoPago + "'" +
                ",'" + dcfd.floEncTipoCambio + "'" +
                ",'" + dcfd.strEncNota + "'" +
                ",'" + dcfd.strEncReferencia + "'" +
                ",'" + dcfd.strEncNumCtaPago + "'" +
                ",'" + dcfd.strEncRegimen + "'" +
                ",'" + dcfd.strEncLugarExpedicion + "'" +
                ",'" + dcfd.strEncFolioImp + "'" +
                ",'" + dcfd.strEncSerieImp + "'" +
                ",'" + dcfd.idUsoCFDI + "'" +
                ",'" + dcfd.tipoDocumento + "'" + valores + ",'P','" + dcfd.idCfdAnt + "')";
                re = bd.insertUpdateDelete(Insertar);
            }

                //int filAfec = comm.ExecuteNonQuery();

                //string resp = comm.Parameters["@IdCfd"].Value.ToString();
                //CfdiID = Convert.ToInt32(comm.Parameters["@IdCfd"].Value);
                //result[0] = true;
                //if (CfdiID != -1)
                //    result[1] = CfdiID;
                //else
                //    result[1] = resp;
            }
            catch (Exception ex)
            {
                re[1] = ex.Message;
            }
            finally
            {
                comm.Dispose();
                conn.Close();
                conn.Dispose();
            }
        //}

        return re;
    }


    public void actualizaTipoFactura()
    {
        object[] result = new object[2] { false, "" };
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        if (conn.State == ConnectionState.Open)
            conn.Close();

        comm = new SqlCommand();

        using (comm)
        {
            try
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = "update enccfd_f set tipo='" + tipoFactura + "', encestatus='P' where idcfd=" + IdCfd;
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                comm.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }
    }
}

public class detDocCfdi
{

    /// <summary>
    /// Para el guardado de la recepcion de pagos
    /// </summary>

    public string UUID { set; get; }
    public string Folio { set; get; }
    public string Moneda { set; get; }
    public string Parcialidad { set; get; }
    public string SaldoAnt { set; get; }
    public string SaldoAct { set; get; }
    public string SaldoPagado { set; get; }
    public string ProductoSAT { set; get; }
    public string ClaveUnidadSAT { set; get; }
    public string idcfdAnterior { set; get; }
    public string FECHA { set; get; }
    public string HORA { set; get; }

    public int IdDetCfd { get; set; }
    int IdCfd { get; set; }
    public int IdEmisor { get; set; }
    public string IdConcepto { get; set; }
    public short IdUnid { get; set; }
    public short DetCantidad { get; set; }
    public decimal DetValorUni { get; set; }
    public short IdTras1 { get; set; }
    public decimal DetImpTras1 { get; set; }
    public short IdTras2 { get; set; }
    public decimal DetImpTras2 { get; set; }
    public short IdTras3 { get; set; }
    public decimal DetImpTras3 { get; set; }
    public short IdRet1 { get; set; }
    public decimal DetImpRet1 { get; set; }
    public short IdRet2 { get; set; }
    public decimal DetImpRet2 { get; set; }
    public decimal DetPorcDesc { get; set; }
    public decimal DetImpDesc { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public string DetDesc { get; set; }
    public string CoCtaConta { get; set; }
    public string CoCuentaPredial { get; set; }
    public string CveProdServSAT { get; set; }
    public string CveUnidadSAT { get; set; }

    public detDocCfdi(int intIdEm)
    {
        IdEmisor = intIdEm;
    }

    public detDocCfdi()
    {
        // TODO: Complete member initialization
    }

}
           