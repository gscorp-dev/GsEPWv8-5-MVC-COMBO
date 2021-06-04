using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{

    public interface IOutboundShipSchedulingInquiryService
    {

        OutboundShipSchedulingInquiry GetOutboundShipScheduleInq(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
        OutboundShipSchedulingInquiry GetShipSchedulelEntityValue(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
        OutboundShipSchedulingInquiry SaveShipScheduleDetails(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
        OutboundShipSchedulingInquiry OutboundShipInqpackSlipRpt(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
        OutboundShipSchedulingInquiry GetTotCubesRpt(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq);
    }
}
