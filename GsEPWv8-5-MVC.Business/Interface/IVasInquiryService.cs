using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IVasInquiryService
    {
        void VasInsert(VasInquiry objVasInquiry);
        void SaveVasEntryDtl(VasInquiry objVasInquiry);
        void SaveVasEntryHdr(VasInquiry objVasInquiry);
        void UpdateVasEntryDtl(VasInquiry objVasInquiry);       
        void UpdateVasEntryHdr(VasInquiry objVasInquiry);
        void GetVasPost(VasInquiry objVasInquiry);
        void GetVasUnPost(VasInquiry objVasInquiry);
        VasInquiry GetDftWhs(VasInquiry objVasInquiry);
        VasInquiry LoadSrDetails(VasInquiry objVasInquiry);       
        VasInquiry GetVasEntry(VasInquiry objVasInquiry);
        void TruncateTempVasEntry(VasInquiry objVasInquiry);
        VasInquiry GetVasEntryGrid(VasInquiry objVasInquiry);
        void DeleteVasEntry(VasInquiry objVasInquiry);
        void DeleteVasedit(VasInquiry objVasInquiry);
        void UpdateVasRateHdr(VasInquiry objVasInquiry);
        VasInquiry GetVasUserId(VasInquiry objVasInquiry);
        VasInquiry GetVasEntryTempGridDtl(VasInquiry objVasInquiry);
        //VasInquiry CheckCompanyUser(VasInquiry objVasInquiry);
        VasInquiry GetVasEntryhdr(VasInquiry objVasInquiry);
        VasInquiry GetVasEntryGriddtl(VasInquiry objVasInquiry);
        VasInquiry GetUpdateTempVasEntryDtl(VasInquiry objVasInquiry);
        VasInquiry GetVasInquiryDetails(VasInquiry objVasInquiry);
        VasInquiry GetVasEntryDtl(VasInquiry objVasInquiry);
        VasInquiry GetVasIdDetail(VasInquiry objVasInquiry);
        VasInquiry GetWhsIdDetail(VasInquiry objVasInquiry);
        VasInquiry GetVasPostDetails(VasInquiry objVasInquiry);
        VasInquiry GetVashdr(VasInquiry objVasInquiry);
        VasInquiry GetVasdtl(VasInquiry objVasInquiry);
        VasInquiry GetVasInquiryDetailsRpt(VasInquiry objVasInquiry);
        VasInquiry GetReUpdateTempVasEntryDtl(VasInquiry objVasInquiry);
        VasInquiry GetVasInquiryPostRptCtnValues(VasInquiry objVasInquiry);
        VasInquiry GetQtyCount(VasInquiry objVasInquiry);
        DataTable GetVasInquiryDetailsRptExcel(string p_str_cmp_id, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status);
        DataTable GetVasPostDetailsExcel(string p_str_cmp_id, string SelectedID);
        VasInquiry fnGetVasRateIdDetails(string pstrCmpid);
    }
}
 