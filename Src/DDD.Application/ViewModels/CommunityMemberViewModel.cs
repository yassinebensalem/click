using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DDD.Application.ViewModels
{
    public class CommunityMemberViewModel
    {
        [Required(ErrorMessage = "Community Id is Required")]
        [Key]
        public Guid CommunityId { get; set; }
        [Required(ErrorMessage = "Member Id is Required")]
        [Key]
        public string MemberId { get; set; }
        public bool IsCommunityAdmin { get; set; }
        public bool Status { get; set; }
        [DisplayName("CreatedAt")]
        public string CreatedAt { get; set; }
        [DisplayName("CreatedBy")]
        public string CreatedBy { get; set; }

    }
}
