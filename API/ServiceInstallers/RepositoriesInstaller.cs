using Totem.API.ServiceInstallers.Base;
using Totem.Business.Repositories;
using Totem.Business.RepositoryInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Totem.Externals.Caching;

namespace Totem.API.ServiceInstallers
{
    /// <summary>
    /// Installs domain repositories an their contracts into Dependency Container
    /// </summary>
    public class RepositoriesInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IWebsiteRepository, WebsiteRepository>();
            services.AddTransient<IAmazonS3Repository, AmazonS3Repository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<IFollowersRepository, FollowersRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ICacheManager, CacheManager>();
        }
    }
}
