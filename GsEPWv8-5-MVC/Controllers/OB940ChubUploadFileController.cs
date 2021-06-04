using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_4_MVC.Common;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Common;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class OB940ChubUploadFileController : Controller
    {
        public List<OB940UploadFileHdr> lstOB940UploadFileHdr;
        public List<OB940UploadFileDtl> lstOB940UploadFileDtl;
        public List<OB940InvalidData> lstOB940InvalidData;
        public OB940UploadFileInfo objOB940UploadFileInfo;
        CustMaster objCustMaster = new CustMaster();
        ICustMasterService objCustMasterService = new CustMasterService();

        // GET: OutboundShipper940Upload
        /// <summary>
        /// Function Call to Load the Upload screen
        /// </summary>
        /// <param name="p_cmp_id"></param>
        /// <param name="p_str_scn_id"></param>
        /// <returns></returns>
          public ActionResult OB940ChubUploadFile(string p_cmp_id, string p_str_scn_id)
        {
            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
            OB940UploadFileInfo objOB940UploadFileInfo = new OB940UploadFileInfo();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            try
            {
                if(p_str_scn_id==null)
                {
                    p_str_scn_id = Session["ses_scn_id"].ToString().Trim();
                }
                Session["ses_scn_id"] = p_str_scn_id;
                objOB940UploadFile.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                objOB940UploadFile.uploadLink = p_str_scn_id;
                objOB940UploadFile.user_id = Session["UserID"].ToString().Trim();
                if (Session["ses_str_batch_id"] != null) objOB940UploadFile.batch_id = Session["ses_str_batch_id"].ToString();
                objCompany.user_id = Session["UserID"].ToString().Trim();
                objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                objOB940UploadFile.ListCompany = objCompany.ListCompanyPickDtl;
                Mapper.CreateMap<OB940UploadFile, OB940UploadFileModel>();
                OB940UploadFileModel objOB940UploadFileModel = Mapper.Map<OB940UploadFile, OB940UploadFileModel>(objOB940UploadFile);
                return View(objOB940UploadFileModel);
            }
            catch
            {
                return null;
            }
            finally
            {
                
                objOB940UploadFile = null;
                objOB940UploadFileInfo = null;
                objCompany = null;
                ServiceObjectCompany = null;
              
            }
        }

        /// <summary>
        /// Download Template
        /// </summary>
        /// <returns></returns>

        public FileResult SampleTemplatedownload()
        {
            return File("~\\templates\\OB_SHIPPER_940_SLF_PICK-ORD_Upload_SAMPLE.xlsx", "text/xlsx", string.Format("OB_SHIPPER_940_SLF_PICK-ORD_Upload_SAMPLE-{0}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }

        /// <summary>
        /// Clear fields
        /// </summary>
        /// <param name="p_str_cmp_id"></param>
        /// <returns></returns>
        public ActionResult ClearAll(string p_str_cmp_id)
        {
            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
            OB940UploadFileInfo objOB940UploadFileInfo = new OB940UploadFileInfo();
            Session["objOB940UploadFileInfo"] = "";
            Session["lstOB940UploadFileDtl"] = "";
            Session["lstOB940UploadFileHdr"] = "";
            objOB940UploadFile.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
            objOB940UploadFile.user_id = Session["UserID"].ToString().Trim();
            objOB940UploadFile.objOB940UploadFileInfo = objOB940UploadFileInfo;
            Mapper.CreateMap<OB940UploadFile, OB940UploadFileModel>();
            OB940UploadFileModel objOB940UploadFileModel = Mapper.Map<OB940UploadFile, OB940UploadFileModel>(objOB940UploadFile);
            return PartialView("_OB940UploadFile", objOB940UploadFileModel);

        }

        public ActionResult Check940UploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
            bool l_bl_file_exist = false;
            l_bl_file_exist = ServiceOB940UploadFile.Check940UploadFileExists(p_str_cmp_id, p_str_file_name);
            return Json(l_bl_file_exist, JsonRequestBehavior.AllowGet);
        }

        private void Get_List_Data(string p_str_cmp_id, string p_str_file_path, string p_str_file_name, string p_str_file_extn, ref string l_str_error_msg)
        {
            lstOB940UploadFileHdr = new List<OB940UploadFileHdr>();
            lstOB940UploadFileDtl = new List<OB940UploadFileDtl>();
            lstOB940InvalidData = new List<OB940InvalidData>();
            bool is_hdr_printed = false;
            string l_str_refno = string.Empty;
            string l_str_error_desc = string.Empty;
            string l_str_hdr_data = string.Empty;
            string l_str_upload_ref_num = string.Empty;
            int l_int_no_of_lines = 0;
            int l_int_line_num = 0;
            int l_int_cur_line = 0;
            try
            {
                if (p_str_file_extn.ToUpper().Equals(".CSV"))
                {
                    List<string> lst_file_line_content = new List<string>(System.IO.File.ReadAllLines(p_str_file_path));
                    // Remove Blank lines
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
                        OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
                        // Get Reference Number
                        l_str_upload_ref_num = Convert.ToString(ServiceOB940UploadFile.Get940UploadRefNum(p_str_cmp_id));
                        l_int_no_of_lines = lst_file_line_content.Count();
                        objOB940UploadFileInfo = new OB940UploadFileInfo();
                        objOB940UploadFileInfo.cmp_id = p_str_cmp_id;
                        objOB940UploadFileInfo.file_name = p_str_file_name;
                        objOB940UploadFileInfo.upload_by = Session["UserID"].ToString().Trim();
                        objOB940UploadFileInfo.upload_date_time = DateTime.Now;
                        objOB940UploadFileInfo.no_of_lines = l_int_no_of_lines;
                        objOB940UploadFileInfo.status = "PEND";
                        objOB940UploadFileInfo.upload_ref_num = l_str_upload_ref_num;
                        Session["objOB940UploadFileInfo"] = objOB940UploadFileInfo;
                         l_int_cur_line = 0;
                        List<string> lst_csv_data = new List<string>();

                        using (var file_reader = new CsvFileReader(p_str_file_path))
                        {
                            while (file_reader.ReadRow(lst_csv_data))
                            {
                                l_int_cur_line = l_int_cur_line + 1;
                                if (l_int_cur_line == 1) // To Skip the first line
                                {
                                    continue;
                                }
                                
                                if (lst_csv_data[0].ToUpper().Equals("SLF940"))
                                {
                                    OB940UploadFileDtl objOB940UploadFileDtl = new OB940UploadFileDtl();
                                    int l_str_length = lst_csv_data.Count;
                                    bool bool_is_valied = false;
                                    if (l_str_length >= 32) bool_is_valied = true;
                                    if (bool_is_valied == false)
                                    {
                                        l_str_error_desc = "Line  " + l_int_cur_line.ToString() + " contains " + (l_str_length ).ToString() + " Fields. It should be 32. Please refer the Link 'OB SHIPPER 940_SLF PICK-ORD Upload Sample' available in this page ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[1].Trim().Length > 10)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Company Id (2nd column) Length should be maximum of 10 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[1].Trim() != p_str_cmp_id)
                                    {
                                        l_str_error_msg = "MISMATCH-CMP-ID";
                                        break;
                                    }

                                    if (lst_csv_data[2].Trim().Length > 20)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Reference Number (3rd column) Length should be maximum of 20 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;

                                    }

                                    if (lst_csv_data[3].Trim().Length > 20)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Batch Id (4th column) Length should be maximum of 20 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[4].Trim().Length > 50)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Cust Id (5th column) Length should be maximum of 50 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[5].Trim().Length > 30)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Cust Order Number (6th column) Length should be maximum of 30 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                    // ordr_num 
                                    if (lst_csv_data[18].Trim().Length > 20)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Order Number (19th column) Length should be maximum of 20 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                    // Department Id 
                                    if (lst_csv_data[23].Trim().Length > 30)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Department Id (24th column) Length should be maximum of 30 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                    // Store Id 
                                    if (lst_csv_data[24].Trim().Length > 30)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Store Id (25th column) Length should be maximum of 30 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                    //st_name
                                    if (lst_csv_data[25].Trim().Length > 50)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Ship To Name (26th column) Length should be maximum of 50 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                    //st_addr_line1
                                    if (lst_csv_data[26].Trim().Length > 50)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Address Line 1 (27th column) Length should be maximum of 50 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                    //st_addr_line2
                                    if (lst_csv_data[27].Trim().Length > 50)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Address Line 2 (28th column) Length should be maximum of 50 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    //City
                                    if (lst_csv_data[28].Trim().Length > 30)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - City Name (29th column) Length should be maximum of 30 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    //st_state_id
                                    if (lst_csv_data[29].Trim().Length > 10)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - State Id (30th column) Length should be maximum of 10 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    //st_post_code
                                    if (lst_csv_data[30].Trim().Length > 20)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Post Code (31th column) Length should be maximum of 20 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                    //st_cntry_id
                                    if (lst_csv_data[31].Trim().Length > 10)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Country Id (32th column) Length should be maximum of 10 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                    //hdr_note
                                    if (lst_csv_data[32].Trim().Length > 200)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Header Note (33th column) Length should be maximum of 200 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    //Ship Via
                                    if (lst_csv_data[33].Trim().Length > 10)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Ship Via (34th column) Length should be maximum of 10 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                    if (l_str_length > 34)
                                    { 
                                    // Pick Number
                                    if (lst_csv_data[34].Trim().Length > 20)
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " -Pick Number (35th column) Length should be maximum of 20 ";
                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }
                                   

                                    }
                                   


                                    if (CheckDate(lst_csv_data[20]))
                                        {
                                            DateTime dtSRDate;
                                            dtSRDate = Utility.ConvertToDateTime(lst_csv_data[20]);
                                            if (dtSRDate.Date > System.DateTime.Now.Date)
                                            {
                                                l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - SR Date (21 column) Should not be greater than system date ";
                                                objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                                OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                                objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                                objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                                objOB940InvalidData.error_desc = l_str_error_desc;
                                                objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                                lstOB940InvalidData.Add(objOB940InvalidData);
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - SR Date (21 column) is blank or invalid ";
                                            objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                            OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                            objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                            objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                            objOB940InvalidData.error_desc = l_str_error_desc;
                                            objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstOB940InvalidData.Add(objOB940InvalidData);
                                            continue;
                                        }
                                  
                                   
                                    if (CheckDate(lst_csv_data[21]))
                                    {
                                        DateTime dtStartDate;
                                        dtStartDate = Utility.ConvertToDateTime(lst_csv_data[21]);
                                        if (dtStartDate.Date > System.DateTime.Now.AddDays(90).Date)
                                        {
                                            l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Start Date (22 column) Should not be greater than system date + 90 Day(s)";
                                            objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                            OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                            objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                            objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                            objOB940InvalidData.error_desc = l_str_error_desc;
                                            objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstOB940InvalidData.Add(objOB940InvalidData);
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Start Date (22 column) is blank or invalid ";

                                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = l_str_error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);
                                        continue;
                                    }

                                        if (CheckDate(lst_csv_data[22]))
                                        {
                                            DateTime dtCancelDate;
                                            dtCancelDate = Utility.ConvertToDateTime(lst_csv_data[22]);
                                            if (dtCancelDate.Date > System.DateTime.Now.AddDays(90).Date)
                                            {
                                                l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Cancel Date (23rd column) Should not be greater than system date + 90 Day(s)";
                                                objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                                OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                                objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                                objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                                objOB940InvalidData.error_desc = l_str_error_desc;
                                                objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                                lstOB940InvalidData.Add(objOB940InvalidData);
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Start Date (22 column) is blank or invalid ";
                                            objOB940UploadFileDtl.error_desc = l_str_error_desc;
                                            OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                            objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                            objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                            objOB940InvalidData.error_desc = l_str_error_desc;
                                            objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstOB940InvalidData.Add(objOB940InvalidData);
                                            continue;
                                        }
                                            
                                    

                                    if (l_str_refno == string.Empty || l_str_refno != lst_csv_data[2].ToUpper())
                                    {
                                        l_str_hdr_data = string.Empty;
                                        OB940UploadFileHdr objOB940UploadFileHdr = new OB940UploadFileHdr();
                                        is_hdr_printed = false;

                                        objOB940UploadFileHdr.upload_ref_num = "";
                                        objOB940UploadFileHdr.cmp_id = lst_csv_data[1].Trim().ToUpper();
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[1];
                                        objOB940UploadFileHdr.ref_num = lst_csv_data[2].Trim().ToUpper();
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[2];
                                        objOB940UploadFileHdr.batch_id = lst_csv_data[3].Trim().ToUpper();
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[3];
                                        objOB940UploadFileHdr.cust_id = lst_csv_data[4].Trim().ToUpper();
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[4];
                                        objOB940UploadFileHdr.cust_ordr_num = lst_csv_data[5].Trim().ToUpper();
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[5];


                                        try
                                        {
                                            objOB940UploadFileHdr.dtl_count = Convert.ToInt16(lst_csv_data[17]);
                                        }
                                        catch
                                        {
                                            objOB940UploadFileHdr.dtl_count = 0;
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[17];
                                        if (lst_csv_data[18].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.ordr_num = lst_csv_data[18].Trim();
                                            
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[18].Trim();

                                        try
                                        {
                                            objOB940UploadFileHdr.rel_id = Convert.ToInt16(lst_csv_data[19]);
                                        }
                                        catch
                                        {
                                            objOB940UploadFileHdr.rel_id = 0;
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[19];
                                        try { objOB940UploadFileHdr.sr_dt = Utility.ConvertToDateTime(lst_csv_data[20]); }
                                        catch { objOB940UploadFileHdr.sr_dt = DateTime.Now; }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[20];
                                        try { objOB940UploadFileHdr.start_dt = Utility.ConvertToDateTime(lst_csv_data[21]); }
                                        catch { objOB940UploadFileHdr.start_dt = DateTime.Now; }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[21];
                                        try { objOB940UploadFileHdr.cancel_dt = Utility.ConvertToDateTime(lst_csv_data[22]); }
                                        catch { objOB940UploadFileHdr.cancel_dt = DateTime.Now; }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[22];


                                        if (lst_csv_data[23].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.dept_id = lst_csv_data[23].Trim().ToUpper();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[23];
                                        if (lst_csv_data[24].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.store_id = lst_csv_data[24].Trim().ToUpper();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[24];
                                        if (lst_csv_data[25].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.st_name = lst_csv_data[25].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[25];
                                        if (lst_csv_data[26].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.st_addr_line1 = lst_csv_data[26].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[26];
                                        if (lst_csv_data[27].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.st_addr_line2 = lst_csv_data[27].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[27];
                                        if (lst_csv_data[28].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.st_city = lst_csv_data[28].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[28];
                                        if (lst_csv_data[29].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.st_state_id = lst_csv_data[29].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[29];
                                        if (lst_csv_data[30].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.st_post_code = lst_csv_data[30].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[30];

                                        if (lst_csv_data[31].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.st_cntry_id = lst_csv_data[31].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[31];

                                        if (lst_csv_data[32].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.hdr_note = lst_csv_data[32].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[32];

                                        if (lst_csv_data[33].Trim().Length > 0)
                                        {
                                            objOB940UploadFileHdr.shipvia_id = lst_csv_data[33].Trim();
                                        }
                                        l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[33];

                                        if (l_str_length > 34)
                                        {
                                            
                                            if (lst_csv_data[34].Trim().Length > 0)
                                            {
                                                l_str_hdr_data = l_str_hdr_data + "|" + lst_csv_data[34];
                                                objOB940UploadFileHdr.pick_no = lst_csv_data[34].Trim();
                                                objOB940UploadFileHdr.ref_no = lst_csv_data[2].Trim();
                                                
                                            }
                                            else
                                            {
                                                objOB940UploadFileHdr.pick_no = lst_csv_data[34].Trim();
                                                objOB940UploadFileHdr.ref_no = lst_csv_data[2].Trim();
                                               
                                            }
                                        }

                                        else
                                        {
                                            objOB940UploadFileHdr.pick_no = string.Empty;
                                            objOB940UploadFileHdr.ref_no = lst_csv_data[2].Trim();
                                           
                                        }

                                            objOB940UploadFileHdr.upload_ref_num = l_str_upload_ref_num;

                                        lstOB940UploadFileHdr.Add(objOB940UploadFileHdr);
                                        l_int_line_num = 0;
                                    }



                                    l_int_line_num = l_int_line_num + 1;
                                    objOB940UploadFileDtl.line_num = l_int_line_num;

                                    l_str_refno = lst_csv_data[2].ToUpper();

                                    if (lst_csv_data[1].Trim().Length > 0)
                                    {
                                        objOB940UploadFileDtl.cmp_id = lst_csv_data[1].Trim().ToUpper();
                                    }
                                    else
                                    {
                                        objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + " Company Id Not Found";
                                    }

                                    if (lst_csv_data[2].Trim().Length > 0)
                                    {
                                        objOB940UploadFileDtl.ref_num = lst_csv_data[2].Trim().ToUpper();
                                    }
                                    else
                                    {
                                        objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + " - Referene Number not found ";
                                    }

                                    if (lst_csv_data[3].Trim().Length > 0)
                                    {
                                        objOB940UploadFileDtl.batch_id = lst_csv_data[3].Trim().ToUpper();
                                    }
                                    else
                                    {
                                        objOB940UploadFileDtl.batch_id = "-";
                                    }

                                    if (lst_csv_data[4].Length > 0)
                                    {
                                        objOB940UploadFileDtl.cust_id = lst_csv_data[4].Trim().ToUpper();
                                    }
                                    else
                                    {
                                        objOB940UploadFileDtl.cust_id = "-";
                                    }


                                    objOB940UploadFileDtl.cust_po_num = lst_csv_data[5].Trim().ToUpper();

                                    try
                                    {
                                        objOB940UploadFileDtl.pick_line_num = Convert.ToInt16(lst_csv_data[6]);
                                    }
                                    catch
                                    {
                                        objOB940UploadFileDtl.pick_line_num = 0;
                                        objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + " - Invalid Pick Line";
                                    }
                                    //Style
                                    if (lst_csv_data[7].Trim().Length > 0)
                                    {
                                        if (lst_csv_data[7].Trim().Length > 20)
                                        {
                                            objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Style (8th column) Length should be maximum of 20";
                                        }
                                        else
                                        {
                                            objOB940UploadFileDtl.itm_num = lst_csv_data[7].Trim().ToUpper();
                                        }
                                    }
                                    else
                                    {
                                        objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Style Not Found";
                                    }
                                    // Color
                                    if (lst_csv_data[8].Trim().Length > 0)
                                    {
                                        if (lst_csv_data[8].Trim().Length > 20)
                                        {
                                            objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Color (9th column) Length should be maximum of 20";
                                        }
                                        else
                                        {
                                            objOB940UploadFileDtl.itm_color = lst_csv_data[8].Trim().ToUpper();
                                        }
      
                                    }
                                    else
                                    {
                                        objOB940UploadFileDtl.itm_color = "-";
                                    }

                                    if (lst_csv_data[9].Trim().Length > 0)
                                    {
                                        if (lst_csv_data[9].Trim().Length > 20)
                                        {
                                            objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Size (10th column) Length should be maximum of 20";
                                        }
                                        else
                                        {
                                            objOB940UploadFileDtl.itm_size = lst_csv_data[9].Trim().ToUpper();
                                        }

                                    }
                                    else
                                    {
                                        objOB940UploadFileDtl.itm_size = "-";
                                    }
                                    ItemMasterService ServiceItemMaster = new ItemMasterService();
                                    ItemMaster objItemMaster = new ItemMaster();
                                    objItemMaster.cmp_id = objOB940UploadFileDtl.cmp_id;
                                    objItemMaster.itm_num = objOB940UploadFileDtl.itm_num;
                                    objItemMaster.itm_color = objOB940UploadFileDtl.itm_color;
                                    objItemMaster.itm_size = objOB940UploadFileDtl.itm_size;
                                    objItemMaster.itm_name = objOB940UploadFileDtl.itm_name;

                                    ServiceItemMaster.GetItemMasterDetails(objItemMaster);
                                    if (objItemMaster.ListItemMaster.Count == 0)
                                    {
                                        //objOB940UploadFileDtl.error_desc = "Style# " + objOB940UploadFileDtl.itm_num + " - Color# " + objOB940UploadFileDtl.itm_color + " - Size#" + objOB940UploadFileDtl.itm_size + " Not found";
                                        objOB940UploadFileDtl.error_desc = "STYLE NOT FOUND - Style# " + objOB940UploadFileDtl.itm_num + " - Color# " + objOB940UploadFileDtl.itm_color + " - Size#" + objOB940UploadFileDtl.itm_size + " Not found";
                                        Session["sesStyleNotFoud"] = "Y";
                                    }

                                    else
                                    {
                                        objOB940UploadFileDtl.itm_code = objItemMaster.ListItemMaster[0].itm_code;
                                    }

                                    if (lst_csv_data[10].Trim().Length > 0)
                                    {
                                        if (lst_csv_data[10].Trim().Length > 30)
                                        {
                                            objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Customer SKU (11th column) Length should be maximum of 30";
                                        }
                                        else
                                        {
                                            objOB940UploadFileDtl.cust_sku = lst_csv_data[10].Trim().ToUpper();
                                        }

                                    }
                                    else
                                    {
                                        objOB940UploadFileDtl.cust_sku = "-";
                                    }


                                    try
                                    {
                                        objOB940UploadFileDtl.ordr_qty = Convert.ToInt32(lst_csv_data[11]);
                                    }
                                    catch
                                    {
                                        objOB940UploadFileDtl.ordr_qty = 0;
                                        objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + " - Invalid Order Qty";
                                    }

                                    try
                                    {
                                        objOB940UploadFileDtl.ordr_ctns = Convert.ToInt32(lst_csv_data[12]);
                                    }
                                    catch
                                    {
                                        objOB940UploadFileDtl.ordr_ctns = 0;
                                        objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + " - Invalid Order Ctns";
                                    }

                                    try
                                    {
                                        objOB940UploadFileDtl.ctn_qty = Convert.ToInt32(lst_csv_data[13]);
                                    }
                                    catch
                                    {
                                        objOB940UploadFileDtl.ctn_qty = 0;
                                        objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + " - Invalid Order PPK";
                                    }

                                    try
                                    {
                                        objOB940UploadFileDtl.cube = Convert.ToDouble(lst_csv_data[14]);
                                    }
                                    catch
                                    {
                                        objOB940UploadFileDtl.cube = 0;
                                    }

                                    try
                                    {
                                        objOB940UploadFileDtl.wgt = Convert.ToDouble(lst_csv_data[15]);
                                    }
                                    catch
                                    {
                                        objOB940UploadFileDtl.wgt = 0;

                                    }

                                    if ((objOB940UploadFileDtl.error_desc.Length > 0) && (objOB940UploadFileDtl.error_desc.Contains("STYLE NOT FOUND") == false))
                                    {
                                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                        objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                        objOB940InvalidData.error_desc = objOB940UploadFileDtl.error_desc;
                                        objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstOB940InvalidData.Add(objOB940InvalidData);


                                    }

                                    else
                                    {
                                        if (objOB940UploadFileDtl.error_desc.Contains("STYLE NOT FOUND") == true)
                                        {

                                            OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                            objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                                            objOB940InvalidData.pick_line_num = objOB940UploadFileDtl.pick_line_num;
                                            objOB940InvalidData.error_desc = objOB940UploadFileDtl.error_desc;
                                            objOB940InvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstOB940InvalidData.Add(objOB940InvalidData);

                                        }
                                        if (is_hdr_printed == false)
                                        {
                                            objOB940UploadFileDtl.header_data = l_str_hdr_data;
                                            is_hdr_printed = true;
                                        }
                                        else
                                        {
                                            objOB940UploadFileDtl.header_data = string.Empty;
                                        }
                                        objOB940UploadFileDtl.upload_ref_num = l_str_upload_ref_num;
                                        lstOB940UploadFileDtl.Add(objOB940UploadFileDtl);
                                    }

                                }
                                else
                                {
                                    l_str_error_desc = "Line should starts with SLF940 ";
                                }


                            }
                        }


                        Session["lstOB940UploadFileHdr"] = lstOB940UploadFileHdr;
                        Session["lstOB940UploadFileDtl"] = lstOB940UploadFileDtl;
                        Session["lstOB940InvalidData"] = lstOB940InvalidData;
                        if (lstOB940InvalidData.Count > 0)
                        {

                            l_str_error_msg = "ERROR";
                            Session["l_str_error_msg"] = "ERROR";
                        }

                    }
                    else
                    {

                        l_str_error_msg = "Empty File ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Get_List_SPS_Data(string p_str_cmp_id, string p_str_file_path, string p_str_file_name, string p_str_file_extn, ref string l_str_error_msg)
        {
            lstOB940UploadFileHdr = new List<OB940UploadFileHdr>();
            lstOB940UploadFileDtl = new List<OB940UploadFileDtl>();
            lstOB940InvalidData = new List<OB940InvalidData>();
            string l_str_refno = string.Empty;
            string l_str_error_desc = string.Empty;
            string l_str_hdr_data = string.Empty;
            string l_str_upload_ref_num = string.Empty;
            int l_int_no_of_lines = 0;
            string p_str_cust_id = "TARGET";
            try
            {
                if (p_str_file_extn.ToUpper().Equals(".CSV"))
                {
                    List<string> lst_file_line_content = new List<string>(System.IO.File.ReadAllLines(p_str_file_path));
                    // Remove Blank lines
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
                        OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
                        l_str_upload_ref_num = Convert.ToString(ServiceOB940UploadFile.Get940UploadRefNum(p_str_cmp_id));
                        l_int_no_of_lines = lst_file_line_content.Count();
                        objOB940UploadFileInfo = new OB940UploadFileInfo();
                        objOB940UploadFileInfo.cmp_id = p_str_cmp_id;
                        objOB940UploadFileInfo.file_name = p_str_file_name;
                        objOB940UploadFileInfo.upload_by = Session["UserID"].ToString().Trim();
                        objOB940UploadFileInfo.upload_date_time = DateTime.Now;
                        objOB940UploadFileInfo.no_of_lines = l_int_no_of_lines;
                        objOB940UploadFileInfo.status = "PEND";
                        objOB940UploadFileInfo.upload_ref_num = l_str_upload_ref_num;
                        Session["objOB940UploadFileInfo"] = objOB940UploadFileInfo;
                        csvFileProcess FileProcess = new csvFileProcess();
                        cls_ob_940_sbs objOB940SBS = new cls_ob_940_sbs();
                        objOB940SBS= FileProcess.processCSVFile(p_str_cmp_id, l_str_upload_ref_num, p_str_file_path);
                        fnMoveSPStoGS(p_str_cmp_id, p_str_cust_id, l_str_upload_ref_num, objOB940SBS);
                        Session["lstOB940UploadFileHdr"] = lstOB940UploadFileHdr;
                        Session["lstOB940UploadFileDtl"] = lstOB940UploadFileDtl;
                        Session["lstOB940InvalidData"] = lstOB940InvalidData;
                        if (lstOB940InvalidData.Count > 0)
                        {

                            l_str_error_msg = "ERROR";
                            Session["l_str_error_msg"] = "ERROR";
                        }

                    }
                }
                else
                {

                    l_str_error_msg = "Invalid File";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void fnMoveSPStoGS(string p_str_cmp_id, string p_str_cust_id, string p_str_batch_id, cls_ob_940_sbs objOB940SBS)
        {

            lstOB940UploadFileHdr = new List<OB940UploadFileHdr>();
            lstOB940UploadFileDtl = new List<OB940UploadFileDtl>();
            lstOB940InvalidData = new List<OB940InvalidData>();

            string l_str_error_desc = string.Empty;
            string l_str_po_num = string.Empty;
            for (int i = 0; i < objOB940SBS.lstOB940SBSHdr.Count; i++)
            {
                OB940UploadFileDtl objOB940UploadFileDtl = new OB940UploadFileDtl();
                l_str_po_num = objOB940SBS.lstOB940SBSHdr[i].po_num.Trim();
                if (objOB940SBS.lstOB940SBSHdr[i].po_num.Length > 30)
                {
                    l_str_error_desc = "Cust Order Number# " + objOB940SBS.lstOB940SBSHdr[i].po_num + " should be maximum of 30 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].retailer_po_num.Length > 20)
                {
                    l_str_error_desc = "Retailers PO # " + objOB940SBS.lstOB940SBSHdr[i].retailer_po_num + " should be maximum of 20 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }
                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_name.Length > 50)
                {
                    l_str_error_desc = "Dept #" + objOB940SBS.lstOB940SBSHdr[i].dept_no + " should be maximum of 50 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[0].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].bill_to_name.Length > 50)
                {
                    l_str_error_desc = "Bill To Name " + objOB940SBS.lstOB940SBSHdr[i].bill_to_name + " should be maximum of 50 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_name.Length > 50)
                {
                    l_str_error_desc = "Ship To Name " + objOB940SBS.lstOB940SBSHdr[i].ship_to_name + " should be maximum of 50 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }
                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_addr1.Length > 50)
                {
                    l_str_error_desc = "Ship To Address 1 " + objOB940SBS.lstOB940SBSHdr[i].ship_to_addr1 + " should be maximum of 50 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_addr2.Length > 50)
                {
                    l_str_error_desc = "Ship To Address 2 " + objOB940SBS.lstOB940SBSHdr[i].ship_to_addr2 + " should be maximum of 50 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_city.Length > 30)
                {
                    l_str_error_desc = "Ship to City " + objOB940SBS.lstOB940SBSHdr[i].ship_to_city + " should be maximum of 30 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }
                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_state.Length > 10)
                {
                    l_str_error_desc = "Ship to State " + objOB940SBS.lstOB940SBSHdr[i].ship_to_state + " should be maximum of 10 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_zip.Length > 20)
                {
                    l_str_error_desc = "Ship to Zip " + objOB940SBS.lstOB940SBSHdr[i].ship_to_zip + " should be maximum of 20 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_cntry.Length > 10)
                {
                    l_str_error_desc = "Ship to Country " + objOB940SBS.lstOB940SBSHdr[i].ship_to_cntry + " should be maximum of 10 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].notes1.Length > 200)
                {
                    l_str_error_desc = "Notes/Comments: " + objOB940SBS.lstOB940SBSHdr[i].notes1 + " should be maximum of 200 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (objOB940SBS.lstOB940SBSHdr[i].carrier.Length > 10)
                {
                    l_str_error_desc = "Carrier: " + objOB940SBS.lstOB940SBSHdr[i].carrier + " should be maximum of 10 ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                    continue;
                }

                if (CheckDate(objOB940SBS.lstOB940SBSHdr[i].po_dt))
                {
                    DateTime dtSRDate;
                    dtSRDate = Utility.ConvertToDateTime(objOB940SBS.lstOB940SBSHdr[i].po_dt);
                    if (dtSRDate.Date > System.DateTime.Now.Date)
                    {
                        l_str_error_desc = "PO Date " + objOB940SBS.lstOB940SBSHdr[i].po_dt + " Should not be greater than system date ";
                        objOB940UploadFileDtl.error_desc = l_str_error_desc;
                        OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                        objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                        objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                        objOB940InvalidData.error_desc = l_str_error_desc;
                        objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                        lstOB940InvalidData.Add(objOB940InvalidData);
                        continue;
                    }

                }
                else
                {
                    l_str_error_desc = "PO Date " + objOB940SBS.lstOB940SBSHdr[i].po_dt + " is blank or invalid ";
                    objOB940UploadFileDtl.error_desc = l_str_error_desc;
                    OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                    objOB940InvalidData.ref_num = objOB940UploadFileDtl.ref_num;
                    objOB940InvalidData.pick_line_num = objOB940SBS.lstOB940SBSHdr[i].line_num;
                    objOB940InvalidData.error_desc = l_str_error_desc;
                    objOB940InvalidData.line_data = "Line#" + objOB940SBS.lstOB940SBSHdr[i].line_num + " | " + objOB940SBS.lstOB940SBSHdr[0].po_num;
                    lstOB940InvalidData.Add(objOB940InvalidData);
                }

                OB940UploadFileHdr objOB940UploadFileHdr = new OB940UploadFileHdr();
                string l_str_hdr_data = string.Empty;
                objOB940UploadFileHdr.upload_ref_num = "";
                objOB940UploadFileHdr.cmp_id = p_str_cmp_id;
                l_str_hdr_data = l_str_hdr_data + "|PO#" + objOB940SBS.lstOB940SBSHdr[i].po_num;
                objOB940UploadFileHdr.ref_num = objOB940SBS.lstOB940SBSHdr[i].release_no;
                objOB940UploadFileHdr.batch_id = p_str_batch_id;

                if (objOB940SBS.lstOB940SBSHdr[i].bill_to_name.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.cust_id = objOB940SBS.lstOB940SBSHdr[i].bill_to_name.Trim();
                }
                else
                {
                    objOB940UploadFileHdr.cust_id = "-";
                }


                objOB940UploadFileHdr.cust_ordr_num = objOB940SBS.lstOB940SBSHdr[i].retailer_po_num;
                l_str_hdr_data = l_str_hdr_data + "|Cust-Order#" + objOB940SBS.lstOB940SBSHdr[i].retailer_po_num;
                objOB940UploadFileHdr.dtl_count = objOB940UploadFileHdr.dtl_count + 1;

                if (objOB940SBS.lstOB940SBSHdr[i].po_num.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.ordr_num = objOB940SBS.lstOB940SBSHdr[i].po_num.Trim();
                }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940UploadFileHdr.ordr_num;

                objOB940UploadFileHdr.rel_id = 0;

                try { objOB940UploadFileHdr.sr_dt = Utility.ConvertToDateTime(objOB940SBS.lstOB940SBSHdr[i].po_dt); }
                catch { objOB940UploadFileHdr.sr_dt = DateTime.Now; }
                l_str_hdr_data = l_str_hdr_data + "|Date#" + objOB940SBS.lstOB940SBSHdr[i].po_dt;

                try { objOB940UploadFileHdr.start_dt = Utility.ConvertToDateTime(objOB940SBS.lstOB940SBSHdr[i].req_shipment_dt); }
                catch { objOB940UploadFileHdr.start_dt = DateTime.Now; }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940SBS.lstOB940SBSHdr[i].req_shipment_dt;
                try { objOB940UploadFileHdr.cancel_dt = Utility.ConvertToDateTime(objOB940SBS.lstOB940SBSHdr[i].req_delivery_dt); }
                catch { objOB940UploadFileHdr.cancel_dt = DateTime.Now; }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940SBS.lstOB940SBSHdr[i].req_delivery_dt;


                if (objOB940SBS.lstOB940SBSHdr[i].dept_no.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.dept_id = objOB940SBS.lstOB940SBSHdr[i].dept_no.Trim().ToUpper();
                }
                if (objOB940SBS.lstOB940SBSHdr[i].carrier.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.shipvia_id = objOB940SBS.lstOB940SBSHdr[i].carrier.Trim().ToUpper();
                }

               
                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_name.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.st_name = objOB940SBS.lstOB940SBSHdr[i].ship_to_name.Trim();
                }
                l_str_hdr_data = l_str_hdr_data + "|ShipTo#" + objOB940SBS.lstOB940SBSHdr[i].ship_to_name.Trim();

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_addr1.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.st_addr_line1 = objOB940SBS.lstOB940SBSHdr[i].ship_to_addr1.Trim();
                }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940SBS.lstOB940SBSHdr[i].ship_to_addr1.Trim();

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_addr2.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.st_addr_line2 = objOB940SBS.lstOB940SBSHdr[i].ship_to_addr2.Trim();
                }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940SBS.lstOB940SBSHdr[i].ship_to_addr2.Trim();


                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_city.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.st_city = objOB940SBS.lstOB940SBSHdr[i].ship_to_city.Trim();
                }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940SBS.lstOB940SBSHdr[i].ship_to_city.Trim();

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_state.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.st_state_id = objOB940SBS.lstOB940SBSHdr[i].ship_to_state.Trim();
                }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940SBS.lstOB940SBSHdr[i].ship_to_state.Trim();

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_cntry.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.st_cntry_id = objOB940SBS.lstOB940SBSHdr[i].ship_to_cntry.Trim();
                }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940SBS.lstOB940SBSHdr[i].ship_to_cntry.Trim();

                if (objOB940SBS.lstOB940SBSHdr[i].ship_to_zip.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.st_post_code = objOB940SBS.lstOB940SBSHdr[i].ship_to_zip.Trim();
                }
                l_str_hdr_data = l_str_hdr_data + "|" + objOB940SBS.lstOB940SBSHdr[i].ship_to_zip.Trim();

                if (objOB940SBS.lstOB940SBSHdr[i].other_info.Trim().Length > 0)
                {
                    objOB940UploadFileHdr.hdr_note = objOB940SBS.lstOB940SBSHdr[i].other_info.Trim();
                }


                objOB940UploadFileHdr.upload_ref_num = p_str_batch_id;

                lstOB940UploadFileHdr.Add(objOB940UploadFileHdr);
               int line_num = 0;
                for (int j = 0; j < objOB940SBS.lstOB940SBSDtl.Count; j++)
                {
                    if (objOB940SBS.lstOB940SBSDtl[j].po_num == l_str_po_num)
                    {
                        OB940UploadFileDtl objOB940UploadFileDtlLine = new OB940UploadFileDtl();
                        line_num += 1;
                        objOB940UploadFileDtlLine.cmp_id = p_str_cmp_id;
                        objOB940UploadFileDtlLine.ref_num = objOB940UploadFileHdr.ref_num;
                        //objOB940UploadFileDtlLine.error_desc = objOB940UploadFileDtl.error_desc;
                        objOB940UploadFileDtlLine.batch_id = p_str_batch_id;
                        objOB940UploadFileDtlLine.cust_id = p_str_cust_id;
                        objOB940UploadFileDtlLine.line_num = line_num;
                        objOB940UploadFileDtlLine.pick_line_num = objOB940SBS.lstOB940SBSDtl[j].po_line_num;
                        objOB940UploadFileDtlLine.cust_po_num = objOB940SBS.lstOB940SBSHdr[i].retailer_po_num;

                        //Style
                        if (objOB940SBS.lstOB940SBSDtl[j].sku_num.Trim().Length > 0)
                        {
                            if (objOB940SBS.lstOB940SBSDtl[j].sku_num.Trim().Length > 20)
                            {
                                objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Style  Length should be maximum of 20";
                            }
                            else
                            {
                                objOB940UploadFileDtlLine.itm_num = objOB940SBS.lstOB940SBSDtl[j].sku_num.Trim().ToUpper();
                            }
                        }
                        else
                        {
                            objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Style Not Found";
                        }

                        objOB940UploadFileDtlLine.itm_color = "-";
                        objOB940UploadFileDtlLine.itm_size = "-";
                        

                        if (objOB940SBS.lstOB940SBSDtl[j].itm_name.Trim().Length > 0)
                        {
                            if (objOB940SBS.lstOB940SBSDtl[j].itm_name.Trim().Length > 75)
                            {
                                objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Item Name should be maximum of 75";
                            }
                            else
                            {
                                objOB940UploadFileDtlLine.itm_name = objOB940SBS.lstOB940SBSDtl[j].itm_name.Trim().ToUpper();
                            }
                        }

                        ItemMasterService ServiceItemMaster = new ItemMasterService();
                        ItemMaster objItemMaster = new ItemMaster();
                        objItemMaster.cmp_id = objOB940UploadFileDtlLine.cmp_id;
                        objItemMaster.itm_num = objOB940UploadFileDtlLine.itm_num;
                        objItemMaster.itm_color = objOB940UploadFileDtlLine.itm_color;
                        objItemMaster.itm_size = objOB940UploadFileDtlLine.itm_size;
                       // objItemMaster.itm_name = objOB940UploadFileDtlLine.itm_name;

                        ServiceItemMaster.GetItemMasterDetails(objItemMaster);
                        if (objItemMaster.ListItemMaster.Count == 0)
                        {
                            objOB940UploadFileDtl.error_desc = "STYLE NOT FOUND - Style# " + objOB940UploadFileDtlLine.itm_num + " - Color# " + objOB940UploadFileDtlLine.itm_color + " - Size#" + objOB940UploadFileDtlLine.itm_size + " Not found";
                            Session["sesStyleNotFoud"] = "Y";
                        }
                        else
                        {
                            objOB940UploadFileDtlLine.itm_code = objItemMaster.ListItemMaster[0].itm_code;
                        }


                        if (objOB940SBS.lstOB940SBSDtl[j].byer_catalog_or_sku.Trim().Length > 0)
                        {
                            if (objOB940SBS.lstOB940SBSDtl[j].byer_catalog_or_sku.Trim().Length > 20)
                            {
                                objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + "Buyer Catalog or SKU Length should be maximum of 20";
                            }
                            else
                            {
                                objOB940UploadFileDtlLine.cust_sku = objOB940SBS.lstOB940SBSDtl[j].byer_catalog_or_sku.Trim().ToUpper();
                            }
                        }
                        else
                        {
                            objOB940UploadFileDtlLine.cust_sku = "-";
                        }

                        try
                        {
                            objOB940UploadFileDtlLine.ordr_qty = Convert.ToInt32(objOB940SBS.lstOB940SBSDtl[j].ordr_qty);
                        }
                        catch
                        {
                            objOB940UploadFileDtlLine.ordr_qty = 0;
                            objOB940UploadFileDtl.error_desc = objOB940UploadFileDtl.error_desc + " - Invalid Order Qty";
                        }

                       

                        if (objOB940SBS.lstOB940SBSDtl[j].pack_size.Trim().Length > 0)
                        {
                            try
                            {
                                objOB940UploadFileDtlLine.ctn_qty = Convert.ToInt32(objOB940SBS.lstOB940SBSDtl[j].pack_size);

                            }
                            catch
                            {
                                objOB940UploadFileDtlLine.ctn_qty = 1;
                            }
                        }
                        else

                        {
                            objOB940UploadFileDtlLine.ctn_qty = 1;
                        }
                        
                            objOB940UploadFileDtlLine.ordr_ctns = objOB940UploadFileDtlLine.ordr_qty / objOB940UploadFileDtlLine.ctn_qty;
                 
                        objOB940UploadFileDtlLine.cube = 0;
                        objOB940UploadFileDtlLine.wgt = 0;

                        if ((objOB940UploadFileDtl.error_desc.Length > 0) && (objOB940UploadFileDtl.error_desc.Contains("STYLE NOT FOUND") == false))
                        {
                            OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                            objOB940InvalidData.ref_num = objOB940UploadFileHdr.ref_num;
                            objOB940InvalidData.pick_line_num = objOB940UploadFileDtlLine.pick_line_num;
                            objOB940InvalidData.error_desc = objOB940UploadFileDtl.error_desc;
                            objOB940InvalidData.line_data = "Ref#" + objOB940UploadFileDtlLine.ref_num + "|Line#" + objOB940SBS.lstOB940SBSDtl[j].line_num.ToString() + "| Style#" + objOB940UploadFileDtlLine.itm_num + "|Qty#" + objOB940UploadFileDtlLine.ordr_qty;
                            lstOB940InvalidData.Add(objOB940InvalidData);
                        }

                        else
                        {
                            if (objOB940UploadFileDtl.error_desc.Contains( "STYLE NOT FOUND") == true)
                            {
                                OB940InvalidData objOB940InvalidData = new OB940InvalidData();
                                objOB940InvalidData.ref_num = objOB940UploadFileHdr.ref_num;
                                objOB940InvalidData.pick_line_num = objOB940UploadFileDtlLine.pick_line_num;
                                objOB940InvalidData.error_desc = objOB940UploadFileDtl.error_desc;
                                objOB940InvalidData.line_data = "Ref#" + objOB940UploadFileDtlLine.ref_num + "|Line#" + objOB940SBS.lstOB940SBSDtl[j].line_num.ToString() + "| Style#" + objOB940UploadFileDtlLine.itm_num + "|Qty#" + objOB940UploadFileDtlLine.ordr_qty;
                                lstOB940InvalidData.Add(objOB940InvalidData);


                            }

                            if (line_num == 1)
                            {
                                objOB940UploadFileDtlLine.header_data = l_str_hdr_data;
                               
                            }
                            else
                            {
                                objOB940UploadFileDtlLine.header_data = string.Empty;
                            }
                            objOB940UploadFileDtlLine.upload_ref_num = p_str_batch_id;
                            lstOB940UploadFileDtl.Add(objOB940UploadFileDtlLine);
                        }

                    }
                }
            }
        }

           
        


        public ActionResult UploadFiles(string l_str_cmp_id,string pstrUploadLink)
        {
            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
            Session["objOB940UploadFileInfo"] = "";
            Session["lstOB940UploadFileDtl"] = "";
            Session["lstOB940UploadFileHdr"] = "";
            Session["sesStyleNotFoud"] = "N";
            Session["sesAllowNewItem"] = "0";
            objOB940UploadFile.error_mode = false;
            if (Request.Files.Count > 0)
            {
                try
                {
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
                                if (fileName.Length > 100)
                                {
                                    objOB940UploadFile.error_mode = true;
                                    objOB940UploadFile.error_desc = "File Name length should be maximum of 100 ";
                                    return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
                                }
                                string l_str_file_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/") + System.Configuration.ConfigurationManager.AppSettings["tempUploadFile"].ToString().Trim(), fileName);
                                //Try and upload
                                try
                                {
                                    FileUpload.SaveAs(l_str_file_path);
                                    string l_str_ext = Path.GetExtension(fileName);
                                    if (l_str_ext.ToUpper() != ".CSV")
                                    {
                                        objOB940UploadFile.error_mode = true;
                                        objOB940UploadFile.error_desc = "Invalid File Format";
                                        return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
                                    }

                                    DataTable dtCustConfig = new DataTable();
                                    OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
                                    dtCustConfig = ServiceOB940UploadFile.fnGetCustConfig(l_str_cmp_id);
                                    if (dtCustConfig.Rows.Count > 0)
                                    {
                                        objOB940UploadFile.allow_940_new_item = dtCustConfig.Rows[0]["allow_940_new_item"].ToString();
                                        Session["sesAllowNewItem"] = objOB940UploadFile.allow_940_new_item;
                                    }

                                    if (pstrUploadLink =="940SPSSRUpload")
                                    {
                                        Get_List_SPS_Data(l_str_cmp_id, l_str_file_path, fileName, ".CSV", ref l_str_error_msg);
                                    }
                                    else
                                    {
                                        Get_List_Data(l_str_cmp_id, l_str_file_path, fileName, ".CSV", ref l_str_error_msg);


                                    }
                                    if (l_str_error_msg != "")
                                    {
                                        if (l_str_error_msg == "MISMATCH-CMP-ID")
                                        {
                                            objOB940UploadFile.error_mode = true;
                                            objOB940UploadFile.error_desc = l_str_error_msg;
                                            return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            objOB940UploadFile.error_mode = true;
                                        }
                                     
                                    }
                                   

                                    objOB940UploadFile.ListOB940UploadFileHdr = lstOB940UploadFileHdr;
                                    objOB940UploadFile.ListOB940UploadFileDtl = lstOB940UploadFileDtl;
                                    objOB940UploadFile.ListOB940InvalidData = lstOB940InvalidData;
                                    ViewBag.l_int_error_count = lstOB940InvalidData.Count;
                                    Mapper.CreateMap<OB940UploadFile, OB940UploadFileModel>();
                                    OB940UploadFileModel objOB940UploadFileModel = Mapper.Map<OB940UploadFile, OB940UploadFileModel>(objOB940UploadFile);
                                    return PartialView("_OB940UploadFile", objOB940UploadFileModel);

                                    //END                                   
                                }
                                catch (Exception ex)
                                {
                                    //Catch errors
                                    //  ViewData["Feedback"] = ex.Message;
                                    objOB940UploadFile.error_mode = true;
                                    objOB940UploadFile.error_desc = ex.InnerException.ToString();


                                    return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }


                        else
                        {
                            //Catch errors
                            objOB940UploadFile.error_mode = true;
                            objOB940UploadFile.error_desc = "Please select a file";
                            return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
                        }



                    }
                }
                catch (Exception ex)
                {
                    objOB940UploadFile.error_mode = true;
                    objOB940UploadFile.error_desc = ex.Message;
                    return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objOB940UploadFile.error_mode = true;
                objOB940UploadFile.error_desc = "No files selected.";
                return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
            }
            Mapper.CreateMap<OB940UploadFile, OB940UploadFileModel>();
            OB940UploadFileModel objOB940UploadFileModel1 = Mapper.Map<OB940UploadFile, OB940UploadFileModel>(objOB940UploadFile);
            return PartialView("_OB940UploadFile", objOB940UploadFileModel1);

        }
        private void fnSaveOB940Doc(string pstrCmpId, string pstrUploadRefNo, string pstrFileName, string pstrFullFileName)
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
            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();

            l_str_config_file_path = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
            l_str_file_extn = System.IO.Path.GetExtension(pstrFileName).ToLower();

            DataTable dtIBList = new DataTable();
            dtIBList = ServiceOB940UploadFile.fnGetOBDocListBy940(pstrCmpId, pstrUploadRefNo);

            string lstrDocId = string.Empty;
            for (int i = 0; i < dtIBList.Rows.Count; i++)
            {
                lstrDocId = dtIBList.Rows[i]["so_num"].ToString();
                l_str_sub_folder = lstrDocId.Substring(0, 3);
                l_str_upload_file = string.Format("0-940-" + pstrCmpId + "-" + lstrDocId + "-" + DateTime.Now.ToString("yyyyMMddHHssmm.") + l_str_file_extn);
                l_str_folder_path = pstrCmpId.Trim() + "\\OB\\" + l_str_sub_folder + "\\" + lstrDocId;

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
                objDocumentUpload.doc_type = "OB";
                objDocumentUpload.orig_file_name = pstrFileName;
                objDocumentUpload.upload_file_name = l_str_upload_file;
                objDocumentUpload.file_path = l_str_folder_path;
                objDocumentUpload.upload_by = Session["UserID"].ToString().Trim();
                objDocumentUpload.comments = "940 Upload";
                objDocumentUpload.upload_dt = DateTime.Now;
                objDocumentUpload.doc_sub_type = "940";
                serviceDocumentUpload.SaveDocumentUpload(objDocumentUpload);

            }


        }
        public ActionResult SaveOB940UploadFile(string p_str_cmp_id, string p_str_file_name, string pstrTransferCmpId )
        {
            string result = string.Empty;
            int l_int_hdr_count = 0;
            string l_str_ref_num = string.Empty;
            string l_str_batch_id = string.Empty;

            string l_str_duplicate_ref_num = string.Empty;

            DataTable p_dt_ob_940_upload_file_info = new DataTable();
            DataTable p_dt_ob_940_upload_file_hdr = new DataTable();
            DataTable p_dt_ob_940_upload_file_dtl = new DataTable();

            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();

            objOB940UploadFile.error_mode = false;
            objOB940UploadFile.cmp_id = p_str_cmp_id;

            List<OB940UploadFileHdr> lstOB940UploadFileHdr = new List<OB940UploadFileHdr>();
            lstOB940UploadFileHdr = Session["lstOB940UploadFileHdr"] as List<OB940UploadFileHdr>;
            p_dt_ob_940_upload_file_hdr = Utility.ConvertListToDataTable(lstOB940UploadFileHdr);


            l_int_hdr_count = p_dt_ob_940_upload_file_hdr.Rows.Count;
            for (int i = 0; i < l_int_hdr_count; i++)
            {
                if (ServiceOB940UploadFile.CheckRefNumExists(p_str_cmp_id, p_dt_ob_940_upload_file_hdr.Rows[i]["ref_num"].ToString()) == true)
                {
                    l_str_duplicate_ref_num = l_str_duplicate_ref_num + p_dt_ob_940_upload_file_hdr.Rows[i]["ref_num"].ToString() + "|";
                }

                if ((p_dt_ob_940_upload_file_hdr.Rows[i]["batch_id"].ToString() == string.Empty) || (p_dt_ob_940_upload_file_hdr.Rows[i]["batch_id"].ToString() == "-"))
                {
                    if (l_str_batch_id.Length == 0)
                    {
                        l_str_batch_id = ServiceOB940UploadFile.Get940BatchId(p_str_cmp_id);
                        Session["ses_str_batch_id"] = l_str_batch_id;
                    }

                    p_dt_ob_940_upload_file_hdr.Rows[i]["batch_id"] = l_str_batch_id;

                }

            }
            if (l_str_duplicate_ref_num.Length > 0)
            {
                objOB940UploadFile.error_mode = true;
                objOB940UploadFile.error_desc = "Ref Num[s]" + l_str_duplicate_ref_num + " Already exists ";
                ViewBag.l_str_error_desc = "Ref Num[s]" + l_str_duplicate_ref_num + " Already exists ";
                return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
            }

            else

            {
                objOB940UploadFile.error_mode = false;
                objOB940UploadFile.error_desc = "NO";
                ViewBag.l_str_error_desc = "NO";
            }



            OB940UploadFileInfo objOB940UploadFileInfo = new OB940UploadFileInfo();
            objOB940UploadFileInfo = Session["objOB940UploadFileInfo"] as OB940UploadFileInfo;
            p_dt_ob_940_upload_file_info = Utility.ObjectToDataTable(objOB940UploadFileInfo);






            List<OB940UploadFileDtl> lstOB940UploadFileDtl = new List<OB940UploadFileDtl>();
            lstOB940UploadFileDtl = Session["lstOB940UploadFileDtl"] as List<OB940UploadFileDtl>;
            if ((Session["sesStyleNotFoud"].ToString() == "Y") && (Session["sesAllowNewItem"].ToString() == "1"))
            {
                for (int i = 0; i < lstOB940UploadFileDtl.Count; i++)
                {
                    OB940UploadFileDtl objOB940UploadFileDtl = lstOB940UploadFileDtl[i];

                    if ((objOB940UploadFileDtl.itm_code == null) || (objOB940UploadFileDtl.itm_code == string.Empty))
                    {
                        ItemMasterService ServiceItemMaster = new ItemMasterService();
                        ItemMaster objItemMaster = new ItemMaster();
                        objItemMaster.cmp_id = objOB940UploadFileDtl.cmp_id;
                        objItemMaster.itm_num = objOB940UploadFileDtl.itm_num;
                        objItemMaster.itm_color = objOB940UploadFileDtl.itm_color;
                        objItemMaster.itm_size = objOB940UploadFileDtl.itm_size;
                        objItemMaster.itm_name = objOB940UploadFileDtl.itm_name;
                        objItemMaster.ctn_qty = objOB940UploadFileDtl.ctn_qty;

                        ServiceItemMaster.GetItemMasterDetails(objItemMaster);
                        if (objItemMaster.ListItemMaster.Count == 0)
                        {
                            lstOB940UploadFileDtl[i].itm_code = ServiceItemMaster.fnAddNew940Item(objItemMaster);
                        }

                        else
                        {
                            lstOB940UploadFileDtl[i].itm_code = objItemMaster.ListItemMaster[0].itm_code;
                        }
                    }
                }
            }

            p_dt_ob_940_upload_file_dtl = Utility.ConvertListToDataTable(lstOB940UploadFileDtl);



            result = ServiceOB940UploadFile.SaveOB940UploadFile(p_str_cmp_id, p_dt_ob_940_upload_file_info, p_dt_ob_940_upload_file_hdr, p_dt_ob_940_upload_file_dtl);

            if (result == "OK")
            {
                result = ServiceOB940UploadFile.MoveOB940UploadToSOTables(p_str_cmp_id, p_dt_ob_940_upload_file_info.Rows[0]["upload_ref_num"].ToString());
                Session["ses_upload_ref_num"] = p_dt_ob_940_upload_file_info.Rows[0]["upload_ref_num"].ToString();
                if (pstrTransferCmpId.Length > 0)
                {
                   if (ServiceOB940UploadFile.fnGenerate943ForTransCmpId(p_str_cmp_id, p_dt_ob_940_upload_file_info.Rows[0]["upload_ref_num"].ToString(),  pstrTransferCmpId) == true)
                    {
                        result = "OK";
                    }
                }

            }
            else
            {
                Session["objOB940UploadFileInfo"] = "";
                Session["lstOB940UploadFileDtl"] = "";
                Session["lstOB940UploadFileHdr"] = "";
                objOB940UploadFile.error_mode = true;
                objOB940UploadFile.error_desc = result;
                return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);

            }

            if (result == "OK")
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/") + Path.Combine(System.Configuration.ConfigurationManager.AppSettings["tempUploadFile"].ToString().Trim(), p_str_file_name);
                string path2 = System.Configuration.ConfigurationManager.AppSettings["Docpath"].ToString().Trim();
                string strFullPath = string.Empty;
                path2 = Path.Combine(path2, p_str_cmp_id, "UPLOAD", "OB940");

                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                strFullPath = Path.Combine(path2, p_str_file_name);

                if (!System.IO.File.Exists(strFullPath))
                {
                    System.IO.File.Move(path, strFullPath);
                    fnSaveOB940Doc(p_str_cmp_id, Session["ses_upload_ref_num"].ToString(), p_str_file_name, strFullPath);
                }
                else
                {
                    string l_str_FileNameOnly = p_str_file_name.Substring(0, p_str_file_name.LastIndexOf("."));
                    string path3 = Path.Combine(path2, l_str_FileNameOnly + "-" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".csv");
                    System.IO.File.Move(path, path3);
                    fnSaveOB940Doc(p_str_cmp_id, Session["ses_upload_ref_num"].ToString(), p_str_file_name, path3);
                }
                Session["objOB940UploadFileInfo"] = "";
                Session["lstOB940UploadFileDtl"] = "";
                Session["lstOB940UploadFileHdr"] = "";
                objOB940UploadFile.error_mode = false;
                Session["g_str_cmp_id"] = p_str_cmp_id;
                clsRptEmail objRptEmail = new clsRptEmail();
                bool lblnRptEmailExists = false;
                Email objEmail = new Email();
                objRptEmail.getEmailDetails(objEmail, p_str_cmp_id, "OUTBOUND", "OB-940-ACK", ref lblnRptEmailExists);
                if (lblnRptEmailExists == true)
                {
                    objOB940UploadFile.email_auto_sent = objEmail.is_auto_email;
                }
                else
                {
                    objOB940UploadFile.email_auto_sent = "N";
                }

                Mapper.CreateMap<OB940UploadFile, OB940UploadFileModel>();
                OB940UploadFileModel objOB940UploadFileModel = Mapper.Map<OB940UploadFile, OB940UploadFileModel>(objOB940UploadFile);
                return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
            }
            else
            {
                objOB940UploadFile.error_mode = true;
                objOB940UploadFile.error_desc = result;
                return Json(objOB940UploadFile, JsonRequestBehavior.AllowGet);
            }



        }

        public ActionResult Show940SRReport(string p_str_cmpid, string p_str_filename, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_batch_id)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_so_num = string.Empty;
            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
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
                  
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                    objOB940UploadFile.cmp_id = p_str_cmpid.Trim();
                    if (p_str_filename != null && p_str_filename != "")
                    {
                        if (p_str_filename.Contains(".csv"))
                        {
                            objOB940UploadFile.file_name = p_str_filename.Trim();
                        }
                        else
                        {
                            objOB940UploadFile.file_name = p_str_filename.Trim() + ".csv";
                        }
                    }
                    else
                    {
                        objOB940UploadFile.file_name = p_str_filename.Trim();
                    }
                    objOB940UploadFile.upload_dt_from = p_str_so_dt_frm.Trim();
                    objOB940UploadFile.upload_dt_to = p_str_so_dt_to.Trim();
                    objOB940UploadFile.batch_id = p_str_batch_id.Trim();


                    objOB940UploadFile = ServiceOB940UploadFile.GetOB940UploadDtlRptData(objOB940UploadFile, string.Empty, string.Empty);

                    if (objOB940UploadFile.ListOB940UploadDtlRpt.Count == 0)
                    {
                        Response.Write("<H2>No Data found</H2>");
                    }
                    else
                    {
                        IList <Core.Entity.OB940Report> rptSource1 = objOB940UploadFile.ListOB940UploadDtlRpt.ToList();
                            using (ReportDocument rd = new ReportDocument())
                            {
                                rd.Load(strRptPath);
                          
                                objCompany.cmp_id = p_str_cmpid.Trim();
                                objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                                objOB940UploadFile.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                                if (rptSource1 != null && rptSource1.GetType().ToString() != "System.String")
                                    rd.SetDataSource(rptSource1);
                                    objOB940UploadFile.cust_logo_path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                                    rd.SetParameterValue("fml_image_path", objOB940UploadFile.cust_logo_path);
                                    rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                                    rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                                    rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                                    rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                                    rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                                    rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                                    rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                                    rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
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

        public ActionResult Show940SRExcelReport(string p_str_cmpid, string p_str_filename, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_batch_id)
        {
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            string strDateFormat = string.Concat(DateTime.Now.Year, "_", DateTime.Now.ToString("MM"), "_", DateTime.Now.ToString("dd"));
            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_so_num = string.Empty;
            string l_str_process_id = string.Empty;

            string l_str_quote_num = string.Empty;
            string l_str_ordr_num = string.Empty;
            string l_str_store_id = string.Empty;
            string l_str_ship_dt = string.Empty;
            string l_str_cust_ordr_num = string.Empty;
            string l_str_shipto_id = string.Empty;
            string l_str_dept_id = string.Empty;
            string l_str_cancel_dt = string.Empty;
            string l_str_cust_name = string.Empty;
            string l_str_dc_id = string.Empty;
            string l_str_st_mail_name = string.Empty;
            string l_str_so_dt_frm = string.Empty;
            string l_str_so_dt_to = string.Empty;

            DataTable dtOB940 = new DataTable();
            DateTime dt_ship;
            DateTime dt_can;
            if (p_str_batch_id == string.Empty)
            {
                p_str_batch_id = Session["ses_str_batch_id"].ToString();
            }
            

            if (p_str_filename != null && p_str_filename != "")
            {
                if (p_str_filename.Contains(".csv"))
                {
                    l_str_file_name = p_str_filename.Trim();
                }
                else
                {
                    l_str_file_name = p_str_filename.Trim() + ".csv";
                }
            }
            else
            {
                l_str_file_name = p_str_filename.Trim();
            }
            l_str_so_dt_frm = p_str_so_dt_frm.Trim();
            l_str_so_dt_to = p_str_so_dt_to.Trim();
            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
            dtOB940 = ServiceOB940UploadFile.GetOB940UploadDtlRptDataExcel(p_str_cmpid, l_str_file_name, p_str_batch_id, l_str_so_dt_frm, l_str_so_dt_to, l_str_process_id, l_str_so_num);

            if (!Directory.Exists(strOutputpath))
            {
                Directory.CreateDirectory(strOutputpath);
            }

            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_SR940_SUMMARY_" + strDateFormat + ".xlsx";

            tempFileName = strOutputpath + l_str_file_name;

            if (System.IO.File.Exists(tempFileName))
                System.IO.File.Delete(tempFileName);
            xls_OB_SR940_Summary mxcel1 = new xls_OB_SR940_Summary(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_SR940_SUMMARY-BY-BATCH.xlsx");

            var dataRows = dtOB940.Rows;
            DataRow dr = dataRows[0];

            l_str_quote_num = dr["quote_num"].ToString();
            l_str_ordr_num = dr["ordr_num"].ToString();
            l_str_store_id = dr["store_id"].ToString();
            dt_ship = DateTime.Parse(dr["ship_dt"].ToString());
            l_str_ship_dt = dt_ship.ToString("MM/dd/yyyy");

            l_str_so_num = dr["so_num"].ToString();
            l_str_cust_ordr_num = dr["cust_ordr_num"].ToString();
            l_str_dept_id = dr["dept_id"].ToString();
            dt_can = DateTime.Parse(dr["ship_dt"].ToString());
            l_str_cancel_dt = dt_can.ToString("MM/dd/yyyy");
            l_str_cust_name = dr["cust_name"].ToString();
            l_str_dc_id = dr["dc_id"].ToString();
            l_str_st_mail_name = dr["st_mail_name"].ToString();


            int l_itn_tot_ctns = 0;
            decimal l_dec_totcube = 0;
            decimal l_dec_totwgt = 0;
            int l_int_tot_orders = 0;

            mxcel1.PopulateHeader(p_str_cmpid);
            mxcel1.PopulateData(dtOB940, ref l_int_tot_orders, ref l_itn_tot_ctns, ref l_dec_totcube, ref l_dec_totwgt);
            mxcel1.SaveAs(tempFileName);
            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
            return File(fs, "application / xlsx", l_str_file_name);
        }

        public ActionResult Send940SRExcelReport(string p_str_cmpid, string p_str_filename, string p_str_so_dt_frm, string p_str_so_dt_to, string p_str_batch_id)
        {
            string strOutputpath = System.Web.HttpContext.Current.Server.MapPath("~/") + ConfigurationManager.AppSettings["tempFilePath"].ToString();
            string strDateFormat = strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
            string tempFileName = string.Empty;
            string l_str_file_name = string.Empty;
            string l_str_so_num = string.Empty;
            string l_str_process_id = string.Empty;

            string l_str_quote_num = string.Empty;
            string l_str_ordr_num = string.Empty;
            string l_str_store_id = string.Empty;
            string l_str_ship_dt = string.Empty;
            string l_str_cust_ordr_num = string.Empty;
            string l_str_shipto_id = string.Empty;
            string l_str_dept_id = string.Empty;
            string l_str_cancel_dt = string.Empty;
            string l_str_cust_name = string.Empty;
            string l_str_dc_id = string.Empty;
            string l_str_st_mail_name = string.Empty;
            string l_str_so_dt_frm = string.Empty;
            string l_str_so_dt_to = string.Empty;
            Email objEmail = null;
            EmailService objEmailService = null;
            OB940UploadFile objOB940UploadFile = null;
            OB940UploadFileService ServiceOB940UploadFile = null;
            DataTable dtOB940 = new DataTable();
            DateTime dt_ship;
            DateTime dt_can;
            EmailAlertHdr objEmailAlertHdr = new EmailAlertHdr();
            try
            {

           
            if (p_str_filename != null && p_str_filename != "")
            {
                if (p_str_filename.Contains(".csv"))
                {
                    l_str_file_name = p_str_filename.Trim();
                }
                else
                {
                    l_str_file_name = p_str_filename.Trim() + ".csv";
                }
            }
            else
            {
                l_str_file_name = string.Empty;
            }
            if (p_str_so_dt_frm != null && p_str_so_dt_frm != "")
            {
                l_str_so_dt_frm = p_str_so_dt_frm.Trim();
            }
            if (p_str_so_dt_to != null && p_str_so_dt_to != "")
            {
                l_str_so_dt_to = p_str_so_dt_to.Trim();
            }
            
            objOB940UploadFile = new OB940UploadFile();
            ServiceOB940UploadFile = new OB940UploadFileService();
            dtOB940 = ServiceOB940UploadFile.GetOB940UploadDtlRptDataExcel(p_str_cmpid, l_str_file_name, p_str_batch_id, l_str_so_dt_frm, l_str_so_dt_to, l_str_process_id, l_str_so_num);

            if (!Directory.Exists(strOutputpath))
            {
                Directory.CreateDirectory(strOutputpath);
            }

            l_str_file_name = "DF_" + p_str_cmpid.ToUpper().ToString().Trim() + "_OB_SR940_SUMMARY_" + strDateFormat + ".xlsx";

            tempFileName = strOutputpath + l_str_file_name;

            if (System.IO.File.Exists(tempFileName))
                System.IO.File.Delete(tempFileName);
            xls_OB_SR940_Summary mxcel1 = new xls_OB_SR940_Summary(ConfigurationManager.AppSettings["XlsTemplateFilePath"].ToString() + "OB_SR940_SUMMARY-BY-BATCH.xlsx");

            var dataRows = dtOB940.Rows;
            DataRow dr = dataRows[0];

            l_str_quote_num = dr["quote_num"].ToString();
            l_str_ordr_num = dr["ordr_num"].ToString();
            l_str_store_id = dr["store_id"].ToString();
            dt_ship = DateTime.Parse(dr["ship_dt"].ToString());
            l_str_ship_dt = dt_ship.ToString("MM/dd/yyyy");

            l_str_so_num = dr["so_num"].ToString();
            l_str_cust_ordr_num = dr["cust_ordr_num"].ToString();
            l_str_dept_id = dr["dept_id"].ToString();
            dt_can = DateTime.Parse(dr["ship_dt"].ToString());
            l_str_cancel_dt = dt_can.ToString("MM/dd/yyyy");
            l_str_cust_name = dr["cust_name"].ToString();
            l_str_dc_id = dr["dc_id"].ToString();
            l_str_st_mail_name = dr["st_mail_name"].ToString();

            int l_itn_tot_ctns = 0;
            decimal l_dec_totcube = 0;
            decimal l_dec_totwgt = 0;
            int l_int_tot_orders = 0;

            mxcel1.PopulateHeader(p_str_cmpid);
            mxcel1.PopulateData(dtOB940, ref l_int_tot_orders, ref l_itn_tot_ctns,ref l_dec_totcube, ref l_dec_totwgt);

            mxcel1.SaveAs(tempFileName);
                mxcel1.Dispose();
            FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
            Session["RptFileName"] = tempFileName;


            objEmail = new Email();
            objEmailService = new EmailService();
            objEmail.CmpId = p_str_cmpid;
            objEmail.screenId = "OB Acknowledgement";
            objEmail.username = Session["UserID"].ToString().Trim();
            objEmail.Reportselection = "SR940Summary";

                clsRptEmail objRptEmail = new clsRptEmail();
                bool lblnRptEmailExists = false;
                objRptEmail.getEmailAlertDetails(objEmailAlertHdr, p_str_cmpid, "OUTBOUND", "OB-940-ACK", ref lblnRptEmailExists);
                string l_str_email_message = string.Empty;
                if (lblnRptEmailExists == false)
                {
                    l_str_email_message = "Hi All, " + "\n\n" + " Please find the attached OB 940 Acknowledgement Report" + "\n\n";
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
                objEmailAlertHdr.emailSubject = p_str_cmpid + "-" + "OB 940 Summary Report ";
                EmailAlert objEmailAlert = new EmailAlert();
                objEmailAlertHdr.cmpId = p_str_cmpid;
                objEmailAlert.objEmailAlertHdr = objEmailAlertHdr;

                Mapper.CreateMap<EmailAlert, EmailAlertModel>();
                EmailAlertModel EmailModel = Mapper.Map<EmailAlert, EmailAlertModel>(objEmailAlert);
                return PartialView("_EmailAlert", EmailModel);
              


            }
            catch (Exception Ex)
            {
                return PartialView("_Email", null);
            }
            finally
            {
                objEmail = null;
                objEmailService = null;
                objOB940UploadFile = null;
                ServiceOB940UploadFile = null;
                if (dtOB940 != null)
                {
                    dtOB940.Dispose();
                    dtOB940 = null;
                }
            }
        }


        public ActionResult ShowReport(string p_str_cmpid, string type)
        {

            bool isValid = true;
            string jsonErrorCode = "0";
            string strReportName = string.Empty;
            string msg = "";
            string batchId = string.Empty;
            string l_str_rpt_selection = string.Empty;
            string l_str_rpt_so_num = string.Empty;
            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
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

                  
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                    if (type == "PDF")
                    {
                        objOB940UploadFile.cmp_id = p_str_cmpid;
                        objOB940UploadFile.file_name = string.Empty;
                        objOB940UploadFile.upload_dt_from = string.Empty;
                        objOB940UploadFile.upload_dt_to = string.Empty;
                        if (Session["ses_str_batch_id"] != null) objOB940UploadFile.batch_id = Session["ses_str_batch_id"].ToString();
                        objOB940UploadFile = ServiceOB940UploadFile.GetOB940UploadDtlRptData(objOB940UploadFile, Session["ses_upload_ref_num"].ToString(), string.Empty);
                        IList <Core.Entity.OB940Report> rptSource1 = objOB940UploadFile.ListOB940UploadDtlRpt.ToList();
                        if (rptSource1.Count > 0)
                        {
                            using (ReportDocument rd = new ReportDocument())
                            { 
                            rd.Load(strRptPath);

                            objCompany.cmp_id = p_str_cmpid.Trim();
                            objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                            objOB940UploadFile.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                            rd.SetDataSource(rptSource1);
                            objOB940UploadFile.cust_logo_path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo.ToString().Trim(); //CR_3PL_MVC_BL_2018_0226_001 
                            rd.SetParameterValue("fml_image_path", objOB940UploadFile.cust_logo_path);
                            rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                            rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                            Session["ses_upload_ref_num"] = string.Empty;
                            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "940ShipRequestUpload");
                            }
                        }

                        else
                        {
                            Response.Write("<H2>Record not found</H2>");
                        }

                    }
                  
                    else
                    if (type == "Excel")
                    {


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

        public ActionResult Send940EmailAckReport(string p_str_cmp_id, string p_str_file_name, string p_str_batch_id)
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
            OB940UploadFile objOB940UploadFile = new OB940UploadFile();
            OB940UploadFileService ServiceOB940UploadFile = new OB940UploadFileService();
            Email objEmail = new Email();
            EmailService objEmailService = new EmailService();
            string strDateFormat = string.Empty;
            string strFileName = string.Empty;
            string reportFileName = string.Empty;
            string l_str_inout_type = string.Empty;
            string l_str_rpt_dtl = string.Empty;

            string l_str_cntr_list = string.Empty;
            string l_str_ib_doc_id_list = string.Empty;
            string l_str_image_Path = string.Empty;
            string Folderpath = System.Configuration.ConfigurationManager.AppSettings["tempFilepath"].ToString().Trim();
            //CR - 3PL_MVC_IB_2018_0219_008
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
            objEmail.screenId = "OB940";
            objEmail.username = objCompany.user_id;
            try
            {
                if (isValid)
                {
                    strReportName = "ShipReq940.rpt";
                   
                    string strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Reports//EDI//" + strReportName;
                    objEmail = objEmailService.GetSendMailDetails(objEmail);
                    if (objEmail.ListEamilDetail.Count != 0)
                    {
                        objEmail.EmailMessageContent = (objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == null || objEmail.ListEamilDetail[0].EmailMessageContent.Trim() == "") ? "" : objEmail.ListEamilDetail[0].EmailMessageContent.Trim();
                    }
                    else
                    {
                        objEmail.EmailMessageContent = "Please find the below Outbound Ship Request details. ";
                    }
                    objOB940UploadFile.cmp_id = p_str_cmp_id;
                    objOB940UploadFile.batch_id = p_str_batch_id;
                    objOB940UploadFile.file_name = p_str_file_name;

                    objOB940UploadFile = ServiceOB940UploadFile.GetOB940UploadDtlRptData(objOB940UploadFile, string.Empty, string.Empty);
                    l_str_rpt_dtl = p_str_cmp_id + "_" + "OB_940_ACK";
                    objEmail.EmailSubject = p_str_cmp_id + "-" + "Outbound 940 confirmation ";
                    objEmail.EmailMessage = "Hi All," + "\n\n" + objEmail.EmailMessageContent + "\n" + "\n" + "CmpId: " + " " + " " + p_str_cmp_id;
                    IList<Core.Entity.OB940Report> rptSource = objOB940UploadFile.ListOB940UploadDtlRpt.ToList();
                    if (rptSource.Count >1)
                    {
                        using (ReportDocument rd = new ReportDocument())
                        {
                            rd.Load(strRptPath);
                            rd.SetDataSource(rptSource);
                            objCompany.cmp_id = p_str_cmp_id.Trim();
                            objCompany = ServiceObjectCompany.CompanyAddresHdrDtls(objCompany);
                            objOB940UploadFile.ListCompanyAddresHdrDtls = objCompany.ListCompanyAddresHdrDtls;
                            l_str_image_Path = System.Web.HttpContext.Current.Server.MapPath("~/") + objCustMaster.ListGetCustLogo[0].cust_logo;
                            rd.SetParameterValue("fml_image_path", l_str_image_Path);
                            strDateFormat = DateTime.Now.ToString("yyyyMMddHHssmm");
                            rd.SetParameterValue("fmlReportTitle", "940 Ship Request Upload");
                            rd.DataDefinition.FormulaFields["fmlCompanyName"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].cmp_name.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompAddress"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].addr_line1.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompCity"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].city.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompstate_id"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].state_id.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPhone"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].tel.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompFax"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].fax.ToString().Trim() + "'";
                            rd.DataDefinition.FormulaFields["fmlCompPostCode"].Text = "'" + objOB940UploadFile.ListCompanyAddresHdrDtls[0].post_code.ToString().Trim() + "'";
                            Session["ses_upload_ref_num"] = string.Empty;
                            strFileName = System.Web.HttpContext.Current.Server.MapPath("~/") + Folderpath + "//" + l_str_rpt_dtl + "_" + strDateFormat + ".pdf";
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, strFileName);
                            string l_str_rptdtl = objOB940UploadFile.cmp_id + "-" + "OB940SUMMARY" + "-" + p_str_batch_id;
                            reportFileName = l_str_rptdtl + "-" + strDateFormat + ".pdf";
                            Session["RptFileName"] = strFileName;
                            objEmail.Attachment = reportFileName;
                        }
                    }
                    else
                    {
                        Response.Write("<H2>Record not found</H2>");
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
            finally
            {
               
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

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

    }
}
