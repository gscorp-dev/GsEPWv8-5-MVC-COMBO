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
   public class MailConfigRepository:IMailConfigRepository
    {
        public MailConfig GetReportDetails(MailConfig objMailConfig)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                   
                    const string storedProcedure3 = "proc_get_mvcweb_get_mail_config_report_detail";
                    IEnumerable<MailConfig> ListInq1 = connection.Query<MailConfig>(storedProcedure3,
                        new
                        {
                            @pstr_cmp_id = objMailConfig.cmp_id,
                            @pstr_scn_id = objMailConfig.scn_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objMailConfig.LstMailConfig = ListInq1.ToList();

                }

            }

            catch (Exception Ex)
            {
            }
            finally
            {
            }
            return objMailConfig;
        }
        public MailConfig GetReportName(MailConfig objMailConfig)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure3 = "proc_get_mvcweb_get_report_name_detail";
                    IEnumerable<MailConfig> ListInq1 = connection.Query<MailConfig>(storedProcedure3,
                        new
                        {
                            @pstr_cmp_id = objMailConfig.cmp_id,
                            @pstr_scn_id = objMailConfig.scn_id,
                            @pstr_rpt_id=objMailConfig.rpt_id



                        },
                        commandType: CommandType.StoredProcedure);
                    objMailConfig.LstMailConfigReportID = ListInq1.ToList();

                }

            }

            catch (Exception Ex)
            {

            }
            finally
            {

            }
            return objMailConfig;
        }
        public MailConfig SaveMailConfigDetails(MailConfig objMailConfig)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure3 = "proc_get_mvcweb_Insert_email_detail";
                    IEnumerable<MailConfig> ListInq1 = connection.Query<MailConfig>(storedProcedure3,
                        new
                        {
                            @cmp_id = objMailConfig.cmp_id,
                            @scn_id = objMailConfig.scn_id,
                            @rpt_id = objMailConfig.rpt_id,
                            @rpt_name = objMailConfig.rpt_name,
                            @rpt_description=objMailConfig.rpt_description,
                            @emailto = objMailConfig.EmailTo,
                            @emailcc = objMailConfig.EmailCC,
                            @emailmessage = objMailConfig.emailbody,
                            @status= objMailConfig.Status,
                            @action=objMailConfig.action,

                        },
                        commandType: CommandType.StoredProcedure);
                    objMailConfig.LstSaveMailConfig = ListInq1.ToList();

                }

            }

            catch (Exception Ex)
            {

            }
            finally
            {

            }
            return objMailConfig;
        }

        public MailConfig GetMailConfigDetails(MailConfig objMailConfig)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure3 = "proc_get_mvcweb_get_email_all_detail";
                    IEnumerable<MailConfig> ListInq1 = connection.Query<MailConfig>(storedProcedure3,
                        new
                        {
                            @pstr_cmp_id = objMailConfig.cmp_id,
                            @p_str_scn_id=objMailConfig.scn_id,
                            @p_str_rpt_name=objMailConfig.rpt_name,
                            @p_str_emailto =objMailConfig.EmailTo,
                            @p_str_emailcc=objMailConfig.EmailCC
                        },
                        commandType: CommandType.StoredProcedure);
                    objMailConfig.LstMail = ListInq1.ToList();

                }

            }

            catch (Exception Ex)
            {

            }
            finally
            {

            }
            return objMailConfig;
        }
        public MailConfig GetExistingMailCount(MailConfig objMailConfig)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure3 = "proc_get_mvcweb_get_existing_email_detail";
                    IEnumerable<MailConfig> ListInq1 = connection.Query<MailConfig>(storedProcedure3,
                        new
                        {
                            @cmp_id = objMailConfig.cmp_id,
                            @scn_id = objMailConfig.scn_id,
                            @rpt_id = objMailConfig.rpt_id,
                            @rpt_name= objMailConfig.rpt_name,
                            @rpt_description = objMailConfig.rpt_description,

                        },
                        commandType: CommandType.StoredProcedure);
                    objMailConfig.LstSavedMailCount = ListInq1.ToList();

                }

            }

            catch (Exception Ex)
            {

            }
            finally
            {

            }
            return objMailConfig;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objMailConfig"></param>
        /// <returns></returns>
        public MailConfig GetUsersEmailList(MailConfig objMailConfig)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure3 = "SP_MVC_GET_USERS_MAIL_LIST";
                    IEnumerable<MailConfig> ListInq1 = connection.Query<MailConfig>(storedProcedure3,
                    new { },
                    commandType: CommandType.StoredProcedure);
                    objMailConfig.LstUsersMail = ListInq1.ToList();
                }
            }
            catch (Exception Ex)
            {
            }
            finally
            {
            }
            return objMailConfig;
        }
    }
}
