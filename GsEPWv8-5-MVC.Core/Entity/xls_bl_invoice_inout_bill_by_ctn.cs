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
    public class xls_bl_invoice_inout_bill_by_ctn : IDisposable
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
        public xls_bl_invoice_inout_bill_by_ctn(string templateFile)
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


        public void PopulateHeader(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_bill_dt, string p_str_bill_by, decimal p_dbl_tot_amount, string l_str_cust_name)
        {

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "INOUT BILL INVOICE DOCUMENT REPORT - BY " + p_str_bill_by;
            ExcelNamedRange ve = this.Names["__DataRptHeader"];

            this.Names["__HDATA_bill_doc_id"].Value = "INVOICE ID : " + p_str_bill_doc_id;
            ExcelNamedRange ve1 = this.Names["__HDATA_bill_doc_id"];

            this.Names["__HDATA_bill_pd_to"].Value = "INVOICE DATE : " + p_str_bill_dt;
            ExcelNamedRange ve2 = this.Names["__HDATA_bill_pd_to"];

            this.Names["__HDATA_bill_Amount"].Value = "INVOICE AMOUNT: " + p_dbl_tot_amount.ToString();
            ExcelNamedRange ve3 = this.Names["__HDATA_bill_Amount"];

            this.Names["__HDATA_cust_name"].Value = "BILL TO: " + l_str_cust_name;
            ExcelNamedRange ve4 = this.Names["__HDATA_cust_name"];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void PopulateData(DataTable data, bool loadImage = false)
        {
            Decimal l_dec_amt = 0;
            DateTime dt;
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
                this.Names["__DATA_dtl_line"].Value = dr["dtl_line"];
                dt = DateTime.Parse(dr["rcvd_dt"].ToString());
                this.Names["__DATA_rcvd_dt"].Value = dt.ToString("MM/dd/yyyy");
                this.Names["__DATA_cont_id"].Value = dr["cont_id"];
                this.Names["__DATA_ib_doc_id"].Value = dr["ib_doc_id"];
                this.Names["__DATA_ib_doc_dt"].Value = dr["ib_doc_dt"];
                this.Names["__DATA_ship_itm_num"].Value = dr["ship_itm_num"];
                this.Names["__DATA_lot_id"].Value = dr["lot_id"];
                this.Names["__DATA_so_itm_num"].Value = dr["so_itm_num"];
                this.Names["__DATA_so_itm_color"].Value = dr["so_itm_color"];
                this.Names["__DATA_so_itm_size"].Value = dr["so_itm_size"];
                this.Names["__DATA_ship_ctns"].Value = dr["ship_ctns"];
                this.Names["__DATA_so_itm_price"].Value = dr["so_itm_price"];
                l_dec_amt = Convert.ToDecimal(dr["ship_ctns"]) * Convert.ToDecimal(dr["so_itm_price"]);
                this.Names["__DATA_Amount"].Value = l_dec_amt.ToString();
                rowIdx = this.InsertRowPaste(this.Names["__TEMPLATE_Data"], rowIdx);
            }
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
        internal int InsertRowPaste(ExcelNamedRange nrng, int ri, int ci = 1)
        {
            if (ri <= 0)
            {
                return -1;
            }
            _RowIndex = ri + 1;
            this.ActiveSheet.InsertRow(_RowIndex, 1);
            nrng.Copy(this.ActiveSheet.Cells[_RowIndex, ci]);
            return _RowIndex;
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
