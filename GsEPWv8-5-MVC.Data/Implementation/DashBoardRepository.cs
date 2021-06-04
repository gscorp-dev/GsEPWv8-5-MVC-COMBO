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
    public class DashBoardRepository : IDashBoardRepository
    {
    
        public DashBoard ListDashBoard(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    

                    const string storedprocedure1 = "proc_mvc_get_web_count_by_inbound";
                    IEnumerable<DashBoardDtl> ListDashBoard = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_DOC_FRM_DT = objDashBoard.frm_dt,
                            @P_DOC_TO_DT = objDashBoard.to_dt,

                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstDashBoard = ListDashBoard.ToList();
     
                  

                    return objDashBoard;
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
        public DashBoard GetInboundOpenData(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_inbound_sts_open";
                    IEnumerable<DashBoardDtl> ListDashBoard = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_DOC_FRM_DT = objDashBoard.frm_dt,
                            @P_DOC_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstInboundOpenData = ListDashBoard.ToList();



                    return objDashBoard;
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

        public DashBoard GetInboundRcvdData(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_inbound_rcvd";
                    IEnumerable<DashBoardDtl> ListDashBoard = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_DOCRCVD_FRM_DT = objDashBoard.frm_dt,
                            @P_DOCRCVD_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstInboundRcvdData = ListDashBoard.ToList();



                    return objDashBoard;
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
        public DashBoard GetInboundPostData(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_inbound_sts_post";
                    IEnumerable<DashBoardDtl> ListDashBoard = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_DOC_FRM_DT = objDashBoard.frm_dt,
                            @P_DOC_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstInboundPostData = ListDashBoard.ToList();



                    return objDashBoard;
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


        public DashBoard ListOutBound(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_outbound";
                    IEnumerable<DashBoardDtl> ListOutBound = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_CMP_ID = objDashBoard.cmp_id,
                            @P_SO_FROM_DT = objDashBoard.frm_dt,
                            @P_SO_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstOutBound = ListOutBound.ToList();
                    return objDashBoard;
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
        public DashBoard GetOutBoundShipReq(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_outboundshipreq";
                    IEnumerable<DashBoardDtl> ListOutBound = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_SO_FRM_DT = objDashBoard.frm_dt,
                            @P_SO_TO_DT = objDashBoard.to_dt

                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstOutBoundShipREq = ListOutBound.ToList();
                    return objDashBoard;
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

        public DashBoard GetOutBoundAloc(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_outboundAloc";
                    IEnumerable<DashBoardDtl> ListOutBound = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_SO_FRM_DT = objDashBoard.frm_dt,
                            @P_SO_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstOutBoundAloc = ListOutBound.ToList();
                    return objDashBoard;
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

        public DashBoard GetOutBoundShipPost(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_outboundShip";
                    IEnumerable<DashBoardDtl> ListOutBound = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_SHIP_FRM_DT = objDashBoard.frm_dt,
                            @P_SHIP_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstOutBoundShipPost = ListOutBound.ToList();
                    return objDashBoard;
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

        public DashBoard ListVas(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_vas";
                    IEnumerable<DashBoardDtl> ListVas = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_VAS_FRM_DT = objDashBoard.frm_dt,
                            @P_VAS_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstVas = ListVas.ToList();
                    return objDashBoard;
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
        public DashBoard ListVasOpen(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_vas_open";
                    IEnumerable<DashBoardDtl> ListVas = connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDashBoard.cmp_id,
                            @P_VAS_FRM_DT = objDashBoard.frm_dt,
                            @P_VAS_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstVasOpen = ListVas.ToList();
                    return objDashBoard;
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
        public DashBoard ListBill(DashBoard objDashBoard)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    const string storedprocedure1 = "proc_mvc_get_web_count_by_bill";
                    IEnumerable<DashBoardDtl> ListBill= connection.Query<DashBoardDtl>(storedprocedure1,
                        new
                        {
                            @P_STR_CUST_ID = objDashBoard.cmp_id,
                            @P_BILL_FRM_DT = objDashBoard.frm_dt,
                            @P_BILL_TO_DT = objDashBoard.to_dt
                        },
                        commandType: CommandType.StoredProcedure);
                    objDashBoard.LstBill = ListBill.ToList();
                    return objDashBoard;
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

    

    public DashBoard ListBillOpen(DashBoard objDashBoard)
    {
        try
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {


                const string storedprocedure1 = "proc_mvc_get_web_count_by_bill_open";
                IEnumerable<DashBoardDtl> ListBill = connection.Query<DashBoardDtl>(storedprocedure1,
                    new
                    {
                        @P_STR_CUST_ID = objDashBoard.cmp_id,
                        @P_BILL_FRM_DT = objDashBoard.frm_dt,
                        @P_BILL_TO_DT = objDashBoard.to_dt
                    },
                    commandType: CommandType.StoredProcedure);
                objDashBoard.LstBillOpen = ListBill.ToList();
                return objDashBoard;
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

