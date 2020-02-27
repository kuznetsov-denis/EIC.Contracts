using EIC.Contracts.Data.Version1;
using EIC.Contracts.Persistence.MongoDb;
using PipServices3.Commons.Data;
using PipServices3.Commons.Data.Mapper;
using PipServices3.MongoDb.Persistence;
using System.Threading.Tasks;

namespace EIC.Contracts.Persistence
{
	public class ContractsMongoDbPersistence : IdentifiableMongoDbPersistence<ContractMongoDbSchema, string>, IContractsPersistence
	{
		public ContractsMongoDbPersistence()
			: base("contracts")
		{
		}

		public async Task<DataPage<ContractV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
		{
			var result = await base.GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
			var data = result.Data.ConvertAll(ToPublic);

			return new DataPage<ContractV1>
			{
			    Data = data,
			    Total = result.Total
			};
		}

		public async Task<ContractV1> GetByIdAsync(string correlationId, string id)
		{
			var result = await GetOneByIdAsync(correlationId, id);

			return ToPublic(result);
		}

		public async Task<ContractV1> CreateAsync(string correlationId, ContractV1 contract)
		{
			var result = await CreateAsync(correlationId, FromPublic(contract));

			return ToPublic(result);
		}

		public async Task<ContractV1> UpdateAsync(string correlationId, ContractV1 contract)
		{
			var result = await UpdateAsync(correlationId, FromPublic(contract));

			return ToPublic(result);
		}

		public new async Task<ContractV1> DeleteByIdAsync(string correlationId, string id)
		{
			var result = await base.DeleteByIdAsync(correlationId, id);

			return ToPublic(result);
		}

		private static ContractV1 ToPublic(ContractMongoDbSchema value)
		{
			if (value == null) return null;

			ContractV1 contract = ObjectMapper.MapTo<ContractV1>(value);
			contract.Customer.Id = value.Id;

			return contract;
		}

		private static ContractMongoDbSchema FromPublic(ContractV1 value)
		{
			ContractMongoDbSchema contract = ObjectMapper.MapTo<ContractMongoDbSchema>(value);

			contract.CustomerId = value.Customer?.Id;

			return contract;
		}
	}
}
