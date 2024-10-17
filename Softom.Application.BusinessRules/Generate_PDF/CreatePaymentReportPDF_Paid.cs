using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.html;
using Softom.Application.Models.MV;

namespace Softom.Application.BusinessRules.Generate_PDF
{
    public class CreatePaymentReportPDF_Paid
    {
        public decimal AccTotal { get; set; }
        public decimal AccVat { get; set; }
        public decimal AccSubTotal { get; set; }

        public MemoryStream GeneratePDFFile(InvoiceVM invoice)
        {
            Document doc = new Document();
            MemoryStream workStream = new MemoryStream();

            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            PdfWriter writer = PdfWriter.GetInstance(doc, workStream);
            doc.Open();

            try
            {
                byte[] imageBytes = invoice.Association.Logo;
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                var scalePercent = (((doc.PageSize.Width / image.Width) * 100) - 425);
                image.ScalePercent(scalePercent);
                //doc.Add(image);
            }
            catch (Exception)
            {
                var logo = iTextSharp.text.Image.GetInstance(@"C:\\Temp\Soweto\unnamed.png");
                doc.Add(logo);
            }
            finally
            {
                doc.Add(HeaderInfo(doc, writer));

                doc.Add(Space(invoice, doc, writer));

                doc.Add(Header(invoice, doc, writer));

                doc.Add(Space(invoice, doc, writer));

                doc.Add(AddContent(invoice, doc, writer));

                doc.Add(Space(invoice, doc, writer));   

                doc.Add(Message(invoice, doc, writer));

                doc.Close();
            }

            return workStream;
        }

        private PdfPTable HeaderInfo(Document doc, PdfWriter writer)
        {
            PdfPTable tableLayout = new PdfPTable(4);
            float[] headers = { 25, 25, 25, 25 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;     //Set the PDF File witdh percentage           

            tableLayout.AddCell(new PdfPCell(new Phrase("Member Paid", new Font(Font.FontFamily.HELVETICA, 12, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });

            return tableLayout;
        }

        private PdfPTable Header(InvoiceVM invoice, Document doc, PdfWriter writer)
        {
            PdfPTable tableLayout = new PdfPTable(4);
            float[] headers = { 25, 25, 25, 25 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;     //Set the PDF File witdh percentage

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.AssociationName + " - " + invoice.ReportDate, new Font(Font.FontFamily.HELVETICA, 10, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            //Add Title to the PDF file at the top   
            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.Address.AddressLine1, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.Address.AddressLine2, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.Address.AddressLine3, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.Address.Code.ToString(), new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase("Phone Number: " + invoice.Association.PhoneNumber, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase("Total Amount : " + invoice.PaymentList.Sum(f=>f.Amount).ToString("c", Softom.Application.BusinessRules.Configuration.CultureInfoSettings.GetZACulture().NumberFormat), new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            
            return tableLayout;
        }
          
        private PdfPTable Space(InvoiceVM invoice, Document doc, PdfWriter writer)
        {
            PdfPTable tableLayout = new PdfPTable(1);
            float[] headers = { 100 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;     //Set the PDF File witdh percentage            

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });

            return tableLayout;
        }

        private PdfPTable AddContent(InvoiceVM invoice, Document doc, PdfWriter writer)
        {
            AccTotal = 0;
            AccVat = 0;
            AccSubTotal = 0;

            PdfPTable tableLayout = new PdfPTable(5);
            float[] headers = { 40, 20, 15, 10, 15};  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;     //Set the PDF File witdh percentage

            string Color = "#FFFFFF";
            //Add header
            AddCellToBody(tableLayout, "Member Name");
            AddCellToBody(tableLayout, "Member Surname");
            AddCellToBody(tableLayout, "Payment Type");
            AddCellToBody(tableLayout, "Amount");
            AddCellToBody(tableLayout, "Payment Date");

            foreach (var item in invoice.PaymentList.OrderBy(f=>f.PaymentDate))
            {
                AddCellToBody(tableLayout, item.Member.ContactInformation.Firstname);
                AddCellToBody(tableLayout, item.Member.ContactInformation.Surname);
                AddCellToBody(tableLayout, item.Notes);
                AddCellToBody(tableLayout, item.Amount.ToString("c", Softom.Application.BusinessRules.Configuration.CultureInfoSettings.GetZACulture().NumberFormat));
                AddCellToBody(tableLayout, item.PaymentDate.ToString("dd MMMM yyyy"));                
            }
            return tableLayout;
        }
        private PdfPTable AddTotals(InvoiceVM invoice, Document doc, PdfWriter writer)
        {
            PdfPTable tableLayout = new PdfPTable(3);
            float[] headers = { 50, 40, 10 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;     //Set the PDF File witdh percentage            

            var total = invoice.Payment.Amount;

            var VAT = Convert.ToDecimal(0);            
            VAT = (AccTotal * Convert.ToDecimal(1.15)) - AccTotal;

            var SubTotal = total + VAT;
            
            tableLayout.AddCell(new PdfPCell(new Phrase("Total : " + SubTotal.ToString("c", Softom.Application.BusinessRules.Configuration.CultureInfoSettings.GetZACulture().NumberFormat), new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 1, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });
            return tableLayout;
        }

        private PdfPTable Message(InvoiceVM invoice, Document doc, PdfWriter writer)
        {
            PdfPTable tableLayout = new PdfPTable(4);
            float[] headers = { 25, 25, 25, 25 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;     //Set the PDF File witdh percentage           

            tableLayout.AddCell(new PdfPCell(new Phrase("Thanks you, for the payment!!!", new Font(Font.FontFamily.HELVETICA, 12, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });

            return tableLayout;
        }

        private void AddCellToHeaderNew(PdfPTable tableLayout, string cellText, string Color)
        {
            BaseColor myColorpan = WebColors.GetRGBColor(Color);
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 0, Border = 1, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
            //tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 7, 0, iTextSharp.text.BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = myColorpan });
        }

        private void AddCellToHeader(PdfPTable tableLayout, string cellText, string Color)
        {
            BaseColor myColorpan = WebColors.GetRGBColor(Color);
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 7, 0, iTextSharp.text.BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = myColorpan });
        }

        private void AddCellToHeaderAlignRight(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 7, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
        }

        private void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 7, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE });
        }

        private void AddNoCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 7, 0, iTextSharp.text.BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = iTextSharp.text.BaseColor.WHITE, Border = 0 });
        }
    }
}
