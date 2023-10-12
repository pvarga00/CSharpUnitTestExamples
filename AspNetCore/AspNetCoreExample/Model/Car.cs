using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreExample.Model.Interfaces;

namespace AspNetCoreExample.Model
{
    public class Car : ICar
    {
        public string Color { get; set; }
        public int NumberOfWheels { get; set; }
        public string VIN { get; set; }

        public string Drive()
        {
            return NumberOfWheels == 4 && !string.IsNullOrWhiteSpace(VIN) ? "SUCCESS" : "FAILURE";
        }
    }
}
