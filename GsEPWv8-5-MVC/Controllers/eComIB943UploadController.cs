using AutoMapper;
using GsEPWv8_4_MVC.Business.Implementation;
using GsEPWv8_4_MVC.Business.Interface;
using GsEPWv8_4_MVC.Common;
using GsEPWv8_4_MVC.Core.Entity;
using GsEPWv8_4_MVC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_4_MVC.Controllers
{
    public class eComIB943UploadController : Controller
    {
        public List<tbl_ib_doc_hdr> lstIB943UploadFileHdr;
        public List<tbl_ib_doc_dtl> lstIB943UploadFileDtl;
        public List<IB943InvalidData> lstOB940InvalidData;
        public IB943UploadFileInfo objIB943UploadFileInfo;
        public ActionResult List()
        {
            List<OrderVM> allOrder = new List<OrderVM>();
            return View(allOrder);
        }
        public string EmailSub = string.Empty;
        public string EmailMsg = string.Empty;
        public string Folderpath = string.Empty;
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult eComIB943Upload(string FullFillType, string cmp_id, string p_str_scn_id)
        {
            string l_str_cmp_id = string.Empty;
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objeComIB943Upload.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
            objeComIB943Upload.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            if (objeComIB943Upload.cmp_id != "" && FullFillType == null)
            {

                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objeComIB943Upload.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                objeComIB943Upload.cmp_id = objeComIB943Upload.ListCompanyPickDtl[0].cmp_id;

            }
            else
            {
                if (FullFillType == null)
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objeComIB943Upload.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objeComIB943Upload.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

                }
            }
            if (FullFillType != null)
            {
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objeComIB943Upload.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;


            }
            Session["lstIB943UploadFileHdr"] = "";
            Session["lstIB943UploadFileDtl"] = "";
            Session["dtl_list"] = "";
            Session["dtl_list_rpt"] = "";
            Session["lstOB940InvalidData"] = "";
            Session["lstOB940InvalidData_count"] = null;
            Mapper.CreateMap<eComIB943Upload, eComIB943UploadModel>();
            eComIB943UploadModel objeComIB943UploadModel = Mapper.Map<eComIB943Upload, eComIB943UploadModel>(objeComIB943Upload);
            return View(objeComIB943UploadModel);

        }
        public FileResult Sampledownload()
        {
            return File("~\\templates\\IB_ECOM_FULL_UPLOAD_TEMPLATE_WITH_SAMPLE.xlsx", "text/xlsx", string.Format("IB_ECOM_FULL_UPLOAD_TEMPLATE_WITH_SAMPLE-{0}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }
        public ActionResult UploadFiles(string l_str_cmp_id)
        {
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceIB943UploadFile = new eComIB943UploadService();
            List<eComIB943Upload> li = new List<eComIB943Upload>();
         
            Session["dtl_list"] = "";
            Session["dtl_list_rpt"] = "";
            Session["lstOB940InvalidData"] = "";
            Session["lstOB940InvalidData_count"] = null;
            Session["Lessthan"] = null;
            objeComIB943Upload.cmp_id = l_str_cmp_id;

          

            objeComIB943Upload = ServiceIB943UploadFile.DeleteTempTable(objeComIB943Upload);
            if (Request.Files.Count > 0)
            {
                try
                {
                   // objeComIB943Upload.error_mode = false;
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase FileUpload = files[i];
                        string fname;
                        string l_str_error_msg = string.Empty;
                        Session["l_str_error_msg"] = string.Empty;
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
                                string p_str_file_path = Path.Combine(Server.MapPath("~/uploads"), fileName);
                                //Try and upload
                                try
                                {
                                    FileUpload.SaveAs(p_str_file_path);
                                    string l_str_ext = Path.GetExtension(fileName);
                                    if (l_str_ext.ToUpper() != ".CSV")
                                    {
                                        objeComIB943Upload.error_mode = true;
                                        objeComIB943Upload.error_desc = "Invalid File Format";
                                        return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                                    }

                                    Get_List_Data(l_str_cmp_id, p_str_file_path, fileName, ".CSV", ref l_str_error_msg, ref BatchResult);

                                    if (l_str_error_msg != "")
                                    {
                                        objeComIB943Upload.error_mode = true;
                                        objeComIB943Upload.error_desc = l_str_error_msg;
                                        if (Session["Lessthan"] != null)
                                        {
                                            objeComIB943Upload.File_Length = "L";
                                        }
                                        

                                    }

                                    DataTable out_dt = (DataTable)CreateGridTable(lstIB943UploadFileHdr, lstIB943UploadFileDtl);
                                    if (out_dt != null)
                                    {
                                        DataTable SRHdrDtl = new DataTable();
                                        li = (from DataRow dr in out_dt.Rows
                                              select new eComIB943Upload()
                                              {
                                                  HeaderInfo = dr["HeaderInfo"].ToString(),
                                                  cmp_id = dr["cmp_id"].ToString(),
                                                  eta_date = dr["eta_date"].ToString(),
                                                  ref_num = dr["ref_num"].ToString(),
                                                  rcvd_via = dr["rcvd_via"].ToString(),
                                                  rcvd_from = dr["rcvd_from"].ToString(),
                                                  master_bol = dr["master_bol"].ToString(),
                                                  vessel_no = dr["vessel_no"].ToString(),
                                                  hdr_notes = dr["hdr_notes"].ToString(),
                                                  cntr_id = dr["cntr_id"].ToString(),
                                                  dtl_line = Convert.ToInt32(dr["dtl_line"].ToString()),
                                                  po_num = dr["po_num"].ToString(),
                                                  itm_num = (dr["itm_num"].ToString()),
                                                  itm_color = dr["itm_color"].ToString(),
                                                  itm_size = dr["itm_size"].ToString(),
                                                  itm_name = dr["itm_name"].ToString(),
                                                  itm_qty = Convert.ToInt32(dr["itm_qty"].ToString()),
                                                  ctn_qty = Convert.ToInt32(dr["ctn_qty"].ToString()),
                                                  ctns = Convert.ToInt32(dr["ctns"].ToString()),
                                                  loc_id = dr["loc_id"].ToString(),
                                                  st_rate_id = dr["st_rate_id"].ToString(),
                                                  io_rate_id = dr["io_rate_id"].ToString(),
                                                  ctn_length = Convert.ToDecimal(dr["ctn_length"].ToString()),
                                                  ctn_width = Convert.ToDecimal(dr["ctn_width"].ToString()),
                                                  ctn_height = Convert.ToDecimal(dr["ctn_height"].ToString()),
                                                  ctn_cube = Convert.ToDecimal(dr["ctn_cube"].ToString()),
                                                  ctn_wgt = Convert.ToDecimal(dr["ctn_wgt"].ToString()),
                                                  dtl_notes = dr["dtl_notes"].ToString()
                                              }).ToList();

                                        objeComIB943Upload.ListeCom940IB943UploadDtl = li;
                                    }
                                    if(objeComIB943Upload.ListeCom940IB943UploadDtl!=null)
                                    {
                                        Session["tbl_rpt_temp_list"] = objeComIB943Upload.ListeCom940IB943UploadDtl;
                                    }
                                    else
                                    {
                                        objeComIB943Upload.error_mode = true;
                                        objeComIB943Upload.error_desc = "Please Correct and Upload file";
                                        return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                                    }             
                                }
                                catch (Exception ex)
                                {
                                    objeComIB943Upload.error_mode = true;
                                    objeComIB943Upload.error_desc = ex.InnerException.ToString();
                                    return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }


                        else
                        {
                            //Catch errors
                            objeComIB943Upload.error_mode = true;
                            objeComIB943Upload.error_desc = "Please select a file";
                            return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                        }



                    }
                }
                catch (Exception ex)
                {
                    objeComIB943Upload.error_mode = true;
                    objeComIB943Upload.error_desc = ex.Message;
                    return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objeComIB943Upload.error_mode = true;
                objeComIB943Upload.error_desc = "No files selected.";
                return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
            }
            Mapper.CreateMap<eComIB943Upload, eComIB943UploadModel>();
            eComIB943UploadModel objeComIB943UploadModel = Mapper.Map<eComIB943Upload, eComIB943UploadModel>(objeComIB943Upload);
            return PartialView("_eComIB943Upload", objeComIB943UploadModel);
        }
        private void Get_List_Data(string p_str_cmp_id, string p_str_file_path, string p_str_file_name, string p_str_file_extn, ref string l_str_error_msg, ref string BatchResult)
        {
            lstIB943UploadFileHdr = new List<tbl_ib_doc_hdr>();
            lstIB943UploadFileDtl = new List<tbl_ib_doc_dtl>();
            lstOB940InvalidData = new List<IB943InvalidData>();

            Boolean hdr_ok = false;
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceIB943UploadFile = new eComIB943UploadService();
            l_str_error_msg = string.Empty;
            string l_str_error_desc = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_upload_ref_num = string.Empty;
            int l_int_no_of_lines = 0;
       
            try
            {
                l_str_file_name = System.IO.Path.GetFileNameWithoutExtension(p_str_file_path);
                if (p_str_file_extn.ToUpper().Equals(".CSV"))
                {
                    string[] l_str_fileContent = System.IO.File.ReadAllLines(p_str_file_path);

                    List<string> lst_file_line_content = new List<string>(System.IO.File.ReadAllLines(p_str_file_path));
                    int l_int_blank_line = lst_file_line_content.FindIndex(x => x.Trim().Length == 0);
                    if (l_int_blank_line != -1)
                    {
                        while (lst_file_line_content.Count > l_int_blank_line)
                        {
                            lst_file_line_content.RemoveAt(lst_file_line_content.Count - 1);
                        }
                    }
                    System.IO.File.WriteAllLines(p_str_file_path, lst_file_line_content.ToArray());
                    string[] fileContent = System.IO.File.ReadAllLines(p_str_file_path);
                    if (fileContent.Count() > 0)

                        
                    l_str_upload_ref_num = Convert.ToString(ServiceIB943UploadFile.Get943UploadRefNum(p_str_cmp_id));
                    l_int_no_of_lines = lst_file_line_content.Count();

                    objIB943UploadFileInfo = new IB943UploadFileInfo();
                    objIB943UploadFileInfo.cmp_id = p_str_cmp_id;
                    objIB943UploadFileInfo.file_name = p_str_file_name;
                    objIB943UploadFileInfo.upload_by = Session["UserID"].ToString().Trim();
                    objIB943UploadFileInfo.upload_date_time = DateTime.Now;
                    objIB943UploadFileInfo.no_of_lines = l_int_no_of_lines;
                    objIB943UploadFileInfo.status = "PEND";
                    objIB943UploadFileInfo.upload_ref_num = l_str_upload_ref_num;
                    Session["objIB943UploadFileInfo"] = objIB943UploadFileInfo;

                    for (int i = 0; i < fileContent.Count(); i++)
                        {

                            string[] field_data = fileContent[i].Split(',');
                            int l_str_length = field_data.Length;

                            if (field_data[0].ToUpper().Equals("HDR943"))
                            {

                                if (l_str_length != 21)
                                {
                                l_str_error_desc = "Line  " + (i + 1).ToString() + " contains " + (l_str_length + 1).ToString() + " commas. It should be 21 ";


                                l_str_error_msg = field_data[1] + ", " + field_data[2] + ", " + field_data[3] + " (" + field_data[4] + "), " + field_data[5] + ", " +
                                                                field_data[6] + ", " + field_data[7] + ", " + field_data[8] + ", " + field_data[9] + ", " + field_data[10] + ", " + field_data[11] + "," +
                                                                field_data[12] + ", " + field_data[13] + ", " + field_data[14] + ", " + field_data[15] + ", " + field_data[16] + ", " + field_data[17] + "," +
                                    field_data[18];
                                    //   return Json(l_str_error_msg, JsonRequestBehavior.AllowGet);
                                    eComSR940 hdr_error = new eComSR940();
                                    hdr_error.File_Line_No = i + 1;
                                    hdr_error.Header_Info = l_str_error_msg;
                                    hdr_error.File_Length = l_str_error_desc;
                                    //lstOB940InvalidData.Add(hdr_error);
                                    Session["lstOB940InvalidData"] = lstOB940InvalidData;

                                }
                            }

                        else
                        {
                            l_str_error_desc = "Line should starts with SLF940 ";
                        }

                        string[] filename = l_str_file_name.Split('_');
                           
                               
                                    //if (filename[0] == field_data[1] && filename[1] == "IB" && filename[2] == "943" && filename[3] == "MLF" && filename[4] == field_data[2])
                                    //{
                                        if (field_data[1].ToLower().Equals(p_str_cmp_id.ToLower()))
                                        {
                                            if (field_data[0].ToUpper().Equals("HDR943"))
                                            {
                                                #region "Header Line"
                                                tbl_ib_doc_hdr hdr_itm = new tbl_ib_doc_hdr();
                                                hdr_itm.cmp_id = field_data[1];
                                                hdr_itm.cntr_id = field_data[2];
                                                objeComIB943Upload.cmp_id = hdr_itm.cmp_id;
                                                objeComIB943Upload.cntr_id = hdr_itm.cntr_id;
                                                hdr_itm.eta_date = field_data[3];
                                                hdr_itm.ref_num = field_data[4];
                                                hdr_itm.rcvd_via = field_data[5];
                                                hdr_itm.rcvd_from = field_data[6];
                                                hdr_itm.master_bol = field_data[7];
                                                hdr_itm.vessel_no = field_data[8];
                                                hdr_itm.hdr_notes = field_data[9];
                                                objeComIB943Upload.ref_num = hdr_itm.ref_num;
                                                objeComIB943Upload.rcvd_via = hdr_itm.rcvd_via;
                                                objeComIB943Upload.rcvd_from = hdr_itm.rcvd_from;
                                                objeComIB943Upload.master_bol = hdr_itm.master_bol;
                                                objeComIB943Upload.vessel_no = hdr_itm.vessel_no;
                                                objeComIB943Upload.hdr_notes = hdr_itm.hdr_notes;
                                                hdr_itm.HeaderInfo = hdr_itm.cmp_id.Trim() + ", " + "," + hdr_itm.eta_date.Trim() + ", " + hdr_itm.ref_num.Trim() + "," + hdr_itm.rcvd_via.Trim() + "," + hdr_itm.rcvd_from.Trim() + ", " +
                                                                       hdr_itm.master_bol.Trim() + ", " + hdr_itm.vessel_no + ", " + hdr_itm.hdr_notes;
                                                lstIB943UploadFileHdr.Add(hdr_itm);
                                                Session["lstIB943UploadFileHdr"] = lstIB943UploadFileHdr;
                                            }
                                            else // dtl line
                                            {
                                                tbl_ib_doc_dtl dtl_itm = new tbl_ib_doc_dtl();
                                                dtl_itm.cmp_id = field_data[1];
                                                dtl_itm.cntr_id = field_data[2];
                                                dtl_itm.dtl_line = Convert.ToInt32(field_data[3]);
                                                dtl_itm.po_num = field_data[4];
                                                dtl_itm.itm_num = field_data[5];

                                                try { dtl_itm.itm_color = field_data[6]; }
                                                catch { dtl_itm.itm_color = ""; }

                                                try { dtl_itm.itm_size = field_data[7]; }
                                                catch { dtl_itm.itm_size = ""; }
                                                try { dtl_itm.itm_name = field_data[8]; }

                                                catch { dtl_itm.itm_name = ""; }

                                                try
                                                {

                                                    dtl_itm.itm_qty = Convert.ToInt32(field_data[9]);
                                                }
                                                catch { dtl_itm.itm_qty = 0; }

                                                try { dtl_itm.ctn_qty = Convert.ToInt32(field_data[10]); }
                                                catch { dtl_itm.ctn_qty = 0; }

                                                try { dtl_itm.ctns = Convert.ToInt32(field_data[11]); }
                                                catch { dtl_itm.ctns = 0; }

                                                try { dtl_itm.loc_id = field_data[12].ToString(); }
                                                catch { dtl_itm.loc_id = ""; }

                                                try { dtl_itm.st_rate_id = field_data[13].ToString(); }
                                                catch { dtl_itm.st_rate_id = ""; }
                                                try { dtl_itm.io_rate_id = field_data[14].ToString(); }
                                                catch { dtl_itm.io_rate_id = ""; }
                                                try { dtl_itm.ctn_length = Convert.ToDecimal(field_data[15].ToString()); }
                                                catch { dtl_itm.ctn_length = 0; }
                                                try { dtl_itm.ctn_width = Convert.ToDecimal(field_data[16].ToString()); }
                                                catch { dtl_itm.ctn_width = 0; }
                                                try { dtl_itm.ctn_height = Convert.ToDecimal(field_data[17].ToString()); }
                                                catch { dtl_itm.ctn_height = 0; }
                                                try { dtl_itm.ctn_wgt = Convert.ToDecimal(field_data[18].ToString()); }
                                                catch { dtl_itm.ctn_wgt = 0; }
                                                try { dtl_itm.ctn_cube = Convert.ToDecimal(field_data[19].ToString()); }
                                                catch { dtl_itm.ctn_cube = 0; }
                                                try { dtl_itm.dtl_notes = field_data[20].ToString(); }
                                                catch { dtl_itm.dtl_notes = ""; }
                                                hdr_ok = true;
                                                if (hdr_ok)
                                                {
                                                    if (dtl_itm.itm_num != null)
                                                    {
                                                        String threePL_Item_Code = "";
                                                        objeComIB943Upload.cmp_id = dtl_itm.cmp_id;
                                                        objeComIB943Upload.itm_num = dtl_itm.itm_num;
                                                        //  objeComIB943Upload.Itm_Code = threePL_Item_Code;
                                                        objeComIB943Upload.itm_name = dtl_itm.itm_name; //Added by Rk to pass Style descryption

                                                        // in need of kit_id=itm_code and pack_id='1'
                                                        // objeComIB943Upload = ServiceObjecteComSR940.CheckExistSR940Style(objeComIB943Upload);
                                                        objeComIB943Upload.Itm_Code = objeComIB943Upload.Itm_Code;
                                                        if (objeComIB943Upload.Itm_Code != null)
                                                        {

                                                            if (Convert.ToDouble(dtl_itm.itm_qty) <= 0)
                                                            {
                                                                dtl_itm.StyleStatus = 2;
                                                                dtl_itm.StatusDesc = "Style Quantity is not valid...";
                                                            }
                                                            if (Convert.ToDouble(dtl_itm.ctn_qty) <= 0)
                                                            {
                                                                dtl_itm.StyleStatus = 3;
                                                                dtl_itm.StatusDesc = "Style Carton is not valid...";
                                                            }
                                                            if (Convert.ToDouble(dtl_itm.ctns) <= 0)
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
                                                lstIB943UploadFileDtl.Add(dtl_itm);
                                                Session["lstIB943UploadFileDtl"] = lstIB943UploadFileDtl;
                                                #endregion "Detail Line"
                                            }
                                        }
                                        else
                                        {
                                            #region "Company mismatch"
                                            tbl_ib_doc_hdr hdr_itm = new tbl_ib_doc_hdr();
                                            hdr_itm.cmp_id = field_data[1];
                                            hdr_itm.eta_date = field_data[2];
                                            hdr_itm.HeaderInfo = "Company Selected: " + p_str_cmp_id + " and Company in the file: " + hdr_itm.cmp_id.Trim() + " please fix this mismatch";
                                            // lstIB943UploadFileHdr.Add(hdr_itm);
                                            tbl_ib_doc_dtl dtl_itm = new tbl_ib_doc_dtl();
                                            dtl_itm.cmp_id = field_data[1];
                                            dtl_itm.cntr_id = field_data[2];
                                            //dtl_itm.CustID = field_data[3];
                                            //dtl_itm.CustPO = field_data[4];
                                            dtl_itm.StyleStatus = 1;
                                            dtl_itm.StyleDesc = "Company Mismatch need to be fixed!!";
                                            // lstIB943UploadFileDtl.Add(dtl_itm);
                                            #endregion "Company mismatch"
                                        }
                                    //}
                                    //else
                                    //{
                                    //    l_str_error_msg = "Invalid File Name";
                                    //}
                                   
                       
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
        private object CreateGridTable(List<tbl_ib_doc_hdr> hdrList, List<tbl_ib_doc_dtl> dtlList)
        {
            List<tbl_ib_doc_grid> gridList = new List<tbl_ib_doc_grid>();
            try
            {
                if (hdrList.Count > 0)
                {
                    foreach (tbl_ib_doc_hdr itemhdr in hdrList.OrderBy(y => y.HeaderInfo).ToList())
                    {
                        foreach (tbl_ib_doc_dtl itemdtl in dtlList.Where(x => x.cmp_id == itemhdr.cmp_id && x.cntr_id == itemhdr.cntr_id).ToList())
                        {
                            tbl_ib_doc_grid gridItem = new tbl_ib_doc_grid();
                            gridItem.HeaderInfo = itemhdr.HeaderInfo;
                            gridItem.cmp_id = itemhdr.cmp_id;
                            gridItem.eta_date = itemhdr.eta_date;
                            gridItem.ref_num = itemhdr.ref_num;
                            gridItem.rcvd_via = itemhdr.rcvd_via;
                            gridItem.rcvd_from = itemhdr.rcvd_from;
                            gridItem.master_bol = itemhdr.master_bol;
                            gridItem.vessel_no = itemhdr.vessel_no;
                            gridItem.hdr_notes = itemhdr.hdr_notes;      
                            gridItem.cntr_id = itemdtl.cntr_id;
                            gridItem.dtl_line = itemdtl.dtl_line;
                            gridItem.po_num = itemdtl.po_num;
                            gridItem.itm_num = itemdtl.itm_num;
                            gridItem.itm_color = itemdtl.itm_color;
                            gridItem.itm_size = itemdtl.itm_size;
                            gridItem.itm_name = itemdtl.itm_name;
                            gridItem.itm_qty = itemdtl.itm_qty;
                            gridItem.ctn_qty = itemdtl.ctn_qty;
                            gridItem.ctns = itemdtl.ctns;
                            gridItem.loc_id = itemdtl.loc_id;
                            gridItem.st_rate_id = itemdtl.st_rate_id;
                            gridItem.io_rate_id = itemdtl.io_rate_id;
                            gridItem.ctn_length = itemdtl.ctn_length;
                            gridItem.ctn_width = itemdtl.ctn_width;
                            gridItem.ctn_height = itemdtl.ctn_height;
                            gridItem.ctn_cube = itemdtl.ctn_cube;
                            gridItem.ctn_wgt = itemdtl.ctn_wgt;
                            gridItem.dtl_notes = itemdtl.dtl_notes;
                            gridList.Add(gridItem);
                        }

                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return gridList.Count > 0 ? Utility.ConvertListToDataTable(gridList) : null;
        }
        public ActionResult SaveEComIB943(string p_str_cmp_id, string p_str_file_name)
        {
            string result = string.Empty;
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceObjecteComIB943Upload = new eComIB943UploadService();
            objeComIB943Upload.error_mode = false;
            objeComIB943Upload.cmp_id = p_str_cmp_id;
            objeComIB943Upload = ServiceObjecteComIB943Upload.DeleteTempTable(objeComIB943Upload);
            Save943IBUpload(p_str_cmp_id, result);
            objeComIB943Upload.cmp_id = p_str_cmp_id;
            objeComIB943Upload.file_name = p_str_file_name;
            objeComIB943Upload.error_desc = "-";
            objeComIB943Upload.user_id = Session["UserID"].ToString().Trim();
            if (Session["lstOB940InvalidData_count"] != null || result == "Fail")
            {
                objeComIB943Upload.error_mode = true;
                Mapper.CreateMap<eComIB943Upload, eComIB943UploadModel>();
                eComIB943UploadModel objeComIB943UploadModel = Mapper.Map<eComIB943Upload, eComIB943UploadModel>(objeComIB943Upload);
                return Json(objeComIB943Upload.error_mode, JsonRequestBehavior.AllowGet);
            }
            objeComIB943Upload = ServiceObjecteComIB943Upload.AddECom943UploadFileNAme(objeComIB943Upload);
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
                System.IO.File.Delete(path2);
                System.IO.File.Move(path, path2);
            }
            Session["lstIB943UploadFileHdr"] = "";
            Session["lstIB943UploadFileDtl"] = "";
            Mapper.CreateMap<eComIB943Upload, eComIB943UploadModel>();
            eComIB943UploadModel IB943UploadModel = Mapper.Map<eComIB943Upload, eComIB943UploadModel>(objeComIB943Upload);
            return Json(IB943UploadModel, JsonRequestBehavior.AllowGet);
        }
        protected string Save943IBUpload(string p_str_cmp_id,string result)
        {
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceObjecteComIB943Upload = new eComIB943UploadService();
            List<tbl_ib_doc_grid> rptSaveList = new List<tbl_ib_doc_grid>();
            List<tbl_ib_doc_hdr> tbl_ib_doc_hdr = new List<tbl_ib_doc_hdr>();
            tbl_ib_doc_hdr = Session["lstIB943UploadFileHdr"] as List<tbl_ib_doc_hdr>;

            lstIB943UploadFileHdr = tbl_ib_doc_hdr;
            List<tbl_ib_doc_dtl> tbl_ib_doc_dtl = new List<tbl_ib_doc_dtl>();
            tbl_ib_doc_dtl = Session["lstIB943UploadFileDtl"] as List<tbl_ib_doc_dtl>;
            lstIB943UploadFileDtl = tbl_ib_doc_dtl;
            try
            {
                foreach (tbl_ib_doc_hdr itemhdr in lstIB943UploadFileHdr.OrderBy(y => y.HeaderInfo).ToList())
                {
                    double tmp_TtalQTY = 0;
                    Boolean saving_OK = true;
                    List<tbl_ib_doc_grid> breakList = new List<tbl_ib_doc_grid>();
                    foreach (tbl_ib_doc_dtl itemdtl in lstIB943UploadFileDtl.Where(x => x.cmp_id == itemhdr.cmp_id && x.cntr_id == itemhdr.cntr_id).OrderBy(y => y.dtl_line).ToList())
                    {
                        Session["dtl_list"] = itemdtl;

                        tbl_ib_doc_grid gridItem = SetData(itemhdr, itemdtl);
                        if (saving_OK)
                        {
                           // saving_OK = gridItem.StyleStatus == 0;

                            if (saving_OK) breakList.Add(gridItem);
                            else
                                //rptExcpList.Add(gridItem);

                            tmp_TtalQTY += Convert.ToDouble(gridItem.ctns);
                        }
                        else { }
                            //rptExcpList.Add(gridItem);
                    }
                    if (saving_OK)
                    {
                        SaveShipRequest(p_str_cmp_id,itemhdr, ref breakList);
                        rptSaveList.AddRange(breakList);
                    }
                }
               // Print_Result_List(rptSaveList, rptExcpList);
                result = "Success";
                return result;
            }
            catch (Exception Ex)
            {
                result = "Fail";
                return result;
            }
        }
        protected tbl_ib_doc_grid SetData(tbl_ib_doc_hdr hdr_itm, tbl_ib_doc_dtl itemdtl)
        {
            tbl_ib_doc_grid gridItem = new tbl_ib_doc_grid();
            gridItem.HeaderInfo = hdr_itm.HeaderInfo;
            gridItem.cmp_id = hdr_itm.cmp_id;
            gridItem.eta_date = hdr_itm.eta_date;
            gridItem.ref_num = hdr_itm.ref_num;
            gridItem.rcvd_via = hdr_itm.rcvd_via;
            gridItem.rcvd_from = hdr_itm.rcvd_from;
            gridItem.master_bol = hdr_itm.master_bol;
            gridItem.vessel_no = hdr_itm.vessel_no;
            gridItem.hdr_notes = hdr_itm.hdr_notes;
            gridItem.cntr_id = itemdtl.cntr_id;
            gridItem.dtl_line = itemdtl.dtl_line;
            gridItem.po_num = itemdtl.po_num;
            gridItem.itm_num = itemdtl.itm_num;
            gridItem.itm_color = itemdtl.itm_color;
            gridItem.itm_size = itemdtl.itm_size;
            gridItem.itm_name = itemdtl.itm_name;
            gridItem.itm_qty = itemdtl.itm_qty;
            gridItem.ctn_qty = itemdtl.ctn_qty;
            gridItem.ctns = itemdtl.ctns;
            gridItem.loc_id = itemdtl.loc_id;
            gridItem.st_rate_id = itemdtl.st_rate_id;
            gridItem.io_rate_id = itemdtl.io_rate_id;
            gridItem.ctn_length = itemdtl.ctn_length;
            gridItem.ctn_width = itemdtl.ctn_width;
            gridItem.ctn_height = itemdtl.ctn_height;
            gridItem.ctn_cube = itemdtl.ctn_cube;
            gridItem.ctn_wgt = itemdtl.ctn_wgt;
            gridItem.dtl_notes = itemdtl.dtl_notes;
            return gridItem;
        }
        public static void SaveShipRequest(string p_str_cmp_id, tbl_ib_doc_hdr hdr_data, ref List<tbl_ib_doc_grid> dtl_data)
        {
            int l_int_ordr_qty1;
            int l_int_ctn_qty1;
            int l_int_tmp_ordr_qty;
            int l_int_prev_dtl_line=0;
            int l_int_prev_next_line;
            int l_int_ctn_line = 1;
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceIB943UploadFile = new eComIB943UploadService();
            try
            {
                objeComIB943Upload.ib_cntr_upld_doc_id = Convert.ToString( ServiceIB943UploadFile.Get943UploadRefNum(p_str_cmp_id));
                //if (objeComIB943Upload.ListGetIBDOCID.Count > 0)
                //{
                //    objeComIB943Upload.ib_cntr_upld_doc_id = objeComIB943Upload.ListGetIBDOCID[0].ib_cntr_upld_doc_id;
                //}
               
                    objeComIB943Upload.cmp_id = hdr_data.cmp_id;
                    objeComIB943Upload.cntr_id = hdr_data.cntr_id;
                    objeComIB943Upload.eta_date = hdr_data.eta_date;
                    objeComIB943Upload.ref_num = hdr_data.ref_num;
                    objeComIB943Upload.rcvd_via = hdr_data.rcvd_via;
                    objeComIB943Upload.rcvd_from = hdr_data.rcvd_from;
                    objeComIB943Upload.master_bol = hdr_data.master_bol;
                    objeComIB943Upload.vessel_no = hdr_data.vessel_no;
                    objeComIB943Upload.hdr_notes = hdr_data.hdr_notes;
                    ServiceIB943UploadFile.InsertIB943UploadTempHdrtblDetails(objeComIB943Upload);
                    foreach (tbl_ib_doc_grid dtl_itm in dtl_data)
                    {
                    objeComIB943Upload.cmp_id = hdr_data.cmp_id;
                    objeComIB943Upload.cntr_id = dtl_itm.cntr_id;
                    objeComIB943Upload.dtl_line = dtl_itm.dtl_line;
                    objeComIB943Upload.po_num = dtl_itm.po_num;
                    objeComIB943Upload.itm_num = dtl_itm.itm_num;
                    objeComIB943Upload.itm_color = dtl_itm.itm_color;
                    objeComIB943Upload.itm_size = dtl_itm.itm_size;
                    objeComIB943Upload.itm_name = dtl_itm.itm_name;
                    objeComIB943Upload.itm_qty = dtl_itm.itm_qty;
                    objeComIB943Upload.ctn_qty = dtl_itm.ctn_qty;
                    objeComIB943Upload.ctns = dtl_itm.ctns;
                    objeComIB943Upload.loc_id = dtl_itm.loc_id;
                    objeComIB943Upload.st_rate_id = dtl_itm.st_rate_id;
                    objeComIB943Upload.io_rate_id = dtl_itm.io_rate_id;
                    objeComIB943Upload.ctn_length = dtl_itm.ctn_length;
                    objeComIB943Upload.ctn_width = dtl_itm.ctn_width;
                    objeComIB943Upload.ctn_height = dtl_itm.ctn_height;
                    objeComIB943Upload.ctn_cube = dtl_itm.ctn_cube;
                    objeComIB943Upload.ctn_wgt = dtl_itm.ctn_wgt;
                    objeComIB943Upload.dtl_notes = dtl_itm.dtl_notes;
                    l_int_tmp_ordr_qty = Convert.ToInt32(objeComIB943Upload.itm_qty);
                    l_int_ordr_qty1 = l_int_tmp_ordr_qty - ((l_int_tmp_ordr_qty) % (objeComIB943Upload.ctn_qty));
                    l_int_ctn_qty1 = l_int_tmp_ordr_qty % (objeComIB943Upload.ctn_qty);
                    if (l_int_ctn_qty1 > 0)
                    {
                        objeComIB943Upload.itm_qty = Convert.ToInt32(objeComIB943Upload.itm_qty) - l_int_ctn_qty1;
                        objeComIB943Upload.ctns = objeComIB943Upload.ctns;
                        objeComIB943Upload.ctn_qty = objeComIB943Upload.ctn_qty;
                        objeComIB943Upload.ctn_line = 1;
                        ServiceIB943UploadFile.InsertIB943UploadTempDtltblDetails(objeComIB943Upload);
                        objeComIB943Upload.itm_qty = l_int_ctn_qty1;
                        objeComIB943Upload.ctn_qty = l_int_ctn_qty1;
                        objeComIB943Upload.ctns = 1;
                        objeComIB943Upload.ctn_line = 2;
                        ServiceIB943UploadFile.InsertIB943UploadTempDtltblDetails(objeComIB943Upload);

                    }
                    else
                    {
                        objeComIB943Upload.ctn_line = l_int_ctn_line;
                        objeComIB943Upload.ctns = Convert.ToInt32(objeComIB943Upload.ctns);
                        objeComIB943Upload.ctn_qty = objeComIB943Upload.ctn_qty;
                        objeComIB943Upload.itm_qty = objeComIB943Upload.itm_qty;
                        ServiceIB943UploadFile.InsertIB943UploadTempDtltblDetails(objeComIB943Upload);
                        l_int_ctn_line++;
                    }
                   

                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

                objeComIB943Upload = ServiceIB943UploadFile.GetIB943UploadTempDtltblDetails(objeComIB943Upload);
                if (objeComIB943Upload.ListeCom940IB943UploadDtl.Count > 0)
                {
                    for (int i = 0; i < objeComIB943Upload.ListeCom940IB943UploadDtl.Count; i++)
                    {
                        objeComIB943Upload.cntr_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].cntr_id;
                        objeComIB943Upload.ib_cntr_upld_doc_id= objeComIB943Upload.ListeCom940IB943UploadDtl[i].ib_cntr_upld_doc_id;
                        objeComIB943Upload.dtl_line = objeComIB943Upload.ListeCom940IB943UploadDtl[i].dtl_line;
                        objeComIB943Upload.ctn_line = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_line;
                        objeComIB943Upload.po_num = objeComIB943Upload.ListeCom940IB943UploadDtl[i].po_num;
                        objeComIB943Upload.Itm_Code = objeComIB943Upload.ListeCom940IB943UploadDtl[i].Itm_Code;
                        objeComIB943Upload.itm_num = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_num;
                        objeComIB943Upload.itm_color = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_color;
                        objeComIB943Upload.itm_size = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_size;
                        objeComIB943Upload.itm_name = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_name;
                        objeComIB943Upload.itm_qty = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_qty;
                        objeComIB943Upload.ctn_qty = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_qty;
                        objeComIB943Upload.ctns = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctns;
                        objeComIB943Upload.loc_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].loc_id;
                        objeComIB943Upload.st_rate_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].st_rate_id;
                        objeComIB943Upload.io_rate_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].io_rate_id;
                        objeComIB943Upload.ctn_length = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_length;
                        objeComIB943Upload.ctn_width = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_width;
                        objeComIB943Upload.ctn_height = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_height;
                        objeComIB943Upload.ctn_cube = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_cube;
                        objeComIB943Upload.ctn_wgt = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_wgt;
                        objeComIB943Upload.dtl_notes = objeComIB943Upload.ListeCom940IB943UploadDtl[i].dtl_notes;
                        l_int_prev_next_line= objeComIB943Upload.ListeCom940IB943UploadDtl[i].dtl_line;
                        try
                        {
                            objeComIB943Upload = ServiceIB943UploadFile.CheckIBUploadedStyleExist(objeComIB943Upload);
                            objeComIB943Upload.Itm_Code = objeComIB943Upload.Itm_Code;
                        if (l_int_prev_dtl_line != l_int_prev_next_line)
                        {
                            ServiceIB943UploadFile.InsertIB943UploadDocDtltblDetails(objeComIB943Upload);
                        }
                        else
                        {
                            ServiceIB943UploadFile.UpdateTblIbDocDtl(objeComIB943Upload);
                           
                        }
                        ServiceIB943UploadFile.InsertDetailstoCtntable(objeComIB943Upload);
                        l_int_prev_dtl_line = l_int_prev_next_line;
                    }
                        catch (Exception Ex)
                        {
                            throw Ex;
                        }

                    }

                }
                objeComIB943Upload = ServiceIB943UploadFile.GetIB943UploadTempHdrtblDetails(objeComIB943Upload);
                if (objeComIB943Upload.ListeCom940IB943UploadHdr.Count > 0)
                {
                    for (int i = 0; i < objeComIB943Upload.ListeCom940IB943UploadHdr.Count; i++)
                    {
                        objeComIB943Upload.cmp_id = objeComIB943Upload.ListeCom940IB943UploadHdr[i].cmp_id;
                        objeComIB943Upload.ib_cntr_upld_doc_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ib_cntr_upld_doc_id;
                        objeComIB943Upload.eta_date = objeComIB943Upload.ListeCom940IB943UploadHdr[i].eta_date;
                        objeComIB943Upload.ref_num = objeComIB943Upload.ListeCom940IB943UploadHdr[i].ref_num;
                        objeComIB943Upload.rcvd_via = objeComIB943Upload.ListeCom940IB943UploadHdr[i].rcvd_via;
                        objeComIB943Upload.rcvd_from = objeComIB943Upload.ListeCom940IB943UploadHdr[i].rcvd_from;
                        objeComIB943Upload.master_bol = objeComIB943Upload.ListeCom940IB943UploadHdr[i].master_bol;
                        objeComIB943Upload.vessel_no = objeComIB943Upload.ListeCom940IB943UploadHdr[i].vessel_no;
                        objeComIB943Upload.hdr_notes = objeComIB943Upload.ListeCom940IB943UploadHdr[i].hdr_notes;
                        try
                        {
                            ServiceIB943UploadFile.InsertIB943UploadDocHdrtblDetails(objeComIB943Upload);
                        ServiceIB943UploadFile.InsertDetailstoAuditTrail(objeComIB943Upload);
                        }
                        catch (Exception Ex)
                        {
                            throw Ex;
                        }

                    }
                    
                }
            objeComIB943Upload = ServiceIB943UploadFile.DeleteTempTable(objeComIB943Upload);
        }
        public ActionResult Check943UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            eComIB943UploadService ServiceIB943UploadFile = new eComIB943UploadService();
            bool l_bl_file_exist = false;
            l_bl_file_exist = ServiceIB943UploadFile.Check943UploadFileExists(p_str_cmp_id, p_str_file_name);
            return Json(l_bl_file_exist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EComSR943InqDetail(string p_str_cmp_id, string p_str_file_name, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_batch_id)
        {
            try
            {
                string l_str_So_num = string.Empty;
                eComIB943Upload objeComIB943Upload = new eComIB943Upload();
                eComIB943UploadService ServiceObjecteComIB943Upload = new eComIB943UploadService();
                Session["g_str_Search_flag"] = "True";
                objeComIB943Upload.cmp_id = p_str_cmp_id.Trim();
                if (p_str_file_name != null && p_str_file_name != "")
                {
                    if (p_str_file_name.Contains(".csv"))
                    {
                        objeComIB943Upload.file_name = p_str_file_name;
                    }
                    else
                    {
                        objeComIB943Upload.file_name = p_str_file_name + ".csv";
                    }
                }
                else
                {
                    objeComIB943Upload.file_name = p_str_file_name;
                }

                objeComIB943Upload.eta_date = p_str_so_dt_frm;
                objeComIB943Upload.eta_date = p_str_so_dt_to;
                objeComIB943Upload.cntr_id = p_str_batch_id;
               // objeComIB943Upload = ServiceObjecteComIB943Upload.GetEcom943Inq(objeComIB943Upload);
                Mapper.CreateMap<eComIB943Upload, eComIB943UploadModel>();
                eComIB943UploadModel objeComIB943UploadModel = Mapper.Map<eComIB943Upload, eComIB943UploadModel>(objeComIB943Upload);
                return PartialView("_eComIB943Upload", objeComIB943UploadModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public ActionResult SingleLineFileUpload(string l_str_cmp_id)
        {
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceIB943UploadFile = new eComIB943UploadService();
            List<eComIB943Upload> li = new List<eComIB943Upload>();

            Session["dtl_list"] = "";
            Session["dtl_list_rpt"] = "";
            Session["lstOB940InvalidData"] = "";
            Session["lstOB940InvalidData_count"] = null;
            Session["Lessthan"] = null;
            objeComIB943Upload.cmp_id = l_str_cmp_id;



            objeComIB943Upload = ServiceIB943UploadFile.DeleteTempTable(objeComIB943Upload);

            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase FileUpload = files[i];
                        string filename;
                        string l_str_error_msg = string.Empty;
                        string BatchResult = string.Empty;


                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = FileUpload.FileName.Split(new char[] { '\\' });
                            filename = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            filename = FileUpload.FileName;
                        }


                        if (FileUpload != null)
                        {
                            if (FileUpload.ContentLength > 0)
                            {
                                //Workout our file path
                                string l_str_file_name = Path.GetFileName(FileUpload.FileName);
                                string l_str_file_path = Path.Combine(Server.MapPath("~/uploads"), l_str_file_name);
                                //Try and upload
                                try
                                {
                                    FileUpload.SaveAs(l_str_file_path);
                                    string l_str_ext = Path.GetExtension(l_str_file_name);
                                    if (l_str_ext.ToUpper() != ".CSV")
                                    {
                                        objeComIB943Upload.error_mode = true;
                                        objeComIB943Upload.error_desc = "Invalid File Format";
                                        return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                                    }

                                    

                                    Get943SingleLineUploadData(l_str_cmp_id, l_str_file_path, l_str_file_name, ".CSV", ref l_str_error_msg);
                                    if (l_str_error_msg != "")
                                    {
                                        objeComIB943Upload.error_mode = true;
                                        objeComIB943Upload.error_desc = l_str_error_msg;
                                        if (Session["Lessthan"] != null)
                                        {
                                            objeComIB943Upload.File_Length = "L";
                                        }


                                    }

                                    DataTable out_dt = (DataTable)CreateSingleLineUploadGridTable( lstIB943UploadFileDtl);
                                    if (out_dt != null)
                                    {
                                        DataTable SRHdrDtl = new DataTable();
                                        li = (from DataRow dr in out_dt.Rows
                                              select new eComIB943Upload()
                                              {
                                                  HeaderInfo = dr["HeaderInfo"].ToString(),
                                                  cmp_id = dr["cmp_id"].ToString(),
                                                  cntr_id = dr["cntr_id"].ToString(),
                                                  ref_num = dr["ref_num"].ToString(),
                                                  dtl_line = Convert.ToInt32(dr["dtl_line"].ToString()),
                                                  po_num = dr["po_num"].ToString(),
                                                  itm_num = (dr["itm_num"].ToString()),
                                                  itm_color = dr["itm_color"].ToString(),
                                                  itm_size = dr["itm_size"].ToString(),
                                                  itm_name = dr["itm_name"].ToString(),
                                                  itm_qty = Convert.ToInt32(dr["itm_qty"].ToString()),
                                                  ctn_qty = Convert.ToInt32(dr["ctn_qty"].ToString()),
                                                  ctns = Convert.ToInt32(dr["ctns"].ToString()),
                                                  loc_id = dr["loc_id"].ToString(),
                                                  st_rate_id = dr["st_rate_id"].ToString(),
                                                  io_rate_id = dr["io_rate_id"].ToString(),
                                                  ctn_length = Convert.ToDecimal(dr["ctn_length"].ToString()),
                                                  ctn_width = Convert.ToDecimal(dr["ctn_width"].ToString()),
                                                  ctn_height = Convert.ToDecimal(dr["ctn_height"].ToString()),
                                                  ctn_cube = Convert.ToDecimal(dr["ctn_cube"].ToString()),
                                                  ctn_wgt = Convert.ToDecimal(dr["ctn_wgt"].ToString()),
                                                  dtl_notes = dr["dtl_notes"].ToString(),
                                                  entry_dt = dr["entry_dt"].ToString(),
                                                  eta_date = dr["eta_date"].ToString(),
                                                  vend_id = dr["vend_id"].ToString(),

                                              }).ToList();

                                        objeComIB943Upload.ListeCom940IB943UploadDtl = li;
                                    }
                                    if (objeComIB943Upload.ListeCom940IB943UploadDtl != null)
                                    {
                                        Session["tbl_rpt_temp_list"] = objeComIB943Upload.ListeCom940IB943UploadDtl;
                                    }
                                    else
                                    {
                                        objeComIB943Upload.error_mode = true;
                                        return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    objeComIB943Upload.error_mode = true;
                                    objeComIB943Upload.error_desc = ex.InnerException.ToString();
                                    return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }


                        else
                        {
                            //Catch errors
                            objeComIB943Upload.error_mode = true;
                            objeComIB943Upload.error_desc = "Please select a file";
                            return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                        }



                    }
                }
                catch (Exception ex)
                {
                    objeComIB943Upload.error_mode = true;
                    objeComIB943Upload.error_desc = ex.Message;
                    return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objeComIB943Upload.error_mode = true;
                objeComIB943Upload.error_desc = "No files selected.";
                return Json(objeComIB943Upload, JsonRequestBehavior.AllowGet);
            }
            Mapper.CreateMap<eComIB943Upload, eComIB943UploadModel>();
            eComIB943UploadModel objeComIB943UploadModel = Mapper.Map<eComIB943Upload, eComIB943UploadModel>(objeComIB943Upload);
            return PartialView("_eComIB943Upload", objeComIB943UploadModel);
        }
        private void Get943SingleLineUploadData(string p_str_cmp_id, string p_str_file_path, string p_str_file_name, string p_str_file_extn, ref string l_str_error_msg)
        {
            lstIB943UploadFileHdr = new List<tbl_ib_doc_hdr>();
            lstIB943UploadFileDtl = new List<tbl_ib_doc_dtl>();
            lstOB940InvalidData = new List<IB943InvalidData>();

            Boolean hdr_ok = false;
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceIB943UploadFile = new eComIB943UploadService();
            l_str_error_msg = string.Empty;
            string l_str_error_desc = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_upload_ref_num = string.Empty;
            int l_int_no_of_lines = 0;




            try
            {
                l_str_file_name = System.IO.Path.GetFileNameWithoutExtension(p_str_file_path);
                if (p_str_file_extn.ToUpper().Equals(".CSV"))
                {
                    List<string> lst_file_line_content = new List<string>(System.IO.File.ReadAllLines(p_str_file_path));
                    int l_int_blank_line = lst_file_line_content.FindIndex(x => x.Trim().Length == 0);
                    if (l_int_blank_line != -1)
                    {
                        while (lst_file_line_content.Count > l_int_blank_line)
                        {
                            lst_file_line_content.RemoveAt(lst_file_line_content.Count - 1);
                        }
                    }

                    if (lst_file_line_content.Count() > 0)
                    {
                     
                        l_str_upload_ref_num = Convert.ToString(ServiceIB943UploadFile.Get943UploadRefNum(p_str_cmp_id));
                        l_int_no_of_lines = lst_file_line_content.Count();
                        objIB943UploadFileInfo = new IB943UploadFileInfo();
                        objIB943UploadFileInfo.cmp_id = p_str_cmp_id;
                        objIB943UploadFileInfo.file_name = p_str_file_name;
                        objIB943UploadFileInfo.upload_by = Session["UserID"].ToString().Trim();
                        objIB943UploadFileInfo.upload_date_time = DateTime.Now;
                        objIB943UploadFileInfo.no_of_lines = l_int_no_of_lines;
                        objIB943UploadFileInfo.status = "PEND";
                        objIB943UploadFileInfo.upload_ref_num = l_str_upload_ref_num;

                        Session["objOB940UploadFileInfo"] = objIB943UploadFileInfo;

                        for (int i = 1; i < l_int_no_of_lines; i++)
                        {

                            string[] field_data = lst_file_line_content[i].Split(',');
                            int l_str_length = field_data.Length;
                            if (field_data[0].ToUpper().Equals("SLF943"))

                            {

                                if (l_str_length != 17)
                                {
                                    l_str_error_desc = "Line  " + (i + 1).ToString() + " contains " + (l_str_length + 1).ToString() + " commas. It should be 17 ";
                                   // objOB940UploadFileDtl.error_desc = l_str_error_desc;

                                    continue;
                                }

                               // string[] filename = l_str_file_name.Split('_');

                                    if (field_data[1] != null && field_data[2] != null && field_data[4] != null && field_data[7] != null && field_data[8] != null)
                                    {
                                       
                                            //if (field_data[1].ToLower().Equals(p_str_cmp_id.ToLower()))
                                            //{
                                               
                                                    tbl_ib_doc_dtl objIB943UploadFileDtl = new tbl_ib_doc_dtl();
                                                    objIB943UploadFileDtl.cmp_id = field_data[1];
                                                    objIB943UploadFileDtl.cntr_id = field_data[2];
                                                    objIB943UploadFileDtl.dtl_line = Convert.ToInt32(field_data[3]);
                                                    objIB943UploadFileDtl.ref_num = field_data[4];
                                                    objIB943UploadFileDtl.itm_num = field_data[5];
                                                    try { objIB943UploadFileDtl.itm_color = field_data[6]; }
                                                    catch { objIB943UploadFileDtl.itm_color = ""; }

                                                    try { objIB943UploadFileDtl.itm_size = field_data[7]; }
                                                    catch { objIB943UploadFileDtl.itm_size = ""; }
                                                    try { objIB943UploadFileDtl.itm_name = field_data[8]; }

                                                    catch { objIB943UploadFileDtl.itm_name = ""; }

                                                    try
                                                    {

                                                        objIB943UploadFileDtl.itm_qty = Convert.ToInt32(field_data[9]);
                                                    }
                                                    catch { objIB943UploadFileDtl.itm_qty = 0; }

                                                    try { objIB943UploadFileDtl.ctn_qty = Convert.ToInt32(field_data[10]); }
                                                    catch { objIB943UploadFileDtl.ctn_qty = 0; }

                                                    try { objIB943UploadFileDtl.ctns = Convert.ToInt32(field_data[11]); }
                                                    catch { objIB943UploadFileDtl.ctns = 0; }
                                                    objIB943UploadFileDtl.loc_id = "FLOOR";
                                                    objIB943UploadFileDtl.st_rate_id = "STRG";
                                                    objIB943UploadFileDtl.io_rate_id = "INOUT";
                                                    objIB943UploadFileDtl.ctn_length = 0;
                                                    objIB943UploadFileDtl.ctn_width = 0;
                                                    objIB943UploadFileDtl.ctn_height = 0;
                                                    try { objIB943UploadFileDtl.ctn_cube = Convert.ToDecimal(field_data[12]); }
                                                    catch { objIB943UploadFileDtl.ctn_cube = 0; }
                                                    try { objIB943UploadFileDtl.ctn_wgt = Convert.ToDecimal(field_data[13]); }
                                                    catch { objIB943UploadFileDtl.ctn_wgt = 0; }
                                                    objIB943UploadFileDtl.entry_dt = ((field_data[14] == "-" || field_data[14] == null) ? DateTime.Now.ToString("MM/dd/yyyy") : field_data[14].ToString());
                                                    objIB943UploadFileDtl.eta_date = ((field_data[15] == "-" || field_data[15] == null) ? DateTime.Now.ToString("MM/dd/yyyy") : field_data[15].ToString());
                                                    objIB943UploadFileDtl.vend_id = field_data[16].ToString();
                                                    objIB943UploadFileDtl.HeaderInfo = objIB943UploadFileDtl.cmp_id.Trim() + ", " + "," + objIB943UploadFileDtl.cntr_id.Trim() + ", " + objIB943UploadFileDtl.eta_date.Trim() + "," + objIB943UploadFileDtl.ref_num.Trim();
                                                    hdr_ok = true;
                                                    if (hdr_ok)
                                                    {
                                                        if (objIB943UploadFileDtl.itm_num != null)
                                                        {
                                                            objeComIB943Upload.cmp_id = objIB943UploadFileDtl.cmp_id;
                                                            objeComIB943Upload.itm_num = objIB943UploadFileDtl.itm_num;
                                                            objeComIB943Upload.itm_name = objIB943UploadFileDtl.itm_name;
                                                            objeComIB943Upload.Itm_Code = objeComIB943Upload.Itm_Code;
                                                            if (objeComIB943Upload.Itm_Code != null)
                                                            {

                                                                if (Convert.ToDouble(objIB943UploadFileDtl.itm_qty) <= 0)
                                                                {
                                                                    objIB943UploadFileDtl.StyleStatus = 2;
                                                                    objIB943UploadFileDtl.StatusDesc = "Style Quantity is not valid...";
                                                                }
                                                                if (Convert.ToDouble(objIB943UploadFileDtl.ctn_qty) <= 0)
                                                                {
                                                                    objIB943UploadFileDtl.StyleStatus = 3;
                                                                    objIB943UploadFileDtl.StatusDesc = "Style Carton is not valid...";
                                                                }
                                                                if (Convert.ToDouble(objIB943UploadFileDtl.ctns) <= 0)
                                                                {
                                                                    objIB943UploadFileDtl.StyleStatus = 4;
                                                                    objIB943UploadFileDtl.StatusDesc = "Style PPK is not valid...";
                                                                }

                                                            }
                                                            else
                                                            {
                                                                objIB943UploadFileDtl.StyleStatus = 1;
                                                                objIB943UploadFileDtl.StatusDesc = "Style Does not Exist in the master item table..";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            objIB943UploadFileDtl.StyleStatus = 1;
                                                            objIB943UploadFileDtl.StatusDesc = "Style is missing";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        objIB943UploadFileDtl.StyleStatus = 5;
                                                        objIB943UploadFileDtl.StatusDesc = "Data already loaded (Duplicated) ";
                                                    }

                                                    if (objIB943UploadFileDtl.StyleStatus != 0)
                                                    {
                                                        objIB943UploadFileDtl.StyleDesc = objIB943UploadFileDtl.StatusDesc + "-" + objIB943UploadFileDtl.StyleDesc;
                                                    }
                                                    lstIB943UploadFileDtl.Add(objIB943UploadFileDtl);
                                                

                                                    //Header Section

                                               
                                            //}
                                            //else
                                            //{
                                            //    #region "Company mismatch"
                                            //    tbl_ib_doc_hdr hdr_itm = new tbl_ib_doc_hdr();
                                            //    hdr_itm.cmp_id = field_data[1];
                                            //    hdr_itm.eta_date = field_data[2];
                                            //    hdr_itm.HeaderInfo = "Company Selected: " + p_str_cmp_id + " and Company in the file: " + hdr_itm.cmp_id.Trim() + " please fix this mismatch";
                                            //    tbl_ib_doc_dtl objIB943UploadFileDtl = new tbl_ib_doc_dtl();
                                            //    objIB943UploadFileDtl.cmp_id = field_data[1];
                                            //    objIB943UploadFileDtl.cntr_id = field_data[2];
                                            //    objIB943UploadFileDtl.StyleStatus = 1;
                                            //    objIB943UploadFileDtl.StyleDesc = "Company Mismatch need to be fixed!!";
                                            //    #endregion "Company mismatch"
                                            //}
                                        
                                    }
                                }


                         
                            else
                            {
                                l_str_error_desc = "Line should starts with SLF943 ";
                            }

                        }
                        Session["lstIB943UploadFileDtl"] = lstIB943UploadFileDtl;

                    }
                    else
                    {
                        l_str_error_msg = "Invalid File Format";
                    }
                }
            }
            catch (Exception ex)
            {
                l_str_error_msg = ex.InnerException.ToString();
            }
        }
        private object CreateSingleLineUploadGridTable(List<tbl_ib_doc_dtl> dtlList)
        {
            List<tbl_ib_doc_grid> gridList = new List<tbl_ib_doc_grid>();
            try
            {
                if (dtlList.Count > 0)
                {
                    foreach (tbl_ib_doc_dtl itemdtl in dtlList)
                    {
                        tbl_ib_doc_grid gridItem = new tbl_ib_doc_grid();
                        gridItem.HeaderInfo = itemdtl.HeaderInfo;
                        gridItem.cmp_id = itemdtl.cmp_id;
                        gridItem.cntr_id = itemdtl.cntr_id;
                        gridItem.eta_date = itemdtl.eta_date;
                        gridItem.entry_dt = itemdtl.entry_dt;
                        gridItem.ref_num = itemdtl.ref_num;
                        gridItem.vend_id = itemdtl.vend_id;
                        gridItem.cntr_id = itemdtl.cntr_id;
                        gridItem.dtl_line = itemdtl.dtl_line;
                        gridItem.po_num = itemdtl.po_num;
                        gridItem.itm_num = itemdtl.itm_num;
                        gridItem.itm_color = itemdtl.itm_color;
                        gridItem.itm_size = itemdtl.itm_size;
                        gridItem.itm_name = itemdtl.itm_name;
                        gridItem.itm_qty = itemdtl.itm_qty;
                        gridItem.ctn_qty = itemdtl.ctn_qty;
                        gridItem.ctns = itemdtl.ctns;
                        gridItem.loc_id = itemdtl.loc_id;
                        gridItem.st_rate_id = itemdtl.st_rate_id;
                        gridItem.io_rate_id = itemdtl.io_rate_id;
                        gridItem.ctn_length = itemdtl.ctn_length;
                        gridItem.ctn_width = itemdtl.ctn_width;
                        gridItem.ctn_height = itemdtl.ctn_height;
                        gridItem.ctn_cube = itemdtl.ctn_cube;
                        gridItem.ctn_wgt = itemdtl.ctn_wgt;
                        gridItem.dtl_notes = itemdtl.dtl_notes;
                        gridList.Add(gridItem);
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return gridList.Count > 0 ? Utility.ConvertListToDataTable(gridList) : null;
        }
        public ActionResult SaveEComIB943SingleLineUpload(string p_str_cmp_id, string p_str_file_name)
        {
            string result = string.Empty;
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceObjecteComIB943Upload = new eComIB943UploadService();
            objeComIB943Upload.error_mode = false;
            objeComIB943Upload.cmp_id = p_str_cmp_id;
            ServiceObjecteComIB943Upload.DeleteSingleLineCntrUploadTempTable(objeComIB943Upload);
            SaveIBSingleLineUpload(p_str_cmp_id,result);
            objeComIB943Upload.cmp_id = p_str_cmp_id;
            objeComIB943Upload.file_name = p_str_file_name;
            objeComIB943Upload.error_desc = "-";
            objeComIB943Upload.user_id = Session["UserID"].ToString().Trim();
            if (Session["lstOB940InvalidData_count"] != null || result == "Fail")
            {
                objeComIB943Upload.error_mode = true;
                Mapper.CreateMap<eComIB943Upload, eComIB943UploadModel>();
                eComIB943UploadModel objeComIB943UploadModel = Mapper.Map<eComIB943Upload, eComIB943UploadModel>(objeComIB943Upload);
                return Json(objeComIB943Upload.error_mode, JsonRequestBehavior.AllowGet);
            }
            objeComIB943Upload = ServiceObjecteComIB943Upload.AddECom943UploadFileNAme(objeComIB943Upload);
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
                System.IO.File.Delete(path2);
                System.IO.File.Move(path, path2);
            }
            Session["lstIB943UploadFileHdr"] = "";
            Session["lstIB943UploadFileDtl"] = "";
            Mapper.CreateMap<eComIB943Upload, eComIB943UploadModel>();
            eComIB943UploadModel IB943UploadModel = Mapper.Map<eComIB943Upload, eComIB943UploadModel>(objeComIB943Upload);
            return Json(IB943UploadModel, JsonRequestBehavior.AllowGet);
        }
        protected tbl_ib_doc_grid SetSingleLineUpload(tbl_ib_doc_dtl itemdtl)
        {
            tbl_ib_doc_grid gridItem = new tbl_ib_doc_grid();
            gridItem.HeaderInfo = itemdtl.HeaderInfo;
            gridItem.cmp_id = itemdtl.cmp_id;
            gridItem.eta_date = itemdtl.eta_date;
            gridItem.entry_dt = itemdtl.entry_dt;
            gridItem.vend_id = itemdtl.vend_id;
            gridItem.ref_num = itemdtl.ref_num;
            gridItem.cntr_id = itemdtl.cntr_id;
            gridItem.dtl_line = itemdtl.dtl_line;
            gridItem.itm_num = itemdtl.itm_num;
            gridItem.itm_color = itemdtl.itm_color;
            gridItem.itm_size = itemdtl.itm_size;
            gridItem.itm_name = itemdtl.itm_name;
            gridItem.itm_qty = itemdtl.itm_qty;
            gridItem.ctn_qty = itemdtl.ctn_qty;
            gridItem.ctns = itemdtl.ctns;
            gridItem.loc_id = itemdtl.loc_id;
            gridItem.st_rate_id = itemdtl.st_rate_id;
            gridItem.io_rate_id = itemdtl.io_rate_id;
            gridItem.ctn_length = itemdtl.ctn_length;
            gridItem.ctn_width = itemdtl.ctn_width;
            gridItem.ctn_height = itemdtl.ctn_height;
            gridItem.ctn_cube = itemdtl.ctn_cube;
            gridItem.ctn_wgt = itemdtl.ctn_wgt;
            gridItem.dtl_notes = itemdtl.dtl_notes;
            return gridItem;
        }
        protected string SaveIBSingleLineUpload(string p_str_cmp_id, string result)
        {
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceObjecteComIB943Upload = new eComIB943UploadService();
            List<tbl_ib_doc_grid> rptSaveList = new List<tbl_ib_doc_grid>();
            List<tbl_ib_doc_dtl> tbl_ib_doc_dtl = new List<tbl_ib_doc_dtl>();
            tbl_ib_doc_dtl = Session["lstIB943UploadFileDtl"] as List<tbl_ib_doc_dtl>;
            lstIB943UploadFileDtl = tbl_ib_doc_dtl;
            try
            {
                double tmp_TtalQTY = 0;
                Boolean saving_OK = true;
                List<tbl_ib_doc_grid> breakList = new List<tbl_ib_doc_grid>();
                foreach (tbl_ib_doc_dtl itemdtl in lstIB943UploadFileDtl)
                {
                    Session["dtl_list"] = itemdtl;

                    tbl_ib_doc_grid gridItem = SetSingleLineUpload(itemdtl);
                    if (saving_OK)
                    {
                        // saving_OK = gridItem.StyleStatus == 0;

                        if (saving_OK) breakList.Add(gridItem);
                        else
                            //rptExcpList.Add(gridItem);

                            tmp_TtalQTY += Convert.ToDouble(gridItem.ctns);
                    }
                    else { }
                    //rptExcpList.Add(gridItem);
                }
                if (saving_OK)
                {
                    Save943IBSingleLineUpload(p_str_cmp_id,ref breakList);
                    rptSaveList.AddRange(breakList);
                }
                result = "Success";
                return result;
            }
            catch (Exception Ex)
            {
                result = "Fail";
                return result;
            }
        }
        public static void Save943IBSingleLineUpload(string p_str_cmp_id, ref List<tbl_ib_doc_grid> dtl_data)
        {
            eComIB943Upload objeComIB943Upload = new eComIB943Upload();
            eComIB943UploadService ServiceIB943UploadFile = new eComIB943UploadService();
            int l_int_prev_ib_doc_id=0;
            int l_int_curr_ib_doc_id=0;
            try
            {
                objeComIB943Upload.ib_cntr_upld_doc_id = Convert.ToString( ServiceIB943UploadFile.Get943UploadRefNum(p_str_cmp_id));
                if (objeComIB943Upload.ListGetIBDOCID.Count > 0)
                {
                    objeComIB943Upload.ib_cntr_upld_doc_id = objeComIB943Upload.ListGetIBDOCID[0].ib_cntr_upld_doc_id;
                }
                foreach (tbl_ib_doc_grid dtl_itm in dtl_data)
                {
                    objeComIB943Upload.cmp_id = dtl_itm.cmp_id;
                    objeComIB943Upload.cntr_id = dtl_itm.cntr_id;
                    objeComIB943Upload.dtl_line = dtl_itm.dtl_line;
                    objeComIB943Upload.po_num = dtl_itm.ref_num;
                    objeComIB943Upload.itm_num = dtl_itm.itm_num;
                    objeComIB943Upload.itm_color = dtl_itm.itm_color;
                    objeComIB943Upload.itm_size = dtl_itm.itm_size;
                    objeComIB943Upload.itm_name = dtl_itm.itm_name;
                    objeComIB943Upload.itm_qty = dtl_itm.itm_qty;
                    objeComIB943Upload.ctn_qty = dtl_itm.ctn_qty;
                    objeComIB943Upload.ctns = dtl_itm.ctns;
                    objeComIB943Upload.loc_id = dtl_itm.loc_id;
                    objeComIB943Upload.st_rate_id = dtl_itm.st_rate_id;
                    objeComIB943Upload.io_rate_id = dtl_itm.io_rate_id;
                    objeComIB943Upload.ctn_length = dtl_itm.ctn_length;
                    objeComIB943Upload.ctn_width = dtl_itm.ctn_width;
                    objeComIB943Upload.ctn_height = dtl_itm.ctn_height;
                    objeComIB943Upload.ctn_cube = dtl_itm.ctn_cube;
                    objeComIB943Upload.ctn_wgt = dtl_itm.ctn_wgt;
                    objeComIB943Upload.dtl_notes = dtl_itm.dtl_notes;
                    objeComIB943Upload.eta_date = dtl_itm.eta_date;
                    objeComIB943Upload.entry_dt = dtl_itm.entry_dt;
                    ServiceIB943UploadFile.InsertIB943SingleLineUploadTempDtltblDetails(objeComIB943Upload);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            objeComIB943Upload = ServiceIB943UploadFile.GetIB943SingleLineUploadTempDtltblDetails(objeComIB943Upload);
            if (objeComIB943Upload.ListeCom940IB943UploadDtl.Count > 0)
            {
                for (int i = 0; i < objeComIB943Upload.ListeCom940IB943UploadDtl.Count; i++)
                {
                    objeComIB943Upload.cntr_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].cntr_id;
                    objeComIB943Upload.ib_cntr_upld_doc_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ib_cntr_upld_doc_id;
                    objeComIB943Upload.dtl_line = objeComIB943Upload.ListeCom940IB943UploadDtl[i].dtl_line;
                    objeComIB943Upload.ctn_line = objeComIB943Upload.ListeCom940IB943UploadDtl[i].dtl_line;
                    objeComIB943Upload.po_num = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ref_num;
                    objeComIB943Upload.itm_num = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_num;
                    objeComIB943Upload.itm_color = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_color;
                    objeComIB943Upload.itm_size = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_size;
                    objeComIB943Upload.itm_name = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_name;
                    objeComIB943Upload.itm_qty = objeComIB943Upload.ListeCom940IB943UploadDtl[i].itm_qty;
                    objeComIB943Upload.ctn_qty = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_qty;
                    objeComIB943Upload.ctns = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctns;
                    objeComIB943Upload.loc_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].loc_id;
                    objeComIB943Upload.st_rate_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].st_rate_id;
                    objeComIB943Upload.io_rate_id = objeComIB943Upload.ListeCom940IB943UploadDtl[i].io_rate_id;
                    objeComIB943Upload.ctn_length = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_length;
                    objeComIB943Upload.ctn_width = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_width;
                    objeComIB943Upload.ctn_height = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_height;
                    objeComIB943Upload.ctn_cube = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_cube;
                    objeComIB943Upload.ctn_wgt = objeComIB943Upload.ListeCom940IB943UploadDtl[i].ctn_wgt;
                    objeComIB943Upload.dtl_notes = objeComIB943Upload.ListeCom940IB943UploadDtl[i].dtl_notes;
                    l_int_curr_ib_doc_id = Convert.ToInt32(objeComIB943Upload.ListeCom940IB943UploadDtl[i].ib_cntr_upld_doc_id);
                    if(l_int_prev_ib_doc_id != l_int_curr_ib_doc_id)
                    {
                        ServiceIB943UploadFile. InsertIB943UploadDocHdrtblDetails(objeComIB943Upload);
                    }
                    try
                    {
                        objeComIB943Upload = ServiceIB943UploadFile.CheckIBUploadedStyleExist(objeComIB943Upload);
                        objeComIB943Upload.Itm_Code = objeComIB943Upload.Itm_Code;
                        ServiceIB943UploadFile.InsertIB943UploadDocDtltblDetails(objeComIB943Upload);
                        ServiceIB943UploadFile.InsertDetailstoCtntable(objeComIB943Upload);
                        ServiceIB943UploadFile.InsertDetailstoAuditTrail(objeComIB943Upload);
                        l_int_prev_ib_doc_id = l_int_curr_ib_doc_id;
                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }

                }

            }
            ServiceIB943UploadFile.DeleteSingleLineCntrUploadTempTable(objeComIB943Upload);
            l_int_prev_ib_doc_id = 0;
            l_int_curr_ib_doc_id = 0;

        }

    }
}