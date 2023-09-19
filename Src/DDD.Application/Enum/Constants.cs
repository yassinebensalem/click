using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Application.Enum
{
    public static class Constants
    {
        public enum SearchEnum
        {
            Category = 0,
            Book = 1, //(Titre,ISBN)
            Editor = 2,
            Author = 3
        }

        public enum UserRoleVM
        {
            Admin = 1,
            Subscriber = 2,
            Author = 3,
            Editor = 4
        }
    }
}
