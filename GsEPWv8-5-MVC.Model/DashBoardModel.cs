using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class DashBoardDtlModel
    {
        public string cmp_id { get; set; }
        public string p_str_cmp_id { get; set; }
        public string cmp_name { get; set; }

        public string cust_id { get; set; }
        public string InboundDesc { get; set; }
        public string Total { get; set; }
        public string OutboundDesc { get; set; }
        public string Bill { get; set; }
        public string VAS { get; set; }
        public string ShipRequestEntry { get; set; }
        public string AlocEntry { get; set; }
        public string ShipEntry { get; set; }
        public string ShipRequestTotal { get; set; }
        public string AlocTotal { get; set; }
        public string ShipTotal { get; set; }
        public string InboundRcvdDesc { get; set; }
        public string RcvdTotal { get; set; }
        public string frm_dt { get; set; }
        public string to_dt { get; set; }
        public string p_str_frm_dt { get; set; }
        public string p_str_to_dt { get; set; }
        public string InboundOpenDesc { get; set; }
        public string IBOpenTotal { get; set; }
        public string InboundPostDesc { get; set; }
        public string IBOPostTotal { get; set; }
        public string VAS_OPEN_DESCRIPTION { get; set; }
        public string VAS_OPEN_TOTAL { get; set; }
        public string BillING_OPEN_DESCRIPTION { get; set; }
        public string BILLING_OPEN_TOTAL { get; set; }
        public string OBShipRequestEntry { get; set; }
        public string OBShipRequestTotal { get; set; }
        public string IS3RDUSER { get; set; }

    }
    public class DashBoardModel : DashBoardDtlModel
    {
        public IList<DashBoardDtl> LstDashBoard { get; set; }
        public IList<DashBoardDtl> LstOutBound { get; set; }
        public IList<DashBoardDtl> LstOutBoundShipREq { get; set; }
        public IList<DashBoardDtl> LstOutBoundAloc { get; set; }
        public IList<DashBoardDtl> LstOutBoundShipPost { get; set; }
        public IList<DashBoardDtl> LstVas { get; set; }
        public IList<DashBoardDtl> LstVasOpen { get; set; }
        public IList<DashBoardDtl> LstBillOpen { get; set; }
        public IList<DashBoardDtl> LstBill { get; set; }
        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<DashBoardDtl> LstInboundRcvdData { get; set; }
        public IList<DashBoardDtl> LstInboundOpenData { get; set; }
        public IList<DashBoardDtl> LstInboundPostData { get; set; }

    }
}
