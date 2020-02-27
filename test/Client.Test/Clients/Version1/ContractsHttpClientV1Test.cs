using PipServices3.Commons.Config;
using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using Xunit;
using EIC.Contracts.Persistence;
using EIC.Contracts.Logic;
using EIC.Contracts.Services.Version1;
using EIC.Customers.Clients.Version1;

namespace EIC.Contracts.Clients.Version1
{
    public class ContractsHttpClientV1Test
    {
        private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", 8080
        );

        private ContractsMemoryPersistence _persistence;
        private ContractsController _controller;
        private ContractsHttpClientV1 _client;
        private ContractsHttpServiceV1 _service;
        private ContractsClientV1Fixture _fixture;

        public ContractsHttpClientV1Test()
        {
            _persistence = new ContractsMemoryPersistence();
            _controller = new ContractsController();
            _client = new ContractsHttpClientV1();
            _service = new ContractsHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("eic-contracts", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("eic-contracts", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("eic-contracts", "client", "http", "default", "1.0"), _client,
                new Descriptor("eic-contracts", "service", "http", "default", "1.0"), _service,
                new Descriptor("eic-customers", "client", "null", "default", "1.0"), new MockCustomersClientV1() // new CustomersNullClientV1()
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            _client.Configure(HttpConfig);
            _client.SetReferences(references);

            _fixture = new ContractsClientV1Fixture(_client);

            _service.OpenAsync(null).Wait();
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
