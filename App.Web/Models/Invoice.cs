namespace App.Web.Models
{
    public class Invoice
    {
        public long Id { get; set; }
        public long IdTraitement { get; set; }
        public string ContractID { get; set; }
        public string CustomerID { get; set; }
        public string InvoiceID { get; set; }
        public string InvoiceFileName { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public bool IsApproved { get; set; }
        public bool IsNotApproved { get; set; }

        public Invoice()
        {
        }

        public Invoice(long id, long idTraitement, string contractID, string customerID, string invoiceID, string invoiceFileName, string invoiceDate, string invoiceAmount, bool isApproved, bool isNotApproved)
        {
            Id = id;
            IdTraitement = idTraitement;
            ContractID = contractID;
            CustomerID = customerID;
            InvoiceID = invoiceID;
            InvoiceFileName = invoiceFileName;
            InvoiceDate = invoiceDate;
            InvoiceAmount = invoiceAmount;
            IsApproved = isApproved;
            IsNotApproved = isNotApproved;
        }
    }
}
