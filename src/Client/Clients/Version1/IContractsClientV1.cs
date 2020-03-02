using PipServices3.Commons.Data;
using System.Threading.Tasks;
using EIC.Contracts.Data.Version1;
using System.Collections.Generic;

namespace EIC.Contracts.Clients.Version1
{
    public interface IContractsClientV1
    {
        Task<DataPage<ContractV1>> GetContractsAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort);
        Task<ContractV1> GetContractByIdAsync(string correlationId, string id);
        Task<ContractV1> CreateContractAsync(string correlationId, ContractV1 contract);
        Task<ContractV1> UpdateContractAsync(string correlationId, ContractV1 contract);
        Task<ContractV1> DeleteContractByIdAsync(string correlationId, string id);
        Task<List<CalendarRow>> CalculateCalendarAsync(string correlationId, string id);
    }
}
