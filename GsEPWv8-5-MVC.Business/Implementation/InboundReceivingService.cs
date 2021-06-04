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
    public class InboundReceivingService : IInboundReceivingService
    {
        IInboundReceivingRepository objRepository = new InboundReceivingRepository();

        public InboundReceiving GetInboundReceivingDtl(InboundReceiving objInboundReceiving)
        {
            return objRepository.GetInboundReceivingDtl(objInboundReceiving);
        }
        public InboundReceiving GetInboundReceivingHdr(InboundReceiving objInboundReceiving)
        {
            return objRepository.GetInboundReceivingHdr(objInboundReceiving);
        }
        public InboundReceiving InboundRecvWhsDetails(string term, string cmpid)
        {
            return objRepository.InboundRecvWhsDetails(term, cmpid);
        }
    }
}
