using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Model
{
    public class GMailer2 : BasicEntity
    {
        public string GmailUsername { get; set; }
        public string GmailPassword { get; set; }
        public string GmailHost { get; set; }
        public int GmailPort { get; set; }
        public bool GmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public string Title { get; set; }

    }
    public class InboundReceivingModel : GMailer2
    {
        public string cmp_id { get; set; }
        public string whs_id { get; set; }
        public string rcv_dt_frm { get; set; }
        public string rcv_dt_to { get; set; }
        public string ib_doc_frm { get; set; }
        public string ib_doc_to { get; set; }

        public string WhsId { get; set; }
        public string ib_doc_id { get; set; }
        public string po_num { get; set; }
        public string rcvd_dt { get; set; }
        public string cont_id { get; set; }
        public string TotLots { get; set; }
        public string TotQty { get; set; }
        public string TotCtns { get; set; }
        public string status { get; set; }
        public string sent_dt { get; set; }
        public string STATUS { get; set; }
        public string CONTAINER { get; set; }
        public string IB_DOC_ID { get; set; }
        public string REF_NUM { get; set; }
        public string LOT_ID { get; set; }
        public string PO_NUM { get; set; }
        public string RCVD_DT { get; set; }
        public string STYLE { get; set; }
        public string COLOR { get; set; }
        public string SIZE { get; set; }
        public string ITEM_NAME { get; set; }
        public string TOTAL_CTNS { get; set; }
        public string PPK { get; set; }
        public string TOTAL_QTY { get; set; }
        public string LENGTH { get; set; }
        public string WIDTH { get; set; }
        public string DEPTH { get; set; }
        public string WEIGHT { get; set; }
        public string CUBE { get; set; }
        public string PALET_ID { get; set; }
        public string Email { get; set; }
        public string whsdtl { get; set; }
        public string h_cmpid { get; set; }




        public IList<InboundReceiving> LstInboundReceiving { get; set; }
        public IList<InboundReceiving> LstInboundReceivingHdr { get; set; }

        public IList<Company> ListCompanyPickDtl { get; set; }
    }
}
