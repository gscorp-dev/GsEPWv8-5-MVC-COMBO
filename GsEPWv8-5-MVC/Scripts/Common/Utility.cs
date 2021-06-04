
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using System.Net;

using System.Security.Cryptography;
using System.Configuration;
using BusinessObject2;

namespace GsEPWv8_4_MVC.Common
{
    sealed class Utility
    {
        #region "Return DateTime"

        public static DateTime ConvertToDateTime(string txt)
        {
            DateTime d = new DateTime();

            try
            {
                if (txt.Length > 0)
                {
                    d = DateTime.Parse(txt);// DateTime.ParseExact(txt, "yyyy-dd-MM", new CultureInfo("en-US"));
                    d = (d > DateTime.Parse("01/01/1900") ? d : DateTime.Parse("01/01/1900"));
                }
                else
                    d = DateTime.Parse("01/01/1900");

            }
            catch
            {
            }

            return d;
        }



        public static DateTime GetDateEnd(DateTime startday)
        {
            DateTime date = new DateTime();
            date = startday.AddDays(6);
            return date;
        }

        public static DateTime ValidateDate(string txt)
        {
            DateTime d = new DateTime();

            try
            {
                if (txt.Length > 0)
                {
                    d = DateTime.Parse(txt);
                }
            }
            catch
            {
                d = DateTime.Parse("01/01/1900");
            }

            return d;
        }

        #endregion "Return DateTime"

        #region "Return String"

        public static string CreateResidentImageHyperLinkUrl(string resident_id, string ccare_id, string fullname)
        {
            return "~/ResidentImageLoader.ashx?resident_id=" + resident_id.Trim() + "&ccare_id=" + ccare_id.Trim() + "&fullname=" + fullname.Trim();
        }

        public static string MIMETypeDescription(Enum mimeType)
        {
            FieldInfo fi = mimeType.GetType().GetField(mimeType.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return mimeType.ToString();
            }
        }

        public static string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }

        public static string RepSingleQuoteChar(string TheText)
        {
            return
                string.IsNullOrEmpty(TheText)
                    ? null
                    : TheText.Replace("'", "''");

            /*
            try
            {
                return TheText.Replace("'", "`");
            }
            catch
            {
                return "";
            }*/

        }

        //public static string GetFileText(string filename, string ext)
        //{
        //    string sreturn = "";
        //    switch (ext.ToUpper())
        //    {
        //        case "DOC":
        //            Doc.TextLoader loader = new Doc.TextLoader(filename);
        //            if (!loader.LoadText(out sreturn))
        //                break;

        //            break;
        //        case "DOCX":
        //            Rhajjie2.DocxToText dtt = new DocxToText(filename);
        //            sreturn = dtt.ExtractText();

        //            break;
        //        case "PDF":
        //            try
        //            {
        //                PDFParser pdfParser = new PDFParser();
        //                sreturn = pdfParser.ExtractText(filename);
        //            }
        //            catch
        //            {
        //                ////Remove - pdftotext

        //                //Winnovative.PdfToTextConverter pdfToTextConverter = new Winnovative.PdfToTextConverter();
        //                //pdfToTextConverter.Layout = Winnovative.TextLayout.OriginalLayout;
        //                //pdfToTextConverter.MarkPageBreaks = false;
        //                //pdfToTextConverter.AddHtmlMetaTags = false;
        //                //pdfToTextConverter.UserPassword = "";
        //                //sreturn = pdfToTextConverter.ConvertToText(filename);

        //                ////Remove - pdftotext
        //            }

        //            break;
        //        case "XLS":
        //        case "XLSX":
        //            break;
        //        case "TXT":
        //        case "SQL":
        //        case "BAT":
        //        case "VBS":
        //            StreamReader re = File.OpenText(filename);
        //            string input = null;
        //            while ((input = re.ReadLine()) != null)
        //            {
        //                sreturn = sreturn + input;
        //            }
        //            re.Close();
        //            break;
        //    }

        //    return sreturn;
        //}

        public static string formatBytes(float bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = 0;

            if (bytes < 1024.0)
            {
                return String.Format("{0:0.00} {1}", bytes, Suffix[0]);
            }
            else
            {

                for (i = 0; (int)(bytes / 1024) > 0; i++, bytes /= 1024)
                    dblSByte = bytes / 1024.0;

                return String.Format("{0:0.00} {1}", dblSByte, Suffix[i]);
            }
        }

