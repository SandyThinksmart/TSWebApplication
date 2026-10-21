using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IWIPTagSplitService
    {
        WIPTagSplitDetails GetTagDetailsByDivisionCD(int divisionCD);
        void DeleteTagSplitData(WIPTagSplitDetails request);
    }
}
