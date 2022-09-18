using Newtonsoft.Json;
using System.Collections.Generic;

namespace App.Domain
{
    public class Invoice
    {
        public long Id { get; set; }
        public string ContractID { get; set; }
        public string CustomerID { get; set; }
        public string InvoiceID { get; set; }
        public string InvoiceFileName { get;set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public bool IsApproved { get; set; }
        public bool IsNotApproved { get; set; }

    }
}
