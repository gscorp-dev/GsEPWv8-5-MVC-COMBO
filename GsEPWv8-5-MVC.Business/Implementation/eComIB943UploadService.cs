using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_4_MVC.Business.Implementation
{
    public class eComIB943UploadService: IeComIB943UploadService
    {
        eComIB943UploadRepository objeComIBCntrUploadRepository = new eComIB943UploadRepository();

        public bool Check943UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            return objeComIBCntrUploadRepository.Check943UploadFileExists(p_str_cmp_id, p_str_file_name);
        }


        public int Get943UploadRefNum(string p_str_cmp_id)
        {
            return objeComIBCntrUploadRepository.Get943UploadRefNum(p_str_cmp_id);
        }

        public void InsertIB943UploadTempDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
             objeComIBCntrUploadRepository.InsertIB943UploadTempDtltblDetails(objeComIB943Upload);
        }
        public void InsertIB943UploadDocHdrtblDetails(eComIB943Upload objeComIB943Upload)
        {
             objeComIBCntrUploadRepository.InsertIB943UploadDocHdrtblDetails(objeComIB943Upload);
        }
        public void InsertIB943UploadDocDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
             objeComIBCntrUploadRepository.InsertIB943UploadDocDtltblDetails(objeComIB943Upload);
        }
        public void InsertDetailstoCtntable(eComIB943Upload objeComIB943Upload)
        {
            objeComIBCntrUploadRepository.InsertDetailstoCtntable(objeComIB943Upload);
        }
        public void InsertDetailstoAuditTrail(eComIB943Upload objeComIB943Upload)
        {
            objeComIBCntrUploadRepository.InsertDetailstoAuditTrail(objeComIB943Upload);
        }
       
        public eComIB943Upload GetIB943UploadTempHdrtblDetails(eComIB943Upload objeComIB943Upload)
        {
            return objeComIBCntrUploadRepository.GetIB943UploadTempHdrtblDetails(objeComIB943Upload);
        }
        public eComIB943Upload GetIB943UploadTempDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
            return objeComIBCntrUploadRepository.GetIB943UploadTempDtltblDetails(objeComIB943Upload);
        }
        public eComIB943Upload CheckIBUploadedStyleExist(eComIB943Upload objeComIB943Upload)
        {
            return objeComIBCntrUploadRepository.CheckIBUploadedStyleExist(objeComIB943Upload);
        }
        public eComIB943Upload DeleteTempTable(eComIB943Upload objeComIB943Upload)
        {
            return objeComIBCntrUploadRepository.DeleteTempTable(objeComIB943Upload);
        }
        public eComIB943Upload AddECom943UploadFileNAme(eComIB943Upload objeComIB943Upload)
        {
            return objeComIBCntrUploadRepository.AddECom943UploadFileNAme(objeComIB943Upload);
        }


       
       
        public void UpdateTblIbDocDtl(eComIB943Upload objeComIB943Upload)
        {
            objeComIBCntrUploadRepository.UpdateTblIbDocDtl(objeComIB943Upload);
        }
        public void InsertIB943SingleLineUploadTempDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
            objeComIBCntrUploadRepository.InsertIB943SingleLineUploadTempDtltblDetails(objeComIB943Upload);
        }
        public eComIB943Upload GetIB943SingleLineUploadTempDtltblDetails(eComIB943Upload objeComIB943Upload)
        {
            return objeComIBCntrUploadRepository.GetIB943SingleLineUploadTempDtltblDetails(objeComIB943Upload);
        }
        public void DeleteSingleLineCntrUploadTempTable(eComIB943Upload eComIB943Upload)
        {
          objeComIBCntrUploadRepository.DeleteSingleLineCntrUploadTempTable(eComIB943Upload);
        }

        public void InsertIB943UploadTempHdrtblDetails(eComIB943Upload objeComIB943Upload)
        {
            objeComIBCntrUploadRepository.InsertIB943UploadTempHdrtblDetails(objeComIB943Upload);
        }
    }
}
