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
           