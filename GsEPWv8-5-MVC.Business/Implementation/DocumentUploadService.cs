using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Business.Implementation
{
    public class DocumentUploadService : IDocumentUploadService
            {
        DocumentUploadRepository objDocUploadRepository = new DocumentUploadRepository();

      public DocumentUpload GetOBDocumentByType(string pstrCmpId, string pstrSoNum, string pstrDocSubType)
        {
            return objDocUploadRepository.GetOBDocumentByType(pstrCmpId, pstrSoNum, pstrDocSubType);
        }

        public EmailAlertHdr fnGetAttachments(string pstrCmpId, string pstrDocId, string pstrDocType)
        {
            return objDocUploadRepository.fnGetAttachments(pstrCmpId, pstrDocId, pstrDocType);
        }

        public  DocumentUpload GetDocumntUpload(DocumentUpload objDocumentUpload)
        {
            return objDocUploadRepository.GetDocumntUpload(objDocumentUpload);
        }

      public  DocumentUpload DeleteDocumntUpload(DocumentUpload objDocumentUpload, string p_str_del_all)
        {
            return objDocUploadRepository.DeleteDocumntUpload( objDocumentUpload,  p_str_del_all);
        }

        public void SaveDocumentUpload(DocumentUpload objDocumentUpload)
        {
            objDocUploadRepository.SaveDocumentUpload(objDocumentUpload);
        }

    }
}
