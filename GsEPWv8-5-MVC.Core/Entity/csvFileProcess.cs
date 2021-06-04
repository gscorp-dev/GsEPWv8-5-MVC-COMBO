using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using GsEPWv8_5_MVC.Core.Entity;

namespace GsEPWv8_5_MVC.Core.Entity
{
    public class csvFileProcess
    {
        public enum sbsFileOrder
        {
            po_num,
            release_no,
            po_dt,
            dept_no,
            retailer_po_num,
            req_delivery_dt,
            ship_no_later,
            req_shipment_dt,
            carrier,
            carrier_desc,
            ship_to_loc,
            po_line_num,
            ordr_qty,
            uom,
            unit_price,
            byer_catalog_or_sku,
            upc_ean,
            itm_num,
            retail_price,
            itm_name,
            itm_color,
            itm_size,
            pack_size,
            pack_size_uom,
            no_of_inner_packs,
            pcs_per_inner_packs,
            store_id,
            qty_per_store,
            rec_type,
            po_purpose,
            po_type,
            contact_no,
            ccy,
            ship_status,
            ltr_of_cr,
            vend_id,
            div_id,
            cust_acc,
            cust_ordr_num,
            promo_num,
            tkt_desc,
            other_info,
            frt_terms,
            carier_ser_level,
            pmt_terms_percent,
            pmt_terms_due_date,
            pmt_terms_days_due,
            pmt_terms_net_due_date,
            pmt_terms_net_days_due,
            pmt_terms_desc_amt,
            pmt_term_desc,
            phone_no,
            fax_no,
            email_id,
            chrge_type,
            chrge_srvc,
            chrge_amt,
            chrge_percent,
            chrge_rate,
            chrge_qty,
            chrge_desc,
            ship_to_name,
            ship_to_addr1,
            ship_to_addr2,
            ship_to_city,
            ship_to_state,
            ship_to_zip,
            ship_to_cntry,
            ship_to_contact,
            bill_to_name,
            bill_to_addr1,
            bill_to_addr2,
            bill_to_city,
            bill_to_state,
            bill_to_zip,
            bill_to_cntry,
            bill_to_contact,
            buyer_name,
            buyer_loc,
            buyer_addr1,
            buyer_addr2,
            buyer_city,
            buyer_state,
            buyer_zip,
            buyer_cntry,
            buyer_contact,
            ultimate_loc,
            comments,
            ship_to_additional_name1,
            ship_to_additional_name2,
            bill_to_additional_name1,
            bill_to_additional_name2,
            buyer_additional_name1,
            buyer_additional_name2,
            notes1,
            notes2,
            sku_num,
            buyer_catlog_num

        };
        public cls_ob_940_sbs processCSVFile(string pstrCmpId, string pstrRefNum, string pstrFilePath)
        {
            string l_str_hdr_note = string.Empty;
            List<string> lst_csv_data = new List<string>();
            cls_ob_940_sbs objOB940SBS = new cls_ob_940_sbs();

            List<cls_ob_940_sbs_hdr> lstOB940SBSHdr;
            lstOB940SBSHdr = new List<cls_ob_940_sbs_hdr>();

            List<cls_ob_940_sbs_dtl> lstOB940SBSDtl;
            lstOB940SBSDtl = new List<cls_ob_940_sbs_dtl>();
            List<cls_ob_940_sbs_notes> lstOB940SBSNotes;
            lstOB940SBSNotes = new List<cls_ob_940_sbs_notes>();
            int l_int_line = 0;
            using (var file_reader = new CsvFileReader(pstrFilePath))
            {
                while (file_reader.ReadRow(lst_csv_data))
                {
                    l_int_line += 1;
                    if (lst_csv_data[((int)sbsFileOrder.rec_type)].ToUpper().Equals("H"))
                    {
                        cls_ob_940_sbs_hdr objOB940SBSHdr = new cls_ob_940_sbs_hdr();
                        objOB940SBSHdr.cmp_id = pstrCmpId;
                        objOB940SBSHdr.ref_num = string.Empty;
                        objOB940SBSHdr.po_num = lst_csv_data[(int)sbsFileOrder.po_num];
                        objOB940SBSHdr.release_no = lst_csv_data[(int)sbsFileOrder.release_no];
                        objOB940SBSHdr.po_dt = lst_csv_data[(int)sbsFileOrder.po_dt];
                        objOB940SBSHdr.dept_no = lst_csv_data[(int)sbsFileOrder.dept_no];
                        objOB940SBSHdr.retailer_po_num = lst_csv_data[(int)sbsFileOrder.retailer_po_num];
                        objOB940SBSHdr.req_delivery_dt = lst_csv_data[(int)sbsFileOrder.req_delivery_dt];
                        objOB940SBSHdr.ship_no_later = lst_csv_data[(int)sbsFileOrder.ship_no_later];
                        objOB940SBSHdr.req_shipment_dt = lst_csv_data[(int)sbsFileOrder.req_shipment_dt];
                        objOB940SBSHdr.carrier = lst_csv_data[(int)sbsFileOrder.carrier];
                        objOB940SBSHdr.carrier_desc = lst_csv_data[(int)sbsFileOrder.carrier_desc];
                        objOB940SBSHdr.ship_to_loc = lst_csv_data[(int)sbsFileOrder.ship_to_loc];
                      
                        objOB940SBSHdr.rec_type = lst_csv_data[(int)sbsFileOrder.rec_type];
                        objOB940SBSHdr.po_purpose = lst_csv_data[(int)sbsFileOrder.po_purpose];
                        objOB940SBSHdr.po_type = lst_csv_data[(int)sbsFileOrder.po_type];
                        objOB940SBSHdr.contact_no = lst_csv_data[(int)sbsFileOrder.contact_no];
                        objOB940SBSHdr.ccy = lst_csv_data[(int)sbsFileOrder.ccy];
                        objOB940SBSHdr.ship_status = lst_csv_data[(int)sbsFileOrder.ship_status];
                        objOB940SBSHdr.ltr_of_cr = lst_csv_data[(int)sbsFileOrder.ltr_of_cr];
                        objOB940SBSHdr.vend_id = lst_csv_data[(int)sbsFileOrder.vend_id];
                        objOB940SBSHdr.div_id = lst_csv_data[(int)sbsFileOrder.div_id];
                        objOB940SBSHdr.cust_acc = lst_csv_data[(int)sbsFileOrder.cust_acc];
                        objOB940SBSHdr.cust_ordr_num = lst_csv_data[(int)sbsFileOrder.cust_ordr_num];
                        objOB940SBSHdr.promo_num = lst_csv_data[(int)sbsFileOrder.promo_num];
                        objOB940SBSHdr.tkt_desc = lst_csv_data[(int)sbsFileOrder.tkt_desc];
                        objOB940SBSHdr.other_info = lst_csv_data[(int)sbsFileOrder.other_info];
                        objOB940SBSHdr.frt_terms = lst_csv_data[(int)sbsFileOrder.frt_terms];
                        objOB940SBSHdr.carier_ser_level = lst_csv_data[(int)sbsFileOrder.carier_ser_level];
                        objOB940SBSHdr.pmt_terms_percent = lst_csv_data[(int)sbsFileOrder.pmt_terms_percent];
                        objOB940SBSHdr.pmt_terms_due_date = lst_csv_data[(int)sbsFileOrder.pmt_terms_due_date];
                        objOB940SBSHdr.pmt_terms_days_due = lst_csv_data[(int)sbsFileOrder.pmt_terms_days_due];
                        objOB940SBSHdr.pmt_terms_net_due_date = lst_csv_data[(int)sbsFileOrder.pmt_terms_net_due_date];
                        objOB940SBSHdr.pmt_terms_net_days_due = lst_csv_data[(int)sbsFileOrder.pmt_terms_net_days_due];
                        objOB940SBSHdr.pmt_terms_desc_amt = lst_csv_data[(int)sbsFileOrder.pmt_terms_desc_amt];
                        objOB940SBSHdr.pmt_term_desc = lst_csv_data[(int)sbsFileOrder.pmt_term_desc];
                        objOB940SBSHdr.phone_no = lst_csv_data[(int)sbsFileOrder.phone_no];
                        objOB940SBSHdr.fax_no = lst_csv_data[(int)sbsFileOrder.fax_no];
                        objOB940SBSHdr.email_id = lst_csv_data[(int)sbsFileOrder.email_id];
                        objOB940SBSHdr.chrge_type = lst_csv_data[(int)sbsFileOrder.chrge_type];
                        objOB940SBSHdr.chrge_srvc = lst_csv_data[(int)sbsFileOrder.chrge_srvc];
                        objOB940SBSHdr.chrge_amt = lst_csv_data[(int)sbsFileOrder.chrge_amt];
                        objOB940SBSHdr.chrge_percent = lst_csv_data[(int)sbsFileOrder.chrge_percent];
                        objOB940SBSHdr.chrge_rate = lst_csv_data[(int)sbsFileOrder.chrge_rate];
                        objOB940SBSHdr.chrge_qty = lst_csv_data[(int)sbsFileOrder.chrge_qty];
                        objOB940SBSHdr.chrge_desc = lst_csv_data[(int)sbsFileOrder.chrge_desc];
                        objOB940SBSHdr.ship_to_name = lst_csv_data[(int)sbsFileOrder.ship_to_name];
                        objOB940SBSHdr.ship_to_addr1 = lst_csv_data[(int)sbsFileOrder.ship_to_addr1];
                        objOB940SBSHdr.ship_to_addr2 = lst_csv_data[(int)sbsFileOrder.ship_to_addr2];
                        objOB940SBSHdr.ship_to_city = lst_csv_data[(int)sbsFileOrder.ship_to_city];
                        objOB940SBSHdr.ship_to_state = lst_csv_data[(int)sbsFileOrder.ship_to_state];
                        objOB940SBSHdr.ship_to_zip = lst_csv_data[(int)sbsFileOrder.ship_to_zip];
                        objOB940SBSHdr.ship_to_cntry = lst_csv_data[(int)sbsFileOrder.ship_to_cntry];
                        objOB940SBSHdr.ship_to_contact = lst_csv_data[(int)sbsFileOrder.ship_to_contact];
                        objOB940SBSHdr.bill_to_name = lst_csv_data[(int)sbsFileOrder.bill_to_name];
                        objOB940SBSHdr.bill_to_addr1 = lst_csv_data[(int)sbsFileOrder.bill_to_addr1];
                        objOB940SBSHdr.bill_to_addr2 = lst_csv_data[(int)sbsFileOrder.bill_to_addr2];
                        objOB940SBSHdr.bill_to_city = lst_csv_data[(int)sbsFileOrder.bill_to_city];
                        objOB940SBSHdr.bill_to_state = lst_csv_data[(int)sbsFileOrder.bill_to_state];
                        objOB940SBSHdr.bill_to_zip = lst_csv_data[(int)sbsFileOrder.bill_to_zip];
                        objOB940SBSHdr.bill_to_cntry = lst_csv_data[(int)sbsFileOrder.bill_to_cntry];
                        objOB940SBSHdr.bill_to_contact = lst_csv_data[(int)sbsFileOrder.bill_to_contact];
                        objOB940SBSHdr.buyer_name = lst_csv_data[(int)sbsFileOrder.buyer_name];
                        objOB940SBSHdr.buyer_loc = lst_csv_data[(int)sbsFileOrder.buyer_loc];
                        objOB940SBSHdr.buyer_addr1 = lst_csv_data[(int)sbsFileOrder.buyer_addr1];
                        objOB940SBSHdr.buyer_addr2 = lst_csv_data[(int)sbsFileOrder.buyer_addr2];
                        objOB940SBSHdr.buyer_city = lst_csv_data[(int)sbsFileOrder.buyer_city];
                        objOB940SBSHdr.buyer_state = lst_csv_data[(int)sbsFileOrder.buyer_state];
                        objOB940SBSHdr.buyer_zip = lst_csv_data[(int)sbsFileOrder.buyer_zip];
                        objOB940SBSHdr.buyer_cntry = lst_csv_data[(int)sbsFileOrder.buyer_cntry];
                        objOB940SBSHdr.buyer_contact = lst_csv_data[(int)sbsFileOrder.buyer_contact];
                        objOB940SBSHdr.ultimate_loc = lst_csv_data[(int)sbsFileOrder.ultimate_loc];
                         objOB940SBSHdr.ship_to_additional_name1 = lst_csv_data[(int)sbsFileOrder.ship_to_additional_name1];
                        objOB940SBSHdr.ship_to_additional_name2 = lst_csv_data[(int)sbsFileOrder.ship_to_additional_name2];
                        objOB940SBSHdr.bill_to_additional_name1 = lst_csv_data[(int)sbsFileOrder.bill_to_additional_name1];
                        objOB940SBSHdr.bill_to_additional_name2 = lst_csv_data[(int)sbsFileOrder.bill_to_additional_name2];
                        objOB940SBSHdr.buyer_additional_name1 = lst_csv_data[(int)sbsFileOrder.buyer_additional_name1];
                        objOB940SBSHdr.buyer_additional_name2 = lst_csv_data[(int)sbsFileOrder.buyer_additional_name2];
                        objOB940SBSHdr.notes1 = lst_csv_data[(int)sbsFileOrder.notes1];
                        objOB940SBSHdr.notes2 = lst_csv_data[(int)sbsFileOrder.notes2];
                       
                        objOB940SBSHdr.line_num = l_int_line;

                        lstOB940SBSHdr.Add(objOB940SBSHdr);
                    }
                    else if (lst_csv_data[((int)sbsFileOrder.rec_type)].ToUpper().Equals("D"))
                    {
                        cls_ob_940_sbs_dtl objOB940SBSDtl = new cls_ob_940_sbs_dtl();
                        objOB940SBSDtl.cmp_id = pstrCmpId;
                        objOB940SBSDtl.ref_num = string.Empty;
                        objOB940SBSDtl.po_num = lst_csv_data[(int)sbsFileOrder.po_num];
                        objOB940SBSDtl.po_line_num = Convert.ToInt16(lst_csv_data[(int)sbsFileOrder.po_line_num]);
                        objOB940SBSDtl.ordr_qty = Convert.ToInt16((lst_csv_data[(int)sbsFileOrder.ordr_qty]).Replace(".0", ""));
                        objOB940SBSDtl.uom = lst_csv_data[(int)sbsFileOrder.uom];
                        objOB940SBSDtl.unit_price = lst_csv_data[(int)sbsFileOrder.unit_price];
                        objOB940SBSDtl.byer_catalog_or_sku = lst_csv_data[(int)sbsFileOrder.byer_catalog_or_sku];
                        objOB940SBSDtl.upc_ean = lst_csv_data[(int)sbsFileOrder.upc_ean];
                        objOB940SBSDtl.itm_num = lst_csv_data[(int)sbsFileOrder.itm_num];
                        objOB940SBSDtl.retail_price = lst_csv_data[(int)sbsFileOrder.retail_price];
                        objOB940SBSDtl.itm_name = lst_csv_data[(int)sbsFileOrder.itm_name];
                        objOB940SBSDtl.itm_color = lst_csv_data[(int)sbsFileOrder.itm_color];
                        objOB940SBSDtl.itm_size = lst_csv_data[(int)sbsFileOrder.itm_size];
                        objOB940SBSDtl.pack_size = lst_csv_data[(int)sbsFileOrder.pack_size];
                        objOB940SBSDtl.pack_size_uom = lst_csv_data[(int)sbsFileOrder.pack_size_uom];
                        objOB940SBSDtl.no_of_inner_packs = lst_csv_data[(int)sbsFileOrder.no_of_inner_packs];
                        objOB940SBSDtl.pcs_per_inner_packs = lst_csv_data[(int)sbsFileOrder.pcs_per_inner_packs];
                        objOB940SBSDtl.store_id = lst_csv_data[(int)sbsFileOrder.store_id];
                        objOB940SBSDtl.qty_per_store = lst_csv_data[(int)sbsFileOrder.qty_per_store];
                        objOB940SBSDtl.sku_num = lst_csv_data[(int)sbsFileOrder.sku_num];
                        objOB940SBSDtl.buyer_catlog_num = lst_csv_data[(int)sbsFileOrder.buyer_catlog_num];

                        objOB940SBSDtl.line_num = l_int_line;
                        lstOB940SBSDtl.Add(objOB940SBSDtl);

                    }
                    else if (lst_csv_data[((int)sbsFileOrder.rec_type)].ToUpper().Equals("N"))
                    {
                        cls_ob_940_sbs_notes objOB940SBSNotes = new cls_ob_940_sbs_notes();
                        objOB940SBSNotes.cmp_id = pstrCmpId;
                        objOB940SBSNotes.ref_num = string.Empty;
                        objOB940SBSNotes.po_num = lst_csv_data[(int)sbsFileOrder.po_num];
                        objOB940SBSNotes.comments = lst_csv_data[(int)sbsFileOrder.comments];
                        objOB940SBSNotes.line_num = l_int_line;
                        lstOB940SBSNotes.Add(objOB940SBSNotes);
                    }
                }
                objOB940SBS.lstOB940SBSHdr = lstOB940SBSHdr;
                objOB940SBS.lstOB940SBSDtl = lstOB940SBSDtl;
                objOB940SBS.lstOB940SBSNotes = lstOB940SBSNotes;

                return objOB940SBS;
            }


        }


