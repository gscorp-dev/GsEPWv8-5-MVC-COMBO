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
   public class LocationMasterRepository: ILocationMasterRepository
    {
        public LocationMaster GetLocationMasterDetails(LocationMaster objLocationMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_LOC_HDR_FETCH";
                    IEnumerable<LocationMaster> ListCmpDetailsLST = connection.Query<LocationMaster>(storedProcedure2,
                        new
                        {
                            @p_cmp_id = objLocationMaster.cmp_id,
                            @p_whs_id = objLocationMaster.whs_id,
                            @p_loc_id = objLocationMaster.loc_id,
                            @p_loc_desc = objLocationMaster.loc_desc,
                        },
                        commandType: CommandType.StoredProcedure);
                    objLocationMaster.ListLocationMasterDetails = ListCmpDetailsLST.ToList();

                    return objLocationMaster;
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
        public LocationMaster InsertLocationMasterDetails(LocationMaster objLocationMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_INSERT_LOC_Hdr";
                    IEnumerable<LocationMaster> ListCmpDetailsLST = connection.Query<LocationMaster>(storedProcedure2,
                        new
                        {
                            @cmpId = objLocationMaster.cmp_id,
                            @whsid = objLocationMaster.whs_id,
                            @Locid = objLocationMaster.loc_id,
                            @Locdesc = objLocationMaster.loc_desc,
                            @Status = objLocationMaster.status,
                            @note = objLocationMaster.note,
                            @length = objLocationMaster.length,
                            @width = objLocationMaster.width,
                            @depth = objLocationMaster.depth,
                            @cube = objLocationMaster.cube,
                            @usage = objLocationMaster.usage,
                            @processid = objLocationMaster.process_id,
                            @opt = objLocationMaster.option,
                            @loc_type = objLocationMaster.loc_type
                        },
                        commandType: CommandType.StoredProcedure);
                    objLocationMaster.ListInsertMasterDetails = ListCmpDetailsLST.ToList();

                    return objLocationMaster;
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
        public LocationMaster DeleteLocationMasterDetails(LocationMaster objLocationMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_Del_LOC_Hdr";
                    IEnumerable<LocationMaster> ListCmpDetailsLST = connection.Query<LocationMaster>(storedProcedure2,
                        new
                        {
                            @cmpid = objLocationMaster.cmp_id,
                            @whsid = objLocationMaster.whs_id,
                            @Locid = objLocationMaster.loc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objLocationMaster.ListLocationMasterDetails = ListCmpDetailsLST.ToList();

                    return objLocationMaster;
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

        public LocationMaster GetWhsPickDetails(string term, string cmp_id)
        {
            try
            {
                LocationMaster objLocationMaster = new LocationMaster();
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_MVC_GET_WHS_DETAIL";
                    IEnumerable<LocationMaster> ListCmpDetailsLST = connection.Query<LocationMaster>(storedProcedure2,
                        new
                        {
                            @cmp_id =cmp_id,
                            @txtId = term,
                            @txtName = objLocationMaster.Whs_name,
                        },
                        commandType: CommandType.StoredProcedure);
                    objLocationMaster.ListWhsDetails = ListCmpDetailsLST.ToList();

                    return objLocationMaster;
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
        public LocationMaster CHECKLOCIDEXIST(LocationMaster objLocationMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedProcedure2 = "SP_MA_LOC_ID_EXIST";
                    IEnumerable<LocationMaster> ListCmpDetailsLST = connection.Query<LocationMaster>(storedProcedure2,
                        new
                        {
                            @p_cmp_id = objLocationMaster.cmp_id,
                            @p_whs_id = objLocationMaster.whs_id,
                            @p_loc_id = objLocationMaster.loc_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objLocationMaster.ListLocationMasterDetails = ListCmpDetailsLST.ToList();

                    return objLocationMaster;
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
