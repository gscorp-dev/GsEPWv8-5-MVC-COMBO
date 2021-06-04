using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Data.Implementation;
using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace GsEPWv8_5_MVC.Business.Implementation
{
   public class OB940ChubUploadFileService : IOB940ChubUploadFileService
    {
        OB940UploadFileRepository objRepository = new OB940UploadFileRepository();
        public bool GenerateShipRequest(string p_str_upload_ref_num)
        {
            return true;
        }

        public bool Check940UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            return objRepository.Check940UploadFileExists(p_str_cmp_id,  p_str_file_name);
        }

        public bool CheckRefNumExists(string p_str_cmp_id, string p_str_ref_num)
        {
            return objRepository.CheckRefNumExists(p_str_cmp_id, p_str_ref_num);
        }

        public int Get940UploadRefNum(string p_str_cmp_id)
        {
            return objRepository.Get940UploadRefNum(p_str_cmp_id);
        }

        public string Get940BatchId(string p_str_cmp_id)
        {
            return objRepository.Get940BatchId(p_str_cmp_id);
        }


        public string SaveOB940UploadFile(string p_str_cmp_id, DataTable p_dt_ob_940_upload_file_info, DataTable p_dt_ob_940_upload_file_hdr, DataTable p_dt_ob_940_upload_file_dtl)
        {
            return objRepository.SaveOB940UploadFile(p_str_cmp_id, p_dt_ob_940_upload_file_info, p_dt_ob_940_upload_file_hdr, p_dt_ob_940_upload_file_dtl);
        }

        public bool fnGenerate943ForTransCmpId(string pstrCmpId, string pstrUploadRefNum, string pstrTransferCmpId)
        {
            return objRepository.fnGenerate943ForTransCmpId(pstrCmpId, pstrUploadRefNum, pstrTransferCmpId);
        }

        public string MoveOB940UploadToSOTables(string p_str_cmp_id, string p_str_upload_ref_num)

        {
            return objRepository.MoveOB940UploadToSOTables(p_str_cmp_id, p_str_upload_ref_num);

        }

     public   OB940UploadFile GetOB940UploadDtlRptData(OB940UploadFile objOB940UploadFile, string p_str_upload_ref_num, string p_str_so_num)
        {
            return objRepository.GetOB940UploadDtlRptData(objOB940UploadFile, p_str_upload_ref_num,  p_str_so_num);
        }
        public DataTable GetOB940UploadDtlRptDataExcel(string l_str_cmp_id, string l_str_File_name, string l_str_batch_no, string l_str_UploaddtFm, string l_str_UploaddtTo, string l_str_process_id, string l_str_so_num)
        {
            return objRepository.GetOB940UploadDtlRptDataExcel(l_str_cmp_id, l_str_File_name, l_str_batch_no, l_str_UploaddtFm, l_str_UploaddtTo, l_str_process_id, l_str_so_num);
        }
    }
}

