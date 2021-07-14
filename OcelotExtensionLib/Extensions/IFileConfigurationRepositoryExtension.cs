namespace OcelotExtensionLib.Extensions
{
    using Ocelot.Configuration.Repository;
    using Ocelot.Responses;
    using System.Threading.Tasks;

    public interface IFileConfigurationRepositoryExtension : IFileConfigurationRepository
    {
        Task<Response<FileConfigurationExtension>> GetDataConfig();
    }
}
