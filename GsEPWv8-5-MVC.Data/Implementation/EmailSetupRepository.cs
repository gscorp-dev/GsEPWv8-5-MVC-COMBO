using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using System.Data;
using GsEPWv8_5_MVC.Data.Interface;
using System.Configuration;
using System.Data.SqlClient;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class EmailSetupRepository : IEmailSetupRepository
    {
       public EmailSetup fnGetEmailList(string pstrCmpId, string pstrUserType)
        {
           
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    EmailSetup objEmailSetup = new EmailSetup();
                    string lstrSpName = "spGetEmailList";
                    IEnumerable<clsRptEmailDtl> ListsRptEmailDtl = connection.Query<clsRptEmailDtl>(lstrSpName,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrIsInternal = pstrUserType
                        },
                        commandType: CommandType.StoredProcedure);
                    objEmailSetup.lstRptEmailDtl = ListsRptEmailDtl.ToList();
                    
                    return objEmailSetup;
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

        public EmailSetup fnGetEmailCommon(string pstrCmpId)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    EmailSetup objEmailSetup = new EmailSetup();
                    string lstrSpName = "spGetRptEmailCommon";
                    IEnumerable<EmailCommon> ListsEmailCommon = connection.Query<EmailCommon>(lstrSpName,
                        new
                        {
                            @pstrCmpId = pstrCmpId
                        },
                        commandType: CommandType.StoredProcedure);

                   
                    if (ListsEmailCommon.ToList().Count > 0)
                    {
                        objEmailSetup.objEmailCommon = ListsEmailCommon.ToList()[0];
                    }

                    return objEmailSetup;
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

        public bool fnSaveEmailList(string pstrCmpId, List<clsRptEmailDtl> lstRptEmailDt, EmailCommon objEmailCommon)
        {
            string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "spSaveRptEmailCommon";
                            cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                            cmd.Parameters.Add("@pstrEmailFrom", SqlDbType.VarChar).Value = objEmailCommon.emailFrom;
                            cmd.Parameters.Add("@pstrCmpEmailTo", SqlDbType.VarChar).Value = objEmailCommon.cmpEmailTo;
                            cmd.Parameters.Add("@pstrCustEmailTo", SqlDbType.VarChar).Value = objEmailCommon.custEmailTo;
                            cmd.Parameters.Add("@pstrEmailCC", SqlDbType.VarChar).Value = objEmailCommon.emailCC;
                            cmd.Parameters.Add("@pstrEmailMessage", SqlDbType.VarChar).Value = objEmailCommon.emailMessage;
                            cmd.Parameters.Add("@pstrEmailFooter", SqlDbType.VarChar).Value = objEmailCommon.emailFooter;
                            cmd.Connection = conn;
                            cmd.Transaction = tran as SqlTransaction;
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
                using (IDbTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                    
                    if (lstRptEmailDt != null)
                    {


                        for (int i = 0; i < lstRptEmailDt.Count; i++)
                        {

                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "spSaveEmailList";
                                cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                                cmd.Parameters.Add("@pstrModuleName", SqlDbType.VarChar).Value = lstRptEmailDt[i].module_name;
                                cmd.Parameters.Add("@pstrRptId", SqlDbType.VarChar).Value = lstRptEmailDt[i].rpt_id;
                                cmd.Parameters.Add("@pstrRptFileFormat", SqlDbType.VarChar).Value = lstRptEmailDt[i].rpt_file_format;
                                cmd.Parameters.Add("@pstrCmpEmailList", SqlDbType.VarChar).Value = lstRptEmailDt[i].cmp_email_list;
                                cmd.Parameters.Add("@pstrCustEmailList", SqlDbType.VarChar).Value = lstRptEmailDt[i].cust_email_list;
                                cmd.Parameters.Add("@pstrEmailBody", SqlDbType.VarChar).Value = lstRptEmailDt[i].email_body;
                                cmd.Parameters.Add("@pstrIsAutoEmail", SqlDbType.VarChar).Value = lstRptEmailDt[i].is_auto_email;
                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                            }
                        }
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
                return true;
           }

        public Email fnGetCmpEmailList(string pstrCmpId)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    Email objEmail = new Email();
                    const string storedProcedure2 = "spGetCmpEmailList";
                    IEnumerable<Email> ListEmail = connection.Query<Email>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                        },
                        commandType: CommandType.StoredProcedure);
                    objEmail.ListGetMail = ListEmail.ToList();

                    return objEmail;
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
        public Email fnGetCustEmailList(string pstrCmpId)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    Email objEmail = new Email();
                    const string storedProcedure2 = "spGetCustEmailList";
                    IEnumerable<Email> ListEmail = connection.Query<Email>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                        },
                        commandType: CommandType.StoredProcedure);
                    objEmail.ListGetMail = ListEmail.ToList();

                    return objEmail;
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

        public EmailSetup GetEmailReportDetails(string pstrCmpId, string pstrModuleName, string pstrRptId)
        {

            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    EmailSetup objEmailSetup = new EmailSetup();
                    const string storedProcedure2 = "spGetEmailReportDetails";
                    IEnumerable<clsRptEmailDtl> ListsRptEmailDtl = connection.Query<clsRptEmailDtl>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrModuleName = pstrModuleName,
                            @pstrRptId = pstrRptId,
                        },
                        commandType: CommandType.StoredProcedure);
                    objEmailSetup.lstRptEmailDtl = ListsRptEmailDtl.ToList();

                    return objEmailSetup;
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
        public EmailAlert getEmailAlertDetails(string pstrCmpId, string pstrModuleName, string pstrRptId)
        {

            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    EmailAlert objEmailAlert = new EmailAlert();
                    const string storedProcedure2 = "spGetEmailAlertDetails";
                    IEnumerable<EmailAlertHdr> ListEmailAlertHdr = connection.Query<EmailAlertHdr>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrModuleName = pstrModuleName,
                            @pstrRptId = pstrRptId,
                        },
                        commandType: CommandType.StoredProcedure);
                    objEmailAlert.lstEmailAlertHdr = ListEmailAlertHdr.ToList();

                    return objEmailAlert;
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
