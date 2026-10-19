using System;
using System.Collections.Generic;

using System.Text;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface INextProcessWaitingStatusSearchService
    {
        List<NameValue> GetProcessList();

        List<WipStatusDetails> GetNextProcessWaitingStatusSearch(GridSearchFields gridSearchRequest);
        List<NameValue> GetLocationNameList();

    }
}
