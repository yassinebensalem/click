using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class InvoiceDetails : EntityAudit
    {
        public Guid InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
        public Guid BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public double Price { get; set; }

        public InvoiceDetails(Guid invoiceId, Invoice invoice, Guid bookId, Book book, double price)
        {
            InvoiceId = invoiceId;
            Invoice = invoice;
            BookId = bookId;
            Book = book;
            Price = price;
        }

        public InvoiceDetails()
        {
        }
    }
}
