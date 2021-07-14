namespace OcelotExtensionLib.ExtensionMethods
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using OcelotExtensionLib.Extensions;
    using OcelotExtensionLib.Handlers;
    using OcelotExtensionLib.Mappers;

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
        }
    }
}
