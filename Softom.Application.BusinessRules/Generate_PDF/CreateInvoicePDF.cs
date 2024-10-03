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
    public class CreateInvoicePDF
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
                doc.Add(image);
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

                doc.Add(AddTotals(invoice, doc, writer));

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

            tableLayout.AddCell(new PdfPCell(new Phrase("Receipt Payment", new Font(Font.FontFamily.HELVETICA, 12, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });

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
            tableLayout.AddCell(new PdfPCell(new Phrase("Payment Receipt #00" + invoice.Payment.PaymentId, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 9, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
            tableLayout.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd MMMM yyyy"), new Font(Font.FontFamily.HELVETICA, 7, 1, iTextSharp.text.BaseColor.BLACK))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.AssociationName,new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Attention : " + invoice.Member.ContactInformation.Firstname + " " + invoice.Member.ContactInformation.Surname, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });
            
            //Add Title to the PDF file at the top   
            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.Address.AddressLine1, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Member.Address.AddressLine1, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.Address.AddressLine2, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Member.Address.AddressLine2, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.Address.AddressLine3, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Member.Address.AddressLine3, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Association.Address.Code.ToString(), new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(invoice.Member.Address.Code.ToString(), new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase("Phone Number: " + invoice.Association.PhoneNumber, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("Phone Number: " + invoice.Member.ContactInformation.PhoneNumber, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase("Email : " + invoice.Association.EmailAddress, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
            tableLayout.AddCell(new PdfPCell(new Phrase(string.Empty, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            tableLayout.AddCell(new PdfPCell(new Phrase("Website : " + invoice.Association.Website, new Font(Font.FontFamily.HELVETICA, 6, 0, iTextSharp.text.BaseColor.BLACK))) { Colspan = 1, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_LEFT });
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

            PdfPTable tableLayout = new PdfPTable(4);
            float[] headers = { 40, 20, 20, 20 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;     //Set the PDF File witdh percentage

            string Color = "#FFFFFF";
            //Add header
            AddCellToHeaderNew(tableLayout, "Description", Color);
            AddCellToHeaderNew(tableLayout, "Payment Status", Color);
            AddCellToHeaderNew(tableLayout, "Quantity", Color);
            AddCellToHeaderNew(tableLayout, "Amount", Color);

            AddCellToHeaderNew(tableLayout, invoice.Payment.PaymentType.Name, Color);
            AddCellToHeaderNew(tableLayout, invoice.Payment.PaymentStatus.Name, Color);
            AddCellToHeaderNew(tableLayout,"1".ToString(), Color);
            AddCellToHeaderNew(tableLayout, invoice.Payment.Amount.ToString("c", Softom.Application.BusinessRules.Configuration.CultureInfoSettings.GetZACulture().NumberFormat), Color);
            AddCellToHeaderAlignRight(tableLayout, invoice.Payment.Amount.ToString("c", Softom.Application.BusinessRules.Configuration.CultureInfoSettings.GetZACulture().NumberFormat));
                       
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