        public enum EmptyLineBehavior
        {
            /// <summary>
            /// Empty lines are interpreted as a line with zero columns.
            /// </summary>
            NoColumns,
            /// <summary>
            /// Empty lines are interpreted as a line with a single empty column.
            /// </summary>
            EmptyColumn,
            /// <summary>
            /// Empty lines are skipped over as though they did not exist.
            /// </summary>
            Ignore,
            /// <summary>
            /// An empty line is interpreted as the end of the input file.
            /// </summary>
            EndOfFile,
        }
        /// <summary>
        /// Common base class for CSV reader and writer classes.
        /// </summary>
        public abstract class CsvFileCommon
        {
            /// <summary>
            /// These are special characters in CSV files. If a column contains any
            /// of these characters, the entire column is wrapped in double quotes.
            /// </summary>
            protected char[] SpecialChars = new char[] { ',', '"', '\r', '\n' };

            // Indexes into SpecialChars for characters with specific meaning
            private const int DelimiterIndex = 0;
            private const int QuoteIndex = 1;

            /// <summary>
            /// Gets/sets the character used for column delimiters.
            /// </summary>
            public char Delimiter
            {
                get { return SpecialChars[DelimiterIndex]; }
                set { SpecialChars[DelimiterIndex] = value; }
            }

