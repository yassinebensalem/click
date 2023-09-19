using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Models
{
    public class SavedPage
    {
        public Guid PageId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }


        [ForeignKey("Id")]
        public Book Book { get; set; }

        public SavedPage()
        {
        }
    }
}
