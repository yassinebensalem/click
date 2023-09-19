using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DDD.Application.ViewModels
{
    public class CategoryViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Category Name is Required")]
        [MinLength(2)]
        [MaxLength(100)]
        [DisplayName("CategoryName")]
        public string CategoryName { get; set;}
        [DisplayName("Status")]
        public bool Status { get; set; }
        public Guid? ParentId { get; set; }

    }
}
