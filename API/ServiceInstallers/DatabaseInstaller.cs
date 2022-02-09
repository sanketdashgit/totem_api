using Totem.API.ServiceInstallers.Base;
using Totem.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Totem.API.ServiceInstallers
{
    public class DatabaseInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //To DO: add db context
            services.AddDbContext<TotemDBContext>(item =>
            {
                item
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .EnableSensitiveDataLogging();
            }, ServiceLifetime.Transient);
        }
    }
}
