using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface INextProcessWaitingStatusSearchRepository
    { 
      List<WipStatusDetails> GetNextProcessWaitingStatusSearch(List<SearchField> searchFields, string sortCriteria, GridSearchFields gridresult);
        List<NameValue> GetLocationNameList();


    }
}
