using System;
using System.Linq;

namespace App.Web.Models
{
    public class DbInitializer
    {
        public static void Initialize(InvoiceContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Invoices.Any())
            {
                return;   // DB has been seeded
            }
            if (context.Traitements.Any())
            {
                return;   // DB has been seeded
            }

            context.SaveChanges();
        }
    }
}
