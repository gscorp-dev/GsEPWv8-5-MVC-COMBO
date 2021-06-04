using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class IBRcvdDataUpdateService: IIBRcvdDataUpdateService

    {
        IBRcvdDataUpdateRepository objRepository = new IBRcvdDataUpdateRepository();
        public  IBRcvdDataUpdate GetRcvdHdr(IBRcvdDataUpdate objIBRcvdDataUpdateHdr)
        {
            return objRepository.GetRcvdHdr(objIBRcvdDataUpdateHdr);
        }
        public IBRcvdDataUpdate ListDocItemList(IBRcvdDataUpdate objIBRcvdDataUpdateHdr)
        {
            return objRepository.ListDocItemList(objIBRcvdDataUpdateHdr);
        }

        public string SaveIBRcvdData(string p_str_cmp_id, DataTable p_dt_ib_rcvd_updt_hdr, DataTable p_dt_ib_rcvd_updt_dtl,string p_str_save_hdr, string p_str_cntr_type, bool p_bln_excld_bill)
        {
            return objRepository.SaveIBRcvdData( p_str_cmp_id, p_dt_ib_rcvd_updt_hdr, p_dt_ib_rcvd_updt_dtl, p_str_save_hdr, p_str_cntr_type, p_bln_excld_bill);
        }

        public DataTable GetIBCheckRcvdDtCanUpdate(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_rcvd_dt)
        {
            return objRepository.GetIBCheckRcvdDtCanUpdate(p_str_cmp_id, p_str_ib_doc_id, p_str_rcvd_dt);

        }
    }
}
