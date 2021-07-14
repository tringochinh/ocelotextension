using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway
{
    public interface IDownstreamExtensionHandler
    {
        Task<string> GetKey(HttpContext httpContext);
    }
}
