using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using Xunit;
using EIC.Contracts.Persistence;
using EIC.Contracts.Logic;
using EIC.Customers.Clients.Version1;

namespace EIC.Contracts.Clients.Version1
{
    public class ContractsDirectClientV1Test
    {
        private ContractsMemoryPersistence _persistence;
        private ContractsController _controller;
        private ContractsDirectClientV1 _client;
        private ContractsClientV1Fixture _fixture;

        public ContractsDirectClientV1Test()
        {
            _persistence = new ContractsMemoryPersistence();
            _controller = new ContractsController();
            _client = new ContractsDirectClientV1();

            IReferences references = References.FromTuples(
                new Descriptor("eic-contracts", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("eic-contracts", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("eic-contracts", "client", "direct", "default", "1.0"), _client,
                new Descriptor("eic-customers", "client", "null", "default", "1.0"), new CustomersNullClientV1()
            );

            _controller.SetReferences(references);

            _client.SetReferences(references);

            _fixture = new ContractsClientV1Fixture(_client);

            _client.OpenAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }

        [Fact]
        public async Task TestCalculateCalendarAsync()
        {
            await _fixture.TestCalculateCalendarAsync();
        }

    }
}
