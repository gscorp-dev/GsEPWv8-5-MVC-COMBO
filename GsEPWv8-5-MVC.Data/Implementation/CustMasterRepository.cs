using Dapper;
using GsEPWv8_5_MVC.Core.Entity;
using GsEPWv8_5_MVC.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsEPWv8_5_MVC.Data.Implementation
{
    public class CustMasterRepository : ICustMasterRepository
    {
        public CustMaster GetCustMasterDetails(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CustMaster objOrderLifeCycleCategory = new CustMaster();

                    const string storedProcedure2 = "proc_get_mvcweb_cust_dtls";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @Cmp_ID = objCustMaster.cust_of_cmp_id,
                            @Cust_ID = objCustMaster.cmp_id,                           
                            @user_id = objCustMaster.user_id,   
                            @whs_id = objCustMaster.whs_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCustDetails = ListCustDetailsLST.ToList();

                    return objCustMaster;
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

        public CustMaster fnGetAppUserList(string pstrCmpId,string pstrMode)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CustMaster objCustMaster = new CustMaster();

                    const string storedProcedure2 = "spGetAppUserList";
                    IEnumerable<appUsers> ListAppUsers = connection.Query<appUsers>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,
                            @pstrMode = pstrMode

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListAppUsers = ListAppUsers.ToList();

                    return objCustMaster;
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

        public CustMaster GetTableEntityValueCount(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CustMaster objOrderLifeCycleCategory = new CustMaster();

                    const string storedProcedure2 = "SP_MVC_MASTER_CHECK_ENTITY_VALUE";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objCustMaster.cmp_id,
                           
                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCheckEntityValue = ListCustDetailsLST.ToList();

                    return objCustMaster;
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
        
        public CustMaster GetCustHdrDetails(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CustMaster objOrderLifeCycleCategory = new CustMaster();

                    const string storedProcedure2 = "sp_ma_get_cust_hdr";
                    IEnumerable<CustMasterDtl> ListCustHdrLST = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @cmpid = objCustMaster.cust_of_cmp_id,
                            @custid = objCustMaster.cmp_id,                         

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCustHdr = ListCustHdrLST.ToList();

                    return objCustMaster;
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
        public CustMaster GetCustConfigDetails(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CustMaster objOrderLifeCycleCategory = new CustMaster();

                    const string storedProcedure2 = "proc_get_webmvc_ma_cust_config";
                    IEnumerable<CustMasterDtl> ListCustConfigLST = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @cmpid = objCustMaster.cust_of_cmp_id,
                            @custid = objCustMaster.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCustConfig = ListCustConfigLST.ToList();

                    return objCustMaster;
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
        public void SaveCustMaster(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_Insert_updt_tbl_ma_cust_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "A",
                        @cmpId = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id,
                        @custname = objCustMaster.cmp_name,
                        @status = objCustMaster.status,
                        @source = objCustMaster.source,
                        @entrydt = objCustMaster.dt_of_entry,
                        @lastchgdt = objCustMaster.last_chg_dt,
                        @region = objCustMaster.region,
                        @ter = objCustMaster.territory,
                        @contact = objCustMaster.contact,
                        @whs = objCustMaster.whs_id,
                        @tel = objCustMaster.Tel,
                        @cell    = objCustMaster.cell,
                        @fax = objCustMaster.fax,
                        @email = objCustMaster.email,
                        @web = objCustMaster.web,
                        @insales = objCustMaster.insaleid,
                        @outsales = objCustMaster.outsaleid,
                        @groupid = objCustMaster.cust_grp_id,
                        @process_id = "Add"
                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public bool fnAddCustDfltRate(CustMaster objCustMaster)
        {
            bool blnStatus = false;
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "spAddCustDfltRate";
                    connection.Execute(storedProcedure1,
                        new
                        {

                            @pstrCmpId = objCustMaster.cmp_id,
                            @pstrIORateId = objCustMaster.io_rate_id,
                            @pstrIORateName = objCustMaster.io_rate_name,
                            @pstrIOBillBy = objCustMaster.inout_bill,
                            @pdecIORatePrice = objCustMaster.io_rate_price,
                            @pstrStrgRateId = objCustMaster.strg_rate_id,
                            @pstrStrgRateName = objCustMaster.strg_rate_name,
                            @pstrStrgBillBy = objCustMaster.strg_bill,
                            @pdecStrgRatePrice = objCustMaster.strg_rate_price
                        }, commandType: CommandType.StoredProcedure);

                }
                blnStatus = true;
            }
            catch (Exception ex)
            {
                blnStatus = false;
            }
            return blnStatus;
        }
        public CustMaster fnGetCustDfltRate(string pstrCmpId)
        {
            CustMaster objCustMaster = new CustMaster();
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CustMaster objOrderLifeCycleCategory = new CustMaster();

                    const string storedProcedure2 = "spGetCustDfltRate";
                    IEnumerable<custDfltRate> ListCustDfltRate = connection.Query<custDfltRate>(storedProcedure2,
                        new
                        {
                            @pstrCmpId = pstrCmpId,

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCustDfltRate = ListCustDfltRate.ToList();

                    return objCustMaster;
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
        public void InsertTableEntityValueByCust(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "SP_MVC_MASTER_INSERT_TABLE_ENTITY_VALUEBYCUST";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @cmp_id = objCustMaster.cmp_id,
                        @ENTITY_CODE = objCustMaster.entity_Code,
                        @ENTITY_NAME = objCustMaster.entity_Code,
                        @CON_STRING = objCustMaster.con_string.Trim(),
                        @Opt = objCustMaster.entity_Code,
                       

                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public void SaveWhsMaster(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_MVC_Master_Add_Whs_Hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                       
                        @cmpId = objCustMaster.cmp_id,
                        @whsid = objCustMaster.whs_id,
                        @whsname = objCustMaster.whs_name,
                        @status = objCustMaster.status,
                        @attn = objCustMaster.attn,
                        @mailname = objCustMaster.mail_name,
                        @addrline1 = objCustMaster.addr1,
                        @city = objCustMaster.city,
                       
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        
        public void SaveCustMasterDtl(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_Insert_tbl_ma_cust_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "A",
                        @cmpid = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id,
                        @mailname = objCustMaster.mail_name,
                        @addrline1 = objCustMaster.addr1,
                        @addrline2 = objCustMaster.addr2,
                        @city = objCustMaster.city,
                        @stateid = objCustMaster.state,
                        @attn = objCustMaster.attn,
                        @zip = objCustMaster.zip,
                        @cntryid = objCustMaster.country,
                        @freightid = objCustMaster.frieght_id,
                        @shipviaid = objCustMaster.ship_via_id,
                        @custcatg = objCustMaster.catg,
                        @billcycle = objCustMaster.bl_cycle,
                        @crdtcode = objCustMaster.code,
                        @crdtchck = objCustMaster.type,
                        @crdtlimit = objCustMaster.credit_lt,
                        @crdtmsg = objCustMaster.msg,
                        @termsid = objCustMaster.term_code,
                        @taxexemptid = objCustMaster.tax_code,
                        @taxexempt = objCustMaster.tax_exempt,
                        @glnum = objCustMaster.gl_num,
                        @ordrvalue = objCustMaster.ord_val,
                        @disc = objCustMaster.disc,
                        @processid = "Add",
                        @custlogo = objCustMaster.cust_logo.Trim(),//CR-20180706
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateCustMasterDtl(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_Insert_tbl_ma_cust_dtl";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "M",
                        @cmpid = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id,
                        @mailname = objCustMaster.mail_name,
                        @addrline1 = objCustMaster.addr1,
                        @addrline2 = objCustMaster.addr2,
                        @city = objCustMaster.city,
                        @stateid = objCustMaster.state,
                        @attn = objCustMaster.attn,
                        @zip = objCustMaster.zip,
                        @cntryid = objCustMaster.country,
                        @freightid = objCustMaster.frieght_id,
                        @shipviaid = objCustMaster.ship_via_id,
                        @custcatg = objCustMaster.catg,
                        @billcycle = objCustMaster.bl_cycle,
                        @crdtcode = objCustMaster.code,
                        @crdtchck = objCustMaster.type,
                        @crdtlimit = objCustMaster.credit_lt,
                        @crdtmsg = objCustMaster.msg,
                        @termsid = objCustMaster.term_code,
                        @taxexemptid = objCustMaster.tax_code,
                        @taxexempt = objCustMaster.tax_exempt,
                        @glnum = objCustMaster.gl_num,
                        @ordrvalue = objCustMaster.ord_val,
                        @disc = objCustMaster.disc,
                        @processid = "Edit",
                        @custlogo = objCustMaster.cust_logo.Trim(),//CR-20180706
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        
        public void SaveCmpHdr(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_Insert_Updt_tbl_ma_cmp_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "A",
                        @cmp_id = objCustMaster.cmp_id,
                        @status = objCustMaster.status,
                        @cmp_name = objCustMaster.cmp_name,
                        @start_dt = objCustMaster.dt_of_entry,
                        @last_chg_dt = objCustMaster.last_chg_dt,
                        @attn = objCustMaster.attn,
                        @addr_line1 = objCustMaster.addr1,
                        @addr_line2 = objCustMaster.addr2,
                        @city = objCustMaster.city,
                        @state_id = objCustMaster.state,
                        @post_code = objCustMaster.zip,
                        @cntry_id = objCustMaster.country,
                        @tel = objCustMaster.Tel,
                        @fax = objCustMaster.fax,
                        @web = objCustMaster.web,
                        @email = objCustMaster.email,
                        @group_id = objCustMaster.cust_grp_id,
                        @process_id = "Add",
                        @remit_attn = "",
                        @remit_addr_line1 = "",
                        @remit_addr_line2 = "",
                        @remit_city = "",
                        @remit_state_id = "",
                        @remit_post_code = "",
                        @remit_cntry_id = "",
                        @remit_tel = "",
                        @remit_fax = "",
                        @remit_email = "",
                        @remit_web = ""

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateCmpHdr(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_Insert_Updt_tbl_ma_cmp_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "M",
                        @cmp_id = objCustMaster.cmp_id,
                        @status = objCustMaster.status,
                        @cmp_name = objCustMaster.cmp_name,
                        @start_dt = objCustMaster.dt_of_entry,
                        @last_chg_dt = objCustMaster.last_chg_dt,
                        @attn = objCustMaster.attn,
                        @addr_line1 = objCustMaster.addr1,
                        @addr_line2 = objCustMaster.addr2,
                        @city = objCustMaster.city,
                        @state_id = objCustMaster.state,
                        @post_code = objCustMaster.zip,
                        @cntry_id = objCustMaster.country,
                        @tel = objCustMaster.Tel,
                        @fax = objCustMaster.fax,
                        @web = objCustMaster.web,
                        @email = objCustMaster.email,
                        @group_id = objCustMaster.cust_grp_id,
                        @process_id = "Edit",
                        @remit_attn = "",
                        @remit_addr_line1 = "",
                        @remit_addr_line2 = "",
                        @remit_city = "",
                        @remit_state_id = "",
                        @remit_post_code = "",
                        @remit_cntry_id = "",
                        @remit_tel = "",
                        @remit_fax = "",
                        @remit_email = "",
                        @remit_web = ""

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void SaveCustMasterConfig(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_Insert_updt_tbl_ma_cust_config";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "A",
                        @cmpid = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id,
                        @billfreedays = objCustMaster.bl_free_days,
                        @bill_type = objCustMaster.strg_bill,
                        @bill_inout_type = objCustMaster.inout_bill,
                        @newItem = objCustMaster.allow_new_item,
                        @ItemPick = objCustMaster.itemlistby,
                        @init_strg_rt_req = objCustMaster.initstrg,
                        @aloc_sort_stmt = "",
                        @Doc_Increment = objCustMaster.autoincre,
                        @recvItmmode = objCustMaster.recv_lot_by,
                        @AlocByItm = objCustMaster.aloc_by,
                        @processid = "Add",
                        @recv_nondoc_itm = objCustMaster.Recv_non_doc_itm,//CR20180829-001
                        @Stk_Chk_Reqt = objCustMaster.Stk_Chk_Reqt,
                        @pmt_terms = objCustMaster.pmt_term,
                        @cube_auto_calc = objCustMaster.cube_auto_calc,
                        @is_bill_by_cube_rounded = objCustMaster.is_bill_by_cube_rounded,
                        @min_inout_cube = objCustMaster.min_inout_cube,
                        @min_strg_cube = objCustMaster.min_strg_cube,
                        @ecom_recv_by_bin = objCustMaster.ecom_recv_by_bin,
                        @allow_940_new_item = objCustMaster.allow_940_new_item,
                        @cust_type = objCustMaster.cust_type,
                        @scan_serial = objCustMaster.is_scan_serial

                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void SaveCustMasterConfigDir(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_insert_updt_cust_config_dir";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "A",
                        @cmpid = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id,
                        @billfreedays = objCustMaster.bl_free_days,
                        @bill_type = objCustMaster.strg_bill,
                        @bill_inout_type = objCustMaster.inout_bill,
                        @newItem = objCustMaster.allow_new_item,
                        @ItemPick = objCustMaster.itemlistby,
                        @init_strg_rt_req = objCustMaster.initstrg,
                        @aloc_sort_stmt = "",
                        @Doc_Increment = objCustMaster.autoincre,
                        @recvItmmode = objCustMaster.recv_lot_by,
                        @AlocByItm = objCustMaster.aloc_by,
                        @processid = "Add",
                        @recv_nondoc_itm= objCustMaster.Recv_non_doc_itm,
                        @Stk_Chk_Reqt = objCustMaster.Stk_Chk_Reqt,
                        @pmt_terms = objCustMaster.pmt_term,
                        @cube_auto_calc = objCustMaster.cube_auto_calc,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateCustMasterConfigDir(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_insert_updt_cust_config_dir";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "M",
                        @cmpid = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id,
                        @billfreedays = objCustMaster.bl_free_days,
                        @bill_type = objCustMaster.strg_bill,
                        @bill_inout_type = objCustMaster.inout_bill,
                        @newItem = objCustMaster.allow_new_item,
                        @ItemPick = objCustMaster.itemlistby,
                        @init_strg_rt_req = objCustMaster.initstrg,
                        @aloc_sort_stmt = "",
                        @Doc_Increment = objCustMaster.autoincre,
                        @recvItmmode = objCustMaster.recv_lot_by,
                        @AlocByItm = objCustMaster.aloc_by,
                        @processid = "Add",
                        @recv_nondoc_itm = objCustMaster.Recv_non_doc_itm,
                        @Stk_Chk_Reqt = objCustMaster.Stk_Chk_Reqt,
                        @pmt_terms = objCustMaster.pmt_term,
                        @cube_auto_calc = objCustMaster.cube_auto_calc,
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateCustMasterConfig(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "sp_ma_Insert_updt_tbl_ma_cust_config";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "M",
                        @cmpid = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id,
                        @billfreedays = objCustMaster.bl_free_days,
                        @bill_type = objCustMaster.strg_bill,
                        @bill_inout_type = objCustMaster.inout_bill,
                        @newItem = objCustMaster.allow_new_item,
                        @ItemPick = objCustMaster.itemlistby,
                        @init_strg_rt_req = objCustMaster.initstrg,
                        @aloc_sort_stmt = "",
                        @Doc_Increment = objCustMaster.autoincre,
                        @recvItmmode = objCustMaster.recv_lot_by,
                        @AlocByItm = objCustMaster.aloc_by,
                        @processid = "Edit",
                        @recv_nondoc_itm = objCustMaster.Recv_non_doc_itm,
                        @Stk_Chk_Reqt = objCustMaster.Stk_Chk_Reqt,//CR20180914
                        @pmt_terms = objCustMaster.pmt_term,
                        @cube_auto_calc = objCustMaster.cube_auto_calc,
                        @is_bill_by_cube_rounded = objCustMaster.is_bill_by_cube_rounded,
                        @min_inout_cube = objCustMaster.min_inout_cube,
                        @min_strg_cube = objCustMaster.min_strg_cube,
                        @ecom_recv_by_bin = objCustMaster.ecom_recv_by_bin,
                        @allow_940_new_item = objCustMaster.allow_940_new_item,
                        @cust_type = objCustMaster.cust_type,
                        @scan_serial = objCustMaster.is_scan_serial
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void UpdateCustMaster(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                //const string storedProcedure1 = "proc_get_mvcweb_ma_cust_hdr";
                const string storedProcedure1 = "sp_ma_Insert_updt_tbl_ma_cust_hdr";
                connection.Execute(storedProcedure1,
                    new
                    {
                        @opid = "M",
                        @cmpId = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id,
                        @custname = objCustMaster.cmp_name,
                        @status = objCustMaster.status,
                        @source = objCustMaster.source,
                        @entrydt = objCustMaster.dt_of_entry,
                        @lastchgdt = objCustMaster.last_chg_dt,
                        @region = objCustMaster.region,
                        @ter = objCustMaster.territory,
                        @contact = objCustMaster.contact,
                        @whs = objCustMaster.whs_id,
                        @tel = objCustMaster.Tel,
                        @cell = objCustMaster.cell,
                        @fax = objCustMaster.fax,
                        @email = objCustMaster.email,
                        @web = objCustMaster.web,
                        @insales = objCustMaster.insaleid,
                        @outsales = objCustMaster.outsaleid,
                        @groupid = objCustMaster.cust_grp_id
                    }, commandType: CommandType.StoredProcedure);

            }
        }

        public CustMaster GetDftCmpWhs(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "sp_ma_cmp_dft_whs_id";
                    IEnumerable<CustMasterDtl> ListInq = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objCustMaster.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                        objCustMaster.ListPickdtl = ListInq.ToList();

                    return objCustMaster;
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
        public CustMaster GetDftWhs(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_MVC_GET_DFTWHS";
                    IEnumerable<CustMasterDtl> ListInq = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objCustMaster.cmp_id,
                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListPickdtl = ListInq.ToList();

                    return objCustMaster;
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
        public CustMaster GetCustDetails(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "sp_ma_get_cust_dtl";
                    IEnumerable<CustMasterDtl> ListInq = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @cmpid = objCustMaster.cust_of_cmp_id,
                            @custid = objCustMaster.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCustDtl = ListInq.ToList();

                    return objCustMaster;
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
        
        public void DeleteCust(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                //const string storedProcedure1 = "proc_get_mvcweb_del_cust";
                const string storedProcedure1 = "sp_ma_del_cust";               
                connection.Execute(storedProcedure1,
                    new
                    {                       
                        @cmpId = objCustMaster.cust_of_cmp_id,
                        @custid = objCustMaster.cmp_id                       
                    }, commandType: CommandType.StoredProcedure);

            }
        }
       
        public void DeleteCmpHdr(CustMaster objCustMaster)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
               // const string storedProcedure1 = "Sp_MVC_MASTER_CMP_HDR_DEL";
                const string storedProcedure1 = "sp_ma_del_cmp_hdr";                
                connection.Execute(storedProcedure1,
                    new
                    {
                        @cmp_id = objCustMaster.cmp_id,
                       
                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public void SaveUserXCmp(CustMaster objCustMaster, string pstrUserList)
        {
            using (IDbConnection connection = ConnectionManager.OpenConnection())
            {
                const string storedProcedure1 = "Sp_MVC_MASTER_INSERT_USR_X_CMP";
                connection.Execute(storedProcedure1,
                    new
                    {

                        @user_id = objCustMaster.user_id,
                        @cmp_id = objCustMaster.cmp_id,
                        @cmp_name = objCustMaster.cmp_name,
                        @pstrUserList = pstrUserList


                    }, commandType: CommandType.StoredProcedure);

            }
        }
        public CustMaster GetTableEntityValueCountByRMA_DocId(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CustMaster objOrderLifeCycleCategory = new CustMaster();

                    const string storedProcedure2 = "SP_MVC_MASTER_CHECK_ENTITY_VALUE_BY_CUST_RMA_DOCID";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objCustMaster.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCheckEntityValueRmaDocId = ListCustDetailsLST.ToList();

                    return objCustMaster;
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
        public CustMaster GetCheckExistCmpId(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {


                    CustMaster objOrderLifeCycleCategory = new CustMaster();

                    const string storedProcedure2 = "SP_MVC_MASTER_CHECK_CMPID_EXIST";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @P_STR_CMP_ID = objCustMaster.cmp_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCheckExistCmpId = ListCustDetailsLST.ToList();

                    return objCustMaster;
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
        public CustMaster GetCustomerLogo(CustMaster objCustMaster)

        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {

                    const string storedProcedure2 = "SP_GET_CUST_LOGO";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                          
                            @P_STR_CUST_ID = objCustMaster.cust_id

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListGetCustLogo = ListCustDetailsLST.ToList();

                    return objCustMaster;
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

        public CustMaster CheckCustomerExist(CustMaster objCustMaster)
        {
            try
            {
                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure2 = "SP_CHECK_CUST_ID_EXIST";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure2,
                        new
                        {
                            @p_str_cmp_id = objCustMaster.cmp_id
                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListGetCustLogo = ListCustDetailsLST.ToList();
                    return objCustMaster;
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

        public CustMaster GetCustMasterRptDetails(CustMaster objCustMaster)
        {
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "sp_ma_get_cust_dtl_rpt";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure1,
                         new
                        {
                            @P_STR_COMP_ID = objCustMaster.cmp_id,
                            @P_STR_CUST_ID = objCustMaster.cust_id,
                            @P_STR_USER_ID = objCustMaster.user_id,

                        },
                        commandType: CommandType.StoredProcedure);
                    objCustMaster.ListCustDetails = ListCustDetailsLST.ToList();
                }

                return objCustMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string CheckExistCustInitial(string p_str_Cust_initial)
        {
            string mOutput = ""; 
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "sp_ma_get_entity_cust_initial";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure1,
                        new
                        {
                            @p_str_Cust_initial = p_str_Cust_initial
                        },
                       commandType: CommandType.StoredProcedure);
                    if (ListCustDetailsLST.Count() > 0)
                    {
                        mOutput = ListCustDetailsLST.First().con_string;
                    }
                    else
                    {
                        mOutput = "";
                    }
                   
                }
              
                return mOutput;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string GetCustInitial(string cmp_id)
        {
            string mOutput = "";
            try
            {

                using (IDbConnection connection = ConnectionManager.OpenConnection())
                {
                    const string storedProcedure1 = "sp_ma_get_cust_initial";
                    IEnumerable<CustMasterDtl> ListCustDetailsLST = connection.Query<CustMasterDtl>(storedProcedure1,
                        new
                        {
                            @cmp_id = cmp_id
                        },
                       commandType: CommandType.StoredProcedure);
                    if (ListCustDetailsLST.Count() > 0)
                    {
                        mOutput = ListCustDetailsLST.First().con_string;
                    }
                    else
                    {
                        mOutput = "";
                    }

                }

                return mOutput;
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
