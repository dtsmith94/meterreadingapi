using MeterReadingsApi.Model.ViewModels;
using System.Threading.Tasks;

namespace MeterReadingsApi.Interface.Service.Services
{
    public interface IMeterReadingService
    {
        Task<MeterReadingUploadResultViewModel> ProcessNewReadings(string readings);
    }
}
