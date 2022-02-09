using System;
using System.Collections.Generic;
using System.Linq;
using Totem.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Totem.API.ServiceInstallers.Base
{
    public static class InstallerExtensions
    {
        public static void InstallServices(this IServiceCollection services, IConfiguration configuration)
        {
            // get list of all non abstract classes which implements IServiceInstaller interface
            List<IServiceInstaller> installers = typeof(Startup).Assembly.ExportedTypes
                .Where(type => typeof(IServiceInstaller).IsAssignableFrom(type)
                    && type.IsInterface == false
                    && type.IsAbstract == false)
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>()
                .ToList();

            // loop through all installers and call install service method from them which in tern install services to IServiceCollection
            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
