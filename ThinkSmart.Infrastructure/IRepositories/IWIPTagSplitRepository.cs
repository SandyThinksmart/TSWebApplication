using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IWIPTagSplitRepository
    {
        WIPTagSplitDetails GetTagDetails(int divisionCD);
        void Delete(int divisionCD);
    }
}
