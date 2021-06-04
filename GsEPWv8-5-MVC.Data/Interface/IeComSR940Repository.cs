using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IeComSR940Repository
    {
        //eComSR940 GeteComSR940Details(eComSR940 objeComSR940);
        eComSR940 GetSaveeComSR940Details(eComSR940 objeComSR940);
        eComSR940 CheckExistSR940Style(eComSR940 objeComSR940);
        eComSR940 GetSaveShipRequest_hdr(eComSR940 objeComSR940);
        eComSR940 CheckExistSalesOrder(eComSR940 objeComSR940);
        eComSR940 CheckExistSRUploadFile(eComSR940 objeComSR940);

        eComSR940 AddECom940SRUploadFileNAme(eComSR940 objeComSR940);
        eComSR940 ECom940SRUploadRpt(eComSR940 objeComSR940);
        eComSR940 GetSaveeComSR940TempDetails(eComSR940 objeComSR940);
        eComSR940 DeleteTempTable(eComSR940 objeComSR940);
        eComSR940 GetEcom940Inq(eComSR940 objeComSR940);
        eComSR940 GetEcom940HdrCount(eComSR940 objeComSR940);
        eComSR940 GetEcom940InqRpt(eComSR940 objeComSR940);
        eComSR940 CheckExistBatchId(eComSR940 objeComSR940);
        eComSR940 deleteExistBatchId(eComSR940 objeComSR940);        
    }
}
