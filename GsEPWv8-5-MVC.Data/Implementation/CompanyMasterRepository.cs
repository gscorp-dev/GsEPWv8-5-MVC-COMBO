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
   
        public class CompanyMasterRepository : ICompanyMasterRepository
        {

        public CompanyMaster GetCmpMasterDetails(CompanyMaster objCompanyMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CompanyMaster objOrderLifeCycleCategory = new CompanyMaster();

                    const string storedProcedure2 = "sp_ma_get_cmp_dtls";
                    IEnumerable<CompanyMasterDtl> ListCmpDetailsLST = connection.Query<CompanyMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objCompanyMaster.cust_of_cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompanyMaster.ListCmpDetails = ListCmpDetailsLST.ToList();

                    return objCompanyMaster;
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
        public CompanyMaster GetCmpHdrDetails(CompanyMaster objCompanyMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CompanyMaster objOrderLifeCycleCategory = new CompanyMaster();

                    const string storedProcedure2 = "sp_ma_get_cmp_hdr_dtls";
                    IEnumerable<CompanyMasterDtl> ListCmpHdrDetailsLST = connection.Query<CompanyMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objCompanyMaster.cust_of_cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompanyMaster.ListCmpHdrDetails = ListCmpHdrDetailsLST.ToList();

                    return objCompanyMaster;
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
        public CompanyMaster CheckExistCmpId(CompanyMaster objCompanyMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CompanyMaster objOrderLifeCycleCategory = new CompanyMaster();

                    const string storedProcedure2 = "sp_ma_check_comp_id_exist";
                    IEnumerable<CompanyMasterDtl> ListCmpHdrDetailsLST = connection.Query<CompanyMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objCompanyMaster.cust_of_cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompanyMaster.ListCheckExistCmpId = ListCmpHdrDetailsLST.ToList();

                    return objCompanyMaster;
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
        public CompanyMaster CheckExistwhsId(CompanyMaster objCompanyMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CompanyMaster objOrderLifeCycleCategory = new CompanyMaster();

                    const string storedProcedure2 = "sp_ma_chk_whs_id_exist";
                    IEnumerable<CompanyMasterDtl> ListCmpHdrDetailsLST = connection.Query<CompanyMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objCompanyMaster.cust_of_cmp_id,
                            @P_STR_WHS_ID = objCompanyMaster.dflt_whs_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompanyMaster.ListCheckExistwhsId = ListCmpHdrDetailsLST.ToList();

                    return objCompanyMaster;
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

        public void SaveCmpMasterDtls(CompanyMaster objCompanyMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_insert_cmp_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_STR_OPT_ID = "A",
                        @P_STR_COMP_ID = objCompanyMaster.cust_of_cmp_id,
                        @P_STR_STATUS = objCompanyMaster.status,
                        @P_STR_COMP_NAME = objCompanyMaster.cmp_name,
                        @P_START_DT = objCompanyMaster.start_dt,
                        @P_STR_LAST_CHG_DT = objCompanyMaster.last_chg_dt,
                        @P_STR_ATTN = objCompanyMaster.attn,
                        @P_STR_ADDR_LINE1 = objCompanyMaster.addr1,
                        @P_STR_ADDR_LINE2 = objCompanyMaster.addr2,
                        @P_STR_CITY = objCompanyMaster.city,
                        @P_STR_STATE_ID = objCompanyMaster.state_id,
                        @P_STR_POST_CODE = objCompanyMaster.zip,
                        @P_STR_CNTRY_ID = objCompanyMaster.country,
                        @P_STR_TEL = objCompanyMaster.Tel,
                        @P_STR_FAX = objCompanyMaster.fax,
                        @P_STR_WEB = objCompanyMaster.web,
                        @P_STR_EMAIL = objCompanyMaster.email,
                        @P_STR_GROUP_ID = objCompanyMaster.group_id,
                        @P_STR_PROCESS_ID = "Add",
                        @P_STR_REMIT_ATTN = objCompanyMaster.remit_attn,
                        @P_STR_REMIT_ADDR_LINE1 = objCompanyMaster.remit_addr_line1,
                        @P_STR_REMIT_ADDR_LINE2 = objCompanyMaster.remit_addr_line2,
                        @P_STR_REMIT_CITY = objCompanyMaster.Remit_City,
                        @P_STR_REMIT_STATE_ID = objCompanyMaster.remit_state_id,
                        @P_STR_REMIT_POST_CODE = objCompanyMaster.Remit_Post_Code,
                        @P_STR_REMIT_CNTRY_ID = objCompanyMaster.remit_cntry_id,
                        @P_STR_REMIT_TEL = objCompanyMaster.remit_tel,
                        @P_STR_REMIT_FAX = objCompanyMaster.remit_fax,
                        @P_STR_REMIT_EMAIL = objCompanyMaster.remit_email,
                        @P_STR_REMIT_WEB = objCompanyMaster.remit_web,
                        @P_STR_DFLT_WHS_ID = objCompanyMaster.dflt_whs_id

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateCmpMasterDtls(CompanyMaster objCompanyMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_insert_cmp_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_STR_OPT_ID = "M",
                        @P_STR_COMP_ID = objCompanyMaster.cust_of_cmp_id,
                        @P_STR_STATUS = objCompanyMaster.status,
                        @P_STR_COMP_NAME = objCompanyMaster.cmp_name,
                        @P_START_DT = objCompanyMaster.start_dt,
                        @P_STR_LAST_CHG_DT = objCompanyMaster.last_chg_dt,
                        @P_STR_ATTN = objCompanyMaster.attn,
                        @P_STR_ADDR_LINE1 = objCompanyMaster.addr1,
                        @P_STR_ADDR_LINE2 = objCompanyMaster.addr2,
                        @P_STR_CITY = objCompanyMaster.city,
                        @P_STR_STATE_ID = objCompanyMaster.state_id,
                        @P_STR_POST_CODE = objCompanyMaster.zip,
                        @P_STR_CNTRY_ID = objCompanyMaster.country,
                        @P_STR_TEL = objCompanyMaster.Tel,
                        @P_STR_FAX = objCompanyMaster.fax,
                        @P_STR_WEB = objCompanyMaster.web,
                        @P_STR_EMAIL = objCompanyMaster.email,
                        @P_STR_GROUP_ID = objCompanyMaster.group_id,
                        @P_STR_PROCESS_ID = "Edit",
                        @P_STR_REMIT_ATTN = objCompanyMaster.remit_attn,
                        @P_STR_REMIT_ADDR_LINE1 = objCompanyMaster.remit_addr_line1,
                        @P_STR_REMIT_ADDR_LINE2 = objCompanyMaster.remit_addr_line2,
                        @P_STR_REMIT_CITY = objCompanyMaster.Remit_City,
                        @P_STR_REMIT_STATE_ID = objCompanyMaster.remit_state_id,
                        @P_STR_REMIT_POST_CODE = objCompanyMaster.Remit_Post_Code,
                        @P_STR_REMIT_CNTRY_ID = objCompanyMaster.remit_cntry_id,
                        @P_STR_REMIT_TEL = objCompanyMaster.remit_tel,
                        @P_STR_REMIT_FAX = objCompanyMaster.remit_fax,
                        @P_STR_REMIT_EMAIL = objCompanyMaster.remit_email,
                        @P_STR_REMIT_WEB = objCompanyMaster.remit_web,
                        @P_STR_DFLT_WHS_ID = objCompanyMaster.dflt_whs_id,

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public CompanyMaster CheckCustDetails(CompanyMaster objCompanyMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CompanyMaster objOrderLifeCycleCategory = new CompanyMaster();

                    const string storedProcedure2 = "sp_ma_check_cust_dtls_for_company";
                    IEnumerable<CompanyMasterDtl> ListCmpHdrDetailsLST = connection.Query<CompanyMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_COMP_ID = objCompanyMaster.cust_of_cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompanyMaster.ListCheckCustDtl = ListCmpHdrDetailsLST.ToList();

                    return objCompanyMaster;
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
        public void DeleteCmp(CompanyMaster objCompanyMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_delete_cmp_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @P_STR_COMP_ID = objCompanyMaster.cust_of_cmp_id,

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        
    }
}
