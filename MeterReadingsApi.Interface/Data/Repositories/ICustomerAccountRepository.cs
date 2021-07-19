using MeterReadingsApi.Model.Data;
using System.Threading.Tasks;

namespace MeterReadingsApi.Interface.Data.Repositories
{
    public interface ICustomerAccountRepository
    {
        Task<CustomerAccount> GetAsync(int id, bool includeMeterReadings);
        Task<CustomerAccount[]> GetAllAsync();
    }
}
