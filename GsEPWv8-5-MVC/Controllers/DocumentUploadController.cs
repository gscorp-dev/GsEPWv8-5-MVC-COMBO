using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_4_MVC.Common;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class DocumentUploadController : Controller
    {
        // GET: InboundDocumentFile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOBDocumentByType(string pstrCmpId, string pstrSONum, string pstrDocType)

        {
            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();
            objDocumentUpload = serviceDocumentUpload.GetOBDocumentByType(pstrCmpId, pstrSONum, pstrDocType);
            try
            {
                string l_str_config_doc_path = string.Empty;
                string l_str_full_file_path = string.Empty;

                l_str_config_doc_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                l_str_full_file_path = Path.Combine((l_str_config_doc_path), objDocumentUpload.LstDocumentUpload[0].file_path, objDocumentUpload.LstDocumentUpload[0].upload_file_name);
                return Json(l_str_full_file_path, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult GetDocumentUpload(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_comments, string p_str_sub_doc_type)
        {
            string l_str_file_name = string.Empty;
            string l_str_file_path = string.Empty;
            string l_str_folder_path = string.Empty;
            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();
            objDocumentUpload.cmp_id = p_str_cmp_id;
            objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
            objDocumentUpload.doc_id = p_str_ib_doc_id;
            objDocumentUpload.upload_by = Session["UserID"].ToString().Trim();
            objDocumentUpload.comments = p_str_comments;
            objDocumentUpload.upload_dt = DateTime.Now;
            objDocumentUpload.doc_sub_type = p_str_sub_doc_type;
            l_str_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            l_str_folder_path = Path.Combine((l_str_file_path), p_str_cmp_id);
            if (!Directory.Exists(l_str_folder_path))
            {
                Directory.CreateDirectory(Path.Combine(l_str_folder_path));
            }
            l_str_folder_path = Path.Combine(l_str_folder_path, Session["sess_str_doc_type"].ToString());
            if (!Directory.Exists(l_str_folder_path))
            {
                Directory.CreateDirectory(Path.Combine(l_str_folder_path));
            }
            l_str_folder_path = Path.Combine(l_str_folder_path, p_str_ib_doc_id);
            objDocumentUpload.file_path = l_str_folder_path;

            DirectoryInfo dir = new DirectoryInfo(l_str_folder_path);
            foreach (FileInfo file_info in dir.GetFiles())
            {
                l_str_file_name = file_info.Name;
                objDocumentUpload.orig_file_name = l_str_file_name;
                l_str_file_path = Path.Combine(l_str_folder_path, l_str_file_name);
                if (!Directory.Exists(l_str_file_path))
                {
                    objDocumentUpload.upload_file_name = p_str_cmp_id + "-" + p_str_ib_doc_id + "-" + l_str_file_name;
                    serviceDocumentUpload.SaveDocumentUpload(objDocumentUpload);
                }
            }
            objDocumentUpload = serviceDocumentUpload.GetDocumntUpload(objDocumentUpload);
            Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
            DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
            return PartialView("_DocumentUploadGrid", objDocumentUploadModel);
        }
        public ActionResult GetIBDocumentUploadFiles(string p_str_cmp_id, string p_str_ib_doc_id)
        {
            string str_err_msg = string.Empty;
            string jsonErrorCode;
            try
            {
                DocumentUpload objDocumentUpload = new DocumentUpload();
                DocumentUploadService serviceDocumentUpload = new DocumentUploadService();
                objDocumentUpload.cmp_id = p_str_cmp_id;
                objDocumentUpload.doc_id = p_str_ib_doc_id;
                objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString(); 
                objDocumentUpload = serviceDocumentUpload.GetDocumntUpload(objDocumentUpload);
                Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
                DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
                return PartialView("_InboundInquiryImportFile", objDocumentUploadModel);
            }
            catch (Exception Ex)
            {
                str_err_msg = Ex.Message;
                jsonErrorCode = "-2";
                return Json(new { result = jsonErrorCode, err = str_err_msg }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetVASDocumentUploadFiles(string p_str_cmp_id, string p_str_vas_doc_id)
        {
            string str_err_msg = string.Empty;
            string jsonErrorCode;
            try
            {
                DocumentUpload objDocumentUpload = new DocumentUpload();
                DocumentUploadService serviceDocumentUpload = new DocumentUploadService();
                objDocumentUpload.cmp_id = p_str_cmp_id;
                objDocumentUpload.doc_id = p_str_vas_doc_id;
                objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
                objDocumentUpload = serviceDocumentUpload.GetDocumntUpload(objDocumentUpload);
                Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
                DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
                return PartialView("_VASInquiryImportFile", objDocumentUploadModel);
            }
            catch (Exception Ex)
            {
                str_err_msg = Ex.Message;
                jsonErrorCode = "-2";
                return Json(new { result = jsonErrorCode, err = str_err_msg }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult EmailDocuments(string p_str_cmp_id, string p_str_doc_id, string p_str_cntr_id, string p_str_cust_name, string p_str_cust_po_num)
        {
            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();

            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            string l_str_email_message = string.Empty;
            objDocumentUpload.cmp_id = p_str_cmp_id;
            objDocumentUpload.doc_id = p_str_doc_id;

            objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
            objDocumentUpload = serviceDocumentUpload.GetDocumntUpload(objDocumentUpload);
            int l_int_file_cont = 0;
            l_int_file_cont = objDocumentUpload.LstDocumentUpload.Count;
            String[] str_ary_file_list = new String[l_int_file_cont];
            string l_str_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();

          
            for (int i = 0; i < l_int_file_cont; i++)
            {
                str_ary_file_list[i] = Path.Combine(l_str_file_path, objDocumentUpload.LstDocumentUpload[i].file_path, objDocumentUpload.LstDocumentUpload[i].upload_file_name);
            }
            Session["str_ary_file_list"] = str_ary_file_list;

            
            l_str_email_message = "Hi All ," + "\n" + "Please find the attached " + Session["sess_str_doc_type"].ToString() + " Document(s)." + "\n" + "\n" + "CmpId: " + " " + p_str_cmp_id + "\n" + "Doc Id: " + " " + p_str_doc_id;

            if (Session["sess_str_doc_type"].ToString().ToUpper() == "IB")
            {
                l_str_email_message = l_str_email_message + "\n" + "Container#" + p_str_cntr_id;
            }
            else if (Session["sess_str_doc_type"].ToString().ToUpper() == "OB")
            {
                l_str_email_message = l_str_email_message + "\n" + "Customer Name#" + p_str_cust_name;
                l_str_email_message = l_str_email_message + "\n" + "Customer PO#" + p_str_cust_po_num;
            }


            EmailAlertHdr objEmailAlertHdr = new EmailAlertHdr();
            clsRptEmail objRptEmail = new clsRptEmail();
            bool lblnRptEmailExists = false;
            if (Session["sess_str_doc_type"].ToString() =="IB")
            {
                objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmp_id, "INBOUND", "IB-SEND-DOC", ref lblnRptEmailExists);
            }
            else
            {
                objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmp_id, "OUTBOUND", "OB-SEND-DOC", ref lblnRptEmailExists);
            }
           
            objEmailAlertHdr.emailMessage = l_str_email_message;

            objEmailAlertHdr.emailMessage = objEmailAlertHdr.emailMessage + "\n" + objEmailAlertHdr.emailFooter + "\n";
     
            objEmailAlertHdr.emailSubject = p_str_cmp_id + "-" + p_str_doc_id + " - " + Session["sess_str_doc_type"].ToString() + " Documents";

            objEmail = objEmailService.GetSendMailDetails(objEmail);
            if (objEmail.ListEamilDetail.Count != 0)
            {

                objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();

            }
            else
            {

                objEmail.EmailTo = "";
                objEmail.EmailCC = "";
            }
            EmailAlert objEmailAlert = new EmailAlert();
            objEmailAlertHdr.cmpId = p_str_cmp_id;
            objEmailAlert.objEmailAlertHdr = objEmailAlertHdr;

            Mapper.CreateMap<EmailAlert, EmailAlertModel>();
            EmailAlertModel EmailModel = Mapper.Map<EmailAlert, EmailAlertModel>(objEmailAlert);
            return PartialView("_EmailAlert", EmailModel);

        }

        public ActionResult EmailAllDocuments(string p_str_cmp_id, string p_str_doc_id, string p_str_doc_type)
        {

            int l_int_TotCtn = 0;
            decimal l_dec_Totwgt = 0;
            decimal l_dec_Totcube = 0;
            string strReportName = string.Empty;
            string l_str_tmp_name = string.Empty;
        
            string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService objCustMasterService = new CustMasterService();
            string strFileName = string.Empty;
            string Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            string reportFileName = string.Empty;
            string l_str_rptdtl = string.Empty;
            l_str_rptdtl = p_str_cmp_id + "-" + p_str_doc_id;

            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }



            AttachDocList objAttachDocList = new AttachDocList();
            IList<InboundInquiry> rptSource = null;
            IList<OBGetBOLConf> OBrptSource = null;

            if (p_str_doc_type == "IB")
            {

            
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();

            objInboundInquiry.cmp_id = p_str_cmp_id;
            objInboundInquiry.ib_doc_id = p_str_doc_id;
      
                objInboundInquiry = ServiceObject.GET_IB_RCVD_DOC_CUBE_AND_WGT(objInboundInquiry);
                if (objInboundInquiry.ListTotalCount.Count > 0)
                {
                    l_int_TotCtn = objInboundInquiry.ListTotalCount[0].TOT_CARTON;
                    l_dec_Totwgt = objInboundInquiry.ListTotalCount[0].TOT_WEIGHT;
                    l_dec_Totcube = objInboundInquiry.ListTotalCount[0].TOTCUBE;
                }
                //end  

                objInboundInquiry = ServiceObject.GetTotal(objInboundInquiry);
                objInboundInquiry.tot_ctn = objInboundInquiry.ListTotalCount[0].tot_ctn;
                objInboundInquiry.tot_cube = objInboundInquiry.ListTotalCount[0].tot_cube;
                objInboundInquiry = ServiceObject.GetInboundConfirmationRptDetails(objInboundInquiry);
             
                rptSource = objInboundInquiry.ListConfirmationRptDetails.ToList();

                if (rptSource.Count > 0)
                {
                    using (ReportDocument rd = new ReportDocument())
                    {
                        strReportName = "rpt_ib_doc_recv_post_confrimation.rpt";
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;
                        rd.Load(strRptPath);
                        rd.SetDataSource(rptSource);
                        rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);

                        rd.SetParameterValue("TotCtn", l_int_TotCtn);
                        rd.SetParameterValue("TotWgt", l_dec_Totwgt);
                        rd.SetParameterValue("TotCube", l_dec_Totcube);

                            objInboundInquiry.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                            rd.SetParameterValue("fml_image_path", objInboundInquiry.Image_Path);

                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "CONF-" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            reportFileName = l_str_rptdtl + "CONF-" + strDateFormat + ".pdf";
                            objAttachDocList.cmp_id = p_str_cmp_id;
                    objAttachDocList.doc_id = p_str_doc_id;
                    objAttachDocList.doc_sub_type = "CONF";
                    objAttachDocList.file_path = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath;
                    objAttachDocList.upload_file_name = reportFileName;
                    objAttachDocList.filePathWithName = Path.Combine(objAttachDocList.file_path, reportFileName);

                }
                }

            }

           else
            {

                strReportName = "rpt_ob_ship_bol_conf.rpt";
                OBGetBOLConf objOBGetBOLConf = new OBGetBOLConf();
                OutboundInqService ServiceObject = new OutboundInqService();

                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
                objOBGetBOLConf.cmp_id = p_str_cmp_id;
                objOBGetBOLConf = ServiceObject.GetOBBOLConfDtlRptData(objOBGetBOLConf, p_str_cmp_id, p_str_doc_id, string.Empty, string.Empty, string.Empty, string.Empty);


                if (objOBGetBOLConf.ListOBBOLConfRpt.Count == 0)
                {

                    return Json(new { result = "-2", err = "No Record Found" }, JsonRequestBehavior.AllowGet);
                }
                OBrptSource = objOBGetBOLConf.ListOBBOLConfRpt.ToList();
                    if (OBrptSource.Count > 0)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);

                            rd.SetDataSource(OBrptSource);
                            rd.SetParameterValue("fml_image_path", System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim());
                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rptdtl + "CONF-" + strDateFormat + ".pdf";
                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        reportFileName = l_str_rptdtl + "CONF-" + strDateFormat + ".pdf";
                        objAttachDocList.cmp_id = p_str_cmp_id;
                        objAttachDocList.doc_id = p_str_doc_id;
                        objAttachDocList.doc_sub_type = "CONF";
                        objAttachDocList.file_path = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath;
                        objAttachDocList.upload_file_name = reportFileName;
                        objAttachDocList.filePathWithName = Path.Combine(objAttachDocList.file_path, reportFileName);


                    }
                    }
            
            }


            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();

            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();

            EmailAlertHdr objEmailAlertHdr = new EmailAlertHdr();
            clsRptEmail objRptEmail = new clsRptEmail();

            string l_str_email_message = string.Empty;
            objDocumentUpload.cmp_id = p_str_cmp_id;
            objDocumentUpload.doc_id = p_str_doc_id;

            objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();

            objEmailAlertHdr.LstAttachDocs = serviceDocumentUpload.fnGetAttachments(p_str_cmp_id,p_str_doc_id, p_str_doc_type).LstAttachDocs;


            int l_int_file_cont = 0;
            l_int_file_cont = objEmailAlertHdr.LstAttachDocs.Count;
            string l_str_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            for (int i = 0; i < l_int_file_cont; i++)
            {
                objEmailAlertHdr.LstAttachDocs[i].filePathWithName = Path.Combine(l_str_file_path, objEmailAlertHdr.LstAttachDocs[i].file_path, objEmailAlertHdr.LstAttachDocs[i].upload_file_name);
                if (objEmailAlertHdr.LstAttachDocs[i].doc_sub_type == null || objEmailAlertHdr.LstAttachDocs[i].doc_sub_type ==string.Empty)
                {
                    objEmailAlertHdr.LstAttachDocs[i].doc_sub_type = "GEN";
                }
             
            }


            if (p_str_doc_type == "IB")
            {
                if (rptSource != null && rptSource.Count > 0)
                {
                    objEmailAlertHdr.LstAttachDocs.Add(objAttachDocList);
                }
            }

            if (p_str_doc_type == "OB")
            {
                if (OBrptSource != null && OBrptSource.Count > 0)
                {
                    objEmailAlertHdr.LstAttachDocs.Add(objAttachDocList);
                }

            }


            l_str_email_message = "Hi All ," + "\n" + "Please find the attached " + Session["sess_str_doc_type"].ToString() + " Document(s)." + "\n" + "\n" + "CmpId: " + " " + p_str_cmp_id + "\n" + "Doc Id: " + " " + p_str_doc_id;

         
            bool lblnRptEmailExists = false;
            if (p_str_doc_type == "IB")
            {
                objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmp_id, "INBOUND", "IB-SEND-DOC", ref lblnRptEmailExists);
            }
            else
            {
                objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmp_id, "OUTBOUND", "OB-940-ACK", ref lblnRptEmailExists);
            }
           
            objEmailAlertHdr.emailMessage = l_str_email_message;

            objEmailAlertHdr.emailMessage = objEmailAlertHdr.emailMessage + "\n" + objEmailAlertHdr.emailFooter + "\n";

            objEmailAlertHdr.emailSubject = p_str_cmp_id + "-" + p_str_doc_id + " - " + Session["sess_str_doc_type"].ToString() + " Documents";

            objEmail = objEmailService.GetSendMailDetails(objEmail);
            if (objEmail.ListEamilDetail.Count != 0)
            {

                objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();

            }
            else
            {

                objEmail.EmailTo = "";
                objEmail.EmailCC = "";
            }


            EmailAlert objEmailAlert = new EmailAlert();
            objEmailAlertHdr.cmpId = p_str_cmp_id;
            objEmailAlert.objEmailAlertHdr = objEmailAlertHdr;

            Mapper.CreateMap<EmailAlert, EmailAlertModel>();
            EmailAlertModel EmailModel = Mapper.Map<EmailAlert, EmailAlertModel>(objEmailAlert);
            return PartialView("_EmailAlertWithFiles", EmailModel);

        }



        public ActionResult LoadInboundDocuments(string p_str_cmp_id, string p_str_doc_id)
        {
            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();

            objDocumentUpload.cmp_id = p_str_cmp_id;
            objDocumentUpload.doc_id = p_str_doc_id;
            objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
            objDocumentUpload = serviceDocumentUpload.GetDocumntUpload(objDocumentUpload);
            Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
            DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
            return PartialView("_InboundInquiryImportFile", objDocumentUploadModel);


        }

        public ActionResult LoadOutboundDocuments(string p_str_cmp_id, string p_str_doc_id, String p_str_doc_sub_type, int p_int_doc_count)
        {
            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();

            objDocumentUpload.cmp_id = p_str_cmp_id;
            objDocumentUpload.doc_id = p_str_doc_id;
            objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
            if (p_str_doc_sub_type == "ALL")
            {
                objDocumentUpload.doc_sub_type = string.Empty;
            }
              
            else
            { 
                objDocumentUpload.doc_sub_type = p_str_doc_sub_type;
            }

            LookUp objLookUp = new LookUp();
            LookUpService ServiceObjects = new LookUpService();
            objLookUp.id = "303";
            objLookUp.lookuptype = "DOC-TYPE";
            objLookUp = ServiceObjects.GetLookUpValue(objLookUp);
            objDocumentUpload.ListDocumentsSubType = objLookUp.ListLookUpDtl;

            objDocumentUpload = serviceDocumentUpload.GetDocumntUpload(objDocumentUpload);
            Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
            DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
            return PartialView("_OutboundInquiryImportFile", objDocumentUploadModel);


        }

        public ActionResult LoadVASDocuments(string p_str_cmp_id, string p_str_doc_id)
        {
            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();
            Session["sess_str_doc_type"] = "VAS";
            objDocumentUpload.cmp_id = p_str_cmp_id;
            objDocumentUpload.doc_id = p_str_doc_id;
            objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
            objDocumentUpload = serviceDocumentUpload.GetDocumntUpload(objDocumentUpload);
            Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
            DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
            return PartialView("_VASInquiryImportFile", objDocumentUploadModel);


        }



        public ActionResult GetDocumentFullFileName(string cmpid, string path)

        {
            try
            {
                string l_str_config_doc_path = string.Empty;
                string l_str_full_file_path = string.Empty;
                l_str_config_doc_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                l_str_full_file_path = Path.Combine((l_str_config_doc_path), path);
                return Json(l_str_full_file_path, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }


        }

        public FileStreamResult ViewDocument(string path, string ext, string filename)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (ext == "pdf" || ext == "PDF")
            {
                return File(fs, "application/pdf");
            }
            if (ext == "doc" || ext == "DOC")
            {
                return File(fs, "application/doc", filename);
            }
            if (ext == "xls" || ext == "XLS")
            {
                return File(fs, "application/xls", filename);
            }
            if (ext == "xlsx" || ext == "XLSX")
            {
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            if (ext == "docx" || ext == "DOCX")
            {
                return File(fs, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", filename);
            }

            if (ext == "jpg" || ext == "jpeg")
            {
                return File(fs, "image/jpeg", filename);
            }
            return File(fs, "application/csv", filename);

          

        }

        public ActionResult DeleteDocumentVAS(string p_str_cmp_id, string p_str_doc_id, string p_str_file_name, string p_str_file_path)
        {
            string l_str_config_file_path = string.Empty;
            string l_str_folder_full_path = string.Empty;
            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();
            objDocumentUpload.cmp_id = p_str_cmp_id;
            objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
            objDocumentUpload.doc_id = p_str_doc_id;
            objDocumentUpload.upload_file_name = p_str_file_name;

            l_str_config_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            l_str_folder_full_path = Path.Combine(l_str_config_file_path, p_str_file_path, p_str_file_name);

            if (System.IO.File.Exists(l_str_folder_full_path))
            {
                System.IO.File.Delete(l_str_folder_full_path);

            }

            objDocumentUpload = serviceDocumentUpload.DeleteDocumntUpload(objDocumentUpload, "N");
            Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
            DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
            return PartialView("_VASInquiryImportFile", objDocumentUploadModel);
        }


        public ActionResult DeleteDocument(string p_str_cmp_id,  string p_str_doc_id, string p_str_file_name , string p_str_file_path )
        {
            string l_str_config_file_path = string.Empty;
            string l_str_folder_full_path = string.Empty;
            DocumentUpload objDocumentUpload = new DocumentUpload();
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();
            objDocumentUpload.cmp_id = p_str_cmp_id;
            objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
            objDocumentUpload.doc_id = p_str_doc_id;
            objDocumentUpload.upload_file_name = p_str_file_name;

            l_str_config_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            l_str_folder_full_path = Path.Combine(l_str_config_file_path, p_str_file_path, p_str_file_name);

           if( System.IO.File.Exists(l_str_folder_full_path))
            {
                System.IO.File.Delete(l_str_folder_full_path);

            }
        
            objDocumentUpload = serviceDocumentUpload.DeleteDocumntUpload(objDocumentUpload,"N");
            Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
            DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
            return PartialView("_InboundInquiryImportFile", objDocumentUploadModel);
        }


        [HttpPost]
        public ActionResult GridUploadFiles(string p_str_cmp_id, string p_str_doc_id, string p_str_comments, string p_str_sub_doc_type)
        {

            string l_str_file_name = string.Empty;
            string l_str_file_extn = string.Empty;
            string l_str_file_name_only = string.Empty;
            string l_str_file_path = string.Empty;
            string l_str_config_file_path = string.Empty;
            string l_str_folder_path = string.Empty;
            string l_str_folder_full_path = string.Empty;
            string l_str_upload_file = string.Empty;
            DocumentUpload objDocumentUpload;
            DocumentUploadService serviceDocumentUpload = new DocumentUploadService();
            string l_str_sub_folder = string.Empty;

            Session["CompanyID"] = p_str_cmp_id;

            Session["Lstp_str_ib_doc_id"] = p_str_doc_id;
            Session["AssignValue"] = "InsertedValue";
            // Checking no of files injected in Request object 
            l_str_config_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase fl_selected_files = Request.Files;
                    for (int i = 0; i < fl_selected_files.Count; i++)
                    {
                        HttpPostedFileBase l_fl_posted_files = fl_selected_files[i];
                        //check we have a file
                        if (l_fl_posted_files != null)
                        {
                            if (l_fl_posted_files.ContentLength > 0)
                            {
                                l_str_file_name_only = Path.GetFileNameWithoutExtension(l_fl_posted_files.FileName).Replace("#","");
                                l_str_file_name = Path.GetFileName(l_fl_posted_files.FileName);
                                l_str_file_extn = System.IO.Path.GetExtension(l_str_file_name).ToLower();
                                l_str_sub_folder = p_str_doc_id.Substring(0, 3);
                                if (p_str_sub_doc_type == "LABEL")
                                {
                                    l_str_upload_file = string.Format("1-LABEL-" + p_str_cmp_id + "-" + p_str_doc_id + "-" + l_str_file_name_only + l_str_file_extn);
                                }
                                else if (p_str_sub_doc_type == "PACKING SLIP")
                                {
                                    l_str_upload_file = string.Format("2-PACK-" + p_str_cmp_id + "-" + p_str_doc_id + "-" + l_str_file_name_only + l_str_file_extn);
                                }
                                else if (p_str_sub_doc_type == "PICK SLIP")
                                {
                                    l_str_upload_file = string.Format("3-PICK-" + p_str_cmp_id + "-" + p_str_doc_id + "-" + l_str_file_name_only + l_str_file_extn);
                                }
                                else if (p_str_sub_doc_type == "POD")
                                {
                                    l_str_upload_file = string.Format("4-POD-" + p_str_cmp_id + "-" + p_str_doc_id + "-" + l_str_file_name_only + l_str_file_extn);
                                }

                                else if (p_str_sub_doc_type == "GENERAL")
                                {
                                    l_str_upload_file = string.Format("5-GEN-" + p_str_cmp_id + "-" + p_str_doc_id + "-" + l_str_file_name_only + l_str_file_extn);
                                }
                                else 
                                {
                                    l_str_upload_file = string.Format("6-OTH-" + p_str_cmp_id + "-" + p_str_doc_id + "-" + l_str_file_name_only + l_str_file_extn);
                                }

                              //  l_str_upload_file = string.Format(p_str_cmp_id + "-" + p_str_doc_id + "-" + l_str_file_name_only + l_str_file_extn);
                                //l_str_folder_path = p_str_cmp_id.Trim() + "\\" + Session["sess_str_doc_type"].ToString() + "\\" + p_str_doc_id.Trim() + "\\" + l_str_sub_folder;
                                l_str_folder_path = p_str_cmp_id.Trim() + "\\" + Session["sess_str_doc_type"].ToString() + "\\" + l_str_sub_folder + "\\" + p_str_doc_id.Trim();

                                // For Getting file Extension
                                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                                l_str_folder_full_path = Path.Combine((l_str_config_file_path), l_str_folder_path);
                                if (!Directory.Exists(l_str_folder_full_path))
                                {
                                    Directory.CreateDirectory(Path.Combine(l_str_folder_full_path));
                                }


                             

                                l_str_file_path = Path.Combine(l_str_folder_full_path, l_str_upload_file);
                                l_fl_posted_files.SaveAs(l_str_file_path);

                                objDocumentUpload = new DocumentUpload();
                                objDocumentUpload.doc_id = p_str_doc_id;
                                objDocumentUpload.cmp_id = p_str_cmp_id;
                                objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
                                objDocumentUpload.orig_file_name = l_str_file_name;
                                objDocumentUpload.upload_file_name = l_str_upload_file;
                                objDocumentUpload.file_path = l_str_folder_path;
                                objDocumentUpload.upload_by = Session["UserID"].ToString().Trim();
                                objDocumentUpload.comments = p_str_comments;
                                objDocumentUpload.upload_dt = DateTime.Now;
                                objDocumentUpload.doc_sub_type = p_str_sub_doc_type;
                                serviceDocumentUpload.SaveDocumentUpload(objDocumentUpload);
                            }
                        }
                        else
                        {
                            //Catch errors
                            ViewData["Feedback"] = "Please select a file";
                        }

                    }


                    objDocumentUpload = new DocumentUpload();
                    objDocumentUpload.doc_id = p_str_doc_id;
                    objDocumentUpload.cmp_id = p_str_cmp_id;
                    objDocumentUpload.doc_type = Session["sess_str_doc_type"].ToString();
         

                    objDocumentUpload = serviceDocumentUpload.GetDocumntUpload(objDocumentUpload);

                    Mapper.CreateMap<DocumentUpload, DocumentUploadModel>();
                    DocumentUploadModel objDocumentUploadModel = Mapper.Map<DocumentUpload, DocumentUploadModel>(objDocumentUpload);
                    string strDocType = Session["sess_str_doc_type"].ToString();
                    if (strDocType == "VAS")
                    {
                        return PartialView("_DocumentUploadGridVAS", objDocumentUploadModel);
                    }
                    else
                    { 
                    return PartialView("_DocumentUploadGrid", objDocumentUploadModel);
                    }
                    //return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

    }
}