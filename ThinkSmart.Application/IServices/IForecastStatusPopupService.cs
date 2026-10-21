using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;

namespace TSWebApplication.Application.IServices
{
    public interface IForecastStatusPopupService
    {
        List<ForecastStatusPopupDetails> GetSalesForecastDataList();
    }
}
