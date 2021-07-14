using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway
{
    public class FileRouteExtension: FileRoute, IRoute
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
