using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IJFENyukoNyukaBatchRepository
    {
        Task<List<JFENyukoNyukaBatchDetails>> ReadCsvData(Stream stream);
    }
}
