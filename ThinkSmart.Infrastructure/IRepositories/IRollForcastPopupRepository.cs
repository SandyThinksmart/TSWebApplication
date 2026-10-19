using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IRollForcastPopupRepository
    {
        List<RollForcastPopup> GetOneMonthProductForecastDetails(string FcSummaryNo,int Roll,int Year);
        List<RollForcastPopup> GetOneMonthMaterialForecastDetails(string FcSummaryNo);
    }
}
