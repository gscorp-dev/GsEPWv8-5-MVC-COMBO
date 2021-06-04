using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
   public class InboundReportModel:BasicEntity
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string cntr_id { get; set; }
        public string ib_doc_id { get; set; }
        public string ib_doc_idFm { get; set; }
        public string ib_doc_idTo { get; set; }
        public string Style { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ItmName { get; set; }
        public string LotId { get; set; }
        public string LocId { get; set; }
        public string PoNum { get; set; }
        public string RcvdDt { get; set; }
        public string RcvdDtFm { get; set; }
        public string RcvdDtTo { get; set; }
        public int Ctns { get; set; }
        public int ppk { get; set; }
        public int TotalQty { get; set; }
        public decimal length { get; set; }
        public decimal Width { get; set; }
        public decimal height { get; set; }
        public decimal Wgt { get; set; }
        public decimal cube { get; set; }
        public string PaletId { get; set; }
        public string whsdtl { get; set; }
        public int tot_ctn { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public string cmp_name { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string addr_line1 { get; set; }
        public int itm_qty { get; set; }
        public string palet_id { get; set; }
        public string po_num { get; set; }
        public string itm_name { get; set; }
        public string loc_id { get; set; }
        public string cont_id { get; set; }
        public int ctn_qty { get; set; }
        public string itm_size { get; set; }
        public string itm_color { get; set; }
        public string itm_num { get; set; }
        public DateTime palet_dt { get; set; }
        public int wgt { get; set; }
        public string lot_id { get; set; }
        public int depth { get; set; }
        public int width { get; set; }
        public float ibcube { get; set; }
        public string dft_whs { get; set; }
        public string Image_Path { get; set; }
        public string status { get; set; }
        public string rcvd_status { get; set; }
        public string bill_status { get; set; }
        public IList<InboundReport> LstinboundRpt { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<InboundReport> LstWhsDetails { get; set; }
        public IList<Company> ListwhsPickDtl { get; set; }

    }

    public class IBRcvdRptByCntr
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string cntr_id { get; set; }
        public string ib_doc_id { get; set; }
        public string loc_id { get; set; }
        public string palet_dt { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public string itm_name { get; set; }
        public int itm_qty { get; set; }
        public int tot_ctn { get; set; }
        public int tot_qty { get; set; }
        public string addr_line1 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
    }

    public class IBRcvdRptByCntrDtlModel
    {

        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string cntr_id { get; set; }
        public string rcvd_dt_from { get; set; }
        public string rcvd_dt_to { get; set; }
        public IList<IBRcvdRptByCntr> ListIBRcvdRptByCntr { get; set; }
    }

}
