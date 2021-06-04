using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IOutboundShipRptRepository
    {
        OutboundShipRpt GetOutboundShipRptInquiryDetails(OutboundShipRpt objOutboundShipRpt);
        OutboundShipRpt GetOutboundShipRptbydateDetails(OutboundShipRpt objOutboundShipRpt);
        OutboundShipRpt GetOutboundShipRptbystyleDetails(OutboundShipRpt objOutboundShipRpt);
        OutboundShipRpt ItemXGetitmDetails(string term, string cmp_id);
        OutboundShipRpt GetDftWhs(OutboundShipRpt objOutboundShipRpt);

    }
}
