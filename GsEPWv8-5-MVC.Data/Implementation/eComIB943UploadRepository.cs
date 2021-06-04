using Dapper;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_4_MVC.Data.Implementation
{
    public class eComIB943UploadRepository : IeComIB943UploadRepository
    {

        public bool Check943UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            try
            {
                int l_int_file_count = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_IB_943_UPLOAD_FILE_EXISTS", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_file_name", SqlDbType.VarChar).Value = p_str_file_name;
                connection.Open();
                l_int_file_count = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_file_count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
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

        public void InsertIB943UploadTempHdrtblDetails(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_insert_ib_cntr_upld_temp_hdr_tbl_dtls";
                    connection.Execute(SearchCustDtls,
                     new
                     {
                      @P_STR_COMP_ID = objeComIB943Upload.cmp_id,
                        @P_STR_IB_DOC_ID= objeComIB943Upload.ib_cntr_upld_doc_id,
                        @P_STR_REF_NUM = objeComIB943Upload.ref_num,
                        @P_STR_CNTR_ID = objeComIB943Upload.cntr_id,
                        @P_STR_MASTER_BOL = objeComIB943Upload.master_bol,
                        @P_STR_VESSEL_NO = objeComIB943Upload.vessel_no,
                        @P_STR_HDR_NOTES = objeComIB943Upload.hdr_notes,
                        @P_STR_RCVD_VIA = objeComIB943Upload.rcvd_via,
                        @P_STR_RCVD_FM = objeComIB943Upload.rcvd_from,
                    }, commandType: CommandType.StoredProcedure);
                  
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
        public void InsertIB943UploadTempDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_insert_ib_cntr_upld_temp_dtl_tbl_dtls";
                    connection.Execute(SearchCustDtls,
                     new
                     {
                         @P_STR_COMP_ID = objeComIB943Upload.cmp_id,
                        @P_STR_IB_DOC_ID = objeComIB943Upload.ib_cntr_upld_doc_id,
                        @P_STR_CNTR_ID = objeComIB943Upload.cntr_id,
                        @P_STR_PO_NUM = objeComIB943Upload.po_num,
                        @P_STR_DTL_LINE = objeComIB943Upload.dtl_line,
                        @P_STR_CTN_LINE = objeComIB943Upload.ctn_line,
                        @P_STR_ITM_CODE =objeComIB943Upload.Itm_Code,
                        @P_STR_ITM_NUM = objeComIB943Upload.itm_num,
                        @P_STR_ITM_COLOR = objeComIB943Upload.itm_color,
                        @P_STR_ITM_SIZE = objeComIB943Upload.itm_size,
                        @P_STR_ITM_NAME = objeComIB943Upload.itm_name,
                        @P_STR_ITM_QTY = objeComIB943Upload.itm_qty,
                        @P_STR_CTN_QTY = objeComIB943Upload.ctn_qty,
                        @P_STR_CTNS = objeComIB943Upload.ctns,
                        @P_STR_LOC_ID = objeComIB943Upload.loc_id,
                        @P_STR_ST_RATE_ID = objeComIB943Upload.io_rate_id,
                        @P_STR_IO_RATE_ID = objeComIB943Upload.st_rate_id,
                        @P_STR_CTN_LENGTH = objeComIB943Upload.ctn_length,
                        @P_STR_CTN_WIDTH = objeComIB943Upload.ctn_width,
                        @P_STR_CTN_HEIGHT = objeComIB943Upload.ctn_height,
                        @P_STR_CTN_CUBE = objeComIB943Upload.ctn_cube,
                        @P_STR_CTN_WGT= objeComIB943Upload.ctn_wgt,
                        @P_STR_DTL_NOTES = objeComIB943Upload.dtl_notes,
                     }, commandType: CommandType.StoredProcedure);
                   
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
        public void InsertIB943UploadDocHdrtblDetails(eComIB943Upload objeComIB943Upload)
        {
            try
            {
               

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvcweb_insert_doc_hdr";
                    connection.Execute(storedProcedure1,
                        new
                        {

                            @cmp_id = objeComIB943Upload.cmp_id,
                            @ib_doc_id = objeComIB943Upload.ib_cntr_upld_doc_id,
                            @status = "OPEN",
                            @step = "OPEN",
                            @ordr_type = "REG",
                            @ib_doc_dt = DateTime.Now.ToString("MM/dd/yyyy"),
                            @vend_id = objeComIB943Upload.rcvd_from,
                            @vend_name = objeComIB943Upload.rcvd_from,
                            @po_num = objeComIB943Upload.po_num,
                            @ordr_dt ="",
                            @req_num = objeComIB943Upload.ref_num,
                            @cntr_id = objeComIB943Upload.cntr_id,
                            @fob = "1000000001",
                            @billto_id = "10001",
                            @ship_from = "10001",
                            @shipto_id = "10001",
                            @dept_id = "10001",
                            @shipvia_id = objeComIB943Upload.rcvd_via,
                            @freight_id = "111111",
                            @terms_id = "10001",
                            @note = objeComIB943Upload.hdr_notes,
                            @process_id = "ADD",
                            @eta_dt = objeComIB943Upload.eta_date,
                            @rma_flag = "",
                            @master_bol = objeComIB943Upload.master_bol,
                            @vessel_no = objeComIB943Upload.vessel_no

                        }, commandType: CommandType.StoredProcedure);
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
        public void InsertIB943UploadDocDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvcweb_insert_Doc_dtl";
                    connection.Execute(storedProcedure1,
                        new
                        {

                            @cmp_id = objeComIB943Upload.cmp_id,
                            @ib_doc_id = objeComIB943Upload.ib_cntr_upld_doc_id,
                            @line_num = objeComIB943Upload.dtl_line,
                            @status = "OPEN",
                            @step = "OPEN",
                            @itm_code = objeComIB943Upload.Itm_Code,
                            @itm_num = objeComIB943Upload.itm_num,
                            @itm_color = objeComIB943Upload.itm_color,
                            @itm_size = objeComIB943Upload.itm_size,
                            @itm_name = objeComIB943Upload.itm_name,
                            @vend_itm_num = objeComIB943Upload.itm_num,
                            @vend_itm_color = objeComIB943Upload.itm_color,
                            @vend_itm_size = objeComIB943Upload.itm_size,
                            @ordr_ctn = objeComIB943Upload.ctns,
                            @ordr_qty = objeComIB943Upload.itm_qty,
                            @rcvd_ctn = "0",
                            @rcvd_qty = "0",
                            @qty_uom = "EA",
                            @back_ordr_qty = "0",
                            @price = "0",
                            @price_uom = "EA",
                            @list_price = "0",
                            @list_uom = "EA",
                            @pack_id = "PF",
                            @note = objeComIB943Upload.dtl_notes,
                            @process_id = "ADD"

                        }, commandType: CommandType.StoredProcedure);

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

        public void UpdateTblIbDocDtl(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_UPDATE_TBL_IB_DOC_DTL";
                    connection.Execute(SearchCustDtls,
                         new
                         {
                        @cmp_id = objeComIB943Upload.cmp_id,
                        @ib_doc_id = objeComIB943Upload.ib_cntr_upld_doc_id,
                        @line_num = objeComIB943Upload.dtl_line,
                        @p_int_ordr_ctn = objeComIB943Upload.ctns,
                        @p_int_ordr_qty = objeComIB943Upload.itm_qty


                    }, commandType: CommandType.StoredProcedure);
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
        public void InsertDetailstoCtntable(eComIB943Upload objeComIB943Upload)
        {
            try
            {


                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvcweb_insert_log_doc_ctn";
                    connection.Execute(storedProcedure1,
                        new
                        {

                            @cmp_id = objeComIB943Upload.cmp_id,
                            @ib_doc_id = objeComIB943Upload.ib_cntr_upld_doc_id,
                            @dtl_line = objeComIB943Upload.dtl_line,
                            @ctn_line = objeComIB943Upload.ctn_line,
                            @itm_line = objeComIB943Upload.ctn_line,
                            @kit_id = objeComIB943Upload.Itm_Code,
                            @itm_code = objeComIB943Upload.Itm_Code,
                            @itm_qty = objeComIB943Upload.ctn_qty,
                            @ctn_qty = objeComIB943Upload.ctn_qty,
                            @tot_ctn = objeComIB943Upload.ctns,
                            @tot_qty = objeComIB943Upload.itm_qty,
                            @rcvd_ctn = "0",
                            @rcvd_qty = "0",
                            @locid = objeComIB943Upload.loc_id,
                            @po_num = objeComIB943Upload.po_num,
                            @iorate = objeComIB943Upload.io_rate_id,
                            @strgrate = objeComIB943Upload.st_rate_id,
                            @length = objeComIB943Upload.ctn_length,
                            @width = objeComIB943Upload.ctn_width,
                            @depth = objeComIB943Upload.ctn_height,
                            @wgt = objeComIB943Upload.ctn_wgt,
                            @cube = objeComIB943Upload.ctn_cube,
                            @process_id = "ADD",
                            @lot_id = ""

                        }, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertDetailstoAuditTrail(eComIB943Upload objeComIB943Upload)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "Sp_ib_insert_audit_trail";
                    connection.Execute(storedProcedure2,
                      new
                        {
                            @p_str_cmp_id = objeComIB943Upload.cmp_id,
                            @p_str_ib_doc_id = objeComIB943Upload.ib_cntr_upld_doc_id,
                            @p_str_mode = "INPUT",
                            @p_str_maker = objeComIB943Upload.user_id,
                            @p_dt_maker_dt = DateTime.Now.ToString("MM/dd/yyyy"),
                            @p_dt_access_stamp = DateTime.Now.ToString("MM/dd/yyyy"),
                            @p_str_audit_comments = "Added new Inbound entry",
                        },
                        commandType: CommandType.StoredProcedure);
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


        public int Get943UploadRefNum(string p_str_cmp_id)
        {

            try
            {
                int l_int_upload_ref_num = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_IB_943_GET_UPLOAD_REFNUM", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //  command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                connection.Open();
                l_int_upload_ref_num = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_upload_ref_num > 0)
                {
                    return l_int_upload_ref_num;
                }
                else
                {
                    return l_int_upload_ref_num;
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

     

        public eComIB943Upload GetIB943UploadTempHdrtblDetails(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_get_ib_cntr_upld_temp_hdr_tbl_dtls";
                    IList<eComIB943Upload> ListIRFP = connection.Query<eComIB943Upload>(SearchCustDtls, new
                    {
                        @P_STR_COMP_ID = objeComIB943Upload.cmp_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComIB943Upload.ListeCom940IB943UploadHdr = ListIRFP.ToList();
                }
                return objeComIB943Upload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComIB943Upload GetIB943UploadTempDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_get_ib_cntr_upld_temp_dtl_tbl_dtls";
                    IList<eComIB943Upload> ListIRFP = connection.Query<eComIB943Upload>(SearchCustDtls, new
                    {
                        @P_STR_COMP_ID = objeComIB943Upload.cmp_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComIB943Upload.ListeCom940IB943UploadDtl = ListIRFP.ToList();
                }
                return objeComIB943Upload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComIB943Upload CheckIBUploadedStyleExist(eComIB943Upload objeComIB943Upload)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();
                string l_str_Itm_Code = string.Empty;
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (IDbTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {

                            // transactional code...
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "sp_get_item_code";
                                cmd.Parameters.Add("@P_STR_COMP_ID", SqlDbType.VarChar).Value = objeComIB943Upload.cmp_id;
                                cmd.Parameters.Add("@P_STR_STYLE", SqlDbType.VarChar).Value = (objeComIB943Upload.itm_num == null || objeComIB943Upload.itm_num == "") ? null : objeComIB943Upload.itm_num;
                                cmd.Parameters.Add("@P_STR_COLOR", SqlDbType.VarChar).Value = (objeComIB943Upload.itm_color == null || objeComIB943Upload.itm_color == "") ? null : objeComIB943Upload.itm_color ;
                                cmd.Parameters.Add("@P_STR_SIZE", SqlDbType.VarChar).Value = (objeComIB943Upload.itm_size == null || objeComIB943Upload.itm_size == "") ? null : objeComIB943Upload.itm_size;
                                cmd.Parameters.Add("@P_STR_DESC", SqlDbType.VarChar).Value = (objeComIB943Upload.itm_name == null || objeComIB943Upload.itm_name == "") ? null : objeComIB943Upload.itm_name;
                                cmd.Parameters.Add("@P_STR_LENGTH", SqlDbType.VarChar).Value = objeComIB943Upload.ctn_length;
                                cmd.Parameters.Add("@P_STR_WIDTH", SqlDbType.VarChar).Value = objeComIB943Upload.ctn_width;
                                cmd.Parameters.Add("@P_STR_HEIGHT", SqlDbType.VarChar).Value = objeComIB943Upload.ctn_height;
                                cmd.Parameters.Add("@P_STR_CUBE", SqlDbType.VarChar).Value = objeComIB943Upload.ctn_cube;
                                cmd.Parameters.Add("@P_STR_WGT", SqlDbType.VarChar).Value = objeComIB943Upload.ctn_wgt;
                                cmd.Parameters.Add("@P_STR_CTN_QTY", SqlDbType.VarChar).Value = objeComIB943Upload.ctn_qty;
                                cmd.Parameters.Add("@P_STR_ITEM_CODE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                                l_str_Itm_Code = cmd.Parameters["@P_STR_ITEM_CODE"].Value.ToString();
                                objeComIB943Upload.Itm_Code = l_str_Itm_Code;


                            }

                            tran.Commit();

                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                }


                return objeComIB943Upload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }



        }
        public eComIB943Upload DeleteTempTable(eComIB943Upload eComIB943Upload)
        {
            try
            {
             
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_delete_ib_cntr_upld_temp_tbl_dtls";
                    IList<eComIB943Upload> ListIRFP = connection.Query<eComIB943Upload>(SearchCustDtls, new
                    {
                        @P_STR_COMP_ID = eComIB943Upload.cmp_id,


                    }, commandType: CommandType.StoredProcedure).ToList();
                 
                }
                return eComIB943Upload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void DeleteSingleLineCntrUploadTempTable(eComIB943Upload eComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_delete_ib_cntr_single_line_upld_temp_tbl_dtls";
                    connection.Execute(SearchCustDtls, new
                    {
                        @P_STR_COMP_ID = eComIB943Upload.cmp_id,
                    }, commandType: CommandType.StoredProcedure);

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

        public eComIB943Upload AddECom943UploadFileNAme(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_insert_upld_file_name_to_tbl";
                    IList<eComIB943Upload> ListIRFP = connection.Query<eComIB943Upload>(SearchCustDtls, new
                    {
                        @P_STR_COMP_ID = objeComIB943Upload.cmp_id,
                        @P_STR_FILE_NAME = (objeComIB943Upload.file_name == null || objeComIB943Upload.file_name == "") ? null : objeComIB943Upload.file_name,
                        @P_STR_USER_ID = (objeComIB943Upload.user_id == null || objeComIB943Upload.user_id == "") ? null : objeComIB943Upload.user_id,
                        @P_STR_UPLOADEDDATE = DateTime.Now.ToString("MM/dd/yyyy"),
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComIB943Upload.ListeCom940IB943UploadDtl = ListIRFP.ToList();
                }
                return objeComIB943Upload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
       

        public void InsertIB943SingleLineUploadTempDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_insert_ib_cntr_single_line_upld_temp_dtl_tbl_dtls";
                    connection.Execute(SearchCustDtls,
                     new
                     {
                         @P_STR_COMP_ID = objeComIB943Upload.cmp_id,
                         @P_STR_IB_DOC_ID = objeComIB943Upload.ib_cntr_upld_doc_id,
                         @P_STR_CNTR_ID = objeComIB943Upload.cntr_id,
                         @P_STR_PO_NUM = objeComIB943Upload.po_num,
                         @P_STR_DTL_LINE = objeComIB943Upload.dtl_line,
                         @P_STR_ITM_NUM = objeComIB943Upload.itm_num,
                         @P_STR_ITM_COLOR = objeComIB943Upload.itm_color,
                         @P_STR_ITM_SIZE = objeComIB943Upload.itm_size,
                         @P_STR_ITM_NAME = objeComIB943Upload.itm_name,
                         @P_STR_ITM_QTY = objeComIB943Upload.itm_qty,
                         @P_STR_CTN_QTY = objeComIB943Upload.ctn_qty,
                         @P_STR_CTNS = objeComIB943Upload.ctns,
                         @P_STR_LOC_ID = objeComIB943Upload.loc_id,
                         @P_STR_ST_RATE_ID = objeComIB943Upload.io_rate_id,
                         @P_STR_IO_RATE_ID = objeComIB943Upload.st_rate_id,
                         @P_STR_CTN_LENGTH = objeComIB943Upload.ctn_length,
                         @P_STR_CTN_WIDTH = objeComIB943Upload.ctn_width,
                         @P_STR_CTN_HEIGHT = objeComIB943Upload.ctn_height,
                         @P_STR_CTN_CUBE = objeComIB943Upload.ctn_cube,
                         @P_STR_CTN_WGT = objeComIB943Upload.ctn_wgt,
                         @P_STR_DTL_NOTES = objeComIB943Upload.dtl_notes,
                         @P_STR_ETA_DT = objeComIB943Upload.eta_date,
                         @P_STR_ENTRY_DT = objeComIB943Upload.entry_dt,
                     }, commandType: CommandType.StoredProcedure);

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


        public eComIB943Upload GetIB943SingleLineUploadTempDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_get_ib_cntr_single_line_upld_temp_dtl_tbl_dtls";
                    IList<eComIB943Upload> ListIRFP = connection.Query<eComIB943Upload>(SearchCustDtls, new
                    {
                        @P_STR_COMP_ID = objeComIB943Upload.cmp_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComIB943Upload.ListeCom940IB943UploadDtl = ListIRFP.ToList();
                }
                return objeComIB943Upload;
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
