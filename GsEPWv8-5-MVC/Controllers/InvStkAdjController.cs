using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using GsEPWv8_5_MVC.Common;

namespace GsEPWv8_5_MVC.Controllers
{
    public class InvStkAdjController : Controller
    {
        public List<InvStkAdjAdd> lstInvStkAdjAdd;
        public List<InvPhyCountInvalidData> lstInvPhyCountInvalidData;
        public InvAdjUploadFileInfo objInvAdjUploadFileInfo;

        // GET: InvStkAdj
        public ActionResult InvStkAdj( string p_str_cmp_id)
        {

            string l_str_cmp_id = string.Empty;
            try
            {
                InvStkAdj objStkAdj = new InvStkAdj();
                IInvStkAdjService ServiceObject = new InvStkAdjService();
                Company objCompany = new Company();
                CompanyService ServiceObjectCompany = new CompanyService();

                if (objStkAdj.cmp_id == null || objStkAdj.cmp_id == string.Empty)
                {
                    objStkAdj.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                }
                else
                {
                    objCompany.cmp_id = Session["g_str_cmp_id"].ToString().Trim();
                    objStkAdj.cmp_id = objCompany.cmp_id;
                }

                if (objStkAdj.cmp_id != "")
                {
                    objCompany.user_id = Session["UserID"].ToString().Trim();
                    objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
                    objStkAdj.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
                }
               
                
                LookUp objLookUp = new LookUp();
                LookUpService ServiceObject1 = new LookUpService();
                objLookUp.id = "5";
                objLookUp.lookuptype = "INVENTORYINQ";
                objLookUp = ServiceObject1.GetLookUpValue(objLookUp);
                objStkAdj.ListLookUpDtl = objLookUp.ListLookUpDtl;
                objStkAdj.cmp_id = p_str_cmp_id;
                Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
                InvStkAdjModel objStockInquiryModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
                return View(objStockInquiryModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public FileResult SampleTemplatedownload()
        {
            return File("~\\templates\\INV-PHY-COUNT-TEMPLATE.xlsx", "text/xlsx", string.Format("INV-PHY-COUNT-TEMPLATE-WITH-SAMPLE-{0}.xlsx", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }
        public EmptyResult CmpIdOnChange(string p_str_cmp_id)
        {
            Session["g_str_cmp_id"] = (p_str_cmp_id == null ? string.Empty : p_str_cmp_id.Trim());
            return null;
        }

        public ActionResult CheckPhyUploadFileExists(string p_str_cmp_id, string p_str_file_name)
        {
            IInvStkAdjService ServiceObject = new InvStkAdjService();
            bool l_bl_file_exist = false;
            l_bl_file_exist = ServiceObject.CheckPhyCountFileExists(p_str_cmp_id, p_str_file_name);
            return Json(l_bl_file_exist, JsonRequestBehavior.AllowGet);
        }

        private void getCsvData(string p_str_cmp_id, string p_str_file_path, string p_str_file_name, string p_str_file_extn, ref string l_str_error_msg)
        {


            lstInvStkAdjAdd = new List<InvStkAdjAdd>();
            lstInvPhyCountInvalidData = new List<InvPhyCountInvalidData>();

            IInvStkAdjService ServiceObject = new InvStkAdjService();
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

                        l_str_upload_ref_num = Convert.ToString(ServiceObject.GetInvPhyCountUploadRefNum(p_str_cmp_id));
                        l_int_no_of_lines = lst_file_line_content.Count();
                        objInvAdjUploadFileInfo = new InvAdjUploadFileInfo();
                        objInvAdjUploadFileInfo.cmp_id = p_str_cmp_id;
                        objInvAdjUploadFileInfo.file_name = p_str_file_name;
                        objInvAdjUploadFileInfo.upload_by = Session["UserID"].ToString().Trim();
                        objInvAdjUploadFileInfo.upload_date_time = DateTime.Now;
                        objInvAdjUploadFileInfo.no_of_lines = l_int_no_of_lines;
                        objInvAdjUploadFileInfo.status = "PEND";
                        objInvAdjUploadFileInfo.upload_ref_num = l_str_upload_ref_num;

                        Session["objIB943UploadFileInfo"] = objInvAdjUploadFileInfo;

                        int l_int_cur_line = 0;
                        List<string> lst_csv_data = new List<string>();
                        using (var file_reader = new CsvFileReader(p_str_file_path))
                        {
                            while (file_reader.ReadRow(lst_csv_data))
                            {
                                if (lst_csv_data[0].ToUpper().Equals("PHYINV") && l_int_cur_line < l_int_no_of_lines)
                                {

                                    l_int_cur_line = l_int_cur_line + 1;
                                    if (l_int_cur_line == 1)
                                    {
                                        continue;
                                    }
                                    InvStkAdjAdd objInvStkAdjAdd = new InvStkAdjAdd();
                                    int l_str_length = lst_csv_data.Count;

                                    bool bool_is_valied = false;
                                    if ((l_str_length == 18)) bool_is_valied = true;
                                    if (bool_is_valied == false)
                                    {
                                        l_str_error_desc = "Line  " + l_int_cur_line.ToString() + " contains " + (l_str_length).ToString() + " fields It should be 18 Please refer the Link 'IB 943 Upload Sample for sample' available in this page ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }


                                    if (lst_csv_data[1].Trim().Length > 10 || lst_csv_data[1].Trim().Length <= 2)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Company Id (2nd column) Length should be between 3 to 10 ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }

                                    if (lst_csv_data[1] != p_str_cmp_id)
                                    {
                                        l_str_error_desc = "Line : " + l_int_cur_line.ToString() + " contains Invalid Company Id ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.cmp_id = lst_csv_data[1].Trim();
                                    }


                                    if (lst_csv_data[3].Trim().Length ==0)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Action Flag can not be empty ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else

                                    {
                                        if ((lst_csv_data[3].Trim()=="REDM") || (lst_csv_data[3].Trim() == "MOVE") || (lst_csv_data[3].Trim() == "ADDN"))

                                        {
                                            objInvStkAdjAdd.action_flag = lst_csv_data[3].Trim();
                                        }
                                        else
                                        {
                                            l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + "- Invalid Action Flag ";
                                            objInvStkAdjAdd.error_desc = l_str_error_desc;
                                            InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                            objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                            objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                            objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                            lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                            continue;
                                        }

                                    }
                                    if ((lst_csv_data[4].Trim().Length > 20) || (lst_csv_data[4].Trim().Length ==0))

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Style length should be maximum of 20 OR Should not be blank ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.itm_num = lst_csv_data[4].Trim();
                                    }

                                    if ((lst_csv_data[5].Length > 20) || (lst_csv_data[5].Trim().Length == 0))

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item Color length should be maximum of 20 OR Should not be blank ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.itm_color = lst_csv_data[5].Trim();
                                    }

                                    if ((lst_csv_data[6].Length > 20) || (lst_csv_data[6].Trim().Length == 0))

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item size length should be maximum of 20 OR Should not be blank ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.itm_size = lst_csv_data[6].Trim();
                                    }

                                    if (lst_csv_data[7].Length > 75)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Item Name length should be maximum of 75 ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.itm_name = lst_csv_data[7].Trim();
                                    }

                                    if ((lst_csv_data[8].Trim().Length > 10)  || (lst_csv_data[8].Trim().Length == 0))

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Lot# length should be maximum of 10 OR Should not be blank ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.lot_id = lst_csv_data[8].Trim();
                                    }

                                    if (lst_csv_data[9].Trim().Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - PO Number / Ref# length should be maximum of 20 ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.po_num = lst_csv_data[9].Trim();
                                    }

                                    if ((lst_csv_data[10].Trim().Length > 10) || (lst_csv_data[10].Trim().Length == 0))

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - From Location length should be maximum of 10 OR Should not be blank ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.cur_loc_id = lst_csv_data[10].Trim();
                                    }
                                    if ((lst_csv_data[11].Trim().Length > 10) )

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - To Location length should be maximum of 10 OR Should not be blank ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {

                                        objInvStkAdjAdd.new_loc_id = lst_csv_data[11].Trim();
                                    }

                                    try {

                                        objInvStkAdjAdd.cur_avail_ctn = Convert.ToInt32(lst_csv_data[12]);
                                    }
                                    catch
                                    {
                                        objInvStkAdjAdd.cur_avail_ctn = 0;
                                        objInvStkAdjAdd.error_desc = objInvStkAdjAdd.error_desc + " - Invalid Available Ctn(s)";
                                    }
                                    try
                                    {

                                        objInvStkAdjAdd.cur_itm_qty = Convert.ToInt32(lst_csv_data[13]);
                                    }
                                    catch
                                    {
                                        objInvStkAdjAdd.cur_itm_qty = 0;
                                        objInvStkAdjAdd.error_desc = objInvStkAdjAdd.error_desc + " - Invalid Current PPK ";
                                    }

                                    try
                                    {

                                        objInvStkAdjAdd.new_avail_ctn = Convert.ToInt32(lst_csv_data[14]);
                                    }
                                    catch
                                    {
                                        objInvStkAdjAdd.new_avail_ctn = 0;
                                       // objInvStkAdjAdd.error_desc = objInvStkAdjAdd.error_desc + " - Invalid New Ctn(s)";
                                    }
                                    try
                                    {

                                        objInvStkAdjAdd.new_itm_qty = Convert.ToInt32(lst_csv_data[15]);
                                    }
                                    catch
                                    {
                                        objInvStkAdjAdd.new_itm_qty = 0;
                                       // objInvStkAdjAdd.error_desc = objInvStkAdjAdd.error_desc + " - Invalid New PPK";
                                    }

                                    if (lst_csv_data[16].Trim().Length > 20)

                                    {
                                        l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Reaseon length should be maximum of 20 ";
                                        objInvStkAdjAdd.error_desc = l_str_error_desc;
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                        continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.adj_reason = lst_csv_data[16].Trim();
                                    }

                                    // Header Note

                                    if (lst_csv_data[17].Trim().Length > 200)
                                    {
                                    l_str_error_desc = "Line : " + (l_int_cur_line).ToString() + " - Header Note length should be maximum of 200 ";
                                    objInvStkAdjAdd.error_desc = l_str_error_desc;
                                    InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                    objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                    objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                    objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                    lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                    continue;
                                    }
                                    else
                                    {
                                        objInvStkAdjAdd.adj_note = lst_csv_data[17].Trim();
                                    }



                                    l_int_line_num = l_int_line_num + 1;
                                    objInvStkAdjAdd.dtl_line = l_int_line_num;


                                    if (objInvStkAdjAdd.error_desc.Length > 0)
                                    {
                                        InvPhyCountInvalidData objInvPhyCountInvalidData = new InvPhyCountInvalidData();
                                        objInvPhyCountInvalidData.line_num = objInvStkAdjAdd.dtl_line;
                                        objInvPhyCountInvalidData.error_desc = objInvStkAdjAdd.error_desc;
                                        objInvPhyCountInvalidData.line_data = lst_file_line_content[l_int_cur_line - 1].ToString();
                                        lstInvPhyCountInvalidData.Add(objInvPhyCountInvalidData);
                                    }
                                    else
                                    {

                                        lstInvStkAdjAdd.Add(objInvStkAdjAdd);
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
                        Session["lstPhyCountFileDtl"] = lstInvStkAdjAdd;
                        Session["lstPhyCountInvalidData"] = lstInvPhyCountInvalidData;
                       
                        if (lstInvPhyCountInvalidData.Count > 0)
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
        public ActionResult loadPhyCountFile(string p_str_cmp_id)
        {
            InvStkAdj objStkAdj = new InvStkAdj();
            IInvStkAdjService ServiceObject = new InvStkAdjService();
            Session["lstPhyCountFileDtl"] = "";
            Session["lstPhyCountInvalidData"] = "";
            Session["objPhyCountUploadFileInfo"] = "";
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
                                string l_str_file_path = Path.Combine(Server.MapPath("~/uploads"), l_str_file_name);
                                //Try and upload
                                try
                                {
                                    FileUpload.SaveAs(l_str_file_path);
                                    string l_str_ext = Path.GetExtension(l_str_file_name);
                                    if (l_str_ext.ToUpper() != ".CSV")
                                    {
                                        objStkAdj.error_mode = true;
                                        objStkAdj.error_desc = "Invalid File Format";
                                        return Json(objStkAdj, JsonRequestBehavior.AllowGet);
                                    }


                                   
                                        getCsvData(p_str_cmp_id, l_str_file_path, l_str_file_name, ".CSV", ref l_str_error_msg);
                           


                                    if (l_str_error_msg != "")
                                    {
                                        objStkAdj.error_mode = true;
                                        objStkAdj.error_desc = l_str_error_msg;

                                    }


                                    objStkAdj.ListInvStkAdjUpload = lstInvStkAdjAdd;
                                    objStkAdj.ListInvPhyCountInvalidData = lstInvPhyCountInvalidData;
                                    ViewBag.l_int_error_count = lstInvPhyCountInvalidData.Count;

                                    Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
                                    InvStkAdjModel objStkAdjModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
                                    return PartialView("_GridStockAdjUpload", objStkAdjModel);


                                }
                                catch (Exception ex)
                                {
                                    objStkAdj.error_mode = true;
                                    objStkAdj.error_desc = ex.InnerException.ToString();
                                    return Json(objStkAdj, JsonRequestBehavior.AllowGet);
                                }

                            }

                        }


                        else
                        {
                            //Catch errors
                            objStkAdj.error_mode = true;
                            objStkAdj.error_desc = "Please select a file";
                            return Json(objStkAdj, JsonRequestBehavior.AllowGet);
                        }



                    }
                }
                catch (Exception ex)
                {
                    objStkAdj.error_mode = true;
                    objStkAdj.error_desc = ex.Message;
                    return Json(objStkAdj, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                objStkAdj.error_mode = true;
                objStkAdj.error_desc = "No files selected.";
                return Json(objStkAdj, JsonRequestBehavior.AllowGet);
            }
            Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
            InvStkAdjModel objStockInquiryModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
            return PartialView("_GridStockAdjUpload", objStockInquiryModel);
        }

        public JsonResult ItemXGetLocDtl(string term, string cmp_id)
        {
            StockChangeService ServiceObject = new StockChangeService();
            var List = ServiceObject.ItemXGetLocDetails(term.Trim(), cmp_id).LstItmxlocdtl.Select(x => new { label = x.loc_id, value = x.loc_id }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ItemXGetitmDtl(string term, string cmp_id)
        {
            IStockInquiryService ServiceObject = new StockInquiryService();
            var List = ServiceObject.ItemXGetitmDetails(term, cmp_id.Trim()).LstItmxCustdtl.Select(x => new { label = x.Itmdtl, value = x.itm_num, itm_num = x.itm_num, itm_color = x.itm_color, itm_size = x.itm_size, itm_name = x.itm_name }).ToList();
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStockAdjGridDetails(string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color,
         string p_str_itm_size, string p_str_itm_name, string p_str_status, string p_str_ib_doc_id,  string p_str_cont_id, string p_str_lot_id, string p_str_loc_id,
         string p_str_ref_no, string p_str_whs_id,
         string p_str_rcvd_from_dt, string p_str_rcvd_to_dt, string p_str_po_num)

        {

            InvStkAdj objStkAdj = new InvStkAdj();
            InvStkAdjInquiry objInvStkAdjInquiry = new InvStkAdjInquiry();
            IInvStkAdjService ServiceObject = new InvStkAdjService();
            objInvStkAdjInquiry.cmp_id = p_str_cmp_id;
            objInvStkAdjInquiry.ib_doc_id = p_str_ib_doc_id;
            objInvStkAdjInquiry.itm_num = p_str_itm_num;
            objInvStkAdjInquiry.itm_color = p_str_itm_color;
            objInvStkAdjInquiry.itm_size = p_str_itm_size;
            objInvStkAdjInquiry.itm_name = p_str_itm_name;
            objInvStkAdjInquiry.status = p_str_status;
            objInvStkAdjInquiry.cont_id = p_str_cont_id;
            objInvStkAdjInquiry.loc_id = p_str_loc_id;
            objInvStkAdjInquiry.lot_id = p_str_lot_id;
            objInvStkAdjInquiry.ref_no = p_str_ref_no;
            objInvStkAdjInquiry.whs_id = p_str_whs_id;
            objInvStkAdjInquiry.rcvd_from_dt = p_str_rcvd_from_dt;
            objInvStkAdjInquiry.rcvd_to_dt = p_str_rcvd_to_dt;
            objInvStkAdjInquiry.po_num = p_str_po_num;
            Session["sesobjInvStkAdjInquiry"] = objInvStkAdjInquiry;
            objStkAdj = ServiceObject.getStockForAdj(objInvStkAdjInquiry);
      
            Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
            InvStkAdjModel objStockInquiryModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
            return PartialView("_StockAdjInquiry", objStockInquiryModel);
           
        }

        public ActionResult GeInvMergeGridDetails(string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color,
      string p_str_itm_size, string p_str_itm_name, string p_str_status, string p_str_ib_doc_id, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id,
      string p_str_ref_no, string p_str_whs_id,
      string p_str_rcvd_from_dt, string p_str_rcvd_to_dt, string p_str_po_num)

        {

            InvStkAdj objStkAdj = new InvStkAdj();
            InvStkAdjInquiry objInvStkAdjInquiry = new InvStkAdjInquiry();
            IInvStkAdjService ServiceObject = new InvStkAdjService();
            objInvStkAdjInquiry.cmp_id = p_str_cmp_id;
            objInvStkAdjInquiry.ib_doc_id = p_str_ib_doc_id;
            objInvStkAdjInquiry.itm_num = p_str_itm_num;
            objInvStkAdjInquiry.itm_color = p_str_itm_color;
            objInvStkAdjInquiry.itm_size = p_str_itm_size;
            objInvStkAdjInquiry.itm_name = p_str_itm_name;
            objInvStkAdjInquiry.cont_id = p_str_cont_id;
            objInvStkAdjInquiry.loc_id = p_str_loc_id;
            objInvStkAdjInquiry.lot_id = p_str_lot_id;
            objInvStkAdjInquiry.ref_no = p_str_ref_no;
            objInvStkAdjInquiry.whs_id = p_str_whs_id;
            objInvStkAdjInquiry.status = p_str_status;
            objInvStkAdjInquiry.rcvd_from_dt = p_str_rcvd_from_dt;
            objInvStkAdjInquiry.rcvd_to_dt = p_str_rcvd_to_dt;
            objInvStkAdjInquiry.po_num = p_str_po_num;
            Session["sesobjInvStkAdjInquiry"] = objInvStkAdjInquiry;
            objStkAdj = ServiceObject.getStockForAdj(objInvStkAdjInquiry);

            Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
            InvStkAdjModel objStockInquiryModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
            return PartialView("_GridInvMergeInquiry", objStockInquiryModel);

        }
        public ActionResult loadInvMergeInquiry(string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color,
       string p_str_itm_size, string p_str_itm_name, string p_str_status, string p_str_ib_doc_id, string p_str_cont_id, string p_str_lot_id, string p_str_loc_id,
       string p_str_ref_no, string p_str_whs_id,
       string p_str_rcvd_from_dt, string p_str_rcvd_to_dt, string p_str_po_num)

        {

            InvStkAdj objStkAdj = new InvStkAdj();
            InvStkAdjInquiry objInvStkAdjInquiry = new InvStkAdjInquiry();
            IInvStkAdjService ServiceObject = new InvStkAdjService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
       

            objInvStkAdjInquiry.cmp_id = p_str_cmp_id;
            objInvStkAdjInquiry.ib_doc_id = p_str_ib_doc_id;
            objInvStkAdjInquiry.itm_num = p_str_itm_num;
            objInvStkAdjInquiry.itm_color = p_str_itm_color;
            objInvStkAdjInquiry.itm_size = p_str_itm_size;
            objInvStkAdjInquiry.itm_name = p_str_itm_name;
            objInvStkAdjInquiry.cont_id = p_str_cont_id;
            objInvStkAdjInquiry.loc_id = p_str_loc_id;
            objInvStkAdjInquiry.lot_id = p_str_lot_id;
            objInvStkAdjInquiry.ref_no = p_str_ref_no;
            objInvStkAdjInquiry.whs_id = p_str_whs_id;
            objInvStkAdjInquiry.rcvd_from_dt = p_str_rcvd_from_dt;
            objInvStkAdjInquiry.rcvd_to_dt = p_str_rcvd_to_dt;
            objInvStkAdjInquiry.po_num = p_str_po_num;
            
            Session["sesobjInvStkAdjInquiry"] = objInvStkAdjInquiry;
            objStkAdj = ServiceObject.getStockForAdj(objInvStkAdjInquiry);
            objStkAdj.objInvStkAdjInquiry = objInvStkAdjInquiry;
           
            objStkAdj.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objStkAdj.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;

            Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
            InvStkAdjModel objStockInquiryModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
            return PartialView("_InvMergeInquiry", objStockInquiryModel);

        }


        [HttpPost]
        public JsonResult SaveMergeCtns(string pstrCmpId, InvMergeHdr ObjInvMergeHdr, List<InvMergeCtns> plstInvMergeCtns)
        {

             DataTable ldtMergeDtl;
            ldtMergeDtl = new DataTable();
            ldtMergeDtl = Utility.ConvertListToDataTable(plstInvMergeCtns);

            InvStkAdjInquiry objInvStkAdjInquiry = new InvStkAdjInquiry();
            IInvStkAdjService ServiceObject = new InvStkAdjService();

            if (ServiceObject.SaveMergeCtns( pstrCmpId, ObjInvMergeHdr, ldtMergeDtl) == true)
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }

           else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult RefreshStockAdjGridDetails(string p_str_cmp_id)

        {

            InvStkAdj objStkAdj = new InvStkAdj();
            InvStkAdjInquiry objInvStkAdjInquiry = new InvStkAdjInquiry();
            IInvStkAdjService ServiceObject = new InvStkAdjService();
            objInvStkAdjInquiry = Session["sesobjInvStkAdjInquiry"] as InvStkAdjInquiry;
            objStkAdj = ServiceObject.getStockForAdj(objInvStkAdjInquiry);

            Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
            InvStkAdjModel objStockInquiryModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
            return PartialView("_StockAdjInquiry", objStockInquiryModel);

        }
        public ActionResult loadInvAdjUpload(string p_str_cmp_id)
        {
            InvStkAdj objStkAdj = new InvStkAdj();
            InvStkAdjAdd objInvStkAdjAdd = new InvStkAdjAdd();
            IInvStkAdjService ServiceObject = new InvStkAdjService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "200";
            objLookUp.lookuptype = "STK-ADJ";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);

            objStkAdj.ListInvAdjType = objLookUp.ListLookUpDtl;
            objInvStkAdjAdd.adj_reason = "Physical Count";
            objInvStkAdjAdd.cmp_id = p_str_cmp_id;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objStkAdj.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objStkAdj.objInvStkAdjAdd = objInvStkAdjAdd;
            Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
            InvStkAdjModel objStockInquiryModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
            return PartialView("_StockAdjUpload", objStockInquiryModel);
        }
        public ActionResult getStockAdj(string p_str_cmp_id,  string p_str_loc_id, string p_str_itm_num, string p_str_itm_color,
         string p_str_itm_size, string p_str_itm_code, string p_str_itm_name, string p_str_lot_id, string p_str_po_num, int p_int_avail_ctn,
         int p_int_itm_qty, int p_int_avail_qty)
        {
            InvStkAdj objStkAdj = new InvStkAdj();
            InvStkAdjAdd objInvStkAdjAdd = new InvStkAdjAdd();
            IInvStkAdjService ServiceObject = new InvStkAdjService();
            Company objCompany = new Company();
            CompanyService ServiceObjectCompany = new CompanyService();

         
            LookUp objLookUp = new LookUp();
            LookUpService ServiceObject1 = new LookUpService();
            objLookUp.id = "200";
            objLookUp.lookuptype = "STK-ADJ";
            objLookUp = ServiceObject1.GetLookUpValue(objLookUp);

            LookUp objLookUpInvAdjType = new LookUp();

            objLookUpInvAdjType.id = "200";
            objLookUpInvAdjType.lookuptype = "STK-ADJ";
            objLookUpInvAdjType.name = "--Select ADJ Reason--";
            objLookUpInvAdjType.description = "--Select ADJ Reason--";
            objLookUp.ListLookUpDtl.Add(objLookUpInvAdjType);

            objStkAdj.ListInvAdjType = objLookUp.ListLookUpDtl;
            objInvStkAdjAdd.adj_reason = "--Select ADJ Reason--";
            objInvStkAdjAdd.cmp_id = p_str_cmp_id;
            objInvStkAdjAdd.cur_loc_id = p_str_loc_id;
            objInvStkAdjAdd.itm_num = p_str_itm_num;
            objInvStkAdjAdd.itm_color = p_str_itm_color;
            objInvStkAdjAdd.itm_size = p_str_itm_size;
            objInvStkAdjAdd.itm_code = p_str_itm_code;
            objInvStkAdjAdd.itm_name = p_str_itm_name;
            objInvStkAdjAdd.lot_id = p_str_lot_id;
            objInvStkAdjAdd.po_num = p_str_po_num;
            objInvStkAdjAdd.cur_loc_id = p_str_loc_id;
            objInvStkAdjAdd.cur_avail_ctn = p_int_avail_ctn;
            objInvStkAdjAdd.cur_itm_qty = p_int_itm_qty;
            objInvStkAdjAdd.cur_avail_qty = p_int_avail_qty;
            //objInvStkAdjAdd.new_avail_ctn = 0;
            //objInvStkAdjAdd.new_itm_qty = 0;
            //objInvStkAdjAdd.new_avail_qty = 0;
            objCompany.user_id = Session["UserID"].ToString().Trim();
            objCompany = ServiceObjectCompany.GetPickCompanyDetails(objCompany);
            objStkAdj.ListCompanyPickDtl = objCompany.ListCompanyPickDtl;
            objStkAdj.objInvStkAdjAdd = objInvStkAdjAdd;
            objStkAdj.cmp_id = p_str_cmp_id;
            Mapper.CreateMap<InvStkAdj, InvStkAdjModel>();
            InvStkAdjModel objStockInquiryModel = Mapper.Map<InvStkAdj, InvStkAdjModel>(objStkAdj);
            return PartialView("_StockAdjByCtn", objStockInquiryModel);
        }

        [HttpGet]
        public ActionResult saveStockAdj(string p_str_cmp_id,  string p_str_itm_num, string p_str_itm_color,
        string p_str_itm_size, string p_str_itm_code,  string p_str_lot_id, string p_str_po_num,
        string p_str_from_loc_id, string p_str_to_loc_id,int p_int_cur_ctns, int p_int_new_ctns,
        int p_int_cur_itm_qty, int p_int_new_itm_qty,string p_str_adj_reason, string p_str_note)
        {
            int l_int_json_status = 0;
            try
            {
                InvStkAdj objStkAdj = new InvStkAdj();
                InvStkAdjAdd objInvStkAdjSave = new InvStkAdjAdd();
                IInvStkAdjService ServiceObject = new InvStkAdjService();
                objInvStkAdjSave.cmp_id = p_str_cmp_id;
                objInvStkAdjSave.itm_num = p_str_itm_num;
                objInvStkAdjSave.itm_color = p_str_itm_color;
                objInvStkAdjSave.itm_size = p_str_itm_size;
                objInvStkAdjSave.itm_code = p_str_itm_code;

                objInvStkAdjSave.lot_id = p_str_lot_id;
                objInvStkAdjSave.po_num = p_str_po_num;
                objInvStkAdjSave.cur_loc_id = p_str_from_loc_id;
                if (p_str_to_loc_id==string.Empty)
                {
                    objInvStkAdjSave.new_loc_id = p_str_from_loc_id;
                }
                else
                { 
                objInvStkAdjSave.new_loc_id = p_str_to_loc_id;
                }
                objInvStkAdjSave.cur_avail_ctn = p_int_cur_ctns;
                if (p_int_new_ctns == 0)
                {
                    objInvStkAdjSave.new_avail_ctn = p_int_cur_ctns;
                }
                else
                {
                    objInvStkAdjSave.new_avail_ctn = p_int_new_ctns;
                }
                objInvStkAdjSave.cur_itm_qty = p_int_cur_itm_qty;
                if (p_int_new_itm_qty == 0)
                {
                    objInvStkAdjSave.new_itm_qty = p_int_cur_itm_qty;
                }
                else
                {
                    objInvStkAdjSave.new_itm_qty = p_int_new_itm_qty;
                }
                int l_int_split_qty = 0;
                l_int_split_qty =  (p_int_new_ctns * p_int_new_itm_qty) % p_int_cur_itm_qty;
                objInvStkAdjSave.split_qty = l_int_split_qty;
                if (l_int_split_qty > 0)
                {
                    int l_int_adj_cur_ctns = (p_int_new_ctns * p_int_new_itm_qty) / p_int_cur_itm_qty;
                    if ((l_int_adj_cur_ctns * p_int_cur_itm_qty ) < (p_int_new_ctns * p_int_new_itm_qty))
                    {

                        objInvStkAdjSave.adj_cur_ctns = l_int_adj_cur_ctns + 1;
                    }
                    else
                    {
                        objInvStkAdjSave.adj_cur_ctns = l_int_adj_cur_ctns;
                    }
                    objInvStkAdjSave.new_split_ctn_qty = p_int_cur_itm_qty - l_int_split_qty;
                }
                else
                {
                    objInvStkAdjSave.adj_cur_ctns = (p_int_new_ctns * p_int_new_itm_qty) / p_int_cur_itm_qty;
                    objInvStkAdjSave.new_split_ctn_qty = 0;
                }

                objInvStkAdjSave.adj_reason = p_str_adj_reason;
                objInvStkAdjSave.adj_note = p_str_note;
                objInvStkAdjSave.user_id = Session["UserID"].ToString().Trim();


                ServiceObject.SaveInvStkAdjTempSingle(objInvStkAdjSave);

                l_int_json_status = 1;

            }
            catch(Exception Ex)
            {
                l_int_json_status = 2;
                throw Ex;
            }

         
            return Json(l_int_json_status, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult saveStockAdjCtns(string p_str_cmp_id, string p_str_itm_num, string p_str_itm_color,
      string p_str_itm_size, string p_str_itm_code, string p_str_lot_id, string p_str_po_num,
      string p_str_from_loc_id,  int p_int_adj_ctns,
      int p_int_cur_itm_qty,  string p_str_adj_reason, string p_str_note)
        {
            int l_int_json_status = 0;
            try
            {
                InvStkAdj objStkAdj = new InvStkAdj();
                InvStkAdjAdd objInvStkAdjByCtnsSave = new InvStkAdjAdd();
                IInvStkAdjService ServiceObject = new InvStkAdjService();
                objInvStkAdjByCtnsSave.cmp_id = p_str_cmp_id;
                objInvStkAdjByCtnsSave.itm_num = p_str_itm_num;
                objInvStkAdjByCtnsSave.itm_color = p_str_itm_color;
                objInvStkAdjByCtnsSave.itm_size = p_str_itm_size;
                objInvStkAdjByCtnsSave.itm_code = p_str_itm_code;
                objInvStkAdjByCtnsSave.lot_id = p_str_lot_id;
                objInvStkAdjByCtnsSave.po_num = p_str_po_num;
                objInvStkAdjByCtnsSave.cur_loc_id = p_str_from_loc_id;
                objInvStkAdjByCtnsSave.adj_ctns = p_int_adj_ctns;
                objInvStkAdjByCtnsSave.cur_itm_qty = p_int_cur_itm_qty;
                objInvStkAdjByCtnsSave.adj_reason = p_str_adj_reason;
                objInvStkAdjByCtnsSave.adj_note = p_str_note;
                objInvStkAdjByCtnsSave.user_id = Session["UserID"].ToString().Trim();
                ServiceObject.SaveInvStkAdjByCtns(objInvStkAdjByCtnsSave);

                l_int_json_status = 1;

            }
            catch (Exception Ex)
            {
                l_int_json_status = 2;
                throw Ex;
            }


            return Json(l_int_json_status, JsonRequestBehavior.AllowGet);
        }
    }
}