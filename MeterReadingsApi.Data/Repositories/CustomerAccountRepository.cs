using MeterReadingsApi.Data.Context;
using MeterReadingsApi.Interface.Data.Repositories;
using MeterReadingsApi.Model.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReadingsApi.Data.Repositories
{
    public class CustomerAccountRepository : ICustomerAccountRepository
    {
        private readonly MeterReadingsContext _context;

        public CustomerAccountRepository(MeterReadingsContext context)
        {
            _context = context;

            // for the purpose of this exercise, we will seed the Customer Account data here (if we haven't already)
            SeedData();
        }

        public Task<CustomerAccount> GetAsync(int id, bool includeMeterReadings)
        {
            var query = _context.CustomerAccounts.AsQueryable();

            if (includeMeterReadings)
            {
                query = query.Include(account => account.MeterReadings);
            }

            return query.FirstOrDefaultAsync(account => account.Id == id);
        }

        public Task<CustomerAccount[]> GetAllAsync()
        {
            return _context.CustomerAccounts.ToArrayAsync();
        }

        private void SeedData()
        {
            if (_context.CustomerAccounts.Count() != 0)
            {
                // we must have already seeded the db
                return;
            }

            // loop over each line of the Test_Accounts.csv and create new CustomerAccount entities (I don't need to validate this data)
            var customerAccounts = File.ReadAllLines($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}SeedData{Path.DirectorySeparatorChar}Test_Accounts.csv");

            for (var row = 1; row <= customerAccounts.Length; row++)
            {
                // skip over the first row which contains headers
                if (row == 1)
                {
                    continue;
                }

                var parts = customerAccounts[row-1].Split(',');

                // check the row contains data
                if (parts.Length != 3)
                {
                    continue;
                }

                _context.CustomerAccounts.Add(new CustomerAccount
                {
                    Id = int.Parse(parts[0]),
                    FirstName = parts[1],
                    LastName = parts[2]
                });
            }

            // save
            _context.SaveChanges();
        }
    }
}
