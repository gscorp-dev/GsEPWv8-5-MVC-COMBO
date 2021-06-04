using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GsEPWv8_5_MVC.Core.Entity;
namespace GsEPWv8_5_MVC.Business.Interface
{
    public interface IOB940UploadFileService
    {
        DataTable fnGetOBDocListBy940(string pstrCmpId, string pstrUploadRefNo);
        DataTable fnGetCustConfig(string p_str_cmp_id);
        bool GenerateShipRequest(string p_str_upload_ref_num);
        bool Check940UploadFileExists(string p_str_cmp_id, string p_str_file_name);
        string Get940BatchId(string p_str_cmp_id);
        int Get940UploadRefNum(string p_str_cmp_id);
        bool CheckRefNumExists(string p_str_cmp_id, string p_str_ref_num);
        string SaveOB940UploadFile(string p_str_cmp_id, DataTable p_dt_ob_940_upload_file_info, DataTable p_dt_ob_940_upload_file_hdr, DataTable p_dt_ob_940_upload_file_dtl);
        bool fnGenerate943ForTransCmpId(string pstrCmpId, string pstrUploadRefNum, string pstrTransferCmpId);
        string MoveOB940UploadToSOTables(string p_str_cmp_id, string p_str_upload_ref_num);
        OB940UploadFile GetOB940UploadDtlRptData(OB940UploadFile objOB940UploadFile, string p_str_upload_ref_num, string p_str_so_num);
        DataTable GetOB940UploadDtlRptDataExcel(string l_str_cmp_id, string l_str_File_name, string l_str_batch_no, string l_str_UploaddtFm, string l_str_UploaddtTo, string l_str_process_id, string l_str_so_num);
    }
}
