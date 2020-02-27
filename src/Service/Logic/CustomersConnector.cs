using System.Threading.Tasks;
using EIC.Customers.Clients.Version1;
using EIC.Customers.Data.Version1;

namespace EIC.Contracts.Logic
{
	public class CustomersConnector
	{
		private ICustomersClientV1 _customersClient;

		public CustomersConnector(ICustomersClientV1 customersClientV1)
		{
			_customersClient = customersClientV1;
		}

		public async Task<CustomerV1> GetByIdAsync(string correlationId, string id)
		{
			if (_customersClient == null) return null;

			return await _customersClient.GetCustomerByIdAsync(correlationId, id);
		}
		
		public async Task<CustomerV1> CreateCustomerAsync(string correlationId, CustomerV1 customer)
		{
			if (_customersClient == null) return null;

			return await _customersClient.CreateCustomerAsync(correlationId, customer);
		}

		public async Task<CustomerV1> UpdateCustomerAsync(string correlationId, CustomerV1 customer)
		{
			if (_customersClient == null) return null;

			return await _customersClient.UpdateCustomerAsync(correlationId, customer);
		}

		public async Task<CustomerV1> DeleteCustomerByIdAsync(string correlationId, string id)
		{
			if (_customersClient == null) return null;

			return await _customersClient.DeleteCustomerByIdAsync(correlationId, id);
		}
	}
}
