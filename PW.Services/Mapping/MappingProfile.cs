using AutoMapper;
using PW.DataTransferObjects.Transactions;
using PW.DataTransferObjects.Users;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PW.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PwUser, UserBalanceDto>();
            CreateMap<PwUser, UserDto>();
            CreateMap<UserDto, UserBalanceDto>();
            CreateMap<PwTransaction, TransactionDto>()
                .ForMember(t => t.Date, opt => opt.MapFrom(src => src.TransactionDateTime.ToString("u", new CultureInfo("en-US"))));                
        }
    }
}
