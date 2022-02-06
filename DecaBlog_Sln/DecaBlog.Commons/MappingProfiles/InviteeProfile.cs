using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DecaBlog.Models;
using DecaBlog.Models.DTO;

namespace DecaBlog.MappingProfiles
{
    public class InviteeProfile : Profile
    {
        public InviteeProfile()
        {
            CreateMap<RegisterInvitedUserDto, Invitee>();
        }
    }
}
