using System;
using Xunit;
using MediCore.Service.Services;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using System.Collections.Generic;
using dna.core.unit.testing;
using Moq;
using MediCore.Data;
using MediCore.Data.UnitOfWork;
using dna.core.auth;
using System.Threading.Tasks;
using MediCore.Service.Model;
using dna.core.service.Infrastructure;
using dna.core.libs.Validation;

namespace MediCore.Service.Test
{
    public class PatientService_Test
    {
        private PatientService _patientService;

        public PatientService_Test()
        {
            //AutoMapperConfiguration.Configure();
            var mockContext = MockDbContext.Initialize();
            var unitOfWork = new MediCoreUnitOfWork(mockContext.Object);
            var auth = new Mock<IAuthenticationService>();
            _patientService = new PatientService(unitOfWork, auth.Object);

        }

        public void AssignState(){
            var dict = new Mock<IValidationDictionary>();
            dict.Setup(cw => cw.IsValid).Returns(true);
            _patientService.Initialize(dict.Object);
        }

        [Fact]
        public void Should_Return_Create_Success_Response()
        {
            AutoMapperConfiguration.Configure();
            AssignState();
            var result = _patientService.Create(new PatientModel
            {
                Id = 0,
                PatientName = "Baswara",
                Gender = Gender.Male,
                PatientStatus = PatientStatus.Active,
                RelationshipStatus = RelationshipStatus.Child
            });

            Assert.NotNull(result.Item);
            Assert.Equal(MessageConstant.Create, result.Message);
            Assert.True(result.Success);

        }

        [Fact]
        public void Should_Return_Edit_Success_Response()
        {

            var result = _patientService.Edit(new PatientModel
            {
                Id = 1,
                PatientName = "Gus Leo Ganteng",
                Gender = Gender.Male,
                PatientStatus = PatientStatus.Active,
                RelationshipStatus = RelationshipStatus.Other
            });

            Assert.NotNull(result.Item);
            Assert.Equal(MessageConstant.Update, result.Message);
            Assert.True(result.Success);

        }
    }
}
