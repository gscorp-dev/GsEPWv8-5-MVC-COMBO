using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class StockInquiryService : IStockInquiryService
    {
        StockInquiryRepository objRepository = new StockInquiryRepository();
        public StockInquiryDtl GetStockInquiryDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockInquiryDetails(objStackInquiry);
        }
        public StockInquiryDtl GetAuditTrailDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetAuditTrailDetails(objStackInquiry);
        }
        public StockInquiryDtl GetStockRptDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockRptDetails(objStackInquiry);
        }
        public StockInquiryDtl GetStockSummRptDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockSummRptDetails(objStackInquiry);
        }
        public StockInquiryDtl GetStockLocRptDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockLocRptDetails(objStackInquiry);
        }
        public StockInquiryDtl GetStockLocSummRptDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockLocSummRptDetails(objStackInquiry);
        }
        public StockInquiryDtl GetAudRptDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetAudRptDetails(objStackInquiry);
        }
        public StockInquiryDtl GetAudSummRptDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetAudSummRptDetails(objStackInquiry);
        }
        public StockInquiryDtl GetStockInquiryDtl(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockInquiryDtl(objStackInquiry);
        }
        public void AddToTempDataStockInq(StockInquiryDtl objStackInquiry)
        {
            objRepository.AddToTempDataStockInq(objStackInquiry);
        }
        //public void TruncateTempStockInquiry(StockInquiryDtl objStackInquiry)
        ////{
        //    objRepository.TruncateTempStockInquiry(objStackInquiry);
        //}
        public void InsertTempInventory(StockInquiryDtl objStackInquiry)
        {
            objRepository.InsertTempInventory(objStackInquiry);
        }
        public StockInquiryDtl StockInquiryDtl(StockInquiryDtl objStackInquiry)
        {
            return objRepository.StockInquiryDtl(objStackInquiry);
        }
        public StockInquiryDtl GetAudRptSummary(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetAudRptSummary(objStackInquiry);
        }
        public StockInquiryDtl GetStockInquiryDetailAloc(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockInquiryDetailAloc(objStackInquiry);
        }
        public StockInquiryDtl ItemXGetitmDetails(string term, string cmp_id)
        {
            return objRepository.ItemXGetitmDetails(term, cmp_id);
        }
        public StockInquiryDtl GetStockInquiryStyleDtlExcel(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockInquiryStyleDtlExcel(objStackInquiry);
        }
        public StockInquiryDtl GetStockInquiryStyleSmryExcel(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockInquiryStyleSmryExcel(objStackInquiry);
        }
        public StockInquiryDtl GetStockInquiryLocDtlExcel(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockInquiryLocDtlExcel(objStackInquiry);
        }
        public StockInquiryDtl GetStockInquiryLocSmryExcel(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockInquiryLocSmryExcel(objStackInquiry);
        }
        public StockInquiryDtl GetAuditWithIsmExcel(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetAuditWithIsmExcel(objStackInquiry);
        }
        public StockInquiryDtl GetAuditWithOutIsmExcel(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetAuditWithOutIsmExcel(objStackInquiry);
        }
        public StockInquiryDtl GetAuditOnlyIsmExcel(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetAuditOnlyIsmExcel(objStackInquiry);
        }

        public StockInquiryDtl GetStockStyleSummRptDetails(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockStyleSummRptDetails(objStackInquiry);
        }

        public StockInquiryDtl GetAuditItemDtl(StockInquiryDtl objStackInquiry, string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size)
        {
            return objRepository.GetAuditItemDtl(objStackInquiry, p_str_cmp_id, p_str_itm_num, p_str_itm_color, p_str_itm_size);
        }

        public DataTable GetStockStyleSummRpt(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockStyleSummRpt( objStackInquiry);
        }
        public DataTable GetStockSmryByLocRpt(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockSmryByLocRpt(objStackInquiry);
        }
        public DataTable GetStockSmryByStyleRpt(StockInquiryDtl objStackInquiry)
        {
            return objRepository.GetStockSmryByStyleRpt(objStackInquiry);
        }
    }
}
