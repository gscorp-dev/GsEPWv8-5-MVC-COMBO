using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class tbl_so_hdr
    {
        public tbl_so_hdr()
        {
        }
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
        //public List<tbl_so_dtl> DetailList { get; set; }
    }
}

