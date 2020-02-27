using EIC.Contracts.Data.Version1;
using PipServices3.Commons.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EIC.Contracts.Logic
{
    public interface IContractsController
    {
        Task<DataPage<ContractV1>> GetContractsAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort);
        Task<ContractV1> GetContractByIdAsync(string correlationId, string id);
        Task<ContractV1> CreateContractAsync(string correlationId, ContractV1 contract);
        Task<ContractV1> UpdateContractAsync(string correlationId, ContractV1 contract);
        Task<ContractV1> DeleteContractByIdAsync(string correlationId, string id);
        Task<List<CalendarRow>> CalculateCalendarAsync(string correlationId, string id);
    }
}
