using AutoMapper;

namespace MediCore.Service
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMissingTypeMaps = true;
                x.AddProfile<DomainToViewModelMappingProfile>();
            });
        }
    }
}
