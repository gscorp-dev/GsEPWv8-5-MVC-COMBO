using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class DataUpdateRepository:IDataUpdateRepository
    {
        public DataUpdate CheckIbDocIdExist(DataUpdate ObjDataUpdate)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "sp_ib_check_ib_doc_id_exist";
                    IEnumerable<DataUpdate> ListUserDetailsLST = connection.Query<DataUpdate>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = ObjDataUpdate.cmp_id,
                            @P_STR_IB_DOC_ID=ObjDataUpdate.ib_doc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    ObjDataUpdate.ListCheckIBDocIDExist = ListUserDetailsLST.ToList();

                    return ObjDataUpdate;
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
        public DataUpdate CheckExistItem(DataUpdate ObjDataUpdate)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "sp_check_itm_exist";
                    IEnumerable<DataUpdate> ListUserDetailsLST = connection.Query<DataUpdate>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = ObjDataUpdate.cmp_id,
                            @P_STR_ITM_NUM = ObjDataUpdate.itm_num,
                            @P_STR_ITM_COLOR = ObjDataUpdate.itm_color,
                            @P_STR_ITM_SIZE = ObjDataUpdate.itm_size,
                        },
                        commandType: CommandType.StoredProcedure);
                    ObjDataUpdate.ListCheckItmExist = ListUserDetailsLST.ToList();

                    return ObjDataUpdate;
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
        public DataUpdate UpdateIBDocRcvdDate(DataUpdate ObjDataUpdate)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_update_ib_doc_rcvd_dt";
                    IEnumerable<DataUpdate> ListUserDetailsLST = connection.Query<DataUpdate>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = ObjDataUpdate.cmp_id,
                            @P_STR_IB_DOC_ID = ObjDataUpdate.ib_doc_id,
                            @P_STR_IB_DOC_RCVD_DT = ObjDataUpdate.ib_doc_new_rcvd_dt,
                        },
                        commandType: CommandType.StoredProcedure);
                    ObjDataUpdate.ListCheckIBDocIDExist = ListUserDetailsLST.ToList();

                    return ObjDataUpdate;
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

        public DataUpdate GetItemDetails(string term, string cmp_id)
        {
            try
            {
                DataUpdate ObjDataUpdate = new DataUpdate();
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_ma_get_itm_dtl";
                    IList<DataUpdate> ListIRFP = connection.Query<DataUpdate>(SearchCustDtls, new
                    {

                        @P_STR_COMP_ID = cmp_id,
                        @P_STR_SEARCH_TEXT = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    ObjDataUpdate.LstItmdtl = ListIRFP.ToList();
                }
                return ObjDataUpdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataUpdate GetItemList(DataUpdate ObjDataUpdate)
        {
            try
            {
             
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "sp_ma_get_itm_list";
                    IList<DataUpdate> ListIRFP = connection.Query<DataUpdate>(SearchCustDtls, new
                    {
                        @P_STR_COMP_ID = ObjDataUpdate.cmp_id,
                        @P_STR_ITM_NUM = ObjDataUpdate.itm_num,
                        @P_STR_ITM_COLOR = ObjDataUpdate.itm_color,
                        @P_STR_ITM_SIZE = ObjDataUpdate.itm_size,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    ObjDataUpdate.LstItm = ListIRFP.ToList();
                }
                return ObjDataUpdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataUpdate UpdateItemDetails(DataUpdate ObjDataUpdate)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "sp_ma_update_itm_dtl";
                    IEnumerable<DataUpdate> ListUserDetailsLST = connection.Query<DataUpdate>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = ObjDataUpdate.cmp_id,
                            @P_STR_ITM_NUM = ObjDataUpdate.itm_num,
                            @P_STR_ITM_COLOR = ObjDataUpdate.itm_color,
                            @P_STR_ITM_SIZE = ObjDataUpdate.itm_size,
                            @P_STR_ITM_LENGTH = ObjDataUpdate.length,
                            @P_STR_ITM_WIDTH = ObjDataUpdate.width,
                            @P_STR_ITM_DEPTH = ObjDataUpdate.depth,
                            @P_STR_ITM_CUBE = ObjDataUpdate.cube,
                            @P_STR_ITM_WGT = ObjDataUpdate.wgt,
                        },
                        commandType: CommandType.StoredProcedure);
                    ObjDataUpdate.ListCheckIBDocIDExist = ListUserDetailsLST.ToList();

                    return ObjDataUpdate;
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
