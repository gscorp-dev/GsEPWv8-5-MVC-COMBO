using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class ExceptionReportsModel
    {
        public string cmp_id { get; set; }
        public string ibcmp_id { get; set; }
        public string obcmp_id { get; set; }        
        public string cmp_name { get; set; }
        public string SRDtFm { get; set; }
        public string SRDtTo { get; set; }
        public string RcvdDtFm { get; set; }
        public string RcvdDtTo { get; set; }
        public string CMP_ID { get; set; }
        public string SO_NUM { get; set; }
        public DateTime SO_DT { get; set; }
        public string ALOC_STATUS { get; set; }
        public string ITM_NUM { get; set; }
        public string ITM_COLOR { get; set; }
        public string ITM_SIZE { get; set; }
        public int SO_QTY { get; set; }
        public string ALOC_DOC_ID { get; set; }
        public int ALOC_QTY { get; set; }
        public string SHIP_DOC_ID { get; set; }
        public int SHIP_QTY { get; set; }
        public int IN_QTY { get; set; }
        public int OUT_QTY { get; set; }
        public int HST_QTY { get; set; }
        public string screentitle { get; set; }
        public string tmp_cmp_id { get; set; }
        public string is_company_user { get; set; }
        public string user_id { get; set; }
        public string Image_Path { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string addr_line1 { get; set; }

        public string IB_DOC_ID { get; set; }
        public string LOT_STATUS { get; set; }
        public string LOT_ID { get; set; }
        public DateTime RCVD_DT { get; set; }
        public int LOT_QTY { get; set; }
        public int RCVD_QTY { get; set; }
        public string Itmdtl { get; set; }      
        public string ivcmp_id { get; set; }
        public string p_str_company { get; set; }
        public string ITM_CODE { get; set; }
        public string ITM_NAME { get; set; }
        public int ALOC_QTY_OUT { get; set; }
        public int AVAIL_QTY_IN { get; set; }
        
        public IList<Company> LstCmpName { get; set; }
        public IList<ExceptionReports> LstExceptionRpt { get; set; }
        public IList<ExceptionReports> LstIBExceptionRpt { get; set; }
        public IList<ExceptionReports> LstIVExceptionRpt { get; set; }
        public IList<ExceptionReports> LstItmDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<ExceptionReports> LstOBExceptionRpt { get; set; }


    }
}
