using System.Threading.Tasks;
using School.Configuration.Dto;

namespace School.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
