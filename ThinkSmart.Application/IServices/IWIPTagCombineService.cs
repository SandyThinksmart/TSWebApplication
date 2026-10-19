using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IWIPTagCombineService
    {
        WIPTagCombineDetails GetTagDetailsByCombineCD(int combineCD);
        void DeleteTagCombineData(WIPTagCombineDetails request);
    }
}
