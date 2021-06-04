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

namespace  GsEPWv8_5_MVC.Data.Implementation
{
    public class LoginRepository : ILoginRepository
    {
        public Login LoginAuthentication(Login objLogin)
        {
            Login objLoginDatas = new Login();
            try
            {
                
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SpLogOnUserCkmvc", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@user_id", SqlDbType.VarChar).Value = objLogin.Email;
                command.Parameters.Add("@passwd ", SqlDbType.VarChar).Value = objLogin.Password;
                connection.Open();

                DataTable dt = new DataTable();

                dt.Load(command.ExecuteReader());
                List<Login> objLoginData = new List<Login>();  

                foreach (DataRow dr in dt.Rows)
                {
                    objLoginDatas.user_id = dr.Field<string>("user_id");
                    objLoginDatas.passwd = dr.Field<string>("passwd");
                    objLoginDatas.dflt_cust_id = dr.Field<string>("dflt_cust_id");
                    objLoginDatas.user_fst_name = dr.Field<string>("user_fst_name");
                    objLoginDatas.user_lst_name = dr.Field<string>("user_lst_name");
                    objLoginDatas.is_cmp_user = dr.Field<string>("user_type");
                    objLoginDatas.user_type = dr.Field<string>("user_type");
                    DateTime? ChangeDate = dr.Field<DateTime?>("chg_dt");
                    if (ChangeDate.HasValue)
                    {
                        objLoginDatas.chg_dt = dr.Field<DateTime>("chg_dt");
                    }
                    
                    if (objLoginDatas.is_cmp_user.ToUpper()== "ADMIN" || objLoginDatas.is_cmp_user.ToUpper() == "3PL-USER" || objLoginDatas.is_cmp_user.ToUpper() == "3PL-ADMIN")
                    {
                        objLoginDatas.is_cmp_user = "Y";
                    }
                    else
                    {
                        objLoginDatas.is_cmp_user = "N";
                    }
                    objLoginDatas.IS3RDUSER = dr.Field<string>("isanother");
                    objLoginDatas.ConnectionName = dr.Field<string>("ConnectionName");
                    objLoginData.Add(objLoginDatas);
                }

                if (objLoginData.Count() == 0)
                {
                    return null;
                }
                return objLoginData.First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CheckPasswordExpiry(string p_str_user_id)
        {
            try
            {
                int l_int_pwd_chg_days =0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ma_get_pwd_chg_days", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@P_STR_USER_ID", SqlDbType.VarChar).Value = p_str_user_id;
                connection.Open();
                l_int_pwd_chg_days = Convert.ToInt32(command.ExecuteScalar());
                if (l_int_pwd_chg_days > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return true;

            }
            finally
            {

            }
        }
    }
}
