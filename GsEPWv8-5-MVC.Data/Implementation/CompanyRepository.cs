using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//CR-3PL_MVC_IB_2018_0219_008 - Display company name instead of Comp Id in all the Reports

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class CompanyRepository : ICompanyRepository
    {

        public Company GetCompanyDetails(Company objCompany)
            {
                try
                {
                    using (IDbConnection connection = ConnectionManager.OpenConnection())
                    {


                        LookUp objOrderLifeCycleCategory = new LookUp();
                    

                        const string storedProcedure2 = "proc_get_mvcweb_validate_cmp_id";
                        IEnumerable<Company> Listcmp = connection.Query<Company>(storedProcedure2,
                            new
                            {
                                @cmp_id = objCompany.cmp_id,
                                @user_id = objCompany.user_id

                            },
                            commandType: CommandType.StoredProcedure);
                    objCompany.ListCompanyPickDtl = Listcmp.ToList();

                        return objCompany;
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


        public Company GetPickCompanyDetails(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "proc_get_mvcweb_pick_cmp_id";
                    IEnumerable<Company> Listcmp = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @user_id = objCompany.user_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListCompanyPickDtl = Listcmp.ToList();

                    return objCompany;
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

        public Company GetPickCompanyDetails(Company objCompany, string pstrCmpId)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "sp_ma_get_cmp_id";
                    IEnumerable<Company> Listcmp = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @P_STR_USER_ID = objCompany.user_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListCompanyPickDtl = Listcmp.ToList();

                    return objCompany;
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
        public Company GetFullFillCompanyDetails(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "proc_get_mvcweb_fulfill_cmp_id";
                    IEnumerable<Company> Listcmp = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @cmp_id = objCompany.cmp_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListFullFillCompanyPickDtl = Listcmp.ToList();

                    return objCompany;
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
        public Company GetCustOfCompanyDetails(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "proc_get_mvcweb_cust_Of_company_dtl";
                    IEnumerable<Company> Listcmp = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @cust_of_cmp_id = objCompany.cust_of_cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListCustofCompanyPickDtl = Listcmp.ToList();

                    return objCompany;
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

        public Company GetWhsIdDetails(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "proc_get_mvcweb_whs_id";
                    IEnumerable<Company> Listwhs = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @cmp_id = objCompany.cust_cmp_id,
                            @whs_id = objCompany.whs_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListwhsPickDtl = Listwhs.ToList();

                    return objCompany;
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
        public Company GetLocIdDetails(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "PROC_GET_MVCWEB_LOC_DTLS";
                    IEnumerable<Company> ListLoc = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objCompany.cust_cmp_id.Trim(),                          
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListLocPickDtl = ListLoc.ToList();

                    return objCompany;
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
        public Company GetCustConfigDtls(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "proc_get_mvcweb_cust_config_dtls";
                    IEnumerable<Company> ListGetCustConfigDtlsList = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @cmp_id = objCompany.cust_cmp_id.Trim(),
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListGetCustConfigDtls = ListGetCustConfigDtlsList.ToList();

                    return objCompany;
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
  public Company Validate_Company(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "SP_MVC_OUT_GET_CMP_VALIDATION";
                    IEnumerable<Company> ListLoc = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @P_STR_USERID = objCompany.user_id.Trim(),
                            @P_STR_CMPID = objCompany.cmp_id.Trim(),
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.Lstcmpdtl = ListLoc.ToList();

                    return objCompany;
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
        public Company Validate_Cmp_whs(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "SP_MVC_OUT_GET_WHS_VALIDATION";
                    IEnumerable<Company> ListLoc = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objCompany.cmp_id.Trim(),
                            @P_STR_WHSID = objCompany.whs_id.Trim(),
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.Lstwhsdtl = ListLoc.ToList();

                    return objCompany;
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
        public Company Validate_Customer(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "SP_MVC_OUT_GET_CUST_VALIDATION";
                    IEnumerable<Company> ListLoc = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @P_STR_CUSTID = objCompany.cust_cmp_id.Trim(),
                            @P_STR_CMPID = objCompany.cmp_id.Trim(),
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.Lstcustdtl = ListLoc.ToList();

                    return objCompany;
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
        //CR - 3PL_MVC_IB_2018_0219_008
        public Company GetCompName(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "SP_MVC_IB_GET_COMPANY_NAME";
                    IEnumerable<Company> ListLoc = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @user_id = objCompany.user_id.Trim(),
                            @cmp_id = objCompany.cmp_id.Trim(),
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.LstCmpName = ListLoc.ToList();

                    return objCompany;
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
        //CR - 3PL_MVC_IB_2018_0219_008
        //CR-3PL_MVC_IB_2018_0219_004
        public Company GetCustOfCompName(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "SP_MVC_CMP_GET_CUST_OF_CMP_NAME";
                    IEnumerable<Company> ListLoc = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @cust_of_cmp_id = objCompany.cmp_id.Trim(),
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.LstCustOfCmpName = ListLoc.ToList();

                    return objCompany;
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
        //CR-3PL_MVC_IB_2018_0219_004

        public Company LoadUserType(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "SP_MVC_USRACNT_GET_USER_TYPE";
                    IEnumerable<Company> ListLoc = connection.Query<Company>(storedProcedure2,
                        new
                        {

                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListUserType = ListLoc.ToList();

                    return objCompany;
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
        public Company CompanyAddresHdrDtls(Company objCompany)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    LookUp objOrderLifeCycleCategory = new LookUp();

                    const string storedProcedure2 = "SP_MVC_GET_RPT_COMPANY_DTLS";
                    IEnumerable<Company> ListLoc = connection.Query<Company>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objCompany.cmp_id.Trim(),
                        },
                        commandType: CommandType.StoredProcedure);
                    objCompany.ListCompanyAddresHdrDtls = ListLoc.ToList();

                    return objCompany;
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
