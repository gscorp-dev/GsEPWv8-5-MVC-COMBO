using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IOutboundShipInqRepository
    {
        OutboundShipInq GetOutboundShipInq(OutboundShipInq objOutboundShipInq);
        OutboundShipInq OutboundShipInqGetCustDetails(string term, string cmpid);
        OutboundShipInq OutboundShipInqGetShipToDetails(string term, string cmpid);
        OutboundShipInq OutboundShipInqGetShipFromDetails(string term, string cmpid);
        OutboundShipInq OutboundShipInqSummaryRpt(OutboundShipInq objOutboundShipInq);
        void SaveShipPost(OutboundShipInq objOutboundShipInq);
        void InsertShipPost(OutboundShipInq objOutboundShipInq);
        void InsertPalletShipDtl(OutboundShipInq objOutboundShipInq);
        OutboundShipInq GetCheckShipPost(OutboundShipInq objOutboundShipInq);
        OutboundShipInq GetContId(OutboundShipInq objOutboundShipInq);
        OutboundShipInq GetPoNum(OutboundShipInq objOutboundShipInq);
        OutboundShipInq GetShipPostGrid(OutboundShipInq objOutboundShipInq);
        OutboundShipInq OutboundShipInqpackSlipRpt(OutboundShipInq objOutboundShipInq);
        OutboundShipInq OutboundShipInqBillofLaddingRpt(OutboundShipInq objOutboundShipInq);
        OutboundShipInq OutboundShipInqhdr(OutboundShipInq objOutboundShipInq);
        OutboundShipInq OutboundShipInqdtl(OutboundShipInq objOutboundShipInq);
        OutboundShipInq GEtStrgBillTYpe(OutboundShipInq objOutboundShipInq);  //CR2018-03-09-001 Added By Soniya
        OutboundShipInq GetTotalPalletCount(OutboundShipInq objOutboundShipInq);  //CR2018-03-13-001 Added By Nithya
        OutboundShipInq GetObShipdtlTotalPalletCount(OutboundShipInq objOutboundShipInq);  //CR2018-03-13-001 Added By Nithya

        OutboundShipInq OutboundShipInqBillofLaddingExcel(OutboundShipInq objOutboundShipInq);  //CR2018-03-13-001 Added By Nithya
        OutboundShipInq GetTotCubesRpt(OutboundShipInq objOutboundShipInq);  //CR2018-05-26-001 Added By Nithya
        OutboundShipInq GETRptValue(OutboundShipInq objOutboundShipInq);  
        OutboundShipInq Add_To_proc_save_audit_trail(OutboundShipInq objOutboundShipInq);
        OutboundShipInq Get_SoNo(OutboundShipInq objOutboundShipInq);
        OutboundShipInq GetShipUnPostGrid(OutboundShipInq objOutboundShipInq);
        void SaveShipUnPost(OutboundShipInq objOutboundShipInq);
        DataTable OutboundShipInqBillofLaddingExcelTemplate(string l_str_cmp_id, string l_str_Ship_DocId);
    }
}
