using PipServices3.Commons.Refer;
using PipServices3.Components.Build;
using EIC.Contracts.Clients.Version1;

namespace EIC.Contracts.Build
{
    public class ContractsClientFactory : Factory
    {
        public static Descriptor NullClientDescriptor = new Descriptor("eic-contracts", "client", "null", "*", "1.0");
        public static Descriptor DirectClientDescriptor = new Descriptor("eic-contracts", "client", "direct", "*", "1.0");
        public static Descriptor HttpClientDescriptor = new Descriptor("eic-contracts", "client", "http", "*", "1.0");

        public ContractsClientFactory()
        {
            RegisterAsType(ContractsClientFactory.NullClientDescriptor, typeof(ContractsNullClientV1));
            RegisterAsType(ContractsClientFactory.DirectClientDescriptor, typeof(ContractsDirectClientV1));
            RegisterAsType(ContractsClientFactory.HttpClientDescriptor, typeof(ContractsHttpClientV1));
        }
    }
}
