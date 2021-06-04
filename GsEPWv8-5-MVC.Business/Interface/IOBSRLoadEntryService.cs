using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IOBSRLoadEntryService
    {
        OBSRLoadInquiry GetOBSRSummary(OBSRLoadInquiry objOBSRLoadInquiry);
        OBSRLoadInquiry getOBLoadHdrByBatch(OBSRLoadInquiry objOBSRLoadInquiry);
        OBSRLoadInquiry GetLoadEntry(OBSRLoadInquiry objOBSRLoadInquiry);
        bool SaveTempBol(string p_str_cmp_id, string p_str_load_number, string p_str_so_list, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id, DataTable dtSoList);
        bool fnSaveMasterBol(string p_str_cmp_id, string p_str_load_number, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id, DataTable dtSoList);

        bool SaveOBLoadEntry(string p_str_cmp_id, string p_str_load_number, string p_str_so_list, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id, DataTable dtSoList);
        bool DeleteLoadEntry(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number);
        int GetOBLoadDocId();
        string GetOBLoadDocIdBySR(string p_str_cmp_id, string p_str_so_num);
        OBSRLoadInquiry GetOBSRSummaryForLoadEntry(OBSRLoadInquiry objOBSRLoadInquiry);
        OBGetSRBOLConfRpt GetOBBOLByBatch(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id, string p_str_load_doc_id, string p_str_is_same_ship_to);
        OBGetSRBOLConfRpt GetOBSRBOLConfRptByLoadNumber(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number);
    }
}
