using AutoMapper;
using DecaBlog.Models;
using DecaBlog.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecaBlog.MappingProfiles
{
    public class StackProfile : Profile
    {
        public StackProfile()
        {
            CreateMap<StackToAddDto, Stack>();
            CreateMap<Stack, StackMinInfoToReturnDto>();
        }
    }
}
