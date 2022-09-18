using Microsoft.EntityFrameworkCore;

namespace App.Web.Models
{
    public class InvoiceContext : DbContext
    {

        public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Traitement> Traitements { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Invoice>().ToTable("Invoice");
        //    modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        //    modelBuilder.Entity<Student>().ToTable("Student");
        //}
    }
}