            /// <summary>
            /// Gets/sets the character used for column quotes.
            /// </summary>
            public char Quote
            {
                get { return SpecialChars[QuoteIndex]; }
                set { SpecialChars[QuoteIndex] = value; }
            }
        }

        /// <summary>
        /// Class for reading from comma-separated-value (CSV) files
        /// </summary>
        public class CsvFileReader : CsvFileCommon, IDisposable
        {
            // Private members
            private StreamReader Reader;
            private string CurrLine;
            private int CurrPos;
            private EmptyLineBehavior EmptyLineBehavior;

            /// <summary>
            /// Initializes a new instance of the CsvFileReader class for the
            /// specified stream.
            /// </summary>
            /// <param name="stream">The stream to read from</param>
            /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
            public CsvFileReader(Stream stream,
                EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
            {
                Reader = new StreamReader(stream);
                EmptyLineBehavior = emptyLineBehavior;
            }

            /// <summary>
            /// Initializes a new instance of the CsvFileReader class for the
            /// specified file path.
            /// </summary>
            /// <param name="path">The name of the CSV file to read from</param>
            /// <param name="emptyLineBehavior">Determines how empty lines are handled</param>
            public CsvFileReader(string path,
                EmptyLineBehavior emptyLineBehavior = EmptyLineBehavior.NoColumns)
            {
                Reader = new StreamReader(path);
                EmptyLineBehavior = emptyLineBehavior;
            }

            /// <summary>
            /// Reads a row of columns from the current CSV file. Returns false if no
            /// more data could be read because the end of the file was reached.
            /// </summary>
            /// <param name="columns">Collection to hold the columns read</param>
            public bool ReadRow(List<string> columns)
            {
                // Verify required argument
                if (columns == null)
                    throw new ArgumentNullException("columns");

                ReadNextLine:
                // Read next line from the file
                CurrLine = Reader.ReadLine();
                CurrPos = 0;
                // Test for end of file
                if (CurrLine == null)
                    return false;
                // Test for empty line
                if (CurrLine.Length == 0)
                {
                    switch (EmptyLineBehavior)
                    {
                        case EmptyLineBehavior.NoColumns:
                            columns.Clear();
                            return true;
                        case EmptyLineBehavior.Ignore:
                            goto ReadNextLine;
                        case EmptyLineBehavior.EndOfFile:
                            return false;
                    }
                }

                // Parse line
                string column;
                int numColumns = 0;
                while (true)
                {
                    // Read next column
                    if (CurrPos < CurrLine.Length && CurrLine[CurrPos] == Quote)
                        column = ReadQuotedColumn();
                    else
                        column = ReadUnquotedColumn();
                    // Add column to list
                    if (numColumns < columns.Count)
                        columns[numColumns] = column;
                    else
                        columns.Add(column);
                    numColumns++;
                    // Break if we reached the end of the line
                    if (CurrLine == null || CurrPos == CurrLine.Length)
                        break;
                    // Otherwise skip delimiter
                    Debug.Assert(CurrLine[CurrPos] == Delimiter);
                    CurrPos++;
                }
                // Remove any unused columns from collection
                if (numColumns < columns.Count)
                    columns.RemoveRange(numColumns, columns.Count - numColumns);
                // Indicate success
                return true;
            }

            /// <summary>
            /// Reads a quoted column by reading from the current line until a
            /// closing quote is found or the end of the file is reached. On return,
            /// the current position points to the delimiter or the end of the last
            /// line in the file. Note: CurrLine may be set to null on return.
            /// </summary>
            private string ReadQuotedColumn()
            {
                // Skip opening quote character
                Debug.Assert(CurrPos < CurrLine.Length && CurrLine[CurrPos] == Quote);
                CurrPos++;

                // Parse column
                StringBuilder builder = new StringBuilder();
                while (true)
                {
                    while (CurrPos == CurrLine.Length)
                    {
                        // End of line so attempt to read the next line
                        CurrLine = Reader.ReadLine();
                        CurrPos = 0;
                        // Done if we reached the end of the file
                        if (CurrLine == null)
                            return builder.ToString();
                        // Otherwise, treat as a multi-line field
                        builder.Append(Environment.NewLine);
                    }

                    // Test for quote character
                    if (CurrLine[CurrPos] == Quote)
                    {
                        // If two quotes, skip first and treat second as literal
                        int nextPos = (CurrPos + 1);
                        if (nextPos < CurrLine.Length && CurrLine[nextPos] == Quote)
                            CurrPos++;
                        else
                            break;  // Single quote ends quoted sequence
                    }
                    // Add current character to the column
                    builder.Append(CurrLine[CurrPos++]);
                }

                if (CurrPos < CurrLine.Length)
                {
                    // Consume closing quote
                    Debug.Assert(CurrLine[CurrPos] == Quote);
                    CurrPos++;
                    // Append any additional characters appearing before next delimiter
                    builder.Append(ReadUnquotedColumn());
                }
                // Return column value
                return builder.ToString();
            }

            /// <summary>
            /// Reads an unquoted column by reading from the current line until a
            /// delimiter is found or the end of the line is reached. On return, the
            /// current position points to the delimiter or the end of the current
            /// line.
            /// </summary>
            private string ReadUnquotedColumn()
            {
                int startPos = CurrPos;
                CurrPos = CurrLine.IndexOf(Delimiter, CurrPos);
                if (CurrPos == -1)
                    CurrPos = CurrLine.Length;
                if (CurrPos > startPos)
                    return CurrLine.Substring(startPos, CurrPos - startPos);
                return String.Empty;
            }

            // Propagate Dispose to StreamReader
            public void Dispose()
            {
                Reader.Dispose();
            }
        }

