using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Business.Interface;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GsEPWv8_5_MVC.Controllers
{
    public class EcomBinScanOutController : BaseController
    {
        // GET: EcomBinScanOut  


        [HttpGet]
        public ActionResult ListEcomBinScanOut()
        {
            try
            {
                EcomBinScanOut objEcomBinScanOut = new EcomBinScanOut();
                IEcomBinScanOutService ServiceObject = new EcomBinScanOutService();
                ServiceObject.EcomBinScanOutTempDelete(objEcomBinScanOut);
                objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString().Trim();
             
                objEcomBinScanOut.LoginUserGroupID = ObjBasicEntity.UserGroupID;
                objEcomBinScanOut.LoginUserName = ObjBasicEntity.UserName;
                objEcomBinScanOut = ServiceObject.GetCompanyNameList(objEcomBinScanOut);
                objEcomBinScanOut = ServiceObject.EcomBinScanOutList(objEcomBinScanOut);
                Mapper.CreateMap<EcomBinScanOut, EcomBinScanOutModel>();
                EcomBinScanOutModel objEcomBinScanOutModel = Mapper.Map<EcomBinScanOut, EcomBinScanOutModel>(objEcomBinScanOut);
                return View(objEcomBinScanOutModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        [HttpGet]
        //public ActionResult ListEcomBinScanOut(EcomBinScanOutModel objEcomBinScanOutModel, string text)
        public ActionResult ListEcomBinScan(string text)
        {
            try
            {
                IEcomBinScanOutService ServiceObject = new EcomBinScanOutService();
                EcomBinScanOutModel objEcomBinScanOutModel = new EcomBinScanOutModel();
                Mapper.CreateMap<EcomBinScanOutModel, EcomBinScanOut>();
                EcomBinScanOut objEcomBinScanOut = Mapper.Map<EcomBinScanOutModel, EcomBinScanOut>(objEcomBinScanOutModel);
                objEcomBinScanOut.AlocNo = text;
                objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString();
                ServiceObject.Getstatus(objEcomBinScanOut);
                ServiceObject.GetCompanyNameListHeader(objEcomBinScanOut);
                ServiceObject.EcomBinScanOutList(objEcomBinScanOut);
                ServiceObject.GetCompanyNameList(objEcomBinScanOut);
                TempData["Details"] = objEcomBinScanOut.GetListEcomBinScanOutHeader;
                if (objEcomBinScanOut.status == "S")
                {
                    ViewBag.Message = "S";
                    //Like Server.Transfer() in Asp.Net WebForm
                    return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (objEcomBinScanOut.GetListEcomBinScanOutSHeader.Count > 0)
                    {
                        ViewBag.ErrorMessage = "";
                        objEcomBinScanOutModel.cmp_id = objEcomBinScanOut.cmp_id;
                        objEcomBinScanOutModel.AllocateNumber = objEcomBinScanOut.AlocNo;
                        objEcomBinScanOutModel.SoDate = Convert.ToDateTime(objEcomBinScanOut.GetListEcomBinScanOutSHeader[0].so_dt).ToString("MM/dd/yyyy");
                        objEcomBinScanOutModel.SoNum = objEcomBinScanOut.GetListEcomBinScanOutSHeader[0].so_num;
                        objEcomBinScanOutModel.cust_ordr_num = objEcomBinScanOut.GetListEcomBinScanOutSHeader[0].cust_ordr_num;
                        objEcomBinScanOutModel.GetListEcomBinScanOutSHeader = objEcomBinScanOut.GetListEcomBinScanOutSHeader;
                        objEcomBinScanOutModel.GetCompanyNameDetails = objEcomBinScanOut.GetCompanyNameDetails;
                        for (int j = 0; j < objEcomBinScanOut.GetListEcomBinScanOutSHeader.Count(); j++)
                        {
                            objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString();
                            objEcomBinScanOut.line_num = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].line_num;
                            objEcomBinScanOut.itm_code = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].itm_code;
                            objEcomBinScanOut.fob = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].fob;
                            objEcomBinScanOut.aloc_doc_id = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].aloc_doc_id;
                            objEcomBinScanOut.so_num = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].so_num;
                            objEcomBinScanOut.loc_id = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].loc_id;
                            objEcomBinScanOut.upc_code = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].upc_code;
                            objEcomBinScanOut.so_itm_num = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].so_itm_num;
                            objEcomBinScanOut.so_itm_size = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].so_itm_size;
                            objEcomBinScanOut.aloc_qty = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].aloc_qty;
                            objEcomBinScanOut.pick_qty = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].pick_qty;
                            ServiceObject.GetCompanyNameListCreateFetch(objEcomBinScanOut);

                        }
                        ServiceObject.EcomBinScanOutListGrid(objEcomBinScanOut);
                        objEcomBinScanOutModel.GetListEcomBinScanOutGridHeader = objEcomBinScanOut.GetListEcomBinScanOutGridHeader;
                        //return PartialView("~/Views/Shared/_EcomBinScanOutListBox.cshtml", objEcomBinScanOutModel);
                        return PartialView("_EcomBinScanOutListBox", objEcomBinScanOutModel);
                    }
                    else
                    {
                        objEcomBinScanOut.AlocNo = text;
                        ServiceObject.Getstatus(objEcomBinScanOut);
                        if (objEcomBinScanOut.status == "S")
                        {
                            ViewBag.Message = "S";
                            //Like Server.Transfer() in Asp.Net WebForm
                            return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //ViewBag.Error = "Wrong Reservation Code, Please enter a valuable reservation code.";
                            //return Json(ViewBag.Error, JsonRequestBehavior.AllowGet);
                            ViewBag.Message = "Wrong Reservation Code, Please enter a valuable reservation code.";
                            //Like Server.Transfer() in Asp.Net WebForm
                            return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        public ActionResult MyIndex()
        {
            ViewBag.Message = "Wrong reservation Code"; // Assigned value : "Hi, Dot Net Tricks"
            return View("MyIndex");
        }


        public JsonResult ListEcomBinScanOutJson(EcomBinScanOutModel objEcomBinScanOutModel, string text)
        {
            try
            {
                IEcomBinScanOutService ServiceObject = new EcomBinScanOutService();
                Mapper.CreateMap<EcomBinScanOutModel, EcomBinScanOut>();
                EcomBinScanOut objEcomBinScanOut = Mapper.Map<EcomBinScanOutModel, EcomBinScanOut>(objEcomBinScanOutModel);
                objEcomBinScanOut.AlocNo = text;
                objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString();
                ServiceObject.GetCompanyNameListHeader(objEcomBinScanOut);
                ServiceObject.EcomBinScanOutList(objEcomBinScanOut);
                ServiceObject.GetCompanyNameList(objEcomBinScanOut);
                TempData["Details"] = objEcomBinScanOut.GetListEcomBinScanOutHeader;
                if (objEcomBinScanOut.GetListEcomBinScanOutSHeader.Count > 0)
                {
                    ViewBag.ErrorMessage = "";
                    objEcomBinScanOutModel.cmp_id = objEcomBinScanOut.cmp_id;
                    objEcomBinScanOutModel.AllocateNumber = objEcomBinScanOut.AlocNo;
                    objEcomBinScanOutModel.SoDate = Convert.ToDateTime(objEcomBinScanOut.GetListEcomBinScanOutSHeader[0].so_dt).ToString("MM/dd/yyyy");
                    objEcomBinScanOutModel.SoNum = objEcomBinScanOut.GetListEcomBinScanOutSHeader[0].so_num;
                    objEcomBinScanOutModel.cust_ordr_num = objEcomBinScanOut.GetListEcomBinScanOutSHeader[0].cust_ordr_num;
                    objEcomBinScanOutModel.GetListEcomBinScanOutSHeader = objEcomBinScanOut.GetListEcomBinScanOutSHeader;
                    objEcomBinScanOutModel.GetCompanyNameDetails = objEcomBinScanOut.GetCompanyNameDetails;
                    return Json(objEcomBinScanOutModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.ErrorMessage = "You are Enter Wrong Reservation Code, Empty data has been displayed.";
                    return Json(ViewBag.ErrorMessage, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        [HttpGet]
        public JsonResult GetRoWIndex(string p_str_scanbin, string p_str_allocatenumber, string p_str_scanupc, string Id)
        {
            EcomBinScanOut objEcomBinScanOut = new EcomBinScanOut();
            IEcomBinScanOutService ServiceObject = new EcomBinScanOutService();
            objEcomBinScanOut.AlocNo = p_str_allocatenumber;
            objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString();
            ServiceObject.GetCompanyNameListHeader(objEcomBinScanOut);
            //for (int j = 0; j < objEcomBinScanOut.GetListEcomBinScanOutSHeader.Count(); j++)
            //{
            //    //var l_str_loc_id = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].loc_id;
            //    //var l_str_style = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].so_itm_num;
            //    //var l_str_aloc_qty = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].aloc_qty;
            //    //var l_str_upc_code = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].upc_code;

            //    if (objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].loc_id.Trim() == p_str_scanbin && objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].upc_code.Trim() == p_str_scanupc)
            //    {

            //        objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].pick_qty = objEcomBinScanOut.GetListEcomBinScanOutSHeader[j].aloc_qty.Trim();
            //    }



            //}
            var result = from r in objEcomBinScanOut.GetListEcomBinScanOutSHeader where r.cmp_id == objEcomBinScanOut.cmp_id select new { r.aloc_doc_id, r.cust_ordr_num };
            var RWID = objEcomBinScanOut.GetListEcomBinScanOutSHeader.Select((Value, Index) => new { Value, Index }).Single(p => p.Value.loc_id == p_str_scanbin);
            return Json(result, JsonRequestBehavior.AllowGet);//,RWID.Index
        }

        [HttpPost]
        public ActionResult ListEcomBinScanOutHeader(EcomBinScanOutModel objEcomBinScanOutModel, string p_str_scanbin, string p_str_scanupc, string p_str_allocatenumber, string p_str_SoNum, string p_str_Sodt, string p_str_Cust)
        {
            try
            {
                IEcomBinScanOutService ServiceObject = new EcomBinScanOutService();
                Mapper.CreateMap<EcomBinScanOutModel, EcomBinScanOut>();
                EcomBinScanOut objEcomBinScanOut = Mapper.Map<EcomBinScanOutModel, EcomBinScanOut>(objEcomBinScanOutModel);
                objEcomBinScanOut.ScanBin = p_str_scanbin;
                objEcomBinScanOut.ScanUPC = p_str_scanupc;
                objEcomBinScanOut.AlocNo = p_str_allocatenumber;
                objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString();
                ServiceObject.GetCompanyNameListHeader(objEcomBinScanOut);
                ServiceObject.EcomBinScanOutListGrid(objEcomBinScanOut);
                ServiceObject.GetCompanyNameList(objEcomBinScanOut);
                ServiceObject.CheckBinUpcStatus(objEcomBinScanOut);
                TempData["Category"] = objEcomBinScanOut.GetListEcomBinScanOutGridHeader;
                if (objEcomBinScanOut.loc_id.Trim() == p_str_scanbin.Trim() && objEcomBinScanOut.upc_code.Trim() == p_str_scanupc.Trim())
                {
                    for (int j = 0; j < objEcomBinScanOut.GetListEcomBinScanOutGridHeader.Count(); j++)
                    {
                        //if(objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].loc_id == p_str_scanbin && objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].upc_code == p_str_scanupc)
                        //{
                        objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString();
                        objEcomBinScanOut.loc_id = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].loc_id;
                        objEcomBinScanOut.upc_code = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].upc_code;
                        objEcomBinScanOut.so_itm_num = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].so_itm_num;
                        objEcomBinScanOut.so_itm_size = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].so_itm_size;
                        objEcomBinScanOut.aloc_qty = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].aloc_qty;
                        objEcomBinScanOut.pick_qty = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].pick_qty;



                        if (objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].loc_id.Trim() == p_str_scanbin && objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].upc_code.Trim() == p_str_scanupc.Trim())
                        {

                            objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].pick_qty = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].aloc_qty.Trim();
                            ServiceObject.GetCompanyNameListCreateFetch(objEcomBinScanOut);
                            ServiceObject.EcomBinScanOutListGrid(objEcomBinScanOut);
                            TempData["pickCategory"] = objEcomBinScanOut.GetListEcomBinScanOutGridHeader;
                            objEcomBinScanOutModel.GetListEcomBinScanOutGridHeader = objEcomBinScanOut.GetListEcomBinScanOutGridHeader;
                            TempData["pick_qty"] = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].pick_qty;
                            objEcomBinScanOutModel.cmp_id = Session["dflt_cmp_id"].ToString();

                        }

                    }

                    objEcomBinScanOutModel.AllocateNumber = p_str_allocatenumber;
                    objEcomBinScanOutModel.cust_ordr_num = p_str_Cust;
                    objEcomBinScanOutModel.ScanBin = p_str_scanbin;
                    objEcomBinScanOutModel.ScanUPC = p_str_scanupc;
                    objEcomBinScanOutModel.SoNum = p_str_SoNum;
                    objEcomBinScanOutModel.SoDate = Convert.ToDateTime(p_str_Sodt).ToString("MM/dd/yyyy");
                    objEcomBinScanOutModel.cmp_id = Session["dflt_cmp_id"].ToString();
                    objEcomBinScanOutModel.GetListEcomBinScanOutSHeader = objEcomBinScanOut.GetListEcomBinScanOutSHeader;
                    objEcomBinScanOutModel.GetListEcomBinScanOutGridHeader = objEcomBinScanOut.GetListEcomBinScanOutGridHeader;
                    objEcomBinScanOutModel.GetCompanyNameDetails = objEcomBinScanOut.GetCompanyNameDetails;

                    return PartialView("_EcomBinScanOutListBox", objEcomBinScanOutModel);
                }
                //else
                //{

                //    if (objEcomBinScanOut.loc_id != p_str_scanbin)
                //    {
                //        ViewBag.Message = "Bin.";
                //        return Content("<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>");
                //    }
                //    else if (objEcomBinScanOut.upc_code.Trim() != p_str_scanupc)
                //    {
                //        ViewBag.Message = "Upc.";
                //        return Content("<script language='javascript' type='text/javascript'>alert('Thanks for Feedback!');</script>");
                //    }

                //}
                return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }
        //public ActionResult dalda()

        //{
        //    ViewBag.ErrorMessage = "Upc.";
        //    return Json(ViewBag.ErrorMessage, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult SaveEcomBinScanOutHeader(EcomBinScanOutModel objEcomBinScanOutModel)
        {
            string l_str_check = String.Empty;

            try
            {
                IEcomBinScanOutService ServiceObject = new EcomBinScanOutService();
                Mapper.CreateMap<EcomBinScanOutModel, EcomBinScanOut>();
                EcomBinScanOut objEcomBinScanOut = Mapper.Map<EcomBinScanOutModel, EcomBinScanOut>(objEcomBinScanOutModel);
                objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString();
                ServiceObject.GetCompanyNameListHeader(objEcomBinScanOut);
                ServiceObject.EcomBinScanOutListGrid(objEcomBinScanOut);
                objEcomBinScanOut.Unique = "S";
                ServiceObject.GetUniqueNumbers(objEcomBinScanOut);
                objEcomBinScanOut.new_ship_doc_id = objEcomBinScanOut.new_ship_doc_id;
                objEcomBinScanOut.GetListEcomBinScanOutSHeader = TempData["Category"] as List<EcomBinScanOut>;
                for (int j = 0; j < objEcomBinScanOut.GetListEcomBinScanOutGridHeader.Count(); j++)
                {
                    if (objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].pick_qty.Trim() == objEcomBinScanOut.GetListEcomBinScanOutGridHeader[j].aloc_qty.Trim())
                    {
                        //l_str_ship_dt = DateTime.Now.ToString("yyyy-MM-dd");
                        l_str_check = "True";

                    }
                    else
                    {
                        l_str_check = "False";
                        ViewBag.Message = "Wrong Reservation Code, Please enter a valuable reservation code.";
                        //Like Server.Transfer() in Asp.Net WebForm
                        return RedirectToAction("MyIndex");

                    }

                }
                if (l_str_check == "True")
                {
                    objEcomBinScanOut.Unique = "P";
                    ServiceObject.GetUniqueNumbers(objEcomBinScanOut);
                    objEcomBinScanOut.ship_dt = DateTime.Now.ToString("yyyy-MM-dd");

                    objEcomBinScanOut.new_process_id = objEcomBinScanOut.new_process_id;
                    objEcomBinScanOut.GetListEcomBinScanOutSHeader = TempData["Category"] as List<EcomBinScanOut>;
                    for (int k = 0; k < objEcomBinScanOut.GetListEcomBinScanOutGridHeader.Count(); k++)
                    {
                        objEcomBinScanOut.line_num = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[k].line_num.Trim();
                        objEcomBinScanOut.itm_code = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[k].itm_code.Trim();
                        objEcomBinScanOut.fob = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[k].fob.Trim();
                        objEcomBinScanOut.pick_qty = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[k].pick_qty.Trim();
                        objEcomBinScanOut.cmp_id = Session["dflt_cmp_id"].ToString();
                        objEcomBinScanOut.aloc_doc_id = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[k].aloc_doc_id.Trim();
                        objEcomBinScanOut.so_num = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[k].so_num.Trim();
                        objEcomBinScanOut.upc_code = objEcomBinScanOut.GetListEcomBinScanOutGridHeader[k].upc_code.Trim();
                        ServiceObject.EcomBinScanOutCreate(objEcomBinScanOut);
                    }
                    ServiceObject.EcomBinScanOutShipHeader(objEcomBinScanOut);
                }
                ServiceObject.EcomBinScanOutTempDelete(objEcomBinScanOut);
                //objEcomBinScanOutModel.cmp_id = Session["dflt_cmp_id"].ToString();
                //objEcomBinScanOutModel.GetListEcomBinScanOutHeader = TempData["Details"] as List<GetEcomBinScanOutHeader>;
                //objEcomBinScanOut.GetListEcomBinScanOutSHeader = TempData["Category"] as List<EcomBinScanOut>;
                //objEcomBinScanOutModel.GetCompanyNameDetails = objEcomBinScanOut.GetCompanyNameDetails;
                //return View(objEcomBinScanOutModel);
                return Json("", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        public ActionResult DeleteEcomBinScanOut()
        {
            EcomBinScanOut objEcomBinScanOut = new EcomBinScanOut();
            IEcomBinScanOutService ServiceObject = new EcomBinScanOutService();
            ServiceObject.EcomBinScanOutTempDelete(objEcomBinScanOut);
            return View("ListEcomBinScanOut", JsonRequestBehavior.AllowGet);
            //return RedirectToAction("ListSideEffect");
        }


    }
}