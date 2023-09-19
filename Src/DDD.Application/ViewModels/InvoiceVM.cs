using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Application.ViewModels
{
    public class InvoiceVM
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public double Price { get; set; }

        public string OrderNumber { get; set; }
        public string AuthorizationCode { get; set; }

        public PaymentType PaymentType { get; set; }
        public PaymentReason PaymentReason { get; set; }
    }
}
