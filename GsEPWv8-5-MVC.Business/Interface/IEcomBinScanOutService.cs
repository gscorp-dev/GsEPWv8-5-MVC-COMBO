using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IEcomBinScanOutService
    {
        EcomBinScanOut CheckBinUpcStatus(EcomBinScanOut objEcomBinScanOut);
        EcomBinScanOut Getstatus(EcomBinScanOut objEcomBinScanOut);
        EcomBinScanOut GetCompanyNameList(EcomBinScanOut objEcomBinScanOut);
        EcomBinScanOut GetUniqueNumbers(EcomBinScanOut objEcomBinScanOut);
        EcomBinScanOut GetCompanyNameListHeader(EcomBinScanOut objEcomBinScanOut);
        void GetCompanyNameListCreateFetch(EcomBinScanOut objEcomBinScanOut);
        EcomBinScanOut EcomBinScanOutList(EcomBinScanOut objEcomBinScanOut);
        EcomBinScanOut EcomBinScanOutListGrid(EcomBinScanOut objEcomBinScanOut);
        void EcomBinScanOutCreate(EcomBinScanOut objEcomBinScanOut);
        void EcomBinScanOutShipHeader(EcomBinScanOut objEcomBinScanOut);
        void EcomBinScanOutTempDelete(EcomBinScanOut objEcomBinScanOut);
    }
}
