using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway
{
    public class FileConfigurationExtension: FileConfiguration
    {
        public new List<FileRouteExtension> Routes { get; set; }
        public FileConfigurationExtension()
        {
            this.Routes = new List<FileRouteExtension>();
        }
    }
}
