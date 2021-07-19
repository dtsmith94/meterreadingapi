using MeterReadingsApi.Interface.Data.Repositories;
using MeterReadingsApi.Interface.Service.Services;
using MeterReadingsApi.Model.Data;
using MeterReadingsApi.Model.ViewModels;
using MeterReadingsApi.Service.Builders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeterReadingsApi.Service.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly ICustomerAccountService _customerAccountService;
        private readonly IMeterReadingRepository _meterReadingRepository;

        public MeterReadingService(ICustomerAccountService customerAccountService, IMeterReadingRepository meterReadingRepository)
        {
            _customerAccountService = customerAccountService;
            _meterReadingRepository = meterReadingRepository;
        }

        public async Task<MeterReadingUploadResultViewModel> ProcessNewReadings(string readings)
        {
            var builder = new MeterReadingBuilder();
            var (meterReadings, failedCount) = builder.ConstructFromCsv(readings);

            // before we push these readings into the db, we need to check the customer account IDs are valid
            var invalidMeterReadings = new List<MeterReading>();

            foreach(var meterReading in meterReadings)
            {
                // check that the customer account is valid
                var customerAccount = await _customerAccountService.GetAsync(meterReading.CustomerAccountId, true);
                if (customerAccount == null)
                {
                    // mark the meter reading as invalid so we can remove it after looping through all meter readings
                    invalidMeterReadings.Add(meterReading);
                    failedCount++;
                }
                else if (customerAccount.MeterReadings != null)
                {
                    // if the account exists, check that we haven't already uploaded this meter reading value
                    foreach (var existingReading in customerAccount.MeterReadings)
                    {
                        if(meterReading.Value == existingReading.Value)
                        {
                            invalidMeterReadings.Add(meterReading);
                            failedCount++;
                            break;
                        }
                    }
                }
            }

            foreach(var invalidMeterReading in invalidMeterReadings)
            {
                meterReadings.Remove(invalidMeterReading);
            }

            await _meterReadingRepository.AddRangeAsync(meterReadings);

            return new MeterReadingUploadResultViewModel
            {
                SuccessfulUploads = meterReadings.Count,
                FailedUploads = failedCount
            };

        }
    }
}
