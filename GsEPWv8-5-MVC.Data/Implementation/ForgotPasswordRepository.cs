using Dapper;
using GsEPWv8_5_MVC.Core;
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
    public class ForgotPasswordRepository: IForgotPasswordRepository
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
        public ForgotPassword CheckUserIDExist(ForgotPassword ObjForgotPassword)
        {
            try
            {
              
                const string storedProcedure2 = "sp_ma_check_user_id_exist";
                connection.Open();
                IEnumerable<ForgotPassword> ListUserDetailsLST = connection.Query<ForgotPassword>(storedProcedure2,
                new
                {
                    @P_STR_USER_ID = ObjForgotPassword.user_id,
                    @P_STR_PWD = "",
                },
                commandType: CommandType.StoredProcedure);
                ObjForgotPassword.ListCheckUserIDExist = ListUserDetailsLST.ToList();

                return ObjForgotPassword;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public ForgotPassword UpdateUserAccPwd(ForgotPassword ObjForgotPassword)
        {
            try
            {
                
                    const string storedProcedure2 = "sp_ma_update_user_acc_pwd";
                    IEnumerable<ForgotPassword> ListUserDetailsLST = connection.Query<ForgotPassword>(storedProcedure2,
                        new
                        {
                            @P_STR_USER_ID = ObjForgotPassword.user_id,
                            @P_STR_OLD_PWD = "",
                            @P_STR_NEW_PWD = ObjForgotPassword.new_pwd,
                            @P_STR_PWD_CHNG_DATE = DateTime.Now.ToString("MM/dd/yyyy"),
                        },
                        commandType: CommandType.StoredProcedure);
                    ObjForgotPassword.ListCheckUserIDExist = ListUserDetailsLST.ToList();

                    return ObjForgotPassword;
                
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
