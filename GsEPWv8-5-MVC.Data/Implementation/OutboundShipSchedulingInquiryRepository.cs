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
    public class OutboundShipSchedulingInquiryRepository: IOutboundShipSchedulingInquiryRepository
    {
        public OutboundShipSchedulingInquiry GetOutboundShipScheduleInq(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var status = objOutboundShipSchedulingInq.status;
                    if (status == "ALL")
                    {
                        status = "";
                    }
                    else if (status == "SHIP")
                    {
                        status = "S";
                    }
                    else if (status == "POST")
                    {
                        status = "P";
                    }
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "proc_get_webmvc_outbound_ship_scheduling_inquiry";
                    IEnumerable<OutboundShipSchedulingInquiry> ListInq = connection.Query<OutboundShipSchedulingInquiry>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objOutboundShipSchedulingInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipSchedulingInq.ship_doc_id_Fm,
                            @ShipDocIdTo = objOutboundShipSchedulingInq.ship_doc_id_To,
                            @AlocNum = objOutboundShipSchedulingInq.aloc_doc_id,
                            @ShipDtFr = objOutboundShipSchedulingInq.Ship_dt_Fm,
                            @ShipDtTo = objOutboundShipSchedulingInq.Ship_dt_To,
                            @ShipVia = objOutboundShipSchedulingInq.ship_via_name,
                            @Status = status,
                            @CustID = objOutboundShipSchedulingInq.cust_id,
                            @Shipto = objOutboundShipSchedulingInq.ship_to,
                            @Whs = objOutboundShipSchedulingInq.whs_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipSchedulingInq.LstOutboundShipScheduleInqdetail = ListInq.ToList();

                    return objOutboundShipSchedulingInq;
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
        public OutboundShipSchedulingInquiry GetShipSchedulelEntityValue(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                  
                    const string storedProcedure2 = "SP_MVC_OB_SHIP_SCHDL_DOC_ID";
                    IEnumerable<OutboundShipSchedulingInquiry> ListInq = connection.Query<OutboundShipSchedulingInquiry>(storedProcedure2,
                         new
                        {
                             @ship_schdl_doc_id = ""
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipSchedulingInq.Lstshipschdldocid = ListInq.ToList();

                    return objOutboundShipSchedulingInq;
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
        public OutboundShipSchedulingInquiry SaveShipScheduleDetails(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {



                    const string storedProcedure2 = "SP_MVC_OB_INSERT_SHIP_SCHDL_DTL";
                    IEnumerable<OutboundShipSchedulingInquiry> ListInq = connection.Query<OutboundShipSchedulingInquiry>(storedProcedure2,
                         new
                         {
                             @P_STR_CMPID= objOutboundShipSchedulingInq.cmp_id,
                             @P_STR_TEMP_SCHDL_ID= objOutboundShipSchedulingInq.ship_schdl_doc_id,
                             @P_STR_ALOC_DOC_ID= objOutboundShipSchedulingInq.aloc_doc_id,
                             @P_STR_SHIP_STATUS= objOutboundShipSchedulingInq.status,
                             @P_STR_SHIP_SCHDL_DATE = objOutboundShipSchedulingInq.shipscheduleddate,
                             @P_STR_SHIP_CAR_NAME= objOutboundShipSchedulingInq.shipcarname,
                             @P_STR_SHIP_TRUCK_ID= objOutboundShipSchedulingInq.shipcarid,
                             @P_STR_SHIP_NOTES= objOutboundShipSchedulingInq.notes,
                             @P_STR_ACTION_TYPE=objOutboundShipSchedulingInq.actiontype,
                         },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipSchedulingInq.LstSaveShipSchdlDetail = ListInq.ToList();

                    return objOutboundShipSchedulingInq;
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

        public OutboundShipSchedulingInquiry OutboundShipInqpackSlipRpt(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                   
                    const string storedProcedure2 = "proc_get_webmvc_outbound_packingslip_rpt";
                    IEnumerable<OutboundShipSchedulingInquiry> ListInq = connection.Query<OutboundShipSchedulingInquiry>(storedProcedure2,
                        new
                        {
                            @CmpId = objOutboundShipSchedulingInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipSchedulingInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipSchedulingInq.LstOutboundShipInqpackingSlipRpt = ListInq.ToList();

                    return objOutboundShipSchedulingInq;
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


        public OutboundShipSchedulingInquiry GetTotCubesRpt(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    OutboundShipInq objOutboundInqCategory = new OutboundShipInq();

                    const string storedProcedure2 = "SP_MVC_OB_GET_TOTALCUBE";
                    IEnumerable<OutboundShipSchedulingInquiry> ListInq = connection.Query<OutboundShipSchedulingInquiry>(storedProcedure2,
                        new
                        {
                            @CmpId = objOutboundShipSchedulingInq.cmp_id,
                            @ShipDocIdFr = objOutboundShipSchedulingInq.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipSchedulingInq.LstPalletCount = ListInq.ToList();

                    return objOutboundShipSchedulingInq;
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
