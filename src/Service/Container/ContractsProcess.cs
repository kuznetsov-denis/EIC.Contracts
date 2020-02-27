using EIC.Contracts.Build;
using PipServices3.Container;
using PipServices3.Rpc.Build;
using PipServices3.Rpc.Services;

namespace EIC.Contracts.Container
{
	public class ContractsProcess : ProcessContainer
	{
		public ContractsProcess()
			: base("contracts", "Contracts microservice")
		{
			_factories.Add(new DefaultRpcFactory());
			_factories.Add(new ContractsServiceFactory());
		}
	}
}
