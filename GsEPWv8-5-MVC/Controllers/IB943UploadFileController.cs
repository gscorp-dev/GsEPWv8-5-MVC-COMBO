using AutoMapper;
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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Diagnostics;
using System.Text;
using System.Configuration;
using Microsoft.Ajax.Utilities;
using Dapper;
using GsEPWv8_4_MVC.Common;

namespace GsEPWv8_5_MVC.Controllers
{
    public class IB943UploadFileController : Controller
    {
        public List<IB943UploadFileHdr> lstIB943UploadFileHdr;
        public List<IB943UploadFileDtl> lstIB943UploadFileDtl;
        public List<IB943InvalidData> lstIB943InvalidData;
        public IB943UploadFileInfo objIB943UploadFileInfo;

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IB943UploadFile(string cmp_id, string p_str_scn_id)
        {
            string l_str_cmp_id = string.Empty;
            IB943UploadFile objIB943UploadFile = new IB943UploadFile();
            IB943UploadFileInfo objIB943UploadFileInfo = new IB943UploadFileInfo();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();

            objIB943UploadFile.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            objIB943UploadFile.user_id = Session["UserID"].ToString().Trim();
            objIB943UploadFile.objIB943UploadFileInfo = objIB943UploadFileInfo;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objIB943UploadFile.ListCompany = objCompany.ListCompanyPickDtl;

            Session["lstIB943UploadFileHdr"] = "";
            Session["lstIB943UploadFileDtl"] = "";
            Session["lstIB943InvalidData"] = "";
            Session["objIB943UploadFileInfo"] = "";
            Session["l_str_error_msg"] = "";
            Mapper.CreateMap<IB943UploadFile, IB943UploadFileModel>();
            IB943UploadFileModel objIB943UploadFileModel = Mapper.Map<IB943UploadFile, IB943UploadFileModel>(objIB943UploadFile);
            return View(objIB943UploadFileModel);

        }
        public FileResult SampleTemplatedownload()
        {
            return File("~\\templates\\IB_943_UPLOAD_TEMPLATE_WITH_SAMPLE.xlsx", "text/xlsx", string.Format("IB_943_UPLOAD_TEMPLATE_WITH_SAMPLE-{0}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }

        public ActionResult ClearAll(string p_str_cmp_id)
        {
            IB943UploadFile objIB943UploadFile = new IB943UploadFile();
            IB943UploadFileInfo objIB943UploadFileInfo = new IB943UploadFileInfo();
            Session["objIB943UploadFileInfo"] = "";
            Session["lstIB943UploadFileDtl"] = "";
            Session["lstIB943UploadFileHdr"] = "";
            objIB943UploadFile.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            objIB943UploadFile.user_id = Session["UserID"].ToString().Trim();
            objIB943UploadFile.objIB943UploadFileInfo = objIB943UploadFileInfo;
            Mapper.CreateMap<IB943UploadFile, IB943UploadFileModel>();
            IB943UploadFileModel objIB943UploadFileModel = Mapper.Map<IB943UploadFile, IB943UploadFileModel>(objIB943UploadFile);
            return PartialView("_IB943UploadFile", objIB943UploadFileModel);

        }


        public ActionResult Check943UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            IB943UploadFileService ServiceIB943UploadFile = new IB943UploadFileService();
            bool l_bl_file_exist = false;
            l_bl_file_exist = ServiceIB943UploadFile.Check943UploadFileExists(p_str_cmp_id, p_str_file_name);
            return Json(l_bl_file_exist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SingleLineFileUpload(string l_str_cmp_id)
        {
            IB943UploadFile objIB943UploadFile = new IB943UploadFile();
            IB943UploadFileService ServiceIB943UploadFile = new IB943UploadFileService();


            Session["lstIB943UploadFileHdr"] = "";
            Session["lstIB943UploadFileDtl"] = "";
            Session["lstIB943InvalidData"] = "";
            Session["objIB943UploadFileInfo"] = "";
            Session["l_str_error_msg"] = "";
            ViewBag.l_int_error_count = "0";


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
                                string l_str_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["tempUploadFile"].ToString().Trim(), l_str_file_name);
                                //Try and upload
                                try
                                {
                                    FileUpload.SaveAs(l_str_file_path);
                                    string l_str_ext = Path.GetExtension(l_str_file_name);
                                    if (l_str_ext.ToUpper() != ".CSV")
                                    {
                                        objIB943UploadFile.error_mode = true;
                                        objIB943UploadFile.error_desc = "Invalid File Format";
                                        return Json(objIB943UploadFile, JsonRequestBehavior.AllowGet);
                                    }


                                    if (l_str_cmp_id == "ENCHLA2015" || l_str_cmp_id == "AMG8" || l_str_cmp_id == "TZMI" || l_str_cmp_id == "RP2020" || l_str_cmp_id == "GSA")
                                    {
                                        Get_CSV_List_Data_ENCH(l_str_cmp_id, l_str_file_path, l_str_file_name, ".CSV", ref l_str_error_msg);
                                    }
                                    else
                                    {
                                        Get_CSV_List_Data(l_str_cmp_id, l_str_file_path, l_str_file_name, ".CSV", ref l_str_error_msg);
                                    }


                                    if (l_str_error_msg != "")
                                    {
                                        objIB943UploadFile.error_mode = true;
                                        objIB943UploadFile.error_desc = l_str_error_msg;

                                    }


                                    objIB943UploadFile.ListIB943UploadFileHdr = lstIB943UploadFileHdr;
                                    objIB943UploadFile.ListIB943UploadFileDtl = lstIB943UploadFileDtl;
                                    objIB943UploadFile.ListIB943InvalidData = lstIB943InvalidData;
                                    ViewBag.l_int_error_count = lstIB943InvalidData.Count;
                                    Mapper.CreateMap<IB943UploadFile, IB943UploadFileModel>();
                                    IB943UploadFileModel objIB943UploadFileModel = Mapper.Map<IB943UploadFile, IB943UploadFileModel>(objIB943UploadFile);
                                    return PartialView("_IB943UploadFile", objIB943UploadFileModel);

                                }
                                catch (Exception ex)
                                {
                                    objIB943UploadFile.error_mode = true;
                                    objIB943UploadFile.error_desc = ex.InnerException.ToString();
                                    return Json(objIB943UploadFile, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }


                        else
                        {
                            //Catch errors
                            objIB943UploadFile.error_mode = true;
                            objIB943UploadFile.error_desc = "Please select a file";
                            return Json(objIB943UploadFile, JsonRequestBehavior.AllowGet);
                        }



                    }
                }
                catch (Exception ex)
                {
                    objIB943UploadFile.error_mode = true;
                    objIB943UploadFile.error_desc = ex.Message;
                    return Json(objIB943UploadFile, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objIB943UploadFile.error_mode = true;
                objIB943UploadFile.error_desc = "No files selected.";
                return Json(objIB943UploadFile, JsonRequestBehavior.AllowGet);
            }
            Mapper.CreateMap<IB943UploadFile, IB943UploadFileModel>();
            IB943UploadFileModel objIB943UploadFileModel1 = Mapper.Map<IB943UploadFile, IB943UploadFileModel>(objIB943UploadFile);
            return PartialView("_IB943UploadFile", objIB943UploadFileModel1);
        }
        private void Get_CSV_List_Data(string p_str_cmp_id, string p_str_file_path, string p_str_file_name, string p_str_file_extn, ref string l_str_error_msg)
        {

            lstIB943UploadFileHdr = new List<IB943UploadFileHdr>();
            lstIB943UploadFileDtl = new List<IB943UploadFileDtl>();
            lstIB943InvalidData = new List<IB943InvalidData>();

            IB943UploadFileService ServiceIB943UploadFile = new IB943UploadFileService();
            l_str_error_msg = string.Empty;
            string l_str_error_desc = string.Empty;
            string l_str_hdr_data = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_upload_ref_num = string.Empty;
            int l_int_no_of_lines = 0;
            string l_str_cntr_id = string.Empty;
            int l_int_line_num = 0;
            int l_int_line_without_data = 0;
            try
            {
                l_str_file_name = System.IO.Path.GetFileNameWithoutExtension(p_str_file_path).Replace("#", "");
                if (p_str_file_extn.ToUpper().Equals(".CSV"))
                {
                    List<string> lst_file_line_content = new List<string>(System.IO.File.ReadAllLines(p_str_file_path));
                    int l_int_blank_line = lst_file_line_content.FindIndex(x => x.Trim().Length == 0);
                    l_int_line_without_data = lst_file_line_content.FindIndex(x => x.Trim().Contains(",,,,,,,,,,,,,,"));
                    //l_int_line_without_data = -1;
                    //l_int_blank_line = -1;
                    if (l_int_blank_line != -1)
                    {
                        while (lst_file_line_content.Count > l_int_blank_line)
                        {
                            lst_file_line_content.RemoveAt(lst_file_line_content.Count - 1);
                        }
                    }

                    if (l_int_line_without_data != -1)
                    {
                        while (lst_file_line_content.Count > l_int_line_without_data)
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

                        Session["objIB943UploadFileInfo"] = objIB943UploadFileInfo;

                        int l_int_cur_line = 0;
                        List<string> lst_csv_data = new List<string>();
                        using (var file_reader = new CsvFileReader(p_str_file_path))
                        {
                            while (file_reader.ReadRow(lst_csv_data))
                            {
                                if (lst_csv_data[0].ToUpper().Equals("SLF943") && l_int_cur_line < l_int_no_of_lines)
                                {

                                    l_int_cur_line = l_int_cur_line + 1;
                                    if (l_int_cur_line == 1)
                                    {
                                        continue;
                                    }
                                    IB943UploadFileDtl objIB943UploadFileDtl = new IB943UploadFileDtl();
                                    int l_str_length = lst_csv_data.Count;

                                    bool bool_is_valied = false;
                                    if ((l_str_length == 22) || (l_str_length == 25)) bool_is_valied = true;
                                    if (bool_is_valied == false)
                                    {
                                        l_str_error_desc = "Line  " + l_int_cur_line.ToString() + " contains " + (l_str_length).ToString() + " fields It should be 22 or 25 ( New format) Please refer the Link 'IB 943 Upload Sample for sample' available in this page ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[1].Trim().Length > 10 || lst_csv_data[1].Trim().Length <= 2)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Company Id (2nd column) Length should be between 3 to 10 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[1] != p_str_cmp_id)
                                    {
                                        l_str_error_desc = "Line : " + l_int_cur_line.ToString() + " contains Invalid Company Id ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[2].Trim().Length > 25)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Container ID length should be maximum of 25 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[4].Trim().Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - PO Number / Ref# length should be maximum of 20 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[5].Trim().Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Style length should be maximum of 20 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[6].Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item Color length should be maximum of 20 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[7].Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item size length should be maximum of 20 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[8].Length > 75)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item Name length should be maximum of 75 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }
                                    // Header Note
                                    if (l_str_length == 22)
                                    {
                                        if (lst_csv_data[20].Trim().Length > 200)
                                        {
                                            l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Header Note length should be maximum of 200 ";
                                            objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                            IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                            objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                            objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                            objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                            objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstIB943InvalidData.Add(objIB943InvalidData);
                                            continue;
                                        }
                                    }

                                    if (l_str_length == 25)
                                    {
                                        if (lst_csv_data[22].Trim().Length > 50)
                                        {
                                            l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Customer length should be maximum of 50 ";
                                            objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                            IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                            objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                            objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                            objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                            objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstIB943InvalidData.Add(objIB943InvalidData);
                                            continue;
                                        }
                                        if (lst_csv_data[23].Trim().Length > 50)
                                        {
                                            l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Customer PO Number should be maximum of 20 ";
                                            objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                            IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                            objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                            objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                            objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                            objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstIB943InvalidData.Add(objIB943InvalidData);
                                            continue;
                                        }
                                        if (lst_csv_data[24].Trim().Length > 50)
                                        {
                                            l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Pick List length should be maximum of 100 ";
                                            objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                            IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                            objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                            objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                            objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                            objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstIB943InvalidData.Add(objIB943InvalidData);
                                            continue;
                                        }
                                    }


                                    if (l_str_cntr_id == string.Empty || l_str_cntr_id != lst_csv_data[2].Trim().ToUpper())
                                    {
                                        l_str_hdr_data = string.Empty;
                                        IB943UploadFileHdr objIB943UploadFileHdr = new IB943UploadFileHdr();
                                        objIB943UploadFileHdr.upload_ref_num = l_str_upload_ref_num;

                                        objIB943UploadFileHdr.cmp_id = lst_csv_data[1].Trim().ToUpper();

                                        objIB943UploadFileHdr.cntr_id = lst_csv_data[2].Trim().ToUpper();

                                        objIB943UploadFileHdr.ref_num = lst_csv_data[4].Trim().ToUpper();

                                        if (CheckDate(lst_csv_data[17]))
                                        {
                                            try
                                            {
                                                objIB943UploadFileHdr.ib_doc_dt = lst_csv_data[17];
                                            }
                                            catch
                                            {
                                                objIB943UploadFileHdr.ib_doc_dt = DateTime.Now.ToString("MM/dd/yyyy");
                                            }
                                        }
                                        else
                                        {
                                            objIB943UploadFileHdr.ib_doc_dt = DateTime.Now.ToString("MM/dd/yyyy");

                                        }


                                        if (CheckDate(lst_csv_data[18]))
                                        {
                                            try
                                            {
                                                objIB943UploadFileHdr.eta_date = lst_csv_data[18];
                                            }
                                            catch
                                            {
                                                objIB943UploadFileHdr.eta_date = DateTime.Now.ToString("MM/dd/yyyy");
                                            }
                                        }

                                        else
                                        {
                                            objIB943UploadFileHdr.eta_date = DateTime.Now.ToString("MM/dd/yyyy");

                                        }

                                        if (lst_csv_data[19].ToString().Length > 10)
                                        {
                                            objIB943UploadFileHdr.rcvd_from = lst_csv_data[19].ToString().Substring(1, 10);
                                        }
                                        else
                                        {
                                            objIB943UploadFileHdr.rcvd_from = lst_csv_data[19].ToString().Trim();
                                        }
                                        if ((l_str_length == 22) || (l_str_length == 25))
                                        {
                                            if (lst_csv_data[20].Trim().Length > 0)
                                            {
                                                objIB943UploadFileHdr.hdr_note = lst_csv_data[20].ToString();
                                            }
                                            objIB943UploadFileHdr.hdr_data = "IB Doc Date: " + String.Format("{0:MM/dd/yyyy}", objIB943UploadFileHdr.ib_doc_dt) + "|" + "ETA Date: " + String.Format("{0:MM/dd/yyyy}", objIB943UploadFileHdr.eta_date) + "|" + "Ref#: " + objIB943UploadFileHdr.ref_num + "Note: " + objIB943UploadFileHdr.hdr_note;
                                        }
                                        else
                                        {
                                            objIB943UploadFileHdr.hdr_data = "IB Doc Date: " + String.Format("{0:MM/dd/yyyy}", objIB943UploadFileHdr.ib_doc_dt) + "|" + "ETA Date: " + String.Format("{0:MM/dd/yyyy}", objIB943UploadFileHdr.eta_date) + "|" + "Ref#: " + objIB943UploadFileHdr.ref_num;
                                        }


                                        l_str_hdr_data = objIB943UploadFileHdr.hdr_data;
                                        lstIB943UploadFileHdr.Add(objIB943UploadFileHdr);
                                        l_int_line_num = 0;

                                    }

                                    l_int_line_num = l_int_line_num + 1;
                                    objIB943UploadFileDtl.line_num = l_int_line_num;
                                    l_str_cntr_id = lst_csv_data[2].Trim().ToUpper(); ;
                                    objIB943UploadFileDtl.cmp_id = lst_csv_data[1].Trim().ToUpper(); ;
                                    objIB943UploadFileDtl.cntr_id = lst_csv_data[2].Trim().ToUpper(); ;
                                    objIB943UploadFileDtl.dtl_line = Convert.ToInt32(lst_csv_data[3]);
                                    if (lst_csv_data[5].Length > 0)
                                    {
                                        objIB943UploadFileDtl.itm_num = lst_csv_data[5].ToUpper().Trim();
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Style Should not be blank";
                                    }

                                    if (lst_csv_data[6].Length > 0)
                                    {
                                        objIB943UploadFileDtl.itm_color = lst_csv_data[6].ToUpper().Trim();
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.itm_color = "-";
                                    }

                                    if (lst_csv_data[7].Trim().Length > 0)
                                    {
                                        objIB943UploadFileDtl.itm_size = lst_csv_data[7].ToUpper().Trim();
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.itm_size = "-";
                                    }

                                    if (lst_csv_data[8].Trim().Length > 0)
                                    {
                                        objIB943UploadFileDtl.itm_name = lst_csv_data[8].Trim();
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.itm_name = "-";
                                    }

                                    try
                                    {

                                        objIB943UploadFileDtl.ordr_qty = Convert.ToInt32(lst_csv_data[9]);
                                    }
                                    catch
                                    {
                                        objIB943UploadFileDtl.ordr_qty = 0;
                                        objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid Receiving Qty";
                                    }

                                    try { objIB943UploadFileDtl.ctn_qty = Convert.ToInt32(lst_csv_data[10]); }
                                    catch
                                    {
                                        objIB943UploadFileDtl.ctn_qty = 0;
                                        objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid Ctn Qty";
                                    }

                                    try
                                    {
                                        objIB943UploadFileDtl.ordr_ctn = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lst_csv_data[11])));

                                        // objIB943UploadFileDtl.ordr_ctn = Convert.ToInt32(lst_csv_data[11]);
                                    }
                                    catch
                                    {
                                        objIB943UploadFileDtl.ordr_ctn = 0;
                                        objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid Ctns";
                                    }

                                    objIB943UploadFileDtl.loc_id = "FLOOR";
                                    objIB943UploadFileDtl.st_rate_id = "STRG-1";
                                    objIB943UploadFileDtl.io_rate_id = "INOUT-1";
                                    objIB943UploadFileDtl.ctn_length = 0;
                                    objIB943UploadFileDtl.ctn_width = 0;
                                    objIB943UploadFileDtl.ctn_height = 0;

                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_length = Convert.ToDecimal(lst_csv_data[12]);
                                        if (objIB943UploadFileDtl.ctn_length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_length = 14;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[12].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_length = 14;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Length";
                                        }
                                    }

                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_width = Convert.ToDecimal(lst_csv_data[13]);
                                        if (objIB943UploadFileDtl.ctn_width == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_width = 14;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[13].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_width = 14;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Width";
                                        }


                                    }
                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_height = Convert.ToDecimal(lst_csv_data[14]);
                                        if (objIB943UploadFileDtl.ctn_height == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_height = 14;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[14].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_height = 14;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Height";
                                        }

                                    }

                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_cube = Math.Round(Convert.ToDecimal(lst_csv_data[15]), 3);

                                        if (objIB943UploadFileDtl.ctn_cube == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_cube = 1.59M;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[15].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_cube = 1.59M;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Cube";
                                        }

                                    }

                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_wgt = Convert.ToDecimal(lst_csv_data[16]);
                                        if (objIB943UploadFileDtl.ctn_wgt == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_wgt = 15;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[16].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_wgt = 15;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Weight";
                                        }

                                    }

                                    if (Convert.ToDouble(objIB943UploadFileDtl.ordr_qty) <= 0)
                                    {
                                        objIB943UploadFileDtl.error_desc = " - Invalid Receiving Qty";
                                    }
                                    if (Convert.ToDouble(objIB943UploadFileDtl.ctn_qty) <= 0)
                                    {
                                        objIB943UploadFileDtl.error_desc = "- Invalid Order Ctns";
                                    }
                                    if (Convert.ToDouble(objIB943UploadFileDtl.ordr_ctn) <= 0)
                                    {
                                        objIB943UploadFileDtl.error_desc = "- Invalid Order PPK";
                                    }
                                    try
                                    {
                                        if ((objIB943UploadFileDtl.ordr_qty / objIB943UploadFileDtl.ctn_qty == objIB943UploadFileDtl.ordr_ctn))
                                        {

                                            // Valied Order Quantity
                                        }

                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = " Carton Quantity mismatch. " + " Order Qty : " + objIB943UploadFileDtl.ordr_qty + " Order Cartons : " + objIB943UploadFileDtl.ordr_ctn + " If its is odd carton , please add a new line and Re-upload";

                                        }
                                    }
                                    catch
                                    {
                                        objIB943UploadFileDtl.error_desc = "- Carton Quantity mismatch";

                                    }

                                    objIB943UploadFileDtl.dtl_note = lst_csv_data[21].ToString();
                                    if (l_str_length == 25)
                                    {
                                        objIB943UploadFileDtl.factory_id = lst_csv_data[19].ToString().Trim();
                                        if (lst_csv_data[4].Trim()!= string.Empty)
                                        {
                                            objIB943UploadFileDtl.po_num = lst_csv_data[4].Trim().ToUpper();
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.po_num = "-";
                                        }
                                       
                                        objIB943UploadFileDtl.cust_name = lst_csv_data[22].ToString().Trim();
                                        objIB943UploadFileDtl.cust_po_num = lst_csv_data[23].ToString().Trim();
                                        objIB943UploadFileDtl.pick_list = lst_csv_data[24].ToString().Trim();
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.factory_id = lst_csv_data[19].ToString().Trim();

                                        if (lst_csv_data[4].Trim() != string.Empty)
                                        {
                                            objIB943UploadFileDtl.po_num = lst_csv_data[4].Trim().ToUpper();
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.po_num = "-";
                                        }

                                       // objIB943UploadFileDtl.po_num = lst_csv_data[4].Trim().ToUpper().Trim();
                                        objIB943UploadFileDtl.cust_name = string.Empty;
                                        objIB943UploadFileDtl.cust_po_num = string.Empty;
                                        objIB943UploadFileDtl.pick_list = string.Empty;
                                    }

                                    if (objIB943UploadFileDtl.error_desc.Length > 0)
                                    {
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.header_data = l_str_hdr_data;
                                        objIB943UploadFileDtl.upload_ref_num = l_str_upload_ref_num;
                                        lstIB943UploadFileDtl.Add(objIB943UploadFileDtl);
                                    }


                                }
                                else
                                {
                                    if (l_int_cur_line < l_int_no_of_lines)
                                        l_str_error_msg = "Invalid File Format";
                                    continue;
                                }
                            }

                        }


