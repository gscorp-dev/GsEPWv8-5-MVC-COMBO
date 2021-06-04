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
    public class InvStkAdjRepository : IInvStkAdjRepository
    {

        public InvStkAdj getStockForAdj(InvStkAdjInquiry objInvStkAdjInquiry)
        {
            try
            {
                InvStkAdj objInvStkAdj = new InvStkAdj();
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "spGetStockForAdj";
                    IEnumerable<InvStkAdjDetail> ListStkAdjDetail = connection.Query<InvStkAdjDetail>(storedProcedure1,
                        new
                        {
                            @p_str_cmp_id = objInvStkAdjInquiry.cmp_id,
                            @p_str_itm_num = objInvStkAdjInquiry.itm_num,
                            @p_str_itm_color = objInvStkAdjInquiry.itm_color,
                            @p_str_itm_size = objInvStkAdjInquiry.itm_size,
                            @p_str_itm_name = objInvStkAdjInquiry.itm_name,
                            @p_str_ib_doc_id = objInvStkAdjInquiry.ib_doc_id,
                            @p_str_cntr_id = objInvStkAdjInquiry.cont_id,
                            @p_str_lot_id = objInvStkAdjInquiry.lot_id,
                            @p_str_loc_id = objInvStkAdjInquiry.loc_id,
                            @p_str_ref_no = objInvStkAdjInquiry.ref_no,
                            @p_str_whs_id = objInvStkAdjInquiry.whs_id,
                            @p_str_fm_dt = objInvStkAdjInquiry.rcvd_from_dt,
                            @p_str_to_dt = objInvStkAdjInquiry.rcvd_to_dt,
                            @p_str_po_num = objInvStkAdjInquiry.po_num,
                            @p_str_status = objInvStkAdjInquiry.status
                        },
                        commandType: CommandType.StoredProcedure);
                    objInvStkAdj.ListInvStkAdjDetail = ListStkAdjDetail.ToList();
                }

                return objInvStkAdj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public bool CheckPhyCountFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            try
            {
                int l_int_file_count = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spGetIvPhyCountFileExists", connection);
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


        public int GetInvPhyCountUploadRefNum(string p_str_cmp_id)
        {

            try
            {
                int l_int_upload_ref_num = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("SP_IB_943_GET_UPLOAD_REFNUM", connection);
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
        public bool SaveInvStkAdjTempSingle(InvStkAdjAdd objInvStkAdjAdd)
        {

            try
            {
                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    connection.Open();
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "spSaveInvStkAdj";
                        cmd.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = objInvStkAdjAdd.cmp_id;
                        cmd.Parameters.Add("@p_str_itm_num", SqlDbType.VarChar).Value = objInvStkAdjAdd.itm_num;
                        cmd.Parameters.Add("@p_str_itm_color", SqlDbType.VarChar).Value = objInvStkAdjAdd.itm_color;
                        cmd.Parameters.Add("@p_str_itm_size", SqlDbType.VarChar).Value = objInvStkAdjAdd.itm_size;
                        cmd.Parameters.Add("@p_str_itm_code", SqlDbType.VarChar).Value = objInvStkAdjAdd.itm_code;
                        cmd.Parameters.Add("@p_str_lot_id", SqlDbType.VarChar).Value = objInvStkAdjAdd.lot_id;
                        cmd.Parameters.Add("@p_str_po_num", SqlDbType.VarChar).Value = objInvStkAdjAdd.po_num;
                        cmd.Parameters.Add("@p_str_from_loc_id", SqlDbType.VarChar).Value = objInvStkAdjAdd.cur_loc_id;
                        cmd.Parameters.Add("@p_str_to_loc_id", SqlDbType.VarChar).Value = objInvStkAdjAdd.new_loc_id;
                        cmd.Parameters.Add("@p_int_cur_ctns", SqlDbType.Int).Value = objInvStkAdjAdd.cur_avail_ctn;
                        cmd.Parameters.Add("@p_int_new_ctns", SqlDbType.Int).Value = objInvStkAdjAdd.new_avail_ctn;
                        cmd.Parameters.Add("@p_int_cur_itm_qty", SqlDbType.Int).Value = objInvStkAdjAdd.cur_itm_qty;
                        cmd.Parameters.Add("@p_int_new_itm_qty", SqlDbType.Int).Value = objInvStkAdjAdd.new_itm_qty;
                        cmd.Parameters.Add("@p_str_adj_reason", SqlDbType.VarChar).Value = objInvStkAdjAdd.adj_reason;
                        cmd.Parameters.Add("@p_str_note", SqlDbType.VarChar).Value = objInvStkAdjAdd.adj_note;
                        cmd.Parameters.Add("@p_str_user_id", SqlDbType.VarChar).Value = objInvStkAdjAdd.user_id;
                        cmd.Parameters.Add("@p_int_split_qty", SqlDbType.Int).Value = objInvStkAdjAdd.split_qty;
                        cmd.Parameters.Add("@p_int_new_split_ctn_qty", SqlDbType.Int).Value = objInvStkAdjAdd.new_split_ctn_qty;
                        cmd.Parameters.Add("@p_int_adj_cur_ctns", SqlDbType.Int).Value = objInvStkAdjAdd.adj_cur_ctns;

                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return true;
        }
        public bool SaveInvStkAdjByCtns(InvStkAdjAdd objInvStkAdjByCtns)
        {

            try
            {
                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    connection.Open();
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "spSaveInvStkAdjByCtns";
                        cmd.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = objInvStkAdjByCtns.cmp_id;
                        cmd.Parameters.Add("@p_str_itm_num", SqlDbType.VarChar).Value = objInvStkAdjByCtns.itm_num;
                        cmd.Parameters.Add("@p_str_itm_color", SqlDbType.VarChar).Value = objInvStkAdjByCtns.itm_color;
                        cmd.Parameters.Add("@p_str_itm_size", SqlDbType.VarChar).Value = objInvStkAdjByCtns.itm_size;
                        cmd.Parameters.Add("@p_str_itm_code", SqlDbType.VarChar).Value = objInvStkAdjByCtns.itm_code;
                        cmd.Parameters.Add("@p_str_lot_id", SqlDbType.VarChar).Value = objInvStkAdjByCtns.lot_id;
                        cmd.Parameters.Add("@p_str_po_num", SqlDbType.VarChar).Value = objInvStkAdjByCtns.po_num;
                        cmd.Parameters.Add("@p_str_from_loc_id", SqlDbType.VarChar).Value = objInvStkAdjByCtns.cur_loc_id;
                        cmd.Parameters.Add("@p_int_adj_ctns", SqlDbType.Int).Value = objInvStkAdjByCtns.adj_ctns;
                        cmd.Parameters.Add("@p_int_cur_itm_qty", SqlDbType.Int).Value = objInvStkAdjByCtns.cur_itm_qty;
                        cmd.Parameters.Add("@p_str_adj_reason", SqlDbType.VarChar).Value = objInvStkAdjByCtns.adj_reason;
                        cmd.Parameters.Add("@p_str_note", SqlDbType.VarChar).Value = objInvStkAdjByCtns.adj_note;
                        cmd.Parameters.Add("@p_str_user_id", SqlDbType.VarChar).Value = objInvStkAdjByCtns.user_id;
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return true;
        }

        public int GetMergeDocId()
        {

            try
            {
                int l_int_merge_doc_id = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spGetMergeDocId", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                l_int_merge_doc_id = Convert.ToInt32(command.ExecuteScalar());

                if (l_int_merge_doc_id > 0)
                {
                    return l_int_merge_doc_id;
                }
                else
                {
                    return l_int_merge_doc_id;
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

        public bool SaveMergeCtns(string pstrCmpId, InvMergeHdr ObjInvMergeHdr, DataTable ldtMergeDtl)
        {
            bool lblnReturnValue = false;
            int lintMergeDocId = 0;
            int lintCount = 0;
          

            try
            {
                lintMergeDocId = GetMergeDocId();

                lintCount = ldtMergeDtl.Rows.Count;
                for (int i = 0; i < lintCount; i++)
                {
                    ldtMergeDtl.Rows[i]["merge_doc_id"] = lintMergeDocId.ToString();
                }

                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();

                using (SqlConnection connection = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_iv_merge_dtl";
                        sqlBulkCopy.WriteToServer(ldtMergeDtl);
                        connection.Close();

                    }
               
                    connection.Open();
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "spIVSaveMergeCtns";
                        cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = ObjInvMergeHdr.cmp_id;
                        cmd.Parameters.Add("@pstrMergeDocId", SqlDbType.VarChar).Value = lintMergeDocId.ToString();

                        cmd.Parameters.Add("@pstrItmCode", SqlDbType.VarChar).Value = ObjInvMergeHdr.itm_code;
                        cmd.Parameters.Add("@pstrItmNum", SqlDbType.VarChar).Value = ObjInvMergeHdr.itm_num;
                        cmd.Parameters.Add("@pstrItmColor", SqlDbType.VarChar).Value = ObjInvMergeHdr.itm_color;
                        cmd.Parameters.Add("@pstrItmSize", SqlDbType.VarChar).Value = ObjInvMergeHdr.itm_size;
                      
                        cmd.Parameters.Add("@pintMergeCtns", SqlDbType.Int).Value = ObjInvMergeHdr.merge_ctns;
                        cmd.Parameters.Add("@pintMergePpk", SqlDbType.Int).Value = ObjInvMergeHdr.merge_ppk;
                        cmd.Parameters.Add("@pstrLocId", SqlDbType.VarChar).Value = ObjInvMergeHdr.merge_loc_id;
                        cmd.Parameters.Add("@pstrMergeNote", SqlDbType.VarChar).Value = ObjInvMergeHdr.merge_note;
                        cmd.Parameters.Add("@pintMergeOddPPK", SqlDbType.Int).Value = ObjInvMergeHdr.merge_odd_ppk;
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();

                }
                lblnReturnValue = true;
            }
            catch (Exception ex)
            {
                lblnReturnValue = false;
                throw ex;
            }
            finally
            {

            }
            return lblnReturnValue;
        }

    }
}
