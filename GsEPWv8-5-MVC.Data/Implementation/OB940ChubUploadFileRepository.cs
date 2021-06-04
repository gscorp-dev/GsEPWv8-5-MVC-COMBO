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
    public class OB940ChubUploadFileRepository : IOB940UploadFileRepository
    {

        public DataTable fnGetOBDocListBy940(string pstrCmpId, string pstrUploadRefNo)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            try
            {
                command = new SqlCommand("spGetOBDocListBy940", connection);
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
        public bool GenerateShipRequest(string p_str_upload_ref_num)
        {
            return true;
        }
        public DataTable fnGetCustConfig(string p_str_cmp_id)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            try
            {
                command = new SqlCommand("spGetCustConfig", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
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
        public bool Check940UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            try
            {
                int l_int_file_count = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_OB_940_UPLOAD_FILE_EXISTS", connection);
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

        public bool CheckRefNumExists(string p_str_cmp_id, string p_str_ref_num)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            try
            {
                int l_int_file_count = 0;
                command = new SqlCommand("SP_OB_SO_REF_NUM_EXISTS", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_ref_num", SqlDbType.VarChar).Value = p_str_ref_num;
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

                connection.Close();
            }
        }


        public string Get940BatchId(string p_str_cmp_id)
        {
            try
            {
                string l_str_batch_id = string.Empty;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_OB_940_GET_BATCH_ID", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                l_str_batch_id = Convert.ToString(command.ExecuteScalar());

                if (l_str_batch_id.Length > 0)
                {
                    return p_str_cmp_id.Substring(0, 2) + l_str_batch_id;
                }
                else
                {
                    return p_str_cmp_id.Substring(0, 2) + "99999";
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

        public int Get940UploadRefNum(string p_str_cmp_id)
        {

            try
            {
                int l_int_upload_ref_num = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_OB_940_GET_UPLOAD_REFNUM", connection);
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

        public string SaveOB940UploadFile(string p_str_cmp_id, DataTable p_dt_ob_940_upload_file_info, DataTable p_dt_ob_940_upload_file_hdr, DataTable p_dt_ob_940_upload_file_dtl)
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

                        sqlBulkCopy.DestinationTableName = "dbo.tbl_ob_940_upload_file_info";
                        sqlBulkCopy.WriteToServer(p_dt_ob_940_upload_file_info);


                        sqlBulkCopy.DestinationTableName = "dbo.tbl_ob_940_upload_hdr";
                        sqlBulkCopy.WriteToServer(p_dt_ob_940_upload_file_hdr);

                        sqlBulkCopy.DestinationTableName = "dbo.tbl_ob_940_upload_dtl";
                        p_dt_ob_940_upload_file_dtl.Columns.Remove("header_data");
                        p_dt_ob_940_upload_file_dtl.Columns.Remove("error_desc");
                        sqlBulkCopy.WriteToServer(p_dt_ob_940_upload_file_dtl);

                        connection.Close();

                    }
                }


                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
                throw ex;
            }
            finally
            {

            }


        }

        public OB940UploadFile GetOB940UploadDtlRptData(OB940UploadFile objOB940UploadFile, string p_str_upload_ref_num, string p_str_so_num)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OB940Report> ListIRFP = connection.Query<OB940Report>("SP_OB_940_UPLOAD_DTL_RPT", new
                    {
                        @cmp_id = objOB940UploadFile.cmp_id,
                        @File_name = objOB940UploadFile.file_name,
                        @batch_no = objOB940UploadFile.batch_id,
                        @UploaddtFm = objOB940UploadFile.upload_dt_from,
                        @UploaddtTo = objOB940UploadFile.upload_dt_to,
                        @p_str_process_id = p_str_upload_ref_num,
                        @p_str_so_num = p_str_so_num,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOB940UploadFile.ListOB940UploadDtlRpt = ListIRFP.ToList();
                }
                return objOB940UploadFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public bool fnGenerate943ForTransCmpId(string pstrCmpId, string pstrUploadRefNum, string  pstrTransferCmpId)
        {
            bool lblnResult = false;
            try
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
                                cmd.CommandText = "spGenerate943ForTransCmpId";
                                cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                                cmd.Parameters.Add("@pstrUploadRefNum", SqlDbType.VarChar).Value = pstrUploadRefNum;
                                cmd.Parameters.Add("@pstrTransferCmpId", SqlDbType.VarChar).Value = pstrTransferCmpId;
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
                }


                return lblnResult;
            }
            catch (Exception ex)
            {
                return lblnResult;
                throw ex;
            }
            finally
            {

            }
        }
        public string MoveOB940UploadToSOTables(string p_str_cmp_id, string p_str_upload_ref_num)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (IDbTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {

                            // transactional code...
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "SP_OB_Move_OB_940Upload_To_SO_Tables";
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
        public DataTable GetOB940UploadDtlRptDataExcel(string l_str_cmp_id, string l_str_File_name, string l_str_batch_no, string l_str_UploaddtFm, string l_str_UploaddtTo, string l_str_process_id, string l_str_so_num)
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_OB_940_UPLOAD_DTL_RPT", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@cmp_id", SqlDbType.VarChar).Value = l_str_cmp_id;
                command.Parameters.Add("@File_name", SqlDbType.VarChar).Value = l_str_File_name;
                command.Parameters.Add("@batch_no", SqlDbType.VarChar).Value = l_str_batch_no;
                command.Parameters.Add("@UploaddtFm", SqlDbType.VarChar).Value = l_str_UploaddtFm;
                command.Parameters.Add("@UploaddtTo", SqlDbType.VarChar).Value = l_str_UploaddtTo;
                command.Parameters.Add("@p_str_process_id", SqlDbType.VarChar).Value = l_str_process_id;
                command.Parameters.Add("@p_str_so_num", SqlDbType.VarChar).Value = l_str_so_num;
                connection.Open();
                DataTable dtGetInboundAckExcelTemplate = new DataTable();
                dtGetInboundAckExcelTemplate.Load(command.ExecuteReader());
                return dtGetInboundAckExcelTemplate;
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