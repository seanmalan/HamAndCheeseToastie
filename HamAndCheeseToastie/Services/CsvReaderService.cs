using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;
using HamAndCheeseToastie.Database; // Your EF Context
using HamAndCheeseToastie.Models;
using CsvHelper.Configuration; // Your EF Models

namespace HamAndCheeseToastie.Services
{
    public class CsvReaderService : ICsvReader
    {
        private readonly DatabaseContext _context;

        public CsvReaderService(DatabaseContext context)
        {
            _context = context;
        }

        // Implement the method with StreamReader
        public List<Product> ImportCsv(StreamReader reader)
        {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // Ignore missing headers (like 'ID')
                MissingFieldFound = null // Ignore missing fields in rows
            };

            using (var csv = new CsvReader(reader, config))
            {
                var products = csv.GetRecords<Product>().ToList(); // Automatically maps CSV rows to Product model
                return products;
            }
        }
    }
}
