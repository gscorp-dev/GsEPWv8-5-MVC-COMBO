using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
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
    public class StockInquiryRepository
    {
        public StockInquiryDtl GetStockInquiryDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_webmvc_stock_inquiry";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @Status = objStackInquiry.Status,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Fm_Dt = objStackInquiry.Date_fm,
                            @To_Dt = objStackInquiry.Date_to,
                            @Po_Num = objStackInquiry.po_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListStockInquiry = ListstockInquiryLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //public void TruncateTempStockInquiry(StockInquiryDtl objStackInquiry)
        //{
        //    using (IDbConnection connection = ConnectionManager.OpenConnection())
        //    {
        //        const string storedProcedure1 = "proc_get_mvc_web_truncate_Temp_Stock_Inquiry";
        //        connection.Execute(storedProcedure1,
        //            new
        //            {


        //            }, commandType: CommandType.StoredProcedure);

        //    }
        //}
        public void InsertTempInventory(StockInquiryDtl objStackInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_Insert_Temptable_Inventory";
                connection.Execute(storedProcedure1,
                    new
                    {
                        
                        @cmp_id = objStackInquiry.cmp_id,
                        @ib_doc_id = objStackInquiry.ib_doc_id,
                        @cont_id = objStackInquiry.cont_id,
                        @itm_num = objStackInquiry.itm_num,
                        @itm_color = objStackInquiry.itm_color,
                        @itm_size = objStackInquiry.itm_size,
                        @itm_name = objStackInquiry.itm_name,
                        @loc_id = objStackInquiry.loc_id,
                        @lot_id = objStackInquiry.lot_id,
                        @palet_id = objStackInquiry.palet_id,
                        @po_num = objStackInquiry.po_num,
                        @tot_ctn = objStackInquiry.TOT_CTN,
                        @pkg_qty = objStackInquiry.pkg_qty,
                        @whs_id = objStackInquiry.whs_id,
                        @itm_qty = objStackInquiry.itm_qty,
                        @tot_qty = objStackInquiry.tot_qty,
                        @aloc_ctn = objStackInquiry.AlocCtn,
                        @aloc_qty = objStackInquiry.Alocqty,
                        @pkg_type = objStackInquiry.pkg_type,
                        @kit_id = objStackInquiry.kit_id
                       

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public StockInquiryDtl GetStockInquiryDetailAloc(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_webmvc_stock_inquiry_Dtl";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @Status = objStackInquiry.Status,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Fm_Dt = objStackInquiry.Date_fm,
                            @To_Dt = objStackInquiry.Date_to,
                            @Po_Num = objStackInquiry.po_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstStockInquirystock = ListstockInquiryLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public StockInquiryDtl GetStockInquiryGridDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvc_web_Temp_Stock_Inquiry";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @P_Str_Cmp_Id = objStackInquiry.cmp_id,
                            @P_Str_Itm_num = objStackInquiry.itm_num,
                            @P_Str_Itm_color = objStackInquiry.itm_color,
                            @P_Str_Itm_size = objStackInquiry.itm_size,                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstStockInquiryGridDtl = ListstockInquiryLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetStockInquiryDtl(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_mvc_stock_inquiry_dtl";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @Status = objStackInquiry.Status,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Fm_Dt = objStackInquiry.Date_fm,
                            @To_Dt = objStackInquiry.Date_to,
                            @Po_Num = objStackInquiry.po_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstStockInquirystock = ListstockInquiryLIST.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetAuditTrailDetails(StockInquiryDtl objStackInquiry) 
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_mvc_audit_trail_inquiry";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            //@Status = objStackInquiry.Status,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Po_Num = objStackInquiry.po_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListStockInquiryaudit = ListstockInquiryLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetStockRptDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_mvc_stock_by_style_rpt";
                    IEnumerable<StockInquiryDtl> ListStockInquiryRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id.Trim(),
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,                          
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Fm_Dt = objStackInquiry.Date_fm,
                            @To_Dt = objStackInquiry.Date_to,
                            @Po_Num = objStackInquiry.po_num,
                            @style_stearch = objStackInquiry.style_stearch
                        },commandTimeout:0,
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListStockInquiryRpt = ListStockInquiryRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetStockSummRptDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_mvc_stock_style_by_summary_rpt";
                    IEnumerable<StockInquiryDtl> ListStockSummRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Fm_Dt = objStackInquiry.Date_fm,
                            @To_Dt = objStackInquiry.Date_to,
                            @Po_Num = objStackInquiry.po_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListStockSummRpt = ListStockSummRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetStockLocRptDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_mvc_stock_by_loc_rpt";
                    IEnumerable<StockInquiryDtl> ListStockLocRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id.Trim(),
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Fm_Dt = objStackInquiry.Date_fm,
                            @To_Dt = objStackInquiry.Date_to,
                            @Po_Num = objStackInquiry.po_num,
                            @style_stearch = objStackInquiry.style_stearch
                        }, commandTimeout:0,
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListStockLocRpt = ListStockLocRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetStockLocSummRptDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_mvc_stock_by_loc_rpt";
                    IEnumerable<StockInquiryDtl> ListStockLocRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Fm_Dt = objStackInquiry.Date_fm,
                            @To_Dt = objStackInquiry.Date_to,
                            @Po_Num = objStackInquiry.po_num,
                            @style_stearch = objStackInquiry.style_stearch
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListStockLocRpt = ListStockLocRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetStockStyleSummRptDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_MVC_IV_STK_SMRY_BY_STYLE_RPT";
                    IEnumerable<StockInquiryDtl> ListStockLocRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {

                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Container = objStackInquiry.cont_id,
                            @LocID = objStackInquiry.loc_id,
                            @WareHouse = objStackInquiry.whs_id,
                            @DateFrom = objStackInquiry.Date_fm,
                            @DateTo = objStackInquiry.Date_to,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @ttalRec = 0,
                            @totalAmt = 0,
                            @style_stearch = objStackInquiry.style_stearch


                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListStockLocRpt = ListStockLocRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public StockInquiryDtl GetAudRptDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_MVC_IV_AUD_DTL_RPT";
                    IEnumerable<StockInquiryDtl> ListAudRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Po_Num = objStackInquiry.po_num,
                            @radio = objStackInquiry.radio,
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListAudRpt = ListAudRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetAudSummRptDetails(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_mvc_aud_summ_rpt";
                    IEnumerable<StockInquiryDtl> ListAudSummRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Po_Num = objStackInquiry.po_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListAudSummRpt = ListAudSummRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void AddToTempDataStockInq(StockInquiryDtl objStackInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objStackInquiry.cmp_id,
                        @bdoc_id = objStackInquiry.ib_doc_id,
                        @cont_id = objStackInquiry.cont_id,
                        @itm_num = objStackInquiry.itm_num,
                        @itm_color = objStackInquiry.itm_color,
                        @itm_size = objStackInquiry.itm_size,
                        @itm_desc = objStackInquiry.itm_name,
                        @loc_id = objStackInquiry.loc_id,
                        @lot_id = objStackInquiry.lot_id,
                        @palet_id = objStackInquiry.palet_id,
                        @po_num = objStackInquiry.po_num,
                        @AvlCtn = objStackInquiry.AvlCtn,
                        @pkg_type = objStackInquiry.pkg_type,
                        @whs_id = objStackInquiry.whs_id,
                        @itm_qty = objStackInquiry.itm_qty,
                        @AvlQty = objStackInquiry.AvlQty,
                        @AlocCtn = objStackInquiry.AlocCtn,
                        @AlocQty = objStackInquiry.Alocqty,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public StockInquiryDtl StockInquiryDtl(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvcweb_fetch_stockInq";
                    IEnumerable<StockInquiryDtl> ListAudSummRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {                               
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListStockSummRpt = ListAudSummRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetAudRptSummary(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_MVC_IV_AUD_SMRY_RPT";
                    IEnumerable<StockInquiryDtl> ListAudRptLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @IB_Doc_ID = objStackInquiry.ib_doc_id,
                            @Cont_ID = objStackInquiry.cont_id,
                            @Lot_ID = objStackInquiry.lot_id,
                            @Loc_ID = objStackInquiry.loc_id,
                            @Ref_No = objStackInquiry.grn_id,
                            @Whs_ID = objStackInquiry.whs_id,
                            @Po_Num = objStackInquiry.po_num,
                            @radio = objStackInquiry.radio,
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.ListAudRpt = ListAudRptLIST.ToList();
                }

                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl ItemXGetitmDetails(string term, string cmp_id)
        {
            try
            {
                StockInquiryDtl objCustDtl = new StockInquiryDtl();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_Itm_dtl";
                    IList<StockInquiryDtl> ListIRFP = connection.Query<StockInquiryDtl>(SearchCustDtls, new
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

        public StockInquiryDtl GetStockInquiryStyleDtlExcel(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "Sp_iv_excel_Stk_Rpt_By_Style_Detail";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objStackInquiry.cmp_id,
                            @itm_num = objStackInquiry.itm_num,
                            @itm_color = objStackInquiry.itm_color,
                            @itm_size = objStackInquiry.itm_size,
                            //@Desc = objStackInquiry.itm_name,
                            //@Status = objStackInquiry.Status,
                            @lot_id = objStackInquiry.lot_id,
                            @loc_id = objStackInquiry.loc_id,
                            @po_num = objStackInquiry.po_num,
                            @doc_id = objStackInquiry.ib_doc_id,
                            @cont_id = objStackInquiry.cont_id,
                            @whs_id = objStackInquiry.whs_id,
                            @stkfm_dt = objStackInquiry.Date_fm,
                            @stkTo_dt = objStackInquiry.Date_to,
                            @style_stearch = objStackInquiry.style_stearch
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstStockInquiryStyleDtl = ListstockInquiryLIST.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetStockInquiryStyleSmryExcel(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "Sp_iv_excel_Stk_Rpt_By_Style_Summary";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objStackInquiry.cmp_id,
                            @itm_num = objStackInquiry.itm_num,
                            @itm_color = objStackInquiry.itm_color,
                            @itm_size = objStackInquiry.itm_size,
                            //@Desc = objStackInquiry.itm_name,
                            //@Status = objStackInquiry.Status,
                            @lot_id = objStackInquiry.lot_id,
                            @loc_id = objStackInquiry.loc_id,
                            @po_num = objStackInquiry.po_num,
                            @doc_id = objStackInquiry.ib_doc_id,
                            @cont_id = objStackInquiry.cont_id,
                            @whs_id = objStackInquiry.whs_id,
                            @stkfm_dt = objStackInquiry.Date_fm,
                            @stkTo_dt = objStackInquiry.Date_to,
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstStockInquiryStyleSmry = ListstockInquiryLIST.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetStockInquiryLocDtlExcel(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "Sp_iv_excel_Stk_Rpt_By_Location_Detail";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objStackInquiry.cmp_id,
                            @itm_num = objStackInquiry.itm_num,
                            @itm_color = objStackInquiry.itm_color,
                            @itm_size = objStackInquiry.itm_size,
                            //@Desc = objStackInquiry.itm_name,
                            //@Status = objStackInquiry.Status,
                            @lot_id = objStackInquiry.lot_id,
                            @loc_id = objStackInquiry.loc_id,
                            @po_num = objStackInquiry.po_num,
                            @doc_id = objStackInquiry.ib_doc_id,
                            @cont_id = objStackInquiry.cont_id,
                            @whs_id = objStackInquiry.whs_id,
                            @stkfm_dt = objStackInquiry.Date_fm,
                            @stkTo_dt = objStackInquiry.Date_to,
                            @style_stearch = objStackInquiry.style_stearch
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstStockInquiryLocDtl = ListstockInquiryLIST.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //CR-180418-001 Commented And Added by Nithya
        public StockInquiryDtl GetStockInquiryLocSmryExcel(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    //const string storedProcedure1 = "Sp_iv_excel_Stk_Rpt_By_Style_Summary";
                    const string storedProcedure1 = "SP_MVC_IV_STK_SMRY_BY_LOC_RPT";                   
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objStackInquiry.cmp_id,
                            @Container = objStackInquiry.cont_id,
                            @LocID = objStackInquiry.loc_id,
                            @WareHouse = objStackInquiry.whs_id,
                            @DateFrom = objStackInquiry.Date_fm,
                            @DateTo = objStackInquiry.Date_to,
                            @Style = objStackInquiry.itm_num,
                            @Color = objStackInquiry.itm_color,
                            @Size = objStackInquiry.itm_size,
                            @Desc = objStackInquiry.itm_name,
                            @ttalRec = 0,
                            @totalAmt = 0,
                            @style_stearch = objStackInquiry.style_stearch
                            //@cmp_id = objStackInquiry.cmp_id,
                            //@itm_num = objStackInquiry.itm_num,
                            //@itm_color = objStackInquiry.itm_color,
                            //@itm_size = objStackInquiry.itm_size,
                            ////@Desc = objStackInquiry.itm_name,
                            ////@Status = objStackInquiry.Status,
                            //@lot_id = objStackInquiry.lot_id,
                            //@loc_id = objStackInquiry.loc_id,
                            //@po_num = objStackInquiry.po_num,
                            //@doc_id = objStackInquiry.ib_doc_id,
                            //@cont_id = objStackInquiry.cont_id,
                            //@whs_id = objStackInquiry.whs_id,
                            //@stkfm_dt = objStackInquiry.Date_fm,
                            //@stkTo_dt = objStackInquiry.Date_to,
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstStockInquiryLocSmry = ListstockInquiryLIST.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetAuditWithIsmExcel(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "Sp_iv_excel_Stk_Rpt_AuditTrail_Summary_WithISM";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objStackInquiry.cmp_id,
                            @itm_num = objStackInquiry.itm_num,
                            @itm_color = objStackInquiry.itm_color,
                            @itm_size = objStackInquiry.itm_size,
                            //@Desc = objStackInquiry.itm_name,
                            //@Status = objStackInquiry.Status,
                            //
                            @doc_id = objStackInquiry.ib_doc_id,
                            @loc_id = objStackInquiry.loc_id,
                            @po_num = objStackInquiry.po_num,
                            @lot_id = objStackInquiry.lot_id,
                            @cont_id = objStackInquiry.cont_id,
                            @whs_id = objStackInquiry.whs_id,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstAuditWithIsm = ListstockInquiryLIST.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetAuditWithOutIsmExcel(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvcweb_iv_excel_Stk_Rpt_AuditTrail_WithoutISM_Detail";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objStackInquiry.cmp_id,
                            @itm_num = objStackInquiry.itm_num,
                            @itm_color = objStackInquiry.itm_color,
                            @itm_size = objStackInquiry.itm_size,
                            //@Desc = objStackInquiry.itm_name,
                            //@Status = objStackInquiry.Status,
                            //
                            @doc_id = objStackInquiry.ib_doc_id,
                            @loc_id = objStackInquiry.loc_id,
                            @po_num = objStackInquiry.po_num,
                            @lot_id = objStackInquiry.lot_id,
                            @cont_id = objStackInquiry.cont_id,
                            @whs_id = objStackInquiry.whs_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstAuditWithOutIsm = ListstockInquiryLIST.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockInquiryDtl GetAuditOnlyIsmExcel(StockInquiryDtl objStackInquiry)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "Sp_iv_excel_Stk_Rpt_AuditTrail_Summary_ISMOnly";
                    IEnumerable<StockInquiryDtl> ListstockInquiryLIST = connection.Query<StockInquiryDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objStackInquiry.cmp_id,
                            @itm_num = objStackInquiry.itm_num,
                            @itm_color = objStackInquiry.itm_color,
                            @itm_size = objStackInquiry.itm_size,
                            //@Desc = objStackInquiry.itm_name,
                            //@Status = objStackInquiry.Status,
                            //
                            @doc_id = objStackInquiry.ib_doc_id,
                            @loc_id = objStackInquiry.loc_id,
                            @po_num = objStackInquiry.po_num,
                            @lot_id = objStackInquiry.lot_id,
                            @cont_id = objStackInquiry.cont_id,
                            @whs_id = objStackInquiry.whs_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstAuditOnlyIsm = ListstockInquiryLIST.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public StockInquiryDtl GetAuditItemDtl(StockInquiryDtl objStackInquiry, string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "sp_iv_audit_dtl_by_style";
                    IEnumerable<AuditItemDtl> LstAuditItemDtl = connection.Query<AuditItemDtl>(storedProcedure1,
                        new
                        {
                            @p_str_cmp_id = p_str_cmp_id,
                            @p_str_itm_num = p_str_itm_num,
                            @p_str_itm_color = p_str_itm_color,
                            @p_str_itm_size = p_str_itm_size,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objStackInquiry.LstAuditItemDtl = LstAuditItemDtl.ToList();
                }
                return objStackInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }



        public DataTable GetStockStyleSummRpt(StockInquiryDtl objStackInquiry)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_MVC_IV_STK_SMRY_BY_STYLE_RPT", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cmp_ID", SqlDbType.VarChar).Value = objStackInquiry.cmp_id;
                command.Parameters.Add("@Container", SqlDbType.VarChar).Value = objStackInquiry.cont_id;
                command.Parameters.Add("@LocID", SqlDbType.VarChar).Value = objStackInquiry.loc_id;
                command.Parameters.Add("@WareHouse", SqlDbType.VarChar).Value = objStackInquiry.whs_id;
                command.Parameters.Add("@DateFrom", SqlDbType.VarChar).Value = objStackInquiry.Date_fm;
                command.Parameters.Add("@DateTo", SqlDbType.VarChar).Value = objStackInquiry.Date_to;
                command.Parameters.Add("@Style", SqlDbType.VarChar).Value = objStackInquiry.itm_num;
                command.Parameters.Add("@Color", SqlDbType.VarChar).Value = objStackInquiry.itm_color;
                command.Parameters.Add("@Size", SqlDbType.VarChar).Value = objStackInquiry.itm_size;
                command.Parameters.Add("@Desc", SqlDbType.VarChar).Value = objStackInquiry.itm_name;
                command.Parameters.Add("@ttalRec", SqlDbType.VarChar).Value = 0;
                command.Parameters.Add("@totalAmt", SqlDbType.VarChar).Value = 0;
                command.Parameters.Add("@style_stearch", SqlDbType.VarChar).Value = objStackInquiry.style_stearch;
                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable GetStockSmryByLocRpt(StockInquiryDtl objStackInquiry)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spIVStkSummaryByLoc", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cmp_ID", SqlDbType.VarChar).Value = objStackInquiry.cmp_id;
                command.Parameters.Add("@Container", SqlDbType.VarChar).Value = objStackInquiry.cont_id;
                command.Parameters.Add("@LocID", SqlDbType.VarChar).Value = objStackInquiry.loc_id;
                command.Parameters.Add("@WareHouse", SqlDbType.VarChar).Value = objStackInquiry.whs_id;
                command.Parameters.Add("@DateFrom", SqlDbType.VarChar).Value = objStackInquiry.Date_fm;
                command.Parameters.Add("@DateTo", SqlDbType.VarChar).Value = objStackInquiry.Date_to;
                command.Parameters.Add("@Style", SqlDbType.VarChar).Value = objStackInquiry.itm_num;
                command.Parameters.Add("@Color", SqlDbType.VarChar).Value = objStackInquiry.itm_color;
                command.Parameters.Add("@Size", SqlDbType.VarChar).Value = objStackInquiry.itm_size;
                command.Parameters.Add("@Desc", SqlDbType.VarChar).Value = objStackInquiry.itm_name;
                command.Parameters.Add("@style_stearch", SqlDbType.VarChar).Value = objStackInquiry.style_stearch;

                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetStockSmryByStyleRpt(StockInquiryDtl objStackInquiry)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spIVStkSummaryByStyle", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cmp_ID", SqlDbType.VarChar).Value = objStackInquiry.cmp_id;
                command.Parameters.Add("@Container", SqlDbType.VarChar).Value = objStackInquiry.cont_id;
                command.Parameters.Add("@LocID", SqlDbType.VarChar).Value = objStackInquiry.loc_id;
                command.Parameters.Add("@WareHouse", SqlDbType.VarChar).Value = objStackInquiry.whs_id;
                command.Parameters.Add("@DateFrom", SqlDbType.VarChar).Value = objStackInquiry.Date_fm;
                command.Parameters.Add("@DateTo", SqlDbType.VarChar).Value = objStackInquiry.Date_to;
                command.Parameters.Add("@Style", SqlDbType.VarChar).Value = objStackInquiry.itm_num;
                command.Parameters.Add("@Color", SqlDbType.VarChar).Value = objStackInquiry.itm_color;
                command.Parameters.Add("@Size", SqlDbType.VarChar).Value = objStackInquiry.itm_size;
                command.Parameters.Add("@Desc", SqlDbType.VarChar).Value = objStackInquiry.itm_name;
                command.Parameters.Add("@style_stearch", SqlDbType.VarChar).Value = objStackInquiry.style_stearch;

                connection.Open();
                DataTable dtBill = new DataTable();
                dtBill.Load(command.ExecuteReader());
                return dtBill;
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
