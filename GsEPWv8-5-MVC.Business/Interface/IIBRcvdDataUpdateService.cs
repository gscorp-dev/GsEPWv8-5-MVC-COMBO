using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IIBRcvdDataUpdateService
    {
        IBRcvdDataUpdate GetRcvdHdr(IBRcvdDataUpdate objIBRcvdDataUpdateHdr);
        IBRcvdDataUpdate ListDocItemList(IBRcvdDataUpdate objIBRcvdDataUpdateHdr);
        string SaveIBRcvdData(string p_str_cmp_id, DataTable p_dt_ib_rcvd_updt_hdr, DataTable p_dt_ib_rcvd_updt_dtl,string p_str_save_hdr, string p_str_cntr_type, bool p_bln_excld_bill);
        DataTable GetIBCheckRcvdDtCanUpdate(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_rcvd_dt);
    }
}
