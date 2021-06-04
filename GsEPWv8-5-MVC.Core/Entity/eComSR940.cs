using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class eComSR940HdrDtl
    {
        public string CompID { get; set; }
        public string HeaderInfo { get; set; }
        public string BatchNo { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public string Store { get; set; }
        public string Dept { get; set; }
        public string CustPO { get; set; }
        public string SOID { get; set; }
        public string RelID { get; set; }
        public DateTime ReqDt { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime CancelDt { get; set; }
        public Int32 DtlCount { get; set; }
        public string ShipVia { get; set; }
        public string ShipName { get; set; }
        public string ShipAdd1 { get; set; }
        public string ShipAdd2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string NoteHdr { get; set; }
        public int TtalQty { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public Int32 POLine { get; set; }
        public string Style { get; set; }
        public string CustSKU { get; set; }
        public string StyleQty { get; set; }
        public string StyleCarton { get; set; }
        public string StylePPK { get; set; }
        public double StyleCube { get; set; }
        public double StyleWgt { get; set; }
        public string StyleDesc { get; set; }
        public int StyleStatus { get; set; }
        public string StatusDesc { get; set; }
        public string Itm_Code { get; set; }
    }
    public class eComSR940 
    {
        public string cmp_id { get; set; }
        public string file_name { get; set; }
        public string CompID { get; set; }
        public string HeaderInfo { get; set; }
        public string BatchNo { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public string Store { get; set; }
        public string Dept { get; set; }
        public string CustPO { get; set; }
        public string SOID { get; set; }
        public string RelID { get; set; }
        public DateTime ReqDt { get; set; }
        public DateTime StartDt { get; set; }
        public DateTime CancelDt { get; set; }
        public Int32 DtlCount { get; set; }
        public string ShipVia { get; set; }
        public string ShipName { get; set; }
        public string ShipAdd1 { get; set; }
        public string ShipAdd2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string NoteHdr { get; set; }
        public int TtalQty { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public Int32 POLine { get; set; }
        public string Style { get; set; }
        public string CustSKU { get; set; }
        public int StyleQty { get; set; }
        public int StyleCarton { get; set; }
        public string StylePPK { get; set; }
        public double StyleCube { get; set; }
        public double StyleWgt { get; set; }
        public string StyleDesc { get; set; }
        public int StyleStatus { get; set; }
        public string StatusDesc { get; set; }
        public string Itm_Code { get; set; }
        public int dtl_Count { get; set; }
        public string so_num { get; set; }
        public int ItemLine { get; set; }
        public string Image_Path { get; set; }
        public string cmp_name { get; set; }
        public string addr_line1 { get; set; }
        public string city { get; set; }
        public string state_id { get; set; }
        public string post_code { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public string user_id { get; set; }
        public DateTime uploaded_date { get; set; }
        public string process_status { get; set; }
        public string error_desc { get; set; }
        public bool error_mode { get; set; }
        public string view_flag { get; set; }
        public int File_Line_No { get; set; }
        public string Header_Info { get; set; }
        public int SR_UPLOAD_FILE_COUNT { get; set; }
        public int Header_count { get; set; }
        public string quote_num { get; set; }
        public string cust_id { get; set; }

        public string cust_name { get; set; }
        public string store_id { get; set; }
        public string dept_id { get; set; }
        public string cust_ordr_num { get; set; }

        public string ordr_num { get; set; }
        public DateTime req_ship_dt { get; set; }

        public DateTime ship_dt { get; set; }
        public DateTime cancel_dt { get; set; }
        public int line_num { get; set; }
        public string itm_num { get; set; }
        public int ordr_qty { get; set; }
        public int ordr_ctns { get; set; }
        public int itm_qty { get; set; }
        public decimal cube { get; set; }
        public decimal wgt { get; set; }

        public string itm_name { get; set; }
        public string File_Length { get; set; }
        public string so_dtFm { get; set; }
        public string so_dtTo { get; set; }
        public string status { get; set; }
        public string Error_flag { get; set; }
        public int statuscount { get; set; }

        public IList<Company> ListCompanyPickDtl { get; set; }
        public IList<eComSR940> ListeCom940SRUploadDtl { get; set; }
        public IList<eComSR940> CheckExistStyle { get; set; }
        public IList<eComSR940> ListeCom940SRDtl { get; set; }
        public IList<eComSR940> ListeCom940SRUploadDtlRpt { get; set; }
        public IList<eComSR940> ListAddECom940SRUpload { get; set; }
        public IList<eComSR940> ListCheckExistSRUploadFile { get; set; }
        public IList<Company> ListCompanyAddresHdrDtls { get; set; }
        public IList<eComSR940> ListEcomError { get; set; }
        public IList<eComSR940> ListEcombatchError { get; set; }
        public IList<eComSR940> ListNo_Of_Records { get; set; }
        public IList<eComSR940> ListCheckExistBatchNo { get; set; }

    }
}
