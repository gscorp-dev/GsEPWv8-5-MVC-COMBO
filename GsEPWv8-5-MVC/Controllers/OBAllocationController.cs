using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Common;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class OBAllocationController : Controller
    {
        public ActionResult OBAllocationEntry(string pCmpId, string pSrNo)
        {

            ClsOBAllocation objOBAllocation = new ClsOBAllocation();
            OBAllocationService oBAllocationService = new OBAllocationService();
            objOBAllocation.cmpId = pCmpId;
            objOBAllocation.srNo = pSrNo;
            objOBAllocation.actionFlag = "A";
            objOBAllocation.lstOBSRAlocDtl = oBAllocationService.getSRAlocDtl(pCmpId, pSrNo);

            Mapper.CreateMap<ClsOBAllocation, OBAllocationModel>();
            OBAllocationModel objOBAllocationModel = Mapper.Map<ClsOBAllocation, OBAllocationModel>(objOBAllocation);
            return PartialView("_OBAlocationEntry", objOBAllocationModel);
        }

        public ActionResult OBAllocationEdit(string pstrCmpId, string pstrAlocDocId,string pstrSrNum, string pstrSrDt)
        {
            ClsOBAlocEdit objOBAlocEdit = new ClsOBAlocEdit();
            OBAllocationService oBAllocationService = new OBAllocationService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBAlocEdit.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objOBAlocEdit.cmp_id = pstrCmpId;
            objOBAlocEdit.aloc_doc_id = pstrAlocDocId;
            objOBAlocEdit.sr_no = pstrSrNum;
            objOBAlocEdit.action_flag = "M";
      

            objOBAlocEdit.lstOBAlocHdr = oBAllocationService.getOBAlocHdr(pstrCmpId, pstrAlocDocId);
            objOBAlocEdit.objOBAlocHdr = objOBAlocEdit.lstOBAlocHdr[0];
            objOBAlocEdit.objOBAlocHdr.so_dt = pstrSrDt;
            objOBAlocEdit.lstOBAlocSmry = oBAllocationService.getOBAlocDtl(pstrCmpId, pstrAlocDocId);
            objOBAlocEdit.lstOBAlocCtn = oBAllocationService.getOBAlocCtn(pstrCmpId, pstrAlocDocId);
     
            Mapper.CreateMap<ClsOBAlocEdit, ClsOBAlocEditModel>();
            ClsOBAlocEditModel objOBAlocEditModel = Mapper.Map<ClsOBAlocEdit, ClsOBAlocEditModel>(objOBAlocEdit);
            return PartialView("_OBAlocationEdit", objOBAlocEditModel);
        }

        public ActionResult getAlocCtnEditByLine(string p_str_cmp_id,string p_str_aloc_doc_id, string p_str_so_no, string p_str_line_num, string p_str_so_line_num, string p_str_itm_code,
            string p_str_itm_num, string p_str_itm_color, string p_str_itm_size, string p_str_due_qty)

        {

            ClsOBAlocEdit objOBAlocEdit = new ClsOBAlocEdit();
            OBAllocationService oBAllocationService = new OBAllocationService();
            clsOBAlocDtl objOBAlocDtl = new clsOBAlocDtl();
            int pstrAlocLine;
            pstrAlocLine = Convert.ToInt16(p_str_line_num);
            objOBAlocDtl.cmp_id = p_str_cmp_id;
            objOBAlocDtl.aloc_doc_id = p_str_aloc_doc_id;
            objOBAlocDtl.so_num = p_str_so_no;
            objOBAlocDtl.line_num = Convert.ToInt16(p_str_line_num);
            objOBAlocDtl.so_itm_code = p_str_itm_code;
            objOBAlocDtl.so_dtl_line = Convert.ToInt16(p_str_so_line_num);
            objOBAlocDtl.so_itm_num = p_str_itm_num;
            objOBAlocDtl.so_itm_color = p_str_itm_color;
            objOBAlocDtl.so_itm_size = p_str_itm_size;
            objOBAlocEdit.cmp_id = p_str_cmp_id;
            objOBAlocEdit.aloc_doc_id = p_str_aloc_doc_id;
            objOBAlocDtl.due_qty = Convert.ToInt32(p_str_due_qty);
            objOBAlocEdit.objBAlocDtl = objOBAlocDtl;
            objOBAlocEdit.lstOBAlocCtnByLine = oBAllocationService.getOBAlocCtnByLine(p_str_cmp_id, p_str_aloc_doc_id, pstrAlocLine);

            Mapper.CreateMap<ClsOBAlocEdit, ClsOBAlocEditModel>();
            ClsOBAlocEditModel objStockInquiryModel = Mapper.Map<ClsOBAlocEdit, ClsOBAlocEditModel>(objOBAlocEdit);
            return PartialView("_alocCtnEditByLine", objStockInquiryModel);

        }

        public JsonResult saveAlocCtnEditByLine(string pstrCmpId, string pstrAlocDocId, string pstrAlocDocDt, string pstrAlocLineNum,List<clsAlocCtnByLoc> lstAlocCtnList)

        {

            ClsOBAlocEdit objOBAlocEdit = new ClsOBAlocEdit();
            OBAllocationService oBAllocationService = new OBAllocationService();
            clsOBAlocDtl objOBAlocDtl = new clsOBAlocDtl();
            DataTable ldtAlocCtnList = new DataTable();
            ldtAlocCtnList =  Utility.ConvertListToDataTable(lstAlocCtnList);
            int pstrAlocLine;
            int lintRefNum = oBAllocationService.getAlocEditTmpRefNum();
            int l_int_row = ldtAlocCtnList.Rows.Count;
            string pstrRefNum = lintRefNum.ToString();
            for (int i = 0; i < l_int_row; i++)
            {
                ldtAlocCtnList.Rows[i]["ref_num"] = pstrRefNum; 
            }

                pstrAlocLine = Convert.ToInt16(pstrAlocLineNum);
            oBAllocationService.saveAlocCtnEditByLine(pstrRefNum, pstrCmpId, pstrAlocDocId, pstrAlocDocDt, pstrAlocLine, ldtAlocCtnList);
        
            objOBAlocDtl.cmp_id = pstrCmpId;
            objOBAlocDtl.aloc_doc_id = pstrAlocDocId;
            objOBAlocDtl.line_num = Convert.ToInt16(pstrAlocLineNum);

            objOBAlocEdit.lstOBAlocCtn = oBAllocationService.getOBAlocCtn(pstrCmpId, pstrAlocDocId);
            return Json("", JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pstrCmpId"></param>
        /// <param name="pstrAlocDocId"></param>
        /// <param name="pstrAlocDocDt"></param>
        /// <param name="pstrSrNum"></param>
        /// <param name="pstrShipDt"></param>
        /// <param name="pstrCancelDt"></param>
        /// <param name="pstrPriceTkt"></param>
        /// <param name="pstrCustId"></param>
        /// <param name="pstrCustName"></param>
        /// <param name="pstrCustOrdrNum"></param>
        /// <param name="pstrCustOrdrDt"></param>
        /// <returns></returns>
        public JsonResult SaveOBAlocEditHdr(string pstrCmpId, string pstrAlocDocId, string pstrAlocDocDt, string pstrSrNum, string pstrShipDt,
            string pstrCancelDt, string pstrPriceTkt, string pstrCustId, string pstrCustName, string pstrCustOrdrNum, string pstrCustOrdrDt,string pstrNote)

        {

            ClsOBAlocEdit objOBAlocEdit = new ClsOBAlocEdit();
            OBAllocationService oBAllocationService = new OBAllocationService();
            clsOBAlocDtl objOBAlocDtl = new clsOBAlocDtl();
           
            oBAllocationService.SaveOBAlocEditHdr(pstrCmpId,  pstrAlocDocId, pstrAlocDocDt, pstrSrNum, pstrShipDt, pstrCancelDt, pstrPriceTkt,
                pstrCustId, pstrCustName, pstrCustOrdrNum, pstrCustOrdrDt, pstrNote);
            return Json(true, JsonRequestBehavior.AllowGet);

        }

  
        public ActionResult getAlocCtnList(string pstrCmpId, string pstrAlocDocId)

        {

            ClsOBAlocEdit objOBAlocEdit = new ClsOBAlocEdit();
            OBAllocationService oBAllocationService = new OBAllocationService();
            clsOBAlocDtl objOBAlocDtl = new clsOBAlocDtl();
            objOBAlocDtl.cmp_id = pstrCmpId;
            objOBAlocDtl.aloc_doc_id = pstrAlocDocId;
            objOBAlocEdit.lstOBAlocCtn = oBAllocationService.getOBAlocCtn(pstrCmpId, pstrAlocDocId);
            Mapper.CreateMap<ClsOBAlocEdit, ClsOBAlocEditModel>();
            ClsOBAlocEditModel objStockInquiryModel = Mapper.Map<ClsOBAlocEdit, ClsOBAlocEditModel>(objOBAlocEdit);
            return PartialView("_alocCtnList", objStockInquiryModel);

        }


        public ActionResult VerifyStockByAlocbatch(string pstrCmpId, string pstrSRNumList)
        {
            string pstrRefNum = string.Empty;
            ClsOBAllocation objOBAllocation = new ClsOBAllocation();
            OBAllocationService oBAllocationService = new OBAllocationService();
            objOBAllocation.cmpId = pstrCmpId;
            objOBAllocation.srNo = pstrSRNumList;
            objOBAllocation.actionFlag = "A";
            objOBAllocation.lstOBSRAlocDtl = oBAllocationService.getSRAlocDtl(pstrCmpId, pstrSRNumList);
            if (objOBAllocation.lstOBSRAlocDtl.Count > 0)
                TempData["obSrAlocRefNum"] = objOBAllocation.lstOBSRAlocDtl[0].ref_num;
                ViewBag.IsBackOrder = "N";
                foreach (ClsOBSRAlocDtl rec in objOBAllocation.lstOBSRAlocDtl)
                {
                    if (rec.bo_qty > 0)
                    {
                        ViewBag.IsBackOrder = "Y";
                        break;
                    }
               
                }
           // objOBAllocation.lstOBSRAlocDtlByLoc = oBAllocationService.getSRAlocDtlByLoc(pstrRefNum, pstrCmpId, pstrSRNumList);

          

            Mapper.CreateMap<ClsOBAllocation, OBAllocationModel>();
            OBAllocationModel objOBAllocationModel = Mapper.Map<ClsOBAllocation, OBAllocationModel>(objOBAllocation);

            return PartialView("_GridSRAlocDtl", objOBAllocationModel);
        }

        public ActionResult getSRAlocDtlByLoc(string pstrCmpId, string pstrSRNumList)
        {
            string pstrRefNum = string.Empty;
            ClsOBAllocation objOBAllocation = new ClsOBAllocation();
            OBAllocationService oBAllocationService = new OBAllocationService();
            objOBAllocation.cmpId = pstrCmpId;
            objOBAllocation.srNo = pstrSRNumList;
            objOBAllocation.actionFlag = "A";
                pstrRefNum = TempData["obSrAlocRefNum"].ToString();
            Session["obSrAlocRefNum"] = pstrRefNum;
            objOBAllocation.lstOBSRAlocDtlByLoc = oBAllocationService.getSRAlocDtlByLoc(pstrRefNum, pstrCmpId, pstrSRNumList);
            Mapper.CreateMap<ClsOBAllocation, OBAllocationModel>();
            OBAllocationModel objOBAllocationModel = Mapper.Map<ClsOBAllocation, OBAllocationModel>(objOBAllocation);
            return PartialView("_GridSRAlocDtlByLoc", objOBAllocationModel);
        }

        public ActionResult SaveAloctionByBatch(string pstrCmpId, string pstrSRNumList)
        {
            string pstrRefNum = string.Empty;
            bool blnSendStatusMail = false;
            ClsOBAllocation objOBAllocation = new ClsOBAllocation();
            OBAllocationService oBAllocationService = new OBAllocationService();
            objOBAllocation.cmpId = pstrCmpId;
            objOBAllocation.srNo = pstrSRNumList;
            objOBAllocation.actionFlag = "A";
            pstrRefNum = Session["obSrAlocRefNum"].ToString();
            oBAllocationService.SaveAloctionByBatch(pstrCmpId, pstrRefNum, pstrSRNumList, Session["UserID"].ToString().Trim());
            if (System.Configuration.ConfigurationManager.AppSettings["CompanyWebLink"].ToString() == "3plpro.gensoftcorp.com")
            {
                blnSendStatusMail = SendOBSRAlocStatusReport(pstrCmpId, pstrRefNum);
                Mapper.CreateMap<ClsOBAllocation, OBAllocationModel>();
                OBAllocationModel objOBAllocationModel = Mapper.Map<ClsOBAllocation, OBAllocationModel>(objOBAllocation);
                return PartialView("_GridSRAlocDtl", objOBAllocationModel);
            }
            else
            {
                return Json("PRINT", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BatchBinListPrint(string pstrCmpId, string pstrSRNumList,string pstrPrintFrom)
        {
            PickLabel objPickLabel = new PickLabel();
            OBAllocationService oBAllocationService = new OBAllocationService();
            string strReportName = string.Empty;
            string strRptPath = string.Empty;
            string strDateFormat = string.Empty;
            string strLabelFileName = string.Empty;
            string strPDFFileName = string.Empty;
            string tempFolderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
            string pstrRefNum = string.Empty;

            string LabelFileName = string.Empty;
            string PdfFileName = string.Empty;

            if (pstrPrintFrom == "BATCH-ALOC")
            {
                pstrRefNum = Session["obSrAlocRefNum"].ToString();
                objPickLabel = oBAllocationService.fnGetOBPickLabel(pstrCmpId, pstrRefNum);
            }
            else
            {
                pstrRefNum = "9999";
                objPickLabel = oBAllocationService.fnGetOBSRListForBinprint(pstrCmpId, pstrSRNumList);
            }

           

            IList<PickLabelDtl> rptSource = objPickLabel.lstPickLabelDtl;
            strReportName = "rpt_iv_batch_bin_picklist.rpt";
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
            if (rptSource.Count > 0)
            {
                using (ReportDocument rd = new ReportDocument())
                {
                    rd.Load(strRptPath);
                    rd.SetDataSource(rptSource);
                   
                    strLabelFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFolderpath + "//" + pstrCmpId + "-" + pstrRefNum + "-LBL-PACKSLIP-" + strDateFormat + ".pdf";
                    LabelFileName = pstrCmpId + "-" + pstrRefNum + "-LBL-PACKSLIP-" + strDateFormat + ".pdf";
                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strLabelFileName);
                }
            }

            strReportName = "rpt_iv_batch_with_bin_by_style.rpt";
            strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Outbound//" + strReportName;
            if (rptSource.Count > 0)
            {
                using (ReportDocument rd = new ReportDocument())
                {
                    rd.Load(strRptPath);

                    rd.SetDataSource(rptSource);
                    rd.SetParameterValue("paramCmpName", System.Configuration.ConfigurationManager.AppSettings["DefaultCompID"].ToString().Trim());
                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                    strPDFFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFolderpath + "//" + pstrCmpId + "-" + pstrRefNum + "-PDFBINLIST-" + strDateFormat + ".pdf";
                    PdfFileName = pstrCmpId + "-" + pstrRefNum + "-PDFBINLIST-" + strDateFormat + ".pdf";
                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strPDFFileName);
                }
            }
            if (pstrPrintFrom  == "BATCH-ALOC")
            {
                objPickLabel = oBAllocationService.fnGetOBDocForPrint(pstrCmpId, pstrRefNum);
            }
            else
            {
                objPickLabel = oBAllocationService.fnGetOBDocBySrList(pstrCmpId, pstrSRNumList);
            }

            string lstrMergedLabelFile = string.Empty;
            string lstrMergedPDFFile = string.Empty;
            int lintLblFile = 0;
            int lintPdfFile = 0;
            string strattachedFileName = string.Empty;
            string lstrMergedLabelFileName = string.Empty;
            string lstrMergedPDFFileName = string.Empty;
            string lstrFilePath = string.Empty;

            //if (objPickLabel.lstDocForPrint.Count > 0)
            //{
                

                lstrMergedLabelFile = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFolderpath + "//" + pstrCmpId + "-" + pstrRefNum + "-LABEL-" + strDateFormat + ".pdf";
                lstrMergedPDFFile = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFolderpath + "//" + pstrCmpId + "-" + pstrRefNum + "-PACKINGSLIP-" + strDateFormat + ".pdf";
                lstrMergedLabelFileName = pstrCmpId + "-" + pstrRefNum + "-LABEL-" + strDateFormat + ".pdf";
                lstrMergedPDFFileName = pstrCmpId + "-" + pstrRefNum + "-PACKINGSLIP-" + strDateFormat + ".pdf";
                lstrFilePath = System.Web.HttpContext.Current.Server.MapPath("~/") + tempFolderpath + "//";
                for (int i = 0; i < objPickLabel.lstDocForPrint.Count; i++)
                {
                    if (objPickLabel.lstDocForPrint[i].upload_file_name.Contains("LABEL"))
                    {
                        lintLblFile = lintLblFile + 1;
                    }
                    else if (objPickLabel.lstDocForPrint[i].upload_file_name.Contains("PACKINGSLIP"))
                    {
                        lintPdfFile = lintPdfFile + 1;
                    }

                }

                string[] lableFileArray = new string[lintLblFile + 1];
                string[] PdfFileArray = new string[lintPdfFile + 1];
                int lintLblCount = 0;
                int lintPdfCount = 0;
                lableFileArray[0] = strLabelFileName;
                PdfFileArray[0] = strPDFFileName;
                lintLblCount = 1;
                lintPdfCount = 1;
                for (int i = 0; i < objPickLabel.lstDocForPrint.Count; i++)
                {
                    if (objPickLabel.lstDocForPrint[i].upload_file_name.Contains("LABEL"))
                    {
                        strattachedFileName = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim(), objPickLabel.lstDocForPrint[i].file_path, objPickLabel.lstDocForPrint[i].upload_file_name);
                        lableFileArray[lintLblCount] = strattachedFileName;
                        lintLblCount = lintLblCount + 1;
                    }

                    else if (objPickLabel.lstDocForPrint[i].upload_file_name.Contains("PACKINGSLIP"))
                    {
                        strattachedFileName = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim(), objPickLabel.lstDocForPrint[i].file_path, objPickLabel.lstDocForPrint[i].upload_file_name);
                        PdfFileArray[lintPdfCount] = strattachedFileName;
                        lintPdfCount = lintPdfCount + 1;
                    }

                }
                if (lintLblCount > 1)
                {
                    MergePDF(lableFileArray, lstrMergedLabelFile);
                }
                else
                {   
                    lstrMergedLabelFileName = LabelFileName;
                }

            if (lintPdfCount > 1)
            {
                MergePDF(PdfFileArray, lstrMergedPDFFile);
            }
            else
            {
                lstrMergedPDFFileName = PdfFileName;
            }
                

               
            //}
            //else
            //{
            //    lstrMergedLabelFileName = strLabelFileName;
            //    lstrMergedPDFFileName = strPDFFileName;

            //}

            PickLabel objPickLabelPrint = new PickLabel();
            objPickLabelPrint.cmp_id = pstrCmpId;
            List<DocForPrint> lstDocForPrint = new List<DocForPrint>
                {
                   new DocForPrint { cmp_id = pstrCmpId ,so_num = "", file_type = "LABEL",file_path =lstrFilePath + lstrMergedLabelFileName, upload_file_name = lstrMergedLabelFileName },
                   new DocForPrint { cmp_id = pstrCmpId ,so_num = "", file_type = "PACKINGSLIP",file_path = lstrFilePath + lstrMergedPDFFileName , upload_file_name= lstrMergedPDFFileName }
                };
            objPickLabelPrint.lstDocForPrint = lstDocForPrint;
            Mapper.CreateMap<PickLabel, PickLabelModel>();
            PickLabelModel objPickLabelModel = Mapper.Map<PickLabel, PickLabelModel>(objPickLabelPrint);
            return PartialView("_OBDocAlocBatchPrint", objPickLabelModel);

        }
        public ActionResult fnGetSrListForLblPackPrint(string pstrCmpId, string pstrBatchNum, string pstrSoNumFrom, string pstrSoNumTo, string pstrSoOrderDtFrom, string pstrSoOrderDtTo)
        {
            PickLabel objPickLabel = new PickLabel();
            OBAllocationService oBAllocationService = new OBAllocationService();
            objPickLabel.cmp_id = pstrCmpId;
            objPickLabel.batch_num = pstrBatchNum;
            objPickLabel.so_num_from = pstrSoNumFrom;
            objPickLabel.so_num_to = pstrSoNumTo;
            objPickLabel.so_dt_from = pstrSoOrderDtFrom;
            objPickLabel.so_dt_to = pstrSoOrderDtTo;
            CompanyService ServiceObjectCompany = new CompanyService();
            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objPickLabel.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objPickLabel.lstPickLabelSmry = oBAllocationService.fnGetOBPickLabelSmry(objPickLabel).lstPickLabelSmry;

            Mapper.CreateMap<PickLabel, PickLabelModel>();
            PickLabelModel objPickLabelModel = Mapper.Map<PickLabel, PickLabelModel>(objPickLabel);
            return PartialView("_BatchLabelPack", objPickLabelModel);

        }
        public ActionResult fnGetSrListForLblPackPrintGrid(string pstrCmpId, string pstrBatchNum, string pstrSoNumFrom, string pstrSoNumTo, string pstrSoOrderDtFrom, string pstrSoOrderDtTo)
        {
            PickLabel objPickLabel = new PickLabel();
            OBAllocationService oBAllocationService = new OBAllocationService();
            objPickLabel.cmp_id = pstrCmpId;
            objPickLabel.batch_num = pstrBatchNum;
            objPickLabel.so_num_from = pstrSoNumFrom;
            objPickLabel.so_num_to = pstrSoNumTo;
            objPickLabel.so_dt_from = pstrSoOrderDtFrom;
            objPickLabel.so_dt_to = pstrSoOrderDtTo;
            CompanyService ServiceObjectCompany = new CompanyService();
            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objPickLabel.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objPickLabel.lstPickLabelSmry = oBAllocationService.fnGetOBPickLabelSmry(objPickLabel).lstPickLabelSmry;

            Mapper.CreateMap<PickLabel, PickLabelModel>();
            PickLabelModel objPickLabelModel = Mapper.Map<PickLabel, PickLabelModel>(objPickLabel);
            return PartialView("_BatchLabelPackGrid", objPickLabelModel);

        }

     
        
        private static void MergePDF(string[] mergeFileArray, string outFileName)
        {
            string[] fileArray = new string[mergeFileArray.Length + 1];
            for (int i = 0; i< mergeFileArray.Length;i ++)
            {
                fileArray[i] = mergeFileArray[i];
            }

            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string outputPdfPath = outFileName;

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int f = 0; f < fileArray.Length - 1; f++)
            {
                int pages = TotalPageCount(fileArray[f]);

                reader = new PdfReader(fileArray[f]);
                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();

        }
        public FileStreamResult ViewDocument(string path, string ext, string filename)
        {
            FileStream fs = new FileStream(path , FileMode.Open, FileAccess.Read);
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
                return File(fs, "image/jpeg");
            }
            return File(fs, "application/csv", filename);



        }
        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }

        public ActionResult OBAllocationByBatch(string pstrCmpId, string pstrBatchNum, string pstrSrNumFrom, string pstrSrNumTo, string pstrSrDtFrom, 
            string pstrSrDtTo,string pstrSrLoadNum, string pstrScreenId)
        {
            ClsOBAllocation objOBAllocation = new ClsOBAllocation();
            clsOBAlocInquiryByBatch objOBAlocInquiryByBatch = new clsOBAlocInquiryByBatch();
            objOBAlocInquiryByBatch.cmpId = pstrCmpId;
            objOBAlocInquiryByBatch.batchNum = pstrBatchNum;
            objOBAlocInquiryByBatch.srNumFrom = pstrSrNumFrom;
            objOBAlocInquiryByBatch.srNumTo = pstrSrNumTo;
            objOBAlocInquiryByBatch.srDtFrom = pstrSrDtFrom;
            objOBAlocInquiryByBatch.srDtTo = pstrSrDtTo;
            objOBAlocInquiryByBatch.srLoadNum = pstrSrLoadNum;
            objOBAlocInquiryByBatch.screenId = pstrScreenId;
            objOBAllocation.OBAlocInquiryByBatch = objOBAlocInquiryByBatch;

            OBAllocationService AllocationService = new OBAllocationService();
            CompanyService ServiceObjectCompany = new CompanyService();
            Company objCompany = new Company();
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objOBAllocation.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objCompany.cmp_id = pstrCmpId;
            objOBAllocation = AllocationService.GetOBSRSummary(objOBAllocation);
            if (pstrScreenId == "OBSRAlocByBatch")
            { 
                Mapper.CreateMap<ClsOBAllocation, OBAllocationModel>();
                OBAllocationModel objOBAlocInquiryByBatchModel = Mapper.Map<ClsOBAllocation, OBAllocationModel>(objOBAllocation);
                return PartialView("_OBSRAlocByBatch", objOBAlocInquiryByBatchModel);
            }
            else
            {
                Mapper.CreateMap<ClsOBAllocation, OBAllocationModel>();
                OBAllocationModel objOBAlocInquiryByBatchModel = Mapper.Map<ClsOBAllocation, OBAllocationModel>(objOBAllocation);
                return PartialView("_GridSRAlocList", objOBAlocInquiryByBatchModel);
            }
        }
   
        public ActionResult getOBSRAlocVerifyStk(string pstrCmpId)
        {
            string lstrRptFile = string.Empty;
            string lstrFileName = string.Empty;
            string lstrtempFileName = string.Empty;
            OBAllocationService AllocationService = new OBAllocationService();
            string lstrRefNum = Session["obSrAlocRefNum"].ToString();
            DataTable dtStk = new DataTable();
            dtStk = AllocationService.getOBSRAlocStatus(pstrCmpId, lstrRefNum);
            lstrRptFile = pstrCmpId + "-" + "OB-SR-ALOC-STK-VERIFY-RPT";
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();

            if (!Directory.Exists(strOutputpath))
            {
                Directory.CreateDirectory(strOutputpath);
            }

            lstrFileName = pstrCmpId.ToUpper().ToString().Trim() + "-SR-ALOC-STK-RPT" + lstrRefNum + ".xlsx";

            lstrtempFileName = strOutputpath + lstrFileName;

            if (System.IO.File.Exists(lstrtempFileName)) System.IO.File.Delete(lstrtempFileName);
            xls_ob_sr_aloc_verify_stk mxcel1 = null;

            mxcel1 = new xls_ob_sr_aloc_verify_stk(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB-SR-ALOC-STK-VERIFY.xlsx");
            mxcel1.PopulateHeader(pstrCmpId);
            mxcel1.PopulateData(dtStk);
            mxcel1.SaveAs(lstrtempFileName);
            FileStream fs = new FileStream(lstrtempFileName, FileMode.Open, FileAccess.Read);
            //Session["RptFileName"] = lstrtempFileName;
            return File(fs, "application / xlsx", lstrFileName);

        }

        public bool SendOBSRAlocStatusReport(string pstrCmpId, string pstrRefNum)
        {
            bool blnStatus = false;
            bool isValid = true;
          
            OBAllocationService AllocationService = new OBAllocationService();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            string lstrRptFile = string.Empty;
            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string lstrEmailId = string.Empty;

            lstrRptFile = pstrCmpId + "-" + "OB-SR-ALOC-STATUS";
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
          

            DataTable dtOBSrAloc = new DataTable();
            DataTable dtUserEmail = new DataTable();

     
          
            objEmail.CmpId = pstrCmpId;
            objEmail.screenId = "OB SR Allocation Status";
            objEmail.username = Session["UserID"].ToString().Trim();

            try
            {
                if (isValid)
                {
                  
                    dtOBSrAloc = AllocationService.getOBSRAlocStatus(pstrCmpId, pstrRefNum, Session["UserID"].ToString().Trim(), ref dtUserEmail);

                    try
                    {
                        lstrEmailId = (dtUserEmail.Rows[0]["email"]).ToString();
                    }
                    catch
                    {
                        lstrEmailId = System.Configuration.ConfigurationManager.AppSettings["FromEmailId"].ToString();
                    }

                    if (!Directory.Exists(strOutputpath))
                    {
                        Directory.CreateDirectory(strOutputpath);
                    }

                    l_str_file_name =  pstrCmpId.ToUpper().ToString().Trim() + "-SR-ALOC-" + pstrRefNum + ".xlsx";

                    tempFileName = strOutputpath + l_str_file_name;

                    if (System.IO.File.Exists(tempFileName)) System.IO.File.Delete(tempFileName);
                    xls_ob_sr_aloc_status mxcel1 = null;

                    mxcel1 = new xls_ob_sr_aloc_status(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB-SR-ALOC-STATUS.xlsx");
                    mxcel1.PopulateHeader(pstrCmpId);
                    mxcel1.PopulateData(dtOBSrAloc);
                    mxcel1.SaveAs(tempFileName);
                    FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                    Session["RptFileName"] = tempFileName;
                    reportFileName = l_str_file_name;

                    objEmail.EmailSubject = "OB-SR-ALOC-STATUS-" + pstrRefNum;
                    objEmail.EmailMessage = "Hi All," + "\n\n" + " Please Find the attached OB Ship Request Alocation Status Report";
                    
                    string l_str_email_regards = string.Empty;
                    string l_str_email_footer1 = string.Empty;
                    string l_str_email_footer2 = string.Empty;
                    try
                    {
                        l_str_email_regards = System.Configuration.ConfigurationManager.AppSettings["EmailRegards"].ToString().Trim();
                        l_str_email_footer1 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter1"].ToString().Trim();
                        l_str_email_footer2 = System.Configuration.ConfigurationManager.AppSettings["EmailFooter2"].ToString().Trim();
                    }
                    catch (Exception ex)
                    {
                        l_str_email_regards = "3PL WAREHOUSE";
                        l_str_email_footer1 = "Thank you for your business.";
                        l_str_email_footer2 = "Please Do not reply to this alert mail, the mail box is not monitored. If any question or help, please contact the CSR";
                    }

                    objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + l_str_email_footer1;
                    objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + "Regards,";
                    objEmail.EmailMessage = objEmail.EmailMessage + "\n" + l_str_email_regards;
                    objEmail.EmailMessage = objEmail.EmailMessage + "\n" + "\n" + l_str_email_footer2;
                    objEmail.FilePath = strOutputpath;
                    objEmail.Attachment = reportFileName;

                    MailMessage mail = new MailMessage();
                    string strSMTPHost = string.Empty;
                    string strSmtpUserId = string.Empty;
                    string strSmtpPassword = string.Empty;
                    string l_str_from_email = string.Empty;
                    string l_str_from_email_display_name = string.Empty;

                    try
                    {
                        strSMTPHost = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString();
                        strSmtpUserId = System.Configuration.ConfigurationManager.AppSettings["SMTPUserId"].ToString();
                        strSmtpPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPUserPwd"].ToString();
                        l_str_from_email = System.Configuration.ConfigurationManager.AppSettings["FromEmailId"].ToString();
                        l_str_from_email_display_name = System.Configuration.ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                    }
                    catch
                    {
                        blnStatus = false;
                    }
                    mail.To.Add(lstrEmailId);

                    mail.From = new MailAddress(l_str_from_email, l_str_from_email_display_name);
                    mail.Subject = objEmail.EmailSubject;
                    StringBuilder myBuilder = new StringBuilder();
                     mail.Body = objEmail.EmailMessage;
                    mail.IsBodyHtml = true;
                    string lstr_file_name = string.Empty;
                    Attachment attach = new Attachment(tempFileName);
                    mail.Attachments.Add(attach);
                   
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = strSMTPHost; //"smtp.gmail.com";
                    smtp.Port = 25;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(strSmtpUserId, strSmtpPassword);
                    smtp.EnableSsl = false;

                    try
                    {
                        smtp.Send(mail);
                        blnStatus = true;
                    }
                    catch (Exception ex)
                    {
                        blnStatus = false;
                    }
             
                }
               
            }
            catch (Exception ex)
            {
                blnStatus = false;
            }

            finally
            {
                if (dtOBSrAloc != null)
                {
                    dtOBSrAloc.Dispose();
                }
                if (dtUserEmail != null)
                {
                    dtUserEmail.Dispose();
                }
                objEmail = null;
                objEmailService = null;
                AllocationService = null;
            }
            return blnStatus;

        }

    }
}