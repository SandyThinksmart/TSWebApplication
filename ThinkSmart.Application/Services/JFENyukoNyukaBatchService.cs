using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class JFENyukoNyukaBatchService : IJFENyukoNyukaBatchService
    {
        private readonly IJFENyukoNyukaBatchRepository _jFENyukoNyukaBatchRepository;

        public JFENyukoNyukaBatchService(IJFENyukoNyukaBatchRepository jFENyukoNyukaBatchRepository)
        {
            _jFENyukoNyukaBatchRepository = jFENyukoNyukaBatchRepository;
        }

        public async Task<List<JFENyukoNyukaBatchDetails>> ReadCsvData(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("CSV file is empty.");
            }

            using var stream = file.OpenReadStream();

            var result = await _jFENyukoNyukaBatchRepository.ReadCsvData(stream);

            return result;
        }

        
    }
}
