using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Application.ViewModels
{
   public class JoinRequestVM
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryId { get; set; }
        public JoinRequestType RequesterType { get; set; }
        public JoinRequestState Status { get; set; }
        public string RaisonSocial { get; set; }
        public string IdFiscal { get; set; }
        public string VoucherNumber { get; set; }
        public double? VoucherValue { get; set; }
        public List<BookCompetitionVM> DesiredBooks { get; set; }
        public string ReceiverEmail { get; set; }

    }
}
