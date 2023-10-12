using AspNetCoreExample.Model;
using AspNetCoreExample.Model.Interfaces;
using System.Data;
using System.Threading.Tasks;

namespace AspNetCoreExample.Repository
{
    public class DataRepository : IDataRepository
    {
        private ISqlDbCoordinator _dbCoordinator { get; }

        public DataRepository(ISqlDbCoordinator dbCoordinator)
        {
            _dbCoordinator = dbCoordinator;
        }

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
    }
}
