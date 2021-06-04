using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class tbl_shipreq_grid
        {
        public tbl_shipreq_grid()
            {
            }
        public string HeaderInfo { get; set; }
        public string CompID { get; set; }
        public string BatchNo { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public string Store { get; set; }
        public string Dept { get; set; }
        public string CustPO { get; set; }
        public string SOID { get; set; }
        public string so_num { get; set; }
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
        public Int32 POLine{ get; set; }
        public string Style { get; set; }
        public string Itm_Code { get; set; }
        public string CustSKU { get; set; }
        public int StyleQty { get; set; }
        public int StyleCarton { get; set; }
        public string StylePPK { get; set; }
        public double StyleCube { get; set; }
        public double StyleWgt { get; set; }
        public string StyleDesc { get; set; }
        public int StyleStatus { get; set; }
        public string StatusDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int TtalQty { get; set; }
      

    }
    }
