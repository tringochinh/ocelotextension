using Ocelot.Configuration;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcelotExtensionLib.Caches
{
    public interface IDictionaryCache
    {
        Task<Response<IInternalConfiguration>> GetCache(string key);
        Task<Response<IInternalConfiguration>> SetCache(string key, Response<IInternalConfiguration> item);
        Task RemoveAll();
        Task RemoveAt(string key);
    }
    public class DictionaryCache: IDictionaryCache
    {
        private Dictionary<string, Response<IInternalConfiguration>> keyValuePairs;
        public DictionaryCache()
        {
            keyValuePairs = new Dictionary<string, Response<IInternalConfiguration>>();
            keyValuePairs["none"] = null;
        }

        public Task<Response<IInternalConfiguration>> GetCache(string key)
        {
            if (keyValuePairs.ContainsKey(key))
            {
                return Task.FromResult(keyValuePairs[key]);
            }
            return Task.FromResult(keyValuePairs["none"]);
        }

        public Task<Response<IInternalConfiguration>> SetCache(string key, Response<IInternalConfiguration> item)
        {
            keyValuePairs[key] = item;
            return Task.FromResult(item);
        }

        public Task RemoveAll()
        {
            keyValuePairs.Clear();
            return Task.CompletedTask;
        }

        public Task RemoveAt(string key)
        {
            if(keyValuePairs.ContainsKey(key))
            {
                keyValuePairs.Remove(key);
            }
            return Task.CompletedTask;
        }
    }
}
