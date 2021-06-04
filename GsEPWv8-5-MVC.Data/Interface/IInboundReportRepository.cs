using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Interface
{
    public interface IInboundReportRepository
    {
        InboundReport GetInboundRpt(InboundReport objInboundReport);
        InboundReport InboundWhsDetails(string term, string cmpid);
        InboundReport GetInboundRptStyle(InboundReport objInboundReport);
        InboundReport GetInboundRptDate(InboundReport objInboundReport);
        InboundReport GetDftWhs(InboundReport objInboundReport);

        InboundRcvngRptByStyleExcel GetInboundRcvngRptByStyleExcel(InboundRcvngRptByStyleExcel objInboundReport);
        InboundRcvngRptByDateExcel GetInboundRcvngRptByDateExcel(InboundRcvngRptByDateExcel objInboundReport);
        IBRcvdRptByCntrDtl GetIBRcvdRptByCntrDtl(IBRcvdRptByCntrDtl objIBRcvdRptByCntrDtl);
        DataTable GetInboundRptDateExcel(string l_str_cmp_id, string p_str_cntr_id, string l_str_whs_id, string l_str_IBDocNumFm, string l_str_IBDocNumTo, string l_InboundRcvdDtFm,
            string L_InboundRcvdDtTo,string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_itm_name, string p_str_status);
        DataTable GetInboundRptStyleExcel(string l_str_cmp_id, string p_str_cntr_id, string l_str_whs_id, string l_str_IBDocNumFm, string l_str_IBDocNumTo, string l_InboundRcvdDtFm,
            string l_InboundRcvdDtTo, string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_itm_name, string p_str_status);
        DataTable GetInboundRptContainerExcel(string l_str_cmp_id, string l_str_whs_id, string l_str_cntr_id, string l_str_rcvd_dt_from, string l_str_rcvd_dt_to);
        DataTable GetIBSummaryRpt(InboundReport objInboundReport);
    }
}
