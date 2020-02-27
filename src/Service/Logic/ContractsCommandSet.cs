using EIC.Contracts.Data.Version1;
using PipServices3.Commons.Data;
using PipServices3.Commons.Commands;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;
using System.Collections.Generic;
using System;

namespace EIC.Contracts.Logic
{
	public class ContractsCommandSet : CommandSet
	{
        private IContractsController _controller;

        public ContractsCommandSet(IContractsController controller)
        {
            _controller = controller;

            AddCommand(MakeGetContractsCommand());
            AddCommand(MakeGetContractByIdContractsCommand());
            AddCommand(MakeCreateContractCommand());
            AddCommand(MakeUpdateContractCommand());
            AddCommand(MakeDeleteContractByIdCommand());
            AddCommand(MakeCalculateCalendarCommand());
        }

        private ICommand MakeGetContractsCommand()
        {
            return new Command(
                "get_contracts",
                new ObjectSchema()
                    .WithOptionalProperty("filter", new FilterParamsSchema())
                    .WithOptionalProperty("paging", new PagingParamsSchema())
                    .WithOptionalProperty("sort", new SortParamsSchema()),
                async (correlationId, parameters) =>
                {
                    var filter = FilterParams.FromValue(parameters.Get("filter"));
                    var paging = PagingParams.FromValue(parameters.Get("paging"));
                    var sort = SortParams.FromValue(parameters.Get("sort"));
                    return await _controller.GetContractsAsync(correlationId, filter, paging, sort);
                });
        }

        private ICommand MakeGetContractByIdContractsCommand()
        {
            return new Command(
                "get_contract_by_id",
                new ObjectSchema()
                    .WithRequiredProperty("contract_id", PipServices3.Commons.Convert.TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var id = parameters.GetAsString("contract_id");
                    return await _controller.GetContractByIdAsync(correlationId, id);
                });
        }

        private ICommand MakeCreateContractCommand()
        {
            return new Command(
                "create_contract",
                new ObjectSchema()
                    .WithRequiredProperty("contract", new ContractV1Schema()),
                async (correlationId, parameters) =>
                {
                    var contract = ConvertToContract(parameters.GetAsObject("contract"));
                    return await _controller.CreateContractAsync(correlationId, contract);
                });
        }

        private ICommand MakeUpdateContractCommand()
        {
            return new Command(
               "update_contract",
               new ObjectSchema()
                    .WithRequiredProperty("contract", new ContractV1Schema()),
               async (correlationId, parameters) =>
               {
                   var contract = ConvertToContract(parameters.GetAsObject("contract"));
                   return await _controller.UpdateContractAsync(correlationId, contract);
               });
        }

        private ICommand MakeDeleteContractByIdCommand()
        {
            return new Command(
               "delete_contract_by_id",
               new ObjectSchema()
                   .WithRequiredProperty("contract_id", PipServices3.Commons.Convert.TypeCode.String),
               async (correlationId, parameters) =>
               {
                   var id = parameters.GetAsString("contract_id");
                   return await _controller.DeleteContractByIdAsync(correlationId, id);
               });
        }

        private ICommand MakeCalculateCalendarCommand()
        {
            return new Command(
                "calculate_calendar",
                new ObjectSchema()
                    .WithRequiredProperty("contract_id", PipServices3.Commons.Convert.TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var id = parameters.GetAsString("contract_id");
                    return await _controller.CalculateCalendarAsync(correlationId, id);
                });
        }

        private ContractV1 ConvertToContract(object value)
        {
            return JsonConverter.FromJson<ContractV1>(JsonConverter.ToJson(value));
        }

    }
}
