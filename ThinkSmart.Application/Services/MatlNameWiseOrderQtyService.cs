using System;
using System.Collections.Generic;
using System.Text;
using TSWebApplication.Application.IServices;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Infrastructure.IRepositories;

namespace TSWebApplication.Application.Services
{
    public class MatlNameWiseOrderQtyService : IMatlNameWiseOrderQtyService
    {
        private readonly IMatlNameWiseOrderQtyRepository _matlNameWiseOrderQtyRepository;
        private readonly IAllCommonRepository _allCommonRepository;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly LocalizationServices _localization;
        private readonly IAllCommonService _allCommonService;

        public MatlNameWiseOrderQtyService(IMatlNameWiseOrderQtyRepository matlNameWiseOrderQtyRepository, IAllCommonRepository allCommonRepository, IUserLoginRepository userLoginRepository, LocalizationServices localization, IAllCommonService allCommonService)
        {
            _matlNameWiseOrderQtyRepository = matlNameWiseOrderQtyRepository;
            _allCommonRepository = allCommonRepository;
            _userLoginRepository = userLoginRepository;
            _localization = localization;
            _allCommonService = allCommonService;
        }
       
        public List<MatlNameWiseOrderQtyDetails> GetMaterialOrderQtyData(int Batch_Roll)
        {
            var obj = _matlNameWiseOrderQtyRepository.GetMaterialOrderQtyData(Batch_Roll);
            return obj;
        }
    }
}
