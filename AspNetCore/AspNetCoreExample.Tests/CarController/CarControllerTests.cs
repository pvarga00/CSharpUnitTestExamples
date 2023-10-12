
using AspNetCore.Tests.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using AspNetCoreExample.Model.Interfaces;
using AspNetCoreExample.Repository;
using System.Threading.Tasks;
using AspNetCoreExample.Model;

namespace AspNetCoreExample.Tests
{
    [TestClass]
    public class CarControllerTests
    {
        private readonly string SUCCESS = "SUCCESS";
        private readonly string FAILURE = "FAILURE";
        private readonly string _vin = "VHQ3131312";
        private Mock<IDataRepository> _moqDataRepository = new Mock<IDataRepository>();

        [TestMethod]
        public void GivenVinValidAndAndCarHas4Wheels_WhenCarIsPutInDrive_ReturnSuccess()
        {
            var car = new Car() { Color = "White", NumberOfWheels = 4, VIN = _vin };

            _moqDataRepository.Setup(data => data.GetMyCar(_vin)).Returns(Task.FromResult((ICar)car));

            var result = new CarController(_moqDataRepository.Object).PutCarInDriver(_vin).Result;

            Assert.AreEqual(result, SUCCESS);
        }
    }
}
