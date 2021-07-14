namespace OcelotExtensionLib.Middlewares
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Ocelot.Configuration;
    using Ocelot.Configuration.Creator;
    using Ocelot.Configuration.File;
    using Ocelot.Configuration.Repository;
    using Ocelot.DownstreamRouteFinder.UrlMatcher;
    using Ocelot.Errors.Middleware;
    using Ocelot.Logging;
    using Ocelot.Middleware;
    using Ocelot.Responses;
    using OcelotExtensionLib.Extensions;
    using OcelotExtensionLib.Handlers;
    using System.Linq;
    using System.Threading.Tasks;

    public class ConfigurationMiddlewareExtension : OcelotMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMapper _mapper;
        private readonly IUrlPathToUrlTemplateMatcher _urlMatcher;
        private readonly IUpstreamTemplatePatternCreator _upstreamTemplatePatternCreator;
        private readonly IDownstreamExtensionHandler _downstreamExtensionHandler;
        private readonly IInternalConfigurationRepository _configRepo;
        public ConfigurationMiddlewareExtension(
            RequestDelegate next, IOcelotLoggerFactory loggerFactory,
            IMapper mapper,
            IUrlPathToUrlTemplateMatcher urlMatcher,
            IUpstreamTemplatePatternCreator upstreamTemplatePatternCreator,
            IDownstreamExtensionHandler downstreamExtensionHandler,
            IInternalConfigurationRepository configRepo)
            : base(loggerFactory.CreateLogger<ExceptionHandlerMiddleware>())
        {
            _next = next;
            _mapper = mapper;
            _urlMatcher = urlMatcher;
            _upstreamTemplatePatternCreator = upstreamTemplatePatternCreator;
            _downstreamExtensionHandler = downstreamExtensionHandler;
            _configRepo = configRepo;
        }

        private async Task<FileConfigurationExtension> GetDataConfig(HttpContext httpContext)
        {
            //Read file config extension 
            var configurationRepo = httpContext.RequestServices.GetRequiredService<IFileConfigurationRepositoryExtension>();
            var ocelotConfiguration = await configurationRepo.GetDataConfig();
            var dataConfig = ocelotConfiguration.Data;

            return dataConfig;
        }

        private FileRouteExtension GetRouteMatch(FileConfigurationExtension dataConfig,
            string upstreamUrlPath, string upstreamQueryString)
        {
            foreach (var route in dataConfig.Routes)
            {
                var pattern = _upstreamTemplatePatternCreator.Create(route);
                var urlMatch = _urlMatcher.Match(upstreamUrlPath, upstreamQueryString, pattern);
                if (urlMatch.Data.Match)
                {
                    return route;
                }
            }

            return null;
        }

        private bool IsExtension(FileRouteExtension routeExtension)
        {
            if (routeExtension.DownstreamHostAndPortsExtension.Any()
                && routeExtension.DownstreamPathTemplateExtension.Any())
            {
                return true;
            }
            return false;
        }

        private async Task<Response<IInternalConfiguration>> GetExtension(HttpContext httpContext)
        {
            var config = _configRepo.Get();
            var upstreamUrlPath = httpContext.Request.Path.ToString();
            var upstreamQueryString = httpContext.Request.QueryString.ToString();
            var dataConfig = await GetDataConfig(httpContext);
            var route = GetRouteMatch(dataConfig, upstreamUrlPath, upstreamQueryString);
            if (route != null && IsExtension(route))
            {
                var key = await _downstreamExtensionHandler.GetKey(httpContext);

                if (!string.IsNullOrEmpty(key))
                {
                    var downstreamRouteExt = route.DownstreamPathTemplateExtension;
                    var hostportExt = route.DownstreamHostAndPortsExtension;
                    route.DownstreamHostAndPorts.RemoveAll(r => true);
                    var listPath = downstreamRouteExt.Where(r => r.Key == key).ToList();
                    var listHostPort = hostportExt.Where(r => r.Key == key).ToList();
                    if (listPath.Any() && listHostPort.Any())
                    {
                        var path = listPath.FirstOrDefault();
                        route.DownstreamPathTemplate = path.Path;
                        foreach (var hp in listHostPort)
                        {
                            var tmp = new FileHostAndPort();
                            tmp.Host = hp.Host;
                            tmp.Port = hp.Port;
                            route.DownstreamHostAndPorts.Add(tmp);
                        }
                    }
                    //Tao config
                    var dataConfigMapped = this._mapper.Map<FileConfigurationExtension, FileConfiguration>(dataConfig);
                    var internalConfigurationCreator = httpContext.RequestServices.GetRequiredService<IInternalConfigurationCreator>();
                    var internalConfiguration = await internalConfigurationCreator.Create(dataConfigMapped);
                    config = internalConfiguration;
                }
                else
                {
                    this.Logger.LogError($"Key handle downstream extension null", new System.Exception());
                }
            }

            return config;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var config = await GetExtension(httpContext);

            if (config.IsError)
            {
                throw new System.Exception("OOOOPS this should not happen raise an issue in GitHub");
            }

            httpContext.Items.SetIInternalConfiguration(config.Data);

            await _next.Invoke(httpContext);
        }
    }
}
