using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Models
{
    public class Language
    {
        [Column("Id", Order = 0)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CodeAlpha2 { get; set; }
        public string CodeAlpha3 { get; set; }
        public string Code { get; set; }

        public Language(string name, string codeAlpha2, string codeAlpha3, string code)
        {
            Name = name;
            CodeAlpha2 = codeAlpha2;
            CodeAlpha3 = codeAlpha3;
            Code = code;
        }

        public Language()
        {
        }
    }
}
