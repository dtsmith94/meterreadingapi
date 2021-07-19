using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MeterReadingsApi.Model.Data
{
    public class MeterReading
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(5)]
        public string Value { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [ForeignKey("CustomerAccount")]
        public int CustomerAccountId { get; set; }
        [JsonIgnore]
        public CustomerAccount CustomerAccount { get; set; }
    }
}
