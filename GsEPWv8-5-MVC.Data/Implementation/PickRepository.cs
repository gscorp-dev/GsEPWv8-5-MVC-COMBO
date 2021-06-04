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
    public class PickRepository : IPickRepository
    {
        public Pick GetWhsPick(Pick objPick)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Pick objPickCategory = new Pick();

                    const string storedProcedure2 = "proc_get_mvcweb_whs_details";
                    IEnumerable<Pick> ListClsPick = connection.Query<Pick>(storedProcedure2,
                        new
                        {
                            @cmp_id = objPick.cmp_id,
                            @txtId = objPick.Whs_id,
                            @txtName = objPick.Whs_name,
                        },
                        commandType: CommandType.StoredProcedure);
                    objPick.ListPick = ListClsPick.ToList();

                    return objPick;
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
        public Pick GetWhsPickDetails(string term, string cmp_id)
        {
            try
            {
                Pick objPick = new Pick();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_mvcweb_whs_details";
                    IList<Pick> ListIRFP = connection.Query<Pick>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmp_id,
                         @txtId = term,
                        @txtName = objPick.Whs_name,

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objPick.ListPick = ListIRFP.ToList();
                }
                return objPick;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public Pick GetCountryPick(Pick objPick)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Pick objPickCategory = new Pick();

                    const string storedProcedure2 = "proc_get_mvcweb_pick_cntry_details";
                    IEnumerable<Pick> ListClsPick = connection.Query<Pick>(storedProcedure2,
                        new
                        {
                            @Cntry_Id = objPick.Cntry_Id,
                            @Cntry_Name = objPick.Cntry_Name,
                        },
                        commandType: CommandType.StoredProcedure);
                    objPick.ListCntryPick = ListClsPick.ToList();

                    return objPick;
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
        public Pick GetStatePick(Pick objPick)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Pick objPickCategory = new Pick();

                    const string storedProcedure2 = "proc_get_mvcweb_pick_state_details";
                    IEnumerable<Pick> ListClsPick = connection.Query<Pick>(storedProcedure2,
                        new
                        {
                            @cntry_id = objPick.Cntry_Id,
                            @state_id = objPick.State_ID,
                        },
                        commandType: CommandType.StoredProcedure);
                    objPick.ListStatePick = ListClsPick.ToList();

                    return objPick;
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
        public Pick GetShipToPick(Pick objPick)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Pick objPickCategory = new Pick();

                    const string storedProcedure2 = "proc_get_mvcweb_pick_shipto_id_Addrs_details";
                    IEnumerable<Pick> ListClsPick = connection.Query<Pick>(storedProcedure2,
                        new
                        {
                            @cmp_id = objPick.cmp_id,
                            @shipto_id = objPick.shipto_id,
                            @cust_id = objPick.cust_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objPick.ListShipToPick = ListClsPick.ToList();

                    return objPick;
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
        public Pick GetPickExsistShipTo(Pick objPick)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Pick objPickCategory = new Pick();

                    const string storedProcedure2 = "proc_get_mvcweb_default_pick_shipto_id_Addrs_details";
                    IEnumerable<Pick> ListClsPick = connection.Query<Pick>(storedProcedure2,
                        new
                        {
                            @cmp_id = objPick.cmp_id,
                            @shipto_id = objPick.shipto_id,
                            @cust_id = objPick.cust_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objPick.ListExistShipToAddrsPick = ListClsPick.ToList();

                    return objPick;
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
        public Pick GetPickUom(Pick objPick)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Pick objPickCategory = new Pick();

                    const string storedProcedure2 = "proc_get_mvcweb_pick_uom_details";
                    IEnumerable<Pick> ListClsPick = connection.Query<Pick>(storedProcedure2,
                        new
                        {
                            @cmp_id = objPick.cmp_id,
                            @uom_id = objPick.uom_id,
                            @uom_desc = objPick.uom_desc,
                        },
                        commandType: CommandType.StoredProcedure);
                    objPick.ListExistShipToAddrsPick = ListClsPick.ToList();
                    if (objPick.ListExistShipToAddrsPick.Count == 0)
                    {
                        objPick.Check_existing_uom = "";
                    }
                    return objPick;
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
        public Pick GetPickItemCode(Pick objPick)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    Pick objPickCategory = new Pick();

                    const string storedProcedure2 = "proc_get_mvcweb_check_itm_dtl";
                    IEnumerable<Pick> ListClsPick = connection.Query<Pick>(storedProcedure2,
                        new
                        {
                            @cmp_id = objPick.cmp_id,
                            @itm_num = objPick.itm_num,
                            @itm_color = objPick.itm_color,
                            @itm_size = objPick.itm_size,

                        },
                        commandType: CommandType.StoredProcedure);
                    objPick.ListExistShipToAddrsPick = ListClsPick.ToList();
                    if (objPick.ListExistShipToAddrsPick.Count == 0)
                    {
                        objPick.Check_existing_uom = "";
                    }
                    return objPick;
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
