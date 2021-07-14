using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway
{
    public class FileHostAndPortExtension: FileHostAndPort
    {
        public string Key { get; set; }
    }
}
