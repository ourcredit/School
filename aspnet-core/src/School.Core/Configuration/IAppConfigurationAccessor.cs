using Microsoft.Extensions.Configuration;

namespace School.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
