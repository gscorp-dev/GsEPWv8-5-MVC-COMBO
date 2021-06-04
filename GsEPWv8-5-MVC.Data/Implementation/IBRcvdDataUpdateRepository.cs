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
    public class IBRcvdDataUpdateRepository : IIBRcvdDataUpdateRepository
    {
        public IBRcvdDataUpdate GetRcvdHdr(IBRcvdDataUpdate objIBRcvdDataUpdateHdr)
        {
            try
            {
               

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_ib_get_rcvd_data_update_hdr";
                    IList<IBRcvdDataUpdate> ListIRFP = connection.Query<IBRcvdDataUpdate>(SearchCustDtls, new
                    {
                        @p_str_cmpid = objIBRcvdDataUpdateHdr.cmp_id,
                        @p_str_ib_doc_id = objIBRcvdDataUpdateHdr.ib_doc_id,
                        @p_str_cntr_id = objIBRcvdDataUpdateHdr.cntr_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objIBRcvdDataUpdateHdr.GetRcvdHdr = ListIRFP.ToList();
                    objIBRcvdDataUpdateHdr.doc_status_changed = CheckDocStatusChanged(objIBRcvdDataUpdateHdr.cmp_id, objIBRcvdDataUpdateHdr.ib_doc_id);
                }

                return objIBRcvdDataUpdateHdr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public int GetRefNum(string enitity_code)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            try
            {
                int l_int_ref_num = 0;
              
                command = new SqlCommand("sp_get_entity_value", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@enitity_code", SqlDbType.VarChar).Value = enitity_code;
                connection.Open();
                l_int_ref_num = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_ref_num > 0)
                {
                    return l_int_ref_num;
                }
                else
                {
                    return l_int_ref_num;
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

        public string CheckDocStatusChanged(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
            SqlCommand command = new SqlCommand();
            try
            {
                int l_int_rec_cnt = 0;

                command = new SqlCommand("sp_ib_doc_status_changed", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_ib_doc_id", SqlDbType.VarChar).Value = p_str_ib_doc_id;
                connection.Open();
                l_int_rec_cnt = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_rec_cnt > 0)
                {
                    return "Y";
                }
                else
                {
                    return "N";
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


        public DataTable GetIBCheckRcvdDtCanUpdate(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_rcvd_dt)

        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ib_rcvd_dt_can_update", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_ib_doc_id", SqlDbType.VarChar).Value = p_str_ib_doc_id;
                command.Parameters.Add("@p_str_rcvd_dt", SqlDbType.VarChar).Value = p_str_rcvd_dt;
                connection.Open();
                DataTable dtIBCheckRcvdDtCanUpdate = new DataTable();
                dtIBCheckRcvdDtCanUpdate.Load(command.ExecuteReader());
                return dtIBCheckRcvdDtCanUpdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public string SaveIBRcvdData(string p_str_cmp_id,  DataTable p_dt_ib_rcvd_updt_hdr, DataTable p_dt_ib_rcvd_updt_dtl,string p_str_save_hdr, string p_str_cntr_type, bool p_bln_excld_bill)
        {
            string p_str_ref_no = string.Empty;
            try
            {
                p_str_ref_no = Convert.ToString( GetRefNum("ib_rcvd_data_update"));
                p_dt_ib_rcvd_updt_hdr.Columns.Add("req_num",typeof(string));
                p_dt_ib_rcvd_updt_hdr.Columns.Add("is_processed", typeof(string));
                
                p_dt_ib_rcvd_updt_dtl.Columns.Add("req_num", typeof(string));

                foreach (DataRow row in p_dt_ib_rcvd_updt_hdr.Rows)
                {
                    row["req_num"] = p_str_ref_no;
                    row["is_processed"] = "N";
                }
                foreach (DataRow row in p_dt_ib_rcvd_updt_dtl.Rows)
                {
                    row["req_num"] = p_str_ref_no;
                }

                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    connection.Open();
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        //Set the database table name
                       
                        sqlBulkCopy.DestinationTableName = "tbl_ib_rcvd_updt_hdr_temp";
                        sqlBulkCopy.WriteToServer(p_dt_ib_rcvd_updt_hdr);
                      
                        sqlBulkCopy.DestinationTableName = "tbl_ib_rcvd_updt_dtl_temp";
                        sqlBulkCopy.WriteToServer(p_dt_ib_rcvd_updt_dtl);
                       
                    }

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "sp_ib_rcvd_data_updt";
                        cmd.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                        cmd.Parameters.Add("@p_str_req_num", SqlDbType.VarChar).Value = p_str_ref_no;
                        cmd.Parameters.Add("@p_str_save_hdr", SqlDbType.VarChar).Value = p_str_save_hdr;
                        cmd.Parameters.Add("@p_str_cntr_type", SqlDbType.VarChar).Value = p_str_cntr_type;
                        if (p_bln_excld_bill)
                        {
                            cmd.Parameters.Add("@p_str_excld_bill", SqlDbType.VarChar).Value = "EXCLD";
                        }
                        else
                        {
                            cmd.Parameters.Add("@p_str_excld_bill", SqlDbType.VarChar).Value = p_str_cntr_type;
                        } 
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();

                    }
                   
                    connection.Close();
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
        public string MoveIBRcvdDataTables(string p_str_cmp_id, string p_str_ref_num)
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
                            cmd.CommandText = "sp_ib_rcvd_data_update";
                            cmd.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                            cmd.Parameters.Add("@p_str_ref_num", SqlDbType.VarChar).Value = p_str_ref_num;
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
        public IBRcvdDataUpdate ListDocItemList(IBRcvdDataUpdate objIBRcvdDataUpdateHdr)
        {
            try
            {
                IBRcvdDataUpdateDtl objCustDtl = new IBRcvdDataUpdateDtl();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_ib_get_rcvd_data_update_dtl";
                    IList<IBRcvdDataUpdateDtl> ListIRFP = connection.Query<IBRcvdDataUpdateDtl>(SearchCustDtls, new
                    {
                        @p_str_cmpid = objIBRcvdDataUpdateHdr.cmp_id,
                        @p_str_ib_doc_id = objIBRcvdDataUpdateHdr.ib_doc_id,
                        @p_str_cntr_id = objIBRcvdDataUpdateHdr.cntr_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objIBRcvdDataUpdateHdr.ListDocItemList = ListIRFP.ToList();
                }
                return objIBRcvdDataUpdateHdr;
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
