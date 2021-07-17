using MeterReadingsApi.Interface.Data.Repositories;
using MeterReadingsApi.Interface.Service.Services;
using MeterReadingsApi.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingsApi.Service.Services
{
    public class CustomerAccountService: ICustomerAccountService
    {
        private readonly ICustomerAccountRepository _customerAccountRepository;

        public CustomerAccountService(ICustomerAccountRepository customerAccountRepository)
        {
            _customerAccountRepository = customerAccountRepository;            
        }

        public async Task<CustomerAccount> GetAsync(int id)
        {
            return await _customerAccountRepository.GetAsync(id).AsTask();
        }

        public async Task<CustomerAccount[]> GetAllAsync()
        {
            return await _customerAccountRepository.GetAllAsync();
        }
    }
}
