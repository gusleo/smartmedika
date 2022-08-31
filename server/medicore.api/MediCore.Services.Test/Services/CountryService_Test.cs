using dna.core.auth;
using dna.core.unit.testing;
using MediCore.Service;
using MediCore.Service.Model;
using MediCore.Service.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using MediCore.Data.Entities;
using MediCore.Data;
using MediCore.Data.UnitOfWork;
using dna.core.data.Infrastructure;
using dna.core.service.Infrastructure;

namespace MediCore.Services.Test.Services
{
    public class CountryService_Test
    {

        private CountryService countryService;
        public CountryService_Test()
        {
            AutoMapperConfiguration.Configure();
            var mockSet = new List<Country>
            {
                new Country {Id = 1, Name = "Indonesia", Code = "IDN", Status = Status.Active },
                new Country {Id = 2, Name = "United State of America", Code = "USA", Status = Status.Active}
            }.ToAsyncDbSetMock();

            
            var mockContext = new Mock<IMediCoreContext>();
            mockContext.Setup(c => c.Set<Country>()).Returns(mockSet.Object);
           

            var unitOfWork = new MediCoreUnitOfWork(mockContext.Object);
            var auth = new Mock<IAuthenticationService>();
            countryService = new CountryService(auth.Object, unitOfWork);

        }

        
        

        [Fact]
        public async Task Should_Return_Pagination_Country_All_Status()
        {
            
            var result = await countryService.GetAllAsync(1, 20);

            Assert.NotNull(result.Item);
            Assert.IsType<PaginationSet<CountryModel>>(result.Item);
            Assert.Equal(MessageConstant.Load, result.Message);
            Assert.True(result.Success);

        }

        [Fact]
        public async Task Should_Return_Country_By_Id()
        {
           
            var result = await countryService.GetSingleAsync(1);

            Assert.NotNull(result.Item);
            Assert.Equal(1, result.Item.Id);
            Assert.Equal(MessageConstant.Load, result.Message);
            Assert.True(result.Success);

        }

        [Fact]
        public void Should_Return_Edit_Success_Response()
        {
            var model = new CountryModel
            {
                Id = 1,
                Name = "Indonesia",
                Code = "IDN",
                Status = Status.InActive
            };
            
            var result = countryService.Edit(model);

            Assert.NotNull(result.Item);
            Assert.Equal(Status.InActive, result.Item.Status);
            Assert.Equal(MessageConstant.Update, result.Message);
            Assert.True(result.Success);
        }

        
    }
}
