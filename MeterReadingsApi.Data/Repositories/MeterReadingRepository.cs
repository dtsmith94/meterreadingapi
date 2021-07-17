using MeterReadingsApi.Data.Context;
using MeterReadingsApi.Interface.Data.Repositories;
using MeterReadingsApi.Model.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReadingsApi.Data.Repositories
{
    public class MeterReadingRepository: IMeterReadingRepository
    {
        private readonly MeterReadingsContext _context;

        public MeterReadingRepository(MeterReadingsContext context)
        {
            _context = context;
        }

        public Task AddRangeAsync(IEnumerable<MeterReading> meterReadings)
        {
            _context.MeterReadings.AddRange(meterReadings);
            return _context.SaveChangesAsync();
        }
    }
}
