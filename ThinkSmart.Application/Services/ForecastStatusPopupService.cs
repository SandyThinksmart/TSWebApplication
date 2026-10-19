using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class ForecastStatusPopupService : IForecastStatusPopupService
    {
        private readonly IForecastStatusPopupRepository _forecastStatusPopupRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;
        public ForecastStatusPopupService(IForecastStatusPopupRepository forecastStatusPopupRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _forecastStatusPopupRepository = forecastStatusPopupRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public List<ForecastStatusPopupDetails> GetSalesForecastDataList()
        {
            var obj = _forecastStatusPopupRepository.GetSalesForecastDataList();
            return obj;
        }
    }

}
