using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DDD.Domain.Core.Models;

namespace DDD.Domain.Models
{
    public class Category :EntityAudit
    {
        public string CategoryName { get; set; }
        public bool Status { get; set; }
        public Guid? ParentId { get; set; }
        public ICollection<Book> Books { get; set; }

        public Category( Guid id, string categoryName, bool status,Guid? parentId)
        {
            Id = id;
            CategoryName = categoryName;
            Status = status;
            ParentId = parentId;
        }

        public Category()
        {
        }
    }

}
