using System;
using System.Threading.Tasks;
using PipServices3.Commons.Config;
using Xunit;

namespace EIC.Contracts.Persistence
{
    public class ContractsMemoryPersistenceTest : IDisposable
    {
        public ContractsMemoryPersistence _persistence;
        public ContractsPersistenceFixture _fixture;

        public ContractsMemoryPersistenceTest()
        {
            _persistence = new ContractsMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _fixture = new ContractsPersistenceFixture(_persistence);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();    
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }

        [Fact]
        public async Task TestGetWithFiltersAsync()
        {
            await _fixture.TestGetWithFiltersAsync();
        }

    }
}
