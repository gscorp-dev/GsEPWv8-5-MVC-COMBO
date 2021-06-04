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
    public class EcomBinScanOutService : IEcomBinScanOutService
    {
        IEcomBinScanOutRepository objRepository = new EcomBinScanOutRepository();
        public EcomBinScanOut CheckBinUpcStatus(EcomBinScanOut objEcomBinScanOut)
        {
            return objRepository.CheckBinUpcStatus(objEcomBinScanOut);
        }

        public EcomBinScanOut Getstatus(EcomBinScanOut objEcomBinScanOut)
        {
            return objRepository.Getstatus(objEcomBinScanOut);
        }
        public EcomBinScanOut GetCompanyNameList(EcomBinScanOut objEcomBinScanOut)
        {
            return objRepository.GetCompanyNameList(objEcomBinScanOut);
        }
        public EcomBinScanOut GetUniqueNumbers(EcomBinScanOut objEcomBinScanOut)
        {
            return objRepository.GetUniqueNumbers(objEcomBinScanOut);
        }

        public EcomBinScanOut GetCompanyNameListHeader(EcomBinScanOut objEcomBinScanOut)
        {
            return objRepository.GetCompanyNameListHeader(objEcomBinScanOut);
        }
        public void GetCompanyNameListCreateFetch(EcomBinScanOut objEcomBinScanOut)
        {
            objRepository.GetCompanyNameListCreateFetch(objEcomBinScanOut);
        }
        public EcomBinScanOut EcomBinScanOutList(EcomBinScanOut objEcomBinScanOut)
        {
            return objRepository.EcomBinScanOutList(objEcomBinScanOut);
        }
        public EcomBinScanOut EcomBinScanOutListGrid(EcomBinScanOut objEcomBinScanOut)
        {
            return objRepository.EcomBinScanOutListGrid(objEcomBinScanOut);
        }
        public void EcomBinScanOutCreate(EcomBinScanOut objEcomBinScanOut)
        {
            objRepository.EcomBinScanOutCreate(objEcomBinScanOut);
        }
        public void EcomBinScanOutShipHeader(EcomBinScanOut objEcomBinScanOut)
        {
            objRepository.EcomBinScanOutShipHeader(objEcomBinScanOut);
        }

        public void EcomBinScanOutTempDelete(EcomBinScanOut objEcomBinScanOut)
        {
            objRepository.EcomBinScanOutTempDelete(objEcomBinScanOut);
        }
    }
}
