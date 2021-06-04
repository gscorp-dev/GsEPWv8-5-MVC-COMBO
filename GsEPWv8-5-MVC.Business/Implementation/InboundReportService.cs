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
   public  class InboundReportService: IInboundReportService
    {
        IInboundReportRepository objRepository = new InboundReportRepository();
        public InboundReport GetInboundRpt(InboundReport objInboundReport)
        {
            return objRepository.GetInboundRpt(objInboundReport);
        }
        public InboundReport InboundWhsDetails(string term, string cmpid)
        {
            return objRepository.InboundWhsDetails(term, cmpid);
        }
        public InboundReport GetInboundRptStyle(InboundReport objInboundReport)
        {
            return objRepository.GetInboundRptStyle(objInboundReport);
        }
        public InboundReport GetInboundRptDate(InboundReport objInboundReport)
        {
            return objRepository.GetInboundRptDate(objInboundReport);
        }
        public InboundRcvngRptByStyleExcel GetInboundRcvngRptByStyleExcel(InboundRcvngRptByStyleExcel objInboundReport)
        {
            return objRepository.GetInboundRcvngRptByStyleExcel(objInboundReport);
        }
        public InboundRcvngRptByDateExcel GetInboundRcvngRptByDateExcel(InboundRcvngRptByDateExcel objInboundReport)
        {
            return objRepository.GetInboundRcvngRptByDateExcel(objInboundReport);
        }
        public InboundReport GetDftWhs(InboundReport objInboundReport)
        {
            return objRepository.GetDftWhs(objInboundReport);
        }

        public IBRcvdRptByCntrDtl GetIBRcvdRptByCntrDtl(IBRcvdRptByCntrDtl objIBRcvdRptByCntrDtl)
        {
            return objRepository.GetIBRcvdRptByCntrDtl(objIBRcvdRptByCntrDtl);

        }
        public DataTable GetInboundRptDateExcel(string l_str_cmp_id, string p_str_cntr_id, string l_str_whs_id, string l_str_IBDocNumFm, string l_str_IBDocNumTo, string l_InboundRcvdDtFm, string l_InboundRcvdDtTo,
             string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_itm_name, string p_str_status)
        {
            return objRepository.GetInboundRptDateExcel(l_str_cmp_id, p_str_cntr_id, l_str_whs_id, l_str_IBDocNumFm, l_str_IBDocNumTo, l_InboundRcvdDtFm, l_InboundRcvdDtTo,
                p_str_itm_num, p_str_itm_color, p_str_itm_size, p_str_itm_name, p_str_status);

        }
        public DataTable GetInboundRptStyleExcel(string l_str_cmp_id, string p_str_cntr_id, string l_str_whs_id, string l_str_IBDocNumFm, string l_str_IBDocNumTo,
            string l_InboundRcvdDtFm, string l_InboundRcvdDtTo, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_itm_name, string p_str_status)
        {
            return objRepository.GetInboundRptStyleExcel(l_str_cmp_id, p_str_cntr_id, l_str_whs_id, l_str_IBDocNumFm, l_str_IBDocNumTo, l_InboundRcvdDtFm, l_InboundRcvdDtTo,
                p_str_itm_num, p_str_itm_color, p_str_itm_size, p_str_itm_name, p_str_status);
        }

        public DataTable GetInboundRptContainerExcel(string l_str_cmp_id, string l_str_whs_id, string l_str_cntr_id, string l_str_rcvd_dt_from, string l_str_rcvd_dt_to)
        {
            return objRepository.GetInboundRptContainerExcel(l_str_cmp_id, l_str_whs_id, l_str_cntr_id, l_str_rcvd_dt_from, l_str_rcvd_dt_to);
        }

        public DataTable GetIBSummaryRpt(InboundReport objInboundReport)
        {
            return objRepository.GetIBSummaryRpt(objInboundReport);
        }
    }
}
