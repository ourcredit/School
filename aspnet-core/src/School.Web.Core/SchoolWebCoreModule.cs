using System;
using System.IO;
using System.Text;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Hangfire;
using Abp.IO;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using School.Authentication.JwtBearer;
using School.Configuration;
using School.EntityFrameworkCore;

#if FEATURE_SIGNALR
using Abp.Web.SignalR;
#endif

namespace School
{
    [DependsOn(
         typeof(SchoolApplicationModule),
         typeof(SchoolEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
#if FEATURE_SIGNALR 
        ,typeof(AbpWebSignalRModule)
#elif FEATURE_SIGNALR_ASPNETCORE
        ,typeof(AbpAspNetCoreSignalRModule)
        , typeof(AbpHangfireAspNetCoreModule)
#endif
     )]
    public class SchoolWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public SchoolWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                SchoolConsts.ConnectionStringName
            );

            // Use database for language management
            // Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(SchoolApplicationModule).GetAssembly()
                 );

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SchoolWebCoreModule).GetAssembly());
        }
        public override void PostInitialize()
        {
            SetAppFolders();
        }
        private void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = Path.Combine(_env.WebRootPath, $"Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}SampleProfilePics");
            appFolders.TempFileDownloadFolder = Path.Combine(_env.WebRootPath, $"Temp{Path.DirectorySeparatorChar}Downloads");
            appFolders.WebLogsFolder = Path.Combine(_env.ContentRootPath, $"App_Data{Path.DirectorySeparatorChar}Logs");

#if NET461
            if (_env.IsDevelopment())
            {
                var currentAssemblyDirectoryPath = typeof(AbpZeroTemplateWebCoreModule).GetAssembly().GetDirectoryPathOrNull();
                if (currentAssemblyDirectoryPath != null)
                {
                    appFolders.WebLogsFolder = Path.Combine(currentAssemblyDirectoryPath, $"App_Data{Path.DirectorySeparatorChar}Logs");
                }
            }
#endif

            try
            {
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder);
            }
            catch { }
        }
    }
}
