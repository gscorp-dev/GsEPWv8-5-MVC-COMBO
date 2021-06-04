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
   public class DailyFlashExceptionReportRepository: IDailyFlashExceptionReportRepository
    {
        public DailyFlashExceptionReport GetExceptionReportName(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_GET_DF_SCHEDULE_JOB_REPORT_NAME";
                    IEnumerable<DailyFlashExceptionReport> ListBilling = connection.Query<DailyFlashExceptionReport>(storedProcedure2,
                        new
                        {
                            @P_STR_SCN_ID=objDailyFlashExceptionReport.scn_id,
                            @P_STR_RPT_ID=objDailyFlashExceptionReport.rpt_id,
                            @ISDAILYFLASH =objDailyFlashExceptionReport.is_daily_flash,
                        },
                        commandType: CommandType.StoredProcedure);
                    objDailyFlashExceptionReport.ListReportName = ListBilling.ToList();

                    return objDailyFlashExceptionReport;
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

        public DailyFlashExceptionReport GetExceptionReportDetails(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_DF_GET_SCHEDULE_JOB_DTL";
                    IEnumerable<DailyFlashExceptionReport> ListBilling = connection.Query<DailyFlashExceptionReport>(storedProcedure2,
                        new
                        {
                            @P_STR_COMPID = objDailyFlashExceptionReport.cmp_id,
                            @P_STR_RPTID = objDailyFlashExceptionReport.rpt_id,
                            @P_STR_EMAILTO = objDailyFlashExceptionReport.email_to,
                            @P_STR_EMAILCC=objDailyFlashExceptionReport.email_cc,
                            @P_STR_MESSAGE=objDailyFlashExceptionReport.email_msg,

                        },
                        commandType: CommandType.StoredProcedure);
                    objDailyFlashExceptionReport.ListExceptionRptDetails = ListBilling.ToList();

                    return objDailyFlashExceptionReport;
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

        public DailyFlashExceptionReport GetExistingJobDetails(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_DF_GET_EXISTING_SCHEDULE_JOB_DTL";
                    IEnumerable<DailyFlashExceptionReport> ListBilling = connection.Query<DailyFlashExceptionReport>(storedProcedure2,
                        new
                        {
                            @P_STR_COMPID = objDailyFlashExceptionReport.cmp_id,
                            @P_STR_RPTID = objDailyFlashExceptionReport.rpt_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objDailyFlashExceptionReport.ListExceptionRptDetails = ListBilling.ToList();

                    return objDailyFlashExceptionReport;
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
        public DailyFlashExceptionReport InsertExceptionReportDetails(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            try
            {
                if (objDailyFlashExceptionReport.cmp_id == "" || objDailyFlashExceptionReport.cmp_id == null)
                {
                    objDailyFlashExceptionReport.cmp_id = "ALL";
                }
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_DF_SCHEDULE_JOB_SAVE";
                    IEnumerable<DailyFlashExceptionReport> ListBilling = connection.Query<DailyFlashExceptionReport>(storedProcedure2,
                        new
                        {
                            @P_STR_COMPID = objDailyFlashExceptionReport.cmp_id,
                            @P_STR_RPTID = objDailyFlashExceptionReport.rpt_id,
                            @P_STR_DAYS = objDailyFlashExceptionReport.rpt_run_days,
                            @P_STR_TIME = objDailyFlashExceptionReport.rpt_run_time,
                            @P_STR_EMAILTO = objDailyFlashExceptionReport.email_to,
                            @P_STR_EMAILCC = objDailyFlashExceptionReport.email_cc,
                            @P_STR_MESSAGE = objDailyFlashExceptionReport.email_msg,
                            @P_STR_FORMAT= objDailyFlashExceptionReport.dflt_frmt,
                            @P_STR_STATUS=objDailyFlashExceptionReport.Status,
                            @P_STR_ACTION=objDailyFlashExceptionReport.action
                           

                        },
                        commandType: CommandType.StoredProcedure);
                    objDailyFlashExceptionReport.ListSaveExceptionRptDetails = ListBilling.ToList();

                    return objDailyFlashExceptionReport;
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


        public DailyFlashExceptionReport InsertExceptionReportMailConfigDetails(DailyFlashExceptionReport objDailyFlashExceptionReport)
        {
            try
            {
                if(objDailyFlashExceptionReport.cmp_id=="" || objDailyFlashExceptionReport.cmp_id== null)
                {
                    objDailyFlashExceptionReport.cmp_id = "ALL";
                }
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure3 = "proc_get_mvcweb_Insert_email_detail";
                    IEnumerable<DailyFlashExceptionReport> ListInq1 = connection.Query<DailyFlashExceptionReport>(storedProcedure3,
                         new
                        {
                            @cmp_id = objDailyFlashExceptionReport.cmp_id,
                            @scn_id = objDailyFlashExceptionReport.scn_id,
                            @rpt_id = objDailyFlashExceptionReport.rpt_id,
                            @rpt_name = objDailyFlashExceptionReport.rpt_name,
                            @rpt_description = objDailyFlashExceptionReport.rpt_description,
                            @emailto = objDailyFlashExceptionReport.email_to,
                            @emailcc = objDailyFlashExceptionReport.email_cc,
                            @emailmessage = objDailyFlashExceptionReport.email_msg,
                            @status = objDailyFlashExceptionReport.Status,
                            @action = objDailyFlashExceptionReport.action,

                         },
                        commandType: CommandType.StoredProcedure);
                    objDailyFlashExceptionReport.ListSaveExceptionRptDetails = ListInq1.ToList();
                  

                }

            }

            catch (Exception Ex)
            {

            }
            finally
            {

            }
            return objDailyFlashExceptionReport;

        }

       

    }
}
