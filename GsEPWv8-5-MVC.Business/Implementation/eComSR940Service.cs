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
    public class eComSR940Service : IeComSR940Service
    {
        eComSR940Repository objRepository = new eComSR940Repository();
        //public eComSR940 GeteComSR940Details(eComSR940 objeComSR940)
        //{
        //    return objRepository.GeteComSR940Details(objeComSR940);
        //}
        public eComSR940 GetSaveeComSR940Details(eComSR940 objeComSR940)
        {
            return objRepository.GetSaveeComSR940Details(objeComSR940);
        }
        public eComSR940 CheckExistSR940Style(eComSR940 objeComSR940)
        {
            return objRepository.CheckExistSR940Style(objeComSR940);
        }
        public eComSR940 GetSaveShipRequest_hdr(eComSR940 objeComSR940)
        {
            return objRepository.GetSaveShipRequest_hdr(objeComSR940);
        }
        public eComSR940 CheckExistSalesOrder(eComSR940 objeComSR940)
        {
            return objRepository.CheckExistSalesOrder(objeComSR940);
        }

        public eComSR940 CheckExistSRUploadFile(eComSR940 objeComSR940)
        {
            return objRepository.CheckExistSRUploadFile(objeComSR940);
        }
        public eComSR940 AddECom940SRUploadFileNAme(eComSR940 objeComSR940)
        {
            return objRepository.AddECom940SRUploadFileNAme(objeComSR940);
        }
        public eComSR940 ECom940SRUploadRpt(eComSR940 objeComSR940)
        {
            return objRepository.ECom940SRUploadRpt(objeComSR940);
        }
        public eComSR940 GetSaveeComSR940TempDetails(eComSR940 objeComSR940)
        {
            return objRepository.GetSaveeComSR940TempDetails(objeComSR940);
        }
        public eComSR940 DeleteTempTable(eComSR940 objeComSR940)
        {
            return objRepository.DeleteTempTable(objeComSR940);
        }
        public eComSR940 GetEcom940Inq(eComSR940 objeComSR940)
        {
            return objRepository.GetEcom940Inq(objeComSR940);
        }
        public eComSR940 GetEcom940HdrCount(eComSR940 objeComSR940)
        {
            return objRepository.GetEcom940HdrCount(objeComSR940);
        }
        public eComSR940 GetEcom940InqRpt(eComSR940 objeComSR940)
        {
            return objRepository.GetEcom940InqRpt(objeComSR940);
        }
        public eComSR940 CheckExistBatchId(eComSR940 objeComSR940)
        {
            return objRepository.CheckExistBatchId(objeComSR940);
        }
        public eComSR940 deleteExistBatchId(eComSR940 objeComSR940)
        {
            return objRepository.deleteExistBatchId(objeComSR940);
        }
    }
}
