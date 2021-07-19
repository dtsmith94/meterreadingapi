using MeterReadingsApi.Interface.Data.Repositories;
using MeterReadingsApi.Interface.Service.Services;
using MeterReadingsApi.Model.Data;
using MeterReadingsApi.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReadingsApi.Test.Service.Services
{
    [TestClass]
    public class MeterReadingServiceTest
    {
        [TestMethod]
        public async Task ProcessNewReadingsSuccessfulTest()
        {
            // arrange
            var customerAccountServiceMock = new Mock<ICustomerAccountService>();
            var meterReadingRepositoryMock = new Mock<IMeterReadingRepository>();
            var meterReadingService = new MeterReadingService(customerAccountServiceMock.Object, meterReadingRepositoryMock.Object);

            customerAccountServiceMock.Setup(mock => mock.GetAsync(2344, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 2344 });
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2233, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 2233 });
            customerAccountServiceMock.Setup(mock => mock.GetAsync(8766, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 8766 });
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2644, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 2644 });
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2345, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 2345 });

            var meterReadingsCsv = "AccountId,MeterReadingDateTime,MeterReadValue\r\n" +
                                    "2344,22/04/2019 09:24,01002\r\n" +
                                    "2233,22/04/2019 12:25,0032Q\r\n" +
                                    "8766,22/04/2019 12:25,34407\r\n" +
                                    "2644,22/04/2019 12:25,4A2G7\r\n" +
                                    "2345,22/04/2019 13:25,ABABA";

            // act
            var result = await meterReadingService.ProcessNewReadings(meterReadingsCsv);

            // assert
            Assert.AreEqual(0, result.FailedUploads);
            Assert.AreEqual(5, result.SuccessfulUploads);
        }

        [TestMethod]
        public async Task ProcessNewReadingsInvalidReadingsTest()
        {
            // arrange
            var customerAccountServiceMock = new Mock<ICustomerAccountService>();
            var meterReadingRepositoryMock = new Mock<IMeterReadingRepository>();
            var meterReadingService = new MeterReadingService(customerAccountServiceMock.Object, meterReadingRepositoryMock.Object);

            // setup the test so that the customers exist, so that we are purely testing the validation of the meter reading values
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2344, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 2344 });
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2233, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 2233 });
            customerAccountServiceMock.Setup(mock => mock.GetAsync(8766, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 8766 });
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2644, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 2644 });
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2345, It.IsAny<bool>())).ReturnsAsync(new CustomerAccount { Id = 2345 });

            var meterReadingsCsv = "AccountId,MeterReadingDateTime,MeterReadValue\r\n" +
                                    "2344,22/04/2019 09:24,01002+\r\n" +
                                    "2233,22/04/2019 12:25,0032!\r\n" +
                                    "8766,22/04/2019 12:25,3440\r\n" +
                                    "2644,22/04/2019 12:25,\r\n" +
                                    "2345,22/04/2019 13:25";

            // act
            var result = await meterReadingService.ProcessNewReadings(meterReadingsCsv);

            // assert
            Assert.AreEqual(5, result.FailedUploads);
            Assert.AreEqual(0, result.SuccessfulUploads);
        }

        [TestMethod]
        public async Task ProcessNewReadingsCustomerInvalidReadingsTest()
        {
            // arrange
            var customerAccountServiceMock = new Mock<ICustomerAccountService>();
            var meterReadingRepositoryMock = new Mock<IMeterReadingRepository>();
            var meterReadingService = new MeterReadingService(customerAccountServiceMock.Object, meterReadingRepositoryMock.Object);

            // return a null customer when passing Customer Account ID 2345 to simulate no customer being matched in the DB
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2345, It.IsAny<bool>())).ReturnsAsync((CustomerAccount)null);

            var meterReadingsCsv = "AccountId,MeterReadingDateTime,MeterReadValue\r\n" +
                                    "2345,22/04/2019 13:25";

            // act
            var result = await meterReadingService.ProcessNewReadings(meterReadingsCsv);

            // assert
            Assert.AreEqual(1, result.FailedUploads);
            Assert.AreEqual(0, result.SuccessfulUploads);
        }

        [TestMethod]
        public async Task ProcessNewReadingsDuplicateReadingsTest()
        {
            // arrange
            var customerAccountServiceMock = new Mock<ICustomerAccountService>();
            var meterReadingRepositoryMock = new Mock<IMeterReadingRepository>();
            var meterReadingService = new MeterReadingService(customerAccountServiceMock.Object, meterReadingRepositoryMock.Object);

            var customerWithDuplicateReadings = new CustomerAccount
            {
                Id = 2344,
                MeterReadings = new List<MeterReading> { new MeterReading {
                    Value = "01002"
                }}
            };

            // return a null customer when passing Customer Account ID 2345 to simulate no customer being matched in the DB
            customerAccountServiceMock.Setup(mock => mock.GetAsync(2344, It.IsAny<bool>())).ReturnsAsync(customerWithDuplicateReadings);

            var meterReadingsCsv = "AccountId,MeterReadingDateTime,MeterReadValue\r\n" +
                                    "2344,22/04/2019 09:24,01002";

            // act
            var result = await meterReadingService.ProcessNewReadings(meterReadingsCsv);

            // assert
            Assert.AreEqual(1, result.FailedUploads);
            Assert.AreEqual(0, result.SuccessfulUploads);
        }
    }
}
