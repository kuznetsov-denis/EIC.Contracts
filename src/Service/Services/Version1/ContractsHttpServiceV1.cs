using PipServices3.Commons.Refer;
using PipServices3.Rpc.Services;

namespace EIC.Contracts.Services.Version1
{
	public class ContractsHttpServiceV1 : CommandableHttpService
	{
		public ContractsHttpServiceV1()
			: base("v1/contracts")
		{
			_dependencyResolver.Put("controller", new Descriptor("eic-contracts", "controller", "default", "*", "1.0"));
		}
	}
}
