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
    public class xls_Item_and_Stock_Dim_Comp : IDisposable
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
        public xls_Item_and_Stock_Dim_Comp(string templateFile)
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

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "ITEM and STOCK DIMS COMPARISON REPORT ";
            ExcelNamedRange ve = this.Names["__DataRptHeader"];
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
                this.Names["__DATA_SlNo"].Value =( i + 1).ToString();
                this.Names["__DATA_ItmCode"].Value = dr["itm_code"];
                this.Names["__DATA_Style"].Value = dr["itm_num"];
                this.Names["__DATA_Color"].Value = dr["itm_color"];
                this.Names["__DATA_Size"].Value = dr["itm_size"];
                this.Names["__DATA_Ib_Doc_Id"].Value = dr["ib_doc_id"];
                this.Names["__DATA_Cntr_id"].Value = dr["cont_id"];
                this.Names["__DATA_Stk_Length"].Value = dr["stk_length"];
                this.Names["__DATA_Stk_Width"].Value = dr["stk_width"];
                this.Names["__DATA_Stk_Depth"].Value = dr["stk_depth"];
                this.Names["__DATA_Stk_Cube"].Value = dr["stk_cube"];
                this.Names["__DATA_Stk_Weight"].Value = dr["stk_wgt"];


                this.Names["__DATA_Avail_ctns"].Value = dr["stk_length"];
                this.Names["__DATA_PPk"].Value = dr["stk_width"];
                this.Names["__DATA_avail_qty"].Value = dr["stk_depth"];
                this.Names["__DATA_LocId"].Value = dr["stk_cube"];
       

                this.Names["__DATA_Length"].Value = dr["Length"];
                this.Names["__DATA_Width"].Value = dr["Width"];
                this.Names["__DATA_Depth"].Value = dr["Depth"];
                this.Names["__DATA_Cube"].Value = dr["Cube"];
                this.Names["__DATA_Weight"].Value = dr["Weight"];
                this.Names["__DATA_cube_mis_match"].Value = dr["cube_mis_match"];

                this.Names["__DATA_Avail_ctns"].Value = dr["avail_ctns"];
                this.Names["__DATA_PPk"].Value = dr["itm_qty"];
                this.Names["__DATA_avail_qty"].Value = dr["avail_qty"];
                this.Names["__DATA_LocId"].Value = dr["loc_id"];

                this.Names["__DATA_New_Length"].Value =string.Empty;
                this.Names["__DATA_New_Width"].Value = string.Empty;
                this.Names["__DATA_New_Depth"].Value = string.Empty;
                this.Names["__DATA_New_Cube"].Value = string.Empty;
                this.Names["__DATA_New_Weight"].Value = string.Empty;



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
