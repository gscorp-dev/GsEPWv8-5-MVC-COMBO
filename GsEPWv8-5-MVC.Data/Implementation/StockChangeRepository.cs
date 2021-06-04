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
   public class StockChangeRepository : IStockChangeRepository
    {

        public StockChange GetStockChangeDetails(StockChange objStockChange)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                   
                    const string storedProcedure1 = "SP_MVC_IV_STOCK_CHANGE_DTLS";
                    IEnumerable<StockChange> ListStockChangeLIST = connection.Query<StockChange>(storedProcedure1,
                        new
                        {
                            @cmp_id = objStockChange.cmp_id,
                            @itm_num = objStockChange.itm_num,
                            @itm_color = objStockChange.itm_color,
                            @itm_size = objStockChange.itm_size,
                            @search_text = objStockChange.search_text,
                            @ib_doc_id = objStockChange.ib_doc_id,
                            @cont_id = objStockChange.cont_id,
                            @lot_id = objStockChange.lot_id,
                            @loc_id = objStockChange.loc_id,
                            @whs_id = objStockChange.whs_id,
                            @rcvd_dt_fm = objStockChange.rcvd_dt_fm,
                            @rcvd_dt_to = objStockChange.rcvd_dt_to,
                            @po_num = objStockChange.po_num


                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListGetStockChangeDetails = ListStockChangeLIST.ToList();
                }

                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockChange GetItemMoveGridLoadItem(StockChange objStockChange)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "SP_IV_STK_MOVE_FETCH";
                    IEnumerable<StockChange> ListStockChangeLIST = connection.Query<StockChange>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID = objStockChange.cmp_id,
                            @P_STR_ITM_CODE = objStockChange.itm_code,
                            //@P_STR_PALETID = objStockChange.palet_id,
                            @P_STR_LOC_ID = string.Empty,
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListGetItemMoveDetails = ListStockChangeLIST.ToList();
                }

                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockChange GetItemMoveTotQty(StockChange objStockChange)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "SP_MVC_STK_ITEM_MOVE_TOTQTY";
                    IEnumerable<StockChange> ListStockChangeLIST = connection.Query<StockChange>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID = objStockChange.cmp_id,
                            @P_STR_ITMCODE = objStockChange.itm_code,
                            @P_STR_PALETID = objStockChange.palet_id,
                            @P_STR_LOCID = objStockChange.loc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListGetItemMoveTotQty = ListStockChangeLIST.ToList();
                }

                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockChange CheckLotStatus(StockChange objStockChange)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "SP_MVC_STK_ITEM_MOVE_CHECK_STATUS";
                    IEnumerable<StockChange> ListStockChangeLIST = connection.Query<StockChange>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID = objStockChange.cmp_id,                          
                            @P_STR_PALETID = objStockChange.palet_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListCheckLotStatus = ListStockChangeLIST.ToList();
                }

                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockChange GetAdjustDocID(StockChange objStockChange)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "SP_MVC_IV_ADJ_DOC_ID";
                    IEnumerable<StockChange> ListStockChangeLIST = connection.Query<StockChange>(storedProcedure1,
                        new
                        {
                            @adj_doc_id = "",
                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListGetAdjustDocID = ListStockChangeLIST.ToList();
                    objStockChange.adj_doc_id = objStockChange.ListGetAdjustDocID[0].adj_doc_id;
                }

                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockChange GetPalletId(StockChange objStockChange)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_pallet_id";
                    IEnumerable<StockChange> ListPaletIdLIST = connection.Query<StockChange>(storedProcedure2,
                        new
                        {
                            @palet_id = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListPaletId = ListPaletIdLIST.ToList();
                    objStockChange.palet_id = objStockChange.ListPaletId[0].palet_id;

                }
                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public StockChange Ck_Loc_Itm_Avail(StockChange objStockChange)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IV_CK_AVAIL_LOC_AND_LOT";
                    IEnumerable<StockChange> ListPaletIdLIST = connection.Query<StockChange>(storedProcedure2,
                        new
                        {
                            @cmp_id = objStockChange.cmp_id,
                            @itm_code = objStockChange.itm_code,
                            @whs_id = objStockChange.whs_id,
                            @lot_id = objStockChange.lot_id,
                            @rcvd_dt = objStockChange.rcvd_dt,
                            @loc_id = objStockChange.loc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListPaletId = ListPaletIdLIST.ToList();
                    objStockChange.cnt = objStockChange.ListPaletId[0].cnt;

                }
                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public StockChange Update_Rcvd_Qty(StockChange objStockChange)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IV_UPDATE_RCVD_QTY";
                    IEnumerable<StockChange> ListPaletIdLIST = connection.Query<StockChange>(storedProcedure2,
                        new
                        {
                            @opId = objStockChange.optid,
                            @cmp_id = objStockChange.cmp_id,
                            @itm_code = objStockChange.itm_code,
                            @whs_id = objStockChange.whs_id,
                            @lot_id = objStockChange.lot_id,
                            @rcvd_dt = objStockChange.rcvd_dt,
                            @loc_id = objStockChange.loc_id,
                            @avail_qty = objStockChange.itm_qty,
                            @process_id = ""
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListPaletId = ListPaletIdLIST.ToList();                   

                }
                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public StockChange Add_Iv_Itm_Trn_Hdr(StockChange objStockChange)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IV_ADD_IV_ITM_TRN_HDR";
                    IEnumerable<StockChange> ListPaletIdLIST = connection.Query<StockChange>(storedProcedure2,
                        new
                        {
                            @opId = "A",
                            @cmp_id = objStockChange.cmp_id,
                            @itm_code = objStockChange.itm_code,
                            @itm_num = objStockChange.itm_num,
                            @itm_color = objStockChange.itm_color,
                            @itm_size = objStockChange.itm_size,
                            @whs_id = objStockChange.whs_id,
                            @lot_id = objStockChange.lot_id,
                            @loc_id = objStockChange.loc_id,
                            @palet_id = "",
                            @rcvd_dt = objStockChange.rcvd_dt,
                            @pkg_type ="CTN",
                            @pkg_qty = 1,
                            @itm_qty = 1,
                            @avail_qty = objStockChange.itm_qty,
                            @aloc_qty = 0,
                            @ship_qty = 0,
                            @process_id = ""
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListPaletId = ListPaletIdLIST.ToList();
                }
                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public StockChange UPD_TRNIN_TRNHST_PART_TPW(StockChange objStockChange)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IV_UPD_TRNIN_TRNHST_PART_TPW";
                    IEnumerable<StockChange> ListPaletIdLIST = connection.Query<StockChange>(storedProcedure2,
                        new
                        {
                            @Cmp_Id = objStockChange.cmp_id,
                            @Palet_Id = objStockChange.lot_num,
                            @New_Palet_Id = objStockChange.palet_id,
                            @adj_doc_id = objStockChange.adj_doc_id,
                            @pkg_id = objStockChange.pkg_id,
                            @OldLocId = objStockChange.frm_loc,
                            @NewLocId = objStockChange.to_loc,
                            @Process_id = "",
                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListPaletId = ListPaletIdLIST.ToList();

                }
                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public StockChange UPD_LOTDTL_STK_XFER_PART_TPW(StockChange objStockChange)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_IV_UPD_LOTDTL_STK_XFER_PART_TPW";
                    IEnumerable<StockChange> ListPaletIdLIST = connection.Query<StockChange>(storedProcedure2,
                        new
                        {
                            @Cmp_Id = objStockChange.cmp_id,
                            @Palet_Id = objStockChange.lot_num,
                            @New_Palet_Id = objStockChange.palet_id,
                            @adj_doc_id = objStockChange.adj_doc_id,
                            @OldLocId = objStockChange.frm_loc,
                            @NewLocId = objStockChange.to_loc,
                            @Process_id = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListPaletId = ListPaletIdLIST.ToList();                   

                }
                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public StockChange Validate_ItmCode(StockChange objStockChange)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_GET_ITEM_CODE";
                    IEnumerable<StockChange> ListPaletIdLIST = connection.Query<StockChange>(storedProcedure2,
                        new
                        {
                            @cmp_id = objStockChange.cmp_id,
                            @itm_code = objStockChange.itm_code,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListCheckLotStatus = ListPaletIdLIST.ToList();                  

                }
                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public StockChange InsertTempStkMove(StockChange objStockChange)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "SP_IV_Insert_STK_MOVE";
                    IEnumerable<StockChange> ListStockChangeLIST = connection.Query<StockChange>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID = objStockChange.cmp_id,
                            @P_STR_AdjdocId = objStockChange.adj_doc_id,
                            @P_STR_Adjdate = objStockChange.adj_date,
                            @P_STR_ITMCODE = objStockChange.itm_code,
                            @P_STR_WhsID = objStockChange.whs_id,
                            @P_STR_IBDOCID = objStockChange.ib_doc_id,
                            @P_STR_revdate = objStockChange.rcvd_dt,
                            @P_STR_CONTRID = objStockChange.cont_id,
                            @P_STR_LOTID = objStockChange.lot_id,
                            @P_STR_PALETID = objStockChange.lot_num,
                            @P_STR_PONUM = objStockChange.po_num,
                            @P_STR_LOCFRM = objStockChange.frm_loc,
                            @P_STR_CTNqty = objStockChange.pkg_qty,
                            @P_STR_totCTN = objStockChange.tot_ctns,
                            @P_STR_LOCTOID = objStockChange.to_loc,
                            @P_STR_MOVECTNS = objStockChange.move_ctns,
                            @P_STR_USERID = objStockChange.userId,
                            @P_STR_PROCESSID = "ADD",
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListGetItemMoveDetails = ListStockChangeLIST.ToList();
                }

                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public StockChange ItemXGetLocDetails(string term, string cmp_id)
        {
            try
            {
                StockChange objCustDtl = new StockChange();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_Loc_dtl";
                    IList<StockChange> ListIRFP = connection.Query<StockChange>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmp_id,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objCustDtl.LstItmxlocdtl = ListIRFP.ToList();
                }
                return objCustDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public int GetAdjDocId(string enitity_code)
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

        public string SaveStkMove(string p_str_cmp_id,string p_str_user_id, DataTable p_dt_item_stock_move)
        {
             string l_str_adj_doc_id = string.Empty;
            try
            {
                l_str_adj_doc_id = Convert.ToString(GetAdjDocId("adj_doc_id"));
                foreach (DataRow row in p_dt_item_stock_move.Rows)
                {
                    if (row["po_num"] is DBNull)
                    {
                        row["po_num"] = string.Empty;
                    }
                    row["adj_doc_id"] = l_str_adj_doc_id;
                    row["adj_dt"] = DateTime.Now.ToString("MM/dd/yyyy");
                    row["user_id"] = p_str_user_id;
                    row["process_id"] = "Add";
                    
                }

                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();
                using (SqlConnection connection = new SqlConnection(consString))
                {
                    connection.Open();
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        sqlBulkCopy.DestinationTableName = "tbl_temp_iv_stk_move";
                        sqlBulkCopy.WriteToServer(p_dt_item_stock_move);
                    }

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "SP_IV_STK_MOVE_SAVE";
                        cmd.Parameters.Add("@P_STR_CMP_ID", SqlDbType.NVarChar).Value = p_str_cmp_id;
                        cmd.Parameters.Add("@P_STR_ADJ_DOC_ID", SqlDbType.NVarChar).Value = l_str_adj_doc_id;
                        cmd.Parameters.Add("@P_STR_ERROR_DESC", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                    return "OK";
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
        public StockChange UpdateTempStkMove(StockChange objStockChange)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "SP_IV_Insert_STK_MOVE";
                    IEnumerable<StockChange> ListStockChangeLIST = connection.Query<StockChange>(storedProcedure1,
                        new
                        {
                            @P_STR_CMPID = objStockChange.cmp_id,
                            @P_STR_AdjdocId = objStockChange.adj_doc_id,
                            @P_STR_Adjdate = objStockChange.adj_date,
                            @P_STR_ITMCODE = objStockChange.itm_code,
                            @P_STR_WhsID = objStockChange.whs_id,
                            @P_STR_IBDOCID = objStockChange.ib_doc_id,
                            @P_STR_revdate = objStockChange.rcvd_dt,
                            @P_STR_CONTRID = objStockChange.cont_id,
                            @P_STR_LOTID = objStockChange.lot_id,
                            @P_STR_PALETID = objStockChange.lot_num,
                            @P_STR_PONUM = objStockChange.po_num,
                            @P_STR_LOCFRM = objStockChange.frm_loc,
                            @P_STR_CTNqty = objStockChange.tot_qty,
                            @P_STR_totCTN = objStockChange.tot_ctns,
                            @P_STR_LOCTOID = objStockChange.to_loc,
                            @P_STR_MOVECTNS = objStockChange.move_ctns,
                            @P_STR_USERID = objStockChange.userId,
                            @P_STR_PROCESSID = "ADD",
                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListGetItemMoveDetails = ListStockChangeLIST.ToList();
                }

                return objStockChange;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public StockChange fnGetStockForMoveByIbDocId(StockChange objStockChange)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "spGetStockForMoveByIbDocId";
                    IEnumerable<StockChange> ListStockChangeLIST = connection.Query<StockChange>(storedProcedure1,
                        new
                        {
                            @pstrCmpId = objStockChange.cmp_id,
                            @pstrIbDocId = objStockChange.ib_doc_id,
                            @pstrCntrId = objStockChange.cont_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objStockChange.ListGetStockChangeDetails = ListStockChangeLIST.ToList();
                }

                return objStockChange;
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
