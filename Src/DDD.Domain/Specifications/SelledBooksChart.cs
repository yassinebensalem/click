using System;
using DDD.Domain.Models;

namespace DDD.Domain.Specifications
{
    public class SelledBooksChart
    {
        public DateTime Date { get; set; }

        public long Count { get; set; }

        public double Value { get; set; }
    }
}
