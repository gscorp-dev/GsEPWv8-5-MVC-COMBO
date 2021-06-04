using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class OutboundShipInqService: IOutboundShipInqService
    {
        IOutboundShipInqRepository objRepository = new OutboundShipInqRepository();
        public OutboundShipInq GetOutboundShipInq(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.GetOutboundShipInq(objOutboundShipInq);
        }
        public OutboundShipInq OutboundShipInqGetCustDetails(string term, string cmpid)
        {
            return objRepository.OutboundShipInqGetCustDetails(term, cmpid);
        }
        public OutboundShipInq OutboundShipInqGetShipToDetails(string term, string cmpid)
        {
            return objRepository.OutboundShipInqGetShipToDetails(term, cmpid);
        }
        public OutboundShipInq OutboundShipInqGetShipFromDetails(string term, string cmpid)
        {
            return objRepository.OutboundShipInqGetShipFromDetails(term, cmpid);
        }       
        public OutboundShipInq GetContId(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.GetContId(objOutboundShipInq);
        }
        public OutboundShipInq GetShipPostGrid(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.GetShipPostGrid(objOutboundShipInq);
        }
        public OutboundShipInq GetPoNum(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.GetPoNum(objOutboundShipInq);
        }
       
        public OutboundShipInq OutboundShipInqSummaryRpt(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.OutboundShipInqSummaryRpt(objOutboundShipInq);
        }
        public void SaveShipPost(OutboundShipInq objOutboundShipInq)
        {
            objRepository.SaveShipPost(objOutboundShipInq);
        }
        public void InsertShipPost(OutboundShipInq objOutboundShipInq)
        {
            objRepository.InsertShipPost(objOutboundShipInq);
        }
        public void InsertPalletShipDtl(OutboundShipInq objOutboundShipInq)
        {
            objRepository.InsertPalletShipDtl(objOutboundShipInq);
        }
       
        public OutboundShipInq GetCheckShipPost(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.GetCheckShipPost(objOutboundShipInq);
        }
        public OutboundShipInq OutboundShipInqpackSlipRpt(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.OutboundShipInqpackSlipRpt(objOutboundShipInq);
        }
        public OutboundShipInq OutboundShipInqBillofLaddingRpt(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.OutboundShipInqBillofLaddingRpt(objOutboundShipInq);
        }
        public OutboundShipInq OutboundShipInqhdr(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.OutboundShipInqhdr(objOutboundShipInq);
        }
        public OutboundShipInq OutboundShipInqdtl(OutboundShipInq objOutboundShipInq)
        {
            return objRepository.OutboundShipInqdtl(objOutboundShipInq);
        }
        public OutboundShipInq GEtStrgBillTYpe(OutboundShipInq objOutboundShipInq) //CR2018-03-09-001 Added By Soniya
        {
            return objRepository.GEtStrgBillTYpe(objOutboundShipInq);
        }
        public OutboundShipInq GetTotalPalletCount(OutboundShipInq objOutboundShipInq) //CR2018-03-13-001 Added By Nithya
        {
            return objRepository.GetTotalPalletCount(objOutboundShipInq);
        }
        public OutboundShipInq GetObShipdtlTotalPalletCount(OutboundShipInq objOutboundShipInq) //CR2018-03-13-001 Added By Nithya
        {
            return objRepository.GetObShipdtlTotalPalletCount(objOutboundShipInq);
        }
        public OutboundShipInq OutboundShipInqBillofLaddingExcel(OutboundShipInq objOutboundShipInq) //CR2018-03-13-001 Added By Nithya
        {
            return objRepository.OutboundShipInqBillofLaddingExcel(objOutboundShipInq);
        }
        public OutboundShipInq GetTotCubesRpt(OutboundShipInq objOutboundShipInq) //CR2018-05-26-001 Added By Nithya
        {
            return objRepository.GetTotCubesRpt(objOutboundShipInq);
        }
        public OutboundShipInq GETRptValue(OutboundShipInq objOutboundShipInq) //CR2018-05-26-001 Added By Nithya
        {
            return objRepository.GETRptValue(objOutboundShipInq);
        }
        public OutboundShipInq Add_To_proc_save_audit_trail(OutboundShipInq objOutboundShipInq) //CR2018-05-26-001 Added By Nithya
        {
            return objRepository.Add_To_proc_save_audit_trail(objOutboundShipInq);
        }
        public OutboundShipInq Get_SoNo(OutboundShipInq objOutboundShipInq) //CR2018-05-26-001 Added By Nithya
        {
            return objRepository.Get_SoNo(objOutboundShipInq);
        }
        public OutboundShipInq GetShipUnPostGrid(OutboundShipInq objOutboundShipInq) //CR2018-05-26-001 Added By Nithya
        {
            return objRepository.GetShipUnPostGrid(objOutboundShipInq);
        }
        public void SaveShipUnPost(OutboundShipInq objOutboundShipInq)
        {
            objRepository.SaveShipUnPost(objOutboundShipInq);
        }

        public DataTable OutboundShipInqBillofLaddingExcelTemplate(string l_str_cmp_id, string l_str_Ship_DocId)
        {
            return objRepository.OutboundShipInqBillofLaddingExcelTemplate(l_str_cmp_id, l_str_Ship_DocId);
        }
    }
}
