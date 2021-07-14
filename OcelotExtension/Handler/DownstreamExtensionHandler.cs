using Microsoft.AspNetCore.Http;
using OcelotExtensionLib.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotExtension.Handler
{
    public class DownstreamExtensionHandler : IDownstreamExtensionHandler
    {
        public Task<string> GetKey(HttpContext httpContext)
        {
            var key = string.Empty;
            if (httpContext.Request.Headers.ContainsKey("headertruong"))
            {
                var value = httpContext.Request.Headers["headertruong"].ToString();
                if (value == "1234") //get Key1
                {
                    key = "Key1";
                }
                if (value == "abc") //get Key2
                {
                    key = "Key2";
                }
            }

            return Task.FromResult(key);
        }
    }
}
