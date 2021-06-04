using Dapper;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class ItemMasterRepository : IItemMasterRepository
    {

        public ItemMaster fnGetItemConfig(string pstrConfigType)
        {
            ItemMaster objItemMaster = new ItemMaster();
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "spGetItemConfig";
                    IEnumerable<clsItemConfig> ListItemConfig = connection.Query<clsItemConfig>(storedProcedure1,
                        new
                        {
                            @pstrConfigType = pstrConfigType
                        },
                        commandType: CommandType.StoredProcedure);
                    objItemMaster.ListItemConfig = ListItemConfig.ToList();
                }

                return objItemMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ItemMaster fnGetItemPkgServiceg(string pstrCmpId, string pstrItmCode)
        {
            ItemMaster objItemMaster = new ItemMaster();
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "spGetItemPkgService";
                    IEnumerable<clsItemPkgService> ListPkgSrvc = connection.Query<clsItemPkgService>(storedProcedure1,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrItmCode = pstrItmCode
                        },
                        commandType: CommandType.StoredProcedure);
                        objItemMaster.ListPkgSrvc = ListPkgSrvc.ToList();
                }

                return objItemMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public bool SaveSaveItemPkgServicer( string pstrMode, string pstrCmpId, string pstrItmCode, string pstrPkgService)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "spSaveItemPkgService";
                    IEnumerable<BinMaster> ListBinMasterTypeList = connection.Query<BinMaster>(storedProcedure2,
                        new
                        {
                            @pstrMode = pstrMode,
                            @pstrCmpId = pstrCmpId,
                            @pstrItmCode = pstrItmCode,
                            @pstrPkgService = pstrPkgService


                        },
                        commandType: CommandType.StoredProcedure);
                    return true;
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


        public ItemMaster GetItemMasterDetails(ItemMaster objItemMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    
                    const string storedProcedure1 = "SP_GET_ITM_DTL";
                    IEnumerable<ItemMaster> ListItemMasterLIST = connection.Query<ItemMaster>(storedProcedure1,
                        new
                        {
                            @p_str_cmp_id = objItemMaster.cmp_id,
                            @p_str_itm_style = objItemMaster.itm_num,
                            @p_str_itm_Color = objItemMaster.itm_color,
                            @p_str_itm_Size = objItemMaster.itm_size,
                            @p_str_itm_Descr = objItemMaster.itm_name
                        },
                        commandType: CommandType.StoredProcedure);
                    objItemMaster.ListItemMaster = ListItemMasterLIST.ToList();
                }

                return objItemMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ItemMaster ExistItem(ItemMaster objItemMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "Sp_Ma_Itm_Exits";
                    IEnumerable<ItemMaster> ListItemExistLISTs = connection.Query<ItemMaster>(storedProcedure1,
                        new
                        {
                            @cmpid = objItemMaster.cmp_id,
                            @itm_num = objItemMaster.itm_num,
                            @itm_color = objItemMaster.itm_color,
                            @itm_size = objItemMaster.itm_size,
                        },
                        commandType: CommandType.StoredProcedure);
                    objItemMaster.LstItemId = ListItemExistLISTs.ToList();
                }

                return objItemMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string fnAddNew940Item(ItemMaster objItemMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "spAddNew940Item";
                    IEnumerable<string> strItmCode = connection.Query<string>(storedProcedure1,
                        new
                        {
                            @pstrCmpId = objItemMaster.cmp_id,
                            @pstrItmNum = objItemMaster.itm_num,
                            @pstrItmColor = objItemMaster.itm_color,
                            @pstrItmSize = objItemMaster.itm_size,
                            @pstrItmName = objItemMaster.itm_name,
                            @pintPPPK = objItemMaster.ctn_qty
                        },
                        commandType: CommandType.StoredProcedure);
                    return strItmCode.ToList()[0];
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

        public void ItemMasterCreateUpdate(ItemMaster objItemMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_Ma_Itm_Dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpid = objItemMaster.cmp_id,
                        @itm_code = objItemMaster.itm_code,
                        @itmnum = objItemMaster.itm_num,
                        @itmcolor = objItemMaster.itm_color,
                        @itm_size = objItemMaster.itm_size,
                        @length = objItemMaster.Length,
                        @width = objItemMaster.Width,
                        @depth = objItemMaster.Depth,
                        @cube = objItemMaster.Cube,
                        @wgt = objItemMaster.wgt,
                        @wgtuom = "",
                        @availqty = 0,
                        @alocqty   = 0,
                        @shipqty   = 0,
                        @resrvqty   = 0,
                        @boqty   = 0,
                        @qtyuom  = "",
                        @ctage = 0,
                        @prvage  = 0,
                        @lastsodt  = DateTime.Now,
                        @lastpodt  = DateTime.Now,
                        @lasttrandt  = DateTime.Now,
                        @minqty   = 0,
                        @maxqty   = 0,
                        @ctnqty   = objItemMaster.ctn_qty,
                        @minval   = 0,
                        @maxval   = 0,
                        @autofil  = "",
                        @dsgnid  = "",
                        @roylpcnt   = 0,
                        @img  = objItemMaster.image_name,
                        @processid = "",
                        @opt = objItemMaster.Opt_id,
                        @sizeuom  = "",
                        @upccode  = "",
                        @landcost  = "",
                        @listprice  = objItemMaster.list_price,
                        @commpcnt  = 1,
                        @priceuom = "",
                        @sku_id = objItemMaster.sku_id,
                        @is_inner_pack = objItemMaster.is_inner_pack,
                        @inner_pack_ctn_qty = objItemMaster.inner_pack_ctn_qty,
                        @inner_pack_length = objItemMaster.inner_pack_length,
                        @inner_pack_width = objItemMaster.inner_pack_width,
                        @inner_pack_depth = objItemMaster.inner_pack_depth,
                        @inner_pack_cube = objItemMaster.inner_pack_cube,
                        @inner_pack_wgt = objItemMaster.inner_pack_wgt,
                        @pkg_type = objItemMaster.pkg_type,
                        @pkg_size = objItemMaster.pkg_size,
                        @pce_length = objItemMaster.pce_length,
                        @pce_width = objItemMaster.pce_width,
                        @pce_depth = objItemMaster.pce_depth,
                        @pce_cube = objItemMaster.pce_cube,
                        @pce_wgt = objItemMaster.pce_wgt,


                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void ItemMasterDeleteItem(ItemMaster objItemMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_Ma_Itm_Delete";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpid = objItemMaster.cmp_id,
                        @itm_code = objItemMaster.itm_code,
                        @opt = objItemMaster.Opt_id,
                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public void ItemMasterHeaderCreateUpdate(ItemMaster objItemMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_ma_Itm_Hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpId = objItemMaster.cmp_id,
                        @itm_code = objItemMaster.itm_code,
                        @Itmnum = objItemMaster.itm_num,
                        @Itmcolor = objItemMaster.itm_color,
                        @itm_size = objItemMaster.itm_size,
                        @status = objItemMaster.Status,
                        @catgid = objItemMaster.catg_id,
                        @groupid = objItemMaster.group_id,
                        @class = objItemMaster.Class,
                        @type = "",
                        @kititm = objItemMaster.Kit_Itm == true ? "K" : "N",
                        @noninvitm = objItemMaster.NonInventorysItem == true ? 1 : 0,
                        @itmname = objItemMaster.itm_name,
                        @processid = "ADD",
                        @opt = objItemMaster.Opt_id
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public ItemMaster GetItmId(ItemMaster objItemMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_itm_id";
                    IEnumerable<ItemMaster> ListItmIdLIST = connection.Query<ItemMaster>(storedProcedure2,
                        new
                        {
                            @itmid = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objItemMaster.ListItmId = ListItmIdLIST.ToList();
                    objItemMaster.itm_code = objItemMaster.ListItmId[0].itmid;

                }
                return objItemMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        public ItemMaster GetItemMasterViewDetails(ItemMaster objItemMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "Sp_Ma_Itm_FetchItem_dtl";
                    IEnumerable<ItemMasterdtl> ListItemMasterViewDtlLIST = connection.Query<ItemMasterdtl>(storedProcedure1,
                        new
                        {
                            @cmpid = objItemMaster.cmp_id,
                            @itm_code = objItemMaster.itm_code
                        },
                        commandType: CommandType.StoredProcedure);
                    objItemMaster.ListItemMasterViewDtl = ListItemMasterViewDtlLIST.ToList();
                }

                return objItemMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public LookUp GetItemMasterCategory(LookUp objItemMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_MVC_RM_GET_CATEGORY";
                    IEnumerable<LookUp> ListRateMasterViewDtlLIST = connection.Query<LookUp>(storedProcedure1,
                        new
                        {
                            @NAME = objItemMaster.lookuptype


                        },
                        commandType: CommandType.StoredProcedure);
                    objItemMaster.ListLookUpCategoryDtl = ListRateMasterViewDtlLIST.ToList();
                }

                return objItemMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public void ItemMasterKithdr(ItemMaster objItemMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_Add_to_kit_Mast_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objItemMaster.cmp_id,
                        @kit_id = objItemMaster.itm_code,
                        @kit_desc = objItemMaster.Opt_id,
                        @itm_cnt = objItemMaster.Opt_id,
                        @tot_qty = objItemMaster.Opt_id,
                        @process_id = objItemMaster.Opt_id,

                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public void ItemMasterKitdtl(ItemMaster objItemMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_Add_to_kit_Mast_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objItemMaster.cmp_id,
                        @kit_id = objItemMaster.itm_code,
                        @itm_code = objItemMaster.itm_code,
                        @itm_num = objItemMaster.itm_num,
                        @itm_color = objItemMaster.itm_color,
                        @itm_size = objItemMaster.itm_size,
                        @itm_qty = objItemMaster.Opt_id,
                        @process_id = objItemMaster.Opt_id,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public ItemMaster CheckAndDeleteItm(ItemMaster objItemMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "SP_CHECK_ITEMCODE_IS_IN_USE";
                    IEnumerable<ItemMaster> ListItemExistLISTs = connection.Query<ItemMaster>(storedProcedure1,
                        new
                        {
                            @P_STR_CMP_ID = objItemMaster.cmp_id,
                            @P_STR_ITM_CODE = objItemMaster.itm_code,                    
                        },
                        commandType: CommandType.StoredProcedure);
                    objItemMaster.LstItemId = ListItemExistLISTs.ToList();
                }

                return objItemMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public ItemdtlReport GetItemDtlList(ItemdtlReport objItemdtlReport, string p_str_cmp_id)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<ItemdtlReport> ListItemdtl = connection.Query<ItemdtlReport>("sp_ib_get_item_dtl_list", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objItemdtlReport.ListItemdtl = ListItemdtl.ToList();
                }
                return objItemdtlReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetItemDtlListExcelRpt(string p_str_cmp_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_ib_get_item_dtl_list", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                connection.Open();
                DataTable ItemDtlList = new DataTable();
                ItemDtlList.Load(command.ExecuteReader());
                return ItemDtlList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public DataTable GetItemAndStockDimRpt(string p_str_cmp_id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spGetItemAndStockDimRpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                connection.Open();
                DataTable ItemDtlList = new DataTable();
                ItemDtlList.Load(command.ExecuteReader());
                return ItemDtlList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        

        public List<ItemStock> getItemStock(string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string pstrStkStatus)
        {
            try
            {
                List<ItemStock> ListItemStock = new List<ItemStock>();
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "spIvGetStockForDimUpdate";
                    List<ItemStock> ListItemStockData = connection.Query<ItemStock>(storedProcedure1,
                        new
                        {
                            @p_str_cmp_id = p_str_cmp_id,
                            @p_str_itm_num = p_str_itm_num,
                            @p_str_itm_color = p_str_itm_color,
                            @p_str_itm_size = p_str_itm_size,
                            @pstrStkStatus = pstrStkStatus
                        },
                        commandType: CommandType.StoredProcedure).ToList();
                    ListItemStock = ListItemStockData.ToList();
                }

                return ListItemStock.ToList(); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public int GetItmDimUpdtRefNum()
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

        public bool SaveDimUpdate(string pstrCmpId, DataTable ldtDimUpdate)
        {
            bool lblnReturnValue = false;
            int lintRefNum = 0;
            int lintCount = 0;


            try
            {
                lintRefNum = GetItmDimUpdtRefNum();

                lintCount = ldtDimUpdate.Rows.Count;
                for (int i = 0; i < lintCount; i++)
                {
                    ldtDimUpdate.Rows[i]["ref_num"] = lintRefNum.ToString();
                }

                string consString = ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString();

                using (SqlConnection connection = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        sqlBulkCopy.DestinationTableName = "dbo.tbl_iv_itm_cube_update";
                        sqlBulkCopy.WriteToServer(ldtDimUpdate);
                        connection.Close();

                    }

                    connection.Open();
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.CommandText = "spIvSaveDimUpdate";
                        cmd.Parameters.Add("@pstrRefNum", SqlDbType.VarChar).Value = lintRefNum.ToString();
                        cmd.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                    

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
