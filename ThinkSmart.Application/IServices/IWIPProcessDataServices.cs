using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Application.IServices
{
    public interface IWIPProcessDataServices
    {
        int GetWIProcessDataList(int locationCd);
    }
}
