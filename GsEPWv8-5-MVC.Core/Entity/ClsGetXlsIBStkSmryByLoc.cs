using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Data;
using System.Drawing;
namespace GsEPWv8_5_MVC.Core.Entity
{
    public class ClsGetXlsIBStkSmryByLoc : IDisposable
    {
        private ExcelWorksheet
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
        public ClsGetXlsIBStkSmryByLoc(string templateFile)
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
        public void PopulateHeader(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_cntr_id)
        {
            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "IB STOCK SUMMARY REPORT";
           // ExcelNamedRange ve = this.Names["__DataRptHeader"];

            this.Names["__HDATA_ib_doc_id"].Value = "IB Doc Id: " + p_str_ib_doc_id;
            ExcelNamedRange ve1 = this.Names["__HDATA_ib_doc_id"];

            this.Names["__HDATA_cntr_id"].Value = "Container# " + p_str_cntr_id;
            ExcelNamedRange ve3 = this.Names["__HDATA_cntr_id"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void PopulateData(DataTable data, bool loadImage = false)
        {
            int l_int_line_num = 0;
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
                if (dr["is_ib_rec"].ToString() == "Y")
                { 
                this.Names["__Data_dtl_line"].Value = l_int_line_num + 1;
                this.Names["__DATA_itm_num"].Value = dr["itm_num"];
                this.Names["__DATA_itm_color"].Value = dr["itm_color"];
                this.Names["__DATA_itm_size"].Value = dr["itm_size"];
                this.Names["__DATA_itm_name"].Value = dr["itm_name"];
                this.Names["__Data_doc_ctn"].Value = dr["doc_ctn"];
                this.Names["__Data_doc_qty"].Value = dr["doc_qty"];
                    this.Names["__Data_loc_id"].Value = string.Empty;
                    this.Names["__Data_stk_ctn"].Value = string.Empty;
                    this.Names["__Data_stk_qty"].Value = string.Empty;
                }
                else
                {

                    this.Names["__Data_dtl_line"].Value = string.Empty; 
                    this.Names["__DATA_itm_num"].Value = string.Empty;
                    this.Names["__DATA_itm_color"].Value = string.Empty;
                    this.Names["__DATA_itm_size"].Value = string.Empty;
                    this.Names["__DATA_itm_name"].Value = string.Empty;
                    this.Names["__Data_doc_ctn"].Value = string.Empty;
                    this.Names["__Data_doc_qty"].Value = string.Empty;
                    this.Names["__Data_loc_id"].Value = dr["loc_id"];
                   

                    if (dr["loc_id"].ToString() == "NO STOCK")
                    {
                        this.Names["__Data_stk_ctn"].Value = string.Empty;
                        this.Names["__Data_stk_qty"].Value = string.Empty;
                    }
                    else
                    { 
                        this.Names["__Data_stk_ctn"].Value = dr["stk_ctn"];
                        this.Names["__Data_stk_qty"].Value = dr["stk_qty"];
                    }
                }
                rowIdx = this.InsertRowPaste(this.Names["__TEMPLATE_Data"], rowIdx,1, dr["is_ib_rec"].ToString());


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
        internal int InsertRowPaste(ExcelNamedRange nrng, int ri, int ci = 1,string is_stock = "N")
        {
            if (ri <= 0)
            {
                return -1;
            }
            _RowIndex = ri + 1;
            this.ActiveSheet.InsertRow(_RowIndex, 1);
            nrng.Copy(this.ActiveSheet.Cells[_RowIndex, ci]);
            if (is_stock == "N")
            {
                 
                this.ActiveSheet.Cells["A" + _RowIndex + ":" + "G" + _RowIndex].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightGray;
            }

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
