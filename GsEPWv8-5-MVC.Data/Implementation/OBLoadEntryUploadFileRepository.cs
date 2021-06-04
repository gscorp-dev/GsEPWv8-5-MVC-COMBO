using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GsEPWv8_5_MVC.Data.Implementation
{
  public      class OBLoadEntryUploadFileRepository
    {
        public bool CheckLoadEntryFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            try
            {
                int l_int_file_count = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_load_entry_file_exist", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_file_name", SqlDbType.VarChar).Value = p_str_file_name;
                connection.Open();
                l_int_file_count = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_file_count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
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

        public string CheckBatchCustPOExists(string p_str_cmp_id, string p_str_batch_num, string p_str_cust_po, string p_str_dept_id, string p_str_store_id)
        {
            try
            {
                string l_str_so_num = string.Empty;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_chk_batch_po_exists", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_batch_num", SqlDbType.VarChar).Value = p_str_batch_num;
                command.Parameters.Add("@p_str_cust_po", SqlDbType.VarChar).Value = p_str_cust_po;
                command.Parameters.Add("@p_str_dept_id", SqlDbType.VarChar).Value = p_str_dept_id;
                command.Parameters.Add("@p_str_store_id", SqlDbType.VarChar).Value = p_str_store_id;
                connection.Open();
                l_str_so_num = command.ExecuteScalar().ToString();

                return l_str_so_num;

            }
            catch (Exception ex)
            {
                throw ex;
               
            }
            finally
            {

            }
        }


        public int GetOBLoadUploadRefNum(string p_str_cmp_id)
        {

            try
            {
                int l_int_upload_ref_num = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_get_load_doc_id", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                l_int_upload_ref_num = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_upload_ref_num > 0)
                {
                    return l_int_upload_ref_num;
                }
                else
                {
                    return l_int_upload_ref_num;
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

        
        public bool SaveOBLoadEntryBatch(string p_str_cmp_id, DataTable dt_ob_load_file_info, DataTable dt_ob_load_batch_dtl, string user_id)
        {
            int l_int_upload_ref_num = 0;

            DataTable dtOBLoadEntry;
            try
            {
                l_int_upload_ref_num = GetOBLoadUploadRefNum(p_str_cmp_id);
                dt_ob_load_file_info.Rows[0]["upload_ref_num"] = l_int_upload_ref_num;
              

                for (int i = 0;i<= dt_ob_load_batch_dtl.Rows.Count -1;i++)
                {
                    dt_ob_load_batch_dtl.Rows[i]["upload_ref_num"] = l_int_upload_ref_num;
                }
              

                dtOBLoadEntry = new DataTable();
                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        //Set the database table name
                        connection.Open();
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_ob_load_entry_upload_file_info";
                        sqlBulkCopy.WriteToServer(dt_ob_load_file_info);

                        sqlBulkCopy.DestinationTableName = "dbo.tbl_ob_load_entry_upload_dtl";
                        sqlBulkCopy.WriteToServer(dt_ob_load_batch_dtl);

                        using (SqlCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "sp_ob_load_entry_updt_batch";
                            cmd.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                            cmd.Parameters.Add("@p_str_upload_ref_num", SqlDbType.VarChar).Value = l_int_upload_ref_num.ToString();
                            cmd.Connection = connection;

                            cmd.ExecuteNonQuery();


                        }
                        connection.Close();
                    }
                }

                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;

            }
            finally
            {

            }

        }
        

    }
}
