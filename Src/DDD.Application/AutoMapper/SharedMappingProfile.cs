using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DDD.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AC = DDD.Application.Enum;
using DC = DDD.Domain.Common.Constants;

namespace DDD.Application.AutoMapper
{
    public class SharedMappingProfile : Profile
    {
        public SharedMappingProfile()
        {
            CreateMap<AC.Constants.SearchEnum, DC.GlobalConstant.SearchEnum>().ReverseMap();
        }
    }
}
