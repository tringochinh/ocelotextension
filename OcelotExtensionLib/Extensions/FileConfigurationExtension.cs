namespace OcelotExtensionLib.Extensions
{
    using Ocelot.Configuration.File;
    using System.Collections.Generic;

    public class FileConfigurationExtension : FileConfiguration
    {
        public new List<FileRouteExtension> Routes { get; set; }
        public FileConfigurationExtension()
        {
            this.Routes = new List<FileRouteExtension>();
        }
    }
}