        public static string AS400DSN
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["AS400DSN"] != null)
                    return System.Web.HttpContext.Current.Session["AS400DSN"].ToString().Trim();
                else
                    return "AS400PRD";
            }
            set
            {
                System.Web.HttpContext.Current.Session["AS400DSN"] = value;
            }
        }

        public static string GetDayName(int day, int type)
        {
            string sday = "MON";
            switch (day)
            {
                case 1:
                    if (type == 0)
                        sday = "MON";
                    else
                        sday = "MONDAY";

                    break;
                case 2:
                    if (type == 0)
                        sday = "TUE";
                    else
                        sday = "TUESDAY";

                    break;
                case 3:
                    if (type == 0)
                        sday = "WED";
                    else
                        sday = "WEDNESDAY";

                    break;
                case 4:
                    if (type == 0)
                        sday = "THU";
                    else
                        sday = "THURSDAY";

                    break;
                case 5:
                    if (type == 0)
                        sday = "FRI";
                    else
                        sday = "FRIDAY";

                    break;
                case 6:
                    if (type == 0)
                        sday = "SAT";
                    else
                        sday = "SATURDAY";

                    break;
                case 7:
                    if (type == 0)
                        sday = "SUN";
                    else
                        sday = "SUNDAY";

                    break;
            }
            return sday;
        }

        #endregion "Return String"

        #region "Return Int"

        public static int ConvertToInt32(string txt)
        {
            int d = 0;

            try
            {
                if (txt.Length > 0)
                {
                    txt = txt.Replace("$", "");
                    txt = txt.Replace("(", "-");
                    txt = txt.Replace(")", "");
                    txt = txt.Replace(",", "");

                    d = System.Convert.ToInt32(txt);
                }
            }
            catch
            {
            }

            return d;
        }

        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            if (dtPassed.Month.Equals(12))
            {
                int tmpweek = weekNum + 1;

                if (tmpweek > 53)
                    weekNum = 52;
            }

            if (dtPassed.Month.Equals(1))
            {
                int tmpweek = weekNum + 1;

                if (tmpweek > 53)
                    weekNum = 0;
            }
            return weekNum + 1;
        }

        public static int GetDayNum(string day)
        {
            int iday = 1;
            switch (day.Trim().ToUpper())
            {
                case "MON":
                case "MONDAY":
                    iday = 1;
                    break;
                case "TUE":
                case "TUESDAY":
                    iday = 2;
                    break;
                case "WED":
                case "WEDNESDAY":
                    iday = 3;
                    break;
                case "THU":
                case "THURSDAY":
                    iday = 4;
                    break;
                case "FRI":
                case "FRIDAY":
                    iday = 5;
                    break;
                case "SAT":
                case "SATURDAY":
                    iday = 6;
                    break;
                case "SUN":
                case "SUNDAY":
                    iday = 7;
                    break;
            }
            return iday;
        }

        #endregion "Return Int"

        #region "Return Double"

        public static double ConvertToDouble(string txt)
        {
            double d = 0;

            try
            {
                if (txt.Length > 0)
                {
                    txt = txt.Replace("$", "");
                    txt = txt.Replace("(", "-");
                    txt = txt.Replace(")", "");

                    d = System.Convert.ToDouble(txt);
                }
            }
            catch
            {
            }

            return d;
        }
        public static Int32 getInt(string strInput)
        {
            int index = strInput.IndexOf(".");
            if (index > 0)
                return Convert.ToInt32(strInput.Substring(0, index));
            else
            {

                return Convert.ToInt32(strInput);
            }

        }

        public static double ConvertSize(double bytes, string type)
        {
            try
            {
                const int CONVERSION_VALUE = 1024;
                //determine what conversion they want
                switch (type)
                {
                    case "BY":
                        //convert to bytes (default)
                        return bytes;
                    case "KB":
                        //convert to kilobytes
                        return (bytes / CONVERSION_VALUE);
                    case "MB":
                        //convert to megabytes
                        return (bytes / CalculateSquare(CONVERSION_VALUE));
                    case "GB":
                        //convert to gigabytes
                        return (bytes / CalculateCube(CONVERSION_VALUE));
                    default:
                        //default
                        return bytes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public static double CalculateSquare(Int32 number)
        {
            return Math.Pow(number, 2);
        }

        public static double CalculateCube(Int32 number)
        {
            return Math.Pow(number, 3);
        }

        public static double ConvertBytesToKilobytes(long bytes)
        {
            return bytes / 1024f;
        }

        public static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        #endregion "Return Double"

        #region "Return long"

        public static long ConvertToLong(string txt)
        {
            long d = 0;

            try
            {
                if (txt.Length > 0)
                {
                    txt = txt.Replace("$", "");
                    txt = txt.Replace("(", "-");
                    txt = txt.Replace(")", "");

                    d = System.Convert.ToInt64(txt);
                }
            }
            catch
            {
            }

            return d;
        }

        public static long GetFileSize(string filename)
        {
            FileInfo f = new FileInfo(filename);
            long s1 = f.Length;

            return s1;
        }

        #endregion "Return long"

        #region "return Bool"

        public static bool ConvertToBool(string txt)
        {
            bool d = false;

            try
            {
                d = System.Convert.ToBoolean(txt);
            }
            catch
            {
                switch (txt.ToUpper())
                {
                    case "TRUE":
                    case "1":
                        d = true;
                        break;
                    case "FALSE":
                    case "0":
                        d = false;
                        break;
                }
            }

            return d;
        }

        public static bool IsDate(string date)
        {
            switch (date)
            {
                case "01/01/0001":
                case "1/1/0001":
                case "01/01/1900":
                case "1/1/1900":
                case "":
                    return false;
            }

            DateTime tempDate;
            return DateTime.TryParse(date, out tempDate) ? true : false;
        }



        public static bool SQLInjectionCheck(string TheText)
        {
            bool isValid = true;


            //Modified regex for detection of SQL meta-characters
            //This signature first looks out for the = sign or its hex equivalent (%3D).
            //It then allows for zero or more non-newline characters, and then it checks
            //for the single-quote, the double-dash or the semi-colon. 
            if (Regex.IsMatch(TheText, @"/((\%3D)|(=))[^\n]*((\%27)|(\')|(\-\-)|(\%3B)|(;))/i"))
            {
                isValid = false;
            }

            //Regex for typical SQL Injection attack
            //\w* - zero or more alphanumeric or underscore characters
            //(\%27)|\' - the ubiquitous single-quote or its hex equivalent
            //(\%6F)|o|(\%4F))((\%72)|r|(\%52) - the word 'or' with various combinations of
            //its upper and lower case hex equivalents. 
            if (Regex.IsMatch(TheText, @"/\w*((\%27)|(\'))((\%6F)|o|(\%4F))((\%72)|r|(\%52))/ix"))
            {
                isValid = false;
            }

            //Regex for detecting SQL Injection with the UNION keyword
            //(\%27)|(\') - the single-quote and its hex equivalent
            //union - the keyword union
            if (Regex.IsMatch(TheText, @"/((\%27)|(\'))union/ix"))
            {
                isValid = false;
            }

            //Regex for detecting SQL Injection attacks on a MS SQL Server
            //exec - the keyword required to run the stored or extended procedure
            //(\s|\+)+ - one or more whitespaces or their HTTP encoded equivalents
            //(s|x)p - the letters 'sp' or 'xp' to identify stored or extended procedures respectively
            //\w+ - one or more alphanumeric or underscore characters to complete the name of the procedure 
            if (Regex.IsMatch(TheText, @"/exec(\s|\+)+(s|x)p\w+/ix"))
            {
                isValid = false;
            }

            return isValid;

        }

        public static bool IsValidEmailAddress(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public static bool IsDirectoryEmpty(string path)
        {
            IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
            using (IEnumerator<string> en = items.GetEnumerator())
            {
                return !en.MoveNext();
            }
        }

        #endregion "return Bool"

        #region "Return DataTable"

        public static DataTable ConvertTo<T>(IList<T> lst)
        {
            //create DataTable Structure
            DataTable tbl = CreateTable<T>();
            Type entType = typeof(T);

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            //get the list item and add into the list
            foreach (T item in lst)
            {
                DataRow row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                tbl.Rows.Add(row);
            }

            return tbl;
        }

        public static DataTable CreateTable<T>()
        {
            //T –> ClassName
            Type entType = typeof(T);
            //set the datatable name as class name
            DataTable tbl = new DataTable(entType.Name);
            //get the property list
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                //add property as column
                tbl.Columns.Add(prop.Name, prop.PropertyType);
            }
            return tbl;
        }

        public static DataTable ConvertListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable SelectDistinct(DataTable SourceTable, params string[] Columns)
        {

            DataTable Result = new DataTable();

            if (SourceTable != null)
            {

                DataView DView = SourceTable.DefaultView;

                try
                {

                    Result = DView.ToTable(true, Columns);

                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }

            return Result;

        }

  

        #endregion "Return DataTable"

        #region "DropDownList"

        public static void SetListBoxSelectedValue(ListBox ddl, string val)
        {
            try
            {
                bool bfound = false;
                int idefault = 0;
                for (int i = 0; i < ddl.Items.Count; i++)
                {
                    ddl.SelectedIndex = i;
                    if (ddl.SelectedValue.Trim().ToUpper().Equals(val.Trim().ToUpper()))
                    {
                        bfound = true;
                        break;
                    }

                    if (ddl.SelectedValue.Equals(0))
                        idefault = i;

                }
                if (!bfound)
                    ddl.SelectedIndex = idefault;
            }
            catch
            {
            }
        }

        public static void SetListBoxSelectedItemText(ListBox ddl, string val)
        {
            try
            {
                bool bfound = false;
                for (int i = 0; i < ddl.Items.Count; i++)
                {
                    ddl.SelectedIndex = i;
                    try
                    {
                        if (ConvertToDouble(ddl.SelectedItem.Text.Trim()).Equals(ConvertToDouble(val.Trim())))
                        {
                            bfound = true;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
                if (!bfound)
                    ddl.SelectedIndex = 0;
            }
            catch
            {
            }
        }

        public static void SetListBoxSelectedText(ListBox ddl, string val)
        {
            try
            {
                bool bfound = false;
                int idefault = 0;
                for (int i = 0; i < ddl.Items.Count; i++)
                {
                    ddl.SelectedIndex = i;
                    try
                    {
                        if (ddl.SelectedItem.Text.Trim().Equals(val.Trim()))
                        {
                            bfound = true;
                            break;
                        }

                        if (ddl.SelectedValue.Equals(0))
                            idefault = i;

                    }
                    catch
                    {
                    }
                }
                if (!bfound)
                    ddl.SelectedIndex = idefault;
            }
            catch
            {
            }
        }

        public static void SetDropDownSelectedValue(DropDownList ddl, string val)
        {
            try
            {
                bool bfound = false;
                int idefault = 0;
                for (int i = 0; i < ddl.Items.Count; i++)
                {
                    ddl.SelectedIndex = i;
                    if (ddl.SelectedValue.Trim().ToUpper().Equals(val.Trim().ToUpper()))
                    {
                        bfound = true;
                        break;
                    }

                    if (ddl.SelectedValue.Equals(0))
                        idefault = i;

                }
                if (!bfound)
                    ddl.SelectedIndex = idefault;
            }
            catch
            {
            }
        }

        public static void SetDropDownSelectedItemText(DropDownList ddl, string val)
        {
            try
            {
                bool bfound = false;
                for (int i = 0; i < ddl.Items.Count; i++)
                {
                    ddl.SelectedIndex = i;
                    try
                    {
                        if (ConvertToDouble(ddl.SelectedItem.Text.Trim()).Equals(ConvertToDouble(val.Trim())))
                        {
                            bfound = true;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
                if (!bfound)
                    ddl.SelectedIndex = 0;
            }
            catch
            {
            }
        }

        public static void SetDropDownSelectedText(DropDownList ddl, string val)
        {
            try
            {
                bool bfound = false;
                int idefault = 0;
                for (int i = 0; i < ddl.Items.Count; i++)
                {
                    ddl.SelectedIndex = i;
                    try
                    {
                        if (ddl.SelectedItem.Text.Trim().Equals(val.Trim()))
                        {
                            bfound = true;
                            break;
                        }

                        if (ddl.SelectedValue.Equals(0))
                            idefault = i;

                    }
                    catch
                    {
                    }
                }
                if (!bfound)
                    ddl.SelectedIndex = idefault;
            }
            catch
            {
            }
        }


        #endregion "DropDownList"

        #region "CheckBoxlist"

        public static void FillCheckBoxList(CheckBoxList ddl, string[] param)
        {
            DataTable dtobj = new DataTable("STATUS");
            DataColumn dc = new DataColumn("ID", System.Type.GetType("System.Double"));
            dtobj.Columns.Add(dc);
            DataColumn dc1 = new DataColumn("DESCRIPTION", System.Type.GetType("System.String"));
            dtobj.Columns.Add(dc1);

            DataRow newRow = dtobj.NewRow();

            for (int i = 0; i < param.Length; i++)
            {
                newRow = dtobj.NewRow();
                newRow["ID"] = i;
                newRow["DESCRIPTION"] = param[i];
                dtobj.Rows.Add(newRow);
            }

            ddl.DataSource = dtobj.DefaultView;
            ddl.DataTextField = "DESCRIPTION";
            ddl.DataValueField = "ID";
            ddl.DataBind();
        }

        public static void FillCheckBoxList(CheckBoxList ddl, string DataValueField, string DataTextField, DataTable dt, string sortDirection)
        {
            string strText = "";
            int IndexIs = -1;
            try
            {
                System.Data.DataTable tbl = null;
                tbl = new System.Data.DataTable("dt");
                tbl = dt;

                string columnKey = DataTextField;
                string sortFormat = "{0} {1}";
                if (sortDirection.Length == 0)
                    sortDirection = "ASC";
                else
                    tbl.DefaultView.Sort = string.Format(sortFormat, columnKey, sortDirection);

                ddl.DataSource = tbl.DefaultView;
                ddl.DataTextField = DataTextField;
                ddl.DataValueField = DataValueField;
                ddl.DataBind();
            }
            catch
            {
                if (ddl.Items.Count > 0)
                {
                    IndexIs = ddl.Items.IndexOf(((ListItem)ddl.Items.FindByText(strText)));
                    ddl.SelectedIndex = IndexIs;
                }
            }
        }

        #endregion "CheckBoxlist"

        #region "RadioButtonList"

        public static void FillRadioButtonList(RadioButtonList ddl, string[] param)
        {
            DataTable dtobj = new DataTable("STATUS");
            DataColumn dc = new DataColumn("ID", System.Type.GetType("System.Double"));
            dtobj.Columns.Add(dc);
            DataColumn dc1 = new DataColumn("DESCRIPTION", System.Type.GetType("System.String"));
            dtobj.Columns.Add(dc1);

            DataRow newRow = dtobj.NewRow();

            for (int i = 0; i < param.Length; i++)
            {
                newRow = dtobj.NewRow();
                newRow["ID"] = i;
                newRow["DESCRIPTION"] = param[i];
                dtobj.Rows.Add(newRow);
            }

            ddl.DataSource = dtobj.DefaultView;
            ddl.DataTextField = "DESCRIPTION";
            ddl.DataValueField = "ID";
            ddl.DataBind();
        }

        public static void FillRadioButtonList(RadioButtonList ddl, string DataValueField, string DataTextField, DataTable dt, string sortDirection)
        {
            string strText = "";
            int IndexIs = -1;
            try
            {
                System.Data.DataTable tbl = null;
                tbl = new System.Data.DataTable("dt");
                tbl = dt;

                string columnKey = DataTextField;
                string sortFormat = "{0} {1}";
                if (sortDirection.Length == 0)
                    sortDirection = "ASC";
                else
                    tbl.DefaultView.Sort = string.Format(sortFormat, columnKey, sortDirection);

                ddl.DataSource = tbl.DefaultView;
                ddl.DataTextField = DataTextField;
                ddl.DataValueField = DataValueField;
                ddl.DataBind();
            }
            catch
            {
                if (ddl.Items.Count > 0)
                {
                    IndexIs = ddl.Items.IndexOf(((ListItem)ddl.Items.FindByText(strText)));
                    ddl.SelectedIndex = IndexIs;
                }
            }
        }

        #endregion "RadioButtonList"

        #region "Crystal Reports"




        public static CrystalDecisions.CrystalReports.Engine.ReportDocument SetReportLogin(CrystalDecisions.CrystalReports.Engine.ReportDocument RptName, string AS400_dsn,
          string AS400_providerName, string AS400_ServerInstance, string AS400_DB, string As400_User, string As400_Password,
          string MSSQL_ServerInstance, string MSSQL_DB, string MSSQL_User, string MSSQL_Password)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = RptName;
            //bool IsAS400 = false;
            string dsn = "AS400PRD";
            string dscon = "";
            for (int i = 0; i < rpt.DataSourceConnections.Count; i++)
            {
                if (rpt.DataSourceConnections[i].ServerName.Length >= 5)
                    dscon = rpt.DataSourceConnections[i].ServerName.Trim().Substring(0, 5);
                else
                    dscon = rpt.DataSourceConnections[i].ServerName.Trim();

                switch (dscon)
                {
                    case "AS400":
                        //rpt.DataSourceConnections[i].SetLogon(As400_User, As400_Password);
                        //rpt.DataSourceConnections[i].IntegratedSecurity = false;
                        //IsAS400 = true;
                        try
                        {
                            if (AS400_dsn.Length > 0)
                                dsn = AS400_dsn;
                            else
                                dsn = AS400DSN.ToString();
                        }
                        catch
                        {
                        }

                        CrystalDecisions.Shared.ConnectionInfo connInfo = new CrystalDecisions.Shared.ConnectionInfo();
                        connInfo.IntegratedSecurity = false;
                        connInfo.UserID = As400_User;
                        connInfo.Password = As400_Password;
                        connInfo.ServerName = dsn;
                        connInfo.DatabaseName = "";
                        connInfo.AllowCustomConnection = true;
                        connInfo.LogonProperties.Clear();
                        connInfo.LogonProperties.Add(new CrystalDecisions.Shared.NameValuePair2("DSN", dsn));
                        connInfo.LogonProperties.Add(new CrystalDecisions.Shared.NameValuePair2("UseDSNProperties", false));
                        connInfo.Type = CrystalDecisions.Shared.ConnectionInfoType.CRQE;

                        foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in rpt.Database.Tables)
                        {
                            if (crTable.LogOnInfo.ConnectionInfo.ServerName.Trim().Equals(rpt.DataSourceConnections[i].ServerName.Trim()))
                            {
                                CrystalDecisions.Shared.TableLogOnInfo myLogonInfo = crTable.LogOnInfo;
                                myLogonInfo.ConnectionInfo = connInfo;
                                crTable.ApplyLogOnInfo(myLogonInfo);
                            }
                        }

                        rpt.DataSourceConnections[i].SetLogon(As400_User, As400_Password);
                        break;
                    default:
                        //IsAS400 = false;
                        rpt.DataSourceConnections[i].SetConnection(MSSQL_ServerInstance, MSSQL_DB, false);
                        rpt.DataSourceConnections[i].SetLogon(MSSQL_User, MSSQL_Password);
                        rpt.DataSourceConnections[i].IntegratedSecurity = false;
                        break;
                }
            }

            //if (IsAS400)
            //{
            //    try
            //    {
            //        //rpt.SetDatabaseLogon(As400_User, As400_Password);
            //    }
            //    catch (Exception ex)
            //    {
            //        //throw ex;
            //    }
            //}

            return rpt;
        }

        private static void SetDBLogonForReport(CrystalDecisions.Shared.ConnectionInfo connectionInfo, CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument, string AS400_DB)
        {
            foreach (CrystalDecisions.CrystalReports.Engine.Table table in reportDocument.Database.Tables)
            {
                CrystalDecisions.Shared.TableLogOnInfo tableLogonInfo = table.LogOnInfo;
                tableLogonInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(tableLogonInfo);
                table.Location = AS400_DB + "." + table.Location;
            }

            CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinitions parameterList = reportDocument.DataDefinition.ParameterFields;
            for (int i = 0; i < parameterList.Count; i++)
            {
                // Here is how you can tell if this parameter is a linked parameter or not.
                if (parameterList[i].IsLinked())
                {
                    // do something with the linked parameter.
                }

                // This let's you check if a parameter already has a value or not.
                if (!parameterList[i].HasCurrentValue)
                {
                    // you can test the parameter 
                    if (parameterList[i].ValueType == CrystalDecisions.Shared.FieldValueType.DateTimeField || parameterList[i].ValueType == CrystalDecisions.Shared.FieldValueType.DateField)
                    {
                        // If the parameter is a range field, add a date time range.
                        CrystalDecisions.Shared.ParameterRangeValue rangeValue = new CrystalDecisions.Shared.ParameterRangeValue();
                        rangeValue.LowerBoundType = CrystalDecisions.Shared.RangeBoundType.BoundInclusive;
                        rangeValue.UpperBoundType = CrystalDecisions.Shared.RangeBoundType.BoundInclusive;
                        rangeValue.StartValue = DateTime.Parse("1/1/1899");
                        rangeValue.EndValue = DateTime.Parse("1/1/1899");
                        reportDocument.SetParameterValue(parameterList[i].ParameterFieldName, rangeValue);
                    }

                    // Again, DON'T access a paramater by it's name unless you are sure it is there because if
                    // this name does not exist in the list, it will throw an exception.  Just like the formulas 
                    // shown previously, it is better to test by name so we don't throw the exception.
                    string sParameterName = parameterList["MyParameterName"].Name;

                    // Test the parameter by field name and add a value if you want.
                    if (parameterList[i].ParameterFieldName == "MyParameterName")
                    {
                        // NOW, here is the deal.  If you want to set a parameter, practically EVERY example I found online, 
                        // says to do it this way:
                        CrystalDecisions.Shared.ParameterDiscreteValue discreteValue = new CrystalDecisions.Shared.ParameterDiscreteValue();
                        discreteValue.Value = "Some value you want to set here";
                        parameterList[i].CurrentValues.Add(discreteValue);
                        // But, this will NOT work.  This doesn't actually set the parameter's value.  It just adds a value to the CurrentValues list.
                        // If you try to run with this code, your report will fail to load with the exception "Missing Paramater Values".
                        // Instead, set the paramater's value this way.
                        reportDocument.SetParameterValue(parameterList[i].ParameterFieldName, "Some value you want to set here");
                    }
                    else
                    {
                        // If you're parameter doesn't have a value but does have a default value, here
                        // is an easy way to set the parameter's value to it's default value (Should do this automatically
                        // for you in my opinion but it doesn't unfortunately.)
                        if (parameterList[i].DefaultValues.Count > 0)
                        {
                            // just set parameter to it's default value
                            reportDocument.SetParameterValue(parameterList[i].ParameterFieldName, parameterList[i].DefaultValues[0]);
                        }
                    }
                }
            }
        }

        private static void SetDBLogonForSubreports(CrystalDecisions.Shared.ConnectionInfo connectionInfo, CrystalDecisions.CrystalReports.Engine.ReportDocument reportDocument, string AS400_DB)
        {
            foreach (CrystalDecisions.CrystalReports.Engine.Section section in reportDocument.ReportDefinition.Sections)
            {
                foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
                {
                    if (reportObject.Kind == CrystalDecisions.Shared.ReportObjectKind.SubreportObject)
                    {
                        CrystalDecisions.CrystalReports.Engine.SubreportObject subreportObject = (CrystalDecisions.CrystalReports.Engine.SubreportObject)reportObject;
                        CrystalDecisions.CrystalReports.Engine.ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        SetDBLogonForReport(connectionInfo, subReportDocument, AS400_DB);
                    }
                }
            }
        }

        public static void CrystalReportLogOn(CrystalDecisions.CrystalReports.Engine.ReportDocument reportParameters,
                                          string serverName,
                                          string databaseName,
                                          string userName,
                                          string password)
        {
            CrystalDecisions.Shared.TableLogOnInfo logOnInfo;
            CrystalDecisions.CrystalReports.Engine.ReportDocument subRd;
            CrystalDecisions.CrystalReports.Engine.Sections sects;
            CrystalDecisions.CrystalReports.Engine.ReportObjects ros;
            CrystalDecisions.CrystalReports.Engine.SubreportObject sro;

            if (reportParameters == null)
            {
                throw new ArgumentNullException("reportParameters");
            }

            try
            {
                foreach (CrystalDecisions.CrystalReports.Engine.Table t in reportParameters.Database.Tables)
                {
                    logOnInfo = t.LogOnInfo;
                    logOnInfo.ReportName = reportParameters.Name;
                    logOnInfo.ConnectionInfo.ServerName = serverName;
                    logOnInfo.ConnectionInfo.DatabaseName = databaseName;
                    logOnInfo.ConnectionInfo.UserID = userName;
                    logOnInfo.ConnectionInfo.Password = password;
                    logOnInfo.TableName = t.Name;
                    t.ApplyLogOnInfo(logOnInfo);
                    t.Location = t.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            sects = reportParameters.ReportDefinition.Sections;
            foreach (CrystalDecisions.CrystalReports.Engine.Section sect in sects)
            {
                ros = sect.ReportObjects;
                foreach (CrystalDecisions.CrystalReports.Engine.ReportObject ro in ros)
                {
                    if (ro.Kind == CrystalDecisions.Shared.ReportObjectKind.SubreportObject)
                    {
                        sro = (CrystalDecisions.CrystalReports.Engine.SubreportObject)ro;
                        subRd = sro.OpenSubreport(sro.SubreportName);
                        try
                        {
                            foreach (CrystalDecisions.CrystalReports.Engine.Table t in subRd.Database.Tables)
                            {
                                logOnInfo = t.LogOnInfo;
                                logOnInfo.ReportName = reportParameters.Name;
                                logOnInfo.ConnectionInfo.ServerName = serverName;
                                logOnInfo.ConnectionInfo.DatabaseName = databaseName;
                                logOnInfo.ConnectionInfo.UserID = userName;
                                logOnInfo.ConnectionInfo.Password = password;
                                logOnInfo.TableName = t.Name;
                                t.ApplyLogOnInfo(logOnInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
        }



        #endregion "Crystal Reports"

       

        #region "Procedures"

        public static void ErrorLogTextFile(String Msg)
        {
            string filename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6).Trim() + "\\Log\\";
            if (!System.IO.Directory.Exists(filename))
                System.IO.Directory.CreateDirectory(filename);

            filename = filename + "Log_" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + ".txt";
            string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> " + Msg;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, true);
            sw.WriteLine(sLogFormat);
            sw.Flush();
            sw.Close();
        }

   

 

     

        public static void ShutDownComputer()
        {
            System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
        }

        public static void RestartComputer()
        {
            System.Diagnostics.Process.Start("Shutdown", "-r -t 00");
        }

        public static void LogOffComputer()
        {
            System.Diagnostics.Process.Start("Shutdown", "-r -l 00");
        }

        public static void KillService(string service)
        {
            string[] s = service.Split(';');
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processes)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (process.ProcessName.ToLower().Equals(s[i].ToLower()))
                    {
                        process.Kill();
                        break;
                    }
                }
            }
        }

        #endregion "Procedures"

    

        #region "EnCryptDecrypt"

        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string securitycode, string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            string key = securitycode;

            // Get the key from config file
            //if(securitycode.Length<=0)
            //    key = (string)settingsReader.GetValue("AlexAustinAngelAubreyYap$", typeof(String));
            //else
            //    key = (string)settingsReader.GetValue(securitycode.Trim(), typeof(String));

            if (securitycode.Trim().Length <= 0)
                key = "AlexAustinAngelAubreyYap$";
            else
                key = securitycode;


            //System.Windows.Forms.MessageBox.Show(key);
            //if (useHashing)
            //{
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            //}
            //else
            //    keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string securitycode, string cipherString)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            string key = securitycode;

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            //if (securitycode.Length <= 0)
            //    key = (string)settingsReader.GetValue("AlexAustinAngelAubreyYap$", typeof(String));
            //else
            //    key = (string)settingsReader.GetValue(securitycode.Trim(), typeof(String));

            if (securitycode.Trim().Length <= 0)
                key = "AlexAustinAngelAubreyYap$";
            else
                key = securitycode;

            //if (useHashing)
            //{
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            //}
            //else
            //    keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }



        #endregion "EnCryptDecrypt"

        #region "Convert TIFF to PDF"


        public static void CreatePDF(DataTable dt, string PDF_filename)
        {

            CrystalDecisions.CrystalReports.Engine.ReportDocument RptName = null;
            RptName = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            string rptfilename = System.Web.HttpContext.Current.Server.MapPath("~\\Reports\\Others\\rpt_Image_obj.rpt");

            RptName.Load(rptfilename);
            RptName.SetDataSource(dt);

            RptName.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, PDF_filename);
            RptName.Close();
            RptName.Clone();
        }

        #endregion "Convert TIFF to PDF"
        
            //public const string _UAccount = "UAccount";
            //public static tbl_ma_user UAccount
            //{
            //    get
            //    {
            //        tbl_ma_user retval = Read<tbl_ma_user>(_UAccount);

            //        if (retval != null)
            //            return retval;
            //        else
            //            return retval = new tbl_ma_user();
            //    }
            //    set
            //    {
            //        Write(_UAccount, value);
            //    }
            //}


          


     

    }
}