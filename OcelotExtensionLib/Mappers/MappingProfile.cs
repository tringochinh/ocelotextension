namespace OcelotExtensionLib.Mappers
{
    using AutoMapper;
    using Ocelot.Configuration.File;
    using OcelotExtensionLib.Extensions;

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
