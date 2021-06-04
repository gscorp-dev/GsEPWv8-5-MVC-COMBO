using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class UserAccountRepository
    {
        public UserAccount GetUserDetails(UserAccount objUserAccount)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    UserAccount objOrderLifeCycleCategory = new UserAccount();

                    const string storedProcedure2 = "sp_ma_get_user_list";
                    IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_USER_ID = objUserAccount.user_id,
                            @P_STR_USER_FST_NAME = objUserAccount.user_fst_name,
                            @P_STR_USER_LST_NAME = objUserAccount.user_lst_name,
                        },
                        commandType: CommandType.StoredProcedure);
                    objUserAccount.ListUserDetails = ListUserDetailsLST.ToList();

                    return objUserAccount;
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
        public UserAccount CheckExistUserId(UserAccount objUserAccount)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    UserAccount objOrderLifeCycleCategory = new UserAccount();

                    const string storedProcedure2 = "sp_ma_check_usr_exist";
                    IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_USER_ID = objUserAccount.user_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objUserAccount.ListCheckUserIdExist = ListUserDetailsLST.ToList();

                    return objUserAccount;
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

        public UserAccount GetGridScnNameDetails(UserAccount objUserAccount)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    UserAccount objOrderLifeCycleCategory = new UserAccount();

                    const string storedProcedure2 = "sp_ma_get_scn_dtls";
                    IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_STATUS = objUserAccount.Status
                        },
                        commandType: CommandType.StoredProcedure);
                    objUserAccount.ListScnDtls = ListUserDetailsLST.ToList();

                    return objUserAccount;
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

        public UserAccount GetGridScnPermDetails(UserAccount objUserAccount)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    UserAccount objOrderLifeCycleCategory = new UserAccount();

                    const string storedProcedure2 = "sp_ma_get_usr_scn_perm_dtls";
                    IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_USER_ID = objUserAccount.user_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objUserAccount.ListScnPermDtls = ListUserDetailsLST.ToList();

                    return objUserAccount;
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

        public UserAccount GetDefaultCompanyDetails(UserAccount objUserAccount)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    UserAccount objOrderLifeCycleCategory = new UserAccount();

                    const string storedProcedure2 = "sp_ma_get_dflt_company_dtls";
                    IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure2,
                        new
                        {

                        },
                        commandType: CommandType.StoredProcedure);
                    objUserAccount.ListCmpDtls = ListUserDetailsLST.ToList();

                    return objUserAccount;
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

        public void SaveUserDtls(UserAccount objUserAccount)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_insert_usr_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @p_str_user_id = objUserAccount.user_id,
                        @p_str_pwd = objUserAccount.password,
                        @p_str_user_type = objUserAccount.usr_type,
                        @p_str_usr_fst_name = objUserAccount.first_name,
                        @p_str_usr_lst_name = objUserAccount.last_name,
                        @p_str_email = objUserAccount.email,
                        @p_str_tel = objUserAccount.tel,
                        @p_str_cmp_perm1 = objUserAccount.cmp_perm1,
                        @p_str_dflt_cust_id = objUserAccount.dflt_cust_id,
                        @P_STR_PWD_CHNG_DATE = DateTime.Now.ToString("MM/dd/yyyy"),

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateUserDtls(UserAccount objUserAccount)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_update_usr_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @p_str_user_id = objUserAccount.user_id,
                        @p_str_pwd = objUserAccount.password,
                        @p_str_user_type = objUserAccount.usr_type,
                        @p_str_usr_fst_name = objUserAccount.first_name,
                        @p_str_usr_lst_name = objUserAccount.last_name,
                        @p_str_email = objUserAccount.email,
                        @p_str_tel = objUserAccount.tel,
                        @p_str_cmp_perm1 = objUserAccount.cmp_perm1,
                        @p_str_dflt_cust_id = objUserAccount.dflt_cust_id,
                        @P_STR_PWD_CHNG_DATE= DateTime.Now.ToString("MM/dd/yyyy"),
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void DeleteUserDtls(UserAccount objUserAccount)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_delete_usr_scn_perm_dtls";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_STR_USER_ID = objUserAccount.user_id,

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void SaveUserPerm(UserAccount objUserAccount)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_insert_usr_scn_perm_dtls";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @p_str_user_id = objUserAccount.user_id,
                        @p_str_scn_id = objUserAccount.scn_id,
                        @p_str_view = objUserAccount.scn_view,
                        @p_str_add = objUserAccount.scn_add,
                        @p_str_mod = objUserAccount.scn_mod,
                        @p_str_del = objUserAccount.scn_del,
                        @p_str_post = objUserAccount.scn_post,
                        @p_str_unpost = objUserAccount.scn_unpost,

                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public UserAccount SaveCmpPerm(UserAccount objUserAccount)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_insert_usr_company_perm_dtls";
                IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure1,
                   new
                   {

                       @P_STR_USER_ID = objUserAccount.user_id,
                       @P_STR_USER_NAME = objUserAccount.first_name,
                       @P_STR_COMP_ID = objUserAccount.cmp_id,
                       @P_STR_COMP_NAME = objUserAccount.cmp_name,

                   },
                    commandType: CommandType.StoredProcedure);
                objUserAccount.ListCompanyPermissiondtl = ListUserDetailsLST.ToList();


            }
            return objUserAccount;
        }
        public UserAccount GetUserPermCompanyDetails(UserAccount objUserAccount)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "sp_ma_get_usr_company_perm_dtls";
                    IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure1,
                        new
                        {
                            @P_STR_USER_ID = objUserAccount.user_id,
                        },
                         commandType: CommandType.StoredProcedure);
                    objUserAccount.ListLoadUserDetails = ListUserDetailsLST.ToList();

                }

            }
            catch (Exception Ex)
            {
                objUserAccount.Exp = Ex.ToString();
            }
            return objUserAccount;
        }


        public void DeleteCmpPerm(UserAccount objUserAccount)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_delete_usr_company_perm_dtls";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_STR_User_ID = objUserAccount.user_id,

                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public UserAccount GetDfltCmpId(UserAccount objUserAccount)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    UserAccount objOrderLifeCycleCategory = new UserAccount();

                    const string storedProcedure2 = "sp_ma_get_dflt_cust_id";
                    IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_User_ID = objUserAccount.user_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objUserAccount.ListGetDfltCmpId = ListUserDetailsLST.ToList();

                    return objUserAccount;
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

        public UserAccount GetGridotherScnPermDetails(UserAccount objUserAccount)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    UserAccount objOrderLifeCycleCategory = new UserAccount();

                    const string storedProcedure2 = "sp_ma_get_other_scn_dtls";
                    IEnumerable<UserAccountDtl> ListUserDetailsLST = connection.Query<UserAccountDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_STATUS = objUserAccount.Status
                        },
                        commandType: CommandType.StoredProcedure);
                    objUserAccount.ListOtherScnPerm = ListUserDetailsLST.ToList();

                    return objUserAccount;
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
