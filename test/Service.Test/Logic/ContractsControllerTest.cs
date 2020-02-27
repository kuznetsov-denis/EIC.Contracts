using EIC.Contracts.Data.Version1;
using EIC.Contracts.Persistence;
using EIC.Customers.Clients.Version1;
using PipServices3.Commons.Config;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EIC.Contracts.Logic
{
    public class ContractsControllerTest: IDisposable
    {
        private ContractV1 CONTRACT1 = new ContractV1
        {
            Id = "1",
            Amount = 12000,
            Currency = "USD",
            DateFrom = new DateTime(2010, 04, 05),
            DateInto = new DateTime(2015, 06, 05),
            DateOrg = new DateTime(2010, 04, 01),
            Number = "N2010/04-0001",
            Rate = 5
        };
        private ContractV1 CONTRACT2 = new ContractV1
        {
            Id = "2",
            Amount = 150000,
            Currency = "UAH",
            DateFrom = new DateTime(2005, 07, 05),
            DateInto = new DateTime(2015, 03, 01),
            DateOrg = new DateTime(2005, 06, 25),
            Number = "N2005/07-0001",
            Rate = 25
        };

        private ContractsController _controller;
        private ContractsMemoryPersistence _persistence;

        public ContractsControllerTest()
        {
            _persistence = new ContractsMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _controller = new ContractsController();

            var references = References.FromTuples(
                new Descriptor("eic-contracts", "persistence", "memory", "*", "1.0"), _persistence,
                new Descriptor("eic-contracts", "controller", "default", "*", "1.0"), _controller,
                new Descriptor("eic-customers", "client", "null", "default", "1.0"), new CustomersNullClientV1()
            );

            _controller.SetReferences(references);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create the first contract
            var contract = await _controller.CreateContractAsync(null, CONTRACT1);

            AssertContracts(CONTRACT1, contract);

            // Create the second contract
            contract = await _controller.CreateContractAsync(null, CONTRACT2);

            AssertContracts(CONTRACT2, contract);

            // Get all contracts
            var page = await _controller.GetContractsAsync(
                null,
                new FilterParams(),
                new PagingParams(),
                new SortParams()
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var contract1 = page.Data[0];

            // Update the contract
            contract1.Number = "ABC";

            contract = await _controller.UpdateContractAsync(null, contract1);

            Assert.NotNull(contract);
            Assert.Equal(contract1.Id, contract.Id);
            Assert.Equal("ABC", contract.Number);

            // Delete the contract
            contract = await _controller.DeleteContractByIdAsync(null, contract1.Id);

            Assert.NotNull(contract);
            Assert.Equal(contract1.Id, contract.Id);

            // Try to get deleted contract
            contract = await _controller.GetContractByIdAsync(null, contract1.Id);

            Assert.Null(contract);
        }

        private static void AssertContracts(ContractV1 etalon, ContractV1 contract)
        {
            Assert.NotNull(contract);
            Assert.Equal(etalon.Amount, contract.Amount);
            Assert.Equal(etalon.Currency, contract.Currency);
            Assert.Equal(etalon.DateFrom, contract.DateFrom);
            Assert.Equal(etalon.DateInto, contract.DateInto);
            Assert.Equal(etalon.DateOrg, contract.DateOrg);
            Assert.Equal(etalon.Number, contract.Number);
            Assert.Equal(etalon.Rate, contract.Rate);
        }
    }
}
