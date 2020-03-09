using Amazon.DynamoDBv2.DataModel;
using POC_Hangfire.Models;
using System.Threading.Tasks;

namespace POC_Hangfire.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IDynamoDBContext _context;

        public CarRepository(IDynamoDBContext context)
            => _context = context;

        public async Task<Car> GetAsync(int id)
        {
            return await _context.LoadAsync<Car>(id);
        }

        public async Task SaveAsync(Car car)
        {
            await _context.SaveAsync(car);
        }
    }
}
