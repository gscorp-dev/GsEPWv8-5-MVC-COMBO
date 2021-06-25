using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////CR-20180217 -Added by nithya for Doc_ctn table value passing value Changes

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class InboundInquiryRepository : IInboundInquiryRepository
    {

        public string fnCheckCheckBinRefered(string pstrCmpId, string pstrBinId, string pstrItmNum, string pstrItmColor, string pstrItmSize, string pstrItmCode)
        {
            try
            {
                string lstrBinItem = string.Empty;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spCheckBinRefered", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                command.Parameters.Add("@pstrBinLocId", SqlDbType.VarChar).Value = pstrBinId;
                command.Parameters.Add("@pstrItmNum", SqlDbType.VarChar).Value = pstrItmNum;
                command.Parameters.Add("@pstrItmColor", SqlDbType.VarChar).Value = pstrItmColor;
                command.Parameters.Add("@pstrItmSize", SqlDbType.VarChar).Value = pstrItmSize;
                command.Parameters.Add("@pstrItmCode", SqlDbType.VarChar).Value = pstrItmCode;
                connection.Open();
                DataTable dtBinItem = new DataTable();
                dtBinItem.Load(command.ExecuteReader());

                if (dtBinItem.Rows.Count ==0)
                {
                    lstrBinItem = "NO";
                }
                else
                {
                    lstrBinItem = "Bin Already refered for the Style " + dtBinItem.Rows[0][0].ToString();
                }

                return lstrBinItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public Login LoginAuthentication(Login objLogin)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    IEnumerable<Login> objLoginData = connection.Query<Login>("SpLogOnUserCkmvc", new
                    {
                        @user_id = objLogin.Email,
                        @passwd = objLogin.Password
                    }, commandType: CommandType.StoredProcedure);
                    if (objLoginData.Count() == 0)
                    {
                        return null;
                    }
                    return objLoginData.ToList().First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public InboundInquiry GetDocuEntryTempGridDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_temp_doc_entry";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListDocdtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetDocumentEntryTempGridDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_temp_doc_entry";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListDocuEntrydtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetCheckExistGridData(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                    {
                    const string SearchCustDtls = "proc_get_mvcweb_temptable_docentry_exist_item";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ibdocid = objInboundInquiry.ib_doc_id,
                        @line_num = objInboundInquiry.line_num,
                        @itm_code = objInboundInquiry.itm_code,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGridEditData = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetRcvngHdr(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_RecvgHdr";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @P_Str_Cmp_Id = objInboundInquiry.cmp_id,
                        @P_Str_Ibdoc_Id = objInboundInquiry.ibdocid,
                        @P_Str_Cntr_Id = objInboundInquiry.cntr_id,                      

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListRcvgHdr = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetRcvngPostDtls(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_RcvgPostDtls";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @P_Str_Cmp_Id = objInboundInquiry.cmp_id,
                        @P_Str_IbDoc_Id = objInboundInquiry.ibdocid,
                        @P_Str_Cont_Id = objInboundInquiry.cntr_id,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListRcvgPost = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetRcvngUnPostDtls(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_RCVG_UNPOST_GRID_DTLS";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                        @P_STR_IB_DOC_ID = objInboundInquiry.ibdocid,
                      

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListRcvgUnPost = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        
        public InboundInquiry LoadAvailDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_LoadAvailDtl";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @P_Str_Cmp_Id = objInboundInquiry.cmp_id,
                        @P_Str_Cont_Id = objInboundInquiry.cntr_id,
                        @P_Str_IbDoc_Id = objInboundInquiry.ibdocid,
                       

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListAvailDtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry LoadLotItem(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_LoadLotItm";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @P_Str_Cmp_Id = objInboundInquiry.cmp_id,                       
                        @P_Str_IbDoc_Id = objInboundInquiry.ibdocid,


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListLotItem = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetRcvngGridDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_RecvgDtl";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @P_Str_Cmp_Id = objInboundInquiry.cmp_id,
                        @P_Str_Ibdoc_Id = objInboundInquiry.ibdocid,
                      

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListRcvgDtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetGridDeleteData(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_temptable_docentry_grid_delete_item";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ibdocid = objInboundInquiry.ib_doc_id,
                        @line_num = objInboundInquiry.line_num,                       
                        @itm_code = objInboundInquiry.itm_code,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListDocuEntrydtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
       
        public InboundInquiry GetGridEditData(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_Temptable_DocEntry_edit";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @Docentry = objInboundInquiry.doc_entry_id,
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ibdocid = objInboundInquiry.ib_doc_id,
                        @line_num = objInboundInquiry.line_num,
                        @itm_code = objInboundInquiry.itm_code,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGridEditData = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetItemHdr(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_itemhdr";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @p_str_cmp_id = objInboundInquiry.cmp_id,
                        @p_str_itm_num = objInboundInquiry.itm_num,
                        @p_str_itm_color = objInboundInquiry.itm_color,
                        @p_str_itm_size = objInboundInquiry.itm_size,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGetItmhdr = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public InboundInquiry CheckItmHdr(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_check_itm_hdr";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @itm_num = objInboundInquiry.itm_num,
                        @itm_color = objInboundInquiry.itm_color,
                        @itm_size = objInboundInquiry.itm_size,
                        //@itm_name = objInboundInquiry.itm_name,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListStyle = ListIRFP.ToList();
                    if (objInboundInquiry.ListStyle.Count == 0)
                    {
                        objInboundInquiry.Check_exist_itm_count = "";
                    }
                    else
                    {
                        objInboundInquiry.Check_exist_itm_count = "1";
                    }
                    
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry CheckItmDimension(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_check_itmdimension_dtl";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @itm_num = objInboundInquiry.itm_num,
                        @itm_color = objInboundInquiry.itm_color,
                        @itm_size = objInboundInquiry.itm_size,                      

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListDimension = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry CanPost(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "PROC_GET_MVC_WEB_CANPOST";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                        @P_STR_PALET_ID = objInboundInquiry.palet_id,
                       

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListCanPost = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void Add_To_proc_save_doc_hdr(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_insert_doc_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                        @status = "OPEN",
                        @step = "OPEN",
                        @ordr_type = "REG",
                        @ib_doc_dt = objInboundInquiry.ib_doc_dt,
                        @vend_id = objInboundInquiry.recvd_fm,
                        @vend_name = objInboundInquiry.recvd_fm,
                        @po_num = objInboundInquiry.po_num,
                        @ordr_dt = objInboundInquiry.orderdt,
                        @req_num = objInboundInquiry.refno,
                        @cntr_id = objInboundInquiry.cont_id,
                        @fob = "1000000001",
                        @billto_id = "10001",
                        @ship_from = "10001",
                        @shipto_id = "10001",
                        @dept_id = "10001",
                        @shipvia_id = objInboundInquiry.recvdvia,
                        @freight_id = "111111",
                        @terms_id = "10001",
                        @note = objInboundInquiry.Note,
                        @process_id = "ADD",
                        @eta_dt = objInboundInquiry.eta_dt,
                        @rma_flag = "",
                        @master_bol = objInboundInquiry.bol,
                        @vessel_no = objInboundInquiry.vessel_num

                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void ReceivingPostDtls(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvc_web_ib_DocPost_Upd_LotInHst_Add_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @CMP_ID = objInboundInquiry.cmp_id,
                        @IB_DOC_ID = objInboundInquiry.ibdocid,
                        @STATUS = "TEMP",
                        @RETURNVALUE = "516",


                    }, commandType: CommandType.StoredProcedure);
            }
        }

        public bool AddAutoSpecialIBSEntry(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id)
        {
       
            try
            { 

            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_add_auto_ibs_entry";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @P_STR_COMP_ID = p_str_cmp_id,
                        @P_STR_IB_DOC_ID = p_str_ib_doc_id,
                        @P_STR_CNTR_ID = p_str_cntr_id


                    }, commandType: CommandType.StoredProcedure);
            }
                return true;
        }
            catch(Exception ex)
            {
             
                return false;
                throw ex;
            }

    }
        public bool AddAutoIBSEntry(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id)
        {

            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "spIBAddAutoIBSEntry";
                    connection.Execute(storedProcedure1,
                        new
                        {

                            @P_STR_COMP_ID = p_str_cmp_id,
                            @P_STR_IB_DOC_ID = p_str_ib_doc_id,
                            @P_STR_CNTR_ID = p_str_cntr_id


                        }, commandType: CommandType.StoredProcedure);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
                throw ex;
            }

        }

        public void DocReceivingUnPost(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_IB_DOCRCVG_UNPOST";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ibdocid,
                        @lot_id = objInboundInquiry.lot_id,
                        @RETURNVALUE = "5",


                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void ReceivingPost9999Dtls(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvc_web_ib_DocPost_Upd_LotInHst_Add_hdr_9999";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @CMP_ID = objInboundInquiry.cmp_id,
                        @IB_DOC_ID = objInboundInquiry.ibdocid,
                        @LOT_ID = objInboundInquiry.lot_id,
                        @PALET_ID = objInboundInquiry.palet_id,
                        @PLOC_ID = objInboundInquiry.loc_id,
                        @STATUS = "TEMP",
                        @RETURNVALUE = "516",


                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public InboundInquiry GetInboundInquiryDetails(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
          
                    var cmp_id = objInboundInquiry.cmp_id;
                    var l_str_status = objInboundInquiry.status;
                    var tmp_str_status = String.Empty;
                    if (l_str_status == "ALL")
                    {
                        tmp_str_status = "";
                    }
                    else
                    {
                        tmp_str_status = l_str_status;
                    }

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();
                    
                    const string storedProcedure2 = "proc_get_mvc_doc_inquiry_dtls";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @cntr_id = objInboundInquiry.cntr_id,
                            @status = tmp_str_status,
                            @ib_doc_id_fm = objInboundInquiry.ib_doc_id_fm,
                            @ib_doc_id_to = objInboundInquiry.ib_doc_id_to,
                            @req_num = objInboundInquiry.req_num,
                            @ib_doc_dt_fm = objInboundInquiry.ib_doc_dt_fm,
                            @ib_doc_dt_to = objInboundInquiry.ib_doc_dt_to,
                            @ib_rcvd_dt_fm = objInboundInquiry.ib_rcvd_dt_fm,
                            @ib_rcvd_dt_to = objInboundInquiry.ib_rcvd_dt_to,
                            @eta_dt_fm = objInboundInquiry.eta_dt_fm,
                            @eta_dt_to = objInboundInquiry.eta_dt_to,
                            @rcvd_frm = objInboundInquiry.vend_name,

                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_size = objInboundInquiry.itm_size,
                        },null,true,60,
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListInboundDocInquiry = ListInbound.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetPickStyleDetails(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_loadStyledtl";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {

                        @cmp_id = objInboundInquiry.cmp_id

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListItmStyledtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GEtStrgBillTYpe(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_strg_bill_type";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {

                        @cmp_id = objInboundInquiry.cmp_id,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListStrgBillType = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public InboundInquiry LoadStrgId(InboundInquiry objInboundInquiry)
        {
            string l_str_bill_tye = string.Empty;
            l_str_bill_tye = objInboundInquiry.bill_type;
            if (l_str_bill_tye == "Location")
            {
                objInboundInquiry.bill_type = "Loc";
            }
          
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_load_StrgId";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {

                        @cmp_id = objInboundInquiry.cmp_id,
                        @catg = objInboundInquiry.bill_type

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.LstStrgIddtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry LoadInoutId(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_load_InoutId";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {

                        @cmp_id = objInboundInquiry.cmp_id,
                        @catg = objInboundInquiry.bill_inout_type,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.LstInoutIddtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry Getitmlist(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_Itmlist";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {

                        @cmp_id = objInboundInquiry.cmp_id,
                        @itm_num = objInboundInquiry.itm_num,
                        @itm_color = objInboundInquiry.itm_color,
                        @itm_size = objInboundInquiry.itm_size,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetInboundAckRptDetails(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_inbound_ack_rpt";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListAckRptDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry DocTallySheetRpt(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_DOC_TALLY_SHEET_RPT";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_LOT_ID = objInboundInquiry.lot_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocTallySheetRpt = ListEShip.ToList();

                    return objInboundInquiry;
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
        
        public InboundInquiry GetInboundWorkSheetRptDetails(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_inbound_worksheet_rpt";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListWorkSheetRptDetails = ListEShip.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry GetInboundTallySheetRptDetails(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_inbound_tally_rpt";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListTallySheetRptDetails = ListInbound.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetInboundConfirmationRptDetails(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_inbound_comfirmation_rpt";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListConfirmationRptDetails = ListInbound.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetInboundConfirmationRptDetailsbyContainer(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_inbound_comfirmation_rpt_by_container";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListConfirmationRptDetails = ListInbound.ToList();

                    return objInboundInquiry;
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
        
        public InboundInquiry GetInboundGridSummaryDetails(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objInboundInquiry.cmp_id;
                    var l_str_status = objInboundInquiry.status;
                    var tmp_str_status = String.Empty;
                    if (l_str_status == "ALL")
                    {
                        tmp_str_status = "";
                    }
                    else
                    {
                        tmp_str_status = l_str_status;
                    }

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_grid_summary_rpt";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @cntr_id = objInboundInquiry.cntr_id,
                            @status = tmp_str_status,
                            @ib_doc_id_fm = objInboundInquiry.ib_doc_id_fm,
                            @ib_doc_id_to = objInboundInquiry.ib_doc_id_to,
                            @req_num = objInboundInquiry.req_num,
                            @ib_doc_dt_fm = objInboundInquiry.ib_doc_dt_fm,
                            @ib_doc_dt_to = objInboundInquiry.ib_doc_dt_to,
                            @eta_dt_fm = objInboundInquiry.eta_dt_fm,
                            @eta_dt_to = objInboundInquiry.eta_dt_to,
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_size = objInboundInquiry.itm_size,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGridSummaryRptDetails = ListInbound.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetInboundStatus(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objInboundInquiry.cmp_id;

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_inbound_status";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListInboundStatusRptDetails = ListInbound.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry GetInboundHdrDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_mvcweb_inbound_Receive_hdr";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundInquiry.CompID,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListAckRptDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetInboundLotHdr(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_mvcweb_inbound_lot_Receive_hdr";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundInquiry.cmp_id,
                            @lot_id = objInboundInquiry.lot_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListAckRptDetails = ListEShip.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry GetInboundScanHdr(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_mvcweb_inbound_lot_Receive_hdr";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundInquiry.cmp_id,
                            @lot_id = objInboundInquiry.lot_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListAckRptDetails = ListEShip.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry GetInboundDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_webmvc_Inbound_receive_details";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListAckRptDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetInboundLotDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_webmvc_Inbound_Lot_details";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundInquiry.CompID,
                            @lot_id = objInboundInquiry.lot_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListAckRptDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry LoadCustConfig(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "PROC_GET_MVCWEB_LOAD_CUST_CONFIG";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListCustConfigDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry LoadCustConfigRcvdItmMode(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "PROC_GET_MVCWEB_LOAD_CUST_CONFIG_RCVD_ITM_MODE";
                    IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListCustConfigRcvdItmModeDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetIbDocIdDetail(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_ib_doc_id";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @ibdocid = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListIbDocId = ListIbDocIdLIST.ToList();
                    objInboundInquiry.ibdocid = objInboundInquiry.ListIbDocId[0].ibdocid;

                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public InboundInquiry GetItmId(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_itm_id";
                    IEnumerable<InboundInquiry> ListItmIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @itmid = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListItmId = ListItmIdLIST.ToList();
                    objInboundInquiry.itm_code = objInboundInquiry.ListItmId[0].itmid;

                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public InboundInquiry GetDocHdr(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_doc_hdr";
                    IEnumerable<InboundInquiry> ListDocHdrLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocHdr = ListDocHdrLIST.ToList();
                   

                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public InboundInquiry GetDocDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_doc_dtl";
                    IEnumerable<InboundInquiry> ListDocDtlLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocDtl = ListDocDtlLIST.ToList();


                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public void InsertTempDocEntryDetails(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_Insert_Temptable_DocEntry";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @DocEntry = objInboundInquiry.doc_entry_id,
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                        @line_num = objInboundInquiry.line_num,
                        @itm_num = objInboundInquiry.itm_num,
                        @itm_color = objInboundInquiry.itm_color,
                        @itm_size = objInboundInquiry.itm_size,
                        @itm_name = objInboundInquiry.itm_name,
                        @ppk = objInboundInquiry.docuppk,
                        @po_num = objInboundInquiry.po_num,
                        @ctns = objInboundInquiry.ctn,
                        @loc_id = objInboundInquiry.loc_id,
                        @strg_rate = objInboundInquiry.strg_rate,
                        @inout_rate = objInboundInquiry.inout_rate,
                        @ordr_qty = objInboundInquiry.ord_qty,
                        @note = objInboundInquiry.note,
                        @length = objInboundInquiry.doclength,
                        @width = objInboundInquiry.docwidth,
                        @height = objInboundInquiry.docheight,
                        @weight = objInboundInquiry.docweight,
                        @cube = objInboundInquiry.doccube,
                        @itm_qty = objInboundInquiry.docuppk,
                        @itm_code = objInboundInquiry.itm_code,
                        @ctn_line = objInboundInquiry.ctn_line,
                        @factory_id = objInboundInquiry.factory_id,
                        @cust_name = objInboundInquiry.cust_name,
                        @cust_po_num = objInboundInquiry.cust_po_num,
                        @pick_list = objInboundInquiry.pick_list

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void Update_doc_hdr(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_ib_updt_doc_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                       
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                        @cntr_id = objInboundInquiry.cont_id,
                        @vend_id = objInboundInquiry.recvd_fm,
                        @vend_name = objInboundInquiry.recvd_fm,
                        @req_num = objInboundInquiry.refno,
                        @ordr_dt = objInboundInquiry.ordr_dt,
                        @shipvia_id = objInboundInquiry.recvdvia,
                        @note = objInboundInquiry.recvd_fm,
                        @process_id = "",
                        @eta_dt = objInboundInquiry.eta_dt,
                        @master_bol = objInboundInquiry.bol,
                        @vessel_no = objInboundInquiry.vessel_num

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void Add_Style_To_Itm_dtl(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_Add_ma_itm_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {                     
                        @cmp_id = objInboundInquiry.cmp_id,
                        @itm_id = objInboundInquiry.itm_code,
                        @itm_num = objInboundInquiry.itm_num,
                        @itm_color = objInboundInquiry.itm_color,
                        @itm_size = objInboundInquiry.itm_size,
                        @wgt = objInboundInquiry.weight,
                        @length = objInboundInquiry.length,
                        @width = objInboundInquiry.width,
                        @depth = objInboundInquiry.height,
                        @cube = objInboundInquiry.cube,
                        @avail_qty = "0",
                        @aloc_qty = "0",
                        @ship_qty = "0",
                        @resrv_qty = "0",
                        @bo_qty = "0",
                        @min_qty = "0",
                        @max_qty = "0",
                        @ctn_qty = objInboundInquiry.ctn_qty,
                        @min_val = "0",
                        @max_val = "0",
                        @process_id = "",
                        @flag = objInboundInquiry.flag

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void Add_Style_To_Itm_hdr(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_Add_ma_itm_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                       
                        @cmp_id = objInboundInquiry.cmp_id,
                        @itm_id = objInboundInquiry.itm_code,
                        @itm_num = objInboundInquiry.itm_num,
                        @itm_color = objInboundInquiry.itm_color,
                        @itm_size = objInboundInquiry.itm_size,
                        @itm_name = objInboundInquiry.itm_name,
                        @kit_itm = "N",
                        @process_id = "MassConv",

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public InboundInquiry GetCSVList(InboundInquiry objInboundInquiry)
        {

            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_webmvc_tempupload_docdtl_fetch";
                IEnumerable<InboundInquiry> ListEcomRecieveOrdersRole = connection.Query<InboundInquiry>(storedProcedure1,
                    new
                    {


                    },
                    commandType: CommandType.StoredProcedure);
                objInboundInquiry.lstobjInboundInq = ListEcomRecieveOrdersRole.ToList();
            }

            return objInboundInquiry;
        }
        public void Add_To_proc_save_doc_ctn(InboundInquiry objInboundInquiry)
        {
            try
            {

          
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_insert_log_doc_ctn";
                connection.Execute(storedProcedure1,
                    new
                    {
                        
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                        @dtl_line = objInboundInquiry.LineNum,
                        @ctn_line = objInboundInquiry.ctn_line,
                        @itm_line = objInboundInquiry.ctn_line,//CR-20180217
                        @kit_id = objInboundInquiry.itm_code, 
                        @itm_code = objInboundInquiry.itm_code,
                        @itm_qty = objInboundInquiry.docuppk,
                        @ctn_qty = objInboundInquiry.docuppk,
                        @tot_ctn = objInboundInquiry.ctn_qty,
                        @tot_qty = objInboundInquiry.ord_qty,
                        @rcvd_ctn = "0",
                        @rcvd_qty = "0",
                        @locid = objInboundInquiry.loc_id,
                        @po_num = objInboundInquiry.po_num,
                        @iorate = objInboundInquiry.inout_rate,
                        @strgrate = objInboundInquiry.strg_rate,
                        @length = objInboundInquiry.length,
                        @width = objInboundInquiry.width,
                        @depth = objInboundInquiry.height,
                        @wgt = objInboundInquiry.weight,
                        @cube = objInboundInquiry.cube,
                        @process_id = "ADD",
                        @lot_id = ""

                    }, commandType: CommandType.StoredProcedure);

            }
        }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void Add_To_proc_save_doc_dtl(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_insert_Doc_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                       
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                        @line_num = objInboundInquiry.LineNum,
                        @status = "OPEN",
                        @step = "OPEN",
                        @itm_code = objInboundInquiry.itm_code,
                        @itm_num = objInboundInquiry.itm_num,
                        @itm_color = objInboundInquiry.itm_color,
                        @itm_size = objInboundInquiry.itm_size,
                        @itm_name = objInboundInquiry.itm_name,
                        @vend_itm_num = objInboundInquiry.itm_num,
                        @vend_itm_color = objInboundInquiry.itm_color,
                        @vend_itm_size = objInboundInquiry.itm_size,
                        @ordr_ctn = objInboundInquiry.ctn_qty,
                        @ordr_qty = objInboundInquiry.ord_qty,
                        @rcvd_ctn = "0",
                        @rcvd_qty = "0",
                        @qty_uom = "EA",
                        @back_ordr_qty = "0",
                        @price = "0",
                        @price_uom = "EA",
                        @list_price = "0",
                        @list_uom = "EA",
                        @pack_id = "PF",
                        @note = objInboundInquiry.note,
                        @process_id = "ADD",
                        @factory_id = objInboundInquiry.factory_id,
                        @po_num = objInboundInquiry.po_num,
                        @cust_name = objInboundInquiry.cust_name,
                        @cust_po_num = objInboundInquiry.cust_po_num,
                        @pick_list = objInboundInquiry.pick_list,


                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void DeleteDocEntry(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_ib_doc_del";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,                       
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void Del_doc_Dtl(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvcweb_ib_del_doc_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        //public InboundInquiry GetDocEntryTempGridDtl(InboundInquiry objInboundInquiry)
        //{
        //    try
        //    {
        //        using (IDbConnection connection = ConnectionManager.OpenConnection())
        //        {
        //            const string storedProcedure2 = "proc_mvc_web_temp_doc_entry_dtl";
        //            IEnumerable<InboundInquiry> ListDocEntryDtlLIST = connection.Query<InboundInquiry>(storedProcedure2,
        //                new
        //                {




        //                },
        //                commandType: CommandType.StoredProcedure);
        //            objInboundInquiry.ListDocEntryDtl = ListDocEntryDtlLIST.ToList();

        //            return objInboundInquiry;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {

        //    }
        //}
        public void TruncateTempDocEntry(InboundInquiry objInboundInquiry)
        {
           
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_mvc_temp_doc_entry_truncate";
                    connection.Execute(storedProcedure1,                  
                        new
                        {


                        },
                        commandType: CommandType.StoredProcedure);                  
                }
                   
        }
        public void TruncateTempDocUpload(InboundInquiry objInboundInquiry)
        {

            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_web_mvc_tempdocupload_truncate";
                connection.Execute(storedProcedure1,
                    new
                    {


                    },
                    commandType: CommandType.StoredProcedure);
            }

        }
        public InboundInquiry GetDocEntryId(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_doc_entry_id";
                    IEnumerable<InboundInquiry> ListDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @doc_entry_id = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocId = ListDocIdLIST.ToList();
                    objInboundInquiry.doc_entry_id = objInboundInquiry.ListDocId[0].doc_entry_id;

                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry IsRMAChecked(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "PROC_GET_MVCWEB_ISRMA_CHECKED";
                    IEnumerable<InboundInquiry> ListDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_ENTITY_CODE = "rma_doc_id",
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListRMADocId = ListDocIdLIST.ToList();
                 //   objInboundInquiry.ib_doc_id = objInboundInquiry.ListRMADocId[0].ct_value;

                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry ItemXGetitmDetails(string term, string cmp_id)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_Itm_dtl";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmp_id,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objCustDtl.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objCustDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
     
        public InboundACKExcel GetInboundAckExcel(InboundACKExcel objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundACKExcel objOrderLifeCycleCategory = new InboundACKExcel();

                    const string storedProcedure2 = "proc_get_mvcweb_ib_excel_ack";
                    IEnumerable<InboundACKExcel> ListEShip = connection.Query<InboundACKExcel>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.PONo,
                            @StrDocNum = objInboundInquiry.Style,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListInboundAckExcelDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundWorkSheetExcel GetInboundWorkSheetExcel(InboundWorkSheetExcel objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundWorkSheetExcel objOrderLifeCycleCategory = new InboundWorkSheetExcel();

                    const string storedProcedure2 = "proc_get_mvcweb_ib_excel_worksheet";
                    IEnumerable<InboundWorkSheetExcel> ListEShip = connection.Query<InboundWorkSheetExcel>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.PONo,
                            @StrDocNum = objInboundInquiry.Style,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListInboundWorkSheetExcelDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundTallySheetExcel GetInboundTallySheetExcel(InboundTallySheetExcel objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundTallySheetExcel objOrderLifeCycleCategory = new InboundTallySheetExcel();

                    const string storedProcedure2 = "proc_get_mvcweb_ib_excel_TallySheet";
                    IEnumerable<InboundTallySheetExcel> ListEShip = connection.Query<InboundTallySheetExcel>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.PoNum,
                            @StrDocNum = objInboundInquiry.Style,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListInboundTallySheetExcelDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundConfirmationExcel GetInboundConfimExcel(InboundConfirmationExcel objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundConfirmationExcel objOrderLifeCycleCategory = new InboundConfirmationExcel();

                    const string storedProcedure2 = "proc_get_mvcweb_ib_excel_Conformation";
                    IEnumerable<InboundConfirmationExcel> ListEShip = connection.Query<InboundConfirmationExcel>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.PoNum,
                            @StrDocNum = objInboundInquiry.Style,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListInboundConfrmExcelDetails = ListEShip.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetPalletId(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_pallet_id";
                    IEnumerable<InboundInquiry> ListPaletIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @palet_id = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPaletId = ListPaletIdLIST.ToList();
                    objInboundInquiry.palet_id = objInboundInquiry.ListPaletId[0].palet_id;

                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public InboundInquiry GetLotId(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_ib_doc_id";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @ibdocid = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListIbDocId = ListIbDocIdLIST.ToList();
                    objInboundInquiry.ibdocid = objInboundInquiry.ListIbDocId[0].ibdocid;

                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public InboundInquiry GetInsertTblIbDocDtlTmp(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    //InboundTallySheetExcel objOrderLifeCycleCategory = new InboundTallySheetExcel();

                    const string storedProcedure2 = "proc_get_mvcweb_insert_into_tbl_ib_doc_dtl_tmp";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListInsertTblIbDocDtlTmp = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetCustRcvdItemMode(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    //InboundTallySheetExcel objOrderLifeCycleCategory = new InboundTallySheetExcel();

                    const string storedProcedure2 = "proc_get_mvc_web_cust_recv_itm_mode";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGetCustConfigRcvdItemMode = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetInsertTblIbDocRecvDtlTemp(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    //InboundTallySheetExcel objOrderLifeCycleCategory = new InboundTallySheetExcel();

                    const string storedProcedure2 = "proc_get_mvc_web_insert_tbl_ib_doc_recv_dtl_temp";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_ibdoc_id = objInboundInquiry.ibdocid,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListRecvdDetails = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetDocRecvEntrySave(InboundInquiry objInboundInquiry)
        {
            SqlDateTime sqldatenull;
            sqldatenull = SqlDateTime.Null;
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_IB_SAVE_IB_RECEIVING_BY_IB_DOC_ID";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_ib_doc_id = objInboundInquiry.ib_doc_id,
                            @p_str_ib_rcvd_dt = objInboundInquiry.ib_doc_dt,
                            @p_str_rcvd_from = objInboundInquiry.rcvd_from,
                            @p_str_refno = objInboundInquiry.refno,
                            @p_str_vend_id = objInboundInquiry.vend_id,
                            @p_str_whs_id = objInboundInquiry.whs_id,
                            @p_str_cont_id = objInboundInquiry.cont_id,
                            @p_str_seal_num = objInboundInquiry.seal_num,
                            @p_str_palet_id = objInboundInquiry.palet_id,
                            @p_str_lot_id = objInboundInquiry.lot_id,
                            @p_str_st_rate = objInboundInquiry.seal_num,
                            @p_str_io_rate = objInboundInquiry.palet_id,
                            @p_str_kit_itm_id = "",
                            @p_str_cntr_type = objInboundInquiry.cntr_type,
                            @p_str_ib_load_dt = objInboundInquiry.ib_load_dt,
                        },
                        commandType: CommandType.StoredProcedure,commandTimeout:0);
                    objInboundInquiry.ListSaveDocRecvdDetails = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public void InsertTempFileDocument(InboundInquiry objInboundInquiry)
        {
            try
            {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                    const string storedProcedure1 = "SP_MVC_INSERT_UPLOAD_DOCUMENT_WEB";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ibdocid.Trim(),
                        @P_STR_DOCTYPE = objInboundInquiry.doctype,
                        @P_STR_FILENAME = objInboundInquiry.Filename,
                        @P_STR_FILEPATH = objInboundInquiry.filepath,
                        @P_STR_UPLOADDT = objInboundInquiry.Uploaddt,
                        @P_STR_UPLOADBY = objInboundInquiry.Uploadby,
                        @P_STR_COMMENT = objInboundInquiry.Comments,
                        //CR_3PL_MVC_BL_2018_0226_001 Added By RAVI
                        @UPLOAD_FILE = objInboundInquiry.UPLOAD_FILE
                        //END
                    }, commandType: CommandType.StoredProcedure);
            }
        }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
        public InboundInquiry GetRecvTempTableCount(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_IB_GET_COUNT_FROM_DOC_RECV_DTL_TEMP";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_ib_doc_id = objInboundInquiry.ib_doc_id,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGetRecvdDetailsCount = ListIbDocIdLIST.ToList();
                    if (objInboundInquiry.ListGetRecvdDetailsCount[0].rcvd_dtl_count == 0)
                    {
                        objInboundInquiry.rcvd_dtl_count = 0;
                    }
 else 
                    {
                        objInboundInquiry.rcvd_dtl_count = 1;
                    }
                    return objInboundInquiry;
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
        public InboundInquiry GetTempFiledtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_GET_UPLOAD_DOCUMENT_WEB";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ibdocid,
                            @P_STR_DOCTYPE = objInboundInquiry.doctype,
                            @P_STR_FILENAME = objInboundInquiry.Filename,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.LstFiledtl = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetDocEntryCount(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_TEMP_DOC_ENTRY_COUNT";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGetDocEntryCount = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry Getuploaddelete(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_GET_UPLOAD_DELETE";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID  = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ibdocid,
                            @P_STR_FILENAME = objInboundInquiry.file_name,
                            @P_STR_FILEPATH = objInboundInquiry.file_path,
                            @P_STR_UPLOADDT = objInboundInquiry.docUploaddt,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGetDocEntryCount = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry GetReceivingdelete(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_DELETE_LOT_BY_IB_DOC_ID";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGetDocEntryCount = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetLoadReceivingdelete(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_DELETE_LOT_BY_IB_DOC_ID_DTL";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                            @cont_id = objInboundInquiry.cntr_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListLoadReceivingDelDtl = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetUpdateRcvdStatus(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_DOC_STATUS_UPDT";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                            @Status = "1-RCVD",
                            @Step="1-RCVD"
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListLoadReceivingDelDtl = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetDocRecvEntrySaveByItem(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_IB_SAVE_IB_RECEIVING_BY_ITEM";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_ib_doc_id = objInboundInquiry.ib_doc_id,
                            @p_str_ib_rcvd_dt = objInboundInquiry.ib_doc_dt,
                            @p_str_rcvd_from = objInboundInquiry.rcvd_from,
                            @p_str_refno = objInboundInquiry.refno,
                            @p_str_vend_id = objInboundInquiry.vend_id,
                            @p_str_whs_id = objInboundInquiry.whs_id,
                            @p_str_cont_id = objInboundInquiry.cont_id,
                            @p_str_seal_num = objInboundInquiry.seal_num,
                            @p_str_palet_id = objInboundInquiry.palet_id,
                            @p_str_cntr_type = objInboundInquiry.cntr_type,
                            @p_str_ib_load_dt = objInboundInquiry.ib_load_dt,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListSaveDocRecvdDetails = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetDocRecvEntrySaveByLotID(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_IB_SAVE_IB_RECEIVING_BY_LOT_ID";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_ib_doc_id = objInboundInquiry.ib_doc_id,
                            @p_str_ib_rcvd_dt = objInboundInquiry.ib_doc_dt,
                            @p_str_rcvd_from = objInboundInquiry.rcvd_from,
                            @p_str_refno = objInboundInquiry.refno,
                            @p_str_vend_id = objInboundInquiry.vend_id,
                            @p_str_whs_id = objInboundInquiry.whs_id,
                            @p_str_cont_id = objInboundInquiry.cont_id,
                            @p_str_seal_num = objInboundInquiry.seal_num,
                            @p_str_palet_id = objInboundInquiry.palet_id,
                            @p_str_cntr_type = objInboundInquiry.cntr_type,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListSaveDocRecvdDetails = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetDocRecvEntrySaveByPo(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_IB_SAVE_IB_RECEIVING_BY_PO";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_ib_doc_id = objInboundInquiry.ib_doc_id,
                            @p_str_ib_rcvd_dt = objInboundInquiry.ib_doc_dt,
                            @p_str_rcvd_from = objInboundInquiry.rcvd_from,
                            @p_str_refno = objInboundInquiry.refno,
                            @p_str_vend_id = objInboundInquiry.vend_id,
                            @p_str_whs_id = objInboundInquiry.whs_id,
                            @p_str_cont_id = objInboundInquiry.cont_id,
                            @p_str_seal_num = objInboundInquiry.seal_num,
                            @p_str_palet_id = objInboundInquiry.palet_id,
                            @p_str_cntr_type = objInboundInquiry.cntr_type,
                          },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListSaveDocRecvdDetails = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetDocEditDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_IB_LOAD_DOC_EDIT_DTL";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id.Trim(),
                            @ib_doc_id = objInboundInquiry.ib_doc_id.Trim(),
                            @DocEntry = objInboundInquiry.doc_entry_id


                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListLoadDocEditDtl = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetEditGridData(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_EDIT_GRID_DATA";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ibdocid = objInboundInquiry.ib_doc_id,
                        @line_num = objInboundInquiry.line_num,
                        @itm_code = objInboundInquiry.itm_code,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGridEditData = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetDeleteTempData(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_DEL_TEMP_DATA";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGridEditData = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //  CR-3PL_MVC_IB_2018_0313_002 Added by Soniya
        public InboundInquiry GetIBRecvDeleteTempData(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_RECV_DEL_TEMP_DATA";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGridEditData = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry UpdateTblIbDocHdr(InboundInquiry objInboundInquiry)
        {
            try
            {

                string l_str_eta_dt = string.Empty;
                l_str_eta_dt = objInboundInquiry.eta_dt;
                if (l_str_eta_dt == "01/01/0001")
                {
                    l_str_eta_dt = null;
                }
                else if (l_str_eta_dt == "")
                {
                    l_str_eta_dt = null;
                }
                else
                {
                    l_str_eta_dt = objInboundInquiry.eta_dt;
                }

                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_UPDATE_DOC_HDR";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                        @ib_doc_dt = objInboundInquiry.ib_doc_dt,
                        @cntr_id = objInboundInquiry.cont_id,
                        @vend_id = objInboundInquiry.recvd_fm,
                        @vend_name = objInboundInquiry.recvd_fm,
                        @req_num = objInboundInquiry.refno,
                        @ordr_dt = objInboundInquiry.orderdt,
                        @shipvia_id = objInboundInquiry.recvdvia,
                        @note = objInboundInquiry.Note,
                        @process_id = "ADD",
                        @eta_dt = l_str_eta_dt,
                        @master_bol = objInboundInquiry.bol,
                        @vessel_no = objInboundInquiry.vessel_num

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGridEditData = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetDeleteDocCtn(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_DEL_DOC_CTN";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGridEditData = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry UpdateTblIbDocDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_UPDATE_TBL_IB_DOC_DTL";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ib_doc_id = objInboundInquiry.ib_doc_id,
                        @line_num = objInboundInquiry.LineNum,
                        @p_int_ordr_ctn = objInboundInquiry.ctn_qty,
                        @p_int_ordr_qty = objInboundInquiry.ord_qty,


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGridEditData = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetPaletIdValidation(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_PALETIDVALIDATE";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,                           
                            @P_STR_PALETID_ID = objInboundInquiry.palet_id,                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPaletId = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetLotIDValidation(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_LOTIDVALIDATE";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_LOT_ID = objInboundInquiry.lot_id,                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPaletId = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetCntrValidation(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_PALETID";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_CONT_ID = objInboundInquiry.cntr_id,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPaletId = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry Gettranstaus(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_TRAN_STATUS";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_CONT_ID = objInboundInquiry.cntr_id,
                            @P_STR_LOT_ID = objInboundInquiry.lot_id,
                            @P_STR_IBDOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_PALETID_ID = objInboundInquiry.palet_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListRcvgDtl = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry Getlotdtltext(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_LOT_DTL";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_CONT_ID = objInboundInquiry.cntr_id,
                            @P_STR_LOT_ID = objInboundInquiry.lot_id,
                            @P_STR_IBDOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_PALETID_ID = objInboundInquiry.palet_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocHdr = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetLotHdr(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_LOT_HDR";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_LOT_ID = objInboundInquiry.lot_id,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocHdr = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetGridlotdtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_LOT_DTL_GRID";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_LOT_ID = objInboundInquiry.lot_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListRcvgDtl = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetDftWhs(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_GET_DFTWHS";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry Del_Doc_qty_Mod(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_DEL_DOC_QTY";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_ITEM_CODE = objInboundInquiry.itm_code,
                            @P_STR_IBDOC_ID = objInboundInquiry.ibdocid,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry Del_iv_itm_trn(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_lot_trn_in_hst_del";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_Id = objInboundInquiry.cmp_id,
                            @lot_id = objInboundInquiry.lot_id,
                            @palet_id = objInboundInquiry.palet_id,
                            @ReturnValue =ParameterDirection.Output,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry Update_Doc_tbl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "Sp_MVC_IB_DOC_DTL_UPDATE";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                            @itm_code = objInboundInquiry.itm_code,
                            @cur_Rcvd_qty = 0,
                            @cur_Rcvd_ctn = 0,
                            @dtl_line = objInboundInquiry.dtl_line,                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry Update_Doc_ctn(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_DOC_CTN_EDIT_UPDATE";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                            @itm_code = objInboundInquiry.itm_code,
                            @cur_Rcvd_qty = objInboundInquiry.rcvd_qty,
                            @cur_Rcvd_ctn = objInboundInquiry.ctn_qty,
                            @dtl_line = objInboundInquiry.dtl_line,
                            @ctn_line = objInboundInquiry.ctn_line,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry Add_To_Itm_Trn_in_CtnQty(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_DOCENTRY_ADD_TRN_IN";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ctn_qty = objInboundInquiry.ctns,
                            @itm_code = objInboundInquiry.itm_code,
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_size = objInboundInquiry.itm_size,
                            @kit_type = "CTN",
                            @kit_id = objInboundInquiry.itm_code,
                            @kit_qty = objInboundInquiry.ppk,
                            @lot_id = objInboundInquiry.lot_id,
                            @palet_id = objInboundInquiry.palet_id,
                            @cont_id = objInboundInquiry.cntr_id,
                            @tran_type = "1RCVD",
                            @rcvd_dt = objInboundInquiry.ib_doc_dt,
                            @status = "TEMP",
                            @bill_status = "OPEN",
                            @io_rate_id = objInboundInquiry.io_rate_id,
                            @st_rate_id = objInboundInquiry.st_rate_id,
                            @doc_id = objInboundInquiry.ibdocid,
                            @doc_date = objInboundInquiry.ib_doc_dt,
                            @doc_notes = objInboundInquiry.notes,
                            @fmto_name = objInboundInquiry.cntr_id,
                            @group_id = objInboundInquiry.cmp_id,
                            @whs_id = objInboundInquiry.whs_id,
                            @loc_id = objInboundInquiry.loc_id,
                            @pkg_type = "CTN",
                            @pkg_qty = objInboundInquiry.ppk,
                            @itm_qty = objInboundInquiry.ppk,
                            @lbl_id = "1111",
                            @grn_id = objInboundInquiry.refno,
                            @po_num = objInboundInquiry.po_num,
                            @process_id = "",
                            @length = objInboundInquiry.length,
                            @width = objInboundInquiry.width,
                            @depth = objInboundInquiry.depth,
                            @wgt = objInboundInquiry.wgt,
                            @cube = objInboundInquiry.cube,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry Add_Iv_Lot_Dtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_SPLY_LOT_DTL_ADD";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @doc_num = objInboundInquiry.ibdocid,
                            @lot_id = objInboundInquiry.lot_id,
                            @palet_id = objInboundInquiry.palet_id,
                            @dtl_line = objInboundInquiry.dtl_line,
                            @palet_dt = objInboundInquiry.ib_doc_dt,
                            @status = "TEMP",
                            @tran_status = "ORIG",
                            @bill_status = "OPEN",
                            @cont_id = objInboundInquiry.cntr_id,
                            @itm_code = objInboundInquiry.itm_code,
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_size = objInboundInquiry.itm_size,
                            @itm_name = objInboundInquiry.itm_name,
                            @kit_id = objInboundInquiry.itm_code,
                            @rate_id = objInboundInquiry.io_rate_id,
                            @rate = objInboundInquiry.rate_id,
                            @whs_id = objInboundInquiry.whs_id,
                            @loc_id = objInboundInquiry.loc_id,
                            @pkg_type = "CTN",
                            @tot_pkg = objInboundInquiry.ctn_qty,
                            @bal_pkg = objInboundInquiry.ctn_qty,
                            @pkg_qty = "1",
                            @tot_qty = objInboundInquiry.ordr_qty,
                            @bal_qty = objInboundInquiry.ordr_qty,
                            @po_num = objInboundInquiry.po_num,
                            @po_line_num = "1",
                            @po_due_line = "1",
                            @notes = objInboundInquiry.notes,
                            @process_id = "",
                            @length = objInboundInquiry.length,
                            @width = objInboundInquiry.width,
                            @depth = objInboundInquiry.depth,
                            @wgt = objInboundInquiry.wgt,
                            @cube = objInboundInquiry.cube,
                            @st_rate_id = objInboundInquiry.st_rate_id,
                            @st_rate = objInboundInquiry.strg_rate,
                            @factory_id = objInboundInquiry.factory_id,
                            @cust_name = objInboundInquiry.cust_name,
                            @cust_po_num = objInboundInquiry.cust_po_num,
                            @pick_list = objInboundInquiry.pick_list
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();                  
                }
               
            }  
            catch
            {

            }       
            //catch (Exception ex)
            //{
            //    throw ex;
                
            //}
            finally
            {

            }
            return objInboundInquiry;
        }

        public InboundInquiry Add_tbl_iv_lot_ctn(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_IV_LOT_CTN_ADD";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ibdocid,
                            @lot_id = objInboundInquiry.lot_id,
                            @palet_id = objInboundInquiry.palet_id,
                            @dtl_line = objInboundInquiry.dtl_line,
                            @itm_line = objInboundInquiry.ctn_line,
                            @kit_id = objInboundInquiry.itm_code,
                            @itm_code = objInboundInquiry.itm_code,
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_size = objInboundInquiry.itm_size,
                            @itm_qty = objInboundInquiry.ppk,
                            @ctn_qty = objInboundInquiry.ppk,
                            @tot_ctn = objInboundInquiry.ctns,
                            @process_id = "",
                            @length = objInboundInquiry.length,
                            @width = objInboundInquiry.width,
                            @depth = objInboundInquiry.depth,
                            @wgt = objInboundInquiry.wgt,
                            @cube = objInboundInquiry.cube,                         
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetKitQty(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_kitQty";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @itm_code = objInboundInquiry.itm_code,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry CkSTRate(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_STRATE";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @strate_id = objInboundInquiry.st_rate_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry CkIORate(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_IORATE";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @iorate_id = objInboundInquiry.io_rate_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocEntryDtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetGridRecvEditData(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_Temptable_RecvEntry_edit";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ibdocid = objInboundInquiry.ib_doc_id,
                            @line_num = objInboundInquiry.line_num,
                            @ctn_line = objInboundInquiry.ctn_line,
                            @itm_code = objInboundInquiry.itm_code,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocRecvEntryDtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry InsertTblIbDocRecvDtlTemp(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {               
                    const string storedProcedure2 = "SP_MVC_IB_INSERT_TEMPLOT_DTL_GRID";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_LOT_ID = objInboundInquiry.lot_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListRcvgDtl = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetItmName(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_ITEMNAME";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_Size = objInboundInquiry.itm_size,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListPickdtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetItmCode(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_ITEMCODE";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ibdocid = objInboundInquiry.ib_doc_id,
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_Size = objInboundInquiry.itm_size,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocRecvEntryDtl = ListInq.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry GetItmCodeByByItemMaster(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_itemhdr";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.CompID,
                            @p_str_itm_num = objInboundInquiry.Style,
                            @p_str_itm_color = objInboundInquiry.Color,
                            @p_str_itm_size = objInboundInquiry.Size,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGetItmhdr = ListInq.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry UpdtItmDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_MVC_Updt_ma_itm_dtl";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_size = objInboundInquiry.itm_size,
                            @wgt = objInboundInquiry.wgt,
                            @cube = objInboundInquiry.cube,
                            @length = objInboundInquiry.length,
                            @width = objInboundInquiry.width,
                            @depth = objInboundInquiry.depth,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocRecvEntryDtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetCheckExistGridDataRecvEntry(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_mvcweb_temptable_recventry_exist_item";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ibdocid = objInboundInquiry.ibdocid,
                            @line_num = objInboundInquiry.line_num,
                            @ctn_line = objInboundInquiry.ctn_line,
                            @itm_code = objInboundInquiry.itm_code,                         
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocRecvEntryDtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry InsertRecvEntryTemptable(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_MVC_INSERT_TEMPRECVD_DTL";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ibdocid,
                            @dtl_line = objInboundInquiry.line_num,
                            @ctn_line = objInboundInquiry.ctn_line,
                            @itm_line = objInboundInquiry.itm_line,
                            @itm_code = objInboundInquiry.itm_code,
                            @ctn_qty = objInboundInquiry.ctn_qty,
                            @tot_ctn = objInboundInquiry.tot_ctn,
                            @tot_qty = objInboundInquiry.tot_qty,
                            @loc_id = objInboundInquiry.loc_id,
                            @po_num = objInboundInquiry.po_num,
                            @st_rate_id = objInboundInquiry.st_rate_id,
                            @io_rate_id = objInboundInquiry.io_rate_id,
                            @length = objInboundInquiry.length,
                            @width = objInboundInquiry.width,
                            @depth = objInboundInquiry.depth,
                            @wgt = objInboundInquiry.wgt,
                            @cube = objInboundInquiry.cube,
                            @process_id = objInboundInquiry.process_id,
                            @lot_id = objInboundInquiry.lot_id,                            
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_size = objInboundInquiry.itm_size,
                            @itm_name = objInboundInquiry.itm_name,
                            @note = objInboundInquiry.Note,
                            @Status = objInboundInquiry.status,
                            @paletId = objInboundInquiry.palet_id,
                            @factory_id = objInboundInquiry.factory_id,
                            @cust_name = objInboundInquiry.cust_name,
                            @cust_po_num = objInboundInquiry.cust_po_num,
                            @pick_list = objInboundInquiry.pick_list,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocRecvEntryDtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetRecvEntryCount(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_TEMP_RECV_ENTRY_COUNT";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListGetDocEntryCount = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetRecvdtlGrid(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_TEMP_RECV_DTL";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListRcvgDtl = ListIbDocIdLIST.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetRecvEntryGridDeleteData(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_temptable_recventry_grid_delete_item";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmp_id = objInboundInquiry.cmp_id,
                        @ibdocid = objInboundInquiry.ib_doc_id,
                        @line_num = objInboundInquiry.line_num,
                        @ctn_line = objInboundInquiry.ctn_line,
                        @itm_code = objInboundInquiry.itm_code,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListRcvgDtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry SumTotqty(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_IB_GET_TOTQTY";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {

                        @cmp_id = objInboundInquiry.cmp_id,
                        @ibdocid = objInboundInquiry.ib_doc_id,
                        @itm_num = objInboundInquiry.itm_num,
                        @itm_color = objInboundInquiry.itm_color,
                        @itm_size = objInboundInquiry.itm_size,                      
                        @dtl_line = objInboundInquiry.line_num,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListRcvgDtl = ListIRFP.ToList();
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //Added By Ravi 17-02-2018
        public InboundInquiry GetContainerandRateID(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    if(objInboundInquiry.CNTR_CHECK == "CNTR_ID")
                    {
                        const string SearchCustDtls = "SP_MVC_GET_CONTAINER_ID";
                        IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                        {
                            @CMP_ID = objInboundInquiry.cmp_id.Trim(),
                            @IB_DOC_ID = objInboundInquiry.ibdocid.Trim()
                        }, commandType: CommandType.StoredProcedure).ToList();
                        objInboundInquiry.ListGETContainerID = ListIRFP.ToList();
                    }
                     //CR_3PL_MVC_BL_2018_0224_001 Added By Ravi 24-02-2018
                    if (objInboundInquiry.CNTR_CHECK == "PO_NUM")
                    {
                        const string SearchCustDtls = "SP_MVC_IB_GET_PONUM";
                        IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                        {
                            @CMP_ID = objInboundInquiry.cmp_id.Trim(),
                            @IB_DOC_ID = objInboundInquiry.ibdocid.Trim()
                        }, commandType: CommandType.StoredProcedure).ToList();
                        objInboundInquiry.ListGetPONUM = ListIRFP.ToList();
                    }
                    if (objInboundInquiry.CNTR_CHECK == "STRG_ID")
                    {
                        const string SearchCustDtls = "SP_MVC_IB_GET_STRG_ID";
                        IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                        {
                            @CMP_ID = objInboundInquiry.cmp_id.Trim(),
                            @TYPE = "STRG"       
                        }, commandType: CommandType.StoredProcedure).ToList();
                        objInboundInquiry.ListGetSTRG_ID = ListIRFP.ToList();
                    }
                    //END
                    if (objInboundInquiry.CNTR_CHECK == "RATE_ID")
                    {
                        const string SearchCustDtls = "SP_MVC_GET_RATE_ID";
                        IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                        {
                            @CMP_ID = objInboundInquiry.cmp_id.Trim(),
                            @CATG = objInboundInquiry.bill_inout_type.Trim()            // CR_3PL_MVC_IB_2018_0223_001
                        }, commandType: CommandType.StoredProcedure).ToList();
                        objInboundInquiry.ListGETRateID = ListIRFP.ToList();
                        if (objInboundInquiry.ListGETRateID.Count == 0)
                        {
                            objInboundInquiry.check_inout_type = "";
                        }
                        else
                        {
                            objInboundInquiry.check_inout_type = "1";
                        }
                    }
                    if (objInboundInquiry.CNTR_CHECK == "GET_WGTCUBE")
                    {
                        const string SearchCustDtls = "SP_MVC_GET_IB_WGT_CUBE";
                        IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                        {
                            @CMP_ID = objInboundInquiry.cmp_id.Trim(),
                            @IB_DOC_ID = objInboundInquiry.ibdocid.Trim(),
                            @CONT_ID = objInboundInquiry.CONTAINERID
                        }, commandType: CommandType.StoredProcedure).ToList();
                        objInboundInquiry.ListGetWgtCubeValue = ListIRFP.ToList();
                    }
                  
                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry InsertCONTAINERRecvDetails(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                   
                        const string SearchCustDtls = "SP_MVC_IB_INSERT_CNTR_REC_DTL";
                        IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                        {
                            @CMP_ID = objInboundInquiry.cmp_id.Trim(),
                            @IB_DOC_ID = objInboundInquiry.ibdocid.Trim(),
                            @RCVD_DT = "",
                            @STATUS = "A",
                            //CR_3PL_MVC_BL_2018_0224_001 Added By Ravi 24-02-2018
                            @CNTR_ID = objInboundInquiry.CONTAINERID.Trim(),
                            @PO_NUM = objInboundInquiry.po_num.Trim(),                           
                            @IO_RATE_ID = objInboundInquiry.RATEID.Trim(),
                            @STRG_RATE_ID = objInboundInquiry.st_rate_id.Trim(),
                            //END
                            @NO_OF_PALLETS = objInboundInquiry.CNTR_PALLET.Trim(),
                            @TOT_WGT = objInboundInquiry.CNTR_WEIGHT.Trim(),
                            @TOT_CUBE = objInboundInquiry.CNTR_CUBE.Trim(),
                            @NOTE = objInboundInquiry.CNTR_NOTE.Trim(),
                            @PROCESS_ID = "ADD"
                        }, commandType: CommandType.StoredProcedure).ToList();
                        objInboundInquiry.ListGETContainerID = ListIRFP.ToList();
                  

                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //END  
        //CR_3PL_MVC_BL_2018_0221_001 Added By ravi 21-02-2018
        public void Del_rcv_dtl(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_ID_DEL_RCVD_DTL_RECORD";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @CMP_ID = objInboundInquiry.cmp_id,
                        @IB_DOC_ID = objInboundInquiry.ibdocid,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        //END
        //CR_3PL_MVC_IB_2018_0228_001 Added By Ravi 21-02-2018
        public InboundInquiry AddLocId(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string SearchCustDtls = "SP_MVC_IB_ADD_LOC_ID";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @cmpId = objInboundInquiry.cmp_id.Trim(),
                        @whsid = objInboundInquiry.whs_id.Trim(),
                        @Locid = "FLOOR",
                        @Locdesc = "FLOOR",
                        @Status = "A",
                        @note = "-",
                        @length = 2,
                        @width = 2,
                        @depth = 2,
                        @cube = 0.01,
                        @usage = 0,
                        @processid = "Add",
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.ListGETContainerID = ListIRFP.ToList();


                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //END
        //  CR-3PL_MVC_IB_2018_0312_002
        public InboundInquiry GetRcvdDtlCount(InboundInquiry objInboundInquiry)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string SearchCustDtls = "SP_MVC_IB_GET_RCEV_DTL_COUNT";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {
                        @p_str_cmp_id = objInboundInquiry.cmp_id.Trim(),
                        @p_str_ibdoc_id = objInboundInquiry.ibdocid.Trim(),
                      
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundInquiry.GetRcvdDtlCount = ListIRFP.ToList();


                }
                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //END CR-3PL_MVC_IB_2018_0312_002 
        public InboundInquiry GetItemRcvdQty(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_ITM_RCVD_QTY";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ibdocid = objInboundInquiry.ib_doc_id,
                            @line_num = objInboundInquiry.line_num,
                            @ctn_line = objInboundInquiry.ctn_line,
                            @itm_code = objInboundInquiry.itm_code,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListItemRcvdQty = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetCtnLineNo(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_CTN_LINE_NUM";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ibdocid = objInboundInquiry.ib_doc_id,
                            @line_num = objInboundInquiry.line_num,
                            @itm_code = objInboundInquiry.itm_code,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListCtnLine = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry Update_Lot_Bill_Status(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_UPDATE_LOT_BILL_STATUS";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListCtnLine = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry Check_Exist_Container_Id(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_CHECK_EXIST_CONTAINER_ID";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_cntr_id = objInboundInquiry.cntr_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListCheckExistContainerId = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry CheckItemExist(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_CHECK_ALREADY_ITEM_EXISTS";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,                        
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_ITM_CODE = objInboundInquiry.itm_code,
                            @P_INT_PO_NO = objInboundInquiry.po_num,
                            @P_INT_PPK = objInboundInquiry.ppk,
                            @P_INT_DTL_LINE = objInboundInquiry.line_num,
                            @P_INT_CTN_LINE = objInboundInquiry.ctn_line,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.LstItmExist = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry Validate_Itm(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_CHECK_ALREADY_ITEMCODE_EXISTS";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_ITEMNUM = objInboundInquiry.itm_num,
                            @P_STR_ITM_COLOR = objInboundInquiry.itm_color,
                            @P_STR_ITM_SIZE = objInboundInquiry.itm_size,                            
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.LstItmExist = ListInq.ToList();

                    return objInboundInquiry;
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

        public InboundInquiry ItemXGetIBitmDetails(string term, string cmp_id)
        {
            try
            {
                InboundInquiry objCustDtl = new InboundInquiry();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_Itm_dtl";
                    IList<InboundInquiry> ListIRFP = connection.Query<InboundInquiry>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmp_id,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objCustDtl.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objCustDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundInquiry GetRcvdEntryCountDtl(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();
                    if (objInboundInquiry.lot_id != "")
                    {
                        const string storedProcedure2 = "SP_MVC_GET_RECV_ENTRY_COUNT";
                        IEnumerable<InboundInquiry> ListEShip = connection.Query<InboundInquiry>(storedProcedure2,
                            new
                            {
                                @Cmp_ID = objInboundInquiry.cmp_id,
                                @ib_doc_id = objInboundInquiry.ib_doc_id,
                                @ib_lot_id = objInboundInquiry.lot_id,
                            },
                            commandType: CommandType.StoredProcedure);
                        objInboundInquiry.LstRcvdEntryCountDtl = ListEShip.ToList();
                    }

                    else { 

                    const string storedProcedure2 = "SP_MVC_IB_GET_TEMP_RECV_ENTRY_COUNT";
                    IEnumerable<InboundInquiry> ListIbDocIdLIST = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.LstRcvdEntryCountDtl = ListIbDocIdLIST.ToList();
                    }
                    return objInboundInquiry;
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
        public InboundInquiry InsertdocEditEntry(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_INSERT_EDIT_MODE_ON";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @scn_id = objInboundInquiry.scn_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                            @Locked_by = objInboundInquiry.lock_by,
                            @Locked_Time = objInboundInquiry.lock_dt,
                            @status = objInboundInquiry.status,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.LstItmExist = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetdocEditCount(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_EDIT_MODE_ON";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @opt = objInboundInquiry.opt,
                            @cmp_id = objInboundInquiry.cmp_id,
                            @scn_id = objInboundInquiry.scn_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,
                            @status = objInboundInquiry.status,
                            @user_id= objInboundInquiry.user_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.LstItmExist = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetrecvEditTotQty(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOutboundInqCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_MVC_IB_GET_TOTQTY_EDIT";
                    IEnumerable<InboundInquiry> ListInq = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ibdocid = objInboundInquiry.ib_doc_id,
                            @itm_num = objInboundInquiry.itm_num,
                            @itm_color = objInboundInquiry.itm_color,
                            @itm_size = objInboundInquiry.itm_size,
                            @dtl_line = objInboundInquiry.dtl_line,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListDocdtl = ListInq.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry GetTotal(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "proc_get_totalcount_mvcweb_inbound_comfirmation_rpt";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @cmp_id = objInboundInquiry.cmp_id,
                            @ib_doc_id = objInboundInquiry.ib_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListTotalCount = ListInbound.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry check_ib_doc_in_use(InboundInquiry objInboundInquiry)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    {
                        try
                        {
                            // transactional code...
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "SP_CHECK_IB_DOC_IN_USE";
                                cmd.Parameters.Add("@PSTR_CMP_ID", SqlDbType.NVarChar).Value = objInboundInquiry.cmp_id;
                                cmd.Parameters.Add("@PSTR_IB_DOC_ID", SqlDbType.NVarChar).Value = objInboundInquiry.ib_doc_id;
                                cmd.Parameters.Add("@PINT_IB_DOC_IN_USE", SqlDbType.Int).Direction = ParameterDirection.Output;
                                cmd.Connection = conn;
                                cmd.ExecuteNonQuery();
                                objInboundInquiry.int_ib_doc_in_use = Convert.ToInt32((cmd.Parameters["@PINT_IB_DOC_IN_USE"].Value));
                            }

                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                        return objInboundInquiry;
                    }
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

        public InboundInquiry Add_To_proc_save_audit_trail(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "Sp_ib_insert_audit_trail";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_ib_doc_id = objInboundInquiry.ib_doc_id,
                            @p_str_mode = objInboundInquiry.mode,
                            @p_str_maker = objInboundInquiry.maker,
                            @p_dt_maker_dt = objInboundInquiry.makerdt,
                            @p_dt_access_stamp = objInboundInquiry.makerdt,
                            @p_str_audit_comments = objInboundInquiry.Auditcomment,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListTotalCount = ListInbound.ToList();

                    return objInboundInquiry;
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
        public InboundInquiry CHECKDOCDATE(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "Sp_ib_check_doc_date";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objInboundInquiry.cmp_id,
                            @p_str_ib_doc_id = objInboundInquiry.ib_doc_id,                            
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListTotalCount = ListInbound.ToList();
                    return objInboundInquiry;
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
        public InboundInquiry GET_IB_DOC_CUBE_AND_WGT(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_GET_IB_DOC_CUBE_AND_WGT";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListTotalCount = ListInbound.ToList();
                    return objInboundInquiry;
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
        public InboundInquiry GET_IB_RCVD_DOC_CUBE_AND_WGT(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "SP_GET_IB_RCVD_DOC_CUBE_AND_WGT";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.ListTotalCount = ListInbound.ToList();
                    return objInboundInquiry;
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
       
        public InboundInquiry GET_IBS_DTL_FROM_RATE_MASTER(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_ma_get_rate_by_type";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objInboundInquiry.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.List_ibs_dtl = ListInbound.ToList();
                    return objInboundInquiry;
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

        public void INSERT_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                { 

                    const string storedProcedure2 = "sp_Insert_updt_ibsEntry_temp_tbl";
                    connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_LINE_NUM = objInboundInquiry.dtl_line,
                            @P_STR_STATUS = objInboundInquiry.Status,
                            @P_STR_ITM_NUM = objInboundInquiry.itm_num,
                            @P_STR_ITM_NAME = objInboundInquiry.itm_name,
                            @P_STR_LIST_PRICE = objInboundInquiry.srvc_price,
                            @P_STR_QTY = objInboundInquiry.srvc_qty,
                            @P_STR_PRICE_UOM = objInboundInquiry.srvc_uom,
                            @P_STR_AMOUNT = objInboundInquiry.amt,
                            @P_STR_NOTES = objInboundInquiry.notes,
                            @P_STR_USER_ID = objInboundInquiry.ibs_user_id,
                            @P_STR_IBS_DOC_ID=objInboundInquiry.ibs_doc_id
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
        public InboundInquiry GET_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_ibs_dtl_Fetch";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_IBS_DOC_ID = objInboundInquiry.ibs_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.List_ibs_dtl = ListInbound.ToList();
                    return objInboundInquiry;
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

        public void DELETE_IBS_DTL_TEMP_TBL(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_delete_ibsEntry_temp_tbl";
                    connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_COMPID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_IBS_DOC_ID = objInboundInquiry.ibs_doc_id
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

        public void INSERT_IBS_DTL(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_Insert_updt_ibsEntry";
                    connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_LINE_NUM = objInboundInquiry.dtl_line,
                            @P_STR_STATUS = objInboundInquiry.Status,
                            @P_STR_ITM_NUM = objInboundInquiry.itm_num,
                            @P_STR_ITM_NAME = objInboundInquiry.itm_name,
                            @P_STR_LIST_PRICE = objInboundInquiry.srvc_price,
                            @P_STR_QTY = objInboundInquiry.srvc_qty,
                            @P_STR_PRICE_UOM = objInboundInquiry.srvc_uom,
                            @P_STR_AMOUNT = objInboundInquiry.amt,
                            @P_STR_NOTES = objInboundInquiry.Note,
                            @P_STR_IBS_DOC_ID = objInboundInquiry.ibs_doc_id

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
        public void UPDATE_IBS_DTL(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_Insert_updt_ibsEntry";
                    connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_LINE_NUM = objInboundInquiry.dtl_line,
                            @P_STR_STATUS = objInboundInquiry.Status,
                            @P_STR_ITM_NUM = objInboundInquiry.itm_num,
                            @P_STR_ITM_NAME = objInboundInquiry.itm_name,
                            @P_STR_LIST_PRICE = objInboundInquiry.srvc_price,
                            @P_STR_QTY = objInboundInquiry.srvc_qty,
                            @P_STR_PRICE_UOM = objInboundInquiry.srvc_uom,
                            @P_STR_AMOUNT = objInboundInquiry.amt,
                            @P_STR_NOTES = objInboundInquiry.notes,
                            @P_STR_IBS_DOC_ID = objInboundInquiry.ibs_doc_id
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
        public void DELETE_IBS_DTL(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_delete_ibsEntry";
                    connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id,
                            @P_STR_IBS_DOC_ID = objInboundInquiry.ibs_doc_id
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

  
        public bool DeleteIOBillByIbDocId(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_lot_id)

        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_bl_delete_inout_bill_by_ib_doc_id";
                    connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cust_id = p_str_cmp_id,
                            @p_str_bill_doc_id = p_str_bill_doc_id,
                            @p_str_lot_id = p_str_lot_id,

                        },
                        commandType: CommandType.StoredProcedure);
                }
                return true;
            }
            catch (Exception ex)
            {
                 return true;
                throw ex;
            }
            finally
            {

            }
        }

        public InboundInquiry GET_IBS_DTL(InboundInquiry objInboundInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_ma_get_ibs_dtl";
                    IEnumerable<InboundInquiry> ListInbound = connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objInboundInquiry.cmp_id,
                            @P_STR_IB_DOC_ID = objInboundInquiry.ib_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.List_get_ibs_dtl = ListInbound.ToList();
                    return objInboundInquiry;
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
        public InboundInquiry GET_IBS_DOC_ID(InboundInquiry objInboundInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "sp_ma_get_ibs_id";
                    IEnumerable<InboundInquiry> ListStockChangeLIST = connection.Query<InboundInquiry>(storedProcedure1,
                        new
                        {
                            @P_STR_IBS_ID = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundInquiry.List_get_Ibs_Doc_ID = ListStockChangeLIST.ToList();
                  
                }

                return objInboundInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public IBDocExcp GetIBDocExcpList(IBDocExcp objIBDocExcp, string p_str_cmp_id)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<IBDocExcp> ListIBDocExcp = connection.Query<IBDocExcp>("sp_ib_get_doc_excp_list", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objIBDocExcp.ListIBDocExcp = ListIBDocExcp.ToList();
                }
                return objIBDocExcp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable GetIBDocExceptionList(string p_str_cmp_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ib_get_ib_excp_list", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                connection.Open();
                DataTable dtIBDocExcp = new DataTable();
                dtIBDocExcp.Load(command.ExecuteReader());
                return dtIBDocExcp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable getXlsIBStkSmryByLocList(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spGetIBStkSmryByLoc", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_ib_doc_id", SqlDbType.VarChar).Value = p_str_ib_doc_id;
                connection.Open();
                DataTable dtIBDocExcp = new DataTable();
                dtIBDocExcp.Load(command.ExecuteReader());
                return dtIBDocExcp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable GetInboundExceptionList(String l_str_cmp_id)
        {

            System.Data.DataTable table1 = null;
            System.Data.SqlClient.SqlCommand sqlCommand = null;
            System.Data.SqlClient.SqlConnection sqlConn;
            System.Data.DataSet dataSet = null;
            System.Data.SqlClient.SqlDataAdapter adapter = null;

            try
            {
                sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                sqlConn.Open();

                sqlCommand = new System.Data.SqlClient.SqlCommand("sp_ib_get_ib_excp_list", sqlConn);
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 0;
                sqlCommand.Parameters.AddWithValue("@p_str_cmp_id", l_str_cmp_id);
                adapter = new System.Data.SqlClient.SqlDataAdapter(sqlCommand);
                dataSet = new System.Data.DataSet();
                adapter.Fill(dataSet);

                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        table1 = dataSet.Tables[0];
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

            return table1;
        }


        public DataTable GetIBCheckIbDocValidateItem(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ib_check_ib_doc_validate_items", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_ib_doc_id", SqlDbType.VarChar).Value = p_str_ib_doc_id;
                connection.Open();
                DataTable dtIBDocValidateItem = new DataTable();
                dtIBDocValidateItem.Load(command.ExecuteReader());
                return dtIBDocValidateItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable GetIBCheckIbDocRcvdCubeCheck(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ib_check_ib_doc_rcvd_cube_check", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_ib_doc_id", SqlDbType.VarChar).Value = p_str_ib_doc_id;
                connection.Open();
                DataTable dtIBDocValidateItem = new DataTable();
                dtIBDocValidateItem.Load(command.ExecuteReader());
                return dtIBDocValidateItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public bool UpdateLotContainer(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id, string p_str_cntr_type, 
            string p_str_rcvd_dt, string p_str_ib_load_dt)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_update_lot_cntr_update";
                    connection.Query<InboundInquiry>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = p_str_cmp_id,
                            @p_str_ib_doc_id = p_str_ib_doc_id,
                            @p_str_cntr_id = p_str_cntr_id,
                            @p_str_cntr_type = p_str_cntr_type,
                            @p_str_rcvd_dt = p_str_rcvd_dt,
                            @p_str_ib_load_dt = p_str_ib_load_dt

                        },
                        commandType: CommandType.StoredProcedure);
                }
                return true;
            }
            catch (Exception ex)
            {
                return true;
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetInboundAckExcelTemplate(string l_str_cmp_id, string l_str_doc_id)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_inbound_ack_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@cmp_id", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@ib_doc_id", SqlDbType.VarChar).Value = l_str_doc_id;
                connection.Open();
                DataTable dtGetInboundAckExcelTemplate = new DataTable();
                dtGetInboundAckExcelTemplate.Load(command.ExecuteReader());
                return dtGetInboundAckExcelTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable fnIBGetInboundAckRpt(string pstrCmpdId, string pstrFileName, string pstrUploadRefNum)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spIBGetInboundAckRpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpdId", SqlDbType.VarChar).Value = pstrCmpdId;
                command.Parameters.Add("@pstrFileName", SqlDbType.VarChar).Value = pstrFileName;
                command.Parameters.Add("@pstrUploadRefNum", SqlDbType.VarChar).Value = pstrUploadRefNum;
                connection.Open();
                DataTable dtGetInboundAckExcelTemplate = new DataTable();
                dtGetInboundAckExcelTemplate.Load(command.ExecuteReader());
                return dtGetInboundAckExcelTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetInboundGridSummaryExcelTemplate(string l_str_cmp_id, string l_str_doc_id, string l_str_cntr_id, string l_str_status, string l_str_ib_doc_dt_fm, string l_str_ib_doc_dt_to, string l_str_req_num, string l_str_eta_dt_fm, string l_str_eta_dt_to, string l_str_itm_num, string l_str_itm_color, string l_str_itm_size)
        {
            try
            {
                var l_str_ib_doc_id_fm = string.Empty;
                var l_str_ib_doc_id_to = string.Empty;
                l_str_status = string.Empty;

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_grid_summary_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@cmp_id", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@cntr_id", SqlDbType.VarChar).Value = l_str_cntr_id;
                command.Parameters.Add("@status", SqlDbType.VarChar).Value = l_str_status;
                command.Parameters.Add("@ib_doc_id_fm", SqlDbType.VarChar).Value = l_str_ib_doc_id_fm;
                command.Parameters.Add ("@ib_doc_id_to" , SqlDbType.VarChar).Value = l_str_ib_doc_id_to;
                command.Parameters.Add("@req_num", SqlDbType.VarChar).Value = l_str_req_num;
                command.Parameters.Add("@ib_doc_dt_fm", SqlDbType.VarChar).Value = l_str_ib_doc_dt_fm;
                command.Parameters.Add("@ib_doc_dt_to", SqlDbType.VarChar).Value = l_str_ib_doc_dt_to;
                command.Parameters.Add("@eta_dt_fm", SqlDbType.VarChar).Value = l_str_eta_dt_fm;
                command.Parameters.Add("@eta_dt_to", SqlDbType.VarChar).Value = l_str_eta_dt_to;
                command.Parameters.Add("@itm_num", SqlDbType.VarChar).Value = l_str_itm_num;
                command.Parameters.Add("@itm_color", SqlDbType.VarChar).Value = l_str_itm_color;
                command.Parameters.Add("@itm_size", SqlDbType.VarChar).Value = l_str_itm_size;
                connection.Open();
                DataTable dtGetGridSummaryTemplate = new DataTable();
                dtGetGridSummaryTemplate.Load(command.ExecuteReader());
                return dtGetGridSummaryTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetInboundWorkSheetExcelTemplate(string l_str_cmp_id, string l_str_ib_doc_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_inbound_worksheet_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@cmp_id", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@ib_doc_id", SqlDbType.VarChar).Value = l_str_ib_doc_id;
                connection.Open();
                DataTable dtnboundWorkSheetTemplate = new DataTable();
                dtnboundWorkSheetTemplate.Load(command.ExecuteReader());
                return dtnboundWorkSheetTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetInboundContainerExcelTemplate(string l_str_cmp_id, string l_str_ib_doc_id, string l_str_rate_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                if (l_str_rate_id == "CNTR")
                {
                    command = new SqlCommand("proc_get_mvcweb_inbound_comfirmation_rpt_by_container", connection);
                }
                else
                {
                    command = new SqlCommand("proc_get_mvcweb_inbound_comfirmation_rpt", connection);
                }
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@cmp_id", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@ib_doc_id", SqlDbType.VarChar).Value = l_str_ib_doc_id;
                connection.Open();
                DataTable dtInboundContainerTemplate = new DataTable();
                dtInboundContainerTemplate.Load(command.ExecuteReader());
                return dtInboundContainerTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string checkIBDocInUse(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            try
            {
                int l_int_rec_cnt = 0;

                command = new SqlCommand("sp_ib_doc_status_changed", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_ib_doc_id", SqlDbType.VarChar).Value = p_str_ib_doc_id;
                connection.Open();
                l_int_rec_cnt = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_rec_cnt > 0)
                {
                    return "Y";
                }
                else
                {
                    return "N";
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
        public bool checkIBDocRcvd(string pstrCmpId, string pstrIbDocId)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            bool blnRecExist = false;
            int lintRecCount = 0;
            try
            {
                command = new SqlCommand("spcheckIBDocRcvd", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                command.Parameters.Add("@pstrIbDocId", SqlDbType.VarChar).Value = pstrIbDocId;
                connection.Open();
                lintRecCount = Convert.ToInt32(command.ExecuteScalar());

                if (lintRecCount > 0)
                {
                    blnRecExist = true;
                }
                else
                {
                    blnRecExist = false;
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
            return blnRecExist;
        }

        public int GetRefNum(string enitity_code)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            try
            {
                int l_int_ref_num = 0;

                command = new SqlCommand("sp_get_entity_value", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@enitity_code", SqlDbType.VarChar).Value = enitity_code;
                connection.Open();
                l_int_ref_num = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_ref_num > 0)
                {
                    return l_int_ref_num;
                }
                else
                {
                    return l_int_ref_num;
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


        public string fnIBRecvDtlEditTempSave(string pstrCmpId, DataTable dtIBRrecvDtlEditTemp)
        {
            string pstrRefNo = string.Empty;
            try
            {
                pstrRefNo = Convert.ToString(GetRefNum("ib_rcvd_data_update"));
                dtIBRrecvDtlEditTemp.Columns.Add("req_num", typeof(string));
              

                foreach (DataRow row in dtIBRrecvDtlEditTemp.Rows)
                {
                    row["req_num"] = pstrRefNo;
                }

                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    connection.Open();
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        //Set the database table name

                        sqlBulkCopy.DestinationTableName = "tbl_ib_recv_dtl_edit_temp";
                        sqlBulkCopy.WriteToServer(dtIBRrecvDtlEditTemp);
                    }

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "SpIBRecvDtlEditTempSave";
                        cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                        cmd.Parameters.Add("@pstrRefNo", SqlDbType.VarChar).Value = pstrRefNo;
                      
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();

                    }

                    connection.Close();
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

        
        public void InsertScanInDetails(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_add_itm_scan_serial_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objInboundInquiry.ItemScanIN.cmp_id,
                       
                        @itm_code = objInboundInquiry.ItemScanIN.itm_code,
                        @itm_serial_num = objInboundInquiry.ItemScanIN.itm_serial_num,
                        @itm_num = objInboundInquiry.ItemScanIN.itm_num,
                        @itm_color = objInboundInquiry.ItemScanIN.itm_color,
                        @itm_size = objInboundInquiry.ItemScanIN.itm_size,
                        @status = objInboundInquiry.ItemScanIN.status,
                        @ib_doc_id = objInboundInquiry.ItemScanIN.ib_doc_id,
                        @ib_doc_dt = objInboundInquiry.ItemScanIN.ib_doc_dt,
                        @ob_doc_id = objInboundInquiry.ItemScanIN.ob_doc_id,
                        @ob_doc_dt = objInboundInquiry.ItemScanIN.ob_doc_dt,
                        @ib_user = objInboundInquiry.ItemScanIN.ib_user??"",
                        @ob_user = objInboundInquiry.ItemScanIN.ob_user??""
                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public void DeleteScanInDetails(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_delete_itm_scan_serial_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objInboundInquiry.ItemScanIN.cmp_id,
                        @itm_code = objInboundInquiry.ItemScanIN.itm_code,
                        @itm_serial_num = objInboundInquiry.ItemScanIN.itm_serial_num,
                       
                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public void EditScanInDetails(InboundInquiry objInboundInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_update_itm_scan_serial_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objInboundInquiry.ItemScanIN.cmp_id,
                        @itm_code = objInboundInquiry.ItemScanIN.itm_code,
                        @itm_serial_num = objInboundInquiry.ItemScanIN.itm_serial_num,
                        @itm_serial_num_exist = objInboundInquiry.ItemScanIN.itm_serial_num_exist

                    }, commandType: CommandType.StoredProcedure);

            }
        }


        public List<ItemScanIN> getScanInDetailsByItemCode (string cmpId, string itm_code, string itm_serial_num)
        {

            try
            {
                InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    //InboundInquiry objOrderLifeCycleCategory = new InboundInquiry();

                    const string storedProcedure2 = "sp_get_mvcweb_scan_by_itm_code";
                    IEnumerable<ItemScanIN> ListEShip = connection.Query<ItemScanIN>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = cmpId,
                            @itm_code = itm_code,
                            @itm_serial_num = itm_serial_num

                        },
                        commandType: CommandType.StoredProcedure);
                    //objOrderLifeCycleCategory.ListItemScanIN = ListEShip.ToList();

                    return ListEShip.ToList();
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
