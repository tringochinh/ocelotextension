namespace OcelotExtensionLib.Handlers
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IDownstreamExtensionHandler
    {
        Task<string> GetKey(HttpContext httpContext);
    }
}
