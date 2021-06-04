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
    public class xls_bl_strg_bill_by_style_smry : IDisposable
    {
        private string l_str_bill_by = string.Empty;
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
        public xls_bl_strg_bill_by_style_smry(string templateFile, string p_str_bill_by)
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
            l_str_bill_by = p_str_bill_by;
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


        public void PopulateHeader(string p_str_cmp_id, string p_str_bill_doc_id, string p_str_bill_dt, string p_str_bill_by, decimal p_dbl_tot_amount)
        {

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "STORAGE BILL DOCUMENT REPORT - BY  Style Summary - " + p_str_bill_by;
            ExcelNamedRange ve = this.Names["__DataRptHeader"];

            this.Names["__Data_bill_doc_id"].Value = "BILL DOC ID : " + p_str_bill_doc_id;
            ExcelNamedRange ve1 = this.Names["__Data_bill_doc_id"];

            this.Names["__Data_bill_doc_dt"].Value = "BILL AS OF DATE : " + p_str_bill_dt;
            ExcelNamedRange ve2 = this.Names["__Data_bill_doc_dt"];

            this.Names["__Data_bill_Amount"].Value = "BILL AMOUNT: " + p_dbl_tot_amount.ToString();
            ExcelNamedRange ve3 = this.Names["__Data_bill_Amount"];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void PopulateData(DataTable data, bool loadImage = false)


        {
            decimal l_dec_total_cube = 0;
            Decimal l_dec_amt = 0;

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
            // int rowIdx = 4;
            //   Image imgNotAvailable = Image.FromFile(ConfigurationManager.AppSettings["templatepath"].ToString() + "\\notavailable.jpg");
            for (int i = 0; i < rowSize; i++)
            {

                DataRow dr = dataRows[i];

                this.Names["__DATA_dtl_line"].Value = i+1;
                this.Names["__Data_RateId"].Value = dr["RateId"];
                this.Names["__DATA_Style"].Value = dr["Style"];
                this.Names["__DATA_Color"].Value = dr["Color"];
                this.Names["__DATA_Size"].Value = dr["Size"];
                this.Names["__DATA_TotalCtns"].Value = dr["TotalCtns"];
                this.Names["__DATA_ctn_cube"].Value = dr["ctn_cube"];
                l_dec_total_cube = Convert.ToDecimal(dr["ctn_cube"]) * Convert.ToDecimal(dr["TotalCtns"]);
                this.Names["__Data_TotalCube"].Value = l_dec_total_cube;
                this.Names["__Data_Rate"].Value = dr["Rate"];
                if (l_str_bill_by == "CUBE")
                {
                    l_dec_amt = l_dec_total_cube * Convert.ToDecimal(dr["Rate"]);
                }
                else if (l_str_bill_by == "CARTON")
                {
                    l_dec_amt = Convert.ToDecimal(dr["TotalCtns"]) * Convert.ToDecimal(dr["Rate"]);
                }
                this.Names["__DATA_Amount"].Value = Math.Round(l_dec_amt, 2);
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
