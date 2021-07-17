using MeterReadingsApi.Model.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace MeterReadingsApi.Data.Context {
    public class MeterReadingsContext : DbContext {

        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }

        public MeterReadingsContext(DbContextOptions<MeterReadingsContext> options) : base(options) { }
    }
}