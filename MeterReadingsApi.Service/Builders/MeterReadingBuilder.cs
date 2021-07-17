using MeterReadingsApi.Model.Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MeterReadingsApi.Service.Builders
{
    public class MeterReadingBuilder
    {
        public (List<MeterReading> meterReadings, int failedToReadCount) ConstructFromCsv(string csvString)
        {
            var meterReadings = new List<MeterReading>();
            var failedToReadCount = 0;

            string[] csvLines = csvString.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            for (var row = 1; row <= csvLines.Length; row++)
            {
                // skip over the first row which contains headers
                if (row == 1)
                {
                    continue;
                }

                var parts = csvLines[row - 1].Split(',');

                // check the row contains data
                if (parts.Length != 3)
                {
                    failedToReadCount++;

                    continue;
                }

                // validate the data
                var parsedIdResult = int.TryParse(parts[0], out var parsedId);
                var parsedDateTimeResult = DateTime.TryParse(parts[1], out var parsedDateTime);

                // check that the meter reading value is in the correct format and only 5 characters long
                var readingRegex = new Regex("/[^A-Za-z0-9]+/");
                var readingRegexResult = readingRegex.IsMatch(parts[2]);

                if (!parsedIdResult || !parsedDateTimeResult || !readingRegexResult)
                {
                    failedToReadCount++;
                    continue;
                }

                meterReadings.Add(new MeterReading
                {
                    CustomerAccountId = parsedId,
                    DateTime = parsedDateTime,
                    Value = parts[2]
                });
                
            }

            return (meterReadings, failedToReadCount);
        }
    }
}
