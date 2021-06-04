using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IInboundReceivingService
    {
        InboundReceiving GetInboundReceivingDtl(InboundReceiving objInboundReceiving);
        InboundReceiving GetInboundReceivingHdr(InboundReceiving objInboundReceiving);
        InboundReceiving InboundRecvWhsDetails(string term, string cmpid);
    }
}
