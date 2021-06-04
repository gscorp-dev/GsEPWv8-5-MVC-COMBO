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
    public class xls_IB_Recv_By_Style_Excel : IDisposable
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
        public xls_IB_Recv_By_Style_Excel(string templateFile)
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

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "INBOUND RECEIVING REPORT BY STYLE ";
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
                this.Names["__DATA_Style"].Value = dr["Style"];
                this.Names["__DATA_WHS"].Value = dr["WHS"];
                this.Names["__DATA_Lot_Id"].Value = dr["Lot_Id"];
                this.Names["__DATA_IB_Doc_ID"].Value = dr["IB_Doc_ID"];
                this.Names["__DATA_Container_ID"].Value = dr["Container_ID"];
                dt = DateTime.Parse(dr["Rcvd_Date"].ToString()); ;
                this.Names["__DATA_Rcvd_Date"].Value = dt.ToString("MM/dd/yyyy");
                this.Names["__DATA_Color"].Value = dr["Color"];
                this.Names["__DATA_Size"].Value = dr["Size"];
                this.Names["__DATA_Item_Name"].Value = dr["Item_Name"];
                this.Names["__DATA_PoNum"].Value = dr["PoNum"];
                this.Names["__DATA_Loc_Id"].Value = dr["Loc_Id"];
                this.Names["__DATA_PPK"].Value = dr["PPK"];
                this.Names["__DATA_Ctns"].Value = dr["Ctns"];
                this.Names["__DATA_Qty"].Value = dr["Qty"];
                this.Names["__DATA_Length"].Value = dr["Length"];
                this.Names["__DATA_width"].Value = dr["width"];
                this.Names["__DATA_Height"].Value = dr["Height"];
                this.Names["__DATA_WGT"].Value = dr["WGT"];
                this.Names["__DATA_Cube"].Value = dr["Cube"];
                this.Names["__DATA_TotCube"].Value = dr["TotCube"];
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
