#### Testing Our Controller by Mocking The Data Repository
How would we test the controller endpoint below? We want to test what happens when we put our car in drive but we have a few pieces to go over first.

Notice how we are injecting the IDataRepository into our controllers constructer. This is done so we can contain the instansiation of IDataRepository in one place([Startup.cs](https://git.rockfin.com/QAPOW/CSharpUnitTestExamples/blob/master/AspNetCore/AspNetCoreExample/Startup.cs))
and it also makes testing much easier as we can pass our Moq data layer through.
```C#
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
```

This class is reaching to our data layer, if left unchecked it will fail every time, we must Moq this layer in order to get our tests running in isolation.
```C#
          var car = await _carDataRepo.GetMyCar(vin);
```

```C#
        public async Task<ICar> GetMyCar(string vin)
        {
            using (var cmd = await _dbCoordinator.CreateCommandAsync("GetCarProcedure"))
            {
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.StoredProcedure;

                var parameter = cmd.CreateParameter();
                parameter.ParameterName = "@vin";
                parameter.DbType = DbType.String;

                cmd.Parameters.Add(parameter);

                using (var dr = await _dbCoordinator.ExecuteReaderAsync(cmd))
                {
                    var car = new Car();

                    while (dr.Read())
                    {
                        car.Color = dr.GetString(0);
                        car.NumberOfWheels = dr.GetInt32(1);
                        car.VIN = dr.GetString(2);
                    }

                    return car;
                }
            }
        }
```

So lets jump into creating the test:
1. We don't need to Mock the model, we just need to intialize our own test data in it
2. Here is where we really utilize Moq, we essentially set up any calls to "GetMyCar(string s)" to return a 'Task of ICar'
3. We instansiate our controller and pass the mocked data repo in and finally we call our "PutCarInDriver(string s)" to kick off the test
4. Now that our application is working in isolation, we are able to assert on the result based on our test data

```C#
        private Mock<IDataRepository> _moqDataRepository = new Mock<IDataRepository>();

        [TestMethod]
        public void GivenVinValidAndAndCarHas4Wheels_WhenCarIsPutInDrive_ReturnSuccess()
        {
            var car = new Car() { Color = "White", NumberOfWheels = 4, VIN = _vin }; //1

            _moqDataRepository.Setup(data => data.GetMyCar(_vin)).Returns(Task.FromResult((ICar)car)); //2

            var result = new CarController(_moqDataRepository.Object).PutCarInDriver(_vin).Result; //3

            Assert.AreEqual(result, "Success"); //4
        }
```

#### What's Next?
1. Write some additional tests for the controller, see if you can come up with more scenarios to max code coverage
2. So you finally have tests around your controller and even your model. But what about your data layer? If you wanna see how we can get test coverage around it then follow this link: [Data Repository Tests](https://git.rockfin.com/QAPOW/CSharpUnitTestExamples/tree/master/AspNetCore/AspNetCoreExample.Tests/DataRepository)
