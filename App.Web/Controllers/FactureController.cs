using App.Domain;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


using Path = System.IO.Path;
using System.Net.NetworkInformation;

namespace App.Web.Controllers
{
    public class FactureController : Controller
    {

        // GET: TraitementController1
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Traitement()
        {
            return View();
        }

        public async Task<string> GetExchangeRate(string from, string to, string invoiceDate)
        {
            invoiceDate = "2020-04-04";
            //Examples:
            //from = "EUR"
            //to = "USD"
            using (var client = new HttpClient())
            {

                try
                {
                    client.BaseAddress = new Uri("https://api.exchangerate.host");
                    var response = await client.GetAsync($"/{invoiceDate}?source=ecb&from={from}");
                    var stringResult = await response.Content.ReadAsStringAsync();

                    var dictResult = JsonConvert.DeserializeObject(stringResult);
                    return "ok";
                    dynamic data = JObject.Parse(stringResult);
                    //data = {"EUR_USD":{"val":1.140661}}
                    //I want to return 1.140661
                    //EUR_USD is dynamic depending on what from/to is
                    return data.val;
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    return "Error calling API. Please do manual lookup.";
                }
            }
        }


        //upload pdf avec l'extraction du donnees
        [HttpPost("Index")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();

            //declarer une liste invoice.

            List<Invoice> lstInvoices = new List<Invoice>(); //lstInvoices c'est le nom de la liste
            string filePathhh = @"C:\Users\USER\Source\Repos\Extract\stage\App.Data\Invoice\invoice_.json";
          
                try
            {
                using (StreamWriter writer = new StreamWriter(filePathhh))
                {
                    string json = "";
                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            // full path to file in temp location
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), formFile.FileName); //Add your own file path.
                            filePaths.Add(filePath);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                            //extraction des données 

                            #region ExtractData

                            StringBuilder sb = new StringBuilder();

                            Invoice invoice = new Invoice();

                            //string file = @"F:\SELFDEV\projet-.net-ateliergit1\Invoice_2022-06-11_202038773912_V87919141.pdf";
                            using (PdfReader reader = new PdfReader(formFile.FileName))
                            {
                                for (int pageNo = 1; pageNo <= reader.NumberOfPages; pageNo++)
                                {
                                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                                    string text = PdfTextExtractor.GetTextFromPage(reader, pageNo, strategy);

                                    text = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
                                    sb.Append(text);
                                }
                            }

                            // Invoice invoice = new Invoice();


                            //Extraction de donnees de la facture ionos

                            //Invoice invoice = new Invoice();


                            string[,] table = new string[,] { { "Contract ID:", ": " },
                        { "Customer ID:", ": " }, { "Invoice:", ": " }, { "Invoice Date:", ": " } };

                            for (int i = 0; i < (table.Length / 2); i++)
                            {
                                int startt = sb.ToString().IndexOf(table[i, 0]);
                                //System.Diagnostics.Debug.WriteLine()
                                if (startt >= 0)
                                {
                                    //lire toute la ligne.

                                    int end = sb.ToString().IndexOf("\n", startt);
                                    string line = sb.ToString().Substring(startt, end - startt);
                                    Console.WriteLine(line);
                                    string token = line.Split(table[i, 1])[1];
                                    Console.WriteLine(token);

                                    //Conversion des donnees dans un fichier json


                                    switch (table[i, 0]) //selon table[i, 0]
                                    {
                                        case "Contract ID:": invoice.ContractID = token; break;
                                        case "Invoice:": invoice.InvoiceID = token; break;
                                        case "Invoice Date:": invoice.InvoiceDate = token; break;
                                        case "Customer ID:": invoice.CustomerID = token; break;
                                    }



                                    //Console.WriteLine(sb.ToString);
                                    //Console.ReadKey();

                                }
                            }


                            //string filePathhh = @"C:\Utilisateurs\Public\invoice_" + DateTime.Now.Ticks + ".json";


                            /*  var filePathh = Path.GetTempFileName();
              using (var writer = System.IO.File.CreateText(filePathh)) // or File.AppendText
              {
                  writer.WriteLine("json"); //or .Write(), if you wish  
              }


                           string filePathhh = @"C:\Utilisateurs\Public\invoice_" + DateTime.Now.Ticks + ".json";

                                  System.IO.File.WriteAllText(filePathhh, json);

                                   */
                            // Extraction du montant en $

                            int dollars = 0;
                            int start = 0;

                            while (dollars < sb.ToString().Length)
                            {
                                start = sb.ToString().IndexOf("$", start + 1);
                                if (start > 0)
                                    dollars = start;
                                else
                                    break;
                            }

                            //Console.WriteLine(dollars);
                            int end_dollars = sb.ToString().IndexOf("\n", dollars);
                            string amountt = sb.ToString().Substring(dollars, end_dollars - dollars);
                            //Console.WriteLine(amount);
                            //string str = amountt.Substring(1, amountt.Length - 1);

                            amountt = amountt.Replace("$", ""); /* renplacer le caractere $ par le vide*/
                            amountt = amountt.Replace(".", ",");

                            float amount = float.Parse(amountt);
                            string from = "USD";
                            string to = "TND";

                            //DateTime oDate = DateTime.ParseExact(invoice.InvoiceDate, "yyyy-dd-MM", null);
                            string invoiceDate = DateTime.Parse(invoice.InvoiceDate).ToString("yyyy-dd-MM");
                            //string invoiceDate = "2020-04-04";
                            //string invoiceDate = DateTime.Now.ToString("yyyy-MM-dd");


                            Convertion conversion = await RateHelper.GetExchangeRate(from, to, invoiceDate);

                            var amountAfterConvertion = conversion.Rates.TND * amount;

                            amountAfterConvertion = Math.Round(amountAfterConvertion, 2);

                            invoice.InvoiceDate = invoiceDate;
                            invoice.InvoiceAmount = amountAfterConvertion.ToString();
                            invoice.InvoiceFileName = formFile.FileName;

                            invoice.InvoiceID = invoice.InvoiceID;

                            if (invoice.InvoiceFileName == "" || invoice.InvoiceFileName == null || invoice.InvoiceAmount == "" || invoice.InvoiceAmount == null || invoice.InvoiceDate == "" || invoice.InvoiceDate == null)
                            {
                                invoice.IsApproved = false;
                            }
                            else
                            {
                                invoice.IsApproved = true;
                            }

                            #endregion
                            lstInvoices.Add(invoice); /** List Of invoices to be displayed in the View **/
                            //json += JsonConvert.SerializeObject(invoice);
                        }
                    }
                    var str = JsonConvert.SerializeObject(lstInvoices);
                    writer.WriteLine(str);
                    //System.IO.File.WriteAllText(filePathhh, json);
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            //return RedirectToAction("Action", "controller", new { @id = id });
            //ViewBag.lstInvoices = lstInvoices;
            //TempData["doc"] = lstInvoices;


            //nesta3melhom bech nkharej les donnees fil vu mte3i


            return RedirectToAction("Traitement_facture", new
            {
                serializedModel = JsonConvert.SerializeObject(lstInvoices.ToList())// lezem ykoun string psq redirectToAction accept les strings

            });

            //return View(new { count = files.Count, size, filePaths });
        }


        //
        //HttpGet
        public IActionResult Traitement_facture(string serializedModel)
        {
            List<Invoice> lstInvoices = JsonConvert.DeserializeObject<List<Invoice>>(serializedModel);

            return View(lstInvoices);
        }

        [HttpPost]
        public string InsertData(IEnumerable<Invoice> lstInvoices)
        {
            return "OK";
        }
    }
}