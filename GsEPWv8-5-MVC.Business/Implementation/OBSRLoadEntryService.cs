using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class OBSRLoadEntryService: IOBSRLoadEntryService
    {


        IOBSRLoadEntryRepository objRepository = new OBSRLoadEntryRepository();

        public OBSRLoadInquiry getOBLoadHdrByBatch(OBSRLoadInquiry objOBSRLoadInquiry)
        {
            return objRepository.getOBLoadHdrByBatch(objOBSRLoadInquiry);
        }
        public OBSRLoadInquiry GetOBSRSummary(OBSRLoadInquiry objOBSRLoadInquiry)
        {
            return objRepository.GetOBSRSummary(objOBSRLoadInquiry);
        }

        public  OBSRLoadInquiry GetOBSRSummaryForLoadEntry(OBSRLoadInquiry objOBSRLoadInquiry)
        {
            return objRepository.GetOBSRSummaryForLoadEntry(objOBSRLoadInquiry);
        }
        public OBSRLoadInquiry GetLoadEntry(OBSRLoadInquiry objOBSRLoadInquiry)
        {
            return objRepository.GetLoadEntry(objOBSRLoadInquiry);
        }
        public int GetOBLoadDocId()
        {
            return objRepository.GetOBLoadDocId();
        }

        public bool SaveOBLoadEntry(string p_str_cmp_id, string p_str_load_number, string p_str_so_list, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id,DataTable dtSoList)
        {
            return objRepository.SaveOBLoadEntry(p_str_cmp_id, p_str_load_number, p_str_so_list, objOBSRLoadEntryHdr, user_id, dtSoList);
        }
        public bool SaveTempBol(string p_str_cmp_id, string p_str_load_number, string p_str_so_list, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id, DataTable dtSoList)
        {
            return objRepository.SaveTempBol(p_str_cmp_id, p_str_load_number, p_str_so_list, objOBSRLoadEntryHdr, user_id, dtSoList);
        }

        public bool fnSaveMasterBol(string p_str_cmp_id, string p_str_load_number, List<OBSRLoadEntryHdr> objOBSRLoadEntryHdr, string user_id, DataTable dtSoList)
        {
            return objRepository.fnSaveMasterBol(p_str_cmp_id, p_str_load_number,  objOBSRLoadEntryHdr, user_id, dtSoList);
        }

        public bool DeleteLoadEntry(string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number)
        {
            return objRepository.DeleteLoadEntry(p_str_cmp_id,  p_str_load_doc_id, p_str_load_number);
        }
        public OBGetSRBOLConfRpt GetOBSRBOLConfRptByLoadNumber(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id, string p_str_load_doc_id, string p_str_load_number)
        {
            return objRepository.GetOBSRBOLConfRptByLoadNumber(objOBSRBOLConfRptData, p_str_cmp_id, p_str_load_doc_id, p_str_load_number);
        }

        public string GetOBLoadDocIdBySR(string p_str_cmp_id, string p_str_so_num)
        {
            return objRepository.GetOBLoadDocIdBySR( p_str_cmp_id,  p_str_so_num);
        }
        public OBGetSRBOLConfRpt GetOBBOLByBatch(OBGetSRBOLConfRpt objOBSRBOLConfRptData, string p_str_cmp_id, string p_str_load_doc_id, string p_str_is_same_ship_to)
        {
            return objRepository.GetOBBOLByBatch(objOBSRBOLConfRptData, p_str_cmp_id, p_str_load_doc_id, p_str_is_same_ship_to);
        }

    }
}
