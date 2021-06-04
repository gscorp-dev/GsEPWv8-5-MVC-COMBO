using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Common;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
namespace GsEPWv8_5_MVC.Controllers
{
    public class eComSR940Controller : Controller
    {
        // GET: ShipRequest940

        public List<tbl_so_hdr> hdr_lst;
        public List<tbl_so_dtl> dtl_lst;
        public List<eComSR940> error_lst;
        public List<eComSR940> batcherror_lst;

        public ActionResult List()
        {
            List<OrderVM> allOrder = new List<OrderVM>();
            return View(allOrder);
        }
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string Folderpath = string.Empty;
        int l_str_hdrCount = 0;
        int resultbatchCount = 0;
        int resultCount = 0;
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
        public ActionResult eComSR940(string FullFillType, string cmp_id,string p_str_scn_id)
        {
            string l_str_cmp_id = string.Empty;


            eComSR940 objeComSR940 = new eComSR940();

            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objeComSR940.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
            objeComSR940.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            if (objeComSR940.cmp_id != "" && FullFillType == null)
            {

                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objeComSR940.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                objeComSR940.cmp_id = objeComSR940.ListCompanyPickDtl[0].cmp_id;

            }
            else
            {
                if (FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objeComSR940.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objeComSR940.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

                }
            }
            if (FullFillType != null)
            {
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objeComSR940.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
               

            }
            objeComSR940.view_flag = "false";
            Session["hdr_lst"] = "";
            Session["dtl_lst"] = "";
            Session["dtl_list"] = "";
            Session["dtl_list_rpt"] = "";
            Session["error_lst"] = "";
            Session["error_lst_count"] = null;
            Mapper.CreateMap<eComSR940, eComSR940Model>();
            eComSR940Model objeComSR940ModelgModel = Mapper.Map<eComSR940, eComSR940Model>(objeComSR940);
            return View(objeComSR940ModelgModel);

        }
        public ActionResult GridClear(string p_str_cmp_id)
        {
            string l_str_cmp_id = string.Empty;
            eComSR940 objeComSR940 = new eComSR940();
            Mapper.CreateMap<eComSR940, eComSR940Model>();
            Session["hdr_lst"] = "";
            Session["dtl_lst"] = "";
            Session["dtl_list"] = "";
            Session["dtl_list_rpt"] = "";
            Session["error_lst"] = "";
            Session["error_lst_count"] = null;
            eComSR940Model objeComSR940ModelgModel = Mapper.Map<eComSR940, eComSR940Model>(objeComSR940);
            return PartialView("_eComSR940", objeComSR940ModelgModel);

        }
        public ActionResult UploadFiles(string l_str_cmp_id)
        {
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            List<eComSR940> li = new List<eComSR940>();
            batcherror_lst = new List<eComSR940>();
            //Session["hdr_lst"] = "";
            //Session["dtl_lst"] = "";
            Session["dtl_list"] = "";
            Session["dtl_list_rpt"] = "";
            Session["error_lst"] = "";
            Session["error_lst_count"] = null;
            Session["Lessthan"] = null;
            objeComSR940.cmp_id = l_str_cmp_id;
            string prevbatchId = string.Empty;
            string nextbatchId = string.Empty;
            string lstrbatchList;
            string lstrstatusList;
            lstrbatchList = ("");
            lstrstatusList = ("");
            int StatusCount = 0;
            objeComSR940 = ServiceObjecteComSR940.DeleteTempTable(objeComSR940);
            if (Request.Files.Count > 0)
            {
                try
                {
                    objeComSR940.error_mode = false;
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase FileUpload = files[i];
                        string fname;
                        string l_str_error_msg = string.Empty;
                        string BatchResult = string.Empty;


                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = FileUpload.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = FileUpload.FileName;
                        }


                        if (FileUpload != null)
                        {
                            if (FileUpload.ContentLength > 0)
                            {
                                //Workout our file path
                                string fileName = Path.GetFileName(FileUpload.FileName);
                                string path = Path.Combine(Server.MapPath("~/uploads"), fileName);
                                //Try and upload
                                try
                                {
                                    FileUpload.SaveAs(path);
                                    string l_str_ext = Path.GetExtension(fileName);
                                    if (l_str_ext.ToUpper() != ".CSV")
                                    {
                                        objeComSR940.error_mode = true;
                                        objeComSR940.error_desc = "Invalid File Format";
                                        return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                    }

                                    Get_List_Data(l_str_cmp_id, path, ".CSV", ref l_str_error_msg, ref BatchResult);
                                    if (l_str_error_msg != "")
                                    {
                                        objeComSR940.error_mode = true;
                                        objeComSR940.error_desc = l_str_error_msg;
                                        if (Session["Lessthan"] != null)
                                        {
                                            objeComSR940.File_Length = "L";
                                        }
                                        //  return Json(objeComSR940, JsonRequestBehavior.AllowGet);

                                    }

                                    DataTable out_dt = (DataTable)CreateGridTable(hdr_lst, dtl_lst);
                                    if (out_dt != null)
                                    {
                                        DataTable SRHdrDtl = new DataTable();
                                        //var model = out_dt.AsEnumerable().ToList();
                                        //objeComSR940.ListeCom940SRUploadDtl = model.ToList();
                                        //List<Student> studentList = new List<Student>();
                                        int linecount = 0;

                                        li = (from DataRow dr in out_dt.Rows
                                              select new eComSR940()
                                              {
                                                  HeaderInfo = dr["HeaderInfo"].ToString(),
                                                  BatchNo = dr["BatchNo"].ToString(),
                                                  CustID = dr["CustID"].ToString(),
                                                  CustName = dr["CustName"].ToString(),
                                                  Store = dr["Store"].ToString(),
                                                  Dept = dr["Dept"].ToString(),
                                                  CustPO = dr["CustPO"].ToString(),
                                                  SOID = dr["SOID"].ToString(),
                                                  RelID = dr["RelID"].ToString(),
                                                  ReqDt = Convert.ToDateTime(dr["ReqDt"].ToString()),
                                                  StartDt = Convert.ToDateTime(dr["StartDt"].ToString()),
                                                  CancelDt = Convert.ToDateTime(dr["CancelDt"].ToString()),
                                                  DtlCount = Convert.ToInt32(dr["DtlCount"].ToString()),
                                                  ShipVia = dr["ShipVia"].ToString(),
                                                  ShipName = dr["ShipName"].ToString(),
                                                  ShipAdd1 = dr["ShipAdd1"].ToString(),
                                                  ShipAdd2 = dr["ShipAdd2"].ToString(),
                                                  City = dr["City"].ToString(),
                                                  State = dr["State"].ToString(),
                                                  ZipCode = dr["ZipCode"].ToString(),
                                                  NoteHdr = dr["NoteHdr"].ToString(),
                                                  TtalQty = Convert.ToInt32(dr["TtalQty"].ToString()),
                                                  CreatedBy = dr["CreatedBy"].ToString(),
                                                  CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString()),
                                                  POLine = Convert.ToInt32(dr["POLine"].ToString()),
                                                  Style = dr["Style"].ToString(),
                                                  CustSKU = dr["CustSKU"].ToString(),
                                                  StyleQty = Convert.ToInt32(dr["StyleQty"].ToString()),
                                                  StyleCarton = Convert.ToInt32(dr["StyleCarton"].ToString()),
                                                  StylePPK = dr["StylePPK"].ToString(),
                                                  StyleCube = Convert.ToDouble(dr["StyleCube"].ToString()),
                                                  StyleWgt = Convert.ToDouble(dr["StyleWgt"].ToString()),
                                                  StyleDesc = dr["StyleDesc"].ToString(),
                                                  StyleStatus = Convert.ToInt32(dr["StyleStatus"].ToString()),
                                                  StatusDesc = dr["StatusDesc"].ToString(),
                                                  Itm_Code = dr["Itm_Code"].ToString()
                                              }).ToList();

                                        objeComSR940.ListeCom940SRUploadDtl = li;
                                    }
                                    Session["tbl_rpt_temp_list"] = objeComSR940.ListeCom940SRUploadDtl;

                                    //CR-20180702-001 Added by nithya BAtchId validation
                                    for (int j = 0; j < objeComSR940.ListeCom940SRUploadDtl.Count; j++)
                                    {
                                        objeComSR940.cmp_id = l_str_cmp_id;
                                        objeComSR940.BatchNo = objeComSR940.ListeCom940SRUploadDtl[j].BatchNo;
                                        nextbatchId = objeComSR940.BatchNo.Substring(0, 5);
                                        if (prevbatchId != nextbatchId)
                                        {
                                            objeComSR940 = ServiceObjecteComSR940.CheckExistBatchId(objeComSR940);
                                            if (objeComSR940.ListCheckExistBatchNo.Count > 0)
                                            {
                                                objeComSR940.BatchNo = objeComSR940.ListCheckExistBatchNo[0].BatchNo;
                                                objeComSR940.status = objeComSR940.ListCheckExistBatchNo[0].status;
                                                prevbatchId = objeComSR940.BatchNo.Substring(0, 5);
                                                eComSR940 batch_error = new eComSR940();
                                                batch_error.File_Line_No = i + 1;
                                                batch_error.BatchNo = objeComSR940.BatchNo;
                                                batch_error.status = objeComSR940.status;
                                                batcherror_lst.Add(batch_error);
                                                Session["batcherror_lst"] = batcherror_lst;
                                                objeComSR940.ListEcombatchError = Session["batcherror_lst"] as List<eComSR940>;
                                            }
                                            
                                        }
                                        
                                    }
                                   // objeComSR940.ListEcombatchError = Session["batcherror_lst"] as List<eComSR940>;
                                    if (objeComSR940.ListEcombatchError != null)
                                    {
                                        for (int k = 0; k < objeComSR940.ListEcombatchError.Count; k++)
                                        {
                                            objeComSR940.BatchNo = objeComSR940.ListEcombatchError[k].BatchNo;
                                            lstrbatchList = lstrbatchList + objeComSR940.BatchNo + "','";
                                            objeComSR940.status = objeComSR940.ListEcombatchError[k].status;
                                            lstrstatusList = lstrstatusList + objeComSR940.status + "','";
                                            if (objeComSR940.status == "A" || objeComSR940.status == "P")
                                            {
                                                StatusCount = StatusCount + 1;
                                                objeComSR940.statuscount = StatusCount;                                                
                                            }                                            
                                        }
                                        lstrbatchList = lstrbatchList.Remove(lstrbatchList.Length - 2, 2);
                                        lstrbatchList = lstrbatchList + "";
                                        lstrstatusList = lstrstatusList.Remove(lstrstatusList.Length - 2, 2);
                                        lstrstatusList = lstrstatusList + "";
                                    }
                                    objeComSR940.error_desc = lstrbatchList + lstrstatusList;
                                    if (objeComSR940.statuscount > 0)
                                    {
                                        objeComSR940.error_mode = true;
                                        objeComSR940.Error_flag = "A";
                                        objeComSR940.error_desc = objeComSR940.error_desc;
                                        objeComSR940.ListeCom940SRUploadDtl = li;
                                        return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                    }
                                    if (objeComSR940.status == "O")
                                    {
                                        objeComSR940.error_mode = true;
                                        objeComSR940.Error_flag = "O";
                                        objeComSR940.error_desc = objeComSR940.error_desc;
                                        objeComSR940.ListeCom940SRUploadDtl = li;
                                        objeComSR940.ListEcomError = Session["error_lst"] as List<eComSR940>;
                                        if (objeComSR940.ListEcomError != null)
                                        {
                                            Session["error_lst_count"] = objeComSR940.ListEcomError.Count();
                                        }
                                        else
                                        {
                                            Session["error_lst_count"] = null;
                                        }
                                        return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                    }

                                    //END                                   
                                }
                                catch (Exception ex)
                                {
                                    //Catch errors
                                    //  ViewData["Feedback"] = ex.Message;
                                    objeComSR940.error_mode = true;
                                    objeComSR940.error_desc = ex.InnerException.ToString();


                                    return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }


                        else
                        {
                            //Catch errors
                            objeComSR940.error_mode = true;
                            objeComSR940.error_desc = "Please select a file";
                            return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                        }



                    }
                }
                catch (Exception ex)
                {
                    objeComSR940.error_mode = true;
                    objeComSR940.error_desc = ex.Message;
                    return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objeComSR940.error_mode = true;
                objeComSR940.error_desc = "No files selected.";
                return Json(objeComSR940, JsonRequestBehavior.AllowGet);
            }
            objeComSR940.ListEcomError = Session["error_lst"] as List<eComSR940>;
            if (objeComSR940.ListEcomError != null)
            {
                Session["error_lst_count"] = objeComSR940.ListEcomError.Count();
            }
            else
            {
                Session["error_lst_count"] = null;
            }
            if (Session["hdr_lst"] != null)
            {
                objeComSR940.Header_count = hdr_lst.Count();
                l_str_hdrCount = objeComSR940.Header_count;
            }
            Mapper.CreateMap<eComSR940, eComSR940Model>();
            eComSR940Model eComSR940Model = Mapper.Map<eComSR940, eComSR940Model>(objeComSR940);
            return PartialView("_eComSR940", eComSR940Model);
            //  return View(eComSR940Model);
        }
        public ActionResult UploadFilesupload(string l_str_cmp_id)
        {
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            List<eComSR940> li = new List<eComSR940>();
            batcherror_lst = new List<eComSR940>();
            //Session["hdr_lst"] = "";
            //Session["dtl_lst"] = "";
            Session["dtl_list"] = "";
            Session["dtl_list_rpt"] = "";
            Session["error_lst"] = "";
            Session["error_lst_count"] = null;
            Session["Lessthan"] = null;
            objeComSR940.cmp_id = l_str_cmp_id;
            string prevbatchId = string.Empty;
            string nextbatchId = string.Empty;
            objeComSR940 = ServiceObjecteComSR940.DeleteTempTable(objeComSR940);
            if (Request.Files.Count > 0)
            {
                try
                {
                    objeComSR940.error_mode = false;
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase FileUpload = files[i];
                        string fname;
                        string l_str_error_msg = string.Empty;
                        string BatchResult = string.Empty;


                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = FileUpload.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = FileUpload.FileName;
                        }


                        if (FileUpload != null)
                        {
                            if (FileUpload.ContentLength > 0)
                            {
                                //Workout our file path
                                string fileName = Path.GetFileName(FileUpload.FileName);
                                string path = Path.Combine(Server.MapPath("~/uploads"), fileName);
                                //Try and upload
                                try
                                {
                                    FileUpload.SaveAs(path);
                                    string l_str_ext = Path.GetExtension(fileName);
                                    if (l_str_ext.ToUpper() != ".CSV")
                                    {
                                        objeComSR940.error_mode = true;
                                        objeComSR940.error_desc = "Invalid File Format";
                                        return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                    }

                                    Get_List_Data(l_str_cmp_id, path, ".CSV", ref l_str_error_msg, ref BatchResult);
                                    if (l_str_error_msg != "")
                                    {
                                        objeComSR940.error_mode = true;
                                        objeComSR940.error_desc = l_str_error_msg;
                                        if (Session["Lessthan"] != null)
                                        {
                                            objeComSR940.File_Length = "L";
                                        }
                                        //  return Json(objeComSR940, JsonRequestBehavior.AllowGet);

                                    }

                                    DataTable out_dt = (DataTable)CreateGridTable(hdr_lst, dtl_lst);
                                    if (out_dt != null)
                                    {
                                        DataTable SRHdrDtl = new DataTable();
                                        //var model = out_dt.AsEnumerable().ToList();
                                        //objeComSR940.ListeCom940SRUploadDtl = model.ToList();
                                        //List<Student> studentList = new List<Student>();
                                        int linecount = 0;

                                        li = (from DataRow dr in out_dt.Rows
                                              select new eComSR940()
                                              {
                                                  HeaderInfo = dr["HeaderInfo"].ToString(),
                                                  BatchNo = dr["BatchNo"].ToString(),
                                                  CustID = dr["CustID"].ToString(),
                                                  CustName = dr["CustName"].ToString(),
                                                  Store = dr["Store"].ToString(),
                                                  Dept = dr["Dept"].ToString(),
                                                  CustPO = dr["CustPO"].ToString(),
                                                  SOID = dr["SOID"].ToString(),
                                                  RelID = dr["RelID"].ToString(),
                                                  ReqDt = Convert.ToDateTime(dr["ReqDt"].ToString()),
                                                  StartDt = Convert.ToDateTime(dr["StartDt"].ToString()),
                                                  CancelDt = Convert.ToDateTime(dr["CancelDt"].ToString()),
                                                  DtlCount = Convert.ToInt32(dr["DtlCount"].ToString()),
                                                  ShipVia = dr["ShipVia"].ToString(),
                                                  ShipName = dr["ShipName"].ToString(),
                                                  ShipAdd1 = dr["ShipAdd1"].ToString(),
                                                  ShipAdd2 = dr["ShipAdd2"].ToString(),
                                                  City = dr["City"].ToString(),
                                                  State = dr["State"].ToString(),
                                                  ZipCode = dr["ZipCode"].ToString(),
                                                  NoteHdr = dr["NoteHdr"].ToString(),
                                                  TtalQty = Convert.ToInt32(dr["TtalQty"].ToString()),
                                                  CreatedBy = dr["CreatedBy"].ToString(),
                                                  CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString()),
                                                  POLine = Convert.ToInt32(dr["POLine"].ToString()),
                                                  Style = dr["Style"].ToString(),
                                                  CustSKU = dr["CustSKU"].ToString(),
                                                  StyleQty = Convert.ToInt32(dr["StyleQty"].ToString()),
                                                  StyleCarton = Convert.ToInt32(dr["StyleCarton"].ToString()),
                                                  StylePPK = dr["StylePPK"].ToString(),
                                                  StyleCube = Convert.ToDouble(dr["StyleCube"].ToString()),
                                                  StyleWgt = Convert.ToDouble(dr["StyleWgt"].ToString()),
                                                  StyleDesc = dr["StyleDesc"].ToString(),
                                                  StyleStatus = Convert.ToInt32(dr["StyleStatus"].ToString()),
                                                  StatusDesc = dr["StatusDesc"].ToString(),
                                                  Itm_Code = dr["Itm_Code"].ToString()
                                              }).ToList();

                                        objeComSR940.ListeCom940SRUploadDtl = li;
                                    }
                                    for (int j = 0; j < objeComSR940.ListeCom940SRUploadDtl.Count; j++)
                                    {
                                        objeComSR940.cmp_id = l_str_cmp_id;
                                        objeComSR940.BatchNo = objeComSR940.ListeCom940SRUploadDtl[j].BatchNo;
                                        nextbatchId = objeComSR940.BatchNo.Substring(0, 5);
                                        if (prevbatchId != nextbatchId)
                                        {
                                            objeComSR940.cmp_id = l_str_cmp_id;
                                            objeComSR940.BatchNo = nextbatchId;
                                            objeComSR940 = ServiceObjecteComSR940.deleteExistBatchId(objeComSR940);
                                            //if(objeComSR940.ListCheckExistBatchNo.Count>0)
                                            //{
                                            //    objeComSR940.so_num = objeComSR940.ListCheckExistBatchNo[0].so_num;
                                            //    objeComSR940.error_mode = true;
                                            //    objeComSR940.Error_flag = "B";
                                            //    objeComSR940.error_desc = objeComSR940.so_num;
                                            //    return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                            //}
                                            prevbatchId = objeComSR940.BatchNo;
                                        }
                                    }
                                    //CR-20180702-001 Added by nithya BAtchId validation
                                    //for (int j = 0; j < objeComSR940.ListeCom940SRUploadDtl.Count; j++)
                                    //{
                                    //    objeComSR940.cmp_id = l_str_cmp_id;
                                    //    objeComSR940.BatchNo = objeComSR940.ListeCom940SRUploadDtl[j].BatchNo;
                                    //    nextbatchId = objeComSR940.BatchNo.Substring(0, 5);
                                    //    if (prevbatchId != nextbatchId)
                                    //    {
                                    //        objeComSR940 = ServiceObjecteComSR940.CheckExistBatchId(objeComSR940);
                                    //        if (objeComSR940.ListCheckExistBatchNo.Count > 0)
                                    //        {
                                    //            objeComSR940.BatchNo = objeComSR940.ListCheckExistBatchNo[0].BatchNo;
                                    //            objeComSR940.status = objeComSR940.ListCheckExistBatchNo[0].status;
                                    //            prevbatchId = objeComSR940.BatchNo.Substring(0, 5);
                                    //            eComSR940 batch_error = new eComSR940();
                                    //            batch_error.File_Line_No = i + 1;
                                    //            batch_error.BatchNo = objeComSR940.BatchNo;
                                    //            batch_error.status = objeComSR940.status;
                                    //            batcherror_lst.Add(batch_error);
                                    //            Session["batcherror_lst"] = batcherror_lst;
                                    //        }
                                    //    }
                                    //}
                                    //objeComSR940.ListEcomError = Session["batcherror_lst"] as List<eComSR940>;
                                    //for (int k = 0; k < objeComSR940.ListEcomError.Count; k++)
                                    //{
                                    //    objeComSR940.status = objeComSR940.ListEcomError[k].status;
                                    //}
                                    //if (objeComSR940.status == "A" || objeComSR940.status == "P")
                                    //{
                                    //    objeComSR940.error_mode = true;
                                    //    objeComSR940.Error_flag = "A";
                                    //    objeComSR940.ListeCom940SRUploadDtl = li;
                                    //    return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                    //}
                                    //if (objeComSR940.status == "O")
                                    //{
                                    //    objeComSR940.error_mode = true;
                                    //    objeComSR940.Error_flag = "O";
                                    //    objeComSR940.ListeCom940SRUploadDtl = li;
                                    //    return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                    //}
                                    //END                                   
                                }
                                catch (Exception ex)
                                {
                                    //Catch errors
                                    //  ViewData["Feedback"] = ex.Message;
                                    objeComSR940.error_mode = true;
                                    objeComSR940.error_desc = ex.InnerException.ToString();


                                    return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }


                        else
                        {
                            //Catch errors
                            objeComSR940.error_mode = true;
                            objeComSR940.error_desc = "Please select a file";
                            return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                        }



                    }
                }
                catch (Exception ex)
                {
                    objeComSR940.error_mode = true;
                    objeComSR940.error_desc = ex.Message;
                    return Json(objeComSR940, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objeComSR940.error_mode = true;
                objeComSR940.error_desc = "No files selected.";
                return Json(objeComSR940, JsonRequestBehavior.AllowGet);
            }
            objeComSR940.ListEcomError = Session["error_lst"] as List<eComSR940>;
            if (objeComSR940.ListEcomError != null)
            {
                Session["error_lst_count"] = objeComSR940.ListEcomError.Count();
            }
            else
            {
                Session["error_lst_count"] = null;
            }
            if (Session["hdr_lst"] != null)
            {
                objeComSR940.Header_count = hdr_lst.Count();
                l_str_hdrCount = objeComSR940.Header_count;
            }
            Mapper.CreateMap<eComSR940, eComSR940Model>();
            eComSR940Model eComSR940Model = Mapper.Map<eComSR940, eComSR940Model>(objeComSR940);
            return PartialView("_eComSR940", eComSR940Model);
            //  return View(eComSR940Model);
        }
        public EmptyResult CmpIdOnChange(string p_str_cmp_id)
        {
            Session["g_str_cmp_id"] = (p_str_cmp_id==null?string.Empty: p_str_cmp_id.Trim());
            return null;
        }
        private void Get_List_Data(string strCmpId, string FilePath, string Extension, ref string l_str_error_msg, ref string BatchResult)
        {
            hdr_lst = new List<tbl_so_hdr>();
            dtl_lst = new List<tbl_so_dtl>();
            error_lst = new List<eComSR940>();

            Boolean hdr_ok = false;
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            l_str_error_msg = "";
            string l_str_error_desc = "";
            try
            {
                if (Extension.ToUpper().Equals(".CSV"))
                {
                    string[] l_str_fileContent = System.IO.File.ReadAllLines(FilePath);

                    List<string> lines = new List<string>(System.IO.File.ReadAllLines(FilePath));
                    int blankLine = lines.FindIndex(x => x.Trim().Length == 0);
                    if (blankLine != -1)
                    {
                        while (lines.Count > blankLine)
                        {
                            lines.RemoveAt(lines.Count - 1);
                        }
                    }
                    System.IO.File.WriteAllLines(FilePath, lines.ToArray());
                    string[] fileContent = System.IO.File.ReadAllLines(FilePath);
                    if (fileContent.Count() > 0)
                        for (int i = 0; i < fileContent.Count(); i++)
                        {

                            string[] LineData = fileContent[i].Split(',');
                            int l_str_length = LineData.Length;

                            if (LineData[0].ToUpper().Equals("HDR940"))
                            {

                                if (l_str_length != 21)
                                {
                                    if (l_str_length < 21)
                                    {
                                        l_str_error_desc = "Extra Commas Required(less than 21)";
                                    }
                                    else
                                    {
                                        l_str_error_desc = "Additional Commas Exist(More than 21)";

                                    }
                                    l_str_error_msg = LineData[1] + ", " + LineData[2] + ", " + LineData[3] + " (" + LineData[4] + "), " + LineData[5] + ", " +
                                                                LineData[6] + ", " + LineData[7] + ", " + LineData[8] + ", " + LineData[9] + ", " + LineData[10] + ", " + LineData[11] + "," +
                                                                LineData[12] + ", " + LineData[13] + ", " + LineData[14] + ", " + LineData[15] + ", " + LineData[16] + ", " + LineData[17] + "," +
                                    LineData[18];
                                    //   return Json(l_str_error_msg, JsonRequestBehavior.AllowGet);
                                    eComSR940 hdr_error = new eComSR940();
                                    hdr_error.File_Line_No = i + 1;
                                    hdr_error.Header_Info = l_str_error_msg;
                                    hdr_error.File_Length = l_str_error_desc;
                                    error_lst.Add(hdr_error);
                                    Session["error_lst"] = error_lst;

                                }
                            }
                            if (LineData[1] != null && LineData[2] != null && LineData[4] != null && LineData[7] != null && LineData[8] != null)
                            {
                                if (LineData[1].ToLower().Equals(strCmpId.ToLower()))
                                {
                                    if (LineData[0].ToUpper().Equals("HDR940"))
                                    {
                                        #region "Header Line"
                                        tbl_so_hdr hdr_itm = new tbl_so_hdr();
                                        hdr_itm.CompID = LineData[1];
                                        hdr_itm.BatchNo = LineData[2];
                                        objeComSR940.cmp_id = hdr_itm.CompID;
                                        objeComSR940.BatchNo = hdr_itm.BatchNo;
                                        Session["BatchNo"] = hdr_itm.BatchNo;
                                        hdr_itm.CustName = LineData[3];
                                        hdr_itm.CustID = LineData[4];
                                        hdr_itm.Store = LineData[5];
                                        hdr_itm.Dept = LineData[6];
                                        hdr_itm.CustPO = LineData[7];
                                        hdr_itm.SOID = LineData[8];
                                        hdr_itm.RelID = LineData[9];

                                        try { hdr_itm.ReqDt = Utility.ConvertToDateTime(LineData[10]); }
                                        catch { hdr_itm.ReqDt = DateTime.Now; }

                                        try { hdr_itm.StartDt = Utility.ConvertToDateTime(LineData[11]); }
                                        catch { hdr_itm.StartDt = DateTime.Now; }

                                        try { hdr_itm.CancelDt = Utility.ConvertToDateTime(LineData[12]); }
                                        catch { hdr_itm.CancelDt = DateTime.Now; }
                                        objeComSR940.CompID = hdr_itm.CompID;
                                        objeComSR940.cmp_id = hdr_itm.CompID;
                                        objeComSR940.CustID = hdr_itm.CustID;
                                        objeComSR940.CustPO = hdr_itm.CustPO;
                                        objeComSR940.SOID = hdr_itm.SOID;

                                        // in need of kit_id=itm_code and pack_id='1'
                                        objeComSR940 = ServiceObjecteComSR940.CheckExistSalesOrder(objeComSR940);
                                        if (objeComSR940.so_num != null)
                                        {
                                            hdr_ok = true;
                                            hdr_itm.HeaderInfo = hdr_itm.CompID.Trim() + ", " + hdr_itm.BatchNo.Trim() + ", " + hdr_itm.CustID.Trim() + " (" + hdr_itm.CustName.Trim() + "), " + hdr_itm.CustPO.Trim() + ", " +
                                                                hdr_itm.SOID.Trim() + ", " + hdr_itm.RelID + ", " + hdr_itm.Store + ", " + hdr_itm.Dept + ", " + hdr_itm.ReqDt.ToShortDateString() + ", " + hdr_itm.StartDt.ToShortDateString();

                                            try { hdr_itm.DtlCount = Utility.ConvertToInt32(LineData[13]); }
                                            catch { hdr_itm.DtlCount = 0; }

                                            try { hdr_itm.ShipVia = LineData[14]; }
                                            catch { hdr_itm.ShipVia = ""; }

                                            try { hdr_itm.ShipName = LineData[15]; }
                                            catch { hdr_itm.ShipName = ""; }

                                            try { hdr_itm.ShipAdd1 = LineData[16]; }
                                            catch { hdr_itm.ShipAdd1 = ""; }

                                            try { hdr_itm.ShipAdd2 = LineData[17]; }
                                            catch { hdr_itm.ShipAdd2 = ""; }

                                            try { hdr_itm.City = LineData[18]; }
                                            catch { hdr_itm.City = ""; }

                                            try { hdr_itm.State = LineData[19]; }
                                            catch { hdr_itm.State = ""; }

                                            try { hdr_itm.ZipCode = LineData[20]; }
                                            catch { hdr_itm.ZipCode = ""; }

                                            try { hdr_itm.NoteHdr = LineData[21]; }
                                            catch { hdr_itm.NoteHdr = ""; }
                                        }
                                        else
                                        {
                                            #region "Duplicated Data"
                                            hdr_itm.HeaderInfo = "Duplicated Data: " + hdr_itm.CompID.Trim() + ", CustID: " + hdr_itm.CustID.Trim() + ", CustPO: " + hdr_itm.CustPO.Trim() + ", SO #: " + hdr_itm.SOID.Trim();
                                            hdr_ok = false;
                                            #endregion "Duplicated Data"
                                        }

                                        //  hdr_itm.CreatedBy = Class.SessionHelper.UAccount.username;
                                        hdr_itm.CreatedOn = DateTime.Now;


                                        hdr_lst.Add(hdr_itm);
                                        Session["hdr_lst"] = hdr_lst;

                                        #endregion "Header Line"
                                    }
                                    else // dtl line
                                    {
                                        #region "Detail Line"
                                        double l_dou_StyleQty = 0;
                                        tbl_so_dtl dtl_itm = new tbl_so_dtl();
                                        dtl_itm.StyleStatus = 0;
                                        dtl_itm.CompID = LineData[1];
                                        dtl_itm.BatchNo = LineData[2];
                                        dtl_itm.CustID = LineData[3];
                                        dtl_itm.CustPO = LineData[4];
                                        dtl_itm.POLine = Convert.ToInt32(Convert.ToDouble(LineData[5]));

                                        try { dtl_itm.Style = LineData[6]; }
                                        catch { dtl_itm.Style = ""; }

                                        try { dtl_itm.CustSKU = LineData[7]; }
                                        catch { dtl_itm.CustSKU = ""; }

                                        //string l_str_style_qty = string.Empty;
                                        //l_str_style_qty = LineData[8];
                                        //string str_style_qty = string.Empty;
                                        //int k = l_str_style_qty.IndexOf(".")+1;
                                        //string str = str_style_qty.Substring(0, k-1);
                                        try { dtl_itm.StyleQty = Utility.getInt(LineData[8].ToString()); }

                                        catch { dtl_itm.StyleQty = 0; }

                                        try
                                        {
                                            double dblCtn = 0.0;
                                            dblCtn = Math.Ceiling(Utility.ConvertToDouble(LineData[9].ToString()));

                                            dtl_itm.StyleCarton = (int)dblCtn;
                                        }
                                        catch { dtl_itm.StyleCarton = 0; }

                                        try { dtl_itm.StylePPK = LineData[10]; }
                                        catch { dtl_itm.StylePPK = "0"; }

                                        try { dtl_itm.StyleCube = Utility.ConvertToDouble(LineData[11].ToString()); }
                                        catch { dtl_itm.StyleCube = 0; }

                                        try { dtl_itm.StyleWgt = Utility.ConvertToDouble(LineData[12].ToString()); }
                                        catch { dtl_itm.StyleWgt = 0; }

                                        try { dtl_itm.StyleDesc = LineData[13]; }
                                        catch { dtl_itm.StyleDesc = ""; }

                                        if (hdr_ok)
                                        {
                                            if (dtl_itm.Style != null)
                                            {
                                                String threePL_Item_Code = "";
                                                objeComSR940.cmp_id = dtl_itm.CompID;
                                                objeComSR940.Style = dtl_itm.Style;
                                                objeComSR940.Itm_Code = threePL_Item_Code;
                                                objeComSR940.StyleDesc = dtl_itm.StyleDesc; //Added by Rk to pass Style descryption

                                                // in need of kit_id=itm_code and pack_id='1'
                                                objeComSR940 = ServiceObjecteComSR940.CheckExistSR940Style(objeComSR940);
                                                objeComSR940.Itm_Code = objeComSR940.Itm_Code;
                                                if (objeComSR940.Itm_Code != null)
                                                {
                                                    dtl_itm.Itm_Code = objeComSR940.Itm_Code;
                                                    if (Convert.ToDouble(dtl_itm.StyleQty) <= 0)
                                                    {
                                                        dtl_itm.StyleStatus = 2;
                                                        dtl_itm.StatusDesc = "Style Quantity is not valid...";
                                                    }
                                                    if (Convert.ToDouble(dtl_itm.StyleCarton) <= 0)
                                                    {
                                                        dtl_itm.StyleStatus = 3;
                                                        dtl_itm.StatusDesc = "Style Carton is not valid...";
                                                    }
                                                    if (Convert.ToDouble(dtl_itm.StylePPK) <= 0)
                                                    {
                                                        dtl_itm.StyleStatus = 4;
                                                        dtl_itm.StatusDesc = "Style PPK is not valid...";
                                                    }

                                                }
                                                else
                                                {
                                                    dtl_itm.StyleStatus = 1;
                                                    dtl_itm.StatusDesc = "Style Does not Exist in the master item table..";
                                                }
                                            }
                                            else
                                            {
                                                dtl_itm.StyleStatus = 1;
                                                dtl_itm.StatusDesc = "Style is missing";
                                            }
                                        }
                                        else
                                        {
                                            dtl_itm.StyleStatus = 5;
                                            dtl_itm.StatusDesc = "Data already loaded (Duplicated) ";
                                        }

                                        if (dtl_itm.StyleStatus != 0)
                                        {
                                            dtl_itm.StyleDesc = dtl_itm.StatusDesc + "-" + dtl_itm.StyleDesc;
                                        }
                                        dtl_lst.Add(dtl_itm);
                                        Session["dtl_lst"] = dtl_lst;
                                        #endregion "Detail Line"
                                    }
                                }
                                else
                                {
                                    #region "Company mismatch"
                                    tbl_so_hdr hdr_itm = new tbl_so_hdr();
                                    hdr_itm.CompID = LineData[1];
                                    hdr_itm.BatchNo = LineData[2];
                                    hdr_itm.CustID = LineData[4];
                                    hdr_itm.CustPO = LineData[7];
                                    hdr_itm.HeaderInfo = "Company Selected: " + strCmpId + " and Company in the file: " + hdr_itm.CompID.Trim() + " please fix this mismatch";
                                    hdr_lst.Add(hdr_itm);
                                    tbl_so_dtl dtl_itm = new tbl_so_dtl();
                                    dtl_itm.CompID = LineData[1];
                                    dtl_itm.BatchNo = LineData[2];
                                    dtl_itm.CustID = LineData[3];
                                    dtl_itm.CustPO = LineData[4];
                                    dtl_itm.StyleStatus = 1;
                                    dtl_itm.StyleDesc = "Company Mismatch need to be fixed!!";
                                    dtl_lst.Add(dtl_itm);
                                    #endregion "Company mismatch"
                                }
                            }

                        }
                }
                else
                {
                    l_str_error_msg = "Invalid File Format";

                }
            }

            catch (Exception ex)
            {
                l_str_error_msg = ex.InnerException.ToString();
            }
            //{ throw ex; }
        }

        private object CreateGridTable(List<tbl_so_hdr> hdrList, List<tbl_so_dtl> dtlList)
        {
            List<tbl_shipreq_grid> gridList = new List<tbl_shipreq_grid>();
            try
            {
                if (hdrList.Count > 0)
                {
                    foreach (tbl_so_hdr itemhdr in hdrList.OrderBy(y => y.HeaderInfo).ToList())
                    {
                        foreach (tbl_so_dtl itemdtl in dtlList.Where(x => x.CompID == itemhdr.CompID && x.BatchNo == itemhdr.BatchNo && x.CustID == itemhdr.CustID && x.CustPO == itemhdr.CustPO).ToList())
                        {
                            tbl_shipreq_grid gridItem = new tbl_shipreq_grid();
                            gridItem.HeaderInfo = itemhdr.HeaderInfo;
                            gridItem.CompID = itemhdr.CompID;
                            gridItem.BatchNo = itemhdr.BatchNo;
                            gridItem.CustID = itemhdr.CustID;
                            gridItem.CustName = itemhdr.CustName;
                            gridItem.Store = itemhdr.Store;
                            gridItem.Dept = itemhdr.Dept;
                            gridItem.CustPO = itemhdr.CustPO;
                            gridItem.SOID = itemhdr.SOID;
                            gridItem.RelID = itemhdr.RelID;
                            gridItem.ReqDt = itemhdr.ReqDt;
                            gridItem.StartDt = itemhdr.StartDt;
                            gridItem.CancelDt = itemhdr.CancelDt;
                            gridItem.DtlCount = itemhdr.DtlCount;
                            gridItem.ShipVia = itemhdr.ShipVia;
                            gridItem.ShipName = itemhdr.ShipName;
                            gridItem.ShipAdd1 = itemhdr.ShipAdd1;
                            gridItem.ShipAdd2 = itemhdr.ShipAdd2;
                            gridItem.City = itemhdr.City;
                            gridItem.State = itemhdr.State;
                            gridItem.ZipCode = itemhdr.ZipCode;
                            gridItem.NoteHdr = itemhdr.NoteHdr;
                            gridItem.POLine = itemdtl.POLine;
                            gridItem.Style = itemdtl.Style;
                            gridItem.Itm_Code = itemdtl.Itm_Code;
                            gridItem.CustSKU = itemdtl.CustSKU;
                            gridItem.StyleQty = itemdtl.StyleQty;
                            gridItem.StyleCarton = itemdtl.StyleCarton;
                            gridItem.StylePPK = itemdtl.StylePPK;
                            gridItem.StyleCube = itemdtl.StyleCube;
                            gridItem.StyleWgt = itemdtl.StyleWgt;
                            gridItem.StyleDesc = itemdtl.StyleDesc;
                            gridItem.StyleStatus = itemdtl.StyleStatus;
                            gridItem.StatusDesc = itemdtl.StatusDesc;
                            gridItem.CreatedBy = itemhdr.CreatedBy;
                            gridItem.CreatedOn = itemhdr.CreatedOn;
                            gridList.Add(gridItem);
                        }

                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return gridList.Count > 0 ? Utility.ConvertListToDataTable(gridList) : null;
        }
        protected string Save940ShipRequest(string result)
        {
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            List<tbl_shipreq_grid> rptSaveList = new List<tbl_shipreq_grid>();
            List<tbl_shipreq_grid> rptExcpList = new List<tbl_shipreq_grid>();
            List<tbl_so_hdr> tbl_so_hdr = new List<tbl_so_hdr>();
            tbl_so_hdr = Session["hdr_lst"] as List<tbl_so_hdr>;

            hdr_lst = tbl_so_hdr;
            List<tbl_so_dtl> tbl_so_dtl = new List<tbl_so_dtl>();
            tbl_so_dtl = Session["dtl_lst"] as List<tbl_so_dtl>;
            dtl_lst = tbl_so_dtl;
            try
            {
                foreach (tbl_so_hdr itemhdr in hdr_lst.OrderBy(y => y.HeaderInfo).ToList())
                {
                    double tmp_TtalQTY = 0;
                    Boolean saving_OK = true;
                    List<tbl_shipreq_grid> breakList = new List<tbl_shipreq_grid>();
                    foreach (tbl_so_dtl itemdtl in dtl_lst.Where(x => x.CompID == itemhdr.CompID && x.BatchNo == itemhdr.BatchNo && x.CustID == itemhdr.CustID && x.CustPO == itemhdr.CustPO).OrderBy(y => y.POLine).ToList())
                    {
                        Session["dtl_list"] = itemdtl;

                        tbl_shipreq_grid gridItem = SetData(itemhdr, itemdtl);
                        if (saving_OK)
                        {
                            saving_OK = gridItem.StyleStatus == 0;

                            if (saving_OK) breakList.Add(gridItem);
                            else rptExcpList.Add(gridItem);

                            tmp_TtalQTY += Convert.ToDouble(gridItem.StyleQty);
                            // tmp_TtalQTY += Convert.ToDouble(gridItem.StyleQty);

                        }
                        else rptExcpList.Add(gridItem);
                    }
                    if (saving_OK)
                    {
                        SaveShipRequest(itemhdr, ref breakList);
                        rptSaveList.AddRange(breakList);
                    }
                }
                Print_Result_List(rptSaveList, rptExcpList);
                result = "Success";
                return result;
            }
            catch (Exception Ex)
            {
                result = "Fail";
                return result;
            }
        }
        public static void SaveShipRequest(tbl_so_hdr hdr_data, ref List<tbl_shipreq_grid> dtl_data)
        {
            String New_SO = "";
            string l_str_reqdate = "";
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            try
            {
                //itemhdr.TtalQty = tmp_TtalQTY;
                objeComSR940.CompID = hdr_data.CompID;
                objeComSR940.BatchNo = hdr_data.BatchNo;
                objeComSR940.CustID = hdr_data.CustID;
                objeComSR940.CustName = hdr_data.CustName;
                objeComSR940.Store = hdr_data.Store;
                objeComSR940.Dept = hdr_data.Dept;
                objeComSR940.SOID = hdr_data.SOID;
                objeComSR940.RelID = hdr_data.RelID;
                //if (reqdate == "")
                //{
                //    objeComSR940.ReqDt = DBNull;
                //}
                //else {
                //    objeComSR940.ReqDt = hdr_data.ReqDt;
                //}
                //if (l_str_reqdate == "")
                //{
                //    objeComSR940.ReqDt = Convert.ToDateTime(l_str_reqdate); 
                //}
                //else
                //{
                //    objeComSR940.ReqDt = objeComSR940.ReqDt;
                //}
                objeComSR940.ReqDt = hdr_data.ReqDt;
                objeComSR940.StartDt = hdr_data.StartDt;
                objeComSR940.CancelDt = hdr_data.CancelDt;

                objeComSR940.dtl_Count = dtl_data.Count();
                objeComSR940.TtalQty = hdr_data.TtalQty;
                objeComSR940.ShipName = hdr_data.ShipName;
                objeComSR940.ShipVia = hdr_data.ShipVia;
                objeComSR940.ShipAdd1 = hdr_data.ShipAdd1;
                objeComSR940.ShipAdd2 = hdr_data.ShipAdd2;
                objeComSR940.City = hdr_data.City;
                objeComSR940.State = hdr_data.State;
                objeComSR940.ZipCode = hdr_data.ZipCode;
                objeComSR940.NoteHdr = hdr_data.NoteHdr;
                objeComSR940.CreatedBy = hdr_data.CreatedBy;
                objeComSR940.CreatedOn = hdr_data.CreatedOn;

                objeComSR940 = ServiceObjecteComSR940.GetSaveShipRequest_hdr(objeComSR940);


                objeComSR940.so_num = objeComSR940.so_num;


                int itm_line = 1;
                foreach (tbl_shipreq_grid dtl_itm in dtl_data)
                {
                    objeComSR940.CompID = dtl_itm.CompID;
                    objeComSR940.BatchNo = dtl_itm.BatchNo;
                    objeComSR940.Store = hdr_data.Store;
                    objeComSR940.Dept = hdr_data.Dept;
                    objeComSR940.ItemLine = dtl_itm.POLine;
                    objeComSR940.Itm_Code = dtl_itm.Itm_Code;
                    objeComSR940.Style = dtl_itm.Style;
                    objeComSR940.StyleQty = dtl_itm.StyleQty;
                    objeComSR940.StyleCarton = dtl_itm.StyleCarton;
                    objeComSR940.StylePPK = dtl_itm.StylePPK;
                    objeComSR940.StyleCube = dtl_itm.StyleCube;
                    objeComSR940.StyleWgt = dtl_itm.StyleWgt;
                    objeComSR940.ReqDt = hdr_data.ReqDt;
                    objeComSR940.StartDt = hdr_data.StartDt;
                    objeComSR940.CancelDt = hdr_data.CancelDt;
                    objeComSR940.cust_ordr_num = hdr_data.CustPO;
                    objeComSR940.CustID = hdr_data.CustID;
                    objeComSR940.CustSKU = dtl_itm.CustSKU;
                    objeComSR940.StyleDesc = dtl_itm.StyleDesc;
                    objeComSR940 = ServiceObjecteComSR940.GetSaveeComSR940Details(objeComSR940);
                    objeComSR940 = ServiceObjecteComSR940.GetSaveeComSR940TempDetails(objeComSR940);
                    //dtl_itm.so_num = objeComSR940.so_num;
                    //DALMsSQL2.SaveShipRequest_dtl(hdr_data, dtl_itm, Convert.ToInt32(Convert.ToDouble(dtl_itm.POLine)));
                    itm_line += 1;
                }


            }


            catch (Exception ex) { throw ex; }
            finally { }

        }
        protected tbl_shipreq_grid SetData(tbl_so_hdr hdr_itm, tbl_so_dtl itemdtl)
        {
            tbl_shipreq_grid gridItem = new tbl_shipreq_grid();
            gridItem.HeaderInfo = hdr_itm.HeaderInfo;
            gridItem.CompID = hdr_itm.CompID;
            gridItem.BatchNo = hdr_itm.BatchNo;
            gridItem.so_num = hdr_itm.CustPO.Trim();
            gridItem.CustID = hdr_itm.CustID;
            gridItem.CustName = hdr_itm.CustName;
            gridItem.Store = hdr_itm.Store;
            gridItem.Dept = hdr_itm.Dept;
            gridItem.CustPO = hdr_itm.CustPO;
            gridItem.SOID = hdr_itm.SOID;
            gridItem.RelID = hdr_itm.RelID;
            gridItem.ReqDt = hdr_itm.ReqDt;
            gridItem.StartDt = hdr_itm.StartDt;
            gridItem.CancelDt = hdr_itm.CancelDt;
            gridItem.DtlCount = hdr_itm.DtlCount;
            gridItem.ShipVia = hdr_itm.ShipVia;
            gridItem.ShipName = hdr_itm.ShipName;
            gridItem.ShipAdd1 = hdr_itm.ShipAdd1;
            gridItem.ShipAdd2 = hdr_itm.ShipAdd2;
            gridItem.City = hdr_itm.City;
            gridItem.State = hdr_itm.State;
            gridItem.ZipCode = hdr_itm.ZipCode;
            gridItem.NoteHdr = hdr_itm.NoteHdr;
            gridItem.POLine = itemdtl.POLine;
            gridItem.Style = itemdtl.Style;
            gridItem.Itm_Code = itemdtl.Itm_Code;
            gridItem.CustSKU = itemdtl.CustSKU;
            gridItem.StyleQty = itemdtl.StyleQty;
            gridItem.StyleCarton = itemdtl.StyleCarton;
            gridItem.StylePPK = itemdtl.StylePPK;
            gridItem.StyleCube = itemdtl.StyleCube;
            gridItem.StyleWgt = itemdtl.StyleWgt;
            gridItem.StyleDesc = itemdtl.StyleDesc;
            gridItem.StyleStatus = itemdtl.StyleStatus > 0 ? 1 : 0;
            gridItem.StatusDesc = itemdtl.StatusDesc;
            gridItem.CreatedBy = "";/*Class.SessionHelper.UAccount.username;*/
            gridItem.CreatedOn = DateTime.Now;
            return gridItem;
        }
        public ActionResult SaveEComSR940(string p_str_cmp_id, string p_str_file_name)
        {
            string result = string.Empty;
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            objeComSR940.error_mode = false;
            objeComSR940.cmp_id = p_str_cmp_id;
            objeComSR940 = ServiceObjecteComSR940.DeleteTempTable(objeComSR940);
            Save940ShipRequest(result);
            objeComSR940.cmp_id = p_str_cmp_id;
            objeComSR940.file_name = p_str_file_name;
            objeComSR940.uploaded_date = DateTime.Now;
            objeComSR940.process_status = "PROCESS";
            objeComSR940.error_desc = "-";
            objeComSR940.user_id = Session["UserID"].ToString().Trim();
            if (Session["error_lst_count"] != null|| result=="Fail")
            {
                objeComSR940.error_mode = true;
                Mapper.CreateMap<eComSR940, eComSR940Model>();
                eComSR940Model EComSR940Model = Mapper.Map<eComSR940, eComSR940Model>(objeComSR940);
                return Json(objeComSR940.error_mode, JsonRequestBehavior.AllowGet);
            }
            objeComSR940 = ServiceObjecteComSR940.AddECom940SRUploadFileNAme(objeComSR940);



            string path = Path.Combine(Server.MapPath("~/uploads"), p_str_file_name);
            string path2 = Path.Combine(Server.MapPath("~/uploads/zProcessed940"), p_str_file_name);
            DirectoryInfo dirInfo = new DirectoryInfo(path2);
            if (!System.IO.File.Exists(path2))
            {
                System.IO.File.Move(path, path2);
            }
            else
            {
                string l_str_FileNameOnly = p_str_file_name.Substring(0, p_str_file_name.LastIndexOf("."));
                //string l_str_Fpath = "zProcessed940";
                //string path3 = Path.Combine(Server.MapPath("~/uploads/zProcessed940"), l_str_FileNameOnly+"-" + DateTime.Now.ToString("yyyyMMddTHHmmss") +".csv");
                //string path4 = Path.Combine(Server.MapPath("~/uploads/"), l_str_Fpath);
                System.IO.File.Delete(path2);
                System.IO.File.Move(path, path2);
            }
            Session["hdr_lst"] = "";
            Session["dtl_lst"] = "";
            Mapper.CreateMap<eComSR940, eComSR940Model>();
            eComSR940Model eComSR940Model = Mapper.Map<eComSR940, eComSR940Model>(objeComSR940);
            return Json(eComSR940Model, JsonRequestBehavior.AllowGet);
            //  return View(eComSR940Model);
        }

        public ActionResult CheckExist940SRUploadFile(string p_str_cmp_id, string p_str_file_name)
        {
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            bool l_bl_count = false;
            objeComSR940.cmp_id = p_str_cmp_id;
            objeComSR940.file_name = p_str_file_name;
            objeComSR940.user_id = Session["UserID"].ToString().Trim();
            objeComSR940 = ServiceObjecteComSR940.CheckExistSRUploadFile(objeComSR940);
            if (objeComSR940.ListeCom940SRUploadDtl.Count() == 1)
            {
                if (objeComSR940.ListeCom940SRUploadDtl[0].SR_UPLOAD_FILE_COUNT == 0)
                {
                    l_bl_count = false;
                }
                else
                {
                    l_bl_count = true;
                }
            }
            else
            {
                l_bl_count = false;
            }


            Mapper.CreateMap<eComSR940, eComSR940Model>();
            eComSR940Model eComSR940Model = Mapper.Map<eComSR940, eComSR940Model>(objeComSR940);
            return Json(l_bl_count, JsonRequestBehavior.AllowGet);
            //  return View(eComSR940Model);
        }
        public ActionResult ShowReport(string p_str_cmpid, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_so_num = string.Empty;
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }

            List<eComSR940> tbl_so_dtl_rpt = new List<eComSR940>();
            tbl_so_dtl_rpt = Session["dtl_list_rpt"] as List<eComSR940>;
         
            try
            {
                if (isValid)
                {

                    strReportName = "ShipReq940.rpt";
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                    using (ReportDocument rd = new ReportDocument())
                    { 
                        if (type == "PDF")
                    {
                        objeComSR940.cmp_id = p_str_cmpid;
                        objeComSR940.BatchNo = Session["BatchNo"].ToString().Trim();


                        objeComSR940 = ServiceObjecteComSR940.ECom940SRUploadRpt(objeComSR940);
                        var rptSource = objeComSR940.ListeCom940SRUploadDtlRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objeComSR940.ListeCom940SRUploadDtlRpt.Count();
                        objCompany.cmp_id = p_str_cmpid.Trim();
                        objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                        objeComSR940.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                        objeComSR940.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                        rd.SetParameterValue("fml_image_path", objeComSR940.Image_Path);
                        rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                        rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                        rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";

                        rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "940ShipRequestUpload");
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
        public FileResult SampleTemplatedownload()
        {
            return File("~\\templates\\OB_ECOM_FULL_UPLOAD_TEMPLATE_WITH_SAMPLE.xlsx", "text/xlsx", string.Format("OB_ECOM_FULL_UPLOAD_TEMPLATE_WITH_SAMPLE-{0}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }
        private void Print_Result_List(List<tbl_shipreq_grid> OKList, List<tbl_shipreq_grid> NoOKList)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            List<eComSR940> li = new List<eComSR940>();
            //ReportDocument rd = new ReportDocument();
            try
            {

                //RYap2.ReportType rptformat = RYap2.ReportType.PDF;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                List<DataTable> dtLst = new List<DataTable>();
                List<DataSet> dsLst = new List<DataSet>();
                //List<CommonLibrary2.Rhajjie2.Reports.Report> myList = new List<CommonLibrary2.Rhajjie2.Reports.Report>();
                //CommonLibrary2.Rhajjie2.Reports.Report report = new CommonLibrary2.Rhajjie2.Reports.Report();
                string selformfinal = "";
                System.Text.StringBuilder selectionformula = new System.Text.StringBuilder();
                System.Text.StringBuilder formulaFields = new System.Text.StringBuilder();
                System.Text.StringBuilder formulaVal = new System.Text.StringBuilder();
                System.Text.StringBuilder paramFields = new System.Text.StringBuilder();
                System.Text.StringBuilder paramVal = new System.Text.StringBuilder();

                strReportName = "ShipReq940.rpt";

                string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                DataTable dtRpt = new DataTable();
                DataSet dsRpt = new DataSet();
                if (OKList != null && OKList.Count > 0)
                {
                    dt = Utility.ConvertListToDataTable(OKList.OrderBy(y => y.so_num).ToList());
                    li = (from DataRow dr in dt.Rows
                          select new eComSR940()
                          {
                              HeaderInfo = dr["HeaderInfo"].ToString(),
                              CompID = dr["CompID"].ToString(),
                              BatchNo = dr["BatchNo"].ToString(),
                              CustID = dr["CustID"].ToString(),
                              CustName = dr["CustName"].ToString(),
                              Store = dr["Store"].ToString(),
                              Dept = dr["Dept"].ToString(),
                              CustPO = dr["CustPO"].ToString(),
                              SOID = dr["SOID"].ToString(),
                              so_num = dr["so_num"].ToString(),
                              RelID = dr["RelID"].ToString(),
                              ReqDt = Convert.ToDateTime(dr["ReqDt"].ToString()),
                              StartDt = Convert.ToDateTime(dr["StartDt"].ToString()),
                              CancelDt = Convert.ToDateTime(dr["CancelDt"].ToString()),
                              DtlCount = Convert.ToInt32(dr["DtlCount"].ToString()),
                              ShipVia = dr["ShipVia"].ToString(),
                              ShipName = dr["ShipName"].ToString(),
                              ShipAdd1 = dr["ShipAdd1"].ToString(),
                              ShipAdd2 = dr["ShipAdd2"].ToString(),
                              City = dr["City"].ToString(),
                              State = dr["State"].ToString(),
                              ZipCode = dr["ZipCode"].ToString(),
                              NoteHdr = dr["NoteHdr"].ToString(),
                              POLine = Convert.ToInt32(dr["POLine"].ToString()),
                              Style = dr["Style"].ToString(),
                              Itm_Code = dr["Itm_Code"].ToString(),
                              CustSKU = dr["CustSKU"].ToString(),
                              StyleQty = Convert.ToInt32(dr["StyleQty"].ToString()),
                              StyleCarton = Convert.ToInt32(dr["StyleCarton"].ToString()),
                              StylePPK = dr["StylePPK"].ToString(),
                              StyleCube = Convert.ToDouble(dr["StyleCube"].ToString()),
                              StyleWgt = Convert.ToDouble(dr["StyleWgt"].ToString()),
                              StyleDesc = dr["StyleDesc"].ToString(),
                              StyleStatus = Convert.ToInt32(dr["StyleStatus"].ToString()),
                              StatusDesc = dr["StatusDesc"].ToString(),
                              CreatedBy = dr["CreatedBy"].ToString(),
                              CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString()),
                              TtalQty = Convert.ToInt32(dr["TtalQty"].ToString()),

                          }).ToList();
                    Session["dtl_list_rpt"] = li;
                    objeComSR940.ListeCom940SRUploadDtlRpt = li;


                }

                if (NoOKList != null && NoOKList.Count > 0)
                {
                    dt = Utility.ConvertListToDataTable(NoOKList.OrderBy(y => y.HeaderInfo).ToList());

                    //formulaFields = new System.Text.StringBuilder();
                    //formulaVal = new System.Text.StringBuilder();
                    //formulaFields.Append(@"rptTitle");
                    //formulaVal.Append("940 Ship Request Exception Data");
                    //report = Class.Common.GetReportDetails(RYap2.GetCurrentPageName(), rptformat, rptname, true, dt, dtLst, dsLst, Class.Common.rptLoc.ToString().Trim(),
                    //                    "", selformfinal.Trim(), formulaFields.ToString().Trim(), formulaVal.ToString().Trim(),
                    //                    paramFields.ToString().Trim(), paramVal.ToString().Trim(), cboCompany.SelectedValue, Class.SessionHelper.UAccount.username.Trim());
                    //if (report != null)
                    //    myList.Add(report);
                }


            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }
            finally
            {
            }

        }
        public ActionResult ErrorDescCount()
        {
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            string l_str_error_desc = "";

            if (Session["error_lst_count"] != null)
            {
                l_str_error_desc = "true";
                // objeComSR940.error_desc = ex.Message;
            }




            return Json(l_str_error_desc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EComSR940InqDetail(string p_str_cmp_id, string p_str_file_name, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_batch_id)
        {
            try
            {
                string l_str_So_num = string.Empty;
                eComSR940 objeComSR940 = new eComSR940();
                eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
                Session["g_str_Search_flag"] = "True";
                objeComSR940.cmp_id = p_str_cmp_id.Trim();
                if(p_str_file_name != null && p_str_file_name != "")
                {
                    if (p_str_file_name.Contains(".csv"))
                    {
                        objeComSR940.file_name = p_str_file_name.Trim();
                    }
                    else
                    {
                        objeComSR940.file_name = p_str_file_name.Trim()+".csv";
                    }
                }
                else
                {
                    objeComSR940.file_name = p_str_file_name.Trim();
                }
               
                objeComSR940.so_dtFm = p_str_so_dt_frm.Trim();
                objeComSR940.so_dtTo = p_str_so_dt_to.Trim();
                objeComSR940.quote_num = p_str_batch_id.Trim();
                objeComSR940 = ServiceObjecteComSR940.GetEcom940Inq(objeComSR940);
                objeComSR940 = ServiceObjecteComSR940.GetEcom940HdrCount(objeComSR940);
                if (objeComSR940.ListEcomError.Count > 0)
                {
                    objeComSR940.Header_count = objeComSR940.ListEcomError[0].Header_count;
                    l_str_hdrCount = objeComSR940.Header_count;
                }
                Mapper.CreateMap<eComSR940, eComSR940Model>();
                eComSR940Model eComSR940Model = Mapper.Map<eComSR940, eComSR940Model>(objeComSR940);
                return PartialView("_eComSR940", eComSR940Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult Show940SRReport(string var_name, string SelectedID, string p_str_cmpid,string p_str_filename, string p_str_so_dt_frm, string p_str_so_dt_to)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_so_num = string.Empty;
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            List<eComSR940> li = new List<eComSR940>();
            DataTable dtRpt = new DataTable();
            DataSet dsRpt = new DataSet();
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }

            try
            {
                if (isValid)
                {

                    strReportName = "ShipReq940.rpt";

                    using (ReportDocument rd = new ReportDocument())
                    { 
                        string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                    objeComSR940.cmp_id = p_str_cmpid.Trim();
                    if (p_str_filename != null && p_str_filename!= "")
                    {
                        if (p_str_filename.Contains(".csv"))
                        {
                            objeComSR940.file_name = p_str_filename.Trim();
                        }
                        else
                        {
                            objeComSR940.file_name = p_str_filename.Trim() + ".csv";
                        }
                    }
                    else
                    {
                        objeComSR940.file_name = p_str_filename.Trim();
                    }
                    objeComSR940.so_dtFm = p_str_so_dt_frm.Trim();
                    objeComSR940.so_dtTo = p_str_so_dt_to.Trim();
                    objeComSR940.quote_num = SelectedID.Trim();
                    objeComSR940 = ServiceObjecteComSR940.GetEcom940InqRpt(objeComSR940);
                    if (objeComSR940.ListeCom940SRUploadDtlRpt.Count==0)
                    {
                        objeComSR940.ListNo_Of_Records = Session["tbl_rpt_temp_list"] as List<eComSR940>;
                     
                        if (objeComSR940.ListNo_Of_Records != null && objeComSR940.ListNo_Of_Records.Count > 0)
                        {
                            dtRpt = Utility.ConvertListToDataTable(objeComSR940.ListNo_Of_Records.OrderBy(y => y.so_num).ToList());
                            li = (from DataRow dr in dtRpt.Rows
                                  select new eComSR940()
                                  {
                                      cmp_id = dr["CompID"].ToString(),
                                      quote_num = dr["BatchNo"].ToString(),
                                      cust_id = dr["CustID"].ToString(),
                                      cust_name = dr["CustName"].ToString(),
                                      store_id = dr["Store"].ToString(),
                                      dept_id = dr["Dept"].ToString(),
                                      cust_ordr_num = dr["CustPO"].ToString(),
                                      ordr_num = dr["SOID"].ToString(),
                                      so_num = dr["so_num"].ToString(),
                                      RelID = dr["RelID"].ToString(),
                                      req_ship_dt = Convert.ToDateTime(dr["ReqDt"].ToString()),
                                      ship_dt = Convert.ToDateTime(dr["StartDt"].ToString()),
                                      CancelDt = Convert.ToDateTime(dr["CancelDt"].ToString()),
                                      DtlCount = Convert.ToInt32(dr["DtlCount"].ToString()),
                                      line_num = Convert.ToInt32(dr["POLine"].ToString()),
                                      itm_num = dr["Style"].ToString(),
                                      Itm_Code = dr["Itm_Code"].ToString(),
                                      CustSKU = dr["CustSKU"].ToString(),
                                      ordr_qty = Convert.ToInt32(dr["StyleQty"].ToString()),
                                      ordr_ctns = Convert.ToInt32(dr["StyleCarton"].ToString()),
                                      itm_qty = Convert.ToInt32(dr["StylePPK"].ToString()),
                                      cube = Convert.ToDecimal(dr["StyleCube"].ToString()),
                                      wgt = Convert.ToDecimal(dr["StyleWgt"].ToString()),
                                      itm_name = dr["StyleDesc"].ToString(),
                                      StyleStatus = Convert.ToInt32(dr["StyleStatus"].ToString()),
                                      StatusDesc = dr["StatusDesc"].ToString(),
                                      CreatedBy = dr["CreatedBy"].ToString(),
                                      CreatedOn = Convert.ToDateTime(dr["CreatedOn"].ToString()),
                                      TtalQty = Convert.ToInt32(dr["TtalQty"].ToString()),

                                  }).ToList();

                            objeComSR940.ListeCom940SRUploadDtlRpt = li;
                        }
                        var rptSource = objeComSR940.ListeCom940SRUploadDtlRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objeComSR940.ListeCom940SRUploadDtlRpt.Count();
                        objCompany.cmp_id = p_str_cmpid.Trim();
                        objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                        objeComSR940.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                        if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource);
                    }
                    else
                    {
                        var rptSource1 = objeComSR940.ListeCom940SRUploadDtlRpt.ToList();
                        rd.Load(strRptPath);
                        int AlocCount = 0;
                        AlocCount = objeComSR940.ListeCom940SRUploadDtlRpt.Count();
                        objCompany.cmp_id = p_str_cmpid.Trim();
                        objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                        objeComSR940.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                        if (rptSource1 != null && rptSource1.GetType().ToString() != "System.String")
                            rd.SetDataSource(rptSource1);
                    }
                    
                    objeComSR940.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                    rd.SetParameterValue("fml_image_path", objeComSR940.Image_Path);
                    rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                    rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                    rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "940ShipRequestUpload");

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
        public ActionResult Show940SREmailReport(string var_name, string SelectedID, string p_str_cmpid, string p_str_so_dt_frm, string p_str_so_dt_to)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            string strFromDate = DateTime.Now.AddDays(-30).ToString("MM/DD/YYYY");
            string strToDate = DateTime.Now.ToString("MM/DD/YYYY");
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_so_num = string.Empty;
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;//CR2018-03-07-001 Added By Nithya
            l_str_rpt_selection = var_name;
            eComSR940 objeComSR940 = new eComSR940();
            eComSR940Service ServiceObjecteComSR940 = new eComSR940Service();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            objCustMaster.cust_id = p_str_cmpid;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            try
            {
                if (isValid)
                {
                    if (var_name == "GridSRRpt")
                    {

                        strReportName = "ShipReq940.rpt";
                        using (ReportDocument rd = new ReportDocument())
                        { 
                            string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                            objeComSR940.cmp_id = p_str_cmpid.Trim();
                            objeComSR940.so_dtFm = p_str_so_dt_frm.Trim();
                            objeComSR940.so_dtTo = p_str_so_dt_to.Trim();
                            objeComSR940.quote_num = SelectedID.Trim();
                            objeComSR940 = ServiceObjecteComSR940.GetEcom940InqRpt(objeComSR940);
                            EmailSub = "Ecomm SR940 Summary Report for" + " " + " " + objeComSR940.cmp_id+"_" + objeComSR940.quote_num;
                            EmailMsg = "Ecomm SR940 Summary Report hasbeen Attached for the Process";
                            var rptSource = objeComSR940.ListeCom940SRUploadDtlRpt.ToList();
                            rd.Load(strRptPath);
                            int AlocCount = 0;
                            AlocCount = objeComSR940.ListeCom940SRUploadDtlRpt.Count();
                            objCompany.cmp_id = p_str_cmpid.Trim();
                            objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                            objeComSR940.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                            if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                                rd.SetDataSource(rptSource);
                            objeComSR940.Image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objeComSR940.Image_Path);
                            rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                            rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objeComSR940.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//Ecomm SR940Summary__" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        }
                        reportFileName = "Ecomm_SR940Summary Report_" + strDateFormat + "_" + objeComSR940.quote_num+".pdf";//CR2018-03-07-001 Added By Nithya
                        Session["RptFileName"] = strFileName;

                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
                //CR2018-03-07-001 Added By Nithya
                Email objEmail = new Email();
                objEmail.CmpId = p_str_cmpid;
                objEmail.EmailSubject = EmailSub;
                objEmail.EmailMessage = EmailMsg;
                EmailService objEmailService = new EmailService();
                objEmail = objEmailService.GetSendMailDetails(objEmail);
                if (objEmail.ListEamilDetail.Count != 0)
                {

                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = (objEmail.ListEamilDetail[0].EmailTo.Trim() == null || objEmail.ListEamilDetail[0].EmailTo.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailTo.Trim();
                    objEmail.EmailCC = (objEmail.ListEamilDetail[0].EmailCC.Trim() == null || objEmail.ListEamilDetail[0].EmailCC.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailCC.Trim();

                }
                else
                {
                    objEmail.Attachment = reportFileName;
                    objEmail.EmailTo = "";
                    objEmail.EmailCC = "";
                }
                Mapper.CreateMap<Email, EmailModel>();
                EmailModel EmailModel = Mapper.Map<Email, EmailModel>(objEmail);
                return PartialView("_Email", EmailModel);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            return Json(new
            {
                result = jsonErrorCode,
                err = msg
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}