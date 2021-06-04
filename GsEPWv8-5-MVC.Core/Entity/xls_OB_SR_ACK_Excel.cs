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
    public class xls_OB_SR_ACK_Excel : IDisposable
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
        public xls_OB_SR_ACK_Excel(string templateFile)
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


        public void PopulateHeader(string p_str_cmp_id, string p_str_ordr_num, string p_str_so_num, string p_str_So_dt, string p_str_cust_name, string p_str_canceldate, string p_str_shipdate, string p_str_quote_num, string p_str_shipto_id, string p_str_cust_ordr_num)
        {

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "SHIP REQUEST ACKNOWLEDGEMENT - REPORT";
            ExcelNamedRange ve = this.Names["__DataRptHeader"];

            this.Names["__HDATA_ordr_num"].Value = "SO# : " + p_str_ordr_num;
            ExcelNamedRange ve1 = this.Names["__HDATA_ordr_num"];

            this.Names["__HDATA_so_num"].Value = "SR# : " + p_str_so_num;
            ExcelNamedRange ve2 = this.Names["__HDATA_so_num"];

            this.Names["__HDATA_So_dt"].Value = "SR DATE : " + p_str_So_dt;
            ExcelNamedRange ve3 = this.Names["__HDATA_So_dt"];

            this.Names["__HDATA_cust_name"].Value = "CUSTOMER : " + p_str_cust_name;
            ExcelNamedRange ve4 = this.Names["__HDATA_cust_name"];

            this.Names["__HDATA_canceldate"].Value = "CANCEL DT : " + p_str_canceldate;
            ExcelNamedRange ve5 = this.Names["__HDATA_canceldate"];

            this.Names["__HDATA_shipdate"].Value = "SHIP DT : " + p_str_shipdate;
            ExcelNamedRange ve6 = this.Names["__HDATA_shipdate"];

            this.Names["__HDATA_quote_num"].Value = "BATCH ID: " + p_str_quote_num;
            ExcelNamedRange ve7 = this.Names["__HDATA_quote_num"];

            this.Names["__HDATA_shipto_id"].Value = "SHIP TO : " + p_str_shipto_id;
            ExcelNamedRange ve8 = this.Names["__HDATA_shipto_id"];

            this.Names["__HDATA_cust_ordr_num"].Value = "CUST PO# : " + p_str_cust_ordr_num;
            ExcelNamedRange ve9 = this.Names["__HDATA_cust_ordr_num"];

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
                this.Names["__DATA_line_num"].Value = dr["line_num"];
                this.Names["__DATA_shipvia_id"].Value = dr["shipvia_id"];
                this.Names["__DATA_freight_id"].Value = dr["freight_id"];
                this.Names["__DATA_FOB"].Value = dr["FOB"];
                this.Names["__DATA_deptid"].Value = dr["deptid"];
                this.Names["__DATA_store_id"].Value = dr["store_id"];
                this.Names["__DATA_ObDcId"].Value = dr["ObDcId"];
                this.Names["__DATA_terms_id"].Value = dr["terms_id"];
                this.Names["__DATA_itm_num"].Value = dr["itm_num"];
                this.Names["__DATA_itm_color"].Value = dr["itm_color"];
                this.Names["__DATA_itm_size"].Value = dr["itm_size"];
                this.Names["__DATA_itm_name"].Value = dr["itm_name"];
                this.Names["__DATA_po_num"].Value = dr["po_num"];
                this.Names["__DATA_ordr_qty"].Value = dr["ordr_qty"];
                this.Names["__DATA_ctn_qty"].Value = dr["ctn_qty"];
                this.Names["__DATA_ordr_ctns"].Value = dr["ordr_ctns"];
                this.Names["__DATA_itm_cube"].Value = dr["itm_cube"];
                this.Names["__DATA_wgt"].Value = dr["wgt"];
                this.Names["__DATA_Note"].Value = dr["Note"];
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
