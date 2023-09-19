using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class FavoriteCategory : EntityAudit
    {
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public FavoriteCategory()
        {
        }

        public FavoriteCategory(Guid id,string userId,  Guid categoryId )
        {
            Id = id;
            UserId = userId; 
            CategoryId = categoryId; 
        }
    }
}