                        Session["lstIB943UploadFileHdr"] = lstIB943UploadFileHdr;
                        Session["lstIB943UploadFileDtl"] = lstIB943UploadFileDtl;
                        Session["lstIB943InvalidData"] = lstIB943InvalidData;
                        var vCntrList = lstIB943UploadFileHdr.Select(x => x.cntr_id).Distinct();
                        string strDuplicateCntrList = string.Empty;
                        foreach (var vCntr in vCntrList)
                        {

                            if (ServiceIB943UploadFile.CheckCntrIdExists(p_str_cmp_id, vCntr) == true)
                            {
                                strDuplicateCntrList = strDuplicateCntrList + vCntr + "|";
                            }
                        }
                        if (strDuplicateCntrList.Length > 0)
                        {
                            ViewBag.strDuplicateCntrList = "Containers[s]" + strDuplicateCntrList + " Already exists ";
                        }
                        else
                        {
                            ViewBag.strDuplicateCntrList = "";
                        }

                        if (lstIB943InvalidData.Count > 0)
                        {

                            l_str_error_msg = "ERROR";
                            Session["l_str_error_msg"] = "ERROR";
                        }

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

        private void Get_CSV_List_Data_ENCH(string p_str_cmp_id, string p_str_file_path, string p_str_file_name, string p_str_file_extn, ref string l_str_error_msg)
        {

            lstIB943UploadFileHdr = new List<IB943UploadFileHdr>();
            lstIB943UploadFileDtl = new List<IB943UploadFileDtl>();
            lstIB943InvalidData = new List<IB943InvalidData>();

            IB943UploadFileService ServiceIB943UploadFile = new IB943UploadFileService();
            l_str_error_msg = string.Empty;
            string l_str_error_desc = string.Empty;
            string l_str_hdr_data = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_upload_ref_num = string.Empty;
            int l_int_no_of_lines = 0;
            string l_str_cntr_id = string.Empty;
            int l_int_line_num = 0;
            int l_int_line_without_data = 0;
            try
            {
                l_str_file_name = System.IO.Path.GetFileNameWithoutExtension(p_str_file_path);
                if (p_str_file_extn.ToUpper().Equals(".CSV"))
                {
                    List<string> lst_file_line_content = new List<string>(System.IO.File.ReadAllLines(p_str_file_path));
                    int l_int_blank_line = lst_file_line_content.FindIndex(x => x.Trim().Length == 0);
                    l_int_line_without_data = lst_file_line_content.FindIndex(x => x.Trim().Contains(",,,,,,,,,,,,,,"));
                    //l_int_line_without_data = -1;
                    //l_int_blank_line = -1;
                    if (l_int_blank_line != -1)
                    {
                        while (lst_file_line_content.Count > l_int_blank_line)
                        {
                            lst_file_line_content.RemoveAt(lst_file_line_content.Count - 1);
                        }
                    }

                    if (l_int_line_without_data != -1)
                    {
                        while (lst_file_line_content.Count > l_int_line_without_data)
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

                        Session["objIB943UploadFileInfo"] = objIB943UploadFileInfo;

                        int l_int_cur_line = 0;
                        List<string> lst_csv_data = new List<string>();
                        using (var file_reader = new CsvFileReader(p_str_file_path))
                        {
                            while (file_reader.ReadRow(lst_csv_data))
                            {
                                if ((lst_csv_data[0].ToUpper().Equals("L") || lst_csv_data[0].ToUpper().Equals("SHIPMENT")) && l_int_cur_line < l_int_no_of_lines)
                                {

                                    l_int_cur_line = l_int_cur_line + 1;
                                    if (l_int_cur_line == 1)
                                    {
                                        continue;
                                    }
                                    IB943UploadFileDtl objIB943UploadFileDtl = new IB943UploadFileDtl();
                                    int l_str_length = lst_csv_data.Count;

                                    bool bool_is_valied = false;
                                    if ((l_str_length == 22)) bool_is_valied = true;
                                    if (bool_is_valied == false)
                                    {
                                        l_str_error_desc = "Line  " + l_int_cur_line.ToString() + " contains " + (l_str_length).ToString() + " Columns. It should be 22 Columns ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }



                                    if (lst_csv_data[3].Trim().Length + lst_csv_data[5].Trim().Length > 24)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Container ID" + lst_csv_data[3].Trim().ToString() + "-" + lst_csv_data[5].Trim().ToString() + " length should be maximum of 25 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[17].Trim().Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - PO Number / Ref# length should be maximum of 20 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[13].Trim().Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Style length should be maximum of 20 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[9].Length > 40) // Style and Color

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item Color length should be maximum of 20 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[15].Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item size length should be maximum of 20 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[14].Length > 75)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item Name length should be maximum of 75 ";
                                        objIB943UploadFileDtl.error_desc = l_str_error_desc;
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                        continue;
                                    }


                                    if (l_str_cntr_id == string.Empty || l_str_cntr_id != lst_csv_data[3].Trim().ToUpper().Trim() + "-" + lst_csv_data[5].Trim().ToUpper().Trim())
                                    {
                                        l_str_hdr_data = string.Empty;
                                        IB943UploadFileHdr objIB943UploadFileHdr = new IB943UploadFileHdr();
                                        objIB943UploadFileHdr.upload_ref_num = l_str_upload_ref_num;

                                        objIB943UploadFileHdr.cmp_id = p_str_cmp_id;

                                        objIB943UploadFileHdr.cntr_id = lst_csv_data[3].Trim().ToUpper() + "-" + lst_csv_data[5].Trim().ToUpper();

                                        objIB943UploadFileHdr.ref_num = lst_csv_data[17].Trim().ToUpper();

                                        objIB943UploadFileHdr.ib_doc_dt = DateTime.Now.ToString("MM/dd/yyyy");

                                        if (CheckDate(lst_csv_data[8]))
                                        {
                                            try
                                            {

                                                objIB943UploadFileHdr.eta_date = lst_csv_data[8];

                                                if (Convert.ToDateTime(lst_csv_data[8]) > DateTime.Now)
                                                {
                                                    objIB943UploadFileHdr.eta_date = lst_csv_data[8];
                                                }
                                                else
                                                {
                                                    objIB943UploadFileHdr.eta_date = DateTime.Now.ToString("MM/dd/yyyy");
                                                }
                                            }
                                            catch
                                            {
                                                objIB943UploadFileHdr.eta_date = DateTime.Now.ToString("MM/dd/yyyy");
                                            }
                                        }
                                        else
                                        {
                                            objIB943UploadFileHdr.eta_date = DateTime.Now.ToString("MM/dd/yyyy");

                                        }
                                        objIB943UploadFileHdr.rcvd_from = "-";
                                        objIB943UploadFileHdr.hdr_data = "IB Doc Date: " + String.Format("{0:MM/dd/yyyy}", objIB943UploadFileHdr.ib_doc_dt) + "|" + "ETA Date: " + String.Format("{0:MM/dd/yyyy}", objIB943UploadFileHdr.eta_date) + "|" + "Ref#: " + objIB943UploadFileHdr.ref_num;
                                        l_str_hdr_data = objIB943UploadFileHdr.hdr_data;
                                        lstIB943UploadFileHdr.Add(objIB943UploadFileHdr);
                                        l_int_line_num = 0;

                                    }

                                    l_int_line_num = l_int_line_num + 1;
                                    objIB943UploadFileDtl.line_num = l_int_line_num;
                                    l_str_cntr_id = lst_csv_data[3].Trim().ToUpper() + "-" + lst_csv_data[5].Trim().ToUpper();
                                    objIB943UploadFileDtl.cmp_id = p_str_cmp_id;
                                    objIB943UploadFileDtl.cntr_id = l_str_cntr_id.Trim();
                                    objIB943UploadFileDtl.dtl_line = l_int_cur_line - 1;
                                    if (lst_csv_data[13].Length > 0)
                                    {
                                        objIB943UploadFileDtl.itm_num = lst_csv_data[13].ToUpper().Trim();
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Style Should not be blank";
                                    }

                                    if (lst_csv_data[9].Length > 0)
                                    {
                                        if (lst_csv_data[9].ToUpper().Trim().Contains("+"))
                                        {
                                            string[] str_style_color = lst_csv_data[9].ToUpper().Trim().Split('+');
                                            objIB943UploadFileDtl.itm_color = str_style_color[1].ToString();
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.itm_color = lst_csv_data[9].ToUpper().Trim();
                                        }
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.itm_color = "-";
                                    }

                                    if (lst_csv_data[15].Trim().Length > 0)
                                    {
                                        objIB943UploadFileDtl.itm_size = lst_csv_data[15].ToUpper().Trim();
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.itm_size = "-";
                                    }

                                    if (lst_csv_data[14].Trim().Length > 0)
                                    {
                                        objIB943UploadFileDtl.itm_name = lst_csv_data[9] + " / " + lst_csv_data[14];
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.itm_name = "-";
                                    }

                                    try
                                    {

                                        objIB943UploadFileDtl.ordr_qty = Convert.ToInt32(lst_csv_data[10]);
                                    }
                                    catch
                                    {
                                        objIB943UploadFileDtl.ordr_qty = 0;
                                        objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid Receiving Qty";
                                    }

                                    try { objIB943UploadFileDtl.ctn_qty = Convert.ToInt32(lst_csv_data[11]); }
                                    catch
                                    {
                                        objIB943UploadFileDtl.ctn_qty = 0;
                                        objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid Ctn Qty";
                                    }

                                    try
                                    {
                                        objIB943UploadFileDtl.ordr_ctn = Convert.ToInt32(objIB943UploadFileDtl.ordr_qty / objIB943UploadFileDtl.ctn_qty);

                                        // objIB943UploadFileDtl.ordr_ctn = Convert.ToInt32(lst_csv_data[11]);
                                    }
                                    catch
                                    {
                                        objIB943UploadFileDtl.ordr_ctn = 0;
                                        objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid Ctns";
                                    }

                                    objIB943UploadFileDtl.loc_id = "FLOOR";
                                    objIB943UploadFileDtl.st_rate_id = "STRG-1";
                                    objIB943UploadFileDtl.io_rate_id = "INOUT-1";
                                    objIB943UploadFileDtl.ctn_length = 0;
                                    objIB943UploadFileDtl.ctn_width = 0;
                                    objIB943UploadFileDtl.ctn_height = 0;


                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_length = Convert.ToDecimal(lst_csv_data[21]);
                                        if (objIB943UploadFileDtl.ctn_length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_length = 14;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[21].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_length = 14;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Length";
                                        }
                                    }

                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_width = Convert.ToDecimal(lst_csv_data[20]);
                                        if (objIB943UploadFileDtl.ctn_width == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_width = 14;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[20].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_width = 14;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Width";
                                        }


                                    }
                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_height = Convert.ToDecimal(lst_csv_data[19]);
                                        if (objIB943UploadFileDtl.ctn_height == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_height = 14;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[19].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_height = 14;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Height";
                                        }

                                    }

                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_cube = Convert.ToDecimal(Math.Round((objIB943UploadFileDtl.ctn_length * objIB943UploadFileDtl.ctn_width * objIB943UploadFileDtl.ctn_height) / 1728, 3));
                                        if (objIB943UploadFileDtl.ctn_cube == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_cube = 1.59M;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[15].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_cube = 1.59M;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Cube";
                                        }

                                    }

                                    try
                                    {
                                        objIB943UploadFileDtl.ctn_wgt = Convert.ToDecimal(lst_csv_data[18]);
                                        if (objIB943UploadFileDtl.ctn_wgt == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_wgt = 15;
                                        }
                                    }
                                    catch
                                    {
                                        if (lst_csv_data[18].Trim().Length == 0)
                                        {
                                            objIB943UploadFileDtl.ctn_wgt = 15;
                                        }
                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = objIB943UploadFileDtl.error_desc + " - Invalid CTN Weight";
                                        }

                                    }

                                    if (Convert.ToDouble(objIB943UploadFileDtl.ordr_qty) <= 0)
                                    {
                                        objIB943UploadFileDtl.error_desc = " - Invalid Receiving Qty";
                                    }
                                    if (Convert.ToDouble(objIB943UploadFileDtl.ctn_qty) <= 0)
                                    {
                                        objIB943UploadFileDtl.error_desc = "- Invalid Order Ctns";
                                    }
                                    if (Convert.ToDouble(objIB943UploadFileDtl.ordr_ctn) <= 0)
                                    {
                                        objIB943UploadFileDtl.error_desc = "- Invalid Order PPK";
                                    }
                                    try
                                    {
                                        if ((objIB943UploadFileDtl.ordr_qty / objIB943UploadFileDtl.ctn_qty == objIB943UploadFileDtl.ordr_ctn))
                                        {

                                            // Valied Order Quantity
                                        }

                                        else
                                        {
                                            objIB943UploadFileDtl.error_desc = " Carton Quantity mismatch. " + " Order Qty : " + objIB943UploadFileDtl.ordr_qty + " Order Cartons : " + objIB943UploadFileDtl.ordr_ctn + " If its is odd carton , please add a new line and Re-upload";

                                        }
                                    }
                                    catch
                                    {
                                        objIB943UploadFileDtl.error_desc = "- Carton Quantity mismatch";

                                    }
                                    if (l_str_length == 22)
                                    {
                                        objIB943UploadFileDtl.dtl_note = "Cust Po#" + lst_csv_data[2].ToString() + "EfNo#" + lst_csv_data[3].ToString() + " Product Line #" + lst_csv_data[2].ToString();
                                    }

                                    objIB943UploadFileDtl.factory_id = string.Empty;
                                    if (lst_csv_data[2].ToString().Trim() != string.Empty)
                                    {
                                        objIB943UploadFileDtl.po_num = lst_csv_data[2].ToString();
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.po_num = "-";
                                    }

                                        //objIB943UploadFileDtl.po_num = lst_csv_data[2].ToString();
                                    objIB943UploadFileDtl.cust_name = string.Empty;
                                    objIB943UploadFileDtl.cust_po_num = lst_csv_data[17].ToString();
                                    objIB943UploadFileDtl.pick_list = string.Empty;

                                    if (objIB943UploadFileDtl.error_desc.Length > 0)
                                    {
                                        IB943InvalidData objIB943InvalidData = new IB943InvalidData();
                                        objIB943InvalidData.cntr_id = objIB943UploadFileDtl.cntr_id;
                                        objIB943InvalidData.line_num = objIB943UploadFileDtl.dtl_line;
                                        objIB943InvalidData.error_desc = objIB943UploadFileDtl.error_desc;
                                        objIB943InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstIB943InvalidData.Add(objIB943InvalidData);
                                    }
                                    else
                                    {
                                        objIB943UploadFileDtl.header_data = l_str_hdr_data;
                                        objIB943UploadFileDtl.upload_ref_num = l_str_upload_ref_num;
                                        lstIB943UploadFileDtl.Add(objIB943UploadFileDtl);
                                    }


                                }
                                else
                                {
                                    if (l_int_cur_line < l_int_no_of_lines)
                                        l_str_error_msg = "Invalid File Format";
                                    continue;
                                }
                            }

                        }


                        Session["lstIB943UploadFileHdr"] = lstIB943UploadFileHdr;
                        Session["lstIB943UploadFileDtl"] = lstIB943UploadFileDtl;
                        Session["lstIB943InvalidData"] = lstIB943InvalidData;

                        var vCntrList = lstIB943UploadFileHdr.Select(x => x.cntr_id).Distinct();
                        string strDuplicateCntrList = string.Empty;
                        foreach (var vCntr in vCntrList)
                        {

                            if (ServiceIB943UploadFile.CheckCntrIdExists(p_str_cmp_id, vCntr) == true)
                            {
                                strDuplicateCntrList = strDuplicateCntrList + vCntr + "|";
                            }
                        }
                        if (strDuplicateCntrList.Length > 0)
                        {
                            ViewBag.strDuplicateCntrList = "Containers[s]" + strDuplicateCntrList + " Already exists ";
                        }
                        else
                        {
                            ViewBag.strDuplicateCntrList = "";
                        }

                        if (lstIB943InvalidData.Count > 0)
                        {

                            l_str_error_msg = "ERROR";
                            Session["l_str_error_msg"] = "ERROR";
                        }

                        //var vCntrList = (IB943UploadFileHdr) lstIB943UploadFileHdr.DistinctBy(x => x.cntr_id);


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
        protected bool CheckDate(String date)

        {

            try

            {

                DateTime dt = DateTime.Parse(date);

                return true;

            }
            catch

            {

                return false;

            }

        }

        private void fnSaveIB943Doc(string pstrCmpId, string pstrUploadRefNo, string pstrFileName, string pstrFullFileName)
        {

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
            IB943UploadFile objIB943UploadFile = new IB943UploadFile();
            IB943UploadFileService ServiceIB943UploadFile = new IB943UploadFileService();

            l_str_config_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            l_str_file_extn = System.IO.Path.GetExtension(pstrFileName).ToLower();

            DataTable dtIBList = new DataTable();
            dtIBList = ServiceIB943UploadFile.fnGetIBDocListBy943(pstrCmpId, pstrUploadRefNo);

            string lstrDocId = string.Empty;
            for (int i = 0; i< dtIBList.Rows.Count; i++)
            {
                lstrDocId = dtIBList.Rows[i]["ib_doc_id"].ToString();
                l_str_sub_folder = lstrDocId.Substring(0, 3);
                l_str_upload_file = string.Format("0-943-" + pstrCmpId + "-" + lstrDocId + "-" + DateTime.Now.ToString("yyyyMMddHHssmm.") + l_str_file_extn);
                l_str_folder_path = pstrCmpId.Trim() + "\\IB\\" + l_str_sub_folder + "\\" + lstrDocId;

                l_str_folder_full_path = Path.Combine((l_str_config_file_path), l_str_folder_path);
                if (!Directory.Exists(l_str_folder_full_path))
                {
                    Directory.CreateDirectory(Path.Combine(l_str_folder_full_path));
                }
                l_str_file_path = Path.Combine(l_str_folder_full_path, l_str_upload_file);
                System.IO.File.Copy(pstrFullFileName, l_str_file_path);
                objDocumentUpload = new DocumentUpload();
                objDocumentUpload.doc_id = lstrDocId;
                objDocumentUpload.cmp_id = pstrCmpId;
                objDocumentUpload.doc_type = "IB";
                objDocumentUpload.orig_file_name = pstrFileName;
                objDocumentUpload.upload_file_name = l_str_upload_file;
                objDocumentUpload.file_path = l_str_folder_path;
                objDocumentUpload.upload_by = Session["UserID"].ToString().Trim();
                objDocumentUpload.comments = "943 Upload";
                objDocumentUpload.upload_dt = DateTime.Now;
                objDocumentUpload.doc_sub_type = "943";
                serviceDocumentUpload.SaveDocumentUpload(objDocumentUpload);

            }

          
       }
        public ActionResult SaveIB943UploadFile(string p_str_cmp_id, string p_str_file_name)
        {
            string result = string.Empty;
            string l_str_ref_num = string.Empty;

            string l_str_duplicate_cntr_id = string.Empty;

            DataTable p_dt_Ib_943_upload_file_info = new DataTable();
            DataTable p_dt_Ib_943_upload_file_hdr = new DataTable();
            DataTable p_dt_Ib_943_upload_file_dtl = new DataTable();

            IB943UploadFile objIB943UploadFile = new IB943UploadFile();
            IB943UploadFileService ServiceIB943UploadFile = new IB943UploadFileService();

            objIB943UploadFile.error_mode = false;
            objIB943UploadFile.cmp_id = p_str_cmp_id;

            List<IB943UploadFileHdr> lstIB943UploadFileHdr = new List<IB943UploadFileHdr>();
            lstIB943UploadFileHdr = Session["lstIB943UploadFileHdr"] as List<IB943UploadFileHdr>;
            p_dt_Ib_943_upload_file_hdr = Utility.ConvertListToDataTable(lstIB943UploadFileHdr);


            objIB943UploadFile.error_mode = false;
            objIB943UploadFile.error_desc = "NO";
            ViewBag.l_str_error_desc = "NO";
            //}
            IB943UploadFileInfo objIB943UploadFileInfo = new IB943UploadFileInfo();
            objIB943UploadFileInfo = Session["objIB943UploadFileInfo"] as IB943UploadFileInfo;
            p_dt_Ib_943_upload_file_info = Utility.ObjectToDataTable(objIB943UploadFileInfo);
            List<IB943UploadFileDtl> lstIB943UploadFileDtl = new List<IB943UploadFileDtl>();
            lstIB943UploadFileDtl = Session["lstIB943UploadFileDtl"] as List<IB943UploadFileDtl>;
            p_dt_Ib_943_upload_file_dtl = Utility.ConvertListToDataTable(lstIB943UploadFileDtl);
            result = ServiceIB943UploadFile.SaveIB943UploadFile(p_str_cmp_id, p_dt_Ib_943_upload_file_info, p_dt_Ib_943_upload_file_hdr, p_dt_Ib_943_upload_file_dtl);

            if (result == "OK")
            {

                result = ServiceIB943UploadFile.MoveIB943UploadToIBDocTables(p_str_cmp_id, p_dt_Ib_943_upload_file_info.Rows[0]["upload_ref_num"].ToString());
                Session["ses_upload_ref_num"] = p_dt_Ib_943_upload_file_info.Rows[0]["upload_ref_num"].ToString();


            }
            else
            {

            }

            if (result == "OK")
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/") + Path.Combine(System.Configuration.ConfigurationManager.AppSettings["tempUploadFile"].ToString().Trim(), p_str_file_name);
                string path2 = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                string strFullPath = string.Empty;
                path2 = Path.Combine(path2, p_str_cmp_id, "UPLOAD", "IB943");

                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                strFullPath = Path.Combine(path2, p_str_file_name);
                if (!System.IO.File.Exists(strFullPath))
                {
                    System.IO.File.Move(path, strFullPath);
                    fnSaveIB943Doc(p_str_cmp_id, Session["ses_upload_ref_num"].ToString(), p_str_file_name, strFullPath);

                }
                else
                {
                    string l_str_FileNameOnly = p_str_file_name.Substring(0, p_str_file_name.LastIndexOf("."));
                    string path3 = Path.Combine(path2, l_str_FileNameOnly + "-" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".csv");
                    System.IO.File.Move(path, path3);
                    fnSaveIB943Doc(p_str_cmp_id, Session["ses_upload_ref_num"].ToString(), p_str_file_name, path3);
                }

                Session["objIB943UploadFileInfo"] = "";
                Session["lstIB943UploadFileDtl"] = "";
                Session["lstIB943UploadFileHdr"] = "";
                objIB943UploadFile.error_mode = false;
                clsRptEmail objRptEmail = new clsRptEmail();
                bool lblnRptEmailExists = false;
                Email objEmail = new Email();
                objRptEmail.getEmailDetails(objEmail, p_str_cmp_id, "INBOUND", "IB-943-ACK", ref lblnRptEmailExists);
                if (lblnRptEmailExists == true)
                {
                    objIB943UploadFile.email_auto_sent = objEmail.is_auto_email;
                }
                else
                {
                    objIB943UploadFile.email_auto_sent = "N";
                }

                Mapper.CreateMap<IB943UploadFile, IB943UploadFileModel>();
                IB943UploadFileModel objIB943UploadFileModel = Mapper.Map<IB943UploadFile, IB943UploadFileModel>(objIB943UploadFile);
                return Json(objIB943UploadFile, JsonRequestBehavior.AllowGet);
            }
            else
            {
                objIB943UploadFile.error_mode = true;
                objIB943UploadFile.error_desc = result;
                return Json(objIB943UploadFile, JsonRequestBehavior.AllowGet);
            }



        }

        public ActionResult Send943EmailAckReport(string p_str_cmp_id, string p_str_file_name, string p_str_doc_type)
        {
            string l_str_upload_ref_num = string.Empty;
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            CustMaster objCustMaster = new CustMaster();
            ICustMasterService objCustMasterService = new CustMasterService();
            string l_str_status = string.Empty;
            string l_str_tmp_name = string.Empty;
            IB943UploadFileService ServiceIB943UploadFile = new IB943UploadFileService();
            IB943UploadFile objIB943UploadFile = new IB943UploadFile();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();

            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            string l_str_inout_type = string.Empty;
            string l_str_rpt_dtl = string.Empty;
            int l_int_tot_ctn = 0;
            int l_int_tot_qty = 0;
            decimal l_dec_tot_wgt = 0;
            decimal l_dec_tot_cube = 0;
            string l_str_cntr_list = string.Empty;
            string l_str_ib_doc_id_list = string.Empty;
            string l_str_image_Path = string.Empty;
            string Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();

            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);

            objCustMaster.cust_id = p_str_cmp_id;
            objCustMaster = objCustMasterService.GetCustomerLogo(objCustMaster);
            if (objCustMaster.ListGetCustLogo[0].cust_logo == null)
            {
                objCustMaster.ListGetCustLogo[0].cust_logo = "";
            }
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = "IB943";
            objEmail.username = objCompany.user_id;

            try
            {
                if (isValid)
                {

                    strReportName = "rpt_ib_doc_entry_ack.rpt";

                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//Inbound//" + strReportName;

                    objEmail = objEmailService.GetSendMailDetails(objEmail);
                    if (objEmail.ListEamilDetail.Count != 0)
                    {
                        objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                    }
                    else
                    {
                        objEmail.EmailMessageContent = "Please find the below inbound details. ";
                    }


                    l_str_upload_ref_num = Session["ses_upload_ref_num"].ToString();


                    DataTable dt943Summary = ServiceIB943UploadFile.GetIB940UploadFileSummary(p_str_cmp_id, p_str_file_name, l_str_upload_ref_num);
                    for (int i = 0; i <= dt943Summary.Rows.Count - 1; i++)
                    {
                        l_int_tot_ctn = l_int_tot_ctn + Convert.ToInt32(dt943Summary.Rows[i]["tot_ctn"]);
                        l_dec_tot_wgt = l_dec_tot_wgt + Convert.ToDecimal(dt943Summary.Rows[i]["tot_wgt"]);
                        l_dec_tot_cube = l_dec_tot_cube + Convert.ToDecimal(dt943Summary.Rows[i]["tot_cube"]);
                        l_int_tot_qty = l_int_tot_qty + Convert.ToInt32(dt943Summary.Rows[i]["tot_qty"]);
                        if (l_str_cntr_list.Length == 0)
                        {
                            l_str_cntr_list = l_str_cntr_list + dt943Summary.Rows[i]["cntr_id"];
                            l_str_ib_doc_id_list = l_str_ib_doc_id_list + dt943Summary.Rows[i]["ib_doc_id"];
                        }
                        else
                        {
                            l_str_cntr_list = l_str_cntr_list + " ; " + dt943Summary.Rows[i]["cntr_id"];
                            l_str_ib_doc_id_list = l_str_ib_doc_id_list + " ; " + dt943Summary.Rows[i]["ib_doc_id"];

                        }

                    }

                    l_str_rpt_dtl = p_str_cmp_id + "_" + "IB943_ACK";
                    objEmail.EmailSubject = p_str_cmp_id + "-" + "Inbound 943 confirmation . - Container# " + l_str_cntr_list;
                    objEmail.EmailMessage = "Hi All," + "\n\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + p_str_cmp_id + "\n" + "IB Doc Id#: " + " " + " " + l_str_ib_doc_id_list + "\n" + "Container# " + " " + l_str_cntr_list;
                    objEmail.EmailMessage = objEmail.EmailMessage + "\n\n" + "Total Qty: " + l_int_tot_qty + "\n" + "Total Ctns(s): " + l_int_tot_ctn + "\n" + "Total Cube: " + l_dec_tot_cube + "\n" + "Total Weight: " + l_dec_tot_wgt;
                    objIB943UploadFile = ServiceIB943UploadFile.Send943EmailAckReport(p_str_cmp_id, p_str_file_name, l_str_upload_ref_num, objIB943UploadFile);
                    l_str_image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                    var rptSource = objIB943UploadFile.ListIB943UploadAckRpt.ToList();


                    if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    {
                        strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                        strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rpt_dtl + "_" + strDateFormat + ".pdf";
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            rd.SetDataSource(rptSource);
                            rd.SetParameterValue("fml_rep_cmp_hdr", l_str_tmp_name);
                            rd.SetParameterValue("TotCtn", l_int_tot_ctn);
                            rd.SetParameterValue("TotWgt", l_dec_tot_wgt);
                            rd.SetParameterValue("TotCube", l_dec_tot_cube);
                            rd.SetParameterValue("fml_image_path", l_str_image_Path);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                        }
                        reportFileName = l_str_rpt_dtl + "_" + strDateFormat + ".pdf";
                        Session["RptFileName"] = strFileName;
                        objEmail.Attachment = reportFileName;
                    }
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

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Send943EmailAck(string p_str_cmp_id, string p_str_file_name, string p_str_doc_type)
        {
            string l_str_upload_ref_num = string.Empty;
            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";

            CustMaster objCustMaster = new CustMaster();
            ICustMasterService objCustMasterService = new CustMasterService();
            IB943UploadFileService ServiceIB943UploadFile = new IB943UploadFileService();
            IB943UploadFile objIB943UploadFile = new IB943UploadFile();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            InboundInquiryService ServiceObject = new InboundInquiryService();

            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            string l_str_inout_type = string.Empty;
            string l_str_rpt_dtl = string.Empty;
            string l_str_doc_id = string.Empty;
            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            int l_int_tot_ctn = 0;
            int l_int_tot_qty = 0;
            decimal l_dec_tot_wgt = 0;
            decimal l_dec_tot_cube = 0;
            string l_str_cntr_list = string.Empty;
            string l_str_ib_doc_id_list = string.Empty;

            l_str_rpt_dtl = p_str_cmp_id + "_" + "IB943_ACK";
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
          //  string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            string strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
            DataTable dtIBAck = new DataTable();
            DataTable dt943Summary = new DataTable();
            EmailAlertHdr objEmailAlertHdr = new EmailAlertHdr();
            objCompany.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetCompName(objCompany);
            objEmail.CmpId = p_str_cmp_id;
            objEmail.screenId = "IB943";
            objEmail.username = objCompany.user_id;

            try
            {
                if (isValid)
                {
                    l_str_upload_ref_num = Session["ses_upload_ref_num"].ToString();
                    dt943Summary = ServiceIB943UploadFile.GetIB940UploadFileSummary(p_str_cmp_id, p_str_file_name, l_str_upload_ref_num);
                    l_str_doc_id = (dt943Summary.Rows[0]["ib_doc_id"]).ToString();

                    for (int i = 0; i <= dt943Summary.Rows.Count - 1; i++)
                    {
                        l_int_tot_ctn = l_int_tot_ctn + Convert.ToInt32(dt943Summary.Rows[i]["tot_ctn"]);
                        l_dec_tot_wgt = l_dec_tot_wgt + Convert.ToDecimal(dt943Summary.Rows[i]["tot_wgt"]);
                        l_dec_tot_cube = l_dec_tot_cube + Convert.ToDecimal(dt943Summary.Rows[i]["tot_cube"]);
                        l_int_tot_qty = l_int_tot_qty + Convert.ToInt32(dt943Summary.Rows[i]["tot_qty"]);
                        l_str_cntr_list = l_str_cntr_list + dt943Summary.Rows[i]["cntr_id"];
                        l_str_ib_doc_id_list = l_str_ib_doc_id_list + dt943Summary.Rows[i]["ib_doc_id"];
                    }

                    dtIBAck = ServiceObject.fnIBGetInboundAckRpt(p_str_cmp_id, p_str_file_name, l_str_upload_ref_num);

                    if (!Directory.Exists(strOutputpath))
                    {
                        Directory.CreateDirectory(strOutputpath);
                    }

                    l_str_file_name = "DF_" + p_str_cmp_id.ToUpper().ToString().Trim() + "_IB943_ACK_" + strDateFormat + ".xlsx";

                    tempFileName = strOutputpath + l_str_file_name;

                    if (System.IO.File.Exists(tempFileName)) System.IO.File.Delete(tempFileName);
                    xls_IB_943_ACK mxcel1 = null;

                    mxcel1 = new xls_IB_943_ACK(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "IB-943-ACK.xlsx");
                    mxcel1.PopulateHeader(p_str_cmp_id);
                    mxcel1.PopulateData(dtIBAck);
                    mxcel1.SaveAs(tempFileName);
                    FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                    Session["RptFileName"] = tempFileName;
                    reportFileName = l_str_file_name;

                    //clsRptEmail objRptEmail = new clsRptEmail();
                    //bool lblnRptEmailExists = false;
                    //objRptEmail.getEmailDetails(objEmail, p_str_cmp_id, "INBOUND", "IB-943-ACK", ref lblnRptEmailExists);

                    //objEmail.EmailSubject = p_str_cmp_id + "-" + "Inbound 943 confirmation . - Container# " + l_str_cntr_list;
                    //string l_str_email_message = string.Empty;
                    //if (lblnRptEmailExists == false)
                    //{
                    //    l_str_email_message = "Hi All, " + "\n\n" + " Please find the attached IB 940 Acknowledgement Report" + "\n\n";
                    //}

                    //else
                    //{
                    //    l_str_email_message = "Hi All, " + "\n\n";
                    //    l_str_email_message = l_str_email_message + objEmail.EmailMessage + "\n\n";
                    //}
                    //objEmail.EmailMessage = l_str_email_message + "\n\n" + "Total Qty: " + l_int_tot_qty + "\n" + "Total Ctns(s): " + l_int_tot_ctn + "\n" + "Total Cube: " + l_dec_tot_cube + "\n" + "Total Weight: " + l_dec_tot_wgt;
                    //objEmail.EmailMessage = objEmail.EmailMessage + "\n" + objEmail.EmailFooter + "\n";
                    //objEmail.FilePath = strOutputpath;
                    //objEmail.Attachment = reportFileName;
             
                    clsRptEmail objRptEmail = new clsRptEmail();
                    bool lblnRptEmailExists = false;
                    objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmp_id, "INBOUND", "IB-943-ACK", ref lblnRptEmailExists);
                    string l_str_email_message = string.Empty;
                    if (lblnRptEmailExists == false)
                    {
                        l_str_email_message = "Hi All, " + "\n\n" + " Please find the attached IB 943 Acknowledgement Report" + "\n\n";
                    }

                    else
                    {
                        l_str_email_message = "Hi All, " + "\n\n";
                        l_str_email_message = l_str_email_message + objEmail.EmailMessage + "\n\n";
                    }
                    objEmailAlertHdr.emailMessage = l_str_email_message;
                    objEmailAlertHdr.emailMessage = objEmailAlertHdr.emailMessage + "\n" + objEmailAlertHdr.emailFooter + "\n";
                    objEmailAlertHdr.filePath = strOutputpath;
                    objEmailAlertHdr.fileName = l_str_file_name;
                    objEmailAlertHdr.emailSubject = p_str_cmp_id + "-" + "Inbound 943 confirmation . - Container# " + l_str_cntr_list;

                }

                EmailAlert objEmailAlert = new EmailAlert();
                objEmailAlertHdr.cmpId = p_str_cmp_id;
                objEmailAlert.objEmailAlertHdr = objEmailAlertHdr;

                Mapper.CreateMap<EmailAlert, EmailAlertModel>();
                EmailAlertModel EmailModel = Mapper.Map<EmailAlert, EmailAlertModel>(objEmailAlert);
                return PartialView("_EmailAlert", EmailModel);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                jsonErrorCode = "-2";
            }

            finally
            {
                if (dt943Summary != null)
                {
                    dt943Summary = null;
                }

                objCustMaster = null;
                objCustMasterService = null;
                ServiceIB943UploadFile = null;
                objIB943UploadFile = null;
                objEmail = null;
                objEmailService = null;
                objCompany = null;
                ServiceObjectCompany = null;
                ServiceObject = null;

            }
            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }

    }


}