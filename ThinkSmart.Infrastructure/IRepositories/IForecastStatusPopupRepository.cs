using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Infrastructure.IRepositories
{
    public interface IForecastStatusPopupRepository
    {
        List<ForecastStatusPopupDetails> GetSalesForecastDataList();
    }
}
