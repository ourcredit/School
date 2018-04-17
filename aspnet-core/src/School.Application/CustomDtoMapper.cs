using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using School.Authorization.Accounts.Dto;
using School.Authorization.Permissions.Dto;
using School.Authorization.Roles;
using School.Authorization.Roles.Dto;
using School.Authorization.Users;
using School.Authorization.Users.Dto;
using School.Devices.Dtos;
using School.Models;
using School.MultiTenancy;
using School.OperatorTrees.Dtos;
using School.Others.Dtos;
using School.Points.Dtos;
using School.Sessions.Dto;

namespace School
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
           
            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

          
            //Tenant
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
        
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
            configuration.CreateMap<Device, DeviceListDto>()
                .ForMember(c=>c.PointName,opt=>opt.MapFrom(c=>c.Point.PointName));
            configuration.CreateMap<DeviceEditDto, Device>();

            configuration.CreateMap<Point, PointListDto>();
            configuration.CreateMap<PointEditDto, Point>();

            configuration.CreateMap<OperatorTree, OperatorTreeListDto>();
            configuration.CreateMap<OperatorTreeEditDto, OperatorTree>();

            configuration.CreateMap<Channel, ChannelListDto>();
            configuration.CreateMap<Show, ShowListDto>();
            
        }
    }
}