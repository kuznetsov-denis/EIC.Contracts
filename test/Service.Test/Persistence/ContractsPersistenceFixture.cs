using EIC.Contracts.Data.Version1;
using PipServices3.Commons.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EIC.Contracts.Persistence
{
    public class ContractsPersistenceFixture
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
        private ContractV1 CONTRACT3 = new ContractV1
        {
            Id = "3",
            Amount = 100000,
            Currency = "USD",
            DateFrom = new DateTime(2015, 07, 05),
            DateInto = new DateTime(2035, 03, 01),
            DateOrg = new DateTime(2015, 07, 05),
            Number = "N2005/07-0001",
            Rate = 3,
            Customer = new CustomerV1
            {
                FirstName = "Evelyn",
                MiddleName = "G.",
                LastName = "Johnson",
                Birthdate = new DateTime(1981, 12, 11),
                Gender = CustomerGenderV1.Female,
                Itin = "07812908983",
                MobilePhone = "079-2959-4308",
                Passport = "NN98712232"
            }
        };

        private IContractsPersistence _persistence;

        public ContractsPersistenceFixture(IContractsPersistence persistence)
        {
            _persistence = persistence;
        }

        private async Task TestCreateContractsAsync()
        {
            // Create the first contract
            var contract = await _persistence.CreateAsync(null, CONTRACT1);

            AssertContracts(CONTRACT1, contract);

            // Create the second contract
            contract = await _persistence.CreateAsync(null, CONTRACT2);

            AssertContracts(CONTRACT2, contract);

            // Create the third contract
            contract = await _persistence.CreateAsync(null, CONTRACT3);

            AssertContracts(CONTRACT3, contract);
        }

        public async Task TestCrudOperationsAsync()
        {
            // Create items
            await TestCreateContractsAsync();

            // Get all contracts
            var page = await _persistence.GetPageByFilterAsync(
                null,
                new FilterParams(),
                new PagingParams(),
                new SortParams()
            );

            Assert.NotNull(page);
            Assert.Equal(3, page.Data.Count);

            var contract1 = page.Data[0];

            // Update the contract
            contract1.Number = "ABC";

            var contract = await _persistence.UpdateAsync(null, contract1);

            Assert.NotNull(contract);
            Assert.Equal(contract1.Id, contract.Id);
            Assert.Equal("ABC", contract.Number);

            // Delete the contract
            contract = await _persistence.DeleteByIdAsync(null, contract1.Id);

            Assert.NotNull(contract);
            Assert.Equal(contract1.Id, contract.Id);

            // Try to get deleted contract
            contract = await _persistence.GetByIdAsync(null, contract1.Id);

            Assert.Null(contract);
        }

        public async Task TestGetWithFiltersAsync()
        {
            // Create items
            await TestCreateContractsAsync();

            // Filter by id
            var page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "id", "1"
                ),
                new PagingParams(),
                new SortParams()
            );

            Assert.Single(page.Data);

            // Filter by number
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "number", "N2015/07-0001"
                ),
                new PagingParams(),
                new SortParams()
            );

            Assert.Single(page.Data);

            // Filter by currency
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "currency", "USD"
                ),
                new PagingParams(),
                new SortParams()
            );

            Assert.Equal(2, page.Data.Count);

            // Filter by org date
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "date_org", "2015-07-05"
                ),
                new PagingParams(),
                new SortParams()
            );

            Assert.Single(page.Data);
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
