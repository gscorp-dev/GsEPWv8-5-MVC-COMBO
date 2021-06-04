using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class OrderSummaryService : IOrderSummaryService

    {
        IOrderSummaryRepository objRepository = new OrderSummaryRepository();
        public OrderSummary ListOrderSummary(OrderSummary objOrderSummary)
        {
            return objRepository.ListOrderSummary(objOrderSummary);
        }
        public ClsEcomLink fnGetCustEcomLinkDtl(string pstrCmpId)
        {
            return objRepository.fnGetCustEcomLinkDtl(pstrCmpId);
        }
    }
}
