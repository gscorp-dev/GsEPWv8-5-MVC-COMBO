using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class OutboundShipSchedulingInquiryService: IOutboundShipSchedulingInquiryService
    {
        OutboundShipSchedulingInquiryRepository objOutboundShipSchedulingInqRepository = new OutboundShipSchedulingInquiryRepository();
        public OutboundShipSchedulingInquiry GetOutboundShipScheduleInq(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            return objOutboundShipSchedulingInqRepository.GetOutboundShipScheduleInq(objOutboundShipSchedulingInq);

        }

        public OutboundShipSchedulingInquiry GetShipSchedulelEntityValue(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            return objOutboundShipSchedulingInqRepository.GetShipSchedulelEntityValue(objOutboundShipSchedulingInq);
        }

        public OutboundShipSchedulingInquiry SaveShipScheduleDetails(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            return objOutboundShipSchedulingInqRepository.SaveShipScheduleDetails(objOutboundShipSchedulingInq);
        }
        public OutboundShipSchedulingInquiry OutboundShipInqpackSlipRpt(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            return objOutboundShipSchedulingInqRepository.OutboundShipInqpackSlipRpt(objOutboundShipSchedulingInq);
        }
        public OutboundShipSchedulingInquiry GetTotCubesRpt(OutboundShipSchedulingInquiry objOutboundShipSchedulingInq)
        {
            return objOutboundShipSchedulingInqRepository.GetTotCubesRpt(objOutboundShipSchedulingInq);
        }

    }
}
