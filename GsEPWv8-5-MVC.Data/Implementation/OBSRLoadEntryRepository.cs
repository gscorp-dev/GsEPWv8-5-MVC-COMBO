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

    public class OBSRLoadEntryRepository : IOBSRLoadEntryRepository
    {
        public OBSRLoadInquiry GetLoadEntry(OBSRLoadInquiry objOBSRLoadInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetSRSummary> ListIRFP = connection.Query<OBGetSRSummary>("sp_ob_get_load_entry_dtl", new
                    {
                        @p_str_cmp_id = objOBSRLoadInquiry.cmp_id,
                        @p_str_load_doc_id = objOBSRLoadInquiry.load_doc_id,
                        @p_str_load_number = objOBSRLoadInquiry.load_number,
                        @p_str_sr_no = objOBSRLoadInquiry.so_num_from


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRLoadInquiry.ListOBGetSRSummary = ListIRFP.ToList();


                    IList<OBSRLoadEntryHdr> ListLoadEntryHdr = connection.Query<OBSRLoadEntryHdr>("sp_ob_get_load_entry_hdr", new
                    {
                        @p_str_cmp_id = objOBSRLoadInquiry.cmp_id,
                        @p_str_load_doc_id = objOBSRLoadInquiry.load_doc_id,
                        @p_str_load_number = objOBSRLoadInquiry.load_number

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRLoadInquiry.ListOBSRLoadEntryHdr = ListLoadEntryHdr.ToList();




                }
                return objOBSRLoadInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public OBSRLoadInquiry getOBLoadHdrByBatch(OBSRLoadInquiry objOBSRLoadInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBSRLoadEntryHdr> ListLoadEntryHdr = connection.Query<OBSRLoadEntryHdr>("spGetOBLoadHdrByBatch", new
                    {
                        @p_str_cmp_id = objOBSRLoadInquiry.cmp_id,
                        @p_str_batch_num = objOBSRLoadInquiry.batch_num

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRLoadInquiry.ListOBSRLoadEntryHdr = ListLoadEntryHdr.ToList();

                  

                 }
                return objOBSRLoadInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public OBSRLoadInquiry GetOBSRSummary(OBSRLoadInquiry objOBSRLoadInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetSRSummary> ListIRFP = connection.Query<OBGetSRSummary>("sp_ob_get_sr_summary", new
                    {
                        @p_str_cmp_id = objOBSRLoadInquiry.cmp_id,
                        @p_str_batch_num = objOBSRLoadInquiry.batch_num,
                        @p_str_so_num_from = objOBSRLoadInquiry.so_num_from,
                        @p_str_so_num_to = objOBSRLoadInquiry.so_num_to,
                        @p_str_so_dt_from = objOBSRLoadInquiry.so_dt_from,
                        @p_str_so_dt_to = objOBSRLoadInquiry.so_dt_to,
                        @p_str_load_number = objOBSRLoadInquiry.load_number


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRLoadInquiry.ListOBGetSRSummary = ListIRFP.ToList();


                    IList<OBSRShipTo> ListShipTo = connection.Query<OBSRShipTo>("sp_ob_get_sr_ship_to", new
                    {
                        @p_str_cmp_id = objOBSRLoadInquiry.cmp_id,
                        @p_str_batch_num = objOBSRLoadInquiry.batch_num,
                        @p_str_so_num_from = objOBSRLoadInquiry.so_num_from,
                        @p_str_so_num_to = objOBSRLoadInquiry.so_num_to,
                        @p_str_so_dt_from = objOBSRLoadInquiry.so_dt_from,
                        @p_str_so_dt_to = objOBSRLoadInquiry.so_dt_to,
                        @p_str_load_number = objOBSRLoadInquiry.load_number


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRLoadInquiry.ListOBSRShipTo = ListShipTo.ToList();




                }
                return objOBSRLoadInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OBSRLoadInquiry GetOBSRSummaryForLoadEntry(OBSRLoadInquiry objOBSRLoadInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetSRSummary> ListIRFP = connection.Query<OBGetSRSummary>("spGetOBSRSummaryForLoadEntry", new
                    {
                        @p_str_cmp_id = objOBSRLoadInquiry.cmp_id,
                        @p_str_batch_num = objOBSRLoadInquiry.batch_num,
                        @p_str_so_num_from = objOBSRLoadInquiry.so_num_from,
                        @p_str_so_num_to = objOBSRLoadInquiry.so_num_to,
                        @p_str_so_dt_from = objOBSRLoadInquiry.so_dt_from,
                        @p_str_so_dt_to = objOBSRLoadInquiry.so_dt_to,
                        @p_str_load_number = objOBSRLoadInquiry.load_number


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRLoadInquiry.ListOBGetSRSummary = ListIRFP.ToList();


                    IList<OBSRShipTo> ListShipTo = connection.Query<OBSRShipTo>("sp_ob_get_sr_ship_to", new
                    {
                        @p_str_cmp_id = objOBSRLoadInquiry.cmp_id,
                        @p_str_batch_num = objOBSRLoadInquiry.batch_num,
                        @p_str_so_num_from = objOBSRLoadInquiry.so_num_from,
                        @p_str_so_num_to = objOBSRLoadInquiry.so_num_to,
                        @p_str_so_dt_from = objOBSRLoadInquiry.so_dt_from,
                        @p_str_so_dt_to = objOBSRLoadInquiry.so_dt_to,
                        @p_str_load_number = objOBSRLoadInquiry.load_number


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRLoadInquiry.ListOBSRShipTo = ListShipTo.ToList();




                }
                return objOBSRLoadInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public bool DeleteLoadEntry(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "sp_so_del_load_entry";
                    connection.Execute(storedProcedure1,
                        new
                        {
                            @p_str_cmp_id = p_str_cmp_id,
                            @p_str_load_doc_id = p_str_load_doc_id,
                            @p_str_load_number = p_str_load_number,

                        }, commandType: CommandType.StoredProcedure);
                    return true;
                }

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
        public bool SaveOBLoadEntry(string p_str_cmp_id, string p_str_load_number, string p_str_so_list, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id, DataTable dtSoList)
        {
            bool l_bln_status = false;
            try
            {



                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure0 = "spDelLoadBOL";
                    connection.Execute(storedProcedure0,
                        new
                        {
                            @cmp_id = objOBSRLoadEntryHdr[0].cmp_id,
                            @load_doc_id = objOBSRLoadEntryHdr[0].load_doc_id,
                            @pstrSrNo = p_str_so_list
                        }, commandType: CommandType.StoredProcedure);

                    const string storedProcedure1 = "sp_so_add_load_entry_hdr";
                    connection.Execute(storedProcedure1,
                        new
                        {
                            @cmp_id = objOBSRLoadEntryHdr[0].cmp_id,
                            @load_doc_id = objOBSRLoadEntryHdr[0].load_doc_id,
                            @batch_num = objOBSRLoadEntryHdr[0].batch_num,
                            @load_number = p_str_load_number != null ? p_str_load_number : string.Empty,
                            @load_approve_dt = objOBSRLoadEntryHdr[0].load_approve_dt,
                            @bol_number = objOBSRLoadEntryHdr[0].bol_number,
                            @spcl_inst = objOBSRLoadEntryHdr[0].spcl_inst,
                            @load_pick_dt = objOBSRLoadEntryHdr[0].load_pick_dt,
                            @carrier_name = objOBSRLoadEntryHdr[0].carrier_name,
                            @trailer_num = objOBSRLoadEntryHdr[0].trailer_num,
                            @seal_num = objOBSRLoadEntryHdr[0].seal_num,
                            @shipto_id = objOBSRLoadEntryHdr[0].shipto_id,
                            @st_mail_name = objOBSRLoadEntryHdr[0].st_mail_name,
                            @st_addr_line1 = objOBSRLoadEntryHdr[0].st_addr_line1,
                            @st_addr_line2 = objOBSRLoadEntryHdr[0].st_addr_line2,
                            @st_city = objOBSRLoadEntryHdr[0].st_city,
                            @st_state_id = objOBSRLoadEntryHdr[0].st_state_id,
                            @st_post_code = objOBSRLoadEntryHdr[0].st_post_code,
                            @st_cntry_id = objOBSRLoadEntryHdr[0].st_cntry_id,

                            @load_note = objOBSRLoadEntryHdr[0].load_note,
                            @tot_ctns = objOBSRLoadEntryHdr[0].tot_ctns,
                            @tot_cube = objOBSRLoadEntryHdr[0].tot_cube,
                            @tot_weight = objOBSRLoadEntryHdr[0].tot_weight,
                            @tot_palet = objOBSRLoadEntryHdr[0].tot_palet,
                            @maker = user_id,
                            @maker_dt = DateTime.Now.ToString("MM/dd/yyyy"),


                        }, commandType: CommandType.StoredProcedure);

                }
                if (SaveSOLoadEtryDtl(dtSoList) == true)
                {

                    l_bln_status = true;
                }
                else
                { l_bln_status = false; }

                return l_bln_status;
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

        public bool SaveTempBol(string p_str_cmp_id, string p_str_load_number, string p_str_so_list, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id, DataTable dtSoList)
        {
            bool l_bln_status = false;
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                   const string storedProcedure1 = "sp_so_add_load_bol_hdr";
                    connection.Execute(storedProcedure1,
                        new
                        {
                            @cmp_id = objOBSRLoadEntryHdr[0].cmp_id,
                            @load_doc_id = objOBSRLoadEntryHdr[0].load_doc_id,
                            @batch_num = objOBSRLoadEntryHdr[0].batch_num,
                            @load_number = p_str_load_number != null ? p_str_load_number : string.Empty,
                            @load_approve_dt = objOBSRLoadEntryHdr[0].load_approve_dt,
                            @bol_number = objOBSRLoadEntryHdr[0].bol_number,
                            @spcl_inst = objOBSRLoadEntryHdr[0].spcl_inst,
                            @load_pick_dt = objOBSRLoadEntryHdr[0].load_pick_dt,
                            @carrier_name = objOBSRLoadEntryHdr[0].carrier_name,
                            @trailer_num = objOBSRLoadEntryHdr[0].trailer_num,
                            @seal_num = objOBSRLoadEntryHdr[0].seal_num,
                            @shipto_id = objOBSRLoadEntryHdr[0].shipto_id,
                            @st_mail_name = objOBSRLoadEntryHdr[0].st_mail_name,
                            @st_addr_line1 = objOBSRLoadEntryHdr[0].st_addr_line1,
                            @st_addr_line2 = objOBSRLoadEntryHdr[0].st_addr_line2,
                            @st_city = objOBSRLoadEntryHdr[0].st_city,
                            @st_state_id = objOBSRLoadEntryHdr[0].st_state_id,
                            @st_post_code = objOBSRLoadEntryHdr[0].st_post_code,
                            @st_cntry_id = objOBSRLoadEntryHdr[0].st_cntry_id,

                            @load_note = objOBSRLoadEntryHdr[0].load_note,
                            @tot_ctns = objOBSRLoadEntryHdr[0].tot_ctns,
                            @tot_cube = objOBSRLoadEntryHdr[0].tot_cube,
                            @tot_weight = objOBSRLoadEntryHdr[0].tot_weight,
                            @tot_palet = objOBSRLoadEntryHdr[0].tot_palet,
                            @maker = user_id,
                            @maker_dt = DateTime.Now.ToString("MM/dd/yyyy"),


                        }, commandType: CommandType.StoredProcedure);

                }
                if (SaveSOLoadBOLDtl(dtSoList) == true)
                {

                    l_bln_status = true;
                }
                else
                { l_bln_status = false; }

                return l_bln_status;
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

        public bool fnSaveMasterBol(string p_str_cmp_id, string p_str_load_number,  List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id, DataTable dtSoList)
        {
            bool l_bln_status = false;
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "sp_so_add_load_bol_hdr";
                    connection.Execute(storedProcedure1,
                        new
                        {
                            @cmp_id = objOBSRLoadEntryHdr[0].cmp_id,
                            @load_doc_id = objOBSRLoadEntryHdr[0].load_doc_id,
                            @batch_num = objOBSRLoadEntryHdr[0].batch_num,
                            @load_number = p_str_load_number != null ? p_str_load_number : string.Empty,
                            @load_approve_dt = objOBSRLoadEntryHdr[0].load_approve_dt,
                            @bol_number = objOBSRLoadEntryHdr[0].bol_number,
                            @spcl_inst = objOBSRLoadEntryHdr[0].spcl_inst,
                            @load_pick_dt = objOBSRLoadEntryHdr[0].load_pick_dt,
                            @carrier_name = objOBSRLoadEntryHdr[0].carrier_name,
                            @trailer_num = objOBSRLoadEntryHdr[0].trailer_num,
                            @seal_num = objOBSRLoadEntryHdr[0].seal_num,
                            @shipto_id = objOBSRLoadEntryHdr[0].shipto_id,
                            @st_mail_name = objOBSRLoadEntryHdr[0].st_mail_name,
                            @st_addr_line1 = objOBSRLoadEntryHdr[0].st_addr_line1,
                            @st_addr_line2 = objOBSRLoadEntryHdr[0].st_addr_line2,
                            @st_city = objOBSRLoadEntryHdr[0].st_city,
                            @st_state_id = objOBSRLoadEntryHdr[0].st_state_id,
                            @st_post_code = objOBSRLoadEntryHdr[0].st_post_code,
                            @st_cntry_id = objOBSRLoadEntryHdr[0].st_cntry_id,

                            @load_note = objOBSRLoadEntryHdr[0].load_note,
                            @tot_ctns = objOBSRLoadEntryHdr[0].tot_ctns,
                            @tot_cube = objOBSRLoadEntryHdr[0].tot_cube,
                            @tot_weight = objOBSRLoadEntryHdr[0].tot_weight,
                            @tot_palet = objOBSRLoadEntryHdr[0].tot_palet,
                            @maker = user_id,
                            @maker_dt = DateTime.Now.ToString("MM/dd/yyyy"),


                        }, commandType: CommandType.StoredProcedure);

                }
                if (SaveSOLoadBOLDtl(dtSoList) == true)
                {

                    l_bln_status = true;
                }
                else
                { l_bln_status = false; }

                return l_bln_status;
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

        public bool SaveSOLoadBOLDtl(DataTable dtSoList)
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
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_so_load_bol_dtl";
                        sqlBulkCopy.WriteToServer(dtSoList);
                        connection.Close();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {

            }


        }

        public bool SaveSOLoadEtryDtl(DataTable dtSoList)
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
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_so_load_dtl";
                        sqlBulkCopy.WriteToServer(dtSoList);
                        connection.Close();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {

            }


        }
        public OBGetSRBOLConfRpt GetOBSRBOLConfRptByLoadNumber(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetSRBOLConfRpt> ListIRFP = connection.Query<OBGetSRBOLConfRpt>("sp_ob_get_sr_bol_conf_by_load_number_rpt", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                        @p_str_load_doc_id = p_str_load_doc_id,
                        @p_str_load_number = p_str_load_number

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRBOLConfRptData.ListOBGetSRBOLConfRpt = ListIRFP.ToList();
                }
                return objOBSRBOLConfRptData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public OBGetSRBOLConfRpt GetOBBOLByBatch(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id, string p_str_load_doc_id, string p_str_is_same_ship_to)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<OBGetSRBOLConfRpt> ListIRFP = connection.Query<OBGetSRBOLConfRpt>("spGetOBBOLByBatch", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                        @p_str_load_doc_id = p_str_load_doc_id,
                        @p_str_is_same_ship_to = p_str_is_same_ship_to

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objOBSRBOLConfRptData.ListOBGetSRBOLConfRpt = ListIRFP.ToList();
                }
                return objOBSRBOLConfRptData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public int GetOBLoadDocId()
        {

            try
            {
                int p_int_load_doc_id = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ob_get_load_doc_id", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                p_int_load_doc_id = Convert.ToInt32(command.ExecuteScalar());

                if (p_int_load_doc_id > 0)
                {
                    return p_int_load_doc_id;
                }
                else
                {
                    return p_int_load_doc_id;
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

        public string GetOBLoadDocIdBySR(string p_str_cmp_id , string p_str_so_num)
        {
            string l_str_load_doc_id = string.Empty;
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spGetOBLoadDocIdBySR", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@p_str_so_num", SqlDbType.VarChar).Value = p_str_so_num;
                connection.Open();
                DataTable dtLoadDocId = new DataTable();
                dtLoadDocId.Load(command.ExecuteReader());
                if (dtLoadDocId.Rows.Count > 0)
                l_str_load_doc_id = dtLoadDocId.Rows[0][0].ToString();
                return l_str_load_doc_id;
            }
            catch (Exception ex)
            {
                return l_str_load_doc_id;
                throw ex;
            }
            finally
            {

            }
         }
    }
}
