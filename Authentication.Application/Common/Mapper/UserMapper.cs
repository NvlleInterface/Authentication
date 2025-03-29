using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Authentication.Application.Command.Register;

namespace Authentication.Application.Common.Mapper;

public sealed class UserMapper : Profile
{
    public UserMapper()
    {


        CreateMap<RegisterCommand, IdentityUser>()
                    .ForMember(
                        dest => dest.Email,
                        opt => opt.MapFrom(src => $"{src.Email}")
                    )
                    .ForMember(
                        dest => dest.UserName,
                        opt => opt.MapFrom(src => $"{src.UserName}")
                    );

        //CreateMap<LoginCommand, IdentityUser>()
        //           .ForMember(
        //               dest => dest.Email,
        //               opt => opt.MapFrom(src => $"{src.Email}")
        //           );

        //CreateMap<CreateRoleCommand, IdentityRole>()
        //            .ForMember(
        //                dest => dest.Name,
        //                opt => opt.MapFrom(src => $"{src.Name}")
        //            )
        //            .ForMember(
        //                dest => dest.NormalizedName,
        //                opt => opt.MapFrom(src => $"{src.NormalizedName}")
        //            );
    }
}