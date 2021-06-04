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
    public class WhsMasterRepository : IWhsMasterRepository
    {

        public void SaveWhsMasterHdr(WhsMaster objWhsMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_insert_whs_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_STR_CMP_ID = objWhsMaster.cmp_id,
                        @P_WHS_ID = objWhsMaster.whs_id,
                        @P_WHS_NAME = objWhsMaster.whs_name,
                        @P_STATUS    = objWhsMaster.status,
                        @P_START_DT = objWhsMaster.StartDate,
                        @P_LAST_CHG_DT = objWhsMaster.lastDate,
                        @P_ATTN = objWhsMaster.attn,
                        @P_MAIL_NAME = objWhsMaster.mail_name,
                        @P_ADDR_LINE1 = objWhsMaster.addr_line1,
                        @P_ADDR_LINE2 = objWhsMaster.addr_line2,
                        @P_CITY = objWhsMaster.city,
                        @P_STATE_ID = objWhsMaster.state_id,
                        @P_POST_CODE = objWhsMaster.post_code,
                        @P_CNTRY_ID = objWhsMaster.cntry_id,
                        @P_TEL = objWhsMaster.tel,
                        @P_CELL = objWhsMaster.cell,
                        @P_FAX = objWhsMaster.fax,
                        @P_EMAIL = objWhsMaster.email,
                        @P_WEB = objWhsMaster.web,
                        @P_NOTES = objWhsMaster.notes,
                        @P_DFT_WHS = objWhsMaster.dft_whs,
                        @P_PROCESS_ID = objWhsMaster.process_id,
                        @P_STR_ACTION= objWhsMaster.action_type

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public WhsMaster GetWhsMasterDetails(WhsMaster objWhsMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                   
                    const string storedProcedure1 = "sp_ma_get_whs_hdr";
                    IEnumerable<WhsMasterDtl> ListWhsMasterLIST = connection.Query<WhsMasterDtl>(storedProcedure1,
                        new
                        {
                            @P_STR_CMP_ID = objWhsMaster.cmp_id,
                            @P_STR_WHS_ID = objWhsMaster.whs_id,
                            @P_STR_WHS_NAME = objWhsMaster.whs_name

                        },
                        commandType: CommandType.StoredProcedure);
                    objWhsMaster.ListWhsMaster = ListWhsMasterLIST.ToList();
                }

                return objWhsMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public WhsMaster CheckWhsIDIsExist(WhsMaster objWhsMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "sp_ma_chk_whs_is_exist";
                    IEnumerable<WhsMasterDtl> ListWhsMasterLIST = connection.Query<WhsMasterDtl>(storedProcedure1,
                        new
                        {
                            @P_STR_CMP_ID = objWhsMaster.cmp_id,
                            @P_STR_WHS_ID = objWhsMaster.whs_id,                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objWhsMaster.LstCheckWhsId = ListWhsMasterLIST.ToList();
                }

                return objWhsMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public WhsMaster CheckWhsIDInUse(WhsMaster objWhsMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "sp_ma_chk_whs_in_use";
                    IEnumerable<WhsMasterDtl> ListWhsMasterLIST = connection.Query<WhsMasterDtl>(storedProcedure1,
                        new
                        {
                            @P_STR_CMP_ID = objWhsMaster.cmp_id,
                            @P_STR_WHS_ID = objWhsMaster.whs_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objWhsMaster.LstCheckWhsId = ListWhsMasterLIST.ToList();
                }

                return objWhsMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public WhsMaster GetPickCompanyDetails(WhsMaster objWhsMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "sp_ma_get_cmp_id";
                    IEnumerable<WhsMasterDtl> Listcmp = connection.Query<WhsMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_USER_ID = objWhsMaster.user_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objWhsMaster.ListCompanyPickDtl = Listcmp.ToList();

                    return objWhsMaster;
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
  public WhsMaster DefltWhsCannotDelete(WhsMaster objWhsMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure1 = "sp_ma_chk_Is_dflt_whs";
                    IEnumerable<WhsMasterDtl> ListWhsMasterLIST = connection.Query<WhsMasterDtl>(storedProcedure1,
                        new
                        {
                            @P_STR_CMP_ID = objWhsMaster.cmp_id,
                            @P_STR_WHS_ID = objWhsMaster.whs_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objWhsMaster.LstCheckWhsIdnotdel = ListWhsMasterLIST.ToList();
                }

                return objWhsMaster;
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
