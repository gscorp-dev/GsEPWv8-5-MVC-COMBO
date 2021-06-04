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
    public class xls_ib_rcvd_summary : IDisposable
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
        public xls_ib_rcvd_summary(string templateFile)
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

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + " INBOUND RECEIVING REPORT SUMMARY ";
            ExcelNamedRange ve = this.Names["__DataRptHeader"];

        }
        public void PopulateHeader(string p_str_cmp_id, string p_str_RcvdDtFm, string p_str_RcvdDtTo, string p_str_bill_status)
        {
            string l_str_bill_status = string.Empty;
            if (p_str_bill_status == "ALL")
                l_str_bill_status = string.Empty;
            else
                l_str_bill_status = "(" + p_str_bill_status + ")";
            if (p_str_RcvdDtFm.Length > 0 && p_str_RcvdDtTo.Length > 0)
            {
                this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "INBOUND RECEIVING SUMMARY REPORT " + l_str_bill_status + " BETWEEN " + p_str_RcvdDtFm + " AND " + p_str_RcvdDtTo;
            }
          
            else
            {
                if (p_str_RcvdDtFm.Length > 0)
                {
                    this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "INBOUND RECEIVING SUMMARY REPORT  " + l_str_bill_status + " -" + p_str_RcvdDtFm;
                }
                if (p_str_RcvdDtTo.Length > 0)
                {
                    this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "INBOUND RECEIVING SUMMARY REPORT  " + l_str_bill_status + " UPTO " + p_str_RcvdDtTo;
                }
            }
           

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void PopulateData(DataTable data, bool loadImage = false)
        {
            int l_itn_tot_ctns = 0;
            decimal l_dec_totcube = 0;
            decimal l_dec_totwgt = 0;

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
                this.Names["__DATA_CntrId"].Value = dr["cont_id"];
                this.Names["__DATA_IBDocId"].Value = dr["ib_doc_id"];
                this.Names["__DATA_RcvdDate"].Value = dr["palet_dt"];
                this.Names["__Data_LotId"].Value = dr["lot_id"];
                this.Names["__Data_PaletId"].Value = dr["palet_id"];
                this.Names["__DATA_TotalCtns"].Value = dr["tot_ctns"];
                this.Names["__Data_TotalCube"].Value = dr["tot_cube"];
                this.Names["__Data_TotatWgt"].Value = dr["tot_wgt"];
                l_itn_tot_ctns += Convert.ToInt32(dr["tot_ctns"]);
                l_dec_totcube += Convert.ToDecimal(dr["tot_cube"]) ;
                l_dec_totwgt += Convert.ToDecimal(dr["tot_wgt"]);
                rowIdx = this.InsertRowPaste(this.Names["__TEMPLATE_Data"], rowIdx);

            }

            
            this.Names["__Data_FtTotalCtns"].Value = l_itn_tot_ctns;
            this.Names["__Data_FtTotalCube"].Value = l_dec_totcube;
            this.Names["__Data_FtTotalWeight"].Value = l_dec_totwgt;
            rowIdx = this.InsertRowPaste(this.Names["__TEMPLATE_Footer"], rowIdx, 1, 2);

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
