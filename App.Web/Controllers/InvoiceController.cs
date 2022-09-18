using App.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly InvoiceContext _context;

        public InvoiceController(InvoiceContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invoices.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UpsertInvoice([FromBody]Invoice invoice)
        {
            _context.Add(invoice);
            await _context.SaveChangesAsync();
            return Json("OK");
        }
        
    }
}
