using EIC.Contracts.Data.Version1;
using PipServices3.Commons.Data;
using System.Threading.Tasks;

namespace EIC.Contracts.Persistence
{
	public interface IContractsPersistence
	{
		 Task<DataPage<ContractV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort);
		 Task<ContractV1> GetByIdAsync(string correlationId, string id);
		 Task<ContractV1> CreateAsync(string correlationId, ContractV1 contract);
		 Task<ContractV1> UpdateAsync(string correlationId, ContractV1 contract);
		 Task<ContractV1> DeleteByIdAsync(string correlationId, string id);
	}
}
