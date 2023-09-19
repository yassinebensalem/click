using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DDD.Application.ViewModels
{
    public class CommunityEditViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Community Name is Required")]
        [MinLength(2)]
        [MaxLength(100)]
        [DisplayName("CommunityName")]
        public string CommunityName { get; set;}
        [Required(ErrorMessage = "Admin email is Required")]
        [MinLength(2)]
        [MaxLength(100)]
        [DisplayName("AdminEmail")]
        public string AdminEmail { get; set; }
        public string AdminId { get; set; }
        public bool Status { get; set; }
        [DisplayName("CreatedAt")]
        public string CreatedAt { get; set; }
        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

    }
}
