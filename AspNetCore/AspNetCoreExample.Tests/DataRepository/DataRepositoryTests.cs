using AspNetCore.Tests.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using AspNetCoreExample.Model.Interfaces;
using AspNetCoreExample.Repository;
using System.Threading.Tasks;
using AspNetCoreExample.Model;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace AspNetCoreExample.Tests
{
    [TestClass]
    public class DataRepositoryTests
    {
        private readonly string _vin = "VHQ3131312";
        private readonly string _sproc = "GetCarProcedure";

        private Mock<ISqlDbCoordinator> _coordinator = new Mock<ISqlDbCoordinator>();

        [TestMethod]
        public void WhenValidVinIsSentToDataLayer_ThenDataLayerReturnsWhiteCarColorAnd4Wheels()
        {
            var car = new Car() { Color = "White", NumberOfWheels = 4, VIN = _vin };
            var myCommand = new SqlCommand();

            var myReader = new Mock<DbDataReader>();
            myReader.Setup(reader => reader.FieldCount).Returns(1);
            myReader.Setup(reader => reader.GetString(0)).Returns(car.Color);
            myReader.Setup(reader => reader.GetInt32(1)).Returns(car.NumberOfWheels);
            myReader.Setup(reader => reader.GetString(2)).Returns(car.VIN);
            myReader.SetupSequence(reader => reader.Read()).Returns(true).Returns(false);

            _coordinator.Setup(cord => cord.CreateCommandAsync(_sproc)).Returns(Task.FromResult(myCommand));
            _coordinator.Setup(cord => cord.ExecuteReaderAsync(myCommand)).Returns(Task.FromResult(myReader.Object));

            var result = new DataRepository(_coordinator.Object).GetMyCar(_vin).Result;

            Assert.AreEqual(result.Color, car.Color);
            Assert.AreEqual(result.NumberOfWheels, car.NumberOfWheels);
            Assert.AreEqual(result.VIN, car.VIN);
        }
    }
}
