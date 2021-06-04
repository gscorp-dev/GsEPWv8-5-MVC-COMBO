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
    public class InboundReportRepository: IInboundReportRepository
    {

        public InboundReport GetInboundRpt(InboundReport objInboundReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objInboundReport.cmp_id;                  
                    InboundReport objOrderLifeCycleCategory = new InboundReport();

                    const string storedProcedure2 = "proc_get_webmvc_Inbound_receive_Report_Style_Inq";
                    IEnumerable<InboundReport> ListInbound = connection.Query<InboundReport>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundReport.cmp_id,
                            @WhsID = objInboundReport.whs_id,
                            @IBDocNumFm = objInboundReport.ib_doc_idFm,
                            @IBDocNumTo = objInboundReport.ib_doc_idTo,
                            @InboundRcvdDtFm = objInboundReport.RcvdDtFm,
                            @InboundRcvdDtTo = objInboundReport.RcvdDtTo,
                            @p_str_cntr_id = objInboundReport.cont_id,
                            @p_str_rcvd_status = objInboundReport.rcvd_status,
                            @p_str_bill_status = objInboundReport.bill_status,
                            @p_str_itm_num = objInboundReport.itm_num,
                            @p_str_itm_color = objInboundReport.itm_color,
                            @p_str_itm_size = objInboundReport.itm_size,
                            @p_str_itm_name = objInboundReport.itm_num,
                            @p_str_status = objInboundReport.itm_search_with

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundReport.LstinboundRpt = ListInbound.ToList();

                    return objInboundReport;
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
        public InboundReport GetInboundRptStyle(InboundReport objInboundReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objInboundReport.cmp_id;
                    InboundReport objOrderLifeCycleCategory = new InboundReport();

                    const string storedProcedure2 = "SP_MVC_IB_RECEIVE_REPORT_BY_STYLE";
                    IEnumerable<InboundReport> ListInbound = connection.Query<InboundReport>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundReport.cmp_id,
                            @p_str_cntr_id = objInboundReport.cntr_id,
                            @WhsID = objInboundReport.whs_id,
                            @IBDocNumFm = objInboundReport.ib_doc_idFm,
                            @IBDocNumTo = objInboundReport.ib_doc_idTo,
                            @InboundRcvdDtFm = objInboundReport.RcvdDtFm,
                            @InboundRcvdDtTo = objInboundReport.RcvdDtTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundReport.LstinboundRpt = ListInbound.ToList();

                    return objInboundReport;
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

        public InboundReport GetInboundRptDate(InboundReport objInboundReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objInboundReport.cmp_id;
                    InboundReport objOrderLifeCycleCategory = new InboundReport();

                    const string storedProcedure2 = "SP_MVC_IB_RECEIVE_REPORT_BY_DATE";
                    IEnumerable<InboundReport> ListInbound = connection.Query<InboundReport>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objInboundReport.cmp_id,
                            @p_str_cntr_id = objInboundReport.cntr_id,
                            @WhsID = objInboundReport.whs_id,
                            @IBDocNumFm = objInboundReport.ib_doc_idFm,
                            @IBDocNumTo = objInboundReport.ib_doc_idTo,
                            @InboundRcvdDtFm = objInboundReport.RcvdDtFm,
                            @InboundRcvdDtTo = objInboundReport.RcvdDtTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundReport.LstinboundRpt = ListInbound.ToList();

                    return objInboundReport;
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

        public IBRcvdRptByCntrDtl GetIBRcvdRptByCntrDtl(IBRcvdRptByCntrDtl objIBRcvdRptByCntrDtl)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "sp_ib_rcv_rpt_by_cntr";
                    IEnumerable<IBRcvdRptByCntr> ListInboundReceivingTypeList = connection.Query<IBRcvdRptByCntr>(storedProcedure1,
                        new
                        {
                            @cmp_id = objIBRcvdRptByCntrDtl.cmp_id,
                            @whs_id = objIBRcvdRptByCntrDtl.whs_id,
                            @cntr_id = objIBRcvdRptByCntrDtl.cntr_id,
                            @p_str_rcvd_dt_from = objIBRcvdRptByCntrDtl.rcvd_dt_from,
                            @p_str_rcvd_dt_to = objIBRcvdRptByCntrDtl.rcvd_dt_to

                        },
                        commandType: CommandType.StoredProcedure);
                    objIBRcvdRptByCntrDtl.ListIBRcvdRptByCntr = ListInboundReceivingTypeList.ToList();
                }

                return objIBRcvdRptByCntrDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public InboundReport InboundWhsDetails(string term, string cmpid)
        {
            try
            {
                InboundReport objInboundReport = new InboundReport();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchShipHdrDtls = "proc_get_webmvc_ship_from_hdr";
                    IList<InboundReport> ListWhs = connection.Query<InboundReport>(SearchShipHdrDtls, new
                    {    
                        @Cmp_ID = cmpid,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objInboundReport.LstWhsDetails = ListWhs.ToList();
                }
                return objInboundReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundRcvngRptByStyleExcel GetInboundRcvngRptByStyleExcel(InboundRcvngRptByStyleExcel objInboundReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    //var cmp_id = objInboundReport.cmp_id;
               

                    const string storedProcedure2 = "proc_get_mvcweb_ib_excel_receiving_rpt_style";
                    IEnumerable<InboundRcvngRptByStyleExcel> ListInbound = connection.Query<InboundRcvngRptByStyleExcel>(storedProcedure2,
                        new
                        {
                            @strCmpId = objInboundReport.Container_ID,
                            @strWhsId = objInboundReport.WHS,
                            @strDateFm = objInboundReport.Rcvd_Date,
                            @strDateTo = objInboundReport.Loc_Id,
                            @strDocIdFm = objInboundReport.IB_Doc_ID,
                            @strDocIdTo = objInboundReport.Lot_Id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundReport.ListInboundRcvngRptByStyleExcel = ListInbound.ToList();

                    return objInboundReport;
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

        public InboundRcvngRptByDateExcel GetInboundRcvngRptByDateExcel(InboundRcvngRptByDateExcel objInboundReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    //var cmp_id = objInboundReport.cmp_id;
                    InboundRcvngRptByDateExcel objOrderLifeCycleCategory = new InboundRcvngRptByDateExcel();

                    const string storedProcedure2 = "proc_get_mvcweb_ib_Receiving_rpt_date";
                    IEnumerable<InboundRcvngRptByDateExcel> ListInbound = connection.Query<InboundRcvngRptByDateExcel>(storedProcedure2,
                        new
                        {
                            @strCmpId = objInboundReport.Container_ID,
                            @strWhsId = objInboundReport.WHS,
                            @strDateFm = objInboundReport.Rcvd_Date,
                            @strDateTo = objInboundReport.Loc_Id,
                            @strDocIdFm = objInboundReport.IB_Doc_ID,
                            @strDocIdTo = objInboundReport.Lot_Id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundReport.ListInboundRcvngRptByDateExcel = ListInbound.ToList();

                    return objInboundReport;
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
        public InboundReport GetDftWhs(InboundReport objInboundReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    InboundReport objOutboundInqCategory = new InboundReport();

                    const string storedProcedure2 = "SP_MVC_GET_DFTWHS";
                    IEnumerable<InboundReport> ListInq = connection.Query<InboundReport>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objInboundReport.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundReport.LstWhsDetails = ListInq.ToList();

                    return objInboundReport;
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
        public DataTable GetInboundRptDateExcel(string l_str_cmp_id, string p_str_cntr_id, string l_str_whs_id, string l_str_IBDocNumFm, string l_str_IBDocNumTo, 
            string l_InboundRcvdDtFm, string l_InboundRcvdDtTo, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_itm_name, string p_str_status)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_ib_Receiving_rpt_date", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@strCmpId", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@p_str_cntr_id", SqlDbType.VarChar).Value = p_str_cntr_id;
                command.Parameters.Add("@strWhsId", SqlDbType.VarChar).Value = l_str_whs_id;
                command.Parameters.Add("@strDateFm", SqlDbType.VarChar).Value = l_InboundRcvdDtFm;
                command.Parameters.Add("@strDateTo", SqlDbType.VarChar).Value = l_InboundRcvdDtTo;
                command.Parameters.Add("@strDocIdFm", SqlDbType.VarChar).Value = l_str_IBDocNumFm;
                command.Parameters.Add("@strDocIdTo", SqlDbType.VarChar).Value = l_str_IBDocNumTo;
                command.Parameters.Add("@p_str_itm_num", SqlDbType.VarChar).Value = p_str_itm_num;
                command.Parameters.Add("@p_str_itm_color", SqlDbType.VarChar).Value = p_str_itm_color;
                command.Parameters.Add("@p_str_itm_size", SqlDbType.VarChar).Value = p_str_itm_size;
                command.Parameters.Add("@p_str_itm_name", SqlDbType.VarChar).Value = p_str_itm_name;
                command.Parameters.Add("@p_str_status", SqlDbType.VarChar).Value = p_str_status;
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

        public DataTable GetIBSummaryRpt(InboundReport objInboundReport)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_rcvd_summary", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = objInboundReport.cmp_id;
                command.Parameters.Add("@p_str_whs_id", SqlDbType.VarChar).Value = objInboundReport.whs_id;
                command.Parameters.Add("@p_str_ib_doc_id_from", SqlDbType.VarChar).Value = objInboundReport.ib_doc_idFm;
                command.Parameters.Add("@p_str_ib_doc_id_to", SqlDbType.VarChar).Value = objInboundReport.ib_doc_idTo;
                command.Parameters.Add("@p_str_ib_rcvd_from", SqlDbType.VarChar).Value = objInboundReport.RcvdDtFm;
                command.Parameters.Add("@p_str_ib_rcvd_to", SqlDbType.VarChar).Value = objInboundReport.RcvdDtTo;
                command.Parameters.Add("@p_str_cntr_id", SqlDbType.VarChar).Value = objInboundReport.cont_id;
                command.Parameters.Add("@p_str_rcvd_status", SqlDbType.VarChar).Value = objInboundReport.rcvd_status;
                command.Parameters.Add("@p_str_bill_status", SqlDbType.VarChar).Value = objInboundReport.bill_status;
                connection.Open();
                DataTable dtGetInboundSummary = new DataTable();
                dtGetInboundSummary.Load(command.ExecuteReader());
                command.Dispose();
                connection.Close();
                connection.Dispose();
                return dtGetInboundSummary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public DataTable GetInboundRptStyleExcel(string l_str_cmp_id, string p_str_cntr_id, string l_str_whs_id, string l_str_IBDocNumFm, string l_str_IBDocNumTo,
            string l_InboundRcvdDtFm, string l_InboundRcvdDtTo, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_itm_name, string p_str_status)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_mvcweb_ib_excel_receiving_rpt_style", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@strCmpId", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@p_str_cntr_id", SqlDbType.VarChar).Value = p_str_cntr_id;
                command.Parameters.Add("@strWhsId", SqlDbType.VarChar).Value = l_str_whs_id;
                command.Parameters.Add("@strDateFm", SqlDbType.VarChar).Value = l_InboundRcvdDtFm;
                command.Parameters.Add("@strDateTo", SqlDbType.VarChar).Value = l_InboundRcvdDtTo;
                command.Parameters.Add("@strDocIdFm", SqlDbType.VarChar).Value = l_str_IBDocNumFm;
                command.Parameters.Add("@strDocIdTo", SqlDbType.VarChar).Value = l_str_IBDocNumTo;
                command.Parameters.Add("@p_str_itm_num", SqlDbType.VarChar).Value = p_str_itm_num;
                command.Parameters.Add("@p_str_itm_color", SqlDbType.VarChar).Value = p_str_itm_color;
                command.Parameters.Add("@p_str_itm_size", SqlDbType.VarChar).Value = p_str_itm_size;
                command.Parameters.Add("@p_str_itm_name", SqlDbType.VarChar).Value = p_str_itm_name;
                command.Parameters.Add("@p_str_status", SqlDbType.VarChar).Value = p_str_status;

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
        public DataTable GetInboundRptContainerExcel(string l_str_cmp_id, string l_str_whs_id, string l_str_cntr_id, string l_str_rcvd_dt_from, string l_str_rcvd_dt_to)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ib_rcv_rpt_by_cntr", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@cmp_id", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@whs_id", SqlDbType.VarChar).Value = l_str_whs_id;
                command.Parameters.Add("@cntr_id", SqlDbType.VarChar).Value = l_str_cntr_id;
                command.Parameters.Add("@p_str_rcvd_dt_from", SqlDbType.VarChar).Value = l_str_rcvd_dt_from;
                command.Parameters.Add("@p_str_rcvd_dt_to", SqlDbType.VarChar).Value = l_str_rcvd_dt_to;
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
