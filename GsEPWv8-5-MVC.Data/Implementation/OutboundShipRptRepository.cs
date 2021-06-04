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
    public class OutboundShipRptRepository : IOutboundShipRptRepository
    {
        public OutboundShipRpt GetOutboundShipRptInquiryDetails(OutboundShipRpt objOutboundShipRpt)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvcweb_outbound_shipping_rpt_inquiry";
                    IEnumerable<OutboundShipRpt> LstOutboundShipRptInqdetailLIST = connection.Query<OutboundShipRpt>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objOutboundShipRpt.cmp_id,
                            @WhsId = objOutboundShipRpt.whs_id,
                            @CustPONum = objOutboundShipRpt.cust_ord,
                            @ShipDtFr = objOutboundShipRpt.ship_dt_fm,
                            @ShipDtTo = objOutboundShipRpt.ship_dt_to,
                            @BoLNum = objOutboundShipRpt.ship_doc_id,
                            @Carrier = objOutboundShipRpt.ship_via_name,
                            @Style = objOutboundShipRpt.itm_num,
                            @Color = objOutboundShipRpt.itm_color,
                            @Size = objOutboundShipRpt.itm_size,
                            @Status= objOutboundShipRpt.Status
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipRpt.LstOutboundShipRptInqdetail = LstOutboundShipRptInqdetailLIST.ToList();
                }

                return objOutboundShipRpt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipRpt GetOutboundShipRptbydateDetails(OutboundShipRpt objOutboundShipRpt)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvcweb_outbound_shipping_rpt_by_date";
                    IEnumerable<OutboundShipRpt> LstOutboundShipRptbyDatedetailLIST = connection.Query<OutboundShipRpt>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objOutboundShipRpt.cmp_id,
                            @WhsId = objOutboundShipRpt.whs_id,
                            @CustPONum = objOutboundShipRpt.cust_ord,
                            @ShipDtFr = objOutboundShipRpt.ship_dt_fm,
                            @ShipDtTo = objOutboundShipRpt.ship_dt_to,
                            @BoLNum = objOutboundShipRpt.ship_doc_id,
                            @Carrier = objOutboundShipRpt.ship_via_name,
                            @Style = objOutboundShipRpt.itm_num,
                            @Color = objOutboundShipRpt.itm_color,
                            @Size = objOutboundShipRpt.itm_size
                            

                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipRpt.LstOutboundShipRptbyDatedetail = LstOutboundShipRptbyDatedetailLIST.ToList();
                }

                return objOutboundShipRpt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipRpt GetOutboundShipRptbystyleDetails(OutboundShipRpt objOutboundShipRpt)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_mvcweb_outbound_shipping_rpt_by_style";
                    IEnumerable<OutboundShipRpt> LstOutboundShipRptbyDatedetailLIST = connection.Query<OutboundShipRpt>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objOutboundShipRpt.cmp_id,
                            @WhsId = objOutboundShipRpt.whs_id,
                            @CustPONum = objOutboundShipRpt.cust_ord,
                            @ShipDtFr = objOutboundShipRpt.ship_dt_fm,
                            @ShipDtTo = objOutboundShipRpt.ship_dt_to,
                            @BoLNum = objOutboundShipRpt.ship_doc_id,
                            @Carrier = objOutboundShipRpt.ship_via_name,
                            @Style = objOutboundShipRpt.itm_num,
                            @Color = objOutboundShipRpt.itm_color,
                            @Size = objOutboundShipRpt.itm_size


                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipRpt.LstOutboundShipRptbyDatedetail = LstOutboundShipRptbyDatedetailLIST.ToList();
                }

                return objOutboundShipRpt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipRpt ItemXGetitmDetails(string term, string cmp_id)
        {
            try
            {
                OutboundShipRpt objCustDtl = new OutboundShipRpt();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchCustDtls = "proc_get_webmvc_Itm_dtl";
                    IList<OutboundShipRpt> ListIRFP = connection.Query<OutboundShipRpt>(SearchCustDtls, new
                    {

                        @Cmp_ID = cmp_id,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objCustDtl.LstItmxCustdtl = ListIRFP.ToList();
                }
                return objCustDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public OutboundShipRpt GetDftWhs(OutboundShipRpt objOutboundShipRpt)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    OutboundShipRpt objOutboundInqCategory = new OutboundShipRpt();

                    const string storedProcedure2 = "SP_MVC_GET_DFTWHS";
                    IEnumerable<OutboundShipRpt> ListInq = connection.Query<OutboundShipRpt>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objOutboundShipRpt.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objOutboundShipRpt.LstWhsDetails = ListInq.ToList();

                    return objOutboundShipRpt;
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
