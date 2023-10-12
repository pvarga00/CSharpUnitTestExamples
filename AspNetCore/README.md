# Asp.Net Core Unit Test Examples
This is a project built using Asp.Net Core 2.2, the project has 1 controller that pulls information about a car. If you run the application you will quickly figure out that the end points are broken since they cannot connect to the Car Database. So in order to test our application and ensure it works, we will employ Moq framework in order to test the application.


#### Addressing Our Layers
In our application we have 3 layers to address when testing the controller.
- **[Controller](https://git.rockfin.com/QAPOW/CSharpUnitTestExamples/blob/master/AspNetCore/AspNetCoreExample/Controllers/CarController.cs)**: The layer we are testing, also the catalyst of the process
- **[Data Repository](https://git.rockfin.com/QAPOW/CSharpUnitTestExamples/blob/master/AspNetCore/AspNetCoreExample/Repository/DataRepository.cs)**: The layer responsible for getting data from our database
- **[Model](https://git.rockfin.com/QAPOW/CSharpUnitTestExamples/blob/master/AspNetCore/AspNetCoreExample/Model/Car.cs)**: Used to collect data from our data repository to interact with our controller 

#### Test Scenarios Links
Below are links to each testing scenario built into this repo, I recommend starting with the controller and then testing the data respository.

1. [Controller Test (Easiest Scenario)](https://git.rockfin.com/QAPOW/CSharpUnitTestExamples/tree/master/AspNetCore/AspNetCoreExample.Tests/CarController)
2. [Data Respository (Harder Scenario)](https://git.rockfin.com/QAPOW/CSharpUnitTestExamples/tree/master/AspNetCore/AspNetCoreExample.Tests/DataRepository)
