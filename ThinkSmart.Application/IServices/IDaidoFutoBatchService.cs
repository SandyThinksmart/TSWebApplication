using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IDaidoFutoBatchService
    {
        List<NameValue> GetFutouList();

       public DaidoFutouNyukoDetails DaidoNyukoEDIBatchDataInsert(List<DaidoFutouNyukoDetails> reuest, string cultureName, int userID);
        public DaidoFutouNyukoDetails GetDataCount(DateTime Importdate);
      
    }
}
