namespace OcelotExtensionLib.Extensions
{
    using Microsoft.AspNetCore.Hosting;
    using Newtonsoft.Json;
    using Ocelot.Configuration.ChangeTracking;
    using Ocelot.Configuration.Repository;
    using Ocelot.Responses;
    using OcelotExtensionLib.ExtensionMethods;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Register singleton
    /// </summary>
    public class DiskFileConfigurationRepositoryExtension : DiskFileConfigurationRepository, IFileConfigurationRepositoryExtension
    {
        private readonly string _environmentFilePath;
        private readonly string _ocelotFilePath;
        private static readonly object _lock = new object();
        private const string ConfigurationFileName = "ocelot";
        private FileConfigurationExtension dataConfig;
        public DiskFileConfigurationRepositoryExtension(
            IWebHostEnvironment hostingEnvironment, IOcelotConfigurationChangeTokenSource changeTokenSource)
            : base(hostingEnvironment, changeTokenSource)
        {
            _environmentFilePath = $"{AppContext.BaseDirectory}{ConfigurationFileName}{(string.IsNullOrEmpty(hostingEnvironment.EnvironmentName) ? string.Empty : ".")}{hostingEnvironment.EnvironmentName}.json";
            _ocelotFilePath = $"{AppContext.BaseDirectory}{ConfigurationFileName}.json";
            dataConfig = GetExtension();
        }

        public Task<Response<FileConfigurationExtension>> GetDataConfig()
        {
            var clone = this.dataConfig.DeepCopy();
            return Task.FromResult<Response<FileConfigurationExtension>>(new OkResponse<FileConfigurationExtension>(clone));
        }

        private FileConfigurationExtension GetExtension()
        {
            string jsonConfiguration = string.Empty;

            lock (_lock)
            {
                if (System.IO.File.Exists(_environmentFilePath))
                {
                    jsonConfiguration = System.IO.File.ReadAllText(_environmentFilePath);
                }
                else if (System.IO.File.Exists(_ocelotFilePath))
                {
                    jsonConfiguration = System.IO.File.ReadAllText(_ocelotFilePath);
                }
            }

            var fileConfiguration = JsonConvert.DeserializeObject<FileConfigurationExtension>(jsonConfiguration);

            return fileConfiguration;
        }
    }
}
