using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface ICSVGenerationRepository
    {
        List<OutputCSVDetails> GetGroupTitleList();
        List<OutputCSVDetails> GetCSVTitleList(string groupTitleName);
        List<OutputCSVDetails> GetConditionsList(int outputCD);
        DataTable GetData(string query);
        DataTable GetCSVProcedureData(StoredProcedureDetails pDetails);
        List<NameValue> GetLocationNameList();
        List<NameValue> GetNextProcessList(int nextProcessCD);
    }
}
