using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.ViewModels
{
  public  class LanguageViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CodeAlpha2 { get; set; }
        public string CodeAlpha3 { get; set; }
        public string Code { get; set; }
    }
}
