namespace OcelotExtensionLib.Extensions
{
    using Ocelot.Configuration.File;
    using System.Collections.Generic;

    public class FileRouteExtension : FileRoute, IRoute
    {
        public List<DownstreamPathTemplateExtensionItem> DownstreamPathTemplateExtension { get; set; }
        public List<FileHostAndPortExtension> DownstreamHostAndPortsExtension { get; set; }
        public FileRouteExtension()
        {
            DownstreamPathTemplateExtension = new List<DownstreamPathTemplateExtensionItem>();
            DownstreamHostAndPortsExtension = new List<FileHostAndPortExtension>();
        }
    }
}
