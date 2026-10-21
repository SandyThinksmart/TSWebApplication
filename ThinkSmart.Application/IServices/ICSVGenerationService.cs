using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
   public interface  ICSVGenerationService
    {
        List<OutputCSVDetails> GetCSVGroupTitle();
        List<OutputCSVDetails> GetCSVOutputTitleList(string groupTitleName);
        List<OutputCSVDetails> GetConditionsList(int outputCD);
        DataTable GetCSVData(string query);
        DataTable GetCSVProcedureData(StoredProcedureDetails pDetails);
        List<NameValue> GetLocationNameList();
        List<NameValue> GetNextProcessList();
    }
}
