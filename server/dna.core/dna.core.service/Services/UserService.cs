using AutoMapper;
using dna.core.auth;
using dna.core.auth.Entity;
using dna.core.data.UnitOfWork;
using dna.core.service.Infrastructure;
using dna.core.service.Models;
using dna.core.service.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dna.core.service.Services
{
    public class UserService: IUserService
    {
        private readonly IDNAUnitOfWork _unitOfWork;
        public UserService(IDNAUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public bool IsUserPhoneNumberExist(string phoneNumber)
        {
            var entity = _unitOfWork.UserRepository.IsUserPhoneNumberExist(phoneNumber);
            return entity;
        }
    }
}
