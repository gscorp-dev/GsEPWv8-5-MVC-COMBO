using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace GsEPWv8_5_MVC.Core.Entity
{
    public class xls_IV_Style_Dtl_Excel : IDisposable
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
        public xls_IV_Style_Dtl_Excel(string templateFile)
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


        public void PopulateHeader_IV_Style_Dtl_Excel(string p_str_cmp_id)
        {

            this.Names["__DataRptHeader"].Value = p_str_cmp_id + " - " + "STOCK DETAIL by STYLE";
            ExcelNamedRange ve = this.Names["__DataRptHeader"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void PopulateData_IV_Style_Dtl_Excel(IList<StockInquiryDtl> data, bool loadImage = false)
        {
            if (data == null)
            {
                return;
            }
 
            DataTable dataTable = null;
            Type listType = data.GetType();

            if (listType.IsGenericType & (data != null))
            {
                //determine the underlying type the List<> contains
                Type elementType = listType.GetGenericArguments()[0];

                //create empty table -- give it a name in case
                //it needs to be serialized
                dataTable = new DataTable(elementType.Name + "List");

                //define the table -- add a column for each public
                //property or field


                //populate the table

                foreach (object record in data)
                {
                    int index = 0;
                    object[] fieldValues = new object[dataTable.Columns.Count];
                    foreach (DataColumn columnData in dataTable.Columns)
                    {
                        MemberInfo memberInfo = elementType.GetMember(columnData.ColumnName)[0];
                        if (memberInfo.MemberType == MemberTypes.Property)
                        {
                            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                            fieldValues[index] = propertyInfo.GetValue(record, null);
                        }
                        else if (memberInfo.MemberType == MemberTypes.Field)
                        {
                            FieldInfo fieldInfo = memberInfo as FieldInfo;
                            fieldValues[index] = fieldInfo.GetValue(record);
                        }




                        index += 1;
                    }
                    dataTable.Rows.Add(fieldValues);
                }
            }




            var dataRows = dataTable.Rows;
            var dataColumns = dataTable.Columns;
            int rowSize = dataRows.Count;
            DateTime dte;
            if (rowSize == 0)
            {
                return;
            }
          
            DataTable dtName = new DataTable("__TEMPLATE_Data");

            //dt.Columns.Add("__DATA_WHS", typeof(string));
           dtName.Columns.Add("__DATA_Style", typeof(string));
           dtName.Columns.Add("__DATA_Color", typeof(string));
           dtName.Columns.Add("__DATA_Size", typeof(string));
           dtName.Columns.Add("__DATA_Item_Name", typeof(string));
           dtName.Columns.Add("__DATA_Loc_Id", typeof(string));
            dtName.Columns.Add("__DATA_Ctns", typeof(int));
            dtName.Columns.Add("__DATA_PPK", typeof(int));
         
            dtName.Columns.Add("__DATA_Pcs", typeof(int));
            dtName.Columns.Add("__DATA_Po_Num", typeof(string));
            dtName.Columns.Add("__DATA_Item_Cube", typeof(decimal));
            dtName.Columns.Add("__DATA_Notes", typeof(string));


            int r = 0;
            foreach (var item in data)
            {

                DataRow row = dtName.NewRow();
                // this.Names["__DATA_Lot_Id"].Value = dr["Lot_Id"];
              //  row["__DATA_WHS"] = "";
                // dte = DateTime.Parse(dr["Rcvd_Date"].ToString()); ;
                //this.Names["__DATA_Rcvd_Date"].Value = dt.ToString("MM/dd/yyyy");
                // this.Names["__DATA_IB_Doc_ID"].Value = dr["IB_Doc_ID"];
                //this.Names["__DATA_Container_ID"].Value = dr["Container_ID"];
                row["__DATA_Style"] = item.Style;
                row["__DATA_Color"] = item.Color;
                row["__DATA_Size"] = item.Size;
                row["__DATA_Item_Name"] = item.Item_Name;
                row["__DATA_Loc_Id"] = item.Loc;
                row["__DATA_Ctns"] = item.Ctns;
                row["__DATA_PPK"] = item.Ppk;

                row["__DATA_Pcs"] = item.Pcs;
                row["__DATA_Po_Num"] = item.Po_Num;
                row["__DATA_Item_Cube"] = item.Item_Cube;
                row["__DATA_Notes"] = item.Notes;
                //this.Names["__DATA_Length"].Value = dr["Length"];
                //this.Names["__DATA_width"].Value = dr["width"];
                //this.Names["__DATA_Height"].Value = dr["Height"];
                //this.Names["__DATA_WGT"].Value = dr["WGT"];
                //this.Names["__DATA_Cube"].Value = dr["Cube"];
                //this.Names["__DATA_TotCube"].Value = dr["TotCube"];
                //this.Names["__DATA_cntr_type"].Value = dr["cntr_type"];

                dtName.Rows.Add(row);
                r = r = 1;

            }

           
            int rowIdx = this.Names["__TEMPLATE_DataLine"].Start.Row;

            for (int i = 0; i < dtName.Rows.Count; i++)
            {
                DataRow dr = dataRows[i];
                this.Names["__DATA_Loc_Id"].Value = dtName.Rows[i].ItemArray[4];
                this.Names["__DATA_Style"].Value = dtName.Rows[i].ItemArray[0];
                this.Names["__DATA_Color"].Value = dtName.Rows[i].ItemArray[1];
                this.Names["__DATA_Size"].Value = dtName.Rows[i].ItemArray[2];
                this.Names["__DATA_ItemName"].Value = dtName.Rows[i].ItemArray[3];
                this.Names["__DATA_Ctns"].Value = dtName.Rows[i].ItemArray[5].ToString();
                this.Names["__DATA_PPK"].Value = dtName.Rows[i].ItemArray[6].ToString();
                this.Names["__DATA_Pcs"].Value = dtName.Rows[i].ItemArray[7].ToString();
                this.Names["__DATA_Po_Num"].Value = dtName.Rows[i].ItemArray[8].ToString();
                this.Names["__DATA_Item_Cube"].Value = dtName.Rows[i].ItemArray[9].ToString();
                this.Names["__DATA_Notes"].Value = dtName.Rows[i].ItemArray[10].ToString();
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
