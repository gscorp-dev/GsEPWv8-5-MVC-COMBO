using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
   public interface IOutboundShipSchedulingInquiryRepository
    {
        OutboundShipSchedulingInquiry GetOutboundShipScheduleInq(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
        OutboundShipSchedulingInquiry GetShipSchedulelEntityValue(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
        OutboundShipSchedulingInquiry SaveShipScheduleDetails(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
        OutboundShipSchedulingInquiry OutboundShipInqpackSlipRpt(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
        OutboundShipSchedulingInquiry GetTotCubesRpt(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
    }
}
