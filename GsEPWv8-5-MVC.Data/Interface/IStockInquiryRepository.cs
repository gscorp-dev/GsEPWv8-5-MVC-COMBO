using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IStockInquiryRepository
    {
        StockInquiryDtl GetStockInquiryDetails(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetAuditTrailDetails(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockRptDetails(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockSummRptDetails(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockLocRptDetails(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockLocSummRptDetails(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockStyleSummRptDetails(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockInquiryDtl(StockInquiryDtl objStackInquiry);
        void AddToTempDataStockInq(StockInquiryDtl objStackInquiry);
        void InsertTempInventory(StockInquiryDtl objStackInquiry);
        //void TruncateTempStockInquiry(StockInquiryDtl objStackInquiry);
        StockInquiryDtl StockInquiryDtl(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetAudRptSummary(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockInquiryDetailAloc(StockInquiryDtl objStackInquiry);
        StockInquiryDtl ItemXGetitmDetails(string term, string cmp_id);
        StockInquiryDtl GetStockInquiryStyleDtlExcel(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockInquiryStyleSmryExcel(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockInquiryLocDtlExcel(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetStockInquiryLocSmryExcel(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetAuditWithIsmExcel(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetAuditWithOutIsmExcel(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetAuditOnlyIsmExcel(StockInquiryDtl objStackInquiry);
        StockInquiryDtl GetAuditItemDtl(StockInquiryDtl objStackInquiry, string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size);
         DataTable GetStockStyleSummRpt(StockInquiryDtl objStackInquiry);
        DataTable GetStockSmryByLocRpt(StockInquiryDtl objStackInquiry);
        DataTable GetStockSmryByStyleRpt(StockInquiryDtl objStackInquiry);
    }
}
