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
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static GsEPWv8_5_MVC.Core.Entity.StockAsOfDate;

namespace GsEPWv8_5_MVC.Controllers
{
    public class StockAsOfDateController : Controller
    {
        Email objEmail = new Email();
        public string ScreenID = "StockAsOfDate Inquiry";
        public string l_str_rptdtl = string.Empty;
        public string l_str_tmp_name = string.Empty;
        EmailService objEmailService = new EmailService();
        Company objCompany = new Company();
        CompanyService ServiceObjectCompany = new CompanyService();
        StockAsOfDate objStockAsOfDate = new StockAsOfDate();
        IStockAsOfDateService ServiceObject = new StockAsOfDateService();
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StockAsOfDate(string FullFillType, string cmp, string status, string DateFm, string DateTo)
        {
            string l_str_cmp_id = string.Empty;
            try
            {

                objStockAsOfDate.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
                objStockAsOfDate.is_company_user = Session["IsCompanyUser"].ToString().Trim();

                if (objStockAsOfDate.cmp_id != "")
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStockAsOfDate.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
                else
                {
                    if (FullFillType == null)
                    {
                        objCompany.user_id = Session["UserID"].ToString().Trim();
                        objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                        objStockAsOfDate.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    }
                }
                if (FullFillType != null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStockAsOfDate.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                    objStockAsOfDate.cmp_id = cmp.Trim();
                    //objStockAsOfDate.ib_doc_id = "";
                    //objStockAsOfDate.po_num = "";
                    //objStockAsOfDate.Ref_Num = "";
                    objStockAsOfDate.cont_id = "";
                    objStockAsOfDate.lot_id = "";
                    objStockAsOfDate.loc_id = "";
                    //objStockAsOfDate.whs_id = "";
                    //objStockAsOfDate.Itmdtl = "";
                    objStockAsOfDate.itm_color = "";
                    objStockAsOfDate.itm_size = "";
                    objStockAsOfDate.itm_name = "";

                }
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objStockAsOfDate.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objStockAsOfDate.p_str_company = objStockAsOfDate.cmp_id;
                Mapper.CreateMap<StockAsOfDate, StockAsOfDateModel>();
                StockAsOfDateModel objStockAsOfDateModel = Mapper.Map<StockAsOfDate, StockAsOfDateModel>(objStockAsOfDate);
                return View(objStockAsOfDateModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult GetStockASOfDateDetails(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_Status, string p_str_itm_num, string p_str_itm_color,
            string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_loc_id, string p_str_whs_id, string p_str_As_Of_Date, string p_str_radio)
        {
            try
            {
                StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                IStockAsOfDateService ServiceObject = new StockAsOfDateService();
                objStockAsOfDate.cmp_id = p_str_cmp_id;
                objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                objStockAsOfDate.itm_num = p_str_itm_num;
                objStockAsOfDate.itm_color = p_str_itm_color;
                objStockAsOfDate.itm_size = p_str_itm_size;
                objStockAsOfDate.status = p_str_Status;
                objStockAsOfDate.itm_name = p_str_itm_name;
                objStockAsOfDate.cont_id = p_str_cont_id;
                objStockAsOfDate.loc_id = p_str_loc_id;
                objStockAsOfDate.whs_id = p_str_whs_id;
                objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                objStockAsOfDate = ServiceObject.GetStockAsOfDateDetails(objStockAsOfDate);
                if (p_str_radio.Trim() == "SummaryStyle")
                {
                    Mapper.CreateMap<StockAsOfDate, StockAsOfDateModel>();
                    StockAsOfDateModel objStockAsOfDateModel = Mapper.Map<StockAsOfDate, StockAsOfDateModel>(objStockAsOfDate);
                    return PartialView("_StockAsOfdateSummary", objStockAsOfDateModel);
                }
                else
                {
                    Mapper.CreateMap<StockAsOfDate, StockAsOfDateModel>();
                    StockAsOfDateModel objStockAsOfDateModel = Mapper.Map<StockAsOfDate, StockAsOfDateModel>(objStockAsOfDate);
                    return PartialView("_StockAsOfDate", objStockAsOfDateModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ShowPDFReport(string p_str_file_name, string p_str_cmp_id)
        {
            FileStream fs = new FileStream(p_str_file_name, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        public ActionResult ShowGridExcelReport(string p_str_grid_name, string p_str_cmp_id)
        {
            return new DownloadFileActionResult((GridView)Session[p_str_grid_name], p_str_grid_name + "" + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xls");
        }
        public ActionResult ShowExcelReport(string p_str_file_name, string p_str_cmp_id)
        {
            string strDateFormat = string.Empty;
            FileStream fs = new FileStream(p_str_file_name, FileMode.Open, FileAccess.Read);
            string l_str_down_load_file_name = string.Empty;
            strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            l_str_down_load_file_name = p_str_cmp_id + "-" + "IV-" + strDateFormat + ".xlsx";

            return File(fs, "application / xlsx", l_str_down_load_file_name);
        }

        public ActionResult ShowReport(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
          string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_loc_id, string p_str_whs_id, string p_str_As_Of_Date, string p_str_Status, string type, string p_str_style_stearch)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            string lstrRptName = string.Empty;
            string strDateFormat = string.Empty;
            l_str_rpt_selection = var_name;
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            objStockAsOfDate.style_stearch = p_str_style_stearch;
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailbyStyle")
                    {
                       
                        objStockAsOfDate.cmp_id = p_str_cmp_id;
                        objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                        objStockAsOfDate.itm_num = p_str_itm_num;
                        objStockAsOfDate.itm_color = p_str_itm_color;
                        objStockAsOfDate.itm_size = p_str_itm_size;
                        objStockAsOfDate.status = p_str_Status;
                        objStockAsOfDate.itm_name = p_str_itm_name;
                        objStockAsOfDate.cont_id = p_str_cont_id;
                        objStockAsOfDate.loc_id = p_str_loc_id;
                        objStockAsOfDate.whs_id = p_str_whs_id;
                        objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                       
                        if (type == "PDF")
                        {
                            strReportName = "rpt_inventory_AsOf_date_Detail_Rpt.rpt";
                            //StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                            IStockAsOfDateService ServiceObject = new StockAsOfDateService();

                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                            objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);
                            var rptSource = objStockAsOfDate.LstAsOfDateRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStockAsOfDate.LstAsOfDateRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objStockAsOfDate.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objStockAsOfDate.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                       
                        else
                        if (type == "Excel")
                        {
                            
                            DataTable dtInvAsOfDate = new DataTable();
                            lstrRptName = "As Of Date Stock Detail Report";
                            dtInvAsOfDate = ServiceObject.getInvAsOfDateByStyle(objStockAsOfDate);
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            strDateFormat = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INV-AS-OF-DATE-DTL-RPT-" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_inv_as_of_date_by_style mxcel1 = new xls_inv_as_of_date_by_style(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IV-AS-OF-DATE-BY-STYLE.xlsx");

                            mxcel1.PopulateHeader(p_str_cmp_id, p_str_As_Of_Date);
                            mxcel1.PopulateData(dtInvAsOfDate, true);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                        }
                    }

                    if (l_str_rpt_selection == "SummarybyStyle")
                    {
                        strReportName = "rpt_inventory_AsOf_date_Summary_Rpt.rpt";
                        StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                        IStockAsOfDateService ServiceObject = new StockAsOfDateService();
          
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStockAsOfDate.cmp_id = p_str_cmp_id;
                        objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                        objStockAsOfDate.itm_num = p_str_itm_num;
                        objStockAsOfDate.itm_color = p_str_itm_color;
                        objStockAsOfDate.itm_size = p_str_itm_size;
                        objStockAsOfDate.status = p_str_Status;
                        objStockAsOfDate.itm_name = p_str_itm_name;
                        objStockAsOfDate.cont_id = p_str_cont_id;
                        objStockAsOfDate.loc_id = p_str_loc_id;
                        objStockAsOfDate.whs_id = p_str_whs_id;
                        objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                        objStockAsOfDate.style_stearch = p_str_style_stearch;
                        objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);//CR-3PL-MVC-180322-001 Added By NIthya
                        if (type == "PDF")
                        {
                            var rptSource = objStockAsOfDate.LstAsOfDateRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStockAsOfDate.LstAsOfDateRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objStockAsOfDate.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objStockAsOfDate.Image_Path);
                                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                                }
                            }
                        }
                        
                        else if (type == "Excel")
                        {
                            DataTable dtInvAsOfDate = new DataTable();
                            lstrRptName = "As Of Date Stock Summary Report";
                            dtInvAsOfDate = ServiceObject.getInvAsOfDateByStyleSummary(objStockAsOfDate);
                            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
                            strDateFormat = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                            string tempFileName = string.Empty;
                            string l_str_file_name = string.Empty;
                            l_str_file_name = p_str_cmp_id.ToUpper().ToString().Trim() + "-INV-AS-OF-DATE-SMRY-RPT-" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + l_str_file_name;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_inv_as_of_date_by_style_smry mxcel1 = new xls_inv_as_of_date_by_style_smry(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IV-AS-OF-DATE-BY-STYLE-SMRY.xlsx");

                            mxcel1.PopulateHeader(p_str_cmp_id, p_str_As_Of_Date);
                            mxcel1.PopulateData(dtInvAsOfDate, true);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            return File(fs, "application / xlsx", l_str_file_name);
                           
                        }
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult EmailShowReport(string var_name, string p_str_cmp_id, string p_str_ib_doc_id, string p_str_itm_num, string p_str_itm_color,
        string p_str_itm_size, string p_str_itm_name, string p_str_cont_id, string p_str_loc_id, string p_str_whs_id, string p_str_As_Of_Date, string p_str_Status, string type, string p_str_style_stearch)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_selection1 = string.Empty;
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            string lstrEmailMessage = string.Empty;
            string lstrRptName = string.Empty;
            string lstrEmailSubject = string.Empty;
           // string Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            l_str_rpt_selection = var_name;
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objStockAsOfDate.LstCmpName = objCompany.LstCmpName;
            l_str_tmp_name = objStockAsOfDate.LstCmpName[0].cmp_name.ToString().Trim();
            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);

            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            try
            {
                if (isValid)
                {
                    if (l_str_rpt_selection == "DetailbyStyle")
                    {
                        if (type == "PDF")
                        {
                            strReportName = "rpt_inventory_AsOf_date_Detail_Rpt.rpt";

        
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                        objStockAsOfDate.cmp_id = p_str_cmp_id;
                        objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                        objStockAsOfDate.itm_num = p_str_itm_num;
                        objStockAsOfDate.itm_color = p_str_itm_color;
                        objStockAsOfDate.itm_size = p_str_itm_size;
                        objStockAsOfDate.status = p_str_Status;
                        objStockAsOfDate.itm_name = p_str_itm_name;
                        objStockAsOfDate.cont_id = p_str_cont_id;
                        objStockAsOfDate.loc_id = p_str_loc_id;
                        objStockAsOfDate.whs_id = p_str_whs_id;
                        objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                            objStockAsOfDate.style_stearch = p_str_style_stearch;
                        objStockAsOfDate = ServiceObject.GetTotalCottonStockAsOfDateDetails(objStockAsOfDate);
                        objStockAsOfDate.Ctns = objStockAsOfDate.ListCottonStockAsOfDateGrid[0].Ctns;
                        objStockAsOfDate.totcube = objStockAsOfDate.ListCottonStockAsOfDateGrid[0].totcube;
                        objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);
                            reportFileName = objStockAsOfDate.cmp_id + "-STOCK-REPORT-AS-ON-" + objStockAsOfDate.As_Of_Date + ".pdf";
                            lstrEmailSubject = objStockAsOfDate.cmp_id + "-" + "STOCK REPORT AS ON " + objStockAsOfDate.As_Of_Date + "|" + "Total Cartons : " + objStockAsOfDate.Ctns + "|" + "Total Cube :" + objStockAsOfDate.totcube;
                            lstrEmailMessage =  "CmpId :" + objStockAsOfDate.cmp_id + " " + l_str_tmp_name + "\n" + "Stock Report as on " + objStockAsOfDate.As_Of_Date + "\n" + "Total Cartons : " + objStockAsOfDate.Ctns + "\n" + "Total Cube :" + objStockAsOfDate.totcube;
                       
                            var rptSource = objStockAsOfDate.LstAsOfDateRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStockAsOfDate.LstAsOfDateRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objStockAsOfDate.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objStockAsOfDate.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    strFileName = strOutputpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                            
                            Session["RptFileName"] = strFileName;
                            // rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
                        }
                       
                        else  if (type == "Excel")
                        {
                            objStockAsOfDate.cmp_id = p_str_cmp_id;
                            objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                            objStockAsOfDate.itm_num = p_str_itm_num;
                            objStockAsOfDate.itm_color = p_str_itm_color;
                            objStockAsOfDate.itm_size = p_str_itm_size;
                            objStockAsOfDate.status = p_str_Status;
                            objStockAsOfDate.itm_name = p_str_itm_name;
                            objStockAsOfDate.cont_id = p_str_cont_id;
                            objStockAsOfDate.loc_id = p_str_loc_id;
                            objStockAsOfDate.whs_id = p_str_whs_id;
                            objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                            objStockAsOfDate.style_stearch = p_str_style_stearch;
                            DataTable dtInvAsOfDate = new DataTable();
                            lstrRptName = "As Of Date Stock Detail Report";
                            dtInvAsOfDate = ServiceObject.getInvAsOfDateByStyle(objStockAsOfDate);
                          
                            strDateFormat = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                            string tempFileName = string.Empty;
                                reportFileName =  p_str_cmp_id.ToUpper().ToString().Trim() + "-INV-AS-OF-DATE-DTL-RPT-" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + reportFileName;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_inv_as_of_date_by_style mxcel1 = new xls_inv_as_of_date_by_style(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IV-AS-OF-DATE-BY-STYLE.xlsx");

                            mxcel1.PopulateHeader(p_str_cmp_id, p_str_As_Of_Date);
                            mxcel1.PopulateData(dtInvAsOfDate, true);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            Session["RptFileName"] = tempFileName;
                            lstrEmailSubject = objStockAsOfDate.cmp_id + "-" + "STOCK REPORT AS ON " + objStockAsOfDate.As_Of_Date;
                            lstrEmailMessage = "CmpId :" + objStockAsOfDate.cmp_id + " " + l_str_tmp_name + "\n\n" + "Stock Report as on " + objStockAsOfDate.As_Of_Date;
                        }
                    }

                   else if (l_str_rpt_selection == "SummarybyStyle")
                    {
                        strReportName = "rpt_inventory_AsOf_date_Summary_Rpt.rpt";
                        StockAsOfDate objStockAsOfDate = new StockAsOfDate();
                        IStockAsOfDateService ServiceObject = new StockAsOfDateService();

                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inventory//" + strReportName;
                    
                        objStockAsOfDate.cmp_id = p_str_cmp_id;
                        objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                        objStockAsOfDate.itm_num = p_str_itm_num;
                        objStockAsOfDate.itm_color = p_str_itm_color;
                        objStockAsOfDate.itm_size = p_str_itm_size;
                        objStockAsOfDate.status = p_str_Status;
                        objStockAsOfDate.itm_name = p_str_itm_name;
                        objStockAsOfDate.cont_id = p_str_cont_id;
                        objStockAsOfDate.loc_id = p_str_loc_id;
                        objStockAsOfDate.whs_id = p_str_whs_id;
                        objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                        objStockAsOfDate.style_stearch = p_str_style_stearch;
                        if (type == "PDF")
                        {
                            objStockAsOfDate = ServiceObject.GetTotalCottonStockAsOfDateDetails(objStockAsOfDate);
                            objStockAsOfDate.Ctns = objStockAsOfDate.ListCottonStockAsOfDateGrid[0].Ctns;
                            objStockAsOfDate.totcube = objStockAsOfDate.ListCottonStockAsOfDateGrid[0].totcube;
                            objStockAsOfDate = ServiceObject.GetStockAsOfDateDetailsRpt(objStockAsOfDate);
                            reportFileName = objStockAsOfDate.cmp_id + "-" + "STOCK-REPORT-AS-ON-" + objStockAsOfDate.As_Of_Date + ".pdf";
                            lstrEmailSubject = objStockAsOfDate.cmp_id + "-" + "STOCK REPORT AS ON " + objStockAsOfDate.As_Of_Date + "|" + "Total Cartons : " + objStockAsOfDate.Ctns + "|" + "Total Cube :" + objStockAsOfDate.totcube;
                            lstrEmailMessage =  "CmpId :" + objStockAsOfDate.cmp_id + " " + l_str_tmp_name + "\n" + "Stock Report as on " + objStockAsOfDate.As_Of_Date + "\n" + "Total Cartons : " + objStockAsOfDate.Ctns + "\n" + "Total Cube :" + objStockAsOfDate.totcube;

                            var rptSource = objStockAsOfDate.LstAsOfDateRpt.ToList();
                            if (rptSource.Count > 0)
                            {
                                using (ReportDocument rd = new ReportDocument())
                                {
                                    rd.Load(strRptPath);
                                    int AlocCount = 0;
                                    AlocCount = objStockAsOfDate.LstAsOfDateRpt.Count();
                                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                        rd.SetDataSource(rptSource);
                                    objStockAsOfDate.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objStockAsOfDate.Image_Path);
                                    strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                                    strFileName = strOutputpath + "//" + reportFileName;
                                    rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                                }
                            }
                          
                            Session["RptFileName"] = strFileName;
                       
                        }
                        
                        else
                        if (type == "Excel")
                        {
                            objStockAsOfDate.cmp_id = p_str_cmp_id;
                            objStockAsOfDate.ib_doc_id = p_str_ib_doc_id;
                            objStockAsOfDate.itm_num = p_str_itm_num;
                            objStockAsOfDate.itm_color = p_str_itm_color;
                            objStockAsOfDate.itm_size = p_str_itm_size;
                            objStockAsOfDate.status = p_str_Status;
                            objStockAsOfDate.itm_name = p_str_itm_name;
                            objStockAsOfDate.cont_id = p_str_cont_id;
                            objStockAsOfDate.loc_id = p_str_loc_id;
                            objStockAsOfDate.whs_id = p_str_whs_id;
                            objStockAsOfDate.As_Of_Date = p_str_As_Of_Date;
                            objStockAsOfDate.style_stearch = p_str_style_stearch;
                            DataTable dtInvAsOfDate = new DataTable();
                            dtInvAsOfDate = ServiceObject.getInvAsOfDateByStyleSummary(objStockAsOfDate);
                         
                            strDateFormat = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                            string tempFileName = string.Empty;

                            reportFileName = p_str_cmp_id.ToUpper().ToString().Trim() + "-INV-AS-OF-DATE-SMRY-RPT-" + strDateFormat + ".xlsx";

                            tempFileName = strOutputpath + "//" + reportFileName;

                            if (System.IO.File.Exists(tempFileName))
                                System.IO.File.Delete(tempFileName);
                            xls_inv_as_of_date_by_style_smry mxcel1 = new xls_inv_as_of_date_by_style_smry(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IV-AS-OF-DATE-BY-STYLE-SMRY.xlsx");

                            mxcel1.PopulateHeader(p_str_cmp_id, p_str_As_Of_Date);
                            mxcel1.PopulateData(dtInvAsOfDate, true);
                            mxcel1.SaveAs(tempFileName);
                            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                            Session["RptFileName"] = tempFileName;
                            lstrEmailSubject = objStockAsOfDate.cmp_id + "-" + " As Of Date Stock Summary Report on - " + objStockAsOfDate.As_Of_Date;
                            lstrEmailMessage = "CmpId :" + objStockAsOfDate.cmp_id + " " + l_str_tmp_name + "\n" + "Stock Report as on " + objStockAsOfDate.As_Of_Date;

                        }
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
               


                EmailAlertHdr objEmailAlertHdr = new EmailAlertHdr();
                clsRptEmail objRptEmail = new clsRptEmail();
                bool lblnRptEmailExists = false;
                objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmp_id, "INV", "STOCK-AS-OF-DATE", ref lblnRptEmailExists);
                string l_str_email_message = string.Empty;
                if (lblnRptEmailExists == false)
                {
                    if (lstrEmailMessage.Length ==0)
                    {
                        objEmailAlertHdr.emailMessage = "Hi All," + "\n\n" + " Please Find the attached As Of Date Stock Report";
                    }
                    else
                    {
                        objEmailAlertHdr.emailMessage = "Hi All," + "\n\n" + " Please Find the attached As Of Date Stock Report" + "\n" + lstrEmailMessage;
                    }
                    
                }
                else
                {
                    if (lstrEmailMessage.Length == 0)
                    {
                        objEmailAlertHdr.emailMessage = "Hi All," + "\n\n" + objEmailAlertHdr.emailMessage;

                    }
                    else
                    {
                        objEmailAlertHdr.emailMessage = "Hi All," + "\n\n" + objEmailAlertHdr.emailMessage + "\n" + lstrEmailMessage;

                    }
                       
                }

                objEmailAlertHdr.filePath = strOutputpath;
                objEmailAlertHdr.fileName = reportFileName;
                objEmailAlertHdr.emailSubject = lstrEmailSubject;
                EmailAlert objEmailAlert = new EmailAlert();
                objEmailAlertHdr.cmpId = p_str_cmp_id;
                objEmailAlert.objEmailAlertHdr = objEmailAlertHdr;

                Mapper.CreateMap<EmailAlert, EmailAlertModel>();
                EmailAlertModel EmailModel = Mapper.Map<EmailAlert, EmailAlertModel>(objEmailAlert);
                return PartialView("_EmailAlert", EmailModel);

              
                return PartialView("_Email", EmailModel);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }
    }
}