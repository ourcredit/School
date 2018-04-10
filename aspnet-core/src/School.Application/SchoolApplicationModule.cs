using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using School.Authorization;

namespace School
{
    [DependsOn(
        typeof(SchoolCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class SchoolApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SchoolApplicationModule).GetAssembly());

        }
    }
}
