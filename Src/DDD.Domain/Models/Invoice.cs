using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Domain.Models
{
    public class Invoice : EntityAudit
    {
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public double Price { get; set; }
        public Guid BookId { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public string OrderNumber { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string AuthorizationCode { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentReason PaymentReason { get; set; }

        public Invoice(string userId, ApplicationUser user, DateTime date, Guid bookId, Book book, double price, string orderNumber, PaymentType paymentType, PaymentReason paymentReason = PaymentReason.Purshase)
        {
            UserId = userId;
            User = user;
            Date = date;
            BookId = bookId;
            Book = book;
            Price = price;
            OrderNumber = orderNumber;
            PaymentType = paymentType;
            PaymentReason = paymentReason;
        }

        public Invoice(string userId, DateTime date, Guid bookId, double price, string orderNumber, PaymentType paymentType, string authorizationCode = null, PaymentReason paymentReason = PaymentReason.Purshase)
        {
            UserId = userId;
            Date = date;
            BookId = bookId;
            Price = price;
            OrderNumber = orderNumber;
            AuthorizationCode = authorizationCode;
            PaymentType = paymentType;
            PaymentReason = paymentReason;
        }

        public Invoice()
        {
        }
    }
}
