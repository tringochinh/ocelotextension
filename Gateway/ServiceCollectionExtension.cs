using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway
{
    public static class ServiceCollectionExtension
    {
        public static void AddOcelotExtension(this IServiceCollection services)
        {
            services.TryAddSingleton<IFileConfigurationRepositoryExtension, DiskFileConfigurationRepositoryExtension>();
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.TryAddSingleton<IDownstreamExtensionHandler, DownstreamExtensionHandler>();
        }
    }
}
