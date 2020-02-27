using PipServices3.Commons.Commands;
using PipServices3.Commons.Config;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using EIC.Contracts.Data.Version1;
using EIC.Contracts.Persistence;
using System;
using System.Collections.Generic;
using EIC.Customers.Clients.Version1;
using PipServices3.Components.Logic;
using PipServices3.Commons.Data.Mapper;

namespace EIC.Contracts.Logic
{
    public class ContractsController : AbstractController, IContractsController, ICommandable
	{
		private IContractsPersistence _persistence;
		private ContractsCommandSet _commandSet;

        private ICustomersClientV1 _customersClient;
        private CustomersConnector _customersConnector;

        public override string Component => "Contracts";

        public ContractsController()
		{
            _dependencyResolver.Put("customers", new Descriptor("eic-customers", "client", "*", "*", "1.0"));
        }

		public override void Configure(ConfigParams config)
		{
            base.Configure(config);

            _dependencyResolver.Configure(config);
        }

        public override void SetReferences(IReferences references)
		{
            base.SetReferences(references);

            _dependencyResolver.SetReferences(references);

            _persistence = references.GetOneRequired<IContractsPersistence>(
				new Descriptor("eic-contracts", "persistence", "*", "*", "1.0")
			);

            _customersClient = _dependencyResolver.GetOneRequired<ICustomersClientV1>("customers");
            _customersConnector = new CustomersConnector(_customersClient);
        }

        public CommandSet GetCommandSet()
		{
			if (_commandSet == null)
				_commandSet = new ContractsCommandSet(this);
			return _commandSet;
		}

		public async Task<ContractV1> CreateContractAsync(string correlationId, ContractV1 contract)
		{
            var newCustomer = await _customersConnector.CreateCustomerAsync(correlationId, ToPublic(contract.Customer));
            
            contract.Customer.Id = newCustomer.Id;

            return await _persistence.CreateAsync(correlationId, contract);
        }

		public async Task<ContractV1> UpdateContractAsync(string correlationId, ContractV1 contract)
		{
            await _customersConnector.UpdateCustomerAsync(correlationId, ToPublic(contract.Customer));

            return await _persistence.UpdateAsync(correlationId, contract);
        }

		public async Task<ContractV1> DeleteContractByIdAsync(string correlationId, string id)
		{
            var contract = await _persistence.GetByIdAsync(correlationId, id);
            if (contract != null)
            {
                await _customersConnector.DeleteCustomerByIdAsync(correlationId, contract.Customer.Id);
            }

            return await _persistence.DeleteByIdAsync(correlationId, id);
        }

		public async Task<ContractV1> GetContractByIdAsync(string correlationId, string id)
		{
            var contract = await _persistence.GetByIdAsync(correlationId, id);

            if (contract != null)
            {
                contract.Customer = ToInternal(await _customersConnector.GetByIdAsync(correlationId, contract.Customer.Id));
            }

            return contract;
        }

		public async Task<DataPage<ContractV1>> GetContractsAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
		{
            DataPage<ContractV1> dataPage = await _persistence.GetPageByFilterAsync(correlationId, filter, paging, sort);

            foreach (var contract in dataPage.Data)
            {
                contract.Customer = ToInternal(await _customersConnector.GetByIdAsync(correlationId, contract.Customer.Id));
            }

            return dataPage;
        }

        public async Task<List<CalendarRow>> CalculateCalendarAsync(string correlationId, string id)
        {
            var contract = await _persistence.GetByIdAsync(correlationId, id);

            return contract == null 
                ? new List<CalendarRow>()
                : CalcPaymentCalendar(contract.Rate, contract.Amount, contract.DateFrom, contract.DateInto);
        }
        
        /// <summary>
        /// Calculation of the payment calendar by equal shares method
        /// </summary>
        /// <param name="rate">annual interest rate</param>
        /// <param name="amount">amount of debt</param>
        /// <param name="dateForm">contract start date</param>
        /// <param name="dateInto">contract expiration date</param>
        /// <returns>List of payment calendar rows</returns>
        private static List<CalendarRow> CalcPaymentCalendar(double rate, double amount, DateTime dateForm, DateTime dateInto)
        {
            List<DateTime> dates = GetMonthlyDates(dateForm, dateInto);

            List<CalendarRow> paymentCalendar = new List<CalendarRow>(dates.Count);

            if (dates.Count > 0)
            {
                decimal calcRate = Convert.ToDecimal(rate) / 100M;
                decimal restBody = Convert.ToDecimal(amount);

                decimal monthlyBody = Math.Round(restBody / dates.Count, 2);

                for (int i = 0; i < dates.Count; i++)
                {
                    DateTime periodFrom = i == 0 ? dateForm : dates[i - 1];
                    DateTime periodInto = dates[i].AddDays(-1);

                    int periodDays = Convert.ToInt32((periodInto - periodFrom).TotalDays) + 1;
                    int yearDays = DateTime.IsLeapYear(periodInto.Year) ? 366 : 365;

                    decimal refundBody = i < dates.Count - 1 ? monthlyBody : restBody;
                    decimal refundPercents = Math.Round(restBody * calcRate * periodDays / yearDays, 2);

                    restBody -= refundBody;

                    paymentCalendar.Add(new CalendarRow
                    {
                        Date = dates[i],
                        Days = periodDays,
                        Body = (double)refundBody,
                        Percens = (double)refundPercents,
                        Rest = (double)restBody
                    });
                }
            }

            return paymentCalendar;
        }

        private static List<DateTime> GetMonthlyDates(DateTime dateForm, DateTime dateInto)
        {
            var dates = new List<DateTime>();

            DateTime from = dateForm.Date;
            DateTime into = dateInto.Date;

            DateTime current = from.AddDays(1 - from.Day).AddMonths(1); ;

            while (current < into)
            {
                dates.Add(current);
                current = current.AddMonths(1);
            }

            if (current != into)
            {
                dates.Add(into);
            }

            return dates;
        }

        public Customers.Data.Version1.CustomerV1 ToPublic(Contracts.Data.Version1.CustomerV1 customer)
        {
            return customer == null ? null : ObjectMapper.MapTo<Customers.Data.Version1.CustomerV1>(customer);
        }

        public Contracts.Data.Version1.CustomerV1 ToInternal(Customers.Data.Version1.CustomerV1 customer)
        {
            return customer == null ? null : ObjectMapper.MapTo<Contracts.Data.Version1.CustomerV1>(customer);
        }
    }
}
