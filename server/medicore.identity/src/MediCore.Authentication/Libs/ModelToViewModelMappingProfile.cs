using AutoMapper;
using dna.core.auth.Model;
using dna.core.service.Mapping;
using MediCore.Authentication.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Authentication.Libs
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            CreateMap<RegisterModel, RegisterViewModel>().IgnoreAllNonExisting(this);
        }
    }
}
