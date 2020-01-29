using POC_Hangfire.Models;
using System.Threading.Tasks;

namespace POC_Hangfire.Repositories
{
    public interface ICarRepository
    {
        Task<Car> GetAsync(string id);
        Task SaveAsync(Car car);
    }
}
