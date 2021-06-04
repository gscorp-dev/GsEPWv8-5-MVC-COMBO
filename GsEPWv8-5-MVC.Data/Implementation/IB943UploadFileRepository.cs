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

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class IB943UploadFileRepository : IIB943UploadFileRepository
    {

        public DataTable fnGetIBDocListBy943(string pstrCmpId, string pstrUploadRefNo)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            try
            {
                command = new SqlCommand("spGetIBDocListBy943", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                command.Parameters.Add("@pstrUploadRefNo", SqlDbType.VarChar).Value = pstrUploadRefNo;
                connection.Open();
                DataTable dtCustConfig = new DataTable();
                dtCustConfig.Load(command.ExecuteReader());
                return dtCustConfig;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                connection.Close();
            }
        }

        public bool Check943UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            try
            {
                int l_int_file_count = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_IB_943_UPLOAD_FILE_EXISTS", connection);
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

        public bool CheckCntrIdExists(string p_str_cmp_id, string p_str_cntr_id)
        {
            try
            {
                int l_int_cntr_id_count = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_IB_CNTR_ID_EXISTS", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_cntr_id", SqlDbType.VarChar).Value = p_str_cntr_id;
                connection.Open();
                l_int_cntr_id_count = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_cntr_id_count > 0)
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

       
        public string SaveIB943UploadFile(string p_str_cmp_id, DataTable p_dt_ib_943_upload_file_info, DataTable p_dt_ib_943_upload_file_hdr, DataTable p_dt_ib_943_upload_file_dtl)
        {
            try
            {
                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        //Set the database table name
                        connection.Open();
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_ib_943_upload_file_info";
                        sqlBulkCopy.WriteToServer(p_dt_ib_943_upload_file_info);

                        p_dt_ib_943_upload_file_hdr.Columns.Remove("hdr_data");
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_ib_943_upload_hdr";
                        sqlBulkCopy.WriteToServer(p_dt_ib_943_upload_file_hdr);

                        sqlBulkCopy.DestinationTableName = "dbo.tbl_ib_943_upload_dtl";

                        p_dt_ib_943_upload_file_dtl.Columns.Remove("header_data");
                        p_dt_ib_943_upload_file_dtl.Columns.Remove("error_desc");
                        sqlBulkCopy.WriteToServer(p_dt_ib_943_upload_file_dtl);

                        connection.Close();

                    }
                }


                return "OK";
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
                throw ex;
            }
            finally
            {

            }


        }


        public string MoveIB943UploadToIBDocTables(string p_str_cmp_id, string p_str_upload_ref_num)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();


                    try
                    {

                        // transactional code...
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_IB_943_Add_New_Item";
                            cmd.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                            cmd.Parameters.Add("@p_str_upload_ref_num", SqlDbType.VarChar).Value = p_str_upload_ref_num;
                            cmd.Connection = conn;

                            cmd.ExecuteNonQuery();


                        }
                    }

                    catch  (Exception ex)
                    {
                        throw ex;
                    }

                    using (IDbTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {

                            // transactional code...
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "SP_IB_Move_IB_943Upload_To_IB_Doc_Tables";
                                cmd.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                                cmd.Parameters.Add("@p_str_upload_ref_num", SqlDbType.VarChar).Value = p_str_upload_ref_num;
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

                    try
                    {

                        // transactional code...
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_IB_943_INSERT_IB_DOC_CTN";
                            cmd.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                            cmd.Parameters.Add("@p_str_upload_ref_num", SqlDbType.VarChar).Value = p_str_upload_ref_num;
                            cmd.Connection = conn;

                            cmd.ExecuteNonQuery();


                        }
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }


                return "OK";
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
                throw ex;
            }
            finally
            {

            }
        }

         public int Get943UploadRefNum(string p_str_cmp_id)
        {

            try
            {
                int l_int_upload_ref_num = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_IB_943_GET_UPLOAD_REFNUM", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //  command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
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

        public IB943UploadFile Send943EmailAckReport(string p_str_cmp_id, string p_str_file_name, string p_str_upload_ref_num, IB943UploadFile objIB943UploadFile)
        {

            try
            {
               

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string str_sp_name = "sp_get_ib_943_upload_ack_rpt";
                    IList<IB943UploadAckRpt> LstIB943UploadFile = connection.Query<IB943UploadAckRpt>(str_sp_name, new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                        @p_str_file_name = p_str_file_name,
                        @p_str_upload_ref_num = p_str_upload_ref_num,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objIB943UploadFile.ListIB943UploadAckRpt = LstIB943UploadFile.ToList();
                }
                return objIB943UploadFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable GetIB940UploadFileSummary(string p_str_cmp_id, string p_str_file_name, string p_str_upload_ref_num)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_get_ib940_upload_file_summary", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_file_name", SqlDbType.VarChar).Value = p_str_file_name;
                command.Parameters.Add("@p_str_upload_ref_num", SqlDbType.VarChar).Value = p_str_upload_ref_num;
                connection.Open();
                DataTable dt943Summary = new DataTable();
                dt943Summary.Load(command.ExecuteReader());
                return dt943Summary;
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
