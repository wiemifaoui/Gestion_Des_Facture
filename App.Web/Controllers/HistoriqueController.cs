using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SelectPdf;
using PdfDocument = SelectPdf.PdfDocument;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using App.Domain;
using Newtonsoft.Json.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using App.Web.Models;
using Invoice = App.Web.Models.Invoice;

namespace App.Web.Controllers
{
  
    public class HistoriqueController : Controller

    {
        private readonly InvoiceContext _context;

        public HistoriqueController(InvoiceContext context)
        {
            _context = context;
        }

        [HttpGet]

        public ActionResult pdf()

        {
            try
            {
                // Example JSON
                string path = @"C:\Users\USER\Source\Repos\Extract\stage\App.Data\Invoice\invoice_.json";
                string readText = System.IO.File.ReadAllText(path);

                List<Invoice> lstInvoices = JsonConvert.DeserializeObject<List<Invoice>>(readText);

                return View(lstInvoices);
            }
            catch (Exception e) { throw e; }

        }

        // GET: HistoriqueController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IonosSubmitAsync(List<Invoice> lstInvoices)
        {
            Traitement tritement = new Traitement();
            try
            {
                tritement.NbrFactures = lstInvoices.Count.ToString();
                tritement.DateTraitement = DateTime.Now.ToString();

                float amount = 0;

                foreach (var invoice in lstInvoices)
                {
                    amount += float.Parse(invoice.InvoiceAmount);
                }

                tritement.TotalAmount = amount.ToString();

                _context.Traitements.Add(tritement);


                _context.SaveChanges();

                long id = tritement.Id;

                Traitement traitement =  _context.Traitements.Find(id);

                foreach (Invoice invoice in lstInvoices)
                {
                    invoice.IdTraitement = traitement.Id;

                    _context.Invoices.Add(invoice);
                    _context.SaveChanges();
                }
            }catch(Exception e) { throw e; }

            return Json("OK");
            //return View();
        }

        [HttpGet]
        public IActionResult Ionos()
        {
            List<Traitement> list = _context.Traitements.ToList();
            return View(list);
        }

        public IActionResult DownloadHistory(string Id)
        { 
            Traitement traitement = _context.Traitements.Find(long.Parse(Id));
            List<Invoice> list = new List<Invoice>();

            if (traitement != null)
            {
                list = _context.Invoices.ToList().FindAll(x => x.IdTraitement == traitement.Id);
            }
            try
            {
                Guid guid = new Guid();
                string PdfOutPut = "C:\\Users\\USER\\source\\repos\\Extract\\stage\\App.Data\\Invoice\\Invoice_" + guid.ToString() + ".pdf";

                Document document = new Document(PageSize.A4, 10, 10, 30, 30);
                FileStream fileStream = null;
                fileStream = new FileStream(PdfOutPut, FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(document, fileStream);
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fontOrder = new Font(bf, 12, Font.BOLD);
                var orderChunk = new Chunk();
                orderChunk.Font = fontOrder;
                document.Open();
                document.NewPage();
               

                PdfPTable table = new PdfPTable(3);

                PdfPCell cell = new PdfPCell(new Phrase("Date"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                table.AddCell("Invoice number");
                table.AddCell("Invoice amount");

                foreach (Invoice invoice in list)
                {
                    table.AddCell(invoice.InvoiceDate.ToString());
                    table.AddCell(invoice.InvoiceID);
                    table.AddCell(invoice.InvoiceAmount + " TND");
                }
                cell = new PdfPCell(new Phrase("Total : "));
                cell.Colspan = 2;
                table.AddCell(cell);

                table.AddCell(traitement.TotalAmount + " TND");
                document.Add(table);
                document.Close();   
                fileStream.Close();

                Document doc = new Document();
                string pdfOut2 = "C:\\Users\\USER\\source\\repos\\Extract\\stage\\App.Data\\Invoice\\Invoice_1_" + guid.ToString() + ".pdf";
                PdfCopy writer1 = new PdfCopy(doc, new FileStream(pdfOut2, FileMode.Create));
                doc.Open();
                if (writer1 == null)
                {
                    return null;
                }
                PdfReader reader = new PdfReader(PdfOutPut);
                reader.ConsolidateNamedDestinations();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer1.GetImportedPage(reader, i);
                    writer1.AddPage(page);
                }
                reader.Close();
                foreach (Invoice invoice in list)
                {
                    reader = new PdfReader("C:\\Users\\USER\\source\\repos\\Extract\\stage\\App.Web\\" + invoice.InvoiceFileName);
                    reader.ConsolidateNamedDestinations();
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer1.GetImportedPage(reader, i);
                        writer1.AddPage(page);
                    }
                    reader.Close();
                }
                writer1.Close();
                doc.Close();

                var content = new FileStream(pdfOut2, FileMode.Open, FileAccess.Read, FileShare.Read);
                var response = File(content, "application/pdf", "Invoice_" + guid.ToString() + ".pdf");
                return response;
            }catch(Exception e) { throw e; }
        }


        //public IActionResult pdf()
        //{

        //    var webClient = new WebClient();
        //    var json = webClient.DownloadString(@"C:\Users\HP\Documents\GitHub\Extract\App.Data\Invoice\invoice_637963472335662688.json");

        //    var invoices = JsonConvert.DeserializeObject<Invoices>(json);
        //    return View(invoices);

        //}

//utiliser pour transformer html en pdf

        [HttpPost]
        public FileStream GeneratePdf(string html,string lst)
        {
            try
            {
                List<Invoice> lstInvoices = JsonConvert.DeserializeObject<List<Invoice>>(lst);
                html = html.Replace("strtTag", "<").Replace("EndTag", ">");

                HtmlToPdf objhtml = new HtmlToPdf();
                PdfDocument objdoc = objhtml.ConvertHtmlString(html);
                byte[] pdf = objdoc.Save();
                objdoc.Close();

                using (FileStream fs = System.IO.File.Create("C:\\Users\\HP\\Documents\\GitHub\\stage\\App.Data\\Invoice\\Invoice_1.pdf"))
                {
                    fs.Write(pdf, 0, pdf.Length);
                }

                string PdfOutPut = "C:\\Users\\HP\\Documents\\GitHub\\stage\\App.Data\\Invoice\\Invoice_Result.pdf";
                Document doc = new Document();
                PdfCopy writer = new PdfCopy(doc, new FileStream(PdfOutPut, FileMode.Create));

                if (writer == null)
                {
                    return null;
                }

                doc.Open();

                PdfReader reader = new PdfReader("C:\\Users\\HP\\Documents\\GitHub\\stage\\App.Data\\Invoice\\Invoice_1.pdf");
                reader.ConsolidateNamedDestinations();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }
                reader.Close();

                foreach (Invoice invoice in lstInvoices)
                {
                    reader = new PdfReader("C:\\Users\\HP\\Documents\\GitHub\\stage\\App.Web\\"+invoice.InvoiceFileName);
                    reader.ConsolidateNamedDestinations();
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }
                    reader.Close();
                }
                writer.Close();
                doc.Close();
            }
            catch(Exception e)
            {
                throw e;
            }
            return null;
        }






    }

}