        /// <summary>
        /// Class for writing to comma-separated-value (CSV) files.
        /// </summary>
        public class CsvFileWriter : CsvFileCommon, IDisposable
        {
            // Private members
            private StreamWriter Writer;
            private string OneQuote = null;
            private string TwoQuotes = null;
            private string QuotedFormat = null;

            /// <summary>
            /// Initializes a new instance of the CsvFileWriter class for the
            /// specified stream.
            /// </summary>
            /// <param name="stream">The stream to write to</param>
            public CsvFileWriter(Stream stream)
            {
                Writer = new StreamWriter(stream);
            }

            /// <summary>
            /// Initializes a new instance of the CsvFileWriter class for the
            /// specified file path.
            /// </summary>
            /// <param name="path">The name of the CSV file to write to</param>
            public CsvFileWriter(string path)
            {
                Writer = new StreamWriter(path);
            }

            /// <summary>
            /// Writes a row of columns to the current CSV file.
            /// </summary>
            /// <param name="columns">The list of columns to write</param>
            public void WriteRow(List<string> columns)
            {
                // Verify required argument
                if (columns == null)
                    throw new ArgumentNullException("columns");

                // Ensure we're using current quote character
                if (OneQuote == null || OneQuote[0] != Quote)
                {
                    OneQuote = String.Format("{0}", Quote);
                    TwoQuotes = String.Format("{0}{0}", Quote);
                    QuotedFormat = String.Format("{0}{{0}}{0}", Quote);
                }

                // Write each column
                for (int i = 0; i < columns.Count; i++)
                {
                    // Add delimiter if this isn't the first column
                    if (i > 0)
                        Writer.Write(Delimiter);
                    // Write this column
                    if (columns[i].IndexOfAny(SpecialChars) == -1)
                        Writer.Write(columns[i]);
                    else
                        Writer.Write(QuotedFormat, columns[i].Replace(OneQuote, TwoQuotes));
                }
                Writer.WriteLine();
            }

            // Propagate Dispose to StreamWriter
            public void Dispose()
            {
                Writer.Dispose();
            }
        }
    }
}
