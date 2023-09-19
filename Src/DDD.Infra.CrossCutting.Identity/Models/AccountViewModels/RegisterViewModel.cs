using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using DDD.Domain.Models;
using Microsoft.AspNetCore.Http;
using static DDD.Application.Enum.Constants;

namespace DDD.Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is Required. It cannot be empty")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

        public DateTime Birthdate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string PhotoPath { get; set; }
        public int? CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public char? Gender { get; set; }

        [DisplayName("PhotoPath")]
        public IFormFile PhotoFile { get; set; }

        public UserRoleVM UserRole { get; set; }
        public double RateOnOriginalPrice { get; set; }
        public double RateOnSale { get; set; }
        public string RaisonSocial { get; set; }
        public string IdFiscal { get; set; }
        public bool IsActive { get; set; }

        public bool OnlySendConfirmEmail { get; set; }

    }

}
