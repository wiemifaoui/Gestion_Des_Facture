namespace App.Web.Models
{
    public class Traitement
    {
        public long Id { get; set; }    
        public string DateTraitement { get; set; }
        public string NbrFactures { get; set; }
        public string TotalAmount { get; set; }

        public Traitement(long id, string dateTraitement, string nbrFactures, string totalAmount)
        {
            Id = id;
            DateTraitement = dateTraitement;
            NbrFactures = nbrFactures;
            TotalAmount = totalAmount;
        }

        public Traitement()
        {
        }
    }
}
