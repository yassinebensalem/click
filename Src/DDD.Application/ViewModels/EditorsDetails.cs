using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;

namespace DDD.Application.ViewModels
{
    public class EditorsDetails
    {
        public List<ApplicationUser> EditorsList { get; set; }
        public List<BookViewModel> PublishedBooks { get; set; }
        
    }
}
