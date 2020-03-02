using System.Collections.Generic;
using System.Threading.Tasks;
using EIC.Contracts.Data.Version1;
using EIC.Contracts.Logic;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using PipServices3.Rpc.Clients;

namespace EIC.Contracts.Clients.Version1
{
    public class ContractsDirectClientV1 : DirectClient<IContractsController>, IContractsClientV1
    {
        public ContractsDirectClientV1() : base()
        {
            _dependencyResolver.Put("controller", new Descriptor("eic-contracts", "controller", "*", "*", "1.0"));
        }

        public async Task<DataPage<ContractV1>> GetContractsAsync(
            string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            using (Instrument(correlationId, "contracts.get_contracts"))
            {
                return await _controller.GetContractsAsync(correlationId, filter, paging, sort);
            }
        }

        public async Task<ContractV1> GetContractByIdAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "contracts.get_contract_by_id"))
            {
                return await _controller.GetContractByIdAsync(correlationId, id);
            }
        }

        public async Task<ContractV1> CreateContractAsync(string correlationId, ContractV1 contract)
        {
            using (Instrument(correlationId, "contracts.create_contract"))
            {
                return await _controller.CreateContractAsync(correlationId, contract);
            }
        }

        public async Task<ContractV1> UpdateContractAsync(string correlationId, ContractV1 contract)
        {
            using (Instrument(correlationId, "contracts.update_contract"))
            {
                return await _controller.UpdateContractAsync(correlationId, contract);
            }
        }

        public async Task<ContractV1> DeleteContractByIdAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "contracts.delete_contract_by_id"))
            {
                return await _controller.DeleteContractByIdAsync(correlationId, id);
            }
        }

        public async Task<List<CalendarRow>> CalculateCalendarAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "contracts.calculate_calendar"))
            {
                return await _controller.CalculateCalendarAsync(correlationId, id);
            }
        }
    }
}
