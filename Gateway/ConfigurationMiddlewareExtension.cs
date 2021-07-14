namespace Gateway
{
    using System.Threading.Tasks;
    using Ocelot.Errors.Middleware;
    using Ocelot.Configuration.Repository;
    using Ocelot.Logging;
    using Microsoft.AspNetCore.Http;
    using Ocelot.DownstreamRouteFinder.Middleware;
    using Ocelot.Middleware;
    using Microsoft.Extensions.DependencyInjection;
    using Ocelot.Configuration.Creator;
    using Ocelot.Configuration;
    using AutoMapper;
    using Ocelot.Configuration.File;
    using Ocelot.DownstreamRouteFinder.UrlMatcher;
    using System.Linq;
    using Microsoft.Extensions.Options;

    public class ConfigurationMiddlewareExtension : OcelotMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMapper _mapper;
        private readonly IUrlPathToUrlTemplateMatcher _urlMatcher;
        private readonly IUpstreamTemplatePatternCreator _upstreamTemplatePatternCreator;
        private readonly IDownstreamExtensionHandler _downstreamExtensionHandler;
        public ConfigurationMiddlewareExtension(
            RequestDelegate next, IOcelotLoggerFactory loggerFactory, 
            IMapper mapper,
            IUrlPathToUrlTemplateMatcher urlMatcher,
            IUpstreamTemplatePatternCreator upstreamTemplatePatternCreator,
            IDownstreamExtensionHandler downstreamExtensionHandler)
            : base(loggerFactory.CreateLogger<ExceptionHandlerMiddleware>())
        {
            _next = next;
            _mapper = mapper;
            _urlMatcher = urlMatcher;
            _upstreamTemplatePatternCreator = upstreamTemplatePatternCreator;
            _downstreamExtensionHandler = downstreamExtensionHandler;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var upstreamUrlPath = httpContext.Request.Path.ToString();

            var upstreamQueryString = httpContext.Request.QueryString.ToString();

            //==================================================================
            //Read file config extension 
            var configurationRepo = httpContext.RequestServices.GetRequiredService<IFileConfigurationRepositoryExtension>();
            var ocelotConfiguration = await configurationRepo.GetDataConfig();
            var dataConfig = ocelotConfiguration.Data;

            var lstRouter = dataConfig.Routes;

            //======================================
            //Set up lai file config
            foreach (var route in lstRouter)
            {
                var pattern = _upstreamTemplatePatternCreator.Create(route);
                var urlMatch = _urlMatcher.Match(upstreamUrlPath, upstreamQueryString, pattern);
                if (urlMatch.Data.Match)
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
                    }     
                }
            }
            //======================================

            //---------------------
            // xu ly uy thac
          
            //--------------------

            


            //dung automapper map FileConfigurationExtension --> FileConfiguration
            var dataConfigMapped = this._mapper.Map<FileConfigurationExtension, FileConfiguration>(dataConfig);

            //Xu ly file config
            //
            //==================================================================
            //Tao internalConfiguration
            // Moi request se phai handle lai ocelotConfiguration.Data
            var internalConfigurationCreator = httpContext.RequestServices.GetRequiredService<IInternalConfigurationCreator>();
            var internalConfiguration = await internalConfigurationCreator.Create(dataConfigMapped);
            //==================================================================


            if (internalConfiguration.IsError)
            {
                throw new System.Exception("OOOOPS this should not happen raise an issue in GitHub");
            }

            httpContext.Items.SetIInternalConfiguration(internalConfiguration.Data);

            await _next.Invoke(httpContext);
        }
    }
}
