using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.ViewModels
{
    public class PaymentDetailsVM
    {
        public string orderId { get; set; }
        public string orderNumber { get; set; }
        public int errorCode { get; set; }
        public string ErrorCode { get; set; }
        public string errorMessage { get; set; }
        public string ErrorMessage { get; set; }
        public int OrderStatus { get; set; }
        public string OrderNumber { get; set; }
        public string formUrl { get; set; }
        public double Amount { get; set; }
        public string UserName { get; set; }
        public string AuthorizationCode { get; set; }
        public DateTime Date { get; set; }


    }
}
