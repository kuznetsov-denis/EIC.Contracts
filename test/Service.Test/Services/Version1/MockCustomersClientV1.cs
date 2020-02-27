using EIC.Customers.Clients.Version1;
using EIC.Customers.Data.Version1;
using PipServices3.Commons.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EIC.Contracts.Services.Version1
{
    public class MockCustomersClientV1 : ICustomersClientV1
    {
        private int _idIdentity = 0;
        private Dictionary<string, CustomerV1> _customersDict = new Dictionary<string, CustomerV1>();

        public async Task<CustomerV1> CreateCustomerAsync(string correlationId, CustomerV1 customer)
        {
            _idIdentity++;

            customer.Id = _idIdentity.ToString();
            _customersDict.Add(customer.Id, customer);

            return await Task.FromResult(customer);
        }

        public async Task<CustomerV1> DeleteCustomerByIdAsync(string correlationId, string id)
        {
            CustomerV1 customer = null;

            if (_customersDict.TryGetValue(id, out CustomerV1 customerV1))
            {
                _customersDict.Remove(id);
                customer = customerV1;
            }

            return await Task.FromResult(customer);
        }

        public async Task<CustomerV1> GetCustomerByIdAsync(string correlationId, string id)
        {
            CustomerV1 customer = _customersDict.TryGetValue(id, out CustomerV1 customerV1) ? customerV1 : null;
            return await Task.FromResult(customer);
        }

        public async Task<DataPage<CustomerV1>> GetCustomersAsync(string correlationId, FilterParams filter, PagingParams paging, SortParams sort)
        {
            var total = Convert.ToInt64(_customersDict.Count);
            var skip = Convert.ToInt32(paging.Skip ?? 0);
            var take = Convert.ToInt32(paging.Take ?? total);

            var data = _customersDict.Select(x => x.Value).Skip(skip).Take(take).ToList();

#warning Filtering is not supported
            DataPage<CustomerV1> dataPage = new DataPage<CustomerV1>(data, total);

            return await Task.FromResult(dataPage);
        }

        public async Task<CustomerV1> UpdateCustomerAsync(string correlationId, CustomerV1 customer)
        {
            _customersDict[customer.Id] = customer;
            return await Task.FromResult(customer);
        }
    }
}
