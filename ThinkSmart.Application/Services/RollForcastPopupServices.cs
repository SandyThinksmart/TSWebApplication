using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Domain.Model;
using TSWebApplication.Infrastructure.IRepositories;
using System.Transactions;
using TSWebApplication.Infrastructure.Repositories;

namespace TSWebApplication.Application.Services
{
    public class RollForcastPopupServices : IRollForcastPopupServices
    {

        private readonly IRollForcastPopupRepository _rollForcastPopupRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;

        public RollForcastPopupServices(IRollForcastPopupRepository rollForcastPopupRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _rollForcastPopupRepository = rollForcastPopupRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
        public List<RollForcastPopup> GetOneMonthProductForecastDetails(string FcSummaryNo, int Roll, int Year)
        {
            var obj = _rollForcastPopupRepository.GetOneMonthProductForecastDetails(FcSummaryNo, Roll, Year);
            return obj;
        }
        public List<RollForcastPopup> GetOneMonthMaterialForecastDetails(string FcSummaryNo)
        {
            var obj = _rollForcastPopupRepository.GetOneMonthMaterialForecastDetails(FcSummaryNo);
            return obj;
        }
    }

}
