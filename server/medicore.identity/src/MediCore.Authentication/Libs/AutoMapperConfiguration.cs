using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Authentication.Libs
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMissingTypeMaps = true;
                x.AddProfile<ModelToViewModelMappingProfile>();
            });
        }
    }
}
