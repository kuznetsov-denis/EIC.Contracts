using EIC.Contracts.Data.Version1;
using PipServices3.Commons.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EIC.Contracts.Clients.Version1
{
    public class ContractsClientV1Fixture
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

        private IContractsClientV1 _client;

        public ContractsClientV1Fixture(IContractsClientV1 client)
        {
            _client = client;
        }

        public async Task TestCrudOperationsAsync()
        {
            // Create the first contract
            var contract = await _client.CreateContractAsync(null, CONTRACT1);

            AssertContracts(CONTRACT1, contract);

            // Create the second contract
            contract = await _client.CreateContractAsync(null, CONTRACT2);

            AssertContracts(CONTRACT2, contract);

            // Get all contracts
            var page = await _client.GetContractsAsync(
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

            contract = await _client.UpdateContractAsync(null, contract1);

            Assert.NotNull(contract);
            Assert.Equal(contract1.Id, contract.Id);
            Assert.Equal("ABC", contract.Number);

            // Delete the contract
            contract = await _client.DeleteContractByIdAsync(null, contract1.Id);

            Assert.NotNull(contract);
            Assert.Equal(contract1.Id, contract.Id);

            // Try to get deleted contract
            contract = await _client.GetContractByIdAsync(null, contract1.Id);

            Assert.Null(contract);

            // Clean up for the second test
            await _client.DeleteContractByIdAsync(null, CONTRACT2.Id);
        }

        public async Task TestCalculateCalendarAsync()
        {
            // Create the first contract
            var contract = await _client.CreateContractAsync(null, CONTRACT1);
            AssertContracts(CONTRACT1, contract);

            var calendar = await _client.CalculateCalendarAsync(null, contract.Id);
            AssertCalendar(calendar, 63, contract.Amount, 1593.55);

            // Create the second contract
            contract = await _client.CreateContractAsync(null, CONTRACT2);
            AssertContracts(CONTRACT2, contract);

            calendar = await _client.CalculateCalendarAsync(null, contract.Id);
            AssertCalendar(calendar, 115, contract.Amount, 180922.58);

            // Clean up for the second test
            await _client.DeleteContractByIdAsync(null, CONTRACT1.Id);
            await _client.DeleteContractByIdAsync(null, CONTRACT2.Id);
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

            AssertCustomers(etalon.Customer, contract.Customer);
        }

        private static void AssertCustomers(CustomerV1 etalon, CustomerV1 customer)
        {
            Assert.NotNull(customer);
            Assert.Equal(etalon.FirstName, customer.FirstName);
            Assert.Equal(etalon.LastName, customer.LastName);
            Assert.Equal(etalon.MiddleName, customer.MiddleName);
            Assert.Equal(etalon.MobilePhone, customer.MobilePhone);
            Assert.Equal(etalon.Gender, customer.Gender);
            Assert.Equal(etalon.Itin, customer.Itin);
            Assert.Equal(etalon.Passport, customer.Passport);
            Assert.Equal(etalon.Birthdate, customer.Birthdate);
        }
    }
}
