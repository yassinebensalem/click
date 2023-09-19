using System;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class PagedSubscriberPurchase
    {
        public string BookTitle { get; set; }
        public DateTime Date { get; set; }
        public string PublisherName { get; set; }
        public double Price { get; set; }
    }
}
