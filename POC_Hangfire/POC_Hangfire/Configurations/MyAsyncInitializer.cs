using AspNetCore.AsyncInitialization;
using POC_Hangfire.Repositories;
using System.Threading.Tasks;

namespace POC_Hangfire.Configurations
{
    public class MyAsyncInitializer : IAsyncInitializer
    {
        private readonly IRegisterTables _registerTables;

        public MyAsyncInitializer(IRegisterTables registerTables) => _registerTables = registerTables;

        public async Task InitializeAsync() => await _registerTables.RegisterAsync();
    }
}
