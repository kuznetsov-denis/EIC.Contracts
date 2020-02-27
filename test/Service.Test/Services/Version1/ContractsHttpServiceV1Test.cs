using EIC.Contracts.Data.Version1;
using EIC.Contracts.Logic;
using EIC.Contracts.Persistence;
using EIC.Customers.Clients.Version1;
using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EIC.Contracts.Services.Version1
{
    public class ContractsHttpServiceV1Test
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
            Rate = 5,
            Customer = new CustomerV1
            {
                FirstName = "Dustin",
                MiddleName = "J.",
                LastName = "Groom",
                Birthdate = new DateTime(1980, 4, 8),
                Gender = CustomerGenderV1.Male,
                Itin = "098190283",
                MobilePhone = "+7809309424749",
                Passport = "TN 671232"
            }
        };
        private ContractV1 CONTRACT2 = new ContractV1
        {
            Id = "2",
            Amount = 150000,
            Currency = "UAH",
            DateFrom = new DateTime(2005, 07, 05),
            DateInto = new DateTime(2015, 03, 01),
            DateOrg = new DateTime(2005, 06, 25),
            Number = "N2015/07-0001",
            Rate = 25,
            Customer = new CustomerV1
            {
                FirstName = "Pamela",
                MiddleName = "A.",
                LastName = "Woods",
                Birthdate = new DateTime(1941, 12, 9),
                Gender = CustomerGenderV1.Female,
                Itin = "909089012",
                MobilePhone = "077-7871-7461",
                Passport = "BK671232"
            }
        };

        private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", "3000"
        );

        private ContractsMemoryPersistence _persistence;
        private ContractsController _controller;
        private ContractsHttpServiceV1 _service;

        public ContractsHttpServiceV1Test()
        {
            _persistence = new ContractsMemoryPersistence();
            _controller = new ContractsController();
            _service = new ContractsHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("eic-contracts", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("eic-contracts", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("eic-contracts", "service", "http", "default", "1.0"), _service,
                new Descriptor("eic-customers", "client", "null", "default", "1.0"), new MockCustomersClientV1() //new CustomersNullClientV1()
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            //_service.OpenAsync(null).Wait();
            // Todo: This is defect! Open shall not block the tread
            Task.Run(() => _service.OpenAsync(null));
            Thread.Sleep(1000); // Just let service a sec to be initialized
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create the first contract
            var contract = await Invoke<ContractV1>("create_contract", new { contract = CONTRACT1 });

            AssertContracts(CONTRACT1, contract);

            // Create the second contract
            contract = await Invoke<ContractV1>("create_contract", new { contract = CONTRACT2 });

            AssertContracts(CONTRACT2, contract);

            // Get all contracts
            var page = await Invoke<DataPage<ContractV1>>(
                "get_contracts",
                new
                {
                    filter = new FilterParams(),
                    paging = new PagingParams()
                }
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var contract1 = page.Data[0];

            // Update the contract
            contract1.Number = "ABC";

            contract = await Invoke<ContractV1>("update_contract", new { contract = contract1 });

            Assert.NotNull(contract);
            Assert.Equal(contract1.Id, contract.Id);
            Assert.Equal("ABC", contract.Number);

            // Delete the contract
            contract = await Invoke<ContractV1>("delete_contract_by_id", new { contract_id = contract1.Id });

            Assert.NotNull(contract);
            Assert.Equal(contract1.Id, contract.Id);

            // Try to get deleted contract
            contract = await Invoke<ContractV1>("get_contract_by_id", new { contract_id = contract1.Id });

            Assert.Null(contract);
        }

        [Fact]
        public async Task TestCalculateCalendarAsync()
        {
            // Create the first contract
            var contract = await Invoke<ContractV1>("create_contract", new { contract = CONTRACT1 });
            AssertContracts(CONTRACT1, contract);

            var calendar = await Invoke<List<CalendarRow>>("calculate_calendar", new { contract_id = contract.Id });
            AssertCalendar(calendar, 63, contract.Amount, 1593.55);

            // Create the second contract
            contract = await Invoke<ContractV1>("create_contract", new { contract = CONTRACT2 });
            AssertContracts(CONTRACT2, contract);

            calendar = await Invoke<List<CalendarRow>>("calculate_calendar", new { contract_id = contract.Id });
            AssertCalendar(calendar, 115, contract.Amount, 180922.58);
        }

        private static async Task<T> Invoke<T>(string route, dynamic request)
        {
            using (var httpClient = new HttpClient())
            {
                var requestValue = JsonConverter.ToJson(request);
                using (var content = new StringContent(requestValue, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync("http://localhost:3000/v1/contracts/" + route, content);
                    var responseValue = response.Content.ReadAsStringAsync().Result;
                    return JsonConverter.FromJson<T>(responseValue);
                }
            }
        }

        private static void AssertCalendar(List<CalendarRow> calendar, int rowCountExpected, double bodySumExpected, double percentsSumExpected)
        {
            Assert.NotNull(calendar);
            Assert.NotEmpty(calendar);

            Assert.Equal(rowCountExpected, calendar.Count);
            Assert.Equal(bodySumExpected, calendar.Sum(x => x.Body), 2);
            Assert.Equal(percentsSumExpected, calendar.Sum(x => x.Percens), 2);
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
