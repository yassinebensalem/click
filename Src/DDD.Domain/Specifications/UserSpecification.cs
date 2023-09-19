using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Domain.Specifications
{
    public class JoinRequestIntervalSpecification : BaseSpecification<JoinRequest>
    {
        public JoinRequestIntervalSpecification(DateTime fromDate, DateTime toDate, JoinRequestType type)
            : base(b => b.RequesterType == type && b.CreatedAt.Value.Date >= fromDate && b.CreatedAt.Value.Date <= toDate)
        {

        }
    }

    public class JoinRequestByEmailSpecification : BaseSpecification<JoinRequest>
    {
        public JoinRequestByEmailSpecification(string email)
            : base(b => b.Email == email)
        {

        }
    }

    public class AuthorByEmailSpecification : BaseSpecification<Author>
    {
        public AuthorByEmailSpecification(string email)
            : base(b => b.Email == email)
        {

        }
    }

    public class AuthorByFullNameSpecification : BaseSpecification<Author>
    {
        public AuthorByFullNameSpecification(string fullName)
            : base(b => String.Concat(String.Concat(b.FirstName, " "), b.LastName) == fullName)
        {

        }
    }

    public class GetAllAuthorsSpecification : BaseSpecification<Author>
    {
        public GetAllAuthorsSpecification()
            : base(b => !b.IsDeleted)
        {

        }
    }
}
