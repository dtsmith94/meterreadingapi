using MeterReadingsApi.Model.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReadingsApi.Interface.Data.Repositories
{
    public interface IMeterReadingRepository
    {
        Task AddRangeAsync(IEnumerable<MeterReading> meterReadings);
    }
}
