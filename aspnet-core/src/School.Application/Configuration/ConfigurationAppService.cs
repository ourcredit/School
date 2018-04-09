using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using School.Configuration.Dto;

namespace School.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : SchoolAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
