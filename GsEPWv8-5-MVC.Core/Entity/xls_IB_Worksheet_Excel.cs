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
    public class xls_IB_Worksheet_Excel : IDisposable
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
        public xls_IB_Worksheet_Excel(string templateFile)
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


        public void PopulateHeader(string p_str_cmp_id, string p_str_ib_doc_id, string p_str_ib_doc_dt, string p_str_cntr_id, string p_str_req_num, string p_str_eta_dt, string p_str_vend_id)
        {

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "INBOUND WORKSHEET - REPORT ";
            ExcelNamedRange ve = this.Names["__DataRptHeader"];

            this.Names["__HDATA_ib_doc_id"].Value = "IB DOC ID : " + p_str_ib_doc_id;
            ExcelNamedRange ve1 = this.Names["__HDATA_ib_doc_id"];

            this.Names["__HDATA_ib_doc_dt"].Value = "IB DOC DATE : " + p_str_ib_doc_dt;
            ExcelNamedRange ve2 = this.Names["__HDATA_ib_doc_dt"];

            this.Names["__HDATA_cntr_id"].Value = "CNTR ID : " + p_str_cntr_id;
            ExcelNamedRange ve3 = this.Names["__HDATA_cntr_id"];

            this.Names["__HDATA_req_num"].Value = "REF NO : " + p_str_req_num;
            ExcelNamedRange ve4 = this.Names["__HDATA_req_num"];

            this.Names["__HDATA_eta_dt"].Value = "ETA DATE : " + p_str_eta_dt;
            ExcelNamedRange ve5 = this.Names["__HDATA_eta_dt"];

            this.Names["__HDATA_vend_id"].Value = "VENDOR ID : " + p_str_vend_id;
            ExcelNamedRange ve6 = this.Names["__HDATA_vend_id"];

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void PopulateData(DataTable data, bool loadImage = false)
        {
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

                this.Names["__Data_dtl_line"].Value = dr["dtl_line"];
                this.Names["__Data_tot_qty"].Value = dr["tot_qty"];
                this.Names["__Data_tot_ctn"].Value = dr["tot_ctn"];
                this.Names["__Data_itm_qty"].Value = dr["itm_qty"];
                this.Names["__Data_po_num"].Value = dr["po_num"];
                this.Names["__Data_itm_num"].Value = dr["itm_num"];
                this.Names["__Data_itm_color"].Value = dr["itm_color"];
                this.Names["__Data_itm_size"].Value = dr["itm_size"];
                this.Names["__Data_itm_name"].Value = dr["itm_name"];
                this.Names["__Data_length"].Value = dr["length"];
                this.Names["__Data_width"].Value = dr["width"];
                this.Names["__Data_depth"].Value = dr["depth"];
                this.Names["__Data_workCube"].Value = dr["workCube"];
                this.Names["__Data_wgt"].Value = dr["wgt"];
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
