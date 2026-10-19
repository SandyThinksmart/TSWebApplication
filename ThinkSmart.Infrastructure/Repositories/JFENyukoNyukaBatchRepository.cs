using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSWebApplication.Domain.Model;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Infrastructure.Repositories
{
    public class JFENyukoNyukaBatchRepository : IJFENyukoNyukaBatchRepository
    {
        public async Task<List<JFENyukoNyukaBatchDetails>> ReadCsvData(Stream stream)
        {
            Encoding.RegisterProvider(
                CodePagesEncodingProvider.Instance);

            using var reader = new StreamReader(
                stream,
                Encoding.GetEncoding(932)
            );

            using var csv = new CsvReader(
                reader,
                CultureInfo.InvariantCulture
            );

            var records = csv.GetRecords<JFENyukoNyukaBatchDetails>().ToList();

            return await Task.FromResult(records);
        }

       
    }
}
