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
   public class eComSR940Repository : IeComSR940Repository
    {
        //public eComSR940 GeteComSR940Details(eComSR940 objeComSR940)
        //{
        //    try
        //    {
        //        eComSR940 objCustDtl = new eComSR940();

        //        using (IDbConnection connection = ConnectionManager.OpenConnection())
        //        {
        //            const string SearchCustDtls = "proc_get_mvcweb_temp_doc_entry";
        //            IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
        //            {
        //                //@cmp_id = objeComSR940.cmp_id,
        //                //@ib_doc_id = objeComSR940.ib_doc_id,
        //            }, commandType: CommandType.StoredProcedure).ToList();
        //            objeComSR940.ListeCom940SRUploadDtl = ListIRFP.ToList();
        //        }
        //        return objeComSR940;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {

        //    }
        //}
        public eComSR940 GetSaveeComSR940Details(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_WEB_SAVE_SHIP_REQUEST_940_DTL";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @CompID = objeComSR940.CompID,
                        @BatchNo = objeComSR940.BatchNo,
                        @So_Num = objeComSR940.so_num,
                        @Store = objeComSR940.Store,
                        @Dept = objeComSR940.Dept,
                        @Itm_Line = objeComSR940.ItemLine,
                        @Itm_Code = objeComSR940.Itm_Code,
                        @Itm_Num = objeComSR940.Style,
                        @Ordr_Qty = objeComSR940.StyleQty,
                        @Ordr_Ctns = objeComSR940.StyleCarton,
                        @Itm_Qty = objeComSR940.StylePPK,
                        @Ctn_Qty = objeComSR940.StylePPK,
                        @Cube = objeComSR940.StyleCube,
                        @Wgt = objeComSR940.StyleWgt,
                        @ReqDt = objeComSR940.ReqDt,
                        @StartDt = objeComSR940.StartDt,
                        @CancelDt = objeComSR940.CancelDt,
                        @Cust_Sku = objeComSR940.CustSKU,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListeCom940SRUploadDtl = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 CheckExistSR940Style(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();
                string l_str_Itm_Code = string.Empty;
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (var conn = new SqlConnection(connString))
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
                                cmd.CommandText = "SP_MVC_GET_WEB_CHECKIF_STYLE_EXIST";
                                cmd.Parameters.Add("@CompID", SqlDbType.VarChar).Value = objeComSR940.cmp_id;
                                cmd.Parameters.Add("@StyleID", SqlDbType.VarChar).Value = (objeComSR940.Style == null || objeComSR940.Style == "") ? null : objeComSR940.Style;
                                cmd.Parameters.Add("@ColorID", SqlDbType.VarChar).Value = "-"; 
                                cmd.Parameters.Add("@SizeID", SqlDbType.VarChar).Value = "-"; 
                                cmd.Parameters.Add("@StyleLength", SqlDbType.VarChar).Value = null; 
                                cmd.Parameters.Add("@StyleWidth", SqlDbType.VarChar).Value = null; 
                                cmd.Parameters.Add("@StyleHeight", SqlDbType.VarChar).Value = null; 
                                cmd.Parameters.Add("@StyleCube", SqlDbType.VarChar).Value = (objeComSR940.StyleCube == 0 || objeComSR940.StyleCube == 0) ? 0 : objeComSR940.StyleCube;
                                cmd.Parameters.Add("@StyleWgt", SqlDbType.VarChar).Value = (objeComSR940.StyleWgt == 0 || objeComSR940.StyleWgt == 0) ? 0 : objeComSR940.StyleWgt;

                                cmd.Parameters.Add("@StyleDesc", SqlDbType.VarChar).Value = (objeComSR940.StyleDesc == null || objeComSR940.StyleDesc == "") ? null : objeComSR940.StyleDesc;
                                cmd.Parameters.Add("@Item_Code", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                                l_str_Itm_Code = cmd.Parameters["@Item_Code"].Value.ToString();
                                objeComSR940.Itm_Code = l_str_Itm_Code;


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


                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }



        }

        public eComSR940 GetSaveShipRequest_hdr(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();
                string l_str_so_num = string.Empty;
                DateTime l_dt_ReqDt ;
                string l_str_ReqDt = string.Empty;
                string l_str_StartDt = string.Empty;
                string l_str_CancelDt = string.Empty;
                string l_str_CreatedOn = string.Empty;
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (var conn = new SqlConnection(connString))
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
                                cmd.CommandText = "SP_MVC_PROC_GET_WEB_SAVE_SHIP_REQUEST_940_HDR";
                                cmd.Parameters.Add("@CompID", SqlDbType.NVarChar).Value = objeComSR940.CompID;
                                cmd.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = (objeComSR940.BatchNo == null || objeComSR940.BatchNo == "") ? null : objeComSR940.BatchNo;
                                cmd.Parameters.Add("@CustID", SqlDbType.NVarChar).Value = (objeComSR940.CustID == null || objeComSR940.CustID == "") ? null : objeComSR940.CustID;
                                cmd.Parameters.Add("@CustName", SqlDbType.NVarChar).Value = (objeComSR940.CustName == null || objeComSR940.CustName == "") ? null : objeComSR940.CustName;
                                cmd.Parameters.Add("@Store", SqlDbType.NVarChar).Value = (objeComSR940.Store == null || objeComSR940.Store == "") ? null : objeComSR940.Store;
                                cmd.Parameters.Add("@Dept", SqlDbType.NVarChar).Value = (objeComSR940.Dept == null || objeComSR940.Dept == "") ? null : objeComSR940.Dept;
                                cmd.Parameters.Add("@SOID", SqlDbType.NVarChar).Value = (objeComSR940.SOID == null || objeComSR940.SOID == "") ? null : objeComSR940.SOID;
                                cmd.Parameters.Add("@RelID", SqlDbType.NVarChar).Value = (objeComSR940.RelID == null || objeComSR940.RelID == "") ? null : objeComSR940.RelID;
                                cmd.Parameters.Add("@CustPO", SqlDbType.NVarChar).Value = (objeComSR940.CustPO == null || objeComSR940.CustPO == "") ? null : objeComSR940.CustPO;
                                l_dt_ReqDt = objeComSR940.ReqDt;
                                l_str_ReqDt = l_dt_ReqDt.ToString("MM/dd/yyyy");
                                if (l_str_ReqDt == "01/01/0001")
                                {
                                    cmd.Parameters.Add("@ReqDt", SqlDbType.DateTime).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@ReqDt", SqlDbType.DateTime).Value = objeComSR940.ReqDt;
                                }
                                l_str_StartDt = objeComSR940.StartDt.ToString("MM/dd/yyyy");
                                if (l_str_StartDt == "01/01/0001")
                                {
                                    cmd.Parameters.Add("@StartDt", SqlDbType.DateTime).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@StartDt", SqlDbType.DateTime).Value = objeComSR940.StartDt;
                                }
                                l_str_CancelDt = objeComSR940.CancelDt.ToString("MM/dd/yyyy");

                                if (l_str_CancelDt == "01/01/0001")
                                {
                                    cmd.Parameters.Add("@CancelDt", SqlDbType.DateTime).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@CancelDt", SqlDbType.DateTime).Value = objeComSR940.CancelDt;
                                }
                                cmd.Parameters.Add("@LineCount", SqlDbType.Int).Value = (objeComSR940.dtl_Count == 0 || objeComSR940.dtl_Count == 0) ? 0 : objeComSR940.dtl_Count;
                                cmd.Parameters.Add("@TtalQTY", SqlDbType.Int).Value = (objeComSR940.TtalQty == 0 || objeComSR940.TtalQty == 0) ? 0 : objeComSR940.TtalQty;
                                cmd.Parameters.Add("@ShipVia", SqlDbType.NVarChar).Value = (objeComSR940.ShipVia == null || objeComSR940.ShipVia == "") ? null : objeComSR940.ShipVia;
                                cmd.Parameters.Add("@ShipName", SqlDbType.NVarChar).Value = (objeComSR940.ShipName == null || objeComSR940.ShipName == "") ? null : objeComSR940.ShipName;
                                cmd.Parameters.Add("@ShipAddr1", SqlDbType.NVarChar).Value = (objeComSR940.ShipAdd1 == null || objeComSR940.ShipAdd1 == "") ? null : objeComSR940.ShipAdd1;
                                cmd.Parameters.Add("@ShipAddr2", SqlDbType.NVarChar).Value = (objeComSR940.ShipAdd2 == null || objeComSR940.ShipAdd2 == "") ? null : objeComSR940.ShipAdd2;
                                cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = (objeComSR940.City == null || objeComSR940.City == "") ? null : objeComSR940.City;
                                cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = (objeComSR940.State == null || objeComSR940.State == "") ? null : objeComSR940.State;
                                cmd.Parameters.Add("@ZipCode", SqlDbType.NVarChar).Value = (objeComSR940.ZipCode == null || objeComSR940.ZipCode == "") ? null : objeComSR940.ZipCode;
                                cmd.Parameters.Add("@NoteHdr", SqlDbType.NVarChar).Value = (objeComSR940.NoteHdr == null || objeComSR940.NoteHdr == "") ? null : objeComSR940.NoteHdr;
                                cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = (objeComSR940.CreatedBy == null || objeComSR940.CreatedBy == "") ? null : objeComSR940.CreatedBy;

                                l_str_CreatedOn = objeComSR940.CreatedOn.ToString("MM/dd/yyyy");
                                if (l_str_CreatedOn == "01/01/0001")
                                {
                                    cmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = objeComSR940.CreatedOn;
                                }
                                cmd.Parameters.Add("@new_SO_NUM", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                               
                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                                l_str_so_num = cmd.Parameters["@new_SO_NUM"].Value.ToString();
                                objeComSR940.so_num = l_str_so_num;
                                

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

               
             return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 CheckExistSalesOrder(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();
                string l_str_so_no= string.Empty;
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (var conn = new SqlConnection(connString))
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
                                cmd.CommandText = "SP_MVC_PROC_GET_WEB_CHECKIF_SALESORDER_EXIST";
                                cmd.Parameters.Add("@CompID", SqlDbType.VarChar).Value = objeComSR940.cmp_id;
                                cmd.Parameters.Add("@CustID", SqlDbType.VarChar).Value = (objeComSR940.CustID == null || objeComSR940.CustID == "") ? null : objeComSR940.CustID;
                                cmd.Parameters.Add("@CustPO", SqlDbType.VarChar).Value = (objeComSR940.CustPO == null || objeComSR940.CustPO == "") ? null : objeComSR940.CustPO;
                                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = (objeComSR940.SOID == null || objeComSR940.SOID == "") ? null : objeComSR940.SOID;
                                cmd.Parameters.Add("@SO_Num", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                                cmd.Connection = conn;
                                cmd.Transaction = tran as SqlTransaction;
                                cmd.ExecuteNonQuery();
                                l_str_so_no = cmd.Parameters["@SO_Num"].Value.ToString();
                                objeComSR940.so_num = l_str_so_no;


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


                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }



        }
        public eComSR940 AddECom940SRUploadFileNAme(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();
                string l_str_so_no = string.Empty;
                string connString = ConfigurationManager.ConnectionStrings["GensoftConnection"].ConnectionString;
                using (var conn = new SqlConnection(connString))
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
                                cmd.CommandText = "SP_MVC_ADD_ECOM_940_TABLE";
                                cmd.Parameters.Add("@cmp_id", SqlDbType.VarChar).Value = objeComSR940.cmp_id;
                                cmd.Parameters.Add("@file_name", SqlDbType.VarChar).Value = (objeComSR940.file_name == null || objeComSR940.file_name == "") ? null : objeComSR940.file_name;
                                cmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = (objeComSR940.user_id == null || objeComSR940.user_id == "") ? null : objeComSR940.user_id;

                              string  l_str_StartDt = objeComSR940.uploaded_date.ToString();
                                if (l_str_StartDt == "01/01/0001")
                                {
                                    cmd.Parameters.Add("@uploaded_date", SqlDbType.DateTime).Value = DBNull.Value;
                                }
                                else
                                {
                                    cmd.Parameters.Add("@uploaded_date", SqlDbType.DateTime).Value = objeComSR940.uploaded_date;
                                }
                                cmd.Parameters.Add("@process_status", SqlDbType.VarChar).Value = (objeComSR940.process_status == null || objeComSR940.process_status == "") ? null : objeComSR940.process_status;
                                cmd.Parameters.Add("@error_desc", SqlDbType.VarChar).Value = (objeComSR940.error_desc == null || objeComSR940.error_desc == "") ? null : objeComSR940.error_desc;
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


                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }



        }
        public eComSR940 CheckExistSRUploadFile(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_CHECK_EXIST_940_FILE";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @cmpid = objeComSR940.cmp_id,
                    @file_name = (objeComSR940.file_name == null || objeComSR940.file_name == "") ? null : objeComSR940.file_name,
                    @user_id = (objeComSR940.user_id == null || objeComSR940.user_id == "") ? null : objeComSR940.user_id


                }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListeCom940SRUploadDtl = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 ECom940SRUploadRpt(eComSR940 objeComSR940)
        {
            try
           {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_940SR_UPLOAD_RPT";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @cmp_id = objeComSR940.cmp_id,
                        @batch_no = (objeComSR940.BatchNo == null || objeComSR940.BatchNo == "") ? null : objeComSR940.BatchNo,


                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListeCom940SRUploadDtlRpt = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 GetSaveeComSR940TempDetails(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_GET_WEB_SAVE_SHIP_REQUEST_940_TEMP_DTL";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @CompID = objeComSR940.CompID,
                        @BatchNo = objeComSR940.BatchNo,
                        @So_Num = objeComSR940.so_num,
                        @cust_ord_no = objeComSR940.cust_ordr_num,
                        @CustID = objeComSR940.cust_id,
                        @Itm_Line = objeComSR940.ItemLine,
                        @Itm_Code = objeComSR940.Itm_Code,
                        @Itm_Num = objeComSR940.Style,
                        @Ordr_Qty = objeComSR940.StyleQty,
                        @Ordr_Ctns = objeComSR940.StyleCarton,
                        @Itm_Qty = objeComSR940.StylePPK,
                        @Ctn_Qty = objeComSR940.StylePPK,
                        @Cube = objeComSR940.StyleCube,
                        @Wgt = objeComSR940.StyleWgt,
                        @StyleDesc = objeComSR940.StyleDesc,
                        @CustSku = objeComSR940.CustSKU

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListeCom940SRUploadDtl = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 DeleteTempTable(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_DELETE_940SR_TEMP_TBL";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @P_STR_CMP_ID = objeComSR940.cmp_id,
                       

                    }, commandType: CommandType.StoredProcedure).ToList();
                  //  objeComSR940.ListeCom940SRUploadDtl = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 GetEcom940Inq(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_940SR_UPLOAD_INQUIRY";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @cmp_id = objeComSR940.cmp_id,
                        @File_name=objeComSR940.file_name,
                        @batch_no = objeComSR940.quote_num,
                        @UploaddtFm = objeComSR940.so_dtFm,
                        @UploaddtTo = objeComSR940.so_dtTo,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListeCom940SRUploadDtl = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 GetEcom940HdrCount(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_940SR_HDR_COUNT";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @cmp_id = objeComSR940.cmp_id,
                        @batch_no = objeComSR940.quote_num,
                        @UploaddtFm = objeComSR940.so_dtFm,
                        @UploaddtTo = objeComSR940.so_dtTo,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListEcomError = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 GetEcom940InqRpt(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_940SR_UPLOAD_INQUIRY_RPT";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @cmp_id = objeComSR940.cmp_id,
                        @File_name= objeComSR940.file_name,
                        @batch_no = objeComSR940.quote_num,
                        @UploaddtFm = objeComSR940.so_dtFm,
                        @UploaddtTo = objeComSR940.so_dtTo,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListeCom940SRUploadDtlRpt = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 CheckExistBatchId(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_OB_940SR_BATCHID_EXIST";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @cmp_id = objeComSR940.cmp_id,
                        @batch_no = objeComSR940.BatchNo,                       
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListCheckExistBatchNo = ListIRFP.ToList();
                }
                return objeComSR940;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public eComSR940 deleteExistBatchId(eComSR940 objeComSR940)
        {
            try
            {
                eComSR940 objCustDtl = new eComSR940();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "SP_MVC_SR940_Delete_OLD_BATCHID";
                    IList<eComSR940> ListIRFP = connection.Query<eComSR940>(SearchCustDtls, new
                    {
                        @p_str_cmp_id = objeComSR940.cmp_id,
                        @p_str_batchId = objeComSR940.BatchNo,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objeComSR940.ListCheckExistBatchNo = ListIRFP.ToList();
                }
                return objeComSR940;
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
