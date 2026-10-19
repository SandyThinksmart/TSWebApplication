using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IRollForcastPopupServices
    {
        List<RollForcastPopup> GetOneMonthProductForecastDetails(string FcSummaryNo, int Roll, int Year);
        List<RollForcastPopup> GetOneMonthMaterialForecastDetails(string FcSummaryNo);

    }
}
