using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeterReadingsApi.Model.Data
{
    public class CustomerAccount
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(150)]
        public string LastName { get; set; }
        public List<MeterReading> MeterReadings { get; set; }
    }
}
