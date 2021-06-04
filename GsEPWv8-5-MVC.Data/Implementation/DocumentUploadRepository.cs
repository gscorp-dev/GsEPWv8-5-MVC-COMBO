using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class DocumentUploadRepository : IDocumentUploadRepository
    {
        public DocumentUpload GetDocumntUpload(DocumentUpload objDocumentUpload)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string str_sp_name = "sp_3pl_get_doc_upld_dtl";
                    IList<DocumentUpload> LstDocUpload = connection.Query<DocumentUpload>(str_sp_name, new
                    {
                        @P_STR_CMP_ID = objDocumentUpload.cmp_id,
                        @P_STR_DOC_TYPE = objDocumentUpload.doc_type,
                        @P_STR_DOC_ID = objDocumentUpload.doc_id,
                        @P_STR_UPLOAD_FILE_NAME = objDocumentUpload.upload_file_name,
                        @P_STR_DOC_SUB_TYPE = objDocumentUpload.doc_sub_type
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objDocumentUpload.LstDocumentUpload = LstDocUpload.ToList();
                }
                return objDocumentUpload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public EmailAlertHdr fnGetAttachments( string pstrCmpId, string pstrDocId, string pstrDocType)
        {
            EmailAlertHdr objDocumentUpload = new EmailAlertHdr();
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string str_sp_name = "spGetAttachments";
                    IList<AttachDocList> LstDocUpload = connection.Query<AttachDocList>(str_sp_name, new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrDocId = pstrDocId,
                        @pstrDocType = pstrDocType,
                   
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objDocumentUpload.LstAttachDocs = LstDocUpload.ToList();
                }
                return objDocumentUpload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


        public DocumentUpload GetOBDocumentByType(string pstrCmpId, string pstrSoNum, string pstrDocSubType)
        {
            DocumentUpload objDocumentUpload = new DocumentUpload();
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string str_sp_name = "spGetOBDocumentByType";
                    IList<DocumentUpload> LstDocUpload = connection.Query<DocumentUpload>(str_sp_name, new
                    {
                        @pstrCmpId = pstrCmpId,
                        @pstrSoNum = pstrSoNum,
                        @pstrDocSubType = pstrDocSubType,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objDocumentUpload.LstDocumentUpload = LstDocUpload.ToList();
                }
                return objDocumentUpload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public  void SaveDocumentUpload(DocumentUpload objDocumentUpload)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "sp_3pl_add_doc_upld_dtl";
                    connection.Execute(storedProcedure1,
                        new
                        {
                            @P_STR_CMP_ID = objDocumentUpload.cmp_id,
                            @P_STR_DOC_TYPE = objDocumentUpload.doc_type,
                            @P_STR_DOC_ID = objDocumentUpload.doc_id,
                            P_STR_CNTR_ID = objDocumentUpload.cntr_id,
                            @P_STR_ORIG_FILE_NAME = objDocumentUpload.orig_file_name,
                            @P_STR_UPLOAD_FILE_NAME = objDocumentUpload.upload_file_name,
                            @P_STR_FILE_PATH = objDocumentUpload.file_path,
                            @P_STR_UPLOAD_DT = objDocumentUpload.upload_dt,
                            @P_STR_UPLOAD_BY = objDocumentUpload.upload_by,
                            @P_STR_COMMENTS = objDocumentUpload.comments,
                            @P_STR_SUB_DOC_TYPE = objDocumentUpload.doc_sub_type

                        }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DocumentUpload DeleteDocumntUpload(DocumentUpload objDocumentUpload, string p_str_del_all)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string str_sp_name = "sp_3pl_del_doc_upld_dtl";
                    IList<DocumentUpload> LstDocUpload = connection.Query<DocumentUpload>(str_sp_name, new
                    {
                        @P_STR_CMP_ID = objDocumentUpload.cmp_id,
                        @P_STR_DOC_TYPE = objDocumentUpload.doc_type,
                        @P_STR_DOC_ID = objDocumentUpload.doc_id,
                        @P_STR_UPLOAD_FILE_NAME = objDocumentUpload.upload_file_name,
                        @P_STR_DEL_ALL = p_str_del_all,
                    }, commandType: CommandType.StoredProcedure).ToList();
                    objDocumentUpload.LstDocumentUpload = LstDocUpload.ToList();
                }
                return objDocumentUpload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }



    }
}
