using System;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class PagedSubscriberPurchaseVM
    {
        public string BookTitle { get; set; }
        public DateTime Date { get; set; }
        public string PublisherName { get; set; }
        public double Price { get; set; }
    }
}
