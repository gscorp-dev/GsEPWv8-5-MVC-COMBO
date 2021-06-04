using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
   public class OutboundShipInqModel: BasicEntity
    {

        public string cmp_id { get; set; }
        public string ship_doc_id { get; set; }
        public string shipdate { get; set; }
        public string ship_doc_id_Fm { get; set; }
        public string ship_doc_id_To { get; set; }
        public string aloc_doc_id  { get; set; }
        public string status { get; set; }
        public string Ship_dt { get; set; }
        public string Ship_dt_Fm { get; set; }
        public string Ship_dt_To { get; set; }
        public string ship_to { get; set; }
        public string whs_id { get; set; }
        public string ship_via_name { get; set; }
        public string cust_id { get; set; }
        public string ship_post_dt { get; set; }
        public string ship_ready_dt { get; set; }
        public string seal_num { get; set; }
        public string picked_by { get; set; }
        public string track_num { get; set; }
        public string cust_po_num { get; set; }
        public string ship_note { get; set; }
        public string cont_id { get; set; }
        public string po_num { get; set; }

        public string whs_name { get; set; }
            public string whsdtl { get; set; }    
            public string shipto_id { get; set; }        
            public string shiptodtl { get; set; }       
            public string cust_name { get; set; }
            public string custdtl { get; set; }
        public string ShipDt { get; set; }
        public string CustId { get; set; }
        public string ShipTo { get; set; }
        public string WhsId { get; set; }
        public string ShipViaName { get; set; }
        public string AlocDocId { get; set; }
        public string Ship_post_dt { get; set; }
        public string cmp_name { get; set; }
        public string addr_line1 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public int ctn_cnt { get; set; }
        public int line_qty { get; set; }
        public string so_num { get; set; }
        public string cust_ord { get; set; }
        public string pick_uom { get; set; }
        public int line_num { get; set; }
        public string itm_num { get; set; }
        public string itm_color { get; set; }
        public string itm_size { get; set; }
        public int ctn_line { get; set; }
        public int ctn_qty { get; set; }
        public string cust_ordr_num { get; set; }
        public string ship_to_name { get; set; }
        public string cust_store { get; set; }
        public int itm_qty { get; set; }

        public string cust_dept { get; set; }
        public string ship_type { get; set; }
        public string notes { get; set; }
        public int so_dtl_line { get; set; }
        public string so_itm_num { get; set; }
        public string so_itm_color { get; set; }
        public string so_itm_size { get; set; }
        public int ship_qty { get; set; }
        public string so_itm_code { get; set; }
        public string ship_Uom { get; set; }
        public string qtyUOM { get; set; }
        public string SoldToId { get; set; }
        public string is_company_user { get; set; }
        public int TotCtns { get; set; }
        public int TotQty { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal depth { get; set; }
        public decimal wgt { get; set; }
        public decimal cube { get; set; }
      
        public string View_Flag { get; set; }
        public string CNTR_PALLET { get; set; }
        public string CNTR_ID { get; set; }
        public decimal PALLET_WEIGHT { get; set; }
        public decimal PALLET_CUBE { get; set; }
        public string PALLET_NOTE { get; set; }      
        public string PROCESS_ID { get; set; }
        public string PONUM { get; set; }
        public string Shipdt { get; set; }
        public DateTime OB_Ship_dt { get; set; }
        public DateTime Bol_ShipDt { get; set; }

        
        public string bill_type { get; set; } //CR2018-03-09-001 Added By Soniya
        public string bill_inout_type { get; set; } //CR2018-03-09-001 Added By Soniya
        public int TotShipPallets { get; set; } //CR2018-03-13-001 Added By Nithya
        public string TotalPallets { get; set; } //CR2018-03-13-001 Added By Nithya

        public string tmp_cmp_id { get; set; }
        public string IS3RDUSER { get; set; }

        public string Image_Path { get; set; }

        public DateTime cancel_dt { get; set; }

        public float TotCube { get; set; } //CR2018-05-26-001 Added By Nithya
        public string quote_num { get; set; }
        public int TotWgt { get; set; }
        public string screentitle { get; set; }
        public string shippost_dt { get; set; }
        public string ref_num { get; set; }
        public IList<OutboundShipInq> LstOutboundShipInqdetail { get; set; }
        public IList<LookUp> ListLookUpDtl { get; set; }
        public IList<Company> ListCompanyDtl { get; set; }
        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<OutboundShipInq> ListShipPost { get; set; }
        public IList<OutboundShipInq> ListCheckShipPost { get; set; }
        public IList<OutboundShipInq> ListGetWgtCubeValue { get; set; }
        public IList<OutboundShipInq> ListGETPONUM { get; set; }
        public IList<OutboundShipInq> ListStrgBillType { get; set; } //CR2018-03-09-001 Added By Soniya
        public IList<OutboundShipInq> OutboundShipInqGetCustDetails { get; set; }
        public IList<OutboundShipInq> ListGETContainerID { get; set; }
        public IList<OutboundShipInq> ListGetContId { get; set; }
        public IList<OutboundShipInq> ListGETRateID { get; set; }
        public IList<OutboundShipInq> LstOutboundShipInqGetShipToDetails { get; set; }
        public IList<OutboundShipInq> LstOutboundShipInqGetShipFromDetails { get; set; }
        public IList<OutboundShipInq> LstOutboundShipInqSummaryRpt { get; set; }
        public IList<OutboundShipInq> LstOutboundShipInqpackingSlipRpt { get; set; }
        public IList<OutboundShipInq> LstOutboundShipInqBillofLaddingRpt { get; set; }
        public IList<OutboundShipInq> LstPalletCount { get; set; }//CR2018-03-13-001 Added By Nithya
        public IList<OutboundShipInq> LstshipPalletCount { get; set; }//CR2018-03-13-001 Added By Nithya
        public IList<OutboundShipInq> LstshipbillOfladding { get; set; }
    }
}
