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
    public class VasInquiryService : IVasInquiryService
    {
        VasInquiryRepository objRepository = new VasInquiryRepository();

        public void VasInsert(VasInquiry objVasInquiry)
        {
            objRepository.VasInsert(objVasInquiry);
        }
        public void DeleteVasEntry(VasInquiry objVasInquiry)
        {
            objRepository.DeleteVasEntry(objVasInquiry);
        }
        //public VasInquiry CheckCompanyUser(VasInquiry objVasInquiry)
        //{
        //    return objRepository.CheckCompanyUser(objVasInquiry);
        //}
        public void DeleteVasedit(VasInquiry objVasInquiry)
        {
            objRepository.DeleteVasedit(objVasInquiry);
        }
        public void GetVasPost(VasInquiry objVasInquiry)
        {
            objRepository.GetVasPost(objVasInquiry);
        }
        public void GetVasUnPost(VasInquiry objVasInquiry)
        {
            objRepository.GetVasUnPost(objVasInquiry);
        }
       
        public VasInquiry LoadSrDetails(VasInquiry objVasInquiry)
        {
            return objRepository.LoadSrDetails(objVasInquiry);
        }
        public VasInquiry GetUpdateTempVasEntryDtl(VasInquiry objVasInquiry)
        {
            return objRepository.GetUpdateTempVasEntryDtl(objVasInquiry);
        }
        public VasInquiry GetDftWhs(VasInquiry objVasInquiry)
        {
            return objRepository.GetDftWhs(objVasInquiry);
        }
       
        public VasInquiry GetVasEntryhdr(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasEntryhdr(objVasInquiry);
        }
        public VasInquiry GetVasEntryGriddtl(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasEntryGriddtl(objVasInquiry);
        }
      
        public VasInquiry GetVasEntryGrid(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasEntryGrid(objVasInquiry);
        }
        public void SaveVasEntryDtl(VasInquiry objVasInquiry)
        {
            objRepository.SaveVasEntryDtl(objVasInquiry);
        }
        public void SaveVasEntryHdr(VasInquiry objVasInquiry)
        {
            objRepository.SaveVasEntryHdr(objVasInquiry);
        }
        public void UpdateVasEntryDtl(VasInquiry objVasInquiry)
        {
            objRepository.UpdateVasEntryDtl(objVasInquiry);
        }
        public void UpdateVasEntryHdr(VasInquiry objVasInquiry)
        {
            objRepository.UpdateVasEntryHdr(objVasInquiry);
        }
        public void TruncateTempVasEntry(VasInquiry objVasInquiry)
        {
            objRepository.TruncateTempVasEntry(objVasInquiry);
        }
        public void UpdateVasRateHdr(VasInquiry objVasInquiry)
        {
            objRepository.UpdateVasRateHdr(objVasInquiry);
        }
      
        public VasInquiry GetVasInquiryDetails(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasInquiryDetails(objVasInquiry);
        }
        public VasInquiry GetVasUserId(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasUserId(objVasInquiry);
        }
        public VasInquiry GetVasEntryTempGridDtl(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasEntryTempGridDtl(objVasInquiry);
        }
        public VasInquiry GetVasEntry(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasEntry(objVasInquiry);
        }
        public VasInquiry GetVasEntryDtl(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasEntryDtl(objVasInquiry);
        }
        public VasInquiry GetVasIdDetail(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasIdDetail(objVasInquiry);
        }
        public VasInquiry GetWhsIdDetail(VasInquiry objVasInquiry)
        {
            return objRepository.GetWhsIdDetail(objVasInquiry);
        }
        public VasInquiry GetVasPostDetails(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasPostDetails(objVasInquiry);
        }
        public VasInquiry GetVashdr(VasInquiry objVasInquiry)
        {
            return objRepository.GetVashdr(objVasInquiry);
        }
        public VasInquiry GetVasdtl(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasdtl(objVasInquiry);
        }
        public VasInquiry GetVasInquiryDetailsRpt(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasInquiryDetailsRpt(objVasInquiry);
        }
        public VasInquiry GetReUpdateTempVasEntryDtl(VasInquiry objVasInquiry)    //CR - 3PL-MVC-VAS-20180505 Added by Soniya
        {
            return objRepository.GetReUpdateTempVasEntryDtl(objVasInquiry);
        }
        public VasInquiry GetVasInquiryPostRptCtnValues(VasInquiry objVasInquiry)
        {
            return objRepository.GetVasInquiryPostRptCtnValues(objVasInquiry);
        }
        public VasInquiry GetQtyCount(VasInquiry objVasInquiry)
        {
            return objRepository.GetQtyCount(objVasInquiry);
        }
        public DataTable GetVasInquiryDetailsRptExcel(string p_str_cmp_id, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status)
        {
            return objRepository.GetVasInquiryDetailsRptExcel(p_str_cmp_id, p_str_vas_id_fm, p_str_vas_id_to, p_str_vas_date_fm, p_str_vas_date_to, p_str_so_num, p_str_Status);
        }
        public DataTable GetVasPostDetailsExcel(string p_str_cmp_id, string SelectedID)
        {
            return objRepository.GetVasPostDetailsExcel(p_str_cmp_id, SelectedID);
        }
        public VasInquiry fnGetVasRateIdDetails(string pstrCmpid)
        {
            return objRepository.fnGetVasRateIdDetails(pstrCmpid);
        }
    }
}
