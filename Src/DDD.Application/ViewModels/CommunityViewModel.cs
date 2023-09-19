using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace DDD.Application.ViewModels
{
    public class CommunityViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Community Name is Required")]
        [MinLength(2)]
        [MaxLength(100)]
        [DisplayName("CommunityName")]
        public string CommunityName { get; set;}
        public ApplicationUserViewModel Admin { get; set; }
        public bool Status { get; set; }
        [DisplayName("CreatedAt")]
        public string CreatedAt { get; set; }
        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

    }
}
