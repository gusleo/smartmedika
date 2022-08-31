using System.Collections.Generic;
using MediCore.Data;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using Moq;
using dna.core.unit.testing;

namespace MediCore.Service.Test
{
    public static class MockDbContext
    {
        
        public static Mock<IMediCoreContext> Initialize(){
            
            var mockPatient = new List<Patient>
            {
                new Patient{ Id = 1, PatientName = "Gus Leo", PatientStatus = PatientStatus.Active, Gender = Gender.Male },
                new Patient{ Id = 1, PatientName = "Dayu Ita", PatientStatus = PatientStatus.Active, Gender = Gender.Female }
            }.ToAsyncDbSetMock();


            var mockContext = new Mock<IMediCoreContext>();
            mockContext.Setup(c => c.Set<Patient>()).Returns(mockPatient.Object);

            return mockContext;
        }
    }
}
