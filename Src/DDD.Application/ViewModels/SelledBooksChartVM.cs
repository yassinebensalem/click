using System;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class SelledBooksChartVM
    {
        public DateTime Date { get; set; }

        public long Count { get; set; }
        public double Value { get; set; }
    }
}
