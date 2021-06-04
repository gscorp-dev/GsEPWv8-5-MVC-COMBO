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
    public class RateMasterRepository : IRateMasterRepository
    {
        public void RateMasterDelete(RateMaster objRateMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvc_web_delete_rateMaster";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objRateMaster.cmp_id,
                        @itm_num = objRateMaster.itm_num
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void RateMasterCreateUpdate(RateMaster objRateMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                string l_str_is_auto_ibs = "N";
                if (objRateMaster.is_auto_ibs == true)
                    l_str_is_auto_ibs = "Y";
                const string storedProcedure1 = "proc_web_ratemasterIU";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objRateMaster.cmp_id,
                        @itm_num = objRateMaster.itm_num,
                        @status = objRateMaster.status,
                        @editable = objRateMaster.editable,
                        @itm_name = objRateMaster.itm_name,
                        @type = objRateMaster.type,
                        @catg = objRateMaster.catg,
                        @list_price = objRateMaster.list_price,
                        @price_uom = objRateMaster.price_uom,
                        @last_so_dt = objRateMaster.last_so_dt,
                        @process_id = objRateMaster.ProcessID,
                        @rate_fm_dt = objRateMaster.last_so_dt,
                        @rate_to_dt = objRateMaster.last_so_dt,
                        @opt = objRateMaster.opt_id,
                        @is_auto_ibs = l_str_is_auto_ibs,
                        @ibs_unit = objRateMaster.ibs_unit,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void RateMasterNew(RateMaster objRateMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_web_ratemasterNew";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objRateMaster.cmp_id,
                        @itm_num = objRateMaster.itm_num,
                        @status = objRateMaster.status,                       
                        @itm_name = objRateMaster.itm_name,
                        @type = objRateMaster.type,
                        @catg = objRateMaster.catg,
                        @list_price = objRateMaster.list_price,
                        @price_uom = objRateMaster.price_uom,
                        @last_so_dt = objRateMaster.last_so_dt
                        
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public RateMaster GetRateMasterDetails(RateMaster objRateMaster)
        {
            try    
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var ratetype = objRateMaster.type.Trim();
                    if(ratetype=="ALL")
                    {
                        ratetype = "";
                    }
                    else
                    {
                        ratetype= objRateMaster.type.Trim();
                    }
                    const string storedProcedure1 = "proc_get_mvc_ratemaster_inquiry_dtls";
                    IEnumerable<RateMasterDtl> ListRateMasterLIST = connection.Query<RateMasterDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objRateMaster.cmp_id,
                            @Rate_Type = objRateMaster.type,
                            @Rate_Id_Fm = objRateMaster.itm_num,
                            @Rate_Id_To = objRateMaster.itm_num


                        },
                        commandType: CommandType.StoredProcedure);
                    objRateMaster.ListRateMaster = ListRateMasterLIST.ToList();
                }

                return objRateMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public RateMaster GetRateMasterRptDetails(RateMaster objRateMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var ratetype = objRateMaster.type;
                    if (ratetype == "ALL"|| ratetype==null)
                    {
                        ratetype = "";
                    }
                    else
                    {
                        ratetype = objRateMaster.type.Trim();
                    }
                    const string storedProcedure1 = "proc_get_mvc_ratemaster_rpt";
                    IEnumerable<RateMasterDtl> ListRateMasterRptLIST = connection.Query<RateMasterDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objRateMaster.cmp_id,
                            @Rate_Type = ratetype,
                            @Rate_Id_Fm = objRateMaster.itm_num,
                            @Rate_Id_To = objRateMaster.itm_num

                        },
                        commandType: CommandType.StoredProcedure);
                    objRateMaster.ListRateMasterRpt = ListRateMasterRptLIST.ToList();
                }

                return objRateMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public RateMaster GetRateMasterViewDetails(RateMaster objRateMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_webmvc_RateMaster_View_Dtl";
                    IEnumerable<RateMasterDtl> ListRateMasterViewDtlLIST = connection.Query<RateMasterDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = objRateMaster.cmp_id,
                            @itm_num = objRateMaster.itm_num


                        },
                        commandType: CommandType.StoredProcedure);
                    objRateMaster.ListRateMasterViewDtl = ListRateMasterViewDtlLIST.ToList();
                }

                return objRateMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //CR_3PL_MVC_BL_2018_0220_001 Added By Ravi
        public LookUp GetRateMasterCategory(LookUp objRateMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_MVC_RM_GET_CATEGORY";
                    IEnumerable<LookUp> ListRateMasterViewDtlLIST = connection.Query<LookUp>(storedProcedure1,
                        new
                        {
                            @NAME = objRateMaster.lookuptype


                        },
                        commandType: CommandType.StoredProcedure);
                    objRateMaster.ListLookUpCategoryDtl = ListRateMasterViewDtlLIST.ToList();
                }

                return objRateMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //END
        //CR-180421-001 Added By Nithya
        public RateMaster ExistRate(RateMaster objRateMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "Sp_MVC_Exist_Rate";
                    IEnumerable<RateMasterDtl> ListRateMasterLstRateId = connection.Query<RateMasterDtl>(storedProcedure1,
                        new
                        {
                            @cmpid = objRateMaster.cmp_id,
                            @itmnum = objRateMaster.itm_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objRateMaster.LstRateId = ListRateMasterLstRateId.ToList();
                }

                return objRateMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        //END
        public RateMaster CHECK_RATEID_IS_IN_USE(RateMaster objRateMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "SP_CHECK_RATEID_IS_IN_USE";
                    IEnumerable<RateMasterDtl> ListRateMasterLstRateId = connection.Query<RateMasterDtl>(storedProcedure1,
                        new
                        {
                            @P_STR_CMP_ID = objRateMaster.cmp_id,
                            @P_STR_ITM_NUM = objRateMaster.itm_num,
                            @P_STR_ITM_TYPE = objRateMaster.type,
                        },
                        commandType: CommandType.StoredProcedure);
                    objRateMaster.LstRateId = ListRateMasterLstRateId.ToList();
                }

                return objRateMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public RateDtl GetRateDtlList(RateDtl objRateDtl, string p_str_cmp_id, string p_str_rate_type, string p_str_rate_id_fm, string p_str_rate_id_to)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    IList<RateDtl> ListRateDtl = connection.Query<RateDtl>("proc_get_rate_dtl", new
                    {
                        @p_str_cmp_id = p_str_cmp_id,
                        @p_str_rate_type = p_str_rate_type,
                        @p_str_rate_id_fm = p_str_rate_id_fm,
                        @p_str_rate_id_to = p_str_rate_id_to,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objRateDtl.ListRateDtl = ListRateDtl.ToList();
                }
                return objRateDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetRateDtlListExcel(string p_str_cmp_id, string p_str_rate_type, string p_str_rate_id_fm, string p_str_rate_id_to)
        {
            try
            {
                    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                    SqlCommand command = new SqlCommand();
                    command = new SqlCommand("proc_get_rate_dtl", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@p_str_cmp_id", SqlDbType.VarChar).Value = p_str_cmp_id;
                    command.Parameters.Add("@p_str_rate_type", SqlDbType.VarChar).Value = p_str_rate_type;
                    command.Parameters.Add("@p_str_rate_id_fm", SqlDbType.VarChar).Value = p_str_rate_id_fm;
                    command.Parameters.Add("@p_str_rate_id_to", SqlDbType.VarChar).Value = p_str_rate_id_to;
                    connection.Open();
                    DataTable dtRateMasterSmryTemplate = new DataTable();
                    dtRateMasterSmryTemplate.Load(command.ExecuteReader());
                    return dtRateMasterSmryTemplate;
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
