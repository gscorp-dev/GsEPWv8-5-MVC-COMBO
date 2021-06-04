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
    public class xls_IB_Recv_By_Container_Excel : IDisposable
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
        public xls_IB_Recv_By_Container_Excel(string templateFile)
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


        public void PopulateHeader(string p_str_cmp_id, string p_str_RcvdDtFm, string p_str_RcvdDtTo)
        {

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "INBOUND RECEIVING REPORT BY CONTAINER";
            ExcelNamedRange ve = this.Names["__DataRptHeader"];
            this.Names["__HDATA_recv_dt_fm"].Value = "RCVD DATE FROM: " + p_str_RcvdDtFm;
            ExcelNamedRange ve1 = this.Names["__HDATA_recv_dt_fm"];
            this.Names["__HDATA_recv_dt_to"].Value = "RCVD DATE TO: " + p_str_RcvdDtTo;
            ExcelNamedRange ve2 = this.Names["__HDATA_recv_dt_to"];

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
            DateTime dt;
            if (rowSize == 0)
            {
                return;
            }
            int rowIdx = this.Names["__TEMPLATE_DataLine"].Start.Row;

            for (int i = 0; i < rowSize; i++)
            {

                DataRow dr = dataRows[i];
                this.Names["__DATA_cntr_id"].Value = dr["cntr_id"];
                this.Names["__DATA_ib_doc_id"].Value = dr["ib_doc_id"];
                this.Names["__DATA_palet_dt"].Value = dr["palet_dt"];
                this.Names["__DATA_ref_num"].Value = dr["po_num"];
                this.Names["__DATA_loc_id"].Value = dr["loc_id"];
                this.Names["__DATA_itm_num"].Value = dr["itm_num"];
                this.Names["__DATA_itm_color"].Value = dr["itm_color"];
                this.Names["__DATA_itm_size"].Value = dr["itm_size"];
                this.Names["__DATA_itm_name"].Value = dr["itm_name"];
                this.Names["__DATA_itm_qty"].Value = dr["itm_qty"];
                this.Names["__DATA_tot_ctn"].Value = dr["tot_ctn"];
                this.Names["__DATA_tot_qty"].Value = dr["tot_qty"];

                this.Names["__DATA_length"].Value = dr["length"];
                this.Names["__DATA_width"].Value = dr["width"];
                this.Names["__DATA_depth"].Value = dr["depth"];
                this.Names["__DATA_cube"].Value = dr["cube"];
                this.Names["__DATA_wgt"].Value = dr["wgt"];
                this.Names["__Data_factory_id"].Value = dr["factory_id"];
                this.Names["__Data_vend_po_num"].Value = dr["po_num"];
                this.Names["__Data_cust_name"].Value = dr["cust_name"];
                this.Names["__Data_cust_po_num"].Value = dr["cust_po_num"];
                this.Names["__Data_pick_list"].Value = dr["pick_list"];
                l_itn_tot_ctns += Convert.ToInt32(dr["tot_ctn"]);
                l_dec_totcube += Convert.ToDecimal(dr["cube"]) * Convert.ToInt32(dr["tot_ctn"]);
                l_dec_totwgt += Convert.ToDecimal(dr["wgt"]) * Convert.ToInt32(dr["tot_ctn"]);
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
