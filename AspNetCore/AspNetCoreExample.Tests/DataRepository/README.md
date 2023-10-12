### Mock Database Methods Using A Wrapper And Test Data Repository
In order to test our data repository, we'll have to address the 3rd party library we are using to access our database.
- System.Data

Here is the method we will be testing in the DataRepo.
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

We can't get rid of this code but we can wrap it in our own database coordinator and use that wrapper to communicate with our database.

Notice how we used the wrapper as a pass through and since our wrapper has an interface, we can mock it! This essentially decouples our repo from the database. Just like in the car controller example we must register our new interface in the [StartUp.cs](https://git.rockfin.com/QAPOW/CSharpUnitTestExamples/blob/master/AspNetCore/AspNetCoreExample/Startup.cs) via Dependancy Injection. That way we can inject our DbCoordinator through our Data Repos constructor(See code below)

```C#
        private ISqlDbCoordinator _dbCoordinator { get; }

        public DataRepository(ISqlDbCoordinator dbCoordinator)
        {
            _dbCoordinator = dbCoordinator;
        }
```

#### Test Creation

Let's break down the components of the test:
1. We don't need to Mock the model, we just need to intialize our own test data in it
2. Here is where we utilize Moq, we need to mock any calls to the database we put in our DbCoordinator wrapper: CreateCommandAsync && ExecuteReaderAsync
	a. On top of mocking the calls we must mock the return data too
3. We instansiate our data repositroy and pass the mocked db coordinator in and finally we call our "GetMyCar(string s)" to kick off the test
4. Now that our application is working in isolation, we are able to assert on the result based on our test data

```C#
        private Mock<ISqlDbCoordinator> _coordinator = new Mock<ISqlDbCoordinator>(); #2

        [TestMethod]
        public void WhenValidVinIsSentToDataLayer_ThenDataLayerReturnsWhiteCarColorAnd4Wheels()
        {
            var car = new Car() { Color = "White", NumberOfWheels = 4, VIN = _vin }; #1
            var myCommand = new SqlCommand(); #2a

            var myReader = new Mock<DbDataReader>(); #2a
            myReader.Setup(reader => reader.FieldCount).Returns(1); #2a
            myReader.Setup(reader => reader.GetString(0)).Returns(car.Color); #2a
            myReader.Setup(reader => reader.GetInt32(1)).Returns(car.NumberOfWheels); #2a
            myReader.Setup(reader => reader.GetString(2)).Returns(car.VIN); #2a
            myReader.SetupSequence(reader => reader.Read()).Returns(true).Returns(false); #2a

            _coordinator.Setup(cord => cord.CreateCommandAsync(_sproc)).Returns(Task.FromResult(myCommand)); #2
            _coordinator.Setup(cord => cord.ExecuteReaderAsync(myCommand)).Returns(Task.FromResult(myReader.Object)); #2

            var result = new DataRepository(_coordinator.Object).GetMyCar(_vin).Result; #3

            Assert.AreEqual(result.Color, car.Color); #4
            Assert.AreEqual(result.NumberOfWheels, car.NumberOfWheels); #4
            Assert.AreEqual(result.VIN, car.VIN); #4
        }
```

### What's Next!
1. If you also looked at the test for the controller and wrote some additional test scenarios then you can do the same for the data repo. Write some additional tests(Negative cases) and 
see what you can come up with.
2. This is the end of the guide, congrats on finishing! For any additional help please reach out to It Team QAPOW via [Email](ITTeamQAPOW@Quickenloans.com) or [Teams Room](https://teams.microsoft.com/l/team/19%3a00f5199ea282405a877fd7915e45f3bc%40thread.skype/conversations?groupId=5a5da90f-7f86-49da-87de-d84eba0474f8&tenantId=e58c8e81-abd8-48a8-929d-eb67611b83bd)