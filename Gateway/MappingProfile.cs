using AutoMapper;
using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            //CreateMap<User, UserDto>();
            CreateMap<FileConfigurationExtension, FileConfiguration>();
        }
    }
}
