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
    public class OutboundInqRepository : IOutboundInqRepository
    {

        public OutboundInq LoadCustConfig(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "PROC_GET_MVCWEB_LOAD_CUST_CONFIG";
                    IEnumerable<CustConfig> ListCustConfig = connection.Query<CustConfig>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);

                    objOutboundInq.objCustConfig = ListCustConfig.ToList()[0];

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

        public OutboundInq GetOutboundInq(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "proc_get_web_Ship_Req_Inq";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundInq.cmp_id,
                            @so_NumFm = objOutboundInq.so_numFm,
                            @so_NumTo = objOutboundInq.so_numTo,
                            @so_DtFm = objOutboundInq.so_dtFm,
                            @so_DtTo = objOutboundInq.so_dtTo,
                            @CustPo = objOutboundInq.cust_ordr_num,
                            @AlocNo = objOutboundInq.aloc_doc_id,
                            @status = objOutboundInq.status,
                            @store = objOutboundInq.store_id,
                            @batchId = objOutboundInq.quote_num,
                            @ShipdtFm = objOutboundInq.ShipdtFm,
                            @ShipdtTo = objOutboundInq.ShipdtTo,
                            @Cust_name = objOutboundInq.cust_name,
                            @Style = objOutboundInq.itm_num,
                            @Color = objOutboundInq.itm_color,
                            @Size = objOutboundInq.itm_size,
                            @route_dt = objOutboundInq.route_dt
                        },
                       commandTimeout:0,
                        
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqdetail = ListInq.ToList();

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
       
        public OutboundInq GetEcomOrderInq(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "proc_get_web_Ecom_Req_Inq";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundInq.cmp_id,
                            @so_NumFm = objOutboundInq.so_numFm,
                            @so_NumTo = objOutboundInq.so_numTo,
                            @so_DtFm = objOutboundInq.so_dtFm,
                            @so_DtTo = objOutboundInq.so_dtTo,
                            @CustPo = objOutboundInq.cust_ordr_num,
                            @AlocNo = objOutboundInq.aloc_doc_id,
                            @status = objOutboundInq.status,
                            @store = objOutboundInq.store_id,
                            @batchId = objOutboundInq.quote_num,
                            @ShipdtFm = objOutboundInq.ShipdtFm,
                            @ShipdtTo = objOutboundInq.ShipdtTo,
                            @Cust_name = objOutboundInq.cust_name,
                            @Style = objOutboundInq.itm_num,
                            @Color = objOutboundInq.itm_color,
                            @Size = objOutboundInq.itm_size,
                            @TrackNum = objOutboundInq.track_num,
                        },
                       commandTimeout: 0,

                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqdetail = ListInq.ToList();


                    IEnumerable<clsEComPrintOrders> ListEComPrintOrders = connection.Query<clsEComPrintOrders>("spEcomGetPrintOrders",
                        new
                        {
                            @pstrCmpId = objOutboundInq.cmp_id,
                            @pstrBatchId = objOutboundInq.quote_num,
                            @pstrSoNumFrom = objOutboundInq.so_numFm,
                            @pstrSoNumTo = objOutboundInq.so_numTo,
                            @pstrSoDtFrom = objOutboundInq.so_dtFm,
                            @pstrSoDtTo = objOutboundInq.so_dtTo,
                            @pstrStatus = string.Empty,
                            @pstrCallFunc = "PRINT"
                        },
                       commandTimeout: 0,

                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListEComPrintOrders = ListEComPrintOrders.ToList();

                    IEnumerable<clsEComPrintOrders> ListEComProcOrders = connection.Query<clsEComPrintOrders>("spEcomGetPrintOrders",
                      new
                      {
                          @pstrCmpId = objOutboundInq.cmp_id,
                          @pstrBatchId = objOutboundInq.quote_num,
                          @pstrSoNumFrom = objOutboundInq.so_numFm,
                          @pstrSoNumTo = objOutboundInq.so_numTo,
                          @pstrSoDtFrom = objOutboundInq.so_dtFm,
                          @pstrSoDtTo = objOutboundInq.so_dtTo,
                          @pstrStatus = string.Empty,
                          @pstrCallFunc = "SHIP"
                      },
                     commandTimeout: 0,

                      commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListEComProcOrders = ListEComProcOrders.ToList();


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

        public bool fnEcomSaveBatchPrint(clsEComPrintOrders objEComPrintOrders)
        {
            bool lblnStatus = false;
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    using (IDbTransaction tran = connection.BeginTransaction())
                    {
                        const string storedProcedure1 = "spEcomSaveBatchPrint";
                        connection.Execute(storedProcedure1,
                            new
                            {
                                @pstrCmpId = objEComPrintOrders.cmp_id,
                                @pstrBatchId = objEComPrintOrders.batch_id,
                                @pstrBatchStatus = objEComPrintOrders.batch_status,
                                @pstrOrdrRcvdDt = objEComPrintOrders.ordr_rcvd_dt,
                                @pintTotalOrders = objEComPrintOrders.total_orders,
                                @pintOpenOrders = objEComPrintOrders.open_orders,
                                @pintAlocOrders = objEComPrintOrders.alloc_orders,
                                @pintShipOrders = objEComPrintOrders.shipped_orders,
                                @pintPrintDt = objEComPrintOrders.printed_dt,
                                @pintPrintBy = objEComPrintOrders.printed_by,
                                @BatchFileName = objEComPrintOrders.batch_file_name,
                                @pstrMode = objEComPrintOrders.mode

                            }, commandType: CommandType.StoredProcedure, transaction: tran);
                        tran.Commit();
                        lblnStatus = true;
                    }
                }
            }
            catch(Exception ex)
            {
                lblnStatus = false;
            }
            return lblnStatus;


        }
        public OutboundInq fnEcomBatchPrint(string pstrCmpId, string pstrBatchId)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundInq objOutboundInq = new OutboundInq();
                    IEnumerable<clsEcomPrintAloc> ListEcomPrintAloc = connection.Query<clsEcomPrintAloc>("spEcomGetBatchPrintByLoc",
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrBatchId = pstrBatchId

                        },
                       commandTimeout: 0,

                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListEcomPrintAloc = ListEcomPrintAloc.ToList();


                    IEnumerable<clsEcomPrintDoc> ListEcomPrintDoc = connection.Query<clsEcomPrintDoc>("spEcomGetOBDocsByBatchId",
                      new
                      {
                          @pstrCmpId = pstrCmpId,
                          @pstrBatchId = pstrBatchId
                      },
                     commandTimeout: 0,

                    commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListEcomPrintDoc = ListEcomPrintDoc.ToList();


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

        public OutboundInq GetStyleDetails(OutboundInq objOutboundInq)
        {
            try
            {


                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvc_Itm_dtl";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @Cmp_ID = objOutboundInq.cmp_id,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmDtls = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq OutboundInqSUmmaryRpt(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundInq objOutboundInqCategory = new OutboundInq();
                    var status = objOutboundInq.status;
                    if (status == "OPEN")
                    {
                        status = "O";
                    }
                    else if (status == "SHIP")
                    {
                        status = "S";
                    }
                    else if (status == "ALL")
                    {
                        status = "";
                    }
                    else if (status == "POST")
                    {
                        status = "P";
                    }
                    else if (status == "ALOC")
                    {
                        status = "S";
                    }

                    const string storedProcedure2 = "proc_get_webmvc_ship_request_inquiry_Rpt";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundInq.cmp_id,
                            @so_NumFm = objOutboundInq.so_numFm,
                            @so_NumTo = objOutboundInq.so_numTo,
                            @so_DtFm = objOutboundInq.so_dtFm,
                            @so_DtTo = objOutboundInq.so_dtTo,
                            @CustPo = objOutboundInq.cust_ordr_num,
                            @AlocNo = objOutboundInq.aloc_doc_id,
                            @status = objOutboundInq.status,
                            @store = objOutboundInq.store_id,
                            @batchId = objOutboundInq.quote_num,
                            @ShipdtFm = objOutboundInq.ShipdtFm,
                            @ShipdtTo = objOutboundInq.ShipdtTo,
                            //CR_MVC_3PL_20180605-001 Added By NIthya
                            @Style = objOutboundInq.itm_num,
                            @Color = objOutboundInq.itm_color,
                            @Size = objOutboundInq.itm_size,
//END
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqSummaryRpt = ListInq.ToList();

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
        public OutboundInq OutboundInqPickStyleRpt(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();
                    string pstrBatchOperator = string.Empty;
                    string pstrBatchIdFm = string.Empty;
                    string pstrBatchIdTo = string.Empty;
                    string[] aryBatch ;
                    if (objOutboundInq.quote_num.Contains(","))
                    {
                        pstrBatchOperator = ",";
                        aryBatch = objOutboundInq.quote_num.Split(',');
                        pstrBatchIdFm = aryBatch[0];
                        pstrBatchIdTo = aryBatch[1];
                    }
                    else
                    {
                        pstrBatchIdFm = objOutboundInq.quote_num;
                        pstrBatchIdTo = string.Empty;
                    }
                    //else if (objOutboundInq.quote_num.Contains("-"))
                    //{
                    //    pstrBatchOperator = "-";
                    //    aryBatch = objOutboundInq.quote_num.Split(',');
                    //    pstrBatchIdFm = aryBatch[0];
                    //    pstrBatchIdTo = aryBatch[1];
                    //}

                    const string storedProcedure2 = "sp_ob_webmvc_ship_inq_pickStyle";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @StrCmpId = objOutboundInq.cmp_id,
                            @StrSoNumFm = objOutboundInq.so_numFm,
                            @StrSoNumTo = objOutboundInq.so_numTo,
                            @StrBatchIdFm = pstrBatchIdFm,
                            @StrBatchIdTo = pstrBatchIdTo,
                            @pstrBatchOperator = pstrBatchOperator,
                            @lstrStatus = "S",
                        },
                        commandType: CommandType.StoredProcedure, commandTimeout: 0);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();
                    if (objOutboundInq.LstOutboundInqpickstyleRpt.Count == 0)
                    {
                        objOutboundInq.RptResult = "empty";
                    }

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
        public OutboundInq GetUnPickQty(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_UNPICKQTY";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_ALOC_DOC_ID = objOutboundInq.aloc_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundUnPickQty = ListInq.ToList();


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

        public OutboundInq OutboundInqPickLocationRpt(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();
                    string pstrBatchOperator = string.Empty;
                    string pstrBatchIdFm = string.Empty;
                    string pstrBatchIdTo = string.Empty;
                    string[] aryBatch;
                    if (objOutboundInq.quote_num.Contains(","))
                    {
                        pstrBatchOperator = ",";
                        aryBatch = objOutboundInq.quote_num.Split(',');
                        pstrBatchIdFm = aryBatch[0];
                        pstrBatchIdTo = aryBatch[1];
                    }
                    else
                    {
                        pstrBatchIdFm = objOutboundInq.quote_num;
                        pstrBatchIdTo = string.Empty;
                    }
                    const string storedProcedure2 = "sp_ob_webmvc_ship_inq_pickLocation";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @StrCmpId = objOutboundInq.cmp_id,
                            @StrSoNumFm = objOutboundInq.so_numFm,
                            @StrSoNumTo = objOutboundInq.so_numTo,
                            @StrBatchIdFm = pstrBatchIdFm,
                            @StrBatchIdTo = pstrBatchIdTo,
                            @pstrBatchOperator = pstrBatchOperator,
                            @lstrStatus = "S",
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();
                    if (objOutboundInq.LstOutboundInqpickstyleRpt.Count == 0)
                    {
                        objOutboundInq.RptResult = "empty";
                    }
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
        public OutboundInq OutboundInqGridSummary(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var status = objOutboundInq.status;
                    if (status == "OPEN")
                    {
                        status = "O";
                    }
                    else if (status == "SHIP")
                    {
                        status = "S";
                    }
                    else if (status == "ALL")
                    {
                        status = "ALL";
                    }
                    else if (status == "POST")
                    {
                        status = "P";
                    }
                    else if (status == "ALOC")
                    {
                        status = "S";
                    }
                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "sp_get_webmvc_Grid_inquiry_Summary";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @CompID = objOutboundInq.cmp_id,
                            @SRNumFm = objOutboundInq.so_numFm,
                            @SRNumTo = objOutboundInq.so_numTo,
                            @ShipReqDtFr = objOutboundInq.so_dtFm,
                            @ShipReqDtTo = objOutboundInq.so_dtTo,
                            @CustPO = objOutboundInq.cust_ordr_num,
                            @AlocNum = objOutboundInq.aloc_doc_id,
                            @Status = status,
                            @StoreId = objOutboundInq.store_id,
                            @BatchID = objOutboundInq.quote_num,
                            @ShipDtFm = objOutboundInq.ShipdtFm,
                            @ShipDtTo = objOutboundInq.ShipdtTo,
                            @Style = objOutboundInq.itm_num,
                            @Color = objOutboundInq.itm_color,
                            @Size = objOutboundInq.itm_size,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();

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
        public OutboundInq OutboundInqShipAck(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "proc_get_webmvc_Ship_Request_Ack_Rpt";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundInq.cmp_id,
                            @Sonum = objOutboundInq.Sonum,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();

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
 public OutboundInq OutboundInqShipAckCtnvalues(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "proc_get_webmvc_Ship_Request_Ack_Rptctns_values";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundInq.cmp_id,
                            @Sonum = objOutboundInq.Sonum,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();

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
        public OutboundInq GetAlocUnPostGridLoadItem(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_ALOC_UNPOST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_ALOC_DOC_ID = objOutboundInq.aloc_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocUnPostGridLoadDtls = ListInq.ToList();

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

        public OutboundInq GetAlocGridLoadItem(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "Proc_get_mvc_web_AlocPost_Load_Itm";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_Str_CmpId = objOutboundInq.cmp_id,
                            @P_Str_AlocId = objOutboundInq.aloc_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqAlocGridLoadDtls = ListInq.ToList();

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
        public OutboundInq GetAlocType(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "Proc_get_mvc_web_AlocType";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_Str_AlocDocId = objOutboundInq.aloc_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstGetAlocType = ListInq.ToList();

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
        public void GetAlocPostDirect(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvc_web_Pick_Post_Aloc_tbl_iv_itm_trn_in_tpw_direc";//CR-201805-02-001 Added By Nithya
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @aloc_doc_id = objOutboundInq.aloc_doc_id,
                        @ship_doc_num = objOutboundInq.bol_num,
                        @ship_to_id = objOutboundInq.ship_to_id,
                        @doc_date = objOutboundInq.aloc_dt,




                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void UpdateTrnHdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_GET_DEL_ALOC_QTY_TO_TRN_HDR";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @P_STR_CMP_ID = objOutboundInq.cmp_id,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_WHS_ID = objOutboundInq.whs_id,
                        @P_STR_LOT_ID = objOutboundInq.lot_id,
                        @P_STR_RCVD_DT = objOutboundInq.rcvd_dt,
                        @P_STR_LOC_ID = objOutboundInq.loc_id,
                        @P_STR_ACTION_QTY = objOutboundInq.itm_qty,
                        @PROCESS_ID = "Modify",

                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void InsertShipDtl(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_OB_GET_SHIP_DTL_DIRECT";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @opt = "A",
                        @ship_doc_num = objOutboundInq.bol_num,
                        @aloc_doc_num = objOutboundInq.aloc_doc_id,
                        @ReturnValue = "1",

                    }, commandType: CommandType.StoredProcedure);
            }
        }

        public void GetAlocPost(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvc_web_Pick_Post_Aloc_tbl_iv_itm_trn_in_tpw";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @aloc_doc_id = objOutboundInq.aloc_doc_id,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public OutboundInq GetPickGridLoadItem(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "Proc_get_mvc_web_AlocPickGrid_Load_Itm";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_Str_CmpId = objOutboundInq.cmp_id,
                            @P_Str_DocId = objOutboundInq.aloc_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqPickGridLoadDtls = ListInq.ToList();

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
        public OutboundInq OutboundShipInqhdr(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "proc_get_webmvc_ship_request_hdr";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @CompID = objOutboundInq.CompID,
                            @BatchID = objOutboundInq.QuoteNum,
                            @ShipReqID = objOutboundInq.ShipReqID,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();

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
        public OutboundInq OutboundShipInqdtl(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "proc_get_webmvc_ship_request_details";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @CompID = objOutboundInq.CompID,
                            @BatchID = objOutboundInq.QuoteNum,
                            @ShipReqID = objOutboundInq.ShipReqID,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();

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
        public OutboundInq GetIbSRIdDetail(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_Ship_doc_id";
                    IEnumerable<OutboundInq> ListSRDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @SRdocid = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListSRDocId = ListSRDocIdLIST.ToList();
                    objOutboundInq.SRNUm = objOutboundInq.ListSRDocId[0].SRNUm;

                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public OutboundInq GetShipNum(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_Ship_doc_num";
                    IEnumerable<OutboundInq> ListSRDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @Bol_num = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListShipDocNum = ListSRDocIdLIST.ToList();
                    objOutboundInq.ShipDocNum = objOutboundInq.ListShipDocNum[0].ShipDocNum;

                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public OutboundInq GetCustId(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_OB_GET_CUSTID";
                    IEnumerable<OutboundInq> ListSRDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_ALOC_DOC_ID = objOutboundInq.aloc_doc_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListCustId = ListSRDocIdLIST.ToList();


                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public OutboundInq GetShipToAddress(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_OB_GET_SHIPTOADDRESS";
                    IEnumerable<OutboundInq> ListSRDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_CUST_ID = objOutboundInq.cust_id,
                            @P_STR_SHIPTO_ID = objOutboundInq.ship_to_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListShipToAddress = ListSRDocIdLIST.ToList();


                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public void UpdateStatusInAlocHdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvc_web_UpdateStatus_alochdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_Str_AlocDocId = objOutboundInq.aloc_doc_id,
                        @P_Str_AlocDt = objOutboundInq.aloc_dt
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateStatusInAlocDtl(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvc_web_UpdateStatus_alocdtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_Str_AlocDocId = objOutboundInq.aloc_doc_id

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public OutboundInq ItemXGetitmDetails(string term, string cmp_id)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_Itm_dtl";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
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

        public OutboundInq fnSaveSoTracking(string pstrMode, string pstrCmpId, string pstrSoNum, string pstrTrackNumType , string pstrTrackNum, string pstrTrackStatus, string pstrTrackDate, string pstrProcessId )
        {
            OutboundInq objOutboundInq = new OutboundInq();
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "spSaveSoTracking";

                IList<SOTracking> ListSOTracking = connection.Query<SOTracking>(storedProcedure1, new
                {

                    @pstrMode = pstrMode,
                    @pstrCmpId = pstrCmpId,
                    @pstrSoNum = pstrSoNum,
                    @pstrTrackNumType = pstrTrackNumType,
                    @pstrTrackNum = pstrTrackNum,
                    @pstrTrackStatus = pstrTrackStatus,
                    @pstrTrackDate = pstrTrackDate,
                    @pstrProcessId = pstrProcessId,

                }, commandType: CommandType.StoredProcedure).ToList();
                objOutboundInq.ListSOTracking = ListSOTracking.ToList();
                return objOutboundInq;
            }

        }

        public OutboundInq fnGetSoTracking( string pstrCmpId, string pstrSoNum, string pstrTrackNum)
        {
            OutboundInq objOutboundInq = new OutboundInq();
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "spGetSoTracking";

                IList<SOTracking> ListSOTracking = connection.Query<SOTracking>(storedProcedure1, new
                {

                    @pstrCmpId = pstrCmpId,
                    @pstrSoNum = pstrSoNum,
                    @pstrTrackNum = pstrTrackNum,

                }, commandType: CommandType.StoredProcedure).ToList();
                objOutboundInq.ListSOTracking = ListSOTracking.ToList();
                return objOutboundInq;
            }

        }


        public void InsertTemptableValue(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "proc_get_mvcweb_Insert_Temptable_ShipEntry";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @shipentry = objOutboundInq.id,
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @line_num = objOutboundInq.LineNum,
                        @status = objOutboundInq.STATUS,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @itm_name = objOutboundInq.itm_name,
                        @po_num = objOutboundInq.po_num,
                        @ordr_qty = objOutboundInq.OrdQty,
                        @ctn_qty = objOutboundInq.ppk,
                        @ordr_ctns = objOutboundInq.ctn,
                        @cube = objOutboundInq.Cube,
                        @wgt = objOutboundInq.weight,
                        @note = objOutboundInq.Note,
                        @lot_id = objOutboundInq.lot_id,
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.height,
                        @Errdesc = objOutboundInq.Errdesc,
                        @qty_uom = objOutboundInq.qty_uom,
                        @list_uom = objOutboundInq.price_uom,
                        @pack_id = objOutboundInq.pack_id,
                        @kit_itm = objOutboundInq.kit_itm,
                        @Size = objOutboundInq.dtlsize,
                        @cust_itm_num = objOutboundInq.cust_itm_num,
                        @cust_itm_color = objOutboundInq.cust_itm_color,
                        @cust_itm_dec = objOutboundInq.cust_itm_desc,
                        @list_price = objOutboundInq.list_price,
                        @temp_stk_ref_num = objOutboundInq.temp_stk_ref_num
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public void InsertShipHdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "SP_MVC_OB_INSERT_SHIP_HDR";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opt = "A",
                        @cmp_id = objOutboundInq.cmp_id,
                        @ship_doc_id = objOutboundInq.bol_num,
                        @ship_dt = objOutboundInq.aloc_dt,
                        @status = "P",
                        @whs_id = objOutboundInq.whs_id,
                        @cust_id = objOutboundInq.cust_id,
                        @ship_to = objOutboundInq.ship_to_id,
                        @mail_name = objOutboundInq.mail_name,
                        @addr_line1 = objOutboundInq.addr_line1,
                        @addr_line2 = objOutboundInq.addr_line2,
                        @city = objOutboundInq.city,
                        @state_id = objOutboundInq.state_id,
                        @post_code = objOutboundInq.post_code,
                        @cntry_id = objOutboundInq.cntry_id,
                        @ship_via_name = "01",
                        @ship_type = "GROUND",
                        @tot_wgt = "0",
                        @notes = objOutboundInq.note,
                        @process_id = "Add",
                        @ReturnValue = "1",

                    }, commandType: CommandType.StoredProcedure);
            }

        }

        public OutboundInq GetGridList(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_gridlist";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @temp_stk_ref_num = objOutboundInq.temp_stk_ref_num

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetSRIdDtl(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_Ship_Entry";
                    IEnumerable<OutboundInq> ListSRDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @SRentry = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListSRDocId = ListSRDocIdLIST.ToList();
                    objOutboundInq.id = objOutboundInq.ListSRDocId[0].id;

                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public OutboundInq LoadAvailQty(OutboundInq objOutboundInq)
        {
            try
            {
                var ALOC_BY = objOutboundInq.aloc_by;
                if(ALOC_BY=="PO")
                {
                    OutboundInq objCustDtl = new OutboundInq();

                    using (IDbConnection connection = ConnectionManager.OpenConnection())
                    {
                        const string SearchCustDtls = "sp_get_po_Availqty";
                        IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                        {

                            @cmp_id = objOutboundInq.cmp_id,
                            @itm_code = objOutboundInq.itm_code,
                            @po_num= objOutboundInq.po_num,

                        }, commandType: CommandType.StoredProcedure).ToList();
                        objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                    }
                }
                else
                {
                    OutboundInq objCustDtl = new OutboundInq();

                    using (IDbConnection connection = ConnectionManager.OpenConnection())
                    {
                        const string SearchCustDtls = "proc_get_mvcweb_Availqty";
                        IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                        {

                            @cmp_id = objOutboundInq.cmp_id,
                            @itm_code = objOutboundInq.itm_code

                        }, commandType: CommandType.StoredProcedure).ToList();
                        objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                    }
                }            
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetItmNameDetails(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_itmname";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @itm_code = objOutboundInq.itm_code

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListItmStyledtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public OutboundInq GetPickStyleDetails(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_loadStyledtl";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @cmp_id = objOutboundInq.cmp_id

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListItmStyledtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetShipReqEntryTempGridDtl(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_tempdtl";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objOutboundInq.cmp_id,//CR2018-03-08-001 Added By Nithya
                        @P_STR_SO_NUM = objOutboundInq.Sonum,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListItmStyledtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetOutSaleId(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "PROC_GET_MVC_WEB_OUTSALEID";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CUST_ID = objOutboundInq.cmp_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListGetOutSaleId = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void Add_To_proc_save_so_dtl_excel(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_save_so_dtl_excel";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @line_num = objOutboundInq.line_num,
                        @itm_line = "1",
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @asrt_type = objOutboundInq.kit_itm,
                        @kit_id = objOutboundInq.itm_code,
                        @lot_id = objOutboundInq.lot_id,
                        @status = "O",
                        @step = "ENTRY",
                        @cust_item_num = objOutboundInq.cust_itm_num,
                        @cust_item_color = objOutboundInq.cust_itm_color,
                        @cust_item_size = objOutboundInq.cust_itm_desc,
                        @ordr_qty = objOutboundInq.ordr_qty,
                        @ordr_ctns = objOutboundInq.ordr_ctns,
                        @qty_uom = objOutboundInq.qty_uom,
                        @list_price = objOutboundInq.list_price,
                        @list_uom = objOutboundInq.list_uom,
                        @sell_price = objOutboundInq.sell_price,
                        @sell_uom = objOutboundInq.sell_uom,
                        @pack_id = objOutboundInq.pack_id,
                        @comm_pcnt = objOutboundInq.comm_pcnt,
                        @cube = objOutboundInq.cube,
                        @size = objOutboundInq.Size,
                        @wgt = objOutboundInq.wgt,
                        @itm_qty = objOutboundInq.itm_qty,
                        @ctn_qty = objOutboundInq.ctn_qty,
                        @aloc_qty = objOutboundInq.aloc_qty,
                        @ship_qty = objOutboundInq.ship_qty,
                        @back_ordr_qty = objOutboundInq.back_ordr_qty,
                        @note = objOutboundInq.note,
                        @process_id = "DI-ADD",
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.depth,
                        @ponum = objOutboundInq.po_num,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Add_To_proc_save_so_dtl_due_excel(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_save_so_due_excel";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @dtl_line = objOutboundInq.line_num,
                        @due_line = "1",
                        @itm_line = "1",
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @asrt_type = objOutboundInq.kit_itm,
                        @kit_id = objOutboundInq.itm_code,
                        @status = "O",
                        @dept_id = objOutboundInq.dept_id,
                        @store_id = objOutboundInq.store_id,
                        @due_qty = objOutboundInq.ordr_qty,
                        @due_ctns = objOutboundInq.ordr_ctns,
                        @whs_id = objOutboundInq.fob,
                        @due_dt = objOutboundInq.due_dt,
                        @avail_dt = objOutboundInq.avail_dt,
                        @aloc_dt = objOutboundInq.aloc_dt,
                        @pick_dt = objOutboundInq.pick_dt,
                        @load_dt = objOutboundInq.load_dt,
                        @req_ship_dt = objOutboundInq.ShipDt,
                        @cancel_dt = objOutboundInq.CancelDt,
                        @advt_dt = objOutboundInq.ShipDt,
                        @aloc_qty = "0",
                        @ship_qty = "0",
                        @back_ordr_qty = "0",
                        @process_id = "DI-ADD",
                        @po_num = objOutboundInq.po_num,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Add_To_proc_save_so_hdr_excel(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_save_so_hdr_excel";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @status = "O",
                        @step = "ENTRY",
                        @price_tkt = objOutboundInq.pricetkt,
                        @so_dt = objOutboundInq.So_dt,
                        @ordr_type = objOutboundInq.OrdType,
                        @cust_id = objOutboundInq.CustName,//CR20180718-001 Added By Nithya
                        @cust_name = objOutboundInq.cust_id,
                        @ordr_num = objOutboundInq.ordr_num,
                        @cust_ordr_num = objOutboundInq.CustPO,
                        @cust_ordr_dt = objOutboundInq.CustOrderdt,
                        @cntr_num = "111",
                        @quote_num = objOutboundInq.Batchid,
                        @in_sale_id = objOutboundInq.AuthId,
                        @out_sale_id = "N",
                        @dept_id = objOutboundInq.dept_id,
                        @store_id = objOutboundInq.store_id,
                        @fob = objOutboundInq.fob,
                        @shipto_id = objOutboundInq.shipto_id,
                        @shipvia_id = objOutboundInq.shipvia_id,
                        @terms_id = 1,
                        @freight_id = objOutboundInq.freight_id,
                        @ordr_lines = objOutboundInq.itm_line,
                        @total_qty = objOutboundInq.total_qty,
                        @ordr_cost = objOutboundInq.ordr_cost,
                        @tax_pcnt = 0,
                        @sh_chg = objOutboundInq.shipchrg,
                        @ship_dt = objOutboundInq.ShipDt,
                        @cancel_dt = objOutboundInq.CancelDt,
                        @note = objOutboundInq.note,
                        @process_id = "DI-ADD",
                        @temp_check = "N",
                        @temp_user = objOutboundInq.user_id,
                        @pick_no = objOutboundInq.pick_no,
                        @ref_no = objOutboundInq.ref_no
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Add_To_proc_save_so_addr_excel(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_save_so_addr_excel";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @soldto_id = objOutboundInq.shipto_id,
                        @sl_attn = objOutboundInq.Attn,
                        @sl_mail_name = objOutboundInq.Mailname,
                        @sl_addr_line1 = objOutboundInq.Addr1,
                        @sl_addr_line2 = objOutboundInq.Addr2,
                        @sl_city = objOutboundInq.ShipToCity,
                        @sl_state_id = objOutboundInq.ShipToState,
                        @sl_post_code = objOutboundInq.ShipToZipCode,
                        @sl_cntry_id = objOutboundInq.ShipToCountry,
                        @shipto_id = objOutboundInq.shipto_id,
                        @st_attn = objOutboundInq.Attn,
                        @st_mail_name = objOutboundInq.Mailname,
                        @st_addr_line1 = objOutboundInq.Addr1,
                        @st_addr_line2 = objOutboundInq.Addr2,
                        @st_city = objOutboundInq.ShipToCity,
                        @st_state_id = objOutboundInq.ShipToState,
                        @st_post_code = objOutboundInq.ShipToZipCode,
                        @st_cntry_id = objOutboundInq.ShipToCountry,
                        @dc_id = objOutboundInq.dc_id,
                        @billto_id = objOutboundInq.shipto_id,
                        @bt_attn = objOutboundInq.Attn,
                        @bt_mail_name = objOutboundInq.Mailname,
                        @bt_addr_line1 = objOutboundInq.Addr1,
                        @bt_addr_line2 = objOutboundInq.Addr2,
                        @bt_city = objOutboundInq.ShipToCity,
                        @bt_state_id = objOutboundInq.ShipToState,
                        @bt_post_code = objOutboundInq.ShipToZipCode,
                        @bt_cntry_id = objOutboundInq.ShipToCountry,
                        @process_id = "DI-ADD",
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public OutboundInq Getkitflag(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_kitflag";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListItmStyledtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetShipReqEditDtl(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_IB_LOAD_SHIPREQ_EDIT_DTL";
                    IEnumerable<OutboundInq> ListIbDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @cmp_id = objOutboundInq.cmp_id.Trim(),
                            @so_num = objOutboundInq.so_num.Trim(),
                            @ShipEntry = objOutboundInq.id.Trim(),


                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListLoadShipReqEditDtl = ListIbDocIdLIST.ToList();

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
        public OutboundInq GetCustDtl(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_CustDtl";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @cust_id = objOutboundInq.cust_id,
                        @itm_num = objOutboundInq.itm_num,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq Getitmlist(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_Itmlist";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();//CR-20180410 -Added By Nithya for cmp_id Validation
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public OutboundInq GetEcomitmlist(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "spEcomItemList";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public OutboundInq GetCSVList(OutboundInq objOutboundInq)
        {

            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_webmvc_temp_shipdtl_fetch";
                IEnumerable<OutboundInq> ListEcomRecieveOrdersRole = connection.Query<OutboundInq>(storedProcedure1,
                    new
                    {


                    },
                    commandType: CommandType.StoredProcedure);
                objOutboundInq.lstobjOutboundInq = ListEcomRecieveOrdersRole.ToList();
            }

            return objOutboundInq;
        }
        public void OutboundInqTempDelete(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_web_rec_odr_temp_shipdtl_delete";
                connection.Execute(storedProcedure1,
                    new
                    {
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void DeleteTempshipEntry(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_temptable_shipentry_del_item";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.so_num,
                        @line_num = objOutboundInq.LineNum,
                        @itm_code = objOutboundInq.itm_code,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public OutboundInq GetViewDetail(OutboundInq objOutboundInq)
        {

            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_webmvc_ShipReq_hdrdetail";
                IEnumerable<OutboundInq> ListEcomRecieveOrdersRole = connection.Query<OutboundInq>(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.so_num,

                    },
                    commandType: CommandType.StoredProcedure);
                objOutboundInq.lstobjOutboundInq = ListEcomRecieveOrdersRole.ToList();
            }

            return objOutboundInq;
        }
        public OutboundInq GetViewDetailgrid(OutboundInq objOutboundInq)
        {

            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_webmvc_ShipReq_griddetail";
                IEnumerable<OutboundInq> ListEcomRecieveOrdersRole = connection.Query<OutboundInq>(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.so_num,
                        @temp_stk_ref_num = objOutboundInq.temp_stk_ref_num,
                        @bo_flag = objOutboundInq.bo_flag

                    },
                    commandType: CommandType.StoredProcedure);
                objOutboundInq.LstItmxCustdtl = ListEcomRecieveOrdersRole.ToList();
            }

            return objOutboundInq;
        }
        public OutboundInq GetViewAddrDetail(OutboundInq objOutboundInq)
        {

            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_webmvc_ShipReq_Addrdetail";
                IEnumerable<OutboundInq> ListEcomRecieveOrdersRole = connection.Query<OutboundInq>(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.so_num,

                    },
                    commandType: CommandType.StoredProcedure);
                objOutboundInq.lstobjOutboundInq = ListEcomRecieveOrdersRole.ToList();
            }

            return objOutboundInq;
        }
        public void DeleteShipEntry(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_webmvc_sp_so_del";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.so_num,
                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public OutboundInq GetGridEditData(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_Temptable_ShipEntry_edit";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        //@shipentry = objOutboundInq.ShipEntry,
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.so_num,
                        @line_num = objOutboundInq.LineNum,
                        @itm_code = objOutboundInq.itm_code,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListGridEditData = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public OutboundInq GetCheckExistGridData(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_temptable_shipentry_exist_item";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @line_num = objOutboundInq.LineNum,
                        @itm_code = objOutboundInq.itm_code,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListGridEditData = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetCheckExistStyle(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "PROC_GET_MVCWEB_CHECK_EXIST_STYLE";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objOutboundInq.cmp_id,
                        @P_STR_SO_NUM = objOutboundInq.Sonum,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_PO_NUM = objOutboundInq.po_num,
                        @P_STR_ALOC_BY = objOutboundInq.aloc_by,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListCheckExistStyle = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void DeleteTempshipEntrytable(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_webmvc_deltemptable";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @p_str_Cmp_id = objOutboundInq.cmp_id,//
                        @p_str_so_num = objOutboundInq.Sonum,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public OutboundInq GetGridDeleteData(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_temptable_shipentry_grid_delete_item";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @line_num = objOutboundInq.LineNum,
                        @itm_code = objOutboundInq.itm_code,
                        @temp_stk_ref_num = objOutboundInq.temp_stk_ref_num

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetShipToAddressSave(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_save_tbl_ma_cust_shipto";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @cust_id = objOutboundInq.cust_id,
                        @shipto = objOutboundInq.shipto_id,
                        @store_id = objOutboundInq.store_id,
                        @attn = objOutboundInq.Attn,                    //CR_3PL_MVC_OB_2018_0226_003 - Modified by Soniya
                        @mail_name = objOutboundInq.Mailname,
                        @addr_line1 = objOutboundInq.Addr1,
                        @addr_line2 = objOutboundInq.Addr2,
                        @city = objOutboundInq.ShipToCity,
                        @state_id = objOutboundInq.ShipToState,
                        @post_code = objOutboundInq.ShipToZipCode,
                        @cntry_id = objOutboundInq.ShipToCountry,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListGridEditData = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq OutboundShipAloc(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OUT_GET_Aloc_Dtl";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_SONUM = objOutboundInq.Sonum,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.lstShipAlocdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq SoNumFrom_Validation(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OUT_GET_SONUM_VALIDATION";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_SONUM = objOutboundInq.Sonum,
                        @P_STR_CMPID = objOutboundInq.CompID,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.lstShipAlocdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq OutboundSoldtoId(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OUT_GET_SOLDTOID";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMPID = objOutboundInq.CompID,
                        @P_STR_SONUM = objOutboundInq.Sonum,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.lstShipAlocdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetObalocIdDetail(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_GET_ALOC_DOC_ID";
                    IEnumerable<OutboundInq> ListIbDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @alocdocid = "",
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocdocid = ListIbDocIdLIST.ToList();
                    objOutboundInq.AlocdocId = objOutboundInq.LstAlocdocid[0].AlocdocId;

                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public OutboundInq OutboundSelectionhdr(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_SELECTION_HDR";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMPID = objOutboundInq.CompID,
                        @P_STR_SONUM_FM = objOutboundInq.Sonum,
                        @P_STR_SONUM_TO = objOutboundInq.Sonum,
                        @P_STR_DELDT_FM = objOutboundInq.DeliverydtFm,
                        @P_STR_DELDT_TO = objOutboundInq.DeliverydtTo,
                        @P_STR_CUSTID = objOutboundInq.Cust_id,
                        @P_STR_ITMNUM = objOutboundInq.itm_num,
                        @P_STR_ITMCOLOR = objOutboundInq.itm_color,
                        @P_STR_ITMSIZE = objOutboundInq.itm_size,
                        @P_STR_DCID = objOutboundInq.DCID,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.lstShipAlocdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void InsertTempAUTOALOC(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "SP_MVC_ALOC_HDR_LIST";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_STR_CMPID = objOutboundInq.cmp_id,
                        @P_STR_SO_NUM = objOutboundInq.so_num,
                        @P_INT_LINE_NUM = objOutboundInq.line_num,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_STATUS = objOutboundInq.status,
                        @P_STR_QTY_UOM = objOutboundInq.qty_uom,
                        @P_INT_ALOC_QTY = objOutboundInq.aloc_qty,
                        @P_INT_DUE_QTY = objOutboundInq.due_qty,
                        @P_INT_BACK_ORDER_QTY = objOutboundInq.back_ordr_qty,
                        @P_INT_DTL_LINE = objOutboundInq.dtl_line,
                        @P_INT_DUE_LINE = objOutboundInq.due_line,
                        @P_STR_ITM_NUM = objOutboundInq.itm_num,
                        @P_STR_ITM_COLOR = objOutboundInq.itm_color,
                        @P_STR_ITM_SIZE = objOutboundInq.itm_size,
                        @P_STR_STORE_ID = objOutboundInq.store_id,
                        @P_STR_WHS_ID = objOutboundInq.whs_id,
                        @P_STR_CUST_ID = objOutboundInq.cust_id,
                        @P_STR_SHIPTO_ID = objOutboundInq.shipto_id,
                        @P_STR_DC_ID = objOutboundInq.dc_id,
                        @P_STR_ITM_NAME = objOutboundInq.itm_name
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public OutboundInq OutboundGETITMNAME(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_ITM_NAME";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMPID = objOutboundInq.cmp_id,
                        @P_STR_ITM_NUM = objOutboundInq.itm_num,
                        @P_STR_ITM_COLOR = objOutboundInq.itm_color,
                        @P_STR_ITM_SIZE = objOutboundInq.itm_size,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq CheckAllocpost(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_CHKALLOC_POST";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objOutboundInq.cmp_id,
                        @P_STR_ALOC_DOC_ID = objOutboundInq.aloc_doc_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstCheckAllocPost = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public OutboundInq OutboundGETALOCSORTSTMT(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OUT_GET_ALOC_SORTSTMT";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMPID = objOutboundInq.cmp_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq OutboundGETAVILQTY(OutboundInq objOutboundInq)
        {
            try
            {
               
                OutboundInq objCustDtl = new OutboundInq();
                var ALOC_BY = objOutboundInq.aloc_by;
                if (ALOC_BY == "PO")
                {
                    using (IDbConnection connection = ConnectionManager.OpenConnection())
                    {
                        const string SearchCustDtls = "Sp_OB_Aloc_Avail_Itm_at_Itm_Trn_In_direc_by_PO";
                        IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                        {
                            @cmp_id = objOutboundInq.cmp_id,
                            @itm_code = objOutboundInq.itm_code,
                            @itm_num = objOutboundInq.itm_num,
                            @itm_color = objOutboundInq.itm_color,
                            @itm_size = objOutboundInq.itm_size,
                            @whs_id = objOutboundInq.whs_id,
                            @po_num = objOutboundInq.po_num,
                        }, commandType: CommandType.StoredProcedure).ToList();
                        objOutboundInq.LstAvailqty = ListIRFP.ToList();
                    }
                }
                else
                {
                    using (IDbConnection connection = ConnectionManager.OpenConnection())
                    {
                        const string SearchCustDtls = "Sp_MVC_OB_Aloc_Avail_Itm_at_Itm_Trn_In_direc";
                        IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                        {
                            @cmp_id = objOutboundInq.cmp_id,
                            @itm_code = objOutboundInq.itm_code,
                            @itm_num = objOutboundInq.itm_num,
                            @itm_color = objOutboundInq.itm_color,
                            @itm_size = objOutboundInq.itm_size,
                            @whs_id = objOutboundInq.whs_id,
                        }, commandType: CommandType.StoredProcedure).ToList();
                        objOutboundInq.LstAvailqty = ListIRFP.ToList();
                    }
                }
                 
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq OutboundGETTEMPLIST(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_TEMPLIST";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @p_str_cmp_id = objOutboundInq.cmp_id,
                        @p_str_so_num = objOutboundInq.so_num

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void DeleteAUTOALOC(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "SP_MVC_DEL_AUTO_ALOC";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_STR_CMP_ID = objOutboundInq.cmp_id,
                        @p_STR_SO_NUM = objOutboundInq.so_num
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void SaveAlocUnPost(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                using (IDbTransaction tran = connection.BeginTransaction())
                {
                    const string storedProcedure1 = "spOBALocUnpost";
                    connection.Execute(storedProcedure1,
                        new
                        {
                            @pstrCmpId = objOutboundInq.cmp_id,
                            @pstrAlocDocId = objOutboundInq.aloc_doc_id
                        }, commandType: CommandType.StoredProcedure, transaction: tran);
                    tran.Commit();
                }
            }
        }
        public void InsertTempAlocdtl(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "SP_MVC_INSERT_TEMP_ALOC_DTL";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @P_INT_ALCLINE = objOutboundInq.aloc_line,
                        @P_INT_CTNLINE = objOutboundInq.ctn_line,
                        @P_INT_ITMLINE = 1,
                        @P_STR_SO_NUM = objOutboundInq.so_num,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_ITM_NUM = objOutboundInq.itm_num,
                        @P_STR_ITM_COLOR = objOutboundInq.itm_color,
                        @P_STR_ITM_SIZE = objOutboundInq.itm_size,
                        @P_STR_WHS_ID = objOutboundInq.whs_id,
                        @P_STR_LOC_ID = objOutboundInq.loc_id,
                        @P_STR_RCVD_DT = objOutboundInq.rcvd_dt,
                        @P_INT_DUE_QTY = objOutboundInq.due_qty,
                        @P_INT_PKG_QTY = objOutboundInq.pkg_qty,
                        @P_INT_AVAIL_QTY = objOutboundInq.avail_qty,
                        @P_INT_ALOC_QTY = objOutboundInq.aloc_qty,
                        @P_INT_BO_QTY = objOutboundInq.back_ordr_qty,
                        @P_STR_LOT_ID = objOutboundInq.lot_id,
                        @P_STR_PALET_ID = objOutboundInq.Palet_id,
                        @P_INT_SOLINE = objOutboundInq.line_num,
                        @P_INT_DUELINE = objOutboundInq.due_line,
                        @P_STR_PO_NUM = objOutboundInq.po_num,
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public OutboundInq OutboundGETTEMPALOCSUMMARY(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_TEMPALOCSUMMARY_LIST";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @p_STR_SO_NUM = objOutboundInq.so_num
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstAlocSummary = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq OutboundGETALOCDTL(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_SELECTION_DTL";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMPID = objOutboundInq.cmp_id,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_ITMNUM = objOutboundInq.itm_num,
                        @P_STR_ITMCOLOR = objOutboundInq.itm_color,
                        @P_STR_ITMSIZE = objOutboundInq.itm_size,
                        @P_STR_WHSID = objOutboundInq.whs_id,
                        @P_STR_SORTSTMT = objOutboundInq.aloc_sort_stmt,
                        @P_STR_ALOCBY = objOutboundInq.aloc_by,
                        @P_STR_PONUM = objOutboundInq.po_num,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstAlocSummary = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void InsertTempAlocSummary(OutboundInq objOutboundInq)
        {
            try { 
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "SP_MVC_INSERT_TEMP_ALOC_SUMMARY";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_INT_LINENUM = objOutboundInq.aloc_line,
                        @P_STR_LOCID = "",
                        @P_STR_WHSID = objOutboundInq.whs_id,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_SO_NUM = objOutboundInq.so_num,
                        @P_STR_ITM_NUM = objOutboundInq.itm_num,
                        @P_STR_ITM_COLOR = objOutboundInq.itm_color,
                        @P_STR_ITM_SIZE = objOutboundInq.itm_size,
                        @P_INT_CTN_QTY = objOutboundInq.ctn_qty,
                        @P_INT_DUE_QTY = objOutboundInq.due_qty,
                        @P_INT_ALOC_QTY = objOutboundInq.aloc_qty,
                        @P_INT_AVAIL_QTY = objOutboundInq.avail_qty,
                        @P_INT_BO_QTY = objOutboundInq.back_ordr_qty,
                        @P_INT_SOLINE = objOutboundInq.line_num,
                        @P_INT_DUELINE = objOutboundInq.due_line,
                        @balance_qty = objOutboundInq.balance_qty,
                    }, commandType: CommandType.StoredProcedure);
            }
            }

            catch (Exception ex)
            {
                string strException = ex.InnerException.ToString();
            }
        }
        public OutboundInq OutboundGETTEMPALOCDTL(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_TEMPALOCDTL_LIST";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @p_STR_SO_NUM = objOutboundInq.so_num
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstAlocDtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public void Update_aloc_num(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_Get_Aloc_Doc_Id";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @Aloc_id = "",
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public OutboundInq Move_to_aloc_hdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
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
                                cmd.CommandText = "Sp_MVC_OB_aloc_hdr_direc";
                                cmd.Parameters.Add("@opt", SqlDbType.NVarChar).Value = "A";
                                cmd.Parameters.Add("@cmp_id", SqlDbType.NVarChar).Value = objOutboundInq.cmp_id;
                                cmd.Parameters.Add("@aloc_doc_id", SqlDbType.NVarChar).Value = objOutboundInq.AlocdocId;
                                cmd.Parameters.Add("@Aloc_dt", SqlDbType.NVarChar).Value = (objOutboundInq.aloc_dt == null || objOutboundInq.aloc_dt == "") ? "" : objOutboundInq.aloc_dt;
                                cmd.Parameters.Add("@aloc_type", SqlDbType.NVarChar).Value = "Direct";
                                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = "S";
                                cmd.Parameters.Add("@price_tkt", SqlDbType.NVarChar).Value = (objOutboundInq.price_tkt == null || objOutboundInq.price_tkt == "") ? "" : objOutboundInq.price_tkt;
                                cmd.Parameters.Add("@ship_dt", SqlDbType.NVarChar).Value = (objOutboundInq.ShipDt == null || objOutboundInq.ShipDt == "") ? "" : objOutboundInq.ShipDt;
                                cmd.Parameters.Add("@cancel_dt", SqlDbType.NVarChar).Value = (objOutboundInq.CancelDt == null || objOutboundInq.CancelDt == "") ? "" : objOutboundInq.CancelDt;
                                cmd.Parameters.Add("@cust_id", SqlDbType.NVarChar).Value = (objOutboundInq.cust_id == null || objOutboundInq.cust_id == "") ? "" : objOutboundInq.cust_id;
                                cmd.Parameters.Add("@cust_ordr_num", SqlDbType.NVarChar).Value = (objOutboundInq.CustOrderNo == null || objOutboundInq.CustOrderNo == "") ? "" : objOutboundInq.CustOrderNo;
                                cmd.Parameters.Add("@cust_ordr_dt", SqlDbType.NVarChar).Value = (objOutboundInq.CustOrderdt == null || objOutboundInq.CustOrderdt == "") ? "" : objOutboundInq.CustOrderdt;
                                cmd.Parameters.Add("@cntr_num", SqlDbType.NVarChar).Value = (objOutboundInq.orderNo == null || objOutboundInq.orderNo == "") ? null : objOutboundInq.orderNo;
                                cmd.Parameters.Add("@whs_id", SqlDbType.NVarChar).Value = (objOutboundInq.whs_id == null || objOutboundInq.whs_id == "") ? null : objOutboundInq.whs_id;
                                cmd.Parameters.Add("@note", SqlDbType.NVarChar).Value = (objOutboundInq.note == null || objOutboundInq.note == "") ? null : objOutboundInq.note;
                                cmd.Parameters.Add("@ship_to_id", SqlDbType.NVarChar).Value = (objOutboundInq.ship_to_id == null || objOutboundInq.ship_to_id == "") ? "-" : objOutboundInq.ship_to_id;
                                cmd.Parameters.Add("@ship_to_name", SqlDbType.NVarChar).Value = (objOutboundInq.ship_to_name == null || objOutboundInq.ship_to_name == "") ? "-" : objOutboundInq.ship_to_name;
                                cmd.Parameters.Add("@process_id", SqlDbType.NVarChar).Value = (objOutboundInq.process_id == null || objOutboundInq.process_id == "") ? "" : objOutboundInq.process_id;
                                cmd.Parameters.Add("@ReturnValue", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                                objOutboundInq.ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                    return objOutboundInq;
                }
            }
        }
        public OutboundInq Move_to_aloc_dtl(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
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
                                cmd.CommandText = "Sp_MVC_OB_aloc_dtl_direc";
                                cmd.Parameters.Add("@opt", SqlDbType.NVarChar).Value = "A";
                                cmd.Parameters.Add("@cmp_id", SqlDbType.NVarChar).Value = objOutboundInq.cmp_id;
                                cmd.Parameters.Add("@aloc_doc_id", SqlDbType.NVarChar).Value = objOutboundInq.AlocdocId;
                                cmd.Parameters.Add("@Line_Num", SqlDbType.Int).Value = objOutboundInq.line_num;
                                cmd.Parameters.Add("@Itm_line", SqlDbType.NVarChar).Value = 1;
                                cmd.Parameters.Add("@itm_code", SqlDbType.NVarChar).Value = objOutboundInq.itm_code;
                                cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "S";
                                cmd.Parameters.Add("@So_num", SqlDbType.NVarChar).Value = (objOutboundInq.so_num == null || objOutboundInq.so_num == "") ? "" : objOutboundInq.so_num;
                                cmd.Parameters.Add("@Cust_ID", SqlDbType.NVarChar).Value = (objOutboundInq.cust_id == null || objOutboundInq.cust_id == "") ? "" : objOutboundInq.cust_id;
                                cmd.Parameters.Add("@so_itm_code", SqlDbType.NVarChar).Value = (objOutboundInq.so_itm_code == null || objOutboundInq.so_itm_code == "") ? "" : objOutboundInq.so_itm_code;
                                cmd.Parameters.Add("@so_itm_num", SqlDbType.NVarChar).Value = (objOutboundInq.itm_num == null || objOutboundInq.itm_num == "") ? "" : objOutboundInq.itm_num;
                                cmd.Parameters.Add("@so_itm_color", SqlDbType.NVarChar).Value = (objOutboundInq.itm_color == null || objOutboundInq.itm_color == "") ? "" : objOutboundInq.itm_color;
                                cmd.Parameters.Add("@so_itm_size", SqlDbType.NVarChar).Value = (objOutboundInq.itm_size == null || objOutboundInq.itm_size == "") ? null : objOutboundInq.itm_size;
                                cmd.Parameters.Add("@so_dtl_line", SqlDbType.NVarChar).Value = objOutboundInq.dtl_line;
                                cmd.Parameters.Add("@so_due_line", SqlDbType.NVarChar).Value = objOutboundInq.due_line;
                                cmd.Parameters.Add("@ship_to", SqlDbType.NVarChar).Value = (objOutboundInq.ship_to_id == null || objOutboundInq.ship_to_id == "") ? null : objOutboundInq.ship_to_id;
                                cmd.Parameters.Add("@Aloc_qty", SqlDbType.NVarChar).Value = objOutboundInq.aloc_qty;
                                cmd.Parameters.Add("@due_qty", SqlDbType.NVarChar).Value = objOutboundInq.due_qty;
                                cmd.Parameters.Add("@aloc_uom", SqlDbType.NVarChar).Value = (objOutboundInq.qty_uom == null || objOutboundInq.qty_uom == "") ? "" : objOutboundInq.qty_uom;
                                cmd.Parameters.Add("@Note", SqlDbType.NVarChar).Value = (objOutboundInq.note == null || objOutboundInq.note == "") ? "" : objOutboundInq.note;
                                cmd.Parameters.Add("@process_id", SqlDbType.NVarChar).Value = (objOutboundInq.process_id == null || objOutboundInq.process_id == "") ? "" : objOutboundInq.process_id;
                                cmd.Parameters.Add("@ReturnValue", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                                objOutboundInq.ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                    return objOutboundInq;
                }
            }
        }
        public OutboundInq Move_to_aloc_ctn(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
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
                                cmd.CommandText = "SP_MVC_OB_Iv_Aloc_Ctn_tpw_direc";
                                cmd.Parameters.Add("@cmp_id", SqlDbType.NVarChar).Value = objOutboundInq.cmp_id.Trim();
                                cmd.Parameters.Add("@aloc_doc_id", SqlDbType.VarChar, 10).Value = objOutboundInq.AlocdocId.Trim();
                                cmd.Parameters.Add("@line_num", SqlDbType.Int).Value = objOutboundInq.line_num;
                                cmd.Parameters.Add("@itm_line", SqlDbType.Int).Value = "1";
                                cmd.Parameters.Add("@ctn_line", SqlDbType.Int).Value = objOutboundInq.ctn_line;
                                cmd.Parameters.Add("@itm_code", SqlDbType.VarChar, 10).Value = objOutboundInq.itm_code.Trim();
                                cmd.Parameters.Add("@itm_num", SqlDbType.VarChar, 20).Value = (objOutboundInq.itm_num.Trim() == null || objOutboundInq.itm_num.Trim() == "") ? "" : objOutboundInq.itm_num.Trim();
                                cmd.Parameters.Add("@itm_color", SqlDbType.VarChar, 20).Value = (objOutboundInq.itm_color.Trim() == null || objOutboundInq.itm_color == "") ? "" : objOutboundInq.itm_color.Trim();
                                cmd.Parameters.Add("@itm_size", SqlDbType.VarChar, 20).Value = (objOutboundInq.itm_size.Trim() == null || objOutboundInq.itm_size.Trim() == "") ? "" : objOutboundInq.itm_size.Trim();
                                cmd.Parameters.Add("@so_num", SqlDbType.VarChar, 10).Value = (objOutboundInq.so_num.Trim() == null || objOutboundInq.so_num.Trim() == "") ? "" : objOutboundInq.so_num.Trim();
                                cmd.Parameters.Add("@so_line_num", SqlDbType.Int).Value = objOutboundInq.Soline;
                                cmd.Parameters.Add("@so_due_line", SqlDbType.Int).Value = objOutboundInq.due_line;
                                cmd.Parameters.Add("@so_itm_line", SqlDbType.Int).Value = "1";
                                cmd.Parameters.Add("@lot_id", SqlDbType.NVarChar, 10).Value = objOutboundInq.lot_id.Trim();
                                cmd.Parameters.Add("@po_num", SqlDbType.NVarChar, 20).Value = objOutboundInq.po_num.Trim();
                                cmd.Parameters.Add("@rcvd_dt", SqlDbType.SmallDateTime).Value = (objOutboundInq.Recvddt == null || objOutboundInq.Recvddt == "") ? null : objOutboundInq.Recvddt;
                                cmd.Parameters.Add("@whs_id", SqlDbType.NVarChar, 10).Value = objOutboundInq.whs_id.Trim();
                                cmd.Parameters.Add("@loc_id", SqlDbType.NVarChar, 10).Value = objOutboundInq.loc_id.Trim();
                                cmd.Parameters.Add("@palet_id", SqlDbType.NVarChar, 10).Value = (objOutboundInq.Palet_id.Trim() == null || objOutboundInq.Palet_id.Trim() == "") ? "" : objOutboundInq.Palet_id.Trim();
                                cmd.Parameters.Add("@pkg_type", SqlDbType.NVarChar, 10).Value = "CTN";
                                cmd.Parameters.Add("@ctn_qty", SqlDbType.Decimal, 9).Value = objOutboundInq.ctn_qty;
                                cmd.Parameters.Add("@itm_qty", SqlDbType.Decimal, 9).Value = objOutboundInq.ctn_qty;
                                cmd.Parameters.Add("@ctn_cnt", SqlDbType.Decimal, 9).Value = objOutboundInq.ctn_cnt;
                                cmd.Parameters.Add("@line_qty", SqlDbType.Decimal, 9).Value = objOutboundInq.lineqty;
                                cmd.Parameters.Add("@qty_uom", SqlDbType.NVarChar, 10).Value = "EA";
                                cmd.Parameters.Add("@pick_qty", SqlDbType.Decimal, 9).Value = "0";
                                cmd.Parameters.Add("@pick_uom", SqlDbType.NVarChar, 50).Value = "EA";
                                cmd.Parameters.Add("@loc_bal_qty", SqlDbType.Decimal, 9).Value = objOutboundInq.Bal;
                                cmd.Parameters.Add("@process_id", SqlDbType.VarChar).Value = (objOutboundInq.process_id == null || objOutboundInq.process_id == "") ? "" : objOutboundInq.process_id;
                                cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                                //objOutboundInq.ReturnValue = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                    return objOutboundInq;
                }
            }

        }
        public void Moveto_TrnHdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_MVC_OB_Update_Aloc_qty_to_trn_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @OpId = "D",
                        @cmp_id = objOutboundInq.cmp_id,
                        @itm_code = objOutboundInq.itm_code,
                        @whs_id = objOutboundInq.whs_id,
                        @lot_id = objOutboundInq.lot_id,
                        @rcvd_dt = objOutboundInq.rcvd_dt,
                        @loc_id = objOutboundInq.loc_id,
                        @action_qty = objOutboundInq.avail_qty,
                        @process_id = objOutboundInq.process_id,
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public OutboundInq Update_so_dtl(OutboundInq objOutboundInq)
        {

            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
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
                                cmd.CommandText = "SP_MVC_ALOC_UPD_So_dtl_and_due";
                                cmd.Parameters.Add("@cmp_id", SqlDbType.NVarChar).Value = objOutboundInq.cmp_id;
                                cmd.Parameters.Add("@so_num", SqlDbType.NVarChar).Value = objOutboundInq.Sonum;
                                cmd.Parameters.Add("@itm_code", SqlDbType.NVarChar).Value = objOutboundInq.itm_code;
                                cmd.Parameters.Add("@line_num", SqlDbType.NVarChar).Value = (objOutboundInq.line_num);
                                cmd.Parameters.Add("@due_line", SqlDbType.NVarChar).Value = objOutboundInq.due_line;
                                cmd.Parameters.Add("@Aloc_qty", SqlDbType.NVarChar).Value = objOutboundInq.aloc_qty;
                                cmd.Parameters.Add("@back_ordr_qty", SqlDbType.NVarChar).Value = objOutboundInq.back_ordr_qty;
                                cmd.Parameters.Add("@Step", SqlDbType.NVarChar).Value = objOutboundInq.step;
                                cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = objOutboundInq.status;
                                cmd.Parameters.Add("@ReturnValue", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                                objOutboundInq.ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                            }
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                    return objOutboundInq;
                }
            }


        }
        public void Move_To_Grd_Bad_Itm(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_INSERT_TEMP_BAD_ITM_ALOC_SUMMARY";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_INT_LINENUM = objOutboundInq.aloc_line,
                        @P_STR_LOCID = "",
                        @P_STR_WHSID = objOutboundInq.whs_id,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_SO_NUM = objOutboundInq.so_num,
                        @P_STR_ITM_NUM = objOutboundInq.itm_num,
                        @P_STR_ITM_COLOR = objOutboundInq.itm_color,
                        @P_STR_ITM_SIZE = objOutboundInq.itm_size,
                        @P_INT_CTN_QTY = 0,
                        @P_INT_DUE_QTY = objOutboundInq.due_qty,
                        @P_INT_ALOC_QTY = objOutboundInq.aloc_qty,
                        @P_INT_AVAIL_QTY = objOutboundInq.avail_qty,
                        @P_INT_BO_QTY = objOutboundInq.back_ordr_qty,
                        @P_INT_SOLINE = objOutboundInq.line_num,
                        @P_INT_DUELINE = objOutboundInq.due_line,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Update_Tbl_iv_itm_trn_in(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_MV_OB_itm_trn_in_direc";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @pkg_id = objOutboundInq.pkg_id,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @kit_type = objOutboundInq.kit_type,
                        @kit_id = objOutboundInq.kit_id,
                        @kit_qty = objOutboundInq.kit_qty,
                        @lot_id = objOutboundInq.lot_id,
                        @palet_id = objOutboundInq.palet_id,
                        @cont_id = objOutboundInq.cont_id,
                        @tran_type = "1RCVD",
                        @rcvd_dt = objOutboundInq.rcvd_dt,
                        @status = "AVAIL",
                        @bill_status = objOutboundInq.bill_status,
                        @io_rate_id = objOutboundInq.io_rate_id,
                        @st_rate_id = objOutboundInq.st_rate_id,
                        @doc_id = objOutboundInq.doc_id,
                        @doc_date = (objOutboundInq.doc_date == null || objOutboundInq.doc_date == "") ? "" : objOutboundInq.doc_date,
                        @doc_notes = objOutboundInq.doc_notes,
                        @fmto_name = objOutboundInq.fmto_name,
                        @group_id = objOutboundInq.group_id,
                        @whs_id = objOutboundInq.whs_id,
                        @loc_id = objOutboundInq.loc_id,
                        @pkg_type = objOutboundInq.pkg_type,
                        @pkg_qty = objOutboundInq.pkg_qty,
                        @itm_qty = objOutboundInq.itm_qty,
                        @lbl_id = objOutboundInq.lbl_id,
                        @grn_id = objOutboundInq.grn_id,
                        @po_num = objOutboundInq.po_num,
                        @process_id = objOutboundInq.process_id,
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.depth,
                        @cube = objOutboundInq.cube,
                        @wgt = objOutboundInq.wgt,
                        @ib_doc_id = objOutboundInq.ib_doc_id
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public void Change_SOHdr_Status_atAdd(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_MVC_OB_Aloc_Upd_So_Hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @Cmp_id = objOutboundInq.cmp_id,
                        @So_num = objOutboundInq.so_num,
                        @Step = objOutboundInq.step,
                        @Status = objOutboundInq.status,
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public OutboundInq Get_Newqty(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_NEWQTY";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_COMPID = objOutboundInq.cmp_id,
                        @P_STR_ALOCDOCID = objOutboundInq.AlocdocId,
                        @P_INT_LINENUM = objOutboundInq.line_num,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_PALET_ID = objOutboundInq.Palet_id,
                        @P_STR_LOCID = objOutboundInq.loc_id,
                        @P_STR_CTN_QTY = 1,
                        @P_STR_CTN_CNT = objOutboundInq.ctn_cnt,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListCheckExistStyle = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq update_alocctn(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "Sp_MVC_OB_Aloc_Mod_Upd_iv_aloc_ctn_direc";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @aloc_doc_id = objOutboundInq.AlocdocId,
                        @line_num = objOutboundInq.line_num,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @palet_id = objOutboundInq.Palet_id,
                        @loc_id = objOutboundInq.loc_id,
                        @ctn_qty = objOutboundInq.ctn_cnt,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListCheckExistStyle = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetSRGridRowCount(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_gridRowcount";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.so_num,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListCheckExistStyle = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq Get_PkgID(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_pkg_id";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @doc_pkg_id = "",

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.Lstpkgid = ListIRFP.ToList();
                    objOutboundInq.doc_pkg_id = objOutboundInq.Lstpkgid[0].doc_pkg_id;
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq Get_itmtrninList(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_ITMTRN_IN_LIST";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_COMPID = objOutboundInq.cmp_id,
                        @P_STR_ITMCODE = objOutboundInq.itm_code,
                        @P_INT_WHSID = objOutboundInq.whs_id,
                        @P_STR_LOCID = objOutboundInq.loc_id,
                        @P_STR_PALETID = objOutboundInq.Palet_id,
                        @P_STR_PKGQTY = objOutboundInq.pkg_qty,
                        @P_STR_PONUM = objOutboundInq.po_num,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.ListCheckExistStyle = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void Aloc_SpltCtn_Upd_Itm_Trn_in_direc(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_Aloc_SpltCtn_Upd_Itm_Trn_in_direc";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @pkg_id = objOutboundInq.pkg_id,
                        @Itm_code = objOutboundInq.itm_code,
                        @Itm_Num = objOutboundInq.itm_num,
                        @Itm_color = objOutboundInq.itm_color,
                        @Itm_size = objOutboundInq.itm_size,
                        @Whs_ID = objOutboundInq.whs_id,
                        @loc_ID = objOutboundInq.loc_id,
                        @palet_id = objOutboundInq.Palet_id,
                        @doc_id = objOutboundInq.doc_id,
                        @po_num = objOutboundInq.po_num,
                        @Pkg_qty = objOutboundInq.pkg_qty,
                        @New_Pkg_qty = objOutboundInq.new_pkg_qty,
                        @doc_date = objOutboundInq.doc_date,
                        @p_str_so_num = objOutboundInq.lbl_id,
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public void Aloc_Upd_data_to_itm_trn_in_direc(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_mvc_Aloc_Upd_data_to_itm_trn_in_direc";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @whs_id = objOutboundInq.whs_id,
                        @loc_id = objOutboundInq.loc_id,
                        @palet_id = objOutboundInq.Palet_id,
                        @pkg_qty = objOutboundInq.pkg_qty,
                        @doc_id = objOutboundInq.doc_id,
                        @doc_date = objOutboundInq.doc_date,
                        @po_num = objOutboundInq.po_num,
                        @aloc_by = objOutboundInq.aloc_by,
                        @process_id = objOutboundInq.process_id,
                        @row_count = objOutboundInq.RowCount,
                        @p_str_so_num = objOutboundInq.lbl_id,
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public void Aloc_into_Itm_Trn_hst_by_itm_del_direc(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_Aloc_into_Itm_Trn_hst_by_itm_del_direc";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @doc_id = objOutboundInq.doc_id,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public void Aloc_into_Itm_Trn_hst_by_itm_direc(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_Aloc_into_Itm_Trn_hst_by_itm_direc";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @doc_id = objOutboundInq.doc_id,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @ship_to_name = objOutboundInq.shipname,
                        @ordr_num = objOutboundInq.orderNo,
                        @doc_date = objOutboundInq.doc_date,
                    }, commandType: CommandType.StoredProcedure);
            }

        }
        public OutboundInq Get_AlocSaveRpt(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_ALOC_SAVE_RPT";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_ALOCDOC_ID = objOutboundInq.AlocdocId,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();

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
        public OutboundInq GetDftWhs(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_DFTWHS";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListPickdtl = ListInq.ToList();

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
        public OutboundInq GetalocdtlList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "proc_get_mvcweb_Aloc_View";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objOutboundInq.cmp_id,
                            @p_str_aloc_id = objOutboundInq.aloc_doc_id,
                            @p_str_so_num = objOutboundInq.so_num,
                            @p_str_aloc_by = objOutboundInq.aloc_by
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.lstShipAlocdtl = ListInq.ToList();

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
        public OutboundInq GetalocctnList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_ALOCCTN_LIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_ALOCDOC_ID = objOutboundInq.aloc_doc_id,
                            @P_INT_LINE_NUM = objOutboundInq.line_num,
                            @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocDtl = ListInq.ToList();

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
        public OutboundInq GetalocdueList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_SODUE_LIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_SO_NUM = objOutboundInq.so_num,
                            @P_INT_DTL_LINE = objOutboundInq.dtl_line,
                            @P_INT_DUE_LINE = objOutboundInq.due_line,
                            @P_STR_ITM_NUM = objOutboundInq.itm_num,
                            @P_STR_ITM_COLOR = objOutboundInq.itm_color,
                            @P_STR_ITM_SIZE = objOutboundInq.itm_size,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAvailqty = ListInq.ToList();

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

        public OutboundInq Del_Alloc_Upd_SO(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_ALOC_MODEL_DIREC";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @cmpid = objOutboundInq.cmp_id,
                            @aloc_docid = objOutboundInq.AlocdocId,
                            @so_num = objOutboundInq.so_num,
                            @ReturnValue = ParameterDirection.Output,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAvailqty = ListInq.ToList();

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

        public OutboundInq Del_data_to_itm_trn_in(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_ALOC_DEL_ITM_TRN_HST_DIREC";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @cmp_id = objOutboundInq.cmp_id,
                            @doc_id = objOutboundInq.AlocdocId,
                            @process_id = "",
                            @ReturnValue = ParameterDirection.Output,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAvailqty = ListInq.ToList();

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

        public bool delAlocAndUpdtLoc(string p_str_cmp_id, string p_str_aloc_doc_id, string p_str_sr_num, string p_str_new_loc_id)
        {
            bool bln_status = false;
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    using (IDbTransaction tran = connection.BeginTransaction())
                    {

                        const string storedProcedure2 = "spOBAlocDelete";
                        IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = p_str_cmp_id,
                            @pstrAlocDocId = p_str_aloc_doc_id,
                            @pstrSrNum = p_str_sr_num,
                            @pstrLocId = p_str_new_loc_id,
                            @pintReturnValue = ParameterDirection.Output,
                        },
                        commandType: CommandType.StoredProcedure,transaction: tran);

                        bln_status = true;
                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                bln_status = false;
                throw ex;
            }
            finally
            {

            }
            return bln_status;
        }


        public OutboundInq Add_To_Trn_Hdr(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_UPDATE_ALOC_QTY_TO_TRN_HDR";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @opId = "A",
                            @cmp_id = objOutboundInq.cmp_id,
                            @itm_code = objOutboundInq.itm_code,
                            @whs_id = objOutboundInq.whs_id,
                            @lot_id = objOutboundInq.lot_id,
                            @rcvd_dt = objOutboundInq.rcvd_dt,
                            @loc_id = objOutboundInq.loc_id,
                            @action_qty = objOutboundInq.aloc_qty,
                            @process_id = objOutboundInq.process_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAvailqty = ListInq.ToList();

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

        public OutboundInq addInvTransHdr(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "spAddInvTransHdr";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                          
                            @cmp_id = objOutboundInq.cmp_id,
                            @itm_code = objOutboundInq.itm_code,
                            @whs_id = objOutboundInq.whs_id,
                            @lot_id = objOutboundInq.lot_id,
                            @rcvd_dt = objOutboundInq.rcvd_dt,
                            @loc_id = objOutboundInq.loc_id,
                            @palet_id = objOutboundInq.palet_id,
                            @action_qty = objOutboundInq.aloc_qty,
                            @process_id = objOutboundInq.process_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAvailqty = ListInq.ToList();

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

        public OutboundInq BackOrderRpt(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_BACKORDER";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_SO_NUM = objOutboundInq.so_num,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocDtl = ListInq.ToList();

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
        public OutboundInq Get_AlocBackOrderRptList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_ALOC_BACKORDER_RPT";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_SONUM = objOutboundInq.so_num,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();

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
        public OutboundInq Get_AlocBackOrderCount(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_ALOC_BACKORDER_COUNT";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_SONUM = objOutboundInq.so_num,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundInqpickstyleRpt = ListInq.ToList();

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
        //CR_3PL_MVC_OB_2018_0220_001
        public OutboundInq GetaloceditmanualList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_ALOC_MANUAL_DTL";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objOutboundInq.cmp_id,
                            @P_STR_ALOCDOCID = objOutboundInq.aloc_doc_id,
                            @P_STR_ITMNUM = objOutboundInq.itm_num,
                            @P_STR_ITMCOLOR = objOutboundInq.itm_color,
                            @P_STR_ITMSIZE = objOutboundInq.itm_size,
                            @P_STR_WHSID = objOutboundInq.whs_id,
                            @P_STR_SORTSTMT = objOutboundInq.aloc_sort_stmt,
                            @P_STR_ALOCBY = objOutboundInq.aloc_by,
                            @P_STR_PO_NUM = objOutboundInq.po_num,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstManualAloc = ListInq.ToList();

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
        public OutboundInq GetSelectedgridValue(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_ALOC_GRID_SELECTED";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_ITMLINE = objOutboundInq.line_num,
                            @P_STR_SONUM = objOutboundInq.so_num,
                            @P_STR_ITMCODE = objOutboundInq.itm_code,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocDtl = ListInq.ToList();

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
        public OutboundInq GetALOCEditList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "PROC_GET_MVC_WEB_ALOC_EDIT_LIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_ALOCDOC_ID = objOutboundInq.AlocdocId,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocDtl = ListInq.ToList();

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
        public OutboundInq SP_Aloc_del_Itm_trn_hst(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "Sp_MVC_OB_Aloc_Del_Itm_Trn_hst_by_Itm_direc";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @cmp_id = objOutboundInq.cmp_id,
                            @doc_id = objOutboundInq.AlocdocId,
                            @itm_code = objOutboundInq.itm_code,
                            @itm_num = objOutboundInq.itm_num,
                            @itm_color = objOutboundInq.itm_color,
                            @itm_size = objOutboundInq.itm_size,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocDtl = ListInq.ToList();

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
        public OutboundInq Sp_Aloc_Mod_Daloc_iv_itm_trn_in(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "Sp_MVC_OB_Aloc_Mod_Daloc_iv_itm_trn_in_direc";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @cmp_id = objOutboundInq.cmp_id,
                            @doc_id = objOutboundInq.AlocdocId,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstAlocDtl = ListInq.ToList();

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

        public OutboundInq GetAddaloceditmanualList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_ALOC_ADD_MANUAL_DTL";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objOutboundInq.cmp_id,
                            @P_STR_ALOCDOCID = objOutboundInq.aloc_doc_id,
                            @P_STR_ITMNUM = objOutboundInq.itm_num,
                            @P_STR_ITMCOLOR = objOutboundInq.itm_color,
                            @P_STR_ITMSIZE = objOutboundInq.itm_size,
                            @P_STR_WHSID = objOutboundInq.whs_id,
                            @P_STR_SORTSTMT = objOutboundInq.aloc_sort_stmt,
                            @P_STR_ALOCBY = objOutboundInq.aloc_by,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstManualAloc = ListInq.ToList();

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
        public OutboundInq GetSelectedgridValueList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_ALOC_ADD_MANUAL_DTL_LIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objOutboundInq.cmp_id,
                            @P_STR_ITMCODE = objOutboundInq.itm_code,
                            @P_STR_SONUM = objOutboundInq.so_num,
                            @P_STR_ALOC_BY = objOutboundInq.aloc_by,
                            @P_STR_SO_LINE = objOutboundInq.Soline
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstManualAloc = ListInq.ToList();
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

        public OutboundInq UpdateTemp_Alloc_Summary(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_UPDATE_TEMPALOCSUMMARY_LIST";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_INT_LINE = objOutboundInq.Line,
                        @P_STR_SO_NUM = objOutboundInq.so_num,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_INT_AVAIL_QTY = objOutboundInq.avail_qty,
                        @P_INT_ALOC_QTY = objOutboundInq.aloc_qty,
                        @P_INT_BO_QTY = objOutboundInq.back_ordr_qty,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstAlocSummary = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq Update_Tbl_Temp_So_Auto_Aloc(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_UPDATE_TBL_TEMP_SO_AUTO_ALOC";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_MP_ID = objOutboundInq.cmp_id,
                        @P_STR_SO_NUM = objOutboundInq.so_num,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_INT_BO_QTY = objOutboundInq.back_ordr_qty,
                        @P_INT_ALOC_QTY = objOutboundInq.aloc_qty,
                        @P_INT_DUE_QTY = objOutboundInq.due_qty,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstAlocSummary = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq DeleteTemp_Alloc_Summary(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_DELETE_Temp_Alloc_detail";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @P_INT_ALCLN = objOutboundInq.aloc_line,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_SO_NUM = objOutboundInq.so_num,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstAlocSummary = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //CR_MVC_3PL_20180315-001 Added By NIthya
        public void DelSo_dtl_Due_Table(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_OB_DEL_SO_DTL_AND_DUE";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Update_To_proc_save_so_hdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_OB_UPDATE_SO_HDR";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @status = "O",
                        @step = "ENTRY",
                        @price_tkt = objOutboundInq.pricetkt,
                        @so_dt = objOutboundInq.So_dt,
                        @ordr_type = objOutboundInq.OrdType,
                        @cust_id = objOutboundInq.CustName,
                        @cust_name = objOutboundInq.cust_id,
                        @ordr_num = objOutboundInq.ordr_num,
                        @cust_ordr_num = objOutboundInq.CustPO,
                        @cust_ordr_dt = objOutboundInq.CustOrderdt,
                        @cntr_num = "111",
                        @quote_num = objOutboundInq.Batchid,
                        @in_sale_id = objOutboundInq.AuthId,
                        @out_sale_id = "N",
                        @dept_id = objOutboundInq.dept_id,
                        @store_id = objOutboundInq.store_id,
                        @fob = objOutboundInq.fob,
                        @shipto_id = objOutboundInq.shipto_id,
                        @shipvia_id = objOutboundInq.shipvia_id,
                        @terms_id = 1,
                        @freight_id = objOutboundInq.freight_id,
                        @ordr_lines = objOutboundInq.itm_line,
                        @total_qty = objOutboundInq.total_qty,
                        @ordr_cost = objOutboundInq.ordr_cost,
                        @tax_pcnt = 0,
                        @sh_chg = objOutboundInq.shipchrg,
                        @ship_dt = objOutboundInq.ShipDt,
                        @cancel_dt = objOutboundInq.CancelDt,
                        @note = objOutboundInq.note,
                        @process_id = "DI-ADD",
                        @temp_check = "N",
                        @temp_user = objOutboundInq.user_id,
                        @pick_no = objOutboundInq.pick_no,
                        @ref_no = objOutboundInq.ref_no,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Update_To_proc_save_so_addr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_OB_UPDATE_SO_ADDR";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @soldto_id = objOutboundInq.shipto_id,
                        @sl_attn = objOutboundInq.Attn,
                        @sl_mail_name = objOutboundInq.Mailname,
                        @sl_addr_line1 = objOutboundInq.Addr1,
                        @sl_addr_line2 = objOutboundInq.Addr2,
                        @sl_city = objOutboundInq.ShipToCity,
                        @sl_state_id = objOutboundInq.ShipToState,
                        @sl_post_code = objOutboundInq.ShipToZipCode,
                        @sl_cntry_id = objOutboundInq.ShipToCountry,
                        @shipto_id = objOutboundInq.shipto_id,
                        @st_attn = objOutboundInq.Attn,
                        @st_mail_name = objOutboundInq.Mailname,
                        @st_addr_line1 = objOutboundInq.Addr1,
                        @st_addr_line2 = objOutboundInq.Addr2,
                        @st_city = objOutboundInq.ShipToCity,
                        @st_state_id = objOutboundInq.ShipToState,
                        @st_post_code = objOutboundInq.ShipToZipCode,
                        @st_cntry_id = objOutboundInq.ShipToCountry,
                        @dc_id = objOutboundInq.dc_id,
                        @billto_id = objOutboundInq.shipto_id,
                        @bt_attn = objOutboundInq.Attn,
                        @bt_mail_name = objOutboundInq.Mailname,
                        @bt_addr_line1 = objOutboundInq.Addr1,
                        @bt_addr_line2 = objOutboundInq.Addr2,
                        @bt_city = objOutboundInq.ShipToCity,
                        @bt_state_id = objOutboundInq.ShipToState,
                        @bt_post_code = objOutboundInq.ShipToZipCode,
                        @bt_cntry_id = objOutboundInq.ShipToCountry,
                        @process_id = "DI-ADD",
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public OutboundInq GetItemCode(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_ITEMCODE";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objOutboundInq.cmp_id,
                        @P_STR_ITM_NUM = objOutboundInq.itm_num,
                        @P_STR_ITM_COLOR = objOutboundInq.itm_color,
                        @P_STR_ITM_SIZE = objOutboundInq.itm_size,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.LstAlocSummary = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //CR - 3PL_MVC-IB-20180405 Added By Nithya
        public void UpdateAvailQtyTrnHdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_GET_UPDATE_AVAIL_QTY_TO_TRN_HDR";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @P_STR_CMP_ID = objOutboundInq.cmp_id,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_STR_WHS_ID = objOutboundInq.whs_id,
                        @P_STR_LOT_ID = objOutboundInq.lot_id,
                        @P_STR_RCVD_DT = objOutboundInq.Recvddt,
                        @P_STR_LOC_ID = objOutboundInq.loc_id,
                        @P_STR_ACTION_QTY = objOutboundInq.itm_qty,
                        @PROCESS_ID = "Modify",
                        @PALET_ID = objOutboundInq.palet_id,
                        @PKG_TYPE = objOutboundInq.pkg_type,
                        @ITM_STYLE = objOutboundInq.itm_num,
                        @ITM_COLOR = objOutboundInq.itm_color,
                        @ITMSIZE = objOutboundInq.itm_size,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public OutboundInq GetPickQty(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_GET_PICKQTY";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_ALOC_DOC_ID = objOutboundInq.bol_num,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundPickQty = ListInq.ToList();
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

        //END
        //CR-20180522-001 Added By Nithya   
        public OutboundInq CheckOpenOrderExist(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_CHECKOPEN_ORDER_EXIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pStrBatchId = objOutboundInq.quote_num,
                            @pStrSoNumFrom = objOutboundInq.so_numFm,
                            @pStrSoNumTo = objOutboundInq.so_numTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstStockverify = ListInq.ToList();
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
        public OutboundInq ShowStockVerifyRpt(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_STOCK_VERIFY";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pStrBatchId = objOutboundInq.quote_num,
                            @pStrSoNumFrom = objOutboundInq.so_numFm,
                            @pStrSoNumTo = objOutboundInq.so_numTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstStkverifyList = ListInq.ToList();
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
        public OutboundInq GetItemName(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_GET_ITEMNAME";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pstrItmCode = objOutboundInq.itm_code,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstOutboundPickQty = ListInq.ToList();
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
        public OutboundInq IsOpenAlocationExists(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_OPEN_ALLOCATION_EXIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pStrBatchId = objOutboundInq.quote_num,
                            @pStrSoNumFrom = objOutboundInq.so_numFm,
                            @pStrSoNumTo = objOutboundInq.so_numTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstStockverify = ListInq.ToList();
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
        public OutboundInq Validate_LotId(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_LOT_ID_EXIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pStrlotId = objOutboundInq.lot_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstLotId = ListInq.ToList();
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
        public OutboundInq Validate_IbdocId(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_IBDOC_ID_EXIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pStrdocId = objOutboundInq.ib_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstIbdocId = ListInq.ToList();
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
        public OutboundInq ExistLoc(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_LOC_ID_EXIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pStrwhsId = objOutboundInq.whs_id,
                            @pStrLocId = objOutboundInq.loc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstLocId = ListInq.ToList();
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
        public void Insert_loc_hdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_Add_ma_loc_hdr_new";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @whs_id = objOutboundInq.whs_id,
                        @loc_id = objOutboundInq.loc_id,
                        @cube = objOutboundInq.cube,
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.depth,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Save_Lot_hdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_IV_ADD_LOT_HDR";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                        @lot_id = objOutboundInq.lot_id,
                        @status = "AVAIL",
                        @post_status = "POST",
                        @vend_id = "Mass-Conv",
                        @vend_name = "-",
                        @vend_ord = "-",
                        @po_num = objOutboundInq.refno,
                        @rcvd_via = objOutboundInq.rcvd_notes,
                        @seal_num = "-",
                        @pkg_type_cnt = 1,
                        @rcvd_by = objOutboundInq.user_id,
                        @rcvd_dt = objOutboundInq.doc_date,
                        @palet_cnt = 1,
                        @process_id = "MassConv",
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Save_Lot_dtl(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_IV_ADD_LOT_DTL";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                        @lot_id = objOutboundInq.lot_id,
                        @palet_id = objOutboundInq.palet_id,
                        @dtl_line = objOutboundInq.dtl_line,
                        @palet_dt = objOutboundInq.doc_date,
                        @status = "AVAIL",
                        @tran_status = "CHNG",
                        @bill_status = "OPEN",
                        @cont_id = "-",
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @itm_name = objOutboundInq.itm_name,
                        @kid_id = objOutboundInq.itm_code,
                        @rate_id = objOutboundInq.io_rate_id,
                        @rate = "1",
                        @whs_id = objOutboundInq.whs_id,
                        @loc_id = objOutboundInq.loc_id,
                        @pkg_type = "CTN",
                        @tot_pkg = 1,
                        @bal_pkg = 1,
                        @pkg_qty = objOutboundInq.ppk,
                        @tot_qty = objOutboundInq.OrderQty,
                        @bal_qty = objOutboundInq.OrderQty,
                        @po_num = "-",
                        @po_line_num = 1,
                        @po_due_line = 1,
                        @notes = objOutboundInq.doc_notes,
                        @process_id = "MassConv",
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.depth,
                        @wgt = objOutboundInq.wgt,
                        @cube = objOutboundInq.cube,
                        @st_rate_id = objOutboundInq.cube,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Save_Lot_ctn(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_IV_ADD_LOT_CTN";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                        @lot_id = objOutboundInq.lot_id,
                        @palet_id = objOutboundInq.palet_id,
                        @dtl_line = objOutboundInq.dtl_line,
                        @itm_line = objOutboundInq.CtnLn,
                        @kit_id = objOutboundInq.itm_code,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @itm_qty = objOutboundInq.ppk,
                        @ctn_qty = objOutboundInq.ppk,
                        @tot_ctn = 1,
                        @process_id = "MassConv",
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.depth,
                        @wgt = objOutboundInq.wgt,
                        @cube = objOutboundInq.cube,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Save_iv_Itm_trn_in(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_IV_ADD_IV_ITM_TRN_IN";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @pkg_id = objOutboundInq.pkg_id,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @kit_Type = "N",
                        @kit_id = objOutboundInq.itm_code,
                        @kit_qty = objOutboundInq.ppk,
                        @lot_id = objOutboundInq.lot_id,
                        @palet_id = objOutboundInq.palet_id,
                        @cont_id = "-",
                        @tran_type = "1RCVD",
                        @rcvd_dt = objOutboundInq.doc_date,
                        @status = "AVAIL",
                        @bill_status = "OPEN",
                        @io_rate_id = objOutboundInq.inout_rate,
                        @st_rate_id = objOutboundInq.st_rate_id,
                        @doc_id = objOutboundInq.lot_id,
                        @doc_date = objOutboundInq.doc_date,
                        @doc_notes = objOutboundInq.doc_notes,
                        @fmto_name = "-",
                        @group_id = objOutboundInq.cmp_id,
                        @whs_id = objOutboundInq.whs_id,
                        @loc_id = objOutboundInq.loc_id,
                        @pkg_type = "CTN",
                        @pkg_qty = objOutboundInq.ppk,
                        @itm_qty = objOutboundInq.ppk,
                        //@lbl_id = "1111",
                        @lbl_id = objOutboundInq.lbl_id,
                        @grn_id = objOutboundInq.refno,
                        @po_num = "-",
                        @process_id = "MassConv",
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.depth,
                        @wgt = objOutboundInq.wgt,
                        @cube = objOutboundInq.cube,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Save_iv_Itm_trn_hst(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_IV_ITM_TRN_HST";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @pkg_id = objOutboundInq.pkg_id,
                        @tran_type = "1RCVD",
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @kit_type = "N",
                        @kit_id = objOutboundInq.itm_code,
                        @kit_qty = objOutboundInq.ppk,
                        @lot_id = objOutboundInq.lot_id,
                        @palet_id = objOutboundInq.palet_id,
                        @cont_id = "-",
                        @status = "AVAIL",
                        @bill_status = "OPEN",
                        @io_rate_id = objOutboundInq.inout_rate,
                        @st_rate_id = objOutboundInq.strg_rate,
                        @doc_id = objOutboundInq.lot_id,
                        @doc_date = objOutboundInq.doc_date,
                        @doc_notes = objOutboundInq.doc_notes,
                        @fmto_name = "-",
                        @group_id = objOutboundInq.cmp_id,
                        @whs_id = objOutboundInq.whs_id,
                        @loc_id = objOutboundInq.loc_id,
                        @pkg_type = "CTN",
                        @pkg_qty = objOutboundInq.ppk,
                        @itm_qty = objOutboundInq.ppk,
                        //@lbl_id = "1111",
                        @lbl_id = objOutboundInq.lbl_id,
                        @grn_id = objOutboundInq.refno,
                        @po_num = "-",
                        @process_id = "MassConv",
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.depth,
                        @wgt = objOutboundInq.wgt,
                        @cube = objOutboundInq.cube,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Update_trn_hdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_update_trn_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @palet_id = objOutboundInq.palet_id,
                        @tran_type = "1RCVD",
                        @status = "AVAIL",
                        @grn_id = objOutboundInq.refno,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Add_To_proc_save_doc_hdr(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_insert_doc_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objOutboundInq.cmp_id,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                        @status = "OPEN",
                        @step = "OPEN",
                        @ordr_type = "REG",
                        @ib_doc_dt = objOutboundInq.doc_date,
                        @vend_id = "-",
                        @vend_name = "-",
                        @po_num = "-",
                        @ordr_dt = objOutboundInq.doc_date,
                        @req_num = objOutboundInq.refno,
                        @cntr_id = "-",
                        @fob = "1000000001",
                        @billto_id = "10001",
                        @ship_from = "10001",
                        @shipto_id = "10001",
                        @dept_id = "10001",
                        @shipvia_id = "-",
                        @freight_id = "111111",
                        @terms_id = "10001",
                        @note = objOutboundInq.Note,
                        @process_id = "ADD",
                        @eta_dt = objOutboundInq.doc_date,
                        @rma_flag = "",
                        @master_bol = "-",
                        @vessel_no = "-"
                    }, commandType: CommandType.StoredProcedure);
            }
        }

        public void Add_To_proc_save_doc_dtl(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_insert_Doc_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {


                        @cmp_id = objOutboundInq.cmp_id,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                        @line_num = objOutboundInq.dtl_line,
                        @status = "OPEN",
                        @step = "OPEN",
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @itm_name = objOutboundInq.itm_name,
                        @vend_itm_num = objOutboundInq.itm_num,
                        @vend_itm_color = objOutboundInq.itm_color,
                        @vend_itm_size = objOutboundInq.itm_size,
                        @ordr_ctn = 1,
                        @ordr_qty = objOutboundInq.OrderQty,
                        @rcvd_ctn = "0",
                        @rcvd_qty = "0",
                        @qty_uom = "EA",
                        @back_ordr_qty = "0",
                        @price = "0",
                        @price_uom = "EA",
                        @list_price = "0",
                        @list_uom = "EA",
                        @pack_id = "PF",
                        @note = "-",
                        @process_id = "ADD"
                    }, commandType: CommandType.StoredProcedure);
            }
        }

        public void Add_To_proc_save_doc_ctn(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_insert_log_doc_ctn";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                        @dtl_line = objOutboundInq.dtl_line,
                        @ctn_line = objOutboundInq.CtnLn,
                        @itm_line = objOutboundInq.CtnLn,//CR-20180217
                        @kit_id = objOutboundInq.itm_code,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_qty = objOutboundInq.ppk,
                        @ctn_qty = objOutboundInq.ppk,
                        @tot_ctn = 1,
                        @tot_qty = objOutboundInq.OrderQty,
                        @rcvd_ctn = "0",
                        @rcvd_qty = "0",
                        @locid = "FLOOR",
                        @po_num = "-",
                        @iorate = "INOUT-1",
                        @strgrate = "STRG-1",
                        @length = objOutboundInq.length,
                        @width = objOutboundInq.width,
                        @depth = objOutboundInq.depth,
                        @wgt = objOutboundInq.wgt,
                        @cube = objOutboundInq.cube,
                        @process_id = "ADD",
                        @lot_id = ""
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Insert_StockVerify(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_Add_ma_stock_verify";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                        @whs_id = objOutboundInq.whs_id,
                        @itm_code = objOutboundInq.itm_code,
                        @itm_num = objOutboundInq.itm_num,
                        @itm_color = objOutboundInq.itm_color,
                        @itm_size = objOutboundInq.itm_size,
                        @itm_name = objOutboundInq.itm_name,
                        @ordr_qty = objOutboundInq.OrderQty,
                        @Avail_qty = objOutboundInq.avail_qty,
                        @Bo_qty = objOutboundInq.Back_Order_Qty,
                        @balance_qty = objOutboundInq.balance_qty,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void Delete_StockVerify(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_delte_stock_verify";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objOutboundInq.cmp_id,
                        @so_num = objOutboundInq.Sonum,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public OutboundInq Get_StockList(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_GET_stock_verify_LIST";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @cmp_id = objOutboundInq.cmp_id,
                    
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstStockverify = ListInq.ToList();
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
        public OutboundInq Get_IbdocId(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "PROC_GET_MVCWEB_ISRMA_CHECKED";
                    IEnumerable<OutboundInq> ListDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_ENTITY_CODE = "rma_doc_id",
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListRMADocId = ListDocIdLIST.ToList();
                    //   objInboundInquiry.ib_doc_id = objInboundInquiry.ListRMADocId[0].ct_value;

                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq Get_LotId(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "PROC_GET_MVCWEB_ISRMA_CHECKED";
                    IEnumerable<OutboundInq> ListDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_ENTITY_CODE = "lot_id",
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstLotId = ListDocIdLIST.ToList();
                    //   objInboundInquiry.ib_doc_id = objInboundInquiry.ListRMADocId[0].ct_value;

                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetPaletIdValidation(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IB_GET_PALETIDVALIDATE";
                    IEnumerable<OutboundInq> ListDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_PALETID_ID = objOutboundInq.palet_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListPaletId = ListDocIdLIST.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GetpaletId(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_palet_id";
                    IEnumerable<OutboundInq> ListIbDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @paletid = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListPaletId = ListIbDocIdLIST.ToList();
                    objOutboundInq.palet_id = objOutboundInq.ListPaletId[0].palet_id;

                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public void Change_dochdr_Status(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_DOC_HDR_STATUS_CHANGE";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @Cmp_id = objOutboundInq.cmp_id,
                        @ib_doc_id = objOutboundInq.ib_doc_id,
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public OutboundInq OutboundGETTEMPLISTvalue(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_TEMPLIST";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                         @p_str_cmp_id = objOutboundInq.cmp_id,
                         @p_str_so_num = objOutboundInq.so_num,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.lstShipAlocdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq Update_Tbl_Temp_So_Auto_Aloc_BackOrdervalue(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_UPDATE_BACK_ORD_QTY_TBL_TEMP_SO_AUTO_ALOC";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @P_STR_MP_ID = objOutboundInq.cmp_id,
                        @P_STR_SO_NUM = objOutboundInq.so_num,
                        @P_STR_ITM_CODE = objOutboundInq.itm_code,
                        @P_INT_BO_QTY = objOutboundInq.back_ordr_qty,
                        @P_INT_ALOC_QTY = objOutboundInq.aloc_qty,
                        @P_INT_LINE_NO = objOutboundInq.line_num,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.lstShipAlocdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq GETSONUMLIST(OutboundInq objOutboundInq)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_GET_SO_NUM";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {
                        @pStrCmpId = objOutboundInq.cmp_id,
                        @pStrBatchId = objOutboundInq.quote_num,
                        @pStrSoNumFrom = objOutboundInq.so_numFm,
                        @pStrSoNumTo = objOutboundInq.so_numTo,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOutboundInq.lstShipAlocdtl = ListIRFP.ToList();
                }
                return objOutboundInq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundInq StockverifyRpt(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_STOCK_VERIFY_Rpt";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pStrBatchId = objOutboundInq.quote_num,
                            @pStrSoNumFrom = objOutboundInq.so_numFm,
                            @pStrSoNumTo = objOutboundInq.so_numTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstStkverifyList = ListInq.ToList();
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
  public OutboundInq GetStockVerifyRptTotalCottonRecords(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_STOCK_VERIFY_Rpt_TotalRecords";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @pStrCmpId = objOutboundInq.cmp_id,
                            @pStrSo_num=objOutboundInq.quote_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstStkverifyListTotal = ListInq.ToList();
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
        public OutboundInq GetEcomSR940Rpt(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundInq objOutboundInqCategory = new OutboundInq();

                    const string storedProcedure2 = "SP_MVC_OB_940SR_RPT";
                    IEnumerable<OutboundInq> ListInq = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @cmp_id = objOutboundInq.cmp_id,
                            @batch_no = objOutboundInq.quote_num,
                            @So_numFm = objOutboundInq.so_numFm,
                            @So_numTo = objOutboundInq.so_numTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.ListeCom940SRUploadDtlRpt = ListInq.ToList();
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
        public OutboundInq InsertTempFileDocument(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                   
                    const string storedProcedure1 = "SP_MVC_INSERT_UPLOAD_DOCUMENT_WEB";
                    connection.Execute(storedProcedure1,
                        new
                        {

                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_IB_DOC_ID = objOutboundInq.shipdocid.Trim(),
                            @P_STR_DOCTYPE = objOutboundInq.doctype,
                            @P_STR_FILENAME = objOutboundInq.Filename,
                            @P_STR_FILEPATH = objOutboundInq.filepath,
                            @P_STR_UPLOADDT = objOutboundInq.Uploaddt,
                            @P_STR_UPLOADBY = objOutboundInq.Uploadby,
                            @P_STR_COMMENT = objOutboundInq.Comments,
                        @UPLOAD_FILE = objOutboundInq.UPLOAD_FILE
                    }, commandType: CommandType.StoredProcedure);
                    return objOutboundInq;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public OutboundInq GetTempFiledtl(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_GET_UPLOAD_DOCUMENT_WEB";
                    IEnumerable<OutboundInq> ListIbDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundInq.cmp_id,
                            @P_STR_IB_DOC_ID = objOutboundInq.Sonum,
                            @P_STR_DOCTYPE = objOutboundInq.doctype,
                            @P_STR_FILENAME = objOutboundInq.Filename,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstFiledtl = ListIbDocIdLIST.ToList();

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
        public OutboundInq Add_To_proc_save_audit_trail(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "Sp_ob_insert_audit_trail";
                    IEnumerable<OutboundInq> ListIbDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objOutboundInq.cmp_id,
                            @p_str_ob_doc_id = objOutboundInq.Sonum,
                            @p_str_mode = objOutboundInq.mode,
                            @p_str_aloc_doc_id = objOutboundInq.aloc_doc_id,
                            @p_str_ship_doc_id = objOutboundInq.shipdocid,
                            @p_str_maker = objOutboundInq.maker,
                            @p_dt_maker_dt = objOutboundInq.makerdt,
                            @p_dt_access_stamp = objOutboundInq.makerdt,
                            @p_str_audit_comments = objOutboundInq.Auditcomment,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstFiledtl = ListIbDocIdLIST.ToList();

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

        public bool fnGenerateIBfromOB(string pstrCmpId, string pstrSoNum, string pstrAlocDocId)
        {
            bool lblnStatus = false;
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string lstrSPName = "spGenerateIBfromOB";
                    connection.Execute(lstrSPName,
                        new
                        {

                            @pstrCmpId = @pstrCmpId,
                            @pstrSoNum = @pstrSoNum,
                            @pstrAlocDocId = @pstrAlocDocId,
                        }, commandType: CommandType.StoredProcedure);
                }
                lblnStatus = true;
            }
            catch (Exception ex)
            {
                lblnStatus = false;
                throw ex;
            }
            finally
            {
              

            }
            return lblnStatus;
        }

        public OutboundInq ItemXGetshiptoDetails(string term, string cmp_id,string cust_id)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_pick_shipto_id_Addrs_details";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmp_id,
                        @SearchText = term,
                        @cust_id=cust_id,

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
        public OutboundInq CheckShipToidExist(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_CHECK_SHIPTOID_EXSITS";
                    IEnumerable<OutboundInq> ListIbDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objOutboundInq.cmp_id,
                            @p_str_shipto_id = objOutboundInq.shipto_id,
                            @p_str_cust_id = objOutboundInq.cust_id,        
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstFiledtl = ListIbDocIdLIST.ToList();

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
        public OutboundInq GetCustDtl(string term, string cmp_id)
        {
            try
            {
                OutboundInq objCustDtl = new OutboundInq();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_GET_CUST_DTL";
                    IList<OutboundInq> ListIRFP = connection.Query<OutboundInq>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmp_id,
                        @SearchText = term,                      

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
        public void Change_LotDtl_Status_atAdd(OutboundInq objOutboundInq)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "Sp_MVC_STATUS_UPDATE_LOT_DTL";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_CMP_ID = objOutboundInq.cmp_id,
                        @P_STR_DOC_ID = objOutboundInq.aloc_doc_id,                      
                        @P_PROCESS_ID = "ADD",                                            
                    }, commandType: CommandType.StoredProcedure, commandTimeout: 0);
            }

        }
        public OutboundInq LoadCustDtls(OutboundInq objOutboundInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_cust_config_dtls";
                    IEnumerable<OutboundInq> ListIbDocIdLIST = connection.Query<OutboundInq>(storedProcedure2,
                        new
                        {
                            @cmp_id = objOutboundInq.cmp_id,                         
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundInq.LstFiledtl = ListIbDocIdLIST.ToList();

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

        public OBGetBOLConf GetOBBOLConfDtlRptData(OBGetBOLConf objOBBOLConfDtlRptData, string p_str_cmp_id, string p_str_so_num_from, string p_str_so_num_to, string p_str_ship_dt_from, string p_str_ship_dt_to, string p_str_batch_id)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetBOLConf> ListIRFP = connection.Query<OBGetBOLConf>("sp_ob_get_bol_conf_rpt", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                        @p_str_so_num_from = p_str_so_num_from,
                        @p_str_so_num_to = p_str_so_num_to,
                        @p_str_ship_dt_from = p_str_ship_dt_from,
                        @p_str_ship_dt_to = p_str_ship_dt_to,
                        @p_str_batch_id = p_str_batch_id
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBBOLConfDtlRptData.ListOBBOLConfRpt = ListIRFP.ToList();
                }
                return objOBBOLConfDtlRptData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }



        public OBGetSRBOLConfRpt GetOBSRBOLConfRpt(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id,  string p_str_batch_id, string p_str_so_num,  string p_str_so_num_from, string p_str_so_num_to, string p_str_so_dt_from, string p_str_so_dt_to)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetSRBOLConfRpt> ListIRFP = connection.Query<OBGetSRBOLConfRpt>("sp_ob_get_sr_bol_conf_rpt", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                        @p_str_batch_id = p_str_batch_id,
                        @p_str_so_num = p_str_so_num,
                        @p_str_so_num_from = p_str_so_num_from,
                        @p_str_so_num_to = p_str_so_num_to,
                        @p_str_so_dt_from = p_str_so_dt_from,
                        @p_str_so_dt_to = p_str_so_dt_to
                        
                       
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRBOLConfRptData.ListOBGetSRBOLConfRpt = ListIRFP.ToList();
                }
                return objOBSRBOLConfRptData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }



        public OBCtnLblPrnt GetCtnLabelPrint(OBCtnLblPrnt objOBCtnLblPrnt, string p_str_cmp_id, string p_str_so_num)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBCtnLblPrnt> ListCtnLabelPrint = connection.Query<OBCtnLblPrnt>("SP_OB_GET_CTN_LBL_PRNT", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                        @p_str_so_num = p_str_so_num
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBCtnLblPrnt.LstCtnLabelPrint = ListCtnLabelPrint.ToList();
                }
                return objOBCtnLblPrnt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OBDocExcp GetOBDocExcpList(OBDocExcp objOBDocExcp, string p_str_cmp_id)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBDocExcp> ListOBDocExcp = connection.Query<OBDocExcp>("sp_ob_get_doc_excp_list", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBDocExcp.ListOBDocExcp = ListOBDocExcp.ToList();
                }
                return objOBDocExcp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string GetSRPickRefNumber(string p_str_cmp_id , string p_str_so_num)
        {

            try
            {
                string p_str_sr_pick_ref_no = string.Empty;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_get_sr_pick_ref_no", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_so_num ", SqlDbType.VarChar).Value = p_str_so_num;
                connection.Open();
                p_str_sr_pick_ref_no = Convert.ToString( command.ExecuteScalar());
                return p_str_sr_pick_ref_no;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public OBAlocPostInquiry GetOBAlocOpenList(OBAlocPostInquiry objOBAlocPostInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetAlocSummary> ListIRFP = connection.Query<OBGetAlocSummary>("sp_ob_get_aloc_open_list", new
                    {
                        @p_str_cmp_id = objOBAlocPostInquiry.cmp_id,
                        @p_str_batch_num = objOBAlocPostInquiry.batch_num,
                        @p_str_so_num_from = objOBAlocPostInquiry.so_num_from,
                        @p_str_so_num_to = objOBAlocPostInquiry.so_num_to,
                        @p_str_so_dt_from = objOBAlocPostInquiry.so_dt_from,
                        @p_str_so_dt_to = objOBAlocPostInquiry.so_dt_to,
                        @p_str_load_number = objOBAlocPostInquiry.load_number


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBAlocPostInquiry.ListOBGetAlocOpenSummary = ListIRFP.ToList();




                }
                return objOBAlocPostInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OBAlocPostInquiry GetEcomOBAlocOpenList(string pstrCmpId, string pstrBatchId)
        {
            OBAlocPostInquiry objOBAlocPostInquiry = new OBAlocPostInquiry();
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetAlocSummary> ListIRFP = connection.Query<OBGetAlocSummary>("spEcomGetOpenAlocListForPost", new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrBatchId = pstrBatchId,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBAlocPostInquiry.ListOBGetAlocOpenSummary = ListIRFP.ToList();

                }
                return objOBAlocPostInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public int GetAlocPostRefNo()
        {

            try
            {
                int p_int_aloc_tmp_ref_no = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_aloc_tmp_ref_no", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                p_int_aloc_tmp_ref_no = Convert.ToInt32(command.ExecuteScalar());

                if (p_int_aloc_tmp_ref_no > 0)
                {
                    return p_int_aloc_tmp_ref_no;
                }
                else
                {
                    return p_int_aloc_tmp_ref_no;
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

        public bool SaveBulkAlocTempDtl(DataTable dtAlocList)
        {
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        //Set the database table name
                        connection.Open();
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_iv_aloc_post_temp";
                        sqlBulkCopy.WriteToServer(dtAlocList);
                        connection.Close();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {

            }


        }

        public  bool SaveOBBulkAlocPost(string p_str_cmp_id, string p_str_aloc_tmp_ref_no,   DataTable dtAlocList)
        {
            try
            {
                if (SaveBulkAlocTempDtl(dtAlocList) == true)
                {



                    using (IDbConnection connection = ConnectionManager.OpenConnection())
                    {
                        using (IDbTransaction tran = connection.BeginTransaction())
                        { 
                            const string storedProcedure1 = "SP_OB_ALOC_POST_BY_BATCH";
                        connection.Execute(storedProcedure1,
                            new
                            {
                                @P_STR_CMP_ID = p_str_cmp_id,
                                @P_STR_REQ_NUM = p_str_aloc_tmp_ref_no,

                            }, commandType: CommandType.StoredProcedure, transaction: tran);
                            tran.Commit();
                        }
                }
                    return true;
                }
                else
                {
                    return false;

                }

               
            }

            catch (Exception ex)
            {
                return false;
                throw ex;

            }
            finally
            {

            }

        }

        public DataTable OutboundInqShipAckExcel(string l_str_cmp_id, string l_str_so_num)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_webmvc_Ship_Request_Ack_Rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@Cmp_ID", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@Sonum", SqlDbType.VarChar).Value = l_str_so_num;
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

        public DataTable OutboundInqGridSummaryExcel(string l_str_cmp_id, string l_str_AlocId, string l_str_so_num_frm, string l_str_so_num_To, string l_str_so_dt_frm, string l_str_so_dt_to, string l_str_CustPO, string l_str_status, string l_str_Store, string l_str_batch_id, string l_str_shipdtFm, string l_str_shipdtTo, string l_str_Style, string l_str_color, string l_str_size)
        {
            try
            {
                var status = l_str_status;
                if (status == "OPEN")
                {
                    status = "O";
                }
                else if (status == "SHIP")
                {
                    status = "S";
                }
                else if (status == "ALL")
                {
                    status = "ALL";
                }
                else if (status == "POST")
                {
                    status = "P";
                }
                else if (status == "ALOC")
                {
                    status = "S";
                }

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_get_webmvc_Grid_inquiry_Summary", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@CompID", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@SRNumFm", SqlDbType.VarChar).Value = l_str_so_num_frm;
                command.Parameters.Add("@SRNumTo", SqlDbType.VarChar).Value = l_str_so_num_To;
                command.Parameters.Add("@ShipReqDtFr", SqlDbType.VarChar).Value = l_str_so_dt_frm;
                command.Parameters.Add("@ShipReqDtTo", SqlDbType.VarChar).Value = l_str_so_dt_to;
                command.Parameters.Add("@CustPO", SqlDbType.VarChar).Value = l_str_CustPO;
                command.Parameters.Add("@AlocNum", SqlDbType.VarChar).Value = l_str_AlocId;
                command.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                command.Parameters.Add("@StoreId", SqlDbType.VarChar).Value = l_str_Store;
                command.Parameters.Add("@BatchID", SqlDbType.VarChar).Value = l_str_batch_id;
                command.Parameters.Add("@ShipDtFm", SqlDbType.VarChar).Value = l_str_shipdtFm;
                command.Parameters.Add("@ShipDtTo", SqlDbType.VarChar).Value = l_str_shipdtTo;
                command.Parameters.Add("@Style", SqlDbType.VarChar).Value = l_str_Style;
                command.Parameters.Add("@Color", SqlDbType.VarChar).Value = l_str_color;
                command.Parameters.Add("@Size", SqlDbType.VarChar).Value = l_str_size;
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
        public DataTable OutboundInqPickByStyle(string l_str_cmp_id, string l_str_so_num_frm, string l_str_so_num_To, string l_str_batch_id)
        {
            try
            {
                string pstrBatchOperator = string.Empty;
                string pstrBatchIdFm = string.Empty;
                string pstrBatchIdTo = string.Empty;
                string[] aryBatch;
                if (l_str_batch_id.Contains(","))
                {
                    pstrBatchOperator = ",";
                    aryBatch = l_str_batch_id.Split(',');
                    pstrBatchIdFm = aryBatch[0];
                    pstrBatchIdTo = aryBatch[1];
                }
                else
                {
                    pstrBatchIdFm = l_str_batch_id;
                    pstrBatchIdTo = string.Empty;
                }

                var l_str_status = "S";
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_webmvc_ship_inq_pickStyle", connection);
                command.CommandTimeout = 0;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@StrCmpId", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@StrSoNumFm", SqlDbType.VarChar).Value = l_str_so_num_frm;
                command.Parameters.Add("@StrSoNumTo", SqlDbType.VarChar).Value = l_str_so_num_To;
                command.Parameters.Add("@StrBatchIdFm", SqlDbType.VarChar).Value = pstrBatchIdFm;
                command.Parameters.Add("@StrBatchIdTo", SqlDbType.VarChar).Value = pstrBatchIdTo;
                command.Parameters.Add("@pstrBatchOperator", SqlDbType.VarChar).Value = pstrBatchOperator;
                command.Parameters.Add("@lstrStatus", SqlDbType.VarChar).Value = l_str_status;
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
        public DataTable OutboundInqPickByLocation(string l_str_cmp_id, string l_str_so_num_frm, string l_str_so_num_To, string l_str_batch_id)
        {
            try
            {
                var l_str_status = "S";
                string pstrBatchOperator = string.Empty;
                string pstrBatchIdFm = string.Empty;
                string pstrBatchIdTo = string.Empty;
                string[] aryBatch;
                if (l_str_batch_id.Contains(","))
                {
                    pstrBatchOperator = ",";
                    aryBatch = l_str_batch_id.Split(',');
                    pstrBatchIdFm = aryBatch[0];
                    pstrBatchIdTo = aryBatch[1];
                }
                else
                {
                    pstrBatchIdFm = l_str_batch_id;
                    pstrBatchIdTo = string.Empty;
                }
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_webmvc_ship_inq_pickLocation", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@StrCmpId", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@StrSoNumFm", SqlDbType.VarChar).Value = l_str_so_num_frm;
                command.Parameters.Add("@StrSoNumTo", SqlDbType.VarChar).Value = l_str_so_num_To;
                command.Parameters.Add("@StrBatchIdFm", SqlDbType.VarChar).Value = pstrBatchIdFm;
                command.Parameters.Add("@StrBatchIdTo", SqlDbType.VarChar).Value = pstrBatchIdTo;
                command.Parameters.Add("@pstrBatchOperator", SqlDbType.VarChar).Value = pstrBatchOperator;

                command.Parameters.Add("@lstrStatus", SqlDbType.VarChar).Value = l_str_status;
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

        public DataTable OutboundInqSummaryExcel(string l_str_cmp_id, string l_str_so_NumFm, string l_str_so_NumTo, string l_str_so_DtFm, string l_str_so_DtTo, string l_str_CustPo, string l_str_AlocNo, string l_str_status, string l_str_store, string l_str_batchId, string l_str_ShipdtFm, string l_str_ShipdtTo, string l_str_Style, string l_str_Color, string l_str_Size)
        {
            try
            {
                var status = l_str_status;
                if (status == "OPEN")
                {
                    status = "O";
                }
                else if (status == "SHIP")
                {
                    status = "S";
                }
                else if (status == "ALL")
                {
                    status = "";
                }
                else if (status == "POST")
                {
                    status = "P";
                }
                else if (status == "ALOC")
                {
                    status = "S";
                }
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_webmvc_ship_request_inquiry_Rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@Cmp_ID", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@so_NumFm", SqlDbType.VarChar).Value = l_str_so_NumFm;
                command.Parameters.Add("@so_NumTo", SqlDbType.VarChar).Value = l_str_so_NumTo;
                command.Parameters.Add("@so_DtFm", SqlDbType.VarChar).Value = l_str_so_DtFm;
                command.Parameters.Add("@so_DtTo", SqlDbType.VarChar).Value = l_str_so_DtTo;
                command.Parameters.Add("@CustPo", SqlDbType.VarChar).Value = l_str_CustPo;
                command.Parameters.Add("@AlocNo", SqlDbType.VarChar).Value = l_str_AlocNo;
                command.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
                command.Parameters.Add("@store", SqlDbType.VarChar).Value = l_str_store;
                command.Parameters.Add("@batchId", SqlDbType.VarChar).Value = l_str_batchId;
                command.Parameters.Add("@ShipdtFm", SqlDbType.VarChar).Value = l_str_ShipdtFm;
                command.Parameters.Add("@ShipdtTo", SqlDbType.VarChar).Value = l_str_ShipdtTo;
                command.Parameters.Add("@Style", SqlDbType.VarChar).Value = l_str_Style;
                command.Parameters.Add("@Color", SqlDbType.VarChar).Value = l_str_Color;
                command.Parameters.Add("@Size", SqlDbType.VarChar).Value = l_str_Size;
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

        public int fnCheckOBCustPOExists(string pstrCmpId, string pstrCustOrdrNum)
        {

            try
            {
                int pintCustPOCount = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spCheckOBCustPOExists", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                command.Parameters.Add("@pstrCustOrdrNum", SqlDbType.VarChar).Value = pstrCustOrdrNum;
                connection.Open();
                pintCustPOCount = Convert.ToInt32(command.ExecuteScalar());
                return pintCustPOCount;
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
