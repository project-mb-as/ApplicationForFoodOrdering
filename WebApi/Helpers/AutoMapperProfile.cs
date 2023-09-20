using AutoMapper;
//using WebApi.Entities;
using WebApi.Models.Users;
using Domain.Models;
using WebApi.ViewModels;
using System.Collections.Generic;

namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<Models.Users.RegisterModel, User>();
            CreateMap<UpdateModel, User>();

            CreateMap<Meni, MeniViewModel>();
            CreateMap<Meni, MeniForCalendarViewModel>();
            CreateMap<Hrana, HranaViewModel>();
            CreateMap<HranaMeni, HranaMeniViewModel>();
            CreateMap<HranaPrilog, HranaPrilogViewModel>();
            CreateMap<Prilog, PrilogViewModel>();

            CreateMap<HranaViewModel, Hrana>();
//                .ForMember(dest => dest.Prilozi, opt => opt.Ignore());
            CreateMap<HranaPrilogViewModel, HranaPrilog>()
                .ForMember(dest => dest.PrilogId,  opt => opt.MapFrom(source => source.Prilog.PrilogId))
                .ForMember(dest => dest.Prilog, opt => opt.Ignore());
            CreateMap<PrilogViewModel, Prilog>();
        }
    }
}