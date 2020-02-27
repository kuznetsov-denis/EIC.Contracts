using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PipServices3.Commons.Data;
using PipServices3.Data.Persistence;
using EIC.Contracts.Data.Version1;
using MongoDB.Driver;

namespace EIC.Contracts.Persistence
{
    public class ContractsMemoryPersistence : IdentifiableMemoryPersistence<ContractV1, string>, IContractsPersistence
    {
        public ContractsMemoryPersistence()
        {
            _maxPageSize = 1000;
        }

		private List<Func<ContractV1, bool>> ComposeFilter(FilterParams filterParams)
		{
			filterParams = filterParams ?? new FilterParams();

			var builder = Builders<ContractV1>.Filter;
			var filter = builder.Empty;

			var id = filterParams.GetAsNullableString("id");
			var number = filterParams.GetAsNullableString("number");
			var date_org = filterParams.GetAsNullableDateTime("date_org");
			var date_from = filterParams.GetAsNullableDateTime("date_from");
			var date_into = filterParams.GetAsNullableDateTime("date_into");
			var amount = filterParams.GetAsNullableDouble("amount");
			var currency = filterParams.GetAsNullableString("currency");
			var rate = filterParams.GetAsNullableDouble("rate");

			return new List<Func<ContractV1, bool>>
			{ 
				(item) => 
				{
					if (!string.IsNullOrWhiteSpace(id) && item.Id != id) return false;
					if (!string.IsNullOrWhiteSpace(number) && item.Number != number)  return false;
					if (!string.IsNullOrWhiteSpace(currency) && item.Currency != currency)  return false;

					if (date_org.HasValue && item.DateOrg != date_org) return false;
					if (date_from.HasValue && item.DateFrom != date_from) return false;
					if (date_into.HasValue && item.DateInto != date_into) return false;

					if (amount.HasValue && item.Amount != amount) return false;
					if (rate.HasValue && item.Rate != rate) return false;

					return true;
				}
			};
		}

		public Task<DataPage<ContractV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            return base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }

		public Task<ContractV1> GetByIdAsync(string correlationId, string id)
		{
			return base.GetOneByIdAsync(correlationId, id);
		}
	}
}
