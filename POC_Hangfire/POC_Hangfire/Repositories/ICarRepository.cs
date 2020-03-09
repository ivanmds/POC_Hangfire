using POC_Hangfire.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POC_Hangfire.Repositories
{
    public interface ICarRepository
    {
        Task<Car> GetAsync(int id);
        Task SaveAsync(Car car);
    }
}
