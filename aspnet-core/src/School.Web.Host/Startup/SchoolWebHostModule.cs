using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Hangfire;
using School.Configuration;
using School.OperatorTrees.DomainServices;

namespace School.Web.Host.Startup
{
    [DependsOn(
       typeof(SchoolWebCoreModule))]
    public class SchoolWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public SchoolWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SchoolWebHostModule).GetAssembly());
        }
    }
}
