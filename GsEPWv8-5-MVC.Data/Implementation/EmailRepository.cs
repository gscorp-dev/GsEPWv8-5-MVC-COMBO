using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using System.Data;
using GsEPWv8_5_MVC.Data.Interface;
namespace GsEPWv8_5_MVC.Data.Implementation
{
   public class EmailRepository:IEmailRepository
    {
        public Email GetSendMailDetails(Email objEmail)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Email objOrderLifeCycleCategory = new Email();

                    const string storedProcedure2 = "proc_get_mvcweb_get_email_detail";
                    IEnumerable<Email> ListEmail = connection.Query<Email>(storedProcedure2,
                        new
                        {
                            @pstr_cmp_id = objEmail.CmpId,
                            @p_str_scn_id = objEmail.screenId,
                            @p_str_rpt_description= objEmail.Reportselection,
                            
                        },
                        commandType: CommandType.StoredProcedure);
                    objEmail.ListEamilDetail = ListEmail.ToList();

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
        public Email GetMailDetails(Email objEmail)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Email objOrderLifeCycleCategory = new Email();

                    const string storedProcedure2 = "sp_get_email_list";
                    IEnumerable<Email> ListEmail = connection.Query<Email>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objEmail.CmpId,
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
    }
}
