using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
   public class ETAInquiry
    {
        public string cmp_id { get; set; }
        public string ETA_dt_Fm { get; set; }
        public string ETA_dt_To { get; set; }
        public string addr_line1 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public string Image_Path { get; set; }
        public string cmp_name { get; set; }
        public string CMP_ID { get; set; }
        public string IB_DOC_ID { get; set; }
        public DateTime IB_DOC_DT { get; set; }
        public string STATUS { get; set; }
        public string ETA_DT { get; set; }
        public string CNTR_ID { get; set; }
        public string REQ_NUM { get; set; }
        public string VEND_NAME { get; set; }
        public string NOTE { get; set; }
        public int TOT_CTN { get; set; }
        public int TOT_QTY { get; set; }
        public decimal TOT_WGT { get; set; }
        public decimal TOT_CUBE { get; set; }
        public string screentitle { get; set; }
        public string tmp_cmp_id { get; set; }
        public string ORDR_TYPE { get; set; }
        public string is_company_user { get; set; }
        public string user_id { get; set; }
        public string PO_NUM { get; set; }
        public int ITM_QTY { get; set; }
        public int DTL_LINE { get; set; }
        public decimal LENGTH { get; set; }
        public decimal WIDTH { get; set; }
        public decimal DEPTH { get; set; }
        public string ITM_NUM { get; set; }
        public string ITM_COLOR { get; set; }
        public string ITM_SIZE { get; set; }
        public string ITM_NAME { get; set; }
        public int CTN_LINE { get; set; }
        public class ETAInqRptSummaryExcel
        {
            public string CMP_ID { get; set; }
            public string IB_DOC_ID { get; set; }
            public DateTime IB_DOC_DT { get; set; }
            public string STATUS { get; set; }
            public string ETA_DT { get; set; }
            public string CNTR_ID { get; set; }
            public string REQ_NUM { get; set; }
            public string VEND_NAME { get; set; }
            public string NOTE { get; set; }
            public int TOT_CTN { get; set; }
            public int TOT_QTY { get; set; }
            public decimal TOT_WGT { get; set; }
            public decimal TOT_CUBE { get; set; }

        }
        public class ETAInqRptDetailExcel
        {
            public string CMP_ID { get; set; }
            public string IB_DOC_ID { get; set; }
            public DateTime IB_DOC_DT { get; set; }
            public string STATUS { get; set; }
            public string ETA_DT { get; set; }
            public string CNTR_ID { get; set; }
            public string REQ_NUM { get; set; }
            public string VEND_NAME { get; set; }
            public int DTL_LINE { get; set; }
            public int CTN_LINE { get; set; }
            public int TOT_QTY { get; set; }
            public int TOT_CTN { get; set; }
            public int ITM_QTY { get; set; }
            public string PO_NUM { get; set; }
            public string ITM_NUM { get; set; }
            public string ITM_COLOR { get; set; }
            public string ITM_SIZE { get; set; }
            public string ITM_NAME { get; set; }
            public decimal LENGTH { get; set; }
            public decimal WIDTH { get; set; }
            public decimal DEPTH { get; set; }
            public decimal TOT_WGT { get; set; }
            public decimal TOT_CUBE { get; set; }
            public string NOTE { get; set; }

        }
        public IList<Company> LstCmpName { get; set; }
        public IList<ETAInquiry> LstIBETAInqdetail { get; set; }
        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
    }
}
