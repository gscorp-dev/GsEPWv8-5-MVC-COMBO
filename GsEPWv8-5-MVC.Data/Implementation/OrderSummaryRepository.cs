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
    public class OrderSummaryRepository : IOrderSummaryRepository
    {
        public OrderSummary ListOrderSummary(OrderSummary objOrderSummary)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objOrderSummary.cmp_id;
                    OrderSummary objOrderSummaryCategory = new OrderSummary();

                    const string spGetSoCountbyStatus = "proc_ecom_get_web_so_count_by_status";
                    IEnumerable<OrderSummary> ListOrderSummary = connection.Query<OrderSummary>(spGetSoCountbyStatus,
                        new
                        {
                              @Cmp_ID = objOrderSummary.cmp_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objOrderSummary.LstOrderSummary = ListOrderSummary.ToList();

                    return objOrderSummary;
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

        public ClsEcomLink fnGetCustEcomLinkDtl(string pstrCmpId )
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    ClsEcomLink objEcomLink = new ClsEcomLink();

                   IEnumerable<ClsCustEcomLinkHdr> lstCustEcomLinkHdr = connection.Query<ClsCustEcomLinkHdr>("spGetCustEcomLinkDtl",
                        new
                        {
                            @pstrCompId = pstrCmpId
                        },
                        commandType: CommandType.StoredProcedure);
                    objEcomLink.lstCustEcomLinkHdr = lstCustEcomLinkHdr.ToList();

                    return objEcomLink;
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
