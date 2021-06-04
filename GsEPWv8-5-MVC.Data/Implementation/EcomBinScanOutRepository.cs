using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class EcomBinScanOutRepository : IEcomBinScanOutRepository
    {
        public EcomBinScanOut CheckBinUpcStatus(EcomBinScanOut objEcomBinScanOut)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                   // string UserID = objEcomBinScanOut.UserID;
                    string status = objEcomBinScanOut.status;
                    const string storedProcedure1 = "proc_get_Pick_bin_status";
                    IEnumerable<GetEcomBinScanOutHeader> ListEcomBinScanOutTypeList = connection.Query<GetEcomBinScanOutHeader>(storedProcedure1,
                        new
                        {
                            @loc_id = objEcomBinScanOut.ScanBin,
                            @upc_code = objEcomBinScanOut.ScanUPC
                        },
                        commandType: CommandType.StoredProcedure);
                    objEcomBinScanOut.Checkbinupc = ListEcomBinScanOutTypeList.ToList();
                    for(int j = 0; j < objEcomBinScanOut.Checkbinupc.Count(); j++)
                    { 
                    objEcomBinScanOut.loc_id=objEcomBinScanOut.Checkbinupc[j].loc_id;
                    objEcomBinScanOut.upc_code = objEcomBinScanOut.Checkbinupc[j].upc_code;
                    }

                    return objEcomBinScanOut;
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
        public EcomBinScanOut Getstatus(EcomBinScanOut objEcomBinScanOut)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    string status= objEcomBinScanOut.status;
                    const string storedProcedure1 = "proc_get_ship_status";
                    IEnumerable<GetEcomBinScanOutDetail> ListEcomBinScanOutTypeList = connection.Query<GetEcomBinScanOutDetail>(storedProcedure1,
                        new
                        {
                            @aloc_doc_id= objEcomBinScanOut.AlocNo
                        },
                        commandType: CommandType.StoredProcedure);
                   objEcomBinScanOut.Getstatus = ListEcomBinScanOutTypeList.ToList();
                    if(objEcomBinScanOut.Getstatus.Count == 0)
                    {
                        objEcomBinScanOut.status = "P";
                    }
                  else
                    {
                        objEcomBinScanOut.status = "S";
                    }

                    return objEcomBinScanOut;
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
        public EcomBinScanOut GetCompanyNameList(EcomBinScanOut objEcomBinScanOut)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_web_cmp_hdr";
                    IEnumerable<GetCompanyName> ListEcomBinScanOutTypeList = connection.Query<GetCompanyName>(storedProcedure1,
                        new
                        {

                        },
                        commandType: CommandType.StoredProcedure);
                    objEcomBinScanOut.GetCompanyNameDetails = ListEcomBinScanOutTypeList.ToList();
                }               
            
                return objEcomBinScanOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public EcomBinScanOut GetUniqueNumbers(EcomBinScanOut objEcomBinScanOut)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    if(objEcomBinScanOut.Unique =="S")
                    {
                        const string storedProcedure1 = "proc_get_mvcweb_ship_doc_id";
                        IEnumerable<GetUniqueNumbers> ListEcomBinScanOutTypeLis = connection.Query<GetUniqueNumbers>(storedProcedure1,
                            new
                            {
                                @new_ship_doc_id = ""
                            },
                            commandType: CommandType.StoredProcedure);
                        objEcomBinScanOut.GetIdentityNumbers = ListEcomBinScanOutTypeLis.ToList();
                        for (int j = 0; j < objEcomBinScanOut.GetIdentityNumbers.Count(); j++)
                        {
                            objEcomBinScanOut.new_ship_doc_id = objEcomBinScanOut.GetIdentityNumbers[j].new_ship_doc_id.Trim();
                        }
                    }
                    
                    if (objEcomBinScanOut.Unique == "P")
                    {
                        const string storedProcedure2 = "proc_get_nvcweb_process_id";
                        IEnumerable<GetUniqueNumbers> ListEcomBinScanOutTypeLists = connection.Query<GetUniqueNumbers>(storedProcedure2,
                            new
                            {
                                @new_process_id = ""
                            },
                            commandType: CommandType.StoredProcedure);
                        objEcomBinScanOut.GetIdentityNumbers = ListEcomBinScanOutTypeLists.ToList();
                        for (int k = 0; k < objEcomBinScanOut.GetIdentityNumbers.Count(); k++)
                        {
                            objEcomBinScanOut.new_process_id = objEcomBinScanOut.GetIdentityNumbers[k].new_process_id.Trim();
                        }
                    }
                   
                }
                return objEcomBinScanOut;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public EcomBinScanOut GetCompanyNameListHeader(EcomBinScanOut objEcomBinScanOut)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {               

                    const string storedProcedure2 = "proc_get_web_bin_pick_slip_dtl";
                    IEnumerable<EcomBinScanOut> listRestaurantLocation = connection.Query<EcomBinScanOut>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objEcomBinScanOut.cmp_id,
                            @aloc_doc_id = objEcomBinScanOut.AlocNo,
                            @status = ""

                        },
                        commandType: CommandType.StoredProcedure);
                    objEcomBinScanOut.GetListEcomBinScanOutSHeader = listRestaurantLocation.ToList();
                }

                return objEcomBinScanOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void GetCompanyNameListCreateFetch(EcomBinScanOut objEcomBinScanOut)
        {
            //string cmp_id = objEcomBinScanOut.cmp_id;
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    //IEnumerable<EcomBinScanOut> ListEcomBinScanOut = new List<EcomBinScanOut>();

                    const string storedProcedure2 = "proc_get_web_bin_pick_slip_temp_dtl";
                    IEnumerable<EcomBinScanOut> listRestaurantLocations = connection.Query<EcomBinScanOut>(storedProcedure2,
                        new
                        {
                            @cmp_id = objEcomBinScanOut.cmp_id,
                            @loc_id = objEcomBinScanOut.loc_id,
                            @upc_code = objEcomBinScanOut.upc_code,
                            @so_itm_num = objEcomBinScanOut.so_itm_num,
                            @so_itm_size = objEcomBinScanOut.so_itm_size,
                            @due_qty = objEcomBinScanOut.aloc_qty,
                            @pick_qty = objEcomBinScanOut.pick_qty,
                            @line_num = objEcomBinScanOut.line_num,
                            @itm_code = objEcomBinScanOut.itm_code,
                            @fob = objEcomBinScanOut.fob,
                            @aloc_doc_id = objEcomBinScanOut.aloc_doc_id,
                            @so_num = objEcomBinScanOut.so_num
                           
                        }, commandType: CommandType.StoredProcedure);
                    objEcomBinScanOut.GetListEcomBinScanOutGridHeader = listRestaurantLocations.ToList();
                }

                //return objEcomBinScanOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public EcomBinScanOut EcomBinScanOutList(EcomBinScanOut objEcomBinScanOut)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objEcomBinScanOut.cmp_id;
                    EcomBinScanOut objEcomBinScanOutCategory = new EcomBinScanOut();

                    const string storedProcedure2 = "proc_get_web_bin_ref_so_dtl";
                    IEnumerable<GetEcomBinScanOutHeader> ListEcomBinScanOut = connection.Query<GetEcomBinScanOutHeader>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objEcomBinScanOut.cmp_id,
                            @status = "A"
                        },
                        commandType: CommandType.StoredProcedure);
                    objEcomBinScanOut.GetListEcomBinScanOutHeader = ListEcomBinScanOut.ToList();

                    return objEcomBinScanOut;
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

        public EcomBinScanOut EcomBinScanOutListGrid(EcomBinScanOut objEcomBinScanOut)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objEcomBinScanOut.cmp_id;
                    EcomBinScanOut objEcomBinScanOutCategory = new EcomBinScanOut();

                    const string storedProcedure2 = "proc_get_web_bin_pick_slip_temp_dtl_fetch";
                    IEnumerable<EcomBinScanOut> ListEcomBinScanOutGrid = connection.Query<EcomBinScanOut>(storedProcedure2,
                        new
                        {
                        },
                        commandType: CommandType.StoredProcedure);
                    objEcomBinScanOut.GetListEcomBinScanOutGridHeader = ListEcomBinScanOutGrid.ToList();

                    return objEcomBinScanOut;
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

        public void EcomBinScanOutCreate(EcomBinScanOut objEcomBinScanOut)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "proc_get_web_ecom_ship_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objEcomBinScanOut.cmp_id,
                        @aloc_doc_id = objEcomBinScanOut.aloc_doc_id,
                        @ship_doc_id = objEcomBinScanOut.new_ship_doc_id,
                        @ship_dt = objEcomBinScanOut.ship_dt,
                        @so_num = objEcomBinScanOut.so_num,
                        @Whs = objEcomBinScanOut.fob,
                        @itmcode = objEcomBinScanOut.itm_code,
                        @line_num = objEcomBinScanOut.line_num,
                        @upc_code = objEcomBinScanOut.upc_code,
                        @process_id = objEcomBinScanOut.new_process_id
                    }, commandType: CommandType.StoredProcedure);

                const string storedProcedure2 = "proc_save_web_bin_pick_dtl";
                connection.Execute(storedProcedure2,
                    new
                    {
                        @Cmp_ID = objEcomBinScanOut.cmp_id,
                        @aloc_doc_id = objEcomBinScanOut.aloc_doc_id,
                        @itm_code = objEcomBinScanOut.itm_code,
                        @pick_qty = objEcomBinScanOut.pick_qty
                    }, commandType: CommandType.StoredProcedure);

            }

        }
        public void EcomBinScanOutShipHeader(EcomBinScanOut objEcomBinScanOut)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {

                const string storedProcedure1 = "proc_get_web_ecom_ship_hdr_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objEcomBinScanOut.cmp_id,
                        @aloc_doc_id = objEcomBinScanOut.aloc_doc_id,
                        @ship_doc_id = objEcomBinScanOut.new_ship_doc_id,
                        @ship_dt = objEcomBinScanOut.ship_dt,
                        @so_num = objEcomBinScanOut.so_num,
                        @Whs = objEcomBinScanOut.fob,
                        @process_id = objEcomBinScanOut.new_process_id
                    }, commandType: CommandType.StoredProcedure);

                const string storedProcedure2 = "proc_get_web_bin_pick_slip_dtl_scan_save";
                connection.Execute(storedProcedure2,
                    new
                    {
                        @Cmp_ID = objEcomBinScanOut.cmp_id,
                        @aloc_doc_id = objEcomBinScanOut.aloc_doc_id,
                        @so_num = objEcomBinScanOut.so_num
                    }, commandType: CommandType.StoredProcedure);

            }

        }

        public void EcomBinScanOutTempDelete(EcomBinScanOut objEcomBinScanOut)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_web_bin_pick_slip_temp_dtl_delete";
                connection.Execute(storedProcedure1,
                    new
                    {
                    }, commandType: CommandType.StoredProcedure);

            }
        }

    }
}
