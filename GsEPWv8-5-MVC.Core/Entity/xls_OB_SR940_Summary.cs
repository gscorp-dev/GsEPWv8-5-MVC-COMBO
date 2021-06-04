using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GsEPWv8_5_MVC.Core.Entity
{
    public class xls_OB_SR940_Summary : IDisposable
    {
        private OfficeOpenXml.ExcelWorksheet
                _ActiveSheet,
                _TemplateSheet;
        private ExcelNamedRangeCollection
            _Names;
        private int
            _RowIndex = -1;

        /// <summary>
        /// 
        /// </summary>
        internal enum FormulaTypes
        {
            Row,
            Column
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFile"></param>
        public xls_OB_SR940_Summary(string templateFile)
        {
            if (string.IsNullOrWhiteSpace(templateFile))
            {
                throw new ArgumentException("The template file cannot be empty or null.", "templateFile");
            }
            this.TemplateFile = new FileInfo(templateFile);
            if (!this.TemplateFile.Exists)
            {
                throw new FileNotFoundException("The template file is not found.", "templateFile");
            }
            this.Package = new ExcelPackage(this.TemplateFile);
            this.Workbook = this.Package.Workbook;
        }

        /// <summary>
        /// 
        /// </summary>
        public FileInfo TemplateFile
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ExcelPackage Package
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ExcelWorkbook Workbook
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ExcelWorksheet ActiveSheet
        {
            get
            {
                return _ActiveSheet = (_ActiveSheet ?? this.Workbook.Worksheets[1]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ExcelWorksheet TemplateSheet
        {
            get
            {
                return _TemplateSheet = (_TemplateSheet ?? this.Workbook.Worksheets["Template"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal ExcelNamedRangeCollection Names
        {
            get
            {
                return _Names = (_Names ?? this.Workbook.Names);
            }
        }

        /// <summary>
        /// 
        /// </summary>


        //public void PopulateHeader(string p_str_cmp_id, string l_str_ordr_num, string l_str_quote_num, string l_str_store_id, string l_str_ship_dt, string l_str_so_num, string l_str_cust_ordr_num, string l_str_dept_id, string l_str_cancel_dt, string l_str_cust_name, string l_str_dc_id, string l_str_st_mail_name)
        //{


        //    this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "OB - SR940 - SUMMARY - REPORT";
        //    ExcelNamedRange ve = this.Names["__DataRptHeader"];

        //    this.Names["__HDATA_quote_num"].Value = "BATCH# : " + l_str_quote_num;
        //    ExcelNamedRange ve1 = this.Names["__HDATA_quote_num"];
        //    this.Names["__HDATA_ordr_num"].Value = "PICK SLIP# : " + l_str_ordr_num;
        //    ExcelNamedRange ve2 = this.Names["__HDATA_ordr_num"];
        //    this.Names["__HDATA_store_id"].Value = "STORE : " + l_str_store_id;
        //    ExcelNamedRange ve3 = this.Names["__HDATA_store_id"];

        //    this.Names["__HDATA_ship_dt"].Value = "START DATE : " + l_str_ship_dt;
        //    ExcelNamedRange ve4 = this.Names["__HDATA_ship_dt"];
        //    this.Names["__HDATA_so_num"].Value = "SHIP REQ# : " + l_str_so_num;
        //    ExcelNamedRange ve5 = this.Names["__HDATA_so_num"];
        //    this.Names["__HDATA_cust_ordr_num"].Value = "CUST PO# : " + l_str_cust_ordr_num;
        //    ExcelNamedRange ve6 = this.Names["__HDATA_cust_ordr_num"];
        //    this.Names["__HDATA_dept_id"].Value = "DEPT : " + l_str_dept_id;
        //    ExcelNamedRange ve7 = this.Names["__HDATA_dept_id"];
        //    this.Names["__HDATA_cancel_dt"].Value = "CANCEL DATE : " + l_str_cancel_dt;
        //    ExcelNamedRange ve8 = this.Names["__HDATA_cancel_dt"];
        //    this.Names["__HDATA_cust_name"].Value = "CUSTOMER : " + l_str_cust_name;
        //    ExcelNamedRange ve9 = this.Names["__HDATA_cust_name"];
        //    this.Names["__HDATA_dc_id"].Value = "DC# : " + l_str_dc_id;
        //    ExcelNamedRange ve10 = this.Names["__HDATA_dc_id"];
        //    this.Names["__HDATA_st_mail_name"].Value = "SHIP TO : " + l_str_st_mail_name;
        //    ExcelNamedRange ve11 = this.Names["__HDATA_st_mail_name"];
        //}

    public void PopulateHeader(string p_str_cmp_id)

        {

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "OB - SR940 - SUMMARY - REPORT";
            ExcelNamedRange ve = this.Names["__DataRptHeader"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void PopulateData(DataTable data,ref int l_int_tot_orders, ref int l_itn_tot_ctns, ref decimal l_dec_totcube, ref decimal l_dec_totwgt)
        {
            string l_str_cur_so_num = string.Empty;
            string l_str_perv_so_num = string.Empty;
           
            string l_str_quote_num = string.Empty;
            string l_str_cust_name = string.Empty;
            string l_str_cust_ordr_num = string.Empty;
            string l_str_so_num = string.Empty;
            string l_str_ordr_num = string.Empty;
            string l_str_dc_id = string.Empty;
            //int l_itn_tot_ctns = 0;
            //decimal l_dec_totcube = 0;
            //decimal l_dec_totwgt = 0;
            //int l_int_tot_orders = 0;

            if (data == null)
            {
                return;
            }
            var dataRows = data.Rows;
            var dataColumns = data.Columns;
            int rowSize = dataRows.Count;
            if (rowSize == 0)
            {
                return;
            }
            int rowIdx = this.Names["__TEMPLATE_DataLine"].Start.Row;
           
            for (int i = 0; i < rowSize; i++)
            {
                DataRow dr = dataRows[i];
                l_str_cur_so_num = dr["so_num"].ToString();
                if (l_str_cur_so_num != l_str_perv_so_num)
                {
                    l_int_tot_orders += 1;
                    this.Names["__Data_Hdr_BatchNum"].Value = dr["quote_num"];
                    this.Names["__Data_Hdr_Customer"].Value = dr["cust_name"];
                    this.Names["__Data_Hdr_CustPONum"].Value = dr["cust_ordr_num"];
                    this.Names["__Data_Hdr_SoNum"].Value = dr["so_num"];
                    this.Names["__Data_Hdr_SoDt"].Value = dr["so_dt"];
                    this.Names["__Data_Hdr_CustRefNum"].Value = dr["ordr_num"]; 
                    this.Names["__Data_Hdr_DC"].Value = dr["dc_id"];
                    rowIdx = this.InsertRowPaste(this.Names["__Data_group_hdr"], rowIdx,1,1);
                }
                
                this.Names["__DATA_line_num"].Value = dr["line_num"];
                this.Names["__DATA_itm_num"].Value = dr["itm_num"];
                this.Names["__DATA_itm_color"].Value = dr["itm_color"];
                this.Names["__DATA_itm_size"].Value = dr["itm_size"];
                this.Names["__DATA_itm_name"].Value = dr["itm_name"];
                this.Names["__DATA_ordr_qty"].Value = dr["ordr_qty"];
                this.Names["__DATA_ordr_ctns"].Value = dr["ordr_ctns"];
                this.Names["__DATA_itm_qty"].Value =  dr["itm_qty"];
                this.Names["__DATA_cube"].Value = dr["cube"];
                this.Names["__DATA_wgt"].Value = dr["wgt"];
                this.Names["__Data_StartDate"].Value = dr["ship_dt"];
                this.Names["__DataCancelDate"].Value = dr["cancel_dt"];
                this.Names["__DeptId"].Value = dr["dept_id"];
                this.Names["__StoreId"].Value = dr["store_id"];
                this.Names["__Data_ShipTo"].Value = dr["shipto_id"];

                l_itn_tot_ctns += Convert.ToInt32( dr["ordr_ctns"]) ;
                l_dec_totcube += Convert.ToDecimal(dr["cube"]) * Convert.ToInt32(dr["ordr_ctns"]);
                l_dec_totwgt += Convert.ToDecimal(dr["wgt"] ) * Convert.ToInt32(dr["ordr_ctns"]);
                rowIdx = this.InsertRowPaste(this.Names["__TEMPLATE_Data"], rowIdx,1);
                l_str_perv_so_num = l_str_cur_so_num;
            }

          //  l_int_tot_orders += l_int_tot_orders;
            this.Names["__Data_TotOrders"].Value = " Total Orders -- > " + l_int_tot_orders;
            this.Names["__Data_TotCtns"].Value = l_itn_tot_ctns;
            this.Names["__DataTotCube"].Value = l_dec_totcube;
            this.Names["__DataTotWeight"].Value = l_dec_totwgt;
            rowIdx = this.InsertRowPaste(this.Names["__Data_footer_Total"], rowIdx,1,2);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clean()
        {
            int size = 0;
            var namedRanges = this.GetDefinedNames();
            if (namedRanges == null || (size = namedRanges.Length) == 0)
            {
                return;
            }
            for (int i = 0; i < size; i++)
            {
                this.Workbook.Names.Remove(namedRanges[i]);
            }
            if (this.TemplateSheet != null)
            {
                this.Workbook.Worksheets.Delete(this.TemplateSheet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Workbook.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public void SaveAs(string file)
        {
            this.Clean();
            this.Package.SaveAs(new FileInfo(file));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nrng"></param>
        /// <param name="ri"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        internal int InsertRowPaste(ExcelNamedRange nrng, int ri, int ci = 1, int totrows = 1)
        {
            if (ri <= 0)
            {
                return -1;
            }
            _RowIndex = ri + 1;
            this.ActiveSheet.Select();
           this.ActiveSheet.InsertRow(_RowIndex, totrows);
            nrng.Copy(this.ActiveSheet.Cells[_RowIndex, ci]);
            return _RowIndex - 1 + totrows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="namedRanges"></param>
        /// <returns></returns>
        internal string[] GetDefinedNames(ExcelNamedRangeCollection namedRanges)
        {
            List<string> list = new List<string>();
            if (namedRanges != null)
            {
                foreach (ExcelNamedRange nr in namedRanges)
                {
                    if (nr.Name.StartsWith("__"))
                    {
                        list.Add(nr.Name);
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string[] GetDefinedNames()
        {
            return
                this.GetDefinedNames(this.Workbook.Names);
        }
    }

}
