using System.Collections.Generic;
using System.Threading.Tasks;
using EIC.Contracts.Data.Version1;
using PipServices3.Commons.Data;
using PipServices3.Rpc.Clients;

namespace EIC.Contracts.Clients.Version1
{
    public class ContractsHttpClientV1 : CommandableHttpClient, IContractsClientV1
    {
        public ContractsHttpClientV1()
            : base("v1/contracts")
        { }

        public async Task<DataPage<ContractV1>> GetContractsAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            return await CallCommandAsync<DataPage<ContractV1>>(
                "get_contracts",
                correlationId,
                new
                {
                    filter = filter,
                    paging = paging,
                    sort = sort
                }
            );
        }

        public async Task<ContractV1> GetContractByIdAsync(string correlationId, string id)
        {
            return await CallCommandAsync<ContractV1>(
                "get_contract_by_id",
                correlationId,
                new
                {
                    contract_id = id
                }
            );
        }

        public async Task<ContractV1> CreateContractAsync(string correlationId, ContractV1 contract)
        {
            return await CallCommandAsync<ContractV1>(
                "create_contract",
                correlationId,
                new
                {
                    contract = contract
                }
            );
        }

        public async Task<ContractV1> UpdateContractAsync(string correlationId, ContractV1 contract)
        {
            return await CallCommandAsync<ContractV1>(
                "update_contract",
                correlationId,
                new
                {
                    contract = contract
                }
            );
        }

        public async Task<ContractV1> DeleteContractByIdAsync(string correlationId, string id)
        {
            return await CallCommandAsync<ContractV1>(
                "delete_contract_by_id",
                correlationId,
                new
                {
                    contract_id = id
                }
            );
        }

        public async Task<List<CalendarRow>> CalculateCalendarAsync(string correlationId, string id)
        {
            return await CallCommandAsync<List<CalendarRow>>(
                "calculate_calendar",
                correlationId,
                new
                {
                    contract_id = id
                }
            );
        }
    }
}
