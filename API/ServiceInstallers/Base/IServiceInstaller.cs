using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Totem.API.ServiceInstallers.Base
{
    public interface IServiceInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
