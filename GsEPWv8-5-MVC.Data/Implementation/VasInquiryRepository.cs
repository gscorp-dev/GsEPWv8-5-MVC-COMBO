using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class VasInquiryRepository : IVasInquiryRepository
    {

        public VasInquiry GetVasInquiryDetails(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objVasInquiry.cmp_id;
                    var l_str_Status = objVasInquiry.Status;
                    var tmp_str_Status = String.Empty;
                    if (l_str_Status == "ALL")
                    {
                        tmp_str_Status = "";
                    }
                    if (l_str_Status == "OPEN")
                    {
                        tmp_str_Status = "O";
                    }
                    if (l_str_Status == "POST")
                    {
                        tmp_str_Status = "P";
                    }                  
                    const string storedProcedure1 = "proc_get_web_mvc_vas_details";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cust_ID = objVasInquiry.cmp_id,
                            @VasIdFrom = objVasInquiry.vas_id_fm,
                            @VasIdTo = objVasInquiry.vas_id_to,
                            @VasDtFrom = objVasInquiry.vas_date_fm,
                            @VasDtTo = objVasInquiry.vas_date_to,
                            @Status = tmp_str_Status,
                            @so_num = objVasInquiry.so_num,
                            @vasRateId = objVasInquiry.vasRateId
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();
                }

                return objVasInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
       
        public VasInquiry GetVasPostDetails(VasInquiry objVasInquiry)
        {  
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {                                    
                    const string storedProcedure2 = "proc_get_webmvc_vas_details_rpt";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objVasInquiry.cmp_id,
                            @ShipDocId = objVasInquiry.ship_doc_id,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();

                    return objVasInquiry;
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

        public VasInquiry GetVasIdDetail(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_vas_id";
                    IEnumerable<VasInquiryDtl> ListVasIdLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @vas_id = "",
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasId = ListVasIdLIST.ToList();
                    objVasInquiry.vas_id = objVasInquiry.ListVasId[0].vas_id;
                   
                }
                return objVasInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public VasInquiry fnGetVasRateIdDetails(string pstrCmpid)
        {
            VasInquiry objVasInquiry = new VasInquiry();
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "spGetVasRateIdDetails";
                    IEnumerable<VasRateDetails> ListVasRateIdDetails = connection.Query<VasRateDetails>(storedProcedure2,
                        new
                        {
                            @pstrCmpid = pstrCmpid,

                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasRateIdDetails = ListVasRateIdDetails.ToList();
                }
                return objVasInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public VasInquiry GetVasUserId(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_vas_user_id";
                    IEnumerable<VasInquiryDtl> ListVasIdLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @vas_user_id = "",

                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasUserId = ListVasIdLIST.ToList();
                    objVasInquiry.vas_user_id = objVasInquiry.ListVasUserId[0].vas_user_id;

                }
                return objVasInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public VasInquiry GetWhsIdDetail(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_vas_entry_whs_id";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objVasInquiry.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();
                }
                return objVasInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public void  TruncateTempVasEntry(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_web_mvc_temp_vas_entry_truncate";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_CMPID = objVasInquiry.cmp_id,
                            @P_STR_VASID = objVasInquiry.vas_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();
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
        public VasInquiry GetUpdateTempVasEntryDtl(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_temp_update_vas";
                    IEnumerable<VasInquiryDtl> ListUpdateTempVasEntryGridDtlLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @itm_num = objVasInquiry.itm_num,
                            @qty = objVasInquiry.qty,
                            @list_price = objVasInquiry.list_price,
                            @Amount = objVasInquiry.amt,
                            @note = objVasInquiry.note,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListUpdateTempVasEntryGridDtl = ListUpdateTempVasEntryGridDtlLIST.ToList();

                    return objVasInquiry;
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
        //CR - 3PL-MVC-VAS-20180505 Added by Soniya
        public VasInquiry GetReUpdateTempVasEntryDtl(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_VAS_REUPDATE_TEMP_VAS_DTL";
                    IEnumerable<VasInquiryDtl> ListUpdateTempVasEntryGridDtlLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @itm_num = objVasInquiry.itm_num,
                            @vas_id = objVasInquiry.vas_id,
                            @qty = objVasInquiry.qty,
                            @list_price = objVasInquiry.list_price,
                            @Amount = objVasInquiry.amt,
                            @note = objVasInquiry.note,
                            @line_num = objVasInquiry.line_num

                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListReUpdateTempVasEntryGridDtl = ListUpdateTempVasEntryGridDtlLIST.ToList();

                    return objVasInquiry;
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
        public VasInquiry LoadSrDetails(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_load_srdetails";
                    IEnumerable<VasInquiryDtl> ListLoadSrDetailsLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @cmp_id= objVasInquiry.cmp_id,
                            @so_num = objVasInquiry.so_num,
                            
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListLoadSrDetails = ListLoadSrDetailsLIST.ToList();
                    return objVasInquiry;
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

       
  
        public VasInquiry GetVasEntryDtl(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_vas_entry_grid_detail";
                    IEnumerable<VasInquiryDtl> ListVasEntryGridDtlLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objVasInquiry.cmp_id,
                           


                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasEntryGridDtl = ListVasEntryGridDtlLIST.ToList();

                    return objVasInquiry;
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
        public VasInquiry GetVasEntryTempGridDtl(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_mvc_web_tempvasnewdtl";
                    IEnumerable<VasInquiryDtl> ListVasEntryTempGridDtlLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {

                            @P_STR_CMPID = objVasInquiry.cmp_id,
                            @P_STR_VASID = objVasInquiry.vas_id,


                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasEntryTempGridDtl = ListVasEntryTempGridDtlLIST.ToList();

                    return objVasInquiry;
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
       
        public VasInquiry GetVasEntry(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_getvasdtl";
                    IEnumerable<VasInquiryDtl> ListVasEntryDtlLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @cmp_id= objVasInquiry.cmp_id,
                            @ship_doc_id = objVasInquiry.ship_doc_id,


                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasEntryDtl = ListVasEntryDtlLIST.ToList();

                    return objVasInquiry;
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
        public void VasInsert(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "proc_get_mvcweb_vasentry";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @line_num = objVasInquiry.dtl_line,
                        @itm_num = objVasInquiry.itm_num,
                        @itm_color = objVasInquiry.itm_color,
                        @itm_name = objVasInquiry.itm_name,
                        @qty = objVasInquiry.qty,
                        @list_price = objVasInquiry.list_price,
                        @Amount = objVasInquiry.amt,
                        @status = objVasInquiry.Status,
                        @note = objVasInquiry.notes,
                        @price_uom = objVasInquiry.price_uom,
                        @cmp_id = objVasInquiry.cmp_id,
                        @vas_id = objVasInquiry.vas_id,
                        @vas_user_id = objVasInquiry.vas_user_id

                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void UpdateVasRateHdr(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_Mvc_Vas_Rate_Hdr_Mod";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpId = objVasInquiry.cmp_id,
                        @itmnum = objVasInquiry.itm_num,
                        @listprice = objVasInquiry.list_price,
                        @processid = "Modify",
                       
                    }, commandType: CommandType.StoredProcedure);
            }
        }
        
        public void GetVasPost(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_VAS_POST";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objVasInquiry.cmp_id,
                        @ship_doc_id = objVasInquiry.vas_id,
                        @returnvalue = "1",


                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public void GetVasUnPost(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_VAS_UnPost";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objVasInquiry.cust_of_cmpid,
                        @cust_id = objVasInquiry.cmp_id,
                        @ship_doc_id = objVasInquiry.vas_id,
                        @bill_doc_id = objVasInquiry.bill_doc_id,
                        @returnvalue = "1",


                    }, commandType: CommandType.StoredProcedure);
            }
        }
        public VasInquiry GetDftWhs(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_GET_DFTWHS";
                    IEnumerable<VasInquiryDtl> ListInq = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objVasInquiry.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListPickdtl = ListInq.ToList();

                    return objVasInquiry;
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
        public void SaveVasEntryDtl(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "prod_get_mvc_web_save_vas_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpid = objVasInquiry.cmp_id,
                        @shipdocid = objVasInquiry.ship_doc_id,
                        @dtlline = objVasInquiry.dtl_line,
                        @status = objVasInquiry.Status,
                        @custdcid = objVasInquiry.cmp_id,
                        @custitm = objVasInquiry.itm_num,
                        @custitmcolor = "-",
                        @custitmname = objVasInquiry.itm_name,
                        @sonum = objVasInquiry.so_num,//CR20180719-001 Added by Nithya
                        @sodtlline = objVasInquiry.dtl_line,
                        @soitmnum = objVasInquiry.itm_num,
                        @soitmcolor = "-",
                        @shipitmprice = objVasInquiry.list_price,
                        @shippriceuom = objVasInquiry.price_uom,
                        @shipqty = objVasInquiry.qty,
                        @shipuom = objVasInquiry.price_uom,
                        @notes = objVasInquiry.Note,
                        @opt = "A",                       

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateVasEntryDtl(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "prod_get_mvc_web_save_vas_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpid = objVasInquiry.cmp_id,
                        @shipdocid = objVasInquiry.ship_doc_id,
                        @dtlline = objVasInquiry.dtl_line,
                        @status = objVasInquiry.Status,
                        @custdcid = objVasInquiry.cmp_id,
                        @custitm = objVasInquiry.cust_itm,
                        @custitmcolor = "-",
                        @custitmname = objVasInquiry.cust_itm_name,
                        @sonum = objVasInquiry.so_num,
                        @sodtlline = objVasInquiry.dtl_line,
                        @soitmnum = objVasInquiry.itm_num,
                        @soitmcolor = "-",
                        @shipitmprice = objVasInquiry.list_price,
                        @shippriceuom = objVasInquiry.price_uom,
                        @shipqty = objVasInquiry.qty,
                        @shipuom = objVasInquiry.price_uom,
                        @notes = objVasInquiry.notes,
                        @opt = "M",

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void SaveVasEntryHdr(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "prod_get_mvc_web_save_vas_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpid = objVasInquiry.cmp_id,
                        @shipdocid = objVasInquiry.ship_doc_id,                        
                        @status = objVasInquiry.Status,
                        @SoNum = objVasInquiry.so_num,
                        @whsid = objVasInquiry.whs_id,
                        @custid = objVasInquiry.cmp_id,
                        @custponum = objVasInquiry.cust_po_num,
                        @shipto = objVasInquiry.ship_to_id,                       
                        @notes = objVasInquiry.notes,
                        @shipdt = objVasInquiry.ship_dt,                       
                        @opt = "A",

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateVasEntryHdr(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "prod_get_mvc_web_save_vas_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpid = objVasInquiry.cmp_id,
                        @shipdocid = objVasInquiry.vas_id,
                        @status = objVasInquiry.Status,
                        @SoNum = objVasInquiry.so_num,
                        @whsid = objVasInquiry.whs_id,
                        @custid = objVasInquiry.cmp_id,
                        @custponum = objVasInquiry.cust_po_num,
                        @shipto = objVasInquiry.ship_to_id,
                        @notes = objVasInquiry.notes,
                        @shipdt = objVasInquiry.ship_dt,
                        @opt = "M",

                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public VasInquiry GetVashdr(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_webmvc_Vas_hdr";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {

                            @Cmp_ID = objVasInquiry.cmp_id,
                            @ShipdocId = objVasInquiry.ship_doc_id,                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();

                    return objVasInquiry;
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
        public VasInquiry GetVasEntryhdr(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_vas_hdr_get";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {

                           @cmp_id= objVasInquiry.cmp_id,
                            @shipdocid = objVasInquiry.vas_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();

                    return objVasInquiry;
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
        public VasInquiry GetVasEntryGriddtl(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_vasdel_griddtl";
                    IEnumerable<VasInquiryDtl> ListVasEntryDtlLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @cmp_id = objVasInquiry.cmp_id,
                            @shipdocid = objVasInquiry.vas_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasEntryDtl = ListVasEntryDtlLIST.ToList();

                    return objVasInquiry;
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
        public VasInquiry GetVasEntryGrid(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_mvcweb_vas_dtl_get";
                    IEnumerable<VasInquiryDtl> ListVasEntryGridDtlLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {

                            @cmpid = objVasInquiry.cmp_id,
                            @shipdocid = objVasInquiry.vas_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasEntryGridDtl = ListVasEntryGridDtlLIST.ToList();

                    return objVasInquiry;
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
        public VasInquiry GetVasdtl(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "proc_get_webmvc_Vas_details";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objVasInquiry.cmp_id,
                            @ShipdocId = objVasInquiry.ship_doc_id,                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();

                    return objVasInquiry;
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
        public void DeleteVasEntry(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvcweb_vas_del";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpid = objVasInquiry.cmp_id,
                        @shipdocid = objVasInquiry.vas_id,
                        @opt = "D",
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void DeleteVasedit(VasInquiry objVasInquiry)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Proc_get_mvcweb_vasedit_del";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmpid = objVasInquiry.cmp_id,
                        @shipdocid = objVasInquiry.vas_id,                      
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public VasInquiry GetVasInquiryDetailsRpt(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objVasInquiry.cmp_id;
                    var l_str_Status = objVasInquiry.Status;
                    var tmp_str_Status = String.Empty;
                    if (l_str_Status == "ALL")
                    {
                        tmp_str_Status = "";
                    }
                    if (l_str_Status == "OPEN")
                    {
                        tmp_str_Status = "O";
                    }
                    if (l_str_Status == "POST")
                    {
                        tmp_str_Status = "P";
                    }
                    const string storedProcedure1 = "proc_get_web_mvc_vas_details_rpt";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cust_ID = objVasInquiry.cmp_id,
                            @VasIdFrom = objVasInquiry.vas_id_fm,
                            @VasIdTo = objVasInquiry.vas_id_to,
                            @VasDtFrom = objVasInquiry.vas_date_fm,
                            @VasDtTo = objVasInquiry.vas_date_to,
                            @Status = tmp_str_Status,
                            @so_num = objVasInquiry.so_num
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();
                }

                return objVasInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public VasInquiry GetVasInquiryPostRptCtnValues(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    var cmp_id = objVasInquiry.cmp_id;
                    var l_str_Status = objVasInquiry.Status;
                    var tmp_str_Status = String.Empty;
                    const string storedProcedure1 = "proc_get_webmvc_vas_details_rpt_ctn_values";
                    IEnumerable<VasInquiryDtl> ListVasInquiryLIST = connection.Query<VasInquiryDtl>(storedProcedure1,
                        new
                        {
                            @Cmp_ID = objVasInquiry.cmp_id,
                            @ShipDocId = objVasInquiry.ship_doc_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasInquiry = ListVasInquiryLIST.ToList();
                }

                return objVasInquiry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public VasInquiry GetQtyCount(VasInquiry objVasInquiry)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_GET_QTY_COUNT";
                    IEnumerable<VasInquiryDtl> ListVasEntryTempGridDtlLIST = connection.Query<VasInquiryDtl>(storedProcedure2,
                        new
                        {

                            @P_STR_CMPID = objVasInquiry.cmp_id,
                            @P_STR_VASID = objVasInquiry.vas_id,


                        },
                        commandType: CommandType.StoredProcedure);
                    objVasInquiry.ListVasEntryDtl = ListVasEntryTempGridDtlLIST.ToList();

                    return objVasInquiry;
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
        public DataTable GetVasInquiryDetailsRptExcel(string p_str_cmp_id, string p_str_vas_id_fm, string p_str_vas_id_to, string p_str_vas_date_fm, string p_str_vas_date_to, string p_str_so_num, string p_str_Status)
        {    
            try
            {

                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_web_mvc_vas_details_rpt", connection);
                var cmp_id = p_str_cmp_id;
                var l_str_Status = p_str_Status;
                var tmp_str_Status = String.Empty;
                if (l_str_Status == "ALL")
                {
                    tmp_str_Status = "";
                }
                if (l_str_Status == "OPEN")
                {
                    tmp_str_Status = "O";
                }
                if (l_str_Status == "POST")
                {
                    tmp_str_Status = "P";
                }
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cust_ID", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@VasIdFrom", SqlDbType.VarChar).Value = p_str_vas_id_fm;
                command.Parameters.Add("@VasIdTo", SqlDbType.VarChar).Value = p_str_vas_id_to;
                command.Parameters.Add("@VasDtFrom", SqlDbType.VarChar).Value = p_str_vas_date_fm;
                command.Parameters.Add("@VasDtTo", SqlDbType.VarChar).Value = p_str_vas_date_to;
                command.Parameters.Add("@Status", SqlDbType.VarChar).Value = tmp_str_Status;
                command.Parameters.Add("@so_num", SqlDbType.VarChar).Value = p_str_so_num;
                connection.Open();
                DataTable dtGetVasSmryRptExcelTemplate = new DataTable();
                dtGetVasSmryRptExcelTemplate.Load(command.ExecuteReader());
                return dtGetVasSmryRptExcelTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        public DataTable GetVasPostDetailsExcel(string p_str_cmp_id, string SelectedID)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GenSoftConnection"].ToString());
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("proc_get_webmvc_vas_details_rpt", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@Cmp_ID", SqlDbType.VarChar).Value = p_str_cmp_id;
                command.Parameters.Add("@ShipDocId", SqlDbType.VarChar).Value = SelectedID;
                connection.Open();
                DataTable dtGetVasPostRptExcelTemplate = new DataTable();
                dtGetVasPostRptExcelTemplate.Load(command.ExecuteReader());
                return dtGetVasPostRptExcelTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
    }
}
