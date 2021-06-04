using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Data;
namespace GsEPWv8_5_MVC.Core.Entity
{
    public class get_ib_excp_template : IDisposable
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
        public get_ib_excp_template(string templateFile)
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
        public void PopulateHeader(string p_str_cmp_id)
        {
            this.Names["__Data_rpt_hdr"].Value = p_str_cmp_id + " - " + "INBOUND EXCEPTION LIST";
            ExcelNamedRange ve = this.Names["__Data_rpt_hdr"];

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
            //   Image imgNotAvailable = Image.FromFile(ConfigurationManager.AppSettings["templatepath"].ToString() + "\\notavailable.jpg");
            for (int i = 0; i < rowSize; i++)
            {
                DataRow dr = dataRows[i];
                this.Names["__DATA_cmp_id"].Value = dr["cmp_id"];
                this.Names["__DATA_ib_doc_id"].Value = dr["ib_doc_id"];
                this.Names["__DATA_cntr_id"].Value = dr["cntr_id"];
                this.Names["__DATA_status"].Value = dr["status"];
                this.Names["__DATA_ib_doc_dt"].Value = dataColumns.Contains("ib_doc_dt") ? dr["ib_doc_dt"] : string.Empty;
                this.Names["__DATA_rcvd_dt"].Value = dataColumns.Contains("rcvd_dt") ? dr["rcvd_dt"] : string.Empty;
                this.Names["__DATA_lot_id"].Value = dataColumns.Contains("lot_id") ? dr["lot_id"] : string.Empty;
                this.Names["__DATA_req_num"].Value = dataColumns.Contains("req_num") ? dr["req_num"] : string.Empty;
                this.Names["__DATA_po_num"].Value = dataColumns.Contains("po_num") ? dr["po_num"] : string.Empty;

                this.Names["__DATA_tot_ctns"].Value = dr["tot_ctns"];
                this.Names["__DATA_tot_cube"].Value = dr["tot_cube"];
                this.Names["__DATA_tot_wgt"].Value = dr["tot_wgt"];

                this.Names["__DATA_cube_excp"].Value = dataColumns.Contains("cube_excp") ? dr["cube_excp"] : string.Empty;
                this.Names["__Data_dup_itm_excp"].Value = dataColumns.Contains("dup_itm_excp") ? dr["dup_itm_excp"] : string.Empty;
                this.Names["__Data_bill_excp"].Value = dataColumns.Contains("bill_excp") ? dr["bill_excp"] : string.Empty;
                this.Names["__DATA_cntr_type"].Value = dataColumns.Contains("cntr_type") ? dr["cntr_type"] : string.Empty;
                this.Names["__Data_cntr_size_excp"].Value = dataColumns.Contains("cntr_size_excp") ? dr["cntr_size_excp"] : string.Empty;
                this.Names["__Data_doc_excp"].Value = dataColumns.Contains("doc_excp") ? dr["doc_excp"] : string.Empty;

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
