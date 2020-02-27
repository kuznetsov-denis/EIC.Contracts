using PipServices3.Commons.Refer;
using PipServices3.Components.Build;
using EIC.Contracts.Persistence;
using EIC.Contracts.Logic;
using EIC.Contracts.Services.Version1;
using EIC.Customers.Clients.Version1;

namespace EIC.Contracts.Build
{
    public class ContractsServiceFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("eic-contracts", "factory", "service", "default", "1.0");
        public static Descriptor MemoryPersistenceDescriptor = new Descriptor("eic-contracts", "persistence", "memory", "*", "1.0");
        public static Descriptor MongoDbPersistenceDescriptor = new Descriptor("eic-contracts", "persistence", "mongodb", "*", "1.0");
        public static Descriptor ControllerDescriptor = new Descriptor("eic-contracts", "controller", "default", "*", "1.0");
        public static Descriptor HttpServiceDescriptor = new Descriptor("eic-contracts", "service", "http", "*", "1.0");
        public static Descriptor CustomersHttpClientDescriptor = new Descriptor("eic-customers", "client", "http", "*", "1.0");

        public ContractsServiceFactory()
        {
            RegisterAsType(CustomersHttpClientDescriptor, typeof(CustomersHttpClientV1));
            RegisterAsType(MemoryPersistenceDescriptor, typeof(ContractsMemoryPersistence));
            RegisterAsType(MongoDbPersistenceDescriptor, typeof(ContractsMongoDbPersistence));
            RegisterAsType(ControllerDescriptor, typeof(ContractsController));
            RegisterAsType(HttpServiceDescriptor, typeof(ContractsHttpServiceV1));
        }
    }
}
