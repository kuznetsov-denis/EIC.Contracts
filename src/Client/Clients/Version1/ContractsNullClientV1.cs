using System.Collections.Generic;
using System.Threading.Tasks;
using EIC.Contracts.Data.Version1;
using PipServices3.Commons.Data;

namespace EIC.Contracts.Clients.Version1
{
    public class ContractsNullClientV1 : IContractsClientV1
    {
        public async Task<DataPage<ContractV1>> GetContractsAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            return await Task.FromResult(new DataPage<ContractV1>());
        }

        public async Task<ContractV1> GetContractByIdAsync(string correlationId, string id)
        {
            return await Task.FromResult(new ContractV1());
        }

        public async Task<ContractV1> CreateContractAsync(string correlationId, ContractV1 beacon)
        {
            return await Task.FromResult(new ContractV1());
        }

        public async Task<ContractV1> UpdateContractAsync(string correlationId, ContractV1 beacon)
        {
            return await Task.FromResult(new ContractV1());
        }

        public async Task<ContractV1> DeleteContractByIdAsync(string correlationId, string id)
        {
            return await Task.FromResult(new ContractV1());
        }

        public async Task<List<CalendarRow>> CalculateCalendarAsync(string correlationId, string id)
        {
            return await Task.FromResult(new List<CalendarRow>());
        }
    }
}
