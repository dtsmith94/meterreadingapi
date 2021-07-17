using MeterReadingsApi.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingsApi.Interface.Data.Repositories
{
    public interface ICustomerAccountRepository
    {
        ValueTask<CustomerAccount> GetAsync(int id);
        Task<CustomerAccount[]> GetAllAsync();
    }
}
