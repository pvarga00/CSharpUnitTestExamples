using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreExample.Repository;
using System.Diagnostics.CodeAnalysis;

namespace AspNetCore.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("Car/")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private IDataRepository _carDataRepo;

        public CarController(IDataRepository carDataRepository)
        {
            _carDataRepo = carDataRepository;
        }

        // GET api/values
        [HttpPost("PutInDrive/{vin}")]
        public async Task<string> PutCarInDriver(string vin)
        {
            var car = await _carDataRepo.GetMyCar(vin);

            return car.Drive();
        }

        // GET api/values
        [HttpGet("{vin}")]
        public void GetCarColor(string vin)
        {
        }
    }
}
