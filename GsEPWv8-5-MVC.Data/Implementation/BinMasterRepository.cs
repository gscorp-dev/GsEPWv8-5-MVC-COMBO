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
   public  class BinMasterRepository : IBinMasterRepository
    {
        string g_str_cmp_id;

        public clsItemPcsDim GetBinspGetItemPcsDimDtl(string pstrCmpId, string pstrItmNum, string pstrItmColor, string pstrItmSize)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "spGetItemPcsDimDtl";
                    IEnumerable<clsItemPcsDim> ListBinMasterPcsDim = connection.Query<clsItemPcsDim>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrItmNum = pstrItmNum,
                            @pstrItmColor = pstrItmColor,
                            @pstrItmSize = pstrItmSize

                        },
                        commandType: CommandType.StoredProcedure);
                        return ListBinMasterPcsDim.ToList()[0];
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

        public BinMaster GetBinMasterInquiryDetails(BinMaster objBinMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objBinMaster.cmp_id;
                    g_str_cmp_id = cmp_id;
            

                    const string storedProcedure2 = "spEcomBinInquiry";
                    IEnumerable<clsBinMater> ListBinMasterTypeList = connection.Query<clsBinMater>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = objBinMaster.cmp_id,
                            @pstrBinId = objBinMaster.bin_id,
                            @pstrBinDesc = objBinMaster.bin_desc,
                            @pstrWhsId = objBinMaster.whs_id,
                            @pstrItmNum = objBinMaster.itm_num,
                            @pstrItmColor = objBinMaster.itm_color,
                            @pstrItmSize = objBinMaster.itm_size,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objBinMaster.ListBinMasterinqury = ListBinMasterTypeList.ToList();

                    return objBinMaster;
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

        public BinMaster fnGetBinMaster(string pstrCmpId, string pstrBinId)
        {
            BinMaster objBinMaster = new BinMaster();
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                  

                    const string storedProcedure2 = "spGetBinMaster";
                    IEnumerable<clsBinMater> ListBinMaster = connection.Query<clsBinMater>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrBinId = pstrBinId,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objBinMaster.ListBinMasterinqury = ListBinMaster.ToList();

                    return objBinMaster;
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

  
        public int fnCheckBinMasterExists(string pstrCmpId, string pstrBinId)
        {
            try
            {
                int lintBinCount = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spCheckBinMasterExists", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                command.Parameters.Add("@pstrBinId", SqlDbType.VarChar).Value = pstrBinId;
                connection.Open();
                lintBinCount = Convert.ToInt32(command.ExecuteScalar());
                return lintBinCount;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public int fnCheckBinStyleExists(string pstrCmpId, string pstrItmCode )
        {
            try
            {
                int lintStyleCount = 0;
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("spCheckBinStyleExists", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@pstrCmpId", SqlDbType.VarChar).Value = pstrCmpId;
                command.Parameters.Add("@pstrItmCode", SqlDbType.VarChar).Value = pstrItmCode;
                connection.Open();
                lintStyleCount = Convert.ToInt32(command.ExecuteScalar());
                return lintStyleCount;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public bool SaveBinMaster(clsBinMater objBinMater, string pstrMode)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objBinMater.cmp_id;
                    BinMaster objBinMasterCategory = new BinMaster();

                    const string storedProcedure2 = "spSaveBinMaster";
                    IEnumerable<BinMaster> ListBinMasterTypeList = connection.Query<BinMaster>(storedProcedure2,
                        new
                        {
                            @pstrMode = pstrMode,
                            @pstrCmpId = objBinMater.cmp_id,
                            @pstrWhsId = objBinMater.whs_id,
                            @pstrBinId = objBinMater.bin_id,
                            @pstrBinType = objBinMater.bin_type,
                            @pstrBinLoc = objBinMater.bin_id,
                            @pstrBinDesc = objBinMater.bin_desc,
                            @pstrStatus = objBinMater.status,
                            @pstrItmCode = objBinMater.itm_code,
                            @pstrItmNum = objBinMater.itm_num,
                            @pstrItmColor = objBinMater.itm_color,
                            @pstrItmSize = objBinMater.itm_size,
                            @pstrItmName = objBinMater.itm_name,
                            @pstrBinLength = objBinMater.bin_length,
                            @pstrBinWidth = objBinMater.bin_width,
                            @pstrBinHeight = objBinMater.bin_height,
                            @pstrBinCube = objBinMater.bin_cube,
                            @pstrBinWgt = objBinMater.bin_wgt,
                            @pstrMinQty = objBinMater.min_qty,
                            @pstrMaxQty = objBinMater.max_qty,
                            @pstrOrdrQty = objBinMater.ordr_qty,
                            @pstrQtyUom = objBinMater.qty_uom,
                            @pstrPrice = objBinMater.price,
                            @pstrQPM = objBinMater.qpm,
                            @pstrLeadTime = objBinMater.lead_time,
                            @pintBinPPK = objBinMater.bin_ppk,
                            @pdecPcsLength = objBinMater.pce_length,
                            @pdecPcsWidth = objBinMater.pce_width,
                            @pdecPcsDepth = objBinMater.pce_depth,
                            @pdecPcsCube = objBinMater.pce_cube,
                            @pdecPcsWgt = objBinMater.pce_wgt,
                            @pstrBinDt = objBinMater.bin_dt,
                            @pstrUserId = objBinMater.user_id,
                            @pstrProcessId =objBinMater.process_id,


                        },
                        commandType: CommandType.StoredProcedure);
                    //objBinMaster.ListBinMasterinqury = ListBinMasterTypeList.ToList();

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

        public bool fnInsertBinLocByIBDocId(string  pstrCmpId, string pstrWhsId, string pstrLocId, string pstrItmCode, string pstrItmName)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "spInsertBinLocByIBDocId";
                    IEnumerable<BinMaster> ListBinMasterTypeList = connection.Query<BinMaster>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrWhsId = pstrWhsId,
                            @pstrLocId = pstrLocId,
                            @pstrItmCode = pstrItmCode,
                            @pstrItmName = pstrItmName
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


    }
}
