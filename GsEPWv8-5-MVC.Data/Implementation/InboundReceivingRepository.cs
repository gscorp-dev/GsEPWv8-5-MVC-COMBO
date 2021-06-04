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
    public class InboundReceivingRepository : IInboundReceivingRepository
    {
        public InboundReceiving GetInboundReceivingDtl(InboundReceiving objInboundReceiving)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "proc_get_webmvc_Ecom_944InboundRecev";
                    IEnumerable<InboundReceiving> ListInboundReceivingTypeList = connection.Query<InboundReceiving>(storedProcedure1,
                        new
                        {
                            @cmp_id = objInboundReceiving.cmp_id,
                            @WhsId = objInboundReceiving.whs_id,
                            @rcvddtFm = objInboundReceiving.rcv_dt_frm,
                            @rcvddtTo = objInboundReceiving.rcv_dt_to,
                            @ib_docid_Fm = objInboundReceiving.ib_doc_frm,
                            @ib_docid_to = objInboundReceiving.ib_doc_to

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundReceiving.LstInboundReceiving = ListInboundReceivingTypeList.ToList();
                }

                return objInboundReceiving;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundReceiving GetInboundReceivingHdr(InboundReceiving objInboundReceiving)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "Get_hdr_dtl_mvcweb";
                    IEnumerable<InboundReceiving> ListInboundReceivingTypeList = connection.Query<InboundReceiving>(storedProcedure1,
                        new
                        {
                            @cmp_id = objInboundReceiving.cmp_id,
                            @ib_doc_id = objInboundReceiving.ib_doc_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objInboundReceiving.LstInboundReceivingHdr = ListInboundReceivingTypeList.ToList();
                }

                return objInboundReceiving;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public InboundReceiving InboundRecvWhsDetails(string term, string cmpid)
        {
            try
            {
                InboundReceiving objShipfromDtl = new InboundReceiving();

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string SearchShipHdrDtls = "proc_get_webmvc_ship_from_hdr";
                    IList<InboundReceiving> ListIRFP = connection.Query<InboundReceiving>(SearchShipHdrDtls, new
                    {

                        @Cmp_ID = cmpid,
                        @SearchText = term

                    }, commandType: CommandType.StoredProcedure).ToList();
                    objShipfromDtl.LstInboundReceivingHdr = ListIRFP.ToList();
                }
                return objShipfromDtl;
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
