using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class OutboundShipInqRepository: IOutboundShipInqRepository
    {
        public OutboundShipInq GetOutboundShipInq(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var status = objOutboundShipInq.status;
                    if (status == "ALL")
                    {
                        status = "";
                    }
                    else if (status == "SHIP")
                    {
                        status = "S";
                    }                   
                    else if (status == "POST")
                    {
                        status = "P";
                    }                  
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "proc_get_webmvc_outbound_shipping_inquiry";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundShipInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipInq.ship_doc_id_Fm,
                            @ShipDocIdTo = objOutboundShipInq.ship_doc_id_To,
                            @AlocNum = objOutboundShipInq.aloc_doc_id,
                            @ShipDtFr = objOutboundShipInq.Ship_dt_Fm,
                            @ShipDtTo = objOutboundShipInq.Ship_dt_To,
                            @ShipVia = objOutboundShipInq.ship_via_name,
                            @Status = status,
                            @CustID = objOutboundShipInq.cust_id,
                            @Shipto = objOutboundShipInq.ship_to,
                            @Whs = objOutboundShipInq.whs_id,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstOutboundShipInqdetail = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq OutboundShipInqGetCustDetails(string term,string cmpid)
        {
            try
            {
                OutboundShipInq objCustDtl = new OutboundShipInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_cust_hdr";
                    IList<OutboundShipInq> ListIRFP = connection.Query<OutboundShipInq>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmpid,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objCustDtl.OutboundShipInqGetCustDetails = ListIRFP.ToList();
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
        public void SaveShipPost(OutboundShipInq objOutboundShipInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ship_Upd_Trn_in_Trn_out_Trn_hst";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundShipInq.cmp_id,
                        @aloc_doc_id = "-",
                        @ship_doc_id = objOutboundShipInq.ship_doc_id,
                       

                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void InsertShipPost(OutboundShipInq objOutboundShipInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_OB_INSERT_SHIP_POST";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundShipInq.cmp_id,
                        @ship_doc_id = objOutboundShipInq.ship_doc_id,
                        @ship_dt = objOutboundShipInq.ship_ready_dt,
                        @ship_post_dt = objOutboundShipInq.ship_post_dt,
                        @ship_via_id = objOutboundShipInq.ship_via_name,
                        @ship_via_name = objOutboundShipInq.ship_via_name,
                        @contr_id = objOutboundShipInq.cont_id,
                        @seal_no = objOutboundShipInq.seal_num,
                        @picked_by = objOutboundShipInq.picked_by,
                        @track_no = objOutboundShipInq.track_num,
                        @ship_note = objOutboundShipInq.ship_note,
                        @process_id = "ADD"
                       
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void InsertPalletShipDtl(OutboundShipInq objOutboundShipInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_OB_INSERT_PALLET_SHIP_DTL";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @CMP_ID = objOutboundShipInq.cmp_id,
                        @SHIP_DOC_ID = objOutboundShipInq.ship_doc_id,
                        @STATUS = "A",
                        @SHIP_DT = objOutboundShipInq.Shipdt,
                        @CONT_ID = objOutboundShipInq.CNTR_ID,      // CR_3PL_MVC_IB_2018_0303_001 Added by Soniya
                        @PO_NUM = objOutboundShipInq.PONUM,
                        @NO_OF_PALLETS = objOutboundShipInq.CNTR_PALLET,
                        @TOT_WGT = objOutboundShipInq.PALLET_WEIGHT,
                        @TOT_CUBE = objOutboundShipInq.PALLET_CUBE,
                        @NOTE = objOutboundShipInq.PALLET_NOTE,
                        @PROCESS_ID = "ADD",

                    }, commandType: CommandType.StoredProcedure);
            }
        }
        
        public OutboundShipInq OutboundShipInqGetShipFromDetails(string term, string cmpid)
        {
            try
            {
                OutboundShipInq objShipfromDtl = new OutboundShipInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchShipHdrDtls = "proc_get_webmvc_ship_from_hdr";
                    IList<OutboundShipInq> ListIRFP = connection.Query<OutboundShipInq>(SearchShipHdrDtls, new
                    {

                        @Cmp_ID = cmpid,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objShipfromDtl.LstOutboundShipInqGetShipFromDetails = ListIRFP.ToList();
                }
                return objShipfromDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public OutboundShipInq OutboundShipInqGetShipToDetails(string term, string cmpid)
        {
            try
            {
                OutboundShipInq objShipToDtl = new OutboundShipInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_ship_To";
                    IList<OutboundShipInq> ListIRFP = connection.Query<OutboundShipInq>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmpid,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objShipToDtl.LstOutboundShipInqGetShipToDetails = ListIRFP.ToList();
                }
                return objShipToDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipInq GetCheckShipPost(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                OutboundShipInq objCustDtl = new OutboundShipInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_CHECK_SHIP_POST";
                    IList<OutboundShipInq> ListIRFP = connection.Query<OutboundShipInq>(SearchCustDtls, new
                    {
                        @P_STR_SHIPDOCID = objOutboundShipInq.ship_doc_id,
                       

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundShipInq.ListCheckShipPost = ListIRFP.ToList();
                }
                return objOutboundShipInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipInq OutboundShipInqSummaryRpt(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var status = objOutboundShipInq.status;
                    if (status == "ALL")
                    {
                        status = "";
                    }
                    else if (status == "SHIP")
                    {
                        status = "S";
                    }
                    else if (status == "POST")
                    {
                        status = "P";
                    }
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "proc_get_webmvc_outbound_shipping_Summary_Rpt";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundShipInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipInq.ship_doc_id_Fm,
                            @ShipDocIdTo = objOutboundShipInq.ship_doc_id_To,
                            @AlocNum = objOutboundShipInq.aloc_doc_id,
                            @ShipDtFr = objOutboundShipInq.Ship_dt_Fm,
                            @ShipDtTo = objOutboundShipInq.Ship_dt_To,
                            @ShipVia = objOutboundShipInq.ship_via_name,
                            @Status = status,
                            @CustID = objOutboundShipInq.cust_id,
                            @Shipto = objOutboundShipInq.ship_to,
                            @Whs = objOutboundShipInq.whs_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstOutboundShipInqdetail = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq OutboundShipInqBillofLaddingRpt(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {                 
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "proc_get_webmvc_outbound_bill_of_lading_rpt";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @CmpId = objOutboundShipInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipInq.ship_doc_id,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq GetShipPostGrid(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "SP_MVC_OB_SHIP_POST_GRIDDTL";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundShipInq.cmp_id,
                            @P_STR_SHIP_DOCID = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.ListShipPost = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq GetPoNum(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "SP_MVC_OB_GET_PONUM";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundShipInq.cmp_id,
                            @P_STR_SHIP_DOC_ID = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.ListGETPONUM = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq GetContId(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "SP_MVC_OB_GET_CONT_ID";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundShipInq.cmp_id,
                            @P_STR_SHIP_DOC_ID = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.ListGetContId = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq OutboundShipInqpackSlipRpt(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {                   
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "proc_get_webmvc_outbound_packingslip_rpt";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @CmpId = objOutboundShipInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipInq.ship_doc_id,                            
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstOutboundShipInqpackingSlipRpt = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq OutboundShipInqhdr(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "proc_get_webmvc_Shipping_Receive_hdr";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundShipInq.cmp_id,
                            @ShipdocId = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstOutboundShipInqpackingSlipRpt = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq OutboundShipInqdtl(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "proc_get_webmvc_Shipping_receive_details";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundShipInq.cmp_id,
                            @ShipdocId = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstOutboundShipInqpackingSlipRpt = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq GEtStrgBillTYpe(OutboundShipInq objOutboundShipInq)    //CR2018-03-09-001 Added By Soniya
        {
            try
            {
                OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_web_strg_bill_type";
                    IList<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(SearchCustDtls, new
                    {

                        @cmp_id = objOutboundShipInq.cmp_id,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundShipInq.ListStrgBillType = ListInq.ToList();
                }
                return objOutboundShipInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipInq GetTotalPalletCount(OutboundShipInq objOutboundShipInq)    //CR2018-03-13-001 Added By Nithya
        {
            try
            {
                OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_GET_TOTALPALLET_COUNT";
                    IList<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(SearchCustDtls, new
                    {

                        @P_STR_CMP_ID = objOutboundShipInq.cmp_id,
                        @P_STR_CNTR_ID = objOutboundShipInq.CNTR_ID,                      

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundShipInq.LstPalletCount = ListInq.ToList();
                }
                return objOutboundShipInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipInq GetObShipdtlTotalPalletCount(OutboundShipInq objOutboundShipInq)    //CR2018-03-13-001 Added By Nithya
        {
            try
            {
                OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_GET_SHIPDTLTOTPALLET_COUNT";
                    IList<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(SearchCustDtls, new
                    {

                        @P_STR_CMP_ID = objOutboundShipInq.cmp_id,
                        @P_STR_CNTR_ID = objOutboundShipInq.CNTR_ID,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundShipInq.LstshipPalletCount = ListInq.ToList();
                }
                return objOutboundShipInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipInq OutboundShipInqBillofLaddingExcel(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "SP_MVC_OB_BILL_OF_LADDING_EXCEL";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @CmpId = objOutboundShipInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstOutboundShipInqBillofLaddingRpt = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq GetTotCubesRpt(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "SP_MVC_OB_GET_TOTALCUBE";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @CmpId = objOutboundShipInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstPalletCount = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq GETRptValue(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "SP_MVC_OB_GET_TOT_WGT_CUBE";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @CmpId = objOutboundShipInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.LstshipbillOfladding = ListInq.ToList();

                    return objOutboundShipInq;
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
        public OutboundShipInq Add_To_proc_save_audit_trail(OutboundShipInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "Sp_ob_insert_audit_trail";
                    IEnumerable<OutboundShipInq> ListIbDocIdLIST = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objOutboundInq.cmp_id,
                            @p_str_ob_doc_id = objOutboundInq.Sonum,
                            @p_str_mode = objOutboundInq.mode,
                            @p_str_aloc_doc_id = objOutboundInq.aloc_doc_id,
                            @p_str_ship_doc_id = objOutboundInq.ship_doc_id,
                            @p_str_maker = objOutboundInq.maker,
                            @p_dt_maker_dt = objOutboundInq.makerdt,
                            @p_dt_access_stamp = objOutboundInq.makerdt,
                            @p_str_audit_comments = objOutboundInq.Auditcomment,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstshipbillOfladding = ListIbDocIdLIST.ToList();

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
        public OutboundShipInq Get_SoNo(OutboundShipInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "Sp_ob_Get_Sono";
                    IEnumerable<OutboundShipInq> ListIbDocIdLIST = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objOutboundInq.cmp_id,
                            @p_str_ship_doc_id = objOutboundInq.ship_doc_id,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstshipbillOfladding = ListIbDocIdLIST.ToList();

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
        public OutboundShipInq GetShipUnPostGrid(OutboundShipInq objOutboundShipInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "SP_MVC_OB_SHIP_UNPOST_GRIDDTL";
                    IEnumerable<OutboundShipInq> ListInq = connection.Query<OutboundShipInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundShipInq.cmp_id,
                            @P_STR_SHIP_DOCID = objOutboundShipInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipInq.ListShipPost = ListInq.ToList();

                    return objOutboundShipInq;
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
        public void SaveShipUnPost(OutboundShipInq objOutboundShipInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_OB_SHIP_UNPOST";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundShipInq.cmp_id,
                        @ship_doc_id = objOutboundShipInq.ship_doc_id,
                    }, commandType: CommandType.StoredProcedure);
            }
        }

        public DataTable OutboundShipInqBillofLaddingExcelTemplate(string l_str_cmp_id, string l_str_Ship_DocId)
        {

            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_webmvc_outbound_bill_of_lading_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@CmpId", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@ShipDocIdFr", SqlDbType.VarChar).Value = l_str_Ship_DocId;
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

    }
}
