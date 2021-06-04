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
    public class OutboundShipRptService : IOutboundShipRptService
    {
        IOutboundShipRptRepository objRepository = new OutboundShipRptRepository();
        public OutboundShipRpt GetOutboundShipRptInquiryDetails(OutboundShipRpt objOutboundShipRpt)
        {
            return objRepository.GetOutboundShipRptInquiryDetails(objOutboundShipRpt);
        }
        public OutboundShipRpt GetOutboundShipRptbydateDetails(OutboundShipRpt objOutboundShipRpt)
        {
            return objRepository.GetOutboundShipRptbydateDetails(objOutboundShipRpt);
        }
        public OutboundShipRpt GetOutboundShipRptbystyleDetails(OutboundShipRpt objOutboundShipRpt)
        {
            return objRepository.GetOutboundShipRptbystyleDetails(objOutboundShipRpt);
        }
        public OutboundShipRpt ItemXGetitmDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetitmDetails(term, cmp_id);
        }
        public OutboundShipRpt GetDftWhs(OutboundShipRpt objOutboundShipRpt)
        {
            return objRepository.GetDftWhs(objOutboundShipRpt);
        }
    }
}
