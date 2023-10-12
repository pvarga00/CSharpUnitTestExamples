using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreExample.Model.Interfaces
{
    public interface ICar : IVehicle
    {
        string Color { get; set; }
        int NumberOfWheels { get; set; }
        string VIN { get; set; }
    }
}
