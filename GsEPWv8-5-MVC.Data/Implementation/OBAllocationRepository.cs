using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsEPWv8_5_MVC.Core.Entity;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class OBAllocationRepository : IOBAllocationRepository
    {

        public bool SaveOBAlocEditHdr(string pstrCmpId, string pstrAlocDocId, string pstrAlocDocDt,  string pstrSrNum, string pstrShipDt,
            string pstrCancelDt, string pstrPriceTkt, string pstrCustId, string pstrCustName, string pstrCustOrdrNum, string pstrCustOrdrDt, string pstrNote)
            {
            bool blnStatus = false;
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
               
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    connection.Open();
                       // transactional code...
                        using (SqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "spOBSaveAlocEditHdr";
                            cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                            cmd.Parameters.Add("@pstrAlocDocId", SqlDbType.VarChar).Value = pstrAlocDocId;
                            cmd.Parameters.Add("@pstrAlocDocDt", SqlDbType.VarChar).Value = pstrAlocDocDt;
                            cmd.Parameters.Add("@pstrSrNum", SqlDbType.VarChar).Value = pstrSrNum;
                            cmd.Parameters.Add("@pstrShipDt", SqlDbType.VarChar).Value = pstrShipDt;
                            cmd.Parameters.Add("@pstrCancelDt", SqlDbType.VarChar).Value = pstrCancelDt;
                            cmd.Parameters.Add("@pstrPriceTkt", SqlDbType.VarChar).Value = pstrPriceTkt;
                            cmd.Parameters.Add("@pstrCustId", SqlDbType.VarChar).Value = pstrCustId;
                            cmd.Parameters.Add("@pstrCustName", SqlDbType.VarChar).Value = pstrCustName;
                            cmd.Parameters.Add("@pstrCustOrdrNum", SqlDbType.VarChar).Value = pstrCustOrdrNum;
                            cmd.Parameters.Add("@pstrCustOrdrDt", SqlDbType.VarChar).Value = pstrCustOrdrDt;
                            cmd.Parameters.Add("@pstrNote", SqlDbType.VarChar).Value = pstrNote;
                            cmd.Connection = connection;
                            cmd.ExecuteNonQuery();
                        }
                }
            }
            catch (Exception ex)
            {
                blnStatus = false;
                throw ex;
            }
            
                return blnStatus;
        }
        public List<clsOBAlocHdr> getOBAlocHdr(string pstrCmpId, string pstrAlocDocId)
        {
            List<clsOBAlocHdr> lstOBAlocHdr = new List<clsOBAlocHdr>();
            try
            {
                using (IDbConnection sqlConn = ConnectionManager.OpenConnection())
                {
                    List<clsOBAlocHdr> lstAlloctionSummary = sqlConn.Query<clsOBAlocHdr>("spGetOBAlocHdr", new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrAlocDocId = pstrAlocDocId
                    }, commandType: CommandType.StoredProcedure).ToList();
                    lstOBAlocHdr = lstAlloctionSummary.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return lstOBAlocHdr;
        }

        public List<clsOBAlocSmry> getOBAlocDtl(string pstrCmpId, string pstrAlocDocId)
        {
            List<clsOBAlocSmry> lstOBAlocDtl = new List<clsOBAlocSmry>();
            try
            {
                using (IDbConnection sqlConn = ConnectionManager.OpenConnection())
                {
                    List<clsOBAlocSmry> llstOBAlocDtl = sqlConn.Query<clsOBAlocSmry>("spGetOBAlocDtl", new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrAlocDocId = pstrAlocDocId
                    }, commandType: CommandType.StoredProcedure).ToList();
                    lstOBAlocDtl = llstOBAlocDtl.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return lstOBAlocDtl;
        }

        public List<clsOBAlocCtn> getOBAlocCtn(string pstrCmpId, string pstrAlocDocId)
        {
            List<clsOBAlocCtn> lstOBAlocCtn = new List<clsOBAlocCtn>();
            try
            {
                using (IDbConnection sqlConn = ConnectionManager.OpenConnection())
                {
                    List<clsOBAlocCtn> llstOBAlocCtn = sqlConn.Query<clsOBAlocCtn>("spGetOBAlocCtn", new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrAlocDocId = pstrAlocDocId
                    }, commandType: CommandType.StoredProcedure).ToList();
                    lstOBAlocCtn = llstOBAlocCtn.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return lstOBAlocCtn;
        }

        public List<clsOBAlocCtnByLine> getOBAlocCtnByLine(string pstrCmpId, string pstrAlocDocId, int pstrAlocLine )
        {
            List<clsOBAlocCtnByLine> lstOBAlocCtnByLine = new List<clsOBAlocCtnByLine>();
            try
            {
                using (IDbConnection sqlConn = ConnectionManager.OpenConnection())
                {
                    List<clsOBAlocCtnByLine> llstOBAlocCtnByLine = sqlConn.Query<clsOBAlocCtnByLine>("spGetOBAlocCtnByLine", new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrAlocDocId = pstrAlocDocId,
                        @pstrAlocLine = pstrAlocLine
                    }, commandType: CommandType.StoredProcedure).ToList();
                    lstOBAlocCtnByLine = llstOBAlocCtnByLine.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return lstOBAlocCtnByLine;
        }

        public int getAlocEditTmpRefNum()
        {

            try
            {
                int lintRefNum = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spGetTempAlocRefNum", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                lintRefNum = Convert.ToInt32(command.ExecuteScalar());

                if (lintRefNum > 0)
                {
                    return lintRefNum;
                }
                else
                {
                    return lintRefNum;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public string saveAlocCtnEditByLine(string pstrRefNum, string pstrCmpId, string pstrAlocDocId, string pstrAlocDocDt, int pintLineNum,  DataTable pdtAlocCtnList)
        {
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_iv_aloc_ctn_temp";
                        sqlBulkCopy.WriteToServer(pdtAlocCtnList);
                        connection.Close();

                    }
                }
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    connection.Open();


                    try
                    {

                        // transactional code...
                        using (SqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "spOBSaveAlocCtnEditByLine";
                            cmd.Parameters.Add("@pstrRefNum", SqlDbType.VarChar).Value = pstrRefNum;
                            cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                            cmd.Parameters.Add("@pstrAlocDocId", SqlDbType.VarChar).Value = pstrAlocDocId;
                            cmd.Parameters.Add("@pstrAlocDocDt", SqlDbType.VarChar).Value = pstrAlocDocDt;
                            cmd.Parameters.Add("@pintLineNum", SqlDbType.Int).Value = pintLineNum;
                            cmd.Connection = connection;

                            cmd.ExecuteNonQuery();


                        }
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }


                    return "OK";
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
                throw ex;
            }
            finally
            {

            }

        }


        public OutboundInq GetalochdrList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_ALOCHDR_LIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_ALOCDOC_ID = objOutboundInq.aloc_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocSummary = ListInq.ToList();

                    return objOutboundInq;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public List<ClsOBSRAlocDtl> getSRAlocDtl(string pstrCmpId, string pstrSrNo)
        {
            List<ClsOBSRAlocDtl> lstOBAlocationSummary = new List<ClsOBSRAlocDtl> () ;
            try
            {
                using (IDbConnection sqlConn = ConnectionManager.OpenConnection())
                {
                    List<ClsOBSRAlocDtl> lstAlloctionSummary = sqlConn.Query<ClsOBSRAlocDtl>("spGetSRAlocDtl", new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrSrNo = pstrSrNo
                    }, commandType: CommandType.StoredProcedure).ToList();
                    lstOBAlocationSummary= lstAlloctionSummary.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return lstOBAlocationSummary;
        }
        public List<ClsOBSRAlocDtlByLoc> getSRAlocDtlByLoc(string @pstrRefNum, string pstrCmpId, string pstrSrNo)
        {
            List<ClsOBSRAlocDtlByLoc> lstOBSRAlocDtlByLoc = new List<ClsOBSRAlocDtlByLoc>();
            try
            {
                using (IDbConnection sqlConn = ConnectionManager.OpenConnection())
                {
                    List<ClsOBSRAlocDtlByLoc> lstOBSRAlocDtlByLocation = sqlConn.Query<ClsOBSRAlocDtlByLoc>("spGetSRAlocDtlByLoc", new
                    {
                        @pstrRefNum = @pstrRefNum,
                        @pstrCmpId = pstrCmpId,
                        @pstrSrNo = pstrSrNo
                    }, commandType: CommandType.StoredProcedure).ToList();
                    lstOBSRAlocDtlByLoc = lstOBSRAlocDtlByLocation.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return lstOBSRAlocDtlByLoc;
        }
        public bool SaveAloctionByBatch(string pstrCmpId, string pstrRefNum, string pstrSrNo, string pstrUserId)
        {
          
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                   connection.Open();
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "spSaveSRAloctionByBatch";
                        cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                        cmd.Parameters.Add("@pstrRefNum", SqlDbType.VarChar).Value = pstrRefNum;
                        cmd.Parameters.Add("@pstrSrNo", SqlDbType.VarChar).Value = pstrSrNo;
                        cmd.Parameters.Add("@pstrUserId", SqlDbType.VarChar).Value = pstrUserId;
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return true;
        }
        public ClsOBAllocation GetOBSRSummary(ClsOBAllocation objOBAllocation)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetSRSummary> ListIRFP = connection.Query<OBGetSRSummary>("spGetOpenSRListForAlloc", new
                    {
                        @pstCmp_Id = objOBAllocation.OBAlocInquiryByBatch.cmpId,
                        @pstrBatchNum = objOBAllocation.OBAlocInquiryByBatch.batchNum,
                        @pstrSrNumFrom = objOBAllocation.OBAlocInquiryByBatch.srNumFrom,
                        @pstrSrNumTo = objOBAllocation.OBAlocInquiryByBatch.srNumTo,
                        @pstrSDtFrom = objOBAllocation.OBAlocInquiryByBatch.srDtFrom,
                        @pstrSDtTo = objOBAllocation.OBAlocInquiryByBatch.srDtTo,
                        @pstrLoadNumber = objOBAllocation.OBAlocInquiryByBatch.srLoadNum


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBAllocation.ListOBGetSRSummary = ListIRFP.ToList();


                }
                return objOBAllocation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable getOBSRAlocStatus(string pstrCmpId, string pstrRefNum, string pstrUserId, ref DataTable dtUserEmail)
        {
            try
            {
                System.Data.DataSet dataSet = null;
                System.Data.SqlClient.SqlDataAdapter adapter = null;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spGetSRAlocStatusRpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                command.Parameters.Add("@pstrRefNum", SqlDbType.VarChar).Value = pstrRefNum;
                command.Parameters.Add("@pstrUserId", SqlDbType.VarChar).Value = pstrUserId;
                connection.Open();
                DataTable dtOBSRAloc = new DataTable();
                adapter = new System.Data.SqlClient.SqlDataAdapter(command);
                dataSet = new System.Data.DataSet();
                adapter.Fill(dataSet);
                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        dtOBSRAloc = dataSet.Tables[0];
                        dtUserEmail = dataSet.Tables[1];
                    }
                }
                return dtOBSRAloc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable getOBSRAlocStatus(string pstrCmpId, string pstrRefNum)
        {
            DataTable dtOBSRAlocStk = new DataTable();
            try
            {
                System.Data.DataSet dataSet = null;
                System.Data.SqlClient.SqlDataAdapter adapter = null;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spGetOBSRAlocVerifyStk", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                command.Parameters.Add("@pstrRefNum", SqlDbType.VarChar).Value = pstrRefNum;
                connection.Open();
                adapter = new System.Data.SqlClient.SqlDataAdapter(command);
                dataSet = new System.Data.DataSet();
                adapter.Fill(dataSet);
                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        dtOBSRAlocStk = dataSet.Tables[0];
                    }
                }
                return dtOBSRAlocStk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtOBSRAlocStk.Dispose();
            }
        }

        public PickLabel fnGetOBPickLabel(string pstrCmpId, string pstrRefNum)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    PickLabel objPickLabel = new PickLabel();
                    const string storedProcedure2 = "spGetOBPickLabel";
                    IEnumerable<PickLabelDtl> lstPickLabel = connection.Query<PickLabelDtl>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrRefNum = pstrRefNum
                        },
                        commandType: CommandType.StoredProcedure);
                    objPickLabel.lstPickLabelDtl = lstPickLabel.ToList();
                   

                    return objPickLabel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public PickLabel fnGetOBDocForPrint(string pstrCmpId, string pstrRefNum)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    PickLabel objPickLabel = new PickLabel();
                    const string storedProcedure2 = "spGetOBDocForBatch";
                    IEnumerable<DocForPrint> lstDocForPrint = connection.Query<DocForPrint>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrRefNum = pstrRefNum
                        },
                        commandType: CommandType.StoredProcedure);
                    objPickLabel.lstDocForPrint = lstDocForPrint.ToList();


                    return objPickLabel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

 
        public PickLabel fnGetOBPickLabelSmry(PickLabel pobjPickLabel)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    PickLabel objPickLabel = new PickLabel();
                    const string storedProcedure2 = "spGetOBPickLabelSmry";
                    IEnumerable<PickLabelSmry> lstPickLabelSmry = connection.Query<PickLabelSmry>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pobjPickLabel.cmp_id,
                            @pstrBatchNum = pobjPickLabel.batch_num,
                            @pstrSoNumFrom = pobjPickLabel.so_num_from,
                            @pstrSoNumTo = pobjPickLabel.so_num_to,
                            @pstrSoOrderDtFrom = pobjPickLabel.so_dt_from,
                            @pstrSoOrderDtTo = pobjPickLabel.so_dt_to,

                        },
                        commandType: CommandType.StoredProcedure);
                    objPickLabel.lstPickLabelSmry = lstPickLabelSmry.ToList();


                    return objPickLabel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
     
        public PickLabel fnGetOBSRListForBinprint(string pstrCmpId, string pstrSrList)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    PickLabel objPickLabel = new PickLabel();
                    const string storedProcedure2 = "spGetOBSRListForBinprint";
                    IEnumerable<PickLabelDtl> lstPickLabel = connection.Query<PickLabelDtl>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrSrList = pstrSrList
                        },
                        commandType: CommandType.StoredProcedure);
                    objPickLabel.lstPickLabelDtl = lstPickLabel.ToList();


                    return objPickLabel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public PickLabel fnGetOBDocBySrList(string pstrCmpId, string pstrSrList)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    PickLabel objPickLabel = new PickLabel();
                    const string storedProcedure2 = "spGetLblPackSlipPrintBySrList";
                    IEnumerable<DocForPrint> lstDocForPrint = connection.Query<DocForPrint>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrSrList = pstrSrList
                        },
                        commandType: CommandType.StoredProcedure);
                    objPickLabel.lstDocForPrint = lstDocForPrint.ToList();


                    return objPickLabel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
    }
}
