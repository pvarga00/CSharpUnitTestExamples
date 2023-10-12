using AspNetCoreExample.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreExample.Repository
{
    public interface IDataRepository
    {
        Task<ICar> GetMyCar(string vin);
    }
}
