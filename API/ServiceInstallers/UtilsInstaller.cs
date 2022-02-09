using Totem.API.ServiceInstallers.Base;
using Totem.Business.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Totem.API.ServiceInstallers
{
    public class UtilsInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<TokenManager>();
            services.AddTransient<CommonMethods>();
        }
    }
}
