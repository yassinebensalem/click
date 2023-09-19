using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Models
{

    public class Metrics
    {
        public Guid MetricsId { get; set; }
        public DateTime MetricsDate { get; set; }
        public MetricType Type { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }


        [ForeignKey("Id")]
        public Book Book { get; set; }

        public Metrics()
        {
        }

    }
    public enum MetricType
    {
        element1,
 element2,
    }
}
