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
    public class UserProfileRepository: IUserProfileRepository
    {
        public UserProfile CheckExistUserID(UserProfile ObjUserProfile)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "sp_ma_check_user_id_exist";
                    IEnumerable<UserProfile> ListUserDetailsLST = connection.Query<UserProfile>(storedProcedure2,
                        new
                        {
                            @P_STR_USER_ID = ObjUserProfile.user_id,
                            @P_STR_PWD = ObjUserProfile.old_pwd,
                        },
                        commandType: CommandType.StoredProcedure);
                    ObjUserProfile.ListCheckUserIdExist = ListUserDetailsLST.ToList();

                    return ObjUserProfile;
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
        public UserProfile UpdateUserAccPwd(UserProfile ObjUserProfile)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_ma_update_user_acc_pwd";
                    IEnumerable<UserProfile> ListUserDetailsLST = connection.Query<UserProfile>(storedProcedure2,
                        new
                        {
                            @P_STR_USER_ID = ObjUserProfile.user_id,
                            @P_STR_OLD_PWD = ObjUserProfile.old_pwd,
                            @P_STR_NEW_PWD= ObjUserProfile.new_pwd,
                            @P_STR_PWD_CHNG_DATE = DateTime.Now.ToString("MM/dd/yyyy"),
                        },
                        commandType: CommandType.StoredProcedure);
                    ObjUserProfile.ListCheckUserIdExist = ListUserDetailsLST.ToList();

                    return ObjUserProfile;
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
