using Microsoft.Extensions.Configuration;

namespace prod.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
