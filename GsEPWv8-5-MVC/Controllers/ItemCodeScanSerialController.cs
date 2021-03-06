using AutoMapper;
using GsEPWv8_5_MVC.Business.Implementation;
using GsEPWv8_5_MVC.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GsEPWv8_5_MVC.Model;


namespace GsEPWv8_4_MVC.Controllers
{
    public class ItemCodeScanSerialController : Controller
    {
        // GET: ScanSerial
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult LoadItemCodeScanDetails(string Id, string cmp_id)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();

            objInboundInquiry.CompID = cmp_id;
            objInboundInquiry.ib_doc_id = Id;
            objInboundInquiry.LineNum = 1;
            objInboundInquiry = ServiceObject.GetInboundHdrDtl(objInboundInquiry);
            objInboundInquiry.Container = (objInboundInquiry.ListAckRptDetails[0].Container == null || objInboundInquiry.ListAckRptDetails[0].Container == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].Container.Trim());
            objInboundInquiry.status = (objInboundInquiry.ListAckRptDetails[0].status == null || objInboundInquiry.ListAckRptDetails[0].status == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].status.Trim());
            objInboundInquiry.InboundRcvdDt = (objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == null || objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt.Trim());
            //objInboundInquiry.vend_id = objInboundInquiry.ListAckRptDetails[0].vend_id.Trim();

            objInboundInquiry.vend_id = (objInboundInquiry.ListAckRptDetails[0].vend_id == null || objInboundInquiry.ListAckRptDetails[0].vend_id == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_id.Trim());
            objInboundInquiry.vend_name = (objInboundInquiry.ListAckRptDetails[0].vend_name == null || objInboundInquiry.ListAckRptDetails[0].vend_name == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_name.Trim());
            objInboundInquiry.FOB = (objInboundInquiry.ListAckRptDetails[0].FOB == null || objInboundInquiry.ListAckRptDetails[0].FOB == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].FOB.Trim());
            objInboundInquiry.refno = (objInboundInquiry.ListAckRptDetails[0].refno == null || objInboundInquiry.ListAckRptDetails[0].refno == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].refno.Trim());

            objInboundInquiry.ibdocid = Id;
            objInboundInquiry = ServiceObject.GetDocEntryId(objInboundInquiry);
            objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry = ServiceObject.GetInboundDtl(objInboundInquiry);
                      
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            objInboundInquiry = ServiceObject.GetItmCode(objInboundInquiry);
            foreach (InboundInquiry item in objInboundInquiry.ListAckRptDetails)
            {
                var inboundItemCode = ServiceObject.GetItmCodeByByItemMaster(item);
                item.Itm_Code = inboundItemCode.ListGetItmhdr.FirstOrDefault().itm_code;
            }
            return PartialView("_ItemCodeScanDetails", InboundInquiryModel);
        }


        public ActionResult LoadScanSerialDetails(string Id, string cmp_id, string Itm_Code, string Style, string Color, string Size, string itm_name, string ppk, string ctn, string TotalQty)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();

            objInboundInquiry.CompID = cmp_id;
            objInboundInquiry.ib_doc_id = Id;
            objInboundInquiry.LineNum = 1;
            objInboundInquiry = ServiceObject.GetInboundHdrDtl(objInboundInquiry);
            objInboundInquiry.Container = (objInboundInquiry.ListAckRptDetails[0].Container == null || objInboundInquiry.ListAckRptDetails[0].Container == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].Container.Trim());
            objInboundInquiry.status = (objInboundInquiry.ListAckRptDetails[0].status == null || objInboundInquiry.ListAckRptDetails[0].status == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].status.Trim());
            objInboundInquiry.InboundRcvdDt = (objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == null || objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].InboundRcvdDt.Trim());
            //objInboundInquiry.vend_id = objInboundInquiry.ListAckRptDetails[0].vend_id.Trim();

            objInboundInquiry.vend_id = (objInboundInquiry.ListAckRptDetails[0].vend_id == null || objInboundInquiry.ListAckRptDetails[0].vend_id == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_id.Trim());
            objInboundInquiry.vend_name = (objInboundInquiry.ListAckRptDetails[0].vend_name == null || objInboundInquiry.ListAckRptDetails[0].vend_name == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].vend_name.Trim());
            objInboundInquiry.FOB = (objInboundInquiry.ListAckRptDetails[0].FOB == null || objInboundInquiry.ListAckRptDetails[0].FOB == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].FOB.Trim());
            objInboundInquiry.refno = (objInboundInquiry.ListAckRptDetails[0].refno == null || objInboundInquiry.ListAckRptDetails[0].refno == string.Empty ? string.Empty : objInboundInquiry.ListAckRptDetails[0].refno.Trim());

            objInboundInquiry.ibdocid = Id;
            objInboundInquiry = ServiceObject.GetDocEntryId(objInboundInquiry);
            objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry = ServiceObject.GetInboundDtl(objInboundInquiry);
            objInboundInquiry.ItemScanIN = new ItemScanIN();
            objInboundInquiry.ItemScanIN.cmp_id = cmp_id;
            objInboundInquiry.ItemScanIN.ib_doc_id = Id;
            objInboundInquiry.ItemScanIN.itm_code = Itm_Code;
            objInboundInquiry.ItemScanIN.itm_num = Style;
            objInboundInquiry.ItemScanIN.itm_color = Color;
            objInboundInquiry.ItemScanIN.itm_size = Size;
            objInboundInquiry.ItemScanIN.itm_name = itm_name;
            objInboundInquiry.ItemScanIN.ppk = ppk;
            objInboundInquiry.ItemScanIN.ctn = ctn;
            objInboundInquiry.ItemScanIN.TotalQty = TotalQty;
            objInboundInquiry.ItemScanIN.ib_doc_dt = null;
            objInboundInquiry.ItemScanIN.ob_doc_dt = null;

            objInboundInquiry.ListItemScanIN = ServiceObject.getScanInDetailsByItemCode(cmp_id, Itm_Code, string.Empty);
            objInboundInquiry.ItemScanIN.balanceScan = Convert.ToInt32(objInboundInquiry.ItemScanIN.TotalQty) - objInboundInquiry.ListItemScanIN.Count();
            Mapper.CreateMap<InboundInquiry, InboundInquiryModel>();
            InboundInquiryModel InboundInquiryModel = Mapper.Map<InboundInquiry, InboundInquiryModel>(objInboundInquiry);
            return PartialView("_ScanSerialDetails", InboundInquiryModel);
        }


       // [HttpPost]
        public ActionResult SaveScanInDetails(string Id, string cmp_id, string Itm_Code, string Style, string Color, string Size, string itm_name, string ppk, string ctn, string TotalQty, string itm_serial_num, string itm_serial_num_exist)
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();

            objInboundInquiry.CompID = cmp_id;
            objInboundInquiry.ib_doc_id = Id;
            objInboundInquiry.LineNum = 1;
            objInboundInquiry.ibdocid = Id;
            objInboundInquiry = ServiceObject.GetDocEntryId(objInboundInquiry);
            objInboundInquiry.doc_entry_id = objInboundInquiry.doc_entry_id;
            objInboundInquiry.cmp_id = cmp_id;
            objInboundInquiry = ServiceObject.GetInboundDtl(objInboundInquiry);
            if ((itm_serial_num.IndexOf(';') > -1) || (itm_serial_num.IndexOf(',') > -1))
            {
                string[] Lst_itm_serial = itm_serial_num.IndexOf(';') > -1 ? itm_serial_num.Split(';') : itm_serial_num.Split(',');
                List<string> exceptionList = new List<string>();
                foreach (var item in Lst_itm_serial)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        objInboundInquiry.ItemScanIN = new ItemScanIN();
                        objInboundInquiry.ItemScanIN.cmp_id = cmp_id;
                        objInboundInquiry.ItemScanIN.ib_doc_id = Id;
                        objInboundInquiry.ItemScanIN.itm_code = Itm_Code;
                        objInboundInquiry.ItemScanIN.itm_num = Style;
                        objInboundInquiry.ItemScanIN.itm_color = Color;
                        objInboundInquiry.ItemScanIN.itm_size = Size;
                        objInboundInquiry.ItemScanIN.itm_name = itm_name;
                        objInboundInquiry.ItemScanIN.status = "Avail";
                        objInboundInquiry.ItemScanIN.ppk = ppk;
                        objInboundInquiry.ItemScanIN.ctn = ctn;
                        objInboundInquiry.ItemScanIN.TotalQty = TotalQty;
                        objInboundInquiry.ItemScanIN.itm_serial_num = item;
                        objInboundInquiry.ItemScanIN.ib_doc_dt = null;
                        objInboundInquiry.ItemScanIN.ob_doc_dt = null;
                        objInboundInquiry.ItemScanIN.itm_serial_num_exist = itm_serial_num_exist;

                        objInboundInquiry.ItemScanIN.itm_serial_num = item;
                        if (!ServiceObject.getScanInDetailsByItemCode(cmp_id, Itm_Code, item).Any() && itm_serial_num_exist.ToString() == string.Empty)
                            ServiceObject.InsertScanInDetails(objInboundInquiry);
                        else if (itm_serial_num_exist.ToString() != string.Empty)
                        {
                            if ((!ServiceObject.getScanInDetailsByItemCode(cmp_id, Itm_Code, item).Any()) || item == itm_serial_num_exist)
                                ServiceObject.EditScanInDetails(objInboundInquiry);
                            else
                                exceptionList.Add(item);
                        }
                        else
                            exceptionList.Add(item);
                    }
                }
               
                if(exceptionList.Any())
                    return Json(exceptionList, JsonRequestBehavior.AllowGet);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                objInboundInquiry.ItemScanIN = new ItemScanIN();
                objInboundInquiry.ItemScanIN.cmp_id = cmp_id;
                objInboundInquiry.ItemScanIN.ib_doc_id = Id;
                objInboundInquiry.ItemScanIN.itm_code = Itm_Code;
                objInboundInquiry.ItemScanIN.itm_num = Style;
                objInboundInquiry.ItemScanIN.itm_color = Color;
                objInboundInquiry.ItemScanIN.itm_size = Size;
                objInboundInquiry.ItemScanIN.itm_name = itm_name;
                objInboundInquiry.ItemScanIN.status = "Avail";
                objInboundInquiry.ItemScanIN.ppk = ppk;
                objInboundInquiry.ItemScanIN.ctn = ctn;
                objInboundInquiry.ItemScanIN.TotalQty = TotalQty;
                objInboundInquiry.ItemScanIN.itm_serial_num = itm_serial_num;
                objInboundInquiry.ItemScanIN.ib_doc_dt = null;
                objInboundInquiry.ItemScanIN.ob_doc_dt = null;
                objInboundInquiry.ItemScanIN.itm_serial_num_exist = itm_serial_num_exist;

                objInboundInquiry.ItemScanIN.itm_serial_num = itm_serial_num;
                if (!ServiceObject.getScanInDetailsByItemCode(cmp_id, Itm_Code, itm_serial_num).Any() && itm_serial_num_exist.ToString() == string.Empty)
                    ServiceObject.InsertScanInDetails(objInboundInquiry);
                else if (itm_serial_num_exist.ToString() != string.Empty)
                {
                    if ((!ServiceObject.getScanInDetailsByItemCode(cmp_id, Itm_Code, itm_serial_num).Any()) || itm_serial_num == itm_serial_num_exist)
                        ServiceObject.EditScanInDetails(objInboundInquiry);
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteScanInDetails ( string cmpid, string ItmCode, string Serial )
        {
            InboundInquiry objInboundInquiry = new InboundInquiry();
            InboundInquiryService ServiceObject = new InboundInquiryService();

            
            objInboundInquiry = ServiceObject.GetInboundDtl(objInboundInquiry);
            objInboundInquiry.ItemScanIN = new ItemScanIN();
            objInboundInquiry.ItemScanIN.cmp_id = cmpid;
            objInboundInquiry.ItemScanIN.itm_code = ItmCode;
            objInboundInquiry.ItemScanIN.itm_serial_num = Serial;
            objInboundInquiry.ItemScanIN.ib_doc_dt = null;
            objInboundInquiry.ItemScanIN.ob_doc_dt = null;

            ServiceObject.DeleteScanInDetails(objInboundInquiry);
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}