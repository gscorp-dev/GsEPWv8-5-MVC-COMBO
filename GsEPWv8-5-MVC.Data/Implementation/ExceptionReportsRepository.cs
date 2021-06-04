using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class ExceptionReportsRepository: IExceptionReportsRepository
    {
        public ExceptionReports GetExceptionReportsInquiryDetails(ExceptionReports objExceptionReports)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderLifeCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IV_RECON_OB_FETCH";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_SO_DT_FROM = objExceptionReports.SRDtFm,
                            @P_STR_SO_DT_TO = objExceptionReports.SRDtTo,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstExceptionRpt = ListBilling.ToList();

                    return objExceptionReports;
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
        public ExceptionReports GetExceptionReportsInquiryDetailsRpt(ExceptionReports objExceptionReports)
        {
            try
            {
                if(objExceptionReports.cmp_id=="ALL")
                {
                    objExceptionReports.cmp_id = "";
                }
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderLifeCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IV_RECON_OB_FETCH_RPT";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_SO_DT_FROM = objExceptionReports.SRDtFm,
                            @P_STR_SO_DT_TO = objExceptionReports.SRDtTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstExceptionRpt = ListBilling.ToList();

                    return objExceptionReports;
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
        public ExceptionReports GetIBExceptionReportsInquiryDetails(ExceptionReports objExceptionReports)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IB_RECON_FETCH";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_RCVD_DT_FROM = objExceptionReports.RcvdDtFm,
                            @P_STR_RCVD_DT_TO = objExceptionReports.RcvdDtTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstIBExceptionRpt = ListBilling.ToList();

                    return objExceptionReports;
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
        public ExceptionReports GetIBExceptionReportsInquiryDetailsRpt(ExceptionReports objExceptionReports)
        {
            try
            {
                if (objExceptionReports.cmp_id == "ALL")
                {
                    objExceptionReports.cmp_id = "";
                }
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderLifeCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IB_RECON_FETCH_RPT";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_RCVD_DT_FROM = objExceptionReports.RcvdDtFm,
                            @P_STR_RCVD_DT_TO = objExceptionReports.RcvdDtTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstIBExceptionRpt = ListBilling.ToList();

                    return objExceptionReports;
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
        public ExceptionReports ItemXGetitmDetails(string term, string cmp_id)
        {
            try
            {
                ExceptionReports objExceptionReports = new ExceptionReports();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_get_webmvc_Itm_dtl";
                    IList<ExceptionReports> ListIRFP = connection.Query<ExceptionReports>(SearchCustDtls, new
                    {

                        @cmp_id = cmp_id,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objExceptionReports.LstItmDtl = ListIRFP.ToList();
                }
                return objExceptionReports;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ExceptionReports GetIVExceptionReportsInqdtl(ExceptionReports objExceptionReports)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderLifeCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IV_RECON_FETCH";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_ITM_CODE = objExceptionReports.ITM_CODE,
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstIVExceptionRpt = ListBilling.ToList();

                    return objExceptionReports;
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
        public ExceptionReports GetIVExceptionReportsInqdtlRpt(ExceptionReports objExceptionReports)
        {
            try
            {
                if (objExceptionReports.cmp_id == "ALL")
                {
                    objExceptionReports.cmp_id = "";
                }
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderLifeCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IV_RECON_FETCH_RPT";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_ITM_CODE = objExceptionReports.ITM_CODE,
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstIVExceptionRpt = ListBilling.ToList();
                    return objExceptionReports;
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
        public ExceptionReports GetIBExceptionReport(ExceptionReports objExceptionReports)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IB_RECON_FETCH_NEW";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_RCVD_DT_FROM = objExceptionReports.RcvdDtFm,
                            @P_STR_RCVD_DT_TO = objExceptionReports.RcvdDtTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstIBExceptionRpt = ListBilling.ToList();

                    return objExceptionReports;
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
        public ExceptionReports GetOBExceptionReport(ExceptionReports objExceptionReports)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IV_RECON_OB_FETCH_NEW";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_RCVD_DT_FROM = objExceptionReports.RcvdDtFm,
                            @P_STR_RCVD_DT_TO = objExceptionReports.RcvdDtTo,
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstOBExceptionRpt = ListBilling.ToList();

                    return objExceptionReports;
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

        public ExceptionReports GetIVExceptionReport(ExceptionReports objExceptionReports)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ExceptionReports objOrderCycleCategory = new ExceptionReports();

                    const string storedProcedure2 = "SP_IV_RECON_FETCH_NEW";
                    IEnumerable<ExceptionReports> ListBilling = connection.Query<ExceptionReports>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objExceptionReports.cmp_id,
                            @P_STR_ITM_CODE = objExceptionReports.ITM_CODE,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objExceptionReports.LstIVExceptionRpt = ListBilling.ToList();

                    return objExceptionReports;
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
