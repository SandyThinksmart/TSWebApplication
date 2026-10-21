using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IJFENyukoNyukaBatchService
    {
       
            Task<List<JFENyukoNyukaBatchDetails>> ReadCsvData(IFormFile file);
       
    }
}
