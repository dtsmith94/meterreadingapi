using MeterReadingsApi.Model.Data;
using System.Threading.Tasks;

namespace MeterReadingsApi.Interface.Service.Services
{
    public interface ICustomerAccountService
    {
        Task<CustomerAccount> GetAsync(int id);
        Task<CustomerAccount[]> GetAllAsync();
    }
}
